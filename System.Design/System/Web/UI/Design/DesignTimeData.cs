using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Design;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class DesignTimeData
	{
		private DesignTimeData()
		{
		}

		public static DataTable CreateDummyDataTable()
		{
			DataTable dataTable = new DataTable();
			dataTable.Locale = CultureInfo.InvariantCulture;
			DataColumnCollection columns = dataTable.Columns;
			columns.Add(SR.GetString("Sample_Column", new object[] { 0 }), typeof(string));
			columns.Add(SR.GetString("Sample_Column", new object[] { 1 }), typeof(string));
			columns.Add(SR.GetString("Sample_Column", new object[] { 2 }), typeof(string));
			return dataTable;
		}

		public static DataTable CreateDummyDataBoundDataTable()
		{
			DataTable dataTable = new DataTable();
			dataTable.Locale = CultureInfo.InvariantCulture;
			DataColumnCollection columns = dataTable.Columns;
			columns.Add(SR.GetString("Sample_Databound_Column", new object[] { 0 }), typeof(string));
			columns.Add(SR.GetString("Sample_Databound_Column", new object[] { 1 }), typeof(int));
			columns.Add(SR.GetString("Sample_Databound_Column", new object[] { 2 }), typeof(string));
			return dataTable;
		}

		public static DataTable CreateSampleDataTable(IEnumerable referenceData)
		{
			return DesignTimeData.CreateSampleDataTableInternal(referenceData, false);
		}

		public static DataTable CreateSampleDataTable(IEnumerable referenceData, bool useDataBoundData)
		{
			return DesignTimeData.CreateSampleDataTableInternal(referenceData, useDataBoundData);
		}

		private static DataTable CreateSampleDataTableInternal(IEnumerable referenceData, bool useDataBoundData)
		{
			DataTable dataTable = new DataTable();
			dataTable.Locale = CultureInfo.InvariantCulture;
			DataColumnCollection columns = dataTable.Columns;
			PropertyDescriptorCollection dataFields = DesignTimeData.GetDataFields(referenceData);
			if (dataFields != null)
			{
				foreach (object obj in dataFields)
				{
					PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
					Type type = propertyDescriptor.PropertyType;
					if (!type.IsPrimitive && type != typeof(DateTime) && type != typeof(decimal) && type != typeof(DateTimeOffset) && type != typeof(TimeSpan))
					{
						type = typeof(string);
					}
					columns.Add(propertyDescriptor.Name, type);
				}
			}
			if (columns.Count != 0)
			{
				return dataTable;
			}
			if (useDataBoundData)
			{
				return DesignTimeData.CreateDummyDataBoundDataTable();
			}
			return DesignTimeData.CreateDummyDataTable();
		}

		public static PropertyDescriptorCollection GetDataFields(IEnumerable dataSource)
		{
			if (dataSource is ITypedList)
			{
				return ((ITypedList)dataSource).GetItemProperties(new PropertyDescriptor[0]);
			}
			Type type = dataSource.GetType();
			PropertyInfo property = type.GetProperty("Item", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, null, new Type[] { typeof(int) }, null);
			if (property != null && property.PropertyType != typeof(object))
			{
				return TypeDescriptor.GetProperties(property.PropertyType);
			}
			return null;
		}

		public static string[] GetDataMembers(object dataSource)
		{
			IListSource listSource = dataSource as IListSource;
			if (listSource != null && listSource.ContainsListCollection)
			{
				IList list = ((IListSource)dataSource).GetList();
				ITypedList typedList = list as ITypedList;
				if (typedList != null)
				{
					PropertyDescriptorCollection itemProperties = typedList.GetItemProperties(new PropertyDescriptor[0]);
					if (itemProperties != null)
					{
						ArrayList arrayList = new ArrayList(itemProperties.Count);
						foreach (object obj in itemProperties)
						{
							PropertyDescriptor propertyDescriptor = (PropertyDescriptor)obj;
							arrayList.Add(propertyDescriptor.Name);
						}
						return (string[])arrayList.ToArray(typeof(string));
					}
				}
			}
			return null;
		}

		public static IEnumerable GetDataMember(IListSource dataSource, string dataMember)
		{
			IEnumerable enumerable = null;
			IList list = dataSource.GetList();
			if (list != null && list is ITypedList)
			{
				if (!dataSource.ContainsListCollection)
				{
					if (dataMember != null && dataMember.Length != 0)
					{
						throw new ArgumentException(SR.GetString("DesignTimeData_BadDataMember"));
					}
					enumerable = list;
				}
				else
				{
					ITypedList typedList = (ITypedList)list;
					PropertyDescriptorCollection itemProperties = typedList.GetItemProperties(new PropertyDescriptor[0]);
					if (itemProperties != null && itemProperties.Count != 0)
					{
						PropertyDescriptor propertyDescriptor;
						if (dataMember == null || dataMember.Length == 0)
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
								enumerable = (IEnumerable)value;
							}
						}
					}
				}
			}
			return enumerable;
		}

		public static IEnumerable GetDesignTimeDataSource(DataTable dataTable, int minimumRows)
		{
			int count = dataTable.Rows.Count;
			if (count < minimumRows)
			{
				int num = minimumRows - count;
				DataRowCollection rows = dataTable.Rows;
				DataColumnCollection columns = dataTable.Columns;
				int count2 = columns.Count;
				for (int i = 0; i < num; i++)
				{
					DataRow dataRow = dataTable.NewRow();
					int num2 = count + i;
					for (int j = 0; j < count2; j++)
					{
						Type dataType = columns[j].DataType;
						object obj;
						if (dataType == typeof(string))
						{
							obj = SR.GetString("Sample_Databound_Text_Alt");
						}
						else if (dataType == typeof(int) || dataType == typeof(short) || dataType == typeof(long) || dataType == typeof(uint) || dataType == typeof(ushort) || dataType == typeof(ulong))
						{
							obj = num2;
						}
						else if (dataType == typeof(byte) || dataType == typeof(sbyte))
						{
							obj = ((num2 % 2 != 0) ? 1 : 0);
						}
						else if (dataType == typeof(bool))
						{
							obj = num2 % 2 != 0;
						}
						else if (dataType == typeof(DateTime))
						{
							obj = DateTime.Today;
						}
						else if (dataType == typeof(double) || dataType == typeof(float) || dataType == typeof(decimal))
						{
							obj = (double)i / 10.0;
						}
						else if (dataType == typeof(char))
						{
							obj = 'x';
						}
						else if (dataType == typeof(TimeSpan))
						{
							obj = TimeSpan.Zero;
						}
						else if (dataType == typeof(DateTimeOffset))
						{
							obj = DateTimeOffset.Now;
						}
						else
						{
							obj = DBNull.Value;
						}
						dataRow[j] = obj;
					}
					rows.Add(dataRow);
				}
			}
			return new DataView(dataTable);
		}

		public static object GetSelectedDataSource(IComponent component, string dataSource)
		{
			object obj = null;
			ISite site = component.Site;
			if (site != null)
			{
				IContainer container = (IContainer)site.GetService(typeof(IContainer));
				if (container != null)
				{
					IComponent component2 = container.Components[dataSource];
					if (component2 is IEnumerable || component2 is IListSource)
					{
						obj = component2;
					}
				}
			}
			return obj;
		}

		public static IEnumerable GetSelectedDataSource(IComponent component, string dataSource, string dataMember)
		{
			IEnumerable enumerable = null;
			object selectedDataSource = DesignTimeData.GetSelectedDataSource(component, dataSource);
			if (selectedDataSource != null)
			{
				IListSource listSource = selectedDataSource as IListSource;
				if (listSource != null)
				{
					if (!listSource.ContainsListCollection)
					{
						enumerable = listSource.GetList();
					}
					else
					{
						enumerable = DesignTimeData.GetDataMember(listSource, dataMember);
					}
				}
				else
				{
					enumerable = (IEnumerable)selectedDataSource;
				}
			}
			return enumerable;
		}

		public static readonly EventHandler DataBindingHandler = new EventHandler(GlobalDataBindingHandler.OnDataBind);
	}
}
