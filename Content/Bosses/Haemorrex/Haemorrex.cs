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
using System.Linq;

namespace TechnologerMod.Content.Bosses.Haemorrex;
[AutoloadBossHead]
public class Haemorrex : ModNPC
{
    private bool secondPhase = false;
    public static int HaemorrexMusicSlot;

    public override void SetDefaults()
    {
        NPC.aiStyle = -1; 
        AIType = NPCID.Retinazer;
        NPC.width = 110;
        NPC.height = 110;
        NPC.damage = 80;
        NPC.defense = 10;
        NPC.lifeMax = 10000;
        NPC.knockBackResist = 0f;
        NPC.noGravity = true;
        NPC.noTileCollide = true;
        NPC.HitSound = SoundID.NPCHit4;
        NPC.DeathSound = SoundID.ScaryScream;
        NPC.value = Item.buyPrice(gold: 10);
        NPC.boss = true;
        //HaemorrexMusicSlot = MusicLoader.AddMusic(this, "Content/Sounds/Music/PlaceHolder");
        
        // ...other settings like life, damage, etc.
    }
    public override void SetStaticDefaults()
    {
			Main.npcFrameCount[Type] = 6;
    }
    public override void AI()
    {
        if (!secondPhase) {
            WormMovement();
        }
// Always target the closest player
NPC.TargetClosest(false);

Player target = Main.player[NPC.target];

// If the player is dead or inactive, disengage
if (!target.active || target.dead)
{
    NPC.velocity.Y -= 0.1f; // Float upward
    NPC.dontTakeDamage = true;

    if (NPC.timeLeft > 10)
        NPC.timeLeft = 10;

    if (Main.netMode != NetmodeID.MultiplayerClient)
    {
    }

    return;
}

        // Second phase trigger


        // NPC.ai[0] = phase tracker
        // NPC.ai[1] = transition timer
        // 0 = phase 1, 1 = transitioning, 2 = phase 2
            NPC.localAI[1]++;
        if (NPC.localAI[1] >= 180 && Main.netMode != NetmodeID.MultiplayerClient && !secondPhase)
        {
            NPC.localAI[1] = 0;

            if (target != null && target.active && !target.dead)
            {
                if (Main.rand.NextBool()) // 50% chance energyball
                {
                    Vector2 shootDirection = target.Center - NPC.Center;
                    shootDirection.Normalize();
                    shootDirection *= 10f;

                    int proj = Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        shootDirection,
                        ProjectileID.GoldenShowerHostile,
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
                else
                {
                    
                }
            }
        }
        if (NPC.ai[0] == 0 && NPC.life <= NPC.lifeMax / 2)
            {
                NPC.ai[0] = 1; // enter transition
                NPC.ai[1] = 0; // reset timer
                NPC.netUpdate = true;
                // Play sound, spawn particles, etc.
                return;
            }

        if (NPC.ai[0] == 1)
        {
            PhaseTransitionSpin();
            return; // Skip rest of AI
        }
        
        // Bobbing movement code
        if (secondPhase)
        {
        float time = Main.GameUpdateCount / 60f;
        float sinOffset = (float)Math.Sin(time * 2f) * 150f;
        Vector2 abovePlayer = target.Top + new Vector2(sinOffset, -NPC.height * 2f);
        Vector2 toAbovePlayer = abovePlayer - NPC.Center;
        Vector2 toAbovePlayerNormalized = toAbovePlayer.SafeNormalize(Vector2.UnitY);

        float speed = NPC.Top.Y > target.Bottom.Y ? 12f : 8f;
        float inertia = 40f;

        Vector2 moveTo = toAbovePlayerNormalized * speed;
        NPC.velocity = (NPC.velocity * (inertia - 1) + moveTo) / inertia;

        NPC.rotation = (target.Center - NPC.Center).ToRotation() - MathHelper.PiOver2;
        }

    }
 private void WormMovement()
        {
            Player player = Main.player[NPC.target];
            NPC.TargetClosest();

            float flySpeed = 15f; // Base movement speed
            float turnSpeed = 0.02f; // How quickly it turns (lower = wider turns)

            Vector2 toPlayer = player.Center - NPC.Center;

            if (toPlayer.LengthSquared() < 20f * 20f)
            {
                toPlayer = NPC.velocity.SafeNormalize(Vector2.UnitY);
            }
            else
            {
                toPlayer.Normalize();
            }

            Vector2 desiredVelocity = toPlayer * flySpeed;
            NPC.velocity = Vector2.Lerp(NPC.velocity, desiredVelocity, turnSpeed);
            NPC.rotation = toPlayer.ToRotation() - MathHelper.PiOver2;
            float time = Main.GameUpdateCount * 0.1f;
            float wave = (float)Math.Sin(time + NPC.whoAmI) * 0.3f;
            Vector2 waveOffset = NPC.velocity.RotatedBy(MathHelper.PiOver2) * wave;
            NPC.velocity += waveOffset * 0.05f;

            if (Main.rand.NextBool(300))
            {
                Vector2 chargeDir = (player.Center - NPC.Center).SafeNormalize(Vector2.Zero) * 40f;
                NPC.velocity = chargeDir;
                SoundEngine.PlaySound(SoundID.Item92, NPC.position);
            }
        }


    private void PhaseTransitionSpin()
    {
        NPC.velocity *= 0.9f; // slow to a stop
        NPC.rotation += 0.6f * NPC.direction; // spin!
        NPC.dontTakeDamage = true;
        NPC.ai[1]++; // count frames

        if (NPC.ai[1] >= 90) // after 90 ticks (1.5s)
        {
            // Enter second phase
            NPC.ai[0] = 2;
            NPC.ai[1] = 0;
            NPC.netUpdate = true;
            if (!secondPhase && NPC.life < NPC.lifeMax / 2)
            {
                PhaseTransitionSpin();
                //ManageBlizzard();
                secondPhase = true;
                SoundEngine.PlaySound(SoundID.Roar, NPC.Center);
                NPC.lifeMax = NPC.lifeMax * 2;
                NPC.life = NPC.lifeMax;
                NPC.dontTakeDamage = false;
                Music = MusicID.OtherworldlyEerie; // More intense music
                Main.instance.CameraModifiers.Add(new PunchCameraModifier(NPC.Center, Main.rand.NextVector2CircularEdge(1f, 1f), 15f, 6f, 20, 1000f, "Genichiro Phase 2"));
                NPC.GivenName = "Haemorrex, Godess of ichor";
                BossIntroScreen.Show("Haemorrex", "\"Goddes of ichor\"", "PlaceHolder", "Placeholder");
                CombatText.NewText(NPC.getRect(), Color.Yellow, "Identity conflict: PR1M–R3X... UNBOUND", true);
                for (int i = 0; i < 30; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Ichor, Scale: 1.5f);
                }
            }
            // Optional: dash, scream, or spawn projectiles
        }
    }

public override void FindFrame(int frameHeight)
{
    int startFrame = 0;
    int finalFrame = 2;

    if (secondPhase)
    {
        startFrame = 3;
        finalFrame = Main.npcFrameCount[NPC.type] - 1;

        if (NPC.frame.Y < startFrame * frameHeight)
        {
            NPC.frame.Y = startFrame * frameHeight;
        }
    }

    int frameSpeed = 5;
    NPC.frameCounter += 0.5f;
    NPC.frameCounter += NPC.velocity.Length() / 10f;

    if (NPC.frameCounter > frameSpeed)
    {
        NPC.frameCounter = 0;
        NPC.frame.Y += frameHeight;

        if (NPC.frame.Y > finalFrame * frameHeight)
        {
            NPC.frame.Y = startFrame * frameHeight;
        }
    }
}

    

}