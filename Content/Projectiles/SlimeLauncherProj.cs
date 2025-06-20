using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TechnologerMod.Content.Projectiles
{
    public class SlimeLauncherProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 120;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            // Trail effect
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueCrystalShard, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f);

            // Scale over time: from 1.0 to 1.5
            // Rotation effect
            Projectile.rotation += 0.4f; // Controls spin speed
            
            // Pulsing glow effect
            float pulseRate = 6f;
            float glowIntensity = 0.3f + (float)System.Math.Sin(Main.GameUpdateCount / pulseRate) * 0.2f;
            Lighting.AddLight(Projectile.Center, 0.5f * glowIntensity, 0.5f * glowIntensity, 1f * glowIntensity); // Blue-tinted light

            // Make the rotation follow the velocity direction
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            // Spawn soul shards
            for (int i = 0; i < 3; i++)
            {
                Vector2 shardVel = Projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * 0.8f;
                Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, shardVel,
                    ProjectileID.SoulDrain, Projectile.damage / 2, 1f, Projectile.owner);
            }

            // Final explosion
            for (int i = 0; i < 10; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BlueMoss, 0f, 0f, 150, default, 1.2f);
            }
        }
    }
}
