using System;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000584 RID: 1412
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class FormViewDeletedEventArgs : EventArgs
	{
		// Token: 0x060045B8 RID: 17848 RVA: 0x0011E8D9 File Offset: 0x0011D8D9
		public FormViewDeletedEventArgs(int affectedRows, Exception e)
		{
			this._affectedRows = affectedRows;
			this._exceptionHandled = false;
			this._exception = e;
		}

		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x060045B9 RID: 17849 RVA: 0x0011E8F6 File Offset: 0x0011D8F6
		public int AffectedRows
		{
			get
			{
				return this._affectedRows;
			}
		}

		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x060045BA RID: 17850 RVA: 0x0011E8FE File Offset: 0x0011D8FE
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x17001116 RID: 4374
		// (get) Token: 0x060045BB RID: 17851 RVA: 0x0011E906 File Offset: 0x0011D906
		// (set) Token: 0x060045BC RID: 17852 RVA: 0x0011E90E File Offset: 0x0011D90E
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

		// Token: 0x17001117 RID: 4375
		// (get) Token: 0x060045BD RID: 17853 RVA: 0x0011E917 File Offset: 0x0011D917
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

		// Token: 0x17001118 RID: 4376
		// (get) Token: 0x060045BE RID: 17854 RVA: 0x0011E932 File Offset: 0x0011D932
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

		// Token: 0x060045BF RID: 17855 RVA: 0x0011E94D File Offset: 0x0011D94D
		internal void SetKeys(IOrderedDictionary keys)
		{
			this._keys = keys;
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x0011E956 File Offset: 0x0011D956
		internal void SetValues(IOrderedDictionary values)
		{
			this._values = values;
		}

		// Token: 0x04002A0E RID: 10766
		private int _affectedRows;

		// Token: 0x04002A0F RID: 10767
		private Exception _exception;

		// Token: 0x04002A10 RID: 10768
		private bool _exceptionHandled;

		// Token: 0x04002A11 RID: 10769
		private IOrderedDictionary _keys;

		// Token: 0x04002A12 RID: 10770
		private IOrderedDictionary _values;
	}
}
