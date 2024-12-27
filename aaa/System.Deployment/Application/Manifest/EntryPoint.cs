using System;
using System.Deployment.Internal.Isolation.Manifest;

namespace System.Deployment.Application.Manifest
{
	// Token: 0x02000016 RID: 22
	internal class EntryPoint
	{
		// Token: 0x060000BD RID: 189 RVA: 0x00005760 File Offset: 0x00004760
		public EntryPoint(EntryPointEntry entryPointEntry, AssemblyManifest manifest)
		{
			this._name = entryPointEntry.Name;
			this._commandLineFile = entryPointEntry.CommandLine_File;
			this._commandLineParamater = entryPointEntry.CommandLine_Parameters;
			this._hostInBrowser = (entryPointEntry.Flags & 1U) != 0U;
			this._customHostSpecified = (entryPointEntry.Flags & 2U) != 0U;
			this._customUX = (entryPointEntry.Flags & 4U) != 0U;
			if (!this._customHostSpecified)
			{
				if (entryPointEntry.Identity != null)
				{
					this._dependentAssembly = manifest.GetDependentAssemblyByIdentity(entryPointEntry.Identity);
				}
				if (this._dependentAssembly == null)
				{
					throw new InvalidDeploymentException(ExceptionTypes.ManifestParse, Resources.GetString("Ex_NoMatchingAssemblyForEntryPoint"));
				}
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000580F File Offset: 0x0000480F
		public DependentAssembly Assembly
		{
			get
			{
				return this._dependentAssembly;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005817 File Offset: 0x00004817
		public string CommandFile
		{
			get
			{
				return this._commandLineFile;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000581F File Offset: 0x0000481F
		public bool HostInBrowser
		{
			get
			{
				return this._hostInBrowser;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00005827 File Offset: 0x00004827
		public bool CustomHostSpecified
		{
			get
			{
				return this._customHostSpecified;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000582F File Offset: 0x0000482F
		public bool CustomUX
		{
			get
			{
				return this._customUX;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005837 File Offset: 0x00004837
		public string CommandParameters
		{
			get
			{
				return this._commandLineParamater;
			}
		}

		// Token: 0x04000068 RID: 104
		private readonly string _name;

		// Token: 0x04000069 RID: 105
		private readonly string _commandLineFile;

		// Token: 0x0400006A RID: 106
		private readonly string _commandLineParamater;

		// Token: 0x0400006B RID: 107
		private readonly DependentAssembly _dependentAssembly;

		// Token: 0x0400006C RID: 108
		private readonly bool _hostInBrowser;

		// Token: 0x0400006D RID: 109
		private readonly bool _customHostSpecified;

		// Token: 0x0400006E RID: 110
		private readonly bool _customUX;
	}
}
