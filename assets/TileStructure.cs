using System.Collections.Generic;

public class TileData
{
    public string? TileType { get; set; }
    public GameData? GameData { get; set; }
}

public class GameData
{
    public string? selectedMap { get; set; }
    public BossData? bossData { get; set; }
    public string? selectedMode { get; set; }
    public int? subGameType { get; set; }
    public string? SelectedDifficulty { get; set; }
    public DcModel? dcModel { get; set; }
}

public class BossData
{
    public int? bossBloon { get; set; }
    public int? TierCount { get; set; }
}

public class DcModel
{
    public StartRules? startRules { get; set; }
    public string? map { get; set; }
    public string? mode { get; set; }
    public string? difficulty { get; set; }
    public int? maxTowers { get; set; }
    public bool? disableMK {  get; set; }
    public bool? disableSelling { get; set; }
    public BloonModifiers? bloonModifiers { get; set; }
    public Towers? towers { get; set; }
}

public class StartRules
{
    public int? lives { get; set; }
    public int? cash {  get; set; }
    public int? round {  get; set; }
    public int? endRound { get; set; }
}

public class BloonModifiers
{
    public HealthMultipliers? healthMultipliers { get; set; }
    public double? speedMultiplier { get; set; }
    public double? moabSpeedMultiplier { get; set; }
    public double? regrowRateMultiplier { get; set; }
}

public class HealthMultipliers
{
    public double? bloons { get; set; }
    public double? moabs { get; set; }
}

public class Towers
{
    public List<Items>? _items {  get; set; }
}

public class Items
{
    public string? tower { get; set; }
    public bool? isHero { get; set; }
    public int? max {  get; set; }
}