using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000148 RID: 328
	public abstract class DesignerDataStoredProcedure
	{
		// Token: 0x06000C98 RID: 3224 RVA: 0x000309F7 File Offset: 0x0002F9F7
		protected DesignerDataStoredProcedure(string name)
		{
			this._name = name;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00030A06 File Offset: 0x0002FA06
		protected DesignerDataStoredProcedure(string name, string owner)
		{
			this._name = name;
			this._owner = owner;
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00030A1C File Offset: 0x0002FA1C
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00030A24 File Offset: 0x0002FA24
		public string Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x00030A2C File Offset: 0x0002FA2C
		public ICollection Parameters
		{
			get
			{
				if (this._parameters == null)
				{
					this._parameters = this.CreateParameters();
				}
				return this._parameters;
			}
		}

		// Token: 0x06000C9D RID: 3229
		protected abstract ICollection CreateParameters();

		// Token: 0x04000EB1 RID: 3761
		private string _name;

		// Token: 0x04000EB2 RID: 3762
		private string _owner;

		// Token: 0x04000EB3 RID: 3763
		private ICollection _parameters;
	}
}
