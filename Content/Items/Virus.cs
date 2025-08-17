using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class Virus : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 20;
        Item.height = 26;
        Item.rare = ItemRarityID.Master;
        Item.value = Item.sellPrice(0, 2, 28);
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => VirusSystem.VirusAmount++;

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.ZombieArm)
            .AddIngredient(ItemID.RottenChunk, 2)
            .AddIngredient(ItemID.Bottle)
            .AddTile(TileID.Bottles)
            .Register();
    }
}

public class VirusSystem : ModSystem
{
    public static int VirusAmount = 0;

    public override void PreUpdateEntities()
    {
        VirusAmount = 0;
    }
}

public class VirusNPC : GlobalNPC
{

    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
    {
        return !entity.friendly;
    }

    public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
    {
        npc.lifeMax = (int)(npc.lifeMax * (1 - 0.05f * VirusSystem.VirusAmount));
    }
}