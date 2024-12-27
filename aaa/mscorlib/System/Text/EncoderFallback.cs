using System;
using System.Threading;

namespace System.Text
{
	// Token: 0x020003EE RID: 1006
	[Serializable]
	public abstract class EncoderFallback
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x00083AA4 File Offset: 0x00082AA4
		private static object InternalSyncObject
		{
			get
			{
				if (EncoderFallback.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref EncoderFallback.s_InternalSyncObject, obj, null);
				}
				return EncoderFallback.s_InternalSyncObject;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060029EE RID: 10734 RVA: 0x00083AD0 File Offset: 0x00082AD0
		public static EncoderFallback ReplacementFallback
		{
			get
			{
				if (EncoderFallback.replacementFallback == null)
				{
					lock (EncoderFallback.InternalSyncObject)
					{
						if (EncoderFallback.replacementFallback == null)
						{
							EncoderFallback.replacementFallback = new EncoderReplacementFallback();
						}
					}
				}
				return EncoderFallback.replacementFallback;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x060029EF RID: 10735 RVA: 0x00083B20 File Offset: 0x00082B20
		public static EncoderFallback ExceptionFallback
		{
			get
			{
				if (EncoderFallback.exceptionFallback == null)
				{
					lock (EncoderFallback.InternalSyncObject)
					{
						if (EncoderFallback.exceptionFallback == null)
						{
							EncoderFallback.exceptionFallback = new EncoderExceptionFallback();
						}
					}
				}
				return EncoderFallback.exceptionFallback;
			}
		}

		// Token: 0x060029F0 RID: 10736
		public abstract EncoderFallbackBuffer CreateFallbackBuffer();

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x060029F1 RID: 10737
		public abstract int MaxCharCount { get; }

		// Token: 0x04001443 RID: 5187
		internal bool bIsMicrosoftBestFitFallback;

		// Token: 0x04001444 RID: 5188
		private static EncoderFallback replacementFallback;

		// Token: 0x04001445 RID: 5189
		private static EncoderFallback exceptionFallback;

		// Token: 0x04001446 RID: 5190
		private static object s_InternalSyncObject;
	}
}
