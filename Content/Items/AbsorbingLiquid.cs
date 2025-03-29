


namespace TheBindingOfRarria.Content.Items
{
    public class AbsorbingLiquid : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 22;
            Item.height = 30;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(0, 0, 8, 7);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AbsorbingPlayer>().ModLiquid = Item;
            player.GetModPlayer<AbsorbingPlayer>().counter.timer--;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Bottle)
                .AddIngredient(ItemID.HallowedBar)
                .AddTile(TileID.ImbuingStation)
                .AddCondition(Condition.NearShimmer)
                .Register();

            base.AddRecipes();
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var key = KeybindSystem.AbsorbingKey.GetAssignedKeys().FirstOrDefault();
            if (key == "" || key == null)
                key = "O";

            var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.AbsorbingLiquid.Tooltip"), key);


            var index = tooltips.FindIndex(line => line.Name == "Tooltip0");
            if (index != -1)
            {
                text = text.Remove(text.LastIndexOf($"\n"));
                text = text.Remove(text.LastIndexOf($"\n"));
                tooltips[index].Text = text;
            }
        }
    }
    public class AbsorbingPlayer : ModPlayer
    {
        public Item ModLiquid = null;
        public (int timer, int heal) counter = (0, 0);
        public override void ResetEffects()
        {
            if (ModLiquid == null)
                counter.heal = 0;

            if (counter.timer <= 0)
                counter.heal = 0;

            ModLiquid = null;
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (counter.heal > 0 && counter.timer > 0 && counter.timer < 30 && ModLiquid != null && (KeybindSystem.AbsorbingKey.JustPressed || (KeybindSystem.AbsorbingKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.O))) && Main.myPlayer == Player.whoAmI)
            {
                Projectile.NewProjectile(Player.GetSource_Accessory(ModLiquid, "AbsorbingLiquid activation"), Player.Center, Vector2.Zero, ModContent.ProjectileType<AbsorbingLiquidEffect>(), 0, 0, Player.whoAmI, counter.heal);
                counter = (1200, 0);
            }
        }
        public override void OnHurt(Player.HurtInfo info) => Absorb(info);
        public void Absorb(Player.HurtInfo info)
        {
            if (counter.timer <= 0)
                counter = (20, info.Damage / 2);
        }
    }
}