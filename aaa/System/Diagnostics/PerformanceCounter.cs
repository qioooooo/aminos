using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x02000763 RID: 1891
	[SRDescription("PerformanceCounterDesc")]
	[InstallerType("System.Diagnostics.PerformanceCounterInstaller,System.Configuration.Install, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SharedState = true)]
	public sealed class PerformanceCounter : Component, ISupportInitialize
	{
		// Token: 0x06003A00 RID: 14848 RVA: 0x000F5570 File Offset: 0x000F4570
		public PerformanceCounter()
		{
			this.machineName = ".";
			this.categoryName = string.Empty;
			this.counterName = string.Empty;
			this.instanceName = string.Empty;
			this.isReadOnly = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x000F55D0 File Offset: 0x000F45D0
		public PerformanceCounter(string categoryName, string counterName, string instanceName, string machineName)
		{
			this.MachineName = machineName;
			this.CategoryName = categoryName;
			this.CounterName = counterName;
			this.InstanceName = instanceName;
			this.isReadOnly = true;
			this.Initialize();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x000F5628 File Offset: 0x000F4628
		internal PerformanceCounter(string categoryName, string counterName, string instanceName, string machineName, bool skipInit)
		{
			this.MachineName = machineName;
			this.CategoryName = categoryName;
			this.CounterName = counterName;
			this.InstanceName = instanceName;
			this.isReadOnly = true;
			this.initialized = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x000F567E File Offset: 0x000F467E
		public PerformanceCounter(string categoryName, string counterName, string instanceName)
			: this(categoryName, counterName, instanceName, true)
		{
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x000F568C File Offset: 0x000F468C
		public PerformanceCounter(string categoryName, string counterName, string instanceName, bool readOnly)
		{
			this.MachineName = ".";
			this.CategoryName = categoryName;
			this.CounterName = counterName;
			this.InstanceName = instanceName;
			this.isReadOnly = readOnly;
			this.Initialize();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x000F56E5 File Offset: 0x000F46E5
		public PerformanceCounter(string categoryName, string counterName)
			: this(categoryName, counterName, true)
		{
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x000F56F0 File Offset: 0x000F46F0
		public PerformanceCounter(string categoryName, string counterName, bool readOnly)
			: this(categoryName, counterName, "", readOnly)
		{
		}

		// Token: 0x17000D8A RID: 3466
		// (get) Token: 0x06003A07 RID: 14855 RVA: 0x000F5700 File Offset: 0x000F4700
		// (set) Token: 0x06003A08 RID: 14856 RVA: 0x000F5708 File Offset: 0x000F4708
		[ReadOnly(true)]
		[SRDescription("PCCategoryName")]
		[RecommendedAsConfigurable(true)]
		[TypeConverter("System.Diagnostics.Design.CategoryValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.categoryName == null || string.Compare(this.categoryName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.categoryName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x06003A09 RID: 14857 RVA: 0x000F573C File Offset: 0x000F473C
		[MonitoringDescription("PC_CounterHelp")]
		[ReadOnly(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string CounterHelp
		{
			get
			{
				string text = this.categoryName;
				string text2 = this.machineName;
				PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, text2, text);
				performanceCounterPermission.Demand();
				this.Initialize();
				if (this.helpMsg == null)
				{
					this.helpMsg = PerformanceCounterLib.GetCounterHelp(text2, text, this.counterName);
				}
				return this.helpMsg;
			}
		}

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x06003A0A RID: 14858 RVA: 0x000F578D File Offset: 0x000F478D
		// (set) Token: 0x06003A0B RID: 14859 RVA: 0x000F5795 File Offset: 0x000F4795
		[ReadOnly(true)]
		[SRDescription("PCCounterName")]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.CounterNameConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[RecommendedAsConfigurable(true)]
		public string CounterName
		{
			get
			{
				return this.counterName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.counterName == null || string.Compare(this.counterName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.counterName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D8D RID: 3469
		// (get) Token: 0x06003A0C RID: 14860 RVA: 0x000F57CC File Offset: 0x000F47CC
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("PC_CounterType")]
		public PerformanceCounterType CounterType
		{
			get
			{
				if (this.counterType == -1)
				{
					string text = this.categoryName;
					string text2 = this.machineName;
					PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, text2, text);
					performanceCounterPermission.Demand();
					this.Initialize();
					CategorySample categorySample = PerformanceCounterLib.GetCategorySample(text2, text);
					CounterDefinitionSample counterDefinitionSample = categorySample.GetCounterDefinitionSample(this.counterName);
					this.counterType = counterDefinitionSample.CounterType;
				}
				return (PerformanceCounterType)this.counterType;
			}
		}

		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06003A0D RID: 14861 RVA: 0x000F582E File Offset: 0x000F482E
		// (set) Token: 0x06003A0E RID: 14862 RVA: 0x000F5836 File Offset: 0x000F4836
		[SRDescription("PCInstanceLifetime")]
		[DefaultValue(PerformanceCounterInstanceLifetime.Global)]
		public PerformanceCounterInstanceLifetime InstanceLifetime
		{
			get
			{
				return this.instanceLifetime;
			}
			set
			{
				if (value > PerformanceCounterInstanceLifetime.Process || value < PerformanceCounterInstanceLifetime.Global)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("CantSetLifetimeAfterInitialized"));
				}
				this.instanceLifetime = value;
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06003A0F RID: 14863 RVA: 0x000F586A File Offset: 0x000F486A
		// (set) Token: 0x06003A10 RID: 14864 RVA: 0x000F5872 File Offset: 0x000F4872
		[SRDescription("PCInstanceName")]
		[RecommendedAsConfigurable(true)]
		[ReadOnly(true)]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.InstanceNameConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string InstanceName
		{
			get
			{
				return this.instanceName;
			}
			set
			{
				if (value == null && this.instanceName == null)
				{
					return;
				}
				if ((value == null && this.instanceName != null) || (value != null && this.instanceName == null) || string.Compare(this.instanceName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.instanceName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06003A11 RID: 14865 RVA: 0x000F58B2 File Offset: 0x000F48B2
		// (set) Token: 0x06003A12 RID: 14866 RVA: 0x000F58BA File Offset: 0x000F48BA
		[MonitoringDescription("PC_ReadOnly")]
		[DefaultValue(true)]
		[Browsable(false)]
		public bool ReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
			set
			{
				if (value != this.isReadOnly)
				{
					this.isReadOnly = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06003A13 RID: 14867 RVA: 0x000F58D2 File Offset: 0x000F48D2
		// (set) Token: 0x06003A14 RID: 14868 RVA: 0x000F58DC File Offset: 0x000F48DC
		[SRDescription("PCMachineName")]
		[RecommendedAsConfigurable(true)]
		[DefaultValue(".")]
		[Browsable(false)]
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", value }));
				}
				if (this.machineName != value)
				{
					this.machineName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06003A15 RID: 14869 RVA: 0x000F5930 File Offset: 0x000F4930
		// (set) Token: 0x06003A16 RID: 14870 RVA: 0x000F5965 File Offset: 0x000F4965
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("PC_RawValue")]
		public long RawValue
		{
			get
			{
				if (this.ReadOnly)
				{
					return this.NextSample().RawValue;
				}
				this.Initialize();
				return this.sharedCounter.Value;
			}
			set
			{
				if (this.ReadOnly)
				{
					this.ThrowReadOnly();
				}
				this.Initialize();
				this.sharedCounter.Value = value;
			}
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x000F5987 File Offset: 0x000F4987
		public void BeginInit()
		{
			this.Close();
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x000F598F File Offset: 0x000F498F
		public void Close()
		{
			this.helpMsg = null;
			this.oldSample = CounterSample.Empty;
			this.sharedCounter = null;
			this.initialized = false;
			this.counterType = -1;
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x000F59B8 File Offset: 0x000F49B8
		public static void CloseSharedResources()
		{
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, ".", "*");
			performanceCounterPermission.Demand();
			PerformanceCounterLib.CloseAllLibraries();
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x000F59E1 File Offset: 0x000F49E1
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x000F59F3 File Offset: 0x000F49F3
		public long Decrement()
		{
			if (this.ReadOnly)
			{
				this.ThrowReadOnly();
			}
			this.Initialize();
			return this.sharedCounter.Decrement();
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x000F5A14 File Offset: 0x000F4A14
		public void EndInit()
		{
			this.Initialize();
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x000F5A1C File Offset: 0x000F4A1C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public long IncrementBy(long value)
		{
			if (this.isReadOnly)
			{
				this.ThrowReadOnly();
			}
			this.Initialize();
			return this.sharedCounter.IncrementBy(value);
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x000F5A3E File Offset: 0x000F4A3E
		public long Increment()
		{
			if (this.isReadOnly)
			{
				this.ThrowReadOnly();
			}
			this.Initialize();
			return this.sharedCounter.Increment();
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x000F5A5F File Offset: 0x000F4A5F
		private void ThrowReadOnly()
		{
			throw new InvalidOperationException(SR.GetString("ReadOnlyCounter"));
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x000F5A70 File Offset: 0x000F4A70
		private void Initialize()
		{
			if (!this.initialized && !base.DesignMode)
			{
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						Monitor.Enter(this);
						flag = true;
					}
					if (!this.initialized)
					{
						string text = this.categoryName;
						string text2 = this.machineName;
						if (text == string.Empty)
						{
							throw new InvalidOperationException(SR.GetString("CategoryNameMissing"));
						}
						if (this.counterName == string.Empty)
						{
							throw new InvalidOperationException(SR.GetString("CounterNameMissing"));
						}
						if (this.ReadOnly)
						{
							PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, text2, text);
							performanceCounterPermission.Demand();
							if (!PerformanceCounterLib.CounterExists(text2, text, this.counterName))
							{
								throw new InvalidOperationException(SR.GetString("CounterExists", new object[] { text, this.counterName }));
							}
							PerformanceCounterCategoryType categoryType = PerformanceCounterLib.GetCategoryType(text2, text);
							if (categoryType == PerformanceCounterCategoryType.MultiInstance)
							{
								if (string.IsNullOrEmpty(this.instanceName))
								{
									throw new InvalidOperationException(SR.GetString("MultiInstanceOnly", new object[] { text }));
								}
							}
							else if (categoryType == PerformanceCounterCategoryType.SingleInstance && !string.IsNullOrEmpty(this.instanceName))
							{
								throw new InvalidOperationException(SR.GetString("SingleInstanceOnly", new object[] { text }));
							}
							if (this.instanceLifetime != PerformanceCounterInstanceLifetime.Global)
							{
								throw new InvalidOperationException(SR.GetString("InstanceLifetimeProcessonReadOnly"));
							}
							this.initialized = true;
						}
						else
						{
							PerformanceCounterPermission performanceCounterPermission2 = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Write, text2, text);
							performanceCounterPermission2.Demand();
							if (text2 != "." && string.Compare(text2, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) != 0)
							{
								throw new InvalidOperationException(SR.GetString("RemoteWriting"));
							}
							SharedUtils.CheckNtEnvironment();
							if (!PerformanceCounterLib.IsCustomCategory(text2, text))
							{
								throw new InvalidOperationException(SR.GetString("NotCustomCounter"));
							}
							PerformanceCounterCategoryType categoryType2 = PerformanceCounterLib.GetCategoryType(text2, text);
							if (categoryType2 == PerformanceCounterCategoryType.MultiInstance)
							{
								if (string.IsNullOrEmpty(this.instanceName))
								{
									throw new InvalidOperationException(SR.GetString("MultiInstanceOnly", new object[] { text }));
								}
							}
							else if (categoryType2 == PerformanceCounterCategoryType.SingleInstance && !string.IsNullOrEmpty(this.instanceName))
							{
								throw new InvalidOperationException(SR.GetString("SingleInstanceOnly", new object[] { text }));
							}
							if (string.IsNullOrEmpty(this.instanceName) && this.InstanceLifetime == PerformanceCounterInstanceLifetime.Process)
							{
								throw new InvalidOperationException(SR.GetString("InstanceLifetimeProcessforSingleInstance"));
							}
							this.sharedCounter = new SharedPerformanceCounter(text.ToLower(CultureInfo.InvariantCulture), this.counterName.ToLower(CultureInfo.InvariantCulture), this.instanceName.ToLower(CultureInfo.InvariantCulture), this.instanceLifetime);
							this.initialized = true;
						}
					}
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this);
					}
				}
			}
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x000F5D50 File Offset: 0x000F4D50
		public CounterSample NextSample()
		{
			string text = this.categoryName;
			string text2 = this.machineName;
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, text2, text);
			performanceCounterPermission.Demand();
			this.Initialize();
			CategorySample categorySample = PerformanceCounterLib.GetCategorySample(text2, text);
			CounterDefinitionSample counterDefinitionSample = categorySample.GetCounterDefinitionSample(this.counterName);
			this.counterType = counterDefinitionSample.CounterType;
			if (!categorySample.IsMultiInstance)
			{
				if (this.instanceName != null && this.instanceName.Length != 0)
				{
					throw new InvalidOperationException(SR.GetString("InstanceNameProhibited", new object[] { this.instanceName }));
				}
				return counterDefinitionSample.GetSingleValue();
			}
			else
			{
				if (this.instanceName == null || this.instanceName.Length == 0)
				{
					throw new InvalidOperationException(SR.GetString("InstanceNameRequired"));
				}
				return counterDefinitionSample.GetInstanceValue(this.instanceName);
			}
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x000F5E20 File Offset: 0x000F4E20
		public float NextValue()
		{
			CounterSample counterSample = this.NextSample();
			float num = CounterSample.Calculate(this.oldSample, counterSample);
			this.oldSample = counterSample;
			return num;
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x000F5E50 File Offset: 0x000F4E50
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void RemoveInstance()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("ReadOnlyRemoveInstance"));
			}
			this.Initialize();
			this.sharedCounter.RemoveInstance(this.instanceName.ToLower(CultureInfo.InvariantCulture), this.instanceLifetime);
		}

		// Token: 0x040032EC RID: 13036
		private string machineName;

		// Token: 0x040032ED RID: 13037
		private string categoryName;

		// Token: 0x040032EE RID: 13038
		private string counterName;

		// Token: 0x040032EF RID: 13039
		private string instanceName;

		// Token: 0x040032F0 RID: 13040
		private PerformanceCounterInstanceLifetime instanceLifetime;

		// Token: 0x040032F1 RID: 13041
		private bool isReadOnly;

		// Token: 0x040032F2 RID: 13042
		private bool initialized;

		// Token: 0x040032F3 RID: 13043
		private string helpMsg;

		// Token: 0x040032F4 RID: 13044
		private int counterType = -1;

		// Token: 0x040032F5 RID: 13045
		private CounterSample oldSample = CounterSample.Empty;

		// Token: 0x040032F6 RID: 13046
		private SharedPerformanceCounter sharedCounter;

		// Token: 0x040032F7 RID: 13047
		[Obsolete("This field has been deprecated and is not used.  Use machine.config or an application configuration file to set the size of the PerformanceCounter file mapping.")]
		public static int DefaultFileMappingSize = 524288;
	}
}
