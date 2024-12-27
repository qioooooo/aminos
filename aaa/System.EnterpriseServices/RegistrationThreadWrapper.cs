using System;

namespace System.EnterpriseServices
{
	// Token: 0x0200008A RID: 138
	internal class RegistrationThreadWrapper
	{
		// Token: 0x06000342 RID: 834 RVA: 0x0000A496 File Offset: 0x00009496
		internal RegistrationThreadWrapper(RegistrationHelper helper, RegistrationConfig regConfig)
		{
			this._regConfig = regConfig;
			this._helper = helper;
			this._exception = null;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000A4B4 File Offset: 0x000094B4
		internal void InstallThread()
		{
			try
			{
				this._helper.InstallAssemblyFromConfig(ref this._regConfig);
			}
			catch (Exception ex)
			{
				this._exception = ex;
			}
			catch
			{
				this._exception = new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationThreadWrapper, InstallThread"));
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000A518 File Offset: 0x00009518
		internal void UninstallThread()
		{
			try
			{
				this._helper.UninstallAssemblyFromConfig(ref this._regConfig);
			}
			catch (Exception ex)
			{
				this._exception = ex;
			}
			catch
			{
				this._exception = new RegistrationException(Resource.FormatString("Err_NonClsException", "RegistrationThreadWrapper, UninstallThread"));
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000A57C File Offset: 0x0000957C
		internal void PropInstallResult()
		{
			if (this._exception != null)
			{
				throw this._exception;
			}
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000A58D File Offset: 0x0000958D
		internal void PropUninstallResult()
		{
			if (this._exception != null)
			{
				throw this._exception;
			}
		}

		// Token: 0x04000146 RID: 326
		private RegistrationHelper _helper;

		// Token: 0x04000147 RID: 327
		private RegistrationConfig _regConfig;

		// Token: 0x04000148 RID: 328
		private Exception _exception;
	}
}
