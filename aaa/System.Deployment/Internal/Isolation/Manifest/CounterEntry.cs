using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D4 RID: 468
	[StructLayout(LayoutKind.Sequential)]
	internal class CounterEntry
	{
		// Token: 0x040007D7 RID: 2007
		public Guid CounterSetGuid;

		// Token: 0x040007D8 RID: 2008
		public uint CounterId;

		// Token: 0x040007D9 RID: 2009
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x040007DA RID: 2010
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;

		// Token: 0x040007DB RID: 2011
		public uint CounterType;

		// Token: 0x040007DC RID: 2012
		public ulong Attributes;

		// Token: 0x040007DD RID: 2013
		public uint BaseId;

		// Token: 0x040007DE RID: 2014
		public uint DefaultScale;
	}
}
