using EntityStates;
using EntityStates.Merc;
using JotaroMod.Modules.BaseStates;
using R2API.Utils;
using RoR2;
using UnityEngine;
//using System;
using System.Linq;

using JotaroMod.Survivors.Jotaro.Components;
using System.Collections.Generic;

namespace JotaroMod.Survivors.Jotaro.SkillStates  
{
    public class RapidPunch : BaseState
    {

        private float stopwatch;

        private float attackStopwatch;

        private float damageFrequency = 10f;

        private float proc = 0.6f;

        public static float duration = 2.5f;

        private float minDuration = 0.5f;

        private OverlapAttack overlapAttack;

        private GameObject oraFX;

        private float attackRecoil = 2f;

        public override void OnEnter()
        {           
            base.OnEnter();

            this.overlapAttack = base.InitMeleeOverlap(JotaroStaticValues.rapidPunchDamageCoefficient, JotaroAssets.impactEffect, base.GetModelTransform(), "PunchGroup");

            this.overlapAttack.damageType.damageSource = DamageSource.Utility;

            overlapAttack.procCoefficient = proc;

            oraFX = GameObject.Instantiate(JotaroAssets.oraOraEffect, FindModelChild("SwingCenter"));

            Util.PlaySound("Play_OraOra", gameObject);
          
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            characterMotor.velocity = Vector3.zero;

            characterMotor.velocity.y = 0.5f;

            characterDirection.forward = GetComponent<StarPlatinum>().aimBuffer;

            float deltaTime = base.GetDeltaTime();
            this.stopwatch += deltaTime;
            this.attackStopwatch += deltaTime;
            float num = 1f / damageFrequency / this.attackSpeedStat;
            if (this.attackStopwatch >= num)
            {
                this.attackStopwatch -= num;

                GetComponent<StarPlatinum>().AddTime(num);            

                if (isAuthority)
                {
                    AddRecoil(-1f * attackRecoil, -2f * attackRecoil, -0.5f * attackRecoil, 0.5f * attackRecoil);
                }
    

                overlapAttack.ResetIgnoredHealthComponents();

                List<HurtBox> hitResults = new List<HurtBox>();

                if (!overlapAttack.Fire(hitResults) && stopwatch >= minDuration && isAuthority)
                {
                    outer.SetNextStateToMain();                        
                }

                for (int i = 0; i < hitResults.Count; i++)
                {
                    HealthComponent healthComponent = hitResults[i].healthComponent;
                    if (healthComponent)
                    {
                        Vector3 force = Vector3.zero;
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

            if (this.stopwatch >= duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }

      
        }

        public override void OnExit()
        {
            Util.PlaySound("Stop_OraOra", gameObject);

            Destroy(oraFX);
  
            base.OnExit();
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}