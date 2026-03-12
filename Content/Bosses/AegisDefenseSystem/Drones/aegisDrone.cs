using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;
using Terraria.Audio;

namespace TechnologerMod.Content.Bosses.AegisDefenseSystem.Drones
{
    public class aegisDrone : ModNPC
    {
    private const int AttackCooldown = 180;
    private const int AimDuration = 30;
    private const float OrbitDistance = 120f;
    private const float TornadoOffset = 100f; // Distance to place tornados from player
    public ref float AttackTimer => ref NPC.ai[2];
    public ref float IsAttacking => ref NPC.ai[3];
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 16;
            NPC.damage = 25;
            NPC.defense = 3;
            NPC.lifeMax = 150;
            NPC.aiStyle = 2;
            NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath3;
        }

        public override void AI()
        {
            AttackTimer++;

        Player target = Main.player[NPC.target];

            if (Main.rand.NextBool() && AttackTimer >= AttackCooldown) // 50% chance energyball
                {
                    AttackTimer = 0;
                    Vector2 shootDirection = target.Center - NPC.Center;
                    shootDirection.Normalize();
                    shootDirection *= 10f;

                    int proj = Projectile.NewProjectile(
                        NPC.GetSource_FromAI(),
                        NPC.Center,
                        shootDirection,
                        ProjectileID.EyeLaser,
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
        }

    }
}