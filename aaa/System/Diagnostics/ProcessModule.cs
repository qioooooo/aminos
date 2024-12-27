using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000786 RID: 1926
	[Designer("System.Diagnostics.Design.ProcessModuleDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class ProcessModule : Component
	{
		// Token: 0x06003B60 RID: 15200 RVA: 0x000FD8AC File Offset: 0x000FC8AC
		internal ProcessModule(ModuleInfo moduleInfo)
		{
			this.moduleInfo = moduleInfo;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x000FD8C1 File Offset: 0x000FC8C1
		internal void EnsureNtProcessInfo()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException(SR.GetString("WinNTRequired"));
			}
		}

		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x06003B62 RID: 15202 RVA: 0x000FD8E0 File Offset: 0x000FC8E0
		[MonitoringDescription("ProcModModuleName")]
		public string ModuleName
		{
			get
			{
				return this.moduleInfo.baseName;
			}
		}

		// Token: 0x17000DE3 RID: 3555
		// (get) Token: 0x06003B63 RID: 15203 RVA: 0x000FD8ED File Offset: 0x000FC8ED
		[MonitoringDescription("ProcModFileName")]
		public string FileName
		{
			get
			{
				return this.moduleInfo.fileName;
			}
		}

		// Token: 0x17000DE4 RID: 3556
		// (get) Token: 0x06003B64 RID: 15204 RVA: 0x000FD8FA File Offset: 0x000FC8FA
		[MonitoringDescription("ProcModBaseAddress")]
		public IntPtr BaseAddress
		{
			get
			{
				return this.moduleInfo.baseOfDll;
			}
		}

		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x06003B65 RID: 15205 RVA: 0x000FD907 File Offset: 0x000FC907
		[MonitoringDescription("ProcModModuleMemorySize")]
		public int ModuleMemorySize
		{
			get
			{
				return this.moduleInfo.sizeOfImage;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x06003B66 RID: 15206 RVA: 0x000FD914 File Offset: 0x000FC914
		[MonitoringDescription("ProcModEntryPointAddress")]
		public IntPtr EntryPointAddress
		{
			get
			{
				this.EnsureNtProcessInfo();
				return this.moduleInfo.entryPoint;
			}
		}

		// Token: 0x17000DE7 RID: 3559
		// (get) Token: 0x06003B67 RID: 15207 RVA: 0x000FD927 File Offset: 0x000FC927
		[Browsable(false)]
		public FileVersionInfo FileVersionInfo
		{
			get
			{
				if (this.fileVersionInfo == null)
				{
					this.fileVersionInfo = FileVersionInfo.GetVersionInfo(this.FileName);
				}
				return this.fileVersionInfo;
			}
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x000FD948 File Offset: 0x000FC948
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", new object[]
			{
				base.ToString(),
				this.ModuleName
			});
		}

		// Token: 0x04003420 RID: 13344
		internal ModuleInfo moduleInfo;

		// Token: 0x04003421 RID: 13345
		private FileVersionInfo fileVersionInfo;
	}
}
