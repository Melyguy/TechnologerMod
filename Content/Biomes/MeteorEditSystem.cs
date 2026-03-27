using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TechnologerMod.Content.Tiles;
using TechnologerMod.Content.Tiles.Furniture;
using Terraria.DataStructures;
using System;


public static class MeteorEditor
{
    private const int METEOR_WIDTH = 30;
    private const int METEOR_HEIGHT = 25;

    private static Random random = new Random();

    public static void ReplaceMeteorTiles()
    {
        // Create the meteorite structure
        CreateMeteorite(Main.spawnTileX + 600);
    }

   public static void CreateMeteorite(int centerX)
{
    int meteorX = centerX;

    // --- FIND REAL GROUND ---
    int groundY = (int)Main.worldSurface;

    while (groundY < Main.maxTilesY)
    {
        Tile tile = Main.tile[meteorX, groundY];

        if (tile.HasTile && Main.tileSolid[tile.TileType])
            break;

        groundY++;
    }

    // --- POSITION METEOR ON SURFACE ---
    // Place meteor so most of it is visible above ground
    int meteorY = groundY; // Center meteor 15 tiles above ground level for maximum visibility

    // Create the meteor body as a crater shape
    int radiusX = METEOR_WIDTH / 2;
    int radiusY = METEOR_HEIGHT / 2;
    double innerRadiusX = radiusX * 0.4; // Inner radius for the crater
    double innerRadiusY = radiusY * 0.4;

    for (int x = meteorX - radiusX; x < meteorX + radiusX; x++)
    {
        for (int y = meteorY - radiusY; y < meteorY + radiusY; y++)
        {
            if (x < 0 || x >= Main.maxTilesX || y < 0 || y >= Main.maxTilesY)
                continue;

            // Use ellipse formula for circular meteor shape
            double distX = (x - meteorX) / (double)radiusX;
            double distY = (y - meteorY) / (double)radiusY;
            double distance = Math.Sqrt(distX * distX + distY * distY);

            // Create crater: tiles only in the outer ring (between inner and outer radius)
            double shatterThreshold = 0.75 + (random.NextDouble() * 0.2);
            bool extraGap = random.NextDouble() < 0.15;

            if (distance <= shatterThreshold && distance >= 0.3 && !extraGap) // Outer ring only
            {
                Tile tile = Main.tile[x, y];
                tile.TileType = (ushort)ModContent.TileType<PhasiumPlaced>();
                tile.HasTile = true;
            }
        }
    }

    // Debris - more scattered and numerous
    for (int i = 0; i < 25; i++) // Increased from 10 to 25
    {
        int debrisX = meteorX + random.Next(-radiusX - 8, radiusX + 8); // More spread out
        int debrisY = meteorY + random.Next(radiusY - 5, radiusY + 12); // More vertical spread

        if (debrisX >= 0 && debrisX < Main.maxTilesX && debrisY >= 0 && debrisY < Main.maxTilesY)
        {
            Tile tile = Main.tile[debrisX, debrisY];
            tile.TileType = (ushort)ModContent.TileType<PhasiumPlaced>();
            tile.HasTile = true;
        }
    }

    // Add some floating debris above the meteor
    for (int i = 0; i < 8; i++)
    {
        int debrisX = meteorX + random.Next(-radiusX - 5, radiusX + 5);
        int debrisY = meteorY + random.Next(-radiusY - 3, -radiusY + 2); // Above the main meteor

        if (debrisX >= 0 && debrisX < Main.maxTilesX && debrisY >= 0 && debrisY < Main.maxTilesY)
        {
            Tile tile = Main.tile[debrisX, debrisY];
            tile.TileType = (ushort)ModContent.TileType<PhasiumPlaced>();
            tile.HasTile = true;
        }
    }

    // Add some cracks/rifts through the meteor for extra destruction
    for (int i = 0; i < 5; i++)
    {
        // Create a random crack line
        int crackStartX = meteorX + random.Next(-radiusX + 2, radiusX - 2);
        int crackStartY = meteorY + random.Next(-radiusY + 2, radiusY - 2);
        int crackLength = random.Next(3, 8);
        int crackDirX = random.Next(-1, 2); // -1, 0, or 1
        int crackDirY = random.Next(-1, 2);

        for (int j = 0; j < crackLength; j++)
        {
            int crackX = crackStartX + (j * crackDirX) + random.Next(-1, 2);
            int crackY = crackStartY + (j * crackDirY) + random.Next(-1, 2);

            if (crackX >= meteorX - radiusX && crackX <= meteorX + radiusX &&
                crackY >= meteorY - radiusY && crackY <= meteorY + radiusY &&
                crackX >= 0 && crackX < Main.maxTilesX && crackY >= 0 && crackY < Main.maxTilesY)
            {
                Main.tile[crackX, crackY].ClearTile();
            }
        }
    }

    // Place station in the meteor midpoint (not below the body)
    int stationY = meteorY;
    PlaceFurnaceAt(meteorX, stationY);
}

    private static void PlaceFurnaceAt(int x, int y)
    {
        if (x < 2 || x >= Main.maxTilesX - 2 || y < 2 || y >= Main.maxTilesY - 2)
            return;

        // AegisTerminal is a 3x2 tile object (Style3x2), place via WorldGen.
        int topLeftX = x - 1;
        int topLeftY = y - 1;

        for (int dx = 0; dx < 3; dx++)
        {
            for (int dy = 0; dy < 2; dy++)
            {
                Main.tile[topLeftX + dx, topLeftY + dy].ClearTile();
            }
        }

        // Add a small platform to support the terminal
        for (int dx = -1; dx <= 1; dx++)
        {
            Tile platform = Main.tile[x + dx, y + 1];
            platform.TileType = (ushort)ModContent.TileType<PhasiumPlaced>();
            platform.HasTile = true;
        }

        bool placed = WorldGen.PlaceObject(topLeftX, topLeftY, ModContent.TileType<AegisTerminal>(), mute: true, style: 0);
        if (!placed)
        {
            for (int dx = 0; dx < 3; dx++)
            {
                for (int dy = 0; dy < 2; dy++)
                {
                    Tile tile = Main.tile[topLeftX + dx, topLeftY + dy];
                    tile.HasTile = true;
                    tile.TileType = (ushort)ModContent.TileType<AegisTerminal>();
                    tile.TileFrameX = (short)(dx * 18);
                    tile.TileFrameY = (short)(dy * 18);
                }
            }
        }
    }
}


