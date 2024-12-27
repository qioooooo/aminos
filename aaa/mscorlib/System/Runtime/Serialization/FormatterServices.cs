using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Runtime.Serialization
{
	// Token: 0x02000349 RID: 841
	[ComVisible(true)]
	public sealed class FormatterServices
	{
		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x0600219E RID: 8606 RVA: 0x000543C8 File Offset: 0x000533C8
		private static object formatterServicesSyncObject
		{
			get
			{
				if (FormatterServices.s_FormatterServicesSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref FormatterServices.s_FormatterServicesSyncObject, obj, null);
				}
				return FormatterServices.s_FormatterServicesSyncObject;
			}
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000543F4 File Offset: 0x000533F4
		private FormatterServices()
		{
			throw new NotSupportedException();
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x00054404 File Offset: 0x00053404
		private static MemberInfo[] GetSerializableMembers(RuntimeType type)
		{
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			int num = 0;
			for (int i = 0; i < fields.Length; i++)
			{
				if ((fields[i].Attributes & FieldAttributes.NotSerialized) != FieldAttributes.NotSerialized)
				{
					num++;
				}
			}
			if (num != fields.Length)
			{
				FieldInfo[] array = new FieldInfo[num];
				num = 0;
				for (int j = 0; j < fields.Length; j++)
				{
					if ((fields[j].Attributes & FieldAttributes.NotSerialized) != FieldAttributes.NotSerialized)
					{
						array[num] = fields[j];
						num++;
					}
				}
				return array;
			}
			return fields;
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x00054488 File Offset: 0x00053488
		private static bool CheckSerializable(RuntimeType type)
		{
			return type.IsSerializable;
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x00054498 File Offset: 0x00053498
		private static MemberInfo[] InternalGetSerializableMembers(RuntimeType type)
		{
			if (type.IsInterface)
			{
				return new MemberInfo[0];
			}
			if (!FormatterServices.CheckSerializable(type))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NonSerType"), new object[]
				{
					type.FullName,
					type.Module.Assembly.FullName
				}));
			}
			MemberInfo[] array = FormatterServices.GetSerializableMembers(type);
			RuntimeType runtimeType = (RuntimeType)type.BaseType;
			if (runtimeType != null && runtimeType != typeof(object))
			{
				Type[] array2 = null;
				int num = 0;
				bool parentTypes = FormatterServices.GetParentTypes(runtimeType, out array2, out num);
				if (num > 0)
				{
					ArrayList arrayList = new ArrayList();
					for (int i = 0; i < num; i++)
					{
						runtimeType = (RuntimeType)array2[i];
						if (!FormatterServices.CheckSerializable(runtimeType))
						{
							throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_NonSerType"), new object[]
							{
								runtimeType.FullName,
								runtimeType.Module.Assembly.FullName
							}));
						}
						FieldInfo[] fields = runtimeType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
						string text = (parentTypes ? runtimeType.Name : runtimeType.FullName);
						foreach (FieldInfo fieldInfo in fields)
						{
							if (!fieldInfo.IsNotSerialized)
							{
								arrayList.Add(new SerializationFieldInfo((RuntimeFieldInfo)fieldInfo, text));
							}
						}
					}
					if (arrayList != null && arrayList.Count > 0)
					{
						MemberInfo[] array4 = new MemberInfo[arrayList.Count + array.Length];
						Array.Copy(array, array4, array.Length);
						arrayList.CopyTo(array4, array.Length);
						array = array4;
					}
				}
			}
			return array;
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x00054644 File Offset: 0x00053644
		private static bool GetParentTypes(Type parentType, out Type[] parentTypes, out int parentTypeCount)
		{
			parentTypes = null;
			parentTypeCount = 0;
			bool flag = true;
			for (Type type = parentType; type != typeof(object); type = type.BaseType)
			{
				if (!type.IsInterface)
				{
					string name = type.Name;
					int num = 0;
					while (flag && num < parentTypeCount)
					{
						string name2 = parentTypes[num].Name;
						if (name2.Length == name.Length && name2[0] == name[0] && name == name2)
						{
							flag = false;
							break;
						}
						num++;
					}
					if (parentTypes == null || parentTypeCount == parentTypes.Length)
					{
						Type[] array = new Type[Math.Max(parentTypeCount * 2, 12)];
						if (parentTypes != null)
						{
							Array.Copy(parentTypes, 0, array, 0, parentTypeCount);
						}
						parentTypes = array;
					}
					parentTypes[parentTypeCount++] = type;
				}
			}
			return flag;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x00054715 File Offset: 0x00053715
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static MemberInfo[] GetSerializableMembers(Type type)
		{
			return FormatterServices.GetSerializableMembers(type, new StreamingContext(StreamingContextStates.All));
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x00054728 File Offset: 0x00053728
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static MemberInfo[] GetSerializableMembers(Type type, StreamingContext context)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InvalidType"), new object[] { type.ToString() }));
			}
			MemberHolder memberHolder = new MemberHolder(type, context);
			if (FormatterServices.m_MemberInfoTable.ContainsKey(memberHolder))
			{
				return FormatterServices.m_MemberInfoTable[memberHolder];
			}
			MemberInfo[] array;
			lock (FormatterServices.formatterServicesSyncObject)
			{
				if (FormatterServices.m_MemberInfoTable.ContainsKey(memberHolder))
				{
					return FormatterServices.m_MemberInfoTable[memberHolder];
				}
				array = FormatterServices.InternalGetSerializableMembers((RuntimeType)type);
				FormatterServices.m_MemberInfoTable[memberHolder] = array;
			}
			return array;
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x000547F4 File Offset: 0x000537F4
		public static void CheckTypeSecurity(Type t, TypeFilterLevel securityLevel)
		{
			if (securityLevel == TypeFilterLevel.Low)
			{
				for (int i = 0; i < FormatterServices.advancedTypes.Length; i++)
				{
					if (FormatterServices.advancedTypes[i].IsAssignableFrom(t))
					{
						throw new SecurityException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_TypeSecurity"), new object[]
						{
							FormatterServices.advancedTypes[i].FullName,
							t.FullName
						}));
					}
				}
			}
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x00054860 File Offset: 0x00053860
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static object GetUninitializedObject(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InvalidType"), new object[] { type.ToString() }));
			}
			return FormatterServices.nativeGetUninitializedObject((RuntimeType)type);
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000548BC File Offset: 0x000538BC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static object GetSafeUninitializedObject(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!(type is RuntimeType))
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InvalidType"), new object[] { type.ToString() }));
			}
			if (type == typeof(ConstructionCall) || type == typeof(LogicalCallContext) || type == typeof(SynchronizationAttribute))
			{
				return FormatterServices.nativeGetUninitializedObject((RuntimeType)type);
			}
			object obj;
			try
			{
				obj = FormatterServices.nativeGetSafeUninitializedObject((RuntimeType)type);
			}
			catch (SecurityException ex)
			{
				throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_Security"), new object[] { type.FullName }), ex);
			}
			return obj;
		}

		// Token: 0x060021A9 RID: 8617
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object nativeGetSafeUninitializedObject(RuntimeType type);

		// Token: 0x060021AA RID: 8618
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern object nativeGetUninitializedObject(RuntimeType type);

		// Token: 0x060021AB RID: 8619
		[SecurityCritical]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetEnableUnsafeTypeForwarders();

		// Token: 0x060021AC RID: 8620 RVA: 0x0005498C File Offset: 0x0005398C
		[SecuritySafeCritical]
		internal static bool UnsafeTypeForwardersIsEnabled()
		{
			return FormatterServices.GetEnableUnsafeTypeForwarders();
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00054994 File Offset: 0x00053994
		internal static void SerializationSetValue(MemberInfo fi, object target, object value)
		{
			RtFieldInfo rtFieldInfo = fi as RtFieldInfo;
			if (rtFieldInfo != null)
			{
				rtFieldInfo.InternalSetValue(target, value, BindingFlags.Default, FormatterServices.s_binder, null, false);
				return;
			}
			((SerializationFieldInfo)fi).InternalSetValue(target, value, BindingFlags.Default, FormatterServices.s_binder, null, false, true);
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x000549D4 File Offset: 0x000539D4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static object PopulateObjectMembers(object obj, MemberInfo[] members, object[] data)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (members == null)
			{
				throw new ArgumentNullException("members");
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (members.Length != data.Length)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_DataLengthDifferent"));
			}
			for (int i = 0; i < members.Length; i++)
			{
				MemberInfo memberInfo = members[i];
				if (memberInfo == null)
				{
					throw new ArgumentNullException("members", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_NullMember"), new object[] { i }));
				}
				if (data[i] != null)
				{
					if (memberInfo.MemberType != MemberTypes.Field)
					{
						throw new SerializationException(Environment.GetResourceString("Serialization_UnknownMemberInfo"));
					}
					FormatterServices.SerializationSetValue(memberInfo, obj, data[i]);
				}
			}
			return obj;
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x00054A94 File Offset: 0x00053A94
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static object[] GetObjectData(object obj, MemberInfo[] members)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (members == null)
			{
				throw new ArgumentNullException("members");
			}
			int num = members.Length;
			object[] array = new object[num];
			for (int i = 0; i < num; i++)
			{
				MemberInfo memberInfo = members[i];
				if (memberInfo == null)
				{
					throw new ArgumentNullException("members", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentNull_NullMember"), new object[] { i }));
				}
				if (memberInfo.MemberType != MemberTypes.Field)
				{
					throw new SerializationException(Environment.GetResourceString("Serialization_UnknownMemberInfo"));
				}
				RtFieldInfo rtFieldInfo = memberInfo as RtFieldInfo;
				if (rtFieldInfo != null)
				{
					array[i] = rtFieldInfo.InternalGetValue(obj, false);
				}
				else
				{
					array[i] = ((SerializationFieldInfo)memberInfo).InternalGetValue(obj, false);
				}
			}
			return array;
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x00054B5A File Offset: 0x00053B5A
		public static ISerializationSurrogate GetSurrogateForCyclicalReference(ISerializationSurrogate innerSurrogate)
		{
			if (innerSurrogate == null)
			{
				throw new ArgumentNullException("innerSurrogate");
			}
			return new SurrogateForCyclicalReference(innerSurrogate);
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x00054B70 File Offset: 0x00053B70
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public static Type GetTypeFromAssembly(Assembly assem, string name)
		{
			if (assem == null)
			{
				throw new ArgumentNullException("assem");
			}
			return assem.GetType(name, false, false);
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x00054B8C File Offset: 0x00053B8C
		internal static Assembly LoadAssemblyFromString(string assemblyName)
		{
			return Assembly.Load(assemblyName);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x00054BA4 File Offset: 0x00053BA4
		internal static Assembly LoadAssemblyFromStringNoThrow(string assemblyName)
		{
			try
			{
				return FormatterServices.LoadAssemblyFromString(assemblyName);
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x04000DEE RID: 3566
		internal static Dictionary<MemberHolder, MemberInfo[]> m_MemberInfoTable = new Dictionary<MemberHolder, MemberInfo[]>(32);

		// Token: 0x04000DEF RID: 3567
		private static object s_FormatterServicesSyncObject = null;

		// Token: 0x04000DF0 RID: 3568
		private static readonly Type[] advancedTypes = new Type[]
		{
			typeof(ObjRef),
			typeof(DelegateSerializationHolder),
			typeof(IEnvoyInfo),
			typeof(ISponsor)
		};

		// Token: 0x04000DF1 RID: 3569
		private static Binder s_binder = Type.DefaultBinder;
	}
}
