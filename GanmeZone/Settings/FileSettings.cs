namespace GanmeZone.Settings
{
    public static class FileSettings
    {
        public const string ImagesPath ="/Assets/Images/Games";
        public const string AllowedExtensions =".Jpg,.Jpeg,.Png";
        public const int MaxFileSizeInMB =1;
        public const int MaxFileSizeInBytes =MaxFileSizeInMB * 1024 * 1024;
    }
}
