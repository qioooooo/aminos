using System;
using System.CodeDom;
using System.Reflection;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200016B RID: 363
	internal class DataBindingExpressionBuilder : ExpressionBuilder
	{
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x00048C18 File Offset: 0x00047C18
		internal static EventInfo Event
		{
			get
			{
				if (DataBindingExpressionBuilder.eventInfo == null)
				{
					DataBindingExpressionBuilder.eventInfo = typeof(Control).GetEvent("DataBinding");
				}
				return DataBindingExpressionBuilder.eventInfo;
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00048C40 File Offset: 0x00047C40
		internal static void BuildEvalExpression(string field, string formatString, string propertyName, Type propertyType, ControlBuilder controlBuilder, CodeStatementCollection methodStatements, CodeStatementCollection statements, CodeLinePragma linePragma, ref bool hasTempObject)
		{
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "Eval";
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(field));
			if (!string.IsNullOrEmpty(formatString))
			{
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(formatString));
			}
			CodeStatementCollection codeStatementCollection = new CodeStatementCollection();
			DataBindingExpressionBuilder.BuildPropertySetExpression(codeMethodInvokeExpression, propertyName, propertyType, controlBuilder, methodStatements, codeStatementCollection, linePragma, ref hasTempObject);
			CodeMethodInvokeExpression codeMethodInvokeExpression2 = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression2.Method.TargetObject = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Page");
			codeMethodInvokeExpression2.Method.MethodName = "GetDataItem";
			CodeConditionStatement codeConditionStatement = new CodeConditionStatement();
			codeConditionStatement.Condition = new CodeBinaryOperatorExpression(codeMethodInvokeExpression2, CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null));
			codeConditionStatement.TrueStatements.AddRange(codeStatementCollection);
			statements.Add(codeConditionStatement);
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00048D14 File Offset: 0x00047D14
		private static void BuildPropertySetExpression(CodeExpression expression, string propertyName, Type propertyType, ControlBuilder controlBuilder, CodeStatementCollection methodStatements, CodeStatementCollection statements, CodeLinePragma linePragma, ref bool hasTempObject)
		{
			CodeDomUtility.CreatePropertySetStatements(methodStatements, statements, new CodeVariableReferenceExpression("dataBindingExpressionBuilderTarget"), propertyName, propertyType, expression, linePragma);
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00048D30 File Offset: 0x00047D30
		internal static void BuildExpressionSetup(ControlBuilder controlBuilder, CodeStatementCollection methodStatements, CodeStatementCollection statements)
		{
			CodeVariableDeclarationStatement codeVariableDeclarationStatement = new CodeVariableDeclarationStatement(controlBuilder.ControlType, "dataBindingExpressionBuilderTarget");
			methodStatements.Add(codeVariableDeclarationStatement);
			CodeVariableReferenceExpression codeVariableReferenceExpression = new CodeVariableReferenceExpression(codeVariableDeclarationStatement.Name);
			CodeAssignStatement codeAssignStatement = new CodeAssignStatement(codeVariableReferenceExpression, new CodeCastExpression(controlBuilder.ControlType, new CodeArgumentReferenceExpression("sender")));
			statements.Add(codeAssignStatement);
			Type bindingContainerType = controlBuilder.BindingContainerType;
			CodeVariableDeclarationStatement codeVariableDeclarationStatement2 = new CodeVariableDeclarationStatement(bindingContainerType, "Container");
			methodStatements.Add(codeVariableDeclarationStatement2);
			CodeAssignStatement codeAssignStatement2 = new CodeAssignStatement(new CodeVariableReferenceExpression(codeVariableDeclarationStatement2.Name), new CodeCastExpression(bindingContainerType, new CodePropertyReferenceExpression(codeVariableReferenceExpression, "BindingContainer")));
			statements.Add(codeAssignStatement2);
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00048DD0 File Offset: 0x00047DD0
		internal override void BuildExpression(BoundPropertyEntry bpe, ControlBuilder controlBuilder, CodeExpression controlReference, CodeStatementCollection methodStatements, CodeStatementCollection statements, CodeLinePragma linePragma, ref bool hasTempObject)
		{
			DataBindingExpressionBuilder.BuildExpressionStatic(bpe, controlBuilder, controlReference, methodStatements, statements, linePragma, ref hasTempObject);
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00048DE4 File Offset: 0x00047DE4
		internal static void BuildExpressionStatic(BoundPropertyEntry bpe, ControlBuilder controlBuilder, CodeExpression controlReference, CodeStatementCollection methodStatements, CodeStatementCollection statements, CodeLinePragma linePragma, ref bool hasTempObject)
		{
			CodeExpression codeExpression = new CodeSnippetExpression(bpe.Expression);
			DataBindingExpressionBuilder.BuildPropertySetExpression(codeExpression, bpe.Name, bpe.Type, controlBuilder, methodStatements, statements, linePragma, ref hasTempObject);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x00048E17 File Offset: 0x00047E17
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			return null;
		}

		// Token: 0x0400164A RID: 5706
		private const string EvalMethodName = "Eval";

		// Token: 0x0400164B RID: 5707
		private const string GetDataItemMethodName = "GetDataItem";

		// Token: 0x0400164C RID: 5708
		private static EventInfo eventInfo;
	}
}
