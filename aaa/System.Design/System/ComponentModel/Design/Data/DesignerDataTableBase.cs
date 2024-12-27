using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000149 RID: 329
	public abstract class DesignerDataTableBase
	{
		// Token: 0x06000C9E RID: 3230 RVA: 0x00030A48 File Offset: 0x0002FA48
		protected DesignerDataTableBase(string name)
		{
			this._name = name;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x00030A57 File Offset: 0x0002FA57
		protected DesignerDataTableBase(string name, string owner)
		{
			this._name = name;
			this._owner = owner;
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00030A6D File Offset: 0x0002FA6D
		public ICollection Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = this.CreateColumns();
				}
				return this._columns;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000CA1 RID: 3233 RVA: 0x00030A89 File Offset: 0x0002FA89
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x00030A91 File Offset: 0x0002FA91
		public string Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x06000CA3 RID: 3235
		protected abstract ICollection CreateColumns();

		// Token: 0x04000EB4 RID: 3764
		private ICollection _columns;

		// Token: 0x04000EB5 RID: 3765
		private string _name;

		// Token: 0x04000EB6 RID: 3766
		private string _owner;
	}
}
