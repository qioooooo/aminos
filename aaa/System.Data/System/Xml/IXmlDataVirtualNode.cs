using System;
using System.Data;

namespace System.Xml
{
	// Token: 0x02000384 RID: 900
	internal interface IXmlDataVirtualNode
	{
		// Token: 0x06002FAA RID: 12202
		bool IsOnNode(XmlNode nodeToCheck);

		// Token: 0x06002FAB RID: 12203
		bool IsOnColumn(DataColumn col);

		// Token: 0x06002FAC RID: 12204
		bool IsInUse();

		// Token: 0x06002FAD RID: 12205
		void OnFoliated(XmlNode foliatedNode);
	}
}
