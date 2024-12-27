using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200076D RID: 1901
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapHexBinary : ISoapXsd
	{
		// Token: 0x17000C00 RID: 3072
		// (get) Token: 0x06004436 RID: 17462 RVA: 0x000EA66E File Offset: 0x000E966E
		public static string XsdType
		{
			get
			{
				return "hexBinary";
			}
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x000EA675 File Offset: 0x000E9675
		public string GetXsdType()
		{
			return SoapHexBinary.XsdType;
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x000EA67C File Offset: 0x000E967C
		public SoapHexBinary()
		{
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x000EA691 File Offset: 0x000E9691
		public SoapHexBinary(byte[] value)
		{
			this._value = value;
		}

		// Token: 0x17000C01 RID: 3073
		// (get) Token: 0x0600443A RID: 17466 RVA: 0x000EA6AD File Offset: 0x000E96AD
		// (set) Token: 0x0600443B RID: 17467 RVA: 0x000EA6B5 File Offset: 0x000E96B5
		public byte[] Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x000EA6C0 File Offset: 0x000E96C0
		public override string ToString()
		{
			this.sb.Length = 0;
			for (int i = 0; i < this._value.Length; i++)
			{
				string text = this._value[i].ToString("X", CultureInfo.InvariantCulture);
				if (text.Length == 1)
				{
					this.sb.Append('0');
				}
				this.sb.Append(text);
			}
			return this.sb.ToString();
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x000EA737 File Offset: 0x000E9737
		public static SoapHexBinary Parse(string value)
		{
			return new SoapHexBinary(SoapHexBinary.ToByteArray(SoapType.FilterBin64(value)));
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x000EA74C File Offset: 0x000E974C
		private static byte[] ToByteArray(string value)
		{
			char[] array = value.ToCharArray();
			if (array.Length % 2 != 0)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:hexBinary", value }));
			}
			byte[] array2 = new byte[array.Length / 2];
			for (int i = 0; i < array.Length / 2; i++)
			{
				array2[i] = SoapHexBinary.ToByte(array[i * 2], value) * 16 + SoapHexBinary.ToByte(array[i * 2 + 1], value);
			}
			return array2;
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x000EA7D4 File Offset: 0x000E97D4
		private static byte ToByte(char c, string value)
		{
			byte b = 0;
			string text = c.ToString();
			try
			{
				text = c.ToString();
				b = byte.Parse(text, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "xsd:hexBinary", value }));
			}
			return b;
		}

		// Token: 0x04002202 RID: 8706
		private byte[] _value;

		// Token: 0x04002203 RID: 8707
		private StringBuilder sb = new StringBuilder(100);
	}
}
