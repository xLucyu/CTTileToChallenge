using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2CppAssets.Scripts.Data.Boss;
using Il2CppAssets.Scripts.Models.ServerEvents;
using Il2CppAssets.Scripts.Unity.UI_New.ChallengeEditor;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;


namespace CTTileToChallenge.assets;

[HarmonyPatch(typeof(ChallengeEditorPlay), nameof(ChallengeEditorPlay.StartNewGame))]
public static class LoadBoss 
{
    private static BossType ConvertIntToBossType(int bossID)
    {
        return bossID switch
        {
            0 => BossType.Bloonarius,
            1 => BossType.Lych,
            2 => BossType.Vortex,
            3 => BossType.Dreadbloon,
            4 => BossType.Phayze,
            5 => BossType.Blastapopoulos,
            _ => BossType.Bloonarius
        };
    }

    [HarmonyPrefix]
    private static void Prefix()
    {
        var bossType = Main.selectedTileDataJson?.GameData?.bossData?.bossBloon;

        if (bossType == null) return;

        BossType bossElement = ConvertIntToBossType(bossType.Value);
        
        InGameData.Editable.SetupBoss("Bloonarius", bossElement, false, true, BossGameData.DefaultSpawnRounds,
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