using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005AE RID: 1454
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class GridViewUpdatedEventArgs : EventArgs
	{
		// Token: 0x06004743 RID: 18243 RVA: 0x00123D88 File Offset: 0x00122D88
		public GridViewUpdatedEventArgs(int affectedRows, Exception e)
		{
			this._affectedRows = affectedRows;
			this._exceptionHandled = false;
			this._exception = e;
			this._keepInEditMode = false;
		}

		// Token: 0x1700118F RID: 4495
		// (get) Token: 0x06004744 RID: 18244 RVA: 0x00123DAC File Offset: 0x00122DAC
		public int AffectedRows
		{
			get
			{
				return this._affectedRows;
			}
		}

		// Token: 0x17001190 RID: 4496
		// (get) Token: 0x06004745 RID: 18245 RVA: 0x00123DB4 File Offset: 0x00122DB4
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x17001191 RID: 4497
		// (get) Token: 0x06004746 RID: 18246 RVA: 0x00123DBC File Offset: 0x00122DBC
		// (set) Token: 0x06004747 RID: 18247 RVA: 0x00123DC4 File Offset: 0x00122DC4
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

		// Token: 0x17001192 RID: 4498
		// (get) Token: 0x06004748 RID: 18248 RVA: 0x00123DCD File Offset: 0x00122DCD
		// (set) Token: 0x06004749 RID: 18249 RVA: 0x00123DD5 File Offset: 0x00122DD5
		public bool KeepInEditMode
		{
			get
			{
				return this._keepInEditMode;
			}
			set
			{
				this._keepInEditMode = value;
			}
		}

		// Token: 0x17001193 RID: 4499
		// (get) Token: 0x0600474A RID: 18250 RVA: 0x00123DDE File Offset: 0x00122DDE
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

		// Token: 0x17001194 RID: 4500
		// (get) Token: 0x0600474B RID: 18251 RVA: 0x00123DF9 File Offset: 0x00122DF9
		public IOrderedDictionary NewValues
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

		// Token: 0x17001195 RID: 4501
		// (get) Token: 0x0600474C RID: 18252 RVA: 0x00123E14 File Offset: 0x00122E14
		public IOrderedDictionary OldValues
		{
			get
			{
				if (this._oldValues == null)
				{
					this._oldValues = new OrderedDictionary();
				}
				return this._oldValues;
			}
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x00123E2F File Offset: 0x00122E2F
		internal void SetKeys(IOrderedDictionary keys)
		{
			this._keys = keys;
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x00123E38 File Offset: 0x00122E38
		internal void SetNewValues(IOrderedDictionary newValues)
		{
			this._values = newValues;
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x00123E41 File Offset: 0x00122E41
		internal void SetOldValues(IOrderedDictionary oldValues)
		{
			this._oldValues = oldValues;
		}

		// Token: 0x04002A8B RID: 10891
		private int _affectedRows;

		// Token: 0x04002A8C RID: 10892
		private Exception _exception;

		// Token: 0x04002A8D RID: 10893
		private bool _exceptionHandled;

		// Token: 0x04002A8E RID: 10894
		private IOrderedDictionary _values;

		// Token: 0x04002A8F RID: 10895
		private IOrderedDictionary _keys;

		// Token: 0x04002A90 RID: 10896
		private IOrderedDictionary _oldValues;

		// Token: 0x04002A91 RID: 10897
		private bool _keepInEditMode;
	}
}
