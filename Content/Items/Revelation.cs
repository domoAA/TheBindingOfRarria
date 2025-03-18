using Terraria.GameInput;

namespace TheBindingOfRarria.Content.Items
{
    public class Revelation : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 28;
            Item.height = 28;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<RevelationPlayer>().revelation = Item;
            player.GetModPlayer<RevelationPlayer>().counter--;
        }
    }
    public class RevelationPlayer : ModPlayer
    {
        public Item revelation = null;
        public int counter = 0;
        public override void ResetEffects() => revelation = null;
        
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if ((KeybindSystem.LightBeamKey.JustPressed || (KeybindSystem.LightBeamKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.L))) && Main.myPlayer == Player.whoAmI && counter <= 0 && revelation != null)
            {
                counter = 60;
                Projectile.NewProjectile(Player.GetSource_Accessory(revelation), Player.Center, (Player.Center - Main.screenPosition).DirectionTo(new(Main.mouseX, Main.mouseY)) * 500, ModContent.ProjectileType<LightBeam>(), 28, 2, Player.whoAmI);
            }
        }
    }
}