using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000658 RID: 1624
	public sealed class ProxyElement : ConfigurationElement
	{
		// Token: 0x06003231 RID: 12849 RVA: 0x000D5CD8 File Offset: 0x000D4CD8
		public ProxyElement()
		{
			this.properties.Add(this.autoDetect);
			this.properties.Add(this.scriptLocation);
			this.properties.Add(this.bypassonlocal);
			this.properties.Add(this.proxyaddress);
			this.properties.Add(this.usesystemdefault);
		}

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x06003232 RID: 12850 RVA: 0x000D5E24 File Offset: 0x000D4E24
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x06003233 RID: 12851 RVA: 0x000D5E2C File Offset: 0x000D4E2C
		// (set) Token: 0x06003234 RID: 12852 RVA: 0x000D5E3F File Offset: 0x000D4E3F
		[ConfigurationProperty("autoDetect", DefaultValue = ProxyElement.AutoDetectValues.Unspecified)]
		public ProxyElement.AutoDetectValues AutoDetect
		{
			get
			{
				return (ProxyElement.AutoDetectValues)base[this.autoDetect];
			}
			set
			{
				base[this.autoDetect] = value;
			}
		}

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x06003235 RID: 12853 RVA: 0x000D5E53 File Offset: 0x000D4E53
		// (set) Token: 0x06003236 RID: 12854 RVA: 0x000D5E66 File Offset: 0x000D4E66
		[ConfigurationProperty("scriptLocation")]
		public Uri ScriptLocation
		{
			get
			{
				return (Uri)base[this.scriptLocation];
			}
			set
			{
				base[this.scriptLocation] = value;
			}
		}

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x06003237 RID: 12855 RVA: 0x000D5E75 File Offset: 0x000D4E75
		// (set) Token: 0x06003238 RID: 12856 RVA: 0x000D5E88 File Offset: 0x000D4E88
		[ConfigurationProperty("bypassonlocal", DefaultValue = ProxyElement.BypassOnLocalValues.Unspecified)]
		public ProxyElement.BypassOnLocalValues BypassOnLocal
		{
			get
			{
				return (ProxyElement.BypassOnLocalValues)base[this.bypassonlocal];
			}
			set
			{
				base[this.bypassonlocal] = value;
			}
		}

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x06003239 RID: 12857 RVA: 0x000D5E9C File Offset: 0x000D4E9C
		// (set) Token: 0x0600323A RID: 12858 RVA: 0x000D5EAF File Offset: 0x000D4EAF
		[ConfigurationProperty("proxyaddress")]
		public Uri ProxyAddress
		{
			get
			{
				return (Uri)base[this.proxyaddress];
			}
			set
			{
				base[this.proxyaddress] = value;
			}
		}

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x0600323B RID: 12859 RVA: 0x000D5EBE File Offset: 0x000D4EBE
		// (set) Token: 0x0600323C RID: 12860 RVA: 0x000D5ED1 File Offset: 0x000D4ED1
		[ConfigurationProperty("usesystemdefault", DefaultValue = ProxyElement.UseSystemDefaultValues.Unspecified)]
		public ProxyElement.UseSystemDefaultValues UseSystemDefault
		{
			get
			{
				return (ProxyElement.UseSystemDefaultValues)base[this.usesystemdefault];
			}
			set
			{
				base[this.usesystemdefault] = value;
			}
		}

		// Token: 0x04002F08 RID: 12040
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F09 RID: 12041
		private readonly ConfigurationProperty autoDetect = new ConfigurationProperty("autoDetect", typeof(ProxyElement.AutoDetectValues), ProxyElement.AutoDetectValues.Unspecified, new EnumConverter(typeof(ProxyElement.AutoDetectValues)), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F0A RID: 12042
		private readonly ConfigurationProperty scriptLocation = new ConfigurationProperty("scriptLocation", typeof(Uri), null, new UriTypeConverter(UriKind.Absolute), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F0B RID: 12043
		private readonly ConfigurationProperty bypassonlocal = new ConfigurationProperty("bypassonlocal", typeof(ProxyElement.BypassOnLocalValues), ProxyElement.BypassOnLocalValues.Unspecified, new EnumConverter(typeof(ProxyElement.BypassOnLocalValues)), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F0C RID: 12044
		private readonly ConfigurationProperty proxyaddress = new ConfigurationProperty("proxyaddress", typeof(Uri), null, new UriTypeConverter(UriKind.Absolute), null, ConfigurationPropertyOptions.None);

		// Token: 0x04002F0D RID: 12045
		private readonly ConfigurationProperty usesystemdefault = new ConfigurationProperty("usesystemdefault", typeof(ProxyElement.UseSystemDefaultValues), ProxyElement.UseSystemDefaultValues.Unspecified, new EnumConverter(typeof(ProxyElement.UseSystemDefaultValues)), null, ConfigurationPropertyOptions.None);

		// Token: 0x02000659 RID: 1625
		public enum BypassOnLocalValues
		{
			// Token: 0x04002F0F RID: 12047
			Unspecified = -1,
			// Token: 0x04002F10 RID: 12048
			False,
			// Token: 0x04002F11 RID: 12049
			True
		}

		// Token: 0x0200065A RID: 1626
		public enum UseSystemDefaultValues
		{
			// Token: 0x04002F13 RID: 12051
			Unspecified = -1,
			// Token: 0x04002F14 RID: 12052
			False,
			// Token: 0x04002F15 RID: 12053
			True
		}

		// Token: 0x0200065B RID: 1627
		public enum AutoDetectValues
		{
			// Token: 0x04002F17 RID: 12055
			Unspecified = -1,
			// Token: 0x04002F18 RID: 12056
			False,
			// Token: 0x04002F19 RID: 12057
			True
		}
	}
}
