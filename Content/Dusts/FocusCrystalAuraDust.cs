using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Content.Dusts;

public class FocusCrystalAuraDust : ModDust
{
        // Bad idea.
    public override string Texture => null;

    public override void OnSpawn(Dust dust)
    {
        dust.frame = Helper.FrameVanillaDust(DustID.RedTorch);
        dust.noGravity = true;
    }

    public override bool Update(Dust dust)
    {
        dust.rotation += 0.1f;
        dust.scale -= 0.03f;
        dust.position += dust.velocity;

        if (dust.customData is Player player)
            dust.position += player.position - player.oldPosition;

        if (dust.scale < 0.25f)
            dust.active = false;

        return false;
    }
}