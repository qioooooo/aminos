using System;
using System.CodeDom;

namespace Microsoft.VisualBasic
{
	// Token: 0x020002C0 RID: 704
	internal class VBMemberAttributeConverter : VBModifierAttributeConverter
	{
		// Token: 0x0600180C RID: 6156 RVA: 0x00052FF4 File Offset: 0x00051FF4
		private VBMemberAttributeConverter()
		{
		}

		// Token: 0x170004A2 RID: 1186
		// (get) Token: 0x0600180D RID: 6157 RVA: 0x00052FFC File Offset: 0x00051FFC
		public static VBMemberAttributeConverter Default
		{
			get
			{
				if (VBMemberAttributeConverter.defaultConverter == null)
				{
					VBMemberAttributeConverter.defaultConverter = new VBMemberAttributeConverter();
				}
				return VBMemberAttributeConverter.defaultConverter;
			}
		}

		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x0600180E RID: 6158 RVA: 0x00053014 File Offset: 0x00052014
		protected override string[] Names
		{
			get
			{
				if (VBMemberAttributeConverter.names == null)
				{
					VBMemberAttributeConverter.names = new string[] { "Public", "Protected", "Protected Friend", "Friend", "Private" };
				}
				return VBMemberAttributeConverter.names;
			}
		}

		// Token: 0x170004A4 RID: 1188
		// (get) Token: 0x0600180F RID: 6159 RVA: 0x00053064 File Offset: 0x00052064
		protected override object[] Values
		{
			get
			{
				if (VBMemberAttributeConverter.values == null)
				{
					VBMemberAttributeConverter.values = new object[]
					{
						MemberAttributes.Public,
						MemberAttributes.Family,
						MemberAttributes.FamilyOrAssembly,
						MemberAttributes.Assembly,
						MemberAttributes.Private
					};
				}
				return VBMemberAttributeConverter.values;
			}
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x06001810 RID: 6160 RVA: 0x000530CB File Offset: 0x000520CB
		protected override object DefaultValue
		{
			get
			{
				return MemberAttributes.Private;
			}
		}

		// Token: 0x0400161B RID: 5659
		private static string[] names;

		// Token: 0x0400161C RID: 5660
		private static object[] values;

		// Token: 0x0400161D RID: 5661
		private static VBMemberAttributeConverter defaultConverter;
	}
}
