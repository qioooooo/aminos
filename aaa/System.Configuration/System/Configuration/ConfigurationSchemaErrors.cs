using System;
using System.Collections.Generic;

namespace System.Configuration
{
	// Token: 0x0200003C RID: 60
	internal class ConfigurationSchemaErrors
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x00011162 File Offset: 0x00010162
		internal ConfigurationSchemaErrors()
		{
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002CA RID: 714 RVA: 0x0001116A File Offset: 0x0001016A
		internal bool HasLocalErrors
		{
			get
			{
				return ErrorsHelper.GetHasErrors(this._errorsLocal);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002CB RID: 715 RVA: 0x00011177 File Offset: 0x00010177
		internal bool HasGlobalErrors
		{
			get
			{
				return ErrorsHelper.GetHasErrors(this._errorsGlobal);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00011184 File Offset: 0x00010184
		private bool HasAllErrors
		{
			get
			{
				return ErrorsHelper.GetHasErrors(this._errorsAll);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060002CD RID: 717 RVA: 0x00011191 File Offset: 0x00010191
		internal int GlobalErrorCount
		{
			get
			{
				return ErrorsHelper.GetErrorCount(this._errorsGlobal);
			}
		}

		// Token: 0x060002CE RID: 718 RVA: 0x000111A0 File Offset: 0x000101A0
		internal void AddError(ConfigurationException ce, ExceptionAction action)
		{
			switch (action)
			{
			case ExceptionAction.NonSpecific:
				ErrorsHelper.AddError(ref this._errorsAll, ce);
				return;
			case ExceptionAction.Local:
				ErrorsHelper.AddError(ref this._errorsLocal, ce);
				return;
			case ExceptionAction.Global:
				ErrorsHelper.AddError(ref this._errorsAll, ce);
				ErrorsHelper.AddError(ref this._errorsGlobal, ce);
				return;
			default:
				return;
			}
		}

		// Token: 0x060002CF RID: 719 RVA: 0x000111F4 File Offset: 0x000101F4
		internal void SetSingleGlobalError(ConfigurationException ce)
		{
			this._errorsAll = null;
			this._errorsLocal = null;
			this._errorsGlobal = null;
			this.AddError(ce, ExceptionAction.Global);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00011213 File Offset: 0x00010213
		internal bool HasErrors(bool ignoreLocal)
		{
			if (ignoreLocal)
			{
				return this.HasGlobalErrors;
			}
			return this.HasAllErrors;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00011225 File Offset: 0x00010225
		internal void ThrowIfErrors(bool ignoreLocal)
		{
			if (!this.HasErrors(ignoreLocal))
			{
				return;
			}
			if (this.HasGlobalErrors)
			{
				throw new ConfigurationErrorsException(this._errorsGlobal);
			}
			throw new ConfigurationErrorsException(this._errorsAll);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00011250 File Offset: 0x00010250
		internal List<ConfigurationException> RetrieveAndResetLocalErrors(bool keepLocalErrors)
		{
			List<ConfigurationException> errorsLocal = this._errorsLocal;
			this._errorsLocal = null;
			if (keepLocalErrors)
			{
				ErrorsHelper.AddErrors(ref this._errorsAll, errorsLocal);
			}
			return errorsLocal;
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0001127B File Offset: 0x0001027B
		internal void AddSavedLocalErrors(ICollection<ConfigurationException> coll)
		{
			ErrorsHelper.AddErrors(ref this._errorsAll, coll);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00011289 File Offset: 0x00010289
		internal void ResetLocalErrors()
		{
			this.RetrieveAndResetLocalErrors(false);
		}

		// Token: 0x04000293 RID: 659
		private List<ConfigurationException> _errorsLocal;

		// Token: 0x04000294 RID: 660
		private List<ConfigurationException> _errorsGlobal;

		// Token: 0x04000295 RID: 661
		private List<ConfigurationException> _errorsAll;
	}
}
