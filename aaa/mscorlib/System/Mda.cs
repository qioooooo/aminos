using System;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x020000D1 RID: 209
	internal static class Mda
	{
		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x00023C6F File Offset: 0x00022C6F
		internal static bool StreamWriterBufferMDAEnabled
		{
			get
			{
				if (Mda._streamWriterMDAState == Mda.MdaState.Unknown)
				{
					if (Mda.IsStreamWriterBufferedDataLostEnabled())
					{
						Mda._streamWriterMDAState = Mda.MdaState.Enabled;
					}
					else
					{
						Mda._streamWriterMDAState = Mda.MdaState.Disabled;
					}
				}
				return Mda._streamWriterMDAState == Mda.MdaState.Enabled;
			}
		}

		// Token: 0x06000BF4 RID: 3060
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void MemberInfoCacheCreation();

		// Token: 0x06000BF5 RID: 3061
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void DateTimeInvalidLocalFormat();

		// Token: 0x06000BF6 RID: 3062
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void StreamWriterBufferedDataLost(string text);

		// Token: 0x06000BF7 RID: 3063
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsStreamWriterBufferedDataLostEnabled();

		// Token: 0x06000BF8 RID: 3064
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsInvalidGCHandleCookieProbeEnabled();

		// Token: 0x06000BF9 RID: 3065
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void FireInvalidGCHandleCookieProbe(IntPtr cookie);

		// Token: 0x04000413 RID: 1043
		private static Mda.MdaState _streamWriterMDAState;

		// Token: 0x020000D2 RID: 210
		private enum MdaState
		{
			// Token: 0x04000415 RID: 1045
			Unknown,
			// Token: 0x04000416 RID: 1046
			Enabled,
			// Token: 0x04000417 RID: 1047
			Disabled
		}
	}
}
