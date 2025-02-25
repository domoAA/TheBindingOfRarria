using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Buffs
{
    public class HolyProtection : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<ProtectedPlayer>().protection != null)
                player.buffTime[buffIndex] = 2;
        }
    }
}