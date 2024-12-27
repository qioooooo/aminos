using System;
using System.Runtime.CompilerServices;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x0200051F RID: 1311
	internal sealed class SafeCredentialReference : CriticalHandleMinusOneIsInvalid
	{
		// Token: 0x06002851 RID: 10321 RVA: 0x000A60F0 File Offset: 0x000A50F0
		internal static SafeCredentialReference CreateReference(SafeFreeCredentials target)
		{
			SafeCredentialReference safeCredentialReference = new SafeCredentialReference(target);
			if (safeCredentialReference.IsInvalid)
			{
				return null;
			}
			return safeCredentialReference;
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x000A6110 File Offset: 0x000A5110
		private SafeCredentialReference(SafeFreeCredentials target)
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				target.DangerousAddRef(ref flag);
			}
			catch
			{
				if (flag)
				{
					target.DangerousRelease();
					flag = false;
				}
			}
			finally
			{
				if (flag)
				{
					this._Target = target;
					base.SetHandle(new IntPtr(0));
				}
			}
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x000A6178 File Offset: 0x000A5178
		protected override bool ReleaseHandle()
		{
			SafeFreeCredentials target = this._Target;
			if (target != null)
			{
				target.DangerousRelease();
			}
			this._Target = null;
			return true;
		}

		// Token: 0x04002779 RID: 10105
		internal SafeFreeCredentials _Target;
	}
}
