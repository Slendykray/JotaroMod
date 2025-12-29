using EntityStates;
using EntityStates.Merc;
using RoR2;
using UnityEngine;


namespace HenryMod.Survivors.Henry.SkillStates
{
    public class BaseDash : BaseSkillState
    {

        public static Vector3 dashVector;

        private float stopwatch;

        protected float duration = 0.3f;

        protected float dashSpeed = 15f;

        protected EntityState nextState;


        public override void OnEnter()
        { 
            base.OnEnter();

            dashVector = base.GetAimRay().direction;

            nextState = new RapidPunch();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            characterDirection.forward = dashVector;

            base.characterMotor.rootMotion += BaseDash.dashVector * (this.moveSpeedStat * dashSpeed * base.GetDeltaTime());

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
       
    }
}