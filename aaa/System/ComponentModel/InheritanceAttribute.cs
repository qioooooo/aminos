using System;

namespace System.ComponentModel
{
	// Token: 0x02000189 RID: 393
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
	public sealed class InheritanceAttribute : Attribute
	{
		// Token: 0x06000C86 RID: 3206 RVA: 0x0002941C File Offset: 0x0002841C
		public InheritanceAttribute()
		{
			this.inheritanceLevel = InheritanceAttribute.Default.inheritanceLevel;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x00029434 File Offset: 0x00028434
		public InheritanceAttribute(InheritanceLevel inheritanceLevel)
		{
			this.inheritanceLevel = inheritanceLevel;
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x00029443 File Offset: 0x00028443
		public InheritanceLevel InheritanceLevel
		{
			get
			{
				return this.inheritanceLevel;
			}
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0002944C File Offset: 0x0002844C
		public override bool Equals(object value)
		{
			if (value == this)
			{
				return true;
			}
			if (!(value is InheritanceAttribute))
			{
				return false;
			}
			InheritanceLevel inheritanceLevel = ((InheritanceAttribute)value).InheritanceLevel;
			return inheritanceLevel == this.inheritanceLevel;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0002947E File Offset: 0x0002847E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x00029486 File Offset: 0x00028486
		public override bool IsDefaultAttribute()
		{
			return this.Equals(InheritanceAttribute.Default);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00029493 File Offset: 0x00028493
		public override string ToString()
		{
			return TypeDescriptor.GetConverter(typeof(InheritanceLevel)).ConvertToString(this.InheritanceLevel);
		}

		// Token: 0x04000AD8 RID: 2776
		private readonly InheritanceLevel inheritanceLevel;

		// Token: 0x04000AD9 RID: 2777
		public static readonly InheritanceAttribute Inherited = new InheritanceAttribute(InheritanceLevel.Inherited);

		// Token: 0x04000ADA RID: 2778
		public static readonly InheritanceAttribute InheritedReadOnly = new InheritanceAttribute(InheritanceLevel.InheritedReadOnly);

		// Token: 0x04000ADB RID: 2779
		public static readonly InheritanceAttribute NotInherited = new InheritanceAttribute(InheritanceLevel.NotInherited);

		// Token: 0x04000ADC RID: 2780
		public static readonly InheritanceAttribute Default = InheritanceAttribute.NotInherited;
	}
}
