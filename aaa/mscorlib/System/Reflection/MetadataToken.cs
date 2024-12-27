using System;
using System.Globalization;

namespace System.Reflection
{
	// Token: 0x0200030C RID: 780
	[Serializable]
	internal struct MetadataToken
	{
		// Token: 0x06001E59 RID: 7769 RVA: 0x0004D25A File Offset: 0x0004C25A
		public static implicit operator int(MetadataToken token)
		{
			return token.Value;
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x0004D263 File Offset: 0x0004C263
		public static implicit operator MetadataToken(int token)
		{
			return new MetadataToken(token);
		}

		// Token: 0x06001E5B RID: 7771 RVA: 0x0004D26C File Offset: 0x0004C26C
		public static bool IsTokenOfType(int token, params MetadataTokenType[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if ((MetadataTokenType)((long)token & (long)((ulong)(-16777216))) == types[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x0004D299 File Offset: 0x0004C299
		public static bool IsNullToken(int token)
		{
			return (token & 16777215) == 0;
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x0004D2A5 File Offset: 0x0004C2A5
		public MetadataToken(int token)
		{
			this.Value = token;
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06001E5E RID: 7774 RVA: 0x0004D2AE File Offset: 0x0004C2AE
		public bool IsGlobalTypeDefToken
		{
			get
			{
				return this.Value == 33554433;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06001E5F RID: 7775 RVA: 0x0004D2BD File Offset: 0x0004C2BD
		public MetadataTokenType TokenType
		{
			get
			{
				return (MetadataTokenType)((long)this.Value & (long)((ulong)(-16777216)));
			}
		}

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x06001E60 RID: 7776 RVA: 0x0004D2CE File Offset: 0x0004C2CE
		public bool IsTypeRef
		{
			get
			{
				return this.TokenType == MetadataTokenType.TypeRef;
			}
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x0004D2DD File Offset: 0x0004C2DD
		public bool IsTypeDef
		{
			get
			{
				return this.TokenType == MetadataTokenType.TypeDef;
			}
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06001E62 RID: 7778 RVA: 0x0004D2EC File Offset: 0x0004C2EC
		public bool IsFieldDef
		{
			get
			{
				return this.TokenType == MetadataTokenType.FieldDef;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x0004D2FB File Offset: 0x0004C2FB
		public bool IsMethodDef
		{
			get
			{
				return this.TokenType == MetadataTokenType.MethodDef;
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001E64 RID: 7780 RVA: 0x0004D30A File Offset: 0x0004C30A
		public bool IsMemberRef
		{
			get
			{
				return this.TokenType == MetadataTokenType.MemberRef;
			}
		}

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x0004D319 File Offset: 0x0004C319
		public bool IsEvent
		{
			get
			{
				return this.TokenType == MetadataTokenType.Event;
			}
		}

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001E66 RID: 7782 RVA: 0x0004D328 File Offset: 0x0004C328
		public bool IsProperty
		{
			get
			{
				return this.TokenType == MetadataTokenType.Property;
			}
		}

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x0004D337 File Offset: 0x0004C337
		public bool IsParamDef
		{
			get
			{
				return this.TokenType == MetadataTokenType.ParamDef;
			}
		}

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001E68 RID: 7784 RVA: 0x0004D346 File Offset: 0x0004C346
		public bool IsTypeSpec
		{
			get
			{
				return this.TokenType == MetadataTokenType.TypeSpec;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x0004D355 File Offset: 0x0004C355
		public bool IsMethodSpec
		{
			get
			{
				return this.TokenType == MetadataTokenType.MethodSpec;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001E6A RID: 7786 RVA: 0x0004D364 File Offset: 0x0004C364
		public bool IsString
		{
			get
			{
				return this.TokenType == MetadataTokenType.String;
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x0004D373 File Offset: 0x0004C373
		public bool IsSignature
		{
			get
			{
				return this.TokenType == MetadataTokenType.Signature;
			}
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x0004D384 File Offset: 0x0004C384
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "0x{0:x8}", new object[] { this.Value });
		}

		// Token: 0x04000CBB RID: 3259
		public int Value;
	}
}
