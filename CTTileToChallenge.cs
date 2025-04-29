using MelonLoader;
using BTD_Mod_Helper;
using CTTileToChallenge;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Unity;
using CTTileToChallenge.api;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;


[assembly: MelonInfo(typeof(CTTileToChallenge.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace CTTileToChallenge
{
    public class Main : BloonsTD6Mod
    {

        public static readonly ModSettingString Tile = new ModSettingString("MRX")
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
            string selectedTileCode = Tile;
            int selectedEventNumber = Event;

            GetApiData fulltileInfo = new GetApiData();
            if (Input.GetKeyDown(KeyCode.X))
            {
                Task.Run(async () =>
                {
                    var json = await GetApiData.FetchData(Tile, Event);
                    ModHelper.Msg<Main>(json);
                });
            }

           // var challengeEditorModel = Game.instance.playerService.Player.Data.challengeEditorModel;
            
        }
    }
}
