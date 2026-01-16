using HenryMod.Modules.BaseStates;
using RoR2;
using UnityEngine;
using HenryMod.Survivors.Henry.Components;
using UnityEngine.AddressableAssets;

namespace HenryMod.Survivors.Henry.SkillStates
{
    public class SlashCombo : BaseMeleeAttack
    {
       
        public override void OnEnter()
        {
            hitboxGroupName = "SwordGroup";

            damageType = DamageTypeCombo.GenericPrimary;
            damageCoefficient = HenryStaticValues.swordDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 300f;
            bonusForce = Vector3.zero;
            baseDuration = 1f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0.2f;
            attackEndPercentTime = 0.4f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.6f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 6f;
             

                        //swingSoundString = "HenrySwordSwing";
            swingSoundString = "Play_loader_m1_swing";
            hitSoundString = "Play_loader_m1_impact";
            //hitSoundString = FireHook.fireSoundString;
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Slash.playbackRate";
            //swingEffectPrefab = HenryAssets.swordSwingEffect;
            swingEffectPrefab = HenryAssets.swingEffect;
            //var loadedAsset = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/LoaderSwingBasic.prefab").WaitForCompletion();
            //swingEffectPrefab = loadedAsset;
            //hitEffectPrefab = HenryAssets.swordHitImpactEffect;
            //hitEffectPrefab = HenryAssets.loaderHit;

            //var loadedAsset2 = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/ImpactLoaderFistSmall.prefab").WaitForCompletion();
            //var loadedAsset2 = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Loader/OmniImpactVFXLoader.prefab").WaitForCompletion();
            hitEffectPrefab = HenryAssets.loaderHit;
            //impactSound = HenryAssets.swordHitSoundEvent.index;
            //var loadedAsset1 = Addressables.LoadAssetAsync<NetworkSoundEventDef>("RoR2/Junk/Loader/nseLoaderM1Impact.asset").WaitForCompletion();
            //impactSound = HenryAssets.loaderHitSound.index;
            base.OnEnter();
            GetComponent<StarPlatinum>().AddTime(duration);

            swingEffectPrefab.GetComponent<DestroyOnTimer>().duration = baseDuration * attackEndPercentTime;


//            GameObject loaderBody =
//Addressables.LoadAssetAsync<GameObject>(
//"RoR2/Base/Loader/LoaderBody.prefab"
//).WaitForCompletion();
//            var akObj = loaderBody.GetComponent<AkGameObj>();

//            //o = Object.Instantiate(akObj, transform);
//            AkSoundEngine.PostEvent("Play_loader_m1_swing", akObj.gameObject);
           

            //RoR2.Audio.PointSoundManager.EmitSoundLocal((RoR2.Audio.AkEventIdArg)swingSoundString, gameObject.transform.position);
        }

        protected override void PlayAttackAnimation()
        {
            PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}