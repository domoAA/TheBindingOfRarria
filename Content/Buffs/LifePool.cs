using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Buffs
{
    public class LifePool : ModBuff
    {
        public int life = 0;
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.GetModPlayer<GeneThiefPlayer>().genePool == 0)
                player.ClearBuff(ModContent.BuffType<LifePool>());

            life = player.GetModPlayer<GeneThiefPlayer>().genePool;
            base.Update(player, ref buffIndex);
        }
        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
        {
            Utils.DrawBorderString(spriteBatch, life.ToString(), drawParams.Position + drawParams.Texture.Size() * 2 / 3, Color.White);
            base.PostDraw(spriteBatch, buffIndex, drawParams);
        }
    }
}