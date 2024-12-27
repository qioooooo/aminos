using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000C7 RID: 199
	[XmlFormatExtension("content", "http://schemas.xmlsoap.org/wsdl/mime/", typeof(MimePart), typeof(InputBinding), typeof(OutputBinding))]
	[XmlFormatExtensionPrefix("mime", "http://schemas.xmlsoap.org/wsdl/mime/")]
	public sealed class MimeContentBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0001B1D7 File Offset: 0x0001A1D7
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x0001B1DF File Offset: 0x0001A1DF
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

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000560 RID: 1376 RVA: 0x0001B1E8 File Offset: 0x0001A1E8
		// (set) Token: 0x06000561 RID: 1377 RVA: 0x0001B1FE File Offset: 0x0001A1FE
		[XmlAttribute("type")]
		public string Type
		{
			get
			{
				if (this.type != null)
				{
					return this.type;
				}
				return string.Empty;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x04000415 RID: 1045
		public const string Namespace = "http://schemas.xmlsoap.org/wsdl/mime/";

		// Token: 0x04000416 RID: 1046
		private string type;

		// Token: 0x04000417 RID: 1047
		private string part;
	}
}
