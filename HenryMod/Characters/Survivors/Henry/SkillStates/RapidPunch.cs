using EntityStates;
using EntityStates.Merc;
using HenryMod.Modules.BaseStates;
using R2API.Utils;
using RoR2;
using UnityEngine;
//using System;
using System.Linq;

using UnityEngine.Networking;
using HenryMod.Survivors.Henry.Components;
using EntityStates.Loader;

namespace HenryMod.Survivors.Henry.SkillStates
{
    public class RapidPunch : BaseSkillState
    {

        private float stopwatch;

        private float attackStopwatch;

        private float damageFrequency = 10f;

        private float duration = 1.5f;

        private float minDuration = 0.5f;

        private OverlapAttack overlapAttack;

        public override void OnEnter()
        {           
            base.OnEnter();
            //this.overlapAttack = base.InitMeleeOverlap(HenryStaticValues.rapidPunchDamageCoefficient, WhirlwindBase.hitEffectPrefab, base.GetModelTransform(), "PunchGroup");
            this.overlapAttack = base.InitMeleeOverlap(HenryStaticValues.rapidPunchDamageCoefficient, LoaderMeleeAttack.overchargeImpactEffectPrefab, base.GetModelTransform(), "PunchGroup");

            this.overlapAttack.damageType.damageSource = DamageSource.Utility;

            GetComponent<StarPlatinum>().AddTime(duration);      
        }

  
     

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


  
              // Util.PlayAttackSpeedSound(WhirlwindBase.attackSoundString, base.gameObject, WhirlwindBase.slashPitch);

                //Util.PlayAttackSpeedSound("HenrySwordSwing", gameObject, attackSpeedStat);

                //EffectManager.SimpleMuzzleFlash(WhirlwindBase.swingEffectPrefab, gameObject, "SwingCenter", false);

                EffectManager.SimpleMuzzleFlash(HenryAssets.oraOraEffect, gameObject, "SwingCenter", false);

                Util.PlaySound("OraOra", gameObject);
                //PlayCrossfade("Gesture, Override", "Slash" + 1, "Slash.playbackRate", duration, 0.05f);

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
            base.OnExit();
        }

     
     
    }
}