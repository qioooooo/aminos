using System;
using System.CodeDom;
using System.Configuration;
using System.Security.Permissions;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000169 RID: 361
	[ExpressionEditor("System.Web.UI.Design.ConnectionStringsExpressionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[ExpressionPrefix("ConnectionStrings")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ConnectionStringsExpressionBuilder : ExpressionBuilder
	{
		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x000487DC File Offset: 0x000477DC
		public override bool SupportsEvaluate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x000487E0 File Offset: 0x000477E0
		public override object ParseExpression(string expression, Type propertyType, ExpressionBuilderContext context)
		{
			string text = string.Empty;
			bool flag = true;
			if (expression != null)
			{
				if (expression.EndsWith(".connectionstring", StringComparison.OrdinalIgnoreCase))
				{
					text = expression.Substring(0, expression.Length - ".connectionstring".Length);
				}
				else if (expression.EndsWith(".providername", StringComparison.OrdinalIgnoreCase))
				{
					flag = false;
					text = expression.Substring(0, expression.Length - ".providername".Length);
				}
				else
				{
					text = expression;
				}
			}
			return new Pair(text, flag);
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x0004885C File Offset: 0x0004785C
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			Pair pair = (Pair)parsedData;
			string text = (string)pair.First;
			bool flag = (bool)pair.Second;
			if (flag)
			{
				return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(base.GetType()), "GetConnectionString", new CodeExpression[]
				{
					new CodePrimitiveExpression(text)
				});
			}
			return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(base.GetType()), "GetConnectionStringProviderName", new CodeExpression[]
			{
				new CodePrimitiveExpression(text)
			});
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x000488DC File Offset: 0x000478DC
		public override object EvaluateExpression(object target, BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			Pair pair = (Pair)parsedData;
			string text = (string)pair.First;
			bool flag = (bool)pair.Second;
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[text];
			if (flag)
			{
				return ConnectionStringsExpressionBuilder.GetConnectionString(text);
			}
			return ConnectionStringsExpressionBuilder.GetConnectionStringProviderName(text);
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x00048924 File Offset: 0x00047924
		public static string GetConnectionStringProviderName(string connectionStringName)
		{
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
			if (connectionStringSettings == null)
			{
				throw new InvalidOperationException(SR.GetString("Connection_string_not_found", new object[] { connectionStringName }));
			}
			return connectionStringSettings.ProviderName;
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00048964 File Offset: 0x00047964
		public static string GetConnectionString(string connectionStringName)
		{
			ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[connectionStringName];
			if (connectionStringSettings == null)
			{
				throw new InvalidOperationException(SR.GetString("Connection_string_not_found", new object[] { connectionStringName }));
			}
			return connectionStringSettings.ConnectionString;
		}
	}
}
