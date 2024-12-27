using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000C9 RID: 201
	[XmlFormatExtension("multipartRelated", "http://schemas.xmlsoap.org/wsdl/mime/", typeof(InputBinding), typeof(OutputBinding))]
	public sealed class MimeMultipartRelatedBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0001B233 File Offset: 0x0001A233
		[XmlElement("part")]
		public MimePartCollection Parts
		{
			get
			{
				return this.parts;
			}
		}

		// Token: 0x04000419 RID: 1049
		private MimePartCollection parts = new MimePartCollection();
	}
}
