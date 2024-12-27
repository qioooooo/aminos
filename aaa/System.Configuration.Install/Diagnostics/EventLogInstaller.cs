using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200000E RID: 14
	public class EventLogInstaller : ComponentInstaller
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004E RID: 78 RVA: 0x0000393B File Offset: 0x0000293B
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00003948 File Offset: 0x00002948
		[ResDescription("Desc_CategoryResourceFile")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[Editor("System.Windows.Forms.Design.FileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ComVisible(false)]
		public string CategoryResourceFile
		{
			get
			{
				return this.sourceData.CategoryResourceFile;
			}
			set
			{
				this.sourceData.CategoryResourceFile = value;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003956 File Offset: 0x00002956
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00003963 File Offset: 0x00002963
		[ResDescription("Desc_CategoryCount")]
		[ComVisible(false)]
		public int CategoryCount
		{
			get
			{
				return this.sourceData.CategoryCount;
			}
			set
			{
				this.sourceData.CategoryCount = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003974 File Offset: 0x00002974
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000039C6 File Offset: 0x000029C6
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResDescription("Desc_Log")]
		public string Log
		{
			get
			{
				if (this.sourceData.LogName == null && this.sourceData.Source != null)
				{
					this.sourceData.LogName = EventLog.LogNameFromSourceName(this.sourceData.Source, ".");
				}
				return this.sourceData.LogName;
			}
			set
			{
				this.sourceData.LogName = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000054 RID: 84 RVA: 0x000039D4 File Offset: 0x000029D4
		// (set) Token: 0x06000055 RID: 85 RVA: 0x000039E1 File Offset: 0x000029E1
		[ComVisible(false)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResDescription("Desc_MessageResourceFile")]
		[Editor("System.Windows.Forms.Design.FileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string MessageResourceFile
		{
			get
			{
				return this.sourceData.MessageResourceFile;
			}
			set
			{
				this.sourceData.MessageResourceFile = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000056 RID: 86 RVA: 0x000039EF File Offset: 0x000029EF
		// (set) Token: 0x06000057 RID: 87 RVA: 0x000039FC File Offset: 0x000029FC
		[ResDescription("Desc_ParameterResourceFile")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ComVisible(false)]
		[Editor("System.Windows.Forms.Design.FileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string ParameterResourceFile
		{
			get
			{
				return this.sourceData.ParameterResourceFile;
			}
			set
			{
				this.sourceData.ParameterResourceFile = value;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000058 RID: 88 RVA: 0x00003A0A File Offset: 0x00002A0A
		// (set) Token: 0x06000059 RID: 89 RVA: 0x00003A17 File Offset: 0x00002A17
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ResDescription("Desc_Source")]
		public string Source
		{
			get
			{
				return this.sourceData.Source;
			}
			set
			{
				this.sourceData.Source = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003A25 File Offset: 0x00002A25
		// (set) Token: 0x0600005B RID: 91 RVA: 0x00003A2D File Offset: 0x00002A2D
		[DefaultValue(UninstallAction.Remove)]
		[ResDescription("Desc_UninstallAction")]
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

		// Token: 0x0600005C RID: 92 RVA: 0x00003A64 File Offset: 0x00002A64
		public override void CopyFromComponent(IComponent component)
		{
			EventLog eventLog = component as EventLog;
			if (eventLog == null)
			{
				throw new ArgumentException(Res.GetString("NotAnEventLog"));
			}
			if (eventLog.Log == null || eventLog.Log == string.Empty || eventLog.Source == null || eventLog.Source == string.Empty)
			{
				throw new ArgumentException(Res.GetString("IncompleteEventLog"));
			}
			this.Log = eventLog.Log;
			this.Source = eventLog.Source;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003AE8 File Offset: 0x00002AE8
		public override void Install(IDictionary stateSaver)
		{
			base.Install(stateSaver);
			base.Context.LogMessage(Res.GetString("CreatingEventLog", new object[] { this.Source, this.Log }));
			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
			{
				throw new PlatformNotSupportedException(Res.GetString("WinNTRequired"));
			}
			stateSaver["baseInstalledAndPlatformOK"] = true;
			bool flag = EventLog.Exists(this.Log, ".");
			stateSaver["logExists"] = flag;
			bool flag2 = EventLog.SourceExists(this.Source, ".");
			stateSaver["alreadyRegistered"] = flag2;
			if (flag2)
			{
				string text = EventLog.LogNameFromSourceName(this.Source, ".");
				if (text == this.Log)
				{
					return;
				}
			}
			EventLog.CreateEventSource(this.sourceData);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003BCC File Offset: 0x00002BCC
		public override bool IsEquivalentInstaller(ComponentInstaller otherInstaller)
		{
			EventLogInstaller eventLogInstaller = otherInstaller as EventLogInstaller;
			return eventLogInstaller != null && eventLogInstaller.Source == this.Source;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003BF8 File Offset: 0x00002BF8
		public override void Rollback(IDictionary savedState)
		{
			base.Rollback(savedState);
			base.Context.LogMessage(Res.GetString("RestoringEventLog", new object[] { this.Source }));
			if (savedState["baseInstalledAndPlatformOK"] != null)
			{
				if (!(bool)savedState["logExists"])
				{
					EventLog.Delete(this.Log, ".");
					return;
				}
				object obj = savedState["alreadyRegistered"];
				bool flag = obj != null && (bool)obj;
				if (!flag && EventLog.SourceExists(this.Source, "."))
				{
					EventLog.DeleteEventSource(this.Source, ".");
				}
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003CA4 File Offset: 0x00002CA4
		public override void Uninstall(IDictionary savedState)
		{
			base.Uninstall(savedState);
			if (this.UninstallAction == UninstallAction.Remove)
			{
				base.Context.LogMessage(Res.GetString("RemovingEventLog", new object[] { this.Source }));
				if (EventLog.SourceExists(this.Source, "."))
				{
					if (string.Compare(this.Log, this.Source, StringComparison.OrdinalIgnoreCase) != 0)
					{
						EventLog.DeleteEventSource(this.Source, ".");
					}
				}
				else
				{
					base.Context.LogMessage(Res.GetString("LocalSourceNotRegisteredWarning", new object[] { this.Source }));
				}
				RegistryKey registryKey = Registry.LocalMachine;
				RegistryKey registryKey2 = null;
				try
				{
					registryKey = registryKey.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\EventLog", false);
					if (registryKey != null)
					{
						registryKey2 = registryKey.OpenSubKey(this.Log, false);
					}
					if (registryKey2 != null)
					{
						string[] subKeyNames = registryKey2.GetSubKeyNames();
						if (subKeyNames == null || subKeyNames.Length == 0 || (subKeyNames.Length == 1 && string.Compare(subKeyNames[0], this.Log, StringComparison.OrdinalIgnoreCase) == 0))
						{
							base.Context.LogMessage(Res.GetString("DeletingEventLog", new object[] { this.Log }));
							EventLog.Delete(this.Log, ".");
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
			}
		}

		// Token: 0x040000EE RID: 238
		private EventSourceCreationData sourceData = new EventSourceCreationData(null, null);

		// Token: 0x040000EF RID: 239
		private UninstallAction uninstallAction;
	}
}
