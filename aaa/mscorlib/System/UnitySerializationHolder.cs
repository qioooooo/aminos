using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace System
{
	// Token: 0x02000120 RID: 288
	[Serializable]
	internal class UnitySerializationHolder : ISerializable, IObjectReference
	{
		// Token: 0x060010EA RID: 4330 RVA: 0x0002F480 File Offset: 0x0002E480
		internal static void GetUnitySerializationInfo(SerializationInfo info, Missing missing)
		{
			info.SetType(typeof(UnitySerializationHolder));
			info.AddValue("UnityType", 3);
		}

		// Token: 0x060010EB RID: 4331 RVA: 0x0002F4A0 File Offset: 0x0002E4A0
		internal static Type AddElementTypes(SerializationInfo info, Type type)
		{
			List<int> list = new List<int>();
			while (type.HasElementType)
			{
				if (type.IsSzArray)
				{
					list.Add(3);
				}
				else if (type.IsArray)
				{
					list.Add(type.GetArrayRank());
					list.Add(2);
				}
				else if (type.IsPointer)
				{
					list.Add(1);
				}
				else if (type.IsByRef)
				{
					list.Add(4);
				}
				type = type.GetElementType();
			}
			info.AddValue("ElementTypes", list.ToArray(), typeof(int[]));
			return type;
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0002F530 File Offset: 0x0002E530
		internal Type MakeElementTypes(Type type)
		{
			for (int i = this.m_elementTypes.Length - 1; i >= 0; i--)
			{
				if (this.m_elementTypes[i] == 3)
				{
					type = type.MakeArrayType();
				}
				else if (this.m_elementTypes[i] == 2)
				{
					type = type.MakeArrayType(this.m_elementTypes[--i]);
				}
				else if (this.m_elementTypes[i] == 1)
				{
					type = type.MakePointerType();
				}
				else if (this.m_elementTypes[i] == 4)
				{
					type = type.MakeByRefType();
				}
			}
			return type;
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0002F5B4 File Offset: 0x0002E5B4
		internal static void GetUnitySerializationInfo(SerializationInfo info, Type type)
		{
			if (type.GetRootElementType().IsGenericParameter)
			{
				type = UnitySerializationHolder.AddElementTypes(info, type);
				info.SetType(typeof(UnitySerializationHolder));
				info.AddValue("UnityType", 7);
				info.AddValue("GenericParameterPosition", type.GenericParameterPosition);
				info.AddValue("DeclaringMethod", type.DeclaringMethod, typeof(MethodBase));
				info.AddValue("DeclaringType", type.DeclaringType, typeof(Type));
				return;
			}
			int num = 4;
			if (!type.IsGenericTypeDefinition && type.ContainsGenericParameters)
			{
				num = 8;
				type = UnitySerializationHolder.AddElementTypes(info, type);
				info.AddValue("GenericArguments", type.GetGenericArguments(), typeof(Type[]));
				type = type.GetGenericTypeDefinition();
			}
			UnitySerializationHolder.GetUnitySerializationInfo(info, num, type.FullName, Assembly.GetAssembly(type));
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0002F690 File Offset: 0x0002E690
		internal static void GetUnitySerializationInfo(SerializationInfo info, int unityType, string data, Assembly assembly)
		{
			info.SetType(typeof(UnitySerializationHolder));
			info.AddValue("Data", data, typeof(string));
			info.AddValue("UnityType", unityType);
			string text;
			if (assembly == null)
			{
				text = string.Empty;
			}
			else
			{
				text = assembly.FullName;
			}
			info.AddValue("AssemblyName", text);
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0002F6F0 File Offset: 0x0002E6F0
		internal UnitySerializationHolder(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_unityType = info.GetInt32("UnityType");
			if (this.m_unityType == 3)
			{
				return;
			}
			if (this.m_unityType == 7)
			{
				this.m_declaringMethod = info.GetValue("DeclaringMethod", typeof(MethodBase)) as MethodBase;
				this.m_declaringType = info.GetValue("DeclaringType", typeof(Type)) as Type;
				this.m_genericParameterPosition = info.GetInt32("GenericParameterPosition");
				this.m_elementTypes = info.GetValue("ElementTypes", typeof(int[])) as int[];
				return;
			}
			if (this.m_unityType == 8)
			{
				this.m_instantiation = info.GetValue("GenericArguments", typeof(Type[])) as Type[];
				this.m_elementTypes = info.GetValue("ElementTypes", typeof(int[])) as int[];
			}
			this.m_data = info.GetString("Data");
			this.m_assemblyName = info.GetString("AssemblyName");
		}

		// Token: 0x060010F0 RID: 4336 RVA: 0x0002F814 File Offset: 0x0002E814
		private void ThrowInsufficientInformation(string field)
		{
			throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_InsufficientDeserializationState"), new object[] { field }));
		}

		// Token: 0x060010F1 RID: 4337 RVA: 0x0002F846 File Offset: 0x0002E846
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_UnitySerHolder"));
		}

		// Token: 0x060010F2 RID: 4338 RVA: 0x0002F858 File Offset: 0x0002E858
		public virtual object GetRealObject(StreamingContext context)
		{
			switch (this.m_unityType)
			{
			case 1:
				return Empty.Value;
			case 2:
				return DBNull.Value;
			case 3:
				return Missing.Value;
			case 4:
			{
				if (this.m_data == null || this.m_data.Length == 0)
				{
					this.ThrowInsufficientInformation("Data");
				}
				if (this.m_assemblyName == null)
				{
					this.ThrowInsufficientInformation("AssemblyName");
				}
				if (this.m_assemblyName.Length == 0)
				{
					return Type.GetType(this.m_data, true, false);
				}
				Assembly assembly = Assembly.Load(this.m_assemblyName);
				return assembly.GetType(this.m_data, true, false);
			}
			case 5:
			{
				if (this.m_data == null || this.m_data.Length == 0)
				{
					this.ThrowInsufficientInformation("Data");
				}
				if (this.m_assemblyName == null)
				{
					this.ThrowInsufficientInformation("AssemblyName");
				}
				Assembly assembly = Assembly.Load(this.m_assemblyName);
				Module module = assembly.GetModule(this.m_data);
				if (module == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Serialization_UnableToFindModule"), new object[] { this.m_data, this.m_assemblyName }));
				}
				return module;
			}
			case 6:
				if (this.m_data == null || this.m_data.Length == 0)
				{
					this.ThrowInsufficientInformation("Data");
				}
				if (this.m_assemblyName == null)
				{
					this.ThrowInsufficientInformation("AssemblyName");
				}
				return Assembly.Load(this.m_assemblyName);
			case 7:
				if (this.m_declaringMethod == null && this.m_declaringType == null)
				{
					this.ThrowInsufficientInformation("DeclaringMember");
				}
				if (this.m_declaringMethod != null)
				{
					return this.m_declaringMethod.GetGenericArguments()[this.m_genericParameterPosition];
				}
				return this.MakeElementTypes(this.m_declaringType.GetGenericArguments()[this.m_genericParameterPosition]);
			case 8:
			{
				this.m_unityType = 4;
				Type type = this.GetRealObject(context) as Type;
				this.m_unityType = 8;
				if (this.m_instantiation[0] == null)
				{
					return null;
				}
				return this.MakeElementTypes(type.MakeGenericType(this.m_instantiation));
			}
			default:
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUnity"));
			}
		}

		// Token: 0x04000574 RID: 1396
		internal const int EmptyUnity = 1;

		// Token: 0x04000575 RID: 1397
		internal const int NullUnity = 2;

		// Token: 0x04000576 RID: 1398
		internal const int MissingUnity = 3;

		// Token: 0x04000577 RID: 1399
		internal const int RuntimeTypeUnity = 4;

		// Token: 0x04000578 RID: 1400
		internal const int ModuleUnity = 5;

		// Token: 0x04000579 RID: 1401
		internal const int AssemblyUnity = 6;

		// Token: 0x0400057A RID: 1402
		internal const int GenericParameterTypeUnity = 7;

		// Token: 0x0400057B RID: 1403
		internal const int PartialInstantiationTypeUnity = 8;

		// Token: 0x0400057C RID: 1404
		internal const int Pointer = 1;

		// Token: 0x0400057D RID: 1405
		internal const int Array = 2;

		// Token: 0x0400057E RID: 1406
		internal const int SzArray = 3;

		// Token: 0x0400057F RID: 1407
		internal const int ByRef = 4;

		// Token: 0x04000580 RID: 1408
		private Type[] m_instantiation;

		// Token: 0x04000581 RID: 1409
		private int[] m_elementTypes;

		// Token: 0x04000582 RID: 1410
		private int m_genericParameterPosition;

		// Token: 0x04000583 RID: 1411
		private Type m_declaringType;

		// Token: 0x04000584 RID: 1412
		private MethodBase m_declaringMethod;

		// Token: 0x04000585 RID: 1413
		private string m_data;

		// Token: 0x04000586 RID: 1414
		private string m_assemblyName;

		// Token: 0x04000587 RID: 1415
		private int m_unityType;
	}
}
