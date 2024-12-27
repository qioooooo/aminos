using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x02000076 RID: 118
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct DodSequenceMerge
	{
		// Token: 0x060006EC RID: 1772 RVA: 0x00025272 File Offset: 0x00024272
		public void Create(XmlQueryRuntime runtime)
		{
			this.firstSequence = null;
			this.sequencesToMerge = null;
			this.nodeCount = 0;
			this.runtime = runtime;
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x00025290 File Offset: 0x00024290
		public void AddSequence(IList<XPathNavigator> sequence)
		{
			if (sequence.Count == 0)
			{
				return;
			}
			if (this.firstSequence == null)
			{
				this.firstSequence = sequence;
				return;
			}
			if (this.sequencesToMerge == null)
			{
				this.sequencesToMerge = new List<IEnumerator<XPathNavigator>>();
				this.MoveAndInsertSequence(this.firstSequence.GetEnumerator());
				this.nodeCount = this.firstSequence.Count;
			}
			this.MoveAndInsertSequence(sequence.GetEnumerator());
			this.nodeCount += sequence.Count;
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0002530C File Offset: 0x0002430C
		public IList<XPathNavigator> MergeSequences()
		{
			if (this.firstSequence == null)
			{
				return XmlQueryNodeSequence.Empty;
			}
			if (this.sequencesToMerge == null || this.sequencesToMerge.Count <= 1)
			{
				return this.firstSequence;
			}
			XmlQueryNodeSequence xmlQueryNodeSequence = new XmlQueryNodeSequence(this.nodeCount);
			while (this.sequencesToMerge.Count != 1)
			{
				IEnumerator<XPathNavigator> enumerator = this.sequencesToMerge[this.sequencesToMerge.Count - 1];
				this.sequencesToMerge.RemoveAt(this.sequencesToMerge.Count - 1);
				xmlQueryNodeSequence.Add(enumerator.Current);
				this.MoveAndInsertSequence(enumerator);
			}
			do
			{
				xmlQueryNodeSequence.Add(this.sequencesToMerge[0].Current);
			}
			while (this.sequencesToMerge[0].MoveNext());
			return xmlQueryNodeSequence;
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x000253CF File Offset: 0x000243CF
		private void MoveAndInsertSequence(IEnumerator<XPathNavigator> sequence)
		{
			if (sequence.MoveNext())
			{
				this.InsertSequence(sequence);
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000253E0 File Offset: 0x000243E0
		private void InsertSequence(IEnumerator<XPathNavigator> sequence)
		{
			for (int i = this.sequencesToMerge.Count - 1; i >= 0; i--)
			{
				int num = this.runtime.ComparePosition(sequence.Current, this.sequencesToMerge[i].Current);
				if (num == -1)
				{
					this.sequencesToMerge.Insert(i + 1, sequence);
					return;
				}
				if (num == 0 && !sequence.MoveNext())
				{
					return;
				}
			}
			this.sequencesToMerge.Insert(0, sequence);
		}

		// Token: 0x0400045E RID: 1118
		private IList<XPathNavigator> firstSequence;

		// Token: 0x0400045F RID: 1119
		private List<IEnumerator<XPathNavigator>> sequencesToMerge;

		// Token: 0x04000460 RID: 1120
		private int nodeCount;

		// Token: 0x04000461 RID: 1121
		private XmlQueryRuntime runtime;
	}
}
