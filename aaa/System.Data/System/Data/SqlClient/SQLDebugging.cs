using System;
using System.Data.Common;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.Data.SqlClient
{
	// Token: 0x020002CD RID: 717
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("afef65ad-4577-447a-a148-83acadd3d4b9")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class SQLDebugging : ISQLDebug
	{
		// Token: 0x060024EF RID: 9455 RVA: 0x00279A9C File Offset: 0x00278E9C
		private IntPtr CreateSD(ref IntPtr pDacl)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			IntPtr zero2 = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			bool flag = false;
			intPtr2 = Marshal.AllocHGlobal(6);
			if (!(intPtr2 == IntPtr.Zero))
			{
				Marshal.WriteInt32(intPtr2, 0, 0);
				Marshal.WriteByte(intPtr2, 4, 0);
				Marshal.WriteByte(intPtr2, 5, 5);
				flag = NativeMethods.AllocateAndInitializeSid(intPtr2, 1, 11, 0, 0, 0, 0, 0, 0, 0, ref zero);
				if (flag && !(zero == IntPtr.Zero))
				{
					flag = NativeMethods.AllocateAndInitializeSid(intPtr2, 2, 32, 544, 0, 0, 0, 0, 0, 0, ref zero2);
					if (flag && !(zero2 == IntPtr.Zero))
					{
						flag = false;
						intPtr = Marshal.AllocHGlobal(20);
						if (!(intPtr == IntPtr.Zero))
						{
							for (int i = 0; i < 20; i++)
							{
								Marshal.WriteByte(intPtr, i, 0);
							}
							int num = 44 + NativeMethods.GetLengthSid(zero) + NativeMethods.GetLengthSid(zero2);
							pDacl = Marshal.AllocHGlobal(num);
							if (!(pDacl == IntPtr.Zero) && NativeMethods.InitializeAcl(pDacl, num, 2) && NativeMethods.AddAccessDeniedAce(pDacl, 2, 262144, zero) && NativeMethods.AddAccessAllowedAce(pDacl, 2, 2147483648U, zero) && NativeMethods.AddAccessAllowedAce(pDacl, 2, 268435456U, zero2) && NativeMethods.InitializeSecurityDescriptor(intPtr, 1) && NativeMethods.SetSecurityDescriptorDacl(intPtr, true, pDacl, false))
							{
								flag = true;
							}
						}
					}
				}
			}
			if (intPtr2 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr2);
			}
			if (zero2 != IntPtr.Zero)
			{
				NativeMethods.FreeSid(zero2);
			}
			if (zero != IntPtr.Zero)
			{
				NativeMethods.FreeSid(zero);
			}
			if (flag)
			{
				return intPtr;
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return IntPtr.Zero;
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x00279C78 File Offset: 0x00279078
		bool ISQLDebug.SQLDebug(int dwpidDebugger, int dwpidDebuggee, [MarshalAs(UnmanagedType.LPStr)] string pszMachineName, [MarshalAs(UnmanagedType.LPStr)] string pszSDIDLLName, int dwOption, int cbData, byte[] rgbData)
		{
			bool flag = false;
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr intPtr4 = IntPtr.Zero;
			IntPtr zero = IntPtr.Zero;
			if (pszMachineName == null || pszSDIDLLName == null)
			{
				return false;
			}
			if (pszMachineName.Length > 32 || pszSDIDLLName.Length > 16)
			{
				return false;
			}
			Encoding encoding = Encoding.GetEncoding(1252);
			byte[] bytes = encoding.GetBytes(pszMachineName);
			byte[] bytes2 = encoding.GetBytes(pszSDIDLLName);
			if (rgbData != null && cbData > 255)
			{
				return false;
			}
			string text;
			if (ADP.IsPlatformNT5)
			{
				text = "Global\\SqlClientSSDebug";
			}
			else
			{
				text = "SqlClientSSDebug";
			}
			text += dwpidDebuggee.ToString(CultureInfo.InvariantCulture);
			intPtr3 = this.CreateSD(ref zero);
			intPtr4 = Marshal.AllocHGlobal(12);
			if (intPtr3 == IntPtr.Zero || intPtr4 == IntPtr.Zero)
			{
				return false;
			}
			Marshal.WriteInt32(intPtr4, 0, 12);
			Marshal.WriteIntPtr(intPtr4, 4, intPtr3);
			Marshal.WriteInt32(intPtr4, 8, 0);
			intPtr = NativeMethods.CreateFileMappingA(ADP.InvalidPtr, intPtr4, 4, 0, Marshal.SizeOf(typeof(MEMMAP)), text);
			if (!(IntPtr.Zero == intPtr))
			{
				intPtr2 = NativeMethods.MapViewOfFile(intPtr, 6, 0, 0, IntPtr.Zero);
				if (!(IntPtr.Zero == intPtr2))
				{
					int num = 0;
					Marshal.WriteInt32(intPtr2, num, dwpidDebugger);
					num += 4;
					Marshal.WriteInt32(intPtr2, num, dwOption);
					num += 4;
					Marshal.Copy(bytes, 0, ADP.IntPtrOffset(intPtr2, num), bytes.Length);
					num += 32;
					Marshal.Copy(bytes2, 0, ADP.IntPtrOffset(intPtr2, num), bytes2.Length);
					num += 16;
					Marshal.WriteInt32(intPtr2, num, cbData);
					num += 4;
					if (rgbData != null)
					{
						Marshal.Copy(rgbData, 0, ADP.IntPtrOffset(intPtr2, num), cbData);
					}
					NativeMethods.UnmapViewOfFile(intPtr2);
					flag = true;
				}
			}
			if (!flag && intPtr != IntPtr.Zero)
			{
				NativeMethods.CloseHandle(intPtr);
			}
			if (intPtr4 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr4);
			}
			if (intPtr3 != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr3);
			}
			if (zero != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(zero);
			}
			return flag;
		}

		// Token: 0x04001756 RID: 5974
		private const int STANDARD_RIGHTS_REQUIRED = 983040;

		// Token: 0x04001757 RID: 5975
		private const int DELETE = 65536;

		// Token: 0x04001758 RID: 5976
		private const int READ_CONTROL = 131072;

		// Token: 0x04001759 RID: 5977
		private const int WRITE_DAC = 262144;

		// Token: 0x0400175A RID: 5978
		private const int WRITE_OWNER = 524288;

		// Token: 0x0400175B RID: 5979
		private const int SYNCHRONIZE = 1048576;

		// Token: 0x0400175C RID: 5980
		private const int FILE_ALL_ACCESS = 2032127;

		// Token: 0x0400175D RID: 5981
		private const uint GENERIC_READ = 2147483648U;

		// Token: 0x0400175E RID: 5982
		private const uint GENERIC_WRITE = 1073741824U;

		// Token: 0x0400175F RID: 5983
		private const uint GENERIC_EXECUTE = 536870912U;

		// Token: 0x04001760 RID: 5984
		private const uint GENERIC_ALL = 268435456U;

		// Token: 0x04001761 RID: 5985
		private const int SECURITY_DESCRIPTOR_REVISION = 1;

		// Token: 0x04001762 RID: 5986
		private const int ACL_REVISION = 2;

		// Token: 0x04001763 RID: 5987
		private const int SECURITY_AUTHENTICATED_USER_RID = 11;

		// Token: 0x04001764 RID: 5988
		private const int SECURITY_LOCAL_SYSTEM_RID = 18;

		// Token: 0x04001765 RID: 5989
		private const int SECURITY_BUILTIN_DOMAIN_RID = 32;

		// Token: 0x04001766 RID: 5990
		private const int SECURITY_WORLD_RID = 0;

		// Token: 0x04001767 RID: 5991
		private const byte SECURITY_NT_AUTHORITY = 5;

		// Token: 0x04001768 RID: 5992
		private const int DOMAIN_GROUP_RID_ADMINS = 512;

		// Token: 0x04001769 RID: 5993
		private const int DOMAIN_ALIAS_RID_ADMINS = 544;

		// Token: 0x0400176A RID: 5994
		private const int sizeofSECURITY_ATTRIBUTES = 12;

		// Token: 0x0400176B RID: 5995
		private const int sizeofSECURITY_DESCRIPTOR = 20;

		// Token: 0x0400176C RID: 5996
		private const int sizeofACCESS_ALLOWED_ACE = 12;

		// Token: 0x0400176D RID: 5997
		private const int sizeofACCESS_DENIED_ACE = 12;

		// Token: 0x0400176E RID: 5998
		private const int sizeofSID_IDENTIFIER_AUTHORITY = 6;

		// Token: 0x0400176F RID: 5999
		private const int sizeofACL = 8;
	}
}
