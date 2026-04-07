using TechnologerMod.Common.Systems;
using TechnologerMod.Content.Items;
using TechnologerMod.Content.Items.Consumables;
using TechnologerMod.Content.Items.Weapons;
using TechnologerMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;
using TechnologerMod.Content.Bosses.Prismatrix.Shards;
using Microsoft.Xna.Framework.Graphics;
using TechnologerMod.Content.Items.Placeable;
using TechnologerMod.Content.Items.Accessories;
using System.Threading;
using TechnologerMod.Content.Items.Placeable.Furniture;
using TechnologerMod.Content.Bosses.AegisDefenseSystem.Drones;
using TechnologerMod.Content.Tiles;


namespace TechnologerMod.Content.Bosses.AegisDefenseSystem;
[AutoloadBossHead]
public class AegisDefenseSystem : ModNPC
{
    
    private const float SpinSpeed = 0.04f;
    private const float ChargeSpeed = 13f;
    private const float IdleSpeed = 7f;
    private const float IdleHeight = -300f; // Increased height
    private const float IdleWidth = 400f;   // Wider figure-8 pattern
    

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.SkeletronHead];
    }
    public bool intro = false;
    public override void SetDefaults()
    {
        NPC.width = 175;
        NPC.height = 175;
        NPC.damage = 40;
        NPC.defense = 50;
        NPC.lifeMax = 15000;
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath3;
			NPC.value = Item.buyPrice(gold: 10);
        NPC.boss = true;
        NPC.aiStyle = -1; // skeletron head style
        Music = MusicID.OtherworldlyWoF;
		NPC.npcSlots = 10f; // Take up open spawn slots, preventing random NPCs from spawning during the fight
    }

        		public override void HitEffect(NPC.HitInfo hit) {
			// If the NPC dies, spawn gore and play a sound
			if (Main.netMode == NetmodeID.Server) {
				// We don't want Mod.Find<ModGore> to run on servers as it will crash because gores are not loaded on servers
				return;
			}

			if (NPC.life <= 0) {
				// These gores work by simply existing as a texture inside any folder which path contains "Gores/"


				var entitySource = NPC.GetSource_Death();

				SoundEngine.PlaySound(SoundID.Roar, NPC.Center);

				// This adds a screen shake (screenshake) similar to Deerclops
				PunchCameraModifier modifier = new PunchCameraModifier(NPC.Center, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2(), 20f, 6f, 20, 1000f, FullName);
				Main.instance.CameraModifiers.Add(modifier);

			}
		}
public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
             Vector2 drawPos = NPC.Center - screenPos;
                // Draw base texture
    spriteBatch.Draw(
        texture,
        drawPos,
        NPC.frame,
        drawColor,
        NPC.rotation,
        origin,
        2f,
        SpriteEffects.None,
        0f
    );
    Texture2D glowTexture = ModContent.Request<Texture2D>("TechnologerMod/Content/Bosses/AegisDefenseSystem/AegisDefenseSystem_Glow").Value;
    spriteBatch.Draw(
        glowTexture,
        drawPos,
        NPC.frame,
        Color.White, // Glow is unaffected by lighting
        NPC.rotation,
        origin,
        2f,
        SpriteEffects.None,
        0f
    );

            
            return false;
        }

    public override void AI()
    {
                            if (intro == false)
        {
            intro = true;
            SoundEngine.PlaySound(SoundID.Item101, NPC.position);
            BossIntroScreen.Show("Aegis Defense System", "\"Defender Of Progress\"", "Boss 5", "ReLogic");
        }
            Player player = Main.player[NPC.target];

    if (!player.active || player.dead)
    {
        NPC.TargetClosest();
        player = Main.player[NPC.target];

        if (!player.active || player.dead)
        {
            NPC.velocity.Y -= 0.2f;
            return;
        }
    }

    Vector2 targetPos = player.Center + new Vector2(0, -300);
    Vector2 move = targetPos - NPC.Center;

    float speed = 8f;

    if (move.Length() > speed)
    {
        move.Normalize();
        move *= speed;
    }

    NPC.velocity = (NPC.velocity * 20f + move) / 21f;

    NPC.rotation = NPC.velocity.X * 0.05f;
                    float timebetweenShots = 120f;
                    float timebetweenSummons = 80f; // Time between shots in frames (2 seconds at 60 FPS)
        NPC.ai[0]++; // Increment the timer
        NPC.ai[1]++; // Increment the timer
    
                    if (Main.rand.NextBool() && NPC.ai[0] > timebetweenShots) // 50% chance energyball
                    {
                        NPC.ai[0] = 0; // Reset the timer

                        Vector2 shootDirection = player.Center - NPC.Center;
                        shootDirection.Normalize();
                        shootDirection *= 10f;

                        int proj = Projectile.NewProjectile(
                            NPC.GetSource_FromAI(),
                            NPC.Center,
                            shootDirection,
                            ModContent.ProjectileType<Content.Projectiles.PrototypeRocket>(),
                            40,
                            1f,
                            Main.myPlayer
                        );

            Main.projectile[proj].hostile = true;
            Main.projectile[proj].friendly = false;
            Main.projectile[proj].owner = 255; // <-- Mark it as NPC-owned (not by a player)
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = -1;

                        SoundEngine.PlaySound(SoundID.NPCHit56, NPC.position);
        }
                            if (Main.rand.NextBool() && NPC.ai[0] > timebetweenShots) // 50% chance energyball
                    {
                        NPC.ai[0] = 0; // Reset the timer
                        
                        SoundEngine.PlaySound(SoundID.NPCHit56, NPC.position);
        }
if (Main.rand.NextBool() && NPC.ai[1] > timebetweenSummons)
{
    NPC.ai[1] = 0; // Reset timer

    SoundEngine.PlaySound(SoundID.NPCHit56, NPC.position);

    if (Main.netMode != NetmodeID.MultiplayerClient)
    {
        NPC.NewNPC(
            NPC.GetSource_FromAI(),
            (int)NPC.Center.X,
            (int)NPC.Center.Y,
            ModContent.NPCType<aegisDrone>() // your minion NPC
        );
    }
}
    }

    public override void OnKill()
    {
        // Spawn the next boss or trigger the next event here
        // For example, you could spawn a new NPC:

            Main.NewText("Aegis Alloy defense mesh deactivated.");

        // New code: Replace ~50% of cobalt ore with AegisOre
        if (Main.netMode != NetmodeID.MultiplayerClient) // Only run on server/single-player
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    if (Main.tile[x, y].TileType == TileID.Copper && Main.rand.NextBool()) // 50% chance to replace
                    {
                        Main.tile[x, y].TileType = (ushort)ModContent.TileType<AegisOrePlaced>();
                        WorldGen.SquareTileFrame(x, y, true); // Update tile visuals
                        if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.SendTileSquare(-1, x, y, 1); // Sync to clients in multiplayer
                        }
                    }
                }
            }
        }
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot) {
			// Do NOT misuse the ModifyNPCLoot and OnKill hooks: the former is only used for registering drops, the latter for everything else

			// The order in which you add loot will appear as such in the Bestiary. To mirror vanilla boss order:
			// 1. Trophy
			// 2. Classic Mode ("not expert")
			// 3. Expert Mode (usually just the treasure bag)
			// 4. Master Mode (relic first, pet last, everything else inbetween)

			// Trophies are spawned with 1/10 chance

			// All the Classic Mode drops here are based on "not expert", meaning we use .OnSuccess() to add them into the rule, which then gets added
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
        notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<VoidGlassItem>(), 1, 30, 45)); // 100% drop chance
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ZuuniteBar>(), 1, 30, 45));
            // Add your new drops here
			notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.SoulofFright, 1, 30, 45)); // 100% drop chance
			
			// Add some materials with different drop chances
			notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.HallowedBar, 1, 30, 45)); // 100% drop chance
            notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<aegisCore>(), 5));
			
			
			// You can also add coins
			notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.GoldCoin, 1, 3, 5)); // Drops 3-5 Gold Coins

			// Notice we use notExpertRule.OnSuccess instead of npcLoot.Add so it only applies in normal mode
			// Boss masks are spawned with 1/7 chance
			//notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ForestGuardianMask>(), 7));

			// This part is not required for a boss and is just showcasing some advanced stuff you can do with drop rules to control how items spawn
			// We make 12-15 ExampleItems spawn randomly in all directions, like the lunar pillar fragments. Hereby we need the DropOneByOne rule,
			// which requires these parameters to be defined
			
			var parameters = new DropOneByOne.Parameters() {
				ChanceNumerator = 1,
				ChanceDenominator = 1,
				MinimumStackPerChunkBase = 1,
				MaximumStackPerChunkBase = 1,
				MinimumItemDropsCount = 1,
				MaximumItemDropsCount = 1,
			};
			

			// Finally add the leading rule
			npcLoot.Add(notExpertRule);

			// Add the treasure bag using ItemDropRule.BossBag (automatically checks for expert mode)
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<HaemorexBag>()));

			// ItemDropRule.MasterModeCommonDrop for the relic

			// ItemDropRule.MasterModeDropOnAllPlayers for the pet
			npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ItemID.SandBlock, 10)); //CHANGE THIS LATER!!!
		}

}
