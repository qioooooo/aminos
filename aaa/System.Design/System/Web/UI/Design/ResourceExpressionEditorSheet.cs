using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Resources;
using System.Web.Compilation;

namespace System.Web.UI.Design
{
	// Token: 0x0200038C RID: 908
	public class ResourceExpressionEditorSheet : ExpressionEditorSheet
	{
		// Token: 0x06002173 RID: 8563 RVA: 0x000B98F0 File Offset: 0x000B88F0
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

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x06002174 RID: 8564 RVA: 0x000B992B File Offset: 0x000B892B
		// (set) Token: 0x06002175 RID: 8565 RVA: 0x000B9941 File Offset: 0x000B8941
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

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x06002176 RID: 8566 RVA: 0x000B994A File Offset: 0x000B894A
		public override bool IsValid
		{
			get
			{
				return !string.IsNullOrEmpty(this.ResourceKey);
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06002177 RID: 8567 RVA: 0x000B995A File Offset: 0x000B895A
		// (set) Token: 0x06002178 RID: 8568 RVA: 0x000B9970 File Offset: 0x000B8970
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

		// Token: 0x06002179 RID: 8569 RVA: 0x000B9979 File Offset: 0x000B8979
		public override string GetExpression()
		{
			string empty = string.Empty;
			if (!string.IsNullOrEmpty(this._classKey))
			{
				return this._classKey + ", " + this._resourceKey;
			}
			return this._resourceKey;
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000B99AC File Offset: 0x000B89AC
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

		// Token: 0x04001814 RID: 6164
		private string _classKey;

		// Token: 0x04001815 RID: 6165
		private string _resourceKey;

		// Token: 0x0200038D RID: 909
		internal class ResourceExpressionFields
		{
			// Token: 0x04001816 RID: 6166
			internal string ClassKey;

			// Token: 0x04001817 RID: 6167
			internal string ResourceKey;
		}

		// Token: 0x0200038E RID: 910
		private class ResourceKeyTypeConverter : StringConverter
		{
			// Token: 0x0600217C RID: 8572 RVA: 0x000B9A20 File Offset: 0x000B8A20
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

			// Token: 0x0600217D RID: 8573 RVA: 0x000B9AC8 File Offset: 0x000B8AC8
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

			// Token: 0x0600217E RID: 8574 RVA: 0x000B9B18 File Offset: 0x000B8B18
			public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
			{
				return false;
			}

			// Token: 0x0600217F RID: 8575 RVA: 0x000B9B1C File Offset: 0x000B8B1C
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
