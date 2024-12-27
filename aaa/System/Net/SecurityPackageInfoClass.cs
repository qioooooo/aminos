using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200054A RID: 1354
	internal class SecurityPackageInfoClass
	{
		// Token: 0x0600291F RID: 10527 RVA: 0x000ABA40 File Offset: 0x000AAA40
		internal SecurityPackageInfoClass(SafeHandle safeHandle, int index)
		{
			if (safeHandle.IsInvalid)
			{
				return;
			}
			IntPtr intPtr = IntPtrHelper.Add(safeHandle.DangerousGetHandle(), SecurityPackageInfo.Size * index);
			this.Capabilities = Marshal.ReadInt32(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Capabilities"));
			this.Version = Marshal.ReadInt16(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Version"));
			this.RPCID = Marshal.ReadInt16(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "RPCID"));
			this.MaxToken = Marshal.ReadInt32(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "MaxToken"));
			IntPtr intPtr2 = Marshal.ReadIntPtr(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Name"));
			if (intPtr2 != IntPtr.Zero)
			{
				if (ComNetOS.IsWin9x)
				{
					this.Name = Marshal.PtrToStringAnsi(intPtr2);
				}
				else
				{
					this.Name = Marshal.PtrToStringUni(intPtr2);
				}
			}
			intPtr2 = Marshal.ReadIntPtr(intPtr, (int)Marshal.OffsetOf(typeof(SecurityPackageInfo), "Comment"));
			if (intPtr2 != IntPtr.Zero)
			{
				if (ComNetOS.IsWin9x)
				{
					this.Comment = Marshal.PtrToStringAnsi(intPtr2);
					return;
				}
				this.Comment = Marshal.PtrToStringUni(intPtr2);
			}
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x000ABBA0 File Offset: 0x000AABA0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Capabilities:",
				string.Format(CultureInfo.InvariantCulture, "0x{0:x}", new object[] { this.Capabilities }),
				" Version:",
				this.Version.ToString(NumberFormatInfo.InvariantInfo),
				" RPCID:",
				this.RPCID.ToString(NumberFormatInfo.InvariantInfo),
				" MaxToken:",
				this.MaxToken.ToString(NumberFormatInfo.InvariantInfo),
				" Name:",
				(this.Name == null) ? "(null)" : this.Name,
				" Comment:",
				(this.Comment == null) ? "(null)" : this.Comment
			});
		}

		// Token: 0x0400282A RID: 10282
		internal int Capabilities;

		// Token: 0x0400282B RID: 10283
		internal short Version;

		// Token: 0x0400282C RID: 10284
		internal short RPCID;

		// Token: 0x0400282D RID: 10285
		internal int MaxToken;

		// Token: 0x0400282E RID: 10286
		internal string Name;

		// Token: 0x0400282F RID: 10287
		internal string Comment;
	}
}
