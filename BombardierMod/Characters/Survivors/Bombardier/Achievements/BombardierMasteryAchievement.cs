using RoR2;
using BombardierMod.Modules.Achievements;

namespace BombardierMod.Survivors.Bombardier.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class BombardierMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = BombardierSurvivor.Bombardier_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = BombardierSurvivor.Bombardier_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => BombardierSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}