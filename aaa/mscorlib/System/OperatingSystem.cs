using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System
{
	// Token: 0x020000E3 RID: 227
	[ComVisible(true)]
	[Serializable]
	public sealed class OperatingSystem : ICloneable, ISerializable
	{
		// Token: 0x06000C69 RID: 3177 RVA: 0x0002549C File Offset: 0x0002449C
		private OperatingSystem()
		{
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x000254A4 File Offset: 0x000244A4
		public OperatingSystem(PlatformID platform, Version version)
			: this(platform, version, null)
		{
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x000254B0 File Offset: 0x000244B0
		internal OperatingSystem(PlatformID platform, Version version, string servicePack)
		{
			if (platform < PlatformID.Win32S || platform > PlatformID.Unix)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[] { (int)platform }), "platform");
			}
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			this._platform = platform;
			this._version = (Version)version.Clone();
			this._servicePack = servicePack;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00025528 File Offset: 0x00024528
		private OperatingSystem(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				if ((name = enumerator.Name) != null)
				{
					if (!(name == "_version"))
					{
						if (!(name == "_platform"))
						{
							if (name == "_servicePack")
							{
								this._servicePack = info.GetString("_servicePack");
							}
						}
						else
						{
							this._platform = (PlatformID)info.GetValue("_platform", typeof(PlatformID));
						}
					}
					else
					{
						this._version = (Version)info.GetValue("_version", typeof(Version));
					}
				}
			}
			if (this._version == null)
			{
				throw new SerializationException(Environment.GetResourceString("Serialization_MissField", new object[] { "_version" }));
			}
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00025608 File Offset: 0x00024608
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("_version", this._version);
			info.AddValue("_platform", this._platform);
			info.AddValue("_servicePack", this._servicePack);
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x0002565B File Offset: 0x0002465B
		public PlatformID Platform
		{
			get
			{
				return this._platform;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00025663 File Offset: 0x00024663
		public string ServicePack
		{
			get
			{
				if (this._servicePack == null)
				{
					return string.Empty;
				}
				return this._servicePack;
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00025679 File Offset: 0x00024679
		public Version Version
		{
			get
			{
				return this._version;
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00025681 File Offset: 0x00024681
		public object Clone()
		{
			return new OperatingSystem(this._platform, this._version, this._servicePack);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0002569A File Offset: 0x0002469A
		public override string ToString()
		{
			return this.VersionString;
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x000256A4 File Offset: 0x000246A4
		public string VersionString
		{
			get
			{
				if (this._versionString != null)
				{
					return this._versionString;
				}
				string text;
				if (this._platform == PlatformID.Win32NT)
				{
					text = "Microsoft Windows NT ";
				}
				else if (this._platform == PlatformID.Win32Windows)
				{
					if (this._version.Major > 4 || (this._version.Major == 4 && this._version.Minor > 0))
					{
						text = "Microsoft Windows 98 ";
					}
					else
					{
						text = "Microsoft Windows 95 ";
					}
				}
				else if (this._platform == PlatformID.Win32S)
				{
					text = "Microsoft Win32S ";
				}
				else if (this._platform == PlatformID.WinCE)
				{
					text = "Microsoft Windows CE ";
				}
				else
				{
					text = "<unknown> ";
				}
				if (string.IsNullOrEmpty(this._servicePack))
				{
					this._versionString = text + this._version.ToString();
				}
				else
				{
					this._versionString = text + this._version.ToString(3) + " " + this._servicePack;
				}
				return this._versionString;
			}
		}

		// Token: 0x0400042D RID: 1069
		private Version _version;

		// Token: 0x0400042E RID: 1070
		private PlatformID _platform;

		// Token: 0x0400042F RID: 1071
		private string _servicePack;

		// Token: 0x04000430 RID: 1072
		private string _versionString;
	}
}
