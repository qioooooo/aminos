using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Soap;

namespace System.Configuration.Install
{
	// Token: 0x0200000C RID: 12
	public class AssemblyInstaller : Installer
	{
		// Token: 0x06000035 RID: 53 RVA: 0x000030A0 File Offset: 0x000020A0
		public AssemblyInstaller()
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000030A8 File Offset: 0x000020A8
		public AssemblyInstaller(string fileName, string[] commandLine)
		{
			this.Path = global::System.IO.Path.GetFullPath(fileName);
			this.commandLine = commandLine;
			this.useNewContext = true;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000030CA File Offset: 0x000020CA
		public AssemblyInstaller(Assembly assembly, string[] commandLine)
		{
			this.Assembly = assembly;
			this.commandLine = commandLine;
			this.useNewContext = true;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000038 RID: 56 RVA: 0x000030E7 File Offset: 0x000020E7
		// (set) Token: 0x06000039 RID: 57 RVA: 0x000030EF File Offset: 0x000020EF
		[ResDescription("Desc_AssemblyInstaller_Assembly")]
		public Assembly Assembly
		{
			get
			{
				return this.assembly;
			}
			set
			{
				this.assembly = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000030F8 File Offset: 0x000020F8
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00003100 File Offset: 0x00002100
		[ResDescription("Desc_AssemblyInstaller_CommandLine")]
		public string[] CommandLine
		{
			get
			{
				return this.commandLine;
			}
			set
			{
				this.commandLine = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0000310C File Offset: 0x0000210C
		public override string HelpText
		{
			get
			{
				if (this.Path != null && this.Path.Length > 0)
				{
					base.Context = new InstallContext(null, new string[0]);
					if (!this.initialized)
					{
						this.InitializeFromAssembly();
					}
				}
				if (AssemblyInstaller.helpPrinted)
				{
					return base.HelpText;
				}
				AssemblyInstaller.helpPrinted = true;
				return Res.GetString("InstallAssemblyHelp") + "\r\n" + base.HelpText;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003D RID: 61 RVA: 0x0000317D File Offset: 0x0000217D
		// (set) Token: 0x0600003E RID: 62 RVA: 0x00003194 File Offset: 0x00002194
		[ResDescription("Desc_AssemblyInstaller_Path")]
		public string Path
		{
			get
			{
				if (this.assembly == null)
				{
					return null;
				}
				return this.assembly.Location;
			}
			set
			{
				if (value == null)
				{
					this.assembly = null;
				}
				this.assembly = Assembly.LoadFrom(value);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600003F RID: 63 RVA: 0x000031AC File Offset: 0x000021AC
		// (set) Token: 0x06000040 RID: 64 RVA: 0x000031B4 File Offset: 0x000021B4
		[ResDescription("Desc_AssemblyInstaller_UseNewContext")]
		public bool UseNewContext
		{
			get
			{
				return this.useNewContext;
			}
			set
			{
				this.useNewContext = value;
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000031C0 File Offset: 0x000021C0
		public static void CheckIfInstallable(string assemblyName)
		{
			AssemblyInstaller assemblyInstaller = new AssemblyInstaller();
			assemblyInstaller.UseNewContext = false;
			assemblyInstaller.Path = assemblyName;
			assemblyInstaller.CommandLine = new string[0];
			assemblyInstaller.Context = new InstallContext(null, new string[0]);
			assemblyInstaller.InitializeFromAssembly();
			if (assemblyInstaller.Installers.Count == 0)
			{
				throw new InvalidOperationException(Res.GetString("InstallNoPublicInstallers", new object[] { assemblyName }));
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003230 File Offset: 0x00002230
		private InstallContext CreateAssemblyContext()
		{
			InstallContext installContext = new InstallContext(global::System.IO.Path.ChangeExtension(this.Path, ".InstallLog"), this.CommandLine);
			if (base.Context != null)
			{
				installContext.Parameters["logtoconsole"] = base.Context.Parameters["logtoconsole"];
			}
			installContext.Parameters["assemblypath"] = this.Path;
			return installContext;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000032A0 File Offset: 0x000022A0
		private void InitializeFromAssembly()
		{
			Type[] array = null;
			try
			{
				array = AssemblyInstaller.GetInstallerTypes(this.assembly);
			}
			catch (Exception ex)
			{
				base.Context.LogMessage(Res.GetString("InstallException", new object[] { this.Path }));
				Installer.LogException(ex, base.Context);
				base.Context.LogMessage(Res.GetString("InstallAbort", new object[] { this.Path }));
				throw new InvalidOperationException(Res.GetString("InstallNoInstallerTypes", new object[] { this.Path }), ex);
			}
			if (array == null || array.Length == 0)
			{
				base.Context.LogMessage(Res.GetString("InstallNoPublicInstallers", new object[] { this.Path }));
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					Installer installer = (Installer)Activator.CreateInstance(array[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.CreateInstance, null, new object[0], null);
					base.Installers.Add(installer);
				}
				catch (Exception ex2)
				{
					base.Context.LogMessage(Res.GetString("InstallCannotCreateInstance", new object[] { array[i].FullName }));
					Installer.LogException(ex2, base.Context);
					throw new InvalidOperationException(Res.GetString("InstallCannotCreateInstance", new object[] { array[i].FullName }), ex2);
				}
			}
			this.initialized = true;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0000343C File Offset: 0x0000243C
		private string GetInstallStatePath(string assemblyPath)
		{
			string text = base.Context.Parameters["InstallStateDir"];
			assemblyPath = global::System.IO.Path.ChangeExtension(assemblyPath, ".InstallState");
			string text2;
			if (!string.IsNullOrEmpty(text))
			{
				text2 = global::System.IO.Path.Combine(text, global::System.IO.Path.GetFileName(assemblyPath));
			}
			else
			{
				text2 = assemblyPath;
			}
			return text2;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003488 File Offset: 0x00002488
		public override void Commit(IDictionary savedState)
		{
			this.PrintStartText(Res.GetString("InstallActivityCommitting"));
			if (!this.initialized)
			{
				this.InitializeFromAssembly();
			}
			string installStatePath = this.GetInstallStatePath(this.Path);
			FileStream fileStream = new FileStream(installStatePath, FileMode.Open, FileAccess.Read);
			try
			{
				SoapFormatter soapFormatter = new SoapFormatter();
				savedState = (IDictionary)soapFormatter.Deserialize(fileStream);
			}
			finally
			{
				fileStream.Close();
				if (base.Installers.Count == 0)
				{
					base.Context.LogMessage(Res.GetString("RemovingInstallState"));
					File.Delete(installStatePath);
				}
			}
			base.Commit(savedState);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003528 File Offset: 0x00002528
		private static Type[] GetInstallerTypes(Assembly assem)
		{
			ArrayList arrayList = new ArrayList();
			Module[] modules = assem.GetModules();
			for (int i = 0; i < modules.Length; i++)
			{
				Type[] types = modules[i].GetTypes();
				for (int j = 0; j < types.Length; j++)
				{
					if (typeof(Installer).IsAssignableFrom(types[j]) && !types[j].IsAbstract && types[j].IsPublic && ((RunInstallerAttribute)TypeDescriptor.GetAttributes(types[j])[typeof(RunInstallerAttribute)]).RunInstaller)
					{
						arrayList.Add(types[j]);
					}
				}
			}
			return (Type[])arrayList.ToArray(typeof(Type));
		}

		// Token: 0x06000047 RID: 71 RVA: 0x000035DC File Offset: 0x000025DC
		public override void Install(IDictionary savedState)
		{
			this.PrintStartText(Res.GetString("InstallActivityInstalling"));
			if (!this.initialized)
			{
				this.InitializeFromAssembly();
			}
			savedState = new Hashtable();
			try
			{
				base.Install(savedState);
			}
			finally
			{
				FileStream fileStream = new FileStream(this.GetInstallStatePath(this.Path), FileMode.Create);
				try
				{
					SoapFormatter soapFormatter = new SoapFormatter();
					soapFormatter.Serialize(fileStream, savedState);
				}
				finally
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003660 File Offset: 0x00002660
		private void PrintStartText(string activity)
		{
			if (this.UseNewContext)
			{
				InstallContext installContext = this.CreateAssemblyContext();
				if (base.Context != null)
				{
					base.Context.LogMessage(Res.GetString("InstallLogContent", new object[] { this.Path }));
					base.Context.LogMessage(Res.GetString("InstallFileLocation", new object[] { installContext.Parameters["logfile"] }));
				}
				base.Context = installContext;
			}
			base.Context.LogMessage(string.Format(CultureInfo.InvariantCulture, activity, new object[] { this.Path }));
			base.Context.LogMessage(Res.GetString("InstallLogParameters"));
			if (base.Context.Parameters.Count == 0)
			{
				base.Context.LogMessage("   " + Res.GetString("InstallLogNone"));
			}
			IDictionaryEnumerator dictionaryEnumerator = (IDictionaryEnumerator)base.Context.Parameters.GetEnumerator();
			while (dictionaryEnumerator.MoveNext())
			{
				base.Context.LogMessage("   " + (string)dictionaryEnumerator.Key + " = " + (string)dictionaryEnumerator.Value);
			}
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000037A4 File Offset: 0x000027A4
		public override void Rollback(IDictionary savedState)
		{
			this.PrintStartText(Res.GetString("InstallActivityRollingBack"));
			if (!this.initialized)
			{
				this.InitializeFromAssembly();
			}
			string installStatePath = this.GetInstallStatePath(this.Path);
			FileStream fileStream = new FileStream(installStatePath, FileMode.Open, FileAccess.Read);
			try
			{
				SoapFormatter soapFormatter = new SoapFormatter();
				savedState = (IDictionary)soapFormatter.Deserialize(fileStream);
			}
			finally
			{
				fileStream.Close();
			}
			try
			{
				base.Rollback(savedState);
			}
			finally
			{
				File.Delete(installStatePath);
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003830 File Offset: 0x00002830
		public override void Uninstall(IDictionary savedState)
		{
			this.PrintStartText(Res.GetString("InstallActivityUninstalling"));
			if (!this.initialized)
			{
				this.InitializeFromAssembly();
			}
			string installStatePath = this.GetInstallStatePath(this.Path);
			if (installStatePath != null && File.Exists(installStatePath))
			{
				FileStream fileStream = new FileStream(installStatePath, FileMode.Open, FileAccess.Read);
				try
				{
					try
					{
						SoapFormatter soapFormatter = new SoapFormatter();
						savedState = (IDictionary)soapFormatter.Deserialize(fileStream);
					}
					catch
					{
						base.Context.LogMessage(Res.GetString("InstallSavedStateFileCorruptedWarning", new object[] { this.Path, installStatePath }));
						savedState = null;
					}
					goto IL_0091;
				}
				finally
				{
					fileStream.Close();
				}
			}
			savedState = null;
			IL_0091:
			base.Uninstall(savedState);
			if (installStatePath != null && installStatePath.Length != 0)
			{
				try
				{
					File.Delete(installStatePath);
				}
				catch
				{
					throw new InvalidOperationException(Res.GetString("InstallUnableDeleteFile", new object[] { installStatePath }));
				}
			}
		}

		// Token: 0x040000E9 RID: 233
		private Assembly assembly;

		// Token: 0x040000EA RID: 234
		private string[] commandLine;

		// Token: 0x040000EB RID: 235
		private bool useNewContext;

		// Token: 0x040000EC RID: 236
		private static bool helpPrinted;

		// Token: 0x040000ED RID: 237
		private bool initialized;
	}
}
