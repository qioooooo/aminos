using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000593 RID: 1427
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewUpdatedEventArgs : EventArgs
	{
		// Token: 0x060045F6 RID: 17910 RVA: 0x0011EB18 File Offset: 0x0011DB18
		public FormViewUpdatedEventArgs(int affectedRows, Exception e)
		{
			this._affectedRows = affectedRows;
			this._exceptionHandled = false;
			this._exception = e;
			this._keepInEditMode = false;
		}

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x060045F7 RID: 17911 RVA: 0x0011EB3C File Offset: 0x0011DB3C
		public int AffectedRows
		{
			get
			{
				return this._affectedRows;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x060045F8 RID: 17912 RVA: 0x0011EB44 File Offset: 0x0011DB44
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x1700112B RID: 4395
		// (get) Token: 0x060045F9 RID: 17913 RVA: 0x0011EB4C File Offset: 0x0011DB4C
		// (set) Token: 0x060045FA RID: 17914 RVA: 0x0011EB54 File Offset: 0x0011DB54
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

		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x060045FB RID: 17915 RVA: 0x0011EB5D File Offset: 0x0011DB5D
		// (set) Token: 0x060045FC RID: 17916 RVA: 0x0011EB65 File Offset: 0x0011DB65
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

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x060045FD RID: 17917 RVA: 0x0011EB6E File Offset: 0x0011DB6E
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

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x060045FE RID: 17918 RVA: 0x0011EB89 File Offset: 0x0011DB89
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

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x060045FF RID: 17919 RVA: 0x0011EBA4 File Offset: 0x0011DBA4
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

		// Token: 0x06004600 RID: 17920 RVA: 0x0011EBBF File Offset: 0x0011DBBF
		internal void SetKeys(IOrderedDictionary keys)
		{
			this._keys = keys;
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x0011EBC8 File Offset: 0x0011DBC8
		internal void SetNewValues(IOrderedDictionary newValues)
		{
			this._values = newValues;
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x0011EBD1 File Offset: 0x0011DBD1
		internal void SetOldValues(IOrderedDictionary oldValues)
		{
			this._oldValues = oldValues;
		}

		// Token: 0x04002A27 RID: 10791
		private int _affectedRows;

		// Token: 0x04002A28 RID: 10792
		private Exception _exception;

		// Token: 0x04002A29 RID: 10793
		private bool _exceptionHandled;

		// Token: 0x04002A2A RID: 10794
		private bool _keepInEditMode;

		// Token: 0x04002A2B RID: 10795
		private IOrderedDictionary _values;

		// Token: 0x04002A2C RID: 10796
		private IOrderedDictionary _keys;

		// Token: 0x04002A2D RID: 10797
		private IOrderedDictionary _oldValues;
	}
}
