using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005FD RID: 1533
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ObjectDataSourceStatusEventArgs : EventArgs
	{
		// Token: 0x06004BB0 RID: 19376 RVA: 0x001337F8 File Offset: 0x001327F8
		public ObjectDataSourceStatusEventArgs(object returnValue, IDictionary outputParameters)
			: this(returnValue, outputParameters, null)
		{
		}

		// Token: 0x06004BB1 RID: 19377 RVA: 0x00133803 File Offset: 0x00132803
		public ObjectDataSourceStatusEventArgs(object returnValue, IDictionary outputParameters, Exception exception)
		{
			this._returnValue = returnValue;
			this._outputParameters = outputParameters;
			this._exception = exception;
		}

		// Token: 0x170012F4 RID: 4852
		// (get) Token: 0x06004BB2 RID: 19378 RVA: 0x00133827 File Offset: 0x00132827
		public IDictionary OutputParameters
		{
			get
			{
				return this._outputParameters;
			}
		}

		// Token: 0x170012F5 RID: 4853
		// (get) Token: 0x06004BB3 RID: 19379 RVA: 0x0013382F File Offset: 0x0013282F
		public Exception Exception
		{
			get
			{
				return this._exception;
			}
		}

		// Token: 0x170012F6 RID: 4854
		// (get) Token: 0x06004BB4 RID: 19380 RVA: 0x00133837 File Offset: 0x00132837
		// (set) Token: 0x06004BB5 RID: 19381 RVA: 0x0013383F File Offset: 0x0013283F
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

		// Token: 0x170012F7 RID: 4855
		// (get) Token: 0x06004BB6 RID: 19382 RVA: 0x00133848 File Offset: 0x00132848
		public object ReturnValue
		{
			get
			{
				return this._returnValue;
			}
		}

		// Token: 0x170012F8 RID: 4856
		// (get) Token: 0x06004BB7 RID: 19383 RVA: 0x00133850 File Offset: 0x00132850
		// (set) Token: 0x06004BB8 RID: 19384 RVA: 0x00133858 File Offset: 0x00132858
		public int AffectedRows
		{
			get
			{
				return this._affectedRows;
			}
			set
			{
				this._affectedRows = value;
			}
		}

		// Token: 0x04002BB1 RID: 11185
		private object _returnValue;

		// Token: 0x04002BB2 RID: 11186
		private IDictionary _outputParameters;

		// Token: 0x04002BB3 RID: 11187
		private Exception _exception;

		// Token: 0x04002BB4 RID: 11188
		private bool _exceptionHandled;

		// Token: 0x04002BB5 RID: 11189
		private int _affectedRows = -1;
	}
}
