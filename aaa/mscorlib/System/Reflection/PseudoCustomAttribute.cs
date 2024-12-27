using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x020002F5 RID: 757
	internal static class PseudoCustomAttribute
	{
		// Token: 0x06001E35 RID: 7733
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetSecurityAttributes(void* module, int token, out object[] securityAttributes);

		// Token: 0x06001E36 RID: 7734 RVA: 0x0004C7F9 File Offset: 0x0004B7F9
		internal static void GetSecurityAttributes(ModuleHandle module, int token, out object[] securityAttributes)
		{
			PseudoCustomAttribute._GetSecurityAttributes(module.Value, token, out securityAttributes);
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x0004C80C File Offset: 0x0004B80C
		static PseudoCustomAttribute()
		{
			Type[] array = new Type[]
			{
				typeof(FieldOffsetAttribute),
				typeof(SerializableAttribute),
				typeof(MarshalAsAttribute),
				typeof(ComImportAttribute),
				typeof(NonSerializedAttribute),
				typeof(InAttribute),
				typeof(OutAttribute),
				typeof(OptionalAttribute),
				typeof(DllImportAttribute),
				typeof(PreserveSigAttribute)
			};
			PseudoCustomAttribute.s_pcasCount = array.Length;
			PseudoCustomAttribute.s_pca = new Hashtable(PseudoCustomAttribute.s_pcasCount);
			for (int i = 0; i < PseudoCustomAttribute.s_pcasCount; i++)
			{
				PseudoCustomAttribute.s_pca[array[i]] = array[i];
			}
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x0004C8DD File Offset: 0x0004B8DD
		[Conditional("_DEBUG")]
		private static void VerifyPseudoCustomAttribute(Type pca)
		{
			CustomAttribute.GetAttributeUsage(pca as RuntimeType);
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x0004C8EB File Offset: 0x0004B8EB
		internal static bool IsSecurityAttribute(Type type)
		{
			return type == typeof(SecurityAttribute) || type.IsSubclassOf(typeof(SecurityAttribute));
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x0004C90C File Offset: 0x0004B90C
		internal static Attribute[] GetCustomAttributes(RuntimeType type, Type caType, bool includeSecCa, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null && !PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				return new Attribute[0];
			}
			List<Attribute> list = new List<Attribute>();
			if (flag || caType == typeof(SerializableAttribute))
			{
				Attribute attribute = SerializableAttribute.GetCustomAttribute(type);
				if (attribute != null)
				{
					list.Add(attribute);
				}
			}
			if (flag || caType == typeof(ComImportAttribute))
			{
				Attribute attribute = ComImportAttribute.GetCustomAttribute(type);
				if (attribute != null)
				{
					list.Add(attribute);
				}
			}
			if (includeSecCa && (flag || PseudoCustomAttribute.IsSecurityAttribute(caType)) && !type.IsGenericParameter)
			{
				if (type.IsGenericType)
				{
					type = (RuntimeType)type.GetGenericTypeDefinition();
				}
				object[] array;
				PseudoCustomAttribute.GetSecurityAttributes(type.Module.ModuleHandle, type.MetadataToken, out array);
				if (array != null)
				{
					foreach (object obj in array)
					{
						if (caType == obj.GetType() || obj.GetType().IsSubclassOf(caType))
						{
							list.Add((Attribute)obj);
						}
					}
				}
			}
			count = list.Count;
			return list.ToArray();
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x0004CA40 File Offset: 0x0004BA40
		internal static bool IsDefined(RuntimeType type, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			int num;
			return (flag || PseudoCustomAttribute.s_pca[caType] != null || PseudoCustomAttribute.IsSecurityAttribute(caType)) && (((flag || caType == typeof(SerializableAttribute)) && SerializableAttribute.IsDefined(type)) || ((flag || caType == typeof(ComImportAttribute)) && ComImportAttribute.IsDefined(type)) || ((flag || PseudoCustomAttribute.IsSecurityAttribute(caType)) && PseudoCustomAttribute.GetCustomAttributes(type, caType, true, out num).Length != 0));
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x0004CAD8 File Offset: 0x0004BAD8
		internal static Attribute[] GetCustomAttributes(RuntimeMethodInfo method, Type caType, bool includeSecCa, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null && !PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				return new Attribute[0];
			}
			List<Attribute> list = new List<Attribute>();
			if (flag || caType == typeof(DllImportAttribute))
			{
				Attribute attribute = DllImportAttribute.GetCustomAttribute(method);
				if (attribute != null)
				{
					list.Add(attribute);
				}
			}
			if (flag || caType == typeof(PreserveSigAttribute))
			{
				Attribute attribute = PreserveSigAttribute.GetCustomAttribute(method);
				if (attribute != null)
				{
					list.Add(attribute);
				}
			}
			if (includeSecCa && (flag || PseudoCustomAttribute.IsSecurityAttribute(caType)))
			{
				object[] array;
				PseudoCustomAttribute.GetSecurityAttributes(method.Module.ModuleHandle, method.MetadataToken, out array);
				if (array != null)
				{
					foreach (object obj in array)
					{
						if (caType == obj.GetType() || obj.GetType().IsSubclassOf(caType))
						{
							list.Add((Attribute)obj);
						}
					}
				}
			}
			count = list.Count;
			return list.ToArray();
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x0004CBEC File Offset: 0x0004BBEC
		internal static bool IsDefined(RuntimeMethodInfo method, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			int num;
			return (flag || PseudoCustomAttribute.s_pca[caType] != null) && (((flag || caType == typeof(DllImportAttribute)) && DllImportAttribute.IsDefined(method)) || ((flag || caType == typeof(PreserveSigAttribute)) && PreserveSigAttribute.IsDefined(method)) || ((flag || PseudoCustomAttribute.IsSecurityAttribute(caType)) && PseudoCustomAttribute.GetCustomAttributes(method, caType, true, out num).Length != 0));
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x0004CC7C File Offset: 0x0004BC7C
		internal static Attribute[] GetCustomAttributes(ParameterInfo parameter, Type caType, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null)
			{
				return null;
			}
			Attribute[] array = new Attribute[PseudoCustomAttribute.s_pcasCount];
			if (flag || caType == typeof(InAttribute))
			{
				Attribute attribute = InAttribute.GetCustomAttribute(parameter);
				if (attribute != null)
				{
					array[count++] = attribute;
				}
			}
			if (flag || caType == typeof(OutAttribute))
			{
				Attribute attribute = OutAttribute.GetCustomAttribute(parameter);
				if (attribute != null)
				{
					array[count++] = attribute;
				}
			}
			if (flag || caType == typeof(OptionalAttribute))
			{
				Attribute attribute = OptionalAttribute.GetCustomAttribute(parameter);
				if (attribute != null)
				{
					array[count++] = attribute;
				}
			}
			if (flag || caType == typeof(MarshalAsAttribute))
			{
				Attribute attribute = MarshalAsAttribute.GetCustomAttribute(parameter);
				if (attribute != null)
				{
					array[count++] = attribute;
				}
			}
			return array;
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0004CD68 File Offset: 0x0004BD68
		internal static bool IsDefined(ParameterInfo parameter, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			return (flag || PseudoCustomAttribute.s_pca[caType] != null) && (((flag || caType == typeof(InAttribute)) && InAttribute.IsDefined(parameter)) || ((flag || caType == typeof(OutAttribute)) && OutAttribute.IsDefined(parameter)) || ((flag || caType == typeof(OptionalAttribute)) && OptionalAttribute.IsDefined(parameter)) || ((flag || caType == typeof(MarshalAsAttribute)) && MarshalAsAttribute.IsDefined(parameter)));
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x0004CE10 File Offset: 0x0004BE10
		internal static Attribute[] GetCustomAttributes(Assembly assembly, Type caType, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null && !PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				return new Attribute[0];
			}
			List<Attribute> list = new List<Attribute>();
			if (flag || PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				object[] array;
				PseudoCustomAttribute.GetSecurityAttributes(assembly.ManifestModule.ModuleHandle, assembly.AssemblyHandle.GetToken(), out array);
				if (array != null)
				{
					foreach (object obj in array)
					{
						if (caType == obj.GetType() || obj.GetType().IsSubclassOf(caType))
						{
							list.Add((Attribute)obj);
						}
					}
				}
			}
			count = list.Count;
			return list.ToArray();
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0004CEE0 File Offset: 0x0004BEE0
		internal static bool IsDefined(Assembly assembly, Type caType)
		{
			int num;
			return PseudoCustomAttribute.GetCustomAttributes(assembly, caType, out num).Length > 0;
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0004CEFB File Offset: 0x0004BEFB
		internal static Attribute[] GetCustomAttributes(Module module, Type caType, out int count)
		{
			count = 0;
			return null;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0004CF01 File Offset: 0x0004BF01
		internal static bool IsDefined(Module module, Type caType)
		{
			return false;
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0004CF04 File Offset: 0x0004BF04
		internal static Attribute[] GetCustomAttributes(RuntimeFieldInfo field, Type caType, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null)
			{
				return null;
			}
			Attribute[] array = new Attribute[PseudoCustomAttribute.s_pcasCount];
			if (flag || caType == typeof(MarshalAsAttribute))
			{
				Attribute attribute = MarshalAsAttribute.GetCustomAttribute(field);
				if (attribute != null)
				{
					array[count++] = attribute;
				}
			}
			if (flag || caType == typeof(FieldOffsetAttribute))
			{
				Attribute attribute = FieldOffsetAttribute.GetCustomAttribute(field);
				if (attribute != null)
				{
					array[count++] = attribute;
				}
			}
			if (flag || caType == typeof(NonSerializedAttribute))
			{
				Attribute attribute = NonSerializedAttribute.GetCustomAttribute(field);
				if (attribute != null)
				{
					array[count++] = attribute;
				}
			}
			return array;
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0004CFC8 File Offset: 0x0004BFC8
		internal static bool IsDefined(RuntimeFieldInfo field, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			return (flag || PseudoCustomAttribute.s_pca[caType] != null) && (((flag || caType == typeof(MarshalAsAttribute)) && MarshalAsAttribute.IsDefined(field)) || ((flag || caType == typeof(FieldOffsetAttribute)) && FieldOffsetAttribute.IsDefined(field)) || ((flag || caType == typeof(NonSerializedAttribute)) && NonSerializedAttribute.IsDefined(field)));
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0004D054 File Offset: 0x0004C054
		internal static Attribute[] GetCustomAttributes(RuntimeConstructorInfo ctor, Type caType, bool includeSecCa, out int count)
		{
			count = 0;
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			if (!flag && PseudoCustomAttribute.s_pca[caType] == null && !PseudoCustomAttribute.IsSecurityAttribute(caType))
			{
				return new Attribute[0];
			}
			List<Attribute> list = new List<Attribute>();
			if (includeSecCa && (flag || PseudoCustomAttribute.IsSecurityAttribute(caType)))
			{
				object[] array;
				PseudoCustomAttribute.GetSecurityAttributes(ctor.Module.ModuleHandle, ctor.MetadataToken, out array);
				if (array != null)
				{
					foreach (object obj in array)
					{
						if (caType == obj.GetType() || obj.GetType().IsSubclassOf(caType))
						{
							list.Add((Attribute)obj);
						}
					}
				}
			}
			count = list.Count;
			return list.ToArray();
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0004D120 File Offset: 0x0004C120
		internal static bool IsDefined(RuntimeConstructorInfo ctor, Type caType)
		{
			bool flag = caType == typeof(object) || caType == typeof(Attribute);
			int num;
			return (flag || PseudoCustomAttribute.s_pca[caType] != null) && ((flag || PseudoCustomAttribute.IsSecurityAttribute(caType)) && PseudoCustomAttribute.GetCustomAttributes(ctor, caType, true, out num).Length != 0);
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x0004D179 File Offset: 0x0004C179
		internal static Attribute[] GetCustomAttributes(RuntimePropertyInfo property, Type caType, out int count)
		{
			count = 0;
			return null;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0004D17F File Offset: 0x0004C17F
		internal static bool IsDefined(RuntimePropertyInfo property, Type caType)
		{
			return false;
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x0004D182 File Offset: 0x0004C182
		internal static Attribute[] GetCustomAttributes(RuntimeEventInfo e, Type caType, out int count)
		{
			count = 0;
			return null;
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x0004D188 File Offset: 0x0004C188
		internal static bool IsDefined(RuntimeEventInfo e, Type caType)
		{
			return false;
		}

		// Token: 0x04000AF5 RID: 2805
		private static Hashtable s_pca;

		// Token: 0x04000AF6 RID: 2806
		private static int s_pcasCount;
	}
}
