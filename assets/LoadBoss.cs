using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Data.Boss;
using Il2CppAssets.Scripts.Models.ServerEvents;
using Il2CppAssets.Scripts.Unity.UI_New.ChallengeEditor;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;


namespace CTTileToChallenge.assets;

[HarmonyPatch(typeof(ChallengeEditorPlay), nameof(ChallengeEditorPlay.StartNewGame))]
public static class StartNewGame
{
    [HarmonyPrefix]
    private static void Prefix()
    {
        LoadBoss.Prefix();
    }
}

[HarmonyPatch(typeof(ChallengeEditorPlay), nameof(ChallengeEditorPlay.ContinueClicked))]
public static class ContinueBtn
{
    [HarmonyPrefix]
    private static void Prefix()
    {
        LoadBoss.Prefix();
    }
}

public static class LoadBoss 
{
   private static BossType ConvertIntToBossType(int bossNumber)
   {
        switch (bossNumber)
        {
            case 0: return BossType.Bloonarius;
            case 1: return BossType.Lych;
            case 2: return BossType.Vortex;
            case 3: return BossType.Dreadbloon;
            case 4: return BossType.Phayze;
            case 5: return BossType.Blastapopoulos;
            default: return BossType.Bloonarius; 
        }
   }

    [HarmonyPrefix]
    public static void Prefix()
    {
        var bossNumber = Main.selectedTileDataJson?.GameData?.bossData?.bossBloon; // bosses are stored as integers in the api
        if (bossNumber == null) return;
        
        BossType bossType = ConvertIntToBossType(bossNumber.Value);

        InGameData.Editable.SetupBoss("Boss", bossType, false, true, BossGameData.DefaultSpawnRounds,
            new DailyChallengeModel
            {
                difficulty = InGameData.Editable.selectedDifficulty,
                map = InGameData.Editable.selectedMap,
                mode = InGameData.Editable.selectedMode,
                towers = new TowerData[]
                {
                    new() { isHero = true, tower = DailyChallengeModel.CHOSENPRIMARYHERO, max = 1},
                }.ToIl2CppList()
            }, LeaderboardScoringType.GameTime);
    }
}