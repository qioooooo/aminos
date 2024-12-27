using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace System.Windows.Forms
{
	// Token: 0x0200047A RID: 1146
	public static class ListBindingHelper
	{
		// Token: 0x17000D36 RID: 3382
		// (get) Token: 0x06004352 RID: 17234 RVA: 0x000F0FC4 File Offset: 0x000EFFC4
		private static Attribute[] BrowsableAttributeList
		{
			get
			{
				if (ListBindingHelper.browsableAttribute == null)
				{
					ListBindingHelper.browsableAttribute = new Attribute[]
					{
						new BrowsableAttribute(true)
					};
				}
				return ListBindingHelper.browsableAttribute;
			}
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x000F0FF3 File Offset: 0x000EFFF3
		public static object GetList(object list)
		{
			if (list is IListSource)
			{
				return (list as IListSource).GetList();
			}
			return list;
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x000F100C File Offset: 0x000F000C
		public static object GetList(object dataSource, string dataMember)
		{
			dataSource = ListBindingHelper.GetList(dataSource);
			if (dataSource == null || dataSource is Type || string.IsNullOrEmpty(dataMember))
			{
				return dataSource;
			}
			PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(dataSource);
			PropertyDescriptor propertyDescriptor = listItemProperties.Find(dataMember, true);
			if (propertyDescriptor == null)
			{
				throw new ArgumentException(SR.GetString("DataSourceDataMemberPropNotFound", new object[] { dataMember }));
			}
			object obj;
			if (dataSource is ICurrencyManagerProvider)
			{
				CurrencyManager currencyManager = (dataSource as ICurrencyManagerProvider).CurrencyManager;
				obj = ((currencyManager != null && currencyManager.Position >= 0 && currencyManager.Position <= currencyManager.Count - 1) ? currencyManager.Current : null);
			}
			else if (dataSource is IEnumerable)
			{
				obj = ListBindingHelper.GetFirstItemByEnumerable(dataSource as IEnumerable);
			}
			else
			{
				obj = dataSource;
			}
			if (obj != null)
			{
				return propertyDescriptor.GetValue(obj);
			}
			return null;
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x000F10D4 File Offset: 0x000F00D4
		public static string GetListName(object list, PropertyDescriptor[] listAccessors)
		{
			if (list == null)
			{
				return string.Empty;
			}
			ITypedList typedList = list as ITypedList;
			string text;
			if (typedList != null)
			{
				text = typedList.GetListName(listAccessors);
			}
			else
			{
				Type type2;
				if (listAccessors == null || listAccessors.Length == 0)
				{
					Type type = list as Type;
					if (type != null)
					{
						type2 = type;
					}
					else
					{
						type2 = list.GetType();
					}
				}
				else
				{
					PropertyDescriptor propertyDescriptor = listAccessors[0];
					type2 = propertyDescriptor.PropertyType;
				}
				text = ListBindingHelper.GetListNameFromType(type2);
			}
			return text;
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x000F1134 File Offset: 0x000F0134
		public static PropertyDescriptorCollection GetListItemProperties(object list)
		{
			if (list == null)
			{
				return new PropertyDescriptorCollection(null);
			}
			PropertyDescriptorCollection propertyDescriptorCollection;
			if (list is Type)
			{
				propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByType(list as Type);
			}
			else
			{
				object list2 = ListBindingHelper.GetList(list);
				if (list2 is ITypedList)
				{
					propertyDescriptorCollection = (list2 as ITypedList).GetItemProperties(null);
				}
				else if (list2 is IEnumerable)
				{
					propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByEnumerable(list2 as IEnumerable);
				}
				else
				{
					propertyDescriptorCollection = TypeDescriptor.GetProperties(list2);
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x000F11A0 File Offset: 0x000F01A0
		public static PropertyDescriptorCollection GetListItemProperties(object list, PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection propertyDescriptorCollection;
			if (listAccessors == null || listAccessors.Length == 0)
			{
				propertyDescriptorCollection = ListBindingHelper.GetListItemProperties(list);
			}
			else if (list is Type)
			{
				propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByType(list as Type, listAccessors);
			}
			else
			{
				object list2 = ListBindingHelper.GetList(list);
				if (list2 is ITypedList)
				{
					propertyDescriptorCollection = (list2 as ITypedList).GetItemProperties(listAccessors);
				}
				else if (list2 is IEnumerable)
				{
					propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByEnumerable(list2 as IEnumerable, listAccessors);
				}
				else
				{
					propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByInstance(list2, listAccessors, 0);
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x06004358 RID: 17240 RVA: 0x000F1214 File Offset: 0x000F0214
		public static PropertyDescriptorCollection GetListItemProperties(object dataSource, string dataMember, PropertyDescriptor[] listAccessors)
		{
			dataSource = ListBindingHelper.GetList(dataSource);
			if (!string.IsNullOrEmpty(dataMember))
			{
				PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(dataSource);
				PropertyDescriptor propertyDescriptor = listItemProperties.Find(dataMember, true);
				if (propertyDescriptor == null)
				{
					throw new ArgumentException(SR.GetString("DataSourceDataMemberPropNotFound", new object[] { dataMember }));
				}
				int num = ((listAccessors == null) ? 1 : (listAccessors.Length + 1));
				PropertyDescriptor[] array = new PropertyDescriptor[num];
				array[0] = propertyDescriptor;
				for (int i = 1; i < num; i++)
				{
					array[i] = listAccessors[i - 1];
				}
				listAccessors = array;
			}
			return ListBindingHelper.GetListItemProperties(dataSource, listAccessors);
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x000F12A0 File Offset: 0x000F02A0
		public static Type GetListItemType(object list)
		{
			if (list == null)
			{
				return null;
			}
			Type type = null;
			Type type2 = ((list is Type) ? (list as Type) : list.GetType());
			object obj = ((list is Type) ? null : list);
			if (typeof(Array).IsAssignableFrom(type2))
			{
				type = type2.GetElementType();
			}
			else
			{
				PropertyInfo typedIndexer = ListBindingHelper.GetTypedIndexer(type2);
				if (typedIndexer != null)
				{
					type = typedIndexer.PropertyType;
				}
				else if (obj is IEnumerable)
				{
					type = ListBindingHelper.GetListItemTypeByEnumerable(obj as IEnumerable);
				}
				if ((type == null || type == typeof(object)) && typeof(IEnumerable).IsAssignableFrom(type2))
				{
					MethodInfo method = type2.GetMethod("GetEnumerator", Type.EmptyTypes);
					if (method != null && typeof(IEnumerator).IsAssignableFrom(method.ReturnType))
					{
						PropertyInfo property = method.ReturnType.GetProperty("Current");
						if (property != null)
						{
							type = property.PropertyType;
						}
					}
				}
				if (type == null)
				{
					type = type2;
				}
			}
			return type;
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x000F1394 File Offset: 0x000F0394
		public static Type GetListItemType(object dataSource, string dataMember)
		{
			if (dataSource == null)
			{
				return typeof(object);
			}
			if (string.IsNullOrEmpty(dataMember))
			{
				return ListBindingHelper.GetListItemType(dataSource);
			}
			PropertyDescriptorCollection listItemProperties = ListBindingHelper.GetListItemProperties(dataSource);
			if (listItemProperties == null)
			{
				return typeof(object);
			}
			PropertyDescriptor propertyDescriptor = listItemProperties.Find(dataMember, true);
			if (propertyDescriptor == null || propertyDescriptor.PropertyType is ICustomTypeDescriptor)
			{
				return typeof(object);
			}
			return ListBindingHelper.GetListItemType(propertyDescriptor.PropertyType);
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x000F1404 File Offset: 0x000F0404
		private static string GetListNameFromType(Type type)
		{
			string text;
			if (typeof(Array).IsAssignableFrom(type))
			{
				text = type.GetElementType().Name;
			}
			else if (typeof(IList).IsAssignableFrom(type))
			{
				PropertyInfo typedIndexer = ListBindingHelper.GetTypedIndexer(type);
				if (typedIndexer != null)
				{
					text = typedIndexer.PropertyType.Name;
				}
				else
				{
					text = type.Name;
				}
			}
			else
			{
				text = type.Name;
			}
			return text;
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x000F146C File Offset: 0x000F046C
		private static PropertyDescriptorCollection GetListItemPropertiesByType(Type type, PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection propertyDescriptorCollection;
			if (listAccessors == null || listAccessors.Length == 0)
			{
				propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByType(type);
			}
			else
			{
				propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByType(type, listAccessors, 0);
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x000F1498 File Offset: 0x000F0498
		private static PropertyDescriptorCollection GetListItemPropertiesByType(Type type, PropertyDescriptor[] listAccessors, int startIndex)
		{
			Type propertyType = listAccessors[startIndex].PropertyType;
			startIndex++;
			PropertyDescriptorCollection propertyDescriptorCollection;
			if (startIndex >= listAccessors.Length)
			{
				propertyDescriptorCollection = ListBindingHelper.GetListItemProperties(propertyType);
			}
			else
			{
				propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByType(propertyType, listAccessors, startIndex);
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x000F14D0 File Offset: 0x000F04D0
		private static PropertyDescriptorCollection GetListItemPropertiesByEnumerable(IEnumerable iEnumerable, PropertyDescriptor[] listAccessors, int startIndex)
		{
			object obj = null;
			object firstItemByEnumerable = ListBindingHelper.GetFirstItemByEnumerable(iEnumerable);
			if (firstItemByEnumerable != null)
			{
				obj = ListBindingHelper.GetList(listAccessors[startIndex].GetValue(firstItemByEnumerable));
			}
			PropertyDescriptorCollection propertyDescriptorCollection;
			if (obj == null)
			{
				propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByType(listAccessors[startIndex].PropertyType, listAccessors, startIndex);
			}
			else
			{
				startIndex++;
				IEnumerable enumerable = obj as IEnumerable;
				if (enumerable != null)
				{
					if (startIndex == listAccessors.Length)
					{
						propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByEnumerable(enumerable);
					}
					else
					{
						propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByEnumerable(enumerable, listAccessors, startIndex);
					}
				}
				else
				{
					propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByInstance(obj, listAccessors, startIndex);
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x000F1544 File Offset: 0x000F0544
		private static PropertyDescriptorCollection GetListItemPropertiesByEnumerable(IEnumerable enumerable, PropertyDescriptor[] listAccessors)
		{
			PropertyDescriptorCollection propertyDescriptorCollection;
			if (listAccessors == null || listAccessors.Length == 0)
			{
				propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByEnumerable(enumerable);
			}
			else
			{
				ITypedList typedList = enumerable as ITypedList;
				if (typedList != null)
				{
					propertyDescriptorCollection = typedList.GetItemProperties(listAccessors);
				}
				else
				{
					propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByEnumerable(enumerable, listAccessors, 0);
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x000F1584 File Offset: 0x000F0584
		private static Type GetListItemTypeByEnumerable(IEnumerable iEnumerable)
		{
			object firstItemByEnumerable = ListBindingHelper.GetFirstItemByEnumerable(iEnumerable);
			if (firstItemByEnumerable == null)
			{
				return typeof(object);
			}
			return firstItemByEnumerable.GetType();
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x000F15AC File Offset: 0x000F05AC
		private static PropertyDescriptorCollection GetListItemPropertiesByInstance(object target, PropertyDescriptor[] listAccessors, int startIndex)
		{
			PropertyDescriptorCollection propertyDescriptorCollection;
			if (listAccessors != null && listAccessors.Length > startIndex)
			{
				object value = listAccessors[startIndex].GetValue(target);
				if (value == null)
				{
					propertyDescriptorCollection = ListBindingHelper.GetListItemPropertiesByType(listAccessors[startIndex].PropertyType);
				}
				else
				{
					PropertyDescriptor[] array = null;
					if (listAccessors.Length > startIndex + 1)
					{
						int num = listAccessors.Length - (startIndex + 1);
						array = new PropertyDescriptor[num];
						for (int i = 0; i < num; i++)
						{
							array[i] = listAccessors[startIndex + 1 + i];
						}
					}
					propertyDescriptorCollection = ListBindingHelper.GetListItemProperties(value, array);
				}
			}
			else
			{
				propertyDescriptorCollection = TypeDescriptor.GetProperties(target, ListBindingHelper.BrowsableAttributeList);
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x000F162C File Offset: 0x000F062C
		private static PropertyInfo GetTypedIndexer(Type type)
		{
			if (!typeof(IList).IsAssignableFrom(type) && !typeof(ITypedList).IsAssignableFrom(type) && !typeof(IListSource).IsAssignableFrom(type))
			{
				return null;
			}
			PropertyInfo propertyInfo = null;
			PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < properties.Length; i++)
			{
				if (properties[i].GetIndexParameters().Length > 0 && properties[i].PropertyType != typeof(object))
				{
					propertyInfo = properties[i];
					if (propertyInfo.Name == "Item")
					{
						break;
					}
				}
			}
			return propertyInfo;
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x000F16C2 File Offset: 0x000F06C2
		private static PropertyDescriptorCollection GetListItemPropertiesByType(Type type)
		{
			return TypeDescriptor.GetProperties(ListBindingHelper.GetListItemType(type), ListBindingHelper.BrowsableAttributeList);
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x000F16D4 File Offset: 0x000F06D4
		private static PropertyDescriptorCollection GetListItemPropertiesByEnumerable(IEnumerable enumerable)
		{
			PropertyDescriptorCollection propertyDescriptorCollection = null;
			Type type = enumerable.GetType();
			if (typeof(Array).IsAssignableFrom(type))
			{
				propertyDescriptorCollection = TypeDescriptor.GetProperties(type.GetElementType(), ListBindingHelper.BrowsableAttributeList);
			}
			else
			{
				ITypedList typedList = enumerable as ITypedList;
				if (typedList != null)
				{
					propertyDescriptorCollection = typedList.GetItemProperties(null);
				}
				else
				{
					PropertyInfo typedIndexer = ListBindingHelper.GetTypedIndexer(type);
					if (typedIndexer != null && !typeof(ICustomTypeDescriptor).IsAssignableFrom(typedIndexer.PropertyType))
					{
						propertyDescriptorCollection = TypeDescriptor.GetProperties(typedIndexer.PropertyType, ListBindingHelper.BrowsableAttributeList);
					}
				}
			}
			if (propertyDescriptorCollection == null)
			{
				object firstItemByEnumerable = ListBindingHelper.GetFirstItemByEnumerable(enumerable);
				if (enumerable is string)
				{
					propertyDescriptorCollection = TypeDescriptor.GetProperties(enumerable, ListBindingHelper.BrowsableAttributeList);
				}
				else if (firstItemByEnumerable == null)
				{
					propertyDescriptorCollection = new PropertyDescriptorCollection(null);
				}
				else
				{
					propertyDescriptorCollection = TypeDescriptor.GetProperties(firstItemByEnumerable, ListBindingHelper.BrowsableAttributeList);
					if (!(enumerable is IList) && (propertyDescriptorCollection == null || propertyDescriptorCollection.Count == 0))
					{
						propertyDescriptorCollection = TypeDescriptor.GetProperties(enumerable, ListBindingHelper.BrowsableAttributeList);
					}
				}
			}
			return propertyDescriptorCollection;
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x000F17B0 File Offset: 0x000F07B0
		private static object GetFirstItemByEnumerable(IEnumerable enumerable)
		{
			object obj = null;
			if (enumerable is IList)
			{
				IList list = enumerable as IList;
				obj = ((list.Count > 0) ? list[0] : null);
			}
			else
			{
				try
				{
					IEnumerator enumerator = enumerable.GetEnumerator();
					enumerator.Reset();
					if (enumerator.MoveNext())
					{
						obj = enumerator.Current;
					}
					enumerator.Reset();
				}
				catch (NotSupportedException)
				{
					obj = null;
				}
			}
			return obj;
		}

		// Token: 0x040020DA RID: 8410
		private static Attribute[] browsableAttribute;
	}
}
