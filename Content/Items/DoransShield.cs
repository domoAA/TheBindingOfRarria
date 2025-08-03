using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class DoransShield : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 26;
        Item.height = 30;
        Item.rare = ItemRarityID.Master;
        Item.master = true;
        Item.value = Item.buyPrice(0, 4, 50);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<TankStartPlayer>().AgainstAnADC = true;
}

public class TankStartPlayer : ModPlayer
{
    public bool AgainstAnADC = false;

    public override void ResetEffects() => AgainstAnADC = false;

    public override void UpdateLifeRegen()
    {
        if (AgainstAnADC)
            Player.lifeRegen += (Player.statLifeMax2 - Player.statLife) / 50;
    }
}

public partial class DoranItemsNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Merchant)
        {
            PatienceShield(shop);

            PetriCuffs(shop);
        }
    }

    public void PatienceShield(NPCShop shop)
    {
        if (shop.TryGetEntry(ItemID.IronAnvil, out var entry))
            shop.InsertAfter(entry, new Item(ModContent.ItemType<DoransShield>()), Condition.InMasterMode);

        else if (shop.TryGetEntry(ItemID.LeadAnvil, out entry))
            shop.InsertAfter(entry, new Item(ModContent.ItemType<DoransShield>()), Condition.InMasterMode);
    }
}