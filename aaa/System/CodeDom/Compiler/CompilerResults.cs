using System;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001F3 RID: 499
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class CompilerResults
	{
		// Token: 0x060010F4 RID: 4340 RVA: 0x000377A1 File Offset: 0x000367A1
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public CompilerResults(TempFileCollection tempFiles)
		{
			this.tempFiles = tempFiles;
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x000377C6 File Offset: 0x000367C6
		// (set) Token: 0x060010F6 RID: 4342 RVA: 0x000377CE File Offset: 0x000367CE
		public TempFileCollection TempFiles
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return this.tempFiles;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.tempFiles = value;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x000377D8 File Offset: 0x000367D8
		// (set) Token: 0x060010F8 RID: 4344 RVA: 0x000377FC File Offset: 0x000367FC
		public Evidence Evidence
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				Evidence evidence = null;
				if (this.evidence != null)
				{
					evidence = CompilerResults.CloneEvidence(this.evidence);
				}
				return evidence;
			}
			[SecurityPermission(SecurityAction.Demand, ControlEvidence = true)]
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				if (value != null)
				{
					this.evidence = CompilerResults.CloneEvidence(value);
					return;
				}
				this.evidence = null;
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x00037818 File Offset: 0x00036818
		// (set) Token: 0x060010FA RID: 4346 RVA: 0x0003785F File Offset: 0x0003685F
		public Assembly CompiledAssembly
		{
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlEvidence)]
			get
			{
				if (this.compiledAssembly == null && this.pathToAssembly != null)
				{
					this.compiledAssembly = Assembly.Load(new AssemblyName
					{
						CodeBase = this.pathToAssembly
					}, this.evidence);
				}
				return this.compiledAssembly;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.compiledAssembly = value;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x00037868 File Offset: 0x00036868
		public CompilerErrorCollection Errors
		{
			get
			{
				return this.errors;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x060010FC RID: 4348 RVA: 0x00037870 File Offset: 0x00036870
		public StringCollection Output
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return this.output;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x00037878 File Offset: 0x00036878
		// (set) Token: 0x060010FE RID: 4350 RVA: 0x00037880 File Offset: 0x00036880
		public string PathToAssembly
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				return this.pathToAssembly;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.pathToAssembly = value;
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x00037889 File Offset: 0x00036889
		// (set) Token: 0x06001100 RID: 4352 RVA: 0x00037891 File Offset: 0x00036891
		public int NativeCompilerReturnValue
		{
			get
			{
				return this.nativeCompilerReturnValue;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				this.nativeCompilerReturnValue = value;
			}
		}

		// Token: 0x06001101 RID: 4353 RVA: 0x0003789C File Offset: 0x0003689C
		internal static Evidence CloneEvidence(Evidence ev)
		{
			new PermissionSet(PermissionState.Unrestricted).Assert();
			MemoryStream memoryStream = new MemoryStream();
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(memoryStream, ev);
			memoryStream.Position = 0L;
			return (Evidence)binaryFormatter.Deserialize(memoryStream);
		}

		// Token: 0x04000F7B RID: 3963
		private CompilerErrorCollection errors = new CompilerErrorCollection();

		// Token: 0x04000F7C RID: 3964
		private StringCollection output = new StringCollection();

		// Token: 0x04000F7D RID: 3965
		private Assembly compiledAssembly;

		// Token: 0x04000F7E RID: 3966
		private string pathToAssembly;

		// Token: 0x04000F7F RID: 3967
		private int nativeCompilerReturnValue;

		// Token: 0x04000F80 RID: 3968
		private TempFileCollection tempFiles;

		// Token: 0x04000F81 RID: 3969
		private Evidence evidence;
	}
}
