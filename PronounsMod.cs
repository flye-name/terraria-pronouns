using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PronounsMod.Core.Players;
using Terraria.ModLoader;

namespace PronounsMod
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class PronounsMod : Mod
	{
		public override void HandlePacket(BinaryReader reader, int whoAmI) => PlayerPronoun.ReceiveChanges(reader, whoAmI);
	}
}
