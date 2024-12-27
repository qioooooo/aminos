using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x02000018 RID: 24
	public class PerformanceCounterInstaller : ComponentInstaller
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000487E File Offset: 0x0000387E
		// (set) Token: 0x0600008E RID: 142 RVA: 0x00004886 File Offset: 0x00003886
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[DefaultValue("")]
		[ResDescription("PCCategoryName")]
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
				PerformanceCounterInstaller.CheckValidCategory(value);
				this.categoryName = value;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600008F RID: 143 RVA: 0x000048A3 File Offset: 0x000038A3
		// (set) Token: 0x06000090 RID: 144 RVA: 0x000048AB File Offset: 0x000038AB
		[DefaultValue("")]
		[ResDescription("PCI_CategoryHelp")]
		public string CategoryHelp
		{
			get
			{
				return this.categoryHelp;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.categoryHelp = value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000048C2 File Offset: 0x000038C2
		// (set) Token: 0x06000092 RID: 146 RVA: 0x000048CA File Offset: 0x000038CA
		[ComVisible(false)]
		[DefaultValue(PerformanceCounterCategoryType.Unknown)]
		[ResDescription("PCI_IsMultiInstance")]
		public PerformanceCounterCategoryType CategoryType
		{
			get
			{
				return this.categoryType;
			}
			set
			{
				if (value < PerformanceCounterCategoryType.Unknown || value > PerformanceCounterCategoryType.MultiInstance)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PerformanceCounterCategoryType));
				}
				this.categoryType = value;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000093 RID: 147 RVA: 0x000048F1 File Offset: 0x000038F1
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[ResDescription("PCI_Counters")]
		public CounterCreationDataCollection Counters
		{
			get
			{
				return this.counters;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000048F9 File Offset: 0x000038F9
		// (set) Token: 0x06000095 RID: 149 RVA: 0x00004901 File Offset: 0x00003901
		[ResDescription("PCI_UninstallAction")]
		[DefaultValue(UninstallAction.Remove)]
		public UninstallAction UninstallAction
		{
			get
			{
				return this.uninstallAction;
			}
			set
			{
				if (!Enum.IsDefined(typeof(UninstallAction), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(UninstallAction));
				}
				this.uninstallAction = value;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004938 File Offset: 0x00003938
		public override void CopyFromComponent(IComponent component)
		{
			if (!(component is PerformanceCounter))
			{
				throw new ArgumentException(Res.GetString("NotAPerformanceCounter"));
			}
			PerformanceCounter performanceCounter = (PerformanceCounter)component;
			if (performanceCounter.CategoryName == null || performanceCounter.CategoryName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("IncompletePerformanceCounter"));
			}
			if (!this.CategoryName.Equals(performanceCounter.CategoryName) && !string.IsNullOrEmpty(this.CategoryName))
			{
				throw new ArgumentException(Res.GetString("NewCategory"));
			}
			PerformanceCounterType performanceCounterType = PerformanceCounterType.NumberOfItems32;
			string text = string.Empty;
			if (string.IsNullOrEmpty(this.CategoryName))
			{
				this.CategoryName = performanceCounter.CategoryName;
			}
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				string machineName = performanceCounter.MachineName;
				if (PerformanceCounterCategory.Exists(this.CategoryName, machineName))
				{
					string text2 = "SYSTEM\\CurrentControlSet\\Services\\" + this.CategoryName + "\\Performance";
					RegistryKey registryKey = null;
					try
					{
						if (machineName == "." || string.Compare(machineName, SystemInformation.ComputerName, StringComparison.OrdinalIgnoreCase) == 0)
						{
							registryKey = Registry.LocalMachine.OpenSubKey(text2);
						}
						else
						{
							RegistryKey registryKey2 = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "\\\\" + machineName);
							registryKey = registryKey2.OpenSubKey(text2);
						}
						if (registryKey == null)
						{
							throw new ArgumentException(Res.GetString("NotCustomPerformanceCategory"));
						}
						object value = registryKey.GetValue("Library", null, RegistryValueOptions.DoNotExpandEnvironmentNames);
						if (value == null || !(value is string) || (string.Compare((string)value, "netfxperf.dll", StringComparison.OrdinalIgnoreCase) != 0 && !((string)value).EndsWith("\\netfxperf.dll", StringComparison.OrdinalIgnoreCase)))
						{
							throw new ArgumentException(Res.GetString("NotCustomPerformanceCategory"));
						}
						PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory(this.CategoryName, machineName);
						this.CategoryHelp = performanceCounterCategory.CategoryHelp;
						if (performanceCounterCategory.CounterExists(performanceCounter.CounterName))
						{
							performanceCounterType = performanceCounter.CounterType;
							text = performanceCounter.CounterHelp;
						}
						this.CategoryType = performanceCounterCategory.CategoryType;
					}
					finally
					{
						if (registryKey != null)
						{
							registryKey.Close();
						}
					}
				}
			}
			CounterCreationData counterCreationData = new CounterCreationData(performanceCounter.CounterName, text, performanceCounterType);
			this.Counters.Add(counterCreationData);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004B58 File Offset: 0x00003B58
		private void DoRollback(IDictionary state)
		{
			base.Context.LogMessage(Res.GetString("RestoringPerformanceCounter", new object[] { this.CategoryName }));
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services", true))
			{
				RegistryKey registryKey2;
				if ((bool)state["categoryKeyExisted"])
				{
					registryKey2 = registryKey.OpenSubKey(this.CategoryName, true);
					if (registryKey2 == null)
					{
						registryKey2 = registryKey.CreateSubKey(this.CategoryName);
					}
					registryKey2.DeleteSubKeyTree("Performance");
					SerializableRegistryKey serializableRegistryKey = (SerializableRegistryKey)state["performanceKeyData"];
					if (serializableRegistryKey != null)
					{
						RegistryKey registryKey3 = registryKey2.CreateSubKey("Performance");
						serializableRegistryKey.CopyToRegistry(registryKey3);
						registryKey3.Close();
					}
					registryKey2.DeleteSubKeyTree("Linkage");
					SerializableRegistryKey serializableRegistryKey2 = (SerializableRegistryKey)state["linkageKeyData"];
					if (serializableRegistryKey2 != null)
					{
						RegistryKey registryKey4 = registryKey2.CreateSubKey("Linkage");
						serializableRegistryKey2.CopyToRegistry(registryKey4);
						registryKey4.Close();
					}
				}
				else
				{
					registryKey2 = registryKey.OpenSubKey(this.CategoryName);
					if (registryKey2 != null)
					{
						registryKey2.Close();
						registryKey2 = null;
						registryKey.DeleteSubKeyTree(this.CategoryName);
					}
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
			}
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004C98 File Offset: 0x00003C98
		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);
			base.Context.LogMessage(Res.GetString("CreatingPerformanceCounter", new object[] { this.CategoryName }));
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services", true);
			stateSaver["categoryKeyExisted"] = false;
			try
			{
				if (registryKey2 != null)
				{
					registryKey = registryKey2.OpenSubKey(this.CategoryName, true);
					if (registryKey != null)
					{
						stateSaver["categoryKeyExisted"] = true;
						RegistryKey registryKey3 = registryKey.OpenSubKey("Performance");
						if (registryKey3 != null)
						{
							stateSaver["performanceKeyData"] = new SerializableRegistryKey(registryKey3);
							registryKey3.Close();
							registryKey.DeleteSubKeyTree("Performance");
						}
						registryKey3 = registryKey.OpenSubKey("Linkage");
						if (registryKey3 != null)
						{
							stateSaver["linkageKeyData"] = new SerializableRegistryKey(registryKey3);
							registryKey3.Close();
							registryKey.DeleteSubKeyTree("Linkage");
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
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
			}
			PerformanceCounterCategory.Create(this.CategoryName, this.CategoryHelp, this.categoryType, this.Counters);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004DC8 File Offset: 0x00003DC8
		public override void Rollback(IDictionary savedState)
		{
			base.Rollback(savedState);
			this.DoRollback(savedState);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004DD8 File Offset: 0x00003DD8
		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);
			if (this.UninstallAction == UninstallAction.Remove)
			{
				base.Context.LogMessage(Res.GetString("RemovingPerformanceCounter", new object[] { this.CategoryName }));
				PerformanceCounterCategory.Delete(this.CategoryName);
			}
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004E28 File Offset: 0x00003E28
		internal static void CheckValidCategory(string categoryName)
		{
			if (categoryName == null)
			{
				throw new ArgumentNullException("categoryName");
			}
			if (!PerformanceCounterInstaller.CheckValidId(categoryName))
			{
				throw new ArgumentException(Res.GetString("PerfInvalidCategoryName", new object[] { 1, 80 }));
			}
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004E78 File Offset: 0x00003E78
		internal static bool CheckValidId(string id)
		{
			if (id.Length == 0 || id.Length > 80)
			{
				return false;
			}
			for (int i = 0; i < id.Length; i++)
			{
				char c = id[i];
				if ((i == 0 || i == id.Length - 1) && c == ' ')
				{
					return false;
				}
				if (c == '"')
				{
					return false;
				}
				if (char.IsControl(c))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040000F5 RID: 245
		private const string ServicePath = "SYSTEM\\CurrentControlSet\\Services";

		// Token: 0x040000F6 RID: 246
		private const string PerfShimName = "netfxperf.dll";

		// Token: 0x040000F7 RID: 247
		private const string PerfShimFullNameSuffix = "\\netfxperf.dll";

		// Token: 0x040000F8 RID: 248
		private string categoryName = string.Empty;

		// Token: 0x040000F9 RID: 249
		private CounterCreationDataCollection counters = new CounterCreationDataCollection();

		// Token: 0x040000FA RID: 250
		private string categoryHelp = string.Empty;

		// Token: 0x040000FB RID: 251
		private UninstallAction uninstallAction;

		// Token: 0x040000FC RID: 252
		private PerformanceCounterCategoryType categoryType = PerformanceCounterCategoryType.Unknown;
	}
}
