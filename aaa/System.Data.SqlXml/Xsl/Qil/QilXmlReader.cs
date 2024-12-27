using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000061 RID: 97
	internal sealed class QilXmlReader
	{
		// Token: 0x06000699 RID: 1689 RVA: 0x0002326C File Offset: 0x0002226C
		static QilXmlReader()
		{
			foreach (MethodInfo methodInfo in typeof(QilFactory).GetMethods(BindingFlags.Instance | BindingFlags.Public))
			{
				ParameterInfo[] parameters = methodInfo.GetParameters();
				int num = 0;
				while (num < parameters.Length && parameters[num].ParameterType == typeof(QilNode))
				{
					num++;
				}
				if (num == parameters.Length && (!QilXmlReader.nameToFactoryMethod.ContainsKey(methodInfo.Name) || QilXmlReader.nameToFactoryMethod[methodInfo.Name].GetParameters().Length < parameters.Length))
				{
					QilXmlReader.nameToFactoryMethod[methodInfo.Name] = methodInfo;
				}
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00023338 File Offset: 0x00022338
		public QilXmlReader(XmlReader r)
		{
			this.r = r;
			this.f = new QilFactory();
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00023354 File Offset: 0x00022354
		public QilExpression Read()
		{
			this.stk = new Stack<QilList>();
			this.inFwdDecls = false;
			this.scope = new Dictionary<string, QilNode>();
			this.fwdDecls = new Dictionary<string, QilNode>();
			this.stk.Push(this.f.Sequence());
			while (this.r.Read())
			{
				XmlNodeType nodeType = this.r.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					switch (nodeType)
					{
					case XmlNodeType.EndElement:
						this.EndElement();
						break;
					}
				}
				else
				{
					bool isEmptyElement = this.r.IsEmptyElement;
					if (this.StartElement() && isEmptyElement)
					{
						this.EndElement();
					}
				}
			}
			return (QilExpression)this.stk.Peek()[0];
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00023430 File Offset: 0x00022430
		private bool StartElement()
		{
			QilXmlReader.ReaderAnnotation readerAnnotation = new QilXmlReader.ReaderAnnotation();
			string localName = this.r.LocalName;
			string localName2;
			QilNode qilNode;
			switch (localName2 = this.r.LocalName)
			{
			case "LiteralString":
				qilNode = this.f.LiteralString(this.ReadText());
				goto IL_026F;
			case "LiteralInt32":
				qilNode = this.f.LiteralInt32(int.Parse(this.ReadText(), CultureInfo.InvariantCulture));
				goto IL_026F;
			case "LiteralInt64":
				qilNode = this.f.LiteralInt64(long.Parse(this.ReadText(), CultureInfo.InvariantCulture));
				goto IL_026F;
			case "LiteralDouble":
				qilNode = this.f.LiteralDouble(double.Parse(this.ReadText(), CultureInfo.InvariantCulture));
				goto IL_026F;
			case "LiteralDecimal":
				qilNode = this.f.LiteralDecimal(decimal.Parse(this.ReadText(), CultureInfo.InvariantCulture));
				goto IL_026F;
			case "LiteralType":
				qilNode = this.f.LiteralType(this.ParseType(this.ReadText()));
				goto IL_026F;
			case "LiteralQName":
				qilNode = this.ParseName(this.r.GetAttribute("name"));
				goto IL_026F;
			case "For":
			case "Let":
			case "Parameter":
			case "Function":
			case "RefTo":
				readerAnnotation.Id = this.r.GetAttribute("id");
				readerAnnotation.Name = this.ParseName(this.r.GetAttribute("name"));
				break;
			case "XsltInvokeEarlyBound":
				readerAnnotation.ClrNamespace = this.r.GetAttribute("clrNamespace");
				break;
			case "ForwardDecls":
				this.inFwdDecls = true;
				break;
			}
			qilNode = this.f.Sequence();
			IL_026F:
			readerAnnotation.XmlType = this.ParseType(this.r.GetAttribute("xmlType"));
			qilNode.SourceLine = this.ParseLineInfo(this.r.GetAttribute("lineInfo"));
			qilNode.Annotation = readerAnnotation;
			if (qilNode is QilList)
			{
				this.stk.Push((QilList)qilNode);
				return true;
			}
			this.stk.Peek().Add(qilNode);
			return false;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00023718 File Offset: 0x00022718
		private void EndElement()
		{
			QilList qilList = this.stk.Pop();
			QilXmlReader.ReaderAnnotation readerAnnotation = (QilXmlReader.ReaderAnnotation)qilList.Annotation;
			string localName = this.r.LocalName;
			string localName2;
			QilNode qilNode;
			switch (localName2 = this.r.LocalName)
			{
			case "QilExpression":
			{
				QilExpression qilExpression = this.f.QilExpression(qilList[qilList.Count - 1]);
				for (int i = 0; i < qilList.Count - 1; i++)
				{
					QilNodeType nodeType = qilList[i].NodeType;
					switch (nodeType)
					{
					case QilNodeType.FunctionList:
						qilExpression.FunctionList = (QilList)qilList[i];
						break;
					case QilNodeType.GlobalVariableList:
						qilExpression.GlobalVariableList = (QilList)qilList[i];
						break;
					case QilNodeType.GlobalParameterList:
						qilExpression.GlobalParameterList = (QilList)qilList[i];
						break;
					default:
						switch (nodeType)
						{
						case QilNodeType.True:
						case QilNodeType.False:
							qilExpression.IsDebug = qilList[i].NodeType == QilNodeType.True;
							break;
						}
						break;
					}
				}
				qilNode = qilExpression;
				goto IL_0605;
			}
			case "ForwardDecls":
				this.inFwdDecls = false;
				return;
			case "Parameter":
			case "Let":
			case "For":
			case "Function":
			{
				string id = readerAnnotation.Id;
				QilName name = readerAnnotation.Name;
				string localName3;
				if ((localName3 = this.r.LocalName) != null)
				{
					if (!(localName3 == "Parameter"))
					{
						if (!(localName3 == "Let"))
						{
							if (localName3 == "For")
							{
								qilNode = this.f.For(qilList[0]);
								goto IL_03C1;
							}
						}
						else
						{
							if (this.inFwdDecls)
							{
								qilNode = this.f.Let(this.f.Unknown(readerAnnotation.XmlType));
								goto IL_03C1;
							}
							qilNode = this.f.Let(qilList[0]);
							goto IL_03C1;
						}
					}
					else
					{
						if (this.inFwdDecls || qilList.Count == 0)
						{
							qilNode = this.f.Parameter(null, name, readerAnnotation.XmlType);
							goto IL_03C1;
						}
						qilNode = this.f.Parameter(qilList[0], name, readerAnnotation.XmlType);
						goto IL_03C1;
					}
				}
				if (this.inFwdDecls)
				{
					qilNode = this.f.Function(qilList[0], qilList[1], readerAnnotation.XmlType);
				}
				else
				{
					qilNode = this.f.Function(qilList[0], qilList[1], qilList[2], (readerAnnotation.XmlType != null) ? readerAnnotation.XmlType : qilList[1].XmlType);
				}
				IL_03C1:
				if (name != null)
				{
					((QilReference)qilNode).DebugName = name.ToString();
				}
				if (this.inFwdDecls)
				{
					this.fwdDecls[id] = qilNode;
					this.scope[id] = qilNode;
				}
				else if (this.fwdDecls.ContainsKey(id))
				{
					qilNode = this.fwdDecls[id];
					this.fwdDecls.Remove(id);
					if (qilList.Count > 0)
					{
						qilNode[0] = qilList[0];
					}
					if (qilList.Count > 1)
					{
						qilNode[1] = qilList[1];
					}
				}
				else
				{
					this.scope[id] = qilNode;
				}
				qilNode.Annotation = readerAnnotation;
				goto IL_0605;
			}
			case "RefTo":
			{
				string id2 = readerAnnotation.Id;
				this.stk.Peek().Add(this.scope[id2]);
				return;
			}
			case "Sequence":
				qilNode = this.f.Sequence(qilList);
				goto IL_0605;
			case "FunctionList":
				qilNode = this.f.FunctionList(qilList);
				goto IL_0605;
			case "GlobalVariableList":
				qilNode = this.f.GlobalVariableList(qilList);
				goto IL_0605;
			case "GlobalParameterList":
				qilNode = this.f.GlobalParameterList(qilList);
				goto IL_0605;
			case "ActualParameterList":
				qilNode = this.f.ActualParameterList(qilList);
				goto IL_0605;
			case "FormalParameterList":
				qilNode = this.f.FormalParameterList(qilList);
				goto IL_0605;
			case "SortKeyList":
				qilNode = this.f.SortKeyList(qilList);
				goto IL_0605;
			case "BranchList":
				qilNode = this.f.BranchList(qilList);
				goto IL_0605;
			case "XsltInvokeEarlyBound":
			{
				MethodInfo methodInfo = null;
				QilName qilName = (QilName)qilList[0];
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					Type type = assembly.GetType(readerAnnotation.ClrNamespace);
					if (type != null)
					{
						methodInfo = type.GetMethod(qilName.LocalName);
						break;
					}
				}
				qilNode = this.f.XsltInvokeEarlyBound(qilName, this.f.LiteralObject(methodInfo), qilList[1], readerAnnotation.XmlType);
				goto IL_0605;
			}
			}
			MethodInfo methodInfo2 = QilXmlReader.nameToFactoryMethod[this.r.LocalName];
			object[] array = new object[qilList.Count];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = qilList[k];
			}
			qilNode = (QilNode)methodInfo2.Invoke(this.f, array);
			IL_0605:
			qilNode.SourceLine = qilList.SourceLine;
			this.stk.Peek().Add(qilNode);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00023D48 File Offset: 0x00022D48
		private string ReadText()
		{
			string text = string.Empty;
			if (!this.r.IsEmptyElement)
			{
				while (this.r.Read())
				{
					XmlNodeType nodeType = this.r.NodeType;
					if (nodeType != XmlNodeType.Text)
					{
						switch (nodeType)
						{
						case XmlNodeType.Whitespace:
						case XmlNodeType.SignificantWhitespace:
							break;
						default:
							return text;
						}
					}
					text += this.r.Value;
				}
			}
			return text;
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00023DB0 File Offset: 0x00022DB0
		private ISourceLineInfo ParseLineInfo(string s)
		{
			if (s != null && s.Length > 0)
			{
				Match match = QilXmlReader.lineInfoRegex.Match(s);
				return new SourceLineInfo("", int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture), int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture), int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture), int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture));
			}
			return null;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00023E54 File Offset: 0x00022E54
		private XmlQueryType ParseType(string s)
		{
			if (s != null && s.Length > 0)
			{
				Match match = QilXmlReader.typeInfoRegex.Match(s);
				XmlQueryCardinality xmlQueryCardinality = new XmlQueryCardinality(match.Groups[1].Value);
				bool flag = bool.Parse(match.Groups[3].Value);
				string[] array = match.Groups[2].Value.Split(new char[] { '|' });
				XmlQueryType[] array2 = new XmlQueryType[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array2[i] = XmlQueryTypeFactory.Type((XmlTypeCode)Enum.Parse(typeof(XmlTypeCode), array[i]), flag);
				}
				return XmlQueryTypeFactory.Product(XmlQueryTypeFactory.Choice(array2), xmlQueryCardinality);
			}
			return null;
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00023F24 File Offset: 0x00022F24
		private QilName ParseName(string name)
		{
			if (name != null && name.Length > 0)
			{
				int num = name.LastIndexOf('}');
				string text;
				if (num != -1 && name[0] == '{')
				{
					text = name.Substring(1, num - 1);
					name = name.Substring(num + 1);
				}
				else
				{
					text = string.Empty;
				}
				string text2;
				string text3;
				ValidateNames.ParseQNameThrow(name, out text2, out text3);
				return this.f.LiteralQName(text3, text, text2);
			}
			return null;
		}

		// Token: 0x04000410 RID: 1040
		private static Regex lineInfoRegex = new Regex("\\[(\\d+),(\\d+) -- (\\d+),(\\d+)\\]");

		// Token: 0x04000411 RID: 1041
		private static Regex typeInfoRegex = new Regex("(\\w+);([\\w|\\|]+);(\\w+)");

		// Token: 0x04000412 RID: 1042
		private static Dictionary<string, MethodInfo> nameToFactoryMethod = new Dictionary<string, MethodInfo>();

		// Token: 0x04000413 RID: 1043
		private QilFactory f;

		// Token: 0x04000414 RID: 1044
		private XmlReader r;

		// Token: 0x04000415 RID: 1045
		private Stack<QilList> stk;

		// Token: 0x04000416 RID: 1046
		private bool inFwdDecls;

		// Token: 0x04000417 RID: 1047
		private Dictionary<string, QilNode> scope;

		// Token: 0x04000418 RID: 1048
		private Dictionary<string, QilNode> fwdDecls;

		// Token: 0x02000062 RID: 98
		private class ReaderAnnotation
		{
			// Token: 0x04000419 RID: 1049
			public string Id;

			// Token: 0x0400041A RID: 1050
			public QilName Name;

			// Token: 0x0400041B RID: 1051
			public XmlQueryType XmlType;

			// Token: 0x0400041C RID: 1052
			public string ClrNamespace;
		}
	}
}
