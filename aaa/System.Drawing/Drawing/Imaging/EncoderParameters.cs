using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Drawing.Imaging
{
	// Token: 0x02000086 RID: 134
	public sealed class EncoderParameters : IDisposable
	{
		// Token: 0x060007A9 RID: 1961 RVA: 0x0001D7E9 File Offset: 0x0001C7E9
		public EncoderParameters(int count)
		{
			this.param = new EncoderParameter[count];
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x0001D7FD File Offset: 0x0001C7FD
		public EncoderParameters()
		{
			this.param = new EncoderParameter[1];
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0001D811 File Offset: 0x0001C811
		// (set) Token: 0x060007AC RID: 1964 RVA: 0x0001D819 File Offset: 0x0001C819
		public EncoderParameter[] Param
		{
			get
			{
				return this.param;
			}
			set
			{
				this.param = value;
			}
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001D824 File Offset: 0x0001C824
		internal IntPtr ConvertToMemory()
		{
			int num = Marshal.SizeOf(typeof(EncoderParameter));
			int num2 = this.param.Length;
			IntPtr intPtr;
			long num3;
			checked
			{
				intPtr = Marshal.AllocHGlobal(num2 * num + Marshal.SizeOf(typeof(IntPtr)));
				if (intPtr == IntPtr.Zero)
				{
					throw SafeNativeMethods.Gdip.StatusException(3);
				}
				Marshal.WriteIntPtr(intPtr, (IntPtr)num2);
				num3 = (long)intPtr + unchecked((long)Marshal.SizeOf(typeof(IntPtr)));
			}
			for (int i = 0; i < num2; i++)
			{
				Marshal.StructureToPtr(this.param[i], (IntPtr)(num3 + (long)(i * num)), false);
			}
			return intPtr;
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001D8C8 File Offset: 0x0001C8C8
		internal static EncoderParameters ConvertFromMemory(IntPtr memory)
		{
			if (memory == IntPtr.Zero)
			{
				throw SafeNativeMethods.Gdip.StatusException(2);
			}
			int num = Marshal.ReadIntPtr(memory).ToInt32();
			EncoderParameters encoderParameters = new EncoderParameters(num);
			int num2 = Marshal.SizeOf(typeof(EncoderParameter));
			long num3 = (long)memory + (long)Marshal.SizeOf(typeof(IntPtr));
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				for (int i = 0; i < num; i++)
				{
					Guid guid = (Guid)UnsafeNativeMethods.PtrToStructure((IntPtr)((long)(i * num2) + num3), typeof(Guid));
					int num4 = Marshal.ReadInt32((IntPtr)((long)(i * num2) + num3 + 16L));
					int num5 = Marshal.ReadInt32((IntPtr)((long)(i * num2) + num3 + 20L));
					int num6 = Marshal.ReadInt32((IntPtr)((long)(i * num2) + num3 + 24L));
					encoderParameters.param[i] = new EncoderParameter(new Encoder(guid), num4, num5, num6);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return encoderParameters;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0001D9E4 File Offset: 0x0001C9E4
		public void Dispose()
		{
			foreach (EncoderParameter encoderParameter in this.param)
			{
				if (encoderParameter != null)
				{
					encoderParameter.Dispose();
				}
			}
			this.param = null;
		}

		// Token: 0x04000612 RID: 1554
		private EncoderParameter[] param;
	}
}
