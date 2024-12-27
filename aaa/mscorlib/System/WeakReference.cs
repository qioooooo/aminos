using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Threading;

namespace System
{
	// Token: 0x02000127 RID: 295
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	[Serializable]
	public class WeakReference : ISerializable
	{
		// Token: 0x06001135 RID: 4405 RVA: 0x00030F73 File Offset: 0x0002FF73
		public WeakReference(object target)
			: this(target, false)
		{
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00030F7D File Offset: 0x0002FF7D
		public WeakReference(object target, bool trackResurrection)
		{
			this.m_IsLongReference = trackResurrection;
			this.m_handle = GCHandle.InternalAlloc(target, trackResurrection ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak);
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00030FA4 File Offset: 0x0002FFA4
		protected WeakReference(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			object value = info.GetValue("TrackedObject", typeof(object));
			this.m_IsLongReference = info.GetBoolean("TrackResurrection");
			this.m_handle = GCHandle.InternalAlloc(value, this.m_IsLongReference ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak);
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06001138 RID: 4408 RVA: 0x00031008 File Offset: 0x00030008
		public virtual bool IsAlive
		{
			get
			{
				IntPtr handle = this.m_handle;
				if (IntPtr.Zero == handle)
				{
					return false;
				}
				bool flag = GCHandle.InternalGet(handle) != null;
				return !(this.m_handle == IntPtr.Zero) && flag;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06001139 RID: 4409 RVA: 0x00031051 File Offset: 0x00030051
		public virtual bool TrackResurrection
		{
			get
			{
				return this.m_IsLongReference;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x0003105C File Offset: 0x0003005C
		// (set) Token: 0x0600113B RID: 4411 RVA: 0x000310A0 File Offset: 0x000300A0
		public virtual object Target
		{
			get
			{
				IntPtr handle = this.m_handle;
				if (IntPtr.Zero == handle)
				{
					return null;
				}
				object obj = GCHandle.InternalGet(handle);
				if (!(this.m_handle == IntPtr.Zero))
				{
					return obj;
				}
				return null;
			}
			set
			{
				IntPtr intPtr = this.m_handle;
				if (intPtr == IntPtr.Zero)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotInitialized"));
				}
				object obj = GCHandle.InternalGet(intPtr);
				intPtr = this.m_handle;
				if (intPtr == IntPtr.Zero)
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_HandleIsNotInitialized"));
				}
				GCHandle.InternalCompareExchange(intPtr, value, obj, false);
				GC.KeepAlive(this);
			}
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x00031110 File Offset: 0x00030110
		protected override void Finalize()
		{
			try
			{
				IntPtr handle = this.m_handle;
				if (handle != IntPtr.Zero && handle == Interlocked.CompareExchange(ref this.m_handle, IntPtr.Zero, handle))
				{
					GCHandle.InternalFree(handle);
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x0003116C File Offset: 0x0003016C
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("TrackedObject", this.Target, typeof(object));
			info.AddValue("TrackResurrection", this.m_IsLongReference);
		}

		// Token: 0x040005BF RID: 1471
		internal volatile IntPtr m_handle;

		// Token: 0x040005C0 RID: 1472
		internal bool m_IsLongReference;
	}
}
