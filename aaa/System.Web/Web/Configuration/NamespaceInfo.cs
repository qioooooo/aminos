using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x02000216 RID: 534
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class NamespaceInfo : ConfigurationElement
	{
		// Token: 0x06001CAA RID: 7338 RVA: 0x000833AC File Offset: 0x000823AC
		static NamespaceInfo()
		{
			NamespaceInfo._properties.Add(NamespaceInfo._propNamespace);
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x000833E8 File Offset: 0x000823E8
		internal NamespaceInfo()
		{
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x000833F0 File Offset: 0x000823F0
		public NamespaceInfo(string name)
			: this()
		{
			this.Namespace = name;
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x00083400 File Offset: 0x00082400
		public override bool Equals(object namespaceInformation)
		{
			NamespaceInfo namespaceInfo = namespaceInformation as NamespaceInfo;
			return namespaceInfo != null && this.Namespace == namespaceInfo.Namespace;
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x0008342A File Offset: 0x0008242A
		public override int GetHashCode()
		{
			return this.Namespace.GetHashCode();
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06001CAF RID: 7343 RVA: 0x00083437 File Offset: 0x00082437
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return NamespaceInfo._properties;
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x0008343E File Offset: 0x0008243E
		// (set) Token: 0x06001CB1 RID: 7345 RVA: 0x00083450 File Offset: 0x00082450
		[ConfigurationProperty("namespace", IsRequired = true, IsKey = true, DefaultValue = "")]
		[StringValidator(MinLength = 1)]
		public string Namespace
		{
			get
			{
				return (string)base[NamespaceInfo._propNamespace];
			}
			set
			{
				base[NamespaceInfo._propNamespace] = value;
			}
		}

		// Token: 0x040018FD RID: 6397
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040018FE RID: 6398
		private static readonly ConfigurationProperty _propNamespace = new ConfigurationProperty("namespace", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);
	}
}
