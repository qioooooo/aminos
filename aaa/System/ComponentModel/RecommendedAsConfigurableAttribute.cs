using System;

namespace System.ComponentModel
{
	// Token: 0x02000130 RID: 304
	[Obsolete("Use System.ComponentModel.SettingsBindableAttribute instead to work with the new settings model.")]
	[AttributeUsage(AttributeTargets.Property)]
	public class RecommendedAsConfigurableAttribute : Attribute
	{
		// Token: 0x060009BD RID: 2493 RVA: 0x00020233 File Offset: 0x0001F233
		public RecommendedAsConfigurableAttribute(bool recommendedAsConfigurable)
		{
			this.recommendedAsConfigurable = recommendedAsConfigurable;
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x00020242 File Offset: 0x0001F242
		public bool RecommendedAsConfigurable
		{
			get
			{
				return this.recommendedAsConfigurable;
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0002024C File Offset: 0x0001F24C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RecommendedAsConfigurableAttribute recommendedAsConfigurableAttribute = obj as RecommendedAsConfigurableAttribute;
			return recommendedAsConfigurableAttribute != null && recommendedAsConfigurableAttribute.RecommendedAsConfigurable == this.recommendedAsConfigurable;
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00020279 File Offset: 0x0001F279
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00020281 File Offset: 0x0001F281
		public override bool IsDefaultAttribute()
		{
			return !this.recommendedAsConfigurable;
		}

		// Token: 0x04000A22 RID: 2594
		private bool recommendedAsConfigurable;

		// Token: 0x04000A23 RID: 2595
		public static readonly RecommendedAsConfigurableAttribute No = new RecommendedAsConfigurableAttribute(false);

		// Token: 0x04000A24 RID: 2596
		public static readonly RecommendedAsConfigurableAttribute Yes = new RecommendedAsConfigurableAttribute(true);

		// Token: 0x04000A25 RID: 2597
		public static readonly RecommendedAsConfigurableAttribute Default = RecommendedAsConfigurableAttribute.No;
	}
}
