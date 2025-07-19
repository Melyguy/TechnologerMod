using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechnologerMod.Content.Projectiles;
using TechnologerMod.Content.Items;
using Terraria.Localization;
using TechnologerMod.Content.Tiles;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using TechnologerMod.Content.Tiles.Walls;

namespace TechnologerMod.Content.Biomes;


public class IronExpanseBiome : ModBiome
{
    // Keep high priority
    public override SceneEffectPriority Priority => SceneEffectPriority.BiomeHigh;

    // Add these required overrides
    public override int Music => MusicID.OtherworldlySnow ; // Temporary until you have custom music
    
    // Specify biome depth for proper detection
    public override ModWaterStyle WaterStyle => null;
    public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => null;

    public override bool IsBiomeActive(Player player)
    {       
        // Increase detection range and reduce required tiles
        int sakuraTileCount = 0;
        Point playerCenter = player.Center.ToTileCoordinates();

        // Check a larger area (60x60 tiles)
        for (int x = playerCenter.X - 30; x < playerCenter.X + 30; x++)
        {
            for (int y = playerCenter.Y - 30; y < playerCenter.Y + 30; y++)
            {
                if (WorldGen.InWorld(x, y))
                {
                    Tile tile = Main.tile[x, y];
                    if (tile.HasTile && tile.TileType == ModContent.TileType<RustStone>() || tile.TileType == ModContent.TileType<RustGrass>() || tile.TileType == ModContent.TileType<RustStoneBrick>() || tile.TileType == ModContent.TileType<RustGrass>() || tile.TileType == ModContent.WallType<RustStoneWall>() || tile.TileType == ModContent.WallType<RustedCoreWall>()  )
                        sakuraTileCount++;
                }
            }
        }
        
        // Reduce required tiles to make detection easier
        // Only active above ground
        bool isAboveSurface = player.position.Y / 16f <= Main.worldSurface + 30;
        return sakuraTileCount >= 100 && isAboveSurface;
    }

public override void SpecialVisuals(Player player, bool isActive)
{
    if (isActive)
    {
        // Add a faint dark-purple tint to the screen
        Lighting.AddLight(player.Center, new Vector3(0.05f, 0.02f, 0.08f));
    }
}

}
