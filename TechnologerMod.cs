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
			if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
				return;

			// Register bosses with Boss Checklist
			// Order: modInstance, internalName, displayName, progress, downed condition, npc type
			bossChecklist.Call(
				"AddBoss",
				this,                // Mod instance
				"BlightWyrm",       // Internal boss name
				"Blight Wyrm",      // Display name
				2.5f,               // Progress (2.5 = between Eye of Cthulhu and Eate r of Worlds)
				DownedBossSystem.DownedBlight, // Downed bool
				ModContent.NPCType<Content.Bosses.BlightWyrm.BlightWyrmHead>()
			);

			bossChecklist.Call(
				"AddBoss",
				this,
				"Prismatrix",
				"Prismatrix",
				4.0f,
				DownedBossSystem.DownedPrismatrix,
				ModContent.NPCType<Content.Bosses.Prismatrix.PrismatrixHead>()
			);

			bossChecklist.Call(
				"AddBoss",
				this,
				"Haemorrex",
				"Haemorrex",
				4.5f,
				DownedBossSystem.DownedHaemorex,
				ModContent.NPCType<Content.Bosses.Haemorrex.Haemorrex>()
			);
		}
	}
}
