using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace TechnologerMod.Content.Projectiles
{
    public class HardenedIchorRocket : ModProjectile
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
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, Projectile.velocity.X * 0.3f, Projectile.velocity.Y * 0.3f);

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

        public override void OnKill(int timeLeft)
        {
            // Explosion sound
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // Smoke and fire dust
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke,
                    Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-5f, 5f), 100, default, 1.5f);
                Main.dust[dust].noGravity = true;
            }

            for (int i = 0; i < 20; i++)
            {
                int fire = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch,
                    Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-6f, 6f), 150, default, 1.5f);
                Main.dust[fire].noGravity = true;
            }
        }
    }
}
