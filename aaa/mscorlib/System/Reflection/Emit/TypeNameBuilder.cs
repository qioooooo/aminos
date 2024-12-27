using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System.Reflection.Emit
{
	// Token: 0x020007EF RID: 2031
	internal class TypeNameBuilder
	{
		// Token: 0x06004868 RID: 18536
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr _CreateTypeNameBuilder();

		// Token: 0x06004869 RID: 18537
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _ReleaseTypeNameBuilder(IntPtr pAQN);

		// Token: 0x0600486A RID: 18538
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _OpenGenericArguments(IntPtr tnb);

		// Token: 0x0600486B RID: 18539
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _CloseGenericArguments(IntPtr tnb);

		// Token: 0x0600486C RID: 18540
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _OpenGenericArgument(IntPtr tnb);

		// Token: 0x0600486D RID: 18541
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _CloseGenericArgument(IntPtr tnb);

		// Token: 0x0600486E RID: 18542
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddName(IntPtr tnb, string name);

		// Token: 0x0600486F RID: 18543
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddPointer(IntPtr tnb);

		// Token: 0x06004870 RID: 18544
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddByRef(IntPtr tnb);

		// Token: 0x06004871 RID: 18545
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddSzArray(IntPtr tnb);

		// Token: 0x06004872 RID: 18546
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddArray(IntPtr tnb, int rank);

		// Token: 0x06004873 RID: 18547
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _AddAssemblySpec(IntPtr tnb, string assemblySpec);

		// Token: 0x06004874 RID: 18548
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string _ToString(IntPtr tnb);

		// Token: 0x06004875 RID: 18549
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _Clear(IntPtr tnb);

		// Token: 0x06004876 RID: 18550 RVA: 0x000FD208 File Offset: 0x000FC208
		internal static string ToString(Type type, TypeNameBuilder.Format format)
		{
			if ((format == TypeNameBuilder.Format.FullName || format == TypeNameBuilder.Format.AssemblyQualifiedName) && !type.IsGenericTypeDefinition && type.ContainsGenericParameters)
			{
				return null;
			}
			TypeNameBuilder typeNameBuilder = new TypeNameBuilder(TypeNameBuilder._CreateTypeNameBuilder());
			typeNameBuilder.Clear();
			typeNameBuilder.ConstructAssemblyQualifiedNameWorker(type, format);
			string text = typeNameBuilder.ToString();
			typeNameBuilder.Dispose();
			return text;
		}

		// Token: 0x06004877 RID: 18551 RVA: 0x000FD256 File Offset: 0x000FC256
		private TypeNameBuilder(IntPtr typeNameBuilder)
		{
			this.m_typeNameBuilder = typeNameBuilder;
		}

		// Token: 0x06004878 RID: 18552 RVA: 0x000FD265 File Offset: 0x000FC265
		internal void Dispose()
		{
			TypeNameBuilder._ReleaseTypeNameBuilder(this.m_typeNameBuilder);
		}

		// Token: 0x06004879 RID: 18553 RVA: 0x000FD274 File Offset: 0x000FC274
		private void AddElementType(Type elementType)
		{
			if (elementType.HasElementType)
			{
				this.AddElementType(elementType.GetElementType());
			}
			if (elementType.IsPointer)
			{
				this.AddPointer();
				return;
			}
			if (elementType.IsByRef)
			{
				this.AddByRef();
				return;
			}
			if (elementType.IsSzArray)
			{
				this.AddSzArray();
				return;
			}
			if (elementType.IsArray)
			{
				this.AddArray(elementType.GetArrayRank());
			}
		}

		// Token: 0x0600487A RID: 18554 RVA: 0x000FD2D8 File Offset: 0x000FC2D8
		private void ConstructAssemblyQualifiedNameWorker(Type type, TypeNameBuilder.Format format)
		{
			Type type2 = type;
			while (type2.HasElementType)
			{
				type2 = type2.GetElementType();
			}
			List<Type> list = new List<Type>();
			for (Type type3 = type2; type3 != null; type3 = (type3.IsGenericParameter ? null : type3.DeclaringType))
			{
				list.Add(type3);
			}
			for (int i = list.Count - 1; i >= 0; i--)
			{
				Type type4 = list[i];
				string text = type4.Name;
				if (i == list.Count - 1 && type4.Namespace != null && type4.Namespace.Length != 0)
				{
					text = type4.Namespace + "." + text;
				}
				this.AddName(text);
			}
			if (type2.IsGenericType && (!type2.IsGenericTypeDefinition || format == TypeNameBuilder.Format.ToString))
			{
				Type[] genericArguments = type2.GetGenericArguments();
				this.OpenGenericArguments();
				for (int j = 0; j < genericArguments.Length; j++)
				{
					TypeNameBuilder.Format format2 = ((format == TypeNameBuilder.Format.FullName) ? TypeNameBuilder.Format.AssemblyQualifiedName : format);
					this.OpenGenericArgument();
					this.ConstructAssemblyQualifiedNameWorker(genericArguments[j], format2);
					this.CloseGenericArgument();
				}
				this.CloseGenericArguments();
			}
			this.AddElementType(type);
			if (format == TypeNameBuilder.Format.AssemblyQualifiedName)
			{
				this.AddAssemblySpec(type.Module.Assembly.FullName);
			}
		}

		// Token: 0x0600487B RID: 18555 RVA: 0x000FD400 File Offset: 0x000FC400
		private void OpenGenericArguments()
		{
			TypeNameBuilder._OpenGenericArguments(this.m_typeNameBuilder);
		}

		// Token: 0x0600487C RID: 18556 RVA: 0x000FD40D File Offset: 0x000FC40D
		private void CloseGenericArguments()
		{
			TypeNameBuilder._CloseGenericArguments(this.m_typeNameBuilder);
		}

		// Token: 0x0600487D RID: 18557 RVA: 0x000FD41A File Offset: 0x000FC41A
		private void OpenGenericArgument()
		{
			TypeNameBuilder._OpenGenericArgument(this.m_typeNameBuilder);
		}

		// Token: 0x0600487E RID: 18558 RVA: 0x000FD427 File Offset: 0x000FC427
		private void CloseGenericArgument()
		{
			TypeNameBuilder._CloseGenericArgument(this.m_typeNameBuilder);
		}

		// Token: 0x0600487F RID: 18559 RVA: 0x000FD434 File Offset: 0x000FC434
		private void AddName(string name)
		{
			TypeNameBuilder._AddName(this.m_typeNameBuilder, name);
		}

		// Token: 0x06004880 RID: 18560 RVA: 0x000FD442 File Offset: 0x000FC442
		private void AddPointer()
		{
			TypeNameBuilder._AddPointer(this.m_typeNameBuilder);
		}

		// Token: 0x06004881 RID: 18561 RVA: 0x000FD44F File Offset: 0x000FC44F
		private void AddByRef()
		{
			TypeNameBuilder._AddByRef(this.m_typeNameBuilder);
		}

		// Token: 0x06004882 RID: 18562 RVA: 0x000FD45C File Offset: 0x000FC45C
		private void AddSzArray()
		{
			TypeNameBuilder._AddSzArray(this.m_typeNameBuilder);
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x000FD469 File Offset: 0x000FC469
		private void AddArray(int rank)
		{
			TypeNameBuilder._AddArray(this.m_typeNameBuilder, rank);
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x000FD477 File Offset: 0x000FC477
		private void AddAssemblySpec(string assemblySpec)
		{
			TypeNameBuilder._AddAssemblySpec(this.m_typeNameBuilder, assemblySpec);
		}

		// Token: 0x06004885 RID: 18565 RVA: 0x000FD485 File Offset: 0x000FC485
		public override string ToString()
		{
			return TypeNameBuilder._ToString(this.m_typeNameBuilder);
		}

		// Token: 0x06004886 RID: 18566 RVA: 0x000FD492 File Offset: 0x000FC492
		private void Clear()
		{
			TypeNameBuilder._Clear(this.m_typeNameBuilder);
		}

		// Token: 0x0400253A RID: 9530
		private IntPtr m_typeNameBuilder;

		// Token: 0x020007F0 RID: 2032
		internal enum Format
		{
			// Token: 0x0400253C RID: 9532
			ToString,
			// Token: 0x0400253D RID: 9533
			FullName,
			// Token: 0x0400253E RID: 9534
			AssemblyQualifiedName
		}
	}
}
