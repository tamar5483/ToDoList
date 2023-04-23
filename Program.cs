using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using ToDpAPI;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(opt => opt.AddPolicy(MyAllowSpecificOrigins, policy =>
{
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
}));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ToDoDBContext>();

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.MapGet("/",()=> { return "it works!";});

var group=app.MapGroup("/items");

  group.MapGet("/",
      (ToDoDBContext db)=>
      {return  db.Items.ToArrayAsync();});

    group.MapGet("/{id}", (int id,ToDoDBContext db)=>
    {
        return    db.Items.FindAsync(id);
    });
  
    group.MapPost("/{name}", async (string name,ToDoDBContext db)=>
    {
      Item item=new Item(){Name=name};
     var newItem=  db.Items.Add(item);
      await  db.SaveChangesAsync();
    });
  
    group.MapPut( "/{id}", async(int id,[FromBody]Item item,ToDoDBContext db)=>
    {
      
        var newItem=new Item(){Id=id,Name=item.Name,IsCompleted=item.IsCompleted};
        db.Update(item);
       await db.SaveChangesAsync();
        return item;
        });
  
    group.MapDelete("/{id}", async (int id,ToDoDBContext db)=>
    {
         Item todo =await  db.Items.FindAsync(id);
        db.Items.Remove(todo);
        await db.SaveChangesAsync();
        return todo;
    });

app.Run();
