using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items
{
    public class ShamanStone : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage(DamageClass.Magic) += 0.1f;
            player.GetModPlayer<ShamanSnailPlayer>().ShamanStone = true;
        }
    }
    public class ShamanSnailPlayer : ModPlayer
    {
        public bool ShamanStone = false;
        public override void ResetEffects()
        {
            ShamanStone = false;
        }
    }
    public class ShamanSnailGlobalProjectile : GlobalProjectile 
    {
        // wip
        public override bool InstancePerEntity => true;

        public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
        {
            return entity.friendly && entity.DamageType == DamageClass.Magic;
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            base.OnSpawn(projectile, source);
            if (Main.player[projectile.owner].GetModPlayer<ShamanSnailPlayer>().ShamanStone)
            {
                projectile.scale *= 1.5f;
                projectile.Resize((int)(projectile.width * 1.5f), (int)(projectile.height * 1.5f));
            }
        }
    }
}