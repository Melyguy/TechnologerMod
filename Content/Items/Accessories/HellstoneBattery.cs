using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TechnologerMod;
using TechnologerMod.Content.Tiles.Furniture;

namespace TechnologerMod.Content.Items.Accessories;
public class HellstoneBattery : ModItem
{
        private float AdditiveDamageBonus = 10f;
    public override void SetStaticDefaults()
    {

    }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 24;
            Item.accessory = true;
            Item.value = Item.buyPrice(gold: 5);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6; // optional
        }

public override void UpdateAccessory(Player player, bool hideVisual)
{
    ModPlayer modPlayer = player.GetModPlayer<TechnologerPlayer>();
    ((TechnologerPlayer)modPlayer).TinkererGoggles = true;
    ((TechnologerPlayer)modPlayer).MaxFocus += 30;
    player.GetAttackSpeed(DamageClass.Generic)  += AdditiveDamageBonus /100f;
}
        public override void AddRecipes()
        {
        CreateRecipe()
                .AddIngredient<SlimeCell>()
                .AddIngredient(ItemID.Obsidian, 5)
                .AddIngredient(ItemID.HellstoneBar , 5)
                .AddTile<HellPunkerTable>()
                .Register();

        }


}

