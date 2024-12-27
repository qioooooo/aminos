using System;
using System.DirectoryServices;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000D4 RID: 212
	[Guid("d8013ef1-730b-45e2-ba24-874b7242c425")]
	public class IISVirtualRoot : IComSoapIISVRoot
	{
		// Token: 0x060004D1 RID: 1233 RVA: 0x0000F588 File Offset: 0x0000E588
		internal bool CheckIfExists(string RootWeb, string VirtualDirectory)
		{
			DirectoryEntry directoryEntry = new DirectoryEntry(RootWeb + "/" + VirtualDirectory);
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

		// Token: 0x060004D2 RID: 1234 RVA: 0x0000F5C8 File Offset: 0x0000E5C8
		public void Create(string RootWeb, string inPhysicalDirectory, string VirtualDirectory, out string Error)
		{
			Error = "";
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				string text = inPhysicalDirectory;
				while (text.EndsWith("/", StringComparison.Ordinal) || text.EndsWith("\\", StringComparison.Ordinal))
				{
					text = text.Remove(text.Length - 1, 1);
				}
				bool flag = this.CheckIfExists(RootWeb, VirtualDirectory);
				if (!flag)
				{
					DirectoryEntry directoryEntry = new DirectoryEntry(RootWeb);
					DirectoryEntry directoryEntry2 = directoryEntry.Children.Add(VirtualDirectory, "IIsWebVirtualDir");
					directoryEntry2.CommitChanges();
					directoryEntry2.Properties["Path"][0] = text;
					directoryEntry2.Properties["AuthFlags"][0] = 5;
					directoryEntry2.Properties["EnableDefaultDoc"][0] = true;
					directoryEntry2.Properties["DirBrowseFlags"][0] = 1073741886;
					directoryEntry2.Properties["AccessFlags"][0] = 513;
					directoryEntry2.CommitChanges();
					directoryEntry2.Invoke("AppCreate2", new object[] { 2 });
					Error = "";
				}
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "IISVirtualRoot.Create"));
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0000F778 File Offset: 0x0000E778
		public void Delete(string RootWeb, string PhysicalDirectory, string VirtualDirectory, out string Error)
		{
			Error = "";
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				if (this.CheckIfExists(RootWeb, VirtualDirectory))
				{
					DirectoryEntry directoryEntry = new DirectoryEntry(RootWeb);
					DirectoryEntry directoryEntry2 = new DirectoryEntry(RootWeb + "/" + VirtualDirectory);
					directoryEntry2.Invoke("AppDelete", null);
					directoryEntry.Invoke("Delete", new object[] { "IIsWebVirtualDir", VirtualDirectory });
					Directory.Delete(PhysicalDirectory, true);
				}
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "IISVirtualRoot.Delete"));
			}
		}
	}
}
