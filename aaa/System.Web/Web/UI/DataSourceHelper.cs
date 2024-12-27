using System;
using System.Collections;
using System.ComponentModel;

namespace System.Web.UI
{
	// Token: 0x020003DF RID: 991
	internal sealed class DataSourceHelper
	{
		// Token: 0x06003024 RID: 12324 RVA: 0x000D4DD3 File Offset: 0x000D3DD3
		private DataSourceHelper()
		{
		}

		// Token: 0x06003025 RID: 12325 RVA: 0x000D4DDC File Offset: 0x000D3DDC
		internal static IEnumerable GetResolvedDataSource(object dataSource, string dataMember)
		{
			if (dataSource == null)
			{
				return null;
			}
			IListSource listSource = dataSource as IListSource;
			if (listSource != null)
			{
				IList list = listSource.GetList();
				if (!listSource.ContainsListCollection)
				{
					return list;
				}
				if (list != null && list is ITypedList)
				{
					ITypedList typedList = (ITypedList)list;
					PropertyDescriptorCollection itemProperties = typedList.GetItemProperties(new PropertyDescriptor[0]);
					if (itemProperties != null && itemProperties.Count != 0)
					{
						PropertyDescriptor propertyDescriptor;
						if (string.IsNullOrEmpty(dataMember))
						{
							propertyDescriptor = itemProperties[0];
						}
						else
						{
							propertyDescriptor = itemProperties.Find(dataMember, true);
						}
						if (propertyDescriptor != null)
						{
							object obj = list[0];
							object value = propertyDescriptor.GetValue(obj);
							if (value != null && value is IEnumerable)
							{
								return (IEnumerable)value;
							}
						}
						throw new HttpException(SR.GetString("ListSource_Missing_DataMember", new object[] { dataMember }));
					}
					throw new HttpException(SR.GetString("ListSource_Without_DataMembers"));
				}
			}
			if (dataSource is IEnumerable)
			{
				return (IEnumerable)dataSource;
			}
			return null;
		}
	}
}
