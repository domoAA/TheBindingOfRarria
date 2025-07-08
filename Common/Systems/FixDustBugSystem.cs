using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Common.Systems;

public class FixDustBugSystem : ModSystem
{
    public override void Load()
    {
        IL_Dust.NewDust += FixStupidBugBecauseTerrariaDevsAreIncompetent;
    }

    public override void Unload()
    {
        IL_Dust.NewDust -= FixStupidBugBecauseTerrariaDevsAreIncompetent;
    }

    private void FixStupidBugBecauseTerrariaDevsAreIncompetent(ILContext il)
    {
        ILCursor c = new(il);

        if (!c.TryGotoNext(MoveType.After,
            i => i.MatchLdloca(1),
            i => i.MatchLdloc2(),
            i => i.MatchCall<Rectangle>("Intersects")))
        {
            Mod.Logger.Error("Could not fix this one stupid fucking dust bug, failed to match to after some weird meaningless garbage check. " +
                "(ps. fuck you (also if this throws EVER I'm killing myself.))");
            return;
        }

        // I would fix this but idc enough to impl viewport size captures.
        c.EmitLdarg3();
        c.EmitDelegate((bool Intersects, int Type) =>
        {
            if (Type == ModContent.DustType<PixellatedDustE98>())
                return true;
            return Intersects;
        });
    }
}