using System;
using System.Reflection;

namespace Microsoft.VisualBasic
{
	// Token: 0x020002C1 RID: 705
	internal class VBTypeAttributeConverter : VBModifierAttributeConverter
	{
		// Token: 0x06001811 RID: 6161 RVA: 0x000530D7 File Offset: 0x000520D7
		private VBTypeAttributeConverter()
		{
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x06001812 RID: 6162 RVA: 0x000530DF File Offset: 0x000520DF
		public static VBTypeAttributeConverter Default
		{
			get
			{
				if (VBTypeAttributeConverter.defaultConverter == null)
				{
					VBTypeAttributeConverter.defaultConverter = new VBTypeAttributeConverter();
				}
				return VBTypeAttributeConverter.defaultConverter;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x06001813 RID: 6163 RVA: 0x000530F8 File Offset: 0x000520F8
		protected override string[] Names
		{
			get
			{
				if (VBTypeAttributeConverter.names == null)
				{
					VBTypeAttributeConverter.names = new string[] { "Public", "Friend" };
				}
				return VBTypeAttributeConverter.names;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001814 RID: 6164 RVA: 0x00053130 File Offset: 0x00052130
		protected override object[] Values
		{
			get
			{
				if (VBTypeAttributeConverter.values == null)
				{
					VBTypeAttributeConverter.values = new object[]
					{
						TypeAttributes.Public,
						TypeAttributes.NotPublic
					};
				}
				return VBTypeAttributeConverter.values;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001815 RID: 6165 RVA: 0x00053168 File Offset: 0x00052168
		protected override object DefaultValue
		{
			get
			{
				return TypeAttributes.Public;
			}
		}

		// Token: 0x0400161E RID: 5662
		private static VBTypeAttributeConverter defaultConverter;

		// Token: 0x0400161F RID: 5663
		private static string[] names;

		// Token: 0x04001620 RID: 5664
		private static object[] values;
	}
}
