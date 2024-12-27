using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	// Token: 0x0200034A RID: 842
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DataColumnSelectionConverter : TypeConverter
	{
		// Token: 0x06001FB2 RID: 8114 RVA: 0x000B5506 File Offset: 0x000B4506
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x000B5518 File Offset: 0x000B4518
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

		// Token: 0x06001FB4 RID: 8116 RVA: 0x000B5544 File Offset: 0x000B4544
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			string[] array = null;
			ArrayList arrayList = new ArrayList();
			if (context != null)
			{
				IComponent component = context.Instance as IComponent;
				if (component != null)
				{
					GridView gridView = component as GridView;
					if (gridView != null)
					{
						if (gridView.AutoGenerateColumns)
						{
							DataFieldConverter dataFieldConverter = new DataFieldConverter();
							TypeConverter.StandardValuesCollection standardValues = dataFieldConverter.GetStandardValues(context);
							foreach (object obj in standardValues)
							{
								arrayList.Add(obj);
							}
						}
						DataControlFieldCollection columns = gridView.Columns;
						foreach (object obj2 in columns)
						{
							DataControlField dataControlField = (DataControlField)obj2;
							BoundField boundField = dataControlField as BoundField;
							if (boundField != null)
							{
								string dataField = boundField.DataField;
								if (!arrayList.Contains(dataField))
								{
									arrayList.Add(dataField);
								}
							}
						}
						arrayList.Sort();
						array = new string[arrayList.Count];
						arrayList.CopyTo(array, 0);
					}
				}
			}
			return new TypeConverter.StandardValuesCollection(array);
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x000B567C File Offset: 0x000B467C
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x000B567F File Offset: 0x000B467F
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return context != null && context.Instance is IComponent;
		}
	}
}
