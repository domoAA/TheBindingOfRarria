using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class PhantomPopper : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.DefaultToAccessory(22, 26);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        // Устанавливаем флаг, чтобы GlobalProjectile знал, что аксессуар экипирован
        player.GetModPlayer<PhantomPopperPlayer>().HasPhantomPopper = true;
    }
}
public class PhantomPopperPlayer : ModPlayer
{
    public bool HasPhantomPopper = false;

    public override void ResetEffects()
    {
        HasPhantomPopper = false;
    }
}

public class PhantomPopperGlobalProjectile : GlobalProjectile
{
    private bool initialized = false;
    private float splitTimer = 180;
    private bool noSplit = false;

    public override bool InstancePerEntity => true;

    public override void AI(Projectile projectile)
    {
        if (projectile.DamageType == DamageClass.Magic && projectile.owner == Main.myPlayer && !noSplit)
        {
            Player player = Main.player[projectile.owner];

            if (player.GetModPlayer<PhantomPopperPlayer>().HasPhantomPopper && !initialized)
            {
                initialized = true;
            }

            if (initialized)
            {
                splitTimer--;

                if (splitTimer <= 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));
                        int proj = Projectile.NewProjectile(
                            player.GetSource_FromThis(),
                            projectile.Center,
                            velocity,
                            ProjectileID.LostSoulFriendly,
                            projectile.damage / 3,
                            projectile.knockBack,
                            player.whoAmI
                        );

                        Main.projectile[proj].GetGlobalProjectile<PhantomPopperGlobalProjectile>().noSplit = true;
                        Main.projectile[proj].timeLeft = 300;
                        Main.projectile[proj].tileCollide = false;
                    }

                    // Уничтожаем оригинальный снаряд
                    projectile.Kill();
                }
            }
        }
    }
}
