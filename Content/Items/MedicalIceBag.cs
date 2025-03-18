
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
            Item.expert = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MedicatedPlayer>().Cool = true;
            if (player.HasBuff(BuffID.OnFire))
            {
                player.AddBuff(ModContent.BuffType<FireImmunity>(), 300);
                player.buffImmune[BuffID.OnFire] = true;
            }
            if (player.HasBuff(BuffID.Poisoned))
            {
                player.AddBuff(ModContent.BuffType<PoisonImmunity>(), 300);
                player.buffImmune[BuffID.Poisoned] = true;
            }
        }
    }
    public class MedicatedPlayer : ModPlayer
    {
        public bool Cool = false;
        public override void ResetEffects() => Cool = false;
        
        public override void Load() => On_Player.AddBuff_DetermineBuffTimeToAdd += CoolBuffTime;
        

        private int CoolBuffTime(On_Player.orig_AddBuff_DetermineBuffTimeToAdd orig, Player self, int type, int time1)
        {
            int buffTime = orig(self, type, time1);

            if (!self.GetModPlayer<MedicatedPlayer>().Cool)
                return buffTime;

            if (type == BuffID.OnFire || type == BuffID.Poisoned)
            {
                var buff = type == BuffID.OnFire ? ModContent.BuffType<FireImmunity>() : ModContent.BuffType<PoisonImmunity>();
                self.AddBuff(buff, buffTime);
                return 0;
            }

            else if (Main.debuff[type])
                return (int)(0.8f * buffTime);

            else 
                return buffTime;
        }
    }
    public class CrateLootMedicalIceBag : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.FrozenCrate || item.type == ItemID.FrozenCrateHard)
            {
                var rule = ItemDropRule.ByCondition(new Conditions.IsExpert(), ModContent.ItemType<MedicalIceBag>(), 5);
                itemLoot.Add(rule);
            }
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