using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Common.Systems;

public class DisablePlayerStepUpSystem : ModSystem
{
    public override void Load()
    {
        IL_Player.Update += KainDidntRenameThisMethodLmao;
    }

    public override void Unload()
    {
        IL_Player.Update -= KainDidntRenameThisMethodLmao;
    }

    private void KainDidntRenameThisMethodLmao(ILContext il)
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

        c.EmitDelegate((Player player) => player.GetModPlayer<CrystalDashPlayer>().Dashing || player.GetModPlayer<DarkDashPlayer>().counter > 0);
        c.EmitBrtrue(target);
    }
}