using System;
using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000112 RID: 274
	[XmlFormatExtension("header", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(InputBinding), typeof(OutputBinding))]
	public class SoapHeaderBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000852 RID: 2130 RVA: 0x0003D696 File Offset: 0x0003C696
		// (set) Token: 0x06000853 RID: 2131 RVA: 0x0003D69E File Offset: 0x0003C69E
		[XmlIgnore]
		public bool MapToProperty
		{
			get
			{
				return this.mapToProperty;
			}
			set
			{
				this.mapToProperty = value;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x0003D6A7 File Offset: 0x0003C6A7
		// (set) Token: 0x06000855 RID: 2133 RVA: 0x0003D6AF File Offset: 0x0003C6AF
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

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x0003D6B8 File Offset: 0x0003C6B8
		// (set) Token: 0x06000857 RID: 2135 RVA: 0x0003D6C0 File Offset: 0x0003C6C0
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

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x0003D6C9 File Offset: 0x0003C6C9
		// (set) Token: 0x06000859 RID: 2137 RVA: 0x0003D6D1 File Offset: 0x0003C6D1
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

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x0003D6DA File Offset: 0x0003C6DA
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x0003D6F0 File Offset: 0x0003C6F0
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

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x0003D6F9 File Offset: 0x0003C6F9
		// (set) Token: 0x0600085D RID: 2141 RVA: 0x0003D70F File Offset: 0x0003C70F
		[XmlAttribute("namespace")]
		[DefaultValue("")]
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

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x0003D718 File Offset: 0x0003C718
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x0003D720 File Offset: 0x0003C720
		[XmlElement("headerfault")]
		public SoapHeaderFaultBinding Fault
		{
			get
			{
				return this.fault;
			}
			set
			{
				this.fault = value;
			}
		}

		// Token: 0x0400059A RID: 1434
		private XmlQualifiedName message = XmlQualifiedName.Empty;

		// Token: 0x0400059B RID: 1435
		private string part;

		// Token: 0x0400059C RID: 1436
		private SoapBindingUse use;

		// Token: 0x0400059D RID: 1437
		private string encoding;

		// Token: 0x0400059E RID: 1438
		private string ns;

		// Token: 0x0400059F RID: 1439
		private bool mapToProperty;

		// Token: 0x040005A0 RID: 1440
		private SoapHeaderFaultBinding fault;
	}
}
