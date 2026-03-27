using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using System;

namespace TechnologerMod.UI
{
    public class AegisUISystem : ModSystem
    {
        internal UserInterface AegisInterface;
        internal AegisUI AegisMenu;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                AegisMenu = new AegisUI();
                AegisMenu.Activate();

                AegisInterface = new UserInterface();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            AegisInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(System.Collections.Generic.List<GameInterfaceLayer> layers)
        {
            int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (index != -1)
            {
                layers.Insert(index, new LegacyGameInterfaceLayer(
                    "TechnologerMod: Aegis UI",
                    delegate
                    {
                        AegisInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        public void ShowUI()
        {
            AegisInterface?.SetState(AegisMenu);
        }

        public void HideUI()
        {
            AegisInterface?.SetState(null);
        }
    }
}