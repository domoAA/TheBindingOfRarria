using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Config;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class BrokenWatch : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 24;
            Item.width = 22;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 5);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ZaWardoPlayer>().counter--;
            player.GetModPlayer<ZaWardoPlayer>().ZaWardo = true;
        }
        public override void AddRecipes()
        {
            Recipe.Create(Item.type)
                .AddIngredient(ItemID.FastClock)
                .AddIngredient(ItemID.StoneBlock)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.BrokenWatch.Tooltip"), KeybindSystem.ZaWardoKey.GetAssignedKeys().First());
            base.ModifyTooltips(tooltips);
            for (int i = 10; i > 0; i--)
            {
                var index = tooltips.FindIndex(line => line.Text.Contains(Language.GetTextValue("Mods.TheBindingOfRarria.Items.BrokenWatch.Tooltip").Remove(7)));
                if (index != -1)
                {
                    tooltips[index].Text = text.Remove(text.IndexOf($"\n"));
                    break;
                }
            }
        }
    }
    public class ZaWardoPlayer : ModPlayer
    {
        public int counter = 0;
        public bool ZaWardo = false;
        public override void ResetEffects()
        {
            ZaWardo = false;
            base.ResetEffects();
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.ZaWardoKey.JustPressed && counter <= 0 && ZaWardo)
            {
                var rand = Main.rand.NextBool();
                foreach (var target in Main.ActiveNPCs)
                {
                    if (target == null || !target.active)
                        continue;

                    if (rand)
                    {
                        target.velocity *= 0.7f;
                        target.GetGlobalNPC<NPCExtensions.SlowedGlobalNPC>().Slowed = (true, 180);
                    }
                    else
                    {
                        target.velocity *= 1.3f;
                        target.GetGlobalNPC<NPCExtensions.SlowedGlobalNPC>().Slowed = (null, 180);
                    }
                }
                foreach (var proj in Main.ActiveProjectiles)
                {
                    if (proj == null)
                        continue;

                    if (rand)
                    {
                        proj.velocity *= 0.7f;
                        proj.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = (true, 180);
                    }
                    else
                    {
                        proj.velocity *= 1.3f;
                        proj.GetGlobalProjectile<SlowedGlobalProjectile>().Slowed = (null, 180);
                    }
                }

                counter = 600;
                SoundEngine.PlaySound(SoundID.Shatter, Player.position);
            }

            base.ProcessTriggers(triggersSet);
        }
    }
}