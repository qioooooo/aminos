using System;
using System.Xml;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200006B RID: 107
	[Serializable]
	public class SoapFaultSubCode
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x0000D785 File Offset: 0x0000C785
		public SoapFaultSubCode(XmlQualifiedName code)
			: this(code, null)
		{
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000D78F File Offset: 0x0000C78F
		public SoapFaultSubCode(XmlQualifiedName code, SoapFaultSubCode subCode)
		{
			this.code = code;
			this.subCode = subCode;
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000D7A5 File Offset: 0x0000C7A5
		public XmlQualifiedName Code
		{
			get
			{
				return this.code;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000D7AD File Offset: 0x0000C7AD
		public SoapFaultSubCode SubCode
		{
			get
			{
				return this.subCode;
			}
		}

		// Token: 0x04000320 RID: 800
		private XmlQualifiedName code;

		// Token: 0x04000321 RID: 801
		private SoapFaultSubCode subCode;
	}
}
