using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class ProductCategory
{
    [Key] public int CategoryID { get; set; }  // Primary Key
    public string CategoryName { get; set; }
    public string Description { get; set; }

    // Navigation property
    public ICollection<Product> Products { get; set; }
}
