using System;
using System.Collections;
using System.ComponentModel;
using System.Text;

namespace System.Configuration.Install
{
	// Token: 0x0200000B RID: 11
	[DefaultEvent("AfterInstall")]
	public class Installer : Component
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000022BF File Offset: 0x000012BF
		// (set) Token: 0x0600000F RID: 15 RVA: 0x000022C7 File Offset: 0x000012C7
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public InstallContext Context
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000022D0 File Offset: 0x000012D0
		[ResDescription("Desc_Installer_HelpText")]
		public virtual string HelpText
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.Installers.Count; i++)
				{
					string helpText = this.Installers[i].HelpText;
					if (helpText.Length > 0)
					{
						stringBuilder.Append("\r\n");
						stringBuilder.Append(helpText);
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000011 RID: 17 RVA: 0x0000232E File Offset: 0x0000132E
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public InstallerCollection Installers
		{
			get
			{
				if (this.installers == null)
				{
					this.installers = new InstallerCollection(this);
				}
				return this.installers;
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000234C File Offset: 0x0000134C
		internal bool InstallerTreeContains(Installer target)
		{
			if (this.Installers.Contains(target))
			{
				return true;
			}
			foreach (object obj in this.Installers)
			{
				Installer installer = (Installer)obj;
				if (installer.InstallerTreeContains(target))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000023C0 File Offset: 0x000013C0
		// (set) Token: 0x06000014 RID: 20 RVA: 0x000023C8 File Offset: 0x000013C8
		[TypeConverter(typeof(InstallerParentConverter))]
		[Browsable(true)]
		[ResDescription("Desc_Installer_Parent")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Installer Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				if (value == this)
				{
					throw new InvalidOperationException(Res.GetString("InstallBadParent"));
				}
				if (value == this.parent)
				{
					return;
				}
				if (value != null && this.InstallerTreeContains(value))
				{
					throw new InvalidOperationException(Res.GetString("InstallRecursiveParent"));
				}
				if (this.parent != null)
				{
					int num = this.parent.Installers.IndexOf(this);
					if (num != -1)
					{
						this.parent.Installers.RemoveAt(num);
					}
				}
				this.parent = value;
				if (this.parent != null && !this.parent.Installers.Contains(this))
				{
					this.parent.Installers.Add(this);
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000015 RID: 21 RVA: 0x00002472 File Offset: 0x00001472
		// (remove) Token: 0x06000016 RID: 22 RVA: 0x0000248B File Offset: 0x0000148B
		public event InstallEventHandler Committed
		{
			add
			{
				this.afterCommitHandler = (InstallEventHandler)Delegate.Combine(this.afterCommitHandler, value);
			}
			remove
			{
				this.afterCommitHandler = (InstallEventHandler)Delegate.Remove(this.afterCommitHandler, value);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000017 RID: 23 RVA: 0x000024A4 File Offset: 0x000014A4
		// (remove) Token: 0x06000018 RID: 24 RVA: 0x000024BD File Offset: 0x000014BD
		public event InstallEventHandler AfterInstall
		{
			add
			{
				this.afterInstallHandler = (InstallEventHandler)Delegate.Combine(this.afterInstallHandler, value);
			}
			remove
			{
				this.afterInstallHandler = (InstallEventHandler)Delegate.Remove(this.afterInstallHandler, value);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000019 RID: 25 RVA: 0x000024D6 File Offset: 0x000014D6
		// (remove) Token: 0x0600001A RID: 26 RVA: 0x000024EF File Offset: 0x000014EF
		public event InstallEventHandler AfterRollback
		{
			add
			{
				this.afterRollbackHandler = (InstallEventHandler)Delegate.Combine(this.afterRollbackHandler, value);
			}
			remove
			{
				this.afterRollbackHandler = (InstallEventHandler)Delegate.Remove(this.afterRollbackHandler, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600001B RID: 27 RVA: 0x00002508 File Offset: 0x00001508
		// (remove) Token: 0x0600001C RID: 28 RVA: 0x00002521 File Offset: 0x00001521
		public event InstallEventHandler AfterUninstall
		{
			add
			{
				this.afterUninstallHandler = (InstallEventHandler)Delegate.Combine(this.afterUninstallHandler, value);
			}
			remove
			{
				this.afterUninstallHandler = (InstallEventHandler)Delegate.Remove(this.afterUninstallHandler, value);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600001D RID: 29 RVA: 0x0000253A File Offset: 0x0000153A
		// (remove) Token: 0x0600001E RID: 30 RVA: 0x00002553 File Offset: 0x00001553
		public event InstallEventHandler Committing
		{
			add
			{
				this.beforeCommitHandler = (InstallEventHandler)Delegate.Combine(this.beforeCommitHandler, value);
			}
			remove
			{
				this.beforeCommitHandler = (InstallEventHandler)Delegate.Remove(this.beforeCommitHandler, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600001F RID: 31 RVA: 0x0000256C File Offset: 0x0000156C
		// (remove) Token: 0x06000020 RID: 32 RVA: 0x00002585 File Offset: 0x00001585
		public event InstallEventHandler BeforeInstall
		{
			add
			{
				this.beforeInstallHandler = (InstallEventHandler)Delegate.Combine(this.beforeInstallHandler, value);
			}
			remove
			{
				this.beforeInstallHandler = (InstallEventHandler)Delegate.Remove(this.beforeInstallHandler, value);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000021 RID: 33 RVA: 0x0000259E File Offset: 0x0000159E
		// (remove) Token: 0x06000022 RID: 34 RVA: 0x000025B7 File Offset: 0x000015B7
		public event InstallEventHandler BeforeRollback
		{
			add
			{
				this.beforeRollbackHandler = (InstallEventHandler)Delegate.Combine(this.beforeRollbackHandler, value);
			}
			remove
			{
				this.beforeRollbackHandler = (InstallEventHandler)Delegate.Remove(this.beforeRollbackHandler, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000023 RID: 35 RVA: 0x000025D0 File Offset: 0x000015D0
		// (remove) Token: 0x06000024 RID: 36 RVA: 0x000025E9 File Offset: 0x000015E9
		public event InstallEventHandler BeforeUninstall
		{
			add
			{
				this.beforeUninstallHandler = (InstallEventHandler)Delegate.Combine(this.beforeUninstallHandler, value);
			}
			remove
			{
				this.beforeUninstallHandler = (InstallEventHandler)Delegate.Remove(this.beforeUninstallHandler, value);
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002604 File Offset: 0x00001604
		public virtual void Commit(IDictionary savedState)
		{
			if (savedState == null)
			{
				throw new ArgumentException(Res.GetString("InstallNullParameter", new object[] { "savedState" }));
			}
			if (savedState["_reserved_lastInstallerAttempted"] == null || savedState["_reserved_nestedSavedStates"] == null)
			{
				throw new ArgumentException(Res.GetString("InstallDictionaryMissingValues", new object[] { "savedState" }));
			}
			Exception ex = null;
			try
			{
				this.OnCommitting(savedState);
			}
			catch (Exception ex2)
			{
				this.WriteEventHandlerError(Res.GetString("InstallSeverityWarning"), "OnCommitting", ex2);
				this.Context.LogMessage(Res.GetString("InstallCommitException"));
				ex = ex2;
			}
			int num = (int)savedState["_reserved_lastInstallerAttempted"];
			IDictionary[] array = (IDictionary[])savedState["_reserved_nestedSavedStates"];
			if (num + 1 != array.Length || num >= this.Installers.Count)
			{
				throw new ArgumentException(Res.GetString("InstallDictionaryCorrupted", new object[] { "savedState" }));
			}
			for (int i = 0; i < this.Installers.Count; i++)
			{
				this.Installers[i].Context = this.Context;
			}
			for (int j = 0; j <= num; j++)
			{
				try
				{
					this.Installers[j].Commit(array[j]);
				}
				catch (Exception ex3)
				{
					if (!this.IsWrappedException(ex3))
					{
						this.Context.LogMessage(Res.GetString("InstallLogCommitException", new object[] { this.Installers[j].ToString() }));
						Installer.LogException(ex3, this.Context);
						this.Context.LogMessage(Res.GetString("InstallCommitException"));
					}
					ex = ex3;
				}
			}
			savedState["_reserved_nestedSavedStates"] = array;
			savedState.Remove("_reserved_lastInstallerAttempted");
			try
			{
				this.OnCommitted(savedState);
			}
			catch (Exception ex4)
			{
				this.WriteEventHandlerError(Res.GetString("InstallSeverityWarning"), "OnCommitted", ex4);
				this.Context.LogMessage(Res.GetString("InstallCommitException"));
				ex = ex4;
			}
			if (ex != null)
			{
				Exception ex5 = ex;
				if (!this.IsWrappedException(ex))
				{
					ex5 = new InstallException(Res.GetString("InstallCommitException"), ex);
					ex5.Source = "WrappedExceptionSource";
				}
				throw ex5;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002880 File Offset: 0x00001880
		public virtual void Install(IDictionary stateSaver)
		{
			if (stateSaver == null)
			{
				throw new ArgumentException(Res.GetString("InstallNullParameter", new object[] { "stateSaver" }));
			}
			try
			{
				this.OnBeforeInstall(stateSaver);
			}
			catch (Exception ex)
			{
				this.WriteEventHandlerError(Res.GetString("InstallSeverityError"), "OnBeforeInstall", ex);
				throw new InvalidOperationException(Res.GetString("InstallEventException", new object[]
				{
					"OnBeforeInstall",
					base.GetType().FullName
				}), ex);
			}
			int num = -1;
			ArrayList arrayList = new ArrayList();
			try
			{
				for (int i = 0; i < this.Installers.Count; i++)
				{
					this.Installers[i].Context = this.Context;
				}
				for (int j = 0; j < this.Installers.Count; j++)
				{
					Installer installer = this.Installers[j];
					IDictionary dictionary = new Hashtable();
					try
					{
						num = j;
						installer.Install(dictionary);
					}
					finally
					{
						arrayList.Add(dictionary);
					}
				}
			}
			finally
			{
				stateSaver.Add("_reserved_lastInstallerAttempted", num);
				stateSaver.Add("_reserved_nestedSavedStates", arrayList.ToArray(typeof(IDictionary)));
			}
			try
			{
				this.OnAfterInstall(stateSaver);
			}
			catch (Exception ex2)
			{
				this.WriteEventHandlerError(Res.GetString("InstallSeverityError"), "OnAfterInstall", ex2);
				throw new InvalidOperationException(Res.GetString("InstallEventException", new object[]
				{
					"OnAfterInstall",
					base.GetType().FullName
				}), ex2);
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002A3C File Offset: 0x00001A3C
		internal static void LogException(Exception e, InstallContext context)
		{
			bool flag = true;
			while (e != null)
			{
				if (flag)
				{
					context.LogMessage(e.GetType().FullName + ": " + e.Message);
					flag = false;
				}
				else
				{
					context.LogMessage(Res.GetString("InstallLogInner", new object[]
					{
						e.GetType().FullName,
						e.Message
					}));
				}
				if (context.IsParameterTrue("showcallstack"))
				{
					context.LogMessage(e.StackTrace);
				}
				e = e.InnerException;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002AC8 File Offset: 0x00001AC8
		private bool IsWrappedException(Exception e)
		{
			return e is InstallException && e.Source == "WrappedExceptionSource" && e.TargetSite.ReflectedType == typeof(Installer);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002AFD File Offset: 0x00001AFD
		protected virtual void OnCommitted(IDictionary savedState)
		{
			if (this.afterCommitHandler != null)
			{
				this.afterCommitHandler(this, new InstallEventArgs(savedState));
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002B19 File Offset: 0x00001B19
		protected virtual void OnAfterInstall(IDictionary savedState)
		{
			if (this.afterInstallHandler != null)
			{
				this.afterInstallHandler(this, new InstallEventArgs(savedState));
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002B35 File Offset: 0x00001B35
		protected virtual void OnAfterRollback(IDictionary savedState)
		{
			if (this.afterRollbackHandler != null)
			{
				this.afterRollbackHandler(this, new InstallEventArgs(savedState));
			}
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B51 File Offset: 0x00001B51
		protected virtual void OnAfterUninstall(IDictionary savedState)
		{
			if (this.afterUninstallHandler != null)
			{
				this.afterUninstallHandler(this, new InstallEventArgs(savedState));
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B6D File Offset: 0x00001B6D
		protected virtual void OnCommitting(IDictionary savedState)
		{
			if (this.beforeCommitHandler != null)
			{
				this.beforeCommitHandler(this, new InstallEventArgs(savedState));
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B89 File Offset: 0x00001B89
		protected virtual void OnBeforeInstall(IDictionary savedState)
		{
			if (this.beforeInstallHandler != null)
			{
				this.beforeInstallHandler(this, new InstallEventArgs(savedState));
			}
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002BA5 File Offset: 0x00001BA5
		protected virtual void OnBeforeRollback(IDictionary savedState)
		{
			if (this.beforeRollbackHandler != null)
			{
				this.beforeRollbackHandler(this, new InstallEventArgs(savedState));
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002BC1 File Offset: 0x00001BC1
		protected virtual void OnBeforeUninstall(IDictionary savedState)
		{
			if (this.beforeUninstallHandler != null)
			{
				this.beforeUninstallHandler(this, new InstallEventArgs(savedState));
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002BE0 File Offset: 0x00001BE0
		public virtual void Rollback(IDictionary savedState)
		{
			if (savedState == null)
			{
				throw new ArgumentException(Res.GetString("InstallNullParameter", new object[] { "savedState" }));
			}
			if (savedState["_reserved_lastInstallerAttempted"] == null || savedState["_reserved_nestedSavedStates"] == null)
			{
				throw new ArgumentException(Res.GetString("InstallDictionaryMissingValues", new object[] { "savedState" }));
			}
			Exception ex = null;
			try
			{
				this.OnBeforeRollback(savedState);
			}
			catch (Exception ex2)
			{
				this.WriteEventHandlerError(Res.GetString("InstallSeverityWarning"), "OnBeforeRollback", ex2);
				this.Context.LogMessage(Res.GetString("InstallRollbackException"));
				ex = ex2;
			}
			int num = (int)savedState["_reserved_lastInstallerAttempted"];
			IDictionary[] array = (IDictionary[])savedState["_reserved_nestedSavedStates"];
			if (num + 1 != array.Length || num >= this.Installers.Count)
			{
				throw new ArgumentException(Res.GetString("InstallDictionaryCorrupted", new object[] { "savedState" }));
			}
			for (int i = this.Installers.Count - 1; i >= 0; i--)
			{
				this.Installers[i].Context = this.Context;
			}
			for (int j = num; j >= 0; j--)
			{
				try
				{
					this.Installers[j].Rollback(array[j]);
				}
				catch (Exception ex3)
				{
					if (!this.IsWrappedException(ex3))
					{
						this.Context.LogMessage(Res.GetString("InstallLogRollbackException", new object[] { this.Installers[j].ToString() }));
						Installer.LogException(ex3, this.Context);
						this.Context.LogMessage(Res.GetString("InstallRollbackException"));
					}
					ex = ex3;
				}
			}
			try
			{
				this.OnAfterRollback(savedState);
			}
			catch (Exception ex4)
			{
				this.WriteEventHandlerError(Res.GetString("InstallSeverityWarning"), "OnAfterRollback", ex4);
				this.Context.LogMessage(Res.GetString("InstallRollbackException"));
				ex = ex4;
			}
			if (ex != null)
			{
				Exception ex5 = ex;
				if (!this.IsWrappedException(ex))
				{
					ex5 = new InstallException(Res.GetString("InstallRollbackException"), ex);
					ex5.Source = "WrappedExceptionSource";
				}
				throw ex5;
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002E44 File Offset: 0x00001E44
		public virtual void Uninstall(IDictionary savedState)
		{
			Exception ex = null;
			try
			{
				this.OnBeforeUninstall(savedState);
			}
			catch (Exception ex2)
			{
				this.WriteEventHandlerError(Res.GetString("InstallSeverityWarning"), "OnBeforeUninstall", ex2);
				this.Context.LogMessage(Res.GetString("InstallUninstallException"));
				ex = ex2;
			}
			IDictionary[] array;
			if (savedState != null)
			{
				array = (IDictionary[])savedState["_reserved_nestedSavedStates"];
				if (array.Length != this.Installers.Count)
				{
					throw new ArgumentException(Res.GetString("InstallDictionaryCorrupted", new object[] { "savedState" }));
				}
			}
			else
			{
				array = new IDictionary[this.Installers.Count];
			}
			for (int i = this.Installers.Count - 1; i >= 0; i--)
			{
				this.Installers[i].Context = this.Context;
			}
			for (int j = this.Installers.Count - 1; j >= 0; j--)
			{
				try
				{
					this.Installers[j].Uninstall(array[j]);
				}
				catch (Exception ex3)
				{
					if (!this.IsWrappedException(ex3))
					{
						this.Context.LogMessage(Res.GetString("InstallLogUninstallException", new object[] { this.Installers[j].ToString() }));
						Installer.LogException(ex3, this.Context);
						this.Context.LogMessage(Res.GetString("InstallUninstallException"));
					}
					ex = ex3;
				}
			}
			try
			{
				this.OnAfterUninstall(savedState);
			}
			catch (Exception ex4)
			{
				this.WriteEventHandlerError(Res.GetString("InstallSeverityWarning"), "OnAfterUninstall", ex4);
				this.Context.LogMessage(Res.GetString("InstallUninstallException"));
				ex = ex4;
			}
			if (ex != null)
			{
				Exception ex5 = ex;
				if (!this.IsWrappedException(ex))
				{
					ex5 = new InstallException(Res.GetString("InstallUninstallException"), ex);
					ex5.Source = "WrappedExceptionSource";
				}
				throw ex5;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000304C File Offset: 0x0000204C
		private void WriteEventHandlerError(string severity, string eventName, Exception e)
		{
			this.Context.LogMessage(Res.GetString("InstallLogError", new object[]
			{
				severity,
				eventName,
				base.GetType().FullName
			}));
			Installer.LogException(e, this.Context);
		}

		// Token: 0x040000DD RID: 221
		private const string wrappedExceptionSource = "WrappedExceptionSource";

		// Token: 0x040000DE RID: 222
		private InstallerCollection installers;

		// Token: 0x040000DF RID: 223
		private InstallContext context;

		// Token: 0x040000E0 RID: 224
		internal Installer parent;

		// Token: 0x040000E1 RID: 225
		private InstallEventHandler afterCommitHandler;

		// Token: 0x040000E2 RID: 226
		private InstallEventHandler afterInstallHandler;

		// Token: 0x040000E3 RID: 227
		private InstallEventHandler afterRollbackHandler;

		// Token: 0x040000E4 RID: 228
		private InstallEventHandler afterUninstallHandler;

		// Token: 0x040000E5 RID: 229
		private InstallEventHandler beforeCommitHandler;

		// Token: 0x040000E6 RID: 230
		private InstallEventHandler beforeInstallHandler;

		// Token: 0x040000E7 RID: 231
		private InstallEventHandler beforeRollbackHandler;

		// Token: 0x040000E8 RID: 232
		private InstallEventHandler beforeUninstallHandler;
	}
}
