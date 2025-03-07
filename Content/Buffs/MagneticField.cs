
namespace TheBindingOfRarria.Content.Buffs
{
    public class MagneticField : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
            var distance = 128 * (1 + npc.Size.Length() / 100);

            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj != null && proj.Center.DistanceSQ(npc.Center) < distance * distance && proj.CanBeReflected())
                {
                    proj.velocity *= 0.99f;
                    proj.velocity = proj.velocity.RotatedBy(proj.velocity.ToRotation().AngleLerp(proj.Center.DirectionTo(npc.Center).ToRotation(), 0.1f) - proj.velocity.ToRotation());
                }
            }
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            return base.ReApply(npc, time, buffIndex);
        }
    }
    public class FerromagneticGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if (npc.HasBuff(ModContent.BuffType<MagneticField>()))
            {
                var texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<RepellingPulse>()].Value;
                texture.DrawWithTransparency(npc.Center - Main.screenPosition, 1 + npc.Size.Length() / 100, Color.SteelBlue, 90);
                Lighting.AddLight(npc.Center, Color.SteelBlue.ToVector3() * (npc.Size.Length() / 100));
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }
    }
}