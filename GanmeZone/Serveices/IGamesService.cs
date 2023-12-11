namespace GanmeZone.Serveices
{
    public interface IGamesService
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game?> GetByIdAsync(int id);
        Task Create(CreateGameFormViewModel game);
    }
}
