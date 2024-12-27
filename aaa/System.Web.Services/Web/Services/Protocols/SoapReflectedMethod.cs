using System;
using System.Web.Services.Description;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000078 RID: 120
	internal class SoapReflectedMethod
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000326 RID: 806 RVA: 0x0000E675 File Offset: 0x0000D675
		internal bool IsClaimsConformance
		{
			get
			{
				return this.binding != null && this.binding.ConformsTo == WsiProfiles.BasicProfile1_1;
			}
		}

		// Token: 0x0400034D RID: 845
		internal LogicalMethodInfo methodInfo;

		// Token: 0x0400034E RID: 846
		internal string action;

		// Token: 0x0400034F RID: 847
		internal string name;

		// Token: 0x04000350 RID: 848
		internal XmlMembersMapping requestMappings;

		// Token: 0x04000351 RID: 849
		internal XmlMembersMapping responseMappings;

		// Token: 0x04000352 RID: 850
		internal XmlMembersMapping inHeaderMappings;

		// Token: 0x04000353 RID: 851
		internal XmlMembersMapping outHeaderMappings;

		// Token: 0x04000354 RID: 852
		internal SoapReflectedHeader[] headers;

		// Token: 0x04000355 RID: 853
		internal SoapReflectedExtension[] extensions;

		// Token: 0x04000356 RID: 854
		internal bool oneWay;

		// Token: 0x04000357 RID: 855
		internal bool rpc;

		// Token: 0x04000358 RID: 856
		internal SoapBindingUse use;

		// Token: 0x04000359 RID: 857
		internal SoapParameterStyle paramStyle;

		// Token: 0x0400035A RID: 858
		internal WebServiceBindingAttribute binding;

		// Token: 0x0400035B RID: 859
		internal XmlQualifiedName requestElementName;

		// Token: 0x0400035C RID: 860
		internal XmlQualifiedName portType;
	}
}
