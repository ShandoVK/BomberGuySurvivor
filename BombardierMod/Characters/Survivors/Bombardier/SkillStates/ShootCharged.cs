using EntityStates;
using BombardierMod.Survivors.Bombardier;
using BombardierMod.Survivors.Bombardier.SkillStates;
using BombardierMod.Characters.Survivors.Bombardier.Content;
using RoR2;
using UnityEngine;
using R2API;

namespace BombardierMod.Characters.Survivors.Bombardier.SkillStates
{
    public class ShootCharged : BaseSkillState
    {
        public static float baseMinChargeDuration = 0.3f;
        public static float baseMaxChargeDuration = 1.0f;
        public static float minChargeForDetonation = 0.5f;

        // thanks for the reference old fart
        public static GameObject chargeupVfxPrefab;
        public static GameObject holdChargeVfxPrefab;
        public static string enterSoundString = "Play_captain_m1_reload";
        public static string playChargeSoundString = "Play_captain_m1_reload";
        public static string stopChargeSoundString = "Stop_captain_m1_reload";

        private float minChargeDuration;
        private float maxChargeDuration;
        private bool released;
        private GameObject chargeupVfxGameObject;
        private GameObject holdChargeVfxGameObject;
        private Transform muzzleTransform;
        private uint enterSoundID;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();

            minChargeDuration = baseMinChargeDuration / attackSpeedStat;
            maxChargeDuration = baseMaxChargeDuration / attackSpeedStat;
            muzzleString = "Muzzle";
            PlayAnimation("LeftArm, Override", "ChargeGun", "ChargeGun.playbackRate", maxChargeDuration);
            muzzleTransform = FindModelChild(muzzleString);
            if (muzzleTransform && chargeupVfxPrefab)
            {
                chargeupVfxGameObject = Object.Instantiate(chargeupVfxPrefab, muzzleTransform);
                ScaleParticleSystemDuration scaleComponent = chargeupVfxGameObject.GetComponent<ScaleParticleSystemDuration>();
                if (scaleComponent)
                {
                    scaleComponent.newDuration = maxChargeDuration;
                }
            }

            enterSoundID = Util.PlayAttackSpeedSound(enterSoundString, gameObject, attackSpeedStat);
            Util.PlaySound(playChargeSoundString, gameObject);
        }

        public override void OnExit()
        {
            if (chargeupVfxGameObject)
            {
                EntityState.Destroy(chargeupVfxGameObject);
                chargeupVfxGameObject = null;
            }
            if (holdChargeVfxGameObject)
            {
                EntityState.Destroy(holdChargeVfxGameObject);
                holdChargeVfxGameObject = null;
            }

            AkSoundEngine.StopPlayingID(enterSoundID);
            Util.PlaySound(stopChargeSoundString, gameObject);

            base.OnExit();
        }

        public override void Update()
        {
            base.Update();
            characterBody.SetSpreadBloom(Mathf.Clamp01(age / maxChargeDuration) * 2f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            characterBody.SetAimTimer(2f);
            float chargeProgress = Mathf.Clamp01(fixedAge / maxChargeDuration);
            if (fixedAge >= maxChargeDuration)
            {
                if (chargeupVfxGameObject)
                {
                    EntityState.Destroy(chargeupVfxGameObject);
                    chargeupVfxGameObject = null;
                }
                if (!holdChargeVfxGameObject && muzzleTransform && holdChargeVfxPrefab)
                {
                    holdChargeVfxGameObject = Object.Instantiate(holdChargeVfxPrefab, muzzleTransform);
                }
            }

            if (isAuthority)
            {
                if (!released && (!inputBank || !inputBank.skill1.down))
                {
                    released = true;
                }

                if (released)
                {
                    if (fixedAge >= minChargeDuration)
                    {
                        outer.SetNextState(new FireDetonator
                        {
                            chargeProgress = chargeProgress
                        });
                    }
                    else
                    {
                        outer.SetNextStateToMain();
                    }
                    return;
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
