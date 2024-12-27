using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x0200046B RID: 1131
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	public abstract class CriticalHandleZeroOrMinusOneIsInvalid : CriticalHandle
	{
		// Token: 0x06002D73 RID: 11635 RVA: 0x00098CA8 File Offset: 0x00097CA8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected CriticalHandleZeroOrMinusOneIsInvalid()
			: base(IntPtr.Zero)
		{
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002D74 RID: 11636 RVA: 0x00098CB5 File Offset: 0x00097CB5
		public override bool IsInvalid
		{
			get
			{
				return this.handle.IsNull() || this.handle == new IntPtr(-1);
			}
		}
	}
}
