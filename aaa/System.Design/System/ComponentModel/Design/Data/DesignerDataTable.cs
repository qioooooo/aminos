using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x0200014A RID: 330
	public abstract class DesignerDataTable : DesignerDataTableBase
	{
		// Token: 0x06000CA4 RID: 3236 RVA: 0x00030A99 File Offset: 0x0002FA99
		protected DesignerDataTable(string name)
			: base(name)
		{
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00030AA2 File Offset: 0x0002FAA2
		protected DesignerDataTable(string name, string owner)
			: base(name, owner)
		{
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00030AAC File Offset: 0x0002FAAC
		public ICollection Relationships
		{
			get
			{
				if (this._relationships == null)
				{
					this._relationships = this.CreateRelationships();
				}
				return this._relationships;
			}
		}

		// Token: 0x06000CA7 RID: 3239
		protected abstract ICollection CreateRelationships();

		// Token: 0x04000EB7 RID: 3767
		private ICollection _relationships;
	}
}
