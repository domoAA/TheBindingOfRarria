using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class PhoenixKunai : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 18;
        Item.height = 30;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 0, 20);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<KunaiPlayer>().kunai = Item;

}

public class KunaiPlayer : ModPlayer
{
    public Item kunai = null;
    private int counter = -1;

    public override void ResetEffects() => kunai = null;
    
    public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (kunai != null)
        {
            counter = counter < 2 ? counter + 1 : 0;
            if (counter == 0)
            {
                var pos = Player.Center + new Vector2(30, 30).RotatedByRandom(TwoPi);

                Projectile.NewProjectile(Player.GetSource_Accessory(kunai), pos, (pos - Main.screenPosition).DirectionTo(new Vector2(Main.mouseX, Main.mouseY)) * 14, ModContent.ProjectileType<FlyingKunai>(), damage / 2, 1, Player.whoAmI);
            }
        }
        
        return base.Shoot(item, source, position, velocity, type, damage, knockback);
    }
}

public class KingSlimeBagLoot : GlobalItem
{
    public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
    {
        if (item.type == ItemID.KingSlimeBossBag)
        {
            IItemDropRule rule = ItemDropRule.Common(ModContent.ItemType<PhoenixKunai>(), 6);
            itemLoot.Add(rule);
        }
    }
}

public class KunaiLootNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.KingSlime)
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhoenixKunai>(), 6));
        base.ModifyNPCLoot(npc, npcLoot);
    }
}