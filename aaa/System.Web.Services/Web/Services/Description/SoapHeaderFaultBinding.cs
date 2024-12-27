using System;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x0200011E RID: 286
	public class SoapHeaderFaultBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00040F46 File Offset: 0x0003FF46
		// (set) Token: 0x060008B9 RID: 2233 RVA: 0x00040F4E File Offset: 0x0003FF4E
		[XmlAttribute("message")]
		public XmlQualifiedName Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060008BA RID: 2234 RVA: 0x00040F57 File Offset: 0x0003FF57
		// (set) Token: 0x060008BB RID: 2235 RVA: 0x00040F5F File Offset: 0x0003FF5F
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

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x00040F68 File Offset: 0x0003FF68
		// (set) Token: 0x060008BD RID: 2237 RVA: 0x00040F70 File Offset: 0x0003FF70
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

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x00040F79 File Offset: 0x0003FF79
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x00040F8F File Offset: 0x0003FF8F
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

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00040F98 File Offset: 0x0003FF98
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x00040FAE File Offset: 0x0003FFAE
		[DefaultValue("")]
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

		// Token: 0x040005C1 RID: 1473
		private XmlQualifiedName message = XmlQualifiedName.Empty;

		// Token: 0x040005C2 RID: 1474
		private string part;

		// Token: 0x040005C3 RID: 1475
		private SoapBindingUse use;

		// Token: 0x040005C4 RID: 1476
		private string encoding;

		// Token: 0x040005C5 RID: 1477
		private string ns;
	}
}
