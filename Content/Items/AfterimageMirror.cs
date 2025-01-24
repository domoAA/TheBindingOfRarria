using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class AfterimageMirror : ModItem
    {
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            return !equippedItem.HasTag(TheBindingOfRarria.blockItems) || !incomingItem.HasTag(TheBindingOfRarria.blockItems);
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
        public bool Reflected = false;
        public int timeLeft = 0;
        public float rotation = 0;
        public Vector2 position = Vector2.Zero;
        public byte alpha = 0;
        public bool apply = false;
        public override void ResetEffects()
        {
            HasPlate = false;
            Reflected = false;
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            base.ModifyHitByProjectile(proj, ref modifiers);
            if (!HasPlate)
                return;

            Reflected = Player.ReflectProjectiles(DamageClass.Ranged, 0.1f);

            if (!Reflected)
                return;

            timeLeft = 240;
            position = proj.Center - Main.screenPosition;
            rotation = proj.velocity.ToRotation();
            alpha = 155;
            apply = Main.rand.NextBool();
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (HasPlate && Reflected)
            {
                return true;
            }
            return base.FreeDodge(info);
        }
        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            base.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
            
            DrawCrack(position, rotation, alpha);

            timeLeft = alpha == 0 ? 0 : timeLeft > 0 ? timeLeft-- : 0;
            alpha -= alpha > 1 ? (byte)2 : (byte)0;

            if (timeLeft != 0)
                return;

            alpha = 0;
            rotation = 0;
            position = Vector2.Zero;
        }
        public void DrawCrack(Vector2 position, float rotation, byte alpha)
        {
            if (timeLeft <= 0)
                return;

            var texture = TheBindingOfRarria.MirrorCrack.Value;
            texture.DrawWithTransparency(position, 2f, rotation, Color.White, alpha, Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically, apply);
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