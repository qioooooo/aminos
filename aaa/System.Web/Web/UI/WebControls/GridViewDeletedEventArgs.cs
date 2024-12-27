using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200059E RID: 1438
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewDeletedEventArgs : EventArgs
	{
		// Token: 0x060046F4 RID: 18164 RVA: 0x00123AC9 File Offset: 0x00122AC9
		public GridViewDeletedEventArgs(int affectedRows, Exception e)
		{
			this._affectedRows = affectedRows;
			this._exceptionHandled = false;
			this._exception = e;
		}

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x060046F5 RID: 18165 RVA: 0x00123AE6 File Offset: 0x00122AE6
		public int AffectedRows
		{
			get
			{
				return this._affectedRows;
			}
		}

		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x060046F6 RID: 18166 RVA: 0x00123AEE File Offset: 0x00122AEE
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x060046F7 RID: 18167 RVA: 0x00123AF6 File Offset: 0x00122AF6
		// (set) Token: 0x060046F8 RID: 18168 RVA: 0x00123AFE File Offset: 0x00122AFE
		public bool ExceptionHandled
		{
			get
			{
				return this._exceptionHandled;
			}
			set
			{
				this._exceptionHandled = value;
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x060046F9 RID: 18169 RVA: 0x00123B07 File Offset: 0x00122B07
		public IOrderedDictionary Keys
		{
			get
			{
				if (this._keys == null)
				{
					this._keys = new OrderedDictionary();
				}
				return this._keys;
			}
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x060046FA RID: 18170 RVA: 0x00123B22 File Offset: 0x00122B22
		public IOrderedDictionary Values
		{
			get
			{
				if (this._values == null)
				{
					this._values = new OrderedDictionary();
				}
				return this._values;
			}
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x00123B3D File Offset: 0x00122B3D
		internal void SetKeys(IOrderedDictionary keys)
		{
			this._keys = keys;
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x00123B46 File Offset: 0x00122B46
		internal void SetValues(IOrderedDictionary values)
		{
			this._values = values;
		}

		// Token: 0x04002A77 RID: 10871
		private int _affectedRows;

		// Token: 0x04002A78 RID: 10872
		private Exception _exception;

		// Token: 0x04002A79 RID: 10873
		private bool _exceptionHandled;

		// Token: 0x04002A7A RID: 10874
		private IOrderedDictionary _keys;

		// Token: 0x04002A7B RID: 10875
		private IOrderedDictionary _values;
	}
}
