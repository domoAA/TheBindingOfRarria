using MonoMod.Cil;

namespace TheBindingOfRarria.Common
{
    public class NoClipSystem : ModSystem
    {
        public override void Load()
        {
            IL_Player.Update += StopTheFloorBecauseDanteSkyblockCanceledYouToo;

        }

        public override void Unload()
        {
            IL_Player.Update -= StopTheFloorBecauseDanteSkyblockCanceledYouToo;

        }

        private void StopTheFloorBecauseDanteSkyblockCanceledYouToo(ILContext il)
        {
            ILCursor c = new(il);

            ILLabel target = c.DefineLabel();

            c.GotoNext(MoveType.After,
                i => i.MatchLdarg0(),
                i => i.MatchCall<Player>("CheckCrackedBrickBreak"),
                i => i.MatchLdarg0(),
                i => i.MatchLdfld<Player>("shimmering"),
                i => i.MatchBrtrue(out target));

            c.EmitLdarg0();

            c.EmitDelegate((Player player) => player.GetModPlayer<CrystalDashPlayer>().Dashing);
            c.EmitBrtrue(target);
        }
    }
}