using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000766 RID: 1894
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapTime : ISoapXsd
	{
		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x060043EE RID: 17390 RVA: 0x000E9ED8 File Offset: 0x000E8ED8
		public static string XsdType
		{
			get
			{
				return "time";
			}
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x000E9EDF File Offset: 0x000E8EDF
		public string GetXsdType()
		{
			return SoapTime.XsdType;
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x000E9EE6 File Offset: 0x000E8EE6
		public SoapTime()
		{
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x000E9EF9 File Offset: 0x000E8EF9
		public SoapTime(DateTime value)
		{
			this._value = value;
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x060043F2 RID: 17394 RVA: 0x000E9F13 File Offset: 0x000E8F13
		// (set) Token: 0x060043F3 RID: 17395 RVA: 0x000E9F1C File Offset: 0x000E8F1C
		public DateTime Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = new DateTime(1, 1, 1, value.Hour, value.Minute, value.Second, value.Millisecond);
			}
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x000E9F53 File Offset: 0x000E8F53
		public override string ToString()
		{
			return this._value.ToString("HH:mm:ss.fffffffzzz", CultureInfo.InvariantCulture);
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x000E9F6C File Offset: 0x000E8F6C
		public static SoapTime Parse(string value)
		{
			string text = value;
			if (value.EndsWith("Z", StringComparison.Ordinal))
			{
				text = value.Substring(0, value.Length - 1) + "-00:00";
			}
			return new SoapTime(DateTime.ParseExact(text, SoapTime.formats, CultureInfo.InvariantCulture, DateTimeStyles.None));
		}

		// Token: 0x040021F1 RID: 8689
		private DateTime _value = DateTime.MinValue;

		// Token: 0x040021F2 RID: 8690
		private static string[] formats = new string[]
		{
			"HH:mm:ss.fffffffzzz", "HH:mm:ss.ffff", "HH:mm:ss.ffffzzz", "HH:mm:ss.fff", "HH:mm:ss.fffzzz", "HH:mm:ss.ff", "HH:mm:ss.ffzzz", "HH:mm:ss.f", "HH:mm:ss.fzzz", "HH:mm:ss",
			"HH:mm:sszzz", "HH:mm:ss.fffff", "HH:mm:ss.fffffzzz", "HH:mm:ss.ffffff", "HH:mm:ss.ffffffzzz", "HH:mm:ss.fffffff", "HH:mm:ss.ffffffff", "HH:mm:ss.ffffffffzzz", "HH:mm:ss.fffffffff", "HH:mm:ss.fffffffffzzz",
			"HH:mm:ss.fffffffff", "HH:mm:ss.fffffffffzzz"
		};
	}
}
