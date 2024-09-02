public class Inventory
{
    public int InventoryID { get; set; }
    public int ProductID { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public int StockQuantity { get; set; }

    public Product Product { get; set; }
}
