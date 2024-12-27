using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200040A RID: 1034
	[StructLayout(LayoutKind.Sequential)]
	internal class SecurityBufferDescriptor
	{
		// Token: 0x060020BC RID: 8380 RVA: 0x00080FBA File Offset: 0x0007FFBA
		public SecurityBufferDescriptor(int count)
		{
			this.Version = 0;
			this.Count = count;
			this.UnmanagedPointer = null;
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00080FD8 File Offset: 0x0007FFD8
		[Conditional("TRAVE")]
		internal void DebugDump()
		{
		}

		// Token: 0x04002093 RID: 8339
		public readonly int Version;

		// Token: 0x04002094 RID: 8340
		public readonly int Count;

		// Token: 0x04002095 RID: 8341
		public unsafe void* UnmanagedPointer;
	}
}
