using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace TheBindingOfRarria.Content.Items
{
    public class WeaverSong : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 30;
            Item.height = 30;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 2, 28);
            Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 2;
            player.GetModPlayer<SpooderPlayer>().Spooder = true;
        }
    }
    public class SpooderPlayer : ModPlayer
    {
        public bool Spooder = false;
        public override void ResetEffects()
        {
            Spooder = false;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPCWithProj(proj, target, ref modifiers);
            
            if (proj.minion && Spooder)
            {
                target.GetSlowed(TheBindingOfRarria.State.Slow, 120);
            }
        }
    }
    public class SpiderDropNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BlackRecluse || npc.type == NPCID.BlackRecluse)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<WeaverSong>(), 50));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
    public class SpiderDropCodweb : GlobalTile
    {
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (Main.expertMode && type == TileID.Cobweb && Main.rand.NextFloat() < 0.001f)
            {
                Item.NewItem(WorldGen.GetItemSource_FromTileBreak(i, j), new Point(i, j).ToWorldCoordinates(), ModContent.ItemType<WeaverSong>());
                noItem = true;
            }
            else
                base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
        }
    }
}
