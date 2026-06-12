using System;
using System.Collections.Generic;

namespace demo_bicycle.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly DateOrder { get; set; }

    public DateOnly DateDelivery { get; set; }

    public int PickUpPointId { get; set; }

    public int UserId { get; set; }

    public int? Code { get; set; }

    public int? StatusId { get; set; }

    public virtual ICollection<OrderTovar> OrderTovars { get; set; } = new List<OrderTovar>();

    public virtual PickUpPoint PickUpPoint { get; set; } = null!;

    public virtual Status? Status { get; set; }

    public virtual User User { get; set; } = null!;
}
