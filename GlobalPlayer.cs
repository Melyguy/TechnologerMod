using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TechnologerMod{
    public class GlobalPlayer : ModPlayer{
        public float TechnologerDamage = 0f;

        public override void PostUpdate(){
            
        }
        public override void ResetEffects()
        {
            TechnologerDamage = 0f;
        }
        

    }
}


