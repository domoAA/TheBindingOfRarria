using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Buffs;

public class ReverseCT : ModBuff
{
    public override string Texture => ContentPath + "Buffs/" + Name;

    public override void Update(Player player, ref int buffIndex) => player.lifeRegenCount += player.statLifeMax2 / 5;

    public override bool ReApply(Player player, int time, int buffIndex) => true;
}

public class ReverseCTPlayer : ModPlayer
{
    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (Player.HasBuff(ModContent.BuffType<ReverseCT>()))
        {
            Dust.NewDustPerfect(Player.position + new Vector2(Main.rand.Next(Player.width), Main.rand.Next((int)(Player.height * 0.7f))), ModContent.DustType<GlorpyHealingPlus>(), new Vector2(0, -1.25f), 0, Color.MediumSpringGreen with { A = 150 }, 1.2f);

        }
    }
}