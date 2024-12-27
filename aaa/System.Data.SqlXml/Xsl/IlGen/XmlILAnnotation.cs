using System;
using System.Reflection;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000030 RID: 48
	internal class XmlILAnnotation : ListBase<object>
	{
		// Token: 0x06000301 RID: 769 RVA: 0x00014DF8 File Offset: 0x00013DF8
		public static XmlILAnnotation Write(QilNode nd)
		{
			XmlILAnnotation xmlILAnnotation = nd.Annotation as XmlILAnnotation;
			if (xmlILAnnotation == null)
			{
				xmlILAnnotation = new XmlILAnnotation(nd.Annotation);
				nd.Annotation = xmlILAnnotation;
			}
			return xmlILAnnotation;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00014E28 File Offset: 0x00013E28
		private XmlILAnnotation(object annPrev)
		{
			this.annPrev = annPrev;
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000303 RID: 771 RVA: 0x00014E37 File Offset: 0x00013E37
		// (set) Token: 0x06000304 RID: 772 RVA: 0x00014E3F File Offset: 0x00013E3F
		public MethodInfo FunctionBinding
		{
			get
			{
				return this.funcMethod;
			}
			set
			{
				this.funcMethod = value;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000305 RID: 773 RVA: 0x00014E48 File Offset: 0x00013E48
		// (set) Token: 0x06000306 RID: 774 RVA: 0x00014E50 File Offset: 0x00013E50
		public int ArgumentPosition
		{
			get
			{
				return this.argPos;
			}
			set
			{
				this.argPos = value;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000307 RID: 775 RVA: 0x00014E59 File Offset: 0x00013E59
		// (set) Token: 0x06000308 RID: 776 RVA: 0x00014E61 File Offset: 0x00013E61
		public IteratorDescriptor CachedIteratorDescriptor
		{
			get
			{
				return this.iterInfo;
			}
			set
			{
				this.iterInfo = value;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000309 RID: 777 RVA: 0x00014E6A File Offset: 0x00013E6A
		// (set) Token: 0x0600030A RID: 778 RVA: 0x00014E72 File Offset: 0x00013E72
		public XmlILConstructInfo ConstructInfo
		{
			get
			{
				return this.constrInfo;
			}
			set
			{
				this.constrInfo = value;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600030B RID: 779 RVA: 0x00014E7B File Offset: 0x00013E7B
		// (set) Token: 0x0600030C RID: 780 RVA: 0x00014E83 File Offset: 0x00013E83
		public OptimizerPatterns Patterns
		{
			get
			{
				return this.optPatt;
			}
			set
			{
				this.optPatt = value;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00014E8C File Offset: 0x00013E8C
		public override int Count
		{
			get
			{
				if (this.annPrev == null)
				{
					return 2;
				}
				return 3;
			}
		}

		// Token: 0x17000089 RID: 137
		public override object this[int index]
		{
			get
			{
				if (this.annPrev != null)
				{
					if (index == 0)
					{
						return this.annPrev;
					}
					index--;
				}
				switch (index)
				{
				case 0:
					return this.constrInfo;
				case 1:
					return this.optPatt;
				default:
					throw new IndexOutOfRangeException();
				}
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x04000294 RID: 660
		private object annPrev;

		// Token: 0x04000295 RID: 661
		private MethodInfo funcMethod;

		// Token: 0x04000296 RID: 662
		private int argPos;

		// Token: 0x04000297 RID: 663
		private IteratorDescriptor iterInfo;

		// Token: 0x04000298 RID: 664
		private XmlILConstructInfo constrInfo;

		// Token: 0x04000299 RID: 665
		private OptimizerPatterns optPatt;
	}
}
