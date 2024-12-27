using System;

namespace System.Text
{
	// Token: 0x020003F5 RID: 1013
	[Serializable]
	public sealed class EncoderReplacementFallback : EncoderFallback
	{
		// Token: 0x06002A23 RID: 10787 RVA: 0x000842D2 File Offset: 0x000832D2
		public EncoderReplacementFallback()
			: this("?")
		{
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x000842E0 File Offset: 0x000832E0
		public EncoderReplacementFallback(string replacement)
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

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x06002A25 RID: 10789 RVA: 0x00084365 File Offset: 0x00083365
		public string DefaultString
		{
			get
			{
				return this.strDefault;
			}
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x0008436D File Offset: 0x0008336D
		public override EncoderFallbackBuffer CreateFallbackBuffer()
		{
			return new EncoderReplacementFallbackBuffer(this);
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x06002A27 RID: 10791 RVA: 0x00084375 File Offset: 0x00083375
		public override int MaxCharCount
		{
			get
			{
				return this.strDefault.Length;
			}
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x00084384 File Offset: 0x00083384
		public override bool Equals(object value)
		{
			EncoderReplacementFallback encoderReplacementFallback = value as EncoderReplacementFallback;
			return encoderReplacementFallback != null && this.strDefault == encoderReplacementFallback.strDefault;
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x000843AE File Offset: 0x000833AE
		public override int GetHashCode()
		{
			return this.strDefault.GetHashCode();
		}

		// Token: 0x0400145A RID: 5210
		private string strDefault;
	}
}
