using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using TheBindingOfRarria.Content.Dusts;
using System;
using System.Linq;

namespace TheBindingOfRarria.Content.Items;

public class PiercingMissile : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.height = 30;
        Item.width = 20;
        Item.accessory = true;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 0, 80);
        Item.expert = true;
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<NilfgaardBallistaPlayer>().BallistaMissile = true;
    }
}

public class NilfgaardBoltItemNPCShop : GlobalNPC
{
    public override void ModifyActiveShop(NPC npc, string shopName, Item[] items)
    {
        // tavernkeep
        if (npc.type == NPCID.DD2Bartender && Main.expertMode)
        {
            var index = Array.FindIndex(items, e => e == null);

            if (index != -1)
                items[index] = new Item(ModContent.ItemType<PiercingMissile>())
                {
                    shopCustomPrice = 5,
                    shopSpecialCurrency = CustomCurrencyID.DefenderMedals

                };

        }
    }
}

public class NilfgaardBallistaPlayer : ModPlayer
{
    public bool BallistaMissile = false;

    public override void ResetEffects() => BallistaMissile = false;
}

public class PiercingMissileProj : GlobalProjectile
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation) => entity.friendly;

    public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (Piercing)
            modifiers.ArmorPenetration += 8;
    }

    public bool Piercing = false;

    public override void PostAI(Projectile projectile)
    {
        Main.LocalPlayer.downedDD2EventAnyDifficulty = true;
        if (Main.player[projectile.owner].GetModPlayer<NilfgaardBallistaPlayer>().BallistaMissile && projectile.damage > 0 && projectile.penetrate != 1)
            Piercing = true;
        else
            Piercing = false;
    }

    public override void PostDraw(Projectile projectile, Color lightColor)
    {
        if (Piercing && projectile.velocity.LengthSquared() > 1)
        {
            Dust.NewDustPerfect(projectile.Center - projectile.velocity * Main.rand.NextFloat(), ModContent.DustType<PixellatedDustE98>(), -projectile.velocity.SafeNormalize(Vector2.Zero) * Math.Min(9, projectile.velocity.Length() * 0.6f), 200, new Color(166, 16, 30), 0.1f * Math.Min(5, projectile.velocity.Length() * 0.6f));
        }
    }
}