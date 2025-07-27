using System;
using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnologerMod
{
public class TechnologerPlayer : ModPlayer
{
 public int Focus = 0;
    public int MaxFocus = 100;

    public bool TinkererGoggles = false;
        private int debugTimer;
        public int regenamount = 1;

        public override void ResetEffects()
        {
            TinkererGoggles = false; // Always reset — set again if the player has armor
            MaxFocus = 100;
            regenamount = 1;
        }

    public override void PostUpdate()
    {
            Player player = this.Player;

        if (TinkererGoggles)
        {


            Focus = Math.Min(Focus + 1, MaxFocus); // Passive regen only if armor is worn
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

    // Draw background
    Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)barPosition.X, (int)barPosition.Y, barWidth, barHeight), Color.Gray * 0.6f);

    // Calculate fill width
    int fillWidth = (int)MathHelper.Clamp((Focus / (float)MaxFocus) * barWidth, 0, barWidth);

    // Draw fill
    if (fillWidth > 0)
    {
        Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)barPosition.X, (int)barPosition.Y, fillWidth, barHeight), Color.LightBlue);
    }

    // Draw text 
    /*
    string focusText = $"Focus: {Focus} / {MaxFocus}";
    Vector2 textSize = Terraria.GameContent.FontAssets.MouseText.Value.MeasureString(focusText);
    Vector2 textPosition = new Vector2(barPosition.X + (barWidth / 2) - (textSize.X / 2), barPosition.Y + barHeight + 2);

    Main.spriteBatch.DrawString(Terraria.GameContent.FontAssets.MouseText.Value, focusText, textPosition, Color.White);*/
}



   
}
}

