using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class SlasherHalberd : ModItem
{
    public override void SetDefaults()
    {
        Item.height = 30;
        Item.width = 26;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 0, 89);
    }
    public int counter = 0;
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<SlasherItemPlayer>().Halberd = Item;
    }
}
class HalberdItemNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Merchant)
        {
            if (shop.TryGetEntry(ItemID.IronAnvil, out var entry))
                shop.InsertAfter(entry, new Item(ModContent.ItemType<SlasherHalberd>()), Condition.Hardmode);

            else if (shop.TryGetEntry(ItemID.LeadAnvil, out entry))
                shop.InsertAfter(entry, new Item(ModContent.ItemType<SlasherHalberd>()), Condition.Hardmode);
        }
    }
}
public class SlasherItemPlayer : ModPlayer
{
    public Item Halberd = null;
    public bool Slashed = false;
    public override void ResetEffects()
    {
        if (Slashed && Player.ItemAnimationEndingOrEnded)
            Slashed = false;

        Halberd = null;
    }
    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (Halberd != null && !Slashed && Player.whoAmI == Main.myPlayer && !target.immortal)
        {
            var distance = 400f * 400;
            var pos = Vector2.Zero;
            foreach (var t in Main.ActiveNPCs)
            {
                if (target.Center.DistanceSQ(Player.Center) < distance && !t.friendly && t.whoAmI != target.whoAmI && !t.immortal)
                {
                    distance = t.Center.DistanceSQ(Player.Center);
                    pos = t.Center;
                }
            }
            if (pos != Vector2.Zero)
            {
                var vel = new Vector2(11f, 11f).RotatedByRandom(TwoPi);
                Projectile.NewProjectile(Player.GetSource_Accessory(Halberd, "SlasherHalberd"), pos - vel * 9, vel, ProjectileID.Muramasa, 30, 1, Player.whoAmI, Main.rand.NextFloat() - 0.5f, 0, 0);
            }
            Slashed = true;
        }
    }
}