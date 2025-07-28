using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TechnologerMod.Content.Tiles.Furniture
{

    public class HellPunkerTable : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
        TileObjectData.addTile(Type);
            //TileID.Sets.HasOutlines[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(200, 200, 200), name);

            DustType = DustID.Iron;
            AdjTiles = new int[] {
                ModContent.TileType<TinkererTable>(),
                ModContent.TileType<SolidifiedTinkererTable>(),
                ModContent.TileType<SpringloadedTinkererTable>(),
                ModContent.TileType<EvilInfusedTinkererTable>()

            };
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
