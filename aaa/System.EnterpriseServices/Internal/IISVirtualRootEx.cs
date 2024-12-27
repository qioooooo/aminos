using System;
using System.DirectoryServices;
using System.Globalization;
using System.IO;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000EF RID: 239
	internal static class IISVirtualRootEx
	{
		// Token: 0x06000563 RID: 1379 RVA: 0x00012778 File Offset: 0x00011778
		internal static bool CheckIfExists(string rootWeb, string virtualDirectory)
		{
			DirectoryEntry directoryEntry = new DirectoryEntry(rootWeb + "/" + virtualDirectory);
			try
			{
				string name = directoryEntry.Name;
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x000127B8 File Offset: 0x000117B8
		internal static void GetStatus(string RootWeb, string PhysicalPath, string VirtualDirectory, out bool bExists, out bool bSSL, out bool bWindowsAuth, out bool bAnonymous, out bool bHomePage, out bool bDiscoFile)
		{
			bSSL = false;
			bWindowsAuth = false;
			bAnonymous = false;
			bHomePage = false;
			bDiscoFile = false;
			bExists = IISVirtualRootEx.CheckIfExists(RootWeb, VirtualDirectory);
			if (!bExists)
			{
				return;
			}
			DirectoryEntry directoryEntry = new DirectoryEntry(RootWeb);
			if (directoryEntry == null)
			{
				return;
			}
			DirectoryEntry directoryEntry2 = directoryEntry.Children.Find(VirtualDirectory, "IIsWebVirtualDir");
			if (directoryEntry2 == null)
			{
				return;
			}
			uint num = uint.Parse(directoryEntry2.Properties["AccessSSLFlags"][0].ToString(), CultureInfo.InvariantCulture);
			if ((num & 8U) > 0U)
			{
				bSSL = true;
			}
			uint num2 = uint.Parse(directoryEntry2.Properties["AuthFlags"][0].ToString(), CultureInfo.InvariantCulture);
			if ((num2 & 1U) > 0U)
			{
				bAnonymous = true;
			}
			if ((num2 & 4U) > 0U)
			{
				bWindowsAuth = true;
			}
			bHomePage = (bool)directoryEntry2.Properties["EnableDefaultDoc"][0];
			if (File.Exists(PhysicalPath + "\\default.disco"))
			{
				bDiscoFile = true;
			}
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x000128A8 File Offset: 0x000118A8
		internal static void CreateOrModify(string rootWeb, string inPhysicalDirectory, string virtualDirectory, bool secureSockets, bool windowsAuth, bool anonymous, bool homePage)
		{
			string text = inPhysicalDirectory;
			while (text.EndsWith("/", StringComparison.Ordinal) || text.EndsWith("\\", StringComparison.Ordinal))
			{
				text = text.Remove(text.Length - 1, 1);
			}
			bool flag = IISVirtualRootEx.CheckIfExists(rootWeb, virtualDirectory);
			DirectoryEntry directoryEntry = new DirectoryEntry(rootWeb);
			DirectoryEntry directoryEntry2;
			if (flag)
			{
				directoryEntry2 = directoryEntry.Children.Find(virtualDirectory, "IIsWebVirtualDir");
			}
			else
			{
				directoryEntry2 = directoryEntry.Children.Add(virtualDirectory, "IIsWebVirtualDir");
			}
			if (directoryEntry2 == null)
			{
				throw new ServicedComponentException(Resource.FormatString("Soap_VRootCreationFailed"));
			}
			directoryEntry2.CommitChanges();
			directoryEntry2.Properties["Path"][0] = text;
			if (secureSockets)
			{
				uint num = uint.Parse(directoryEntry2.Properties["AccessSSLFlags"][0].ToString(), CultureInfo.InvariantCulture);
				num |= 8U;
				directoryEntry2.Properties["AccessSSLFlags"][0] = num;
			}
			uint num2 = uint.Parse(directoryEntry2.Properties["AuthFlags"][0].ToString(), CultureInfo.InvariantCulture);
			if (!flag && anonymous)
			{
				num2 |= 1U;
			}
			if (windowsAuth)
			{
				num2 = 4U;
			}
			directoryEntry2.Properties["AuthFlags"][0] = num2;
			directoryEntry2.Properties["EnableDefaultDoc"][0] = homePage;
			if (secureSockets && windowsAuth && !anonymous)
			{
				directoryEntry2.Properties["DirBrowseFlags"][0] = 0U;
			}
			else if (!flag)
			{
				directoryEntry2.Properties["DirBrowseFlags"][0] = 1073741854U;
			}
			directoryEntry2.Properties["AccessFlags"][0] = 513U;
			directoryEntry2.Properties["AppFriendlyName"][0] = virtualDirectory;
			directoryEntry2.CommitChanges();
			directoryEntry2.Invoke("AppCreate2", new object[] { 2 });
		}

		// Token: 0x04000244 RID: 580
		private const uint MD_ACCESS_SSL = 8U;

		// Token: 0x04000245 RID: 581
		private const uint MD_AUTH_ANONYMOUS = 1U;

		// Token: 0x04000246 RID: 582
		private const uint MD_AUTH_NT = 4U;

		// Token: 0x04000247 RID: 583
		private const uint MD_DIRBROW_NONE = 0U;

		// Token: 0x04000248 RID: 584
		private const uint MD_DIRBROW_LOADDEFAULT = 1073741854U;

		// Token: 0x04000249 RID: 585
		private const uint MD_ACCESS_READ = 1U;

		// Token: 0x0400024A RID: 586
		private const uint MD_ACCESS_SCRIPT = 512U;

		// Token: 0x0400024B RID: 587
		private const int POOLED = 2;
	}
}
