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
public class BlightedCore : ModItem
{
    public override void SetStaticDefaults()
    {

    }

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
    ((TechnologerPlayer)modPlayer).TinkererGoggles = true;
    ((TechnologerPlayer)modPlayer).MaxFocus += 30;
    ((TechnologerPlayer)modPlayer).regenamount += 2;
}
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DemoniteBar, 10)
                .AddIngredient(ItemID.ShadowScale, 15)
                .AddIngredient<EyesOfTheWyrm>(5)
                .AddTile<EvilInfusedTinkererTable>()
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 10)
                .AddIngredient(ItemID.TissueSample, 15)
                .AddIngredient<EyesOfTheWyrm>(5)
                .AddTile<EvilInfusedTinkererTable>()
                .Register();

        }


}

