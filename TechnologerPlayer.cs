using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnologerMod
{
public class TechnologerPlayer : ModPlayer
{
 public int Focus = 0;
    public const int MaxFocus = 100;

    public bool TinkererGoggles = false;
        private int debugTimer;

        public override void ResetEffects()
    {
        TinkererGoggles = false; // Always reset — set again if the player has armor
    }

    public override void PostUpdate()
    {
            Player player = this.Player;

        if (TinkererGoggles)
        {


            Focus = (int)Math.Min(Focus + 0.5f, MaxFocus); // Passive regen only if armor is worn
        }
    }

    public bool ConsumeFocus(int amount)
    {
        if (TinkererGoggles && Focus >= amount)
        {
            Focus -= amount;
            return true;
        }
        return false;
    }

    public override void ModifyDrawInfo(ref Terraria.DataStructures.PlayerDrawSet drawInfo)
    {
        if (TinkererGoggles)
        {
            DrawFocusBar();
        }
    }

    private void DrawFocusBar()
    {
        Vector2 screenPosition = Main.screenPosition;
        Vector2 playerCenter = Player.Center;
        Vector2 barPosition = playerCenter - screenPosition + new Vector2(-20, 40);

        int barWidth = 40;
        int barHeight = 5;

        // Background
        Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)barPosition.X, (int)barPosition.Y, barWidth, barHeight), Color.Gray * 0.6f);
        int fillWidth = (int)((Focus / (float)MaxFocus) * barWidth);
        Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)barPosition.X, (int)barPosition.Y, fillWidth, barHeight), Color.LightBlue);
        
        
    }

   
}
}

