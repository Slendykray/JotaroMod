using HenryMod.Modules.BaseStates;
using R2API.Utils;
using RoR2;
using UnityEngine;
using HenryMod.Survivors.Henry.Components;

using EntityStates;


namespace HenryMod.Survivors.Henry.SkillStates
{
    public class Punch : BaseMeleeAttack
    {
        public override void OnEnter()
        {
            hitboxGroupName = "PunchGroup";

            //damageType = DamageTypeCombo.GenericPrimary;
            damageType = DamageType.Stun1s;
            damageCoefficient = HenryStaticValues.punchDamageCoefficient;
            procCoefficient = 1f;
            pushForce = 3000f;
            //bonusForce = Vector3.zero;
            //bonusForce = GetAimRay().direction;
            baseDuration = 0.4f;

            //0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            //for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            attackStartPercentTime = 0f;
            attackEndPercentTime = 1f;

            //this is the point at which the attack can be interrupted by itself, continuing a combo
            earlyExitPercentTime = 0.6f;

            hitStopDuration = 0.012f;
            attackRecoil = 0.5f;
            hitHopVelocity = 6f;
             
             
            swingSoundString = "OraMega";
            hitSoundString = "Play_loader_m1_impact";
            muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            playbackRateParam = "Slash.playbackRate";
            //swingEffectPrefab = HenryAssets.swordSwingEffect;
            hitEffectPrefab = HenryAssets.loaderHit;

            //impactSound = HenryAssets.swordHitSoundEvent.index;

            base.OnEnter();

            punchVector = inputBank.aimDirection;
            //characterDirection.forward = punchVector;

            //punchVector = GetAimRay().direction;
            //characterMotor.velocity = punchVector * punchVelocity;
          
            //Util.PlaySound("Ora", gameObject);
            GetComponent<StarPlatinum>().AddTime(duration);
        }


        private float punchVelocity = 75f;

        private Vector3 punchVector;

        private bool hasHit;

        private bool hasKnockbackedSelf;

        private float knockbackForce = 10f;

        public override void FixedUpdate()
        {
            base.FixedUpdate();
           

            if (hasHit && !hasKnockbackedSelf && !inHitPause)
            {
                hasKnockbackedSelf = true;
                characterMotor.velocity = -punchVector * knockbackForce;
                //Util.PlaySound("OraHit", gameObject);

                this.outer.SetNextStateToMain();
            }
            //else
            //{
            //    base.characterMotor.velocity = punchVector * punchVelocity;
            //    base.characterDirection.forward = punchVector;
            //}
        }

        protected override void OnHitEnemyAuthority()
        {
            if (hasHit)
                return;

            base.OnHitEnemyAuthority();

            hasHit = true;
        }

        //protected override void PlayAttackAnimation()
        //{
        //    PlayCrossfade("Gesture, Override", "Slash" + (1 + swingIndex), playbackRateParam, duration, 0.1f * duration);
        //}

        //protected override void PlaySwingEffect()
        //{
        //    base.PlaySwingEffect();
        //}

        //private float speedCoefficientOnExit = 0.4f;
        public override void OnExit()
        {
            //base.characterMotor.velocity *= speedCoefficientOnExit;
            base.OnExit();
        }
    }
}