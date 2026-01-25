using EntityStates;
using EntityStates.Merc;
using JotaroMod.Survivors.Jotaro.Components;
using JotaroMod.Modules;
using RoR2;
using UnityEngine;


namespace JotaroMod.Survivors.Jotaro.SkillStates
{
    public class BaseDash : BaseSkillState
    {

        protected Vector3 dashVector;

        private float stopwatch;

        protected float duration = 0.3f;

        protected float dashSpeed = 15f;

        protected EntityState nextState;

  

        public override void OnEnter()
        { 
            base.OnEnter();

            PlayAnimation("FullBody, Override", "Dash", "Dash.playbackRate", duration * 2);

            dashVector = inputBank.aimDirection;
            GetComponent<StarPlatinum>().aimBuffer = dashVector;

            nextState = new RapidPunch();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            characterDirection.forward = dashVector;

            base.characterMotor.rootMotion += dashVector * (this.moveSpeedStat * dashSpeed * base.GetDeltaTime());

            this.stopwatch += GetDeltaTime();

            bool flag = this.stopwatch >= duration;

            if (base.isAuthority)
            {
                Collider[] array;
                int num = HGPhysics.OverlapSphere(out array, base.transform.position, base.characterBody.radius + EvisDash.overlapSphereRadius * (flag ? EvisDash.lollypopFactor : 1f), 
                    LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal);

                for (int i = 0; i < num; i++)
                {
                    HurtBox component = array[i].GetComponent<HurtBox>();
                    if (component && component.healthComponent != base.healthComponent && component.teamIndex != TeamIndex.Player)
                    {
                        this.outer.SetNextState(nextState);
                        break;
                    }
                }
                HGPhysics.ReturnResults(array);
            }

            if (flag && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.characterMotor.velocity *= 0.1f;
            base.OnExit();         
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

    }
}