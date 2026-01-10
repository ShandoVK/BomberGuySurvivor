using BombardierMod.Characters.Survivors.Bombardier.Content;
using BombardierMod.Characters.Survivors.Bombardier.SkillStates;
using BombardierMod.Survivors.Bombardier;
using EntityStates;
using R2API;
using RoR2;
using UnityEngine;

/* Primary 1: Vaporizer
 * Player shoots a "VaporizerOrb" projectile that sticks to the enemy hit by it, inflicting the "Primed" effect.
 * When the player charges the attack, they instead shoot a "VaporizerDetonator" projectile. If the enemy hit by
 * the detonator projectile, all stacks of "Primed" are removed and the enemy is hit by an explosion whose damage
 * is scaled based on the number of "Primed" stacks it has. If the enemy is hit by the explosion and also has
 * primed stacks on them, they will also explode accordingly.
 */

namespace BombardierMod.Survivors.Bombardier.SkillStates
{
    public class Shoot : BaseSkillState
    {
        public static float damageCoefficient = BombardierStaticValues.gunDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        // Only edit fire delay if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 800f;
        public static float recoil = 1f;
        public static float range = 256f;
        public static float chargeTransitionTime = 0.2f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

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

            if (fixedAge >= fireTime && !hasFired)
            {
                Fire();
            }

            if (hasFired && isAuthority && inputBank && inputBank.skill1.down)
            {
                outer.SetNextState(new ShootCharged());
                return;
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

                characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("BombardierShootPistol", gameObject);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = damageCoefficient * damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageTypeCombo.GenericPrimary,
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
                        modifyOutgoingDamageCallback = (BulletAttack bulletAttack, ref BulletAttack.BulletHit hitInfo, DamageInfo damageInfo) =>
                        {
                            if (damageInfo != null)
                            {
                                DamageAPI.AddModdedDamageType(damageInfo, BombardierDamageTypes.VaporizerPrimed);
                            }
                        }
                    }.Fire();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}