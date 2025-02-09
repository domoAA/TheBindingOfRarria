using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameInput;
using TheBindingOfRarria.Common.Config;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.Localization;
using System.Linq;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class ToothAndNail : ModItem
    {
        //public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        //{
        //    return !equippedItem.HasTag(TheBindingOfRarria.invulItems) || !incomingItem.HasTag(TheBindingOfRarria.invulItems);
        //}
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 24;
            Item.accessory = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var key = KeybindSystem.StonedKey.GetAssignedKeys().FirstOrDefault();
            if (key == "" || key == null)
                key = "LeftShift";

            var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.ToothAndNail.Tooltip"), key);
            base.ModifyTooltips(tooltips);
            for (int i = 10; i > 0; i--)
            {
                var index = tooltips.FindIndex(line => line.Text.Contains(Language.GetTextValue("Mods.TheBindingOfRarria.Items.ToothAndNail.Tooltip").Remove(7)));
                if (index != -1)
                {
                    tooltips[index].Text = text.Remove(text.IndexOf($"\n"));
                    break;
                }
            }
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StonePlayer>().Stone = Item;
            player.GetModPlayer<StonePlayer>().counter--;
            
        }
    }
    public class StonePlayer : ModPlayer
    {
        public int counter = 0;
        public Item Stone = null;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if ((KeybindSystem.StonedKey.JustPressed || (KeybindSystem.StonedKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))) && Main.myPlayer == Player.whoAmI && counter <= 0 && Stone != null)
            {
                var time = Player.longInvince ? 90 : 50;
                Player.immune = true;
                Player.immuneTime = time;
                Player.immuneAlpha = 150;
                Player.SetImmuneTimeForAllTypes(time);
                counter = 180 + time;
                Projectile.NewProjectile(Player.GetSource_Accessory(Stone), Player.Center, Vector2.Zero, ModContent.ProjectileType<BlockBoulder>(), 0, 0, Player.whoAmI, time * 3 + 100);
                if (Main.myPlayer == Player.whoAmI)
                    SoundEngine.PlaySound(SoundID.Item69);
            }

            base.ProcessTriggers(triggersSet);
        }
        public override void ResetEffects()
        {
            Stone = null;
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            var info = new NPC().CalculateHitInfo(hurtInfo.SourceDamage, -hurtInfo.HitDirection);
            if (Stone != null) {
                Player.AddBuff(BuffID.Stoned, 10);
                npc.StrikeNPC(info);
                NetMessage.SendStrikeNPC(npc, info);
                    }
            base.OnHitByNPC(npc, hurtInfo);
        }
    }
}