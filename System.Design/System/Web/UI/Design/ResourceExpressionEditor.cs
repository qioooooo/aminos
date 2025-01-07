using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.Compilation;

namespace System.Web.UI.Design
{
	public class ResourceExpressionEditor : ExpressionEditor
	{
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

		public override ExpressionEditorSheet GetExpressionEditorSheet(string expression, IServiceProvider serviceProvider)
		{
			return new ResourceExpressionEditorSheet(expression, serviceProvider);
		}
	}
}
