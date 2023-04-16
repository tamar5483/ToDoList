using System;
using System.Collections.Generic;

namespace ToDpAPI;

public partial class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsCompleted { get; set; }
}
