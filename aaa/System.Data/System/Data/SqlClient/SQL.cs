using System;
using System.Data.Common;
using System.Data.SqlTypes;
using System.Globalization;
using System.Transactions;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x02000314 RID: 788
	internal sealed class SQL
	{
		// Token: 0x0600291C RID: 10524 RVA: 0x00292600 File Offset: 0x00291A00
		private SQL()
		{
		}

		// Token: 0x0600291D RID: 10525 RVA: 0x00292614 File Offset: 0x00291A14
		internal static Exception CannotGetDTCAddress()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_CannotGetDTCAddress"));
		}

		// Token: 0x0600291E RID: 10526 RVA: 0x00292630 File Offset: 0x00291A30
		internal static Exception InvalidOptionLength(string key)
		{
			return ADP.Argument(Res.GetString("SQL_InvalidOptionLength", new object[] { key }));
		}

		// Token: 0x0600291F RID: 10527 RVA: 0x00292658 File Offset: 0x00291A58
		internal static Exception InvalidInternalPacketSize(string str)
		{
			return ADP.ArgumentOutOfRange(str);
		}

		// Token: 0x06002920 RID: 10528 RVA: 0x0029266C File Offset: 0x00291A6C
		internal static Exception InvalidPacketSize()
		{
			return ADP.ArgumentOutOfRange(Res.GetString("SQL_InvalidTDSPacketSize"));
		}

		// Token: 0x06002921 RID: 10529 RVA: 0x00292688 File Offset: 0x00291A88
		internal static Exception InvalidPacketSizeValue()
		{
			return ADP.Argument(Res.GetString("SQL_InvalidPacketSizeValue"));
		}

		// Token: 0x06002922 RID: 10530 RVA: 0x002926A4 File Offset: 0x00291AA4
		internal static Exception InvalidSSPIPacketSize()
		{
			return ADP.Argument(Res.GetString("SQL_InvalidSSPIPacketSize"));
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x002926C0 File Offset: 0x00291AC0
		internal static Exception NullEmptyTransactionName()
		{
			return ADP.Argument(Res.GetString("SQL_NullEmptyTransactionName"));
		}

		// Token: 0x06002924 RID: 10532 RVA: 0x002926DC File Offset: 0x00291ADC
		internal static Exception SnapshotNotSupported(IsolationLevel level)
		{
			return ADP.Argument(Res.GetString("SQL_SnapshotNotSupported", new object[]
			{
				typeof(IsolationLevel),
				level.ToString()
			}));
		}

		// Token: 0x06002925 RID: 10533 RVA: 0x0029271C File Offset: 0x00291B1C
		internal static Exception UserInstanceFailoverNotCompatible()
		{
			return ADP.Argument(Res.GetString("SQL_UserInstanceFailoverNotCompatible"));
		}

		// Token: 0x06002926 RID: 10534 RVA: 0x00292738 File Offset: 0x00291B38
		internal static Exception InvalidSQLServerVersionUnknown()
		{
			return ADP.DataAdapter(Res.GetString("SQL_InvalidSQLServerVersionUnknown"));
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x00292754 File Offset: 0x00291B54
		internal static Exception ConnectionLockedForBcpEvent()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ConnectionLockedForBcpEvent"));
		}

		// Token: 0x06002928 RID: 10536 RVA: 0x00292770 File Offset: 0x00291B70
		internal static Exception AsyncConnectionRequired()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_AsyncConnectionRequired"));
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x0029278C File Offset: 0x00291B8C
		internal static Exception FatalTimeout()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_FatalTimeout"));
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x002927A8 File Offset: 0x00291BA8
		internal static Exception InstanceFailure()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_InstanceFailure"));
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x002927C4 File Offset: 0x00291BC4
		internal static Exception ChangePasswordArgumentMissing(string argumentName)
		{
			return ADP.ArgumentNull(Res.GetString("SQL_ChangePasswordArgumentMissing", new object[] { argumentName }));
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x002927EC File Offset: 0x00291BEC
		internal static Exception ChangePasswordConflictsWithSSPI()
		{
			return ADP.Argument(Res.GetString("SQL_ChangePasswordConflictsWithSSPI"));
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x00292808 File Offset: 0x00291C08
		internal static Exception ChangePasswordRequiresYukon()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ChangePasswordRequiresYukon"));
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x00292824 File Offset: 0x00291C24
		internal static Exception UnknownSysTxIsolationLevel(IsolationLevel isolationLevel)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_UnknownSysTxIsolationLevel", new object[] { isolationLevel.ToString() }));
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x00292858 File Offset: 0x00291C58
		internal static Exception ChangePasswordUseOfUnallowedKey(string key)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ChangePasswordUseOfUnallowedKey", new object[] { key }));
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x00292880 File Offset: 0x00291C80
		internal static Exception InvalidPartnerConfiguration(string server, string database)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_InvalidPartnerConfiguration", new object[] { server, database }));
		}

		// Token: 0x06002931 RID: 10545 RVA: 0x002928AC File Offset: 0x00291CAC
		internal static Exception MARSUnspportedOnConnection()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_MarsUnsupportedOnConnection"));
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x002928C8 File Offset: 0x00291CC8
		internal static Exception AsyncInProcNotSupported()
		{
			return ADP.NotSupported(Res.GetString("SQL_AsyncInProcNotSupported"));
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x002928E4 File Offset: 0x00291CE4
		internal static Exception CannotModifyPropertyAsyncOperationInProgress(string property)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_CannotModifyPropertyAsyncOperationInProgress", new object[] { property }));
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x0029290C File Offset: 0x00291D0C
		internal static Exception NonLocalSSEInstance()
		{
			return ADP.NotSupported(Res.GetString("SQL_NonLocalSSEInstance"));
		}

		// Token: 0x06002935 RID: 10549 RVA: 0x00292928 File Offset: 0x00291D28
		internal static Exception NotificationsRequireYukon()
		{
			return ADP.NotSupported(Res.GetString("SQL_NotificationsRequireYukon"));
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x00292944 File Offset: 0x00291D44
		internal static ArgumentOutOfRangeException NotSupportedEnumerationValue(Type type, int value)
		{
			return ADP.ArgumentOutOfRange(Res.GetString("SQL_NotSupportedEnumerationValue", new object[]
			{
				type.Name,
				value.ToString(CultureInfo.InvariantCulture)
			}), type.Name);
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x00292988 File Offset: 0x00291D88
		internal static ArgumentOutOfRangeException NotSupportedCommandType(CommandType value)
		{
			return SQL.NotSupportedEnumerationValue(typeof(CommandType), (int)value);
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x002929A8 File Offset: 0x00291DA8
		internal static ArgumentOutOfRangeException NotSupportedIsolationLevel(IsolationLevel value)
		{
			return SQL.NotSupportedEnumerationValue(typeof(IsolationLevel), (int)value);
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x002929C8 File Offset: 0x00291DC8
		internal static Exception OperationCancelled()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_OperationCancelled"));
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x002929E4 File Offset: 0x00291DE4
		internal static Exception PendingBeginXXXExists()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_PendingBeginXXXExists"));
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x00292A00 File Offset: 0x00291E00
		internal static ArgumentOutOfRangeException InvalidSqlDependencyTimeout(string param)
		{
			return ADP.ArgumentOutOfRange(Res.GetString("SqlDependency_InvalidTimeout"), param);
		}

		// Token: 0x0600293C RID: 10556 RVA: 0x00292A20 File Offset: 0x00291E20
		internal static Exception NonXmlResult()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_NonXmlResult"));
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x00292A3C File Offset: 0x00291E3C
		internal static Exception InvalidUdt3PartNameFormat()
		{
			return ADP.Argument(Res.GetString("SQL_InvalidUdt3PartNameFormat"));
		}

		// Token: 0x0600293E RID: 10558 RVA: 0x00292A58 File Offset: 0x00291E58
		internal static Exception InvalidParameterTypeNameFormat()
		{
			return ADP.Argument(Res.GetString("SQL_InvalidParameterTypeNameFormat"));
		}

		// Token: 0x0600293F RID: 10559 RVA: 0x00292A74 File Offset: 0x00291E74
		internal static Exception InvalidParameterNameLength(string value)
		{
			return ADP.Argument(Res.GetString("SQL_InvalidParameterNameLength", new object[] { value }));
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x00292A9C File Offset: 0x00291E9C
		internal static Exception PrecisionValueOutOfRange(byte precision)
		{
			return ADP.Argument(Res.GetString("SQL_PrecisionValueOutOfRange", new object[] { precision.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x00292AD0 File Offset: 0x00291ED0
		internal static Exception ScaleValueOutOfRange(byte scale)
		{
			return ADP.Argument(Res.GetString("SQL_ScaleValueOutOfRange", new object[] { scale.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x06002942 RID: 10562 RVA: 0x00292B04 File Offset: 0x00291F04
		internal static Exception TimeScaleValueOutOfRange(byte scale)
		{
			return ADP.Argument(Res.GetString("SQL_TimeScaleValueOutOfRange", new object[] { scale.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x00292B38 File Offset: 0x00291F38
		internal static Exception InvalidSqlDbType(SqlDbType value)
		{
			return ADP.InvalidEnumerationValue(typeof(SqlDbType), (int)value);
		}

		// Token: 0x06002944 RID: 10564 RVA: 0x00292B58 File Offset: 0x00291F58
		internal static Exception UnsupportedTVPOutputParameter(ParameterDirection direction, string paramName)
		{
			return ADP.NotSupported(Res.GetString("SqlParameter_UnsupportedTVPOutputParameter", new object[]
			{
				direction.ToString(CultureInfo.InvariantCulture),
				paramName
			}));
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x00292B94 File Offset: 0x00291F94
		internal static Exception DBNullNotSupportedForTVPValues(string paramName)
		{
			return ADP.NotSupported(Res.GetString("SqlParameter_DBNullNotSupportedForTVP", new object[] { paramName }));
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x00292BBC File Offset: 0x00291FBC
		internal static Exception InvalidTableDerivedPrecisionForTvp(string columnName, byte precision)
		{
			return ADP.InvalidOperation(Res.GetString("SqlParameter_InvalidTableDerivedPrecisionForTvp", new object[]
			{
				precision,
				columnName,
				SqlDecimal.MaxPrecision
			}));
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x00292BFC File Offset: 0x00291FFC
		internal static Exception UnexpectedTypeNameForNonStructParams(string paramName)
		{
			return ADP.NotSupported(Res.GetString("SqlParameter_UnexpectedTypeNameForNonStruct", new object[] { paramName }));
		}

		// Token: 0x06002948 RID: 10568 RVA: 0x00292C24 File Offset: 0x00292024
		internal static Exception SingleValuedStructNotSupported()
		{
			return ADP.NotSupported(Res.GetString("MetaType_SingleValuedStructNotSupported"));
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x00292C40 File Offset: 0x00292040
		internal static Exception ParameterInvalidVariant(string paramName)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ParameterInvalidVariant", new object[] { paramName }));
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x00292C68 File Offset: 0x00292068
		internal static Exception MustSetTypeNameForParam(string paramType, string paramName)
		{
			return ADP.Argument(Res.GetString("SQL_ParameterTypeNameRequired", new object[] { paramType, paramName }));
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x00292C94 File Offset: 0x00292094
		internal static Exception NullSchemaTableDataTypeNotSupported(string columnName)
		{
			return ADP.Argument(Res.GetString("NullSchemaTableDataTypeNotSupported", new object[] { columnName }));
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x00292CBC File Offset: 0x002920BC
		internal static Exception InvalidSchemaTableOrdinals()
		{
			return ADP.Argument(Res.GetString("InvalidSchemaTableOrdinals"));
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x00292CD8 File Offset: 0x002920D8
		internal static Exception EnumeratedRecordMetaDataChanged(string fieldName, int recordNumber)
		{
			return ADP.Argument(Res.GetString("SQL_EnumeratedRecordMetaDataChanged", new object[] { fieldName, recordNumber }));
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x00292D0C File Offset: 0x0029210C
		internal static Exception EnumeratedRecordFieldCountChanged(int recordNumber)
		{
			return ADP.Argument(Res.GetString("SQL_EnumeratedRecordFieldCountChanged", new object[] { recordNumber }));
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x00292D3C File Offset: 0x0029213C
		internal static Exception InvalidTDSVersion()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_InvalidTDSVersion"));
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x00292D58 File Offset: 0x00292158
		internal static Exception ParsingError()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ParsingError"));
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x00292D74 File Offset: 0x00292174
		internal static Exception MoneyOverflow(string moneyValue)
		{
			return ADP.Overflow(Res.GetString("SQL_MoneyOverflow", new object[] { moneyValue }));
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x00292D9C File Offset: 0x0029219C
		internal static Exception SmallDateTimeOverflow(string datetime)
		{
			return ADP.Overflow(Res.GetString("SQL_SmallDateTimeOverflow", new object[] { datetime }));
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x00292DC4 File Offset: 0x002921C4
		internal static Exception SNIPacketAllocationFailure()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_SNIPacketAllocationFailure"));
		}

		// Token: 0x06002954 RID: 10580 RVA: 0x00292DE0 File Offset: 0x002921E0
		internal static Exception TimeOverflow(string time)
		{
			return ADP.Overflow(Res.GetString("SQL_TimeOverflow", new object[] { time }));
		}

		// Token: 0x06002955 RID: 10581 RVA: 0x00292E08 File Offset: 0x00292208
		internal static Exception InvalidRead()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_InvalidRead"));
		}

		// Token: 0x06002956 RID: 10582 RVA: 0x00292E24 File Offset: 0x00292224
		internal static Exception NonBlobColumn(string columnName)
		{
			return ADP.InvalidCast(Res.GetString("SQL_NonBlobColumn", new object[] { columnName }));
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x00292E4C File Offset: 0x0029224C
		internal static Exception NonCharColumn(string columnName)
		{
			return ADP.InvalidCast(Res.GetString("SQL_NonCharColumn", new object[] { columnName }));
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x00292E74 File Offset: 0x00292274
		internal static Exception UDTUnexpectedResult(string exceptionText)
		{
			return ADP.TypeLoad(Res.GetString("SQLUDT_Unexpected", new object[] { exceptionText }));
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x00292E9C File Offset: 0x0029229C
		internal static Exception CannotCompleteDelegatedTransactionWithOpenResults()
		{
			return SqlException.CreateException(new SqlErrorCollection
			{
				new SqlError(-2, 0, 11, null, Res.GetString("ADP_OpenReaderExists"), "", 0)
			}, null);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x00292ED8 File Offset: 0x002922D8
		internal static TransactionPromotionException PromotionFailed(Exception inner)
		{
			TransactionPromotionException ex = new TransactionPromotionException(Res.GetString("SqlDelegatedTransaction_PromotionFailed"), inner);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x00292F00 File Offset: 0x00292300
		internal static Exception SqlCommandHasExistingSqlNotificationRequest()
		{
			return ADP.InvalidOperation(Res.GetString("SQLNotify_AlreadyHasCommand"));
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x00292F1C File Offset: 0x0029231C
		internal static Exception SqlDepCannotBeCreatedInProc()
		{
			return ADP.InvalidOperation(Res.GetString("SqlNotify_SqlDepCannotBeCreatedInProc"));
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x00292F38 File Offset: 0x00292338
		internal static Exception SqlDepDefaultOptionsButNoStart()
		{
			return ADP.InvalidOperation(Res.GetString("SqlDependency_DefaultOptionsButNoStart"));
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x00292F54 File Offset: 0x00292354
		internal static Exception SqlDependencyDatabaseBrokerDisabled()
		{
			return ADP.InvalidOperation(Res.GetString("SqlDependency_DatabaseBrokerDisabled"));
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x00292F70 File Offset: 0x00292370
		internal static Exception SqlDependencyEventNoDuplicate()
		{
			return ADP.InvalidOperation(Res.GetString("SqlDependency_EventNoDuplicate"));
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x00292F8C File Offset: 0x0029238C
		internal static Exception SqlDependencyDuplicateStart()
		{
			return ADP.InvalidOperation(Res.GetString("SqlDependency_DuplicateStart"));
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x00292FA8 File Offset: 0x002923A8
		internal static Exception SqlDependencyIdMismatch()
		{
			return ADP.InvalidOperation(Res.GetString("SqlDependency_IdMismatch"));
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x00292FC4 File Offset: 0x002923C4
		internal static Exception SqlDependencyNoMatchingServerStart()
		{
			return ADP.InvalidOperation(Res.GetString("SqlDependency_NoMatchingServerStart"));
		}

		// Token: 0x06002963 RID: 10595 RVA: 0x00292FE0 File Offset: 0x002923E0
		internal static Exception SqlDependencyNoMatchingServerDatabaseStart()
		{
			return ADP.InvalidOperation(Res.GetString("SqlDependency_NoMatchingServerDatabaseStart"));
		}

		// Token: 0x06002964 RID: 10596 RVA: 0x00292FFC File Offset: 0x002923FC
		internal static Exception SqlNotificationException(SqlNotificationEventArgs notify)
		{
			return ADP.InvalidOperation(Res.GetString("SQLNotify_ErrorFormat", new object[] { notify.Type, notify.Info, notify.Source }));
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x0029304C File Offset: 0x0029244C
		internal static Exception SqlMetaDataNoMetaData()
		{
			return ADP.InvalidOperation(Res.GetString("SqlMetaData_NoMetadata"));
		}

		// Token: 0x06002966 RID: 10598 RVA: 0x00293068 File Offset: 0x00292468
		internal static Exception MustSetUdtTypeNameForUdtParams()
		{
			return ADP.Argument(Res.GetString("SQLUDT_InvalidUdtTypeName"));
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x00293084 File Offset: 0x00292484
		internal static Exception UnexpectedUdtTypeNameForNonUdtParams()
		{
			return ADP.Argument(Res.GetString("SQLUDT_UnexpectedUdtTypeName"));
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x002930A0 File Offset: 0x002924A0
		internal static Exception UDTInvalidSqlType(string typeName)
		{
			return ADP.Argument(Res.GetString("SQLUDT_InvalidSqlType", new object[] { typeName }));
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x002930C8 File Offset: 0x002924C8
		internal static Exception InvalidSqlDbTypeForConstructor(SqlDbType type)
		{
			return ADP.Argument(Res.GetString("SqlMetaData_InvalidSqlDbTypeForConstructorFormat", new object[] { type.ToString() }));
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x002930FC File Offset: 0x002924FC
		internal static Exception NameTooLong(string parameterName)
		{
			return ADP.Argument(Res.GetString("SqlMetaData_NameTooLong"), parameterName);
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x0029311C File Offset: 0x0029251C
		internal static Exception InvalidSortOrder(SortOrder order)
		{
			return ADP.InvalidEnumerationValue(typeof(SortOrder), (int)order);
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x0029313C File Offset: 0x0029253C
		internal static Exception MustSpecifyBothSortOrderAndOrdinal(SortOrder order, int ordinal)
		{
			return ADP.InvalidOperation(Res.GetString("SqlMetaData_SpecifyBothSortOrderAndOrdinal", new object[]
			{
				order.ToString(),
				ordinal
			}));
		}

		// Token: 0x0600296D RID: 10605 RVA: 0x00293178 File Offset: 0x00292578
		internal static Exception TableTypeCanOnlyBeParameter()
		{
			return ADP.Argument(Res.GetString("SQLTVP_TableTypeCanOnlyBeParameter"));
		}

		// Token: 0x0600296E RID: 10606 RVA: 0x00293194 File Offset: 0x00292594
		internal static Exception UnsupportedColumnTypeForSqlProvider(string columnName, string typeName)
		{
			return ADP.Argument(Res.GetString("SqlProvider_InvalidDataColumnType", new object[] { columnName, typeName }));
		}

		// Token: 0x0600296F RID: 10607 RVA: 0x002931C0 File Offset: 0x002925C0
		internal static Exception InvalidColumnMaxLength(string columnName, long maxLength)
		{
			return ADP.Argument(Res.GetString("SqlProvider_InvalidDataColumnMaxLength", new object[] { columnName, maxLength }));
		}

		// Token: 0x06002970 RID: 10608 RVA: 0x002931F4 File Offset: 0x002925F4
		internal static Exception InvalidColumnPrecScale()
		{
			return ADP.Argument(Res.GetString("SqlMisc_InvalidPrecScaleMessage"));
		}

		// Token: 0x06002971 RID: 10609 RVA: 0x00293210 File Offset: 0x00292610
		internal static Exception NotEnoughColumnsInStructuredType()
		{
			return ADP.Argument(Res.GetString("SqlProvider_NotEnoughColumnsInStructuredType"));
		}

		// Token: 0x06002972 RID: 10610 RVA: 0x0029322C File Offset: 0x0029262C
		internal static Exception DuplicateSortOrdinal(int sortOrdinal)
		{
			return ADP.InvalidOperation(Res.GetString("SqlProvider_DuplicateSortOrdinal", new object[] { sortOrdinal }));
		}

		// Token: 0x06002973 RID: 10611 RVA: 0x0029325C File Offset: 0x0029265C
		internal static Exception MissingSortOrdinal(int sortOrdinal)
		{
			return ADP.InvalidOperation(Res.GetString("SqlProvider_MissingSortOrdinal", new object[] { sortOrdinal }));
		}

		// Token: 0x06002974 RID: 10612 RVA: 0x0029328C File Offset: 0x0029268C
		internal static Exception SortOrdinalGreaterThanFieldCount(int columnOrdinal, int sortOrdinal)
		{
			return ADP.InvalidOperation(Res.GetString("SqlProvider_SortOrdinalGreaterThanFieldCount", new object[] { sortOrdinal, columnOrdinal }));
		}

		// Token: 0x06002975 RID: 10613 RVA: 0x002932C4 File Offset: 0x002926C4
		internal static Exception IEnumerableOfSqlDataRecordHasNoRows()
		{
			return ADP.Argument(Res.GetString("IEnumerableOfSqlDataRecordHasNoRows"));
		}

		// Token: 0x06002976 RID: 10614 RVA: 0x002932E0 File Offset: 0x002926E0
		internal static Exception SqlPipeCommandHookedUpToNonContextConnection()
		{
			return ADP.InvalidOperation(Res.GetString("SqlPipe_CommandHookedUpToNonContextConnection"));
		}

		// Token: 0x06002977 RID: 10615 RVA: 0x002932FC File Offset: 0x002926FC
		internal static Exception SqlPipeMessageTooLong(int messageLength)
		{
			return ADP.Argument(Res.GetString("SqlPipe_MessageTooLong", new object[] { messageLength }));
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x0029332C File Offset: 0x0029272C
		internal static Exception SqlPipeIsBusy()
		{
			return ADP.InvalidOperation(Res.GetString("SqlPipe_IsBusy"));
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x00293348 File Offset: 0x00292748
		internal static Exception SqlPipeAlreadyHasAnOpenResultSet(string methodName)
		{
			return ADP.InvalidOperation(Res.GetString("SqlPipe_AlreadyHasAnOpenResultSet", new object[] { methodName }));
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x00293370 File Offset: 0x00292770
		internal static Exception SqlPipeDoesNotHaveAnOpenResultSet(string methodName)
		{
			return ADP.InvalidOperation(Res.GetString("SqlPipe_DoesNotHaveAnOpenResultSet", new object[] { methodName }));
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x00293398 File Offset: 0x00292798
		internal static Exception SqlResultSetClosed(string methodname)
		{
			if (methodname == null)
			{
				return ADP.InvalidOperation(Res.GetString("SQL_SqlResultSetClosed2"));
			}
			return ADP.InvalidOperation(Res.GetString("SQL_SqlResultSetClosed", new object[] { methodname }));
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x002933D4 File Offset: 0x002927D4
		internal static Exception SqlResultSetNoData(string methodname)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DataReaderNoData", new object[] { methodname }));
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x002933FC File Offset: 0x002927FC
		internal static Exception SqlRecordReadOnly(string methodname)
		{
			if (methodname == null)
			{
				return ADP.InvalidOperation(Res.GetString("SQL_SqlRecordReadOnly2"));
			}
			return ADP.InvalidOperation(Res.GetString("SQL_SqlRecordReadOnly", new object[] { methodname }));
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x00293438 File Offset: 0x00292838
		internal static Exception SqlResultSetRowDeleted(string methodname)
		{
			if (methodname == null)
			{
				return ADP.InvalidOperation(Res.GetString("SQL_SqlResultSetRowDeleted2"));
			}
			return ADP.InvalidOperation(Res.GetString("SQL_SqlResultSetRowDeleted", new object[] { methodname }));
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x00293474 File Offset: 0x00292874
		internal static Exception SqlResultSetCommandNotInSameConnection()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_SqlResultSetCommandNotInSameConnection"));
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x00293490 File Offset: 0x00292890
		internal static Exception SqlResultSetNoAcceptableCursor()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_SqlResultSetNoAcceptableCursor"));
		}

		// Token: 0x06002981 RID: 10625 RVA: 0x002934AC File Offset: 0x002928AC
		internal static Exception BulkLoadMappingInaccessible()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadMappingInaccessible"));
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x002934C8 File Offset: 0x002928C8
		internal static Exception BulkLoadMappingsNamesOrOrdinalsOnly()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadMappingsNamesOrOrdinalsOnly"));
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x002934E4 File Offset: 0x002928E4
		internal static Exception BulkLoadCannotConvertValue(Type sourcetype, MetaType metatype, Exception e)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadCannotConvertValue", new object[] { sourcetype.Name, metatype.TypeName }), e);
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x0029351C File Offset: 0x0029291C
		internal static Exception BulkLoadNonMatchingColumnMapping()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadNonMatchingColumnMapping"));
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x00293538 File Offset: 0x00292938
		internal static Exception BulkLoadNonMatchingColumnName(string columnName)
		{
			return SQL.BulkLoadNonMatchingColumnName(columnName, null);
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x0029354C File Offset: 0x0029294C
		internal static Exception BulkLoadNonMatchingColumnName(string columnName, Exception e)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadNonMatchingColumnName", new object[] { columnName }), e);
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x00293578 File Offset: 0x00292978
		internal static Exception BulkLoadStringTooLong()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadStringTooLong"));
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x00293594 File Offset: 0x00292994
		internal static Exception BulkLoadInvalidVariantValue()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadInvalidVariantValue"));
		}

		// Token: 0x06002989 RID: 10633 RVA: 0x002935B0 File Offset: 0x002929B0
		internal static Exception BulkLoadInvalidTimeout(int timeout)
		{
			return ADP.Argument(Res.GetString("SQL_BulkLoadInvalidTimeout", new object[] { timeout.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x002935E4 File Offset: 0x002929E4
		internal static Exception BulkLoadExistingTransaction()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadExistingTransaction"));
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x00293600 File Offset: 0x00292A00
		internal static Exception BulkLoadNoCollation()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadNoCollation"));
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x0029361C File Offset: 0x00292A1C
		internal static Exception BulkLoadConflictingTransactionOption()
		{
			return ADP.Argument(Res.GetString("SQL_BulkLoadConflictingTransactionOption"));
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x00293638 File Offset: 0x00292A38
		internal static Exception BulkLoadLcidMismatch(int sourceLcid, string sourceColumnName, int destinationLcid, string destinationColumnName)
		{
			return ADP.InvalidOperation(Res.GetString("Sql_BulkLoadLcidMismatch", new object[] { sourceLcid, sourceColumnName, destinationLcid, destinationColumnName }));
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x00293678 File Offset: 0x00292A78
		internal static Exception InvalidOperationInsideEvent()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadInvalidOperationInsideEvent"));
		}

		// Token: 0x0600298F RID: 10639 RVA: 0x00293694 File Offset: 0x00292A94
		internal static Exception BulkLoadMissingDestinationTable()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadMissingDestinationTable"));
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x002936B0 File Offset: 0x00292AB0
		internal static Exception BulkLoadInvalidDestinationTable(string tableName, Exception inner)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadInvalidDestinationTable", new object[] { tableName }), inner);
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x002936DC File Offset: 0x00292ADC
		internal static Exception BulkLoadBulkLoadNotAllowDBNull(string columnName)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BulkLoadNotAllowDBNull", new object[] { columnName }));
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x00293704 File Offset: 0x00292B04
		internal static Exception ConnectionDoomed()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ConnectionDoomed"));
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x00293720 File Offset: 0x00292B20
		internal static Exception MultiSubnetFailoverWithFailoverPartner(bool serverProvidedFailoverPartner)
		{
			string @string = Res.GetString("SQLMSF_FailoverPartnerNotSupported");
			if (serverProvidedFailoverPartner)
			{
				return ADP.InvalidOperation(@string);
			}
			return ADP.Argument(@string);
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x00293748 File Offset: 0x00292B48
		internal static Exception MultiSubnetFailoverWithMoreThan64IPs()
		{
			string snierrorMessage = SQL.GetSNIErrorMessage(47);
			return ADP.InvalidOperation(snierrorMessage);
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x00293764 File Offset: 0x00292B64
		internal static Exception MultiSubnetFailoverWithInstanceSpecified()
		{
			string snierrorMessage = SQL.GetSNIErrorMessage(48);
			return ADP.Argument(snierrorMessage);
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x00293780 File Offset: 0x00292B80
		internal static Exception MultiSubnetFailoverWithNonTcpProtocol()
		{
			string snierrorMessage = SQL.GetSNIErrorMessage(49);
			return ADP.Argument(snierrorMessage);
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x0029379C File Offset: 0x00292B9C
		internal static Exception ROR_FailoverNotSupportedConnString()
		{
			return ADP.Argument(Res.GetString("SQLROR_FailoverNotSupported"));
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x002937B8 File Offset: 0x00292BB8
		internal static Exception ROR_FailoverNotSupportedServer()
		{
			SqlException ex = SqlException.CreateException(new SqlErrorCollection
			{
				new SqlError(0, 0, 20, null, Res.GetString("SQLROR_FailoverNotSupported"), "", 0)
			}, null);
			ex._doNotReconnect = true;
			return ex;
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x002937FC File Offset: 0x00292BFC
		internal static Exception ROR_RecursiveRoutingNotSupported()
		{
			SqlException ex = SqlException.CreateException(new SqlErrorCollection
			{
				new SqlError(0, 0, 20, null, Res.GetString("SQLROR_RecursiveRoutingNotSupported"), "", 0)
			}, null);
			ex._doNotReconnect = true;
			return ex;
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x00293840 File Offset: 0x00292C40
		internal static Exception ROR_UnexpectedRoutingInfo()
		{
			SqlException ex = SqlException.CreateException(new SqlErrorCollection
			{
				new SqlError(0, 0, 20, null, Res.GetString("SQLROR_UnexpectedRoutingInfo"), "", 0)
			}, null);
			ex._doNotReconnect = true;
			return ex;
		}

		// Token: 0x0600299B RID: 10651 RVA: 0x00293884 File Offset: 0x00292C84
		internal static Exception ROR_InvalidRoutingInfo()
		{
			SqlException ex = SqlException.CreateException(new SqlErrorCollection
			{
				new SqlError(0, 0, 20, null, Res.GetString("SQLROR_InvalidRoutingInfo"), "", 0)
			}, null);
			ex._doNotReconnect = true;
			return ex;
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x002938C8 File Offset: 0x00292CC8
		internal static Exception ROR_TimeoutAfterRoutingInfo()
		{
			SqlException ex = SqlException.CreateException(new SqlErrorCollection
			{
				new SqlError(0, 0, 20, null, Res.GetString("SQLROR_TimeoutAfterRoutingInfo"), "", 0)
			}, null);
			ex._doNotReconnect = true;
			return ex;
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x0029390C File Offset: 0x00292D0C
		internal static Exception BatchedUpdatesNotAvailableOnContextConnection()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_BatchedUpdatesNotAvailableOnContextConnection"));
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x00293928 File Offset: 0x00292D28
		internal static Exception ContextAllowsLimitedKeywords()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ContextAllowsLimitedKeywords"));
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x00293944 File Offset: 0x00292D44
		internal static Exception ContextAllowsOnlyTypeSystem2005()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ContextAllowsOnlyTypeSystem2005"));
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x00293960 File Offset: 0x00292D60
		internal static Exception ContextConnectionIsInUse()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ContextConnectionIsInUse"));
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x0029397C File Offset: 0x00292D7C
		internal static Exception ContextUnavailableOutOfProc()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ContextUnavailableOutOfProc"));
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x00293998 File Offset: 0x00292D98
		internal static Exception ContextUnavailableWhileInProc()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_ContextUnavailableWhileInProc"));
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x002939B4 File Offset: 0x00292DB4
		internal static Exception NestedTransactionScopesNotSupported()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_NestedTransactionScopesNotSupported"));
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x002939D0 File Offset: 0x00292DD0
		internal static Exception NotAvailableOnContextConnection()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_NotAvailableOnContextConnection"));
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x002939EC File Offset: 0x00292DEC
		internal static Exception NotificationsNotAvailableOnContextConnection()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_NotificationsNotAvailableOnContextConnection"));
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x00293A08 File Offset: 0x00292E08
		internal static Exception UnexpectedSmiEvent(SmiEventSink_Default.UnexpectedEventType eventType)
		{
			return ADP.InvalidOperation(Res.GetString("SQL_UnexpectedSmiEvent", new object[] { (int)eventType }));
		}

		// Token: 0x060029A7 RID: 10663 RVA: 0x00293A38 File Offset: 0x00292E38
		internal static Exception UserInstanceNotAvailableInProc()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_UserInstanceNotAvailableInProc"));
		}

		// Token: 0x060029A8 RID: 10664 RVA: 0x00293A54 File Offset: 0x00292E54
		internal static Exception ArgumentLengthMismatch(string arg1, string arg2)
		{
			return ADP.Argument(Res.GetString("SQL_ArgumentLengthMismatch", new object[] { arg1, arg2 }));
		}

		// Token: 0x060029A9 RID: 10665 RVA: 0x00293A80 File Offset: 0x00292E80
		internal static Exception InvalidSqlDbTypeOneAllowedType(SqlDbType invalidType, string method, SqlDbType allowedType)
		{
			return ADP.Argument(Res.GetString("SQL_InvalidSqlDbTypeWithOneAllowedType", new object[] { invalidType, method, allowedType }));
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x00293ABC File Offset: 0x00292EBC
		internal static Exception SqlPipeErrorRequiresSendEnd()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_PipeErrorRequiresSendEnd"));
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x00293AD8 File Offset: 0x00292ED8
		internal static Exception TooManyValues(string arg)
		{
			return ADP.Argument(Res.GetString("SQL_TooManyValues"), arg);
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x00293AF8 File Offset: 0x00292EF8
		internal static Exception StreamWriteNotSupported()
		{
			return ADP.NotSupported(Res.GetString("SQL_StreamWriteNotSupported"));
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x00293B14 File Offset: 0x00292F14
		internal static Exception StreamReadNotSupported()
		{
			return ADP.NotSupported(Res.GetString("SQL_StreamReadNotSupported"));
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x00293B30 File Offset: 0x00292F30
		internal static Exception StreamSeekNotSupported()
		{
			return ADP.NotSupported(Res.GetString("SQL_StreamSeekNotSupported"));
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x00293B4C File Offset: 0x00292F4C
		internal static SqlNullValueException SqlNullValue()
		{
			SqlNullValueException ex = new SqlNullValueException();
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x060029B0 RID: 10672 RVA: 0x00293B68 File Offset: 0x00292F68
		internal static Exception ParameterSizeRestrictionFailure(int index)
		{
			return ADP.InvalidOperation(Res.GetString("OleDb_CommandParameterError", new object[]
			{
				index.ToString(CultureInfo.InvariantCulture),
				"SqlParameter.Size"
			}));
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x00293BA4 File Offset: 0x00292FA4
		internal static Exception SubclassMustOverride()
		{
			return ADP.InvalidOperation(Res.GetString("SqlMisc_SubclassMustOverride"));
		}

		// Token: 0x060029B2 RID: 10674 RVA: 0x00293BC0 File Offset: 0x00292FC0
		internal static string GetSNIErrorMessage(int sniError)
		{
			string text = string.Format(null, "SNI_ERROR_{0}", new object[] { sniError });
			return Res.GetString(text);
		}

		// Token: 0x040019A9 RID: 6569
		internal const string WriteToServer = "WriteToServer";

		// Token: 0x040019AA RID: 6570
		internal const int SqlDependencyTimeoutDefault = 0;

		// Token: 0x040019AB RID: 6571
		internal const int SqlDependencyServerTimeout = 432000;

		// Token: 0x040019AC RID: 6572
		internal const string SqlNotificationServiceDefault = "SqlQueryNotificationService";

		// Token: 0x040019AD RID: 6573
		internal const string SqlNotificationStoredProcedureDefault = "SqlQueryNotificationStoredProcedure";

		// Token: 0x040019AE RID: 6574
		internal const string Transaction = "Transaction";

		// Token: 0x040019AF RID: 6575
		internal const string Connection = "Connection";

		// Token: 0x040019B0 RID: 6576
		internal static readonly byte[] AttentionHeader = new byte[] { 6, 1, 0, 8, 0, 0, 0, 0 };
	}
}
