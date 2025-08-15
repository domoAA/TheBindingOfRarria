using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;

namespace TheBindingOfRarria.Content.Items;

public class Sihil : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 22;
        Item.height = 30;
        Item.accessory = true;
        Item.value = Item.sellPrice(0, 8);
        Item.rare = ItemRarityID.Orange;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<SwearSworPlayer>().SwearSwor = true;
}

public class SwearSworPlayer : ModPlayer
{
    public bool SwearSwor = false;

    public override void ResetEffects() => SwearSwor = false;

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Main.myPlayer != Player.whoAmI)
            return;


        var buff = SihilBuffs.PotionBuffs[Main.rand.Next(SihilBuffs.PotionBuffs.Count)];

        if (SwearSwor && !target.active)
            Player.AddBuff(buff, 660);
    }
}

public class SihilBuffs : ModSystem
{
    public static List<int> PotionBuffs = [];

    public override void PostSetupContent()
    {
        foreach (var i in ContentSamples.ItemsByType)
        {
            if (i.Value.consumable && i.Value.useStyle == ItemUseStyleID.DrinkLiquid && i.Value.buffType != 0 && !Main.debuff[i.Value.buffType] && !BuffID.Sets.IsWellFed[i.Value.buffType] && !BuffID.Sets.IsAFlaskBuff[i.Value.buffType])
                PotionBuffs.Add(i.Value.buffType);
        }
    }

    public override void Unload()
    {
        PotionBuffs?.Clear();
    }
}