using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnologerMod.Content.Items.Placeable.Furniture
{
    public class SolidifiedTinkererTablePlaceable : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 14;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.Furniture.SolidifiedTinkererTable>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<TinkererTablePlaceable>();
            recipe.AddIngredient(ItemID.Solidifier, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
