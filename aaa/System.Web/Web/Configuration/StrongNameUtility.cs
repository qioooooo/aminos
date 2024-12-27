using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x0200024F RID: 591
	internal class StrongNameUtility
	{
		// Token: 0x06001F41 RID: 8001 RVA: 0x0008ACD9 File Offset: 0x00089CD9
		private StrongNameUtility()
		{
		}

		// Token: 0x06001F42 RID: 8002
		[DllImport("mscoree.dll")]
		internal static extern void StrongNameFreeBuffer(IntPtr pbMemory);

		// Token: 0x06001F43 RID: 8003
		[DllImport("mscoree.dll")]
		internal static extern int StrongNameErrorInfo();

		// Token: 0x06001F44 RID: 8004
		[DllImport("mscoree.dll")]
		internal static extern bool StrongNameKeyGen([MarshalAs(UnmanagedType.LPWStr)] string wszKeyContainer, uint dwFlags, out IntPtr ppbKeyBlob, out long pcbKeyBlob);

		// Token: 0x06001F45 RID: 8005 RVA: 0x0008ACE4 File Offset: 0x00089CE4
		internal static bool GenerateStrongNameFile(string filename)
		{
			IntPtr zero = IntPtr.Zero;
			long num = 0L;
			bool flag = StrongNameUtility.StrongNameKeyGen(null, 0U, out zero, out num);
			if (!flag || zero == IntPtr.Zero)
			{
				throw Marshal.GetExceptionForHR(StrongNameUtility.StrongNameErrorInfo());
			}
			try
			{
				if (num <= 0L || num > 2147483647L)
				{
					throw new InvalidOperationException(SR.GetString("Browser_InvalidStrongNameKey"));
				}
				byte[] array = new byte[num];
				Marshal.Copy(zero, array, 0, (int)num);
				using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write))
				{
					using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
					{
						binaryWriter.Write(array);
					}
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					StrongNameUtility.StrongNameFreeBuffer(zero);
				}
			}
			return true;
		}
	}
}
