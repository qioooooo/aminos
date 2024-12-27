using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004E7 RID: 1255
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
	[ComVisible(true)]
	public sealed class StructLayoutAttribute : Attribute
	{
		// Token: 0x06003136 RID: 12598 RVA: 0x000A92B8 File Offset: 0x000A82B8
		internal static Attribute GetCustomAttribute(Type type)
		{
			if (!StructLayoutAttribute.IsDefined(type))
			{
				return null;
			}
			int num = 0;
			int num2 = 0;
			LayoutKind layoutKind = LayoutKind.Auto;
			TypeAttributes typeAttributes = type.Attributes & TypeAttributes.LayoutMask;
			if (typeAttributes != TypeAttributes.NotPublic)
			{
				if (typeAttributes != TypeAttributes.SequentialLayout)
				{
					if (typeAttributes == TypeAttributes.ExplicitLayout)
					{
						layoutKind = LayoutKind.Explicit;
					}
				}
				else
				{
					layoutKind = LayoutKind.Sequential;
				}
			}
			else
			{
				layoutKind = LayoutKind.Auto;
			}
			CharSet charSet = CharSet.None;
			TypeAttributes typeAttributes2 = type.Attributes & TypeAttributes.StringFormatMask;
			if (typeAttributes2 != TypeAttributes.NotPublic)
			{
				if (typeAttributes2 != TypeAttributes.UnicodeClass)
				{
					if (typeAttributes2 == TypeAttributes.AutoClass)
					{
						charSet = CharSet.Auto;
					}
				}
				else
				{
					charSet = CharSet.Unicode;
				}
			}
			else
			{
				charSet = CharSet.Ansi;
			}
			type.Module.MetadataImport.GetClassLayout(type.MetadataToken, out num, out num2);
			if (num == 0)
			{
				num = 8;
			}
			return new StructLayoutAttribute(layoutKind, num, num2, charSet);
		}

		// Token: 0x06003137 RID: 12599 RVA: 0x000A9359 File Offset: 0x000A8359
		internal static bool IsDefined(Type type)
		{
			return !type.IsInterface && !type.HasElementType && !type.IsGenericParameter;
		}

		// Token: 0x06003138 RID: 12600 RVA: 0x000A9376 File Offset: 0x000A8376
		internal StructLayoutAttribute(LayoutKind layoutKind, int pack, int size, CharSet charSet)
		{
			this._val = layoutKind;
			this.Pack = pack;
			this.Size = size;
			this.CharSet = charSet;
		}

		// Token: 0x06003139 RID: 12601 RVA: 0x000A939B File Offset: 0x000A839B
		public StructLayoutAttribute(LayoutKind layoutKind)
		{
			this._val = layoutKind;
		}

		// Token: 0x0600313A RID: 12602 RVA: 0x000A93AA File Offset: 0x000A83AA
		public StructLayoutAttribute(short layoutKind)
		{
			this._val = (LayoutKind)layoutKind;
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x0600313B RID: 12603 RVA: 0x000A93B9 File Offset: 0x000A83B9
		public LayoutKind Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x0400194A RID: 6474
		private const int DEFAULT_PACKING_SIZE = 8;

		// Token: 0x0400194B RID: 6475
		internal LayoutKind _val;

		// Token: 0x0400194C RID: 6476
		public int Pack;

		// Token: 0x0400194D RID: 6477
		public int Size;

		// Token: 0x0400194E RID: 6478
		public CharSet CharSet;
	}
}
