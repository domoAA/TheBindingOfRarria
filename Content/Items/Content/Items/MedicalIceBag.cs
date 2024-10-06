using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

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
        public override void PostUpdateBuffs()
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
            bool IsPoisoned = Player.HasBuff(BuffID.Poisoned) || Player.HasBuff(BuffID.Venom);
            bool IsOnFire = Player.HasBuff(BuffID.OnFire) || Player.HasBuff(BuffID.OnFire3);

            if (IsOnFire && !IsPoisoned)
                CurrentImmunity = DiseaseImmunity.Fire;
            else if (IsPoisoned && !IsOnFire)
                CurrentImmunity = DiseaseImmunity.Poison;
        }
        public override void ResetEffects()
        {
            IsMedicated = false;
        }
    }
}