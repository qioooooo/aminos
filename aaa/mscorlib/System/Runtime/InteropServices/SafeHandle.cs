using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000132 RID: 306
	[SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)]
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public abstract class SafeHandle : CriticalFinalizerObject, IDisposable
	{
		// Token: 0x06001195 RID: 4501 RVA: 0x00031F99 File Offset: 0x00030F99
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected SafeHandle(IntPtr invalidHandleValue, bool ownsHandle)
		{
			this.handle = invalidHandleValue;
			this._state = 4;
			this._ownsHandle = ownsHandle;
			if (!ownsHandle)
			{
				GC.SuppressFinalize(this);
			}
			this._fullyInitialized = true;
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00031FC8 File Offset: 0x00030FC8
		~SafeHandle()
		{
			this.Dispose(false);
		}

		// Token: 0x06001197 RID: 4503
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalFinalize();

		// Token: 0x06001198 RID: 4504 RVA: 0x00031FF8 File Offset: 0x00030FF8
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected void SetHandle(IntPtr handle)
		{
			this.handle = handle;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00032001 File Offset: 0x00031001
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public IntPtr DangerousGetHandle()
		{
			return this.handle;
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600119A RID: 4506 RVA: 0x00032009 File Offset: 0x00031009
		public bool IsClosed
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get
			{
				return (this._state & 1) == 1;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600119B RID: 4507
		public abstract bool IsInvalid
		{
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			get;
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00032016 File Offset: 0x00031016
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Close()
		{
			this.Dispose(true);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0003201F File Offset: 0x0003101F
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x00032028 File Offset: 0x00031028
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.InternalDispose();
				return;
			}
			this.InternalFinalize();
		}

		// Token: 0x0600119F RID: 4511
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InternalDispose();

		// Token: 0x060011A0 RID: 4512
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetHandleAsInvalid();

		// Token: 0x060011A1 RID: 4513
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected abstract bool ReleaseHandle();

		// Token: 0x060011A2 RID: 4514
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DangerousAddRef(ref bool success);

		// Token: 0x060011A3 RID: 4515
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void DangerousRelease();

		// Token: 0x040005D7 RID: 1495
		protected IntPtr handle;

		// Token: 0x040005D8 RID: 1496
		private int _state;

		// Token: 0x040005D9 RID: 1497
		private bool _ownsHandle;

		// Token: 0x040005DA RID: 1498
		private bool _fullyInitialized;
	}
}
