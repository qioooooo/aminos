using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000588 RID: 1416
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewInsertedEventArgs : EventArgs
	{
		// Token: 0x060045CD RID: 17869 RVA: 0x0011E9AD File Offset: 0x0011D9AD
		public FormViewInsertedEventArgs(int affectedRows, Exception e)
		{
			this._affectedRows = affectedRows;
			this._exceptionHandled = false;
			this._exception = e;
			this._keepInInsertMode = false;
		}

		// Token: 0x1700111C RID: 4380
		// (get) Token: 0x060045CE RID: 17870 RVA: 0x0011E9D1 File Offset: 0x0011D9D1
		public int AffectedRows
		{
			get
			{
				return this._affectedRows;
			}
		}

		// Token: 0x1700111D RID: 4381
		// (get) Token: 0x060045CF RID: 17871 RVA: 0x0011E9D9 File Offset: 0x0011D9D9
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x1700111E RID: 4382
		// (get) Token: 0x060045D0 RID: 17872 RVA: 0x0011E9E1 File Offset: 0x0011D9E1
		// (set) Token: 0x060045D1 RID: 17873 RVA: 0x0011E9E9 File Offset: 0x0011D9E9
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

		// Token: 0x1700111F RID: 4383
		// (get) Token: 0x060045D2 RID: 17874 RVA: 0x0011E9F2 File Offset: 0x0011D9F2
		// (set) Token: 0x060045D3 RID: 17875 RVA: 0x0011E9FA File Offset: 0x0011D9FA
		public bool KeepInInsertMode
		{
			get
			{
				return this._keepInInsertMode;
			}
			set
			{
				this._keepInInsertMode = value;
			}
		}

		// Token: 0x17001120 RID: 4384
		// (get) Token: 0x060045D4 RID: 17876 RVA: 0x0011EA03 File Offset: 0x0011DA03
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

		// Token: 0x060045D5 RID: 17877 RVA: 0x0011EA1E File Offset: 0x0011DA1E
		internal void SetValues(IOrderedDictionary values)
		{
			this._values = values;
		}

		// Token: 0x04002A16 RID: 10774
		private int _affectedRows;

		// Token: 0x04002A17 RID: 10775
		private Exception _exception;

		// Token: 0x04002A18 RID: 10776
		private bool _exceptionHandled;

		// Token: 0x04002A19 RID: 10777
		private bool _keepInInsertMode;

		// Token: 0x04002A1A RID: 10778
		private IOrderedDictionary _values;
	}
}
