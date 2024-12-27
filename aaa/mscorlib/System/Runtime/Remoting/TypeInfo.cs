using System;

namespace System.Runtime.Remoting
{
	// Token: 0x0200071D RID: 1821
	[Serializable]
	internal class TypeInfo : IRemotingTypeInfo
	{
		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x060041A9 RID: 16809 RVA: 0x000E0325 File Offset: 0x000DF325
		// (set) Token: 0x060041AA RID: 16810 RVA: 0x000E032D File Offset: 0x000DF32D
		public virtual string TypeName
		{
			get
			{
				return this.serverType;
			}
			set
			{
				this.serverType = value;
			}
		}

		// Token: 0x060041AB RID: 16811 RVA: 0x000E0338 File Offset: 0x000DF338
		public virtual bool CanCastTo(Type castType, object o)
		{
			if (castType != null)
			{
				if (castType == typeof(MarshalByRefObject) || castType == typeof(object))
				{
					return true;
				}
				if (castType.IsInterface)
				{
					return this.interfacesImplemented != null && this.CanCastTo(castType, this.InterfacesImplemented);
				}
				if (castType.IsMarshalByRef)
				{
					if (this.CompareTypes(castType, this.serverType))
					{
						return true;
					}
					if (this.serverHierarchy != null && this.CanCastTo(castType, this.ServerHierarchy))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060041AC RID: 16812 RVA: 0x000E03B7 File Offset: 0x000DF3B7
		internal static string GetQualifiedTypeName(Type type)
		{
			if (type == null)
			{
				return null;
			}
			return RemotingServices.GetDefaultQualifiedTypeName(type);
		}

		// Token: 0x060041AD RID: 16813 RVA: 0x000E03C4 File Offset: 0x000DF3C4
		internal static bool ParseTypeAndAssembly(string typeAndAssembly, out string typeName, out string assemName)
		{
			if (typeAndAssembly == null)
			{
				typeName = null;
				assemName = null;
				return false;
			}
			int num = typeAndAssembly.IndexOf(',');
			if (num == -1)
			{
				typeName = typeAndAssembly;
				assemName = null;
				return true;
			}
			typeName = typeAndAssembly.Substring(0, num);
			assemName = typeAndAssembly.Substring(num + 1).Trim();
			return true;
		}

		// Token: 0x060041AE RID: 16814 RVA: 0x000E040C File Offset: 0x000DF40C
		internal TypeInfo(Type typeOfObj)
		{
			this.ServerType = TypeInfo.GetQualifiedTypeName(typeOfObj);
			Type type = typeOfObj.BaseType;
			int num = 0;
			while (type != typeof(MarshalByRefObject) && type != null)
			{
				type = type.BaseType;
				num++;
			}
			string[] array = null;
			if (num > 0)
			{
				array = new string[num];
				type = typeOfObj.BaseType;
				for (int i = 0; i < num; i++)
				{
					array[i] = TypeInfo.GetQualifiedTypeName(type);
					type = type.BaseType;
				}
			}
			this.ServerHierarchy = array;
			Type[] interfaces = typeOfObj.GetInterfaces();
			string[] array2 = null;
			bool isInterface = typeOfObj.IsInterface;
			if (interfaces.Length > 0 || isInterface)
			{
				array2 = new string[interfaces.Length + (isInterface ? 1 : 0)];
				for (int j = 0; j < interfaces.Length; j++)
				{
					array2[j] = TypeInfo.GetQualifiedTypeName(interfaces[j]);
				}
				if (isInterface)
				{
					array2[array2.Length - 1] = TypeInfo.GetQualifiedTypeName(typeOfObj);
				}
			}
			this.InterfacesImplemented = array2;
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x060041AF RID: 16815 RVA: 0x000E04F7 File Offset: 0x000DF4F7
		// (set) Token: 0x060041B0 RID: 16816 RVA: 0x000E04FF File Offset: 0x000DF4FF
		internal string ServerType
		{
			get
			{
				return this.serverType;
			}
			set
			{
				this.serverType = value;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x060041B1 RID: 16817 RVA: 0x000E0508 File Offset: 0x000DF508
		// (set) Token: 0x060041B2 RID: 16818 RVA: 0x000E0510 File Offset: 0x000DF510
		private string[] ServerHierarchy
		{
			get
			{
				return this.serverHierarchy;
			}
			set
			{
				this.serverHierarchy = value;
			}
		}

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x060041B3 RID: 16819 RVA: 0x000E0519 File Offset: 0x000DF519
		// (set) Token: 0x060041B4 RID: 16820 RVA: 0x000E0521 File Offset: 0x000DF521
		private string[] InterfacesImplemented
		{
			get
			{
				return this.interfacesImplemented;
			}
			set
			{
				this.interfacesImplemented = value;
			}
		}

		// Token: 0x060041B5 RID: 16821 RVA: 0x000E052C File Offset: 0x000DF52C
		private bool CompareTypes(Type type1, string type2)
		{
			Type type3 = RemotingServices.InternalGetTypeFromQualifiedTypeName(type2);
			return type1 == type3;
		}

		// Token: 0x060041B6 RID: 16822 RVA: 0x000E0544 File Offset: 0x000DF544
		private bool CanCastTo(Type castType, string[] types)
		{
			bool flag = false;
			if (castType != null)
			{
				for (int i = 0; i < types.Length; i++)
				{
					if (this.CompareTypes(castType, types[i]))
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x040020C6 RID: 8390
		private string serverType;

		// Token: 0x040020C7 RID: 8391
		private string[] serverHierarchy;

		// Token: 0x040020C8 RID: 8392
		private string[] interfacesImplemented;
	}
}
