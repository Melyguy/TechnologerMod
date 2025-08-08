using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

public class BossIntroScreen : ModSystem
{
    private static int timer = 0;
    private static string bossName;
    private static string subtitle;
    private static string songTitle;
    private static string artist;
    private static bool active = false;

    public static void Show(string name, string quote, string song, string musicArtist)
    {
        bossName = name;
        subtitle = quote;
        songTitle = song;
        artist = musicArtist;
        timer = 180;
        active = true;
    }

    public override void PostDrawInterface(SpriteBatch spriteBatch)
    {
        if (!active || timer <= 0)
            return;

        float opacity = MathHelper.Clamp(timer / 60f, 0f, 1f);

        var screenCenter = new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f);
        var font = FontAssets.DeathText.Value;

        // Black slash effect (top and bottom)
        Texture2D blackBar = ModContent.Request<Texture2D>("TechnologerMod/Common/Systems/blackBar").Value;
        spriteBatch.Draw(blackBar, new Rectangle(0, 0, Main.screenWidth, 100), Color.Black * opacity);
        spriteBatch.Draw(blackBar, new Rectangle(0, Main.screenHeight - 100, Main.screenWidth, 100), Color.Black * opacity);

        // Subtitle
        Utils.DrawBorderStringBig(spriteBatch, subtitle, screenCenter - new Vector2(0, 80), Color.Orange * opacity, 0.8f, 0.5f, 0.5f);

        // Boss Name
        Utils.DrawBorderStringBig(spriteBatch, bossName, screenCenter, Color.Cyan * opacity, 1.5f, 0.5f, 0.5f);

        // Music Title
        string musicText = $"♪ {songTitle} - {artist}";
        Vector2 textSize = font.MeasureString(musicText);
        spriteBatch.DrawString(font, musicText, new Vector2(Main.screenWidth - textSize.X - 20, 20), Color.White * opacity);

        timer--;
        if (timer <= 0)
            active = false;
    }
}
