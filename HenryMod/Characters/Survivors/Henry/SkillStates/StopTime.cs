using EntityStates;
using EntityStates.Merc;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace HenryMod.Survivors.Henry.SkillStates
{
    public class StopTime : BaseSkillState
    {

        private float stopwatch;

        public static float TIMESTOP_DURATION = 5f;
        private static float duration = 5f;
        public static CharacterBody characterBodyUsed = null;

        public static bool TIMESTOP_ACTIVE = false;

        public override void OnEnter()
        {
            if (TIMESTOP_ACTIVE)
            {
                return;
            }

            base.OnEnter();


            TIMESTOP_ACTIVE = true;

            //Chat.AddMessage("<style=cWorldEvent>ZA WARUDO</style>");
            Log.Warning("ZA WARUDO");

            //On.RoR2.CharacterBody.Start += (orig, self) =>
            //{
            //    characterBodyUsed = self;
            //    orig(self);
            //};
            characterBodyUsed = base.characterBody;
            //characterBodyUsed = GetComponent<CharacterBody>();
            //Time.timeScale = 0.3f;
            //characterBodyUsed = master.GetBody();
            //TimeStopFreezeAllEnemies();

            TimeStopFreezeAllEnemies();
        }

        public override void FixedUpdate()
        {       
            base.FixedUpdate();

            this.stopwatch += GetDeltaTime();

            if (this.stopwatch >= TIMESTOP_DURATION && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            //Time.timeScale = 1;
            //ForceEndTimeStop();
            //Chat.AddMessage("<style=cWorldEvent>napotuzhnichav</style>");
            Log.Warning("ZA WARUDO stop");
            TIMESTOP_ACTIVE = false;
            base.OnExit();        
        }




    


        private static readonly float TIMESTOP_STARTUP_DURATION = 2f;

   


        private static float TIMESTOP_STARTED_TIME = 0f;

        

        public static void TimeStopHooks()
        {
            On.RoR2.CombatDirector.Simulate += CombatDirector_Simulate_Hook;

            On.RoR2.Run.ShouldUpdateRunStopwatch += Run_ShouldUpdateRunStopwatch_Hook;
        }

        private static void CombatDirector_Simulate_Hook(On.RoR2.CombatDirector.orig_Simulate orig, CombatDirector self, float deltaTime)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self, deltaTime);
            }
        }

        private static bool Run_ShouldUpdateRunStopwatch_Hook(On.RoR2.Run.orig_ShouldUpdateRunStopwatch orig, Run self)
        {
            return orig.Invoke(self) && !TIMESTOP_ACTIVE;
        }


        public static void TimeStopFreezeAllEnemies()
        {
            if (StopTime.characterBodyUsed != null)
            {
                //StopTime.TimeStopEnemies(Physics.OverlapSphere(StopTime.characterBodyUsed.transform.position, float.MaxValue, LayerIndex.entityPrecise.mask).ToArray<Collider>(),
                //    StopTime.characterBodyUsed, StopTime.TIMESTOP_STARTED_TIME + (StopTime.TIMESTOP_STARTUP_DURATION + StopTime.TIMESTOP_DURATION + StopTime.TIMESTOP_STARTUP_DURATION - 1f) - Time.time);

                StopTime.TimeStopEnemies(Physics.OverlapSphere(StopTime.characterBodyUsed.transform.position, float.MaxValue, LayerIndex.entityPrecise.mask).ToArray<Collider>(),
                  StopTime.characterBodyUsed, StopTime.TIMESTOP_DURATION);
            }
        }

        public static void ForceEndTimeStop()
        {
      
            //if (StopTime.timeStopFX != null)
            //{
            //    LeanTween.cancel(StopTime.timeStopFX);
            //}
            if (StopTime.characterBodyUsed != null)
            {
                StopTime.TimeStopEnemies(Physics.OverlapSphere(StopTime.characterBodyUsed.transform.position, float.MaxValue, LayerIndex.entityPrecise.mask).ToArray<Collider>(), StopTime.characterBodyUsed, 0f);
            }
            //StopTime.OnTimeStop_PreEnd();
            //StopTime.OnTimeStop_End();
        }

        private static bool TimeStopEnemies(Collider[] array, CharacterBody self, float ts_duration)
        {
            IEnumerable<Collider> enumerable = from x in array
                                               where x.GetComponent<HurtBox>() != null
                                               select x;
            List<HurtBoxGroup> list = new List<HurtBoxGroup>();
            foreach (Collider collider in enumerable)
            {
                HurtBox hurtBox2 = collider.GetComponentInChildren<HurtBox>();
                if (!(hurtBox2 == null) && !(hurtBox2.healthComponent == self.healthComponent) && (from x in list
                                                                                                   where x == hurtBox2.hurtBoxGroup
                                                                                                   select x).Count<HurtBoxGroup>() <= 0 && (hurtBox2.teamIndex != self.teamComponent.teamIndex || FriendlyFireManager.friendlyFireMode != FriendlyFireManager.FriendlyFireMode.Off))
                {
                    HurtBox hurtBox = hurtBox2;
                    list.Add(hurtBox2.hurtBoxGroup);
                    HealthComponent componentInChildren = hurtBox2.healthComponent.gameObject.GetComponentInChildren<HealthComponent>();
                    if (componentInChildren != null)
                    {
                        CharacterBody body = componentInChildren.body;
                        if (body != null)
                        {
                            EntityStateMachine component = body.GetComponent<EntityStateMachine>();
                            if (component != null)
                            {
                                TimeStopFreezeState state = new TimeStopFreezeState
                                {
                                    duration = ts_duration
                                };
                                component.SetState(state);
                            }
                        }
                    }
                }
            }
            return list != null && list.Count<HurtBoxGroup>() > 0;
        }

    }
}