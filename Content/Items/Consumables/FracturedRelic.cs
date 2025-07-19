
using TechnologerMod.Content.Bosses.BlightWyrm;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TechnologerMod.Content.Items.Consumables
{
	// This is the item used to summon a boss, in this case the modded Minion Boss from Example Mod. For vanilla boss summons, see comments in SetStaticDefaults
	public class FracturedRelic : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 3;
			ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.

			// If this would be for a vanilla boss that has no summon item, you would have to include this line here:
			// NPCID.Sets.MPAllowedEnemies[NPCID.Plantera] = true;

			// Otherwise the UseItem code to spawn it will not work in multiplayer
		}

		public override void SetDefaults() {
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 20;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}

		public override void ModifyResearchSorting(ref ContentSamples.CreativeHelper.ItemGroup itemGroup) {
			itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossSpawners;
		}

		public override bool CanUseItem(Player player) {
        bool isInForest = player.ZoneCorrupt || player.ZoneCrimson;
			if (!isInForest) {
				Main.NewText("I can only be summoned in the Evil Biome.", 250, 150, 50);
				return false;
			}
			return !NPC.AnyNPCs(ModContent.NPCType<BlightWyrmHead>()); // Removed extra parenthesis
		}

		public override bool? UseItem(Player player) {
			if (player.whoAmI == Main.myPlayer) {
				// If the player using the item is the client
				// (explicitly excluded serverside here)
				SoundEngine.PlaySound(SoundID.Zombie105, player.position);

				int type = ModContent.NPCType<BlightWyrmHead>();

				if (Main.netMode != NetmodeID.MultiplayerClient) {
					// If the player is not in multiplayer, spawn directly
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else {
					// If the player is in multiplayer, request a spawn
					// This will only work if NPCID.Sets.MPAllowedEnemies[type] is true, which we set in MinionBossBody
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
				}
			}

			return true;
		}

        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.CrimtaneBar, 30)
                .AddIngredient(ItemID.TissueSample, 30)
                .AddTile(TileID.DemonAltar)
                .Register();
			CreateRecipe()
				.AddIngredient(ItemID.DemoniteBar, 30)
				.AddIngredient(ItemID.ShadowScale, 30)
				.AddTile(TileID.DemonAltar)
				.Register();                
		}
	}
}