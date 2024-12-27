using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x02000113 RID: 275
	internal class Keys : KeyedCollection<QilName, List<Key>>
	{
		// Token: 0x06000BEA RID: 3050 RVA: 0x0003D4F7 File Offset: 0x0003C4F7
		protected override QilName GetKeyForItem(List<Key> list)
		{
			return list[0].Name;
		}
	}
}
