using System;
using System.Data.SqlTypes;

namespace System.Data.SqlClient
{
	// Token: 0x02000323 RID: 803
	internal sealed class SqlCollation
	{
		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x06002A62 RID: 10850 RVA: 0x0029CA68 File Offset: 0x0029BE68
		// (set) Token: 0x06002A63 RID: 10851 RVA: 0x0029CA84 File Offset: 0x0029BE84
		internal int LCID
		{
			get
			{
				return (int)(this.info & 1048575U);
			}
			set
			{
				this.info = (this.info & 32505856U) | (uint)(value & 1048575);
			}
		}

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x06002A64 RID: 10852 RVA: 0x0029CAAC File Offset: 0x0029BEAC
		// (set) Token: 0x06002A65 RID: 10853 RVA: 0x0029CB1C File Offset: 0x0029BF1C
		internal SqlCompareOptions SqlCompareOptions
		{
			get
			{
				SqlCompareOptions sqlCompareOptions = SqlCompareOptions.None;
				if ((this.info & 1048576U) != 0U)
				{
					sqlCompareOptions |= SqlCompareOptions.IgnoreCase;
				}
				if ((this.info & 2097152U) != 0U)
				{
					sqlCompareOptions |= SqlCompareOptions.IgnoreNonSpace;
				}
				if ((this.info & 4194304U) != 0U)
				{
					sqlCompareOptions |= SqlCompareOptions.IgnoreWidth;
				}
				if ((this.info & 8388608U) != 0U)
				{
					sqlCompareOptions |= SqlCompareOptions.IgnoreKanaType;
				}
				if ((this.info & 16777216U) != 0U)
				{
					sqlCompareOptions |= SqlCompareOptions.BinarySort;
				}
				return sqlCompareOptions;
			}
			set
			{
				uint num = 0U;
				if ((value & SqlCompareOptions.IgnoreCase) != SqlCompareOptions.None)
				{
					num |= 1048576U;
				}
				if ((value & SqlCompareOptions.IgnoreNonSpace) != SqlCompareOptions.None)
				{
					num |= 2097152U;
				}
				if ((value & SqlCompareOptions.IgnoreWidth) != SqlCompareOptions.None)
				{
					num |= 4194304U;
				}
				if ((value & SqlCompareOptions.IgnoreKanaType) != SqlCompareOptions.None)
				{
					num |= 8388608U;
				}
				if ((value & SqlCompareOptions.BinarySort) != SqlCompareOptions.None)
				{
					num |= 16777216U;
				}
				this.info = (this.info & 1048575U) | num;
			}
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x0029CB88 File Offset: 0x0029BF88
		internal string TraceString()
		{
			return string.Format(null, "(LCID={0}, Opts={1})", new object[]
			{
				this.LCID,
				(int)this.SqlCompareOptions
			});
		}

		// Token: 0x04001B81 RID: 7041
		private const uint IgnoreCase = 1048576U;

		// Token: 0x04001B82 RID: 7042
		private const uint IgnoreNonSpace = 2097152U;

		// Token: 0x04001B83 RID: 7043
		private const uint IgnoreWidth = 4194304U;

		// Token: 0x04001B84 RID: 7044
		private const uint IgnoreKanaType = 8388608U;

		// Token: 0x04001B85 RID: 7045
		private const uint BinarySort = 16777216U;

		// Token: 0x04001B86 RID: 7046
		internal const uint MaskLcid = 1048575U;

		// Token: 0x04001B87 RID: 7047
		private const uint MaskCompareOpt = 32505856U;

		// Token: 0x04001B88 RID: 7048
		internal uint info;

		// Token: 0x04001B89 RID: 7049
		internal byte sortId;
	}
}
