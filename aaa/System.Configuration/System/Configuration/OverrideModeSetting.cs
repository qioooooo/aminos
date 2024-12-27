using System;
using System.Globalization;

namespace System.Configuration
{
	// Token: 0x02000080 RID: 128
	internal struct OverrideModeSetting
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x00018ADC File Offset: 0x00017ADC
		static OverrideModeSetting()
		{
			OverrideModeSetting.SectionDefault._mode = 1;
			OverrideModeSetting.LocationDefault = default(OverrideModeSetting);
			OverrideModeSetting.LocationDefault._mode = 0;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00018B0C File Offset: 0x00017B0C
		internal static OverrideModeSetting CreateFromXmlReadValue(bool allowOverride)
		{
			OverrideModeSetting overrideModeSetting = default(OverrideModeSetting);
			overrideModeSetting.SetMode(allowOverride ? OverrideMode.Inherit : OverrideMode.Deny);
			overrideModeSetting._mode |= 64;
			return overrideModeSetting;
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00018B44 File Offset: 0x00017B44
		internal static OverrideModeSetting CreateFromXmlReadValue(OverrideMode mode)
		{
			OverrideModeSetting overrideModeSetting = default(OverrideModeSetting);
			overrideModeSetting.SetMode(mode);
			overrideModeSetting._mode |= 128;
			return overrideModeSetting;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x00018B78 File Offset: 0x00017B78
		internal static OverrideMode ParseOverrideModeXmlValue(string value, XmlUtil xmlUtil)
		{
			if (value != null)
			{
				if (value == "Inherit")
				{
					return OverrideMode.Inherit;
				}
				if (value == "Allow")
				{
					return OverrideMode.Allow;
				}
				if (value == "Deny")
				{
					return OverrideMode.Deny;
				}
			}
			throw new ConfigurationErrorsException(SR.GetString("Config_section_override_mode_attribute_invalid"), xmlUtil);
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x00018BCC File Offset: 0x00017BCC
		internal static bool CanUseSameLocationTag(OverrideModeSetting x, OverrideModeSetting y)
		{
			bool flag = x.OverrideMode == y.OverrideMode;
			if (flag)
			{
				if ((x._mode & 48) != 0)
				{
					flag = OverrideModeSetting.IsMatchingApiChangedLocationTag(x, y);
				}
				else if ((y._mode & 48) != 0)
				{
					flag = OverrideModeSetting.IsMatchingApiChangedLocationTag(y, x);
				}
				else
				{
					flag = ((x._mode & 192) == 0 && (y._mode & 192) == 0) || (x._mode & 192) == (y._mode & 192);
				}
			}
			return flag;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00018C60 File Offset: 0x00017C60
		private static bool IsMatchingApiChangedLocationTag(OverrideModeSetting x, OverrideModeSetting y)
		{
			bool flag = false;
			if ((y._mode & 48) != 0)
			{
				flag = (x._mode & 48) == (y._mode & 48);
			}
			else if ((y._mode & 192) != 0)
			{
				flag = ((x._mode & 16) != 0 && (y._mode & 64) != 0) || ((x._mode & 32) != 0 && (y._mode & 128) != 0);
			}
			return flag;
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x00018CE4 File Offset: 0x00017CE4
		internal bool IsDefaultForSection
		{
			get
			{
				OverrideMode overrideMode = this.OverrideMode;
				return overrideMode == OverrideMode.Allow || overrideMode == OverrideMode.Inherit;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060004CE RID: 1230 RVA: 0x00018D04 File Offset: 0x00017D04
		internal bool IsDefaultForLocationTag
		{
			get
			{
				OverrideModeSetting locationDefault = OverrideModeSetting.LocationDefault;
				return locationDefault.OverrideMode == this.OverrideMode && (this._mode & 48) == 0 && (this._mode & 192) == 0;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x00018D42 File Offset: 0x00017D42
		internal bool IsLocked
		{
			get
			{
				return this.OverrideMode == OverrideMode.Deny;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060004D0 RID: 1232 RVA: 0x00018D50 File Offset: 0x00017D50
		internal string LocationTagXmlString
		{
			get
			{
				string text = string.Empty;
				bool flag = false;
				bool flag2 = false;
				if ((this._mode & 48) != 0)
				{
					flag2 = (this._mode & 16) != 0;
					flag = true;
				}
				else if ((this._mode & 192) != 0)
				{
					flag2 = (this._mode & 64) != 0;
					flag = true;
				}
				if (flag)
				{
					string text2;
					string text3;
					if (flag2)
					{
						text2 = "allowOverride";
						text3 = (this.AllowOverride ? "true" : "false");
					}
					else
					{
						text2 = "overrideMode";
						text3 = this.OverrideModeXmlValue;
					}
					text = string.Format(CultureInfo.InvariantCulture, "{0}=\"{1}\"", new object[] { text2, text3 });
				}
				return text;
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00018E04 File Offset: 0x00017E04
		internal string OverrideModeXmlValue
		{
			get
			{
				switch (this.OverrideMode)
				{
				case OverrideMode.Inherit:
					return "Inherit";
				case OverrideMode.Allow:
					return "Allow";
				case OverrideMode.Deny:
					return "Deny";
				default:
					return null;
				}
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00018E3F File Offset: 0x00017E3F
		internal void ChangeModeInternal(OverrideMode mode)
		{
			this.SetMode(mode);
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060004D3 RID: 1235 RVA: 0x00018E48 File Offset: 0x00017E48
		// (set) Token: 0x060004D4 RID: 1236 RVA: 0x00018E53 File Offset: 0x00017E53
		internal OverrideMode OverrideMode
		{
			get
			{
				return (OverrideMode)(this._mode & 15);
			}
			set
			{
				this.VerifyConsistentChangeModel(32);
				this.SetMode(value);
				this._mode |= 32;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x00018E74 File Offset: 0x00017E74
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00018EA5 File Offset: 0x00017EA5
		internal bool AllowOverride
		{
			get
			{
				bool flag = true;
				switch (this.OverrideMode)
				{
				case OverrideMode.Inherit:
				case OverrideMode.Allow:
					flag = true;
					break;
				case OverrideMode.Deny:
					flag = false;
					break;
				}
				return flag;
			}
			set
			{
				this.VerifyConsistentChangeModel(16);
				this.SetMode(value ? OverrideMode.Inherit : OverrideMode.Deny);
				this._mode |= 16;
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00018ECC File Offset: 0x00017ECC
		private void SetMode(OverrideMode mode)
		{
			this._mode = (byte)mode;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00018ED8 File Offset: 0x00017ED8
		private void VerifyConsistentChangeModel(byte required)
		{
			byte b = this._mode & 48;
			if (b != 0 && b != required)
			{
				throw new ConfigurationErrorsException(SR.GetString("Cannot_change_both_AllowOverride_and_OverrideMode"));
			}
		}

		// Token: 0x04000354 RID: 852
		private const byte ApiDefinedLegacy = 16;

		// Token: 0x04000355 RID: 853
		private const byte ApiDefinedNewMode = 32;

		// Token: 0x04000356 RID: 854
		private const byte ApiDefinedAny = 48;

		// Token: 0x04000357 RID: 855
		private const byte XmlDefinedLegacy = 64;

		// Token: 0x04000358 RID: 856
		private const byte XmlDefinedNewMode = 128;

		// Token: 0x04000359 RID: 857
		private const byte XmlDefinedAny = 192;

		// Token: 0x0400035A RID: 858
		private const byte ModeMask = 15;

		// Token: 0x0400035B RID: 859
		private byte _mode;

		// Token: 0x0400035C RID: 860
		internal static OverrideModeSetting SectionDefault = default(OverrideModeSetting);

		// Token: 0x0400035D RID: 861
		internal static OverrideModeSetting LocationDefault;
	}
}
