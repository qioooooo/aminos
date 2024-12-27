using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000548 RID: 1352
	internal class NegotiationInfoClass
	{
		// Token: 0x0600291D RID: 10525 RVA: 0x000AB958 File Offset: 0x000AA958
		internal NegotiationInfoClass(SafeHandle safeHandle, int negotiationState)
		{
			if (safeHandle.IsInvalid)
			{
				return;
			}
			IntPtr intPtr = safeHandle.DangerousGetHandle();
			if (negotiationState == 0 || negotiationState == 1)
			{
				IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr, SecurityPackageInfo.NameOffest);
				string text = null;
				if (intPtr2 != IntPtr.Zero)
				{
					text = (ComNetOS.IsWin9x ? Marshal.PtrToStringAnsi(intPtr2) : Marshal.PtrToStringUni(intPtr2));
				}
				if (string.Compare(text, "Kerberos", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.AuthenticationPackage = "Kerberos";
					return;
				}
				if (string.Compare(text, "NTLM", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.AuthenticationPackage = "NTLM";
					return;
				}
				if (string.Compare(text, "WDigest", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.AuthenticationPackage = "WDigest";
					return;
				}
				this.AuthenticationPackage = text;
			}
		}

		// Token: 0x0400281D RID: 10269
		internal const string NTLM = "NTLM";

		// Token: 0x0400281E RID: 10270
		internal const string Kerberos = "Kerberos";

		// Token: 0x0400281F RID: 10271
		internal const string WDigest = "WDigest";

		// Token: 0x04002820 RID: 10272
		internal const string Negotiate = "Negotiate";

		// Token: 0x04002821 RID: 10273
		internal string AuthenticationPackage;
	}
}
