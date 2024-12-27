using System;
using System.Reflection;

namespace Microsoft.CSharp
{
	// Token: 0x020002BB RID: 699
	internal class CSharpTypeAttributeConverter : CSharpModifierAttributeConverter
	{
		// Token: 0x0600177B RID: 6011 RVA: 0x0004E4C8 File Offset: 0x0004D4C8
		private CSharpTypeAttributeConverter()
		{
		}

		// Token: 0x17000491 RID: 1169
		// (get) Token: 0x0600177C RID: 6012 RVA: 0x0004E4D0 File Offset: 0x0004D4D0
		public static CSharpTypeAttributeConverter Default
		{
			get
			{
				if (CSharpTypeAttributeConverter.defaultConverter == null)
				{
					CSharpTypeAttributeConverter.defaultConverter = new CSharpTypeAttributeConverter();
				}
				return CSharpTypeAttributeConverter.defaultConverter;
			}
		}

		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x0600177D RID: 6013 RVA: 0x0004E4E8 File Offset: 0x0004D4E8
		protected override string[] Names
		{
			get
			{
				if (CSharpTypeAttributeConverter.names == null)
				{
					CSharpTypeAttributeConverter.names = new string[] { "Public", "Internal" };
				}
				return CSharpTypeAttributeConverter.names;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x0600177E RID: 6014 RVA: 0x0004E520 File Offset: 0x0004D520
		protected override object[] Values
		{
			get
			{
				if (CSharpTypeAttributeConverter.values == null)
				{
					CSharpTypeAttributeConverter.values = new object[]
					{
						TypeAttributes.Public,
						TypeAttributes.NotPublic
					};
				}
				return CSharpTypeAttributeConverter.values;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x0004E558 File Offset: 0x0004D558
		protected override object DefaultValue
		{
			get
			{
				return TypeAttributes.NotPublic;
			}
		}

		// Token: 0x0400160E RID: 5646
		private static string[] names;

		// Token: 0x0400160F RID: 5647
		private static object[] values;

		// Token: 0x04001610 RID: 5648
		private static CSharpTypeAttributeConverter defaultConverter;
	}
}
