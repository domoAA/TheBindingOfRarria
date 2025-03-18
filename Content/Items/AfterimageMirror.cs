
namespace TheBindingOfRarria.Content.Items
{
    public class AfterimageMirror : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 2);
            Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<PlatedPlayer>().Mirror = Item;
        
    }
    public class PlatedPlayer : ModPlayer
    {
        public Item Mirror = null;
        public bool reflected = false;
        public override void ResetEffects() =>Mirror = null;
        
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (Mirror != null && reflected)
            {
                Player.immune = true;
                Player.immuneTime = 70;
                reflected = false;
                return true;
            }
            else
                return base.FreeDodge(info);
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (Mirror == null)
            {
                base.ModifyHitByProjectile(proj, ref modifiers);
            }
            else
            {
                reflected = Main.rand.NextFloat() < 0.16f;

                if (!reflected)
                    return;

                Projectile.NewProjectileDirect(Player.GetSource_Accessory_OnHurt(Mirror, modifiers.DamageSource), proj.Center, Vector2.Zero, ModContent.ProjectileType<MirrorCrack>(), 0, 0, Player.whoAmI, proj.velocity.ToRotation() + MathHelper.Pi, 0, Main.rand.Next(0, 2));

                

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                    packet.Write((int)TheBindingOfRarria.PacketTypes.ProjectileReflect);
                    packet.Write(proj.identity);
                    packet.Send();
                }
                else
                    proj.GetReflected();

                var sound = SoundID.Shatter;
                sound.Volume *= 0.4f;
                sound.Pitch -= 0.6f;
                SoundEngine.PlaySound(sound, proj.Center);
                return;
            }
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