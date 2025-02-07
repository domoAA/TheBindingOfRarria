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

namespace TheBindingOfRarria.Content.Items
{
    public class ToothAndNail : ModItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !equippedItem.HasTag(TheBindingOfRarria.invulItems) || !incomingItem.HasTag(TheBindingOfRarria.invulItems);
        }
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
            player.GetModPlayer<StonePlayer>().HasAStone = true;
            player.GetModPlayer<StonePlayer>().counter--;
            
        }
    }
    public class StonePlayer : ModPlayer
    {
        public int counter = 0;
        public bool HasAStone = false;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if ((KeybindSystem.StonedKey.JustPressed || (KeybindSystem.StonedKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift))) && Main.myPlayer == Player.whoAmI && counter <= 0 && HasAStone)
            {
                var time = Player.longInvince ? 90 : 50;
                Player.immune = true;
                Player.immuneTime = time;
                Player.immuneAlpha = 150;
                Player.SetImmuneTimeForAllTypes(time);
                counter = 1800 + time;
                if (Main.myPlayer == Player.whoAmI)
                    SoundEngine.PlaySound(SoundID.Item69);
            }

            base.ProcessTriggers(triggersSet);
        }
        public override void ResetEffects()
        {
            HasAStone = false;
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            var info = new NPC().CalculateHitInfo(hurtInfo.SourceDamage, -hurtInfo.HitDirection);
            if (HasAStone) {
                Player.AddBuff(BuffID.Stoned, 10);
                npc.StrikeNPC(info);
                NetMessage.SendStrikeNPC(npc, info);
                    }
            base.OnHitByNPC(npc, hurtInfo);
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
            if (counter > 1800)
            {
                Player.immuneAlpha = 255;
                Player.immuneNoBlink = false;
                //a = 0;
                //drawInfo.hideEntirePlayer = true;
                var texture = TheBindingOfRarria.Boulder.Value;
                var color = Color.Gray;
                color.A = (byte)(counter % 1800);
                texture.DrawPixellated((Player.Center - Main.screenPosition) / 2, 4, 0, color, PixellationSystem.RenderType.Additive);
            }
        }
    }
}