using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ID.PrefixID;

namespace TheBindingOfRarria.Content.Items;

public class Aegis : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 34;
        Item.width = 34;
        Item.rare = ItemRarityID.Expert;
        Item.value = Item.sellPrice(0, 9, 60);
        Item.expert = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<AegisPlayer>().Actor = true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        var t0 = tooltips.FindIndex(t => t.Name == "Tooltip0");
        if (t0 == -1)
            return;

        int index = t0 + (int)Main.LocalPlayer.GetModPlayer<AegisPlayer>().CurrentAffinity;

        for (int i = t0 + 1; i < t0 + 4; i++)
        {
            tooltips[i].OverrideColor = i == index ? new Color(144, 238, 144, 255) : Color.Gray;
        }
    }
}

public class AegisDropNPC : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.BigMimicHallow)
            npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<Aegis>(), 6));

        base.ModifyNPCLoot(npc, npcLoot);
    }
}

public class AegisPlayer : ModPlayer
{
    public bool Actor = false;
    public enum Affinity
    {
        None,
        Offense,
        Defense,
        Other
    }

    public Affinity CurrentAffinity = Affinity.None;

    public (List<bool> offense, List<bool> defense, List<bool> other) affinities = ([], [], []);

    public static int[] Offensive = [Precise, Lucky, Jagged, Spiked, Angry, Menacing, Wild, Rash, Intrepid, Violent];

    public static int[] Defensive = [Hard, Guarding, Armored, Warding, Brisk, Fleeting, Hasty, Quick, Hasty2, Quick2];

    public override void PostUpdateEquips()
    {
        if (!Actor)
            CurrentAffinity = Affinity.None;

        if (CurrentAffinity == Affinity.Defense)
            Player.DefenseEffectiveness *= 1.1f;

        else if (CurrentAffinity == Affinity.Offense)
            Player.GetArmorPenetration(DamageClass.Generic) += 10;
    }

    public override void PostUpdateMiscEffects()
    {

        var def = affinities.defense.Count;
        var off = affinities.offense.Count;
        var oth = affinities.other.Count;

        if (def >= off && def > oth)
            CurrentAffinity = Affinity.Defense;

        else if (off > def && off > oth)
            CurrentAffinity = Affinity.Offense;

        else if (oth > 0)
            CurrentAffinity = Affinity.Other;

        else 
            CurrentAffinity = Affinity.None;

        affinities.offense.Clear();
        affinities.defense.Clear();
        affinities.other.Clear();
    }

    public override void PreUpdate()
    {
        Actor = false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (!Actor || CurrentAffinity != Affinity.Other || Player.lifeSteal <= 0 || !target.canGhostHeal || target.immortal || target.lifeMax <= 5)
            return;

        Player.Heal(damageDone / 10);
        Player.lifeSteal -= damageDone * 2;
    }
}

public class PrefixCountAccessory : GlobalItem
{
    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.accessory;
    }

    public override void UpdateAccessory(Item item, Player player, bool hideVisual)
    {
        var p = player.GetModPlayer<AegisPlayer>();

        if (AegisPlayer.Offensive.Contains(item.prefix))
            p.affinities.offense.Add(true);

        else if (AegisPlayer.Defensive.Contains(item.prefix))
            p.affinities.defense.Add(true);

        else if (item.prefix != 0)
            p.affinities.other.Add(true);
    }
}