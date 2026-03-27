using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using TechnologerMod.Content.Tiles;
using TechnologerMod.UI;
using Terraria.Audio;

namespace TechnologerMod.Content.Tiles.Furniture
{

    public class AegisTerminal : ModTile
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
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
            public override bool RightClick(int i, int j)
    {
        Player player = Main.LocalPlayer;
        if (player == null)
            return false;
        if (Vector2.Distance(player.Center, new Vector2(i * 16 + 24, j * 16 + 16)) < 100f)
        {
            var uiSystem = ModContent.GetInstance<AegisUISystem>();
            if(uiSystem.AegisInterface.CurrentState == null){
        SoundEngine.PlaySound(SoundID.MenuOpen);
                uiSystem.ShowUI();
            Main.NewText("Aegis Terminal activated!", Color.Cyan);
            Main.NewText("Welcome User " + player.name, Color.Cyan);}
            else
                uiSystem.HideUI();
            return true;
        }
        return false;
    }
    public override void MouseOver(int i, int j)
{
    Player player = Main.LocalPlayer;
    player.cursorItemIconEnabled = true;
    player.cursorItemIconText = "Aegis Terminal";
}
    }

}
