using System;
using System.Text;

namespace Microsoft.JScript
{
	// Token: 0x0200004A RID: 74
	internal sealed class ConcatString : IConvertible
	{
		// Token: 0x0600038A RID: 906 RVA: 0x00016674 File Offset: 0x00015674
		internal ConcatString(string str1, string str2)
		{
			this.length = str1.Length + str2.Length;
			int num = this.length * 2;
			if (num < 256)
			{
				num = 256;
			}
			this.buf = new StringBuilder(str1, num);
			this.buf.Append(str2);
			this.isOwner = true;
		}

		// Token: 0x0600038B RID: 907 RVA: 0x000166D4 File Offset: 0x000156D4
		internal ConcatString(ConcatString str1, string str2)
		{
			this.length = str1.length + str2.Length;
			if (str1.isOwner)
			{
				this.buf = str1.buf;
				str1.isOwner = false;
			}
			else
			{
				int num = this.length * 2;
				if (num < 256)
				{
					num = 256;
				}
				this.buf = new StringBuilder(str1.ToString(), num);
			}
			this.buf.Append(str2);
			this.isOwner = true;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00016754 File Offset: 0x00015754
		TypeCode IConvertible.GetTypeCode()
		{
			return TypeCode.String;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00016758 File Offset: 0x00015758
		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return this.ToIConvertible().ToBoolean(provider);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00016766 File Offset: 0x00015766
		char IConvertible.ToChar(IFormatProvider provider)
		{
			return this.ToIConvertible().ToChar(provider);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00016774 File Offset: 0x00015774
		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return this.ToIConvertible().ToSByte(provider);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00016782 File Offset: 0x00015782
		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return this.ToIConvertible().ToByte(provider);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00016790 File Offset: 0x00015790
		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return this.ToIConvertible().ToInt16(provider);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x0001679E File Offset: 0x0001579E
		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return this.ToIConvertible().ToUInt16(provider);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000167AC File Offset: 0x000157AC
		private IConvertible ToIConvertible()
		{
			return this.ToString();
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000167B4 File Offset: 0x000157B4
		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return this.ToIConvertible().ToInt32(provider);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000167C2 File Offset: 0x000157C2
		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return this.ToIConvertible().ToUInt32(provider);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000167D0 File Offset: 0x000157D0
		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return this.ToIConvertible().ToInt64(provider);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000167DE File Offset: 0x000157DE
		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return this.ToIConvertible().ToUInt64(provider);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000167EC File Offset: 0x000157EC
		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return this.ToIConvertible().ToSingle(provider);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000167FA File Offset: 0x000157FA
		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return this.ToIConvertible().ToDouble(provider);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00016808 File Offset: 0x00015808
		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return this.ToIConvertible().ToDecimal(provider);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00016816 File Offset: 0x00015816
		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return this.ToIConvertible().ToDateTime(provider);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00016824 File Offset: 0x00015824
		string IConvertible.ToString(IFormatProvider provider)
		{
			return this.ToIConvertible().ToString(provider);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00016832 File Offset: 0x00015832
		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return this.ToIConvertible().ToType(conversionType, provider);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00016841 File Offset: 0x00015841
		public override string ToString()
		{
			return this.buf.ToString(0, this.length);
		}

		// Token: 0x040001CD RID: 461
		private StringBuilder buf;

		// Token: 0x040001CE RID: 462
		private bool isOwner;

		// Token: 0x040001CF RID: 463
		private int length;
	}
}
