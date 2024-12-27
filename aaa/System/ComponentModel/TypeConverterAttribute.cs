using System;
using System.Globalization;

namespace System.ComponentModel
{
	// Token: 0x02000142 RID: 322
	[AttributeUsage(AttributeTargets.All)]
	public sealed class TypeConverterAttribute : Attribute
	{
		// Token: 0x06000A6A RID: 2666 RVA: 0x000241D3 File Offset: 0x000231D3
		public TypeConverterAttribute()
		{
			this.typeName = string.Empty;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000241E6 File Offset: 0x000231E6
		public TypeConverterAttribute(Type type)
		{
			this.typeName = type.AssemblyQualifiedName;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x000241FA File Offset: 0x000231FA
		public TypeConverterAttribute(string typeName)
		{
			typeName.ToUpper(CultureInfo.InvariantCulture);
			this.typeName = typeName;
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00024215 File Offset: 0x00023215
		public string ConverterTypeName
		{
			get
			{
				return this.typeName;
			}
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00024220 File Offset: 0x00023220
		public override bool Equals(object obj)
		{
			TypeConverterAttribute typeConverterAttribute = obj as TypeConverterAttribute;
			return typeConverterAttribute != null && typeConverterAttribute.ConverterTypeName == this.typeName;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x0002424A File Offset: 0x0002324A
		public override int GetHashCode()
		{
			return this.typeName.GetHashCode();
		}

		// Token: 0x04000A74 RID: 2676
		private string typeName;

		// Token: 0x04000A75 RID: 2677
		public static readonly TypeConverterAttribute Default = new TypeConverterAttribute();
	}
}
