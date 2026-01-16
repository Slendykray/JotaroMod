using EntityStates;
using EntityStates.Merc;
using RoR2;
using UnityEngine;


namespace HenryMod.Survivors.Henry.SkillStates
{
    public class TimeStopFreezeState : BaseState
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public override void OnEnter()
        {
            base.OnEnter();
            Animator modelAnimator = base.GetModelAnimator();
            if (modelAnimator)
            {
                //modelAnimator.enabled = false;
                modelAnimator.speed = 0f;
            }
            if (base.rigidbody && !base.rigidbody.isKinematic)
            {
                base.rigidbody.velocity = Vector3.zero;
                //base.rigidbody.angularVelocity = Vector3.zero;
                if (base.rigidbodyMotor)
                {
                    base.rigidbodyMotor.moveVector = Vector3.zero;
                }
            }
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020C0 File Offset: 0x000002C0
        public override void OnExit()
        {
            Animator modelAnimator = base.GetModelAnimator();
            if (modelAnimator)
            {
                //modelAnimator.enabled = true;
             
                modelAnimator.speed = 1f;
            }
            base.OnExit();
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000020E9 File Offset: 0x000002E9
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.isAuthority && base.fixedAge >= this.duration)
            {
                this.outer.SetNextStateToMain();
            }
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002112 File Offset: 0x00000312
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        // Token: 0x04000001 RID: 1
        public float duration;
    }
}