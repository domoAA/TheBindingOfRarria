
namespace TheBindingOfRarria.Content.Items
{
    public class UnendingDespair : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 28;
            Item.height = 30;
        }
        public int counter = 0;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.GetModPlayer<LifeSuckerPlayer>().heal != 0)
            {
                player.Heal(player.GetModPlayer<LifeSuckerPlayer>().heal);
                player.GetModPlayer<LifeSuckerPlayer>().heal = 0;
            }
            counter++;
            if (counter >= 300)
            {
                List<int> victims = [];
                for (int i = 0; i < 4; i++)
                {
                    float dist = 325 * 325;
                    victims.Add(-1);
                    foreach (var t in Main.ActiveNPCs)
                    {
                        if (!victims.Contains(t.whoAmI) && !t.friendly && t.Center.DistanceSQ(player.Center) < dist)
                        {
                            dist = t.Center.DistanceSQ(player.Center);
                            
                            victims[i] = t.whoAmI;
                        }
                    }
                    if (victims.Count == i + 1 && victims[i] != -1)
                    {
                        counter = 0;
                        Projectile.NewProjectile(player.GetSource_Accessory(Item, "Unending Despair sucking"), Main.npc[victims[i]].Center, Main.npc[victims[i]].Center - player.Center, ModContent.ProjectileType<LifeSucker>(), player.statLifeMax2 / 10, 0, player.whoAmI, victims[i]);
                    }
                }
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(t => t.Name == "Tooltip0");
            if (index != -1)
            {
                var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.UnendingDespair.Tooltip"), $"[c/{Color.LightGreen.Hex3()}:({Main.LocalPlayer.statLifeMax2 / 10})]");

                text = text.Remove(text.LastIndexOf($"\n"));
                text = text.Remove(text.LastIndexOf($"\n"));
                tooltips[index].Text = text;
            }
        }
    }
    public class LifeSuckerPlayer : ModPlayer
    {
        public int heal = 0;
    }
}