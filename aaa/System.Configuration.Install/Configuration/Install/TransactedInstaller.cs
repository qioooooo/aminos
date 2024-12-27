using System;
using System.Collections;

namespace System.Configuration.Install
{
	// Token: 0x0200001A RID: 26
	public class TransactedInstaller : Installer
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x00005074 File Offset: 0x00004074
		public override void Install(IDictionary savedState)
		{
			if (base.Context == null)
			{
				base.Context = new InstallContext();
			}
			base.Context.LogMessage(Environment.NewLine + Res.GetString("InstallInfoTransacted"));
			try
			{
				bool flag = true;
				try
				{
					base.Context.LogMessage(Environment.NewLine + Res.GetString("InstallInfoBeginInstall"));
					base.Install(savedState);
				}
				catch (Exception ex)
				{
					flag = false;
					base.Context.LogMessage(Environment.NewLine + Res.GetString("InstallInfoException"));
					Installer.LogException(ex, base.Context);
					base.Context.LogMessage(Environment.NewLine + Res.GetString("InstallInfoBeginRollback"));
					try
					{
						this.Rollback(savedState);
					}
					catch (Exception)
					{
					}
					base.Context.LogMessage(Environment.NewLine + Res.GetString("InstallInfoRollbackDone"));
					throw new InvalidOperationException(Res.GetString("InstallRollback"), ex);
				}
				if (flag)
				{
					base.Context.LogMessage(Environment.NewLine + Res.GetString("InstallInfoBeginCommit"));
					try
					{
						this.Commit(savedState);
					}
					finally
					{
						base.Context.LogMessage(Environment.NewLine + Res.GetString("InstallInfoCommitDone"));
					}
				}
			}
			finally
			{
				base.Context.LogMessage(Environment.NewLine + Res.GetString("InstallInfoTransactedDone"));
			}
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000523C File Offset: 0x0000423C
		public override void Uninstall(IDictionary savedState)
		{
			if (base.Context == null)
			{
				base.Context = new InstallContext();
			}
			base.Context.LogMessage(Environment.NewLine + Environment.NewLine + Res.GetString("InstallInfoBeginUninstall"));
			try
			{
				base.Uninstall(savedState);
			}
			finally
			{
				base.Context.LogMessage(Environment.NewLine + Res.GetString("InstallInfoUninstallDone"));
			}
		}
	}
}
