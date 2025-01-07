using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	public struct AlmExtensionFields
	{
		public short iExtensionType;

		[MarshalAs(23, SizeConst = 81)]
		public string szExtText1;

		public char cSpare1;

		public char cSpare2;

		[MarshalAs(23, SizeConst = 81)]
		public string szExtText2;

		public int lReserved2;
	}
}
