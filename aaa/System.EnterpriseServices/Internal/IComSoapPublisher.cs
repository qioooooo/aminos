using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000C5 RID: 197
	[Guid("d8013eee-730b-45e2-ba24-874b7242c425")]
	public interface IComSoapPublisher
	{
		// Token: 0x0600048A RID: 1162
		[DispId(4)]
		void CreateVirtualRoot([MarshalAs(UnmanagedType.BStr)] string Operation, [MarshalAs(UnmanagedType.BStr)] string FullUrl, [MarshalAs(UnmanagedType.BStr)] out string BaseUrl, [MarshalAs(UnmanagedType.BStr)] out string VirtualRoot, [MarshalAs(UnmanagedType.BStr)] out string PhysicalPath, [MarshalAs(UnmanagedType.BStr)] out string Error);

		// Token: 0x0600048B RID: 1163
		[DispId(5)]
		void DeleteVirtualRoot([MarshalAs(UnmanagedType.BStr)] string RootWebServer, [MarshalAs(UnmanagedType.BStr)] string FullUrl, [MarshalAs(UnmanagedType.BStr)] out string Error);

		// Token: 0x0600048C RID: 1164
		[DispId(6)]
		void CreateMailBox([MarshalAs(UnmanagedType.BStr)] string RootMailServer, [MarshalAs(UnmanagedType.BStr)] string MailBox, [MarshalAs(UnmanagedType.BStr)] out string SmtpName, [MarshalAs(UnmanagedType.BStr)] out string Domain, [MarshalAs(UnmanagedType.BStr)] out string PhysicalPath, [MarshalAs(UnmanagedType.BStr)] out string Error);

		// Token: 0x0600048D RID: 1165
		[DispId(7)]
		void DeleteMailBox([MarshalAs(UnmanagedType.BStr)] string RootMailServer, [MarshalAs(UnmanagedType.BStr)] string MailBox, [MarshalAs(UnmanagedType.BStr)] out string Error);

		// Token: 0x0600048E RID: 1166
		[DispId(8)]
		void ProcessServerTlb([MarshalAs(UnmanagedType.BStr)] string ProgId, [MarshalAs(UnmanagedType.BStr)] string SrcTlbPath, [MarshalAs(UnmanagedType.BStr)] string PhysicalPath, [MarshalAs(UnmanagedType.BStr)] string Operation, [MarshalAs(UnmanagedType.BStr)] out string AssemblyName, [MarshalAs(UnmanagedType.BStr)] out string TypeName, [MarshalAs(UnmanagedType.BStr)] out string Error);

		// Token: 0x0600048F RID: 1167
		[DispId(9)]
		void ProcessClientTlb([MarshalAs(UnmanagedType.BStr)] string ProgId, [MarshalAs(UnmanagedType.BStr)] string SrcTlbPath, [MarshalAs(UnmanagedType.BStr)] string PhysicalPath, [MarshalAs(UnmanagedType.BStr)] string VRoot, [MarshalAs(UnmanagedType.BStr)] string BaseUrl, [MarshalAs(UnmanagedType.BStr)] string Mode, [MarshalAs(UnmanagedType.BStr)] string Transport, [MarshalAs(UnmanagedType.BStr)] out string AssemblyName, [MarshalAs(UnmanagedType.BStr)] out string TypeName, [MarshalAs(UnmanagedType.BStr)] out string Error);

		// Token: 0x06000490 RID: 1168
		[DispId(10)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetTypeNameFromProgId([MarshalAs(UnmanagedType.BStr)] string AssemblyPath, [MarshalAs(UnmanagedType.BStr)] string ProgId);

		// Token: 0x06000491 RID: 1169
		[DispId(11)]
		void RegisterAssembly([MarshalAs(UnmanagedType.BStr)] string AssemblyPath);

		// Token: 0x06000492 RID: 1170
		[DispId(12)]
		void UnRegisterAssembly([MarshalAs(UnmanagedType.BStr)] string AssemblyPath);

		// Token: 0x06000493 RID: 1171
		[DispId(13)]
		void GacInstall([MarshalAs(UnmanagedType.BStr)] string AssemblyPath);

		// Token: 0x06000494 RID: 1172
		[DispId(14)]
		void GacRemove([MarshalAs(UnmanagedType.BStr)] string AssemblyPath);

		// Token: 0x06000495 RID: 1173
		[DispId(15)]
		void GetAssemblyNameForCache([MarshalAs(UnmanagedType.BStr)] string TypeLibPath, [MarshalAs(UnmanagedType.BStr)] out string CachePath);
	}
}
