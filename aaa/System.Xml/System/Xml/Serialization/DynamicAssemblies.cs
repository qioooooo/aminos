using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;

namespace System.Xml.Serialization
{
	// Token: 0x02000332 RID: 818
	internal static class DynamicAssemblies
	{
		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000CEDA9 File Offset: 0x000CDDA9
		private static FileIOPermission UnrestrictedFileIOPermission
		{
			get
			{
				if (DynamicAssemblies.fileIOPermission == null)
				{
					DynamicAssemblies.fileIOPermission = new FileIOPermission(PermissionState.Unrestricted);
				}
				return DynamicAssemblies.fileIOPermission;
			}
		}

		// Token: 0x060027F9 RID: 10233 RVA: 0x000CEDC4 File Offset: 0x000CDDC4
		internal static bool IsTypeDynamic(Type type)
		{
			object obj = DynamicAssemblies.tableIsTypeDynamic[type];
			if (obj == null)
			{
				DynamicAssemblies.UnrestrictedFileIOPermission.Assert();
				Module module = type.Module;
				Assembly assembly = module.Assembly;
				bool flag = module is ModuleBuilder || assembly.Location == null || assembly.Location.Length == 0;
				if (!flag)
				{
					if (type.IsArray)
					{
						flag = DynamicAssemblies.IsTypeDynamic(type.GetElementType());
					}
					else if (type.IsGenericType)
					{
						Type[] genericArguments = type.GetGenericArguments();
						if (genericArguments != null)
						{
							foreach (Type type2 in genericArguments)
							{
								if (type2 != null && !type2.IsGenericParameter)
								{
									flag = DynamicAssemblies.IsTypeDynamic(type2);
									if (flag)
									{
										break;
									}
								}
							}
						}
					}
				}
				obj = (DynamicAssemblies.tableIsTypeDynamic[type] = flag);
			}
			return (bool)obj;
		}

		// Token: 0x060027FA RID: 10234 RVA: 0x000CEE98 File Offset: 0x000CDE98
		internal static bool IsTypeDynamic(Type[] arguments)
		{
			foreach (Type type in arguments)
			{
				if (DynamicAssemblies.IsTypeDynamic(type))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000CEEC8 File Offset: 0x000CDEC8
		internal static void Add(Assembly a)
		{
			lock (DynamicAssemblies.nameToAssemblyMap)
			{
				if (DynamicAssemblies.assemblyToNameMap[a] == null)
				{
					Assembly assembly = DynamicAssemblies.nameToAssemblyMap[a.FullName] as Assembly;
					string text = null;
					if (assembly == null)
					{
						text = a.FullName;
					}
					else if (assembly != a)
					{
						text = a.FullName + ", " + DynamicAssemblies.nameToAssemblyMap.Count;
					}
					if (text != null)
					{
						DynamicAssemblies.nameToAssemblyMap.Add(text, a);
						DynamicAssemblies.assemblyToNameMap.Add(a, text);
					}
				}
			}
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000CEF6C File Offset: 0x000CDF6C
		internal static Assembly Get(string fullName)
		{
			if (DynamicAssemblies.nameToAssemblyMap == null)
			{
				return null;
			}
			return (Assembly)DynamicAssemblies.nameToAssemblyMap[fullName];
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000CEF87 File Offset: 0x000CDF87
		internal static string GetName(Assembly a)
		{
			if (DynamicAssemblies.assemblyToNameMap == null)
			{
				return null;
			}
			return (string)DynamicAssemblies.assemblyToNameMap[a];
		}

		// Token: 0x04001665 RID: 5733
		private static ArrayList assembliesInConfig = new ArrayList();

		// Token: 0x04001666 RID: 5734
		private static Hashtable nameToAssemblyMap = new Hashtable();

		// Token: 0x04001667 RID: 5735
		private static Hashtable assemblyToNameMap = new Hashtable();

		// Token: 0x04001668 RID: 5736
		private static Hashtable tableIsTypeDynamic = Hashtable.Synchronized(new Hashtable());

		// Token: 0x04001669 RID: 5737
		private static FileIOPermission fileIOPermission;
	}
}
