using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	public sealed class LoadedEventArgs : EventArgs
	{
		public LoadedEventArgs(bool succeeded, ICollection errors)
		{
			this._succeeded = succeeded;
			this._errors = errors;
			if (this._errors == null)
			{
				this._errors = new object[0];
			}
		}

		public ICollection Errors
		{
			get
			{
				return this._errors;
			}
		}

		public bool HasSucceeded
		{
			get
			{
				return this._succeeded;
			}
		}

		private bool _succeeded;

		private ICollection _errors;
	}
}
