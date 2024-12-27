using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x0200076E RID: 1902
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapBase64Binary : ISoapXsd
	{
		// Token: 0x17000C02 RID: 3074
		// (get) Token: 0x06004440 RID: 17472 RVA: 0x000EA848 File Offset: 0x000E9848
		public static string XsdType
		{
			get
			{
				return "base64Binary";
			}
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x000EA84F File Offset: 0x000E984F
		public string GetXsdType()
		{
			return SoapBase64Binary.XsdType;
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x000EA856 File Offset: 0x000E9856
		public SoapBase64Binary()
		{
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x000EA85E File Offset: 0x000E985E
		public SoapBase64Binary(byte[] value)
		{
			this._value = value;
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x06004444 RID: 17476 RVA: 0x000EA86D File Offset: 0x000E986D
		// (set) Token: 0x06004445 RID: 17477 RVA: 0x000EA875 File Offset: 0x000E9875
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

		// Token: 0x06004446 RID: 17478 RVA: 0x000EA87E File Offset: 0x000E987E
		public override string ToString()
		{
			if (this._value == null)
			{
				return null;
			}
			return SoapType.LineFeedsBin64(Convert.ToBase64String(this._value));
		}

		// Token: 0x06004447 RID: 17479 RVA: 0x000EA89C File Offset: 0x000E989C
		public static SoapBase64Binary Parse(string value)
		{
			if (value == null || value.Length == 0)
			{
				return new SoapBase64Binary(new byte[0]);
			}
			byte[] array;
			try
			{
				array = Convert.FromBase64String(SoapType.FilterBin64(value));
			}
			catch (Exception)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_SOAPInteropxsdInvalid"), new object[] { "base64Binary", value }));
			}
			return new SoapBase64Binary(array);
		}

		// Token: 0x04002204 RID: 8708
		private byte[] _value;
	}
}
