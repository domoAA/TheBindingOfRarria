using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Terraria.Chat;
namespace TheBindingOfRarria.Content.Buffs
{
    public class PoisonDoT : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffDoubleApply[ModContent.BuffType<PoisonDoT>()] = true;
            Main.buffNoTimeDisplay[ModContent.BuffType<PoisonDoT>()] = true;
        }
        public int stacks = 0;
        public override void Update(Player player, ref int buffIndex)
        {
            stacks = player.GetModPlayer<DamageOverTtimeUserInterfacePlayer>().poison;
            if (stacks <= 0) {
                player.ClearBuff(ModContent.BuffType<PoisonDoT>());
                return; }
            else
                player.buffTime[buffIndex] = 2;
        }
        public override bool ReApply(Player player, int time, int buffIndex)
        {
            if (player.GetModPlayer<DamageOverTtimeUserInterfacePlayer>().poison < player.GetModPlayer<DamageOverTtimeUserInterfacePlayer>().maxPoison)
                player.GetModPlayer<DamageOverTtimeUserInterfacePlayer>().poison++;

            return base.ReApply(player, time, buffIndex);
        }
        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
        {
            //TextSnippet[] eek = [new TextSnippet(stacks.ToString())];
            //AAAGRGRGRGAR HRIWR AWW I AJTW    ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, eek, drawParams.Position + new Vector2(drawParams.Texture.Width * 0.6f, drawParams.Texture.Height * 0.5f), new Color(Color.White.R, Color.White.G, Color.White.B, 0.2f), Color.Black, 0f, new Vector2(0, 0), new Vector2(1, 1));
            Utils.DrawBorderString(spriteBatch, stacks.ToString(), drawParams.Position + new Vector2(drawParams.Texture.Width * 0.6f, drawParams.Texture.Height * 0.45f), new Color(Color.White.R, Color.White.G, Color.White.B, 0.05f), 0.9f);
            //spriteBatch.DrawString(FontAssets.MouseText.Value, stacks.ToString(), drawParams.Position + new Vector2(drawParams.Texture.Width * 0.55f, drawParams.Texture.Height * 0.4f), Color.Black, 0, new(0, 0), 1f, SpriteEffects.None, 1f);
            //spriteBatch.DrawString(FontAssets.MouseText.Value, stacks.ToString(), drawParams.Position + new Vector2(drawParams.Texture.Width * 0.6f, drawParams.Texture.Height * 0.45f), new(Color.White.R, Color.White.G, Color.White.B, Color.White.A * 0.8f), 0, new(0, 0), 0.8f, SpriteEffects.None, 1f);
        }
    }
    public class FireDoT : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffDoubleApply[ModContent.BuffType<FireDoT>()] = true;
            Main.buffNoTimeDisplay[ModContent.BuffType<FireDoT>()] = true;
        }
        public int stacks = 0;
        public override void Update(Player player, ref int buffIndex)
        {
            stacks = player.GetModPlayer<DamageOverTtimeUserInterfacePlayer>().fire;
            if (stacks <= 0)
            {
                player.ClearBuff(ModContent.BuffType<FireDoT>());
                return;
            }
        }
        public override bool ReApply(Player player, int time, int buffIndex)
        {
            if (player.GetModPlayer<DamageOverTtimeUserInterfacePlayer>().fire < player.GetModPlayer<DamageOverTtimeUserInterfacePlayer>().maxFire)
                player.GetModPlayer<DamageOverTtimeUserInterfacePlayer>().fire++;

            return base.ReApply(player, time, buffIndex);
        }
        public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
        {
            Utils.DrawBorderString(spriteBatch, stacks.ToString(), drawParams.Position + new Vector2(drawParams.Texture.Width * 0.6f, drawParams.Texture.Height * 0.45f), new Color(Color.White.R, Color.White.G, Color.White.B, 0.05f), 0.9f);
        }
    }
    public class DamageOverTtimeUserInterfacePlayer : ModPlayer
    {
        public int poisonCD = 90;
        public int burnCD = 60;
        public int poison = 0;
        public int maxPoison = 9;
        public int fire = 0;
        public int maxFire = 9;
        public static Dictionary<(Player, DamageOverTimeType), int> DamageOverTimeUICollection = [];
        public enum DamageOverTimeType
        {
            Poison,
            Fire
        }
        public override void PostUpdateBuffs()
        {
            if (!Player.HasBuff(ModContent.BuffType<PoisonDoT>()))
            {
                poison = 1;
                poisonCD = 90;
                if (DamageOverTimeUICollection.ContainsKey((Player, DamageOverTimeType.Poison)))
                    DamageOverTimeUICollection.Remove((Player, DamageOverTimeType.Poison));
            }
            else
                UpdateDamageOverTimeCollection(DamageOverTimeUICollection, DamageOverTimeType.Poison, poison);

            poisonCD--;

            if (!Player.HasBuff(ModContent.BuffType<FireDoT>()))
            {
                fire = 1;
                burnCD = 60;
                if (DamageOverTimeUICollection.ContainsKey((Player, DamageOverTimeType.Fire)))
                    DamageOverTimeUICollection.Remove((Player, DamageOverTimeType.Fire));
            }
            else
                UpdateDamageOverTimeCollection(DamageOverTimeUICollection, DamageOverTimeType.Fire, fire);

            burnCD--;
        }
        public void UpdateDamageOverTimeCollection(Dictionary<(Player, DamageOverTimeType), int> dict, DamageOverTimeType type, int stacks)
        {
            if (poison == 0 && dict.ContainsKey((Player, type)))
                dict.Remove((Player, type));
            else if (poison == 0)
                return;
            else if (!dict.ContainsKey((Player, type)))
                dict.Add((Player, type), stacks);
            else
                dict[(Player, type)] = stacks;
        }
        public override void UpdateBadLifeRegen()
        {
            if (poison > 0 && poisonCD <= 0) {
                Player.lifeRegen -= 400;
                poison--;
                poisonCD = 90; }
            if (fire > 0 && burnCD <= 0)
            {
                Player.lifeRegen -= fire * 120;
                burnCD = 60;
            }
        }
    }
}