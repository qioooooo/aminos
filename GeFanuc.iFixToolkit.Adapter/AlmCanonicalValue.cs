using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	[StructLayout(0, Pack = 2)]
	public struct AlmCanonicalValue
	{
		[MarshalAs(8)]
		public int ulValueType;

		public almUnion unionValue;

		public short bTextValid;

		[MarshalAs(23, SizeConst = 80)]
		public string szText;
	}
}
