using System;
using System.Collections;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000195 RID: 405
	internal class Stylesheet
	{
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001152 RID: 4434 RVA: 0x00053AAC File Offset: 0x00052AAC
		internal bool Whitespace
		{
			get
			{
				return this.whitespace;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x00053AB4 File Offset: 0x00052AB4
		internal ArrayList Imports
		{
			get
			{
				return this.imports;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06001154 RID: 4436 RVA: 0x00053ABC File Offset: 0x00052ABC
		internal Hashtable AttributeSetTable
		{
			get
			{
				return this.attributeSetTable;
			}
		}

		// Token: 0x06001155 RID: 4437 RVA: 0x00053AC4 File Offset: 0x00052AC4
		internal void AddSpace(Compiler compiler, string query, double Priority, bool PreserveSpace)
		{
			Stylesheet.WhitespaceElement whitespaceElement;
			if (this.queryKeyTable != null)
			{
				if (this.queryKeyTable.Contains(query))
				{
					whitespaceElement = (Stylesheet.WhitespaceElement)this.queryKeyTable[query];
					whitespaceElement.ReplaceValue(PreserveSpace);
					return;
				}
			}
			else
			{
				this.queryKeyTable = new Hashtable();
				this.whitespaceList = new ArrayList();
			}
			int num = compiler.AddQuery(query);
			whitespaceElement = new Stylesheet.WhitespaceElement(num, Priority, PreserveSpace);
			this.queryKeyTable[query] = whitespaceElement;
			this.whitespaceList.Add(whitespaceElement);
		}

		// Token: 0x06001156 RID: 4438 RVA: 0x00053B44 File Offset: 0x00052B44
		internal void SortWhiteSpace()
		{
			if (this.queryKeyTable != null)
			{
				for (int i = 0; i < this.whitespaceList.Count; i++)
				{
					for (int j = this.whitespaceList.Count - 1; j > i; j--)
					{
						Stylesheet.WhitespaceElement whitespaceElement = (Stylesheet.WhitespaceElement)this.whitespaceList[j - 1];
						Stylesheet.WhitespaceElement whitespaceElement2 = (Stylesheet.WhitespaceElement)this.whitespaceList[j];
						if (whitespaceElement2.Priority < whitespaceElement.Priority)
						{
							this.whitespaceList[j - 1] = whitespaceElement2;
							this.whitespaceList[j] = whitespaceElement;
						}
					}
				}
				this.whitespace = true;
			}
			if (this.imports != null)
			{
				for (int k = this.imports.Count - 1; k >= 0; k--)
				{
					Stylesheet stylesheet = (Stylesheet)this.imports[k];
					if (stylesheet.Whitespace)
					{
						stylesheet.SortWhiteSpace();
						this.whitespace = true;
					}
				}
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x00053C30 File Offset: 0x00052C30
		internal bool PreserveWhiteSpace(Processor proc, XPathNavigator node)
		{
			if (this.whitespaceList != null)
			{
				int num = this.whitespaceList.Count - 1;
				while (0 <= num)
				{
					Stylesheet.WhitespaceElement whitespaceElement = (Stylesheet.WhitespaceElement)this.whitespaceList[num];
					if (proc.Matches(node, whitespaceElement.Key))
					{
						return whitespaceElement.PreserveSpace;
					}
					num--;
				}
			}
			if (this.imports != null)
			{
				for (int i = this.imports.Count - 1; i >= 0; i--)
				{
					Stylesheet stylesheet = (Stylesheet)this.imports[i];
					if (!stylesheet.PreserveWhiteSpace(proc, node))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x00053CC4 File Offset: 0x00052CC4
		internal void AddAttributeSet(AttributeSetAction attributeSet)
		{
			if (this.attributeSetTable == null)
			{
				this.attributeSetTable = new Hashtable();
			}
			if (!this.attributeSetTable.ContainsKey(attributeSet.Name))
			{
				this.attributeSetTable[attributeSet.Name] = attributeSet;
				return;
			}
			((AttributeSetAction)this.attributeSetTable[attributeSet.Name]).Merge(attributeSet);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x00053D28 File Offset: 0x00052D28
		internal void AddTemplate(TemplateAction template)
		{
			XmlQualifiedName xmlQualifiedName = template.Mode;
			if (template.Name != null)
			{
				if (this.templateNameTable.ContainsKey(template.Name))
				{
					throw XsltException.Create("Xslt_DupTemplateName", new string[] { template.Name.ToString() });
				}
				this.templateNameTable[template.Name] = template;
			}
			if (template.MatchKey != -1)
			{
				if (this.modeManagers == null)
				{
					this.modeManagers = new Hashtable();
				}
				if (xmlQualifiedName == null)
				{
					xmlQualifiedName = XmlQualifiedName.Empty;
				}
				TemplateManager templateManager = (TemplateManager)this.modeManagers[xmlQualifiedName];
				if (templateManager == null)
				{
					templateManager = new TemplateManager(this, xmlQualifiedName);
					this.modeManagers[xmlQualifiedName] = templateManager;
					if (xmlQualifiedName.IsEmpty)
					{
						this.templates = templateManager;
					}
				}
				template.TemplateId = ++this.templateCount;
				templateManager.AddTemplate(template);
			}
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x00053E14 File Offset: 0x00052E14
		internal void ProcessTemplates()
		{
			if (this.modeManagers != null)
			{
				IDictionaryEnumerator enumerator = this.modeManagers.GetEnumerator();
				while (enumerator.MoveNext())
				{
					TemplateManager templateManager = (TemplateManager)enumerator.Value;
					templateManager.ProcessTemplates();
				}
			}
			if (this.imports != null)
			{
				for (int i = this.imports.Count - 1; i >= 0; i--)
				{
					Stylesheet stylesheet = (Stylesheet)this.imports[i];
					stylesheet.ProcessTemplates();
				}
			}
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x00053E8C File Offset: 0x00052E8C
		internal void ReplaceNamespaceAlias(Compiler compiler)
		{
			if (this.modeManagers != null)
			{
				IDictionaryEnumerator enumerator = this.modeManagers.GetEnumerator();
				while (enumerator.MoveNext())
				{
					TemplateManager templateManager = (TemplateManager)enumerator.Value;
					if (templateManager.templates != null)
					{
						for (int i = 0; i < templateManager.templates.Count; i++)
						{
							TemplateAction templateAction = (TemplateAction)templateManager.templates[i];
							templateAction.ReplaceNamespaceAlias(compiler);
						}
					}
				}
			}
			if (this.templateNameTable != null)
			{
				IDictionaryEnumerator enumerator2 = this.templateNameTable.GetEnumerator();
				while (enumerator2.MoveNext())
				{
					TemplateAction templateAction2 = (TemplateAction)enumerator2.Value;
					templateAction2.ReplaceNamespaceAlias(compiler);
				}
			}
			if (this.imports != null)
			{
				for (int j = this.imports.Count - 1; j >= 0; j--)
				{
					Stylesheet stylesheet = (Stylesheet)this.imports[j];
					stylesheet.ReplaceNamespaceAlias(compiler);
				}
			}
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x00053F70 File Offset: 0x00052F70
		internal TemplateAction FindTemplate(Processor processor, XPathNavigator navigator, XmlQualifiedName mode)
		{
			TemplateAction templateAction = null;
			if (this.modeManagers != null)
			{
				TemplateManager templateManager = (TemplateManager)this.modeManagers[mode];
				if (templateManager != null)
				{
					templateAction = templateManager.FindTemplate(processor, navigator);
				}
			}
			if (templateAction == null)
			{
				templateAction = this.FindTemplateImports(processor, navigator, mode);
			}
			return templateAction;
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x00053FB4 File Offset: 0x00052FB4
		internal TemplateAction FindTemplateImports(Processor processor, XPathNavigator navigator, XmlQualifiedName mode)
		{
			TemplateAction templateAction = null;
			if (this.imports != null)
			{
				for (int i = this.imports.Count - 1; i >= 0; i--)
				{
					Stylesheet stylesheet = (Stylesheet)this.imports[i];
					templateAction = stylesheet.FindTemplate(processor, navigator, mode);
					if (templateAction != null)
					{
						return templateAction;
					}
				}
			}
			return templateAction;
		}

		// Token: 0x0600115E RID: 4446 RVA: 0x00054008 File Offset: 0x00053008
		internal TemplateAction FindTemplate(Processor processor, XPathNavigator navigator)
		{
			TemplateAction templateAction = null;
			if (this.templates != null)
			{
				templateAction = this.templates.FindTemplate(processor, navigator);
			}
			if (templateAction == null)
			{
				templateAction = this.FindTemplateImports(processor, navigator);
			}
			return templateAction;
		}

		// Token: 0x0600115F RID: 4447 RVA: 0x0005403C File Offset: 0x0005303C
		internal TemplateAction FindTemplate(XmlQualifiedName name)
		{
			TemplateAction templateAction = null;
			if (this.templateNameTable != null)
			{
				templateAction = (TemplateAction)this.templateNameTable[name];
			}
			if (templateAction == null && this.imports != null)
			{
				for (int i = this.imports.Count - 1; i >= 0; i--)
				{
					Stylesheet stylesheet = (Stylesheet)this.imports[i];
					templateAction = stylesheet.FindTemplate(name);
					if (templateAction != null)
					{
						return templateAction;
					}
				}
			}
			return templateAction;
		}

		// Token: 0x06001160 RID: 4448 RVA: 0x000540A8 File Offset: 0x000530A8
		internal TemplateAction FindTemplateImports(Processor processor, XPathNavigator navigator)
		{
			TemplateAction templateAction = null;
			if (this.imports != null)
			{
				for (int i = this.imports.Count - 1; i >= 0; i--)
				{
					Stylesheet stylesheet = (Stylesheet)this.imports[i];
					templateAction = stylesheet.FindTemplate(processor, navigator);
					if (templateAction != null)
					{
						return templateAction;
					}
				}
			}
			return templateAction;
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06001161 RID: 4449 RVA: 0x000540F8 File Offset: 0x000530F8
		internal Hashtable ScriptObjectTypes
		{
			get
			{
				return this.scriptObjectTypes;
			}
		}

		// Token: 0x04000BB7 RID: 2999
		private ArrayList imports = new ArrayList();

		// Token: 0x04000BB8 RID: 3000
		private Hashtable modeManagers;

		// Token: 0x04000BB9 RID: 3001
		private Hashtable templateNameTable = new Hashtable();

		// Token: 0x04000BBA RID: 3002
		private Hashtable attributeSetTable;

		// Token: 0x04000BBB RID: 3003
		private int templateCount;

		// Token: 0x04000BBC RID: 3004
		private Hashtable queryKeyTable;

		// Token: 0x04000BBD RID: 3005
		private ArrayList whitespaceList;

		// Token: 0x04000BBE RID: 3006
		private bool whitespace;

		// Token: 0x04000BBF RID: 3007
		private Hashtable scriptObjectTypes = new Hashtable();

		// Token: 0x04000BC0 RID: 3008
		private TemplateManager templates;

		// Token: 0x02000196 RID: 406
		private class WhitespaceElement
		{
			// Token: 0x170002B7 RID: 695
			// (get) Token: 0x06001163 RID: 4451 RVA: 0x00054129 File Offset: 0x00053129
			internal double Priority
			{
				get
				{
					return this.priority;
				}
			}

			// Token: 0x170002B8 RID: 696
			// (get) Token: 0x06001164 RID: 4452 RVA: 0x00054131 File Offset: 0x00053131
			internal int Key
			{
				get
				{
					return this.key;
				}
			}

			// Token: 0x170002B9 RID: 697
			// (get) Token: 0x06001165 RID: 4453 RVA: 0x00054139 File Offset: 0x00053139
			internal bool PreserveSpace
			{
				get
				{
					return this.preserveSpace;
				}
			}

			// Token: 0x06001166 RID: 4454 RVA: 0x00054141 File Offset: 0x00053141
			internal WhitespaceElement(int Key, double priority, bool PreserveSpace)
			{
				this.key = Key;
				this.priority = priority;
				this.preserveSpace = PreserveSpace;
			}

			// Token: 0x06001167 RID: 4455 RVA: 0x0005415E File Offset: 0x0005315E
			internal void ReplaceValue(bool PreserveSpace)
			{
				this.preserveSpace = PreserveSpace;
			}

			// Token: 0x04000BC1 RID: 3009
			private int key;

			// Token: 0x04000BC2 RID: 3010
			private double priority;

			// Token: 0x04000BC3 RID: 3011
			private bool preserveSpace;
		}
	}
}
