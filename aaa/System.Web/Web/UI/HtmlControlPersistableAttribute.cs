using System;

namespace System.Web.UI
{
	// Token: 0x020003FC RID: 1020
	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class HtmlControlPersistableAttribute : Attribute
	{
		// Token: 0x06003254 RID: 12884 RVA: 0x000DC79A File Offset: 0x000DB79A
		internal HtmlControlPersistableAttribute(bool persistable)
		{
			this.persistable = persistable;
		}

		// Token: 0x17000B16 RID: 2838
		// (get) Token: 0x06003255 RID: 12885 RVA: 0x000DC7B0 File Offset: 0x000DB7B0
		internal bool HtmlControlPersistable
		{
			get
			{
				return this.persistable;
			}
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x000DC7B8 File Offset: 0x000DB7B8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			HtmlControlPersistableAttribute htmlControlPersistableAttribute = obj as HtmlControlPersistableAttribute;
			return htmlControlPersistableAttribute != null && htmlControlPersistableAttribute.HtmlControlPersistable == this.persistable;
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x000DC7E5 File Offset: 0x000DB7E5
		public override int GetHashCode()
		{
			return this.persistable.GetHashCode();
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x000DC7F2 File Offset: 0x000DB7F2
		public override bool IsDefaultAttribute()
		{
			return this.Equals(HtmlControlPersistableAttribute.Default);
		}

		// Token: 0x040022FD RID: 8957
		internal static readonly HtmlControlPersistableAttribute Yes = new HtmlControlPersistableAttribute(true);

		// Token: 0x040022FE RID: 8958
		internal static readonly HtmlControlPersistableAttribute No = new HtmlControlPersistableAttribute(false);

		// Token: 0x040022FF RID: 8959
		internal static readonly HtmlControlPersistableAttribute Default = HtmlControlPersistableAttribute.Yes;

		// Token: 0x04002300 RID: 8960
		private bool persistable = true;
	}
}
