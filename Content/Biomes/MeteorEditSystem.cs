using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TechnologerMod.Content.Tiles;

public static class MeteorEditor
{
    public static void ReplaceMeteorTiles()
    {
        for (int x = 0; x < Main.maxTilesX; x++)
        {
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                if (Main.tile[x, y].TileType == TileID.Meteorite)
                {
                    Main.tile[x, y].TileType = (ushort)ModContent.TileType<PhasiumPlaced>();
                }
            }
        }
        PlaceFurnace(Main.spawnTileX);
    }

public static void PlaceFurnace(int centerX)
{
    for (int y = 100; y < Main.worldSurface; y++)
    {
        if (Main.tile[centerX, y].HasTile &&
            !Main.tile[centerX, y - 1].HasTile)
        {
            // Create a 3-tile platform under the furnace
            for (int i = -1; i <= 1; i++)
            {
                int x = centerX + i;

                if (!Main.tile[x, y].HasTile)
                {
                    WorldGen.PlaceTile(x, y, TileID.Stone, true, true);
                }
            }

            // Clear space above for the furnace
            for (int i = -1; i <= 1; i++)
            {
                int x = centerX + i;

                if (Main.tile[x, y - 1].HasTile)
                {
                    WorldGen.KillTile(x, y - 1);
                }
            }

            // Place the furnace
            WorldGen.PlaceTile(centerX, y - 1, TileID.Furnaces, true, true);

            break;
        }
    }
}
}

