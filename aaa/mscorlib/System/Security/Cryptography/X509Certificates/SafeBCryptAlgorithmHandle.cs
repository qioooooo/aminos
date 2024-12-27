using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008C1 RID: 2241
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeBCryptAlgorithmHandle : SafeHandle
	{
		// Token: 0x06005221 RID: 21025
		[DllImport("bcrypt.dll")]
		private static extern int BCryptCloseAlgorithmProvider([In] IntPtr hAlgorithm, [In] uint dwFlags);

		// Token: 0x06005222 RID: 21026 RVA: 0x0012875B File Offset: 0x0012775B
		public SafeBCryptAlgorithmHandle()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x17000E3D RID: 3645
		// (get) Token: 0x06005223 RID: 21027 RVA: 0x00128769 File Offset: 0x00127769
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x06005224 RID: 21028 RVA: 0x0012877C File Offset: 0x0012777C
		protected sealed override bool ReleaseHandle()
		{
			int num = SafeBCryptAlgorithmHandle.BCryptCloseAlgorithmProvider(this.handle, 0U);
			return num == 0;
		}
	}
}
