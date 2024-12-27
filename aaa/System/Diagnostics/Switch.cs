using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Xml.Serialization;

namespace System.Diagnostics
{
	// Token: 0x020001B8 RID: 440
	public abstract class Switch
	{
		// Token: 0x06000D69 RID: 3433 RVA: 0x0002B05A File Offset: 0x0002A05A
		protected Switch(string displayName, string description)
			: this(displayName, description, "0")
		{
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0002B06C File Offset: 0x0002A06C
		protected Switch(string displayName, string description, string defaultSwitchValue)
		{
			if (displayName == null)
			{
				displayName = string.Empty;
			}
			this.displayName = displayName;
			this.description = description;
			lock (Switch.switches)
			{
				Switch.switches.Add(new WeakReference(this));
			}
			this.defaultValue = defaultSwitchValue;
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0002B0E0 File Offset: 0x0002A0E0
		[XmlIgnore]
		public StringDictionary Attributes
		{
			get
			{
				this.Initialize();
				if (this.attributes == null)
				{
					this.attributes = new StringDictionary();
				}
				return this.attributes;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000D6C RID: 3436 RVA: 0x0002B101 File Offset: 0x0002A101
		public string DisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000D6D RID: 3437 RVA: 0x0002B109 File Offset: 0x0002A109
		public string Description
		{
			get
			{
				if (this.description != null)
				{
					return this.description;
				}
				return string.Empty;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0002B11F File Offset: 0x0002A11F
		// (set) Token: 0x06000D6F RID: 3439 RVA: 0x0002B13F File Offset: 0x0002A13F
		protected int SwitchSetting
		{
			get
			{
				if (!this.initialized)
				{
					if (!this.InitializeWithStatus())
					{
						return 0;
					}
					this.OnSwitchSettingChanged();
				}
				return this.switchSetting;
			}
			set
			{
				this.initialized = true;
				if (this.switchSetting != value)
				{
					this.switchSetting = value;
					this.OnSwitchSettingChanged();
				}
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000D70 RID: 3440 RVA: 0x0002B15E File Offset: 0x0002A15E
		// (set) Token: 0x06000D71 RID: 3441 RVA: 0x0002B16C File Offset: 0x0002A16C
		protected string Value
		{
			get
			{
				this.Initialize();
				return this.switchValueString;
			}
			set
			{
				this.Initialize();
				this.switchValueString = value;
				try
				{
					this.OnValueChanged();
				}
				catch (ArgumentException ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("BadConfigSwitchValue", new object[] { this.DisplayName }), ex);
				}
				catch (FormatException ex2)
				{
					throw new ConfigurationErrorsException(SR.GetString("BadConfigSwitchValue", new object[] { this.DisplayName }), ex2);
				}
				catch (OverflowException ex3)
				{
					throw new ConfigurationErrorsException(SR.GetString("BadConfigSwitchValue", new object[] { this.DisplayName }), ex3);
				}
			}
		}

		// Token: 0x06000D72 RID: 3442 RVA: 0x0002B228 File Offset: 0x0002A228
		private void Initialize()
		{
			this.InitializeWithStatus();
		}

		// Token: 0x06000D73 RID: 3443 RVA: 0x0002B234 File Offset: 0x0002A234
		private bool InitializeWithStatus()
		{
			if (!this.initialized)
			{
				if (this.initializing)
				{
					return false;
				}
				this.initializing = true;
				if (this.switchSettings == null && !this.InitializeConfigSettings())
				{
					return false;
				}
				if (this.switchSettings != null)
				{
					SwitchElement switchElement = this.switchSettings[this.displayName];
					if (switchElement != null)
					{
						string value = switchElement.Value;
						if (value != null)
						{
							this.Value = value;
						}
						else
						{
							this.Value = this.defaultValue;
						}
						try
						{
							TraceUtils.VerifyAttributes(switchElement.Attributes, this.GetSupportedAttributes(), this);
						}
						catch (ConfigurationException)
						{
							this.initialized = false;
							this.initializing = false;
							throw;
						}
						this.attributes = new StringDictionary();
						this.attributes.contents = switchElement.Attributes;
					}
					else
					{
						this.switchValueString = this.defaultValue;
						this.OnValueChanged();
					}
				}
				else
				{
					this.switchValueString = this.defaultValue;
					this.OnValueChanged();
				}
				this.initialized = true;
				this.initializing = false;
			}
			return true;
		}

		// Token: 0x06000D74 RID: 3444 RVA: 0x0002B338 File Offset: 0x0002A338
		private bool InitializeConfigSettings()
		{
			if (this.switchSettings != null)
			{
				return true;
			}
			if (!DiagnosticsConfiguration.CanInitialize())
			{
				return false;
			}
			this.switchSettings = DiagnosticsConfiguration.SwitchSettings;
			return true;
		}

		// Token: 0x06000D75 RID: 3445 RVA: 0x0002B359 File Offset: 0x0002A359
		protected internal virtual string[] GetSupportedAttributes()
		{
			return null;
		}

		// Token: 0x06000D76 RID: 3446 RVA: 0x0002B35C File Offset: 0x0002A35C
		protected virtual void OnSwitchSettingChanged()
		{
		}

		// Token: 0x06000D77 RID: 3447 RVA: 0x0002B35E File Offset: 0x0002A35E
		protected virtual void OnValueChanged()
		{
			this.SwitchSetting = int.Parse(this.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06000D78 RID: 3448 RVA: 0x0002B378 File Offset: 0x0002A378
		internal static void RefreshAll()
		{
			lock (Switch.switches)
			{
				for (int i = 0; i < Switch.switches.Count; i++)
				{
					Switch @switch = (Switch)Switch.switches[i].Target;
					if (@switch != null)
					{
						@switch.Refresh();
					}
				}
			}
		}

		// Token: 0x06000D79 RID: 3449 RVA: 0x0002B3E0 File Offset: 0x0002A3E0
		internal void Refresh()
		{
			this.initialized = false;
			this.switchSettings = null;
			this.Initialize();
		}

		// Token: 0x04000EC0 RID: 3776
		private SwitchElementsCollection switchSettings;

		// Token: 0x04000EC1 RID: 3777
		private string description;

		// Token: 0x04000EC2 RID: 3778
		private string displayName;

		// Token: 0x04000EC3 RID: 3779
		private int switchSetting;

		// Token: 0x04000EC4 RID: 3780
		private bool initialized;

		// Token: 0x04000EC5 RID: 3781
		private bool initializing;

		// Token: 0x04000EC6 RID: 3782
		private string switchValueString = string.Empty;

		// Token: 0x04000EC7 RID: 3783
		private StringDictionary attributes;

		// Token: 0x04000EC8 RID: 3784
		private string defaultValue;

		// Token: 0x04000EC9 RID: 3785
		private static List<WeakReference> switches = new List<WeakReference>();
	}
}
