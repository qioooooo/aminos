using System;

namespace GeFanuc.iFixToolkit.Adapter
{
	[Serializable]
	public struct VSP
	{
		public byte mach_data;

		public byte type;

		public short ipn;

		public short field;

		public short index;

		public short size;
	}
}
