using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.Design.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x0200034C RID: 844
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataMemberConverter : TypeConverter
	{
		// Token: 0x06001FBF RID: 8127 RVA: 0x000B5900 File Offset: 0x000B4900
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x000B5912 File Offset: 0x000B4912
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

		// Token: 0x06001FC1 RID: 8129 RVA: 0x000B5940 File Offset: 0x000B4940
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

		// Token: 0x06001FC2 RID: 8130 RVA: 0x000B5984 File Offset: 0x000B4984
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			string[] array = null;
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
								IDataSourceDesigner dataSourceDesigner = view.DataSourceDesigner;
								if (dataSourceDesigner != null)
								{
									string[] viewNames = dataSourceDesigner.GetViewNames();
									if (viewNames != null)
									{
										array = new string[viewNames.Length];
										viewNames.CopyTo(array, 0);
									}
								}
							}
							if (array == null && designer != null && designer is IDataSourceProvider)
							{
								IDataSourceProvider dataSourceProvider = designer as IDataSourceProvider;
								object obj = null;
								if (dataSourceProvider != null)
								{
									obj = dataSourceProvider.GetSelectedDataSource();
								}
								if (obj != null)
								{
									array = DesignTimeData.GetDataMembers(obj);
								}
							}
						}
					}
				}
				if (array == null)
				{
					array = new string[0];
				}
				Array.Sort(array, Comparer.Default);
			}
			return new TypeConverter.StandardValuesCollection(array);
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x000B5A65 File Offset: 0x000B4A65
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000B5A68 File Offset: 0x000B4A68
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return context != null && context.Instance is IComponent;
		}
	}
}
