using System;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000AE RID: 174
	public sealed class ExcludePathInfo
	{
		// Token: 0x0600049F RID: 1183 RVA: 0x000174D1 File Offset: 0x000164D1
		public ExcludePathInfo()
		{
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x000174D9 File Offset: 0x000164D9
		public ExcludePathInfo(string path)
		{
			this.path = path;
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x000174E8 File Offset: 0x000164E8
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x000174F0 File Offset: 0x000164F0
		[XmlAttribute("path")]
		public string Path
		{
			get
			{
				return this.path;
			}
			set
			{
				this.path = value;
			}
		}

		// Token: 0x040003D0 RID: 976
		private string path;
	}
}
