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
            PixellationSystem.QueuePixelationAction(() => {
                Utils.DrawBorderString(spriteBatch, life.ToString(), (drawParams.Position + drawParams.Texture.Size() / 2) / 2, Color.White, 0.3f);
            }, PixellationSystem.RenderType.Additive);

            base.PostDraw(spriteBatch, buffIndex, drawParams);
        }
    }
}