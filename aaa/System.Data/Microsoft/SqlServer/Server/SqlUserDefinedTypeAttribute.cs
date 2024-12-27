using System;
using System.Data.Common;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x020002A0 RID: 672
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
	public sealed class SqlUserDefinedTypeAttribute : Attribute
	{
		// Token: 0x060022A6 RID: 8870 RVA: 0x0026DF04 File Offset: 0x0026D304
		public SqlUserDefinedTypeAttribute(Format format)
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

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x060022A7 RID: 8871 RVA: 0x0026DF48 File Offset: 0x0026D348
		// (set) Token: 0x060022A8 RID: 8872 RVA: 0x0026DF5C File Offset: 0x0026D35C
		public int MaxByteSize
		{
			get
			{
				return this.m_MaxByteSize;
			}
			set
			{
				if (value < -1)
				{
					throw ADP.ArgumentOutOfRange("MaxByteSize");
				}
				this.m_MaxByteSize = value;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060022A9 RID: 8873 RVA: 0x0026DF80 File Offset: 0x0026D380
		// (set) Token: 0x060022AA RID: 8874 RVA: 0x0026DF94 File Offset: 0x0026D394
		public bool IsFixedLength
		{
			get
			{
				return this.m_IsFixedLength;
			}
			set
			{
				this.m_IsFixedLength = value;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060022AB RID: 8875 RVA: 0x0026DFA8 File Offset: 0x0026D3A8
		// (set) Token: 0x060022AC RID: 8876 RVA: 0x0026DFBC File Offset: 0x0026D3BC
		public bool IsByteOrdered
		{
			get
			{
				return this.m_IsByteOrdered;
			}
			set
			{
				this.m_IsByteOrdered = value;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060022AD RID: 8877 RVA: 0x0026DFD0 File Offset: 0x0026D3D0
		public Format Format
		{
			get
			{
				return this.m_format;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060022AE RID: 8878 RVA: 0x0026DFE4 File Offset: 0x0026D3E4
		// (set) Token: 0x060022AF RID: 8879 RVA: 0x0026DFF8 File Offset: 0x0026D3F8
		public string ValidationMethodName
		{
			get
			{
				return this.m_ValidationMethodName;
			}
			set
			{
				this.m_ValidationMethodName = value;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060022B0 RID: 8880 RVA: 0x0026E00C File Offset: 0x0026D40C
		// (set) Token: 0x060022B1 RID: 8881 RVA: 0x0026E020 File Offset: 0x0026D420
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

		// Token: 0x04001669 RID: 5737
		internal const int YukonMaxByteSizeValue = 8000;

		// Token: 0x0400166A RID: 5738
		private int m_MaxByteSize;

		// Token: 0x0400166B RID: 5739
		private bool m_IsFixedLength;

		// Token: 0x0400166C RID: 5740
		private bool m_IsByteOrdered;

		// Token: 0x0400166D RID: 5741
		private Format m_format;

		// Token: 0x0400166E RID: 5742
		private string m_fName;

		// Token: 0x0400166F RID: 5743
		private string m_ValidationMethodName;
	}
}
