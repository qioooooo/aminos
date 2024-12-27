using System;
using System.Collections;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000033 RID: 51
	internal class XmlILConstructInfo : IQilAnnotation
	{
		// Token: 0x06000310 RID: 784 RVA: 0x00014EEC File Offset: 0x00013EEC
		public static XmlILConstructInfo Read(QilNode nd)
		{
			XmlILAnnotation xmlILAnnotation = nd.Annotation as XmlILAnnotation;
			XmlILConstructInfo xmlILConstructInfo = ((xmlILAnnotation != null) ? xmlILAnnotation.ConstructInfo : null);
			if (xmlILConstructInfo == null)
			{
				if (XmlILConstructInfo.Default == null)
				{
					xmlILConstructInfo = new XmlILConstructInfo(QilNodeType.Unknown);
					xmlILConstructInfo.isReadOnly = true;
					XmlILConstructInfo.Default = xmlILConstructInfo;
				}
				else
				{
					xmlILConstructInfo = XmlILConstructInfo.Default;
				}
			}
			return xmlILConstructInfo;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00014F3C File Offset: 0x00013F3C
		public static XmlILConstructInfo Write(QilNode nd)
		{
			XmlILAnnotation xmlILAnnotation = XmlILAnnotation.Write(nd);
			XmlILConstructInfo xmlILConstructInfo = xmlILAnnotation.ConstructInfo;
			if (xmlILConstructInfo == null || xmlILConstructInfo.isReadOnly)
			{
				xmlILConstructInfo = new XmlILConstructInfo(nd.NodeType);
				xmlILAnnotation.ConstructInfo = xmlILConstructInfo;
			}
			return xmlILConstructInfo;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00014F78 File Offset: 0x00013F78
		private XmlILConstructInfo(QilNodeType nodeType)
		{
			this.nodeType = nodeType;
			this.xstatesInitial = (this.xstatesFinal = PossibleXmlStates.Any);
			this.xstatesBeginLoop = (this.xstatesEndLoop = PossibleXmlStates.None);
			this.isNmspInScope = false;
			this.mightHaveNmsp = true;
			this.mightHaveAttrs = true;
			this.mightHaveDupAttrs = true;
			this.mightHaveNmspAfterAttrs = true;
			this.constrMeth = XmlILConstructMethod.Iterator;
			this.parentInfo = null;
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000313 RID: 787 RVA: 0x00014FE3 File Offset: 0x00013FE3
		// (set) Token: 0x06000314 RID: 788 RVA: 0x00014FEB File Offset: 0x00013FEB
		public PossibleXmlStates InitialStates
		{
			get
			{
				return this.xstatesInitial;
			}
			set
			{
				this.xstatesInitial = value;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00014FF4 File Offset: 0x00013FF4
		// (set) Token: 0x06000316 RID: 790 RVA: 0x00014FFC File Offset: 0x00013FFC
		public PossibleXmlStates FinalStates
		{
			get
			{
				return this.xstatesFinal;
			}
			set
			{
				this.xstatesFinal = value;
			}
		}

		// Token: 0x1700008C RID: 140
		// (set) Token: 0x06000317 RID: 791 RVA: 0x00015005 File Offset: 0x00014005
		public PossibleXmlStates BeginLoopStates
		{
			set
			{
				this.xstatesBeginLoop = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (set) Token: 0x06000318 RID: 792 RVA: 0x0001500E File Offset: 0x0001400E
		public PossibleXmlStates EndLoopStates
		{
			set
			{
				this.xstatesEndLoop = value;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00015017 File Offset: 0x00014017
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0001501F File Offset: 0x0001401F
		public XmlILConstructMethod ConstructMethod
		{
			get
			{
				return this.constrMeth;
			}
			set
			{
				this.constrMeth = value;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00015028 File Offset: 0x00014028
		// (set) Token: 0x0600031C RID: 796 RVA: 0x00015040 File Offset: 0x00014040
		public bool PushToWriterFirst
		{
			get
			{
				return this.constrMeth == XmlILConstructMethod.Writer || this.constrMeth == XmlILConstructMethod.WriterThenIterator;
			}
			set
			{
				XmlILConstructMethod xmlILConstructMethod = this.constrMeth;
				if (xmlILConstructMethod == XmlILConstructMethod.Iterator)
				{
					this.constrMeth = XmlILConstructMethod.WriterThenIterator;
					return;
				}
				if (xmlILConstructMethod != XmlILConstructMethod.IteratorThenWriter)
				{
					return;
				}
				this.constrMeth = XmlILConstructMethod.Writer;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0001506C File Offset: 0x0001406C
		// (set) Token: 0x0600031E RID: 798 RVA: 0x00015084 File Offset: 0x00014084
		public bool PushToWriterLast
		{
			get
			{
				return this.constrMeth == XmlILConstructMethod.Writer || this.constrMeth == XmlILConstructMethod.IteratorThenWriter;
			}
			set
			{
				switch (this.constrMeth)
				{
				case XmlILConstructMethod.Iterator:
					this.constrMeth = XmlILConstructMethod.IteratorThenWriter;
					return;
				case XmlILConstructMethod.Writer:
					break;
				case XmlILConstructMethod.WriterThenIterator:
					this.constrMeth = XmlILConstructMethod.Writer;
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000150BA File Offset: 0x000140BA
		// (set) Token: 0x06000320 RID: 800 RVA: 0x000150D0 File Offset: 0x000140D0
		public bool PullFromIteratorFirst
		{
			get
			{
				return this.constrMeth == XmlILConstructMethod.IteratorThenWriter || this.constrMeth == XmlILConstructMethod.Iterator;
			}
			set
			{
				switch (this.constrMeth)
				{
				case XmlILConstructMethod.Writer:
					this.constrMeth = XmlILConstructMethod.IteratorThenWriter;
					return;
				case XmlILConstructMethod.WriterThenIterator:
					this.constrMeth = XmlILConstructMethod.Iterator;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (set) Token: 0x06000321 RID: 801 RVA: 0x00015104 File Offset: 0x00014104
		public XmlILConstructInfo ParentInfo
		{
			set
			{
				this.parentInfo = value;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0001510D File Offset: 0x0001410D
		public XmlILConstructInfo ParentElementInfo
		{
			get
			{
				if (this.parentInfo != null && this.parentInfo.nodeType == QilNodeType.ElementCtor)
				{
					return this.parentInfo;
				}
				return null;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0001512E File Offset: 0x0001412E
		// (set) Token: 0x06000324 RID: 804 RVA: 0x00015136 File Offset: 0x00014136
		public bool IsNamespaceInScope
		{
			get
			{
				return this.isNmspInScope;
			}
			set
			{
				this.isNmspInScope = value;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000325 RID: 805 RVA: 0x0001513F File Offset: 0x0001413F
		// (set) Token: 0x06000326 RID: 806 RVA: 0x00015147 File Offset: 0x00014147
		public bool MightHaveNamespaces
		{
			get
			{
				return this.mightHaveNmsp;
			}
			set
			{
				this.mightHaveNmsp = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00015150 File Offset: 0x00014150
		// (set) Token: 0x06000328 RID: 808 RVA: 0x00015158 File Offset: 0x00014158
		public bool MightHaveNamespacesAfterAttributes
		{
			get
			{
				return this.mightHaveNmspAfterAttrs;
			}
			set
			{
				this.mightHaveNmspAfterAttrs = value;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00015161 File Offset: 0x00014161
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00015169 File Offset: 0x00014169
		public bool MightHaveAttributes
		{
			get
			{
				return this.mightHaveAttrs;
			}
			set
			{
				this.mightHaveAttrs = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600032B RID: 811 RVA: 0x00015172 File Offset: 0x00014172
		// (set) Token: 0x0600032C RID: 812 RVA: 0x0001517A File Offset: 0x0001417A
		public bool MightHaveDuplicateAttributes
		{
			get
			{
				return this.mightHaveDupAttrs;
			}
			set
			{
				this.mightHaveDupAttrs = value;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00015183 File Offset: 0x00014183
		public ArrayList CallersInfo
		{
			get
			{
				if (this.callersInfo == null)
				{
					this.callersInfo = new ArrayList();
				}
				return this.callersInfo;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600032E RID: 814 RVA: 0x0001519E File Offset: 0x0001419E
		public virtual string Name
		{
			get
			{
				return "ConstructInfo";
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x000151A8 File Offset: 0x000141A8
		public override string ToString()
		{
			string text = "";
			if (this.constrMeth != XmlILConstructMethod.Iterator)
			{
				text += this.constrMeth.ToString();
				text = text + ", " + this.xstatesInitial;
				if (this.xstatesBeginLoop != PossibleXmlStates.None)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						" => ",
						this.xstatesBeginLoop.ToString(),
						" => ",
						this.xstatesEndLoop.ToString()
					});
				}
				text = text + " => " + this.xstatesFinal;
				if (!this.MightHaveAttributes)
				{
					text += ", NoAttrs";
				}
				if (!this.MightHaveDuplicateAttributes)
				{
					text += ", NoDupAttrs";
				}
				if (!this.MightHaveNamespaces)
				{
					text += ", NoNmsp";
				}
				if (!this.MightHaveNamespacesAfterAttributes)
				{
					text += ", NoNmspAfterAttrs";
				}
			}
			return text;
		}

		// Token: 0x040002A8 RID: 680
		private QilNodeType nodeType;

		// Token: 0x040002A9 RID: 681
		private PossibleXmlStates xstatesInitial;

		// Token: 0x040002AA RID: 682
		private PossibleXmlStates xstatesFinal;

		// Token: 0x040002AB RID: 683
		private PossibleXmlStates xstatesBeginLoop;

		// Token: 0x040002AC RID: 684
		private PossibleXmlStates xstatesEndLoop;

		// Token: 0x040002AD RID: 685
		private bool isNmspInScope;

		// Token: 0x040002AE RID: 686
		private bool mightHaveNmsp;

		// Token: 0x040002AF RID: 687
		private bool mightHaveAttrs;

		// Token: 0x040002B0 RID: 688
		private bool mightHaveDupAttrs;

		// Token: 0x040002B1 RID: 689
		private bool mightHaveNmspAfterAttrs;

		// Token: 0x040002B2 RID: 690
		private XmlILConstructMethod constrMeth;

		// Token: 0x040002B3 RID: 691
		private XmlILConstructInfo parentInfo;

		// Token: 0x040002B4 RID: 692
		private ArrayList callersInfo;

		// Token: 0x040002B5 RID: 693
		private bool isReadOnly;

		// Token: 0x040002B6 RID: 694
		private static XmlILConstructInfo Default;
	}
}
