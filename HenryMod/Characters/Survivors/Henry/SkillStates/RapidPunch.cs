using EntityStates;
using EntityStates.Merc;
using HenryMod.Modules.BaseStates;
using R2API.Utils;
using RoR2;
using UnityEngine;
//using System;
using System.Linq;

using HenryMod.Survivors.Henry.Components;
using UnityEngine.AddressableAssets;

namespace HenryMod.Survivors.Henry.SkillStates  
{
    public class RapidPunch : BaseSkillState
    {

        private float stopwatch;

        private float attackStopwatch;

        private float damageFrequency = 10f;

        public static float duration = 4f;

        private float minDuration = 0.5f;

        private OverlapAttack overlapAttack;

        private GameObject oraFX;

        public override void OnEnter()
        {           
            base.OnEnter();

            this.overlapAttack = base.InitMeleeOverlap(HenryStaticValues.rapidPunchDamageCoefficient, HenryAssets.loaderHit, base.GetModelTransform(), "PunchGroup");

            this.overlapAttack.damageType.damageSource = DamageSource.Utility;

            overlapAttack.procCoefficient = 0.8f;

            oraFX = GameObject.Instantiate(HenryAssets.oraOraEffect, FindModelChild("SwingCenter"));

            Util.PlaySound("Play_OraOra", gameObject);
          
        }


        private float attackRecoil = 2f;

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            characterMotor.velocity = Vector3.zero;

            characterDirection.forward = GetComponent<AimBuffer>().direction;

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

                if (!overlapAttack.Fire() && stopwatch >= minDuration && isAuthority)
                {
                    outer.SetNextStateToMain();                        
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