using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Profile;
using System.Web.UI;
using System.Web.Util;
using Microsoft.VisualBasic;

namespace System.Web.Compilation
{
	// Token: 0x02000126 RID: 294
	internal abstract class BaseCodeDomTreeGenerator
	{
		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000D4D RID: 3405 RVA: 0x000369FA File Offset: 0x000359FA
		private TemplateParser Parser
		{
			get
			{
				return this._parser;
			}
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00036A02 File Offset: 0x00035A02
		internal void SetDesignerMode()
		{
			this._designerMode = true;
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000D4F RID: 3407 RVA: 0x00036A0B File Offset: 0x00035A0B
		internal IDictionary LinePragmasTable
		{
			get
			{
				return this._linePragmasTable;
			}
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x00036A14 File Offset: 0x00035A14
		static BaseCodeDomTreeGenerator()
		{
			CompilationSection compilation = RuntimeConfig.GetAppConfig().Compilation;
			BaseCodeDomTreeGenerator._urlLinePragmas = compilation.UrlLinePragmas;
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00036A37 File Offset: 0x00035A37
		internal CodeCompileUnit GetCodeDomTree(CodeDomProvider codeDomProvider, StringResourceBuilder stringResourceBuilder, VirtualPath virtualPath)
		{
			this._codeDomProvider = codeDomProvider;
			this._stringResourceBuilder = stringResourceBuilder;
			this._virtualPath = virtualPath;
			if (!this.BuildSourceDataTree())
			{
				return null;
			}
			return this._codeCompileUnit;
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000D52 RID: 3410 RVA: 0x00036A5E File Offset: 0x00035A5E
		protected CompilerParameters CompilParams
		{
			get
			{
				return this._compilParams;
			}
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00036A66 File Offset: 0x00035A66
		internal string GetInstantiatableFullTypeName()
		{
			if (this.PrecompilingForUpdatableDeployment)
			{
				return null;
			}
			return Util.MakeFullTypeName(this._sourceDataNamespace.Name, this._sourceDataClass.Name);
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00036A8D File Offset: 0x00035A8D
		internal string GetIntermediateFullTypeName()
		{
			return Util.MakeFullTypeName(this.Parser.BaseTypeNamespace, this._intermediateClass.Name);
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x00036AAA File Offset: 0x00035AAA
		protected BaseCodeDomTreeGenerator(TemplateParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x00036AC0 File Offset: 0x00035AC0
		protected void ApplyEditorBrowsableCustomAttribute(CodeTypeMember member)
		{
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration();
			codeAttributeDeclaration.Name = typeof(EditorBrowsableAttribute).FullName;
			codeAttributeDeclaration.Arguments.Add(new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(EditorBrowsableState)), "Never")));
			member.CustomAttributes.Add(codeAttributeDeclaration);
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x00036B20 File Offset: 0x00035B20
		protected virtual string GetGeneratedClassName()
		{
			if (this.Parser.GeneratedClassName != null)
			{
				return this.Parser.GeneratedClassName;
			}
			string text = this._virtualPath.FileName;
			string appRelativeVirtualPathStringOrNull = this._virtualPath.Parent.AppRelativeVirtualPathStringOrNull;
			if (appRelativeVirtualPathStringOrNull != null)
			{
				text = appRelativeVirtualPathStringOrNull.Substring(2) + text;
			}
			text = Util.MakeValidTypeNameFromString(text);
			text = text.ToLowerInvariant();
			string text2 = ((this.Parser.BaseTypeName != null) ? this.Parser.BaseTypeName : this.Parser.BaseType.Name);
			if (StringUtil.EqualsIgnoreCase(text, text2))
			{
				text = "_" + text;
			}
			return text;
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x00036BC3 File Offset: 0x00035BC3
		internal static bool IsAspNetNamespace(string ns)
		{
			return ns == "ASP";
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000D59 RID: 3417 RVA: 0x00036BD0 File Offset: 0x00035BD0
		private bool PrecompilingForUpdatableDeployment
		{
			get
			{
				return !this.IsGlobalAsaxGenerator && BuildManager.PrecompilingForUpdatableDeployment;
			}
		}

		// Token: 0x06000D5A RID: 3418 RVA: 0x00036BE4 File Offset: 0x00035BE4
		private bool BuildSourceDataTree()
		{
			this._compilParams = this.Parser.CompilParams;
			this._codeCompileUnit = new CodeCompileUnit();
			this._codeCompileUnit.UserData["AllowLateBound"] = !this.Parser.FStrict;
			this._codeCompileUnit.UserData["RequireVariableDeclaration"] = this.Parser.FExplicit;
			this._usingVJSCompiler = this._codeDomProvider.FileExtension == ".jsl";
			this._sourceDataNamespace = new CodeNamespace(this.Parser.GeneratedNamespace);
			string generatedClassName = this.GetGeneratedClassName();
			if (this.Parser.BaseTypeName != null)
			{
				CodeNamespace codeNamespace = new CodeNamespace(this.Parser.BaseTypeNamespace);
				this._codeCompileUnit.Namespaces.Add(codeNamespace);
				this._intermediateClass = new CodeTypeDeclaration(this.Parser.BaseTypeName);
				if (this._designerMode)
				{
					this._intermediateClass.UserData["BaseClassDefinition"] = this.Parser.DefaultBaseType;
				}
				else
				{
					this._intermediateClass.UserData["BaseClassDefinition"] = this.Parser.BaseType;
				}
				codeNamespace.Types.Add(this._intermediateClass);
				this._intermediateClass.IsPartial = true;
				if (!this.PrecompilingForUpdatableDeployment)
				{
					this._sourceDataClass = new CodeTypeDeclaration(generatedClassName);
					this._sourceDataClass.BaseTypes.Add(new CodeTypeReference(Util.MakeFullTypeName(this.Parser.BaseTypeNamespace, this.Parser.BaseTypeName), CodeTypeReferenceOptions.GlobalReference));
					this._sourceDataNamespace.Types.Add(this._sourceDataClass);
				}
			}
			else
			{
				this._intermediateClass = new CodeTypeDeclaration(generatedClassName);
				this._intermediateClass.BaseTypes.Add(new CodeTypeReference(this.Parser.BaseType, CodeTypeReferenceOptions.GlobalReference));
				this._sourceDataNamespace.Types.Add(this._intermediateClass);
				this._sourceDataClass = this._intermediateClass;
			}
			this._codeCompileUnit.Namespaces.Add(this._sourceDataNamespace);
			if (this.PrecompilingForUpdatableDeployment && this.Parser.CodeFileVirtualPath == null)
			{
				return false;
			}
			this.GenerateClassAttributes();
			if (this._codeDomProvider is VBCodeProvider)
			{
				this._sourceDataNamespace.Imports.Add(new CodeNamespaceImport("Microsoft.VisualBasic"));
			}
			if (this.Parser.NamespaceEntries != null)
			{
				foreach (object obj in this.Parser.NamespaceEntries.Values)
				{
					NamespaceEntry namespaceEntry = (NamespaceEntry)obj;
					CodeLinePragma codeLinePragma;
					if (namespaceEntry.VirtualPath != null)
					{
						codeLinePragma = this.CreateCodeLinePragma(namespaceEntry.VirtualPath, namespaceEntry.Line);
					}
					else
					{
						codeLinePragma = null;
					}
					CodeNamespaceImport codeNamespaceImport = new CodeNamespaceImport(namespaceEntry.Namespace);
					codeNamespaceImport.LinePragma = codeLinePragma;
					this._sourceDataNamespace.Imports.Add(codeNamespaceImport);
				}
			}
			if (this._sourceDataClass != null)
			{
				string text = Util.MakeFullTypeName(this._sourceDataNamespace.Name, this._sourceDataClass.Name);
				CodeTypeReference codeTypeReference = new CodeTypeReference(text, CodeTypeReferenceOptions.GlobalReference);
				this._classTypeExpr = new CodeTypeReferenceExpression(codeTypeReference);
			}
			this.GenerateInterfaces();
			this.BuildMiscClassMembers();
			if (!this._designerMode && this._sourceDataClass != null)
			{
				this._ctor = new CodeConstructor();
				this.AddDebuggerNonUserCodeAttribute(this._ctor);
				this._sourceDataClass.Members.Add(this._ctor);
				this.BuildDefaultConstructor();
			}
			return true;
		}

		// Token: 0x06000D5B RID: 3419 RVA: 0x00036F90 File Offset: 0x00035F90
		protected virtual void GenerateClassAttributes()
		{
			if (this.CompilParams.IncludeDebugInformation && this._sourceDataClass != null)
			{
				CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration("System.Runtime.CompilerServices.CompilerGlobalScopeAttribute");
				this._sourceDataClass.CustomAttributes.Add(codeAttributeDeclaration);
			}
		}

		// Token: 0x06000D5C RID: 3420 RVA: 0x00036FD0 File Offset: 0x00035FD0
		protected virtual void GenerateInterfaces()
		{
			if (this.Parser.ImplementedInterfaces != null)
			{
				foreach (object obj in this.Parser.ImplementedInterfaces)
				{
					Type type = (Type)obj;
					this._intermediateClass.BaseTypes.Add(new CodeTypeReference(type));
				}
			}
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x0003704C File Offset: 0x0003604C
		protected virtual void BuildInitStatements(CodeStatementCollection trueStatements, CodeStatementCollection topLevelStatements)
		{
		}

		// Token: 0x06000D5E RID: 3422 RVA: 0x00037050 File Offset: 0x00036050
		protected virtual void BuildDefaultConstructor()
		{
			this._ctor.Attributes &= (MemberAttributes)(-61441);
			this._ctor.Attributes |= MemberAttributes.Public;
			CodeMemberField codeMemberField = new CodeMemberField(typeof(bool), "__initialized");
			codeMemberField.Attributes |= MemberAttributes.Static;
			this._sourceDataClass.Members.Add(codeMemberField);
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(this._classTypeExpr, "__initialized"), CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(false));
			this.BuildInitStatements(codeConditionStatement.TrueStatements, this._ctor.Statements);
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(this._classTypeExpr, "__initialized"), new CodePrimitiveExpression(true)));
			this._ctor.Statements.Add(codeConditionStatement);
		}

		// Token: 0x06000D5F RID: 3423 RVA: 0x00037144 File Offset: 0x00036144
		protected virtual void BuildMiscClassMembers()
		{
			if (this.NeedProfileProperty)
			{
				this.BuildProfileProperty();
			}
			if (this._sourceDataClass == null)
			{
				return;
			}
			this.BuildApplicationObjectProperties();
			this.BuildSessionObjectProperties();
			this.BuildPageObjectProperties();
			foreach (object obj in this.Parser.ScriptList)
			{
				ScriptBlockData scriptBlockData = (ScriptBlockData)obj;
				string text = scriptBlockData.Script;
				text = text.PadLeft(text.Length + scriptBlockData.Column - 1);
				CodeSnippetTypeMember codeSnippetTypeMember = new CodeSnippetTypeMember(text);
				codeSnippetTypeMember.LinePragma = this.CreateCodeLinePragma(scriptBlockData.VirtualPath, scriptBlockData.Line, scriptBlockData.Column, scriptBlockData.Column, scriptBlockData.Script.Length, false);
				this._sourceDataClass.Members.Add(codeSnippetTypeMember);
			}
		}

		// Token: 0x06000D60 RID: 3424 RVA: 0x00037230 File Offset: 0x00036230
		private void BuildProfileProperty()
		{
			if (!ProfileManager.Enabled)
			{
				return;
			}
			string profileClassName = ProfileBase.GetProfileClassName();
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Attributes &= (MemberAttributes)(-61441);
			codeMemberProperty.Attributes &= (MemberAttributes)(-16);
			codeMemberProperty.Attributes |= (MemberAttributes)12290;
			codeMemberProperty.Name = "Profile";
			if (this._designerMode)
			{
				this.ApplyEditorBrowsableCustomAttribute(codeMemberProperty);
			}
			codeMemberProperty.Type = new CodeTypeReference(profileClassName);
			CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Context");
			codePropertyReferenceExpression = new CodePropertyReferenceExpression(codePropertyReferenceExpression, "Profile");
			codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(profileClassName, codePropertyReferenceExpression)));
			this._intermediateClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000D61 RID: 3425 RVA: 0x000372F0 File Offset: 0x000362F0
		protected virtual bool NeedProfileProperty
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x000372F4 File Offset: 0x000362F4
		protected void BuildAccessorProperty(string propName, CodeFieldReferenceExpression fieldRef, Type propType, MemberAttributes attributes, CodeAttributeDeclarationCollection attrDeclarations)
		{
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Attributes = attributes;
			codeMemberProperty.Name = propName;
			codeMemberProperty.Type = new CodeTypeReference(propType);
			codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(fieldRef));
			codeMemberProperty.SetStatements.Add(new CodeAssignStatement(fieldRef, new CodePropertySetValueReferenceExpression()));
			if (attrDeclarations != null)
			{
				codeMemberProperty.CustomAttributes = attrDeclarations;
			}
			this._sourceDataClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0003736C File Offset: 0x0003636C
		protected void BuildFieldAndAccessorProperty(string propName, string fieldName, Type propType, bool fStatic, CodeAttributeDeclarationCollection attrDeclarations)
		{
			CodeMemberField codeMemberField = new CodeMemberField(propType, fieldName);
			if (fStatic)
			{
				codeMemberField.Attributes |= MemberAttributes.Static;
			}
			this._sourceDataClass.Members.Add(codeMemberField);
			CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);
			this.BuildAccessorProperty(propName, codeFieldReferenceExpression, propType, MemberAttributes.Public, attrDeclarations);
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x000373C4 File Offset: 0x000363C4
		private void BuildInjectedGetPropertyMethod(string propName, Type propType, CodeExpression propertyInitExpression, bool fPublicProp)
		{
			string text = "cached" + propName;
			CodeExpression codeExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), text);
			this._sourceDataClass.Members.Add(new CodeMemberField(propType, text));
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			if (fPublicProp)
			{
				codeMemberProperty.Attributes &= (MemberAttributes)(-61441);
				codeMemberProperty.Attributes |= MemberAttributes.Public;
			}
			codeMemberProperty.Name = propName;
			codeMemberProperty.Type = new CodeTypeReference(propType);
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression(codeExpression, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
			codeConditionStatement.TrueStatements.Add(new CodeAssignStatement(codeExpression, propertyInitExpression));
			codeMemberProperty.GetStatements.Add(codeConditionStatement);
			codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(codeExpression));
			this._sourceDataClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x000374A0 File Offset: 0x000364A0
		private void BuildObjectPropertiesHelper(IDictionary objects, bool useApplicationState)
		{
			IDictionaryEnumerator enumerator = objects.GetEnumerator();
			while (enumerator.MoveNext())
			{
				HttpStaticObjectsEntry httpStaticObjectsEntry = (HttpStaticObjectsEntry)enumerator.Value;
				CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), useApplicationState ? "Application" : "Session"), "StaticObjects");
				CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(codePropertyReferenceExpression, "GetObject", new CodeExpression[0]);
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(httpStaticObjectsEntry.Name));
				Type declaredType = httpStaticObjectsEntry.DeclaredType;
				if (useApplicationState)
				{
					this.BuildInjectedGetPropertyMethod(httpStaticObjectsEntry.Name, declaredType, new CodeCastExpression(declaredType, codeMethodInvokeExpression), false);
				}
				else
				{
					CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
					codeMemberProperty.Name = httpStaticObjectsEntry.Name;
					codeMemberProperty.Type = new CodeTypeReference(declaredType);
					codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(declaredType, codeMethodInvokeExpression)));
					this._sourceDataClass.Members.Add(codeMemberProperty);
				}
			}
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x00037590 File Offset: 0x00036590
		private void BuildApplicationObjectProperties()
		{
			if (this.Parser.ApplicationObjects != null)
			{
				this.BuildObjectPropertiesHelper(this.Parser.ApplicationObjects.Objects, true);
			}
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x000375B6 File Offset: 0x000365B6
		private void BuildSessionObjectProperties()
		{
			if (this.Parser.SessionObjects != null)
			{
				this.BuildObjectPropertiesHelper(this.Parser.SessionObjects.Objects, false);
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000D68 RID: 3432 RVA: 0x000375DC File Offset: 0x000365DC
		protected virtual bool IsGlobalAsaxGenerator
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x000375E0 File Offset: 0x000365E0
		private void BuildPageObjectProperties()
		{
			if (this.Parser.PageObjectList == null)
			{
				return;
			}
			foreach (object obj in this.Parser.PageObjectList)
			{
				ObjectTagBuilder objectTagBuilder = (ObjectTagBuilder)obj;
				CodeExpression codeExpression;
				if (objectTagBuilder.Progid != null)
				{
					codeExpression = new CodeMethodInvokeExpression
					{
						Method = 
						{
							TargetObject = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Server"),
							MethodName = "CreateObject"
						},
						Parameters = 
						{
							new CodePrimitiveExpression(objectTagBuilder.Progid)
						}
					};
				}
				else if (objectTagBuilder.Clsid != null)
				{
					codeExpression = new CodeMethodInvokeExpression
					{
						Method = 
						{
							TargetObject = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Server"),
							MethodName = "CreateObjectFromClsid"
						},
						Parameters = 
						{
							new CodePrimitiveExpression(objectTagBuilder.Clsid)
						}
					};
				}
				else
				{
					codeExpression = new CodeObjectCreateExpression(objectTagBuilder.ObjectType, new CodeExpression[0]);
				}
				this.BuildInjectedGetPropertyMethod(objectTagBuilder.ID, objectTagBuilder.DeclaredType, codeExpression, this.IsGlobalAsaxGenerator);
			}
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x00037724 File Offset: 0x00036724
		protected CodeLinePragma CreateCodeLinePragma(ControlBuilder builder)
		{
			string virtualPathString = builder.VirtualPathString;
			int line = builder.Line;
			int num = 1;
			int num2 = 1;
			int num3 = -1;
			CodeBlockBuilder codeBlockBuilder = builder as CodeBlockBuilder;
			if (codeBlockBuilder != null)
			{
				num = codeBlockBuilder.Column;
				num3 = codeBlockBuilder.Content.Length;
				if (codeBlockBuilder.BlockType == CodeBlockType.Code)
				{
					num2 = num;
				}
				else
				{
					num2 = "__o".Length + BaseCodeDomTreeGenerator.GetGeneratedColumnOffset(this._codeDomProvider);
				}
			}
			return this.CreateCodeLinePragma(virtualPathString, line, num, num2, num3);
		}

		// Token: 0x06000D6B RID: 3435 RVA: 0x00037798 File Offset: 0x00036798
		private static int GetGeneratedColumnOffset(CodeDomProvider codeDomProvider)
		{
			object obj = null;
			if (BaseCodeDomTreeGenerator._generatedColumnOffsetDictionary == null)
			{
				BaseCodeDomTreeGenerator._generatedColumnOffsetDictionary = new ListDictionary();
			}
			else
			{
				obj = BaseCodeDomTreeGenerator._generatedColumnOffsetDictionary[codeDomProvider.GetType()];
			}
			if (obj == null)
			{
				CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
				CodeNamespace codeNamespace = new CodeNamespace("ASP");
				codeCompileUnit.Namespaces.Add(codeNamespace);
				CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration("ColumnOffsetCalculator");
				codeTypeDeclaration.IsClass = true;
				codeNamespace.Types.Add(codeTypeDeclaration);
				CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
				codeMemberMethod.ReturnType = new CodeTypeReference(typeof(void));
				codeMemberMethod.Name = "GenerateMethod";
				codeTypeDeclaration.Members.Add(codeMemberMethod);
				CodeStatement codeStatement = new CodeAssignStatement(new CodeVariableReferenceExpression("__o"), new CodeSnippetExpression("__dummyVar"));
				codeMemberMethod.Statements.Add(codeStatement);
				StringBuilder stringBuilder = new StringBuilder();
				StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
				codeDomProvider.GenerateCodeFromCompileUnit(codeCompileUnit, stringWriter, null);
				StringReader stringReader = new StringReader(stringBuilder.ToString());
				int num = 4;
				string text;
				while ((text = stringReader.ReadLine()) != null)
				{
					text = text.TrimStart(new char[0]);
					int num2;
					if ((num2 = text.IndexOf("__dummyVar", StringComparison.Ordinal)) != -1)
					{
						num = num2 - "__o".Length + 1;
					}
				}
				BaseCodeDomTreeGenerator._generatedColumnOffsetDictionary[codeDomProvider.GetType()] = num;
				return num;
			}
			return (int)obj;
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x00037906 File Offset: 0x00036906
		protected CodeLinePragma CreateCodeLinePragma(string virtualPath, int lineNumber)
		{
			return this.CreateCodeLinePragma(virtualPath, lineNumber, 1, 1, -1, true);
		}

		// Token: 0x06000D6D RID: 3437 RVA: 0x00037914 File Offset: 0x00036914
		protected CodeLinePragma CreateCodeLinePragma(string virtualPath, int lineNumber, int column, int generatedColumn, int codeLength)
		{
			return this.CreateCodeLinePragma(virtualPath, lineNumber, column, generatedColumn, codeLength, true);
		}

		// Token: 0x06000D6E RID: 3438 RVA: 0x00037924 File Offset: 0x00036924
		protected CodeLinePragma CreateCodeLinePragma(string virtualPath, int lineNumber, int column, int generatedColumn, int codeLength, bool isCodeNugget)
		{
			if (!this.Parser.FLinePragmas)
			{
				return null;
			}
			if (string.IsNullOrEmpty(virtualPath))
			{
				return null;
			}
			if (this._designerMode)
			{
				if (codeLength < 0)
				{
					return null;
				}
				LinePragmaCodeInfo linePragmaCodeInfo = new LinePragmaCodeInfo();
				linePragmaCodeInfo._startLine = lineNumber;
				linePragmaCodeInfo._startColumn = column;
				linePragmaCodeInfo._startGeneratedColumn = generatedColumn;
				linePragmaCodeInfo._codeLength = codeLength;
				linePragmaCodeInfo._isCodeNugget = isCodeNugget;
				lineNumber = this._pragmaIdGenerator++;
				if (this._linePragmasTable == null)
				{
					this._linePragmasTable = new Hashtable();
				}
				this._linePragmasTable[lineNumber] = linePragmaCodeInfo;
			}
			return BaseCodeDomTreeGenerator.CreateCodeLinePragmaHelper(virtualPath, lineNumber);
		}

		// Token: 0x06000D6F RID: 3439 RVA: 0x000379C4 File Offset: 0x000369C4
		internal static CodeLinePragma CreateCodeLinePragmaHelper(string virtualPath, int lineNumber)
		{
			string text = null;
			if (UrlPath.IsAbsolutePhysicalPath(virtualPath))
			{
				text = virtualPath;
			}
			else if (BaseCodeDomTreeGenerator._urlLinePragmas)
			{
				text = ErrorFormatter.MakeHttpLinePragma(virtualPath);
			}
			else
			{
				try
				{
					text = HostingEnvironment.MapPathInternal(virtualPath);
					if (!File.Exists(text))
					{
						text = ErrorFormatter.MakeHttpLinePragma(virtualPath);
					}
				}
				catch
				{
					text = ErrorFormatter.MakeHttpLinePragma(virtualPath);
				}
			}
			return new CodeLinePragma(text, lineNumber);
		}

		// Token: 0x06000D70 RID: 3440 RVA: 0x00037A28 File Offset: 0x00036A28
		protected void AddDebuggerNonUserCodeAttribute(CodeMemberMethod method)
		{
			if (method == null)
			{
				return;
			}
			if (!this.Parser.FLinePragmas)
			{
				return;
			}
			CodeAttributeDeclaration codeAttributeDeclaration = new CodeAttributeDeclaration(new CodeTypeReference(typeof(DebuggerNonUserCodeAttribute)));
			method.CustomAttributes.Add(codeAttributeDeclaration);
		}

		// Token: 0x040014EC RID: 5356
		internal const string defaultNamespace = "ASP";

		// Token: 0x040014ED RID: 5357
		internal const string internalAspNamespace = "__ASP";

		// Token: 0x040014EE RID: 5358
		private const string initializedFieldName = "__initialized";

		// Token: 0x040014EF RID: 5359
		private const string _dummyVariable = "__dummyVar";

		// Token: 0x040014F0 RID: 5360
		private const int _defaultColumnOffset = 4;

		// Token: 0x040014F1 RID: 5361
		protected CodeDomProvider _codeDomProvider;

		// Token: 0x040014F2 RID: 5362
		protected CodeCompileUnit _codeCompileUnit;

		// Token: 0x040014F3 RID: 5363
		private CodeNamespace _sourceDataNamespace;

		// Token: 0x040014F4 RID: 5364
		protected CodeTypeDeclaration _sourceDataClass;

		// Token: 0x040014F5 RID: 5365
		protected CodeTypeDeclaration _intermediateClass;

		// Token: 0x040014F6 RID: 5366
		private CompilerParameters _compilParams;

		// Token: 0x040014F7 RID: 5367
		protected StringResourceBuilder _stringResourceBuilder;

		// Token: 0x040014F8 RID: 5368
		protected bool _usingVJSCompiler;

		// Token: 0x040014F9 RID: 5369
		private static IDictionary _generatedColumnOffsetDictionary;

		// Token: 0x040014FA RID: 5370
		private VirtualPath _virtualPath;

		// Token: 0x040014FB RID: 5371
		protected CodeConstructor _ctor;

		// Token: 0x040014FC RID: 5372
		protected CodeTypeReferenceExpression _classTypeExpr;

		// Token: 0x040014FD RID: 5373
		private TemplateParser _parser;

		// Token: 0x040014FE RID: 5374
		protected bool _designerMode;

		// Token: 0x040014FF RID: 5375
		private IDictionary _linePragmasTable;

		// Token: 0x04001500 RID: 5376
		private int _pragmaIdGenerator = 1;

		// Token: 0x04001501 RID: 5377
		private static bool _urlLinePragmas;
	}
}
