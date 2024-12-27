using System;
using System.Text;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x0200034A RID: 842
	public class ExtendedProtectionPolicy
	{
		// Token: 0x06001A79 RID: 6777 RVA: 0x0005C6DC File Offset: 0x0005B6DC
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ProtectionScenario protectionScenario, ServiceNameCollection customServiceNames)
		{
			if (policyEnforcement == PolicyEnforcement.Never)
			{
				throw new ArgumentException(SR.GetString("security_ExtendedProtectionPolicy_UseDifferentConstructorForNever"), "policyEnforcement");
			}
			if (customServiceNames != null && customServiceNames.Count == 0)
			{
				throw new ArgumentException(SR.GetString("security_ExtendedProtectionPolicy_NoEmptyServiceNameCollection"), "customServiceNames");
			}
			this.policyEnforcement = policyEnforcement;
			this.protectionScenario = protectionScenario;
			this.customServiceNames = customServiceNames;
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x0005C73C File Offset: 0x0005B73C
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement, ChannelBinding customChannelBinding)
		{
			if (policyEnforcement == PolicyEnforcement.Never)
			{
				throw new ArgumentException(SR.GetString("security_ExtendedProtectionPolicy_UseDifferentConstructorForNever"), "policyEnforcement");
			}
			if (customChannelBinding == null)
			{
				throw new ArgumentNullException("customChannelBinding");
			}
			this.policyEnforcement = policyEnforcement;
			this.protectionScenario = ProtectionScenario.TransportSelected;
			this.customChannelBinding = customChannelBinding;
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x0005C78A File Offset: 0x0005B78A
		public ExtendedProtectionPolicy(PolicyEnforcement policyEnforcement)
		{
			this.policyEnforcement = policyEnforcement;
			this.protectionScenario = ProtectionScenario.TransportSelected;
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001A7C RID: 6780 RVA: 0x0005C7A0 File Offset: 0x0005B7A0
		public ServiceNameCollection CustomServiceNames
		{
			get
			{
				return this.customServiceNames;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001A7D RID: 6781 RVA: 0x0005C7A8 File Offset: 0x0005B7A8
		public PolicyEnforcement PolicyEnforcement
		{
			get
			{
				return this.policyEnforcement;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001A7E RID: 6782 RVA: 0x0005C7B0 File Offset: 0x0005B7B0
		public ProtectionScenario ProtectionScenario
		{
			get
			{
				return this.protectionScenario;
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001A7F RID: 6783 RVA: 0x0005C7B8 File Offset: 0x0005B7B8
		public ChannelBinding CustomChannelBinding
		{
			get
			{
				return this.customChannelBinding;
			}
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x0005C7C0 File Offset: 0x0005B7C0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ProtectionScenario=");
			stringBuilder.Append(this.protectionScenario.ToString());
			stringBuilder.Append("; PolicyEnforcement=");
			stringBuilder.Append(this.policyEnforcement.ToString());
			stringBuilder.Append("; CustomChannelBinding=");
			if (this.customChannelBinding == null)
			{
				stringBuilder.Append("<null>");
			}
			else
			{
				stringBuilder.Append(this.customChannelBinding.ToString());
			}
			stringBuilder.Append("; ServiceNames=");
			if (this.customServiceNames == null)
			{
				stringBuilder.Append("<null>");
			}
			else
			{
				bool flag = true;
				foreach (object obj in this.customServiceNames)
				{
					string text = (string)obj;
					if (flag)
					{
						flag = false;
					}
					else
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04001B3E RID: 6974
		private ServiceNameCollection customServiceNames;

		// Token: 0x04001B3F RID: 6975
		private PolicyEnforcement policyEnforcement;

		// Token: 0x04001B40 RID: 6976
		private ProtectionScenario protectionScenario;

		// Token: 0x04001B41 RID: 6977
		private ChannelBinding customChannelBinding;
	}
}
