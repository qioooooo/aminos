using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000044 RID: 68
	internal class QilNode : IList<QilNode>, ICollection<QilNode>, IEnumerable<QilNode>, IEnumerable
	{
		// Token: 0x06000466 RID: 1126 RVA: 0x0001F0FB File Offset: 0x0001E0FB
		public QilNode(QilNodeType nodeType)
		{
			this.nodeType = nodeType;
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0001F10A File Offset: 0x0001E10A
		public QilNode(QilNodeType nodeType, XmlQueryType xmlType)
		{
			this.nodeType = nodeType;
			this.xmlType = xmlType;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x0001F120 File Offset: 0x0001E120
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0001F128 File Offset: 0x0001E128
		public QilNodeType NodeType
		{
			get
			{
				return this.nodeType;
			}
			set
			{
				this.nodeType = value;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0001F131 File Offset: 0x0001E131
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x0001F139 File Offset: 0x0001E139
		public virtual XmlQueryType XmlType
		{
			get
			{
				return this.xmlType;
			}
			set
			{
				this.xmlType = value;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x0001F142 File Offset: 0x0001E142
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x0001F14A File Offset: 0x0001E14A
		public ISourceLineInfo SourceLine
		{
			get
			{
				return this.sourceLine;
			}
			set
			{
				this.sourceLine = value;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001F153 File Offset: 0x0001E153
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x0001F15B File Offset: 0x0001E15B
		public object Annotation
		{
			get
			{
				return this.annotation;
			}
			set
			{
				this.annotation = value;
			}
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0001F164 File Offset: 0x0001E164
		public virtual QilNode DeepClone(QilFactory f)
		{
			return new QilCloneVisitor(f).Clone(this);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001F174 File Offset: 0x0001E174
		public virtual QilNode ShallowClone(QilFactory f)
		{
			return (QilNode)base.MemberwiseClone();
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x0001F18E File Offset: 0x0001E18E
		public virtual int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000A7 RID: 167
		public virtual QilNode this[int index]
		{
			get
			{
				throw new IndexOutOfRangeException();
			}
			set
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0001F19F File Offset: 0x0001E19F
		public virtual void Insert(int index, QilNode node)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001F1A6 File Offset: 0x0001E1A6
		public virtual void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001F1AD File Offset: 0x0001E1AD
		public IEnumerator<QilNode> GetEnumerator()
		{
			return new IListEnumerator<QilNode>(this);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0001F1BA File Offset: 0x0001E1BA
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new IListEnumerator<QilNode>(this);
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x0001F1C7 File Offset: 0x0001E1C7
		public virtual bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001F1CA File Offset: 0x0001E1CA
		public virtual void Add(QilNode node)
		{
			this.Insert(this.Count, node);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0001F1DC File Offset: 0x0001E1DC
		public virtual void Add(IList<QilNode> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.Insert(this.Count, list[i]);
			}
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001F210 File Offset: 0x0001E210
		public virtual void Clear()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.RemoveAt(i);
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0001F237 File Offset: 0x0001E237
		public virtual bool Contains(QilNode node)
		{
			return this.IndexOf(node) != -1;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001F248 File Offset: 0x0001E248
		public virtual void CopyTo(QilNode[] array, int index)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[index + i] = this[i];
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001F274 File Offset: 0x0001E274
		public virtual bool Remove(QilNode node)
		{
			int num = this.IndexOf(node);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001F298 File Offset: 0x0001E298
		public virtual int IndexOf(QilNode node)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (node.Equals(this[i]))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0400037B RID: 891
		protected QilNodeType nodeType;

		// Token: 0x0400037C RID: 892
		protected XmlQueryType xmlType;

		// Token: 0x0400037D RID: 893
		protected ISourceLineInfo sourceLine;

		// Token: 0x0400037E RID: 894
		protected object annotation;
	}
}
