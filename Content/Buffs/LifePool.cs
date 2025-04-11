
namespace TheBindingOfRarria.Content.Buffs;

public class LifePool : ModBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        if (player.GetModPlayer<GeneThiefPlayer>().genePool == 0)
            player.ClearBuff(Type);

        base.Update(player, ref buffIndex);
    }
    public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
    {
        var color = Color.Black;
        color.A = 150;
        spriteBatch.DrawString(FontAssets.MouseText.Value, Main.LocalPlayer.GetModPlayer<GeneThiefPlayer>().genePool.ToString(), (drawParams.Position + drawParams.Texture.Size()), color, 0, drawParams.Texture.Size() / 2 + new Vector2(6f * (Main.LocalPlayer.GetModPlayer<GeneThiefPlayer>().genePool / 50), 4.5f), 0.8f, SpriteEffects.None, 0);
        spriteBatch.DrawString(FontAssets.MouseText.Value, Main.LocalPlayer.GetModPlayer<GeneThiefPlayer>().genePool.ToString(), (drawParams.Position + drawParams.Texture.Size()), Color.White, 0, drawParams.Texture.Size() / 2 + new Vector2(6.5f * (Main.LocalPlayer.GetModPlayer<GeneThiefPlayer>().genePool / 50), 5), 0.7f, SpriteEffects.None, 0);

        base.PostDraw(spriteBatch, buffIndex, drawParams);
    }
}