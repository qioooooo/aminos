using System;
using System.CodeDom;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.Design;

namespace System.Web.Compilation
{
	// Token: 0x02000128 RID: 296
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class ExpressionBuilder
	{
		// Token: 0x06000D73 RID: 3443 RVA: 0x00037A7C File Offset: 0x00036A7C
		internal virtual void BuildExpression(BoundPropertyEntry bpe, ControlBuilder controlBuilder, CodeExpression controlReference, CodeStatementCollection methodStatements, CodeStatementCollection statements, CodeLinePragma linePragma, ref bool hasTempObject)
		{
			CodeExpression codeExpression = this.GetCodeExpression(bpe, bpe.ParsedExpressionData, new ExpressionBuilderContext(controlBuilder.VirtualPath));
			CodeDomUtility.CreatePropertySetStatements(methodStatements, statements, controlReference, bpe.Name, bpe.Type, codeExpression, linePragma);
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x00037ABB File Offset: 0x00036ABB
		internal static ExpressionBuilder GetExpressionBuilder(string expressionPrefix, VirtualPath virtualPath)
		{
			return ExpressionBuilder.GetExpressionBuilder(expressionPrefix, virtualPath, null);
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x00037AC8 File Offset: 0x00036AC8
		internal static ExpressionBuilder GetExpressionBuilder(string expressionPrefix, VirtualPath virtualPath, IDesignerHost host)
		{
			if (expressionPrefix.Length == 0)
			{
				if (ExpressionBuilder.dataBindingExpressionBuilder == null)
				{
					ExpressionBuilder.dataBindingExpressionBuilder = new DataBindingExpressionBuilder();
				}
				return ExpressionBuilder.dataBindingExpressionBuilder;
			}
			CompilationSection compilationSection = null;
			if (host != null)
			{
				IWebApplication webApplication = (IWebApplication)host.GetService(typeof(IWebApplication));
				if (webApplication != null)
				{
					compilationSection = webApplication.OpenWebConfiguration(true).GetSection("system.web/compilation") as CompilationSection;
				}
			}
			if (compilationSection == null)
			{
				compilationSection = RuntimeConfig.GetConfig(virtualPath).Compilation;
			}
			ExpressionBuilder expressionBuilder = compilationSection.ExpressionBuilders[expressionPrefix];
			if (expressionBuilder == null)
			{
				throw new HttpParseException(SR.GetString("InvalidExpressionPrefix", new object[] { expressionPrefix }));
			}
			Type type = null;
			if (host != null)
			{
				ITypeResolutionService typeResolutionService = (ITypeResolutionService)host.GetService(typeof(ITypeResolutionService));
				if (typeResolutionService != null)
				{
					type = typeResolutionService.GetType(expressionBuilder.Type);
				}
			}
			if (type == null)
			{
				type = expressionBuilder.TypeInternal;
			}
			if (!typeof(ExpressionBuilder).IsAssignableFrom(type))
			{
				throw new HttpParseException(SR.GetString("ExpressionBuilder_InvalidType", new object[] { type.FullName }));
			}
			return (ExpressionBuilder)HttpRuntime.FastCreatePublicInstance(type);
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00037BE4 File Offset: 0x00036BE4
		public virtual bool SupportsEvaluate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x00037BE7 File Offset: 0x00036BE7
		public virtual object ParseExpression(string expression, Type propertyType, ExpressionBuilderContext context)
		{
			return null;
		}

		// Token: 0x06000D78 RID: 3448
		public abstract CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context);

		// Token: 0x06000D79 RID: 3449 RVA: 0x00037BEA File Offset: 0x00036BEA
		public virtual object EvaluateExpression(object target, BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			return null;
		}

		// Token: 0x04001503 RID: 5379
		private static ExpressionBuilder dataBindingExpressionBuilder;
	}
}
