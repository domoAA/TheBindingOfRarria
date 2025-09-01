using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class SlasherHalberd : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.height = 30;
        Item.width = 26;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 0, 89);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<SlasherItemPlayer>().Halberd = Item;
}

public class HalberdItemNPCShop : GlobalNPC
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
    private bool Slashed = false;

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
            float distance = 400f * 400;
            Vector2 position = Vector2.Zero;

            foreach (NPC n in Main.ActiveNPCs)
            {
                if (target.Center.DistanceSQ(Player.Center) < distance && !target.friendly && target.lifeMax > 5 && target.CanBeChasedBy() && !target.immortal && !target.CountsAsACritter && n.whoAmI != target.whoAmI && !n.immortal)
                {
                    distance = n.Center.DistanceSQ(Player.Center);
                    position = n.Center;
                }
            }

            if (position != Vector2.Zero)
            {
                Vector2 vel = new Vector2(11f, 11f).RotatedByRandom(TwoPi);
                Projectile.NewProjectile(Player.GetSource_Accessory(Halberd, "SlasherHalberd"), position - vel * 9, vel, ProjectileID.Muramasa, 30, 1, Player.whoAmI, Main.rand.NextFloat() - 0.5f, 0, 0);
            }

            Slashed = true;
        }
    }
}