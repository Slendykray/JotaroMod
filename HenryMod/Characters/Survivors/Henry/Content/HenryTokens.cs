using System;
using HenryMod.Modules;
using HenryMod.Survivors.Henry.Achievements;
using HenryMod.Survivors.Henry.SkillStates;

namespace HenryMod.Survivors.Henry
{
    public static class HenryTokens
    {
        public static void Init()
        {
            AddHenryTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("Henry.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddHenryTokens()
        {
            string prefix = HenrySurvivor.HENRY_PREFIX;

            string desc = "Jotaro is nuts.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > If fully utilized, Ora-Ora does a lot of damage and procs." + Environment.NewLine + Environment.NewLine
             + "< ! > Use ZA WARUDO and ORA to get yourself a good opening." + Environment.NewLine + Environment.NewLine
             + "< ! > You can cancel out of Ora-Ora with ORA." + Environment.NewLine + Environment.NewLine           
             + "< ! > lol." + Environment.NewLine + Environment.NewLine;
    

            string outro = "..yare yare daze.";
            string outroFailure = "..yare yare daze.";
            string lore = "So, Za Warudo is the same type of stand as Star Platinum..";

            Language.Add(prefix + "NAME", "Jotaro");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "Stardust Crusader");
            Language.Add(prefix + "LORE", lore);
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "Henry passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * HenryStaticValues.swordDamageCoefficient}% damage</style>.");

            Language.Add(prefix + "PRIMARY_PUNCH_NAME", "Ora");
            Language.Add(prefix + "PRIMARY_PUNCH_DESCRIPTION", Tokens.agilePrefix + $" Swing for <style=cIsDamage>{100f * HenryStaticValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * HenryStaticValues.gunDamageCoefficient}% damage</style>.");

            Language.Add(prefix + "SECONDARY_PUNCH_NAME", "ORA!");
            Language.Add(prefix + "SECONDARY_PUNCH_DESCRIPTION", $"<style=cIsUtility>Dash</style>. <style=cIsDamage>Stunning</style>. Punch for <style=cIsDamage>{100f * HenryStaticValues.punchDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");

            Language.Add(prefix + "UTILITY_PUNCH_NAME", "Ora-ora-ora-ora-ora!");
            Language.Add(prefix + "UTILITY_PUNCH_DESCRIPTION", $"<style=cIsUtility>Dash</style>. Repeatedly punch for <style=cIsDamage>{100f * HenryStaticValues.rapidPunchDamageCoefficient}% damage</style>.");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * HenryStaticValues.bombDamageCoefficient}% damage</style>.");

            Language.Add(prefix + "SPECIAL_ZAWARUDO_NAME", "ZA WARUDO");          
            Language.Add(prefix + "SPECIAL_ZAWARUDO_DESCRIPTION", $"Stop <style=cIsHealth>FUCKING</style> <style=cIsUtility>TIME</style> for <style=cIsUtility>{StopTime.TIMESTOP_DURATION}s</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier), "Jotaro: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(HenryMasteryAchievement.identifier), "As Jotaro, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
