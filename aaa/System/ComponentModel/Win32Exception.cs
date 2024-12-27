using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.ComponentModel
{
	// Token: 0x02000157 RID: 343
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class Win32Exception : ExternalException, ISerializable
	{
		// Token: 0x06000B45 RID: 2885 RVA: 0x00027EAB File Offset: 0x00026EAB
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception()
			: this(Marshal.GetLastWin32Error())
		{
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00027EB8 File Offset: 0x00026EB8
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception(int error)
			: this(error, Win32Exception.GetErrorMessage(error))
		{
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00027EC7 File Offset: 0x00026EC7
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception(int error, string message)
			: base(message)
		{
			this.nativeErrorCode = error;
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00027ED7 File Offset: 0x00026ED7
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception(string message)
			: this(Marshal.GetLastWin32Error(), message)
		{
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00027EE5 File Offset: 0x00026EE5
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public Win32Exception(string message, Exception innerException)
			: base(message, innerException)
		{
			this.nativeErrorCode = Marshal.GetLastWin32Error();
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00027EFA File Offset: 0x00026EFA
		protected Win32Exception(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			IntSecurity.UnmanagedCode.Demand();
			this.nativeErrorCode = info.GetInt32("NativeErrorCode");
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00027F1F File Offset: 0x00026F1F
		public int NativeErrorCode
		{
			get
			{
				return this.nativeErrorCode;
			}
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x00027F28 File Offset: 0x00026F28
		private static string GetErrorMessage(int error)
		{
			StringBuilder stringBuilder = new StringBuilder(256);
			int num = SafeNativeMethods.FormatMessage(12800, NativeMethods.NullHandleRef, error, 0, stringBuilder, stringBuilder.Capacity + 1, IntPtr.Zero);
			string text;
			if (num != 0)
			{
				int i;
				for (i = stringBuilder.Length; i > 0; i--)
				{
					char c = stringBuilder[i - 1];
					if (c > ' ' && c != '.')
					{
						break;
					}
				}
				text = stringBuilder.ToString(0, i);
			}
			else
			{
				text = "Unknown error (0x" + Convert.ToString(error, 16) + ")";
			}
			return text;
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00027FB5 File Offset: 0x00026FB5
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("NativeErrorCode", this.nativeErrorCode);
			base.GetObjectData(info, context);
		}

		// Token: 0x04000A9E RID: 2718
		private readonly int nativeErrorCode;
	}
}
