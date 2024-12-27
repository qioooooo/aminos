using System;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F8 RID: 1272
	[ComVisible(true)]
	public struct GCHandle
	{
		// Token: 0x06003165 RID: 12645 RVA: 0x000A96C2 File Offset: 0x000A86C2
		static GCHandle()
		{
			if (GCHandle.s_probeIsActive)
			{
				GCHandle.s_cookieTable = new GCHandleCookieTable();
			}
		}

		// Token: 0x06003166 RID: 12646 RVA: 0x000A96F6 File Offset: 0x000A86F6
		internal GCHandle(object value, GCHandleType type)
		{
			this.m_handle = GCHandle.InternalAlloc(value, type);
			if (type == GCHandleType.Pinned)
			{
				this.SetIsPinned();
			}
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x000A970F File Offset: 0x000A870F
		internal GCHandle(IntPtr handle)
		{
			GCHandle.InternalCheckDomain(handle);
			this.m_handle = handle;
		}

		// Token: 0x06003168 RID: 12648 RVA: 0x000A971E File Offset: 0x000A871E
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static GCHandle Alloc(object value)
		{
			return new GCHandle(value, GCHandleType.Normal);
		}

		// Token: 0x06003169 RID: 12649 RVA: 0x000A9727 File Offset: 0x000A8727
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static GCHandle Alloc(object value, GCHandleType type)
		{
			return new GCHandle(value, type);
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x000A9730 File Offset: 0x000A8730
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void Free()
		{
			IntPtr handle = this.m_handle;
			if (!(handle != IntPtr.Zero) || !(Interlocked.CompareExchange(ref this.m_handle, IntPtr.Zero, handle) == handle))
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotInitialized"));
			}
			GCHandle.InternalFree((IntPtr)((int)handle & -2));
			if (GCHandle.s_probeIsActive)
			{
				GCHandle.s_cookieTable.RemoveHandleIfPresent(handle);
				return;
			}
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x000A97A0 File Offset: 0x000A87A0
		// (set) Token: 0x0600316C RID: 12652 RVA: 0x000A97CF File Offset: 0x000A87CF
		public object Target
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (this.m_handle == IntPtr.Zero)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotInitialized"));
				}
				return GCHandle.InternalGet(this.GetHandleValue());
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				if (this.m_handle == IntPtr.Zero)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotInitialized"));
				}
				GCHandle.InternalSet(this.GetHandleValue(), value, this.IsPinned());
			}
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x000A9808 File Offset: 0x000A8808
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public IntPtr AddrOfPinnedObject()
		{
			if (this.IsPinned())
			{
				return GCHandle.InternalAddrOfPinnedObject(this.GetHandleValue());
			}
			if (this.m_handle == IntPtr.Zero)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotInitialized"));
			}
			throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotPinned"));
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x000A985A File Offset: 0x000A885A
		public bool IsAllocated
		{
			get
			{
				return this.m_handle != IntPtr.Zero;
			}
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x000A986C File Offset: 0x000A886C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static explicit operator GCHandle(IntPtr value)
		{
			return GCHandle.FromIntPtr(value);
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x000A9874 File Offset: 0x000A8874
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static GCHandle FromIntPtr(IntPtr value)
		{
			if (value == IntPtr.Zero)
			{
				throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotInitialized"));
			}
			IntPtr intPtr = value;
			if (GCHandle.s_probeIsActive)
			{
				intPtr = GCHandle.s_cookieTable.GetHandle(value);
				if (IntPtr.Zero == intPtr)
				{
					Mda.FireInvalidGCHandleCookieProbe(value);
					return new GCHandle(IntPtr.Zero);
				}
			}
			return new GCHandle(intPtr);
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x000A98D7 File Offset: 0x000A88D7
		public static explicit operator IntPtr(GCHandle value)
		{
			return GCHandle.ToIntPtr(value);
		}

		// Token: 0x06003172 RID: 12658 RVA: 0x000A98DF File Offset: 0x000A88DF
		public static IntPtr ToIntPtr(GCHandle value)
		{
			if (GCHandle.s_probeIsActive)
			{
				return GCHandle.s_cookieTable.FindOrAddHandle(value.m_handle);
			}
			return value.m_handle;
		}

		// Token: 0x06003173 RID: 12659 RVA: 0x000A9901 File Offset: 0x000A8901
		public override int GetHashCode()
		{
			return (int)this.m_handle;
		}

		// Token: 0x06003174 RID: 12660 RVA: 0x000A9910 File Offset: 0x000A8910
		public override bool Equals(object o)
		{
			if (o == null || !(o is GCHandle))
			{
				return false;
			}
			GCHandle gchandle = (GCHandle)o;
			return this.m_handle == gchandle.m_handle;
		}

		// Token: 0x06003175 RID: 12661 RVA: 0x000A9943 File Offset: 0x000A8943
		public static bool operator ==(GCHandle a, GCHandle b)
		{
			return a.m_handle == b.m_handle;
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x000A9958 File Offset: 0x000A8958
		public static bool operator !=(GCHandle a, GCHandle b)
		{
			return a.m_handle != b.m_handle;
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x000A996D File Offset: 0x000A896D
		internal IntPtr GetHandleValue()
		{
			return new IntPtr((int)this.m_handle & -2);
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x000A9982 File Offset: 0x000A8982
		internal bool IsPinned()
		{
			return ((int)this.m_handle & 1) != 0;
		}

		// Token: 0x06003179 RID: 12665 RVA: 0x000A9997 File Offset: 0x000A8997
		internal void SetIsPinned()
		{
			this.m_handle = new IntPtr((int)this.m_handle | 1);
		}

		// Token: 0x0600317A RID: 12666
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr InternalAlloc(object value, GCHandleType type);

		// Token: 0x0600317B RID: 12667
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalFree(IntPtr handle);

		// Token: 0x0600317C RID: 12668
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object InternalGet(IntPtr handle);

		// Token: 0x0600317D RID: 12669
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalSet(IntPtr handle, object value, bool isPinned);

		// Token: 0x0600317E RID: 12670
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object InternalCompareExchange(IntPtr handle, object value, object oldValue, bool isPinned);

		// Token: 0x0600317F RID: 12671
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern IntPtr InternalAddrOfPinnedObject(IntPtr handle);

		// Token: 0x06003180 RID: 12672
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void InternalCheckDomain(IntPtr handle);

		// Token: 0x04001970 RID: 6512
		internal static readonly IntPtr InvalidCookie = new IntPtr(-1);

		// Token: 0x04001971 RID: 6513
		private IntPtr m_handle;

		// Token: 0x04001972 RID: 6514
		private static GCHandleCookieTable s_cookieTable = null;

		// Token: 0x04001973 RID: 6515
		private static bool s_probeIsActive = Mda.IsInvalidGCHandleCookieProbeEnabled();
	}
}
