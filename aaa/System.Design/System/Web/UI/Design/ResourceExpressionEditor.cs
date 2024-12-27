using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Compilation;

namespace System.Web.UI.Design
{
	// Token: 0x0200038B RID: 907
	public class ResourceExpressionEditor : ExpressionEditor
	{
		// Token: 0x06002170 RID: 8560 RVA: 0x000B982C File Offset: 0x000B882C
		public override object EvaluateExpression(string expression, object parseTimeData, Type propertyType, IServiceProvider serviceProvider)
		{
			ResourceExpressionFields resourceExpressionFields;
			if (parseTimeData is ResourceExpressionFields)
			{
				resourceExpressionFields = (ResourceExpressionFields)parseTimeData;
			}
			else
			{
				resourceExpressionFields = ResourceExpressionBuilder.ParseExpression(expression);
			}
			if (string.IsNullOrEmpty(resourceExpressionFields.ResourceKey))
			{
				return null;
			}
			object obj = null;
			DesignTimeResourceProviderFactory designTimeResourceProviderFactory = ControlDesigner.GetDesignTimeResourceProviderFactory(serviceProvider);
			IResourceProvider resourceProvider;
			if (string.IsNullOrEmpty(resourceExpressionFields.ClassKey))
			{
				resourceProvider = designTimeResourceProviderFactory.CreateDesignTimeLocalResourceProvider(serviceProvider);
			}
			else
			{
				resourceProvider = designTimeResourceProviderFactory.CreateDesignTimeGlobalResourceProvider(serviceProvider, resourceExpressionFields.ClassKey);
			}
			if (resourceProvider != null)
			{
				obj = resourceProvider.GetObject(resourceExpressionFields.ResourceKey, CultureInfo.InvariantCulture);
			}
			if (obj != null)
			{
				Type type = obj.GetType();
				if (!propertyType.IsAssignableFrom(type))
				{
					TypeConverter converter = TypeDescriptor.GetConverter(propertyType);
					if (converter != null && converter.CanConvertFrom(type))
					{
						return converter.ConvertFrom(obj);
					}
				}
			}
			return obj;
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000B98DC File Offset: 0x000B88DC
		public override ExpressionEditorSheet GetExpressionEditorSheet(string expression, IServiceProvider serviceProvider)
		{
			return new ResourceExpressionEditorSheet(expression, serviceProvider);
		}
	}
}
