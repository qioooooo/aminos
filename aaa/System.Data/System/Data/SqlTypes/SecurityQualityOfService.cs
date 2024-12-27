using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Data.SqlTypes
{
	// Token: 0x0200034D RID: 845
	internal class SecurityQualityOfService : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D56 RID: 11606 RVA: 0x002AA4F4 File Offset: 0x002A98F4
		public SecurityQualityOfService(UnsafeNativeMethods.SecurityImpersonationLevel impersonationLevel, bool effectiveOnly, bool dynamicTrackingMode)
			: base(true)
		{
			this.Initialize(impersonationLevel, effectiveOnly, dynamicTrackingMode);
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x002AA514 File Offset: 0x002A9914
		protected override bool ReleaseHandle()
		{
			if (this.m_hQos.IsAllocated)
			{
				this.m_hQos.Free();
			}
			this.handle = IntPtr.Zero;
			return true;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x002AA548 File Offset: 0x002A9948
		internal void Initialize(UnsafeNativeMethods.SecurityImpersonationLevel impersonationLevel, bool effectiveOnly, bool dynamicTrackingMode)
		{
			this.m_qos.length = (uint)Marshal.SizeOf(typeof(UnsafeNativeMethods.SECURITY_QUALITY_OF_SERVICE));
			this.m_qos.impersonationLevel = (int)impersonationLevel;
			this.m_qos.effectiveOnly = (effectiveOnly ? 1 : 0);
			this.m_qos.contextDynamicTrackingMode = (dynamicTrackingMode ? 1 : 0);
			IntPtr intPtr = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this.m_hQos = GCHandle.Alloc(this.m_qos, GCHandleType.Pinned);
				intPtr = this.m_hQos.AddrOfPinnedObject();
				if (intPtr != IntPtr.Zero)
				{
					base.SetHandle(intPtr);
				}
			}
		}

		// Token: 0x04001CE7 RID: 7399
		private UnsafeNativeMethods.SECURITY_QUALITY_OF_SERVICE m_qos;

		// Token: 0x04001CE8 RID: 7400
		private GCHandle m_hQos;
	}
}
