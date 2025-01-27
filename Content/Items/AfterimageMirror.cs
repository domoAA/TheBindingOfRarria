using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items
{
    public class AfterimageMirror : ModItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !equippedItem.HasTag(TheBindingOfRarria.reflectItems) || !incomingItem.HasTag(TheBindingOfRarria.reflectItems);
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 2);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PlatedPlayer>().HasPlate = true;
        }
    }
    public class PlatedPlayer : ModPlayer
    {
        public bool HasPlate = false;
        public override void ResetEffects()
        {
            HasPlate = false;
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitByProjectile(proj, ref modifiers);
            if (!HasPlate || proj.reflected)
                return;

            var reflected = Main.rand.NextFloat() < 0.15f;

            if (!reflected)
                return;

            modifiers.Cancel();
            Player.immune = true;
            Player.SetImmuneTimeForAllTypes(70);

            Projectile.NewProjectile(Player.GetSource_OnHurt(modifiers.DamageSource), proj.Center, Vector2.Zero, ModContent.ProjectileType<MirrorCrack>(), 0, 0, Player.whoAmI, proj.velocity.ToRotation());
        }
    }
    public class ReflectiveLootNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.BigMimicCorruption || npc.type == NPCID.BigMimicCrimson || npc.type == NPCID.BigMimicHallow || npc.type == NPCID.BigMimicJungle || npc.type == NPCID.ShimmerSlime)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<AfterimageMirror>(), 6));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}