using System;
using System.CodeDom;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200017D RID: 381
	internal abstract class TemplateControlCodeDomTreeGenerator : BaseTemplateCodeDomTreeGenerator
	{
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x06001095 RID: 4245 RVA: 0x0004943E File Offset: 0x0004843E
		private TemplateControlParser Parser
		{
			get
			{
				return this._tcParser;
			}
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00049446 File Offset: 0x00048446
		internal TemplateControlCodeDomTreeGenerator(TemplateControlParser tcParser)
			: base(tcParser)
		{
			this._tcParser = tcParser;
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x00049458 File Offset: 0x00048458
		protected override void BuildInitStatements(CodeStatementCollection trueStatements, CodeStatementCollection topLevelStatements)
		{
			base.BuildInitStatements(trueStatements, topLevelStatements);
			if (this._stringResourceBuilder.HasStrings)
			{
				CodeMemberField codeMemberField = new CodeMemberField(typeof(object), "__stringResource");
				codeMemberField.Attributes |= MemberAttributes.Static;
				this._sourceDataClass.Members.Add(codeMemberField);
				trueStatements.Add(new CodeAssignStatement
				{
					Left = new CodeFieldReferenceExpression(this._classTypeExpr, "__stringResource"),
					Right = new CodeMethodInvokeExpression
					{
						Method = 
						{
							TargetObject = new CodeThisReferenceExpression(),
							MethodName = "ReadStringResource"
						}
					}
				});
			}
			CodeTypeReference codeTypeReference = new CodeTypeReference(this.Parser.BaseType, CodeTypeReferenceOptions.GlobalReference);
			CodeAssignStatement codeAssignStatement = new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeCastExpression(codeTypeReference, new CodeThisReferenceExpression()), "AppRelativeVirtualPath"), new CodePrimitiveExpression(this.Parser.CurrentVirtualPath.AppRelativeVirtualPathString));
			if (!this._designerMode && this.Parser.CodeFileVirtualPath != null)
			{
				codeAssignStatement.LinePragma = BaseCodeDomTreeGenerator.CreateCodeLinePragmaHelper(this.Parser.CodeFileVirtualPath.VirtualPathString, 912304);
			}
			topLevelStatements.Add(codeAssignStatement);
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0004958B File Offset: 0x0004858B
		protected override void BuildMiscClassMembers()
		{
			base.BuildMiscClassMembers();
			if (!this._designerMode)
			{
				this.BuildAutomaticEventHookup();
			}
			this.BuildApplicationInstanceProperty();
			this.BuildSourceDataTreeFromBuilder(this.Parser.RootBuilder, false, false, null);
			if (!this._designerMode)
			{
				this.BuildFrameworkInitializeMethod();
			}
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x000495CC File Offset: 0x000485CC
		internal void BuildStronglyTypedProperty(string propertyName, Type propertyType)
		{
			if (this._usingVJSCompiler)
			{
				return;
			}
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Attributes &= (MemberAttributes)(-61441);
			codeMemberProperty.Attributes &= (MemberAttributes)(-16);
			codeMemberProperty.Attributes |= (MemberAttributes)24594;
			codeMemberProperty.Name = propertyName;
			codeMemberProperty.Type = new CodeTypeReference(propertyType);
			CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodeBaseReferenceExpression(), propertyName);
			codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(propertyType, codePropertyReferenceExpression)));
			this._intermediateClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x00049664 File Offset: 0x00048664
		private void BuildFrameworkInitializeMethod()
		{
			if (this._sourceDataClass == null)
			{
				return;
			}
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			base.AddDebuggerNonUserCodeAttribute(codeMemberMethod);
			codeMemberMethod.Attributes &= (MemberAttributes)(-61441);
			codeMemberMethod.Attributes &= (MemberAttributes)(-16);
			codeMemberMethod.Attributes |= (MemberAttributes)12292;
			codeMemberMethod.Name = "FrameworkInitialize";
			this.BuildFrameworkInitializeMethodContents(codeMemberMethod);
			if (!this._designerMode && this.Parser.CodeFileVirtualPath != null)
			{
				codeMemberMethod.LinePragma = BaseCodeDomTreeGenerator.CreateCodeLinePragmaHelper(this.Parser.CodeFileVirtualPath.VirtualPathString, 912304);
			}
			this._sourceDataClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x0004971C File Offset: 0x0004871C
		protected virtual void BuildFrameworkInitializeMethodContents(CodeMemberMethod method)
		{
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression(new CodeBaseReferenceExpression(), method.Name, new CodeExpression[0]);
			method.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression));
			if (this._stringResourceBuilder.HasStrings)
			{
				CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(), "SetStringResourcePointer", new CodeExpression[0]);
				codeMethodInvokeExpression2.Parameters.Add(new CodeFieldReferenceExpression(this._classTypeExpr, "__stringResource"));
				codeMethodInvokeExpression2.Parameters.Add(new CodePrimitiveExpression(0));
				method.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression2));
			}
			CodeMethodInvokeExpression codeMethodInvokeExpression3 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression3.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression3.Method.MethodName = "__BuildControlTree";
			codeMethodInvokeExpression3.Parameters.Add(new CodeThisReferenceExpression());
			method.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression3));
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x00049804 File Offset: 0x00048804
		private void BuildAutomaticEventHookup()
		{
			if (this._sourceDataClass == null)
			{
				return;
			}
			if (!this.Parser.FAutoEventWireup)
			{
				CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
				codeMemberProperty.Attributes &= (MemberAttributes)(-61441);
				codeMemberProperty.Attributes &= (MemberAttributes)(-16);
				codeMemberProperty.Attributes |= (MemberAttributes)12292;
				codeMemberProperty.Name = "SupportAutoEvents";
				codeMemberProperty.Type = new CodeTypeReference(typeof(bool));
				codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(false)));
				this._sourceDataClass.Members.Add(codeMemberProperty);
			}
		}

		// Token: 0x0600109D RID: 4253 RVA: 0x000498B4 File Offset: 0x000488B4
		private void BuildApplicationInstanceProperty()
		{
			Type globalAsaxType = BuildManager.GetGlobalAsaxType();
			CodeMemberProperty codeMemberProperty = new CodeMemberProperty();
			codeMemberProperty.Attributes &= (MemberAttributes)(-61441);
			codeMemberProperty.Attributes &= (MemberAttributes)(-16);
			codeMemberProperty.Attributes |= (MemberAttributes)12290;
			if (this._designerMode)
			{
				base.ApplyEditorBrowsableCustomAttribute(codeMemberProperty);
			}
			codeMemberProperty.Name = "ApplicationInstance";
			codeMemberProperty.Type = new CodeTypeReference(globalAsaxType);
			CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Context");
			codePropertyReferenceExpression = new CodePropertyReferenceExpression(codePropertyReferenceExpression, "ApplicationInstance");
			codeMemberProperty.GetStatements.Add(new CodeMethodReturnStatement(new CodeCastExpression(globalAsaxType, codePropertyReferenceExpression)));
			this._intermediateClass.Members.Add(codeMemberProperty);
		}

		// Token: 0x0400165B RID: 5723
		private const string stringResourcePointerName = "__stringResource";

		// Token: 0x0400165C RID: 5724
		private const string literalMemoryBlockName = "__literals";

		// Token: 0x0400165D RID: 5725
		internal const int badBaseClassLineMarker = 912304;

		// Token: 0x0400165E RID: 5726
		private TemplateControlParser _tcParser;
	}
}
