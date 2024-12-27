using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000663 RID: 1635
	internal sealed class SmtpSectionInternal
	{
		// Token: 0x06003295 RID: 12949 RVA: 0x000D6D50 File Offset: 0x000D5D50
		internal SmtpSectionInternal(SmtpSection section)
		{
			this.deliveryMethod = section.DeliveryMethod;
			this.from = section.From;
			this.network = new SmtpNetworkElementInternal(section.Network);
			this.specifiedPickupDirectory = new SmtpSpecifiedPickupDirectoryElementInternal(section.SpecifiedPickupDirectory);
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06003296 RID: 12950 RVA: 0x000D6D9D File Offset: 0x000D5D9D
		internal SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return this.deliveryMethod;
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x000D6DA5 File Offset: 0x000D5DA5
		internal SmtpNetworkElementInternal Network
		{
			get
			{
				return this.network;
			}
		}

		// Token: 0x17000BD4 RID: 3028
		// (get) Token: 0x06003298 RID: 12952 RVA: 0x000D6DAD File Offset: 0x000D5DAD
		internal string From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x000D6DB5 File Offset: 0x000D5DB5
		internal SmtpSpecifiedPickupDirectoryElementInternal SpecifiedPickupDirectory
		{
			get
			{
				return this.specifiedPickupDirectory;
			}
		}

		// Token: 0x17000BD6 RID: 3030
		// (get) Token: 0x0600329A RID: 12954 RVA: 0x000D6DBD File Offset: 0x000D5DBD
		internal static object ClassSyncObject
		{
			get
			{
				if (SmtpSectionInternal.classSyncObject == null)
				{
					Interlocked.CompareExchange(ref SmtpSectionInternal.classSyncObject, new object(), null);
				}
				return SmtpSectionInternal.classSyncObject;
			}
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x000D6DDC File Offset: 0x000D5DDC
		internal static SmtpSectionInternal GetSection()
		{
			SmtpSectionInternal smtpSectionInternal;
			lock (SmtpSectionInternal.ClassSyncObject)
			{
				SmtpSection smtpSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.SmtpSectionPath) as SmtpSection;
				if (smtpSection == null)
				{
					smtpSectionInternal = null;
				}
				else
				{
					smtpSectionInternal = new SmtpSectionInternal(smtpSection);
				}
			}
			return smtpSectionInternal;
		}

		// Token: 0x04002F4F RID: 12111
		private SmtpDeliveryMethod deliveryMethod;

		// Token: 0x04002F50 RID: 12112
		private string from;

		// Token: 0x04002F51 RID: 12113
		private SmtpNetworkElementInternal network;

		// Token: 0x04002F52 RID: 12114
		private SmtpSpecifiedPickupDirectoryElementInternal specifiedPickupDirectory;

		// Token: 0x04002F53 RID: 12115
		private static object classSyncObject;
	}
}
