using System;
using System.CodeDom;

namespace Microsoft.CSharp
{
	// Token: 0x020002BC RID: 700
	internal class CSharpMemberAttributeConverter : CSharpModifierAttributeConverter
	{
		// Token: 0x06001780 RID: 6016 RVA: 0x0004E560 File Offset: 0x0004D560
		private CSharpMemberAttributeConverter()
		{
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x0004E568 File Offset: 0x0004D568
		public static CSharpMemberAttributeConverter Default
		{
			get
			{
				if (CSharpMemberAttributeConverter.defaultConverter == null)
				{
					CSharpMemberAttributeConverter.defaultConverter = new CSharpMemberAttributeConverter();
				}
				return CSharpMemberAttributeConverter.defaultConverter;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x0004E580 File Offset: 0x0004D580
		protected override string[] Names
		{
			get
			{
				if (CSharpMemberAttributeConverter.names == null)
				{
					CSharpMemberAttributeConverter.names = new string[] { "Public", "Protected", "Protected Internal", "Internal", "Private" };
				}
				return CSharpMemberAttributeConverter.names;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x0004E5D0 File Offset: 0x0004D5D0
		protected override object[] Values
		{
			get
			{
				if (CSharpMemberAttributeConverter.values == null)
				{
					CSharpMemberAttributeConverter.values = new object[]
					{
						MemberAttributes.Public,
						MemberAttributes.Family,
						MemberAttributes.FamilyOrAssembly,
						MemberAttributes.Assembly,
						MemberAttributes.Private
					};
				}
				return CSharpMemberAttributeConverter.values;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x0004E637 File Offset: 0x0004D637
		protected override object DefaultValue
		{
			get
			{
				return MemberAttributes.Private;
			}
		}

		// Token: 0x04001611 RID: 5649
		private static string[] names;

		// Token: 0x04001612 RID: 5650
		private static object[] values;

		// Token: 0x04001613 RID: 5651
		private static CSharpMemberAttributeConverter defaultConverter;
	}
}
