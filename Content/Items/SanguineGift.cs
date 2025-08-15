using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.UI;

namespace TheBindingOfRarria.Content.Items;

public class SanguineGift : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 32;
        Item.height = 30;
        Item.expert = true;
        Item.value = Item.sellPrice(0, 2);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        SanguinePlayer p = player.GetModPlayer<SanguinePlayer>();
        p.Sanguine = true;


        if (Main.LocalPlayer.whoAmI == player.whoAmI)
            ModContent.GetInstance<BloodStorageUISystem>()?.Show();
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        int index = tooltips.FindIndex(t => t.Name == "Tooltip1");
        if (index != -1) 
        {
            string text = string.Format(Language.GetTextValue("Mods.TheBindingOfRarria.Items.SanguineGift.Tooltip"), $"{Main.LocalPlayer.statLifeMax2 / 10}", $"[c/{Color.DarkGreen.Hex3()}:({Main.LocalPlayer.GetModPlayer<SanguinePlayer>().Stored})]");

            text = text.Remove(text.LastIndexOf($"\n"));
            string cur = text[(text.LastIndexOf($"\n") + 1)..];
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(text.LastIndexOf($"\n"));
            text = text.Remove(0, text.IndexOf($"\n") + 1);
            tooltips[index].Text = text;
            tooltips[index + 2].Hide();
                // tooltips[index + 2].Text = cur;
                // tooltips[index + 2].OverrideColor = Color.Gray;
        } 
    }
}

public class SanguinePlayer : ModPlayer
{
    public int Stored = 0;

    public bool Sanguine = false;

    public override void ResetEffects()
    {
        if (!Sanguine) 
        { 
            Stored = 0;
            if (Main.LocalPlayer.whoAmI == Player.whoAmI)
                ModContent.GetInstance<BloodStorageUISystem>()?.Hide();
        }

        Sanguine = false;
    }

    public void OnHit(Player.HurtInfo info)
    {
        if (Sanguine)
            Stored += info.Damage / 5;
    }

    public override void OnHurt(Player.HurtInfo info) => OnHit(info);

    public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
    {
        if (attempt.fishingLevel > 69 && attempt.legendary && Main.bloodMoon && Main.hardMode && Main.expertMode && Main.rand.NextBool(3))
        {
            npcSpawn = -1;
            sonar.Color = Color.Red;
            sonar.Text = Language.GetTextValue("Mods.TheBindingOfRarria.Items.SanguineGift.DisplayName");
            itemDrop = ModContent.ItemType<SanguineGift>();
        }
    }

    public override void PostUpdate()
    {
        if (Stored >= Player.statLifeMax2 / 10)
        {
            int teammate = -1;
            float dist = 800 * 800;
            foreach (var pl in Main.ActivePlayers)
            {
                if (pl.team == Player.team && pl.whoAmI != Player.whoAmI && !pl.dead && pl.statLife > 0 && pl.Center.DistanceSQ(Player.Center) < dist)
                {
                    dist = pl.Center.DistanceSQ(Player.Center);
                    teammate = pl.whoAmI;
                }
            }
            if (teammate != -1)
            {
                Stored /= 2;
                Main.player[teammate].Heal(Stored);
            }
            Player.Heal(Stored);
            Stored = 0;
        }
    }
}

public class SanguineDropNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.BloodNautilus)
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<SanguineGift>(), 6));

        base.ModifyNPCLoot(npc, npcLoot);
    }
}