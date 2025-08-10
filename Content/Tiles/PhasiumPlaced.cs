using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TechnologerMod.Content.Projectiles;
using TechnologerMod.Content.Items;
using Terraria.Localization;

namespace TechnologerMod.Content.Tiles;

public class PhasiumPlaced
 : ModTile
{
    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;
        DustType = DustID.BrownMoss;
        AddMapEntry(new Color(247, 182, 255));
        HitSound = SoundID.Tink;
        // Optionally: SoundType, drop item, etc.
        RegisterItemDrop(ItemID.StoneBlock);
        MineResist = 4f; // Higher = harder to mine (optional)
        MinPick = 200;   // 200 pickaxe power = Pickaxe Axe or better
    }
}
