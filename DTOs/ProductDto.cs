public class UpdateProductDto
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}