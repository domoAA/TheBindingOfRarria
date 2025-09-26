using System;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class GhizaWheel : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name + "Handle";

    public override void SetDefaults()
    {
        Item.height = 100;
        Item.width = 200;
        Item.shoot = ModContent.ProjectileType<GhizaWheelSaw>();
        Item.damage = 33;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.noUseGraphic = true;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.noMelee = true;
        Item.useTime = 50;
        Item.useAnimation = 50;
        Item.useTurn = false;
        Item.value = Item.sellPrice(gold: 1, silver: 60);
        Item.rare = ItemRarityID.Pink;
        Item.knockBack = 5f;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.ownedProjectileCounts[Item.shoot] > 0)
        {
            Array.Find(Main.projectile, p => p.active && p.type == Item.shoot && p.owner == player.whoAmI).timeLeft = 55;
            return false;
        }
        return base.Shoot(player, source, position, velocity, type, damage, knockback);
    }

    public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
    {
        var texture = TextureAssets.Projectile[Item.shoot];

        var origin = new Vector2(31, 30);

        Main.spriteBatch.Draw(texture.Value, Item.Center - Main.screenPosition, null, lightColor, rotation, origin, scale, SpriteEffects.None, 0);

        return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
    }

    public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
    {
        var texture = TextureAssets.Projectile[Item.shoot];

        var Origin = new Vector2(31, 30);
        var width = (int)(frame.Width * scale);
        var height = (int)(frame.Height * scale);
        position += new Vector2(-width / 4, height / 4);

        var rot = (frame with { X = (int)position.X - width, Y = (int)position.Y - height, Width = (int)(width * 1.8f), Height = (int)(height * 1.8f) }).Contains((Main.MouseWorld - Main.screenPosition).ToPoint()) ? -Main.GlobalTimeWrappedHourly : 0;

        Main.spriteBatch.Draw(texture.Value, position + new Vector2(width / 2, - height / 2), null, drawColor, rot, Origin, scale, SpriteEffects.None, 0);


        texture = TextureAssets.Item[Type];

        position += new Vector2(width / 8, 0);

        Main.spriteBatch.Draw(texture.Value, position , frame, drawColor, 0, origin, scale, SpriteEffects.None, 0);

        return false;
    }
}