using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	public struct AlmOperRec
	{
		[MarshalAs(23, SizeConst = 9)]
		public string szOperatorNodeName;

		[MarshalAs(23, SizeConst = 31)]
		public string szOperatorLoginName;

		[MarshalAs(23, SizeConst = 20)]
		public string szAppName;

		[MarshalAs(23, SizeConst = 31)]
		public string szTag;

		public int ulReserved;
	}
}
