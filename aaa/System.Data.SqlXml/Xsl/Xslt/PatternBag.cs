using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000F8 RID: 248
	internal class PatternBag
	{
		// Token: 0x06000AEB RID: 2795 RVA: 0x00034E7E File Offset: 0x00033E7E
		public void Clear()
		{
			this.FixedNamePatterns.Clear();
			this.FixedNamePatternsNames.Clear();
			this.NonFixedNamePatterns.Clear();
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00034EA4 File Offset: 0x00033EA4
		public void Add(Pattern pattern)
		{
			QilName qname = pattern.Match.QName;
			List<Pattern> list;
			if (qname == null)
			{
				list = this.NonFixedNamePatterns;
			}
			else if (!this.FixedNamePatterns.TryGetValue(qname, out list))
			{
				this.FixedNamePatternsNames.Add(qname);
				list = (this.FixedNamePatterns[qname] = new List<Pattern>());
			}
			list.Add(pattern);
		}

		// Token: 0x040007A9 RID: 1961
		public Dictionary<QilName, List<Pattern>> FixedNamePatterns = new Dictionary<QilName, List<Pattern>>();

		// Token: 0x040007AA RID: 1962
		public List<QilName> FixedNamePatternsNames = new List<QilName>();

		// Token: 0x040007AB RID: 1963
		public List<Pattern> NonFixedNamePatterns = new List<Pattern>();
	}
}
