using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	[StructLayout(0, Pack = 2)]
	public struct ENUMBUF
	{
		public short api_code;

		public short lookup_done;

		public VSP vsp;

		public int pdbsn;
	}
}
