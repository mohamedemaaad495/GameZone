namespace GanmeZone.Serveices
{
    public interface IDevicesService
    {
        IEnumerable<SelectListItem> GetSelectListOfDevices();
    }
}
