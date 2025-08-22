using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Config;
using TheBindingOfRarria.Content.Buffs;
using TheBindingOfRarria.Content.Buffs.Debuffs;
using TheBindingOfRarria.Content.Projectiles;

namespace TheBindingOfRarria.Content.Items;

public class CursedBlood : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 24;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.sellPrice(0, 1, 38);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<KamoPlayer>().Leaky = true;
        player.GetModPlayer<KamoPlayer>().counter--;
        if (player.GetModPlayer<KamoPlayer>().counter > 0)
            player.AddBuff(ModContent.BuffType<BloodShield_CD>(), player.GetModPlayer<KamoPlayer>().counter);
    }

        // Make a helper method for this, you tend to paste this alot.
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        string key = KeybindSystem.BloodDripKey.GetAssignedKeys().FirstOrDefault();
        if (key == "" || key == null)
            key = "K";

        string text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.CursedBlood.Tooltip"), key);


        int index = tooltips.FindIndex(line => line.Name == "Tooltip0");
        if (index != -1)
        {
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(text.LastIndexOf($"\n"));
            tooltips[index].Text = text;
        }
    }
}

public class KamoPlayer : ModPlayer
{
    public bool Leaky = false;
    public int counter = 0;
    public float Stored = 0;

    public override void ResetEffects()
    {
        if ((!Leaky || counter <= 0) && Player.HasBuff(ModContent.BuffType<BloodShield_CD>()))
            Player.ClearBuff(ModContent.BuffType<BloodShield_CD>());

        if (counter > 1400 && !Player.HasBuff(ModContent.BuffType<BloodShieldBleed>()) && !Player.HasBuff(ModContent.BuffType<BloodShield>()))
            Player.AddBuff(ModContent.BuffType<BloodShield>(), 240);

        if (counter < 1200 && !Player.HasBuff(ModContent.BuffType<BloodShield>()))
            Stored = 0;

        Leaky = false;
    }

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (Leaky && (KeybindSystem.BloodDripKey.JustPressed || (KeybindSystem.BloodDripKey.GetAssignedKeys().FirstOrDefault() == null && Main.keyState.IsKeyDown(Keys.K))) && Main.myPlayer == Player.whoAmI)
        {
            if (counter <= 0)
            {
                Player.AddBuff(ModContent.BuffType<BloodShieldBleed>(), 360);

                Projectile.NewProjectile(Player.GetSource_Buff(Player.FindBuffIndex(ModContent.BuffType<BloodShieldBleed>())), Player.Center, Vector2.Zero, ModContent.ProjectileType<CursedBloodEffect>(), 0, 0, Player.whoAmI);
                
                counter = 1800;
            }
            else if (counter < 1740 && Player.HasBuff(ModContent.BuffType<BloodShieldBleed>()))
            {
                Player.ClearBuff(ModContent.BuffType<BloodShieldBleed>());
                Player.AddBuff(ModContent.BuffType<BloodShield>(), 240);
            }
        }
    }
}

public class KamoLootNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.GoblinShark)
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CursedBlood>(), 10));
        base.ModifyNPCLoot(npc, npcLoot);
    }
}