using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000092 RID: 146
	[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
	internal class DirectoryEntryManager
	{
		// Token: 0x0600049B RID: 1179 RVA: 0x00019E7C File Offset: 0x00018E7C
		internal DirectoryEntryManager(DirectoryContext context)
		{
			this.context = context;
			this.pathCracker = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
			this.pathCracker.EscapedMode = 2;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00019EB2 File Offset: 0x00018EB2
		internal ICollection GetCachedDirectoryEntries()
		{
			return this.directoryEntries.Values;
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00019EBF File Offset: 0x00018EBF
		internal DirectoryEntry GetCachedDirectoryEntry(WellKnownDN dn)
		{
			return this.GetCachedDirectoryEntry(this.ExpandWellKnownDN(dn));
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00019ED0 File Offset: 0x00018ED0
		internal DirectoryEntry GetCachedDirectoryEntry(string distinguishedName)
		{
			object obj = distinguishedName;
			if (string.Compare(distinguishedName, "rootdse", StringComparison.OrdinalIgnoreCase) != 0 && string.Compare(distinguishedName, "schema", StringComparison.OrdinalIgnoreCase) != 0)
			{
				obj = new DistinguishedName(distinguishedName);
			}
			if (!this.directoryEntries.ContainsKey(obj))
			{
				DirectoryEntry newDirectoryEntry = this.GetNewDirectoryEntry(distinguishedName);
				this.directoryEntries.Add(obj, newDirectoryEntry);
			}
			return (DirectoryEntry)this.directoryEntries[obj];
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00019F38 File Offset: 0x00018F38
		internal void RemoveIfExists(string distinguishedName)
		{
			object obj = distinguishedName;
			if (string.Compare(distinguishedName, "rootdse", StringComparison.OrdinalIgnoreCase) != 0)
			{
				obj = new DistinguishedName(distinguishedName);
			}
			if (this.directoryEntries.ContainsKey(obj))
			{
				DirectoryEntry directoryEntry = (DirectoryEntry)this.directoryEntries[obj];
				if (directoryEntry != null)
				{
					this.directoryEntries.Remove(obj);
					directoryEntry.Dispose();
				}
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00019F94 File Offset: 0x00018F94
		private DirectoryEntry GetNewDirectoryEntry(string dn)
		{
			if (this.bindingPrefix == null)
			{
				this.bindingPrefix = "LDAP://" + this.context.GetServerName() + "/";
			}
			this.pathCracker.Set(dn, 4);
			string text = this.pathCracker.Retrieve(7);
			return DirectoryEntryManager.Bind(this.bindingPrefix + text, this.context.UserName, this.context.Password, this.context.useServerBind());
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001A018 File Offset: 0x00019018
		internal string ExpandWellKnownDN(WellKnownDN dn)
		{
			string text;
			switch (dn)
			{
			case WellKnownDN.RootDSE:
				text = "RootDSE";
				break;
			case WellKnownDN.DefaultNamingContext:
			{
				DirectoryEntry cachedDirectoryEntry = this.GetCachedDirectoryEntry("RootDSE");
				text = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.DefaultNamingContext);
				break;
			}
			case WellKnownDN.SchemaNamingContext:
			{
				DirectoryEntry cachedDirectoryEntry2 = this.GetCachedDirectoryEntry("RootDSE");
				text = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry2, PropertyManager.SchemaNamingContext);
				break;
			}
			case WellKnownDN.ConfigurationNamingContext:
			{
				DirectoryEntry cachedDirectoryEntry3 = this.GetCachedDirectoryEntry("RootDSE");
				text = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry3, PropertyManager.ConfigurationNamingContext);
				break;
			}
			case WellKnownDN.PartitionsContainer:
				text = "CN=Partitions," + this.ExpandWellKnownDN(WellKnownDN.ConfigurationNamingContext);
				break;
			case WellKnownDN.SitesContainer:
				text = "CN=Sites," + this.ExpandWellKnownDN(WellKnownDN.ConfigurationNamingContext);
				break;
			case WellKnownDN.SystemContainer:
				text = "CN=System," + this.ExpandWellKnownDN(WellKnownDN.DefaultNamingContext);
				break;
			case WellKnownDN.RidManager:
				text = "CN=RID Manager$," + this.ExpandWellKnownDN(WellKnownDN.SystemContainer);
				break;
			case WellKnownDN.Infrastructure:
				text = "CN=Infrastructure," + this.ExpandWellKnownDN(WellKnownDN.DefaultNamingContext);
				break;
			case WellKnownDN.RootDomainNamingContext:
			{
				DirectoryEntry cachedDirectoryEntry4 = this.GetCachedDirectoryEntry("RootDSE");
				text = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry4, PropertyManager.RootDomainNamingContext);
				break;
			}
			default:
				throw new InvalidEnumArgumentException("dn", (int)dn, typeof(WellKnownDN));
			}
			return text;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001A183 File Offset: 0x00019183
		internal static DirectoryEntry GetDirectoryEntry(DirectoryContext context, WellKnownDN dn)
		{
			return DirectoryEntryManager.GetDirectoryEntry(context, DirectoryEntryManager.ExpandWellKnownDN(context, dn));
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001A194 File Offset: 0x00019194
		internal static DirectoryEntry GetDirectoryEntry(DirectoryContext context, string dn)
		{
			string text = "LDAP://" + context.GetServerName() + "/";
			NativeComInterfaces.IAdsPathname adsPathname = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
			adsPathname.EscapedMode = 2;
			adsPathname.Set(dn, 4);
			string text2 = adsPathname.Retrieve(7);
			return DirectoryEntryManager.Bind(text + text2, context.UserName, context.Password, context.useServerBind());
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001A1F8 File Offset: 0x000191F8
		internal static DirectoryEntry GetDirectoryEntryInternal(DirectoryContext context, string path)
		{
			return DirectoryEntryManager.Bind(path, context.UserName, context.Password, context.useServerBind());
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001A214 File Offset: 0x00019214
		internal static DirectoryEntry Bind(string ldapPath, string username, string password, bool useServerBind)
		{
			AuthenticationTypes authenticationTypes = Utils.DefaultAuthType;
			if (DirectoryContext.ServerBindSupported && useServerBind)
			{
				authenticationTypes |= AuthenticationTypes.ServerBind;
			}
			return new DirectoryEntry(ldapPath, username, password, authenticationTypes);
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x0001A248 File Offset: 0x00019248
		internal static string ExpandWellKnownDN(DirectoryContext context, WellKnownDN dn)
		{
			string text = null;
			switch (dn)
			{
			case WellKnownDN.RootDSE:
				return "RootDSE";
			case WellKnownDN.DefaultNamingContext:
				break;
			case WellKnownDN.SchemaNamingContext:
				goto IL_0098;
			case WellKnownDN.ConfigurationNamingContext:
				goto IL_00C2;
			case WellKnownDN.PartitionsContainer:
				goto IL_00EF;
			case WellKnownDN.SitesContainer:
				return "CN=Sites," + DirectoryEntryManager.ExpandWellKnownDN(context, WellKnownDN.ConfigurationNamingContext);
			case WellKnownDN.SystemContainer:
				return "CN=System," + DirectoryEntryManager.ExpandWellKnownDN(context, WellKnownDN.DefaultNamingContext);
			case WellKnownDN.RidManager:
				return "CN=RID Manager$," + DirectoryEntryManager.ExpandWellKnownDN(context, WellKnownDN.SystemContainer);
			case WellKnownDN.Infrastructure:
				return "CN=Infrastructure," + DirectoryEntryManager.ExpandWellKnownDN(context, WellKnownDN.DefaultNamingContext);
			case WellKnownDN.RootDomainNamingContext:
			{
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, "RootDSE");
				try
				{
					return (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.RootDomainNamingContext);
				}
				finally
				{
					directoryEntry.Dispose();
				}
				break;
			}
			default:
				throw new InvalidEnumArgumentException("dn", (int)dn, typeof(WellKnownDN));
			}
			DirectoryEntry directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(context, "RootDSE");
			try
			{
				return (string)PropertyManager.GetPropertyValue(context, directoryEntry2, PropertyManager.DefaultNamingContext);
			}
			finally
			{
				directoryEntry2.Dispose();
			}
			IL_0098:
			DirectoryEntry directoryEntry3 = DirectoryEntryManager.GetDirectoryEntry(context, "RootDSE");
			try
			{
				return (string)PropertyManager.GetPropertyValue(context, directoryEntry3, PropertyManager.SchemaNamingContext);
			}
			finally
			{
				directoryEntry3.Dispose();
			}
			IL_00C2:
			DirectoryEntry directoryEntry4 = DirectoryEntryManager.GetDirectoryEntry(context, "RootDSE");
			try
			{
				return (string)PropertyManager.GetPropertyValue(context, directoryEntry4, PropertyManager.ConfigurationNamingContext);
			}
			finally
			{
				directoryEntry4.Dispose();
			}
			IL_00EF:
			text = "CN=Partitions," + DirectoryEntryManager.ExpandWellKnownDN(context, WellKnownDN.ConfigurationNamingContext);
			return text;
		}

		// Token: 0x040003F4 RID: 1012
		private Hashtable directoryEntries = new Hashtable();

		// Token: 0x040003F5 RID: 1013
		private string bindingPrefix;

		// Token: 0x040003F6 RID: 1014
		private DirectoryContext context;

		// Token: 0x040003F7 RID: 1015
		private NativeComInterfaces.IAdsPathname pathCracker;
	}
}
