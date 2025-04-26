using Terraria.ID;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TheBindingOfRarria.Content.Items;

namespace TheBindingOfRarria.Content.Tiles;

public class SihilStone : ModTile
{
    public override string Texture => ContentPath + "Tiles/" + Name;

    public override void SetStaticDefaults()
    {
        Main.tilePile[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileFrameImportant[Type] = true;
        Main.tileSpelunker[Type] = true;
        Main.tileOreFinderPriority[Type] = 404;
        Main.tileShine2[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.DrawYOffset = 2;


        LocalizedText name = CreateMapEntryName();
        AddMapEntry(Color.LightSteelBlue, name);
        RegisterItemDrop(ModContent.ItemType<Sihil>());
        DustType = DustID.Ice;

        TileObjectData.addTile(Type);
    }
}