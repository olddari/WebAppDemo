using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ProductCategory
{
    [Key] public int CategoryID { get; set; } 
    public string CategoryName { get; set; }
    public string Description { get; set; }

    [JsonIgnore]
    public ICollection<Product> Products { get; set; }
}
