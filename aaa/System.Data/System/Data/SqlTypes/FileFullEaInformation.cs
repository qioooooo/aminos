using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace System.Data.SqlTypes
{
	// Token: 0x0200034E RID: 846
	internal class FileFullEaInformation : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D59 RID: 11609 RVA: 0x002AA600 File Offset: 0x002A9A00
		public FileFullEaInformation(byte[] transactionContext)
			: base(true)
		{
			this.m_cbBuffer = 0;
			this.InitializeEaBuffer(transactionContext);
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x002AA630 File Offset: 0x002A9A30
		protected override bool ReleaseHandle()
		{
			this.m_cbBuffer = 0;
			if (this.handle == IntPtr.Zero)
			{
				return true;
			}
			Marshal.FreeHGlobal(this.handle);
			this.handle = IntPtr.Zero;
			return true;
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002D5B RID: 11611 RVA: 0x002AA670 File Offset: 0x002A9A70
		public int Length
		{
			get
			{
				return this.m_cbBuffer;
			}
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x002AA684 File Offset: 0x002A9A84
		private void InitializeEaBuffer(byte[] transactionContext)
		{
			if (transactionContext.Length >= 65535)
			{
				throw ADP.ArgumentOutOfRange("transactionContext");
			}
			UnsafeNativeMethods.FILE_FULL_EA_INFORMATION file_FULL_EA_INFORMATION;
			file_FULL_EA_INFORMATION.nextEntryOffset = 0U;
			file_FULL_EA_INFORMATION.flags = 0;
			file_FULL_EA_INFORMATION.EaName = 0;
			file_FULL_EA_INFORMATION.EaNameLength = (byte)this.EA_NAME_STRING.Length;
			file_FULL_EA_INFORMATION.EaValueLength = (ushort)transactionContext.Length;
			this.m_cbBuffer = Marshal.SizeOf(file_FULL_EA_INFORMATION) + (int)file_FULL_EA_INFORMATION.EaNameLength + (int)file_FULL_EA_INFORMATION.EaValueLength;
			IntPtr intPtr = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				intPtr = Marshal.AllocHGlobal(this.m_cbBuffer);
				if (intPtr != IntPtr.Zero)
				{
					base.SetHandle(intPtr);
				}
			}
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				base.DangerousAddRef(ref flag);
				IntPtr intPtr2 = base.DangerousGetHandle();
				Marshal.StructureToPtr(file_FULL_EA_INFORMATION, intPtr2, false);
				ASCIIEncoding asciiencoding = new ASCIIEncoding();
				byte[] bytes = asciiencoding.GetBytes(this.EA_NAME_STRING);
				int num = Marshal.OffsetOf(typeof(UnsafeNativeMethods.FILE_FULL_EA_INFORMATION), "EaName").ToInt32();
				int num2 = 0;
				while (num < this.m_cbBuffer && num2 < (int)file_FULL_EA_INFORMATION.EaNameLength)
				{
					Marshal.WriteByte(intPtr2, num, bytes[num2]);
					num2++;
					num++;
				}
				Marshal.WriteByte(intPtr2, num, 0);
				num++;
				int num3 = 0;
				while (num < this.m_cbBuffer && num3 < (int)file_FULL_EA_INFORMATION.EaValueLength)
				{
					Marshal.WriteByte(intPtr2, num, transactionContext[num3]);
					num3++;
					num++;
				}
			}
			finally
			{
				if (flag)
				{
					base.DangerousRelease();
				}
			}
		}

		// Token: 0x04001CE9 RID: 7401
		private string EA_NAME_STRING = "Filestream_Transaction_Tag";

		// Token: 0x04001CEA RID: 7402
		private int m_cbBuffer;
	}
}
