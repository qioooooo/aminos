using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008B8 RID: 2232
	internal struct HashAlgorithmName : IEquatable<HashAlgorithmName>
	{
		// Token: 0x17000E37 RID: 3639
		// (get) Token: 0x060051F4 RID: 20980 RVA: 0x00127084 File Offset: 0x00126084
		public static HashAlgorithmName MD5
		{
			get
			{
				return new HashAlgorithmName("MD5");
			}
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x060051F5 RID: 20981 RVA: 0x00127090 File Offset: 0x00126090
		public static HashAlgorithmName SHA1
		{
			get
			{
				return new HashAlgorithmName("SHA1");
			}
		}

		// Token: 0x17000E39 RID: 3641
		// (get) Token: 0x060051F6 RID: 20982 RVA: 0x0012709C File Offset: 0x0012609C
		public static HashAlgorithmName SHA256
		{
			get
			{
				return new HashAlgorithmName("SHA256");
			}
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x060051F7 RID: 20983 RVA: 0x001270A8 File Offset: 0x001260A8
		public static HashAlgorithmName SHA384
		{
			get
			{
				return new HashAlgorithmName("SHA384");
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x060051F8 RID: 20984 RVA: 0x001270B4 File Offset: 0x001260B4
		public static HashAlgorithmName SHA512
		{
			get
			{
				return new HashAlgorithmName("SHA512");
			}
		}

		// Token: 0x060051F9 RID: 20985 RVA: 0x001270C0 File Offset: 0x001260C0
		public HashAlgorithmName(string name)
		{
			this._name = name;
		}

		// Token: 0x17000E3C RID: 3644
		// (get) Token: 0x060051FA RID: 20986 RVA: 0x001270C9 File Offset: 0x001260C9
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x001270D1 File Offset: 0x001260D1
		public override string ToString()
		{
			return this._name ?? string.Empty;
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x001270E2 File Offset: 0x001260E2
		public override bool Equals(object obj)
		{
			return obj is HashAlgorithmName && this.Equals((HashAlgorithmName)obj);
		}

		// Token: 0x060051FD RID: 20989 RVA: 0x001270FA File Offset: 0x001260FA
		public bool Equals(HashAlgorithmName other)
		{
			return this._name == other._name;
		}

		// Token: 0x060051FE RID: 20990 RVA: 0x0012710E File Offset: 0x0012610E
		public override int GetHashCode()
		{
			if (this._name != null)
			{
				return this._name.GetHashCode();
			}
			return 0;
		}

		// Token: 0x060051FF RID: 20991 RVA: 0x00127125 File Offset: 0x00126125
		public static bool operator ==(HashAlgorithmName left, HashAlgorithmName right)
		{
			return left.Equals(right);
		}

		// Token: 0x06005200 RID: 20992 RVA: 0x0012712F File Offset: 0x0012612F
		public static bool operator !=(HashAlgorithmName left, HashAlgorithmName right)
		{
			return !(left == right);
		}

		// Token: 0x04002A07 RID: 10759
		private readonly string _name;
	}
}
