using System;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;
using BTD_Mod_Helper;
using Il2CppAssets.Scripts.Unity;
using CTTileToChallenge;
using CTTileToChallenge.assets;
using BTD_Mod_Helper.Api.ModOptions;

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

            if (Input.GetKeyDown(KeyCode.X))
            {
                Task.Run(async () =>
                {
                    Main.selectedTileDataJson = await GetApiData.fetchApiData(Tile, Event);
                    Console.WriteLine($"Map: {selectedTileDataJson?.GameData?.bossData?.bossBloon}");

                });
            }
        }
    }
}
