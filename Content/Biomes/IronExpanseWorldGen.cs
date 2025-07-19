using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using Terraria.IO;
using TechnologerMod.Content.Tiles;
using System;
using TechnologerMod.Content.Tiles.Walls;

namespace TechnologerMod.Content.Biomes
{
    public class IronExpanseWorldGen : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            // Run AFTER everything else so it overwrites vanilla tiles/ores
            int genIndex = tasks.FindIndex(pass => pass.Name.Equals("Final Cleanup"));

            if (genIndex != -1)
            {
                tasks.Insert(genIndex + 1, new PassLegacy("Null Zone Planetoid", delegate (GenerationProgress progress, GameConfiguration config)
                {
                    progress.Message = "Constructing Null Zone Planetoid...";

                    int count = 1; // How many planetoids to generate

                    for (int i = 0; i < count; i++)
                    {
                        int x = WorldGen.genRand.Next(Main.maxTilesX / 4, Main.maxTilesX * 3 / 4);
                        int y = WorldGen.genRand.Next(100, (int)(Main.worldSurface * 0.35));
                        int radius = 45;

                        GeneratePlanetoid(x, y, radius);
                    }
                }));
            }
        }

        private void GeneratePlanetoid(int centerX, int centerY, int radius)
        {
            // First clean any vanilla tiles/ores inside the area
            CleanupVanillaTiles(centerX, centerY, radius);

            int hollowRadius = radius - 10;        // Interior hollow radius
            int glassWallRadius = hollowRadius + 4; // Slightly beyond the hollow radius for walls

            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                for (int y = centerY - radius; y <= centerY + radius; y++)
                {
                    if (!WorldGen.InWorld(x, y)) continue;

                    double dist = Math.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));
                    Tile tile = Main.tile[x, y];

                    if (dist <= radius)
                    {
                        if (dist >= hollowRadius)
                        {
                            if (dist > radius - 3)
                                tile.TileType = (ushort)ModContent.TileType<RustGrass>();
                            else if (dist > radius - 6)
                                tile.TileType = (ushort)ModContent.TileType<RustStone>();
                            else
                                tile.TileType = (ushort)ModContent.TileType<RustStone>();

                            tile.HasTile = true;
                        }
                        else
                        {
                            // HOLLOW INTERIOR
                            tile.HasTile = false;
                            tile.WallType = (ushort)ModContent.WallType<RustStoneWall>();
                        }

                        WorldGen.SquareTileFrame(x, y, true);
                    }
                    else if (dist <= glassWallRadius)
                    {
                        // Glass transition layer
                        tile.HasTile = false;
                        tile.WallType = WallID.Glass;
                    }
                }
            }

            // Place the special ruin room inside
            PlaceCircularGlassRoom(centerX, centerY);

            // Spawn floating debris around the planetoid
            PlaceFloatingChunks(centerX, centerY, radius);

            // Scatter your custom ore after everything else
            ScatterCustomOre(centerX, centerY, radius - 6);
        }

        private void CleanupVanillaTiles(int centerX, int centerY, int radius)
        {
            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                for (int y = centerY - radius; y <= centerY + radius; y++)
                {
                    if (!WorldGen.InWorld(x, y)) continue;

                    double dist = Math.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));

                    if (dist <= radius)
                    {
                        Tile tile = Main.tile[x, y];

                        // Remove grass types
                        if (tile.TileType == TileID.Grass || tile.TileType == TileID.CorruptGrass || tile.TileType == TileID.CrimsonGrass || tile.TileType == TileID.JungleGrass)
                            tile.TileType = TileID.Dirt;

                        // Remove vanilla ores
                        if (tile.TileType == TileID.Copper || tile.TileType == TileID.Tin ||
                            tile.TileType == TileID.Iron || tile.TileType == TileID.Lead ||
                            tile.TileType == TileID.Silver || tile.TileType == TileID.Tungsten ||
                            tile.TileType == TileID.Gold || tile.TileType == TileID.Platinum)
                        {
                            tile.TileType = TileID.Stone;
                        }

                        // Optionally clear out vanilla stone
                        if (tile.TileType == TileID.Stone)
                            tile.HasTile = false;
                    }
                }
            }
        }

        private void ScatterCustomOre(int centerX, int centerY, int radius)
        {
            int oreClusters = WorldGen.genRand.Next(30, 45); // number of ore nodes

            for (int i = 0; i < oreClusters; i++)
            {
                int ox = centerX + WorldGen.genRand.Next(-radius, radius);
                int oy = centerY + WorldGen.genRand.Next(-radius, radius);

                if (!WorldGen.InWorld(ox, oy)) continue;

                double dist = Math.Sqrt((ox - centerX) * (ox - centerX) + (oy - centerY) * (oy - centerY));
                if (dist > radius - 3) continue; // stay inside walls

                Tile tile = Main.tile[ox, oy];
                if (tile.HasTile && tile.TileType == ModContent.TileType<RustStone>())
                {
                    tile.TileType = (ushort)ModContent.TileType<VoidGlass>(); // Replace with your custom ore tile type
                }
            }
        }

        private void PlaceCircularGlassRoom(int centerX, int centerY)
        {
            int radius = 15;

            for (int i = -radius; i <= radius; i++)
            {
                for (int j = -radius; j <= radius; j++)
                {
                    int x = centerX + i;
                    int y = centerY + j;

                    if (!WorldGen.InWorld(x, y)) continue;

                    double distanceSq = i * i + j * j;
                    if (distanceSq <= radius * radius)
                    {
                        Tile tile = Main.tile[x, y];

                        if (distanceSq >= (radius - 3) * (radius - 3))
                        {
                            tile.HasTile = true;
                            tile.TileType = (ushort)ModContent.TileType<RustStone>();
                        }
                        else
                        {
                            tile.HasTile = true;
                            tile.TileType = (ushort)ModContent.TileType<VoidGlass>();
                            tile.WallType = (ushort)ModContent.WallType<RustedCoreWall>();
                        }

                        WorldGen.SquareTileFrame(x, y, true);
                    }
                }
            }
        }

        private void PlaceFloatingChunks(int centerX, int centerY, int mainRadius)
        {
            int chunkCount = WorldGen.genRand.Next(30, 45);

            for (int i = 0; i < chunkCount; i++)
            {
                double angle = WorldGen.genRand.NextDouble() * Math.PI * 2;
                int distance = WorldGen.genRand.Next(mainRadius + 10, mainRadius + 40);

                int chunkX = centerX + (int)(Math.Cos(angle) * distance);
                int chunkY = centerY + (int)(Math.Sin(angle) * distance);
                int chunkRadius = WorldGen.genRand.Next(3, 8);

                PlaceSmallChunk(chunkX, chunkY, chunkRadius);
            }
        }

        private void PlaceSmallChunk(int centerX, int centerY, int radius)
        {
            for (int x = centerX - radius; x <= centerX + radius; x++)
            {
                for (int y = centerY - radius; y <= centerY + radius; y++)
                {
                    if (!WorldGen.InWorld(x, y)) continue;

                    double dist = Math.Sqrt((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY));

                    if (dist <= radius)
                    {
                        Tile tile = Main.tile[x, y];

                        int r = WorldGen.genRand.Next(3);
                        if (r == 0) tile.TileType = (ushort)ModContent.TileType<VoidGlass>();
                        else if (r == 1) tile.TileType = (ushort)ModContent.TileType<PhasiumPlaced>();
                        else tile.TileType = (ushort)ModContent.TileType<RustStone>();

                        tile.HasTile = true;
                        tile.WallType = (ushort)ModContent.WallType<RustedCoreWall>();

                        if (WorldGen.genRand.NextBool(4))

                        WorldGen.SquareTileFrame(x, y, true);
                    }
                }
            }
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                Main.NewText("[Debug] SpiritWorldGen loaded", Color.LightSkyBlue);
            }
        }
    }
}
