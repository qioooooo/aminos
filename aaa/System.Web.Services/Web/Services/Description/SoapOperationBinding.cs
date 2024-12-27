using System;
using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x0200010C RID: 268
	[XmlFormatExtension("operation", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(OperationBinding))]
	public class SoapOperationBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x0003D4AA File Offset: 0x0003C4AA
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x0003D4C0 File Offset: 0x0003C4C0
		[XmlAttribute("soapAction")]
		public string SoapAction
		{
			get
			{
				if (this.soapAction != null)
				{
					return this.soapAction;
				}
				return string.Empty;
			}
			set
			{
				this.soapAction = value;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x0003D4C9 File Offset: 0x0003C4C9
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x0003D4D1 File Offset: 0x0003C4D1
		[DefaultValue(SoapBindingStyle.Default)]
		[XmlAttribute("style")]
		public SoapBindingStyle Style
		{
			get
			{
				return this.style;
			}
			set
			{
				this.style = value;
			}
		}

		// Token: 0x0400058C RID: 1420
		private string soapAction;

		// Token: 0x0400058D RID: 1421
		private SoapBindingStyle style;
	}
}
