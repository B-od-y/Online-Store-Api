using DAL;

public class clsProduct
{
    public enum enMode { enAdd, enUpdate }

    public int ProductID { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public int QuantityInStock { get; set; }
    public int CatagoryID { get; set; }
    public string ImageURL { get; set; }

    public enMode Mode { get; set; }

    public ProductDTO DTO => new(ProductID, ProductName, Description, Price, QuantityInStock, CatagoryID, ImageURL);

    public clsProduct(ProductDTO dto, enMode mode = enMode.enAdd)
    {
        ProductID = dto.ProductID;
        ProductName = dto.ProductName;
        Description = dto.Description;
        Price = dto.Price;
        QuantityInStock = dto.QuantityInStock;
        CatagoryID = dto.CatagoryID;
        ImageURL = dto.ImageURL;
        Mode = mode;
    }

    public static List<ProductDTO> GetAll() => clsProductData.GetAllProducts();

    public static clsProduct Find(int id)
    {
        var dto = clsProductData.GetProductByID(id);
        return dto == null ? null : new clsProduct(dto, enMode.enUpdate);
    }

    private bool Add() => (ProductID = clsProductData.AddProduct(DTO)) != -1;

    private bool Update() => clsProductData.UpdateProduct(DTO);

    public bool Save()
    {
        return Mode switch
        {
            enMode.enAdd => Add() && (Mode = enMode.enUpdate) == enMode.enUpdate,
            enMode.enUpdate => Update(),
            _ => false
        };
    }

    public static bool Delete(int id) => clsProductData.DeleteProduct(id);
}
