using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000B7 RID: 183
	[XmlFormatExtensionPrefix("http", "http://schemas.xmlsoap.org/wsdl/http/")]
	[XmlFormatExtension("binding", "http://schemas.xmlsoap.org/wsdl/http/", typeof(Binding))]
	public sealed class HttpBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060004CC RID: 1228 RVA: 0x00017E09 File Offset: 0x00016E09
		// (set) Token: 0x060004CD RID: 1229 RVA: 0x00017E11 File Offset: 0x00016E11
		[XmlAttribute("verb")]
		public string Verb
		{
			get
			{
				return this.verb;
			}
			set
			{
				this.verb = value;
			}
		}

		// Token: 0x040003E2 RID: 994
		public const string Namespace = "http://schemas.xmlsoap.org/wsdl/http/";

		// Token: 0x040003E3 RID: 995
		private string verb;
	}
}
