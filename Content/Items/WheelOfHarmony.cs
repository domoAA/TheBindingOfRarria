
namespace TheBindingOfRarria.Content.Items
{
    public class WheelOfHarmony : ModItem
    {
        public int Frame = 0;
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.height = 30;
            Item.width = 30;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MakoraPlayer>().adaptable = true;
            player.GetModPlayer<MakoraPlayer>().AltFunc = Frame == 1;
        }
        public override bool CanRightClick() => true;
        public override void RightClick(Player player) => Frame = Frame == 0 ? 1 : 0;
        
        public override bool ConsumeItem(Player player) => false;
        
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            frame = new Rectangle(0, Frame * frame.Height / 2, frame.Width, frame.Height / 2);
            origin = frame.Size() / 2;
            var texture = TextureAssets.Item[Item.type].Value;
            spriteBatch.Draw(texture, position, frame, drawColor, 0, origin, scale * 2, SpriteEffects.None, 0);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Main.GetItemDrawFrame(Item.type, out var texture, out var rect);
            var frame = new Rectangle(0, Frame * rect.Height / 2, rect.Width, rect.Height / 2);
            var origin = frame.Size() / 2;
            spriteBatch.Draw(texture, Item.Bottom - Main.screenPosition - new Vector2(0, origin.Y), frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0);
            return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var index = tooltips.FindIndex(t => t.Name.Contains("Tooltip"));
            if (index == -1)
                return;

            tooltips.RemoveAt(index);
            tooltips.RemoveAt(index);

            var alt = Frame == 0 ? "" : "Alt";
            var line = new TooltipLine(Mod, "Tooltip0", Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.Tooltip{alt}"));

            var change = new TooltipLine(Mod, "Tooltip1", Language.GetTextValue($"Mods.TheBindingOfRarria.Items.{Name}.TooltipChange"));
            change.OverrideColor = Color.LightSkyBlue;

            tooltips.Insert(index, change);
            tooltips.Insert(index, line);
        }
    }
    public class MakoraPlayer : ModPlayer
    {
        public bool adaptable = false;
        public override void ResetEffects() => adaptable = false;
        
        public bool AltFunc = false;
        public int hits = 0;
        public (bool, int) adaptationType = (false, 0);
        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (adaptable && !AltFunc)
            {
                bool modded = npc.aiStyle == -1;
                if ((adaptationType == (false, npc.aiStyle) && !modded) || (modded && adaptationType == (false, npc.type)))
                    hits++;
                else
                {
                    hits = 1;
                    adaptationType = modded ? (false, npc.type) : (false, npc.aiStyle);
                }

                if (adaptable && hits >= 2)
                {
                    hits = 0;
                    Player.Heal(Math.Max(1, hurtInfo.Damage / 2));
                    var sound = TheBindingOfRarria.AdaptedSoundLoud;
                    sound.Volume = 0.2f;
                    sound.Pitch = -0.5f;
                    SoundEngine.PlaySound(sound, Player.Center);
                }
            }
        }
        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (adaptable && !AltFunc)
            {
                bool modded = proj.aiStyle == 0;
                if ((adaptationType == (true, proj.aiStyle) && !modded) || (modded && adaptationType == (true, proj.type)))
                    hits++;
                else
                {
                    hits = 1;
                    adaptationType = modded ? (true, proj.type) : (true, proj.aiStyle);
                }

                if (adaptable && hits >= 2)
                {
                    hits = 0;
                    Player.Heal(Math.Max(1, hurtInfo.Damage / 2));
                    var sound = TheBindingOfRarria.AdaptedSoundLoud;
                    sound.Volume = 0.3f;
                    //sound.Pitch = -0.6f;
                    //sound.PitchVariance = 0.05f;
                    SoundEngine.PlaySound(sound, Player.Center);
                }
            }
        }
        public override bool FreeDodge(Player.HurtInfo info)
        {
            if (adaptable && hits >= 2 && AltFunc)
            {
                Player.immune = true;
                Player.immuneTime = 80;
                hits = 0;
                return true;
            }
            else
                return base.FreeDodge(info);
        }
        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (adaptable && AltFunc)
            {
                bool modded = npc.aiStyle == -1;
                if ((adaptationType == (false, npc.aiStyle) && !modded) || (modded && adaptationType == (false, npc.type)))
                    hits++;
                else
                {
                    hits = 1;
                    adaptationType = modded ? (false, npc.type) : (false, npc.aiStyle);
                }

                if (adaptable && hits >= 2)
                {
                    npc.SimpleStrikeNPC(112, -modifiers.HitDirection, false, modifiers.Knockback.ApplyTo(8), DamageClass.Melee, true, Player.luck);
                    SoundEngine.PlaySound(TheBindingOfRarria.AdaptedSound, Player.Center);
                }
            }
            else if (!adaptable)
                hits = 0;
        }
        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (adaptable && AltFunc)
            {
                bool modded = proj.aiStyle == 0;
                if ((adaptationType == (true, proj.aiStyle) && !modded) || (modded && adaptationType == (true, proj.type)))
                    hits++;
                else
                {
                    hits = 1;
                    adaptationType = modded ? (true, proj.type) : (true, proj.aiStyle);
                }

                if (adaptable && hits >= 2)
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        ModPacket packet = ModContent.GetInstance<TheBindingOfRarria>().GetPacket();
                        packet.Write((int)TheBindingOfRarria.PacketTypes.ProjectileReflect);
                        packet.Write(proj.identity);
                        packet.Send();
                    }
                    else
                        proj.GetReflected();
                    SoundEngine.PlaySound(TheBindingOfRarria.AdaptedSound, Player.Center);
                }
            }
            else if (!adaptable)
                hits = 0;
        }
    }
}