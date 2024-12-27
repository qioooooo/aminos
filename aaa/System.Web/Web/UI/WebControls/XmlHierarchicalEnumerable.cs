using System;
using System.Collections;
using System.Xml;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000694 RID: 1684
	internal sealed class XmlHierarchicalEnumerable : IHierarchicalEnumerable, IEnumerable
	{
		// Token: 0x0600527E RID: 21118 RVA: 0x0014D03D File Offset: 0x0014C03D
		internal XmlHierarchicalEnumerable(XmlNodeList nodeList)
		{
			this._nodeList = nodeList;
		}

		// Token: 0x170014FE RID: 5374
		// (get) Token: 0x0600527F RID: 21119 RVA: 0x0014D04C File Offset: 0x0014C04C
		// (set) Token: 0x06005280 RID: 21120 RVA: 0x0014D054 File Offset: 0x0014C054
		internal string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
			}
		}

		// Token: 0x06005281 RID: 21121 RVA: 0x0014D1E0 File Offset: 0x0014C1E0
		IEnumerator IEnumerable.GetEnumerator()
		{
			XmlHierarchicalEnumerable.GetEnumerator>d__0 getEnumerator>d__ = new XmlHierarchicalEnumerable.GetEnumerator>d__0(0);
			getEnumerator>d__.<>4__this = this;
			return getEnumerator>d__;
		}

		// Token: 0x06005282 RID: 21122 RVA: 0x0014D1FC File Offset: 0x0014C1FC
		IHierarchyData IHierarchicalEnumerable.GetHierarchyData(object enumeratedItem)
		{
			return (IHierarchyData)enumeratedItem;
		}

		// Token: 0x04002DFD RID: 11773
		private string _path;

		// Token: 0x04002DFE RID: 11774
		private XmlNodeList _nodeList;
	}
}
