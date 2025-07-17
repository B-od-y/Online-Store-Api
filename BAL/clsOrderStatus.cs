using DAL;

public class clsOrderStatus
{
    public enum enMode { enAdd, enUpdate }

    public int OrderStatusID { get; set; }
    public string Status { get; set; }
    public enMode Mode { get; set; }

    public OrderStatusDTO DTO => new(OrderStatusID, Status);

    public clsOrderStatus(OrderStatusDTO dto, enMode mode = enMode.enAdd)
    {
        OrderStatusID = dto.OrderStatusID;
        Status = dto.Status;
        Mode = mode;
    }

    public static List<OrderStatusDTO> GetAll() => clsOrderStatusData.GetAll();

    public static clsOrderStatus Find(int id)
    {
        var dto = clsOrderStatusData.GetByID(id);
        return dto == null ? null : new clsOrderStatus(dto, enMode.enUpdate);
    }

    private bool Add() => (OrderStatusID = clsOrderStatusData.Add(DTO)) != -1;

    private bool Update() => clsOrderStatusData.Update(DTO);

    public bool Save()
    {
        return Mode switch
        {
            enMode.enAdd => Add() && (Mode = enMode.enUpdate) == enMode.enUpdate,
            enMode.enUpdate => Update(),
            _ => false
        };
    }

    public static bool Delete(int id) => clsOrderStatusData.Delete(id);
}
