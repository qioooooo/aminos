using System;

namespace System.Windows.Forms
{
	// Token: 0x02000332 RID: 818
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DataGridViewColumnDesignTimeVisibleAttribute : Attribute
	{
		// Token: 0x0600346E RID: 13422 RVA: 0x000B980B File Offset: 0x000B880B
		public DataGridViewColumnDesignTimeVisibleAttribute(bool visible)
		{
			this.visible = visible;
		}

		// Token: 0x0600346F RID: 13423 RVA: 0x000B981A File Offset: 0x000B881A
		public DataGridViewColumnDesignTimeVisibleAttribute()
		{
		}

		// Token: 0x17000970 RID: 2416
		// (get) Token: 0x06003470 RID: 13424 RVA: 0x000B9822 File Offset: 0x000B8822
		public bool Visible
		{
			get
			{
				return this.visible;
			}
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x000B982C File Offset: 0x000B882C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataGridViewColumnDesignTimeVisibleAttribute dataGridViewColumnDesignTimeVisibleAttribute = obj as DataGridViewColumnDesignTimeVisibleAttribute;
			return dataGridViewColumnDesignTimeVisibleAttribute != null && dataGridViewColumnDesignTimeVisibleAttribute.Visible == this.visible;
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x000B9859 File Offset: 0x000B8859
		public override int GetHashCode()
		{
			return typeof(DataGridViewColumnDesignTimeVisibleAttribute).GetHashCode() ^ (this.visible ? (-1) : 0);
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x000B9877 File Offset: 0x000B8877
		public override bool IsDefaultAttribute()
		{
			return this.Visible == DataGridViewColumnDesignTimeVisibleAttribute.Default.Visible;
		}

		// Token: 0x04001B26 RID: 6950
		private bool visible;

		// Token: 0x04001B27 RID: 6951
		public static readonly DataGridViewColumnDesignTimeVisibleAttribute Yes = new DataGridViewColumnDesignTimeVisibleAttribute(true);

		// Token: 0x04001B28 RID: 6952
		public static readonly DataGridViewColumnDesignTimeVisibleAttribute No = new DataGridViewColumnDesignTimeVisibleAttribute(false);

		// Token: 0x04001B29 RID: 6953
		public static readonly DataGridViewColumnDesignTimeVisibleAttribute Default = DataGridViewColumnDesignTimeVisibleAttribute.Yes;
	}
}
