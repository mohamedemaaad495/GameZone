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
            var coverName = await SaveCover(model.Cover);
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

        public async Task<Game?> Edit(EditGameFormViewModel model)
        {
            var game =await _context.Games
                .Include(g=>g.Devices)
                .SingleOrDefaultAsync(g=>g.Id==model.Id);
            if (game is null)
                return null;


            var hasNewCover = model.Cover is not null;
            var oldCover=game.Cover;


            game.Name = model.Name;
            game.Description=model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices=model.SelectedDevices.Select(d=>new GameDevice { DeviceId = d }).ToList();

            if (hasNewCover)
            {
                game.Cover=await SaveCover(model.Cover!);
            }

            var effectedRows=_context.SaveChanges();
            if(effectedRows > 0)
            {
                if(hasNewCover)
                {
                    var cover = Path.Combine(_imgPath, oldCover);
                    File.Delete(cover);
                }

                return game;
            }
            else
            {
                var cover = Path.Combine(_imgPath, game.Cover);
                File.Delete(cover);

                return null;
            }
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
        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imgPath, coverName);

            using var Stream = File.Create(path);
            await cover.CopyToAsync(Stream);

            return coverName;
        }
    }
    
}
