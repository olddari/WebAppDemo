using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class Product
{
    public int ProductID { get; set; }
    public int CategoryID { get; set; }
    public string ProductName { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
    public string ImageURL { get; set; }

    // Navigation properties
    public ProductCategory ProductCategory { get; set; }  // Correct type
    public ICollection<ProductAttribute> ProductAttributes { get; set; }
    public ICollection<Inventory> Inventories { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}
