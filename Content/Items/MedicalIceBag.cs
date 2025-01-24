using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using TheBindingOfRarria.Content.Buffs;
using System;
using Terraria.GameContent.ItemDropRules;

namespace TheBindingOfRarria.Content.Items
{
    public class MedicalIceBag : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 2);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MedicatedPlayer>().IsMedicated = true;
        }
    }
    public class MedicatedPlayer : ModPlayer
    {
        public enum DiseaseImmunity
        {
            Poison,
            Fire
        }
        public DiseaseImmunity CurrentImmunity = DiseaseImmunity.Poison;
        public bool IsMedicated = false;
        public override void PostUpdateEquips()
        {
            if (!IsMedicated) {
                var type = ModContent.BuffType<PoisonImmunity>();
                int index = Array.FindIndex(Player.buffType, buff => buff == type);
                if (index == -1) {
                    type = ModContent.BuffType<FireImmunity>();
                    index = Array.FindIndex(Player.buffType, buff => buff == type);}

                if (index != -1)
                    Player.ClearBuff(type);
                return; }

            bool IsPoisoned = Player.HasBuff(BuffID.Poisoned) || Player.HasBuff(BuffID.Venom);
            bool IsOnFire = Player.HasBuff(BuffID.OnFire) || Player.HasBuff(BuffID.OnFire3);

            if (IsOnFire && !IsPoisoned)
                CurrentImmunity = DiseaseImmunity.Fire;
            else if (IsPoisoned && !IsOnFire)
                CurrentImmunity = DiseaseImmunity.Poison;

            if (Player.HasBuff(ModContent.BuffType<FireImmunity>()) || Player.HasBuff(ModContent.BuffType<PoisonImmunity>()))
                return;

            if (CurrentImmunity == DiseaseImmunity.Poison)
                Player.AddBuff(ModContent.BuffType<PoisonImmunity>(), 300);
            else
                Player.AddBuff(ModContent.BuffType<FireImmunity>(), 300);
        }
        public override void ResetEffects()
        {
            IsMedicated = false;
        }
    }
    public class MedicatedAndCoolNPC : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.type == NPCID.SpikedIceSlime)
            {
                npcLoot.Add(ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<MedicalIceBag>(), 5));
            }
            base.ModifyNPCLoot(npc, npcLoot);
        }
    }
}