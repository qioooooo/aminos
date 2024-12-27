using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	// Token: 0x02000494 RID: 1172
	[ComVisible(true)]
	[Serializable]
	public class CodeConnectAccess
	{
		// Token: 0x06002EF5 RID: 12021 RVA: 0x0009FB9C File Offset: 0x0009EB9C
		public CodeConnectAccess(string allowScheme, int allowPort)
		{
			if (!CodeConnectAccess.IsValidScheme(allowScheme))
			{
				throw new ArgumentOutOfRangeException("allowScheme");
			}
			this.SetCodeConnectAccess(allowScheme.ToLower(CultureInfo.InvariantCulture), allowPort);
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x0009FBCC File Offset: 0x0009EBCC
		public static CodeConnectAccess CreateOriginSchemeAccess(int allowPort)
		{
			CodeConnectAccess codeConnectAccess = new CodeConnectAccess();
			codeConnectAccess.SetCodeConnectAccess(CodeConnectAccess.OriginScheme, allowPort);
			return codeConnectAccess;
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x0009FBEC File Offset: 0x0009EBEC
		public static CodeConnectAccess CreateAnySchemeAccess(int allowPort)
		{
			CodeConnectAccess codeConnectAccess = new CodeConnectAccess();
			codeConnectAccess.SetCodeConnectAccess(CodeConnectAccess.AnyScheme, allowPort);
			return codeConnectAccess;
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x0009FC0C File Offset: 0x0009EC0C
		private CodeConnectAccess()
		{
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x0009FC14 File Offset: 0x0009EC14
		private void SetCodeConnectAccess(string lowerCaseScheme, int allowPort)
		{
			this._LowerCaseScheme = lowerCaseScheme;
			if (allowPort == CodeConnectAccess.DefaultPort)
			{
				this._LowerCasePort = "$default";
			}
			else if (allowPort == CodeConnectAccess.OriginPort)
			{
				this._LowerCasePort = "$origin";
			}
			else
			{
				if (allowPort < 0 || allowPort > 65535)
				{
					throw new ArgumentOutOfRangeException("allowPort");
				}
				this._LowerCasePort = allowPort.ToString(CultureInfo.InvariantCulture);
			}
			this._IntPort = allowPort;
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x0009FC82 File Offset: 0x0009EC82
		public string Scheme
		{
			get
			{
				return this._LowerCaseScheme;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (get) Token: 0x06002EFB RID: 12027 RVA: 0x0009FC8A File Offset: 0x0009EC8A
		public int Port
		{
			get
			{
				return this._IntPort;
			}
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x0009FC94 File Offset: 0x0009EC94
		public override bool Equals(object o)
		{
			if (this == o)
			{
				return true;
			}
			CodeConnectAccess codeConnectAccess = o as CodeConnectAccess;
			return codeConnectAccess != null && this.Scheme == codeConnectAccess.Scheme && this.Port == codeConnectAccess.Port;
		}

		// Token: 0x06002EFD RID: 12029 RVA: 0x0009FCD8 File Offset: 0x0009ECD8
		public override int GetHashCode()
		{
			return this.Scheme.GetHashCode() + this.Port.GetHashCode();
		}

		// Token: 0x06002EFE RID: 12030 RVA: 0x0009FD00 File Offset: 0x0009ED00
		internal CodeConnectAccess(string allowScheme, string allowPort)
		{
			if (allowScheme == null || allowScheme.Length == 0)
			{
				throw new ArgumentNullException("allowScheme");
			}
			if (allowPort == null || allowPort.Length == 0)
			{
				throw new ArgumentNullException("allowPort");
			}
			this._LowerCaseScheme = allowScheme.ToLower(CultureInfo.InvariantCulture);
			if (this._LowerCaseScheme == CodeConnectAccess.OriginScheme)
			{
				this._LowerCaseScheme = CodeConnectAccess.OriginScheme;
			}
			else if (this._LowerCaseScheme == CodeConnectAccess.AnyScheme)
			{
				this._LowerCaseScheme = CodeConnectAccess.AnyScheme;
			}
			else if (!CodeConnectAccess.IsValidScheme(this._LowerCaseScheme))
			{
				throw new ArgumentOutOfRangeException("allowScheme");
			}
			this._LowerCasePort = allowPort.ToLower(CultureInfo.InvariantCulture);
			if (this._LowerCasePort == "$default")
			{
				this._IntPort = CodeConnectAccess.DefaultPort;
				return;
			}
			if (this._LowerCasePort == "$origin")
			{
				this._IntPort = CodeConnectAccess.OriginPort;
				return;
			}
			this._IntPort = int.Parse(allowPort, CultureInfo.InvariantCulture);
			if (this._IntPort < 0 || this._IntPort > 65535)
			{
				throw new ArgumentOutOfRangeException("allowPort");
			}
			this._LowerCasePort = this._IntPort.ToString(CultureInfo.InvariantCulture);
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x06002EFF RID: 12031 RVA: 0x0009FE3B File Offset: 0x0009EE3B
		internal bool IsOriginScheme
		{
			get
			{
				return this._LowerCaseScheme == CodeConnectAccess.OriginScheme;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002F00 RID: 12032 RVA: 0x0009FE4A File Offset: 0x0009EE4A
		internal bool IsAnyScheme
		{
			get
			{
				return this._LowerCaseScheme == CodeConnectAccess.AnyScheme;
			}
		}

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002F01 RID: 12033 RVA: 0x0009FE59 File Offset: 0x0009EE59
		internal bool IsDefaultPort
		{
			get
			{
				return this.Port == CodeConnectAccess.DefaultPort;
			}
		}

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002F02 RID: 12034 RVA: 0x0009FE68 File Offset: 0x0009EE68
		internal bool IsOriginPort
		{
			get
			{
				return this.Port == CodeConnectAccess.OriginPort;
			}
		}

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002F03 RID: 12035 RVA: 0x0009FE77 File Offset: 0x0009EE77
		internal string StrPort
		{
			get
			{
				return this._LowerCasePort;
			}
		}

		// Token: 0x06002F04 RID: 12036 RVA: 0x0009FE80 File Offset: 0x0009EE80
		internal static bool IsValidScheme(string scheme)
		{
			if (scheme == null || scheme.Length == 0 || !CodeConnectAccess.IsAsciiLetter(scheme[0]))
			{
				return false;
			}
			for (int i = scheme.Length - 1; i > 0; i--)
			{
				if (!CodeConnectAccess.IsAsciiLetterOrDigit(scheme[i]) && scheme[i] != '+' && scheme[i] != '-' && scheme[i] != '.')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002F05 RID: 12037 RVA: 0x0009FEED File Offset: 0x0009EEED
		private static bool IsAsciiLetterOrDigit(char character)
		{
			return CodeConnectAccess.IsAsciiLetter(character) || (character >= '0' && character <= '9');
		}

		// Token: 0x06002F06 RID: 12038 RVA: 0x0009FF08 File Offset: 0x0009EF08
		private static bool IsAsciiLetter(char character)
		{
			return (character >= 'a' && character <= 'z') || (character >= 'A' && character <= 'Z');
		}

		// Token: 0x040017BF RID: 6079
		private const string DefaultStr = "$default";

		// Token: 0x040017C0 RID: 6080
		private const string OriginStr = "$origin";

		// Token: 0x040017C1 RID: 6081
		internal const int NoPort = -1;

		// Token: 0x040017C2 RID: 6082
		internal const int AnyPort = -2;

		// Token: 0x040017C3 RID: 6083
		private string _LowerCaseScheme;

		// Token: 0x040017C4 RID: 6084
		private string _LowerCasePort;

		// Token: 0x040017C5 RID: 6085
		private int _IntPort;

		// Token: 0x040017C6 RID: 6086
		public static readonly int DefaultPort = -3;

		// Token: 0x040017C7 RID: 6087
		public static readonly int OriginPort = -4;

		// Token: 0x040017C8 RID: 6088
		public static readonly string OriginScheme = "$origin";

		// Token: 0x040017C9 RID: 6089
		public static readonly string AnyScheme = "*";
	}
}
