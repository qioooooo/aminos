using System;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000D7 RID: 215
	internal class MimeXmlReturn : MimeReturn
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x0001BBE7 File Offset: 0x0001ABE7
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x0001BBEF File Offset: 0x0001ABEF
		internal XmlTypeMapping TypeMapping
		{
			get
			{
				return this.mapping;
			}
			set
			{
				this.mapping = value;
			}
		}

		// Token: 0x0400042F RID: 1071
		private XmlTypeMapping mapping;
	}
}
