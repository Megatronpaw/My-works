public class Purchase
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }

    public Client? Client { get; set; }
    public Product? Product { get; set; }

}

