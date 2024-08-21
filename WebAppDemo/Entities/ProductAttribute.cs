
public class ProductAttribute
{
    public int AttributeID { get; set; } 
    public int ProductID { get; set; }
    public string AttributeName { get; set; }
    public string AttributeValue { get; set; }

    // Navigation property
    public Product Product { get; set; }
}
