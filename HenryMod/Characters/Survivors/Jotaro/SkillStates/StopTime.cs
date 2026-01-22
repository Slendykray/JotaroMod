using EntityStates;
using RoR2;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using EntityStates.ClayBruiser.Weapon;
using EntityStates.GolemMonster;
using EntityStates.TitanMonster;
using RoR2.Orbs;
using RoR2.Projectile;
using EntityStates.Wisp1Monster;
using EntityStates.GreaterWispMonster;
using R2API.Utils;

namespace JotaroMod.Survivors.Jotaro.SkillStates
{
    public class StopTime : BaseSkillState
    {

        public static CharacterBody characterBodyUsed = null;

        private float stopwatch;

        private static float TIMESTOP_STARTUP_DURATION = 0.2f;

        public static float TIMESTOP_DURATION = 5f;
 
        public static bool TIMESTOP_ACTIVE = false;

        private bool end = false;

        private float startEnd = 0.75f;

        private static GameObject timeStopFX;

        PostProcessVolume volume;
        ColorGrading cg;
        LensDistortion ld;

        public override void OnEnter()
        {
            base.OnEnter();

            characterBodyUsed = base.characterBody;

            Util.PlaySound("ZaWarudo", gameObject);
        }

    

        public override void FixedUpdate()
        {       
            base.FixedUpdate();

            this.stopwatch += GetDeltaTime();

            if (this.stopwatch >= TIMESTOP_DURATION + TIMESTOP_STARTUP_DURATION && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }

            if (stopwatch >= TIMESTOP_STARTUP_DURATION && !TIMESTOP_ACTIVE)
            {
                timeStopFX = GameObject.Instantiate(JotaroAssets.ZaWarudoEffect);

                timeStopFX.layer = LayerMask.NameToLayer("PostProcess");

                volume = StopTime.timeStopFX.GetComponent<PostProcessVolume>();

                volume.profile.TryGetSettings(out cg);
                volume.profile.TryGetSettings(out ld);

                ld.intensity.value = 0f;
                cg.saturation.value = 0f;
                cg.hueShift.value = 0f;

                foreach (ParticleSystem particleSystem in UnityEngine.Object.FindObjectsOfType<ParticleSystem>())
                {
                    if (!particleSystem.name.Contains("OraOraEffect"))
                        particleSystem.Pause(true);
                }

                TIMESTOP_ACTIVE = true;

                TimeStopFreezeAllEnemies();

                Util.PlaySound("ZaWarudoStart", gameObject);
            }


            if (stopwatch >= TIMESTOP_DURATION + TIMESTOP_STARTUP_DURATION - startEnd && !end)
            {
                end = true;
                Util.PlaySound("ZaWarudoEnd", gameObject);
            }
        }

      
        public override void Update()
        {
            base.Update();

            if (!TIMESTOP_ACTIVE)
                return;

            Animate(cg.hueShift, 0f, 180f, 0f, 0.5f, stopwatch);
            Animate(ld.intensity, 0f, 65f, 0f, 0.5f, stopwatch);

            Animate(cg.saturation, 0f, -75f, 0.5f, 1f, stopwatch);
            Animate(ld.intensity, 65f, 0f, 0.5f, 1f, stopwatch);

            Animate(cg.saturation, -75f, 0f, TIMESTOP_DURATION - startEnd, startEnd, stopwatch);
            Animate(cg.hueShift, 180f, 0f, TIMESTOP_DURATION - startEnd, startEnd, stopwatch);

            volume.weight += 1;
        }

        void Animate(
        FloatParameter param,
        float from,
        float to,
        float start,
        float duration,
        float time
        )
        {
            float newTime = time - (start + TIMESTOP_STARTUP_DURATION);

            if (newTime <= 0)
                return;
            float t = newTime / duration;
            param.value = Mathf.Lerp(from, to, t);
        }
        
        public override void OnExit()
        {
            TIMESTOP_ACTIVE = false;

            Destroy(timeStopFX);

            foreach (ProjectileSimple projectileSimple in UnityEngine.Object.FindObjectsOfType<ProjectileSimple>())
            {
                Reflection.InvokeMethod(projectileSimple, "SetForwardSpeed", new object[]
                {
                    projectileSimple.desiredForwardSpeed
                });
            }
            foreach (ParticleSystem particleSystem in UnityEngine.Object.FindObjectsOfType<ParticleSystem>())
            {
                if (particleSystem.isPaused)
                {
                    particleSystem.Play(true);
                }
            }

            base.OnExit();        
        }



        #region Hooks
        public static void TimeStopHooks()
        {
            On.RoR2.CombatDirector.Simulate += CombatDirector_Simulate_Hook;
            On.RoR2.Run.ShouldUpdateRunStopwatch += Run_ShouldUpdateRunStopwatch_Hook;

            On.RoR2.Projectile.ProjectileSimple.FixedUpdate += ProjectileSimple_FixedUpdate_Hook;
            On.RoR2.Orbs.OrbManager.FixedUpdate += OrbManager_FixedUpdate_Hook;
            On.RoR2.Orbs.OrbEffect.UpdateOrb += OrbEffect_UpdateOrb_Hook;
            On.RoR2.Projectile.ProjectileImpactExplosion.FixedUpdate += ProjectileImpactExplosion_FixedUpdate_Hook;
            On.RoR2.RigidbodyMotor.FixedUpdate += RigidbodyMotor_FixedUpdate_Hook;
            On.RoR2.DelayBlast.FixedUpdate += DelayBlast_FixedUpdate_Hook;
            On.RoR2.DestroyOnTimer.FixedUpdate += DestroyOnTimer_FixedUpdate_Hook;

            On.EntityStates.GolemMonster.ChargeLaser.Update += ChargeLaser_Update;
            On.EntityStates.GolemMonster.ChargeLaser.FixedUpdate += ChargeLaser_FixedUpdate;
            On.EntityStates.TitanMonster.ChargeMegaLaser.Update += ChargeMegaLaser_Update_Hook;
            On.EntityStates.TitanMonster.ChargeMegaLaser.FixedUpdate += ChargeMegaLaser_FixedUpdate_Hook;
            On.EntityStates.TitanMonster.FireMegaLaser.FixedUpdate += FireMegaLaser_FixedUpdate_Hook;
            On.EntityStates.TitanMonster.ChargeGoldMegaLaser.FixedUpdate += ChargeGoldMegaLaser_FixedUpdate_Hook;
            On.EntityStates.TitanMonster.FireGoldMegaLaser.FixedUpdate += FireGoldMegaLaser_FixedUpdate_Hook;
            On.RoR2.WormBodyPositionsDriver.FixedUpdateServer += WormBodyPositionsDriver_FixedUpdateServer_Hook;
            On.RoR2.WormBodyPositions2.OnDeathStart += WormBodyPositions2_OnDeathStart_Hook;
            On.EntityStates.ClayBruiser.Weapon.MinigunFire.FixedUpdate += MinigunFire_FixedUpdate_Hook;

            On.EntityStates.Wisp1Monster.ChargeEmbers.Update += ChargeEmbers_Update;
            On.EntityStates.Wisp1Monster.ChargeEmbers.FixedUpdate += ChargeEmbers_FixedUpdate;
            On.EntityStates.GreaterWispMonster.ChargeCannons.FixedUpdate += ChargeCannons_FixedUpdate;

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

        private static Vector3 GenerateTimeStopFrozenVelocity(Rigidbody rb)
        {
            bool flag = false;
            AntiGravityForce component = rb.GetComponent<AntiGravityForce>();
            if (component != null)
            {
                flag = (component.antiGravityCoefficient == 1f);
            }
            if (!rb.useGravity || flag)
            {
                return Vector3.zero;
            }
            string stage = SceneCatalog.GetSceneDefForCurrentScene()?.baseSceneName;
            return new Vector3(0f, 0.5f - (stage == "moon2"? 0.16f : 0f), 0f);
        }

        private static void ProjectileSimple_FixedUpdate_Hook(On.RoR2.Projectile.ProjectileSimple.orig_FixedUpdate orig, ProjectileSimple self)
        {
            if (StopTime.TIMESTOP_ACTIVE)
            {
                Rigidbody component = self.GetComponent<Rigidbody>();
                if (component != null)
                {
                    component.velocity = StopTime.GenerateTimeStopFrozenVelocity(component);
                    component.angularVelocity = Vector3.zero;
                    return;
                }
            }
            else
            {
                orig.Invoke(self);
            }
        }

        private static void OrbManager_FixedUpdate_Hook(On.RoR2.Orbs.OrbManager.orig_FixedUpdate orig, OrbManager self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void OrbEffect_UpdateOrb_Hook(On.RoR2.Orbs.OrbEffect.orig_UpdateOrb orig, OrbEffect self, float deltaTime)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self, deltaTime);
            }
        }

        private static void ProjectileImpactExplosion_FixedUpdate_Hook(On.RoR2.Projectile.ProjectileImpactExplosion.orig_FixedUpdate orig, ProjectileImpactExplosion self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void RigidbodyMotor_FixedUpdate_Hook(On.RoR2.RigidbodyMotor.orig_FixedUpdate orig, RigidbodyMotor self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
                return;
            }
            self.rigid.velocity = StopTime.GenerateTimeStopFrozenVelocity(self.rigid);
        }


        //wisp
        private static void ChargeEmbers_FixedUpdate(On.EntityStates.Wisp1Monster.ChargeEmbers.orig_FixedUpdate orig, ChargeEmbers self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void ChargeEmbers_Update(On.EntityStates.Wisp1Monster.ChargeEmbers.orig_Update orig, ChargeEmbers self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }
        private static void ChargeCannons_FixedUpdate(On.EntityStates.GreaterWispMonster.ChargeCannons.orig_FixedUpdate orig, ChargeCannons self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }


        private static void ChargeLaser_FixedUpdate(On.EntityStates.GolemMonster.ChargeLaser.orig_FixedUpdate orig, ChargeLaser self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void ChargeLaser_Update(On.EntityStates.GolemMonster.ChargeLaser.orig_Update orig, ChargeLaser self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void ChargeMegaLaser_FixedUpdate_Hook(On.EntityStates.TitanMonster.ChargeMegaLaser.orig_FixedUpdate orig, ChargeMegaLaser self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void ChargeMegaLaser_Update_Hook(On.EntityStates.TitanMonster.ChargeMegaLaser.orig_Update orig, ChargeMegaLaser self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void FireMegaLaser_FixedUpdate_Hook(On.EntityStates.TitanMonster.FireMegaLaser.orig_FixedUpdate orig, FireMegaLaser self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void ChargeGoldMegaLaser_FixedUpdate_Hook(On.EntityStates.TitanMonster.ChargeGoldMegaLaser.orig_FixedUpdate orig, ChargeGoldMegaLaser self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void FireGoldMegaLaser_FixedUpdate_Hook(On.EntityStates.TitanMonster.FireGoldMegaLaser.orig_FixedUpdate orig, FireGoldMegaLaser self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void WormBodyPositions2_OnDeathStart_Hook(On.RoR2.WormBodyPositions2.orig_OnDeathStart orig, WormBodyPositions2 self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void WormBodyPositionsDriver_FixedUpdateServer_Hook(On.RoR2.WormBodyPositionsDriver.orig_FixedUpdateServer orig, WormBodyPositionsDriver self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
                return;
            }
            self.chaserVelocity = Vector3.zero;
        }

        private static void DelayBlast_FixedUpdate_Hook(On.RoR2.DelayBlast.orig_FixedUpdate orig, DelayBlast self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void DestroyOnTimer_FixedUpdate_Hook(On.RoR2.DestroyOnTimer.orig_FixedUpdate orig, DestroyOnTimer self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }

        private static void MinigunFire_FixedUpdate_Hook(On.EntityStates.ClayBruiser.Weapon.MinigunFire.orig_FixedUpdate orig, MinigunFire self)
        {
            if (!StopTime.TIMESTOP_ACTIVE)
            {
                orig.Invoke(self);
            }
        }
        #endregion



        public static void TimeStopFreezeAllEnemies()
        {
            if (StopTime.characterBodyUsed != null)
            {
                StopTime.TimeStopEnemies(Physics.OverlapSphere(StopTime.characterBodyUsed.transform.position, float.MaxValue, LayerIndex.entityPrecise.mask).ToArray<Collider>(),
                  StopTime.characterBodyUsed, StopTime.TIMESTOP_DURATION);
            }
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