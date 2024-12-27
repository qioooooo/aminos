using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Security.Policy
{
	// Token: 0x020004A6 RID: 1190
	[ComVisible(true)]
	[Serializable]
	public sealed class Zone : IIdentityPermissionFactory, IBuiltInEvidence
	{
		// Token: 0x06003012 RID: 12306 RVA: 0x000A5DD4 File Offset: 0x000A4DD4
		internal Zone()
		{
			this.m_url = null;
			this.m_zone = SecurityZone.NoZone;
		}

		// Token: 0x06003013 RID: 12307 RVA: 0x000A5DEA File Offset: 0x000A4DEA
		public Zone(SecurityZone zone)
		{
			if (zone < SecurityZone.NoZone || zone > SecurityZone.Untrusted)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalZone"));
			}
			this.m_url = null;
			this.m_zone = zone;
		}

		// Token: 0x06003014 RID: 12308 RVA: 0x000A5E18 File Offset: 0x000A4E18
		private Zone(string url)
		{
			this.m_url = url;
			this.m_zone = SecurityZone.NoZone;
		}

		// Token: 0x06003015 RID: 12309 RVA: 0x000A5E2E File Offset: 0x000A4E2E
		public static Zone CreateFromUrl(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			return new Zone(url);
		}

		// Token: 0x06003016 RID: 12310
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern SecurityZone _CreateFromUrl(string url);

		// Token: 0x06003017 RID: 12311 RVA: 0x000A5E44 File Offset: 0x000A4E44
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new ZoneIdentityPermission(this.SecurityZone);
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06003018 RID: 12312 RVA: 0x000A5E51 File Offset: 0x000A4E51
		public SecurityZone SecurityZone
		{
			get
			{
				if (this.m_url != null)
				{
					this.m_zone = Zone._CreateFromUrl(this.m_url);
				}
				return this.m_zone;
			}
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x000A5E74 File Offset: 0x000A4E74
		public override bool Equals(object o)
		{
			if (o is Zone)
			{
				Zone zone = (Zone)o;
				return this.SecurityZone == zone.SecurityZone;
			}
			return false;
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x000A5EA0 File Offset: 0x000A4EA0
		public override int GetHashCode()
		{
			return (int)this.SecurityZone;
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x000A5EA8 File Offset: 0x000A4EA8
		public object Copy()
		{
			return new Zone
			{
				m_zone = this.m_zone,
				m_url = this.m_url
			};
		}

		// Token: 0x0600301C RID: 12316 RVA: 0x000A5ED4 File Offset: 0x000A4ED4
		internal SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Zone");
			securityElement.AddAttribute("version", "1");
			if (this.SecurityZone != SecurityZone.NoZone)
			{
				securityElement.AddChild(new SecurityElement("Zone", Zone.s_names[(int)this.SecurityZone]));
			}
			else
			{
				securityElement.AddChild(new SecurityElement("Zone", Zone.s_names[Zone.s_names.Length - 1]));
			}
			return securityElement;
		}

		// Token: 0x0600301D RID: 12317 RVA: 0x000A5F43 File Offset: 0x000A4F43
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position] = '\u0003';
			BuiltInEvidenceHelper.CopyIntToCharArray((int)this.SecurityZone, buffer, position + 1);
			return position + 3;
		}

		// Token: 0x0600301E RID: 12318 RVA: 0x000A5F5B File Offset: 0x000A4F5B
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return 3;
		}

		// Token: 0x0600301F RID: 12319 RVA: 0x000A5F5E File Offset: 0x000A4F5E
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			this.m_url = null;
			this.m_zone = (SecurityZone)BuiltInEvidenceHelper.GetIntFromCharArray(buffer, position);
			return position + 2;
		}

		// Token: 0x06003020 RID: 12320 RVA: 0x000A5F77 File Offset: 0x000A4F77
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06003021 RID: 12321 RVA: 0x000A5F84 File Offset: 0x000A4F84
		internal object Normalize()
		{
			return Zone.s_names[(int)this.SecurityZone];
		}

		// Token: 0x04001819 RID: 6169
		[OptionalField(VersionAdded = 2)]
		private string m_url;

		// Token: 0x0400181A RID: 6170
		private SecurityZone m_zone;

		// Token: 0x0400181B RID: 6171
		private static readonly string[] s_names = new string[] { "MyComputer", "Intranet", "Trusted", "Internet", "Untrusted", "NoZone" };
	}
}
