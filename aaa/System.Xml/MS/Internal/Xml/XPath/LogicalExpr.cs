using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000147 RID: 327
	internal sealed class LogicalExpr : ValueQuery
	{
		// Token: 0x0600124A RID: 4682 RVA: 0x0004FF54 File Offset: 0x0004EF54
		public LogicalExpr(Operator.Op op, Query opnd1, Query opnd2)
		{
			this.op = op;
			this.opnd1 = opnd1;
			this.opnd2 = opnd2;
		}

		// Token: 0x0600124B RID: 4683 RVA: 0x0004FFA0 File Offset: 0x0004EFA0
		private LogicalExpr(LogicalExpr other)
			: base(other)
		{
			this.op = other.op;
			this.opnd1 = Query.Clone(other.opnd1);
			this.opnd2 = Query.Clone(other.opnd2);
		}

		// Token: 0x0600124C RID: 4684 RVA: 0x00050004 File Offset: 0x0004F004
		public override void SetXsltContext(XsltContext context)
		{
			this.opnd1.SetXsltContext(context);
			this.opnd2.SetXsltContext(context);
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00050020 File Offset: 0x0004F020
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			Operator.Op op = this.op;
			object obj = this.opnd1.Evaluate(nodeIterator);
			object obj2 = this.opnd2.Evaluate(nodeIterator);
			int num = (int)base.GetXPathType(obj);
			int num2 = (int)base.GetXPathType(obj2);
			if (num < num2)
			{
				op = this.invertOp[(int)op];
				object obj3 = obj;
				obj = obj2;
				obj2 = obj3;
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			if (op == Operator.Op.EQ || op == Operator.Op.NE)
			{
				return LogicalExpr.CompXsltE[num][num2](op, obj, obj2);
			}
			return LogicalExpr.CompXsltO[num][num2](op, obj, obj2);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x000500B8 File Offset: 0x0004F0B8
		private static bool cmpQueryQueryE(Operator.Op op, object val1, object val2)
		{
			bool flag = op == Operator.Op.EQ;
			LogicalExpr.NodeSet nodeSet = new LogicalExpr.NodeSet(val1);
			LogicalExpr.NodeSet nodeSet2 = new LogicalExpr.NodeSet(val2);
			IL_0015:
			while (nodeSet.MoveNext())
			{
				if (!nodeSet2.MoveNext())
				{
					return false;
				}
				string value = nodeSet.Value;
				while (value == nodeSet2.Value != flag)
				{
					if (!nodeSet2.MoveNext())
					{
						nodeSet2.Reset();
						goto IL_0015;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x0005011C File Offset: 0x0004F11C
		private static bool cmpQueryQueryO(Operator.Op op, object val1, object val2)
		{
			LogicalExpr.NodeSet nodeSet = new LogicalExpr.NodeSet(val1);
			LogicalExpr.NodeSet nodeSet2 = new LogicalExpr.NodeSet(val2);
			IL_0010:
			while (nodeSet.MoveNext())
			{
				if (!nodeSet2.MoveNext())
				{
					return false;
				}
				double num = NumberFunctions.Number(nodeSet.Value);
				while (!LogicalExpr.cmpNumberNumber(op, num, NumberFunctions.Number(nodeSet2.Value)))
				{
					if (!nodeSet2.MoveNext())
					{
						nodeSet2.Reset();
						goto IL_0010;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00050184 File Offset: 0x0004F184
		private static bool cmpQueryNumber(Operator.Op op, object val1, object val2)
		{
			LogicalExpr.NodeSet nodeSet = new LogicalExpr.NodeSet(val1);
			double num = (double)val2;
			while (nodeSet.MoveNext())
			{
				if (LogicalExpr.cmpNumberNumber(op, NumberFunctions.Number(nodeSet.Value), num))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x000501C4 File Offset: 0x0004F1C4
		private static bool cmpQueryStringE(Operator.Op op, object val1, object val2)
		{
			LogicalExpr.NodeSet nodeSet = new LogicalExpr.NodeSet(val1);
			string text = (string)val2;
			while (nodeSet.MoveNext())
			{
				if (LogicalExpr.cmpStringStringE(op, nodeSet.Value, text))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00050200 File Offset: 0x0004F200
		private static bool cmpQueryStringO(Operator.Op op, object val1, object val2)
		{
			LogicalExpr.NodeSet nodeSet = new LogicalExpr.NodeSet(val1);
			double num = NumberFunctions.Number((string)val2);
			while (nodeSet.MoveNext())
			{
				if (LogicalExpr.cmpNumberNumberO(op, NumberFunctions.Number(nodeSet.Value), num))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00050244 File Offset: 0x0004F244
		private static bool cmpRtfQueryE(Operator.Op op, object val1, object val2)
		{
			string text = LogicalExpr.Rtf(val1);
			LogicalExpr.NodeSet nodeSet = new LogicalExpr.NodeSet(val2);
			while (nodeSet.MoveNext())
			{
				if (LogicalExpr.cmpStringStringE(op, text, nodeSet.Value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001254 RID: 4692 RVA: 0x00050280 File Offset: 0x0004F280
		private static bool cmpRtfQueryO(Operator.Op op, object val1, object val2)
		{
			double num = NumberFunctions.Number(LogicalExpr.Rtf(val1));
			LogicalExpr.NodeSet nodeSet = new LogicalExpr.NodeSet(val2);
			while (nodeSet.MoveNext())
			{
				if (LogicalExpr.cmpNumberNumberO(op, num, NumberFunctions.Number(nodeSet.Value)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001255 RID: 4693 RVA: 0x000502C4 File Offset: 0x0004F2C4
		private static bool cmpQueryBoolE(Operator.Op op, object val1, object val2)
		{
			LogicalExpr.NodeSet nodeSet = new LogicalExpr.NodeSet(val1);
			bool flag = nodeSet.MoveNext();
			bool flag2 = (bool)val2;
			return LogicalExpr.cmpBoolBoolE(op, flag, flag2);
		}

		// Token: 0x06001256 RID: 4694 RVA: 0x000502F0 File Offset: 0x0004F2F0
		private static bool cmpQueryBoolO(Operator.Op op, object val1, object val2)
		{
			LogicalExpr.NodeSet nodeSet = new LogicalExpr.NodeSet(val1);
			double num = (nodeSet.MoveNext() ? 1.0 : 0.0);
			double num2 = NumberFunctions.Number((bool)val2);
			return LogicalExpr.cmpNumberNumberO(op, num, num2);
		}

		// Token: 0x06001257 RID: 4695 RVA: 0x00050337 File Offset: 0x0004F337
		private static bool cmpBoolBoolE(Operator.Op op, bool n1, bool n2)
		{
			return op == Operator.Op.EQ == (n1 == n2);
		}

		// Token: 0x06001258 RID: 4696 RVA: 0x00050344 File Offset: 0x0004F344
		private static bool cmpBoolBoolE(Operator.Op op, object val1, object val2)
		{
			bool flag = (bool)val1;
			bool flag2 = (bool)val2;
			return LogicalExpr.cmpBoolBoolE(op, flag, flag2);
		}

		// Token: 0x06001259 RID: 4697 RVA: 0x00050368 File Offset: 0x0004F368
		private static bool cmpBoolBoolO(Operator.Op op, object val1, object val2)
		{
			double num = NumberFunctions.Number((bool)val1);
			double num2 = NumberFunctions.Number((bool)val2);
			return LogicalExpr.cmpNumberNumberO(op, num, num2);
		}

		// Token: 0x0600125A RID: 4698 RVA: 0x00050398 File Offset: 0x0004F398
		private static bool cmpBoolNumberE(Operator.Op op, object val1, object val2)
		{
			bool flag = (bool)val1;
			bool flag2 = BooleanFunctions.toBoolean((double)val2);
			return LogicalExpr.cmpBoolBoolE(op, flag, flag2);
		}

		// Token: 0x0600125B RID: 4699 RVA: 0x000503C0 File Offset: 0x0004F3C0
		private static bool cmpBoolNumberO(Operator.Op op, object val1, object val2)
		{
			double num = NumberFunctions.Number((bool)val1);
			double num2 = (double)val2;
			return LogicalExpr.cmpNumberNumberO(op, num, num2);
		}

		// Token: 0x0600125C RID: 4700 RVA: 0x000503E8 File Offset: 0x0004F3E8
		private static bool cmpBoolStringE(Operator.Op op, object val1, object val2)
		{
			bool flag = (bool)val1;
			bool flag2 = BooleanFunctions.toBoolean((string)val2);
			return LogicalExpr.cmpBoolBoolE(op, flag, flag2);
		}

		// Token: 0x0600125D RID: 4701 RVA: 0x00050410 File Offset: 0x0004F410
		private static bool cmpRtfBoolE(Operator.Op op, object val1, object val2)
		{
			bool flag = BooleanFunctions.toBoolean(LogicalExpr.Rtf(val1));
			bool flag2 = (bool)val2;
			return LogicalExpr.cmpBoolBoolE(op, flag, flag2);
		}

		// Token: 0x0600125E RID: 4702 RVA: 0x00050438 File Offset: 0x0004F438
		private static bool cmpBoolStringO(Operator.Op op, object val1, object val2)
		{
			return LogicalExpr.cmpNumberNumberO(op, NumberFunctions.Number((bool)val1), NumberFunctions.Number((string)val2));
		}

		// Token: 0x0600125F RID: 4703 RVA: 0x00050456 File Offset: 0x0004F456
		private static bool cmpRtfBoolO(Operator.Op op, object val1, object val2)
		{
			return LogicalExpr.cmpNumberNumberO(op, NumberFunctions.Number(LogicalExpr.Rtf(val1)), NumberFunctions.Number((bool)val2));
		}

		// Token: 0x06001260 RID: 4704 RVA: 0x00050474 File Offset: 0x0004F474
		private static bool cmpNumberNumber(Operator.Op op, double n1, double n2)
		{
			switch (op)
			{
			case Operator.Op.LT:
				return n1 < n2;
			case Operator.Op.GT:
				return n1 > n2;
			case Operator.Op.LE:
				return n1 <= n2;
			case Operator.Op.GE:
				return n1 >= n2;
			case Operator.Op.EQ:
				return n1 == n2;
			case Operator.Op.NE:
				return n1 != n2;
			default:
				return false;
			}
		}

		// Token: 0x06001261 RID: 4705 RVA: 0x000504CC File Offset: 0x0004F4CC
		private static bool cmpNumberNumberO(Operator.Op op, double n1, double n2)
		{
			switch (op)
			{
			case Operator.Op.LT:
				return n1 < n2;
			case Operator.Op.GT:
				return n1 > n2;
			case Operator.Op.LE:
				return n1 <= n2;
			case Operator.Op.GE:
				return n1 >= n2;
			default:
				return false;
			}
		}

		// Token: 0x06001262 RID: 4706 RVA: 0x00050510 File Offset: 0x0004F510
		private static bool cmpNumberNumber(Operator.Op op, object val1, object val2)
		{
			double num = (double)val1;
			double num2 = (double)val2;
			return LogicalExpr.cmpNumberNumber(op, num, num2);
		}

		// Token: 0x06001263 RID: 4707 RVA: 0x00050534 File Offset: 0x0004F534
		private static bool cmpStringNumber(Operator.Op op, object val1, object val2)
		{
			double num = (double)val2;
			double num2 = NumberFunctions.Number((string)val1);
			return LogicalExpr.cmpNumberNumber(op, num2, num);
		}

		// Token: 0x06001264 RID: 4708 RVA: 0x0005055C File Offset: 0x0004F55C
		private static bool cmpRtfNumber(Operator.Op op, object val1, object val2)
		{
			double num = (double)val2;
			double num2 = NumberFunctions.Number(LogicalExpr.Rtf(val1));
			return LogicalExpr.cmpNumberNumber(op, num2, num);
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x00050584 File Offset: 0x0004F584
		private static bool cmpStringStringE(Operator.Op op, string n1, string n2)
		{
			return op == Operator.Op.EQ == (n1 == n2);
		}

		// Token: 0x06001266 RID: 4710 RVA: 0x00050594 File Offset: 0x0004F594
		private static bool cmpStringStringE(Operator.Op op, object val1, object val2)
		{
			string text = (string)val1;
			string text2 = (string)val2;
			return LogicalExpr.cmpStringStringE(op, text, text2);
		}

		// Token: 0x06001267 RID: 4711 RVA: 0x000505B8 File Offset: 0x0004F5B8
		private static bool cmpRtfStringE(Operator.Op op, object val1, object val2)
		{
			string text = LogicalExpr.Rtf(val1);
			string text2 = (string)val2;
			return LogicalExpr.cmpStringStringE(op, text, text2);
		}

		// Token: 0x06001268 RID: 4712 RVA: 0x000505DC File Offset: 0x0004F5DC
		private static bool cmpRtfRtfE(Operator.Op op, object val1, object val2)
		{
			string text = LogicalExpr.Rtf(val1);
			string text2 = LogicalExpr.Rtf(val2);
			return LogicalExpr.cmpStringStringE(op, text, text2);
		}

		// Token: 0x06001269 RID: 4713 RVA: 0x00050600 File Offset: 0x0004F600
		private static bool cmpStringStringO(Operator.Op op, object val1, object val2)
		{
			double num = NumberFunctions.Number((string)val1);
			double num2 = NumberFunctions.Number((string)val2);
			return LogicalExpr.cmpNumberNumberO(op, num, num2);
		}

		// Token: 0x0600126A RID: 4714 RVA: 0x00050630 File Offset: 0x0004F630
		private static bool cmpRtfStringO(Operator.Op op, object val1, object val2)
		{
			double num = NumberFunctions.Number(LogicalExpr.Rtf(val1));
			double num2 = NumberFunctions.Number((string)val2);
			return LogicalExpr.cmpNumberNumberO(op, num, num2);
		}

		// Token: 0x0600126B RID: 4715 RVA: 0x00050660 File Offset: 0x0004F660
		private static bool cmpRtfRtfO(Operator.Op op, object val1, object val2)
		{
			double num = NumberFunctions.Number(LogicalExpr.Rtf(val1));
			double num2 = NumberFunctions.Number(LogicalExpr.Rtf(val2));
			return LogicalExpr.cmpNumberNumberO(op, num, num2);
		}

		// Token: 0x0600126C RID: 4716 RVA: 0x0005068D File Offset: 0x0004F68D
		public override XPathNodeIterator Clone()
		{
			return new LogicalExpr(this);
		}

		// Token: 0x0600126D RID: 4717 RVA: 0x00050695 File Offset: 0x0004F695
		private static string Rtf(object o)
		{
			return ((XPathNavigator)o).Value;
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600126E RID: 4718 RVA: 0x000506A2 File Offset: 0x0004F6A2
		public override XPathResultType StaticType
		{
			get
			{
				return XPathResultType.Boolean;
			}
		}

		// Token: 0x0600126F RID: 4719 RVA: 0x000506A8 File Offset: 0x0004F6A8
		public override void PrintQuery(XmlWriter w)
		{
			w.WriteStartElement(base.GetType().Name);
			w.WriteAttributeString("op", this.op.ToString());
			this.opnd1.PrintQuery(w);
			this.opnd2.PrintQuery(w);
			w.WriteEndElement();
		}

		// Token: 0x06001270 RID: 4720 RVA: 0x00050700 File Offset: 0x0004F700
		// Note: this type is marked as 'beforefieldinit'.
		static LogicalExpr()
		{
			LogicalExpr.cmpXslt[][] array = new LogicalExpr.cmpXslt[5][];
			LogicalExpr.cmpXslt[][] array2 = array;
			int num = 0;
			LogicalExpr.cmpXslt[] array3 = new LogicalExpr.cmpXslt[5];
			array3[0] = new LogicalExpr.cmpXslt(LogicalExpr.cmpNumberNumber);
			array2[num] = array3;
			LogicalExpr.cmpXslt[][] array4 = array;
			int num2 = 1;
			LogicalExpr.cmpXslt[] array5 = new LogicalExpr.cmpXslt[5];
			array5[0] = new LogicalExpr.cmpXslt(LogicalExpr.cmpStringNumber);
			array5[1] = new LogicalExpr.cmpXslt(LogicalExpr.cmpStringStringE);
			array4[num2] = array5;
			LogicalExpr.cmpXslt[][] array6 = array;
			int num3 = 2;
			LogicalExpr.cmpXslt[] array7 = new LogicalExpr.cmpXslt[5];
			array7[0] = new LogicalExpr.cmpXslt(LogicalExpr.cmpBoolNumberE);
			array7[1] = new LogicalExpr.cmpXslt(LogicalExpr.cmpBoolStringE);
			array7[2] = new LogicalExpr.cmpXslt(LogicalExpr.cmpBoolBoolE);
			array6[num3] = array7;
			LogicalExpr.cmpXslt[][] array8 = array;
			int num4 = 3;
			LogicalExpr.cmpXslt[] array9 = new LogicalExpr.cmpXslt[5];
			array9[0] = new LogicalExpr.cmpXslt(LogicalExpr.cmpQueryNumber);
			array9[1] = new LogicalExpr.cmpXslt(LogicalExpr.cmpQueryStringE);
			array9[2] = new LogicalExpr.cmpXslt(LogicalExpr.cmpQueryBoolE);
			array9[3] = new LogicalExpr.cmpXslt(LogicalExpr.cmpQueryQueryE);
			array8[num4] = array9;
			array[4] = new LogicalExpr.cmpXslt[]
			{
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfNumber),
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfStringE),
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfBoolE),
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfQueryE),
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfRtfE)
			};
			LogicalExpr.CompXsltE = array;
			LogicalExpr.cmpXslt[][] array10 = new LogicalExpr.cmpXslt[5][];
			LogicalExpr.cmpXslt[][] array11 = array10;
			int num5 = 0;
			LogicalExpr.cmpXslt[] array12 = new LogicalExpr.cmpXslt[5];
			array12[0] = new LogicalExpr.cmpXslt(LogicalExpr.cmpNumberNumber);
			array11[num5] = array12;
			LogicalExpr.cmpXslt[][] array13 = array10;
			int num6 = 1;
			LogicalExpr.cmpXslt[] array14 = new LogicalExpr.cmpXslt[5];
			array14[0] = new LogicalExpr.cmpXslt(LogicalExpr.cmpStringNumber);
			array14[1] = new LogicalExpr.cmpXslt(LogicalExpr.cmpStringStringO);
			array13[num6] = array14;
			LogicalExpr.cmpXslt[][] array15 = array10;
			int num7 = 2;
			LogicalExpr.cmpXslt[] array16 = new LogicalExpr.cmpXslt[5];
			array16[0] = new LogicalExpr.cmpXslt(LogicalExpr.cmpBoolNumberO);
			array16[1] = new LogicalExpr.cmpXslt(LogicalExpr.cmpBoolStringO);
			array16[2] = new LogicalExpr.cmpXslt(LogicalExpr.cmpBoolBoolO);
			array15[num7] = array16;
			LogicalExpr.cmpXslt[][] array17 = array10;
			int num8 = 3;
			LogicalExpr.cmpXslt[] array18 = new LogicalExpr.cmpXslt[5];
			array18[0] = new LogicalExpr.cmpXslt(LogicalExpr.cmpQueryNumber);
			array18[1] = new LogicalExpr.cmpXslt(LogicalExpr.cmpQueryStringO);
			array18[2] = new LogicalExpr.cmpXslt(LogicalExpr.cmpQueryBoolO);
			array18[3] = new LogicalExpr.cmpXslt(LogicalExpr.cmpQueryQueryO);
			array17[num8] = array18;
			array10[4] = new LogicalExpr.cmpXslt[]
			{
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfNumber),
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfStringO),
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfBoolO),
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfQueryO),
				new LogicalExpr.cmpXslt(LogicalExpr.cmpRtfRtfO)
			};
			LogicalExpr.CompXsltO = array10;
		}

		// Token: 0x04000B92 RID: 2962
		private Operator.Op op;

		// Token: 0x04000B93 RID: 2963
		private Query opnd1;

		// Token: 0x04000B94 RID: 2964
		private Query opnd2;

		// Token: 0x04000B95 RID: 2965
		private Operator.Op[] invertOp = new Operator.Op[]
		{
			Operator.Op.GT,
			Operator.Op.LT,
			Operator.Op.GE,
			Operator.Op.LE,
			Operator.Op.EQ,
			Operator.Op.NE
		};

		// Token: 0x04000B96 RID: 2966
		private static readonly LogicalExpr.cmpXslt[][] CompXsltE;

		// Token: 0x04000B97 RID: 2967
		private static readonly LogicalExpr.cmpXslt[][] CompXsltO;

		// Token: 0x02000148 RID: 328
		// (Invoke) Token: 0x06001272 RID: 4722
		private delegate bool cmpXslt(Operator.Op op, object val1, object val2);

		// Token: 0x02000149 RID: 329
		private struct NodeSet
		{
			// Token: 0x06001275 RID: 4725 RVA: 0x00050984 File Offset: 0x0004F984
			public NodeSet(object opnd)
			{
				this.opnd = (Query)opnd;
				this.current = null;
			}

			// Token: 0x06001276 RID: 4726 RVA: 0x00050999 File Offset: 0x0004F999
			public bool MoveNext()
			{
				this.current = this.opnd.Advance();
				return this.current != null;
			}

			// Token: 0x06001277 RID: 4727 RVA: 0x000509B8 File Offset: 0x0004F9B8
			public void Reset()
			{
				this.opnd.Reset();
			}

			// Token: 0x17000482 RID: 1154
			// (get) Token: 0x06001278 RID: 4728 RVA: 0x000509C5 File Offset: 0x0004F9C5
			public string Value
			{
				get
				{
					return this.current.Value;
				}
			}

			// Token: 0x04000B98 RID: 2968
			private Query opnd;

			// Token: 0x04000B99 RID: 2969
			private XPathNavigator current;
		}
	}
}
