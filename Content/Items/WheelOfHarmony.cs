using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using TheBindingOfRarria.Common.Registries;

namespace TheBindingOfRarria.Content.Items;

/*public class WheelOfHarmony : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;

    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.height = 30;
        Item.width = 30;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<MakoraPlayer>().adaptable = true;
    }
}

public class MakoraPlayer : ModPlayer
{
    public bool adaptable = false;

    public override void ResetEffects() => adaptable = false;
    
    private int hits = 0;

    private (int time, int heal) counter = (30, 0);
    private (bool proj, int ai) adaptationType = (false, 0);

    public override void PostUpdate()
    {
        if (adaptable && hits >= 2)
        {
            if (counter.time > 0)
            {
                counter.time--;
                if (counter.time == 10)
                {
                    SoundStyle sound = Sounds.AdaptedSound;
                    sound.Volume = 0.4f;
                    sound.Pitch = -0.2f;
                    SoundEngine.PlaySound(sound, Player.Center);
                }
            }
            else
            {
                Player.Heal(counter.heal);
                hits = 0;
                counter = (30, 0);
            }
        }
        else
            counter = (20, 0);
    }

    public static void Creak(Player player)
    {
        SoundStyle sound = Sounds.WheelCreak;
        sound.Volume = 0.4f;
        sound.Pitch = -0.6f;
        sound.PitchVariance = 0.1f;
        SoundEngine.PlaySound(sound, player.Center);
    }

    public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
    {
        if (adaptable)
        {
            bool modded = npc.aiStyle == -1;
            if ((adaptationType == (false, npc.aiStyle) && !modded) || (modded && adaptationType == (false, npc.type)))
                hits++;
            else
            {
                hits = 1;
                adaptationType = modded ? (false, npc.type) : (false, npc.aiStyle);
            }

            Creak(Player);
            if (hits >= 2)
                counter.heal += Math.Max(1, hurtInfo.Damage / 2);

        }
    }

    public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
    {
        if (adaptable)
        {
            bool modded = proj.aiStyle == 0;
            if ((adaptationType == (true, proj.aiStyle) && !modded) || (modded && adaptationType == (true, proj.type)))
                hits++;
            else
            {
                hits = 1;
                adaptationType = modded ? (true, proj.type) : (true, proj.aiStyle);
            }

            Creak(Player);
            if (hits >= 2)
                counter.heal += Math.Max(1, hurtInfo.Damage / 2);

        }
    }
}*/