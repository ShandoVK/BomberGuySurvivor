using BombardierMod.Survivors.Bombardier.Achievements;
using RoR2;
using UnityEngine;

namespace BombardierMod.Survivors.Bombardier
{
    public static class BombardierUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                BombardierMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(BombardierMasteryAchievement.identifier),
                BombardierSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
