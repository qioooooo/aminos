using System;
using System.Drawing.Printing;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing
{
	// Token: 0x0200004C RID: 76
	internal static class IntSecurity
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x000128EC File Offset: 0x000118EC
		internal static void DemandReadFileIO(string fileName)
		{
			string text = IntSecurity.UnsafeGetFullPath(fileName);
			new FileIOPermission(FileIOPermissionAccess.Read, text).Demand();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00012910 File Offset: 0x00011910
		internal static void DemandWriteFileIO(string fileName)
		{
			string text = IntSecurity.UnsafeGetFullPath(fileName);
			new FileIOPermission(FileIOPermissionAccess.Write, text).Demand();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00012934 File Offset: 0x00011934
		internal static string UnsafeGetFullPath(string fileName)
		{
			string text = fileName;
			new FileIOPermission(PermissionState.None)
			{
				AllFiles = FileIOPermissionAccess.PathDiscovery
			}.Assert();
			try
			{
				text = Path.GetFullPath(fileName);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return text;
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x00012978 File Offset: 0x00011978
		public static PermissionSet AllPrintingAndUnmanagedCode
		{
			get
			{
				if (IntSecurity.allPrintingAndUnmanagedCode == null)
				{
					PermissionSet permissionSet = new PermissionSet(PermissionState.None);
					permissionSet.SetPermission(IntSecurity.UnmanagedCode);
					permissionSet.SetPermission(IntSecurity.AllPrinting);
					IntSecurity.allPrintingAndUnmanagedCode = permissionSet;
				}
				return IntSecurity.allPrintingAndUnmanagedCode;
			}
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000129B8 File Offset: 0x000119B8
		internal static bool HasPermission(PrintingPermission permission)
		{
			bool flag;
			try
			{
				permission.Demand();
				flag = true;
			}
			catch (SecurityException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x040002C8 RID: 712
		private static readonly UIPermission AllWindows = new UIPermission(UIPermissionWindow.AllWindows);

		// Token: 0x040002C9 RID: 713
		private static readonly UIPermission SafeSubWindows = new UIPermission(UIPermissionWindow.SafeSubWindows);

		// Token: 0x040002CA RID: 714
		public static readonly CodeAccessPermission UnmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

		// Token: 0x040002CB RID: 715
		public static readonly CodeAccessPermission ObjectFromWin32Handle = IntSecurity.UnmanagedCode;

		// Token: 0x040002CC RID: 716
		public static readonly CodeAccessPermission Win32HandleManipulation = IntSecurity.UnmanagedCode;

		// Token: 0x040002CD RID: 717
		public static readonly PrintingPermission NoPrinting = new PrintingPermission(PrintingPermissionLevel.NoPrinting);

		// Token: 0x040002CE RID: 718
		public static readonly PrintingPermission SafePrinting = new PrintingPermission(PrintingPermissionLevel.SafePrinting);

		// Token: 0x040002CF RID: 719
		public static readonly PrintingPermission DefaultPrinting = new PrintingPermission(PrintingPermissionLevel.DefaultPrinting);

		// Token: 0x040002D0 RID: 720
		public static readonly PrintingPermission AllPrinting = new PrintingPermission(PrintingPermissionLevel.AllPrinting);

		// Token: 0x040002D1 RID: 721
		private static PermissionSet allPrintingAndUnmanagedCode;
	}
}
