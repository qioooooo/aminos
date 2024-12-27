using System;
using System.ComponentModel;
using System.Text;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x0200010E RID: 270
	[XmlFormatExtension("body", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(InputBinding), typeof(OutputBinding), typeof(MimePart))]
	public class SoapBodyBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x0003D52E File Offset: 0x0003C52E
		// (set) Token: 0x0600083D RID: 2109 RVA: 0x0003D536 File Offset: 0x0003C536
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

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x0003D53F File Offset: 0x0003C53F
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x0003D555 File Offset: 0x0003C555
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

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x0003D55E File Offset: 0x0003C55E
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x0003D574 File Offset: 0x0003C574
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

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x0003D580 File Offset: 0x0003C580
		// (set) Token: 0x06000843 RID: 2115 RVA: 0x0003D5D4 File Offset: 0x0003C5D4
		[XmlAttribute("parts")]
		public string PartsString
		{
			get
			{
				if (this.parts == null)
				{
					return null;
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.parts.Length; i++)
				{
					if (i > 0)
					{
						stringBuilder.Append(' ');
					}
					stringBuilder.Append(this.parts[i]);
				}
				return stringBuilder.ToString();
			}
			set
			{
				if (value == null)
				{
					this.parts = null;
					return;
				}
				this.parts = value.Split(new char[] { ' ' });
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x0003D605 File Offset: 0x0003C605
		// (set) Token: 0x06000845 RID: 2117 RVA: 0x0003D60D File Offset: 0x0003C60D
		[XmlIgnore]
		public string[] Parts
		{
			get
			{
				return this.parts;
			}
			set
			{
				this.parts = value;
			}
		}

		// Token: 0x04000592 RID: 1426
		private SoapBindingUse use;

		// Token: 0x04000593 RID: 1427
		private string ns;

		// Token: 0x04000594 RID: 1428
		private string encoding;

		// Token: 0x04000595 RID: 1429
		private string[] parts;
	}
}
