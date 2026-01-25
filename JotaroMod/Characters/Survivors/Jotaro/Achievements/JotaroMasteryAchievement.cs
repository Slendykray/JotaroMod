using RoR2;
using JotaroMod.Modules.Achievements;

namespace JotaroMod.Survivors.Jotaro.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class JotaroMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = JotaroSurvivor.HENRY_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = JotaroSurvivor.HENRY_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => JotaroSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}