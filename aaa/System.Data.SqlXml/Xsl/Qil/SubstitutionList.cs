using System;
using System.Collections;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000069 RID: 105
	internal sealed class SubstitutionList
	{
		// Token: 0x060006BE RID: 1726 RVA: 0x000246DC File Offset: 0x000236DC
		public SubstitutionList()
		{
			this.s = new ArrayList(4);
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x000246F0 File Offset: 0x000236F0
		public void AddSubstitutionPair(QilNode find, QilNode replace)
		{
			this.s.Add(find);
			this.s.Add(replace);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002470C File Offset: 0x0002370C
		public void RemoveLastSubstitutionPair()
		{
			this.s.RemoveRange(this.s.Count - 2, 2);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00024727 File Offset: 0x00023727
		public void RemoveLastNSubstitutionPairs(int n)
		{
			if (n > 0)
			{
				n *= 2;
				this.s.RemoveRange(this.s.Count - n, n);
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002474C File Offset: 0x0002374C
		public QilNode FindReplacement(QilNode n)
		{
			for (int i = this.s.Count - 2; i >= 0; i -= 2)
			{
				if (this.s[i] == n)
				{
					return (QilNode)this.s[i + 1];
				}
			}
			return null;
		}

		// Token: 0x04000435 RID: 1077
		private ArrayList s;
	}
}
