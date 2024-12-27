using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002EB RID: 747
	[ComVisible(true)]
	[Serializable]
	public struct CustomAttributeNamedArgument
	{
		// Token: 0x06001DEE RID: 7662 RVA: 0x0004AFFF File Offset: 0x00049FFF
		public static bool operator ==(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return left.Equals(right);
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x0004B014 File Offset: 0x0004A014
		public static bool operator !=(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return !left.Equals(right);
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x0004B02C File Offset: 0x0004A02C
		internal CustomAttributeNamedArgument(MemberInfo memberInfo, object value)
		{
			this.m_memberInfo = memberInfo;
			this.m_value = new CustomAttributeTypedArgument(value);
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x0004B041 File Offset: 0x0004A041
		internal CustomAttributeNamedArgument(MemberInfo memberInfo, CustomAttributeTypedArgument value)
		{
			this.m_memberInfo = memberInfo;
			this.m_value = value;
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x0004B054 File Offset: 0x0004A054
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} = {1}", new object[]
			{
				this.MemberInfo.Name,
				this.TypedValue.ToString(this.ArgumentType != typeof(object))
			});
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0004B0AC File Offset: 0x0004A0AC
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x0004B0BE File Offset: 0x0004A0BE
		public override bool Equals(object obj)
		{
			return obj == this;
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x0004B0CE File Offset: 0x0004A0CE
		internal Type ArgumentType
		{
			get
			{
				if (!(this.m_memberInfo is FieldInfo))
				{
					return ((PropertyInfo)this.m_memberInfo).PropertyType;
				}
				return ((FieldInfo)this.m_memberInfo).FieldType;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x0004B0FE File Offset: 0x0004A0FE
		public MemberInfo MemberInfo
		{
			get
			{
				return this.m_memberInfo;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x0004B106 File Offset: 0x0004A106
		public CustomAttributeTypedArgument TypedValue
		{
			get
			{
				return this.m_value;
			}
		}

		// Token: 0x04000AC6 RID: 2758
		private MemberInfo m_memberInfo;

		// Token: 0x04000AC7 RID: 2759
		private CustomAttributeTypedArgument m_value;
	}
}
