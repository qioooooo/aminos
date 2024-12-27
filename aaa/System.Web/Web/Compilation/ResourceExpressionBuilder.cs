using System;
using System.CodeDom;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200018B RID: 395
	[ExpressionPrefix("Resources")]
	[ExpressionEditor("System.Web.UI.Design.ResourceExpressionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ResourceExpressionBuilder : ExpressionBuilder
	{
		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x0004C441 File Offset: 0x0004B441
		public override bool SupportsEvaluate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0004C444 File Offset: 0x0004B444
		public static ResourceExpressionFields ParseExpression(string expression)
		{
			return ResourceExpressionBuilder.ParseExpressionInternal(expression);
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0004C44C File Offset: 0x0004B44C
		public override object ParseExpression(string expression, Type propertyType, ExpressionBuilderContext context)
		{
			ResourceExpressionFields resourceExpressionFields = null;
			try
			{
				resourceExpressionFields = ResourceExpressionBuilder.ParseExpressionInternal(expression);
			}
			catch
			{
			}
			if (resourceExpressionFields == null)
			{
				throw new HttpException(SR.GetString("Invalid_res_expr", new object[] { expression }));
			}
			if (context.VirtualPathObject != null)
			{
				IResourceProvider resourceProvider = ResourceExpressionBuilder.GetResourceProvider(resourceExpressionFields, VirtualPath.Create(context.VirtualPath));
				object obj = null;
				if (resourceProvider != null)
				{
					try
					{
						obj = resourceProvider.GetObject(resourceExpressionFields.ResourceKey, CultureInfo.InvariantCulture);
					}
					catch
					{
					}
				}
				if (obj == null)
				{
					throw new HttpException(SR.GetString("Res_not_found", new object[] { resourceExpressionFields.ResourceKey }));
				}
			}
			return resourceExpressionFields;
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0004C508 File Offset: 0x0004B508
		public override CodeExpression GetCodeExpression(BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			ResourceExpressionFields resourceExpressionFields = (ResourceExpressionFields)parsedData;
			if (resourceExpressionFields.ClassKey.Length == 0)
			{
				return this.GetPageResCodeExpression(resourceExpressionFields.ResourceKey, entry);
			}
			return this.GetAppResCodeExpression(resourceExpressionFields.ClassKey, resourceExpressionFields.ResourceKey, entry);
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0004C54C File Offset: 0x0004B54C
		public override object EvaluateExpression(object target, BoundPropertyEntry entry, object parsedData, ExpressionBuilderContext context)
		{
			ResourceExpressionFields resourceExpressionFields = (ResourceExpressionFields)parsedData;
			IResourceProvider resourceProvider = ResourceExpressionBuilder.GetResourceProvider(resourceExpressionFields, context.VirtualPathObject);
			if (entry.Type == typeof(string))
			{
				return ResourceExpressionBuilder.GetResourceObject(resourceProvider, resourceExpressionFields.ResourceKey, null);
			}
			return ResourceExpressionBuilder.GetResourceObject(resourceProvider, resourceExpressionFields.ResourceKey, null, entry.DeclaringType, entry.PropertyInfo.Name);
		}

		// Token: 0x060010F3 RID: 4339 RVA: 0x0004C5AC File Offset: 0x0004B5AC
		private CodeExpression GetAppResCodeExpression(string classKey, string resourceKey, BoundPropertyEntry entry)
		{
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "GetGlobalResourceObject";
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(classKey));
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(resourceKey));
			if (entry.Type != typeof(string) && entry.Type != null)
			{
				codeMethodInvokeExpression.Parameters.Add(new CodeTypeOfExpression(entry.DeclaringType));
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(entry.PropertyInfo.Name));
			}
			return codeMethodInvokeExpression;
		}

		// Token: 0x060010F4 RID: 4340 RVA: 0x0004C654 File Offset: 0x0004B654
		private CodeExpression GetPageResCodeExpression(string resourceKey, BoundPropertyEntry entry)
		{
			CodeMethodInvokeExpression codeMethodInvokeExpression = new CodeMethodInvokeExpression();
			codeMethodInvokeExpression.Method.TargetObject = new CodeThisReferenceExpression();
			codeMethodInvokeExpression.Method.MethodName = "GetLocalResourceObject";
			codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(resourceKey));
			if (entry.Type != typeof(string) && entry.Type != null)
			{
				codeMethodInvokeExpression.Parameters.Add(new CodeTypeOfExpression(entry.DeclaringType));
				codeMethodInvokeExpression.Parameters.Add(new CodePrimitiveExpression(entry.PropertyInfo.Name));
			}
			return codeMethodInvokeExpression;
		}

		// Token: 0x060010F5 RID: 4341 RVA: 0x0004C6E7 File Offset: 0x0004B6E7
		internal static object GetGlobalResourceObject(string classKey, string resourceKey)
		{
			return ResourceExpressionBuilder.GetGlobalResourceObject(classKey, resourceKey, null, null, null);
		}

		// Token: 0x060010F6 RID: 4342 RVA: 0x0004C6F4 File Offset: 0x0004B6F4
		internal static object GetGlobalResourceObject(string classKey, string resourceKey, Type objType, string propName, CultureInfo culture)
		{
			IResourceProvider globalResourceProvider = ResourceExpressionBuilder.GetGlobalResourceProvider(classKey);
			return ResourceExpressionBuilder.GetResourceObject(globalResourceProvider, resourceKey, culture, objType, propName);
		}

		// Token: 0x060010F7 RID: 4343 RVA: 0x0004C713 File Offset: 0x0004B713
		internal static object GetResourceObject(IResourceProvider resourceProvider, string resourceKey, CultureInfo culture)
		{
			return ResourceExpressionBuilder.GetResourceObject(resourceProvider, resourceKey, culture, null, null);
		}

		// Token: 0x060010F8 RID: 4344 RVA: 0x0004C720 File Offset: 0x0004B720
		internal static object GetResourceObject(IResourceProvider resourceProvider, string resourceKey, CultureInfo culture, Type objType, string propName)
		{
			if (resourceProvider == null)
			{
				return null;
			}
			object @object = resourceProvider.GetObject(resourceKey, culture);
			if (objType == null)
			{
				return @object;
			}
			string text = @object as string;
			if (text == null)
			{
				return @object;
			}
			return ResourceExpressionBuilder.ObjectFromString(text, objType, propName);
		}

		// Token: 0x060010F9 RID: 4345 RVA: 0x0004C758 File Offset: 0x0004B758
		private static object ObjectFromString(string value, Type objType, string propName)
		{
			PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(objType)[propName];
			if (propertyDescriptor == null)
			{
				return null;
			}
			TypeConverter converter = propertyDescriptor.Converter;
			if (converter == null)
			{
				return null;
			}
			return converter.ConvertFromInvariantString(value);
		}

		// Token: 0x060010FA RID: 4346 RVA: 0x0004C78C File Offset: 0x0004B78C
		private static ResourceExpressionFields ParseExpressionInternal(string expression)
		{
			string text = null;
			string text2 = null;
			if (expression.Length == 0)
			{
				return new ResourceExpressionFields(text, text2);
			}
			string[] array = expression.Split(new char[] { ',' });
			int num = array.Length;
			if (num > 2)
			{
				return null;
			}
			if (num == 1)
			{
				text2 = array[0].Trim();
			}
			else
			{
				text = array[0].Trim();
				text2 = array[1].Trim();
			}
			return new ResourceExpressionFields(text, text2);
		}

		// Token: 0x060010FB RID: 4347 RVA: 0x0004C7FB File Offset: 0x0004B7FB
		private static IResourceProvider GetResourceProvider(ResourceExpressionFields fields, VirtualPath virtualPath)
		{
			if (fields.ClassKey.Length == 0)
			{
				return ResourceExpressionBuilder.GetLocalResourceProvider(virtualPath);
			}
			return ResourceExpressionBuilder.GetGlobalResourceProvider(fields.ClassKey);
		}

		// Token: 0x060010FC RID: 4348 RVA: 0x0004C81C File Offset: 0x0004B81C
		private static void EnsureResourceProviderFactory()
		{
			if (ResourceExpressionBuilder.s_resourceProviderFactory != null)
			{
				return;
			}
			GlobalizationSection globalization = RuntimeConfig.GetAppConfig().Globalization;
			Type resourceProviderFactoryTypeInternal = globalization.ResourceProviderFactoryTypeInternal;
			if (resourceProviderFactoryTypeInternal == null)
			{
				ResourceExpressionBuilder.s_resourceProviderFactory = new ResXResourceProviderFactory();
				return;
			}
			ResourceExpressionBuilder.s_resourceProviderFactory = (ResourceProviderFactory)HttpRuntime.CreatePublicInstance(resourceProviderFactoryTypeInternal);
		}

		// Token: 0x060010FD RID: 4349 RVA: 0x0004C864 File Offset: 0x0004B864
		private static IResourceProvider GetGlobalResourceProvider(string classKey)
		{
			string text = "Resources." + classKey;
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			string text2 = "A" + text;
			IResourceProvider resourceProvider = cacheInternal[text2] as IResourceProvider;
			if (resourceProvider != null)
			{
				return resourceProvider;
			}
			ResourceExpressionBuilder.EnsureResourceProviderFactory();
			resourceProvider = ResourceExpressionBuilder.s_resourceProviderFactory.CreateGlobalResourceProvider(classKey);
			cacheInternal.UtcInsert(text2, resourceProvider);
			return resourceProvider;
		}

		// Token: 0x060010FE RID: 4350 RVA: 0x0004C8BB File Offset: 0x0004B8BB
		internal static IResourceProvider GetLocalResourceProvider(TemplateControl templateControl)
		{
			return ResourceExpressionBuilder.GetLocalResourceProvider(templateControl.VirtualPath);
		}

		// Token: 0x060010FF RID: 4351 RVA: 0x0004C8C8 File Offset: 0x0004B8C8
		internal static IResourceProvider GetLocalResourceProvider(VirtualPath virtualPath)
		{
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			string text = "A" + virtualPath.VirtualPathString;
			IResourceProvider resourceProvider = cacheInternal[text] as IResourceProvider;
			if (resourceProvider != null)
			{
				return resourceProvider;
			}
			ResourceExpressionBuilder.EnsureResourceProviderFactory();
			resourceProvider = ResourceExpressionBuilder.s_resourceProviderFactory.CreateLocalResourceProvider(virtualPath.VirtualPathString);
			cacheInternal.UtcInsert(text, resourceProvider);
			return resourceProvider;
		}

		// Token: 0x06001100 RID: 4352 RVA: 0x0004C91D File Offset: 0x0004B91D
		internal static object GetParsedData(string resourceKey)
		{
			return new ResourceExpressionFields(string.Empty, resourceKey);
		}

		// Token: 0x0400168B RID: 5771
		private static ResourceProviderFactory s_resourceProviderFactory;
	}
}
