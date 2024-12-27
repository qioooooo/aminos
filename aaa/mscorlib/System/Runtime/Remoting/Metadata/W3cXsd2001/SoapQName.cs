using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Metadata.W3cXsd2001
{
	// Token: 0x02000775 RID: 1909
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapQName : ISoapXsd
	{
		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06004478 RID: 17528 RVA: 0x000EADE6 File Offset: 0x000E9DE6
		public static string XsdType
		{
			get
			{
				return "QName";
			}
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x000EADED File Offset: 0x000E9DED
		public string GetXsdType()
		{
			return SoapQName.XsdType;
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x000EADF4 File Offset: 0x000E9DF4
		public SoapQName()
		{
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x000EADFC File Offset: 0x000E9DFC
		public SoapQName(string value)
		{
			this._name = value;
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x000EAE0B File Offset: 0x000E9E0B
		public SoapQName(string key, string name)
		{
			this._name = name;
			this._key = key;
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x000EAE21 File Offset: 0x000E9E21
		public SoapQName(string key, string name, string namespaceValue)
		{
			this._name = name;
			this._namespace = namespaceValue;
			this._key = key;
		}

		// Token: 0x17000C11 RID: 3089
		// (get) Token: 0x0600447E RID: 17534 RVA: 0x000EAE3E File Offset: 0x000E9E3E
		// (set) Token: 0x0600447F RID: 17535 RVA: 0x000EAE46 File Offset: 0x000E9E46
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x06004480 RID: 17536 RVA: 0x000EAE4F File Offset: 0x000E9E4F
		// (set) Token: 0x06004481 RID: 17537 RVA: 0x000EAE57 File Offset: 0x000E9E57
		public string Namespace
		{
			get
			{
				return this._namespace;
			}
			set
			{
				this._namespace = value;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x06004482 RID: 17538 RVA: 0x000EAE60 File Offset: 0x000E9E60
		// (set) Token: 0x06004483 RID: 17539 RVA: 0x000EAE68 File Offset: 0x000E9E68
		public string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x000EAE71 File Offset: 0x000E9E71
		public override string ToString()
		{
			if (this._key == null || this._key.Length == 0)
			{
				return this._name;
			}
			return this._key + ":" + this._name;
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x000EAEA8 File Offset: 0x000E9EA8
		public static SoapQName Parse(string value)
		{
			if (value == null)
			{
				return new SoapQName();
			}
			string text = "";
			string text2 = value;
			int num = value.IndexOf(':');
			if (num > 0)
			{
				text = value.Substring(0, num);
				text2 = value.Substring(num + 1);
			}
			return new SoapQName(text, text2);
		}

		// Token: 0x0400220B RID: 8715
		private string _name;

		// Token: 0x0400220C RID: 8716
		private string _namespace;

		// Token: 0x0400220D RID: 8717
		private string _key;
	}
}
