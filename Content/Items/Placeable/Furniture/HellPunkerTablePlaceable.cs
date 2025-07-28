using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnologerMod.Content.Items.Placeable.Furniture
{
    public class HellPunkerTablePlaceable : ModItem
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
            Item.createTile = ModContent.TileType<Tiles.Furniture.HellPunkerTable>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<EvilInfusedTinkererTablePlaceable>();
            recipe.AddIngredient(ItemID.Hellforge, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
}
