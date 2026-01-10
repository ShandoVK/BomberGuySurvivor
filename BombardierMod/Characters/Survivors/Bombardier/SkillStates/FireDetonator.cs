using BombardierMod.Characters.Survivors.Bombardier.Content;
using BombardierMod.Survivors.Bombardier;
using EntityStates;
using R2API;
using RoR2;
using System.Linq;
using UnityEngine;

namespace BombardierMod.Survivors.Bombardier.SkillStates
{
    public class FireDetonator : BaseSkillState
    {
        // todo: Revise the scaling for these super well cause it'll get silly damage-wise without proper scaling
        public static float baseDamageCoefficient = 2.5f;
        public static float damagePerStack = 1.5f;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        public static float firePercentTime = 0.0f;
        public static float force = 1000f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerCommandoBoost");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;
        public float chargeProgress = 1f;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime)
            {
                Fire();
            }

            if (fixedAge >= duration && isAuthority)
            {
                outer.SetNextStateToMain();
                return;
            }
        }

        private void Fire()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(2f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("BombardierShootDetonator", gameObject);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = baseDamageCoefficient * damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = range,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = RollCrit(),
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        procChainMask = default,
                        procCoefficient = procCoefficient,
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = tracerEffectPrefab,
                        spreadPitchScale = 1f,
                        spreadYawScale = 1f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                        hitCallback = (BulletAttack bulletAttack, ref BulletAttack.BulletHit hitInfo) =>
                        {
                            bool result = BulletAttack.defaultHitCallback(bulletAttack, ref hitInfo);
                            if (hitInfo.hitHurtBox)
                            {
                                DetonatePrimedStacks(hitInfo.hitHurtBox, bulletAttack);
                            }

                            return result;
                        }
                    }.Fire();
                }
            }
        }

        // This lets me do the big booms with the gun
        private void DetonatePrimedStacks(HurtBox hitHurtBox, BulletAttack bulletAttack)
        {
            if (!hitHurtBox.healthComponent || !hitHurtBox.healthComponent.body)
            {
                return;
            }

            CharacterBody victimBody = hitHurtBox.healthComponent.body;
            int primedStacks = victimBody.GetBuffCount(BombardierBuffs.vaporizerPrimed);
            if (primedStacks > 0)
            {
                while (victimBody.HasBuff(BombardierBuffs.vaporizerPrimed))
                {
                    victimBody.RemoveBuff(BombardierBuffs.vaporizerPrimed);
                }

                // todo: fine tune this to avoid it being busted as fuck, seriously
                float explosionDamage = damageStat * baseDamageCoefficient * (1f + (primedStacks * damagePerStack));
                float explosionRadius = 8f + (primedStacks * 1f);

                BlastAttack blastAttack = new BlastAttack
                {
                    attacker = gameObject,
                    baseDamage = explosionDamage,
                    baseForce = 2000f,
                    bonusForce = Vector3.up * 1000f,
                    crit = RollCrit(),
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = gameObject,
                    position = hitHurtBox.transform.position,
                    procChainMask = default,
                    procCoefficient = 1f,
                    radius = explosionRadius,
                    teamIndex = GetTeam(),
                };

                BlastAttack.Result result = blastAttack.Fire();
                foreach (BlastAttack.HitPoint hitPoint in result.hitPoints)
                {
                    if (hitPoint.hurtBox && hitPoint.hurtBox.healthComponent && hitPoint.hurtBox.healthComponent.body)
                    {
                        CharacterBody hitBody = hitPoint.hurtBox.healthComponent.body;
                        int chainPrimedStacks = hitBody.GetBuffCount(BombardierBuffs.vaporizerPrimed);

                        if (chainPrimedStacks > 0)
                        {
                            while (hitBody.HasBuff(BombardierBuffs.vaporizerPrimed))
                            {
                                hitBody.RemoveBuff(BombardierBuffs.vaporizerPrimed);
                            }

                            float chainExplosionDamage = damageStat * baseDamageCoefficient * (1f + (chainPrimedStacks * damagePerStack));
                            float chainExplosionRadius = 8f + (chainPrimedStacks * 1f);

                            new BlastAttack
                            {
                                attacker = gameObject,
                                baseDamage = chainExplosionDamage,
                                baseForce = 2000f,
                                bonusForce = Vector3.up * 1000f,
                                crit = RollCrit(),
                                damageType = DamageType.Generic,
                                falloffModel = BlastAttack.FalloffModel.None,
                                inflictor = gameObject,
                                position = hitPoint.hurtBox.transform.position,
                                procChainMask = default,
                                procCoefficient = 0.5f,
                                radius = chainExplosionRadius,
                                teamIndex = GetTeam(),
                            }.Fire();

                            EffectManager.SpawnEffect(
                                LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFX"),
                                new EffectData
                                {
                                    origin = hitPoint.hurtBox.transform.position,
                                    scale = chainExplosionRadius,
                                    rotation = Quaternion.identity
                                },
                                true);

                            Util.PlaySound("Play_item_proc_missile_explo", hitBody.gameObject);
                        }
                    }
                }

                EffectManager.SpawnEffect(
                    LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/OmniEffect/OmniExplosionVFX"),
                    new EffectData
                    {
                        origin = hitHurtBox.transform.position,
                        scale = explosionRadius,
                        rotation = Quaternion.identity
                    },
                    true);

                Util.PlaySound("Play_item_proc_missile_explo", hitHurtBox.healthComponent.gameObject);

                //Log.Info($"Detonated {victimBody.GetDisplayName()} for {explosionDamage} damage!");
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}
