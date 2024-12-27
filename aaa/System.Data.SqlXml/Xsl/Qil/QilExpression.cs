using System;
using System.Collections.Generic;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200004A RID: 74
	internal class QilExpression : QilNode
	{
		// Token: 0x060004A2 RID: 1186 RVA: 0x0001F7EC File Offset: 0x0001E7EC
		public QilExpression(QilNodeType nodeType, QilNode root)
			: this(nodeType, root, new QilFactory())
		{
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001F7FC File Offset: 0x0001E7FC
		public QilExpression(QilNodeType nodeType, QilNode root, QilFactory factory)
			: base(nodeType)
		{
			this.factory = factory;
			this.isDebug = factory.False();
			this.defWSet = factory.LiteralObject(new XmlWriterSettings
			{
				ConformanceLevel = ConformanceLevel.Auto
			});
			this.wsRules = factory.LiteralObject(new List<WhitespaceRule>());
			this.gloVars = factory.GlobalVariableList();
			this.gloParams = factory.GlobalParameterList();
			this.earlBnd = factory.LiteralObject(new List<EarlyBoundInfo>());
			this.funList = factory.FunctionList();
			this.rootNod = root;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0001F88A File Offset: 0x0001E88A
		public override int Count
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x170000B2 RID: 178
		public override QilNode this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.isDebug;
				case 1:
					return this.defWSet;
				case 2:
					return this.wsRules;
				case 3:
					return this.gloParams;
				case 4:
					return this.gloVars;
				case 5:
					return this.earlBnd;
				case 6:
					return this.funList;
				case 7:
					return this.rootNod;
				default:
					throw new IndexOutOfRangeException();
				}
			}
			set
			{
				switch (index)
				{
				case 0:
					this.isDebug = value;
					return;
				case 1:
					this.defWSet = value;
					return;
				case 2:
					this.wsRules = value;
					return;
				case 3:
					this.gloParams = value;
					return;
				case 4:
					this.gloVars = value;
					return;
				case 5:
					this.earlBnd = value;
					return;
				case 6:
					this.funList = value;
					return;
				case 7:
					this.rootNod = value;
					return;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0001F980 File Offset: 0x0001E980
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x0001F988 File Offset: 0x0001E988
		public QilFactory Factory
		{
			get
			{
				return this.factory;
			}
			set
			{
				this.factory = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001F991 File Offset: 0x0001E991
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x0001F9A2 File Offset: 0x0001E9A2
		public bool IsDebug
		{
			get
			{
				return this.isDebug.NodeType == QilNodeType.True;
			}
			set
			{
				this.isDebug = (value ? this.factory.True() : this.factory.False());
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001F9C5 File Offset: 0x0001E9C5
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x0001F9DC File Offset: 0x0001E9DC
		public XmlWriterSettings DefaultWriterSettings
		{
			get
			{
				return (XmlWriterSettings)((QilLiteral)this.defWSet).Value;
			}
			set
			{
				value.ReadOnly = true;
				((QilLiteral)this.defWSet).Value = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x0001F9F6 File Offset: 0x0001E9F6
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x0001FA0D File Offset: 0x0001EA0D
		public IList<WhitespaceRule> WhitespaceRules
		{
			get
			{
				return (IList<WhitespaceRule>)((QilLiteral)this.wsRules).Value;
			}
			set
			{
				((QilLiteral)this.wsRules).Value = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0001FA20 File Offset: 0x0001EA20
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x0001FA2D File Offset: 0x0001EA2D
		public QilList GlobalParameterList
		{
			get
			{
				return (QilList)this.gloParams;
			}
			set
			{
				this.gloParams = value;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001FA36 File Offset: 0x0001EA36
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x0001FA43 File Offset: 0x0001EA43
		public QilList GlobalVariableList
		{
			get
			{
				return (QilList)this.gloVars;
			}
			set
			{
				this.gloVars = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001FA4C File Offset: 0x0001EA4C
		// (set) Token: 0x060004B4 RID: 1204 RVA: 0x0001FA63 File Offset: 0x0001EA63
		public IList<EarlyBoundInfo> EarlyBoundTypes
		{
			get
			{
				return (IList<EarlyBoundInfo>)((QilLiteral)this.earlBnd).Value;
			}
			set
			{
				((QilLiteral)this.earlBnd).Value = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001FA76 File Offset: 0x0001EA76
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x0001FA83 File Offset: 0x0001EA83
		public QilList FunctionList
		{
			get
			{
				return (QilList)this.funList;
			}
			set
			{
				this.funList = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0001FA8C File Offset: 0x0001EA8C
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x0001FA94 File Offset: 0x0001EA94
		public QilNode Root
		{
			get
			{
				return this.rootNod;
			}
			set
			{
				this.rootNod = value;
			}
		}

		// Token: 0x04000383 RID: 899
		private QilFactory factory;

		// Token: 0x04000384 RID: 900
		private QilNode isDebug;

		// Token: 0x04000385 RID: 901
		private QilNode defWSet;

		// Token: 0x04000386 RID: 902
		private QilNode wsRules;

		// Token: 0x04000387 RID: 903
		private QilNode gloVars;

		// Token: 0x04000388 RID: 904
		private QilNode gloParams;

		// Token: 0x04000389 RID: 905
		private QilNode earlBnd;

		// Token: 0x0400038A RID: 906
		private QilNode funList;

		// Token: 0x0400038B RID: 907
		private QilNode rootNod;
	}
}
