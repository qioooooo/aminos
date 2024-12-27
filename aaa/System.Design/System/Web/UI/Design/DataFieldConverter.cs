using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.Design.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x0200034B RID: 843
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataFieldConverter : TypeConverter
	{
		// Token: 0x06001FB8 RID: 8120 RVA: 0x000B569C File Offset: 0x000B469C
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000B56AE File Offset: 0x000B46AE
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value == null)
			{
				return string.Empty;
			}
			if (value.GetType() == typeof(string))
			{
				return (string)value;
			}
			throw base.GetConvertFromException(value);
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000B56DC File Offset: 0x000B46DC
		private DesignerDataSourceView GetView(IDesigner dataBoundControlDesigner)
		{
			DataBoundControlDesigner dataBoundControlDesigner2 = dataBoundControlDesigner as DataBoundControlDesigner;
			if (dataBoundControlDesigner2 != null)
			{
				return dataBoundControlDesigner2.DesignerView;
			}
			BaseDataListDesigner baseDataListDesigner = dataBoundControlDesigner as BaseDataListDesigner;
			if (baseDataListDesigner != null)
			{
				return baseDataListDesigner.DesignerView;
			}
			RepeaterDesigner repeaterDesigner = dataBoundControlDesigner as RepeaterDesigner;
			if (repeaterDesigner != null)
			{
				return repeaterDesigner.DesignerView;
			}
			return null;
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x000B5720 File Offset: 0x000B4720
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			object[] array = null;
			if (context != null)
			{
				IComponent component = context.Instance as IComponent;
				if (component != null)
				{
					ISite site = component.Site;
					if (site != null)
					{
						IDesignerHost designerHost = (IDesignerHost)site.GetService(typeof(IDesignerHost));
						if (designerHost != null)
						{
							IDesigner designer = designerHost.GetDesigner(component);
							DesignerDataSourceView view = this.GetView(designer);
							if (view != null)
							{
								IDataSourceViewSchema dataSourceViewSchema = null;
								try
								{
									dataSourceViewSchema = view.Schema;
								}
								catch (Exception ex)
								{
									IComponentDesignerDebugService componentDesignerDebugService = (IComponentDesignerDebugService)site.GetService(typeof(IComponentDesignerDebugService));
									if (componentDesignerDebugService != null)
									{
										componentDesignerDebugService.Fail(SR.GetString("DataSource_DebugService_FailedCall", new object[] { "DesignerDataSourceView.Schema", ex.Message }));
									}
								}
								if (dataSourceViewSchema != null)
								{
									IDataSourceFieldSchema[] fields = dataSourceViewSchema.GetFields();
									if (fields != null)
									{
										array = new object[fields.Length];
										for (int i = 0; i < fields.Length; i++)
										{
											array[i] = fields[i].Name;
										}
									}
								}
							}
							if (array == null && designer != null && designer is IDataSourceProvider)
							{
								IDataSourceProvider dataSourceProvider = designer as IDataSourceProvider;
								IEnumerable enumerable = null;
								if (dataSourceProvider != null)
								{
									enumerable = dataSourceProvider.GetResolvedSelectedDataSource();
								}
								if (enumerable != null)
								{
									PropertyDescriptorCollection dataFields = DesignTimeData.GetDataFields(enumerable);
									if (dataFields != null)
									{
										ArrayList arrayList = new ArrayList();
										foreach (object obj in dataFields)
										{
											PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
											arrayList.Add(propertyDescriptor.Name);
										}
										array = arrayList.ToArray();
									}
								}
							}
						}
					}
				}
			}
			return new TypeConverter.StandardValuesCollection(array);
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x000B58E0 File Offset: 0x000B48E0
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x000B58E3 File Offset: 0x000B48E3
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return context != null && context.Instance is IComponent;
		}
	}
}
