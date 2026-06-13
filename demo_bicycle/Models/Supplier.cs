using System;
using System.Collections.Generic;

namespace demo_bicycle.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public virtual ICollection<Tovar> Tovars { get; set; } = new List<Tovar>();
}
