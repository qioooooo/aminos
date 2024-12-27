using System;
using System.Collections.ObjectModel;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000E4 RID: 228
	internal class DecimalFormats : KeyedCollection<XmlQualifiedName, DecimalFormatDecl>
	{
		// Token: 0x06000A96 RID: 2710 RVA: 0x00033309 File Offset: 0x00032309
		protected override XmlQualifiedName GetKeyForItem(DecimalFormatDecl format)
		{
			return format.Name;
		}
	}
}
