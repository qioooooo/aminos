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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataFieldConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

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

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return context != null && context.Instance is IComponent;
		}
	}
}
