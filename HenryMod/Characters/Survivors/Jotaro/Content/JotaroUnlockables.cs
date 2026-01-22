using JotaroMod.Survivors.Jotaro.Achievements;
using RoR2;
using UnityEngine;

namespace JotaroMod.Survivors.Jotaro
{
    public static class JotaroUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                JotaroMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(JotaroMasteryAchievement.identifier),
                JotaroSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
