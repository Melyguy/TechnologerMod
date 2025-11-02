using System;
using Terraria.ModLoader;
using TechnologerMod.Common.Systems;

namespace TechnologerMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class TechnologerMod : Mod
	{
		public override void PostSetupContent()
		{
			// Register bosses with Boss Checklist (if the player has the BossChecklist mod installed).
			// This call is optional and only executed when BossChecklist is present.
			if (ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
			{
				// Each AddBoss call typically takes: (string displayName, float progression, Func<bool> downedCondition, int npcType)
				// BossChecklist's Mod.Call accepts a flexible set of parameters. We use a minimal form: name, progression, downed delegate and NPC type.

				// Blight Wyrm
				try
				{
					bossChecklist.Call("AddBoss", this,
						"Blight Wyrm",
						2.5f,
						(Func<bool>)(() => DownedBossSystem.DownedBlight),
						ModContent.NPCType<Content.Bosses.BlightWyrm.BlightWyrmHead>());
				}
				catch { }

				// Prototype ZR1
				/*try
				{
					bossChecklist.Call("AddBoss", this,
						"Prototype ZR-1",
						3.0f,
						(Func<bool>)(() => DownedBossSystem.DownedPrototype),
						ModContent.NPCType<Content.Bosses.PrototypeZR1.PrototypeZR1Head>());
				}
				catch { }*/

				// Prismatrix
				try
				{
					bossChecklist.Call("AddBoss", this,
						"Prismatrix",
						4.0f,
						(Func<bool>)(() => DownedBossSystem.DownedPrismatrix),
						ModContent.NPCType<Content.Bosses.Prismatrix.PrismatrixHead>());
				}
				catch { }

				// Haemorrex
				try
				{
					bossChecklist.Call("AddBoss", this,
						"Haemorrex",
						4.5f,
						(Func<bool>)(() => DownedBossSystem.DownedHaemorex),
						ModContent.NPCType<Content.Bosses.Haemorrex.Haemorrex>());
				}
				catch { }
			}
		}
	}
}
