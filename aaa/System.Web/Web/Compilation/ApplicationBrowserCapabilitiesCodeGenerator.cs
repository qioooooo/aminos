using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web.Configuration;
using System.Web.UI;
using System.Xml;

namespace System.Web.Compilation
{
	// Token: 0x02000130 RID: 304
	internal class ApplicationBrowserCapabilitiesCodeGenerator : BrowserCapabilitiesCodeGenerator
	{
		// Token: 0x06000E09 RID: 3593 RVA: 0x0003F438 File Offset: 0x0003E438
		internal ApplicationBrowserCapabilitiesCodeGenerator(BuildProvider buildProvider)
		{
			this._browserOverrides = new OrderedDictionary();
			this._defaultBrowserOverrides = new OrderedDictionary();
			this._buildProvider = buildProvider;
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000E0A RID: 3594 RVA: 0x0003F45D File Offset: 0x0003E45D
		internal override bool GenerateOverrides
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000E0B RID: 3595 RVA: 0x0003F460 File Offset: 0x0003E460
		internal override string TypeName
		{
			get
			{
				return "ApplicationBrowserCapabilitiesFactory";
			}
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x0003F467 File Offset: 0x0003E467
		public override void Create()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x0003F470 File Offset: 0x0003E470
		private static void AddStringToHashtable(OrderedDictionary table, object key, string content, bool before)
		{
			ArrayList arrayList = (ArrayList)table[key];
			if (arrayList == null)
			{
				arrayList = new ArrayList(1);
				table[key] = arrayList;
			}
			if (before)
			{
				arrayList.Insert(0, content);
				return;
			}
			arrayList.Add(content);
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x0003F4B0 File Offset: 0x0003E4B0
		private static string GetFirstItemFromKey(OrderedDictionary table, object key)
		{
			ArrayList arrayList = (ArrayList)table[key];
			if (arrayList != null && arrayList.Count > 0)
			{
				return arrayList[0] as string;
			}
			return null;
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x0003F4E4 File Offset: 0x0003E4E4
		internal override void HandleUnRecognizedParentElement(BrowserDefinition bd, bool isDefault)
		{
			string parentName = bd.ParentName;
			int num = bd.GetType().GetHashCode() ^ parentName.GetHashCode();
			if (isDefault)
			{
				ApplicationBrowserCapabilitiesCodeGenerator.AddStringToHashtable(this._defaultBrowserOverrides, num, bd.Name, bd.IsRefID);
				return;
			}
			ApplicationBrowserCapabilitiesCodeGenerator.AddStringToHashtable(this._browserOverrides, num, bd.Name, bd.IsRefID);
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x0003F54C File Offset: 0x0003E54C
		internal void GenerateCode(AssemblyBuilder assemblyBuilder)
		{
			base.ProcessBrowserFiles(true, BrowserCapabilitiesCompiler.AppBrowsersVirtualDir.VirtualPathString);
			base.ProcessCustomBrowserFiles(true, BrowserCapabilitiesCompiler.AppBrowsersVirtualDir.VirtualPathString);
			CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < base.CustomTreeNames.Count; i++)
			{
				arrayList.Add((BrowserDefinition)((BrowserTree)base.CustomTreeList[i])[base.CustomTreeNames[i]]);
			}
			CodeNamespace codeNamespace = new CodeNamespace("ASP");
			codeNamespace.Imports.Add(new CodeNamespaceImport("System"));
			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Web"));
			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Web.Configuration"));
			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Reflection"));
			codeCompileUnit.Namespaces.Add(codeNamespace);
			Type browserCapabilitiesFactoryBaseType = BrowserCapabilitiesCompiler.GetBrowserCapabilitiesFactoryBaseType();
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration();
			codeTypeDeclaration.Attributes = MemberAttributes.Private;
			codeTypeDeclaration.IsClass = true;
			codeTypeDeclaration.Name = this.TypeName;
			codeTypeDeclaration.BaseTypes.Add(new CodeTypeReference(browserCapabilitiesFactoryBaseType));
			codeNamespace.Types.Add(codeTypeDeclaration);
			BindingFlags bindingFlags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic;
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			codeMemberMethod.Attributes = (MemberAttributes)24580;
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
			codeMemberMethod.Name = "ConfigureCustomCapabilities";
			CodeParameterDeclarationExpression codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(NameValueCollection), "headers");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(HttpBrowserCapabilities), "browserCaps");
			codeMemberMethod.Parameters.Add(codeParameterDeclarationExpression);
			codeTypeDeclaration.Members.Add(codeMemberMethod);
			for (int j = 0; j < arrayList.Count; j++)
			{
				base.GenerateSingleProcessCall((BrowserDefinition)arrayList[j], codeMemberMethod);
			}
			foreach (object obj in this._browserOverrides)
			{
				object key = ((DictionaryEntry)obj).Key;
				BrowserDefinition browserDefinition = (BrowserDefinition)base.BrowserTree[ApplicationBrowserCapabilitiesCodeGenerator.GetFirstItemFromKey(this._browserOverrides, key)];
				string parentName = browserDefinition.ParentName;
				if (browserCapabilitiesFactoryBaseType.GetMethod(parentName + "ProcessBrowsers", bindingFlags) == null || browserCapabilitiesFactoryBaseType.GetMethod(parentName + "ProcessGateways", bindingFlags) == null)
				{
					string parentID = browserDefinition.ParentID;
					if (browserDefinition != null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Browser_parentID_Not_Found", new object[] { parentID }), browserDefinition.XmlNode);
					}
					throw new ConfigurationErrorsException(SR.GetString("Browser_parentID_Not_Found", new object[] { parentID }));
				}
				else
				{
					bool flag = true;
					if (browserDefinition is GatewayDefinition)
					{
						flag = false;
					}
					string text = parentName + (flag ? "ProcessBrowsers" : "ProcessGateways");
					CodeMemberMethod codeMemberMethod2 = new CodeMemberMethod();
					codeMemberMethod2.Name = text;
					codeMemberMethod2.ReturnType = new CodeTypeReference(typeof(void));
					codeMemberMethod2.Attributes = (MemberAttributes)12292;
					if (flag)
					{
						codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(bool), "ignoreApplicationBrowsers");
						codeMemberMethod2.Parameters.Add(codeParameterDeclarationExpression);
					}
					codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(NameValueCollection), "headers");
					codeMemberMethod2.Parameters.Add(codeParameterDeclarationExpression);
					codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(HttpBrowserCapabilities), "browserCaps");
					codeMemberMethod2.Parameters.Add(codeParameterDeclarationExpression);
					codeTypeDeclaration.Members.Add(codeMemberMethod2);
					ArrayList arrayList2 = (ArrayList)this._browserOverrides[key];
					CodeStatementCollection codeStatementCollection = codeMemberMethod2.Statements;
					bool flag2 = false;
					foreach (object obj2 in arrayList2)
					{
						string text2 = (string)obj2;
						BrowserDefinition browserDefinition2 = (BrowserDefinition)base.BrowserTree[text2];
						if (browserDefinition2 is GatewayDefinition || browserDefinition2.IsRefID)
						{
							base.GenerateSingleProcessCall(browserDefinition2, codeMemberMethod2);
						}
						else
						{
							if (!flag2)
							{
								CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
								codeConditionStatement.Condition = new CodeVariableReferenceExpression("ignoreApplicationBrowsers");
								codeMemberMethod2.Statements.Add(codeConditionStatement);
								codeStatementCollection = codeConditionStatement.FalseStatements;
								flag2 = true;
							}
							codeStatementCollection = base.GenerateTrackedSingleProcessCall(codeStatementCollection, browserDefinition2, codeMemberMethod2);
							if (this._baseInstance == null)
							{
								this._baseInstance = (BrowserCapabilitiesFactoryBase)Activator.CreateInstance(browserCapabilitiesFactoryBaseType);
							}
							int num = (int)((Triplet)this._baseInstance.InternalGetBrowserElements()[parentName]).Third;
							base.AddBrowserToCollectionRecursive(browserDefinition2, num + 1);
						}
					}
				}
			}
			foreach (object obj3 in this._defaultBrowserOverrides)
			{
				object key2 = ((DictionaryEntry)obj3).Key;
				BrowserDefinition browserDefinition3 = (BrowserDefinition)base.DefaultTree[ApplicationBrowserCapabilitiesCodeGenerator.GetFirstItemFromKey(this._defaultBrowserOverrides, key2)];
				string parentName2 = browserDefinition3.ParentName;
				if (browserCapabilitiesFactoryBaseType.GetMethod("Default" + parentName2 + "ProcessBrowsers", bindingFlags) == null)
				{
					string parentID2 = browserDefinition3.ParentID;
					if (browserDefinition3 != null)
					{
						throw new ConfigurationErrorsException(SR.GetString("DefaultBrowser_parentID_Not_Found", new object[] { parentID2 }), browserDefinition3.XmlNode);
					}
				}
				string text3 = "Default" + parentName2 + "ProcessBrowsers";
				CodeMemberMethod codeMemberMethod3 = new CodeMemberMethod();
				codeMemberMethod3.Name = text3;
				codeMemberMethod3.ReturnType = new CodeTypeReference(typeof(void));
				codeMemberMethod3.Attributes = (MemberAttributes)12292;
				codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(bool), "ignoreApplicationBrowsers");
				codeMemberMethod3.Parameters.Add(codeParameterDeclarationExpression);
				codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(NameValueCollection), "headers");
				codeMemberMethod3.Parameters.Add(codeParameterDeclarationExpression);
				codeParameterDeclarationExpression = new CodeParameterDeclarationExpression(typeof(HttpBrowserCapabilities), "browserCaps");
				codeMemberMethod3.Parameters.Add(codeParameterDeclarationExpression);
				codeTypeDeclaration.Members.Add(codeMemberMethod3);
				ArrayList arrayList3 = (ArrayList)this._defaultBrowserOverrides[key2];
				CodeConditionStatement codeConditionStatement2 = new CodeConditionStatement();
				codeConditionStatement2.Condition = new CodeVariableReferenceExpression("ignoreApplicationBrowsers");
				codeMemberMethod3.Statements.Add(codeConditionStatement2);
				CodeStatementCollection codeStatementCollection2 = codeConditionStatement2.FalseStatements;
				foreach (object obj4 in arrayList3)
				{
					string text4 = (string)obj4;
					BrowserDefinition browserDefinition2 = (BrowserDefinition)base.DefaultTree[text4];
					if (browserDefinition2.IsRefID)
					{
						base.GenerateSingleProcessCall(browserDefinition2, codeMemberMethod3, "Default");
					}
					else
					{
						codeStatementCollection2 = base.GenerateTrackedSingleProcessCall(codeStatementCollection2, browserDefinition2, codeMemberMethod3, "Default");
					}
				}
			}
			foreach (object obj5 in base.BrowserTree)
			{
				BrowserDefinition browserDefinition2 = ((DictionaryEntry)obj5).Value as BrowserDefinition;
				base.GenerateProcessMethod(browserDefinition2, codeTypeDeclaration);
			}
			for (int k = 0; k < arrayList.Count; k++)
			{
				foreach (object obj6 in ((BrowserTree)base.CustomTreeList[k]))
				{
					BrowserDefinition browserDefinition2 = ((DictionaryEntry)obj6).Value as BrowserDefinition;
					base.GenerateProcessMethod(browserDefinition2, codeTypeDeclaration);
				}
			}
			foreach (object obj7 in base.DefaultTree)
			{
				BrowserDefinition browserDefinition2 = ((DictionaryEntry)obj7).Value as BrowserDefinition;
				base.GenerateProcessMethod(browserDefinition2, codeTypeDeclaration, "Default");
			}
			base.GenerateOverrideMatchedHeaders(codeTypeDeclaration);
			base.GenerateOverrideBrowserElements(codeTypeDeclaration);
			Assembly assembly = BrowserCapabilitiesCompiler.GetBrowserCapabilitiesFactoryBaseType().Assembly;
			assemblyBuilder.AddAssemblyReference(assembly, codeCompileUnit);
			assemblyBuilder.AddCodeCompileUnit(this._buildProvider, codeCompileUnit);
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x0003FE84 File Offset: 0x0003EE84
		internal override void ProcessBrowserNode(XmlNode node, BrowserTree browserTree)
		{
			if (node.Name == "defaultBrowser")
			{
				throw new ConfigurationErrorsException(SR.GetString("Browser_Not_Allowed_InAppLevel", new object[] { node.Name }), node);
			}
			base.ProcessBrowserNode(node, browserTree);
		}

		// Token: 0x04001557 RID: 5463
		internal const string FactoryTypeName = "ApplicationBrowserCapabilitiesFactory";

		// Token: 0x04001558 RID: 5464
		private OrderedDictionary _browserOverrides;

		// Token: 0x04001559 RID: 5465
		private OrderedDictionary _defaultBrowserOverrides;

		// Token: 0x0400155A RID: 5466
		private BrowserCapabilitiesFactoryBase _baseInstance;

		// Token: 0x0400155B RID: 5467
		private BuildProvider _buildProvider;
	}
}
