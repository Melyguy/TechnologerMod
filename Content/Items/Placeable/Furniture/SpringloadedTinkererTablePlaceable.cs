using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnologerMod.Content.Items.Placeable.Furniture
{
    public class SpringloadedTinkererTablePlaceable : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.Furniture.SpringloadedTinkererTable>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<SolidifiedTinkererTablePlaceable>();
            recipe.AddIngredient(ItemID.Chain, 100);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
