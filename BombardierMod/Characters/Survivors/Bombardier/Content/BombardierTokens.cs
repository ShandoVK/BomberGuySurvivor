using BombardierMod.Modules;
using BombardierMod.Survivors.Bombardier.Achievements;
using R2API;
using System;

namespace BombardierMod.Survivors.Bombardier
{
    public static class BombardierTokens
    {
        public const string KEYWORD_PRIMED = Tokens.primedPrefix + "Marks an enemy for detonation. Blast damage is based on the number of prime stacks present on the enemy.";

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

            string desc = "Bombardier is a demolitionist expert in with a wide array of explosives to clear hordes of foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Vaporizer converts enemies into walking bombs, ready to detonate them at any time." + Environment.NewLine + Environment.NewLine
             + "< ! > Shrapnel Spread is a high-grade explosive that splits into other smaller explosives." + Environment.NewLine + Environment.NewLine
             + "< ! > Backfire Ram provides spacing safety from high density groups while still remaining aggressive." + Environment.NewLine + Environment.NewLine
             + "< ! > Voltaic Vacuums pulls foes together and shuts them down for crowd clearing moments." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, ears ringing from countless explosions.";
            string outroFailure = "..and so he vanished, fuses yet to be ignited.";

            Language.Add(prefix + "NAME", "Bombardier");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Primed Grenadier");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);
            LanguageAPI.Add("KEYWORD_PRIMED", KEYWORD_PRIMED);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Grave Bomber");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Advantageous");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", $"Attacks are <style=cIsDamage>{10f * BombardierStaticValues.survivorDebuffPassiveCoefficient}%</style> stronger for every <style=cIsUtility>debuff</style> the enemy is inflicted by.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Vaporizer"); // Remember to readjust values, there's 2 here
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.primedPrefix + $"Fire your arm cannon for <style=cIsDamage>{100f * BombardierStaticValues.swordDamageCoefficient}% damage</style> and inflicts <style=cIsDamage>primed</style> " +
                                                               $"to the enemy struck. Charging the attack allows you to detonate targets afflicted by <style=cIsDamage>primed</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Shrapnel Spread"); // Again, 2 values
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", $"Throw an explosive that deals <style=cIsDamage>{100f * BombardierStaticValues.gunDamageCoefficient}% damage</style>. " + 
                                                               $"Upon impact, it splits into 4 smaller bombs to deal <style=cIsDamage>{100f * BombardierStaticValues.spreadMiniDamageCoefficient}% damage</style> each.");
            
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Backfire Ram"); // And 2- oh wait no values here.
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", Tokens.ignitePrefix + $"Rush forward a short distance, pushing nearby enemies away from you. Inflicts <style=cIsDamage>burn</style> to enemies pushed back by " + 
                                                              $"this attack. <style=cIsUtility>You cannot be hit during the rush.</style>");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Voltaic Vacuum"); // Guess what, 2 values go here too!
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", Tokens.shockingPrefix + $"Throw a bomb that unleashes a vortex that pulls in and <style=cIsUtility>electrocutes</style> enemies for <style=cIsDamage>{100f * BombardierStaticValues.bombDamageCoefficient}%" +
                                                              $" damage</style>. After some time, the vortex collapses and releases a <style=cIsUtility>discharge</style> for <style=cIsDamage>{100f * BombardierStaticValues.vacuumFinishDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(BombardierMasteryAchievement.identifier), "Bombardier: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(BombardierMasteryAchievement.identifier), "As Bombardier, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
