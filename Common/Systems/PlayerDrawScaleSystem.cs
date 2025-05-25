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

// Thanks davidfdev for allowing me to use most of his implementation, though I did tweak things here and there
// the original that Dave, and by extension myself, based our implementations on: https://github.com/NotLe0n/Creativetools/blob/1.4.4/src/Tools/Modify/ModifyPlayer.cs
public static class ResizedPlayerUtils
{
    #region Static Methods
    public static void ResetPlayerSize(this Player player)
    {
        var p = player.GetModPlayer<ResizedPlayer>();
        player.position = player.BottomLeft;

        player.width = (int)p.OldSize.X;
        player.height = (int)p.OldSize.Y;

        player.BottomLeft = player.position;
    }

    public static void ApplyPlayerSize(this Player player, float scale)
    {
        // https://github.com/NotLe0n/Creativetools/blob/1.4.4/src/Tools/Modify/ModifyPlayer.cs
        var p = player.GetModPlayer<ResizedPlayer>();
        player.position = player.BottomLeft;

        p.OldSize.X = player.width;
        p.OldSize.Y = player.height;

        player.width = (int)(player.width * scale);
        player.height = (int)(player.height * scale);

        player.BottomLeft = player.position;
    }

    /// <summary>
    ///     Set the player's custom scale.
    /// </summary>
    public static void SetScale(this Player player, float scale)
    {
        var resizedPlayer = player.GetModPlayer<ResizedPlayer>();
        if (Math.Abs(scale - resizedPlayer.Scale) < float.Epsilon)
        {
            // No change
            return;
        }

        //player.ResetPlayerSize();
        //player.ApplyPlayerSize(scale);

        resizedPlayer.Scale = Math.Max(scale, 0.05f);
    }

    /// <summary>
    ///     Reset the player's custom scale.
    /// </summary>
    public static void ResetScale(this Player player)
    {
        SetScale(player, 1);
    }

    #endregion


    private sealed class ResizedPlayer : ModPlayer
    {
        #region Static Methods
         
        private static void OnDrawPlayer(On_LegacyPlayerRenderer.orig_DrawPlayerInternal orig, LegacyPlayerRenderer self, Camera camera, Player drawPlayer, Vector2 position, float rotation, Vector2 rotationOrigin, float shadow, float alpha, float scale, bool headOnly)
        {
            if (drawPlayer.TryGetModPlayer(out ResizedPlayer resizedPlayer) && resizedPlayer.IsScaled)
            {
                scale *= resizedPlayer.Scale;
            }

            //drawPlayer.MountedCenter += new Vector2(0, drawPlayer.height / 2);
            //drawPlayer.Center += new Vector2(0, drawPlayer.height);
            drawPlayer.position += new Vector2(0, drawPlayer.height);

            orig(self, camera, drawPlayer, position, rotation, rotationOrigin, shadow, alpha, scale, headOnly);

            //drawPlayer.MountedCenter -= new Vector2(0, drawPlayer.height / 2);
            //drawPlayer.Center -= new Vector2(0, drawPlayer.height);
            drawPlayer.position -= new Vector2(0, drawPlayer.height);
        }

        #endregion

        #region Fields

        public float Scale = 1;

        public Vector2 OldSize = new(Player.defaultWidth, Player.defaultHeight);

        public bool IsScaled => Scale is < 1 or > 1;

        #endregion

        #region Methods

        public override void Load()
        {
            On_LegacyPlayerRenderer.DrawPlayerInternal += OnDrawPlayer;
        }
        public override void Initialize()
        {
            Scale = 1;
            OldSize = new(Player.defaultWidth, Player.defaultHeight);
        }

        public override void PreUpdateMovement()
        {
            if (IsScaled)
            {
                Player.ApplyPlayerSize(Scale);
            }
        }

        public override void PreUpdate()
        {
            if (IsScaled)
            {
                ResetPlayerSize(Player);
                Player.ResetScale();
            }
        }


        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            if (IsScaled)
            {
                drawInfo.ItemLocation.Y += Player.defaultHeight * Scale * 0.33f * (Scale > 1 ? 1 : -1);
            }
        }

        #endregion
    }
}