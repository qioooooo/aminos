using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x0200055E RID: 1374
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class DetailsViewInsertedEventArgs : EventArgs
	{
		// Token: 0x06004410 RID: 17424 RVA: 0x001197DD File Offset: 0x001187DD
		public DetailsViewInsertedEventArgs(int affectedRows, Exception e)
		{
			this._affectedRows = affectedRows;
			this._exceptionHandled = false;
			this._exception = e;
			this._keepInInsertMode = false;
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06004411 RID: 17425 RVA: 0x00119801 File Offset: 0x00118801
		public int AffectedRows
		{
			get
			{
				return this._affectedRows;
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x06004412 RID: 17426 RVA: 0x00119809 File Offset: 0x00118809
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x06004413 RID: 17427 RVA: 0x00119811 File Offset: 0x00118811
		// (set) Token: 0x06004414 RID: 17428 RVA: 0x00119819 File Offset: 0x00118819
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

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x06004415 RID: 17429 RVA: 0x00119822 File Offset: 0x00118822
		// (set) Token: 0x06004416 RID: 17430 RVA: 0x0011982A File Offset: 0x0011882A
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

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06004417 RID: 17431 RVA: 0x00119833 File Offset: 0x00118833
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

		// Token: 0x06004418 RID: 17432 RVA: 0x0011984E File Offset: 0x0011884E
		internal void SetValues(IOrderedDictionary values)
		{
			this._values = values;
		}

		// Token: 0x04002995 RID: 10645
		private int _affectedRows;

		// Token: 0x04002996 RID: 10646
		private Exception _exception;

		// Token: 0x04002997 RID: 10647
		private bool _exceptionHandled;

		// Token: 0x04002998 RID: 10648
		private bool _keepInInsertMode;

		// Token: 0x04002999 RID: 10649
		private IOrderedDictionary _values;
	}
}
