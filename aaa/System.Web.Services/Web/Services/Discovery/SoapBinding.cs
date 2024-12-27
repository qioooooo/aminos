using System;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Discovery
{
	// Token: 0x020000B3 RID: 179
	[XmlRoot("soap", Namespace = "http://schemas.xmlsoap.org/disco/soap/")]
	public sealed class SoapBinding
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x00017D4C File Offset: 0x00016D4C
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x00017D54 File Offset: 0x00016D54
		[XmlAttribute("address")]
		public string Address
		{
			get
			{
				return this.address;
			}
			set
			{
				if (value == null)
				{
					this.address = "";
					return;
				}
				this.address = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00017D6C File Offset: 0x00016D6C
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x00017D74 File Offset: 0x00016D74
		[XmlAttribute("binding")]
		public XmlQualifiedName Binding
		{
			get
			{
				return this.binding;
			}
			set
			{
				this.binding = value;
			}
		}

		// Token: 0x040003DB RID: 987
		public const string Namespace = "http://schemas.xmlsoap.org/disco/soap/";

		// Token: 0x040003DC RID: 988
		private XmlQualifiedName binding;

		// Token: 0x040003DD RID: 989
		private string address = "";
	}
}
