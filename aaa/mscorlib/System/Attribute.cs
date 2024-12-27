using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000061 RID: 97
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_Attribute))]
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = false)]
	[Serializable]
	public abstract class Attribute : _Attribute
	{
		// Token: 0x060005C3 RID: 1475 RVA: 0x00014090 File Offset: 0x00013090
		private static Attribute[] InternalGetCustomAttributes(PropertyInfo element, Type type, bool inherit)
		{
			Attribute[] array = (Attribute[])element.GetCustomAttributes(type, inherit);
			if (!inherit)
			{
				return array;
			}
			Hashtable hashtable = new Hashtable(11);
			ArrayList arrayList = new ArrayList();
			Attribute.CopyToArrayList(arrayList, array, hashtable);
			for (PropertyInfo propertyInfo = Attribute.GetParentDefinition(element); propertyInfo != null; propertyInfo = Attribute.GetParentDefinition(propertyInfo))
			{
				array = Attribute.GetCustomAttributes(propertyInfo, type, false);
				Attribute.AddAttributesToList(arrayList, array, hashtable);
			}
			return (Attribute[])arrayList.ToArray(type);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x000140F8 File Offset: 0x000130F8
		private static bool InternalIsDefined(PropertyInfo element, Type attributeType, bool inherit)
		{
			if (element.IsDefined(attributeType, inherit))
			{
				return true;
			}
			if (inherit)
			{
				AttributeUsageAttribute attributeUsageAttribute = Attribute.InternalGetAttributeUsage(attributeType);
				if (!attributeUsageAttribute.Inherited)
				{
					return false;
				}
				for (PropertyInfo propertyInfo = Attribute.GetParentDefinition(element); propertyInfo != null; propertyInfo = Attribute.GetParentDefinition(propertyInfo))
				{
					if (propertyInfo.IsDefined(attributeType, false))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00014148 File Offset: 0x00013148
		private static PropertyInfo GetParentDefinition(PropertyInfo property)
		{
			MethodInfo methodInfo = property.GetGetMethod(true);
			if (methodInfo == null)
			{
				methodInfo = property.GetSetMethod(true);
			}
			if (methodInfo != null)
			{
				methodInfo = methodInfo.GetParentDefinition();
				if (methodInfo != null)
				{
					return methodInfo.DeclaringType.GetProperty(property.Name, property.PropertyType);
				}
			}
			return null;
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x00014190 File Offset: 0x00013190
		private static Attribute[] InternalGetCustomAttributes(EventInfo element, Type type, bool inherit)
		{
			Attribute[] array = (Attribute[])element.GetCustomAttributes(type, inherit);
			if (inherit)
			{
				Hashtable hashtable = new Hashtable(11);
				ArrayList arrayList = new ArrayList();
				Attribute.CopyToArrayList(arrayList, array, hashtable);
				for (EventInfo eventInfo = Attribute.GetParentDefinition(element); eventInfo != null; eventInfo = Attribute.GetParentDefinition(eventInfo))
				{
					array = Attribute.GetCustomAttributes(eventInfo, type, false);
					Attribute.AddAttributesToList(arrayList, array, hashtable);
				}
				return (Attribute[])arrayList.ToArray(type);
			}
			return array;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x000141F8 File Offset: 0x000131F8
		private static EventInfo GetParentDefinition(EventInfo ev)
		{
			MethodInfo methodInfo = ev.GetAddMethod(true);
			if (methodInfo != null)
			{
				methodInfo = methodInfo.GetParentDefinition();
				if (methodInfo != null)
				{
					return methodInfo.DeclaringType.GetEvent(ev.Name);
				}
			}
			return null;
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00014230 File Offset: 0x00013230
		private static bool InternalIsDefined(EventInfo element, Type attributeType, bool inherit)
		{
			if (element.IsDefined(attributeType, inherit))
			{
				return true;
			}
			if (inherit)
			{
				AttributeUsageAttribute attributeUsageAttribute = Attribute.InternalGetAttributeUsage(attributeType);
				if (!attributeUsageAttribute.Inherited)
				{
					return false;
				}
				for (EventInfo eventInfo = Attribute.GetParentDefinition(element); eventInfo != null; eventInfo = Attribute.GetParentDefinition(eventInfo))
				{
					if (eventInfo.IsDefined(attributeType, false))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00014280 File Offset: 0x00013280
		private static Attribute[] InternalParamGetCustomAttributes(MethodInfo method, ParameterInfo param, Type type, bool inherit)
		{
			ArrayList arrayList = new ArrayList();
			if (type == null)
			{
				type = typeof(Attribute);
			}
			object[] array = param.GetCustomAttributes(type, false);
			for (int i = 0; i < array.Length; i++)
			{
				Type type2 = array[i].GetType();
				AttributeUsageAttribute attributeUsageAttribute = Attribute.InternalGetAttributeUsage(type2);
				if (!attributeUsageAttribute.AllowMultiple)
				{
					arrayList.Add(type2);
				}
			}
			Attribute[] array2;
			if (array.Length == 0)
			{
				array2 = (Attribute[])Array.CreateInstance(type, 0);
			}
			else
			{
				array2 = (Attribute[])array;
			}
			if (method.DeclaringType == null)
			{
				return array2;
			}
			if (!inherit)
			{
				return array2;
			}
			int position = param.Position;
			for (method = method.GetParentDefinition(); method != null; method = method.GetParentDefinition())
			{
				ParameterInfo[] parameters = method.GetParameters();
				param = parameters[position];
				array = param.GetCustomAttributes(type, false);
				int num = 0;
				for (int j = 0; j < array.Length; j++)
				{
					Type type3 = array[j].GetType();
					AttributeUsageAttribute attributeUsageAttribute2 = Attribute.InternalGetAttributeUsage(type3);
					if (attributeUsageAttribute2.Inherited && !arrayList.Contains(type3))
					{
						if (!attributeUsageAttribute2.AllowMultiple)
						{
							arrayList.Add(type3);
						}
						num++;
					}
					else
					{
						array[j] = null;
					}
				}
				Attribute[] array3 = (Attribute[])Array.CreateInstance(type, num);
				num = 0;
				for (int k = 0; k < array.Length; k++)
				{
					if (array[k] != null)
					{
						array3[num] = (Attribute)array[k];
						num++;
					}
				}
				Attribute[] array4 = array2;
				array2 = (Attribute[])Array.CreateInstance(type, array4.Length + num);
				Array.Copy(array4, array2, array4.Length);
				int num2 = array4.Length;
				for (int l = 0; l < array3.Length; l++)
				{
					array2[num2 + l] = array3[l];
				}
			}
			return array2;
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00014430 File Offset: 0x00013430
		private static bool InternalParamIsDefined(MethodInfo method, ParameterInfo param, Type type, bool inherit)
		{
			if (param.IsDefined(type, false))
			{
				return true;
			}
			if (method.DeclaringType == null || !inherit)
			{
				return false;
			}
			int position = param.Position;
			for (method = method.GetParentDefinition(); method != null; method = method.GetParentDefinition())
			{
				ParameterInfo[] parameters = method.GetParameters();
				param = parameters[position];
				object[] customAttributes = param.GetCustomAttributes(type, false);
				for (int i = 0; i < customAttributes.Length; i++)
				{
					Type type2 = customAttributes[i].GetType();
					AttributeUsageAttribute attributeUsageAttribute = Attribute.InternalGetAttributeUsage(type2);
					if (customAttributes[i] is Attribute && attributeUsageAttribute.Inherited)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000144C0 File Offset: 0x000134C0
		private static void CopyToArrayList(ArrayList attributeList, Attribute[] attributes, Hashtable types)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				attributeList.Add(attributes[i]);
				Type type = attributes[i].GetType();
				if (!types.Contains(type))
				{
					types[type] = Attribute.InternalGetAttributeUsage(type);
				}
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00014504 File Offset: 0x00013504
		private static void AddAttributesToList(ArrayList attributeList, Attribute[] attributes, Hashtable types)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				Type type = attributes[i].GetType();
				AttributeUsageAttribute attributeUsageAttribute = (AttributeUsageAttribute)types[type];
				if (attributeUsageAttribute == null)
				{
					attributeUsageAttribute = Attribute.InternalGetAttributeUsage(type);
					types[type] = attributeUsageAttribute;
					if (attributeUsageAttribute.Inherited)
					{
						attributeList.Add(attributes[i]);
					}
				}
				else if (attributeUsageAttribute.Inherited && attributeUsageAttribute.AllowMultiple)
				{
					attributeList.Add(attributes[i]);
				}
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00014578 File Offset: 0x00013578
		private static AttributeUsageAttribute InternalGetAttributeUsage(Type type)
		{
			object[] customAttributes = type.GetCustomAttributes(typeof(AttributeUsageAttribute), false);
			if (customAttributes.Length == 1)
			{
				return (AttributeUsageAttribute)customAttributes[0];
			}
			if (customAttributes.Length == 0)
			{
				return AttributeUsageAttribute.Default;
			}
			throw new FormatException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Format_AttributeUsage"), new object[] { type }));
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000145D6 File Offset: 0x000135D6
		public static Attribute[] GetCustomAttributes(MemberInfo element, Type type)
		{
			return Attribute.GetCustomAttributes(element, type, true);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000145E0 File Offset: 0x000135E0
		public static Attribute[] GetCustomAttributes(MemberInfo element, Type type, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsSubclassOf(typeof(Attribute)) && type != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			MemberTypes memberType = element.MemberType;
			if (memberType == MemberTypes.Event)
			{
				return Attribute.InternalGetCustomAttributes((EventInfo)element, type, inherit);
			}
			if (memberType == MemberTypes.Property)
			{
				return Attribute.InternalGetCustomAttributes((PropertyInfo)element, type, inherit);
			}
			return element.GetCustomAttributes(type, inherit) as Attribute[];
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00014671 File Offset: 0x00013671
		public static Attribute[] GetCustomAttributes(MemberInfo element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0001467C File Offset: 0x0001367C
		public static Attribute[] GetCustomAttributes(MemberInfo element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			MemberTypes memberType = element.MemberType;
			if (memberType == MemberTypes.Event)
			{
				return Attribute.InternalGetCustomAttributes((EventInfo)element, typeof(Attribute), inherit);
			}
			if (memberType == MemberTypes.Property)
			{
				return Attribute.InternalGetCustomAttributes((PropertyInfo)element, typeof(Attribute), inherit);
			}
			return element.GetCustomAttributes(typeof(Attribute), inherit) as Attribute[];
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000146EB File Offset: 0x000136EB
		public static bool IsDefined(MemberInfo element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, true);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x000146F8 File Offset: 0x000136F8
		public static bool IsDefined(MemberInfo element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			MemberTypes memberType = element.MemberType;
			if (memberType == MemberTypes.Event)
			{
				return Attribute.InternalIsDefined((EventInfo)element, attributeType, inherit);
			}
			if (memberType == MemberTypes.Property)
			{
				return Attribute.InternalIsDefined((PropertyInfo)element, attributeType, inherit);
			}
			return element.IsDefined(attributeType, inherit);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00014784 File Offset: 0x00013784
		public static Attribute GetCustomAttribute(MemberInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00014790 File Offset: 0x00013790
		public static Attribute GetCustomAttribute(MemberInfo element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.AmbigCust"));
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000147C9 File Offset: 0x000137C9
		public static Attribute[] GetCustomAttributes(ParameterInfo element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000147D2 File Offset: 0x000137D2
		public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType, true);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000147DC File Offset: 0x000137DC
		public static Attribute[] GetCustomAttributes(ParameterInfo element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			MemberInfo member = element.Member;
			if (member.MemberType == MemberTypes.Method && inherit)
			{
				return Attribute.InternalParamGetCustomAttributes((MethodInfo)member, element, attributeType, inherit);
			}
			return element.GetCustomAttributes(attributeType, inherit) as Attribute[];
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00014864 File Offset: 0x00013864
		public static Attribute[] GetCustomAttributes(ParameterInfo element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			MemberInfo member = element.Member;
			if (member.MemberType == MemberTypes.Method && inherit)
			{
				return Attribute.InternalParamGetCustomAttributes((MethodInfo)member, element, null, inherit);
			}
			return element.GetCustomAttributes(typeof(Attribute), inherit) as Attribute[];
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000148B7 File Offset: 0x000138B7
		public static bool IsDefined(ParameterInfo element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, true);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x000148C4 File Offset: 0x000138C4
		public static bool IsDefined(ParameterInfo element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			MemberInfo member = element.Member;
			MemberTypes memberType = member.MemberType;
			if (memberType == MemberTypes.Constructor)
			{
				return element.IsDefined(attributeType, false);
			}
			if (memberType == MemberTypes.Method)
			{
				return Attribute.InternalParamIsDefined((MethodInfo)member, element, attributeType, inherit);
			}
			if (memberType != MemberTypes.Property)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidParamInfo"));
			}
			return element.IsDefined(attributeType, false);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00014969 File Offset: 0x00013969
		public static Attribute GetCustomAttribute(ParameterInfo element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00014974 File Offset: 0x00013974
		public static Attribute GetCustomAttribute(ParameterInfo element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.AmbigCust"));
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000149B4 File Offset: 0x000139B4
		public static Attribute[] GetCustomAttributes(Module element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType, true);
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x000149BE File Offset: 0x000139BE
		public static Attribute[] GetCustomAttributes(Module element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000149C7 File Offset: 0x000139C7
		public static Attribute[] GetCustomAttributes(Module element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Attribute[])element.GetCustomAttributes(typeof(Attribute), inherit);
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000149F0 File Offset: 0x000139F0
		public static Attribute[] GetCustomAttributes(Module element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			return (Attribute[])element.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x00014A55 File Offset: 0x00013A55
		public static bool IsDefined(Module element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, false);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x00014A60 File Offset: 0x00013A60
		public static bool IsDefined(Module element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			return element.IsDefined(attributeType, false);
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00014AC0 File Offset: 0x00013AC0
		public static Attribute GetCustomAttribute(Module element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00014ACC File Offset: 0x00013ACC
		public static Attribute GetCustomAttribute(Module element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.AmbigCust"));
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00014B05 File Offset: 0x00013B05
		public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType)
		{
			return Attribute.GetCustomAttributes(element, attributeType, true);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00014B10 File Offset: 0x00013B10
		public static Attribute[] GetCustomAttributes(Assembly element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			return (Attribute[])element.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00014B75 File Offset: 0x00013B75
		public static Attribute[] GetCustomAttributes(Assembly element)
		{
			return Attribute.GetCustomAttributes(element, true);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x00014B7E File Offset: 0x00013B7E
		public static Attribute[] GetCustomAttributes(Assembly element, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			return (Attribute[])element.GetCustomAttributes(typeof(Attribute), inherit);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00014BA4 File Offset: 0x00013BA4
		public static bool IsDefined(Assembly element, Type attributeType)
		{
			return Attribute.IsDefined(element, attributeType, true);
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00014BB0 File Offset: 0x00013BB0
		public static bool IsDefined(Assembly element, Type attributeType, bool inherit)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (!attributeType.IsSubclassOf(typeof(Attribute)) && attributeType != typeof(Attribute))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MustHaveAttributeBaseClass"));
			}
			return element.IsDefined(attributeType, false);
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00014C10 File Offset: 0x00013C10
		public static Attribute GetCustomAttribute(Assembly element, Type attributeType)
		{
			return Attribute.GetCustomAttribute(element, attributeType, true);
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00014C1C File Offset: 0x00013C1C
		public static Attribute GetCustomAttribute(Assembly element, Type attributeType, bool inherit)
		{
			Attribute[] customAttributes = Attribute.GetCustomAttributes(element, attributeType, inherit);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length == 1)
			{
				return customAttributes[0];
			}
			throw new AmbiguousMatchException(Environment.GetResourceString("RFLCT.AmbigCust"));
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00014C60 File Offset: 0x00013C60
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			RuntimeType runtimeType = (RuntimeType)base.GetType();
			RuntimeType runtimeType2 = (RuntimeType)obj.GetType();
			if (runtimeType2 != runtimeType)
			{
				return false;
			}
			FieldInfo[] fields = runtimeType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < fields.Length; i++)
			{
				object value = ((RuntimeFieldInfo)fields[i]).GetValue(this);
				object value2 = ((RuntimeFieldInfo)fields[i]).GetValue(obj);
				if (value == null)
				{
					if (value2 != null)
					{
						return false;
					}
				}
				else if (!value.Equals(value2))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00014CE8 File Offset: 0x00013CE8
		public override int GetHashCode()
		{
			Type type = base.GetType();
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			object obj = null;
			foreach (FieldInfo fieldInfo in fields)
			{
				obj = fieldInfo.GetValue(this);
				if (obj != null)
				{
					break;
				}
			}
			if (obj != null)
			{
				return obj.GetHashCode();
			}
			return type.GetHashCode();
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x00014D36 File Offset: 0x00013D36
		public virtual object TypeId
		{
			get
			{
				return base.GetType();
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00014D3E File Offset: 0x00013D3E
		public virtual bool Match(object obj)
		{
			return this.Equals(obj);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00014D47 File Offset: 0x00013D47
		public virtual bool IsDefaultAttribute()
		{
			return false;
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00014D4A File Offset: 0x00013D4A
		void _Attribute.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x00014D51 File Offset: 0x00013D51
		void _Attribute.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00014D58 File Offset: 0x00013D58
		void _Attribute.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x00014D5F File Offset: 0x00013D5F
		void _Attribute.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}
	}
}
