using System.Diagnostics.Metrics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;
using TheBindingOfRarria.Content.Dusts;

namespace TheBindingOfRarria.Content.Items;

public class HornOfTheRoundDeer : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 26;
        Item.height = 32;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 2);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        

        if (player.GetModPlayer<ReverseCTPLayer>().counter <= 0 && player.statLife <= player.statLifeMax2 / 3)
        {
            player.AddBuff(ModContent.BuffType<ReverseCT>(), 100);
            player.GetModPlayer<ReverseCTPLayer>().counter = 1800;
        }

        player.GetModPlayer<ReverseCTPLayer>().counter--;
    }
}

public class ReverseCTPLayer : ModPlayer
{
    public bool Talented = false;

    public int counter = 0;

    public override void ResetEffects()
    {
        if ((!Talented || counter <= 0) && Player.HasBuff(ModContent.BuffType<ReverseCT_CD>()))
            Player.ClearBuff(ModContent.BuffType<ReverseCT_CD>());

        Talented = false;
    }
    public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
    {
        if (Player.HasBuff(ModContent.BuffType<ReverseCT>()))
            Dust.NewDustPerfect(Player.position + new Vector2(Main.rand.Next(Player.width), Main.rand.Next((int)(Player.height * 0.7f))), ModContent.DustType<GlorpyHealingPlus>(), new Vector2(0, -1.25f), 0, Color.MediumSpringGreen with { A = 150 }, 1.2f);

    }
}
