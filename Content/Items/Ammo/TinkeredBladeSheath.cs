using TechnologerMod.Content.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnologerMod.Content.Items.Ammo
{
	public class TinkeredBladeSheath : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 99;
		}

		public override void SetDefaults() {
			Item.damage = 12; // The damage for projectiles isn't actually 12, it actually is the damage combined with the projectile and the item together.
			Item.DamageType = DamageClass.Melee;
			Item.width = 8;
			Item.height = 8;
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true; // This marks the item as consumable, making it automatically be consumed when it's used as ammunition, or something else, if possible.
			Item.knockBack = 1.5f;
			Item.value = 10;
			Item.rare = ItemRarityID.Green;
			//Item.shoot = ModContent.ProjectileType<Projectiles.ExampleBullet>(); // The projectile that weapons fire when using this item as ammunition.
			Item.shootSpeed = 4.5f; // The speed of the projectile. This value equivalent to Silver Bullet since ExampleBullet's Projectile.extraUpdates is 1.
			Item.ammo = ModContent.ItemType<TinkeredBladeSheath>();
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
			CreateRecipe(100)
				.AddIngredient(ItemID.IronBar, 1)
				.AddTile<TinkererTable>()
				.Register();
						CreateRecipe(100)
				.AddIngredient(ItemID.LeadBar, 1)
				.AddTile<TinkererTable>()
				.Register();
		}
	}
}