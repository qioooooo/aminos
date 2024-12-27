using System;
using System.Collections;
using System.Text;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200018F RID: 399
	internal sealed class RecordBuilder
	{
		// Token: 0x060010EE RID: 4334 RVA: 0x000516B8 File Offset: 0x000506B8
		internal RecordBuilder(RecordOutput output, XmlNameTable nameTable)
		{
			this.output = output;
			this.nameTable = ((nameTable != null) ? nameTable : new NameTable());
			this.atoms = new OutKeywords(this.nameTable);
			this.scopeManager = new OutputScopeManager(this.nameTable, this.atoms);
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x00051737 File Offset: 0x00050737
		// (set) Token: 0x060010F0 RID: 4336 RVA: 0x0005173F File Offset: 0x0005073F
		internal int OutputState
		{
			get
			{
				return this.outputState;
			}
			set
			{
				this.outputState = value;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00051748 File Offset: 0x00050748
		// (set) Token: 0x060010F2 RID: 4338 RVA: 0x00051750 File Offset: 0x00050750
		internal RecordBuilder Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00051759 File Offset: 0x00050759
		internal RecordOutput Output
		{
			get
			{
				return this.output;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00051761 File Offset: 0x00050761
		internal BuilderInfo MainNode
		{
			get
			{
				return this.mainNode;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00051769 File Offset: 0x00050769
		internal ArrayList AttributeList
		{
			get
			{
				return this.attributeList;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00051771 File Offset: 0x00050771
		internal int AttributeCount
		{
			get
			{
				return this.attributeCount;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00051779 File Offset: 0x00050779
		internal OutputScopeManager Manager
		{
			get
			{
				return this.scopeManager;
			}
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x00051781 File Offset: 0x00050781
		private void ValueAppend(string s, bool disableOutputEscaping)
		{
			this.currentInfo.ValueAppend(s, disableOutputEscaping);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x00051790 File Offset: 0x00050790
		private bool CanOutput(int state)
		{
			if (this.recordState == 0 || (state & 8192) == 0)
			{
				return true;
			}
			this.recordState = 2;
			this.FinalizeRecord();
			this.SetEmptyFlag(state);
			return this.output.RecordDone(this) == Processor.OutputResult.Continue;
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x000517C8 File Offset: 0x000507C8
		internal Processor.OutputResult BeginEvent(int state, XPathNodeType nodeType, string prefix, string name, string nspace, bool empty, object htmlProps, bool search)
		{
			if (!this.CanOutput(state))
			{
				return Processor.OutputResult.Overflow;
			}
			this.AdjustDepth(state);
			this.ResetRecord(state);
			this.PopElementScope();
			prefix = ((prefix != null) ? this.nameTable.Add(prefix) : this.atoms.Empty);
			name = ((name != null) ? this.nameTable.Add(name) : this.atoms.Empty);
			nspace = ((nspace != null) ? this.nameTable.Add(nspace) : this.atoms.Empty);
			switch (nodeType)
			{
			case XPathNodeType.Element:
				this.mainNode.htmlProps = htmlProps as HtmlElementProps;
				this.mainNode.search = search;
				this.BeginElement(prefix, name, nspace, empty);
				break;
			case XPathNodeType.Attribute:
				this.BeginAttribute(prefix, name, nspace, htmlProps, search);
				break;
			case XPathNodeType.Namespace:
				this.BeginNamespace(name, nspace);
				break;
			case XPathNodeType.ProcessingInstruction:
				if (!this.BeginProcessingInstruction(prefix, name, nspace))
				{
					return Processor.OutputResult.Error;
				}
				break;
			case XPathNodeType.Comment:
				this.BeginComment();
				break;
			}
			return this.CheckRecordBegin(state);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x000518EC File Offset: 0x000508EC
		internal Processor.OutputResult TextEvent(int state, string text, bool disableOutputEscaping)
		{
			if (!this.CanOutput(state))
			{
				return Processor.OutputResult.Overflow;
			}
			this.AdjustDepth(state);
			this.ResetRecord(state);
			this.PopElementScope();
			if ((state & 8192) != 0)
			{
				this.currentInfo.Depth = this.recordDepth;
				this.currentInfo.NodeType = XmlNodeType.Text;
			}
			this.ValueAppend(text, disableOutputEscaping);
			return this.CheckRecordBegin(state);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x00051950 File Offset: 0x00050950
		internal Processor.OutputResult EndEvent(int state, XPathNodeType nodeType)
		{
			if (!this.CanOutput(state))
			{
				return Processor.OutputResult.Overflow;
			}
			this.AdjustDepth(state);
			this.PopElementScope();
			this.popScope = (state & 65536) != 0;
			if ((state & 4096) != 0 && this.mainNode.IsEmptyTag)
			{
				return Processor.OutputResult.Continue;
			}
			this.ResetRecord(state);
			if ((state & 8192) != 0 && nodeType == XPathNodeType.Element)
			{
				this.EndElement();
			}
			return this.CheckRecordEnd(state);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x000519C1 File Offset: 0x000509C1
		internal void Reset()
		{
			if (this.recordState == 2)
			{
				this.recordState = 0;
			}
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x000519D3 File Offset: 0x000509D3
		internal void TheEnd()
		{
			if (this.recordState == 1)
			{
				this.recordState = 2;
				this.FinalizeRecord();
				this.output.RecordDone(this);
			}
			this.output.TheEnd();
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x00051A04 File Offset: 0x00050A04
		private int FindAttribute(string name, string nspace, ref string prefix)
		{
			for (int i = 0; i < this.attributeCount; i++)
			{
				BuilderInfo builderInfo = (BuilderInfo)this.attributeList[i];
				if (Keywords.Equals(builderInfo.LocalName, name))
				{
					if (Keywords.Equals(builderInfo.NamespaceURI, nspace))
					{
						return i;
					}
					if (Keywords.Equals(builderInfo.Prefix, prefix))
					{
						prefix = string.Empty;
					}
				}
			}
			return -1;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x00051A6C File Offset: 0x00050A6C
		private void BeginElement(string prefix, string name, string nspace, bool empty)
		{
			this.currentInfo.NodeType = XmlNodeType.Element;
			this.currentInfo.Prefix = prefix;
			this.currentInfo.LocalName = name;
			this.currentInfo.NamespaceURI = nspace;
			this.currentInfo.Depth = this.recordDepth;
			this.currentInfo.IsEmptyTag = empty;
			this.scopeManager.PushScope(name, nspace, prefix);
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x00051AD8 File Offset: 0x00050AD8
		private void EndElement()
		{
			OutputScope currentElementScope = this.scopeManager.CurrentElementScope;
			this.currentInfo.NodeType = XmlNodeType.EndElement;
			this.currentInfo.Prefix = currentElementScope.Prefix;
			this.currentInfo.LocalName = currentElementScope.Name;
			this.currentInfo.NamespaceURI = currentElementScope.Namespace;
			this.currentInfo.Depth = this.recordDepth;
		}

		// Token: 0x06001102 RID: 4354 RVA: 0x00051B44 File Offset: 0x00050B44
		private int NewAttribute()
		{
			if (this.attributeCount >= this.attributeList.Count)
			{
				this.attributeList.Add(new BuilderInfo());
			}
			return this.attributeCount++;
		}

		// Token: 0x06001103 RID: 4355 RVA: 0x00051B88 File Offset: 0x00050B88
		private void BeginAttribute(string prefix, string name, string nspace, object htmlAttrProps, bool search)
		{
			int num = this.FindAttribute(name, nspace, ref prefix);
			if (num == -1)
			{
				num = this.NewAttribute();
			}
			BuilderInfo builderInfo = (BuilderInfo)this.attributeList[num];
			builderInfo.Initialize(prefix, name, nspace);
			builderInfo.Depth = this.recordDepth;
			builderInfo.NodeType = XmlNodeType.Attribute;
			builderInfo.htmlAttrProps = htmlAttrProps as HtmlAttributeProps;
			builderInfo.search = search;
			this.currentInfo = builderInfo;
		}

		// Token: 0x06001104 RID: 4356 RVA: 0x00051BF8 File Offset: 0x00050BF8
		private void BeginNamespace(string name, string nspace)
		{
			bool flag = false;
			if (Keywords.Equals(name, this.atoms.Empty))
			{
				if (!Keywords.Equals(nspace, this.scopeManager.DefaultNamespace) && !Keywords.Equals(this.mainNode.NamespaceURI, this.atoms.Empty))
				{
					this.DeclareNamespace(nspace, name);
				}
			}
			else
			{
				string text = this.scopeManager.ResolveNamespace(name, out flag);
				if (text != null)
				{
					if (!Keywords.Equals(nspace, text) && !flag)
					{
						this.DeclareNamespace(nspace, name);
					}
				}
				else
				{
					this.DeclareNamespace(nspace, name);
				}
			}
			this.currentInfo = this.dummy;
			this.currentInfo.NodeType = XmlNodeType.Attribute;
		}

		// Token: 0x06001105 RID: 4357 RVA: 0x00051C9C File Offset: 0x00050C9C
		private bool BeginProcessingInstruction(string prefix, string name, string nspace)
		{
			this.currentInfo.NodeType = XmlNodeType.ProcessingInstruction;
			this.currentInfo.Prefix = prefix;
			this.currentInfo.LocalName = name;
			this.currentInfo.NamespaceURI = nspace;
			this.currentInfo.Depth = this.recordDepth;
			return true;
		}

		// Token: 0x06001106 RID: 4358 RVA: 0x00051CEB File Offset: 0x00050CEB
		private void BeginComment()
		{
			this.currentInfo.NodeType = XmlNodeType.Comment;
			this.currentInfo.Depth = this.recordDepth;
		}

		// Token: 0x06001107 RID: 4359 RVA: 0x00051D0C File Offset: 0x00050D0C
		private void AdjustDepth(int state)
		{
			int num = state & 768;
			if (num == 256)
			{
				this.recordDepth++;
				return;
			}
			if (num != 512)
			{
				return;
			}
			this.recordDepth--;
		}

		// Token: 0x06001108 RID: 4360 RVA: 0x00051D50 File Offset: 0x00050D50
		private void ResetRecord(int state)
		{
			if ((state & 8192) != 0)
			{
				this.attributeCount = 0;
				this.namespaceCount = 0;
				this.currentInfo = this.mainNode;
				this.currentInfo.Initialize(this.atoms.Empty, this.atoms.Empty, this.atoms.Empty);
				this.currentInfo.NodeType = XmlNodeType.None;
				this.currentInfo.IsEmptyTag = false;
				this.currentInfo.htmlProps = null;
				this.currentInfo.htmlAttrProps = null;
			}
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x00051DDC File Offset: 0x00050DDC
		private void PopElementScope()
		{
			if (this.popScope)
			{
				this.scopeManager.PopScope();
				this.popScope = false;
			}
		}

		// Token: 0x0600110A RID: 4362 RVA: 0x00051DF8 File Offset: 0x00050DF8
		private Processor.OutputResult CheckRecordBegin(int state)
		{
			if ((state & 16384) != 0)
			{
				this.recordState = 2;
				this.FinalizeRecord();
				this.SetEmptyFlag(state);
				return this.output.RecordDone(this);
			}
			this.recordState = 1;
			return Processor.OutputResult.Continue;
		}

		// Token: 0x0600110B RID: 4363 RVA: 0x00051E2C File Offset: 0x00050E2C
		private Processor.OutputResult CheckRecordEnd(int state)
		{
			if ((state & 16384) != 0)
			{
				this.recordState = 2;
				this.FinalizeRecord();
				this.SetEmptyFlag(state);
				return this.output.RecordDone(this);
			}
			return Processor.OutputResult.Continue;
		}

		// Token: 0x0600110C RID: 4364 RVA: 0x00051E59 File Offset: 0x00050E59
		private void SetEmptyFlag(int state)
		{
			if ((state & 1024) != 0)
			{
				this.mainNode.IsEmptyTag = false;
			}
		}

		// Token: 0x0600110D RID: 4365 RVA: 0x00051E70 File Offset: 0x00050E70
		private void AnalyzeSpaceLang()
		{
			for (int i = 0; i < this.attributeCount; i++)
			{
				BuilderInfo builderInfo = (BuilderInfo)this.attributeList[i];
				if (Keywords.Equals(builderInfo.Prefix, this.atoms.Xml))
				{
					OutputScope currentElementScope = this.scopeManager.CurrentElementScope;
					if (Keywords.Equals(builderInfo.LocalName, this.atoms.Lang))
					{
						currentElementScope.Lang = builderInfo.Value;
					}
					else if (Keywords.Equals(builderInfo.LocalName, this.atoms.Space))
					{
						currentElementScope.Space = RecordBuilder.TranslateXmlSpace(builderInfo.Value);
					}
				}
			}
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x00051F1C File Offset: 0x00050F1C
		private void FixupElement()
		{
			if (Keywords.Equals(this.mainNode.NamespaceURI, this.atoms.Empty))
			{
				this.mainNode.Prefix = this.atoms.Empty;
			}
			if (Keywords.Equals(this.mainNode.Prefix, this.atoms.Empty))
			{
				if (!Keywords.Equals(this.mainNode.NamespaceURI, this.scopeManager.DefaultNamespace))
				{
					this.DeclareNamespace(this.mainNode.NamespaceURI, this.mainNode.Prefix);
				}
			}
			else
			{
				bool flag = false;
				string text = this.scopeManager.ResolveNamespace(this.mainNode.Prefix, out flag);
				if (text != null)
				{
					if (!Keywords.Equals(this.mainNode.NamespaceURI, text))
					{
						if (flag)
						{
							this.mainNode.Prefix = this.GetPrefixForNamespace(this.mainNode.NamespaceURI);
						}
						else
						{
							this.DeclareNamespace(this.mainNode.NamespaceURI, this.mainNode.Prefix);
						}
					}
				}
				else
				{
					this.DeclareNamespace(this.mainNode.NamespaceURI, this.mainNode.Prefix);
				}
			}
			OutputScope currentElementScope = this.scopeManager.CurrentElementScope;
			currentElementScope.Prefix = this.mainNode.Prefix;
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x00052064 File Offset: 0x00051064
		private void FixupAttributes(int attributeCount)
		{
			for (int i = 0; i < attributeCount; i++)
			{
				BuilderInfo builderInfo = (BuilderInfo)this.attributeList[i];
				if (Keywords.Equals(builderInfo.NamespaceURI, this.atoms.Empty))
				{
					builderInfo.Prefix = this.atoms.Empty;
				}
				else if (Keywords.Equals(builderInfo.Prefix, this.atoms.Empty))
				{
					builderInfo.Prefix = this.GetPrefixForNamespace(builderInfo.NamespaceURI);
				}
				else
				{
					bool flag = false;
					string text = this.scopeManager.ResolveNamespace(builderInfo.Prefix, out flag);
					if (text != null)
					{
						if (!Keywords.Equals(builderInfo.NamespaceURI, text))
						{
							if (flag)
							{
								builderInfo.Prefix = this.GetPrefixForNamespace(builderInfo.NamespaceURI);
							}
							else
							{
								this.DeclareNamespace(builderInfo.NamespaceURI, builderInfo.Prefix);
							}
						}
					}
					else
					{
						this.DeclareNamespace(builderInfo.NamespaceURI, builderInfo.Prefix);
					}
				}
			}
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x00052154 File Offset: 0x00051154
		private void AppendNamespaces()
		{
			for (int i = this.namespaceCount - 1; i >= 0; i--)
			{
				BuilderInfo builderInfo = (BuilderInfo)this.attributeList[this.NewAttribute()];
				builderInfo.Initialize((BuilderInfo)this.namespaceList[i]);
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x000521A4 File Offset: 0x000511A4
		private void AnalyzeComment()
		{
			StringBuilder stringBuilder = null;
			string value = this.mainNode.Value;
			bool flag = false;
			int i = 0;
			int num = 0;
			while (i < value.Length)
			{
				char c = value[i];
				if (c == '-')
				{
					if (flag)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value, num, i, 2 * value.Length);
						}
						else
						{
							stringBuilder.Append(value, num, i - num);
						}
						stringBuilder.Append(" -");
						num = i + 1;
					}
					flag = true;
				}
				else
				{
					flag = false;
				}
				i++;
			}
			if (stringBuilder != null)
			{
				if (num < value.Length)
				{
					stringBuilder.Append(value, num, value.Length - num);
				}
				if (flag)
				{
					stringBuilder.Append(" ");
				}
				this.mainNode.Value = stringBuilder.ToString();
				return;
			}
			if (flag)
			{
				this.mainNode.ValueAppend(" ", false);
			}
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00052278 File Offset: 0x00051278
		private void AnalyzeProcessingInstruction()
		{
			StringBuilder stringBuilder = null;
			string value = this.mainNode.Value;
			bool flag = false;
			int i = 0;
			int num = 0;
			while (i < value.Length)
			{
				switch (value[i])
				{
				case '>':
					if (flag)
					{
						if (stringBuilder == null)
						{
							stringBuilder = new StringBuilder(value, num, i, 2 * value.Length);
						}
						else
						{
							stringBuilder.Append(value, num, i - num);
						}
						stringBuilder.Append(" >");
						num = i + 1;
					}
					flag = false;
					break;
				case '?':
					flag = true;
					break;
				default:
					flag = false;
					break;
				}
				i++;
			}
			if (stringBuilder != null)
			{
				if (num < value.Length)
				{
					stringBuilder.Append(value, num, value.Length - num);
				}
				this.mainNode.Value = stringBuilder.ToString();
			}
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x0005233C File Offset: 0x0005133C
		private void FinalizeRecord()
		{
			XmlNodeType nodeType = this.mainNode.NodeType;
			if (nodeType == XmlNodeType.Element)
			{
				int num = this.attributeCount;
				this.FixupElement();
				this.FixupAttributes(num);
				this.AnalyzeSpaceLang();
				this.AppendNamespaces();
				return;
			}
			switch (nodeType)
			{
			case XmlNodeType.ProcessingInstruction:
				this.AnalyzeProcessingInstruction();
				return;
			case XmlNodeType.Comment:
				this.AnalyzeComment();
				return;
			default:
				return;
			}
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x00052398 File Offset: 0x00051398
		private int NewNamespace()
		{
			if (this.namespaceCount >= this.namespaceList.Count)
			{
				this.namespaceList.Add(new BuilderInfo());
			}
			return this.namespaceCount++;
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x000523DC File Offset: 0x000513DC
		private void DeclareNamespace(string nspace, string prefix)
		{
			int num = this.NewNamespace();
			BuilderInfo builderInfo = (BuilderInfo)this.namespaceList[num];
			if (prefix == this.atoms.Empty)
			{
				builderInfo.Initialize(this.atoms.Empty, this.atoms.Xmlns, this.atoms.XmlnsNamespace);
			}
			else
			{
				builderInfo.Initialize(this.atoms.Xmlns, prefix, this.atoms.XmlnsNamespace);
			}
			builderInfo.Depth = this.recordDepth;
			builderInfo.NodeType = XmlNodeType.Attribute;
			builderInfo.Value = nspace;
			this.scopeManager.PushNamespace(prefix, nspace);
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x00052484 File Offset: 0x00051484
		private string DeclareNewNamespace(string nspace)
		{
			string text = this.scopeManager.GeneratePrefix("xp_{0}");
			this.DeclareNamespace(nspace, text);
			return text;
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x000524AC File Offset: 0x000514AC
		internal string GetPrefixForNamespace(string nspace)
		{
			string text = null;
			if (this.scopeManager.FindPrefix(nspace, out text))
			{
				return text;
			}
			return this.DeclareNewNamespace(nspace);
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x000524D4 File Offset: 0x000514D4
		private static XmlSpace TranslateXmlSpace(string space)
		{
			if (Keywords.Compare(space, "default"))
			{
				return XmlSpace.Default;
			}
			if (Keywords.Compare(space, "preserve"))
			{
				return XmlSpace.Preserve;
			}
			return XmlSpace.None;
		}

		// Token: 0x04000B37 RID: 2871
		private const int NoRecord = 0;

		// Token: 0x04000B38 RID: 2872
		private const int SomeRecord = 1;

		// Token: 0x04000B39 RID: 2873
		private const int HaveRecord = 2;

		// Token: 0x04000B3A RID: 2874
		private const char s_Minus = '-';

		// Token: 0x04000B3B RID: 2875
		private const string s_Space = " ";

		// Token: 0x04000B3C RID: 2876
		private const string s_SpaceMinus = " -";

		// Token: 0x04000B3D RID: 2877
		private const char s_Question = '?';

		// Token: 0x04000B3E RID: 2878
		private const char s_Greater = '>';

		// Token: 0x04000B3F RID: 2879
		private const string s_SpaceGreater = " >";

		// Token: 0x04000B40 RID: 2880
		private const string PrefixFormat = "xp_{0}";

		// Token: 0x04000B41 RID: 2881
		private const string s_SpaceDefault = "default";

		// Token: 0x04000B42 RID: 2882
		private const string s_SpacePreserve = "preserve";

		// Token: 0x04000B43 RID: 2883
		private int outputState;

		// Token: 0x04000B44 RID: 2884
		private RecordBuilder next;

		// Token: 0x04000B45 RID: 2885
		private RecordOutput output;

		// Token: 0x04000B46 RID: 2886
		private XmlNameTable nameTable;

		// Token: 0x04000B47 RID: 2887
		private OutKeywords atoms;

		// Token: 0x04000B48 RID: 2888
		private OutputScopeManager scopeManager;

		// Token: 0x04000B49 RID: 2889
		private BuilderInfo mainNode = new BuilderInfo();

		// Token: 0x04000B4A RID: 2890
		private ArrayList attributeList = new ArrayList();

		// Token: 0x04000B4B RID: 2891
		private int attributeCount;

		// Token: 0x04000B4C RID: 2892
		private ArrayList namespaceList = new ArrayList();

		// Token: 0x04000B4D RID: 2893
		private int namespaceCount;

		// Token: 0x04000B4E RID: 2894
		private BuilderInfo dummy = new BuilderInfo();

		// Token: 0x04000B4F RID: 2895
		private BuilderInfo currentInfo;

		// Token: 0x04000B50 RID: 2896
		private bool popScope;

		// Token: 0x04000B51 RID: 2897
		private int recordState;

		// Token: 0x04000B52 RID: 2898
		private int recordDepth;
	}
}
