namespace KafkaFlowIntegration;

public class ProductDTO
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description{ get; set; }
    public byte[]? Images { get; set; }
    public bool isOutOfStock { get; set; }
    public int Quantity{ get; set; }

    public decimal? Price { get; set; }

}
