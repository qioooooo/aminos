using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Util;

namespace System.Security.Policy
{
	// Token: 0x020004A7 RID: 1191
	[ComVisible(true)]
	[Serializable]
	public sealed class ZoneMembershipCondition : IConstantMembershipCondition, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x06003023 RID: 12323 RVA: 0x000A5FDE File Offset: 0x000A4FDE
		internal ZoneMembershipCondition()
		{
			this.m_zone = SecurityZone.NoZone;
		}

		// Token: 0x06003024 RID: 12324 RVA: 0x000A5FED File Offset: 0x000A4FED
		public ZoneMembershipCondition(SecurityZone zone)
		{
			ZoneMembershipCondition.VerifyZone(zone);
			this.SecurityZone = zone;
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x000A6011 File Offset: 0x000A5011
		// (set) Token: 0x06003025 RID: 12325 RVA: 0x000A6002 File Offset: 0x000A5002
		public SecurityZone SecurityZone
		{
			get
			{
				if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
				{
					this.ParseZone();
				}
				return this.m_zone;
			}
			set
			{
				ZoneMembershipCondition.VerifyZone(value);
				this.m_zone = value;
			}
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x000A6030 File Offset: 0x000A5030
		private static void VerifyZone(SecurityZone zone)
		{
			if (zone < SecurityZone.MyComputer || zone > SecurityZone.Untrusted)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_IllegalZone"));
			}
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x000A604C File Offset: 0x000A504C
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000A6064 File Offset: 0x000A5064
		bool IReportMatchMembershipCondition.Check(Evidence evidence, out object usedEvidence)
		{
			usedEvidence = null;
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				Zone zone = obj as Zone;
				if (zone != null)
				{
					if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
					{
						this.ParseZone();
					}
					if (zone.SecurityZone == this.m_zone)
					{
						usedEvidence = zone;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x000A60C4 File Offset: 0x000A50C4
		public IMembershipCondition Copy()
		{
			if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
			{
				this.ParseZone();
			}
			return new ZoneMembershipCondition(this.m_zone);
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x000A60E8 File Offset: 0x000A50E8
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x000A60F1 File Offset: 0x000A50F1
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x000A60FC File Offset: 0x000A50FC
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
			{
				this.ParseZone();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.ZoneMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_zone != SecurityZone.NoZone)
			{
				securityElement.AddAttribute("Zone", Enum.GetName(typeof(SecurityZone), this.m_zone));
			}
			return securityElement;
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x000A617C File Offset: 0x000A517C
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (!e.Tag.Equals("IMembershipCondition"))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MembershipConditionElement"));
			}
			lock (this)
			{
				this.m_zone = SecurityZone.NoZone;
				this.m_element = e;
			}
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x000A61E8 File Offset: 0x000A51E8
		private void ParseZone()
		{
			lock (this)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("Zone");
					this.m_zone = SecurityZone.NoZone;
					if (text == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ZoneCannotBeNull"));
					}
					this.m_zone = (SecurityZone)Enum.Parse(typeof(SecurityZone), text);
					ZoneMembershipCondition.VerifyZone(this.m_zone);
					this.m_element = null;
				}
			}
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x000A627C File Offset: 0x000A527C
		public override bool Equals(object o)
		{
			ZoneMembershipCondition zoneMembershipCondition = o as ZoneMembershipCondition;
			if (zoneMembershipCondition != null)
			{
				if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
				{
					this.ParseZone();
				}
				if (zoneMembershipCondition.m_zone == SecurityZone.NoZone && zoneMembershipCondition.m_element != null)
				{
					zoneMembershipCondition.ParseZone();
				}
				if (this.m_zone == zoneMembershipCondition.m_zone)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x000A62D2 File Offset: 0x000A52D2
		public override int GetHashCode()
		{
			if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
			{
				this.ParseZone();
			}
			return (int)this.m_zone;
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x000A62F4 File Offset: 0x000A52F4
		public override string ToString()
		{
			if (this.m_zone == SecurityZone.NoZone && this.m_element != null)
			{
				this.ParseZone();
			}
			return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Zone_ToString"), new object[] { ZoneMembershipCondition.s_names[(int)this.m_zone] });
		}

		// Token: 0x0400181C RID: 6172
		private static readonly string[] s_names = new string[] { "MyComputer", "Intranet", "Trusted", "Internet", "Untrusted" };

		// Token: 0x0400181D RID: 6173
		private SecurityZone m_zone;

		// Token: 0x0400181E RID: 6174
		private SecurityElement m_element;
	}
}
