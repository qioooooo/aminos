using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;

namespace System.Resources
{
	// Token: 0x0200014B RID: 331
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ResXResourceSet : ResourceSet
	{
		// Token: 0x0600052C RID: 1324 RVA: 0x0000D981 File Offset: 0x0000C981
		public ResXResourceSet(string fileName)
		{
			this.Reader = new ResXResourceReader(fileName);
			this.Table = new Hashtable();
			this.ReadResources();
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000D9A6 File Offset: 0x0000C9A6
		public ResXResourceSet(Stream stream)
		{
			this.Reader = new ResXResourceReader(stream);
			this.Table = new Hashtable();
			this.ReadResources();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0000D9CB File Offset: 0x0000C9CB
		public override Type GetDefaultReader()
		{
			return typeof(ResXResourceReader);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000D9D7 File Offset: 0x0000C9D7
		public override Type GetDefaultWriter()
		{
			return typeof(ResXResourceWriter);
		}
	}
}
