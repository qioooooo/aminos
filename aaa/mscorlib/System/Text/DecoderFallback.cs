using System;
using System.Threading;

namespace System.Text
{
	// Token: 0x020003E4 RID: 996
	[Serializable]
	public abstract class DecoderFallback
	{
		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x0600299E RID: 10654 RVA: 0x00082C04 File Offset: 0x00081C04
		private static object InternalSyncObject
		{
			get
			{
				if (DecoderFallback.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref DecoderFallback.s_InternalSyncObject, obj, null);
				}
				return DecoderFallback.s_InternalSyncObject;
			}
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x0600299F RID: 10655 RVA: 0x00082C30 File Offset: 0x00081C30
		public static DecoderFallback ReplacementFallback
		{
			get
			{
				if (DecoderFallback.replacementFallback == null)
				{
					lock (DecoderFallback.InternalSyncObject)
					{
						if (DecoderFallback.replacementFallback == null)
						{
							DecoderFallback.replacementFallback = new DecoderReplacementFallback();
						}
					}
				}
				return DecoderFallback.replacementFallback;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060029A0 RID: 10656 RVA: 0x00082C80 File Offset: 0x00081C80
		public static DecoderFallback ExceptionFallback
		{
			get
			{
				if (DecoderFallback.exceptionFallback == null)
				{
					lock (DecoderFallback.InternalSyncObject)
					{
						if (DecoderFallback.exceptionFallback == null)
						{
							DecoderFallback.exceptionFallback = new DecoderExceptionFallback();
						}
					}
				}
				return DecoderFallback.exceptionFallback;
			}
		}

		// Token: 0x060029A1 RID: 10657
		public abstract DecoderFallbackBuffer CreateFallbackBuffer();

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060029A2 RID: 10658
		public abstract int MaxCharCount { get; }

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060029A3 RID: 10659 RVA: 0x00082CD0 File Offset: 0x00081CD0
		internal bool IsMicrosoftBestFitFallback
		{
			get
			{
				return this.bIsMicrosoftBestFitFallback;
			}
		}

		// Token: 0x0400142A RID: 5162
		internal bool bIsMicrosoftBestFitFallback;

		// Token: 0x0400142B RID: 5163
		private static DecoderFallback replacementFallback;

		// Token: 0x0400142C RID: 5164
		private static DecoderFallback exceptionFallback;

		// Token: 0x0400142D RID: 5165
		private static object s_InternalSyncObject;
	}
}
