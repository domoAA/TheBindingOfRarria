using Terraria.GameInput;

namespace TheBindingOfRarria.Content.Items
{
    public class CursedBlood : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 24;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(0, 1, 38);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<KamoPlayer>().Leaky = true;
            player.GetModPlayer<KamoPlayer>().counter--;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var key = KeybindSystem.BloodDripKey.GetAssignedKeys().FirstOrDefault();
            if (key == "" || key == null)
                key = "K";

            var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.CursedBlood.Tooltip"), key);


            var index = tooltips.FindIndex(line => line.Name == "Tooltip0");
            if (index != -1)
            {
                text = text.Remove(text.LastIndexOf($"\n"));
                text = text.Remove(text.LastIndexOf($"\n"));
                text = text.Remove(text.LastIndexOf($"\n"));
                text = text.Remove(text.LastIndexOf($"\n"));
                tooltips[index].Text = text;
            }
        }
    }
    public class KamoPlayer : ModPlayer
    {
        public bool Leaky = false;
        public int counter = 0;
        public float Stored = 0;
        public override void ResetEffects()
        {
            if (counter > 1400 && !Player.HasBuff(ModContent.BuffType<BloodShieldBleed>()) && !Player.HasBuff(ModContent.BuffType<BloodShield>()))
            {
                Player.AddBuff(ModContent.BuffType<BloodShield>(), 240);
            }

            if (counter < 1200 && !Player.HasBuff(ModContent.BuffType<BloodShield>()))
                Stored = 0;

            Leaky = false;
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (Leaky && (KeybindSystem.BloodDripKey.JustPressed || (KeybindSystem.BloodDripKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.K))) && Main.myPlayer == Player.whoAmI)
            {
                if (counter <= 0)
                {
                    Player.AddBuff(ModContent.BuffType<BloodShieldBleed>(), 360);

                    Projectile.NewProjectile(Player.GetSource_Buff(Player.FindBuffIndex(ModContent.BuffType<BloodShieldBleed>())), Player.Center, Vector2.Zero, ModContent.ProjectileType<CursedBloodEffect>(), 0, 0, Player.whoAmI);
                    
                    counter = 1800;
                }
                else if (counter < 1740 && Player.HasBuff(ModContent.BuffType<BloodShieldBleed>()))
                {
                    Player.ClearBuff(ModContent.BuffType<BloodShieldBleed>());
                    Player.AddBuff(ModContent.BuffType<BloodShield>(), 240);
                }
            }
        }
    }
    public class KamoLootNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.GoblinShark)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CursedBlood>(), 6));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}