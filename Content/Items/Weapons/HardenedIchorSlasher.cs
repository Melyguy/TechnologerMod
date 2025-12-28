using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using TechnologerMod.Content.Projectiles;
using TechnologerMod;
using TechnologerMod.Content.Tiles.Furniture;
using TechnologerMod.Content.Projectiles;
using TechnologerMod.Content.Items.Placeable;

namespace TechnologerMod.Content.Items.Weapons
{
	/// <summary>
	///     Star Wrath/Starfury style weapon. Spawn projectiles from sky that aim towards mouse.
	///     See Source code for Star Wrath projectile to see how it passes through tiles.
	///     For a detailed sword guide see <see cref="ExampleSword" />
	/// </summary>
	public class HardenedIchorSlasher : ModItem
	{
		// Add this field at class level
		private bool alternateSlash;

		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 28;
			//Item.scale = 0.16f; // Extreme scale down for 500x500 texture
			
			//Item.holdStyle = ItemHoldStyleID.HoldGuitar;
			//Item.noUseGraphic = false;
			Item.useStyle = ItemUseStyleID.Shoot;
			
			Item.useTime = 8; // Balanced attack speed
			Item.useAnimation = 8;
			Item.autoReuse = true;
			
			Item.damage = 50; // Significantly increased base damage
			Item.knockBack = 8;
			Item.crit = 15; // Higher crit chance
			
			Item.UseSound = SoundID.Item73; // More phoenix-like sound
			Item.DamageType = DamageClass.Melee;
			Item.rare = ItemRarityID.Red; // Increased rarity
			
			Item.useTime = 12; // Slightly slower but more powerful
			Item.useAnimation = 12;
			Item.knockBack = 3; // Lower knockback for faster hits
			
			Item.UseSound = SoundID.Item88;
			Item.DamageType = DamageClass.Melee;
			Item.knockBack = 6;

			Item.value = Item.buyPrice(gold: 5);
			Item.rare = ItemRarityID.Orange;

			Item.shoot = ModContent.ProjectileType<blackmortalSlash>(); // Default slash projectile
			Item.shootSpeed = 12f; // Reset to normal speed as we'll multiply in Shoot method
		}
        // This method gets called when firing your weapon/sword.
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
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-20,5); // X=0 for no horizontal offset, Y=-20 to move the hold point up
		}

		// Change the alternateSlash from bool to int to track multiple states
		private int slashCounter = 0;
public override bool CanUseItem(Player player)
{
    var modPlayer = player.GetModPlayer<TechnologerPlayer>();

    // Only allow shooting if the player has enough Focus
    return modPlayer.TinkererGoggles && modPlayer.Focus >= 35;
}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {

			var modPlayer = player.GetModPlayer<TechnologerPlayer>();
            int focusCost = 35; // Example cost
			if (modPlayer.ConsumeFocus(focusCost))
			{
                if (player.altFunctionUse == 2)
                {
                    Projectile.NewProjectile(source, position, velocity, ProjectileID.IceBoomerang, damage, knockback, player.whoAmI);
                    return false;
                }


                else
                {
                    float adjustedItemScale = player.GetAdjustedItemScale(Item) * 1.5f;

                    // Create afterimages

                    // Alternate between slash types
                    int projectileType = ModContent.ProjectileType<PhoenixSlash2>();
                    Vector2 mousePosition = Main.MouseWorld;
                    Vector2 direction = mousePosition - player.MountedCenter;
                    direction.Normalize();
                    // Multiple slashes
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 slashSpeed = direction.RotatedBy(MathHelper.ToRadians(i == 0 ? -10 : 10)) * 24f;
                        Projectile.NewProjectile(source, player.MountedCenter, slashSpeed, projectileType, damage, knockback,
                            player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
                    }

                    // Toggle the slash type for next shot
                    alternateSlash = !alternateSlash;
                }
            }
                return false; // Don't fire the original projectile

		}




		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
		public override void AddRecipes() {
             CreateRecipe()
				.AddIngredient<CrystalineKatana>()
                .AddIngredient<HardenedIchor>(30)
                .AddIngredient<ZuuniteBar>(30)
                .AddTile<ZuuniteAnvil>()
                .Register();
            
		}
	}
}