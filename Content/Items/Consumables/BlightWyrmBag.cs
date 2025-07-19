using TechnologerMod.Content.Bosses.BlightWyrm;
using TechnologerMod.Content.Bosses.PrototypeZR1;
using TechnologerMod.Content.Items.Armor;
using TechnologerMod.Content.Items.Placeable;
using TechnologerMod.Content.Items.Placeable.Furniture;
using TechnologerMod.Content.Items.Weapons;
using TechnologerMod.Content.Tiles.Furniture;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnologerMod.Content.Items.Consumables
{
	// Basic code for a boss treasure bag
	public class BlightWyrmBag : ModItem
	{
		public override void SetStaticDefaults() {
			// This set is one that every boss bag should have.
			// It will create a glowing effect around the item when dropped in the world.
			// It will also let our boss bag drop dev armor..
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true; // ..But this set ensures that dev armor will only be dropped on special world seeds, since that's the behavior of pre-hardmode boss bags.

			Item.ResearchUnlockCount = 3;
		}

		public override void SetDefaults() {
			Item.maxStack = Item.CommonMaxStack;
			Item.consumable = true;
			Item.width = 24;
			Item.height = 24;
			Item.rare = ItemRarityID.Purple;
			Item.expert = true; // This makes sure that "Expert" displays in the tooltip and the item name color changes
		}

		public override bool CanRightClick() {
			return true;
		}

		public override void ModifyItemLoot(ItemLoot itemLoot) {
			// Guaranteed expert drops
			itemLoot.Add(ItemDropRule.Common(ItemID.CrimtaneBar, 1, 30, 45));
			itemLoot.Add(ItemDropRule.Common(ItemID.DemoniteBar, 1, 30, 45));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyesOfTheWyrm>(), 1, 30, 45));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeOfEvil>(), 1));
			
			// Chance-based drops (33% chance each)
			
			// Wood drops (5-15 pieces, guaranteed)
			
			// Add coins based on NPC value
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<BlightWyrmHead>()));
		}
	}
}