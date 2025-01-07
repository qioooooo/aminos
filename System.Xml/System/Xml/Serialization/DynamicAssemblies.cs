using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Permissions;

namespace System.Xml.Serialization
{
	internal static class DynamicAssemblies
	{
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

		internal static Assembly Get(string fullName)
		{
			if (DynamicAssemblies.nameToAssemblyMap == null)
			{
				return null;
			}
			return (Assembly)DynamicAssemblies.nameToAssemblyMap[fullName];
		}

		internal static string GetName(Assembly a)
		{
			if (DynamicAssemblies.assemblyToNameMap == null)
			{
				return null;
			}
			return (string)DynamicAssemblies.assemblyToNameMap[a];
		}

		private static ArrayList assembliesInConfig = new ArrayList();

		private static Hashtable nameToAssemblyMap = new Hashtable();

		private static Hashtable assemblyToNameMap = new Hashtable();

		private static Hashtable tableIsTypeDynamic = Hashtable.Synchronized(new Hashtable());

		private static FileIOPermission fileIOPermission;
	}
}
