using MelonLoader;
using UnityEngine;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Unity;
using System.Threading.Tasks;
using CTTileToChallenge;
using CTTileToChallenge.api;
using System;


[assembly: MelonInfo(typeof(CTTileToChallenge.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace CTTileToChallenge
{
    public class Main : BloonsTD6Mod
    {

        public static readonly ModSettingString Tile = new ModSettingString("")
        {
            displayName = "Tile Code"
        };

        public static readonly ModSettingInt Event = new ModSettingInt(1)
        {
            displayName = "Event Number"
        };

        public override void OnApplicationStart()
        {
            ModHelper.Msg<Main>("CTTileToChallenge loaded!");
        }

        public override void OnUpdate()
        {
            if (Game.instance == null || Game.instance.playerService == null || Game.instance.playerService.Player == null)
            {
                return;
            }

            string selectedTileCode = Tile;
            int selectedEventNumber = Event;
            var challengeEditorModel = Game.instance.playerService.Player.Data.challengeEditorModel;

            GetApiData fulltileInfo = new GetApiData();
            if (Input.GetKeyDown(KeyCode.X))
            {
                Task.Run(async () =>
                {
                    var selectedTileDataJson = await GetApiData.fetchApiData(Tile, Event);
                    Console.WriteLine(selectedTileDataJson);
                    Console.WriteLine(selectedTileDataJson?.GameData?.dcModel?.bloonModifiers?.speedMultiplier);
                });
                
            }
        }
    }
}
