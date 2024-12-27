using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200025A RID: 602
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TrustLevel : ConfigurationElement
	{
		// Token: 0x06001FCC RID: 8140 RVA: 0x0008C06C File Offset: 0x0008B06C
		static TrustLevel()
		{
			TrustLevel._properties.Add(TrustLevel._propName);
			TrustLevel._properties.Add(TrustLevel._propPolicyFile);
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x0008C0E5 File Offset: 0x0008B0E5
		internal TrustLevel()
		{
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x0008C0ED File Offset: 0x0008B0ED
		public TrustLevel(string name, string policyFile)
		{
			this.Name = name;
			this.PolicyFile = policyFile;
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x0008C103 File Offset: 0x0008B103
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TrustLevel._properties;
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x0008C10A File Offset: 0x0008B10A
		// (set) Token: 0x06001FD1 RID: 8145 RVA: 0x0008C11C File Offset: 0x0008B11C
		[ConfigurationProperty("name", IsRequired = true, DefaultValue = "Full", IsKey = true)]
		[StringValidator(MinLength = 1)]
		public string Name
		{
			get
			{
				return (string)base[TrustLevel._propName];
			}
			set
			{
				base[TrustLevel._propName] = value;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x0008C12A File Offset: 0x0008B12A
		// (set) Token: 0x06001FD3 RID: 8147 RVA: 0x0008C13C File Offset: 0x0008B13C
		[ConfigurationProperty("policyFile", IsRequired = true, DefaultValue = "internal")]
		public string PolicyFile
		{
			get
			{
				return (string)base[TrustLevel._propPolicyFile];
			}
			set
			{
				base[TrustLevel._propPolicyFile] = value;
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x0008C14C File Offset: 0x0008B14C
		internal string PolicyFileExpanded
		{
			get
			{
				if (this._PolicyFileExpanded == null)
				{
					string source = base.ElementInformation.Properties["policyFile"].Source;
					string text = source.Substring(0, source.LastIndexOf('\\') + 1);
					bool flag = true;
					if (this.PolicyFile.Length > 1)
					{
						char c = this.PolicyFile[1];
						char c2 = this.PolicyFile[0];
						if (c == ':')
						{
							flag = false;
						}
						else if (c2 == '\\' && c == '\\')
						{
							flag = false;
						}
					}
					if (flag)
					{
						this._PolicyFileExpanded = text + this.PolicyFile;
					}
					else
					{
						this._PolicyFileExpanded = this.PolicyFile;
					}
				}
				return this._PolicyFileExpanded;
			}
		}

		// Token: 0x04001A6F RID: 6767
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001A70 RID: 6768
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), "Full", null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001A71 RID: 6769
		private static readonly ConfigurationProperty _propPolicyFile = new ConfigurationProperty("policyFile", typeof(string), "internal", ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04001A72 RID: 6770
		private string _PolicyFileExpanded;
	}
}
