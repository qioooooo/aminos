using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.Adapters;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x020001AA RID: 426
	internal class BrowserDefinition
	{
		// Token: 0x060018BA RID: 6330 RVA: 0x00076AF0 File Offset: 0x00075AF0
		internal static string MakeValidTypeNameFromString(string s)
		{
			if (s == null)
			{
				return s;
			}
			s = s.ToLower(CultureInfo.InvariantCulture);
			StringBuilder stringBuilder = new StringBuilder();
			int i = 0;
			while (i < s.Length)
			{
				if (i != 0)
				{
					goto IL_0064;
				}
				if (char.IsDigit(s[0]))
				{
					stringBuilder.Append("N");
					goto IL_0064;
				}
				if (!char.IsLetter(s[0]))
				{
					goto IL_0064;
				}
				stringBuilder.Append(s.Substring(0, 1).ToUpper(CultureInfo.InvariantCulture));
				IL_0096:
				i++;
				continue;
				IL_0064:
				if (char.IsLetterOrDigit(s[i]) || s[i] == '_')
				{
					stringBuilder.Append(s[i]);
					goto IL_0096;
				}
				stringBuilder.Append('A');
				goto IL_0096;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060018BB RID: 6331 RVA: 0x00076BA9 File Offset: 0x00075BA9
		internal BrowserDefinition(XmlNode node)
			: this(node, false)
		{
		}

		// Token: 0x060018BC RID: 6332 RVA: 0x00076BB4 File Offset: 0x00075BB4
		internal BrowserDefinition(XmlNode node, bool isDefaultBrowser)
		{
			if (node == null)
			{
				throw new ArgumentNullException("node");
			}
			this._capabilities = new NameValueCollection();
			this._idHeaderChecks = new ArrayList();
			this._idCapabilityChecks = new ArrayList();
			this._captureHeaderChecks = new ArrayList();
			this._captureCapabilityChecks = new ArrayList();
			this._adapters = new AdapterDictionary();
			this._browsers = new BrowserDefinitionCollection();
			this._gateways = new BrowserDefinitionCollection();
			this._refBrowsers = new BrowserDefinitionCollection();
			this._refGateways = new BrowserDefinitionCollection();
			this._node = node;
			this._isDefaultBrowser = isDefaultBrowser;
			string text = null;
			HandlerBase.GetAndRemoveNonEmptyStringAttribute(node, "id", ref this._id);
			HandlerBase.GetAndRemoveNonEmptyStringAttribute(node, "refID", ref text);
			if (text != null && this._id != null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Browser_mutually_exclusive_attributes", new object[] { "id", "refID" }), node);
			}
			if (this._id != null)
			{
				if (!CodeGenerator.IsValidLanguageIndependentIdentifier(this._id))
				{
					throw new ConfigurationErrorsException(SR.GetString("Browser_InvalidID", new object[] { "id", this._id }), node);
				}
			}
			else if (text == null)
			{
				if (this is GatewayDefinition)
				{
					throw new ConfigurationErrorsException(SR.GetString("Browser_attributes_required", new object[] { "gateway", "refID", "id" }), node);
				}
				throw new ConfigurationErrorsException(SR.GetString("Browser_attributes_required", new object[] { "browser", "refID", "id" }), node);
			}
			else
			{
				if (!CodeGenerator.IsValidLanguageIndependentIdentifier(text))
				{
					throw new ConfigurationErrorsException(SR.GetString("Browser_InvalidID", new object[] { "refID", text }), node);
				}
				this._parentID = text;
				this._isRefID = true;
				this._id = text;
				if (this is GatewayDefinition)
				{
					this._name = "refgatewayid$";
				}
				else
				{
					this._name = "refbrowserid$";
				}
				string text2 = null;
				HandlerBase.GetAndRemoveNonEmptyStringAttribute(node, "parentID", ref text2);
				if (text2 != null && text2.Length != 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("Browser_mutually_exclusive_attributes", new object[] { "parentID", "refID" }), node);
				}
			}
			this._name = BrowserDefinition.MakeValidTypeNameFromString(this._id + this._name);
			if (!this._isRefID)
			{
				if (!"Default".Equals(this._id))
				{
					HandlerBase.GetAndRemoveNonEmptyStringAttribute(node, "parentID", ref this._parentID);
				}
				else
				{
					HandlerBase.GetAndRemoveNonEmptyStringAttribute(node, "parentID", ref this._parentID);
					if (this._parentID != null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Browser_parentID_applied_to_default"), node);
					}
				}
			}
			this._parentName = BrowserDefinition.MakeValidTypeNameFromString(this._parentID);
			if (this._id.IndexOf(" ", StringComparison.Ordinal) != -1)
			{
				throw new ConfigurationErrorsException(SR.GetString("Space_attribute", new object[] { "id " + this._id }), node);
			}
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					string name;
					if ((name = xmlNode.Name) != null)
					{
						if (!(name == "identification"))
						{
							if (name == "capture")
							{
								this.ProcessCaptureNode(xmlNode, BrowserCapsElementType.Capture);
								continue;
							}
							if (name == "capabilities")
							{
								this.ProcessCapabilitiesNode(xmlNode);
								continue;
							}
							if (name == "controlAdapters")
							{
								this.ProcessControlAdaptersNode(xmlNode);
								continue;
							}
							if (name == "sampleHeaders")
							{
								continue;
							}
						}
						else
						{
							if (this._isRefID)
							{
								throw new ConfigurationErrorsException(SR.GetString("Browser_refid_prohibits_identification"), node);
							}
							this.ProcessIdentificationNode(xmlNode, BrowserCapsElementType.Identification);
							continue;
						}
					}
					throw new ConfigurationErrorsException(SR.GetString("Browser_invalid_element", new object[] { xmlNode.Name }), node);
				}
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060018BD RID: 6333 RVA: 0x00076FF4 File Offset: 0x00075FF4
		public bool IsDefaultBrowser
		{
			get
			{
				return this._isDefaultBrowser;
			}
		}

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060018BE RID: 6334 RVA: 0x00076FFC File Offset: 0x00075FFC
		public BrowserDefinitionCollection Browsers
		{
			get
			{
				return this._browsers;
			}
		}

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x00077004 File Offset: 0x00076004
		public BrowserDefinitionCollection RefBrowsers
		{
			get
			{
				return this._refBrowsers;
			}
		}

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x0007700C File Offset: 0x0007600C
		public BrowserDefinitionCollection RefGateways
		{
			get
			{
				return this._refGateways;
			}
		}

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x060018C1 RID: 6337 RVA: 0x00077014 File Offset: 0x00076014
		public BrowserDefinitionCollection Gateways
		{
			get
			{
				return this._gateways;
			}
		}

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x060018C2 RID: 6338 RVA: 0x0007701C File Offset: 0x0007601C
		public string ID
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x00077024 File Offset: 0x00076024
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x0007702C File Offset: 0x0007602C
		public string ParentName
		{
			get
			{
				return this._parentName;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060018C5 RID: 6341 RVA: 0x00077034 File Offset: 0x00076034
		// (set) Token: 0x060018C6 RID: 6342 RVA: 0x0007703C File Offset: 0x0007603C
		internal bool IsDeviceNode
		{
			get
			{
				return this._isDeviceNode;
			}
			set
			{
				this._isDeviceNode = value;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x060018C7 RID: 6343 RVA: 0x00077045 File Offset: 0x00076045
		// (set) Token: 0x060018C8 RID: 6344 RVA: 0x0007704D File Offset: 0x0007604D
		internal int Depth
		{
			get
			{
				return this._depth;
			}
			set
			{
				this._depth = value;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x060018C9 RID: 6345 RVA: 0x00077056 File Offset: 0x00076056
		public string ParentID
		{
			get
			{
				return this._parentID;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x060018CA RID: 6346 RVA: 0x0007705E File Offset: 0x0007605E
		internal bool IsRefID
		{
			get
			{
				return this._isRefID;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x060018CB RID: 6347 RVA: 0x00077066 File Offset: 0x00076066
		public NameValueCollection Capabilities
		{
			get
			{
				return this._capabilities;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x060018CC RID: 6348 RVA: 0x0007706E File Offset: 0x0007606E
		public ArrayList IdHeaderChecks
		{
			get
			{
				return this._idHeaderChecks;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060018CD RID: 6349 RVA: 0x00077076 File Offset: 0x00076076
		public ArrayList CaptureHeaderChecks
		{
			get
			{
				return this._captureHeaderChecks;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x060018CE RID: 6350 RVA: 0x0007707E File Offset: 0x0007607E
		public ArrayList IdCapabilityChecks
		{
			get
			{
				return this._idCapabilityChecks;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x060018CF RID: 6351 RVA: 0x00077086 File Offset: 0x00076086
		public ArrayList CaptureCapabilityChecks
		{
			get
			{
				return this._captureCapabilityChecks;
			}
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x060018D0 RID: 6352 RVA: 0x0007708E File Offset: 0x0007608E
		public AdapterDictionary Adapters
		{
			get
			{
				return this._adapters;
			}
		}

		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x060018D1 RID: 6353 RVA: 0x00077096 File Offset: 0x00076096
		internal XmlNode XmlNode
		{
			get
			{
				return this._node;
			}
		}

		// Token: 0x17000469 RID: 1129
		// (get) Token: 0x060018D2 RID: 6354 RVA: 0x0007709E File Offset: 0x0007609E
		public string HtmlTextWriterString
		{
			get
			{
				return this._htmlTextWriterString;
			}
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x000770A8 File Offset: 0x000760A8
		private void DisallowNonMatchAttribute(XmlNode node)
		{
			string text = null;
			HandlerBase.GetAndRemoveStringAttribute(node, "nonMatch", ref text);
			if (text != null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Browser_mutually_exclusive_attributes", new object[] { "match", "nonMatch" }), node);
			}
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x000770F4 File Offset: 0x000760F4
		private void HandleMissingMatchAndNonMatchError(XmlNode node)
		{
			throw new ConfigurationErrorsException(SR.GetString("Missing_required_attributes", new object[] { "match", "nonMatch", node.Name }), node);
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00077134 File Offset: 0x00076134
		internal void ProcessIdentificationNode(XmlNode node, BrowserCapsElementType elementType)
		{
			string text = null;
			string text2 = null;
			bool flag = true;
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				text = string.Empty;
				bool flag2 = false;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					string name;
					if ((name = xmlNode.Name) != null)
					{
						if (!(name == "userAgent"))
						{
							if (!(name == "header"))
							{
								if (name == "capability")
								{
									flag = false;
									HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "name", ref text2);
									HandlerBase.GetAndRemoveNonEmptyStringAttribute(xmlNode, "match", ref text);
									if (string.IsNullOrEmpty(text))
									{
										HandlerBase.GetAndRemoveNonEmptyStringAttribute(xmlNode, "nonMatch", ref text);
										if (string.IsNullOrEmpty(text))
										{
											this.HandleMissingMatchAndNonMatchError(xmlNode);
										}
										flag2 = true;
									}
									this._idCapabilityChecks.Add(new CheckPair(text2, text, flag2));
									if (!flag2)
									{
										this.DisallowNonMatchAttribute(xmlNode);
										continue;
									}
									continue;
								}
							}
							else
							{
								flag = false;
								HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "name", ref text2);
								HandlerBase.GetAndRemoveNonEmptyStringAttribute(xmlNode, "match", ref text);
								if (string.IsNullOrEmpty(text))
								{
									HandlerBase.GetAndRemoveNonEmptyStringAttribute(xmlNode, "nonMatch", ref text);
									if (string.IsNullOrEmpty(text))
									{
										this.HandleMissingMatchAndNonMatchError(xmlNode);
									}
									flag2 = true;
								}
								this._idHeaderChecks.Add(new CheckPair(text2, text, flag2));
								if (!flag2)
								{
									this.DisallowNonMatchAttribute(xmlNode);
									continue;
								}
								continue;
							}
						}
						else
						{
							flag = false;
							HandlerBase.GetAndRemoveNonEmptyStringAttribute(xmlNode, "match", ref text);
							if (string.IsNullOrEmpty(text))
							{
								HandlerBase.GetAndRemoveNonEmptyStringAttribute(xmlNode, "nonMatch", ref text);
								if (string.IsNullOrEmpty(text))
								{
									this.HandleMissingMatchAndNonMatchError(xmlNode);
								}
								flag2 = true;
							}
							this._idHeaderChecks.Add(new CheckPair("User-Agent", text, flag2));
							if (!flag2)
							{
								this.DisallowNonMatchAttribute(xmlNode);
								continue;
							}
							continue;
						}
					}
					throw new ConfigurationErrorsException(SR.GetString("Config_invalid_element", new object[] { xmlNode.ToString() }), xmlNode);
				}
			}
			if (flag)
			{
				throw new ConfigurationErrorsException(SR.GetString("Browser_empty_identification"), node);
			}
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x00077378 File Offset: 0x00076378
		internal void ProcessCaptureNode(XmlNode node, BrowserCapsElementType elementType)
		{
			string text = null;
			string text2 = null;
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					string name;
					if ((name = xmlNode.Name) != null)
					{
						if (name == "userAgent")
						{
							HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "match", ref text);
							this._captureHeaderChecks.Add(new CheckPair("User-Agent", text));
							continue;
						}
						if (name == "header")
						{
							HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "name", ref text2);
							HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "match", ref text);
							this._captureHeaderChecks.Add(new CheckPair(text2, text));
							continue;
						}
						if (name == "capability")
						{
							HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "name", ref text2);
							HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "match", ref text);
							this._captureCapabilityChecks.Add(new CheckPair(text2, text));
							continue;
						}
					}
					throw new ConfigurationErrorsException(SR.GetString("Config_invalid_element", new object[] { xmlNode.ToString() }), xmlNode);
				}
			}
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x000774E0 File Offset: 0x000764E0
		internal void ProcessCapabilitiesNode(XmlNode node)
		{
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (xmlNode.Name != "capability")
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), xmlNode);
					}
					string text = null;
					string text2 = null;
					HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "name", ref text);
					HandlerBase.GetAndRemoveRequiredStringAttribute(xmlNode, "value", ref text2);
					this._capabilities[text] = text2;
				}
			}
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x00077590 File Offset: 0x00076590
		internal void ProcessControlAdaptersNode(XmlNode node)
		{
			HandlerBase.GetAndRemoveStringAttribute(node, "markupTextWriterType", ref this._htmlTextWriterString);
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (xmlNode.Name != "adapter")
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_element"), xmlNode);
					}
					XmlAttributeCollection attributes = xmlNode.Attributes;
					string text = null;
					string text2 = null;
					HandlerBase.GetAndRemoveRequiredNonEmptyStringAttribute(xmlNode, "controlType", ref text);
					HandlerBase.GetAndRemoveRequiredStringAttribute(xmlNode, "adapterType", ref text2);
					Type type = BrowserDefinition.CheckType(text, typeof(Control), xmlNode);
					text = type.AssemblyQualifiedName;
					if (!string.IsNullOrEmpty(text2))
					{
						BrowserDefinition.CheckType(text2, typeof(ControlAdapter), xmlNode);
					}
					this._adapters[text] = text2;
				}
			}
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00077698 File Offset: 0x00076698
		private static Type CheckType(string typeName, Type baseType, XmlNode child)
		{
			Type type = ConfigUtil.GetType(typeName, child, true);
			if (!baseType.IsAssignableFrom(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_doesnt_inherit_from_type", new object[] { typeName, baseType.FullName }), child);
			}
			if (!HttpRuntime.IsTypeAllowedInConfig(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_from_untrusted_assembly", new object[] { typeName }), child);
			}
			return type;
		}

		// Token: 0x060018DA RID: 6362 RVA: 0x00077704 File Offset: 0x00076704
		internal void MergeWithDefinition(BrowserDefinition definition)
		{
			foreach (object obj in definition.Capabilities.Keys)
			{
				string text = (string)obj;
				this._capabilities[text] = definition.Capabilities[text];
			}
			foreach (object obj2 in definition.Adapters.Keys)
			{
				string text2 = (string)obj2;
				this._adapters[text2] = definition.Adapters[text2];
			}
			this._htmlTextWriterString = definition.HtmlTextWriterString;
		}

		// Token: 0x040016DC RID: 5852
		private ArrayList _idHeaderChecks;

		// Token: 0x040016DD RID: 5853
		private ArrayList _idCapabilityChecks;

		// Token: 0x040016DE RID: 5854
		private ArrayList _captureHeaderChecks;

		// Token: 0x040016DF RID: 5855
		private ArrayList _captureCapabilityChecks;

		// Token: 0x040016E0 RID: 5856
		private AdapterDictionary _adapters;

		// Token: 0x040016E1 RID: 5857
		private string _id;

		// Token: 0x040016E2 RID: 5858
		private string _parentID;

		// Token: 0x040016E3 RID: 5859
		private string _name;

		// Token: 0x040016E4 RID: 5860
		private string _parentName;

		// Token: 0x040016E5 RID: 5861
		private NameValueCollection _capabilities;

		// Token: 0x040016E6 RID: 5862
		private BrowserDefinitionCollection _browsers;

		// Token: 0x040016E7 RID: 5863
		private BrowserDefinitionCollection _gateways;

		// Token: 0x040016E8 RID: 5864
		private BrowserDefinitionCollection _refBrowsers;

		// Token: 0x040016E9 RID: 5865
		private BrowserDefinitionCollection _refGateways;

		// Token: 0x040016EA RID: 5866
		private XmlNode _node;

		// Token: 0x040016EB RID: 5867
		private bool _isRefID;

		// Token: 0x040016EC RID: 5868
		private bool _isDeviceNode;

		// Token: 0x040016ED RID: 5869
		private bool _isDefaultBrowser;

		// Token: 0x040016EE RID: 5870
		private string _htmlTextWriterString;

		// Token: 0x040016EF RID: 5871
		private int _depth;
	}
}
