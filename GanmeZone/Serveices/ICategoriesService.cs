namespace GanmeZone.Serveices
{
    public interface ICategoriesService
    {
        IEnumerable<SelectListItem> GetSelectListOfCategories();
    }
}
