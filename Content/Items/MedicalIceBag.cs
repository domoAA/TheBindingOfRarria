using Terraria.ModLoader;
using Terraria.ID;
using System;

namespace TheBindingOfRarria.Content.Items
{
    public class MedicalIceBag : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
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
        public override void PostUpdateBuffs()
        {
            if (Player.HasBuff(BuffID.OnFire) || Player.HasBuff(BuffID.OnFire3))
                CurrentImmunity = DiseaseImmunity.Fire;
            else
                CurrentImmunity = DiseaseImmunity.Poison;
        }
        public override void PostUpdateEquips()
        {
            var index = Array.FindIndex(Player.armor, item => !item.social && item.type == ModContent.ItemType<MedicalIceBag>());
            if (index != -1)
                IsMedicated = true;
            else
                IsMedicated = false;
        }
        public override void PreUpdate()
        {
            if (!IsMedicated)
                return;

            if (CurrentImmunity == DiseaseImmunity.Poison)
            {
                Player.buffImmune[BuffID.Poisoned] = true;
                Player.buffImmune[BuffID.Venom] = true;
            }
            else
            {
                Player.buffImmune[BuffID.OnFire] = true;
                Player.buffImmune[BuffID.OnFire3] = true;
            }
        }
    }
}