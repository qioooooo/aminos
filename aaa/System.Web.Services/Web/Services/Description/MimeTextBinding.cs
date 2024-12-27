using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000CC RID: 204
	[XmlFormatExtension("text", "http://microsoft.com/wsdl/mime/textMatching/", typeof(InputBinding), typeof(OutputBinding), typeof(MimePart))]
	[XmlFormatExtensionPrefix("tm", "http://microsoft.com/wsdl/mime/textMatching/")]
	public sealed class MimeTextBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x0001B2E7 File Offset: 0x0001A2E7
		[XmlElement("match", typeof(MimeTextMatch))]
		public MimeTextMatchCollection Matches
		{
			get
			{
				return this.matches;
			}
		}

		// Token: 0x0400041B RID: 1051
		public const string Namespace = "http://microsoft.com/wsdl/mime/textMatching/";

		// Token: 0x0400041C RID: 1052
		private MimeTextMatchCollection matches = new MimeTextMatchCollection();
	}
}
