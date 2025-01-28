using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameInput;
using TheBindingOfRarria.Common.Config;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.Localization;

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
            base.ModifyTooltips(tooltips);
            for (int i = 10; i > 0; i--)
            {
                var index = tooltips.FindIndex(line => line.Text.Contains(Language.GetTextValue("Mods.TheBindingOfRarria.Items.AnemoiBracelet.Tooltip").Remove(7)));
                if (index != -1)
                {
                    tooltips[index].Text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.AnemoiBracelet.Tooltip"), KeybindSystem.StonedKey.GetAssignedKeys().ToString());
                    break;
                }
            }
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<StonePlayer>().HasAStone = true;
            player.GetModPlayer<StonePlayer>().StoneBlink--;

            if (player.GetModPlayer<StonePlayer>().StoneBlink <= -10)
            {
                player.GetModPlayer<StonePlayer>().StoneBlink = 300;
                if (!player.GetModPlayer<StonePlayer>().Stoned)
                {
                    player.buffImmune[BuffID.Stoned] = false;
                    player.AddBuff(BuffID.Stoned, 30);
                }
                else
                    player.GetModPlayer<StonePlayer>().Stoned = false;
            }
            else if (player.GetModPlayer<StonePlayer>().StoneBlink == 0)
            {
                if (player.whoAmI == Main.myPlayer)
                    SoundEngine.PlaySound(SoundID.Duck);
            }
        }
    }
    public class StonePlayer : ModPlayer
    {
        public int StoneBlink = 300;
        public bool HasAStone = false;
        public bool Stoned = false;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.StonedKey.JustPressed && StoneBlink <= 0 && HasAStone)
            {
                Stoned = true;
                Player.immune = true;
                var time = Player.longInvince ? 120 : 60;
                Player.SetImmuneTimeForAllTypes(time);
            }

            base.ProcessTriggers(triggersSet);
        }
        public override void ResetEffects()
        {
            if (!HasAStone)
                StoneBlink = 300 + Main.rand.Next(300);

            HasAStone = false;
        }
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            var info = new NPC().CalculateHitInfo(hurtInfo.SourceDamage / 10, -hurtInfo.HitDirection);
            if (HasAStone) {
                npc.StrikeNPC(info);
                NetMessage.SendStrikeNPC(npc, info);
                    }
            base.OnHitByNPC(npc, hurtInfo);
        }
    }
}