using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000A7 RID: 167
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class XmlILIndex
	{
		// Token: 0x060007E6 RID: 2022 RVA: 0x00028446 File Offset: 0x00027446
		internal XmlILIndex()
		{
			this.table = new Dictionary<string, XmlQueryNodeSequence>();
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0002845C File Offset: 0x0002745C
		public void Add(string key, XPathNavigator navigator)
		{
			XmlQueryNodeSequence xmlQueryNodeSequence;
			if (!this.table.TryGetValue(key, out xmlQueryNodeSequence))
			{
				xmlQueryNodeSequence = new XmlQueryNodeSequence();
				xmlQueryNodeSequence.AddClone(navigator);
				this.table.Add(key, xmlQueryNodeSequence);
				return;
			}
			if (!navigator.IsSamePosition(xmlQueryNodeSequence[xmlQueryNodeSequence.Count - 1]))
			{
				xmlQueryNodeSequence.AddClone(navigator);
			}
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000284B4 File Offset: 0x000274B4
		public XmlQueryNodeSequence Lookup(string key)
		{
			XmlQueryNodeSequence xmlQueryNodeSequence;
			if (!this.table.TryGetValue(key, out xmlQueryNodeSequence))
			{
				xmlQueryNodeSequence = new XmlQueryNodeSequence();
			}
			return xmlQueryNodeSequence;
		}

		// Token: 0x04000561 RID: 1377
		private Dictionary<string, XmlQueryNodeSequence> table;
	}
}
