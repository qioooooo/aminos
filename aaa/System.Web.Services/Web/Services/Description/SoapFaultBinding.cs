using System;
using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000110 RID: 272
	[XmlFormatExtension("fault", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(FaultBinding))]
	public class SoapFaultBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x0003D626 File Offset: 0x0003C626
		// (set) Token: 0x06000849 RID: 2121 RVA: 0x0003D62E File Offset: 0x0003C62E
		[XmlAttribute("use")]
		[DefaultValue(SoapBindingUse.Default)]
		public SoapBindingUse Use
		{
			get
			{
				return this.use;
			}
			set
			{
				this.use = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x0003D637 File Offset: 0x0003C637
		// (set) Token: 0x0600084B RID: 2123 RVA: 0x0003D63F File Offset: 0x0003C63F
		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600084C RID: 2124 RVA: 0x0003D648 File Offset: 0x0003C648
		// (set) Token: 0x0600084D RID: 2125 RVA: 0x0003D65E File Offset: 0x0003C65E
		[XmlAttribute("namespace")]
		public string Namespace
		{
			get
			{
				if (this.ns != null)
				{
					return this.ns;
				}
				return string.Empty;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600084E RID: 2126 RVA: 0x0003D667 File Offset: 0x0003C667
		// (set) Token: 0x0600084F RID: 2127 RVA: 0x0003D67D File Offset: 0x0003C67D
		[DefaultValue("")]
		[XmlAttribute("encodingStyle")]
		public string Encoding
		{
			get
			{
				if (this.encoding != null)
				{
					return this.encoding;
				}
				return string.Empty;
			}
			set
			{
				this.encoding = value;
			}
		}

		// Token: 0x04000596 RID: 1430
		private SoapBindingUse use;

		// Token: 0x04000597 RID: 1431
		private string ns;

		// Token: 0x04000598 RID: 1432
		private string encoding;

		// Token: 0x04000599 RID: 1433
		private string name;
	}
}
