using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TechnologerMod;
using TechnologerMod.Content.Tiles.Furniture;
using TechnologerMod.Content.Items.Placeable;

namespace TechnologerMod.Content.Items.Accessories;
public class GemCore : ModItem
{
    public override void SetStaticDefaults()
    {

    }

		public static readonly float RoninDamageIncrease = 5f;
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4; // optional
        }

public override void UpdateAccessory(Player player, bool hideVisual)
{
    ModPlayer modPlayer = player.GetModPlayer<TechnologerPlayer>();
    ((TechnologerPlayer)modPlayer).regenamount += 2;
	player.GetModPlayer<GlobalPlayer>().TechnologerDamage += RoninDamageIncrease / 100f;
}


}

