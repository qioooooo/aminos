using System;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x0200009D RID: 157
	public sealed class DiscoveryClientResult
	{
		// Token: 0x06000422 RID: 1058 RVA: 0x00014CC7 File Offset: 0x00013CC7
		public DiscoveryClientResult()
		{
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00014CCF File Offset: 0x00013CCF
		public DiscoveryClientResult(Type referenceType, string url, string filename)
		{
			this.referenceTypeName = ((referenceType == null) ? string.Empty : referenceType.FullName);
			this.url = url;
			this.filename = filename;
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00014CFB File Offset: 0x00013CFB
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x00014D03 File Offset: 0x00013D03
		[XmlAttribute("referenceType")]
		public string ReferenceTypeName
		{
			get
			{
				return this.referenceTypeName;
			}
			set
			{
				this.referenceTypeName = value;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00014D0C File Offset: 0x00013D0C
		// (set) Token: 0x06000427 RID: 1063 RVA: 0x00014D14 File Offset: 0x00013D14
		[XmlAttribute("url")]
		public string Url
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00014D1D File Offset: 0x00013D1D
		// (set) Token: 0x06000429 RID: 1065 RVA: 0x00014D25 File Offset: 0x00013D25
		[XmlAttribute("filename")]
		public string Filename
		{
			get
			{
				return this.filename;
			}
			set
			{
				this.filename = value;
			}
		}

		// Token: 0x040003A2 RID: 930
		private string referenceTypeName;

		// Token: 0x040003A3 RID: 931
		private string url;

		// Token: 0x040003A4 RID: 932
		private string filename;
	}
}
