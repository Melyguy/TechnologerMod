using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using TechnologerMod;
using TechnologerMod.Content.Tiles.Furniture;
using TechnologerMod.Content.Items.Placeable;

namespace TechnologerMod.Content.Items.Accessories;
public class IchorBattery : ModItem
{
        private float AdditiveDamageBonus = 15f;
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
    ((TechnologerPlayer)modPlayer).MaxFocus += 60;
    player.GetAttackSpeed(DamageClass.Generic)  += AdditiveDamageBonus /100f;
}
        public override void AddRecipes()
        {
        CreateRecipe()
                .AddIngredient<HellstoneBattery>()
                .AddIngredient<HardenedIchor>(10)
                .AddTile<ZuuniteAnvil>()
                .Register();

        }


}

