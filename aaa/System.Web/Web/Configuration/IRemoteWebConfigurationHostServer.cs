using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000209 RID: 521
	[ComVisible(true)]
	[Guid("A99B591A-23C6-4238-8452-C7B0E895063D")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IRemoteWebConfigurationHostServer
	{
		// Token: 0x06001C2B RID: 7211
		byte[] GetData(string fileName, bool getReadTimeOnly, out long readTime);

		// Token: 0x06001C2C RID: 7212
		void WriteData(string fileName, string templateFileName, byte[] data, ref long readTime);

		// Token: 0x06001C2D RID: 7213
		string GetFilePaths(int webLevel, string path, string site, string locationSubPath);

		// Token: 0x06001C2E RID: 7214
		string DoEncryptOrDecrypt(bool doEncrypt, string xmlString, string protectionProviderName, string protectionProviderType, string[] parameterKeys, string[] parameterValues);

		// Token: 0x06001C2F RID: 7215
		void GetFileDetails(string name, out bool exists, out long size, out long createDate, out long lastWriteDate);
	}
}
