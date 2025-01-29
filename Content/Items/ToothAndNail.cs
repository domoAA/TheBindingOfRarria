using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameInput;
using TheBindingOfRarria.Common.Config;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.Localization;
using System.Linq;

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
            var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.ToothAndNail.Tooltip"), KeybindSystem.StonedKey.GetAssignedKeys().First());
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
            if (KeybindSystem.StonedKey.JustPressed && counter < 0 && HasAStone)
            {
                var time = Player.longInvince ? 70 : 35;
                Player.immune = true;
                Player.immuneTime = time;
                Player.immuneAlpha = 150;
                Player.SetImmuneTimeForAllTypes(time);
                counter = 1800;
                if (Main.myPlayer == Player.whoAmI)
                    SoundEngine.PlaySound(SoundID.Shatter);
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
    }
}