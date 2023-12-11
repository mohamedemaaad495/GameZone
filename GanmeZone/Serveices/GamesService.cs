namespace GanmeZone.Serveices
{
    public class GamesService : IGamesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imgPath;
        public GamesService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imgPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
        }

        public async Task Create(CreateGameFormViewModel model)
        {
                var coverName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";
                var path = Path.Combine(_imgPath, coverName);

                using var Stream = File.Create(path);
                await model.Cover.CopyToAsync(Stream);
                // Stream.Dispose();

                Game game = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    Cover = coverName,
                    Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList()
                };

                _context.Add(game);
                _context.SaveChanges();
            }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context
                .Games
                .Include(g=>g.Category)
                .Include(g=>g.Devices)
                .ThenInclude(g=>g.Device)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context
                .Games
                .Include(g => g.Category)
                .Include(g => g.Devices)
                .ThenInclude(g => g.Device)
                .AsNoTracking()
                .SingleOrDefaultAsync(g => g.Id == id);
        }
    }
}
