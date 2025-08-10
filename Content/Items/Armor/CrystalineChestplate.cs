using TechnologerMod.Content.Items.Placeable;
using TechnologerMod.Content.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TechnologerMod.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Body value here will result in TML expecting a X_Body.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Body)]
	public class CrystalineChestplate : ModItem
	{
		public static readonly float RoninDamageIncrease = 13f;

		public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(RoninDamageIncrease);

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 19; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
			
			player.GetModPlayer<GlobalPlayer>().TechnologerDamage += RoninDamageIncrease / 100f;// Increase how many minions the player can have by one
		}

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.TitaniumBar, 25)
                .AddIngredient<GemCoreShards>(25)
                .AddTile<HellPunkerTable>()
                .Register();
                            CreateRecipe()
                .AddIngredient(ItemID.AdamantiteBar, 25)
                .AddIngredient<GemCoreShards>(25)
                .AddTile<HellPunkerTable>()
                .Register();
        }
	}
}