using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using R2API;

namespace BombardierMod.Characters.Survivors.Bombardier.Content
{
    internal static class BombardierDamageTypes
    {
        public static DamageAPI.ModdedDamageType VaporizerPrimed;

        internal static void Init()
        {
            VaporizerPrimed = DamageAPI.ReserveDamageType();
        }
    }
}