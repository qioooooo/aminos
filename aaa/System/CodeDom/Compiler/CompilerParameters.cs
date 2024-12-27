using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Policy;
using Microsoft.Win32.SafeHandles;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001F2 RID: 498
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[Serializable]
	public class CompilerParameters
	{
		// Token: 0x060010D4 RID: 4308 RVA: 0x000375D0 File Offset: 0x000365D0
		public CompilerParameters()
			: this(null, null)
		{
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x000375DA File Offset: 0x000365DA
		public CompilerParameters(string[] assemblyNames)
			: this(assemblyNames, null, false)
		{
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x000375E5 File Offset: 0x000365E5
		public CompilerParameters(string[] assemblyNames, string outputName)
			: this(assemblyNames, outputName, false)
		{
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x000375F0 File Offset: 0x000365F0
		public CompilerParameters(string[] assemblyNames, string outputName, bool includeDebugInformation)
		{
			if (assemblyNames != null)
			{
				this.ReferencedAssemblies.AddRange(assemblyNames);
			}
			this.outputName = outputName;
			this.includeDebugInformation = includeDebugInformation;
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x060010D8 RID: 4312 RVA: 0x00037648 File Offset: 0x00036648
		// (set) Token: 0x060010D9 RID: 4313 RVA: 0x00037650 File Offset: 0x00036650
		public bool GenerateExecutable
		{
			get
			{
				return this.generateExecutable;
			}
			set
			{
				this.generateExecutable = value;
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x060010DA RID: 4314 RVA: 0x00037659 File Offset: 0x00036659
		// (set) Token: 0x060010DB RID: 4315 RVA: 0x00037661 File Offset: 0x00036661
		public bool GenerateInMemory
		{
			get
			{
				return this.generateInMemory;
			}
			set
			{
				this.generateInMemory = value;
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x060010DC RID: 4316 RVA: 0x0003766A File Offset: 0x0003666A
		public StringCollection ReferencedAssemblies
		{
			get
			{
				return this.assemblyNames;
			}
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x00037672 File Offset: 0x00036672
		// (set) Token: 0x060010DE RID: 4318 RVA: 0x0003767A File Offset: 0x0003667A
		public string MainClass
		{
			get
			{
				return this.mainClass;
			}
			set
			{
				this.mainClass = value;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00037683 File Offset: 0x00036683
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x0003768B File Offset: 0x0003668B
		public string OutputAssembly
		{
			get
			{
				return this.outputName;
			}
			set
			{
				this.outputName = value;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x00037694 File Offset: 0x00036694
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x000376AF File Offset: 0x000366AF
		public TempFileCollection TempFiles
		{
			get
			{
				if (this.tempFiles == null)
				{
					this.tempFiles = new TempFileCollection();
				}
				return this.tempFiles;
			}
			set
			{
				this.tempFiles = value;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x000376B8 File Offset: 0x000366B8
		// (set) Token: 0x060010E4 RID: 4324 RVA: 0x000376C0 File Offset: 0x000366C0
		public bool IncludeDebugInformation
		{
			get
			{
				return this.includeDebugInformation;
			}
			set
			{
				this.includeDebugInformation = value;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x000376C9 File Offset: 0x000366C9
		// (set) Token: 0x060010E6 RID: 4326 RVA: 0x000376D1 File Offset: 0x000366D1
		public bool TreatWarningsAsErrors
		{
			get
			{
				return this.treatWarningsAsErrors;
			}
			set
			{
				this.treatWarningsAsErrors = value;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x000376DA File Offset: 0x000366DA
		// (set) Token: 0x060010E8 RID: 4328 RVA: 0x000376E2 File Offset: 0x000366E2
		public int WarningLevel
		{
			get
			{
				return this.warningLevel;
			}
			set
			{
				this.warningLevel = value;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x000376EB File Offset: 0x000366EB
		// (set) Token: 0x060010EA RID: 4330 RVA: 0x000376F3 File Offset: 0x000366F3
		public string CompilerOptions
		{
			get
			{
				return this.compilerOptions;
			}
			set
			{
				this.compilerOptions = value;
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x000376FC File Offset: 0x000366FC
		// (set) Token: 0x060010EC RID: 4332 RVA: 0x00037704 File Offset: 0x00036704
		public string Win32Resource
		{
			get
			{
				return this.win32Resource;
			}
			set
			{
				this.win32Resource = value;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x0003770D File Offset: 0x0003670D
		[ComVisible(false)]
		public StringCollection EmbeddedResources
		{
			get
			{
				return this.embeddedResources;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060010EE RID: 4334 RVA: 0x00037715 File Offset: 0x00036715
		[ComVisible(false)]
		public StringCollection LinkedResources
		{
			get
			{
				return this.linkedResources;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x0003771D File Offset: 0x0003671D
		// (set) Token: 0x060010F0 RID: 4336 RVA: 0x00037738 File Offset: 0x00036738
		public IntPtr UserToken
		{
			get
			{
				if (this.userToken != null)
				{
					return this.userToken.DangerousGetHandle();
				}
				return IntPtr.Zero;
			}
			set
			{
				if (this.userToken != null)
				{
					this.userToken.Close();
				}
				this.userToken = new SafeUserTokenHandle(value, false);
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x0003775A File Offset: 0x0003675A
		internal SafeUserTokenHandle SafeUserToken
		{
			get
			{
				return this.userToken;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x060010F2 RID: 4338 RVA: 0x00037764 File Offset: 0x00036764
		// (set) Token: 0x060010F3 RID: 4339 RVA: 0x00037788 File Offset: 0x00036788
		public Evidence Evidence
		{
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

		// Token: 0x04000F6C RID: 3948
		private StringCollection assemblyNames = new StringCollection();

		// Token: 0x04000F6D RID: 3949
		[OptionalField]
		private StringCollection embeddedResources = new StringCollection();

		// Token: 0x04000F6E RID: 3950
		[OptionalField]
		private StringCollection linkedResources = new StringCollection();

		// Token: 0x04000F6F RID: 3951
		private string outputName;

		// Token: 0x04000F70 RID: 3952
		private string mainClass;

		// Token: 0x04000F71 RID: 3953
		private bool generateInMemory;

		// Token: 0x04000F72 RID: 3954
		private bool includeDebugInformation;

		// Token: 0x04000F73 RID: 3955
		private int warningLevel = -1;

		// Token: 0x04000F74 RID: 3956
		private string compilerOptions;

		// Token: 0x04000F75 RID: 3957
		private string win32Resource;

		// Token: 0x04000F76 RID: 3958
		private bool treatWarningsAsErrors;

		// Token: 0x04000F77 RID: 3959
		private bool generateExecutable;

		// Token: 0x04000F78 RID: 3960
		private TempFileCollection tempFiles;

		// Token: 0x04000F79 RID: 3961
		[NonSerialized]
		private SafeUserTokenHandle userToken;

		// Token: 0x04000F7A RID: 3962
		private Evidence evidence;
	}
}
