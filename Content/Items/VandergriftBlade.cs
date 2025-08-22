using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TheBindingOfRarria.Content.Items;
public class VandergriftBlade : ModItem
{
    public override string Texture =>  ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 28;
        Item.height = 34;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(0, 1, 30, 0);
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<VandergriftBladePlayer>().Active = true;
}

public class VandergriftBladePlayer : ModPlayer {
    public bool Active;

    public int StoredHeal = 0;

    public float DefenseConversion = .5f;

    public bool Healed = false;

    public override void ResetEffects() => Active = false;

    public override void Load()
    {
        On_Player.Heal += ModifyHealAmount;
        On_Player.HealEffect += ModifyHealEffect;
    }

    public override void Unload()
    {
        On_Player.Heal -= ModifyHealAmount;
        On_Player.HealEffect -= ModifyHealEffect;
    }

    public static void ModifyHealEffect(On_Player.orig_HealEffect orig, Player self, int healAmount, bool broadcast)
    {
        int heal_value = healAmount;

        VandergriftBladePlayer player = self.GetModPlayer<VandergriftBladePlayer>();
        if (!player.Healed && player.Active) {
            heal_value += player.StoredHeal;
            self.statLife += player.StoredHeal;
            player.StoredHeal = 0;
        }

        player.Healed = false;
        orig(self, heal_value, broadcast);
    }

    public static void ModifyHealAmount(On_Player.orig_Heal orig, Player self, int amount)
    {
        int heal_value = amount;

        VandergriftBladePlayer player = self.GetModPlayer<VandergriftBladePlayer>();
        if (player.Active)
        {
            heal_value += player.StoredHeal;
            self.statLife += player.StoredHeal;
            player.StoredHeal = 0;
        }

        player.Healed = true;
        orig(self, heal_value);
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (!Active) return;
        int target_life_old = target.life + damageDone;
        if (damageDone > target_life_old)
        {
            StoredHeal += (int)(target.defense * DefenseConversion);
        }
    }

    public override void PostUpdateEquips() {
        if (!Active) StoredHeal = 0;
    }
}