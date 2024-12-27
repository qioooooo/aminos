using System;

namespace System.ComponentModel
{
	// Token: 0x02000198 RID: 408
	[AttributeUsage(AttributeTargets.All)]
	public sealed class RefreshPropertiesAttribute : Attribute
	{
		// Token: 0x06000CC9 RID: 3273 RVA: 0x00029955 File Offset: 0x00028955
		public RefreshPropertiesAttribute(RefreshProperties refresh)
		{
			this.refresh = refresh;
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x00029964 File Offset: 0x00028964
		public RefreshProperties RefreshProperties
		{
			get
			{
				return this.refresh;
			}
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0002996C File Offset: 0x0002896C
		public override bool Equals(object value)
		{
			return value is RefreshPropertiesAttribute && ((RefreshPropertiesAttribute)value).RefreshProperties == this.refresh;
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0002998B File Offset: 0x0002898B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x00029993 File Offset: 0x00028993
		public override bool IsDefaultAttribute()
		{
			return this.Equals(RefreshPropertiesAttribute.Default);
		}

		// Token: 0x04000AF3 RID: 2803
		public static readonly RefreshPropertiesAttribute All = new RefreshPropertiesAttribute(RefreshProperties.All);

		// Token: 0x04000AF4 RID: 2804
		public static readonly RefreshPropertiesAttribute Repaint = new RefreshPropertiesAttribute(RefreshProperties.Repaint);

		// Token: 0x04000AF5 RID: 2805
		public static readonly RefreshPropertiesAttribute Default = new RefreshPropertiesAttribute(RefreshProperties.None);

		// Token: 0x04000AF6 RID: 2806
		private RefreshProperties refresh;
	}
}
