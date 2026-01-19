using EntityStates;
using RoR2;
using UnityEngine;


namespace HenryMod.Survivors.Henry.SkillStates
{
    public class TimeStopFreezeState : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Animator modelAnimator = base.GetModelAnimator();
            if (modelAnimator)
            {

                modelAnimator.speed = 0f;
            }
            if (base.rigidbody && !base.rigidbody.isKinematic)
            {
                base.rigidbody.velocity = Vector3.zero;
 
                if (base.rigidbodyMotor)
                {
                    base.rigidbodyMotor.moveVector = Vector3.zero;
                }
            }
        }

        public override void OnExit()
        {
            Animator modelAnimator = base.GetModelAnimator();
            if (modelAnimator)
            {

                modelAnimator.speed = 1f;
            }
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public float duration;
    }
}