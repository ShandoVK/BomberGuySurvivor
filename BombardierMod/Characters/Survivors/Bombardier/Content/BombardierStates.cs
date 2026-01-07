using BombardierMod.Survivors.Bombardier.SkillStates;

namespace BombardierMod.Survivors.Bombardier
{
    public static class BombardierStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));
        }
    }
}
