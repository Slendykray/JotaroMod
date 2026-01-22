using RoR2;
using UnityEngine;

namespace HenryMod.Survivors.Henry.Components
{
    internal class StarPlatinum: MonoBehaviour
    {             
        private float stopwatch;

        private bool active = false;

        private float minActiveTime = 0.2f;

        public int swingIndex;

        private bool finger = false;

        public Vector3 aimBuffer;

        GameObject GetModel()
        {
            var body = GetComponent<CharacterBody>();

            var modelTransform = body.modelLocator.modelTransform;

            var childLocator = modelTransform.GetComponent<ChildLocator>();

            return childLocator.FindChild("StarPlatinum").gameObject;
        }

        Animator GetAnimator()
        {
            return GetModel().GetComponent<Animator>();
        }

        Animator GetParentAnimator()
        {
            return GetModel().transform.parent.GetComponent<Animator>();
        }

        public void AddTimeBase(float duration)
        {
            GetModel().SetActive(true);

            active = true;
            stopwatch = duration + minActiveTime;

            finger = false;

            Animator anim = GetAnimator();
       
            anim.SetFloat("aimPitchCycle", 0.5f);

            anim.SetFloat("aimYawCycle", 0.5f);

           
        }

        public void AddTime(float duration)
        {
            AddTimeBase(duration);

            PlayAnim(duration);
        }

        public void AddTime(float duration, float speed)
        {
            AddTimeBase(duration);

            PlayAnim(speed);
        }


        public void StarFinger(float duration)
        {
            AddTimeBase(duration);

            finger = true;

            Animator anim = GetAnimator();
            Animator pAnim = GetParentAnimator();

            anim.SetFloat("aimPitchCycle", pAnim.GetFloat("aimPitchCycle"));

            anim.CrossFadeInFixedTime("StarFinger", 0.1f);
        }


        public void PlayAnim(float duration)
        {
            Animator anim = GetAnimator();
        
            bool flag = swingIndex > 1;
            swingIndex = flag ? 1 : 2;

            anim.Update(0f);

            int layerIndex = anim.GetLayerIndex("Punch");

            anim.SetFloat("Punch.playbackRate", 1f);
      
            anim.Play("Punch" + swingIndex);
            anim.Update(0f);

            float length = anim.GetCurrentAnimatorStateInfo(layerIndex).length;

            anim.SetFloat("Punch.playbackRate", length / duration);

        }

        void FixedUpdate()
        {
            if (stopwatch > 0)
            {
                stopwatch -= Time.fixedDeltaTime;
            }             
            else
            {
                if (active)
                {
                    GetModel().SetActive(false);
                    active = false;
                    finger = false;
                }             
            }

            if (finger)
            {
                Animator anim = GetAnimator();
                Animator pAnim = GetParentAnimator();

                anim.SetFloat("aimYawCycle", pAnim.GetFloat("aimYawCycle"));
            }              
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
              
                Animator pAnim = GetParentAnimator();

                bool emote = pAnim.GetBool("Emote");

                if (!emote)
                {
                    Util.PlaySound("YareYareDaze", gameObject);
                    pAnim.SetBool("Emote", true);           
                }
                else
                {
                    pAnim.SetBool("Emote", false);
                }

            }
        }

    }
}
