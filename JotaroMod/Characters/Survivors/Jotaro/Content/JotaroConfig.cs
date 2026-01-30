using BepInEx.Configuration;
using JotaroMod.Modules;
using UnityEngine;

namespace JotaroMod.Survivors.Jotaro
{
    public static class JotaroConfig
    {
        public static ConfigEntry<float> baseHealth;
        public static ConfigEntry<float> baseRegen;
        public static ConfigEntry<float> baseArmor;
        public static ConfigEntry<float> baseDamage;
        public static ConfigEntry<float> baseMovementSpeed;
        public static ConfigEntry<int> jumpCount;


        public static ConfigEntry<float> primaryDamageCoefficient;

        public static ConfigEntry<float> secondaryDamageCoefficient;
        public static ConfigEntry<float> secondaryCD;

        public static ConfigEntry<float> secondaryAltDamageCoefficient;
        public static ConfigEntry<float> secondaryAltCD;

        public static ConfigEntry<float> utilityDamageCoefficient;
        public static ConfigEntry<float> utilityCD;

        public static ConfigEntry<float> timeStopDuration;
        public static ConfigEntry<float> specialCD;


        public static ConfigEntry<KeyboardShortcut> emoteButton;
        public static ConfigEntry<float> voiceVolume;
        public static ConfigEntry<float> secondaryChance;
        public static ConfigEntry<float> utilityChance;
        public static ConfigEntry<float> specialChance;
        public static void Init()
        {


            baseHealth = Config.BindAndOptions("02 - Stats", "baseHealth", 160f);
            baseRegen = Config.BindAndOptions("02 - Stats", "baseRegen", 1.5f);
            baseArmor = Config.BindAndOptions("02 - Stats", "baseArmor", 20f);
            baseDamage = Config.BindAndOptions("02 - Stats", "baseDamage", 12f);
            baseMovementSpeed = Config.BindAndOptions("02 - Stats", "baseMovementSpeed", 7f);
            jumpCount = Config.BindAndOptions("02 - Stats", "jumpCount", 1);



            primaryDamageCoefficient = Config.BindAndOptions("03 - Primary", "DamageCoefficient", 2.8f);

            secondaryDamageCoefficient = Config.BindAndOptions("04 - Secondary", "DamageCoefficient", 7f);
            secondaryCD = Config.BindAndOptions("04 - Secondary", "Cooldown", 5f);

            secondaryAltDamageCoefficient = Config.BindAndOptions("05 - SecondaryAlt", "DamageCoefficient", 12f);
            secondaryAltCD = Config.BindAndOptions("05 - SecondaryAlt", "Cooldown", 5f);

            utilityDamageCoefficient = Config.BindAndOptions("06 - Utility", "DamageCoefficient", 1.6f);
            utilityCD = Config.BindAndOptions("06 - Utility", "Cooldown", 6f);

            timeStopDuration = Config.BindAndOptions("07 - ZaWarudo", "TimeStopDuration", 5f);
            specialCD = Config.BindAndOptions("07 - ZaWarudo", "Cooldown", 10f);




            voiceVolume = Config.BindAndOptions("Voice Lines", "Volume", 100f, risk: true);
            secondaryChance = Config.BindAndOptions("Voice Lines", "Secondary Probability", 100f, description: "chance to play voiceline", risk: true);
            utilityChance = Config.BindAndOptions("Voice Lines", "Utility Probability", 100f, description: "chance to play voiceline", risk: true);
            specialChance = Config.BindAndOptions("Voice Lines", "Special Probability", 100f, description: "chance to play voiceline", risk: true);

            emoteButton = Config.BindAndOptions("Emote", "YareYare", new KeyboardShortcut(KeyCode.None), risk: true);
        }

        public static bool Roll(float chance)
        {
            return Random.value < chance / 100;
        }

    }
}
