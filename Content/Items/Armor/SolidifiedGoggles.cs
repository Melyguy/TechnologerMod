using TechnologerMod.Content.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace TechnologerMod.Content.Items.Armor
{
	// The AutoloadEquip attribute automatically attaches an equip texture to this item.
	// Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
	[AutoloadEquip(EquipType.Head)]
	public class SolidifiedGoggles : ModItem
	{
		public static readonly int AdditiveGenericDamageBonus = 40;
		public static readonly float RoninDamageIncrease = 7f;
		public static LocalizedText SetBonusText { get; private set; }
		public static LocalizedText DMGText { get; private set; }

		public override void SetStaticDefaults() {
			// If your head equipment should draw hair while drawn, use one of the following:
			// ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
			// ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
			ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
			// ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true;
            DMGText = this.GetLocalization("Tooltip").WithFormatArgs(RoninDamageIncrease);
			SetBonusText = this.GetLocalization("SetBonus").WithFormatArgs(AdditiveGenericDamageBonus);
		}

		public override void SetDefaults() {
			Item.width = 18; // Width of the item
			Item.height = 18; // Height of the item
			Item.value = Item.sellPrice(gold: 10); // How many coins the item is worth
			Item.rare = ItemRarityID.Green; // The rarity of the item
			Item.defense = 5; // The amount of defense the item will give when equipped
		}

		public override void UpdateEquip(Player player) {
			player.GetModPlayer<GlobalPlayer>().TechnologerDamage += RoninDamageIncrease / 100f;

		}

		// IsArmorSet determines what armor pieces are needed for the setbonus to take effect
		public override bool IsArmorSet(Item head, Item body, Item legs) {
			return body.type == ModContent.ItemType<SolidifiedExoArmor>() && legs.type == ModContent.ItemType<SolidifiedLeggings>();
		}

		// UpdateArmorSet allows you to give set bonuses to the armor.
		public override void UpdateArmorSet(Player player) {
			player.setBonus = SetBonusText.Value; // This is the setbonus tooltip: "Increases dealt damage by 20%"
			//player.GetModPlayer<GlobalPlayer>().TechnologerDamage += RoninDamageIncrease / 100f; // Increase dealt damage for all weapon classes by 20%
                ModPlayer modPlayer = player.GetModPlayer<TechnologerPlayer>();
                ((TechnologerPlayer)modPlayer).TinkererGoggles = true;
                ((TechnologerPlayer)modPlayer).MaxFocus += AdditiveGenericDamageBonus;
		}

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.PlatinumBar, 10)
                .AddIngredient(ItemID.Gel, 15)
                .AddIngredient<TinkeredGoggles>()
                .AddTile<SolidifiedTinkererTable>()
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.GoldBar, 10)
                .AddIngredient(ItemID.Gel, 15)
                .AddIngredient<TinkeredGoggles>()
                .AddTile<SolidifiedTinkererTable>()
                .Register();
        }
	}
}