using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TechnologerMod.UI
{
    public class AegisUI : UIState
    {
        public UIPanel panel;

        public override void OnInitialize()
        {
            panel = new UIPanel();
            panel.Width.Set(1000, 0f);
            panel.Height.Set(500, 0f);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;

            UIText text = new UIText("Aegis Terminal");
            text.HAlign = 0.5f;
            text.Top.Set(10, 0f);

            panel.Append(text);

            Append(panel);
        }
    }
}