﻿namespace GanmeZone.ViewModels
{
    public class CreateGameFormViewModel
    {
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();
        public List<int> SelectedDevices { get; set; } = default!;
        [Display(Name = "Supported Devices")]
        public IEnumerable<SelectListItem> Devices { get; set; }=Enumerable.Empty<SelectListItem>();    

        [MaxLength(2500)]
        public string Description { get; set; } = string.Empty;
        [AllowedExtensions(FileSettings.AllowedExtensions),MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile Cover { get; set; } = default!;
    }
}
