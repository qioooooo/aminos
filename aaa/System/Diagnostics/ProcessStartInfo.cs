using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000789 RID: 1929
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true, SelfAffectingProcessMgmt = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class ProcessStartInfo
	{
		// Token: 0x06003B6F RID: 15215 RVA: 0x000FD9D8 File Offset: 0x000FC9D8
		public ProcessStartInfo()
		{
		}

		// Token: 0x06003B70 RID: 15216 RVA: 0x000FD9E7 File Offset: 0x000FC9E7
		internal ProcessStartInfo(Process parent)
		{
			this.weakParentProcess = new WeakReference(parent);
		}

		// Token: 0x06003B71 RID: 15217 RVA: 0x000FDA02 File Offset: 0x000FCA02
		public ProcessStartInfo(string fileName)
		{
			this.fileName = fileName;
		}

		// Token: 0x06003B72 RID: 15218 RVA: 0x000FDA18 File Offset: 0x000FCA18
		public ProcessStartInfo(string fileName, string arguments)
		{
			this.fileName = fileName;
			this.arguments = arguments;
		}

		// Token: 0x17000DE9 RID: 3561
		// (get) Token: 0x06003B73 RID: 15219 RVA: 0x000FDA35 File Offset: 0x000FCA35
		// (set) Token: 0x06003B74 RID: 15220 RVA: 0x000FDA4B File Offset: 0x000FCA4B
		[MonitoringDescription("ProcessVerb")]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.VerbConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[NotifyParentProperty(true)]
		public string Verb
		{
			get
			{
				if (this.verb == null)
				{
					return string.Empty;
				}
				return this.verb;
			}
			set
			{
				this.verb = value;
			}
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06003B75 RID: 15221 RVA: 0x000FDA54 File Offset: 0x000FCA54
		// (set) Token: 0x06003B76 RID: 15222 RVA: 0x000FDA6A File Offset: 0x000FCA6A
		[MonitoringDescription("ProcessArguments")]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[RecommendedAsConfigurable(true)]
		[NotifyParentProperty(true)]
		public string Arguments
		{
			get
			{
				if (this.arguments == null)
				{
					return string.Empty;
				}
				return this.arguments;
			}
			set
			{
				this.arguments = value;
			}
		}

		// Token: 0x17000DEB RID: 3563
		// (get) Token: 0x06003B77 RID: 15223 RVA: 0x000FDA73 File Offset: 0x000FCA73
		// (set) Token: 0x06003B78 RID: 15224 RVA: 0x000FDA7B File Offset: 0x000FCA7B
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		[MonitoringDescription("ProcessCreateNoWindow")]
		public bool CreateNoWindow
		{
			get
			{
				return this.createNoWindow;
			}
			set
			{
				this.createNoWindow = value;
			}
		}

		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06003B79 RID: 15225 RVA: 0x000FDA84 File Offset: 0x000FCA84
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Diagnostics.Design.StringDictionaryEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue(null)]
		[MonitoringDescription("ProcessEnvironmentVariables")]
		[NotifyParentProperty(true)]
		public StringDictionary EnvironmentVariables
		{
			get
			{
				if (this.environmentVariables == null)
				{
					this.environmentVariables = new StringDictionary();
					if (this.weakParentProcess == null || !this.weakParentProcess.IsAlive || ((Component)this.weakParentProcess.Target).Site == null || !((Component)this.weakParentProcess.Target).Site.DesignMode)
					{
						foreach (object obj in Environment.GetEnvironmentVariables())
						{
							DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
							this.environmentVariables.Add((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
						}
					}
				}
				return this.environmentVariables;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06003B7A RID: 15226 RVA: 0x000FDB5C File Offset: 0x000FCB5C
		// (set) Token: 0x06003B7B RID: 15227 RVA: 0x000FDB64 File Offset: 0x000FCB64
		[DefaultValue(false)]
		[MonitoringDescription("ProcessRedirectStandardInput")]
		[NotifyParentProperty(true)]
		public bool RedirectStandardInput
		{
			get
			{
				return this.redirectStandardInput;
			}
			set
			{
				this.redirectStandardInput = value;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06003B7C RID: 15228 RVA: 0x000FDB6D File Offset: 0x000FCB6D
		// (set) Token: 0x06003B7D RID: 15229 RVA: 0x000FDB75 File Offset: 0x000FCB75
		[NotifyParentProperty(true)]
		[DefaultValue(false)]
		[MonitoringDescription("ProcessRedirectStandardOutput")]
		public bool RedirectStandardOutput
		{
			get
			{
				return this.redirectStandardOutput;
			}
			set
			{
				this.redirectStandardOutput = value;
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06003B7E RID: 15230 RVA: 0x000FDB7E File Offset: 0x000FCB7E
		// (set) Token: 0x06003B7F RID: 15231 RVA: 0x000FDB86 File Offset: 0x000FCB86
		[DefaultValue(false)]
		[MonitoringDescription("ProcessRedirectStandardError")]
		[NotifyParentProperty(true)]
		public bool RedirectStandardError
		{
			get
			{
				return this.redirectStandardError;
			}
			set
			{
				this.redirectStandardError = value;
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06003B80 RID: 15232 RVA: 0x000FDB8F File Offset: 0x000FCB8F
		// (set) Token: 0x06003B81 RID: 15233 RVA: 0x000FDB97 File Offset: 0x000FCB97
		public Encoding StandardErrorEncoding
		{
			get
			{
				return this.standardErrorEncoding;
			}
			set
			{
				this.standardErrorEncoding = value;
			}
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06003B82 RID: 15234 RVA: 0x000FDBA0 File Offset: 0x000FCBA0
		// (set) Token: 0x06003B83 RID: 15235 RVA: 0x000FDBA8 File Offset: 0x000FCBA8
		public Encoding StandardOutputEncoding
		{
			get
			{
				return this.standardOutputEncoding;
			}
			set
			{
				this.standardOutputEncoding = value;
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06003B84 RID: 15236 RVA: 0x000FDBB1 File Offset: 0x000FCBB1
		// (set) Token: 0x06003B85 RID: 15237 RVA: 0x000FDBB9 File Offset: 0x000FCBB9
		[DefaultValue(true)]
		[NotifyParentProperty(true)]
		[MonitoringDescription("ProcessUseShellExecute")]
		public bool UseShellExecute
		{
			get
			{
				return this.useShellExecute;
			}
			set
			{
				this.useShellExecute = value;
			}
		}

		// Token: 0x17000DF3 RID: 3571
		// (get) Token: 0x06003B86 RID: 15238 RVA: 0x000FDBC4 File Offset: 0x000FCBC4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string[] Verbs
		{
			get
			{
				ArrayList arrayList = new ArrayList();
				RegistryKey registryKey = null;
				string extension = Path.GetExtension(this.FileName);
				try
				{
					if (extension != null && extension.Length > 0)
					{
						registryKey = Registry.ClassesRoot.OpenSubKey(extension);
						if (registryKey != null)
						{
							string text = (string)registryKey.GetValue(string.Empty);
							registryKey.Close();
							registryKey = Registry.ClassesRoot.OpenSubKey(text + "\\shell");
							if (registryKey != null)
							{
								string[] subKeyNames = registryKey.GetSubKeyNames();
								for (int i = 0; i < subKeyNames.Length; i++)
								{
									if (string.Compare(subKeyNames[i], "new", StringComparison.OrdinalIgnoreCase) != 0)
									{
										arrayList.Add(subKeyNames[i]);
									}
								}
								registryKey.Close();
								registryKey = null;
							}
						}
					}
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Close();
					}
				}
				string[] array = new string[arrayList.Count];
				arrayList.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x17000DF4 RID: 3572
		// (get) Token: 0x06003B87 RID: 15239 RVA: 0x000FDCAC File Offset: 0x000FCCAC
		// (set) Token: 0x06003B88 RID: 15240 RVA: 0x000FDCC2 File Offset: 0x000FCCC2
		[NotifyParentProperty(true)]
		public string UserName
		{
			get
			{
				if (this.userName == null)
				{
					return string.Empty;
				}
				return this.userName;
			}
			set
			{
				this.userName = value;
			}
		}

		// Token: 0x17000DF5 RID: 3573
		// (get) Token: 0x06003B89 RID: 15241 RVA: 0x000FDCCB File Offset: 0x000FCCCB
		// (set) Token: 0x06003B8A RID: 15242 RVA: 0x000FDCD3 File Offset: 0x000FCCD3
		public SecureString Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		// Token: 0x17000DF6 RID: 3574
		// (get) Token: 0x06003B8B RID: 15243 RVA: 0x000FDCDC File Offset: 0x000FCCDC
		// (set) Token: 0x06003B8C RID: 15244 RVA: 0x000FDCF2 File Offset: 0x000FCCF2
		[NotifyParentProperty(true)]
		public string Domain
		{
			get
			{
				if (this.domain == null)
				{
					return string.Empty;
				}
				return this.domain;
			}
			set
			{
				this.domain = value;
			}
		}

		// Token: 0x17000DF7 RID: 3575
		// (get) Token: 0x06003B8D RID: 15245 RVA: 0x000FDCFB File Offset: 0x000FCCFB
		// (set) Token: 0x06003B8E RID: 15246 RVA: 0x000FDD03 File Offset: 0x000FCD03
		[NotifyParentProperty(true)]
		public bool LoadUserProfile
		{
			get
			{
				return this.loadUserProfile;
			}
			set
			{
				this.loadUserProfile = value;
			}
		}

		// Token: 0x17000DF8 RID: 3576
		// (get) Token: 0x06003B8F RID: 15247 RVA: 0x000FDD0C File Offset: 0x000FCD0C
		// (set) Token: 0x06003B90 RID: 15248 RVA: 0x000FDD22 File Offset: 0x000FCD22
		[Editor("System.Diagnostics.Design.StartFileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[NotifyParentProperty(true)]
		[DefaultValue("")]
		[MonitoringDescription("ProcessFileName")]
		[RecommendedAsConfigurable(true)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string FileName
		{
			get
			{
				if (this.fileName == null)
				{
					return string.Empty;
				}
				return this.fileName;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x17000DF9 RID: 3577
		// (get) Token: 0x06003B91 RID: 15249 RVA: 0x000FDD2B File Offset: 0x000FCD2B
		// (set) Token: 0x06003B92 RID: 15250 RVA: 0x000FDD41 File Offset: 0x000FCD41
		[RecommendedAsConfigurable(true)]
		[DefaultValue("")]
		[MonitoringDescription("ProcessWorkingDirectory")]
		[Editor("System.Diagnostics.Design.WorkingDirectoryEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[NotifyParentProperty(true)]
		public string WorkingDirectory
		{
			get
			{
				if (this.directory == null)
				{
					return string.Empty;
				}
				return this.directory;
			}
			set
			{
				this.directory = value;
			}
		}

		// Token: 0x17000DFA RID: 3578
		// (get) Token: 0x06003B93 RID: 15251 RVA: 0x000FDD4A File Offset: 0x000FCD4A
		// (set) Token: 0x06003B94 RID: 15252 RVA: 0x000FDD52 File Offset: 0x000FCD52
		[DefaultValue(false)]
		[NotifyParentProperty(true)]
		[MonitoringDescription("ProcessErrorDialog")]
		public bool ErrorDialog
		{
			get
			{
				return this.errorDialog;
			}
			set
			{
				this.errorDialog = value;
			}
		}

		// Token: 0x17000DFB RID: 3579
		// (get) Token: 0x06003B95 RID: 15253 RVA: 0x000FDD5B File Offset: 0x000FCD5B
		// (set) Token: 0x06003B96 RID: 15254 RVA: 0x000FDD63 File Offset: 0x000FCD63
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IntPtr ErrorDialogParentHandle
		{
			get
			{
				return this.errorDialogParentHandle;
			}
			set
			{
				this.errorDialogParentHandle = value;
			}
		}

		// Token: 0x17000DFC RID: 3580
		// (get) Token: 0x06003B97 RID: 15255 RVA: 0x000FDD6C File Offset: 0x000FCD6C
		// (set) Token: 0x06003B98 RID: 15256 RVA: 0x000FDD74 File Offset: 0x000FCD74
		[NotifyParentProperty(true)]
		[DefaultValue(ProcessWindowStyle.Normal)]
		[MonitoringDescription("ProcessWindowStyle")]
		public ProcessWindowStyle WindowStyle
		{
			get
			{
				return this.windowStyle;
			}
			set
			{
				if (!Enum.IsDefined(typeof(ProcessWindowStyle), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ProcessWindowStyle));
				}
				this.windowStyle = value;
			}
		}

		// Token: 0x04003429 RID: 13353
		private string fileName;

		// Token: 0x0400342A RID: 13354
		private string arguments;

		// Token: 0x0400342B RID: 13355
		private string directory;

		// Token: 0x0400342C RID: 13356
		private string verb;

		// Token: 0x0400342D RID: 13357
		private ProcessWindowStyle windowStyle;

		// Token: 0x0400342E RID: 13358
		private bool errorDialog;

		// Token: 0x0400342F RID: 13359
		private IntPtr errorDialogParentHandle;

		// Token: 0x04003430 RID: 13360
		private bool useShellExecute = true;

		// Token: 0x04003431 RID: 13361
		private string userName;

		// Token: 0x04003432 RID: 13362
		private string domain;

		// Token: 0x04003433 RID: 13363
		private SecureString password;

		// Token: 0x04003434 RID: 13364
		private bool loadUserProfile;

		// Token: 0x04003435 RID: 13365
		private bool redirectStandardInput;

		// Token: 0x04003436 RID: 13366
		private bool redirectStandardOutput;

		// Token: 0x04003437 RID: 13367
		private bool redirectStandardError;

		// Token: 0x04003438 RID: 13368
		private Encoding standardOutputEncoding;

		// Token: 0x04003439 RID: 13369
		private Encoding standardErrorEncoding;

		// Token: 0x0400343A RID: 13370
		private bool createNoWindow;

		// Token: 0x0400343B RID: 13371
		private WeakReference weakParentProcess;

		// Token: 0x0400343C RID: 13372
		internal StringDictionary environmentVariables;
	}
}
