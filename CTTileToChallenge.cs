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
using System;
using UnityEngine.Playables;
using Il2CppAssets.Scripts.Unity.UI_New.ChallengeEditor;

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
                    var selectedTileDataJson = await GetApiData.fetchApiData(Tile, Event);
                    if (selectedTileDataJson == null) return;

                    challengeEditorModel.name = Tile;

                    var apiTowers = selectedTileDataJson?.GameData?.dcModel?.towers?._items;
                    var apiModifiers = selectedTileDataJson?.GameData?.dcModel?.bloonModifiers;

                    SetTowersInChallengeEditor(challengeEditorModel.towers, apiTowers);
                    SetModifiersInChallengeEditor(challengeEditorModel.bloonModifiers, apiModifiers);

                    challengeEditorModel.map = selectedTileDataJson?.GameData?.selectedMap;
                    challengeEditorModel.mode = selectedTileDataJson?.GameData?.selectedMode;
                    challengeEditorModel.difficulty = selectedTileDataJson?.GameData?.SelectedDifficulty;

                    challengeEditorModel.startRules.lives = selectedTileDataJson?.GameData?.dcModel?.startRules?.lives ?? -1;
                    challengeEditorModel.startRules.cash = selectedTileDataJson?.GameData?.dcModel?.startRules?.cash ?? 650;
                    challengeEditorModel.startRules.round = selectedTileDataJson?.GameData?.dcModel?.startRules?.round ?? 150;
                    challengeEditorModel.startRules.endRound = selectedTileDataJson?.GameData?.dcModel?.startRules?.endRound ?? -1;

                    challengeEditorModel.disableMK = selectedTileDataJson?.GameData?.dcModel?.disableMK ?? false; // bool MonkeyKnowledge

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

    }
}
