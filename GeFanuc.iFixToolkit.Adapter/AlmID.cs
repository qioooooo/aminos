using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	public struct AlmID
	{
		private short iType;

		private short iSpare;

		[MarshalAs(23, SizeConst = 168)]
		public string szBuf1;

		[MarshalAs(23, SizeConst = 168)]
		public string szBuf2;
	}
}
