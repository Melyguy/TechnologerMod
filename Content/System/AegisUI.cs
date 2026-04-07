using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using Terraria.GameInput;
using TechnologerMod.Content.Bosses.AegisDefenseSystem;

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

            UIText text = new UIText("Aegis Terminal: User Login Required");
            text.HAlign = 0.5f;
            text.Top.Set(10, 0f);

            panel.Append(text);

            // Close button
            UITextPanel<string> closeButton = new UITextPanel<string>("X");
            closeButton.Width.Set(30, 0f);
            closeButton.Height.Set(30, 0f);
            closeButton.Left.Set(-35, 1f);
            closeButton.Top.Set(5, 0f);
            closeButton.OnLeftClick += (evt, element) => ModContent.GetInstance<AegisUISystem>().HideUI();
            panel.Append(closeButton);

            // Dialogue button 2
            UITextPanel<string> dialogueButton2 = new UITextPanel<string>("Combat Login");
            dialogueButton2.Width.Set(150, 0f);
            dialogueButton2.Height.Set(40, 0f);
            dialogueButton2.HAlign = 0.7f;
            dialogueButton2.Top.Set(60, 0f);
            dialogueButton2.OnLeftClick += (evt, element) => {
                int bossType = ModContent.NPCType<AegisDefenseSystem>();
                NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, bossType);
                ModContent.GetInstance<AegisUISystem>().HideUI();
            };
            panel.Append(dialogueButton2);

            Append(panel);
        }
    }
}