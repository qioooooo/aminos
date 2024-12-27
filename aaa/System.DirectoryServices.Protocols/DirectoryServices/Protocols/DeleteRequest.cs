using System;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000035 RID: 53
	public class DeleteRequest : DirectoryRequest
	{
		// Token: 0x060000FF RID: 255 RVA: 0x000054C8 File Offset: 0x000044C8
		public DeleteRequest()
		{
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000054D0 File Offset: 0x000044D0
		public DeleteRequest(string distinguishedName)
		{
			this.dn = distinguishedName;
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000101 RID: 257 RVA: 0x000054DF File Offset: 0x000044DF
		// (set) Token: 0x06000102 RID: 258 RVA: 0x000054E7 File Offset: 0x000044E7
		public string DistinguishedName
		{
			get
			{
				return this.dn;
			}
			set
			{
				this.dn = value;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x000054F0 File Offset: 0x000044F0
		protected override XmlElement ToXmlNode(XmlDocument doc)
		{
			return base.CreateRequestElement(doc, "delRequest", true, this.dn);
		}

		// Token: 0x04000104 RID: 260
		private string dn;
	}
}
