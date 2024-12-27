using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Transactions;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000049 RID: 73
	internal abstract class SmiRequestExecutor : SmiTypedGetterSetter, ITypedSettersV3, ITypedSetters, ITypedGetters, IDisposable
	{
		// Token: 0x06000284 RID: 644 RVA: 0x001CD254 File Offset: 0x001CC654
		public virtual void Close(SmiEventSink eventSink)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x001CD268 File Offset: 0x001CC668
		internal virtual SmiEventStream Execute(SmiConnection connection, long transactionId, Transaction associatedTransaction, CommandBehavior behavior, SmiExecuteType executeType)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000286 RID: 646 RVA: 0x001CD27C File Offset: 0x001CC67C
		internal override bool CanGet
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000287 RID: 647 RVA: 0x001CD28C File Offset: 0x001CC68C
		internal override bool CanSet
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000288 RID: 648
		internal abstract void SetDefault(int ordinal);

		// Token: 0x06000289 RID: 649 RVA: 0x001CD29C File Offset: 0x001CC69C
		internal virtual SmiEventStream Execute(SmiConnection connection, long transactionId, CommandBehavior behavior, SmiExecuteType executeType)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600028A RID: 650 RVA: 0x001CD2B0 File Offset: 0x001CC6B0
		public virtual void Dispose()
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x001CD2C4 File Offset: 0x001CC6C4
		internal virtual bool IsSetAsDefault(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600028C RID: 652 RVA: 0x001CD2D8 File Offset: 0x001CC6D8
		public virtual int Count
		{
			get
			{
				throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
			}
		}

		// Token: 0x0600028D RID: 653 RVA: 0x001CD2EC File Offset: 0x001CC6EC
		public virtual SmiParameterMetaData GetMetaData(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x001CD300 File Offset: 0x001CC700
		public virtual bool IsDBNull(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x001CD314 File Offset: 0x001CC714
		public virtual SqlDbType GetVariantType(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x001CD328 File Offset: 0x001CC728
		public virtual bool GetBoolean(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x001CD33C File Offset: 0x001CC73C
		public virtual byte GetByte(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x001CD350 File Offset: 0x001CC750
		public virtual long GetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x001CD364 File Offset: 0x001CC764
		public virtual char GetChar(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x001CD378 File Offset: 0x001CC778
		public virtual long GetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x001CD38C File Offset: 0x001CC78C
		public virtual short GetInt16(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x001CD3A0 File Offset: 0x001CC7A0
		public virtual int GetInt32(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x001CD3B4 File Offset: 0x001CC7B4
		public virtual long GetInt64(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x001CD3C8 File Offset: 0x001CC7C8
		public virtual float GetFloat(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x001CD3DC File Offset: 0x001CC7DC
		public virtual double GetDouble(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x001CD3F0 File Offset: 0x001CC7F0
		public virtual string GetString(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x001CD404 File Offset: 0x001CC804
		public virtual decimal GetDecimal(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x001CD418 File Offset: 0x001CC818
		public virtual DateTime GetDateTime(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x001CD42C File Offset: 0x001CC82C
		public virtual Guid GetGuid(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x001CD440 File Offset: 0x001CC840
		public virtual SqlBoolean GetSqlBoolean(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x001CD454 File Offset: 0x001CC854
		public virtual SqlByte GetSqlByte(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x001CD468 File Offset: 0x001CC868
		public virtual SqlInt16 GetSqlInt16(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x001CD47C File Offset: 0x001CC87C
		public virtual SqlInt32 GetSqlInt32(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x001CD490 File Offset: 0x001CC890
		public virtual SqlInt64 GetSqlInt64(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x001CD4A4 File Offset: 0x001CC8A4
		public virtual SqlSingle GetSqlSingle(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x001CD4B8 File Offset: 0x001CC8B8
		public virtual SqlDouble GetSqlDouble(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x001CD4CC File Offset: 0x001CC8CC
		public virtual SqlMoney GetSqlMoney(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x001CD4E0 File Offset: 0x001CC8E0
		public virtual SqlDateTime GetSqlDateTime(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x001CD4F4 File Offset: 0x001CC8F4
		public virtual SqlDecimal GetSqlDecimal(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x001CD508 File Offset: 0x001CC908
		public virtual SqlString GetSqlString(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x001CD51C File Offset: 0x001CC91C
		public virtual SqlBinary GetSqlBinary(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x001CD530 File Offset: 0x001CC930
		public virtual SqlGuid GetSqlGuid(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x001CD544 File Offset: 0x001CC944
		public virtual SqlChars GetSqlChars(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x001CD558 File Offset: 0x001CC958
		public virtual SqlBytes GetSqlBytes(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x001CD56C File Offset: 0x001CC96C
		public virtual SqlXml GetSqlXml(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x001CD580 File Offset: 0x001CC980
		public virtual SqlXml GetSqlXmlRef(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x001CD594 File Offset: 0x001CC994
		public virtual SqlBytes GetSqlBytesRef(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x001CD5A8 File Offset: 0x001CC9A8
		public virtual SqlChars GetSqlCharsRef(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x001CD5BC File Offset: 0x001CC9BC
		public virtual void SetDBNull(int ordinal)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x001CD5D0 File Offset: 0x001CC9D0
		public virtual void SetBoolean(int ordinal, bool value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x001CD5E4 File Offset: 0x001CC9E4
		public virtual void SetByte(int ordinal, byte value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x001CD5F8 File Offset: 0x001CC9F8
		public virtual void SetBytes(int ordinal, long fieldOffset, byte[] buffer, int bufferOffset, int length)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x001CD60C File Offset: 0x001CCA0C
		public virtual void SetChar(int ordinal, char value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x001CD620 File Offset: 0x001CCA20
		public virtual void SetChars(int ordinal, long fieldOffset, char[] buffer, int bufferOffset, int length)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x001CD634 File Offset: 0x001CCA34
		public virtual void SetInt16(int ordinal, short value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x001CD648 File Offset: 0x001CCA48
		public virtual void SetInt32(int ordinal, int value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x001CD65C File Offset: 0x001CCA5C
		public virtual void SetInt64(int ordinal, long value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002BA RID: 698 RVA: 0x001CD670 File Offset: 0x001CCA70
		public virtual void SetFloat(int ordinal, float value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x001CD684 File Offset: 0x001CCA84
		public virtual void SetDouble(int ordinal, double value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x001CD698 File Offset: 0x001CCA98
		public virtual void SetString(int ordinal, string value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x001CD6AC File Offset: 0x001CCAAC
		public virtual void SetString(int ordinal, string value, int offset)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x001CD6C0 File Offset: 0x001CCAC0
		public virtual void SetDecimal(int ordinal, decimal value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002BF RID: 703 RVA: 0x001CD6D4 File Offset: 0x001CCAD4
		public virtual void SetDateTime(int ordinal, DateTime value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x001CD6E8 File Offset: 0x001CCAE8
		public virtual void SetGuid(int ordinal, Guid value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x001CD6FC File Offset: 0x001CCAFC
		public virtual void SetSqlBoolean(int ordinal, SqlBoolean value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x001CD710 File Offset: 0x001CCB10
		public virtual void SetSqlByte(int ordinal, SqlByte value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x001CD724 File Offset: 0x001CCB24
		public virtual void SetSqlInt16(int ordinal, SqlInt16 value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x001CD738 File Offset: 0x001CCB38
		public virtual void SetSqlInt32(int ordinal, SqlInt32 value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x001CD74C File Offset: 0x001CCB4C
		public virtual void SetSqlInt64(int ordinal, SqlInt64 value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x001CD760 File Offset: 0x001CCB60
		public virtual void SetSqlSingle(int ordinal, SqlSingle value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x001CD774 File Offset: 0x001CCB74
		public virtual void SetSqlDouble(int ordinal, SqlDouble value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x001CD788 File Offset: 0x001CCB88
		public virtual void SetSqlMoney(int ordinal, SqlMoney value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x001CD79C File Offset: 0x001CCB9C
		public virtual void SetSqlDateTime(int ordinal, SqlDateTime value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x001CD7B0 File Offset: 0x001CCBB0
		public virtual void SetSqlDecimal(int ordinal, SqlDecimal value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x001CD7C4 File Offset: 0x001CCBC4
		public virtual void SetSqlString(int ordinal, SqlString value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x001CD7D8 File Offset: 0x001CCBD8
		public virtual void SetSqlString(int ordinal, SqlString value, int offset)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x001CD7EC File Offset: 0x001CCBEC
		public virtual void SetSqlBinary(int ordinal, SqlBinary value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x001CD800 File Offset: 0x001CCC00
		public virtual void SetSqlBinary(int ordinal, SqlBinary value, int offset)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x001CD814 File Offset: 0x001CCC14
		public virtual void SetSqlGuid(int ordinal, SqlGuid value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x001CD828 File Offset: 0x001CCC28
		public virtual void SetSqlChars(int ordinal, SqlChars value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x001CD83C File Offset: 0x001CCC3C
		public virtual void SetSqlChars(int ordinal, SqlChars value, int offset)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x001CD850 File Offset: 0x001CCC50
		public virtual void SetSqlBytes(int ordinal, SqlBytes value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x001CD864 File Offset: 0x001CCC64
		public virtual void SetSqlBytes(int ordinal, SqlBytes value, int offset)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x001CD878 File Offset: 0x001CCC78
		public virtual void SetSqlXml(int ordinal, SqlXml value)
		{
			throw ADP.InternalError(ADP.InternalErrorCode.UnimplementedSMIMethod);
		}
	}
}
