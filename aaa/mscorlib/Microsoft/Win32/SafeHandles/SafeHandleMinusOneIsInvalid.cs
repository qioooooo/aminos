using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000469 RID: 1129
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public abstract class SafeHandleMinusOneIsInvalid : SafeHandle
	{
		// Token: 0x06002D65 RID: 11621 RVA: 0x00098BBD File Offset: 0x00097BBD
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected SafeHandleMinusOneIsInvalid(bool ownsHandle)
			: base(new IntPtr(-1), ownsHandle)
		{
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002D66 RID: 11622 RVA: 0x00098BCC File Offset: 0x00097BCC
		public override bool IsInvalid
		{
			get
			{
				return this.handle == new IntPtr(-1);
			}
		}
	}
}
