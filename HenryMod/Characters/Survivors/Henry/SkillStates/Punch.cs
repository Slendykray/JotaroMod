using HenryMod.Modules.BaseStates;
using R2API.Utils;
using RoR2;
using UnityEngine;
using HenryMod.Survivors.Henry.Components;

using EntityStates;
using System.Collections.Generic;

using System;

using System.Linq;
using EntityStates.Loader;

namespace HenryMod.Survivors.Henry.SkillStates
{
    public class Punch : BaseState
    {
        private OverlapAttack overlapAttack;

        private float punchForce = 35f;

        private float knockbackForce = 10f;

        private float duration = 0.3f;

        private float attackRecoil = 3f;

        public override void OnEnter()
        {

            //damageType = DamageType.Stun1s;
            //damageCoefficient = HenryStaticValues.punchDamageCoefficient;
            //procCoefficient = 1f;
            //pushForce = 300f;
            ////bonusForce = Vector3.zero;
            //baseDuration = 1f;

            ////0-1 multiplier of baseduration, used to time when the hitbox is out (usually based on the run time of the animation)
            ////for example, if attackStartPercentTime is 0.5, the attack will start hitting halfway through the ability. if baseduration is 3 seconds, the attack will start happening at 1.5 seconds
            //attackStartPercentTime = 0.2f;
            //attackEndPercentTime = 0.4f;

            ////this is the point at which the attack can be interrupted by itself, continuing a combo
            //earlyExitPercentTime = 0.6f;

            //hitStopDuration = 0.012f;
            //attackRecoil = 0.5f;
            //hitHopVelocity = 6f;

            //swingSoundString = "OraMega";
            //hitSoundString = "Play_loader_m1_impact";

            ////swingEffectPrefab = HenryAssets.swingEffect;
            //hitEffectPrefab = HenryAssets.impactEffect;

            //muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            //playbackRateParam = "Slash.playbackRate";

            base.OnEnter();
             
            Util.PlaySound("OraMega", gameObject);

            this.overlapAttack = base.InitMeleeOverlap(HenryStaticValues.punchDamageCoefficient, HenryAssets.impactEffect, base.GetModelTransform(), "PunchGroup");

            this.overlapAttack.damageType.damageSource = DamageSource.Secondary;
            this.overlapAttack.damageType.damageType = DamageType.Stun1s;
       

            GetComponent<StarPlatinum>().AddTime(duration);

            Vector3 aim = GetComponent<AimBuffer>().direction;
            characterDirection.forward = aim;

            List<HurtBox> hitResults = new List<HurtBox>();

            if (overlapAttack.Fire(hitResults))
            {
                characterMotor.velocity = -aim * knockbackForce;
                SmallHop(characterMotor, 4);

                if (isAuthority)
                {
                    AddRecoil(-1f * attackRecoil, -2f * attackRecoil, -0.5f * attackRecoil, 0.5f * attackRecoil);
                }


                for (int i = 0; i < hitResults.Count; i++)
                {
                    HealthComponent healthComponent = hitResults[i].healthComponent;
                    if (healthComponent)
                    {
                        Vector3 force = aim.normalized * punchForce;
                        CharacterMotor motor = healthComponent.body.characterMotor;
                        if (motor)
                        {
                            motor.velocity = force;
                        }

                        Rigidbody rb = healthComponent.body.rigidbody;
                        if (rb)
                        {
                            rb.velocity = force;
                        }

                    }
                }
            }


            this.outer.SetNextStateToMain();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            //if (this.stopwatch >= duration && base.isAuthority)
            //{
            //    this.outer.SetNextStateToMain();
            //}
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}