using System.Collections.Generic;
using System.Linq;
using TechnologerMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TechnologerMod;
using TechnologerMod.Content.Items.Ammo;
using TechnologerMod.Content.Tiles.Furniture;
using Microsoft.Build.Evaluation;

namespace TechnologerMod.Content.Items.Weapons
{
	/// <summary>
	///     Star Wrath/Starfury style weapon. Spawn projectiles from sky that aim towards mouse.
	///     See Source code for Star Wrath projectile to see how it passes through tiles.
	///     For a detailed sword guide see <see cref="ExampleSword" />
	/// </summary>
	public class TinkeredBlade : ModItem
	{
		// Add this field at class level
		private bool alternateSlash;
		public override bool AltFunctionUse(Player player) {
			return true;
		}
		
		public override void SetDefaults()
		{
			Item.width = 100;
			Item.height = 100;

			//Item.holdStyle = ItemHoldStyleID.HoldGuitar;
			//Item.noUseGraphic = false;
			Item.useStyle = ItemUseStyleID.Swing;

			Item.useTime = 20; // Faster attack speed
			Item.useAnimation = 20;
			Item.autoReuse = true;

			Item.damage = 15;
			Item.knockBack = 3; // Lower knockback for faster hits

			Item.UseSound = SoundID.Item60;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 6;
			Item.crit = 6;

			Item.value = Item.buyPrice(platinum: 4);
			Item.rare = ItemRarityID.Pink;

			Item.shoot = ModContent.ProjectileType<blackmortalSlash>(); // Default slash projectile
			Item.shootSpeed = 16f;
			Item.useAmmo = ModContent.ItemType<TinkeredBladeSheath>();
			// If you want melee speed to only affect the swing speed of the weapon and not the shoot speed (not recommended)
			// Item.attackSpeedOnlyAffectsWeaponAnimation = true;

			// Normally shooting a projectile makes the player face the projectile, but if you don't want that (like the beam sword) use this line of code
			// Item.ChangePlayerDirectionOnShoot = false;
		}
        // This method gets called when firing your weapon/sword.

		/*public override Vector2? HoldoutOffset()
		{
			return new Vector2(-20,5); // X=0 for no horizontal offset, Y=-20 to move the hold point up
		}*/
        	public override bool CanUseItem(Player player)
{
	if (player.altFunctionUse == 2) // Right-click
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTime = 10;
		Item.useAnimation = 10;
		Item.UseSound = SoundID.Item60;
		Item.damage = 10;
		Item.shoot = ProjectileID.WoodenBoomerang; // Change to desired alt projectile
		Item.shootSpeed = 12f;
		Item.noMelee = true; // Optional: prevents melee damage if you want a pure projectile
	}
	else // Left-click (normal)
	{
		Item.useStyle = ItemUseStyleID.Swing;
		Item.useTime = 20;
		Item.useAnimation = 20;
		Item.damage = 15;
		Item.shoot = ModContent.ProjectileType<blackmortalSlash>();
		Item.shootSpeed = 16f;
		Item.noMelee = false;
	}
	return base.CanUseItem(player);
}

		
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.IronBar, 10)
                .AddIngredient(ItemID.Wood, 10)
                .AddIngredient(ItemID.StoneBlock, 10)
                .AddTile<TinkererTable>()
                .Register();

        }
		// Add this field at class level
		// Remove the dashCooldown field
		// private int dashCooldown = 0;
       public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var linetochange = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
            if (linetochange != null)
            {
                string[] splittext = linetochange.Text.Split(' ');
                linetochange.Text = splittext.First() + " Technologer " + splittext.Last();
            }
        }
		        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            damage += player.GetModPlayer<GlobalPlayer>().TechnologerDamage;
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(player.altFunctionUse == 2){
				Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBoomerang, damage, knockback, player.whoAmI);
				return false;
			}


			else {
				float adjustedItemScale = player.GetAdjustedItemScale(Item);

				// Create afterimages

				// Alternate between slash types
				int projectileType = ModContent.ProjectileType<blackmortalSlash>();

				// Multiple slashes
				for (int i = 0; i < 2; i++) {
					Vector2 perturbedSpeed = new Vector2(player.direction, 0f).RotatedBy(MathHelper.ToRadians(Main.rand.Next(-15, 15)));
					Projectile.NewProjectile(source, player.MountedCenter, perturbedSpeed, projectileType, damage, knockback,
						player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
				}

				// Toggle the slash type for next shot
				alternateSlash = !alternateSlash;

				return false; // Don't fire the original projectile
			}

		}



		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
	}
}