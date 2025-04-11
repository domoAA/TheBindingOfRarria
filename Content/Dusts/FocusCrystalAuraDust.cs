using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Common;

namespace TheBindingOfRarria.Content.Dusts;

public class FocusCrystalAuraDust : ModDust
{
    // Use vanilla texture
    public override string Texture => null;

    public override void OnSpawn(Dust dust)
    {
        dust.frame = GeneralExtensions.FrameVanillaDust(DustID.RedTorch);
        dust.noGravity = true;
    }

    public override bool Update(Dust dust)
    {
        base.Update(dust);

        dust.rotation += 0.1f;
        dust.scale -= 0.03f;
        dust.position += dust.velocity;

        if (dust.customData is Player player)
        {
            dust.position += player.position - player.oldPosition;
        }

        if (dust.scale < 0.25f)
        {
            dust.active = false;
        }

        return false;
    }
}