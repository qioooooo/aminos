﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using Microsoft.Win32;

namespace System.Security.AccessControl
{
	// Token: 0x02000928 RID: 2344
	internal static class Win32
	{
		// Token: 0x0600550F RID: 21775 RVA: 0x00135080 File Offset: 0x00134080
		static Win32()
		{
			Win32Native.OSVERSIONINFO osversioninfo = new Win32Native.OSVERSIONINFO();
			if (!Win32Native.GetVersionEx(osversioninfo))
			{
				throw new SystemException(Environment.GetResourceString("InvalidOperation_GetVersion"));
			}
			if (osversioninfo.PlatformId == 2 && osversioninfo.MajorVersion >= 5)
			{
				Win32._isConversionSupported = true;
				return;
			}
			Win32._isConversionSupported = false;
		}

		// Token: 0x06005510 RID: 21776 RVA: 0x001350CA File Offset: 0x001340CA
		internal static bool IsSddlConversionSupported()
		{
			return Win32._isConversionSupported;
		}

		// Token: 0x06005511 RID: 21777 RVA: 0x001350D1 File Offset: 0x001340D1
		internal static bool IsLsaPolicySupported()
		{
			return Win32._isConversionSupported;
		}

		// Token: 0x06005512 RID: 21778 RVA: 0x001350D8 File Offset: 0x001340D8
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		internal static int ConvertSdToSddl(byte[] binaryForm, int requestedRevision, SecurityInfos si, out string resultSddl)
		{
			uint num = 0U;
			if (!Win32.IsSddlConversionSupported())
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			IntPtr intPtr;
			if (1 == Win32Native.ConvertSdToStringSd(binaryForm, (uint)requestedRevision, (uint)si, out intPtr, ref num))
			{
				resultSddl = Marshal.PtrToStringUni(intPtr);
				Win32Native.LocalFree(intPtr);
				return 0;
			}
			int lastWin32Error = Marshal.GetLastWin32Error();
			resultSddl = null;
			if (lastWin32Error == 8)
			{
				throw new OutOfMemoryException();
			}
			return lastWin32Error;
		}

		// Token: 0x06005513 RID: 21779 RVA: 0x00135134 File Offset: 0x00134134
		internal static int GetSecurityInfo(ResourceType resourceType, string name, SafeHandle handle, AccessControlSections accessControlSections, out RawSecurityDescriptor resultSd)
		{
			resultSd = null;
			if (!Win32.IsLsaPolicySupported())
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresNT"));
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			SecurityInfos securityInfos = (SecurityInfos)0;
			Privilege privilege = null;
			if ((accessControlSections & AccessControlSections.Owner) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.Owner;
			}
			if ((accessControlSections & AccessControlSections.Group) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.Group;
			}
			if ((accessControlSections & AccessControlSections.Access) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.DiscretionaryAcl;
			}
			if ((accessControlSections & AccessControlSections.Audit) != AccessControlSections.None)
			{
				securityInfos |= SecurityInfos.SystemAcl;
				privilege = new Privilege("SeSecurityPrivilege");
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			IntPtr intPtr5;
			int num;
			try
			{
				if (privilege != null)
				{
					try
					{
						privilege.Enable();
					}
					catch (PrivilegeNotHeldException)
					{
					}
				}
				if (name != null)
				{
					IntPtr intPtr;
					IntPtr intPtr2;
					IntPtr intPtr3;
					IntPtr intPtr4;
					num = (int)Win32Native.GetSecurityInfoByName(name, (uint)resourceType, (uint)securityInfos, out intPtr, out intPtr2, out intPtr3, out intPtr4, out intPtr5);
				}
				else
				{
					if (handle == null)
					{
						throw new SystemException();
					}
					if (handle.IsInvalid)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSafeHandle"), "handle");
					}
					IntPtr intPtr;
					IntPtr intPtr2;
					IntPtr intPtr3;
					IntPtr intPtr4;
					num = (int)Win32Native.GetSecurityInfoByHandle(handle, (uint)resourceType, (uint)securityInfos, out intPtr, out intPtr2, out intPtr3, out intPtr4, out intPtr5);
				}
				if (num == 0 && IntPtr.Zero.Equals(intPtr5))
				{
					throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_NoSecurityDescriptor"));
				}
				if (num == 1300 || num == 1314)
				{
					throw new PrivilegeNotHeldException("SeSecurityPrivilege");
				}
				if (num == 5 || num == 1347)
				{
					throw new UnauthorizedAccessException();
				}
				if (num != 0)
				{
					goto IL_0182;
				}
			}
			catch
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
				throw;
			}
			finally
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
			}
			uint securityDescriptorLength = Win32Native.GetSecurityDescriptorLength(intPtr5);
			byte[] array = new byte[securityDescriptorLength];
			Marshal.Copy(intPtr5, array, 0, (int)securityDescriptorLength);
			Win32Native.LocalFree(intPtr5);
			resultSd = new RawSecurityDescriptor(array, 0);
			return 0;
			IL_0182:
			if (num == 8)
			{
				throw new OutOfMemoryException();
			}
			return num;
		}

		// Token: 0x06005514 RID: 21780 RVA: 0x001352F8 File Offset: 0x001342F8
		internal static int SetSecurityInfo(ResourceType type, string name, SafeHandle handle, SecurityInfos securityInformation, SecurityIdentifier owner, SecurityIdentifier group, GenericAcl sacl, GenericAcl dacl)
		{
			if (!Win32.IsLsaPolicySupported())
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresNT"));
			}
			byte[] array = null;
			byte[] array2 = null;
			byte[] array3 = null;
			byte[] array4 = null;
			Privilege privilege = null;
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			if (owner != null)
			{
				int num = owner.BinaryLength;
				array = new byte[num];
				owner.GetBinaryForm(array, 0);
			}
			if (group != null)
			{
				int num = group.BinaryLength;
				array2 = new byte[num];
				group.GetBinaryForm(array2, 0);
			}
			if (dacl != null)
			{
				int num = dacl.BinaryLength;
				array4 = new byte[num];
				dacl.GetBinaryForm(array4, 0);
			}
			if (sacl != null)
			{
				int num = sacl.BinaryLength;
				array3 = new byte[num];
				sacl.GetBinaryForm(array3, 0);
			}
			if ((securityInformation & SecurityInfos.SystemAcl) != (SecurityInfos)0)
			{
				privilege = new Privilege("SeSecurityPrivilege");
			}
			RuntimeHelpers.PrepareConstrainedRegions();
			int num2;
			try
			{
				if (privilege != null)
				{
					try
					{
						privilege.Enable();
					}
					catch (PrivilegeNotHeldException)
					{
					}
				}
				if (name != null)
				{
					num2 = (int)Win32Native.SetSecurityInfoByName(name, (uint)type, (uint)securityInformation, array, array2, array4, array3);
				}
				else
				{
					if (handle == null)
					{
						throw new InvalidProgramException();
					}
					if (handle.IsInvalid)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidSafeHandle"), "handle");
					}
					num2 = (int)Win32Native.SetSecurityInfoByHandle(handle, (uint)type, (uint)securityInformation, array, array2, array4, array3);
				}
				if (num2 == 1300 || num2 == 1314)
				{
					throw new PrivilegeNotHeldException("SeSecurityPrivilege");
				}
				if (num2 == 5 || num2 == 1347)
				{
					throw new UnauthorizedAccessException();
				}
				if (num2 != 0)
				{
					goto IL_0172;
				}
			}
			catch
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
				throw;
			}
			finally
			{
				if (privilege != null)
				{
					privilege.Revert();
				}
			}
			return 0;
			IL_0172:
			if (num2 == 8)
			{
				throw new OutOfMemoryException();
			}
			return num2;
		}

		// Token: 0x04002C0F RID: 11279
		internal const int TRUE = 1;

		// Token: 0x04002C10 RID: 11280
		private static bool _isConversionSupported;
	}
}
