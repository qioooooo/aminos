using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200089D RID: 2205
	[ComVisible(true)]
	public class SignatureDescription
	{
		// Token: 0x06005096 RID: 20630 RVA: 0x00121FE1 File Offset: 0x00120FE1
		public SignatureDescription()
		{
		}

		// Token: 0x06005097 RID: 20631 RVA: 0x00121FEC File Offset: 0x00120FEC
		public SignatureDescription(SecurityElement el)
		{
			if (el == null)
			{
				throw new ArgumentNullException("el");
			}
			this._strKey = el.SearchForTextOfTag("Key");
			this._strDigest = el.SearchForTextOfTag("Digest");
			this._strFormatter = el.SearchForTextOfTag("Formatter");
			this._strDeformatter = el.SearchForTextOfTag("Deformatter");
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x06005098 RID: 20632 RVA: 0x00122051 File Offset: 0x00121051
		// (set) Token: 0x06005099 RID: 20633 RVA: 0x00122059 File Offset: 0x00121059
		public string KeyAlgorithm
		{
			get
			{
				return this._strKey;
			}
			set
			{
				this._strKey = value;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x0600509A RID: 20634 RVA: 0x00122062 File Offset: 0x00121062
		// (set) Token: 0x0600509B RID: 20635 RVA: 0x0012206A File Offset: 0x0012106A
		public string DigestAlgorithm
		{
			get
			{
				return this._strDigest;
			}
			set
			{
				this._strDigest = value;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x0600509C RID: 20636 RVA: 0x00122073 File Offset: 0x00121073
		// (set) Token: 0x0600509D RID: 20637 RVA: 0x0012207B File Offset: 0x0012107B
		public string FormatterAlgorithm
		{
			get
			{
				return this._strFormatter;
			}
			set
			{
				this._strFormatter = value;
			}
		}

		// Token: 0x17000E16 RID: 3606
		// (get) Token: 0x0600509E RID: 20638 RVA: 0x00122084 File Offset: 0x00121084
		// (set) Token: 0x0600509F RID: 20639 RVA: 0x0012208C File Offset: 0x0012108C
		public string DeformatterAlgorithm
		{
			get
			{
				return this._strDeformatter;
			}
			set
			{
				this._strDeformatter = value;
			}
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x00122098 File Offset: 0x00121098
		public virtual AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = (AsymmetricSignatureDeformatter)CryptoConfig.CreateFromName(this._strDeformatter);
			asymmetricSignatureDeformatter.SetKey(key);
			return asymmetricSignatureDeformatter;
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x001220C0 File Offset: 0x001210C0
		public virtual AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
		{
			AsymmetricSignatureFormatter asymmetricSignatureFormatter = (AsymmetricSignatureFormatter)CryptoConfig.CreateFromName(this._strFormatter);
			asymmetricSignatureFormatter.SetKey(key);
			return asymmetricSignatureFormatter;
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x001220E6 File Offset: 0x001210E6
		public virtual HashAlgorithm CreateDigest()
		{
			return (HashAlgorithm)CryptoConfig.CreateFromName(this._strDigest);
		}

		// Token: 0x04002939 RID: 10553
		private string _strKey;

		// Token: 0x0400293A RID: 10554
		private string _strDigest;

		// Token: 0x0400293B RID: 10555
		private string _strFormatter;

		// Token: 0x0400293C RID: 10556
		private string _strDeformatter;
	}
}
