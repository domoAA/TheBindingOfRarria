using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class CurveSickle : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.width = 50;
        Item.height = 50;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(gold: 1);

        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.consumable = true;
        Item.maxStack = 8;

        Item.shoot = ModContent.ProjectileType<CurveSickleProj>();
        Item.shootSpeed = 13f;
        Item.damage = 40;
        Item.knockBack = 8f;
        Item.DamageType = DamageClass.MeleeNoSpeed;
    }
}

public class CurveSickleItemNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.BestiaryGirl)
        {
            shop.Add(new Item(ModContent.ItemType<CurveSickle>()), Condition.DownedSkeletron);
        }
    }
}