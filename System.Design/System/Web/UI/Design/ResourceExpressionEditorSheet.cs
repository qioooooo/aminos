using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Resources;
using System.Web.Compilation;

namespace System.Web.UI.Design
{
	public class ResourceExpressionEditorSheet : ExpressionEditorSheet
	{
		public ResourceExpressionEditorSheet(string expression, IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			if (!string.IsNullOrEmpty(expression))
			{
				ResourceExpressionEditorSheet.ResourceExpressionFields resourceExpressionFields = ResourceExpressionEditorSheet.ParseExpressionInternal(expression);
				this.ClassKey = resourceExpressionFields.ClassKey;
				this.ResourceKey = resourceExpressionFields.ResourceKey;
			}
		}

		[DefaultValue("")]
		[SRDescription("ResourceExpressionEditorSheet_ClassKey")]
		public string ClassKey
		{
			get
			{
				if (this._classKey == null)
				{
					return string.Empty;
				}
				return this._classKey;
			}
			set
			{
				this._classKey = value;
			}
		}

		public override bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.ResourceKey);
			}
		}

		[DefaultValue("")]
		[SRDescription("ResourceExpressionEditorSheet_ResourceKey")]
		[TypeConverter(typeof(ResourceExpressionEditorSheet.ResourceKeyTypeConverter))]
		public string ResourceKey
		{
			get
			{
				if (this._resourceKey == null)
				{
					return string.Empty;
				}
				return this._resourceKey;
			}
			set
			{
				this._resourceKey = value;
			}
		}

		public override string GetExpression()
		{
			string empty = string.Empty;
			if (!string.IsNullOrEmpty(this._classKey))
			{
				return this._classKey + ", " + this._resourceKey;
			}
			return this._resourceKey;
		}

		private static ResourceExpressionEditorSheet.ResourceExpressionFields ParseExpressionInternal(string expression)
		{
			ResourceExpressionEditorSheet.ResourceExpressionFields resourceExpressionFields = new ResourceExpressionEditorSheet.ResourceExpressionFields();
			int length = expression.Length;
			string[] array = expression.Split(new char[] { ',' });
			int num = array.Length;
			if (num > 2)
			{
				return null;
			}
			if (num == 1)
			{
				resourceExpressionFields.ResourceKey = array[0].Trim();
			}
			else
			{
				resourceExpressionFields.ClassKey = array[0].Trim();
				resourceExpressionFields.ResourceKey = array[1].Trim();
			}
			return resourceExpressionFields;
		}

		private string _classKey;

		private string _resourceKey;

		internal class ResourceExpressionFields
		{
			internal string ClassKey;

			internal string ResourceKey;
		}

		private class ResourceKeyTypeConverter : StringConverter
		{
			private static ICollection GetResourceKeys(IServiceProvider serviceProvider, string classKey)
			{
				DesignTimeResourceProviderFactory designTimeResourceProviderFactory = ControlDesigner.GetDesignTimeResourceProviderFactory(serviceProvider);
				IResourceProvider resourceProvider;
				if (string.IsNullOrEmpty(classKey))
				{
					resourceProvider = designTimeResourceProviderFactory.CreateDesignTimeLocalResourceProvider(serviceProvider);
				}
				else
				{
					resourceProvider = designTimeResourceProviderFactory.CreateDesignTimeGlobalResourceProvider(serviceProvider, classKey);
				}
				if (resourceProvider != null)
				{
					IResourceReader resourceReader = resourceProvider.ResourceReader;
					if (resourceReader != null)
					{
						ArrayList arrayList = new ArrayList();
						foreach (object obj in resourceReader)
						{
							arrayList.Add(((DictionaryEntry)obj).Key);
						}
						arrayList.Sort(StringComparer.CurrentCultureIgnoreCase);
						return arrayList;
					}
				}
				return null;
			}

			public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
			{
				if (context != null && context.Instance != null)
				{
					ResourceExpressionEditorSheet resourceExpressionEditorSheet = (ResourceExpressionEditorSheet)context.Instance;
					ICollection resourceKeys = ResourceExpressionEditorSheet.ResourceKeyTypeConverter.GetResourceKeys(resourceExpressionEditorSheet.ServiceProvider, resourceExpressionEditorSheet.ClassKey);
					if (resourceKeys != null && resourceKeys.Count > 0)
					{
						return new TypeConverter.StandardValuesCollection(resourceKeys);
					}
				}
				return base.GetStandardValues(context);
			}

			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
			{
				if (context != null && context.Instance != null)
				{
					ResourceExpressionEditorSheet resourceExpressionEditorSheet = (ResourceExpressionEditorSheet)context.Instance;
					ICollection resourceKeys = ResourceExpressionEditorSheet.ResourceKeyTypeConverter.GetResourceKeys(resourceExpressionEditorSheet.ServiceProvider, resourceExpressionEditorSheet.ClassKey);
					if (resourceKeys != null && resourceKeys.Count > 0)
					{
						return true;
					}
				}
				return base.GetStandardValuesSupported(context);
			}
		}
	}
}
