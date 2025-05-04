using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Unity;
using CTTileToChallenge;
using CTTileToChallenge.assets;
using BTD_Mod_Helper.Api.ModOptions;
using System.Collections.Generic;
using System.Linq;
using Il2CppAssets.Scripts.Models.ServerEvents;

[assembly: MelonInfo(typeof(CTTileToChallenge.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace CTTileToChallenge
{   
    public class Main : BloonsTD6Mod
    {
        public static TileData? selectedTileDataJson { get; set; }
      
        public static ModSettingString Tile = new ModSettingString("")
        {
            displayName = "Tile Code"
        };

        public static ModSettingInt Event = new ModSettingInt(1)
        {
            displayName = "Event Number"
        };

        public override void OnApplicationStart()
        {
            ModHelper.Msg<Main>("CTTileToChallenge loaded!");
        }

        public override void OnUpdate()
        {
            if (Game.instance?.playerService?.Player == null) return;

            var challengeEditorModel = Game.instance.playerService.Player.Data.challengeEditorModel;

            if (Input.GetKeyDown(KeyCode.X))
            {
                Task.Run(async () =>
                {
                    selectedTileDataJson = await GetApiData.fetchApiData(Tile, Event);
                    if (selectedTileDataJson == null) return;

                    challengeEditorModel.name = Tile;

                    var apiTowers = selectedTileDataJson?.GameData?.dcModel?.towers?._items;
                    var apiModifiers = selectedTileDataJson?.GameData?.dcModel?.bloonModifiers;

                    SetTowersInChallengeEditor(challengeEditorModel.towers, apiTowers);
                    SetModifiersInChallengeEditor(challengeEditorModel.bloonModifiers, apiModifiers);

                    // determine general settings
                    challengeEditorModel.map = selectedTileDataJson?.GameData?.selectedMap ?? "Tutorial";
                    challengeEditorModel.mode = selectedTileDataJson?.GameData?.selectedMode ?? "Standard";
                    challengeEditorModel.difficulty = selectedTileDataJson?.GameData?.selectedDifficulty ?? "Medium";

                    // determine start rules
                    challengeEditorModel.startRules.lives = DetermineLifesForChallenge(selectedTileDataJson?.GameData?.selectedDifficulty ?? "Medium");
                    challengeEditorModel.startRules.cash = selectedTileDataJson?.GameData?.dcModel?.startRules?.cash ?? 650;
                    challengeEditorModel.startRules.round = selectedTileDataJson?.GameData?.dcModel?.startRules?.round ?? 1;
                    challengeEditorModel.startRules.endRound = DetermineEndRound(selectedTileDataJson?.GameData?.dcModel?.startRules?.endRound ?? -1, selectedTileDataJson?.GameData?.bossData?.TierCount ?? 0);

                    // generic modifiers
                    challengeEditorModel.disableMK = selectedTileDataJson?.GameData?.dcModel?.disableMK ?? false;
                    challengeEditorModel.disableSelling = selectedTileDataJson?.GameData?.dcModel?.disableSelling ?? false;
                    challengeEditorModel.disableDoubleCash = true;
                    challengeEditorModel.disableFastTrack = true;
                    challengeEditorModel.disableInstas = true;

                    // SubGameType f.e. least cash/tiers
                    DetermineScoreType(challengeEditorModel, selectedTileDataJson?.GameData?.subGameType ?? 0); // 0 = default mode
                });
            }
        }

        private static void SetTowersInChallengeEditor(Il2CppSystem.Collections.Generic.List<TowerData> towers, List<Items>? apiTowers)
        {
            foreach (var tower in towers)
            {
                var matchingTower = apiTowers?.FirstOrDefault(api => api.tower == tower.tower && api.isHero == tower.isHero);

                if (matchingTower != null && matchingTower.max.HasValue)
                {
                    tower.max = matchingTower.max.Value;
                }
            }
        }

        private static void SetModifiersInChallengeEditor(Il2CppAssets.Scripts.Models.ServerEvents.BloonModifiers challengeModifiers, BloonModifiers? apiModifiers)
        {
            if (challengeModifiers == null || apiModifiers == null) return;

            if (challengeModifiers.healthMultipliers != null && apiModifiers.healthMultipliers != null)
            {
                challengeModifiers.healthMultipliers.bloons = (float)(apiModifiers.healthMultipliers.bloons ?? 1.0f);
                challengeModifiers.healthMultipliers.moabs = (float)(apiModifiers.healthMultipliers.moabs ?? 1.0f);
            }

            challengeModifiers.speedMultiplier = (float)(apiModifiers.speedMultiplier ?? 1.0f);
            challengeModifiers.moabSpeedMultiplier = (float)(apiModifiers.moabSpeedMultiplier ?? 1.0f); 
        }

        private static int DetermineLifesForChallenge(string difficulty)
        {
            switch (difficulty)
            {
                case "Easy": return 200;
                case "Medium": return 150;
                case "Hard": return 100;
                default: return -1;
            }
        }
        public static int DetermineEndRound(int endRound, int? bossTiers)
        {
            switch (bossTiers)
            {
                case 1: return 59;
                case 2: return 79;
                default: return endRound;
            }
        }

        private static void DetermineScoreType(DailyChallengeModel challengeEditorModel, int subGameType)
        {
            switch (subGameType)
            {
                case 8: 
                    challengeEditorModel.leastCashUsed = 99999999;
                    break;
                case 9: 
                    challengeEditorModel.leastTiersUsed = 99999999;
                    break;
                default:
                    challengeEditorModel.ignoreLeastMode = true;
                    break;
            }       
        }
    }
}
