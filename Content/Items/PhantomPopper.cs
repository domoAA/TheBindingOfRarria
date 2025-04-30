using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TheBindingOfRarria.Content.Items;

public class PhantomPopper : ModItem
{
    public override string Texture => ContentPath + "Items/" + Name;
    public override void SetDefaults()
    {
        Item.accessory = true;
        Item.width = 22;
        Item.height = 26;
        Item.rare = ItemRarityID.Yellow;
        Item.value = Item.buyPrice(0, 4, 80, 42);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<PhantomPopperPlayer>().HasPhantomPopper = true;
    }
}

public class PhantomCasterDrop : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.RaggedCaster || npc.type == NPCID.RaggedCasterOpenCoat)
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhantomPopper>(), 17));

        base.ModifyNPCLoot(npc, npcLoot);
    }
}

public class PhantomPopperPlayer : ModPlayer
{
    public bool HasPhantomPopper = false;

    public override void ResetEffects()
    {
        HasPhantomPopper = false;
    }
}

public class PhantomPopperGlobalProjectile : GlobalProjectile
{
    public int counter = -1;
    public bool ASoul = false;

    public override bool InstancePerEntity => true;

    public override bool AppliesToEntity(Projectile entity, bool lateInstantiation)
    {
        return entity.DamageType == DamageClass.Magic && entity.friendly;
    }

    public override void OnSpawn(Projectile projectile, IEntitySource source)
    {
        if (source != null && source.Context != null && source.Context == "Phantom Popper accessory")
        {
            projectile.penetrate = 1;
            projectile.timeLeft = 120;
            projectile.scale *= 0.5f;
            ASoul = true;
        }
        else if (Main.player[projectile.owner].GetModPlayer<PhantomPopperPlayer>().HasPhantomPopper)
        {
            counter = 180;
        }
    }

    public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
    {
        bitWriter.WriteBit(ASoul);

        if (ASoul)
        {
            binaryWriter.Write(projectile.penetrate);
            binaryWriter.Write(projectile.timeLeft);
            binaryWriter.Write(projectile.scale);
        }
    }

    public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
    {
        var flag = bitReader.ReadBit();

        if (!ASoul)
            ASoul = flag;

        if (ASoul)
        {
            projectile.penetrate = binaryReader.ReadInt32();
            projectile.timeLeft = binaryReader.ReadInt32();
            projectile.scale = binaryReader.ReadSingle();
        }
    }

    public override void PostAI(Projectile projectile)
    {
        if (projectile.owner == Main.myPlayer && !ASoul && counter != -1)
        {
            counter--;

            Player owner = Main.player[projectile.owner];

            if (owner.GetModPlayer<PhantomPopperPlayer>().HasPhantomPopper)
            {
                if (counter <= 10)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 velocity = new Vector2(2).RotatedByRandom(TwoPi);
                        int proj = Projectile.NewProjectile(
                            owner.GetSource_FromThis("Phantom Popper accessory"),
                            projectile.Center,
                            velocity,
                            ProjectileID.LostSoulFriendly,
                            projectile.damage / 3,
                            projectile.knockBack,
                            owner.whoAmI
                        );
                    }
                    projectile.Kill();
                }
            }
        }
    }
}
