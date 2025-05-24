using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;
using Terraria.Utilities;
using TheBindingOfRarria.Common.Helpers;

namespace TheBindingOfRarria.Common.Systems;

public static class ResizedPlayerUtils
{
    #region Static Methods

    /// <summary>
    ///     Set the player's custom scale.
    /// </summary>
    public static void SetScale(this Player player, float scale)
    {
        if (!NetUtils.IsServer && player.whoAmI != Main.myPlayer)
        {
            // Not allowed
            return;
        }

        var resizedPlayer = player.GetModPlayer<ResizedPlayer>();
        if (Math.Abs(scale - resizedPlayer.Scale) < float.Epsilon)
        {
            // No change
            return;
        }

        resizedPlayer.Scale = Math.Max(scale, 0.05f);
        if (!NetUtils.IsSinglePlayer)
        {
            resizedPlayer.SyncScale();
        }
    }

    /// <summary>
    ///     Reset the player's custom scale.
    /// </summary>
    public static void ResetScale(this Player player)
    {
        SetScale(player, 1);
    }

    /// <summary>
    ///     Get the player's custom scale.
    /// </summary>
    public static float GetScale(this Player player)
    {
        return player.GetModPlayer<ResizedPlayer>().Scale;
    }

    /// <summary>
    ///     Check if the player has a custom scale set.
    /// </summary>
    public static bool GetIsScaled(this Player player)
    {
        return player.GetModPlayer<ResizedPlayer>().IsScaled;
    }

    public static void HandleSync(BinaryReader reader)
    {
        var whoAmI = reader.ReadByte();
        var scale = reader.ReadSingle();
        var resizedPlayer = Main.player[whoAmI].GetModPlayer<ResizedPlayer>();
        resizedPlayer.Scale = scale;
        if (NetUtils.IsServer)
        {
            resizedPlayer.SyncScale(-1, whoAmI);
        }
    }

    #endregion

    #region Nested Types

    // ReSharper disable once ClassNeverInstantiated.Local
    private sealed class ResizedPlayer : ModPlayer
    {
        #region Static Methods

        private static void ResetPlayerSize(Player player)
        {
            player.position = player.BottomLeft;
            player.width = Player.defaultWidth;
            player.height = Player.defaultHeight;
            player.BottomLeft = player.position;
        }

        private static void ApplyPlayerSize(Player player, float scale)
        {
            // https://github.com/NotLe0n/Creativetools/blob/1.4.4/src/Tools/Modify/ModifyPlayer.cs
            player.position = player.BottomLeft;
            player.width = (int)(Player.defaultWidth * scale);
            player.height = (int)(Player.defaultHeight * scale);
            player.BottomLeft = player.position;
        }

        private static void OnDrawPlayer(On_LegacyPlayerRenderer.orig_DrawPlayerInternal orig, LegacyPlayerRenderer self, Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow, float alpha, float scale, bool headOnly)
        {
            if (drawPlayer.TryGetModPlayer(out ResizedPlayer resizedPlayer) && resizedPlayer.IsScaled)
            {
                ResetPlayerSize(drawPlayer);
                ApplyPlayerSize(drawPlayer, resizedPlayer.Scale);
                scale *= resizedPlayer.Scale;
            }

            orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, alpha, scale, headOnly);



            if (drawPlayer.TryGetModPlayer(out ResizedPlayer p) && resizedPlayer.IsScaled && p.OldMountedY != 0 && drawPlayer.MountedCenter.Y != p.OldMountedY)
            {
                drawPlayer.MountedCenter = drawPlayer.MountedCenter with { Y = p.OldMountedY };
                p.OldMountedY = 0;
            }
        }

        #endregion


        public float Scale = 1;

        public float OldMountedY = 0;

        public bool IsScaled => Scale is < 1 or > 1;

        #region Methods

        public override void Load()
        {
            On_LegacyPlayerRenderer.DrawPlayerInternal += OnDrawPlayer;
        }

        public override void Initialize()
        {
            Scale = 1;
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (IsScaled)
            {
                SyncScale(toWho, fromWho);
            }
        }

        public override void PreUpdateMovement()
        {
            ApplyPlayerSize(Player, Scale);
        }

        public override void ResetEffects()
        {
            ResetScale(Player);
        }

        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (IsScaled)
            {
                drawInfo.ItemLocation.Y += Player.defaultHeight * Scale * 0.33f * (Scale > 1 ? 1 : -1);

                OldMountedY = drawInfo.drawPlayer.MountedCenter.Y;
                drawInfo.drawPlayer.MountedCenter += new Vector2(0, Player.defaultHeight * Scale);
            }
        }

        public void SyncScale(int toClient = -1, int ignoreClient = -1)
        {
            var packet = Mod.GetPacket();
            packet.Write((int)TheBindingOfRarria.PacketTypes.SyncResizedPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write(Scale);
            packet.Send(toClient, ignoreClient);
        }

        #endregion
    }

    #endregion
}