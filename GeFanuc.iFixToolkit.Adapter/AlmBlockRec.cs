using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	[StructLayout(0, Pack = 1)]
	public struct AlmBlockRec
	{
		public AlmCanonicalValue aCurrentValue;

		public VSP aVsp;

		[MarshalAs(23, SizeConst = 31)]
		public string szTag;

		[MarshalAs(23, SizeConst = 20)]
		public string szField;

		[MarshalAs(23, SizeConst = 34)]
		public string szEgu;

		public int ulState;

		public int ulFlag;

		public int ulAlmNum;

		public int ulReserved;
	}
}
