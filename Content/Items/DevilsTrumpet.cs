using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class DevilsTrumpet : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 32;
            Item.height = 30;
            Item.value = Item.buyPrice(0, 1, 30);
            Item.rare = ItemRarityID.LightRed;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MasterMirrorPlayer>().IsEvilIncarnate = true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.JungleRose)
                .AddIngredient(ItemID.SpiderFang, 10)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
    }
    public class MasterMirrorPlayer : ModPlayer
    {
        public bool IsEvilIncarnate = false;
        public override void ResetEffects()
        {
            IsEvilIncarnate = false;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPC(target, hit, damageDone);
            if (IsEvilIncarnate && target.HasBuff(BuffID.Poisoned))
            {
                var index = target.FindBuffIndex(BuffID.Poisoned);
                var time = target.buffTime[index];
                target.DelBuff(index);
                target.AddBuff(BuffID.Venom, time);
            }
        }
    }
    public class CrateLootDevilsTrumpet : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.JungleFishingCrateHard)
            {
                var rule = ItemDropRule.Common(ModContent.ItemType<DevilsTrumpet>(), 6);
                itemLoot.Add(rule);
            }
        }
    }
}