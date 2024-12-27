using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x0200036B RID: 875
	internal class TypeName
	{
		// Token: 0x060022B4 RID: 8884 RVA: 0x00058407 File Offset: 0x00057407
		private TypeName()
		{
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x00058410 File Offset: 0x00057410
		internal static Type GetType(Assembly initialAssembly, string fullTypeName)
		{
			Type typeFromCLSID = Type.GetTypeFromCLSID(new Guid(3089101169U, 8435, 4562, 141, 204, 0, 160, 201, 176, 5, 37));
			TypeName.ITypeNameFactory typeNameFactory = (TypeName.ITypeNameFactory)Activator.CreateInstance(typeFromCLSID);
			int num;
			TypeName.ITypeName typeName = typeNameFactory.ParseTypeName(fullTypeName, out num);
			Type type = null;
			if (num == -1)
			{
				type = TypeName.LoadTypeWithPartialName(typeName, initialAssembly, fullTypeName);
			}
			return type;
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x00058480 File Offset: 0x00057480
		private static Type LoadTypeWithPartialName(TypeName.ITypeName typeNameInfo, Assembly initialAssembly, string fullTypeName)
		{
			uint num = typeNameInfo.GetNameCount();
			uint num2 = typeNameInfo.GetTypeArgumentCount();
			IntPtr[] array = new IntPtr[num];
			IntPtr[] array2 = new IntPtr[num2];
			Type type2;
			try
			{
				if (num == 0U)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[] { fullTypeName }));
				}
				GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				num = typeNameInfo.GetNames(num, gchandle.AddrOfPinnedObject());
				gchandle.Free();
				string text = Marshal.PtrToStringBSTR(array[0]);
				string assemblyName = typeNameInfo.GetAssemblyName();
				Type type;
				if (!string.IsNullOrEmpty(assemblyName))
				{
					Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
					if (assembly == null)
					{
						assembly = Assembly.LoadWithPartialName(new AssemblyName(assemblyName).Name);
					}
					type = assembly.GetType(text);
				}
				else if (initialAssembly != null)
				{
					type = initialAssembly.GetType(text);
				}
				else
				{
					type = Type.GetType(text);
				}
				if (type == null)
				{
					throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[] { fullTypeName }));
				}
				int num3 = 1;
				while ((long)num3 < (long)((ulong)num))
				{
					string text2 = Marshal.PtrToStringBSTR(array[num3]);
					type = type.GetNestedType(text2, BindingFlags.Public | BindingFlags.NonPublic);
					if (type == null)
					{
						throw new SerializationException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[] { fullTypeName }));
					}
					num3++;
				}
				if (num2 != 0U)
				{
					GCHandle gchandle2 = GCHandle.Alloc(array2, GCHandleType.Pinned);
					num2 = typeNameInfo.GetTypeArguments(num2, gchandle2.AddrOfPinnedObject());
					gchandle2.Free();
					Type[] array3 = new Type[num2];
					int num4 = 0;
					while ((long)num4 < (long)((ulong)num2))
					{
						array3[num4] = TypeName.LoadTypeWithPartialName((TypeName.ITypeName)Marshal.GetObjectForIUnknown(array2[num4]), null, fullTypeName);
						num4++;
					}
					type2 = type.MakeGenericType(array3);
				}
				else
				{
					type2 = type;
				}
			}
			finally
			{
				for (int i = 0; i < array.Length; i++)
				{
					IntPtr intPtr = array[i];
					Marshal.FreeBSTR(array[i]);
				}
				for (int j = 0; j < array2.Length; j++)
				{
					IntPtr intPtr2 = array2[j];
					Marshal.Release(array2[j]);
				}
			}
			return type2;
		}

		// Token: 0x0200036C RID: 876
		[Guid("B81FF171-20F3-11D2-8DCC-00A0C9B00522")]
		[TypeLibType(256)]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ITypeName
		{
			// Token: 0x060022B7 RID: 8887
			uint GetNameCount();

			// Token: 0x060022B8 RID: 8888
			uint GetNames([In] uint count, IntPtr rgbszNamesArray);

			// Token: 0x060022B9 RID: 8889
			uint GetTypeArgumentCount();

			// Token: 0x060022BA RID: 8890
			uint GetTypeArguments([In] uint count, IntPtr rgpArgumentsArray);

			// Token: 0x060022BB RID: 8891
			uint GetModifierLength();

			// Token: 0x060022BC RID: 8892
			uint GetModifiers([In] uint count, out uint rgModifiers);

			// Token: 0x060022BD RID: 8893
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetAssemblyName();
		}

		// Token: 0x0200036D RID: 877
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[TypeLibType(256)]
		[Guid("B81FF171-20F3-11D2-8DCC-00A0C9B00521")]
		[ComImport]
		internal interface ITypeNameFactory
		{
			// Token: 0x060022BE RID: 8894
			[return: MarshalAs(UnmanagedType.Interface)]
			TypeName.ITypeName ParseTypeName([MarshalAs(UnmanagedType.LPWStr)] [In] string szName, out int pError);
		}
	}
}
