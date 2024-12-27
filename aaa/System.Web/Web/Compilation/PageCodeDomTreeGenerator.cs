using System;
using System.CodeDom;
using System.Collections;
using System.Reflection;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000183 RID: 387
	internal class PageCodeDomTreeGenerator : TemplateControlCodeDomTreeGenerator
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x00049D6F File Offset: 0x00048D6F
		private PageParser Parser
		{
			get
			{
				return this._pageParser;
			}
		}

		// Token: 0x060010B4 RID: 4276 RVA: 0x00049D77 File Offset: 0x00048D77
		internal PageCodeDomTreeGenerator(PageParser pageParser)
			: base(pageParser)
		{
			this._pageParser = pageParser;
		}

		// Token: 0x060010B5 RID: 4277 RVA: 0x00049D88 File Offset: 0x00048D88
		protected override void GenerateInterfaces()
		{
			base.GenerateInterfaces();
			if (this.Parser.FRequiresSessionState)
			{
				this._intermediateClass.BaseTypes.Add(new CodeTypeReference(typeof(IRequiresSessionState)));
			}
			if (this.Parser.FReadOnlySessionState)
			{
				this._intermediateClass.BaseTypes.Add(new CodeTypeReference(typeof(IReadOnlySessionState)));
			}
			if (!this._designerMode && this._sourceDataClass != null && (this.Parser.AspCompatMode || this.Parser.AsyncMode))
			{
				this._sourceDataClass.BaseTypes.Add(new CodeTypeReference(typeof(IHttpAsyncHandler)));
			}
		}

		// Token: 0x060010B6 RID: 4278 RVA: 0x00049E40 File Offset: 0x00048E40
		protected override void BuildInitStatements(CodeStatementCollection trueStatements, CodeStatementCollection topLevelStatements)
		{
			base.BuildInitStatements(trueStatements, topLevelStatements);
			CodeMemberField codeMemberField = new CodeMemberField(typeof(object), "__fileDependencies");
			codeMemberField.Attributes |= MemberAttributes.Static;
			this._sourceDataClass.Members.Add(codeMemberField);
			topLevelStatements.Insert(0, new CodeVariableDeclarationStatement
			{
				Type = new CodeTypeReference(typeof(string[])),
				Name = "dependencies"
			});
			StringSet stringSet = new StringSet();
			stringSet.AddCollection(this.Parser.SourceDependencies);
			trueStatements.Add(new CodeAssignStatement
			{
				Left = new CodeVariableReferenceExpression("dependencies"),
				Right = new CodeArrayCreateExpression(typeof(string), stringSet.Count)
			});
			int num = 0;
			foreach (object obj in ((IEnumerable)stringSet))
			{
				string text = (string)obj;
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
				codeAssignStatement.Left = new CodeArrayIndexerExpression(new CodeVariableReferenceExpression("dependencies"), new CodeExpression[]
				{
					new CodePrimitiveExpression(num++)
				});
				string text2 = UrlPath.MakeVirtualPathAppRelative(text);
				codeAssignStatement.Right = new CodePrimitiveExpression(text2);
				trueStatements.Add(codeAssignStatement);
			}
			trueStatements.Add(new CodeAssignStatement
			{
				Left = new CodeFieldReferenceExpression(this._classTypeExpr, "__fileDependencies"),
				Right = new CodeMethodInvokeExpression
				{
					Method = 
					{
						TargetObject = new CodeThisReferenceExpression(),
						MethodName = "GetWrappedFileDependencies"
					},
					Parameters = 
					{
						new CodeVariableReferenceExpression("dependencies")
					}
				}
			});
		}

		// Token: 0x060010B7 RID: 4279 RVA: 0x0004A020 File Offset: 0x00049020
		protected override void BuildDefaultConstructor()
		{
			base.BuildDefaultConstructor();
			if (base.CompilParams.IncludeDebugInformation)
			{
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement();
				codeAssignStatement.Left = new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Server"), "ScriptTimeout");
				codeAssignStatement.Right = new CodePrimitiveExpression(30000000);
				this._ctor.Statements.Add(codeAssignStatement);
			}
			if (this.Parser.TransactionMode != 0)
			{
				this._ctor.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "TransactionMode"), new CodePrimitiveExpression(this.Parser.TransactionMode)));
			}
			if (this.Parser.AspCompatMode)
			{
				this._ctor.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "AspCompatMode"), new CodePrimitiveExpression(this.Parser.AspCompatMode)));
			}
			if (this.Parser.AsyncMode)
			{
				this._ctor.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "AsyncMode"), new CodePrimitiveExpression(this.Parser.AsyncMode)));
			}
			if (this.Parser.OutputCacheParameters != null)
			{
				OutputCacheParameters outputCacheParameters = this.Parser.OutputCacheParameters;
				if ((outputCacheParameters.CacheProfile != null && outputCacheParameters.CacheProfile.Length != 0) || outputCacheParameters.Duration != 0 || outputCacheParameters.Location == OutputCacheLocation.None)
				{
					CodeMemberField codeMemberField = new CodeMemberField(typeof(OutputCacheParameters), "__outputCacheSettings");
					codeMemberField.Attributes |= MemberAttributes.Static;
					codeMemberField.InitExpression = new CodePrimitiveExpression(null);
					this._sourceDataClass.Members.Add(codeMemberField);
					CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
					codeConditionStatement.Condition = new CodeBinaryOperatorExpression(new CodeFieldReferenceExpression(this._classTypeExpr, "__outputCacheSettings"), CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null));
					CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement();
					codeVariableDeclarationStatement.Type = new CodeTypeReference(typeof(OutputCacheParameters));
					codeVariableDeclarationStatement.Name = "outputCacheSettings";
					codeConditionStatement.TrueStatements.Insert(0, codeVariableDeclarationStatement);
					CodeObjectCreateExpression codeObjectCreateExpression = new CodeObjectCreateExpression();
					codeObjectCreateExpression.CreateType = new CodeTypeReference(typeof(OutputCacheParameters));
					CodeVariableReferenceExpression codeVariableReferenceExpression = new CodeVariableReferenceExpression("outputCacheSettings");
					CodeAssignStatement codeAssignStatement2 = new CodeAssignStatement(codeVariableReferenceExpression, codeObjectCreateExpression);
					codeConditionStatement.TrueStatements.Add(codeAssignStatement2);
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.CacheProfile))
					{
						CodeAssignStatement codeAssignStatement3 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "CacheProfile"), new CodePrimitiveExpression(outputCacheParameters.CacheProfile));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement3);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.Duration))
					{
						CodeAssignStatement codeAssignStatement4 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "Duration"), new CodePrimitiveExpression(outputCacheParameters.Duration));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement4);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.Enabled))
					{
						CodeAssignStatement codeAssignStatement5 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "Enabled"), new CodePrimitiveExpression(outputCacheParameters.Enabled));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement5);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.Location))
					{
						CodeAssignStatement codeAssignStatement6 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "Location"), new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(OutputCacheLocation)), outputCacheParameters.Location.ToString()));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement6);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.NoStore))
					{
						CodeAssignStatement codeAssignStatement7 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "NoStore"), new CodePrimitiveExpression(outputCacheParameters.NoStore));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement7);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.SqlDependency))
					{
						CodeAssignStatement codeAssignStatement8 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "SqlDependency"), new CodePrimitiveExpression(outputCacheParameters.SqlDependency));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement8);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.VaryByControl))
					{
						CodeAssignStatement codeAssignStatement9 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "VaryByControl"), new CodePrimitiveExpression(outputCacheParameters.VaryByControl));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement9);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.VaryByCustom))
					{
						CodeAssignStatement codeAssignStatement10 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "VaryByCustom"), new CodePrimitiveExpression(outputCacheParameters.VaryByCustom));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement10);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.VaryByContentEncoding))
					{
						CodeAssignStatement codeAssignStatement11 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "VaryByContentEncoding"), new CodePrimitiveExpression(outputCacheParameters.VaryByContentEncoding));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement11);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.VaryByHeader))
					{
						CodeAssignStatement codeAssignStatement12 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "VaryByHeader"), new CodePrimitiveExpression(outputCacheParameters.VaryByHeader));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement12);
					}
					if (outputCacheParameters.IsParameterSet(OutputCacheParameter.VaryByParam))
					{
						CodeAssignStatement codeAssignStatement13 = new CodeAssignStatement(new CodePropertyReferenceExpression(codeVariableReferenceExpression, "VaryByParam"), new CodePrimitiveExpression(outputCacheParameters.VaryByParam));
						codeConditionStatement.TrueStatements.Add(codeAssignStatement13);
					}
					CodeFieldReferenceExpression codeFieldReferenceExpression = new CodeFieldReferenceExpression(this._classTypeExpr, "__outputCacheSettings");
					CodeAssignStatement codeAssignStatement14 = new CodeAssignStatement(codeFieldReferenceExpression, codeVariableReferenceExpression);
					codeConditionStatement.TrueStatements.Add(codeAssignStatement14);
					this._ctor.Statements.Add(codeConditionStatement);
				}
			}
		}

		// Token: 0x060010B8 RID: 4280 RVA: 0x0004A548 File Offset: 0x00049548
		protected override void BuildMiscClassMembers()
		{
			base.BuildMiscClassMembers();
			if (!this._designerMode && this._sourceDataClass != null)
			{
				this.BuildGetTypeHashCodeMethod();
				if (this.Parser.AspCompatMode)
				{
					this.BuildAspCompatMethods();
				}
				if (this.Parser.AsyncMode)
				{
					this.BuildAsyncPageMethods();
				}
				this.BuildProcessRequestOverride();
			}
			if (this.Parser.PreviousPageType != null)
			{
				base.BuildStronglyTypedProperty("PreviousPage", this.Parser.PreviousPageType);
			}
			if (this.Parser.MasterPageType != null)
			{
				base.BuildStronglyTypedProperty("Master", this.Parser.MasterPageType);
			}
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0004A5E4 File Offset: 0x000495E4
		private void BuildGetTypeHashCodeMethod()
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			base.AddDebuggerNonUserCodeAttribute(codeMemberMethod);
			codeMemberMethod.Name = "GetTypeHashCode";
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(int));
			codeMemberMethod.Attributes &= (MemberAttributes)(-61441);
			codeMemberMethod.Attributes &= (MemberAttributes)(-16);
			codeMemberMethod.Attributes |= (MemberAttributes)24580;
			this._sourceDataClass.Members.Add(codeMemberMethod);
			codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(this.Parser.TypeHashCode)));
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x0004A689 File Offset: 0x00049689
		internal override CodeExpression BuildPagePropertyReferenceExpression()
		{
			return new CodeThisReferenceExpression();
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x0004A690 File Offset: 0x00049690
		protected override void BuildFrameworkInitializeMethodContents(CodeMemberMethod method)
		{
			if (this.Parser.StyleSheetTheme != null)
			{
				CodeExpression codeExpression = new CodePrimitiveExpression(this.Parser.StyleSheetTheme);
				CodeExpression codeExpression2 = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "StyleSheetTheme");
				CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeExpression2, codeExpression);
				method.Statements.Add(codeAssignStatement);
			}
			base.BuildFrameworkInitializeMethodContents(method);
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "AddWrappedFileDependencies";
			codeMethodInvokeExpression.Parameters.Add(new CodeFieldReferenceExpression(this._classTypeExpr, "__fileDependencies"));
			method.Statements.Add(codeMethodInvokeExpression);
			if (this.Parser.OutputCacheParameters != null)
			{
				OutputCacheParameters outputCacheParameters = this.Parser.OutputCacheParameters;
				if ((outputCacheParameters.CacheProfile != null && outputCacheParameters.CacheProfile.Length != 0) || outputCacheParameters.Duration != 0 || outputCacheParameters.Location == OutputCacheLocation.None)
				{
					CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
					codeMethodInvokeExpression2.Method.TargetObject = new CodeThisReferenceExpression();
					codeMethodInvokeExpression2.Method.MethodName = "InitOutputCache";
					codeMethodInvokeExpression2.Parameters.Add(new CodeFieldReferenceExpression(this._classTypeExpr, "__outputCacheSettings"));
					method.Statements.Add(codeMethodInvokeExpression2);
				}
			}
			if (this.Parser.TraceEnabled != TraceEnable.Default)
			{
				method.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "TraceEnabled"), new CodePrimitiveExpression(this.Parser.TraceEnabled == TraceEnable.Enable)));
			}
			if (this.Parser.TraceMode != TraceMode.Default)
			{
				method.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "TraceModeValue"), new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(typeof(TraceMode)), this.Parser.TraceMode.ToString())));
			}
			if (this.Parser.ValidateRequest)
			{
				CodeMethodInvokeExpression codeMethodInvokeExpression3 = new CodeMethodInvokeExpression();
				codeMethodInvokeExpression3.Method.TargetObject = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Request");
				codeMethodInvokeExpression3.Method.MethodName = "ValidateInput";
				method.Statements.Add(new CodeExpressionStatement(codeMethodInvokeExpression3));
			}
		}

		// Token: 0x060010BC RID: 4284 RVA: 0x0004A8C0 File Offset: 0x000498C0
		private void BuildAspCompatMethods()
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			base.AddDebuggerNonUserCodeAttribute(codeMemberMethod);
			codeMemberMethod.Name = "BeginProcessRequest";
			codeMemberMethod.Attributes &= (MemberAttributes)(-61441);
			codeMemberMethod.Attributes &= (MemberAttributes)(-16);
			codeMemberMethod.Attributes |= MemberAttributes.Public;
			codeMemberMethod.ImplementationTypes.Add(new CodeTypeReference(typeof(IHttpAsyncHandler)));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(HttpContext), "context"));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(AsyncCallback), "cb"));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "data"));
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(IAsyncResult));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "AspCompatBeginProcessRequest";
			codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("context"));
			codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("cb"));
			codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("data"));
			codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(codeMethodInvokeExpression));
			this._sourceDataClass.Members.Add(codeMemberMethod);
			codeMemberMethod = new CodeMemberMethod();
			base.AddDebuggerNonUserCodeAttribute(codeMemberMethod);
			codeMemberMethod.Name = "EndProcessRequest";
			codeMemberMethod.Attributes &= (MemberAttributes)(-61441);
			codeMemberMethod.Attributes &= (MemberAttributes)(-16);
			codeMemberMethod.Attributes |= MemberAttributes.Public;
			codeMemberMethod.ImplementationTypes.Add(typeof(IHttpAsyncHandler));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IAsyncResult), "ar"));
			CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression2.Method.MethodName = "AspCompatEndProcessRequest";
			codeMethodInvokeExpression2.Parameters.Add(new CodeArgumentReferenceExpression("ar"));
			codeMemberMethod.Statements.Add(codeMethodInvokeExpression2);
			this._sourceDataClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x060010BD RID: 4285 RVA: 0x0004AB10 File Offset: 0x00049B10
		private void BuildAsyncPageMethods()
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			base.AddDebuggerNonUserCodeAttribute(codeMemberMethod);
			codeMemberMethod.Name = "BeginProcessRequest";
			codeMemberMethod.Attributes &= (MemberAttributes)(-61441);
			codeMemberMethod.Attributes &= (MemberAttributes)(-16);
			codeMemberMethod.Attributes |= MemberAttributes.Public;
			codeMemberMethod.ImplementationTypes.Add(new CodeTypeReference(typeof(IHttpAsyncHandler)));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(HttpContext), "context"));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(AsyncCallback), "cb"));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "data"));
			codeMemberMethod.ReturnType = new CodeTypeReference(typeof(IAsyncResult));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "AsyncPageBeginProcessRequest";
			codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("context"));
			codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("cb"));
			codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("data"));
			codeMemberMethod.Statements.Add(new CodeMethodReturnStatement(codeMethodInvokeExpression));
			this._sourceDataClass.Members.Add(codeMemberMethod);
			codeMemberMethod = new CodeMemberMethod();
			base.AddDebuggerNonUserCodeAttribute(codeMemberMethod);
			codeMemberMethod.Name = "EndProcessRequest";
			codeMemberMethod.Attributes &= (MemberAttributes)(-61441);
			codeMemberMethod.Attributes &= (MemberAttributes)(-16);
			codeMemberMethod.Attributes |= MemberAttributes.Public;
			codeMemberMethod.ImplementationTypes.Add(typeof(IHttpAsyncHandler));
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IAsyncResult), "ar"));
			CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression2.Method.MethodName = "AsyncPageEndProcessRequest";
			codeMethodInvokeExpression2.Parameters.Add(new CodeArgumentReferenceExpression("ar"));
			codeMemberMethod.Statements.Add(codeMethodInvokeExpression2);
			this._sourceDataClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x0004AD60 File Offset: 0x00049D60
		private void BuildProcessRequestOverride()
		{
			CodeMemberMethod codeMemberMethod = new CodeMemberMethod();
			base.AddDebuggerNonUserCodeAttribute(codeMemberMethod);
			codeMemberMethod.Name = "ProcessRequest";
			codeMemberMethod.Attributes &= (MemberAttributes)(-61441);
			codeMemberMethod.Attributes &= (MemberAttributes)(-16);
			MethodInfo methodInfo = null;
			if (this.Parser.BaseType != typeof(Page))
			{
				methodInfo = this.Parser.BaseType.GetMethod("ProcessRequest", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(HttpContext) }, null);
			}
			this._sourceDataClass.BaseTypes.Add(new CodeTypeReference(typeof(IHttpHandler)));
			if (methodInfo != null && methodInfo.DeclaringType != typeof(Page))
			{
				codeMemberMethod.Attributes |= (MemberAttributes)24592;
			}
			else
			{
				codeMemberMethod.Attributes |= (MemberAttributes)24580;
			}
			codeMemberMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(HttpContext), "context"));
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeBaseReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "ProcessRequest";
			codeMethodInvokeExpression.Parameters.Add(new CodeArgumentReferenceExpression("context"));
			codeMemberMethod.Statements.Add(codeMethodInvokeExpression);
			this._sourceDataClass.Members.Add(codeMemberMethod);
		}

		// Token: 0x04001667 RID: 5735
		private const string fileDependenciesName = "__fileDependencies";

		// Token: 0x04001668 RID: 5736
		private const string dependenciesLocalName = "dependencies";

		// Token: 0x04001669 RID: 5737
		private const string outputCacheSettingsLocalName = "outputCacheSettings";

		// Token: 0x0400166A RID: 5738
		private const string _previousPagePropertyName = "PreviousPage";

		// Token: 0x0400166B RID: 5739
		private const string _masterPropertyName = "Master";

		// Token: 0x0400166C RID: 5740
		private const string _styleSheetThemePropertyName = "StyleSheetTheme";

		// Token: 0x0400166D RID: 5741
		private const string outputCacheSettingsFieldName = "__outputCacheSettings";

		// Token: 0x0400166E RID: 5742
		internal const int DebugScriptTimeout = 30000000;

		// Token: 0x0400166F RID: 5743
		private PageParser _pageParser;
	}
}
