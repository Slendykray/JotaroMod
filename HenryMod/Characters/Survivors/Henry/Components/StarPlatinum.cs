using RoR2;
using UnityEngine;


namespace HenryMod.Survivors.Henry.Components
{
    internal class StarPlatinum: MonoBehaviour
    {             
        private float stopwatch;
        private bool active = false;
        private float minActiveTime = 1;

        GameObject GetModel()
        {
            var body = GetComponent<CharacterBody>();

            var modelTransform = body.modelLocator.modelTransform;

            var childLocator = modelTransform.GetComponent<ChildLocator>();

            return childLocator.FindChild("StarPlatinum").gameObject;
        }

        public void AddTime(float val)
        {
            GetModel().SetActive(true);

            active = true;
            stopwatch = val + minActiveTime;
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
                }             
            }
                
        }



    }
}
