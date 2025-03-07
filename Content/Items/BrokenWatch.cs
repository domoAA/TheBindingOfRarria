 

 

using Terraria.GameInput;
 
 
 
 
 

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
            CreateRecipe()
                .AddIngredient(ItemID.FastClock)
                .AddIngredient(ItemID.StoneBlock)
                .AddTile(TileID.MythrilAnvil)
                .Register();

            base.AddRecipes();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var key = KeybindSystem.ZaWardoKey.GetAssignedKeys().FirstOrDefault();
            if (key == "" || key == null)
                key = "P";

            var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.BrokenWatch.Tooltip"), key);
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
        public override void ResetEffects() => ZaWardo = false;
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if ((KeybindSystem.ZaWardoKey.JustPressed || (KeybindSystem.ZaWardoKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.P))) && Main.myPlayer == Player.whoAmI && counter <= 0 && ZaWardo)
            {
                var rand = (TheBindingOfRarria.State)Main.rand.Next(1, 3);
                foreach (var target in Main.ActiveNPCs)
                {
                    target?.GetSlowed(rand, 180);
                }
                foreach (var proj in Main.ActiveProjectiles)
                {
                    proj?.GetSlowed(rand, 180);
                }

                counter = 600;
                SoundEngine.PlaySound(SoundID.Shatter, Player.position);
            }

            base.ProcessTriggers(triggersSet);
        }
    }
}