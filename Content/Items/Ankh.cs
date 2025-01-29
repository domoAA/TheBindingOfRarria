using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Buffs;

namespace TheBindingOfRarria.Content.Items
{
    public class Ankh : ModItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if ((equippedItem.type == ModContent.ItemType<BrokenAnkh>() || equippedItem.type == ModContent.ItemType<Ankh>()) && (incomingItem.type == ModContent.ItemType<BrokenAnkh>() || incomingItem.type == ModContent.ItemType<Ankh>()))
                return false;
            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightPurple;
            Item.value = Item.buyPrice(0, 3);
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.AnkhCharm)
                .AddIngredient(ItemID.AnkhBanner)
                .AddIngredient(ItemID.LifeCrystal)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();

            base.AddRecipes();
        }
    }
    public class RevivePlayer : ModPlayer
    {
        public override void PostUpdateBuffs()
        {
            var index = Array.FindIndex(Player.armor, item => !item.social && (item.type == ModContent.ItemType<Ankh>() || item.type == ModContent.ItemType<BrokenAnkh>()));
            if (index != -1)
            {
                if (!Player.HasBuff(ModContent.BuffType<AnubisCurse>()))
                    Player.armor[index] = new Item(ModContent.ItemType<Ankh>());
                else
                    Player.armor[index] = new Item(ModContent.ItemType<BrokenAnkh>());
            }
        }
    
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
        {
            if (Array.FindIndex(Player.armor, type => type.type == ModContent.ItemType<Ankh>()) != -1)
            {
                Player.AddBuff(ModContent.BuffType<AnubisCurse>(), 3600);
                Player.statLife = (Player.ConsumedLifeCrystals + 5) * 10;
                return false;
            }
            else if (Array.FindIndex(Player.armor, type => type.type == ModContent.ItemType<BrokenAnkh>()) != -1) {
                var rand = new Random();
                if (rand.NextDouble() < 0.2222)
                {
                    Player.AddBuff(ModContent.BuffType<AnubisCurse>(), 3600);
                    Player.statLife = 60;
                    return false;
                }
                else return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
            }
            else
                return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genDust, ref damageSource);
        }
    }
}