using System;

namespace System.Text
{
	// Token: 0x020003EB RID: 1003
	[Serializable]
	public sealed class DecoderReplacementFallback : DecoderFallback
	{
		// Token: 0x060029D0 RID: 10704 RVA: 0x00083316 File Offset: 0x00082316
		public DecoderReplacementFallback()
			: this("?")
		{
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x00083324 File Offset: 0x00082324
		public DecoderReplacementFallback(string replacement)
		{
			if (replacement == null)
			{
				throw new ArgumentNullException("replacement");
			}
			bool flag = false;
			for (int i = 0; i < replacement.Length; i++)
			{
				if (char.IsSurrogate(replacement, i))
				{
					if (char.IsHighSurrogate(replacement, i))
					{
						if (flag)
						{
							break;
						}
						flag = true;
					}
					else
					{
						if (!flag)
						{
							flag = true;
							break;
						}
						flag = false;
					}
				}
				else if (flag)
				{
					break;
				}
			}
			if (flag)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex", new object[] { "replacement" }));
			}
			this.strDefault = replacement;
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060029D2 RID: 10706 RVA: 0x000833A9 File Offset: 0x000823A9
		public string DefaultString
		{
			get
			{
				return this.strDefault;
			}
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x000833B1 File Offset: 0x000823B1
		public override DecoderFallbackBuffer CreateFallbackBuffer()
		{
			return new DecoderReplacementFallbackBuffer(this);
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060029D4 RID: 10708 RVA: 0x000833B9 File Offset: 0x000823B9
		public override int MaxCharCount
		{
			get
			{
				return this.strDefault.Length;
			}
		}

		// Token: 0x060029D5 RID: 10709 RVA: 0x000833C8 File Offset: 0x000823C8
		public override bool Equals(object value)
		{
			DecoderReplacementFallback decoderReplacementFallback = value as DecoderReplacementFallback;
			return decoderReplacementFallback != null && this.strDefault == decoderReplacementFallback.strDefault;
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x000833F2 File Offset: 0x000823F2
		public override int GetHashCode()
		{
			return this.strDefault.GetHashCode();
		}

		// Token: 0x0400143A RID: 5178
		private string strDefault;
	}
}
