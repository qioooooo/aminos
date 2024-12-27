using System;
using System.Runtime.Serialization;

namespace System.Data.SqlClient
{
	// Token: 0x020002F2 RID: 754
	[Serializable]
	public sealed class SqlError
	{
		// Token: 0x06002729 RID: 10025 RVA: 0x0028992C File Offset: 0x00288D2C
		internal SqlError(int infoNumber, byte errorState, byte errorClass, string server, string errorMessage, string procedure, int lineNumber)
		{
			this.number = infoNumber;
			this.state = errorState;
			this.errorClass = errorClass;
			this.server = server;
			this.message = errorMessage;
			this.procedure = procedure;
			this.lineNumber = lineNumber;
			if (errorClass != 0)
			{
				Bid.Trace("<sc.SqlError.SqlError|ERR> infoNumber=%d, errorState=%d, errorClass=%d, errorMessage='%ls', procedure='%ls', lineNumber=%d\n", infoNumber, (int)errorState, (int)errorClass, errorMessage, (procedure == null) ? "None" : procedure, lineNumber);
			}
		}

		// Token: 0x0600272A RID: 10026 RVA: 0x002899A0 File Offset: 0x00288DA0
		public override string ToString()
		{
			return typeof(SqlError).ToString() + ": " + this.message;
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x0600272B RID: 10027 RVA: 0x002899CC File Offset: 0x00288DCC
		public string Source
		{
			get
			{
				return this.source;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x0600272C RID: 10028 RVA: 0x002899E0 File Offset: 0x00288DE0
		public int Number
		{
			get
			{
				return this.number;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x0600272D RID: 10029 RVA: 0x002899F4 File Offset: 0x00288DF4
		public byte State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x0600272E RID: 10030 RVA: 0x00289A08 File Offset: 0x00288E08
		public byte Class
		{
			get
			{
				return this.errorClass;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x0600272F RID: 10031 RVA: 0x00289A1C File Offset: 0x00288E1C
		public string Server
		{
			get
			{
				return this.server;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x06002730 RID: 10032 RVA: 0x00289A30 File Offset: 0x00288E30
		public string Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06002731 RID: 10033 RVA: 0x00289A44 File Offset: 0x00288E44
		public string Procedure
		{
			get
			{
				return this.procedure;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06002732 RID: 10034 RVA: 0x00289A58 File Offset: 0x00288E58
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x040018D9 RID: 6361
		private string source = ".Net SqlClient Data Provider";

		// Token: 0x040018DA RID: 6362
		private int number;

		// Token: 0x040018DB RID: 6363
		private byte state;

		// Token: 0x040018DC RID: 6364
		private byte errorClass;

		// Token: 0x040018DD RID: 6365
		[OptionalField(VersionAdded = 2)]
		private string server;

		// Token: 0x040018DE RID: 6366
		private string message;

		// Token: 0x040018DF RID: 6367
		private string procedure;

		// Token: 0x040018E0 RID: 6368
		private int lineNumber;
	}
}
