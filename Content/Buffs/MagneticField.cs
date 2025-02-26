
using Terraria.ModLoader;
using Terraria;
using System;
using TheBindingOfRarria.Content.Projectiles;
using Microsoft.Xna.Framework;

namespace TheBindingOfRarria.Content.Buffs
{
    public class MagneticField : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
        }
        public float power = 0.2f;
        public override void Update(NPC npc, ref int buffIndex)
        {
            power = Math.Clamp(power, 0, 1);


            foreach (var proj in Main.ActiveProjectiles)
            {
                if (proj != null && proj.Center.DistanceSQ(npc.Center) < 169 * 169 && power > 0 && proj.CanBeReflected())
                {
                    proj.velocity *= 0.97f;
                    proj.velocity += proj.Center.DirectionTo(npc.Center) * 3 * power;
                }
            }
        }
        public override bool ReApply(NPC npc, int time, int buffIndex)
        {
            power += 0.2f;
            return base.ReApply(npc, time, buffIndex);
        }
    }
    public class FerromagneticGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public override void DrawBehind(NPC npc, int index)
        {
            if (npc.HasBuff(ModContent.BuffType<MagneticField>()))
            {
                var texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<RepellingPulse>()].Value;

                Main.EntitySpriteDraw(texture, npc.Center, texture.Bounds, Color.Red, npc.rotation, texture.Size() / 2, npc.scale * 8, Microsoft.Xna.Framework.Graphics.SpriteEffects.None);
            }
        }
    }
}