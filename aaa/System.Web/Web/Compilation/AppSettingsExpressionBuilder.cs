using System;
using System.CodeDom;
using System.ComponentModel;
using System.Configuration;
using System.Security.Permissions;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000129 RID: 297
	[ExpressionPrefix("AppSettings")]
	[ExpressionEditor("System.Web.UI.Design.AppSettingsExpressionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AppSettingsExpressionBuilder : ExpressionBuilder
	{
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06000D7B RID: 3451 RVA: 0x00037BF5 File Offset: 0x00036BF5
		public override bool SupportsEvaluate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x00037BF8 File Offset: 0x00036BF8
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			if (entry.DeclaringType == null || entry.PropertyInfo == null)
			{
				return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(base.GetType()), "GetAppSetting", new CodeExpression[]
				{
					new CodePrimitiveExpression(entry.Expression.Trim())
				});
			}
			return new CodeMethodInvokeExpression(new CodeTypeReferenceExpression(base.GetType()), "GetAppSetting", new CodeExpression[]
			{
				new CodePrimitiveExpression(entry.Expression.Trim()),
				new CodeTypeOfExpression(entry.DeclaringType),
				new CodePrimitiveExpression(entry.PropertyInfo.Name)
			});
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x00037C97 File Offset: 0x00036C97
		public override object EvaluateExpression(object target, BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			return AppSettingsExpressionBuilder.GetAppSetting(entry.Expression, target.GetType(), entry.PropertyInfo.Name);
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00037CB8 File Offset: 0x00036CB8
		public static object GetAppSetting(string key)
		{
			string text = ConfigurationManager.AppSettings[key];
			if (text == null)
			{
				throw new InvalidOperationException(SR.GetString("AppSetting_not_found", new object[] { key }));
			}
			return text;
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x00037CF4 File Offset: 0x00036CF4
		public static object GetAppSetting(string key, Type targetType, string propertyName)
		{
			string text = ConfigurationManager.AppSettings[key];
			if (targetType != null)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(targetType)[propertyName];
				if (propertyDescriptor != null && propertyDescriptor.PropertyType != typeof(string))
				{
					TypeConverter converter = propertyDescriptor.Converter;
					if (converter.CanConvertFrom(typeof(string)))
					{
						return converter.ConvertFrom(text);
					}
					throw new InvalidOperationException(SR.GetString("AppSetting_not_convertible", new object[]
					{
						text,
						propertyDescriptor.PropertyType.Name,
						propertyDescriptor.Name
					}));
				}
			}
			if (text == null)
			{
				throw new InvalidOperationException(SR.GetString("AppSetting_not_found", new object[] { key }));
			}
			return text;
		}
	}
}
