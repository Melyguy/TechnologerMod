using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader.Utilities;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Personalities;
using TechnologerMod.Content.Biomes;
using TechnologerMod.Content.Items.Placeable;
using TechnologerMod.Common.Systems;

namespace TechnologerMod.Content.NPCs.Hoarder
{
    [AutoloadHead]
    
    public class PeddlerOfRemains : ModNPC
    {

        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 20;
            NPC.height = 20;
            NPC.aiStyle = 7;
            NPC.defense = 40;
            NPC.lifeMax = 320;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.5f;

            Main.npcFrameCount[NPC.type] = 25;
            NPCID.Sets.ExtraFramesCount[NPC.type] = 0;
            NPCID.Sets.AttackFrameCount[NPC.type] = 1;
            NPCID.Sets.DangerDetectRange[NPC.type] = 200;
            NPCID.Sets.AttackType[NPC.type] = 1;
            NPCID.Sets.AttackTime[NPC.type] = 30;
            NPCID.Sets.AttackAverageChance[NPC.type] = 30;
            NPCID.Sets.HatOffsetY[NPC.type] = 4;
            AnimationType = 22;
            NPC.Happiness
            .SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
            .SetBiomeAffection<DesertBiome>(AffectionLevel.Love)
            .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Like)
            .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
            .SetBiomeAffection<OceanBiome>(AffectionLevel.Dislike)
            .SetNPCAffection(NPCID.Wizard, AffectionLevel.Love)
            .SetNPCAffection(NPCID.Mechanic, AffectionLevel.Love)
            .SetNPCAffection(NPCID.Angler, AffectionLevel.Like)
             .SetNPCAffection(NPCID.PartyGirl, AffectionLevel.Dislike)
             .SetNPCAffection(NPCID.TaxCollector, AffectionLevel.Hate)
            .SetNPCAffection(NPCID.Golfer, AffectionLevel.Dislike);

        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            for (var i = 0; i < 255; i++)
            {
                Player player = Main.player[i];
                foreach (Item item in player.inventory)
                {
                    if (item.type == ModContent.ItemType<EyesOfTheWyrm>())
                    {
                        return true;
                    }

                }

            }
            if (Main.hardMode)
            {
                        return true;
                
            }
            return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
           {
                "Kalé",
                "Thren",
                "Kael",
                "Varn"
           };
        }



        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = "Shop";
            button2 = "";
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = "Shop";
            }
        }

        /* public override void SetupShop(Chest shop, ref int nextSlot){
             shop.item[nextSlot].SetDefaults(ModContent.ItemType<Content.Items.Weapons.mortalBlade>());
             shop.item[nextSlot].value = 1000000;
             nextSlot++;
             shop.item[nextSlot].SetDefaults(ItemID.Shuriken);
             shop.item[nextSlot].value = 10;
             nextSlot++;
             shop.item[nextSlot].SetDefaults(ItemID.Katana);
             shop.item[nextSlot].value = 50000;
             nextSlot++;
             shop.item[nextSlot].SetDefaults(ItemID.Muramasa);
             shop.item[nextSlot].value = 500000;
             nextSlot++;
             shop.item[nextSlot].SetDefaults(ItemID.SmokeBomb);
             shop.item[nextSlot].value = 40;
             nextSlot++;
             shop.item[nextSlot].SetDefaults(ItemID.GrapplingHook);
             shop.item[nextSlot].value = 500;
             nextSlot++;
             shop.item[nextSlot].SetDefaults(ItemID.MasterNinjaGear);
             shop.item[nextSlot].value = 1000000;
             nextSlot++;
         }*/
        public static readonly Condition DefeatedBlightedWyrm = 
        new("Mods.TechnologerMod.Conditions.DefeatedBlightedWyrm", () => DownedBossSystem.DownedBlight);
public static readonly Condition DefeatedPrismatrix = 
        new("Mods.TechnologerMod.Conditions.DefeatedPrismatrix", () => DownedBossSystem.DownedPrismatrix);
        public static readonly Condition DefeatedHaemorex = 
        new("Mods.TechnologerMod.Conditions.DefeatedHaemorex", () => DownedBossSystem.DownedHaemorex);
        
        public override void AddShops()
        {
            NPCShop Sekiroshop = new NPCShop(Type, "Shop")
                .Add(ModContent.ItemType<EyesOfTheWyrm>(), DefeatedBlightedWyrm)
                .Add(ModContent.ItemType<GemCoreShards>(), DefeatedPrismatrix)
                .Add(ModContent.ItemType<HardenedIchor>(), DefeatedHaemorex)
                .Add(ItemID.Gel, Condition.DownedKingSlime)
                .Add(ItemID.CrimtaneOre, Condition.DownedEyeOfCthulhu)
                .Add(ItemID.DemoniteOre, Condition.DownedEyeOfCthulhu)
                .Add(ItemID.ShadowScale, Condition.DownedEowOrBoc)
                .Add(ItemID.TissueSample, Condition.DownedEowOrBoc)
                .Add(ItemID.BeeWax, Condition.DownedQueenBee)
                .Add(ItemID.SoulofLight, Condition.Hardmode)
                .Add(ItemID.SoulofNight, Condition.Hardmode)
                .Add(ItemID.SoulofFright, Condition.DownedSkeletronPrime)
                .Add(ItemID.SoulofSight, Condition.DownedTwins)
                .Add(ItemID.SoulofMight, Condition.DownedDestroyer)
                .Add(ItemID.HallowedBar, Condition.DownedMechBossAll)
                .Add(ItemID.BeetleHusk, Condition.DownedGolem)
                .Add(ItemID.FragmentNebula, Condition.DownedMoonLord)
                .Add(ItemID.FragmentSolar, Condition.DownedMoonLord)
                .Add(ItemID.FragmentStardust, Condition.DownedMoonLord)
                .Add(ItemID.FragmentVortex, Condition.DownedMoonLord)
                .Add(ItemID.Bone, Condition.DownedSkeletron);

            Sekiroshop.Register();
        }

        

        public override string GetChat()
        {
            NPC.FindFirstNPC(ModContent.NPCType<Hoarder.PeddlerOfRemains>());
            switch (Main.rand.Next(7))
            {
                case 0:
                    return "Trophies of the dead... for those who'd wear their pride.";
                case 1:
                    return "You're a Terrarian, I can see it. And i can also see... That you're not after my throat. Then why not purchase a little something?";
                case 2:
                    return "That race of technologer fools, always crafting. Even their own demise...";
                case 3:
                    return "Felled gods leave traces. I merely gather what remains.";
                case 4:
                    return "The road is long and littered in bones... But bones sell well, dont they";
                case 5:
                    return "I've seen technology like yours before. Long ago. Far below.";
                case 6:
                    return "I've seen your battles. Know that i shall be right behind you to collect what remains.";
                default:
                    return "Some say the technologers vanished. i say however, they were wiped out... By what i dont know.";
            }
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 50;
            knockback = 10f;


        }
        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 5;
            randExtraCooldown = 10;


        }
        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.Shuriken;
            attackDelay = 1;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 7f;
        }
        public override void OnKill()
        {
            Item.NewItem(NPC.GetSource_Death(), NPC.getRect(), ItemID.Shuriken, Main.rand.Next(1, 3), false, 0, false, false);
        }





    }


}

