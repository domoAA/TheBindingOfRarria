using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;

public class PetriciteCuffs : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.DefaultToAccessory(26, 28);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<PetriciteCuffsPlayer>().HasPetriciteCuffs = true;
    }
}
public class PetriciteCuffsPlayer : ModPlayer
{
    public bool HasPetriciteCuffs = false;

    public override void ResetEffects()
    {
        HasPetriciteCuffs = false;
    }

    public void ModifyHitByAnything(ref Player.HurtModifiers modifiers)
    {
        int ManaCost = (int)(20 * Player.manaCost);
        if (HasPetriciteCuffs && Player.statMana > ManaCost) {
            float MagicDamagePercent = (Player.GetTotalDamage(DamageClass.Magic).Additive - 1) * 100;
            float MagicDamageSqrt = MathF.Sqrt(MagicDamagePercent);
            float DamageBlock = 1 - ((MagicDamageSqrt / 100) + 0.10f);
            modifiers.FinalDamage *= DamageBlock;
            Player.statMana -= ManaCost;
        }
    }
    public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
    {
        ModifyHitByAnything(ref modifiers);
    }
    public override void ModifyHitByProjectile(Projectile projectile, ref Player.HurtModifiers modifiers)
    {
        ModifyHitByAnything(ref modifiers);
    }
}