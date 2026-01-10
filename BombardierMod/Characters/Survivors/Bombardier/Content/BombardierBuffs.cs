using RoR2;
using UnityEngine;

namespace BombardierMod.Survivors.Bombardier
{
    public static class BombardierBuffs
    {
        public static BuffDef armorBuff; // armor buff gained during roll
        public static BuffDef vaporizerPrimed; // Used for Vaporizer (Default Primary)
        public static BuffDef mistVariable; // Used for Chaotic Convergence (Alt Special)

        public static void Init(AssetBundle assetBundle)
        {
            // Remove this when the proper utility is built out
            armorBuff = Modules.Content.CreateAndAddBuff("BombardierArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            // todo: add a custom icon for the buff instead of an orange ruin icon
            vaporizerPrimed = Modules.Content.CreateAndAddBuff("VaporizerPrimed",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/LunarDetonationCharge").iconSprite,
                new Color(1f, 0.5f, 0f),
                true,
                false);

        }
    }
}
