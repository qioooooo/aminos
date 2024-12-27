using System;
using System.Security;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x020000A9 RID: 169
	internal static class InternalSecurityPermissions
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00025184 File Offset: 0x00024184
		internal static IStackWalk Unrestricted
		{
			get
			{
				if (InternalSecurityPermissions._unrestricted == null)
				{
					InternalSecurityPermissions._unrestricted = new PermissionSet(PermissionState.Unrestricted);
				}
				return InternalSecurityPermissions._unrestricted;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0002519D File Offset: 0x0002419D
		internal static IStackWalk UnmanagedCode
		{
			get
			{
				if (InternalSecurityPermissions._unmanagedCode == null)
				{
					InternalSecurityPermissions._unmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				}
				return InternalSecurityPermissions._unmanagedCode;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x000251B6 File Offset: 0x000241B6
		internal static IStackWalk ControlPrincipal
		{
			get
			{
				if (InternalSecurityPermissions._controlPrincipal == null)
				{
					InternalSecurityPermissions._controlPrincipal = new SecurityPermission(SecurityPermissionFlag.ControlPrincipal);
				}
				return InternalSecurityPermissions._controlPrincipal;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x000251D3 File Offset: 0x000241D3
		internal static IStackWalk Reflection
		{
			get
			{
				if (InternalSecurityPermissions._reflection == null)
				{
					InternalSecurityPermissions._reflection = new ReflectionPermission(ReflectionPermissionFlag.MemberAccess);
				}
				return InternalSecurityPermissions._reflection;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x000251EC File Offset: 0x000241EC
		internal static IStackWalk AppPathDiscovery
		{
			get
			{
				if (InternalSecurityPermissions._appPathDiscovery == null)
				{
					InternalSecurityPermissions._appPathDiscovery = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, HttpRuntime.AppDomainAppPathInternal);
				}
				return InternalSecurityPermissions._appPathDiscovery;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000869 RID: 2153 RVA: 0x0002520A File Offset: 0x0002420A
		internal static IStackWalk ControlThread
		{
			get
			{
				if (InternalSecurityPermissions._controlThread == null)
				{
					InternalSecurityPermissions._controlThread = new SecurityPermission(SecurityPermissionFlag.ControlThread);
				}
				return InternalSecurityPermissions._controlThread;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x00025224 File Offset: 0x00024224
		internal static IStackWalk AspNetHostingPermissionLevelLow
		{
			get
			{
				if (InternalSecurityPermissions._levelLow == null)
				{
					InternalSecurityPermissions._levelLow = new AspNetHostingPermission(AspNetHostingPermissionLevel.Low);
				}
				return InternalSecurityPermissions._levelLow;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x00025241 File Offset: 0x00024241
		internal static IStackWalk AspNetHostingPermissionLevelMedium
		{
			get
			{
				if (InternalSecurityPermissions._levelMedium == null)
				{
					InternalSecurityPermissions._levelMedium = new AspNetHostingPermission(AspNetHostingPermissionLevel.Medium);
				}
				return InternalSecurityPermissions._levelMedium;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x0002525E File Offset: 0x0002425E
		internal static IStackWalk AspNetHostingPermissionLevelHigh
		{
			get
			{
				if (InternalSecurityPermissions._levelHigh == null)
				{
					InternalSecurityPermissions._levelHigh = new AspNetHostingPermission(AspNetHostingPermissionLevel.High);
				}
				return InternalSecurityPermissions._levelHigh;
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x0002527B File Offset: 0x0002427B
		internal static IStackWalk FileReadAccess(string filename)
		{
			return new FileIOPermission(FileIOPermissionAccess.Read, filename);
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00025284 File Offset: 0x00024284
		internal static IStackWalk PathDiscovery(string path)
		{
			return new FileIOPermission(FileIOPermissionAccess.PathDiscovery, path);
		}

		// Token: 0x0400119F RID: 4511
		private static IStackWalk _unrestricted;

		// Token: 0x040011A0 RID: 4512
		private static IStackWalk _unmanagedCode;

		// Token: 0x040011A1 RID: 4513
		private static IStackWalk _controlPrincipal;

		// Token: 0x040011A2 RID: 4514
		private static IStackWalk _reflection;

		// Token: 0x040011A3 RID: 4515
		private static IStackWalk _appPathDiscovery;

		// Token: 0x040011A4 RID: 4516
		private static IStackWalk _controlThread;

		// Token: 0x040011A5 RID: 4517
		private static IStackWalk _levelLow;

		// Token: 0x040011A6 RID: 4518
		private static IStackWalk _levelMedium;

		// Token: 0x040011A7 RID: 4519
		private static IStackWalk _levelHigh;
	}
}
