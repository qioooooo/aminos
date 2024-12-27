using System;
using System.ComponentModel;
using System.Web.Services.Configuration;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x0200010D RID: 269
	[XmlFormatExtension("operation", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(OperationBinding))]
	public sealed class Soap12OperationBinding : SoapOperationBinding
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0003D4E2 File Offset: 0x0003C4E2
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0003D4EA File Offset: 0x0003C4EA
		[DefaultValue(false)]
		[XmlAttribute("soapActionRequired")]
		public bool SoapActionRequired
		{
			get
			{
				return this.soapActionRequired;
			}
			set
			{
				this.soapActionRequired = value;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x0003D4F3 File Offset: 0x0003C4F3
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x0003D4FB File Offset: 0x0003C4FB
		internal SoapReflectedMethod Method
		{
			get
			{
				return this.method;
			}
			set
			{
				this.method = value;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x0003D504 File Offset: 0x0003C504
		// (set) Token: 0x06000838 RID: 2104 RVA: 0x0003D50C File Offset: 0x0003C50C
		internal Soap12OperationBinding DuplicateBySoapAction
		{
			get
			{
				return this.duplicateBySoapAction;
			}
			set
			{
				this.duplicateBySoapAction = value;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x0003D515 File Offset: 0x0003C515
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x0003D51D File Offset: 0x0003C51D
		internal Soap12OperationBinding DuplicateByRequestElement
		{
			get
			{
				return this.duplicateByRequestElement;
			}
			set
			{
				this.duplicateByRequestElement = value;
			}
		}

		// Token: 0x0400058E RID: 1422
		private bool soapActionRequired;

		// Token: 0x0400058F RID: 1423
		private Soap12OperationBinding duplicateBySoapAction;

		// Token: 0x04000590 RID: 1424
		private Soap12OperationBinding duplicateByRequestElement;

		// Token: 0x04000591 RID: 1425
		private SoapReflectedMethod method;
	}
}
