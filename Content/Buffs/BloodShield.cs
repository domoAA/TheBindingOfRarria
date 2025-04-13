using Terraria;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Buffs;

public class BloodShield : ModBuff
{
    public override string Texture => ContentPath + "Buffs/" + Name;

        // public override void Update(Player player, ref int buffIndex)
        // {
        //         // player.statDefense += (int)player.GetModPlayer<KamoPlayer>().Stored;
        // }
}
