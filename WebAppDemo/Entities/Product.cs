using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductID { get; set; }

    public int CategoryID { get; set; }
    public string ProductName { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
    public string ImageURL { get; set; }


    [JsonIgnore]
    public ProductCategory ProductCategory { get; set; } 

    [JsonIgnore]
    public ICollection<ProductAttribute> ProductAttributes { get; set; }

    [JsonIgnore]
    public ICollection<Inventory> Inventories { get; set; }

    [JsonIgnore]
    public ICollection<OrderDetail> OrderDetails { get; set; }
}
