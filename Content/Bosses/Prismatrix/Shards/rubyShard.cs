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
using TechnologerMod.Content.Bosses.Prismatrix;


namespace TechnologerMod.Content.Bosses.Prismatrix.Shards;
public class rubyShard : ModNPC
{
    private const int AttackCooldown = 180;
    private const int AimDuration = 30;
    private const float OrbitDistance = 120f;
    private const float TornadoOffset = 100f; // Distance to place tornados from player
    
    // AI States
    public ref float AttackTimer => ref NPC.ai[2];
    public ref float IsAttacking => ref NPC.ai[3];

    public override void SetDefaults()
    {
        NPC.width = 30;
        NPC.height = 30;
        NPC.damage = 30;
        NPC.defense = 5;
        NPC.lifeMax = 2000;
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath3;
        NPC.aiStyle = -1;
    }

    public override void AI()
    {
        int headIndex = (int)NPC.ai[0];
        if (!Main.npc[headIndex].active || Main.npc[headIndex].type != ModContent.NPCType<PrismatrixHead>())
        {
            NPC.active = false;
            return;
        }

        Player player = Main.player[Main.npc[headIndex].target];
        Vector2 toPlayer = player.Center - NPC.Center;
        
        AttackTimer++;
        
        if (AttackTimer >= AttackCooldown)
        {
            // Reset timer and start attack
            AttackTimer = 0;
            IsAttacking = 1f;
            
            // Telegraph the attack with a sound
            SoundEngine.PlaySound(SoundID.Item8, NPC.Center);
        }

        if (IsAttacking == 1f)
        {
            if (AttackTimer == AimDuration)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    // Calculate offset based on which hand this is (left or right)
                    float sideOffset = NPC.ai[1] * TornadoOffset; // Uses the hand's side indicator (-1 or 1)
                    
                    // Calculate position beside the player
                    Vector2 targetPos = player.Center + new Vector2(sideOffset, 0);
                    Vector2 shootVelocity = (targetPos - NPC.Center).SafeNormalize(Vector2.Zero) * 8f;
                    
                    int damage = NPC.damage / 2;
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, shootVelocity, 
                        ProjectileID.MartianWalkerLaser, damage, 2f, Main.myPlayer);
                    
                    SoundEngine.PlaySound(SoundID.Item60, NPC.Center);
                }
                IsAttacking = 0f;
            }
            else
            {
                // Aim at the target position beside the player
                float sideOffset = NPC.ai[1] * TornadoOffset;
                Vector2 targetPos = player.Center + new Vector2(sideOffset, 0);
                NPC.rotation = (targetPos - NPC.Center).ToRotation() + MathHelper.PiOver2;
                NPC.velocity *= 0.9f;
            }
        }
        else
        {
// Maintain a rotating angle around the boss
ref float orbitAngle = ref NPC.localAI[0]; // Store angle in localAI
orbitAngle += 0.03f * NPC.ai[1]; // Direction based on side (-1 or 1)

Vector2 center = Main.npc[headIndex].Center;
Vector2 offset = OrbitDistance * new Vector2((float)Math.Cos(orbitAngle), (float)Math.Sin(orbitAngle));
Vector2 desiredPosition = center + offset;

// Move smoothly toward the orbit position
Vector2 moveTo = desiredPosition - NPC.Center;
NPC.velocity = moveTo * 0.1f;

// Face movement direction
if (NPC.velocity.LengthSquared() > 0.01f)
    NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;

        }

        // Update rotation to face movement or aim direction
        if (IsAttacking == 1f && AttackTimer < AimDuration)
        {
            // Face the player while aiming
            NPC.rotation = toPlayer.ToRotation() + MathHelper.PiOver2;
        }
        else if (NPC.velocity != Vector2.Zero)
        {
            // Face movement direction
            NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}
