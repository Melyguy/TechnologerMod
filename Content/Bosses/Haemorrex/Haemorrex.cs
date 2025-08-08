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

namespace TechnologerMod.Content.Bosses.Haemorrex;
[AutoloadBossHead]
public class Haemorrex : ModNPC
{
    private bool secondPhase = false;
    public static int HaemorrexMusicSlot;

    public override void SetDefaults()
    {
        NPC.aiStyle = 5; // Eye of Cthulhu movement
        AIType = NPCID.EyeofCthulhu;
        NPC.width = 110;
        NPC.height = 110;
        NPC.damage = 40;
        NPC.defense = 10;
        NPC.lifeMax = 16000;
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
        Player target = Main.player[NPC.target];
    NPC.TargetClosest(false);
    NPC.velocity.Y -= 0.1f; // Fly upward
        // Second phase trigger


        // NPC.ai[0] = phase tracker
        // NPC.ai[1] = transition timer
        // 0 = phase 1, 1 = transitioning, 2 = phase 2

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
                NPC.lifeMax = 20000;
                NPC.life = 20000;
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
private void ManageBlizzard()
            {
                // Force blizzard weather while the boss is alive
                Main.windSpeedTarget = 0.8f * NPC.direction; // Strong wind in the direction the boss is facing
                Main.maxRaining = 1f; // Maximum rain intensity
                Main.raining = true;
                Main.rainTime = 2; // Keep rain active
                Main.cloudAlpha = 1f; // Full cloud coverage
                
                // Create snow particles
                if (Main.rand.NextBool(2)) // 50% chance each frame
                {
                    int snowDust = Dust.NewDust(NPC.position, Main.screenWidth, 10, DustID.Snow, 
                        Main.windSpeedTarget * 5f, 2f, 0, default, 1.2f);
                    Main.dust[snowDust].noGravity = true;
                    Main.dust[snowDust].velocity.X *= 2f;
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