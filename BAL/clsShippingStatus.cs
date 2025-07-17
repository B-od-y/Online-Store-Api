using DAL;

public class clsShippingStatus
{
    public enum enMode { enAdd, enUpdate }

    public int ShippingStatusID { get; set; }
    public string Status { get; set; }
    public enMode Mode { get; set; }

    public ShippingStatusDTO DTO => new(ShippingStatusID, Status);

    public clsShippingStatus(ShippingStatusDTO dto, enMode mode = enMode.enAdd)
    {
        ShippingStatusID = dto.ShippingStatusID;
        Status = dto.Status;
        Mode = mode;
    }

    public static List<ShippingStatusDTO> GetAll() => clsShippingStatusData.GetAllStatuses();

    public static clsShippingStatus Find(int id)
    {
        var dto = clsShippingStatusData.GetStatusByID(id);
        return dto == null ? null : new clsShippingStatus(dto, enMode.enUpdate);
    }

    private bool Add() => (ShippingStatusID = clsShippingStatusData.AddStatus(DTO)) != -1;

    private bool Update() => clsShippingStatusData.UpdateStatus(DTO);

    public bool Save()
    {
        return Mode switch
        {
            enMode.enAdd => Add() && (Mode = enMode.enUpdate) == enMode.enUpdate,
            enMode.enUpdate => Update(),
            _ => false
        };
    }

    public static bool Delete(int id) => clsShippingStatusData.DeleteStatus(id);
}
