using System.Collections;

namespace MonBazou_ModManager
{
    internal class Mod
    {
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? ModVersion { get; set; }
        public string? GameVersion { get; set; }
        public string? Type { get; set; }
        public string? dllName { get; set; }
        public string? zipName { get; set; }
        public bool? Disabled { get; set; }
        public string? Reason { get; set; }
    }
}