using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000CA RID: 202
	[XmlFormatExtension("mimeXml", "http://schemas.xmlsoap.org/wsdl/mime/", typeof(MimePart), typeof(InputBinding), typeof(OutputBinding))]
	public sealed class MimeXmlBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001B24E File Offset: 0x0001A24E
		// (set) Token: 0x06000568 RID: 1384 RVA: 0x0001B256 File Offset: 0x0001A256
		[XmlAttribute("part")]
		public string Part
		{
			get
			{
				return this.part;
			}
			set
			{
				this.part = value;
			}
		}

		// Token: 0x0400041A RID: 1050
		private string part;
	}
}
