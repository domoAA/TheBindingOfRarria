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
        Item.width = 30;
        Item.height = 30;
        Item.rare = ItemRarityID.Green; // ?
        Item.value = Item.sellPrice(0, 0, 30, 80); // ?
    }

    public override void UpdateAccessory(Player player, bool hideVisual) => player.GetModPlayer<VandergriftBladePlayer>().Active = true;
}

public class VandergriftBladePlayer : ModPlayer {
    public bool Active;

    public int StoredHeal = 0;

    public float DefenseConversion = .5f;

    public override void ResetEffects() => Active = false;

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

    public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
    {
        if (!Active) return;
        healValue += StoredHeal;
    }
}

public class VandergriftBladeitem : GlobalItem {
    public override void OnConsumeItem(Item item, Player player)
    {
        if (item.healLife > 0)
            player.GetModPlayer<VandergriftBladePlayer>().StoredHeal = 0;
    }
}