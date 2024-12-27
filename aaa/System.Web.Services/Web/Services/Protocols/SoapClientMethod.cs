using System;
using System.Web.Services.Description;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000061 RID: 97
	internal class SoapClientMethod
	{
		// Token: 0x040002E8 RID: 744
		internal XmlSerializer returnSerializer;

		// Token: 0x040002E9 RID: 745
		internal XmlSerializer parameterSerializer;

		// Token: 0x040002EA RID: 746
		internal XmlSerializer inHeaderSerializer;

		// Token: 0x040002EB RID: 747
		internal XmlSerializer outHeaderSerializer;

		// Token: 0x040002EC RID: 748
		internal string action;

		// Token: 0x040002ED RID: 749
		internal LogicalMethodInfo methodInfo;

		// Token: 0x040002EE RID: 750
		internal SoapHeaderMapping[] inHeaderMappings;

		// Token: 0x040002EF RID: 751
		internal SoapHeaderMapping[] outHeaderMappings;

		// Token: 0x040002F0 RID: 752
		internal SoapReflectedExtension[] extensions;

		// Token: 0x040002F1 RID: 753
		internal object[] extensionInitializers;

		// Token: 0x040002F2 RID: 754
		internal bool oneWay;

		// Token: 0x040002F3 RID: 755
		internal bool rpc;

		// Token: 0x040002F4 RID: 756
		internal SoapBindingUse use;

		// Token: 0x040002F5 RID: 757
		internal SoapParameterStyle paramStyle;
	}
}
