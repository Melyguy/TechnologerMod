﻿using System.Collections;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace TechnologerMod.Common.Systems
{
	// Acts as a container for "downed boss" flags.
	// Set a flag like this in your bosses OnKill hook:
	//    NPC.SetEventFlagCleared(ref DownedBossSystem.downedForestGuardian, -1);

	// Saving and loading these flags requires TagCompounds, a guide exists on the wiki: https://github.com/tModLoader/tModLoader/wiki/Saving-and-loading-using-TagCompound
	public class DownedBossSystem : ModSystem
	{

		public static bool DownedPrototype = false;
		
		// public static bool downedOtherBoss = false;

		public override void ClearWorld() {

			DownedPrototype = false;

			// downedOtherBoss = false;
		}

		// We save our data sets using TagCompounds.
		// NOTE: The tag instance provided here is always empty by default.
		public override void SaveWorldData(TagCompound tag) {

			if(DownedPrototype) {
				tag["downedPrototype"] = true;	
			}
			// if (downedOtherBoss) {
			//	tag["downedOtherBoss"] = true;
			// }
		}

		public override void LoadWorldData(TagCompound tag) {
			DownedPrototype = tag.ContainsKey("downedPrototype");
			// downedOtherBoss = tag.ContainsKey("downedOtherBoss");
		}

		public override void NetSend(BinaryWriter writer) {
			// Order of parameters is important and has to match that of NetReceive
			writer.WriteFlags(DownedPrototype/*, downedOtherBoss*/);

			// WriteFlags supports up to 8 entries, if you have more than 8 flags to sync, call WriteFlags again.

			// If you need to send a large number of flags, such as a flag per item type or something similar, BitArray can be used to efficiently send them. See Utils.SendBitArray documentation.
		}

		public override void NetReceive(BinaryReader reader) {
			// Order of parameters is important and has to match that of NetSend
			reader.ReadFlags(out DownedPrototype/*, out downedOtherBoss*/);
			// ReadFlags supports up to 8 entries, if you have more than 8 flags to sync, call ReadFlags again.
		}
	}
}