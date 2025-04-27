using MelonLoader;
using BTD_Mod_Helper;
using CTTileToChallenge;
using Il2CppAssets.Scripts.Unity;
using UnityEngine;
using System.Linq;
using CTTileToChallenge.api;

[assembly: MelonInfo(typeof(CTTileToChallenge.CTTileToChallenge), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace CTTileToChallenge
{
    public class CTTileToChallenge : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            ModHelper.Msg<CTTileToChallenge>("CTTileToChallenge loaded!");
        }

        // Corrected the method name case to "OnNewGameModel"
        public override void OnUpdate()
        {
 
            string[] allowedTowers = { "BoomerangMonkey", "Psi", "WizardMonkey", "NinjaMonkey", "Mermonkey", "BeastHandler" };

            if (Input.GetKeyDown(KeyCode.X))
            {
                var challengeEditorModel = Game.instance.playerService.Player.Data.challengeEditorModel;
                
                foreach (var tower in challengeEditorModel.towers)
                {
                    if (allowedTowers.Contains(tower.tower))
                    {
                        tower.max = -1;
                    }
                    else
                    {
                        tower.max = 0;
                    }
                }
            }
        }
    }
}
