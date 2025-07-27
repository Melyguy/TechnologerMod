using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TechnologerMod.Content.Items.Placeable;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnologerMod.Content.Items.Weapons
{
	public class BlightedEyeStaff : ModItem
	{
		public override void SetDefaults() {
			// DefaultToStaff handles setting various Item values that magic staff weapons use.
			// Hover over DefaultToStaff in Visual Studio to read the documentation!
			// Shoot a black bolt, also known as the projectile shot from the onyx blaster.
			Item.DefaultToStaff(ModContent.ProjectileType<Projectiles.BlightedEye>(), 7, 15, 10);
			Item.width = 34;
			Item.height = 40;
			Item.UseSound = SoundID.NPCHit55;

			// A special method that sets the damage, knockback, and bonus critical strike chance.
			// This weapon has a crit of 32% which is added to the players default crit chance of 4%
			Item.SetWeaponValues(30, 6, 32);

			Item.SetShopValues(ItemRarityColor.LightRed4, 10000);
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-5, 0); // Adjusted X to hold more towards the blade
		}
        public override void AddRecipes()
        {
        CreateRecipe()
            .AddIngredient(ItemID.DemoniteBar, 10)
            .AddIngredient<EyesOfTheWyrm>(20)
            .AddTile(TileID.Anvils)
            .Register();
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 10)
                .AddIngredient<EyesOfTheWyrm>(20)
                .AddTile(TileID.Anvils)
                .Register();
		}
		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
        {
            // We can use ModifyManaCost to dynamically adjust the mana cost of this item, similar to how Space Gun works with the Meteor armor set.
            // See ExampleHood to see how accessories give the reduce mana cost effect.
            if (player.statLife < player.statLifeMax2 / 2)
            {
                mult *= 0.5f; // Half the mana cost when at low health. Make sure to use multiplication with the mult parameter.
            }
        }
	}
}