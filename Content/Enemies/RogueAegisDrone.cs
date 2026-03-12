using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Utilities;

namespace TechnologerMod.Content.Enemies
{
    public class RogueAegisDrone : ModNPC
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 16;
            NPC.damage = 11;
            NPC.defense = 3;
            NPC.lifeMax = 50;
            NPC.value = 10000f;
            NPC.aiStyle = 2;
            NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath3;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.OverworldDaySlime.Chance * 0.1f;
        }
    

        public override void OnKill()
        {
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.IronOre, Main.rand.Next(0, 2));
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.Wood, Main.rand.Next(0, 4));
        }

    }
}