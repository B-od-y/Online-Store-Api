using DAL;

public class clsOrder
{
    public enum enMode { enAdd, enUpdate }

    public int OrderID { get; set; }
    public int CustomerID { get; set; }
    public DateTime OrderDate { get; set; }
    public float TotalAmount { get; set; }
    public int StatusID { get; set; }

    public enMode Mode { get; set; }

    public OrderDTO DTO => new(OrderID, CustomerID, OrderDate, TotalAmount, StatusID);

    public clsOrder(OrderDTO dto, enMode mode = enMode.enAdd)
    {
        OrderID = dto.OrderID;
        CustomerID = dto.CustomerID;
        OrderDate = dto.OrderDate;
        TotalAmount = dto.TotalAmount;
        StatusID = dto.StatusID;
        Mode = mode;
    }

    public static List<OrderDTO> GetAll() => clsOrderData.GetAllOrders();

    public static clsOrder Find(int id)
    {
        var dto = clsOrderData.GetOrderByID(id);
        return dto == null ? null : new clsOrder(dto, enMode.enUpdate);
    }

    private bool Add() => (OrderID = clsOrderData.AddOrder(DTO)) != -1;

    private bool Update() => clsOrderData.UpdateOrder(DTO);

    public bool Save()
    {
        return Mode switch
        {
            enMode.enAdd => Add() && (Mode = enMode.enUpdate) == enMode.enUpdate,
            enMode.enUpdate => Update(),
            _ => false
        };
    }

    public static bool Delete(int id) => clsOrderData.DeleteOrder(id);
}
