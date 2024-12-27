using System;
using System.Configuration;
using System.Configuration.Internal;

namespace System.Web.Configuration
{
	// Token: 0x020001D5 RID: 469
	internal class ErrorRuntimeConfig : RuntimeConfig
	{
		// Token: 0x06001A4B RID: 6731 RVA: 0x0007B442 File Offset: 0x0007A442
		internal ErrorRuntimeConfig()
			: base(new ErrorRuntimeConfig.ErrorConfigRecord(), false)
		{
		}

		// Token: 0x06001A4C RID: 6732 RVA: 0x0007B450 File Offset: 0x0007A450
		protected override object GetSectionObject(string sectionName)
		{
			throw new ConfigurationErrorsException();
		}

		// Token: 0x020001D6 RID: 470
		private class ErrorConfigRecord : IInternalConfigRecord
		{
			// Token: 0x06001A4D RID: 6733 RVA: 0x0007B457 File Offset: 0x0007A457
			internal ErrorConfigRecord()
			{
			}

			// Token: 0x170004F7 RID: 1271
			// (get) Token: 0x06001A4E RID: 6734 RVA: 0x0007B45F File Offset: 0x0007A45F
			string IInternalConfigRecord.ConfigPath
			{
				get
				{
					throw new ConfigurationErrorsException();
				}
			}

			// Token: 0x170004F8 RID: 1272
			// (get) Token: 0x06001A4F RID: 6735 RVA: 0x0007B466 File Offset: 0x0007A466
			string IInternalConfigRecord.StreamName
			{
				get
				{
					throw new ConfigurationErrorsException();
				}
			}

			// Token: 0x170004F9 RID: 1273
			// (get) Token: 0x06001A50 RID: 6736 RVA: 0x0007B46D File Offset: 0x0007A46D
			bool IInternalConfigRecord.HasInitErrors
			{
				get
				{
					return true;
				}
			}

			// Token: 0x06001A51 RID: 6737 RVA: 0x0007B470 File Offset: 0x0007A470
			void IInternalConfigRecord.ThrowIfInitErrors()
			{
				throw new ConfigurationErrorsException();
			}

			// Token: 0x06001A52 RID: 6738 RVA: 0x0007B477 File Offset: 0x0007A477
			object IInternalConfigRecord.GetSection(string configKey)
			{
				throw new ConfigurationErrorsException();
			}

			// Token: 0x06001A53 RID: 6739 RVA: 0x0007B47E File Offset: 0x0007A47E
			object IInternalConfigRecord.GetLkgSection(string configKey)
			{
				throw new ConfigurationErrorsException();
			}

			// Token: 0x06001A54 RID: 6740 RVA: 0x0007B485 File Offset: 0x0007A485
			void IInternalConfigRecord.RefreshSection(string configKey)
			{
				throw new ConfigurationErrorsException();
			}

			// Token: 0x06001A55 RID: 6741 RVA: 0x0007B48C File Offset: 0x0007A48C
			void IInternalConfigRecord.Remove()
			{
				throw new ConfigurationErrorsException();
			}
		}
	}
}
