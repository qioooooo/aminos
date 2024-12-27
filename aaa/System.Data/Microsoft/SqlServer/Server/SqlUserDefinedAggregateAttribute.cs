using System;
using System.Data;
using System.Data.Common;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x0200029E RID: 670
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	public sealed class SqlUserDefinedAggregateAttribute : Attribute
	{
		// Token: 0x06002298 RID: 8856 RVA: 0x0026DD8C File Offset: 0x0026D18C
		public SqlUserDefinedAggregateAttribute(Format format)
		{
			switch (format)
			{
			case Format.Unknown:
				throw ADP.NotSupportedUserDefinedTypeSerializationFormat(format, "format");
			case Format.Native:
			case Format.UserDefined:
				this.m_format = format;
				return;
			default:
				throw ADP.InvalidUserDefinedTypeSerializationFormat(format);
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06002299 RID: 8857 RVA: 0x0026DDD8 File Offset: 0x0026D1D8
		// (set) Token: 0x0600229A RID: 8858 RVA: 0x0026DDEC File Offset: 0x0026D1EC
		public int MaxByteSize
		{
			get
			{
				return this.m_MaxByteSize;
			}
			set
			{
				if (value < 0 || value > 8000)
				{
					throw ADP.ArgumentOutOfRange(Res.GetString("SQLUDT_MaxByteSizeValue"), "MaxByteSize", value);
				}
				this.m_MaxByteSize = value;
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x0600229B RID: 8859 RVA: 0x0026DE28 File Offset: 0x0026D228
		// (set) Token: 0x0600229C RID: 8860 RVA: 0x0026DE3C File Offset: 0x0026D23C
		public bool IsInvariantToDuplicates
		{
			get
			{
				return this.m_fInvariantToDup;
			}
			set
			{
				this.m_fInvariantToDup = value;
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x0600229D RID: 8861 RVA: 0x0026DE50 File Offset: 0x0026D250
		// (set) Token: 0x0600229E RID: 8862 RVA: 0x0026DE64 File Offset: 0x0026D264
		public bool IsInvariantToNulls
		{
			get
			{
				return this.m_fInvariantToNulls;
			}
			set
			{
				this.m_fInvariantToNulls = value;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x0600229F RID: 8863 RVA: 0x0026DE78 File Offset: 0x0026D278
		// (set) Token: 0x060022A0 RID: 8864 RVA: 0x0026DE8C File Offset: 0x0026D28C
		public bool IsInvariantToOrder
		{
			get
			{
				return this.m_fInvariantToOrder;
			}
			set
			{
				this.m_fInvariantToOrder = value;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x060022A1 RID: 8865 RVA: 0x0026DEA0 File Offset: 0x0026D2A0
		// (set) Token: 0x060022A2 RID: 8866 RVA: 0x0026DEB4 File Offset: 0x0026D2B4
		public bool IsNullIfEmpty
		{
			get
			{
				return this.m_fNullIfEmpty;
			}
			set
			{
				this.m_fNullIfEmpty = value;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x060022A3 RID: 8867 RVA: 0x0026DEC8 File Offset: 0x0026D2C8
		public Format Format
		{
			get
			{
				return this.m_format;
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060022A4 RID: 8868 RVA: 0x0026DEDC File Offset: 0x0026D2DC
		// (set) Token: 0x060022A5 RID: 8869 RVA: 0x0026DEF0 File Offset: 0x0026D2F0
		public string Name
		{
			get
			{
				return this.m_fName;
			}
			set
			{
				this.m_fName = value;
			}
		}

		// Token: 0x0400165D RID: 5725
		public const int MaxByteSizeValue = 8000;

		// Token: 0x0400165E RID: 5726
		private int m_MaxByteSize;

		// Token: 0x0400165F RID: 5727
		private bool m_fInvariantToDup;

		// Token: 0x04001660 RID: 5728
		private bool m_fInvariantToNulls;

		// Token: 0x04001661 RID: 5729
		private bool m_fInvariantToOrder = true;

		// Token: 0x04001662 RID: 5730
		private bool m_fNullIfEmpty;

		// Token: 0x04001663 RID: 5731
		private Format m_format;

		// Token: 0x04001664 RID: 5732
		private string m_fName;
	}
}
