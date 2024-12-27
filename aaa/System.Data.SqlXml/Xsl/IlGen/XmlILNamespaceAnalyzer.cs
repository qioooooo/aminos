using System;
using System.Collections.Generic;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000036 RID: 54
	internal class XmlILNamespaceAnalyzer
	{
		// Token: 0x06000342 RID: 834 RVA: 0x00015ADC File Offset: 0x00014ADC
		public void Analyze(QilNode nd, bool defaultNmspInScope)
		{
			this.addInScopeNmsp = false;
			this.cntNmsp = 0;
			if (defaultNmspInScope)
			{
				this.nsmgr.PushScope();
				this.nsmgr.AddNamespace(string.Empty, string.Empty);
				this.cntNmsp++;
			}
			this.AnalyzeContent(nd);
			if (defaultNmspInScope)
			{
				this.nsmgr.PopScope();
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x00015B40 File Offset: 0x00014B40
		private void AnalyzeContent(QilNode nd)
		{
			QilNodeType nodeType = nd.NodeType;
			if (nodeType <= QilNodeType.Sequence)
			{
				if (nodeType != QilNodeType.Nop)
				{
					switch (nodeType)
					{
					case QilNodeType.Conditional:
						break;
					case QilNodeType.Choice:
					{
						this.addInScopeNmsp = false;
						QilList branches = (nd as QilChoice).Branches;
						for (int i = 0; i < branches.Count; i++)
						{
							this.AnalyzeContent(branches[i]);
						}
						return;
					}
					case QilNodeType.Length:
						goto IL_018C;
					case QilNodeType.Sequence:
					{
						using (IEnumerator<QilNode> enumerator = nd.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								QilNode qilNode = enumerator.Current;
								this.AnalyzeContent(qilNode);
							}
							return;
						}
						break;
					}
					default:
						goto IL_018C;
					}
					this.addInScopeNmsp = false;
					this.AnalyzeContent((nd as QilTernary).Center);
					this.AnalyzeContent((nd as QilTernary).Right);
					return;
				}
				this.AnalyzeContent((nd as QilUnary).Child);
				return;
			}
			else
			{
				if (nodeType == QilNodeType.Loop)
				{
					this.addInScopeNmsp = false;
					this.AnalyzeContent((nd as QilLoop).Body);
					return;
				}
				switch (nodeType)
				{
				case QilNodeType.ElementCtor:
				{
					this.addInScopeNmsp = true;
					this.nsmgr.PushScope();
					int num = this.cntNmsp;
					if (this.CheckNamespaceInScope(nd as QilBinary))
					{
						this.AnalyzeContent((nd as QilBinary).Right);
					}
					this.nsmgr.PopScope();
					this.addInScopeNmsp = false;
					this.cntNmsp = num;
					return;
				}
				case QilNodeType.AttributeCtor:
					this.addInScopeNmsp = false;
					this.CheckNamespaceInScope(nd as QilBinary);
					return;
				default:
					if (nodeType == QilNodeType.NamespaceDecl)
					{
						this.CheckNamespaceInScope(nd as QilBinary);
						return;
					}
					break;
				}
			}
			IL_018C:
			this.addInScopeNmsp = false;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00015CF0 File Offset: 0x00014CF0
		private bool CheckNamespaceInScope(QilBinary nd)
		{
			string text;
			string text2;
			XPathNodeType xpathNodeType;
			switch (nd.NodeType)
			{
			case QilNodeType.ElementCtor:
			case QilNodeType.AttributeCtor:
			{
				QilName qilName = nd.Left as QilName;
				if (qilName == null)
				{
					return false;
				}
				text = qilName.Prefix;
				text2 = qilName.NamespaceUri;
				xpathNodeType = ((nd.NodeType == QilNodeType.ElementCtor) ? XPathNodeType.Element : XPathNodeType.Attribute);
				break;
			}
			default:
				text = (QilLiteral)nd.Left;
				text2 = (QilLiteral)nd.Right;
				xpathNodeType = XPathNodeType.Namespace;
				break;
			}
			if ((nd.NodeType == QilNodeType.AttributeCtor && text2.Length == 0) || (text == "xml" && text2 == "http://www.w3.org/XML/1998/namespace"))
			{
				XmlILConstructInfo.Write(nd).IsNamespaceInScope = true;
				return true;
			}
			if (!ValidateNames.ValidateName(text, string.Empty, text2, xpathNodeType, ValidateNames.Flags.CheckPrefixMapping))
			{
				return false;
			}
			text = this.nsmgr.NameTable.Add(text);
			text2 = this.nsmgr.NameTable.Add(text2);
			int i = 0;
			while (i < this.cntNmsp)
			{
				string text3;
				string text4;
				this.nsmgr.GetNamespaceDeclaration(i, out text3, out text4);
				if (text == text3)
				{
					if (text2 == text4)
					{
						XmlILConstructInfo.Write(nd).IsNamespaceInScope = true;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			if (this.addInScopeNmsp)
			{
				this.nsmgr.AddNamespace(text, text2);
				this.cntNmsp++;
			}
			return true;
		}

		// Token: 0x040002BD RID: 701
		private XmlNamespaceManager nsmgr = new XmlNamespaceManager(new NameTable());

		// Token: 0x040002BE RID: 702
		private bool addInScopeNmsp;

		// Token: 0x040002BF RID: 703
		private int cntNmsp;
	}
}
