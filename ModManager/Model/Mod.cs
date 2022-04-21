namespace MonBazou_ModManager.Model;

internal class Mod
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? ModVersion { get; set; }
    public string? GameVersion { get; set; }
    public string? Type { get; set; }
    public string? FileName { get; set; }
    public bool? Disabled { get; set; }
    public string? Reason { get; set; }
    public string? DownloadLink { get; set; }
}