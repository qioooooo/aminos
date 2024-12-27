using System;

namespace System.Runtime.Remoting.MetadataServices
{
	// Token: 0x0200006D RID: 109
	public class ServiceType
	{
		// Token: 0x06000371 RID: 881 RVA: 0x00010797 File Offset: 0x0000F797
		public ServiceType(Type type)
		{
			this._type = type;
			this._url = null;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x000107AD File Offset: 0x0000F7AD
		public ServiceType(Type type, string url)
		{
			this._type = type;
			this._url = url;
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000373 RID: 883 RVA: 0x000107C3 File Offset: 0x0000F7C3
		public Type ObjectType
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000374 RID: 884 RVA: 0x000107CB File Offset: 0x0000F7CB
		public string Url
		{
			get
			{
				return this._url;
			}
		}

		// Token: 0x0400027C RID: 636
		private Type _type;

		// Token: 0x0400027D RID: 637
		private string _url;
	}
}
