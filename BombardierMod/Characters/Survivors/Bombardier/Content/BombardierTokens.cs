using System;
using BombardierMod.Modules;
using BombardierMod.Survivors.Bombardier.Achievements;

namespace BombardierMod.Survivors.Bombardier
{
    public static class BombardierTokens
    {
        public static void Init()
        {
            AddBombardierTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Bombardier.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddBombardierTokens()
        {
            string prefix = BombardierSurvivor.Bombardier_PREFIX;

            string desc = "Bombardier is a demolitions expert in everything that goes kaboom.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Vaporizer makes foes become walking bombs that can remotely go boom." + Environment.NewLine + Environment.NewLine
             + "< ! > Shrapnel Spread goes boom, then launches multiple mini booms." + Environment.NewLine + Environment.NewLine
             + "< ! > Backfire Ram doesn't make the enemy go boom, but they wish they did." + Environment.NewLine + Environment.NewLine
             + "< ! > Voltaic Vacuums pulls foes together, easy to set them up to go boom." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, ears ringing from countless explosions.";
            string outroFailure = "..and so he vanished, fuses yet to be ignited.";

            Language.Add(prefix + "NAME", "Bombardier");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Primed Grenadier");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Grave Bomber");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Advantageous");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * BombardierStaticValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * BombardierStaticValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * BombardierStaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(BombardierMasteryAchievement.identifier), "Bombardier: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(BombardierMasteryAchievement.identifier), "As Bombardier, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
