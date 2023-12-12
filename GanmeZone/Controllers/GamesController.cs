namespace GanmeZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IDevicesService _devicesService;
        private readonly IGamesService _gamesService;

        public GamesController(ICategoriesService categoriesService, IDevicesService devicesService, IGamesService gamesService)
        {
            _categoriesService = categoriesService;
            _devicesService = devicesService;
            _gamesService = gamesService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _gamesService.GetAllAsync());
        }
        public async Task<IActionResult> Details(int id)
        {
            var game=_gamesService.GetByIdAsync(id);
            if (game is null)
                return NotFound();

            return View(await game);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categories = _categoriesService.GetSelectListOfCategories(),
                Devices = _devicesService.GetSelectListOfDevices()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            //Server Side Validation
            if(!ModelState.IsValid)
            {
                model.Categories = _categoriesService.GetSelectListOfCategories();
                model.Devices = _devicesService.GetSelectListOfDevices();
                return View(model);
            }
            //Save The Game In DataBsae
              await _gamesService.Create(model);
            //Save The Cover In Server

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gamesService.GetByIdAsync(id);

            if(game is null)
                return NotFound();

            EditGameFormViewModel viewModel = new()
            {
                Id=id,
                Name=game.Name,
                Description=game.Description,
                CategoryId=game.CategoryId,
                SelectedDevices=game.Devices.Select(d=>d.DeviceId).ToList(),
                Categories=_categoriesService.GetSelectListOfCategories(),
                Devices=_devicesService.GetSelectListOfDevices(),
                CurrentCover=game.Cover
            };

            return View(viewModel);
        }
    }
}
