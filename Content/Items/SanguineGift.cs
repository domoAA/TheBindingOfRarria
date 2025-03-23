

namespace TheBindingOfRarria.Content.Items
{
    public class SanguineGift : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 32;
            Item.height = 30;
            Item.expert = true;
            Item.value = Item.buyPrice(0, 2);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var p = player.GetModPlayer<SanguinePlayer>();
            p.Sanguine = true;
            if (p.Stored >= player.statLifeMax2 / 10)
            {
                Player teammate = null;
                float dist = 700 * 700;
                foreach (var pl in Main.ActivePlayers)
                {
                    if (pl.whoAmI != player.whoAmI && !pl.dead && pl.statLife > 0 && pl.Center.DistanceSQ(player.Center) < dist)
                    {
                        dist = pl.Center.DistanceSQ(player.Center);
                        teammate = pl;
                    }
                }
                if (teammate != null)
                {
                    p.Stored /= 2;
                    teammate.Heal(p.Stored);
                }
                player.Heal(p.Stored);
                p.Stored = 0;
            }
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(t => t.Name == "Tooltip1");
            if (index != -1) 
            {
                var text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.SanguineGift.Tooltip"), $"[c/{Color.LightGreen.Hex3()}:({Main.LocalPlayer.statLifeMax2 / 10})]", $"[c/{Color.DarkGreen.Hex3()}:({Main.LocalPlayer.GetModPlayer<SanguinePlayer>().Stored})]");

                text = text.Remove(text.LastIndexOf($"\n"));
                var cur = text[(text.LastIndexOf($"\n") + 1)..];
                text = text.Remove(text.LastIndexOf($"\n"));
                text = text.Remove(text.LastIndexOf($"\n"));
                text = text.Remove(0, text.IndexOf($"\n")+1);
                tooltips[index].Text = text;
                tooltips[index + 2].Text = cur;
                tooltips[index + 2].OverrideColor = Color.Gray;
            } 
        }
    }
    public class SanguinePlayer : ModPlayer
    {
        public int Stored = 0;
        public bool Sanguine = false;
        public override void ResetEffects()
        {
            if (!Sanguine)
                Stored = 0;
            Sanguine = false;
        }
        public void OnHit(Player.HurtInfo info)
        {
            if (Sanguine)
                Stored += info.Damage / 5;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo) => OnHit(hurtInfo);
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo) => OnHit(hurtInfo);
    }
    public class SanguineDropNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BloodNautilus)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<SanguineGift>(), 6));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}