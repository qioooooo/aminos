using System;

namespace System.ComponentModel
{
	// Token: 0x02000005 RID: 5
	[AttributeUsage(AttributeTargets.All)]
	public class DescriptionAttribute : Attribute
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		public DescriptionAttribute()
			: this(string.Empty)
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020DD File Offset: 0x000010DD
		public DescriptionAttribute(string description)
		{
			this.description = description;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020EC File Offset: 0x000010EC
		public virtual string Description
		{
			get
			{
				return this.DescriptionValue;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020F4 File Offset: 0x000010F4
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020FC File Offset: 0x000010FC
		protected string DescriptionValue
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002108 File Offset: 0x00001108
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DescriptionAttribute descriptionAttribute = obj as DescriptionAttribute;
			return descriptionAttribute != null && descriptionAttribute.Description == this.Description;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002138 File Offset: 0x00001138
		public override int GetHashCode()
		{
			return this.Description.GetHashCode();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002145 File Offset: 0x00001145
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DescriptionAttribute.Default);
		}

		// Token: 0x04000035 RID: 53
		public static readonly DescriptionAttribute Default = new DescriptionAttribute();

		// Token: 0x04000036 RID: 54
		private string description;
	}
}
