using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Transactions;
using System.Xml;
using Microsoft.SqlServer.Server;
using Microsoft.Win32;

namespace System.Data.Common
{
	// Token: 0x02000106 RID: 262
	internal static class ADP
	{
		// Token: 0x06000F5B RID: 3931 RVA: 0x00215D74 File Offset: 0x00215174
		private static void TraceException(string trace, Exception e)
		{
			if (e != null)
			{
				Bid.Trace(trace, e.ToString());
			}
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x00215D90 File Offset: 0x00215190
		internal static void TraceExceptionAsReturnValue(Exception e)
		{
			ADP.TraceException("<comm.ADP.TraceException|ERR|THROW> '%ls'\n", e);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x00215DA8 File Offset: 0x002151A8
		internal static void TraceExceptionForCapture(Exception e)
		{
			ADP.TraceException("<comm.ADP.TraceException|ERR|CATCH> '%ls'\n", e);
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x00215DC0 File Offset: 0x002151C0
		internal static void TraceExceptionWithoutRethrow(Exception e)
		{
			ADP.TraceException("<comm.ADP.TraceException|ERR|CATCH> '%ls'\n", e);
		}

		// Token: 0x06000F5F RID: 3935 RVA: 0x00215DD8 File Offset: 0x002151D8
		internal static ArgumentException Argument(string error)
		{
			ArgumentException ex = new ArgumentException(error);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F60 RID: 3936 RVA: 0x00215DF4 File Offset: 0x002151F4
		internal static ArgumentException Argument(string error, Exception inner)
		{
			ArgumentException ex = new ArgumentException(error, inner);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F61 RID: 3937 RVA: 0x00215E10 File Offset: 0x00215210
		internal static ArgumentException Argument(string error, string parameter)
		{
			ArgumentException ex = new ArgumentException(error, parameter);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F62 RID: 3938 RVA: 0x00215E2C File Offset: 0x0021522C
		internal static ArgumentException Argument(string error, string parameter, Exception inner)
		{
			ArgumentException ex = new ArgumentException(error, parameter, inner);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F63 RID: 3939 RVA: 0x00215E4C File Offset: 0x0021524C
		internal static ArgumentNullException ArgumentNull(string parameter)
		{
			ArgumentNullException ex = new ArgumentNullException(parameter);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F64 RID: 3940 RVA: 0x00215E68 File Offset: 0x00215268
		internal static ArgumentNullException ArgumentNull(string parameter, string error)
		{
			ArgumentNullException ex = new ArgumentNullException(parameter, error);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F65 RID: 3941 RVA: 0x00215E84 File Offset: 0x00215284
		internal static ArgumentOutOfRangeException ArgumentOutOfRange(string parameterName)
		{
			ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(parameterName);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F66 RID: 3942 RVA: 0x00215EA0 File Offset: 0x002152A0
		internal static ArgumentOutOfRangeException ArgumentOutOfRange(string message, string parameterName)
		{
			ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(parameterName, message);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F67 RID: 3943 RVA: 0x00215EBC File Offset: 0x002152BC
		internal static ArgumentOutOfRangeException ArgumentOutOfRange(string message, string parameterName, object value)
		{
			ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(parameterName, value, message);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F68 RID: 3944 RVA: 0x00215EDC File Offset: 0x002152DC
		internal static ConfigurationException Configuration(string message)
		{
			ConfigurationException ex = new ConfigurationErrorsException(message);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F69 RID: 3945 RVA: 0x00215EF8 File Offset: 0x002152F8
		internal static ConfigurationException Configuration(string message, XmlNode node)
		{
			ConfigurationException ex = new ConfigurationErrorsException(message, node);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F6A RID: 3946 RVA: 0x00215F14 File Offset: 0x00215314
		internal static DataException Data(string message)
		{
			DataException ex = new DataException(message);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F6B RID: 3947 RVA: 0x00215F30 File Offset: 0x00215330
		internal static IndexOutOfRangeException IndexOutOfRange(int value)
		{
			IndexOutOfRangeException ex = new IndexOutOfRangeException(value.ToString(CultureInfo.InvariantCulture));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x00215F58 File Offset: 0x00215358
		internal static IndexOutOfRangeException IndexOutOfRange(string error)
		{
			IndexOutOfRangeException ex = new IndexOutOfRangeException(error);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x00215F74 File Offset: 0x00215374
		internal static InvalidCastException InvalidCast(string error)
		{
			return ADP.InvalidCast(error, null);
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x00215F88 File Offset: 0x00215388
		internal static InvalidCastException InvalidCast(string error, Exception inner)
		{
			InvalidCastException ex = new InvalidCastException(error, inner);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x00215FA4 File Offset: 0x002153A4
		internal static InvalidOperationException InvalidOperation(string error)
		{
			InvalidOperationException ex = new InvalidOperationException(error);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00215FC0 File Offset: 0x002153C0
		internal static InvalidOperationException InvalidOperation(string error, Exception inner)
		{
			InvalidOperationException ex = new InvalidOperationException(error, inner);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x00215FDC File Offset: 0x002153DC
		internal static NotImplementedException NotImplemented(string error)
		{
			NotImplementedException ex = new NotImplementedException(error);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00215FF8 File Offset: 0x002153F8
		internal static NotSupportedException NotSupported()
		{
			NotSupportedException ex = new NotSupportedException();
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00216014 File Offset: 0x00215414
		internal static NotSupportedException NotSupported(string error)
		{
			NotSupportedException ex = new NotSupportedException(error);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x00216030 File Offset: 0x00215430
		internal static OverflowException Overflow(string error)
		{
			return ADP.Overflow(error, null);
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x00216044 File Offset: 0x00215444
		internal static OverflowException Overflow(string error, Exception inner)
		{
			OverflowException ex = new OverflowException(error, inner);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00216060 File Offset: 0x00215460
		internal static PlatformNotSupportedException PropertyNotSupported(string property)
		{
			PlatformNotSupportedException ex = new PlatformNotSupportedException(Res.GetString("ADP_PropertyNotSupported", new object[] { property }));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00216090 File Offset: 0x00215490
		internal static TypeLoadException TypeLoad(string error)
		{
			TypeLoadException ex = new TypeLoadException(error);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x002160AC File Offset: 0x002154AC
		internal static InvalidCastException InvalidCast()
		{
			InvalidCastException ex = new InvalidCastException();
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F79 RID: 3961 RVA: 0x002160C8 File Offset: 0x002154C8
		internal static InvalidOperationException DataAdapter(string error)
		{
			return ADP.InvalidOperation(error);
		}

		// Token: 0x06000F7A RID: 3962 RVA: 0x002160DC File Offset: 0x002154DC
		internal static InvalidOperationException DataAdapter(string error, Exception inner)
		{
			return ADP.InvalidOperation(error, inner);
		}

		// Token: 0x06000F7B RID: 3963 RVA: 0x002160F0 File Offset: 0x002154F0
		private static InvalidOperationException Provider(string error)
		{
			return ADP.InvalidOperation(error);
		}

		// Token: 0x06000F7C RID: 3964 RVA: 0x00216104 File Offset: 0x00215504
		internal static ObjectDisposedException ObjectDisposed(object instance)
		{
			ObjectDisposedException ex = new ObjectDisposedException(instance.GetType().Name);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F7D RID: 3965 RVA: 0x0021612C File Offset: 0x0021552C
		internal static InvalidOperationException MethodCalledTwice(string method)
		{
			InvalidOperationException ex = new InvalidOperationException(Res.GetString("ADP_CalledTwice", new object[] { method }));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F7E RID: 3966 RVA: 0x0021615C File Offset: 0x0021555C
		internal static ArgumentException IncorrectAsyncResult()
		{
			ArgumentException ex = new ArgumentException(Res.GetString("ADP_IncorrectAsyncResult"), "AsyncResult");
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00216188 File Offset: 0x00215588
		internal static ArgumentException SingleValuedProperty(string propertyName, string value)
		{
			ArgumentException ex = new ArgumentException(Res.GetString("ADP_SingleValuedProperty", new object[] { propertyName, value }));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x002161BC File Offset: 0x002155BC
		internal static ArgumentException DoubleValuedProperty(string propertyName, string value1, string value2)
		{
			ArgumentException ex = new ArgumentException(Res.GetString("ADP_DoubleValuedProperty", new object[] { propertyName, value1, value2 }));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F81 RID: 3969 RVA: 0x002161F4 File Offset: 0x002155F4
		internal static ArgumentException InvalidPrefixSuffix()
		{
			ArgumentException ex = new ArgumentException(Res.GetString("ADP_InvalidPrefixSuffix"));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F82 RID: 3970 RVA: 0x00216218 File Offset: 0x00215618
		internal static ArgumentException InvalidMultipartName(string property, string value)
		{
			ArgumentException ex = new ArgumentException(Res.GetString("ADP_InvalidMultipartName", new object[]
			{
				Res.GetString(property),
				value
			}));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F83 RID: 3971 RVA: 0x00216254 File Offset: 0x00215654
		internal static ArgumentException InvalidMultipartNameIncorrectUsageOfQuotes(string property, string value)
		{
			ArgumentException ex = new ArgumentException(Res.GetString("ADP_InvalidMultipartNameQuoteUsage", new object[]
			{
				Res.GetString(property),
				value
			}));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F84 RID: 3972 RVA: 0x00216290 File Offset: 0x00215690
		internal static ArgumentException InvalidMultipartNameToManyParts(string property, string value, int limit)
		{
			ArgumentException ex = new ArgumentException(Res.GetString("ADP_InvalidMultipartNameToManyParts", new object[]
			{
				Res.GetString(property),
				value,
				limit
			}));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F85 RID: 3973 RVA: 0x002162D4 File Offset: 0x002156D4
		internal static ArgumentException BadParameterName(string parameterName)
		{
			ArgumentException ex = new ArgumentException(Res.GetString("ADP_BadParameterName", new object[] { parameterName }));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F86 RID: 3974 RVA: 0x00216304 File Offset: 0x00215704
		internal static ArgumentException MultipleReturnValue()
		{
			ArgumentException ex = new ArgumentException(Res.GetString("ADP_MultipleReturnValue"));
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000F87 RID: 3975 RVA: 0x00216328 File Offset: 0x00215728
		internal static void CheckArgumentLength(string value, string parameterName)
		{
			ADP.CheckArgumentNull(value, parameterName);
			if (value.Length == 0)
			{
				throw ADP.Argument(Res.GetString("ADP_EmptyString", new object[] { parameterName }));
			}
		}

		// Token: 0x06000F88 RID: 3976 RVA: 0x00216360 File Offset: 0x00215760
		internal static void CheckArgumentLength(Array value, string parameterName)
		{
			ADP.CheckArgumentNull(value, parameterName);
			if (value.Length == 0)
			{
				throw ADP.Argument(Res.GetString("ADP_EmptyArray", new object[] { parameterName }));
			}
		}

		// Token: 0x06000F89 RID: 3977 RVA: 0x00216398 File Offset: 0x00215798
		internal static void CheckArgumentNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw ADP.ArgumentNull(parameterName);
			}
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x002163B0 File Offset: 0x002157B0
		internal static bool IsCatchableExceptionType(Exception e)
		{
			Type type = e.GetType();
			return type != ADP.StackOverflowType && type != ADP.OutOfMemoryType && type != ADP.ThreadAbortType && type != ADP.NullReferenceType && type != ADP.AccessViolationType && !ADP.SecurityType.IsAssignableFrom(type);
		}

		// Token: 0x06000F8B RID: 3979 RVA: 0x002163FC File Offset: 0x002157FC
		internal static bool IsCatchableOrSecurityExceptionType(Exception e)
		{
			Type type = e.GetType();
			return type != ADP.StackOverflowType && type != ADP.OutOfMemoryType && type != ADP.ThreadAbortType && type != ADP.NullReferenceType && type != ADP.AccessViolationType;
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x00216440 File Offset: 0x00215840
		internal static ArgumentOutOfRangeException InvalidEnumerationValue(Type type, int value)
		{
			return ADP.ArgumentOutOfRange(Res.GetString("ADP_InvalidEnumerationValue", new object[]
			{
				type.Name,
				value.ToString(CultureInfo.InvariantCulture)
			}), type.Name);
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x00216484 File Offset: 0x00215884
		internal static ArgumentOutOfRangeException NotSupportedEnumerationValue(Type type, string value, string method)
		{
			return ADP.ArgumentOutOfRange(Res.GetString("ADP_NotSupportedEnumerationValue", new object[] { type.Name, value, method }), type.Name);
		}

		// Token: 0x06000F8E RID: 3982 RVA: 0x002164C0 File Offset: 0x002158C0
		internal static ArgumentOutOfRangeException InvalidAcceptRejectRule(AcceptRejectRule value)
		{
			return ADP.InvalidEnumerationValue(typeof(AcceptRejectRule), (int)value);
		}

		// Token: 0x06000F8F RID: 3983 RVA: 0x002164E0 File Offset: 0x002158E0
		internal static ArgumentOutOfRangeException InvalidCatalogLocation(CatalogLocation value)
		{
			return ADP.InvalidEnumerationValue(typeof(CatalogLocation), (int)value);
		}

		// Token: 0x06000F90 RID: 3984 RVA: 0x00216500 File Offset: 0x00215900
		internal static ArgumentOutOfRangeException InvalidCommandBehavior(CommandBehavior value)
		{
			return ADP.InvalidEnumerationValue(typeof(CommandBehavior), (int)value);
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x00216520 File Offset: 0x00215920
		internal static void ValidateCommandBehavior(CommandBehavior value)
		{
			if (value < CommandBehavior.Default || (CommandBehavior.SingleResult | CommandBehavior.SchemaOnly | CommandBehavior.KeyInfo | CommandBehavior.SingleRow | CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection) < value)
			{
				throw ADP.InvalidCommandBehavior(value);
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x00216540 File Offset: 0x00215940
		internal static ArgumentException InvalidArgumentLength(string argumentName, int limit)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidArgumentLength", new object[] { argumentName, limit }));
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00216574 File Offset: 0x00215974
		internal static ArgumentOutOfRangeException InvalidCommandType(CommandType value)
		{
			return ADP.InvalidEnumerationValue(typeof(CommandType), (int)value);
		}

		// Token: 0x06000F94 RID: 3988 RVA: 0x00216594 File Offset: 0x00215994
		internal static ArgumentOutOfRangeException InvalidConflictOptions(ConflictOption value)
		{
			return ADP.InvalidEnumerationValue(typeof(ConflictOption), (int)value);
		}

		// Token: 0x06000F95 RID: 3989 RVA: 0x002165B4 File Offset: 0x002159B4
		internal static ArgumentOutOfRangeException InvalidDataRowState(DataRowState value)
		{
			return ADP.InvalidEnumerationValue(typeof(DataRowState), (int)value);
		}

		// Token: 0x06000F96 RID: 3990 RVA: 0x002165D4 File Offset: 0x002159D4
		internal static ArgumentOutOfRangeException InvalidDataRowVersion(DataRowVersion value)
		{
			return ADP.InvalidEnumerationValue(typeof(DataRowVersion), (int)value);
		}

		// Token: 0x06000F97 RID: 3991 RVA: 0x002165F4 File Offset: 0x002159F4
		internal static ArgumentOutOfRangeException InvalidIsolationLevel(IsolationLevel value)
		{
			return ADP.InvalidEnumerationValue(typeof(IsolationLevel), (int)value);
		}

		// Token: 0x06000F98 RID: 3992 RVA: 0x00216614 File Offset: 0x00215A14
		internal static ArgumentOutOfRangeException InvalidKeyRestrictionBehavior(KeyRestrictionBehavior value)
		{
			return ADP.InvalidEnumerationValue(typeof(KeyRestrictionBehavior), (int)value);
		}

		// Token: 0x06000F99 RID: 3993 RVA: 0x00216634 File Offset: 0x00215A34
		internal static ArgumentOutOfRangeException InvalidLoadOption(LoadOption value)
		{
			return ADP.InvalidEnumerationValue(typeof(LoadOption), (int)value);
		}

		// Token: 0x06000F9A RID: 3994 RVA: 0x00216654 File Offset: 0x00215A54
		internal static ArgumentOutOfRangeException InvalidMissingMappingAction(MissingMappingAction value)
		{
			return ADP.InvalidEnumerationValue(typeof(MissingMappingAction), (int)value);
		}

		// Token: 0x06000F9B RID: 3995 RVA: 0x00216674 File Offset: 0x00215A74
		internal static ArgumentOutOfRangeException InvalidMissingSchemaAction(MissingSchemaAction value)
		{
			return ADP.InvalidEnumerationValue(typeof(MissingSchemaAction), (int)value);
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00216694 File Offset: 0x00215A94
		internal static ArgumentOutOfRangeException InvalidParameterDirection(ParameterDirection value)
		{
			return ADP.InvalidEnumerationValue(typeof(ParameterDirection), (int)value);
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x002166B4 File Offset: 0x00215AB4
		internal static ArgumentOutOfRangeException InvalidPermissionState(PermissionState value)
		{
			return ADP.InvalidEnumerationValue(typeof(PermissionState), (int)value);
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x002166D4 File Offset: 0x00215AD4
		internal static ArgumentOutOfRangeException InvalidRule(Rule value)
		{
			return ADP.InvalidEnumerationValue(typeof(Rule), (int)value);
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x002166F4 File Offset: 0x00215AF4
		internal static ArgumentOutOfRangeException InvalidSchemaType(SchemaType value)
		{
			return ADP.InvalidEnumerationValue(typeof(SchemaType), (int)value);
		}

		// Token: 0x06000FA0 RID: 4000 RVA: 0x00216714 File Offset: 0x00215B14
		internal static ArgumentOutOfRangeException InvalidStatementType(StatementType value)
		{
			return ADP.InvalidEnumerationValue(typeof(StatementType), (int)value);
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x00216734 File Offset: 0x00215B34
		internal static ArgumentOutOfRangeException InvalidUpdateRowSource(UpdateRowSource value)
		{
			return ADP.InvalidEnumerationValue(typeof(UpdateRowSource), (int)value);
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00216754 File Offset: 0x00215B54
		internal static ArgumentOutOfRangeException InvalidUpdateStatus(UpdateStatus value)
		{
			return ADP.InvalidEnumerationValue(typeof(UpdateStatus), (int)value);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x00216774 File Offset: 0x00215B74
		internal static ArgumentOutOfRangeException NotSupportedCommandBehavior(CommandBehavior value, string method)
		{
			return ADP.NotSupportedEnumerationValue(typeof(CommandBehavior), value.ToString(), method);
		}

		// Token: 0x06000FA4 RID: 4004 RVA: 0x0021679C File Offset: 0x00215B9C
		internal static ArgumentOutOfRangeException NotSupportedStatementType(StatementType value, string method)
		{
			return ADP.NotSupportedEnumerationValue(typeof(StatementType), value.ToString(), method);
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x002167C4 File Offset: 0x00215BC4
		internal static ArgumentOutOfRangeException InvalidUserDefinedTypeSerializationFormat(Format value)
		{
			return ADP.InvalidEnumerationValue(typeof(Format), (int)value);
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x002167E4 File Offset: 0x00215BE4
		internal static ArgumentOutOfRangeException NotSupportedUserDefinedTypeSerializationFormat(Format value, string method)
		{
			return ADP.NotSupportedEnumerationValue(typeof(Format), value.ToString(), method);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x0021680C File Offset: 0x00215C0C
		internal static ArgumentException ConfigProviderNotFound()
		{
			return ADP.Argument(Res.GetString("ConfigProviderNotFound"));
		}

		// Token: 0x06000FA8 RID: 4008 RVA: 0x00216828 File Offset: 0x00215C28
		internal static InvalidOperationException ConfigProviderInvalid()
		{
			return ADP.InvalidOperation(Res.GetString("ConfigProviderInvalid"));
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x00216844 File Offset: 0x00215C44
		internal static ConfigurationException ConfigProviderNotInstalled()
		{
			return ADP.Configuration(Res.GetString("ConfigProviderNotInstalled"));
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00216860 File Offset: 0x00215C60
		internal static ConfigurationException ConfigProviderMissing()
		{
			return ADP.Configuration(Res.GetString("ConfigProviderMissing"));
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0021687C File Offset: 0x00215C7C
		internal static ConfigurationException ConfigBaseNoChildNodes(XmlNode node)
		{
			return ADP.Configuration(Res.GetString("ConfigBaseNoChildNodes"), node);
		}

		// Token: 0x06000FAC RID: 4012 RVA: 0x0021689C File Offset: 0x00215C9C
		internal static ConfigurationException ConfigBaseElementsOnly(XmlNode node)
		{
			return ADP.Configuration(Res.GetString("ConfigBaseElementsOnly"), node);
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x002168BC File Offset: 0x00215CBC
		internal static ConfigurationException ConfigUnrecognizedAttributes(XmlNode node)
		{
			return ADP.Configuration(Res.GetString("ConfigUnrecognizedAttributes", new object[] { node.Attributes[0].Name }), node);
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x002168F8 File Offset: 0x00215CF8
		internal static ConfigurationException ConfigUnrecognizedElement(XmlNode node)
		{
			return ADP.Configuration(Res.GetString("ConfigUnrecognizedElement"), node);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x00216918 File Offset: 0x00215D18
		internal static ConfigurationException ConfigSectionsUnique(string sectionName)
		{
			return ADP.Configuration(Res.GetString("ConfigSectionsUnique", new object[] { sectionName }));
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00216940 File Offset: 0x00215D40
		internal static ConfigurationException ConfigRequiredAttributeMissing(string name, XmlNode node)
		{
			return ADP.Configuration(Res.GetString("ConfigRequiredAttributeMissing", new object[] { name }), node);
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0021696C File Offset: 0x00215D6C
		internal static ConfigurationException ConfigRequiredAttributeEmpty(string name, XmlNode node)
		{
			return ADP.Configuration(Res.GetString("ConfigRequiredAttributeEmpty", new object[] { name }), node);
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x00216998 File Offset: 0x00215D98
		internal static ArgumentException ConnectionStringSyntax(int index)
		{
			return ADP.Argument(Res.GetString("ADP_ConnectionStringSyntax", new object[] { index }));
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x002169C8 File Offset: 0x00215DC8
		internal static ArgumentException KeywordNotSupported(string keyword)
		{
			return ADP.Argument(Res.GetString("ADP_KeywordNotSupported", new object[] { keyword }));
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x002169F0 File Offset: 0x00215DF0
		internal static ArgumentException UdlFileError(Exception inner)
		{
			return ADP.Argument(Res.GetString("ADP_UdlFileError"), inner);
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x00216A10 File Offset: 0x00215E10
		internal static ArgumentException InvalidUDL()
		{
			return ADP.Argument(Res.GetString("ADP_InvalidUDL"));
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00216A2C File Offset: 0x00215E2C
		internal static InvalidOperationException InvalidDataDirectory()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InvalidDataDirectory"));
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00216A48 File Offset: 0x00215E48
		internal static ArgumentException InvalidKeyname(string parameterName)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidKey"), parameterName);
		}

		// Token: 0x06000FB8 RID: 4024 RVA: 0x00216A68 File Offset: 0x00215E68
		internal static ArgumentException InvalidValue(string parameterName)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidValue"), parameterName);
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x00216A88 File Offset: 0x00215E88
		internal static ArgumentException InvalidMinMaxPoolSizeValues()
		{
			return ADP.Argument(Res.GetString("ADP_InvalidMinMaxPoolSizeValues"));
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00216AA4 File Offset: 0x00215EA4
		internal static ArgumentException ConvertFailed(Type fromType, Type toType, Exception innerException)
		{
			return ADP.Argument(Res.GetString("SqlConvert_ConvertFailed", new object[] { fromType.FullName, toType.FullName }), innerException);
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00216ADC File Offset: 0x00215EDC
		internal static InvalidOperationException NoConnectionString()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NoConnectionString"));
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00216AF8 File Offset: 0x00215EF8
		internal static NotImplementedException MethodNotImplemented(string methodName)
		{
			NotImplementedException ex = new NotImplementedException(methodName);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00216B14 File Offset: 0x00215F14
		private static string ConnectionStateMsg(ConnectionState state)
		{
			switch (state)
			{
			case ConnectionState.Closed:
				break;
			case ConnectionState.Open:
				return Res.GetString("ADP_ConnectionStateMsg_Open");
			case ConnectionState.Connecting:
				return Res.GetString("ADP_ConnectionStateMsg_Connecting");
			case ConnectionState.Open | ConnectionState.Connecting:
			case ConnectionState.Executing:
				goto IL_0061;
			case ConnectionState.Open | ConnectionState.Executing:
				return Res.GetString("ADP_ConnectionStateMsg_OpenExecuting");
			default:
				if (state == (ConnectionState.Open | ConnectionState.Fetching))
				{
					return Res.GetString("ADP_ConnectionStateMsg_OpenFetching");
				}
				if (state != (ConnectionState.Connecting | ConnectionState.Broken))
				{
					goto IL_0061;
				}
				break;
			}
			return Res.GetString("ADP_ConnectionStateMsg_Closed");
			IL_0061:
			return Res.GetString("ADP_ConnectionStateMsg", new object[] { state.ToString() });
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00216BA4 File Offset: 0x00215FA4
		internal static ConfigurationException ConfigUnableToLoadXmlMetaDataFile(string settingName)
		{
			return ADP.Configuration(Res.GetString("OleDb_ConfigUnableToLoadXmlMetaDataFile", new object[] { settingName }));
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x00216BCC File Offset: 0x00215FCC
		internal static ConfigurationException ConfigWrongNumberOfValues(string settingName)
		{
			return ADP.Configuration(Res.GetString("OleDb_ConfigWrongNumberOfValues", new object[] { settingName }));
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00216BF4 File Offset: 0x00215FF4
		internal static Exception InvalidConnectionOptionValue(string key)
		{
			return ADP.InvalidConnectionOptionValue(key, null);
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00216C08 File Offset: 0x00216008
		internal static Exception InvalidConnectionOptionValueLength(string key, int limit)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidConnectionOptionValueLength", new object[] { key, limit }));
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x00216C3C File Offset: 0x0021603C
		internal static Exception InvalidConnectionOptionValue(string key, Exception inner)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidConnectionOptionValue", new object[] { key }), inner);
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x00216C68 File Offset: 0x00216068
		internal static Exception MissingConnectionOptionValue(string key, string requiredAdditionalKey)
		{
			return ADP.Argument(Res.GetString("ADP_MissingConnectionOptionValue", new object[] { key, requiredAdditionalKey }));
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00216C94 File Offset: 0x00216094
		internal static Exception InvalidXMLBadVersion()
		{
			return ADP.Argument(Res.GetString("ADP_InvalidXMLBadVersion"));
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00216CB0 File Offset: 0x002160B0
		internal static Exception NotAPermissionElement()
		{
			return ADP.Argument(Res.GetString("ADP_NotAPermissionElement"));
		}

		// Token: 0x06000FC6 RID: 4038 RVA: 0x00216CCC File Offset: 0x002160CC
		internal static Exception PermissionTypeMismatch()
		{
			return ADP.Argument(Res.GetString("ADP_PermissionTypeMismatch"));
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00216CE8 File Offset: 0x002160E8
		internal static Exception WrongType(Type got, Type expected)
		{
			return ADP.Argument(Res.GetString("SQL_WrongType", new object[]
			{
				got.ToString(),
				expected.ToString()
			}));
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00216D20 File Offset: 0x00216120
		internal static Exception PooledOpenTimeout()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_PooledOpenTimeout"));
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00216D3C File Offset: 0x0021613C
		internal static ArgumentException CollectionRemoveInvalidObject(Type itemType, ICollection collection)
		{
			return ADP.Argument(Res.GetString("ADP_CollectionRemoveInvalidObject", new object[]
			{
				itemType.Name,
				collection.GetType().Name
			}));
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00216D78 File Offset: 0x00216178
		internal static ArgumentNullException CollectionNullValue(string parameter, Type collection, Type itemType)
		{
			return ADP.ArgumentNull(parameter, Res.GetString("ADP_CollectionNullValue", new object[] { collection.Name, itemType.Name }));
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00216DB0 File Offset: 0x002161B0
		internal static IndexOutOfRangeException CollectionIndexInt32(int index, Type collection, int count)
		{
			return ADP.IndexOutOfRange(Res.GetString("ADP_CollectionIndexInt32", new object[]
			{
				index.ToString(CultureInfo.InvariantCulture),
				collection.Name,
				count.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00216DFC File Offset: 0x002161FC
		internal static IndexOutOfRangeException CollectionIndexString(Type itemType, string propertyName, string propertyValue, Type collection)
		{
			return ADP.IndexOutOfRange(Res.GetString("ADP_CollectionIndexString", new object[] { itemType.Name, propertyName, propertyValue, collection.Name }));
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00216E3C File Offset: 0x0021623C
		internal static InvalidCastException CollectionInvalidType(Type collection, Type itemType, object invalidValue)
		{
			return ADP.InvalidCast(Res.GetString("ADP_CollectionInvalidType", new object[]
			{
				collection.Name,
				itemType.Name,
				invalidValue.GetType().Name
			}));
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00216E80 File Offset: 0x00216280
		internal static Exception CollectionUniqueValue(Type itemType, string propertyName, string propertyValue)
		{
			return ADP.Argument(Res.GetString("ADP_CollectionUniqueValue", new object[] { itemType.Name, propertyName, propertyValue }));
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00216EB8 File Offset: 0x002162B8
		internal static ArgumentException ParametersIsNotParent(Type parameterType, ICollection collection)
		{
			return ADP.Argument(Res.GetString("ADP_CollectionIsNotParent", new object[]
			{
				parameterType.Name,
				collection.GetType().Name
			}));
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x00216EF4 File Offset: 0x002162F4
		internal static ArgumentException ParametersIsParent(Type parameterType, ICollection collection)
		{
			return ADP.Argument(Res.GetString("ADP_CollectionIsNotParent", new object[]
			{
				parameterType.Name,
				collection.GetType().Name
			}));
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x00216F30 File Offset: 0x00216330
		internal static InvalidOperationException TransactionConnectionMismatch()
		{
			return ADP.Provider(Res.GetString("ADP_TransactionConnectionMismatch"));
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00216F4C File Offset: 0x0021634C
		internal static InvalidOperationException TransactionRequired(string method)
		{
			return ADP.Provider(Res.GetString("ADP_TransactionRequired", new object[] { method }));
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00216F74 File Offset: 0x00216374
		internal static InvalidOperationException MissingSelectCommand(string method)
		{
			return ADP.Provider(Res.GetString("ADP_MissingSelectCommand", new object[] { method }));
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00216F9C File Offset: 0x0021639C
		private static InvalidOperationException DataMapping(string error)
		{
			return ADP.InvalidOperation(error);
		}

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00216FB0 File Offset: 0x002163B0
		internal static InvalidOperationException ColumnSchemaExpression(string srcColumn, string cacheColumn)
		{
			return ADP.DataMapping(Res.GetString("ADP_ColumnSchemaExpression", new object[] { srcColumn, cacheColumn }));
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x00216FDC File Offset: 0x002163DC
		internal static InvalidOperationException ColumnSchemaMismatch(string srcColumn, Type srcType, DataColumn column)
		{
			return ADP.DataMapping(Res.GetString("ADP_ColumnSchemaMismatch", new object[]
			{
				srcColumn,
				srcType.Name,
				column.ColumnName,
				column.DataType.Name
			}));
		}

		// Token: 0x06000FD7 RID: 4055 RVA: 0x00217024 File Offset: 0x00216424
		internal static InvalidOperationException ColumnSchemaMissing(string cacheColumn, string tableName, string srcColumn)
		{
			if (ADP.IsEmpty(tableName))
			{
				return ADP.InvalidOperation(Res.GetString("ADP_ColumnSchemaMissing1", new object[] { cacheColumn, tableName, srcColumn }));
			}
			return ADP.DataMapping(Res.GetString("ADP_ColumnSchemaMissing2", new object[] { cacheColumn, tableName, srcColumn }));
		}

		// Token: 0x06000FD8 RID: 4056 RVA: 0x00217080 File Offset: 0x00216480
		internal static InvalidOperationException MissingColumnMapping(string srcColumn)
		{
			return ADP.DataMapping(Res.GetString("ADP_MissingColumnMapping", new object[] { srcColumn }));
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x002170A8 File Offset: 0x002164A8
		internal static InvalidOperationException MissingTableSchema(string cacheTable, string srcTable)
		{
			return ADP.DataMapping(Res.GetString("ADP_MissingTableSchema", new object[] { cacheTable, srcTable }));
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x002170D4 File Offset: 0x002164D4
		internal static InvalidOperationException MissingTableMapping(string srcTable)
		{
			return ADP.DataMapping(Res.GetString("ADP_MissingTableMapping", new object[] { srcTable }));
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x002170FC File Offset: 0x002164FC
		internal static InvalidOperationException MissingTableMappingDestination(string dstTable)
		{
			return ADP.DataMapping(Res.GetString("ADP_MissingTableMappingDestination", new object[] { dstTable }));
		}

		// Token: 0x06000FDC RID: 4060 RVA: 0x00217124 File Offset: 0x00216524
		internal static Exception InvalidSourceColumn(string parameter)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidSourceColumn"), parameter);
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00217144 File Offset: 0x00216544
		internal static Exception ColumnsAddNullAttempt(string parameter)
		{
			return ADP.CollectionNullValue(parameter, typeof(DataColumnMappingCollection), typeof(DataColumnMapping));
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0021716C File Offset: 0x0021656C
		internal static Exception ColumnsDataSetColumn(string cacheColumn)
		{
			return ADP.CollectionIndexString(typeof(DataColumnMapping), "DataSetColumn", cacheColumn, typeof(DataColumnMappingCollection));
		}

		// Token: 0x06000FDF RID: 4063 RVA: 0x00217198 File Offset: 0x00216598
		internal static Exception ColumnsIndexInt32(int index, IColumnMappingCollection collection)
		{
			return ADP.CollectionIndexInt32(index, collection.GetType(), collection.Count);
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x002171B8 File Offset: 0x002165B8
		internal static Exception ColumnsIndexSource(string srcColumn)
		{
			return ADP.CollectionIndexString(typeof(DataColumnMapping), "SourceColumn", srcColumn, typeof(DataColumnMappingCollection));
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x002171E4 File Offset: 0x002165E4
		internal static Exception ColumnsIsNotParent(ICollection collection)
		{
			return ADP.ParametersIsNotParent(typeof(DataColumnMapping), collection);
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x00217204 File Offset: 0x00216604
		internal static Exception ColumnsIsParent(ICollection collection)
		{
			return ADP.ParametersIsParent(typeof(DataColumnMapping), collection);
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x00217224 File Offset: 0x00216624
		internal static Exception ColumnsUniqueSourceColumn(string srcColumn)
		{
			return ADP.CollectionUniqueValue(typeof(DataColumnMapping), "SourceColumn", srcColumn);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x00217248 File Offset: 0x00216648
		internal static Exception NotADataColumnMapping(object value)
		{
			return ADP.CollectionInvalidType(typeof(DataColumnMappingCollection), typeof(DataColumnMapping), value);
		}

		// Token: 0x06000FE5 RID: 4069 RVA: 0x00217270 File Offset: 0x00216670
		internal static Exception InvalidSourceTable(string parameter)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidSourceTable"), parameter);
		}

		// Token: 0x06000FE6 RID: 4070 RVA: 0x00217290 File Offset: 0x00216690
		internal static Exception TablesAddNullAttempt(string parameter)
		{
			return ADP.CollectionNullValue(parameter, typeof(DataTableMappingCollection), typeof(DataTableMapping));
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x002172B8 File Offset: 0x002166B8
		internal static Exception TablesDataSetTable(string cacheTable)
		{
			return ADP.CollectionIndexString(typeof(DataTableMapping), "DataSetTable", cacheTable, typeof(DataTableMappingCollection));
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x002172E4 File Offset: 0x002166E4
		internal static Exception TablesIndexInt32(int index, ITableMappingCollection collection)
		{
			return ADP.CollectionIndexInt32(index, collection.GetType(), collection.Count);
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x00217304 File Offset: 0x00216704
		internal static Exception TablesIsNotParent(ICollection collection)
		{
			return ADP.ParametersIsNotParent(typeof(DataTableMapping), collection);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x00217324 File Offset: 0x00216724
		internal static Exception TablesIsParent(ICollection collection)
		{
			return ADP.ParametersIsParent(typeof(DataTableMapping), collection);
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00217344 File Offset: 0x00216744
		internal static Exception TablesSourceIndex(string srcTable)
		{
			return ADP.CollectionIndexString(typeof(DataTableMapping), "SourceTable", srcTable, typeof(DataTableMappingCollection));
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x00217370 File Offset: 0x00216770
		internal static Exception TablesUniqueSourceTable(string srcTable)
		{
			return ADP.CollectionUniqueValue(typeof(DataTableMapping), "SourceTable", srcTable);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x00217394 File Offset: 0x00216794
		internal static Exception NotADataTableMapping(object value)
		{
			return ADP.CollectionInvalidType(typeof(DataTableMappingCollection), typeof(DataTableMapping), value);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x002173BC File Offset: 0x002167BC
		internal static InvalidOperationException CommandAsyncOperationCompleted()
		{
			return ADP.InvalidOperation(Res.GetString("SQL_AsyncOperationCompleted"));
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x002173D8 File Offset: 0x002167D8
		internal static Exception CommandTextRequired(string method)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_CommandTextRequired", new object[] { method }));
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00217400 File Offset: 0x00216800
		internal static InvalidOperationException ConnectionRequired(string method)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ConnectionRequired", new object[] { method }));
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00217428 File Offset: 0x00216828
		internal static InvalidOperationException OpenConnectionRequired(string method, ConnectionState state)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_OpenConnectionRequired", new object[]
			{
				method,
				ADP.ConnectionStateMsg(state)
			}));
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x0021745C File Offset: 0x0021685C
		internal static InvalidOperationException UpdateConnectionRequired(StatementType statementType, bool isRowUpdatingCommand)
		{
			string text;
			if (!isRowUpdatingCommand)
			{
				switch (statementType)
				{
				case StatementType.Insert:
					text = "ADP_ConnectionRequired_Insert";
					goto IL_004C;
				case StatementType.Update:
					text = "ADP_ConnectionRequired_Update";
					goto IL_004C;
				case StatementType.Delete:
					text = "ADP_ConnectionRequired_Delete";
					goto IL_004C;
				}
				throw ADP.InvalidStatementType(statementType);
			}
			text = "ADP_ConnectionRequired_Clone";
			IL_004C:
			return ADP.InvalidOperation(Res.GetString(text));
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x002174C0 File Offset: 0x002168C0
		internal static InvalidOperationException ConnectionRequired_Res(string method)
		{
			string text = "ADP_ConnectionRequired_" + method;
			return ADP.InvalidOperation(Res.GetString(text));
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x002174E4 File Offset: 0x002168E4
		internal static InvalidOperationException UpdateOpenConnectionRequired(StatementType statementType, bool isRowUpdatingCommand, ConnectionState state)
		{
			string text;
			if (isRowUpdatingCommand)
			{
				text = "ADP_OpenConnectionRequired_Clone";
			}
			else
			{
				switch (statementType)
				{
				case StatementType.Insert:
					text = "ADP_OpenConnectionRequired_Insert";
					break;
				case StatementType.Update:
					text = "ADP_OpenConnectionRequired_Update";
					break;
				case StatementType.Delete:
					text = "ADP_OpenConnectionRequired_Delete";
					break;
				default:
					throw ADP.InvalidStatementType(statementType);
				}
			}
			return ADP.InvalidOperation(Res.GetString(text, new object[] { ADP.ConnectionStateMsg(state) }));
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00217550 File Offset: 0x00216950
		internal static Exception NoStoredProcedureExists(string sproc)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NoStoredProcedureExists", new object[] { sproc }));
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00217578 File Offset: 0x00216978
		internal static Exception OpenReaderExists()
		{
			return ADP.OpenReaderExists(null);
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0021758C File Offset: 0x0021698C
		internal static Exception OpenReaderExists(Exception e)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_OpenReaderExists"), e);
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x002175AC File Offset: 0x002169AC
		internal static Exception TransactionCompleted()
		{
			return ADP.DataAdapter(Res.GetString("ADP_TransactionCompleted"));
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x002175C8 File Offset: 0x002169C8
		internal static Exception NonSeqByteAccess(long badIndex, long currIndex, string method)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NonSeqByteAccess", new object[]
			{
				badIndex.ToString(CultureInfo.InvariantCulture),
				currIndex.ToString(CultureInfo.InvariantCulture),
				method
			}));
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x00217610 File Offset: 0x00216A10
		internal static Exception NegativeParameter(string parameterName)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NegativeParameter", new object[] { parameterName }));
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00217638 File Offset: 0x00216A38
		internal static Exception NumericToDecimalOverflow()
		{
			return ADP.InvalidCast(Res.GetString("ADP_NumericToDecimalOverflow"));
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x00217654 File Offset: 0x00216A54
		internal static Exception ExceedsMaxDataLength(long specifiedLength, long maxLength)
		{
			return ADP.IndexOutOfRange(Res.GetString("SQL_ExceedsMaxDataLength", new object[]
			{
				specifiedLength.ToString(CultureInfo.InvariantCulture),
				maxLength.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x00217698 File Offset: 0x00216A98
		internal static Exception InvalidSeekOrigin(string parameterName)
		{
			return ADP.ArgumentOutOfRange(Res.GetString("ADP_InvalidSeekOrigin"), parameterName);
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x002176B8 File Offset: 0x00216AB8
		internal static Exception InvalidImplicitConversion(Type fromtype, string totype)
		{
			return ADP.InvalidCast(Res.GetString("ADP_InvalidImplicitConversion", new object[] { fromtype.Name, totype }));
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x002176EC File Offset: 0x00216AEC
		internal static Exception InvalidMetaDataValue()
		{
			return ADP.Argument(Res.GetString("ADP_InvalidMetaDataValue"));
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00217708 File Offset: 0x00216B08
		internal static Exception NotRowType()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NotRowType"));
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00217724 File Offset: 0x00216B24
		internal static ArgumentException UnwantedStatementType(StatementType statementType)
		{
			return ADP.Argument(Res.GetString("ADP_UnwantedStatementType", new object[] { statementType.ToString() }));
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00217758 File Offset: 0x00216B58
		internal static InvalidOperationException NonSequentialColumnAccess(int badCol, int currCol)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NonSequentialColumnAccess", new object[]
			{
				badCol.ToString(CultureInfo.InvariantCulture),
				currCol.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0021779C File Offset: 0x00216B9C
		internal static Exception FillSchemaRequiresSourceTableName(string parameter)
		{
			return ADP.Argument(Res.GetString("ADP_FillSchemaRequiresSourceTableName"), parameter);
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x002177BC File Offset: 0x00216BBC
		internal static Exception InvalidMaxRecords(string parameter, int max)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidMaxRecords", new object[] { max.ToString(CultureInfo.InvariantCulture) }), parameter);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x002177F0 File Offset: 0x00216BF0
		internal static Exception InvalidStartRecord(string parameter, int start)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidStartRecord", new object[] { start.ToString(CultureInfo.InvariantCulture) }), parameter);
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00217824 File Offset: 0x00216C24
		internal static Exception FillRequires(string parameter)
		{
			return ADP.ArgumentNull(parameter);
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00217838 File Offset: 0x00216C38
		internal static Exception FillRequiresSourceTableName(string parameter)
		{
			return ADP.Argument(Res.GetString("ADP_FillRequiresSourceTableName"), parameter);
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00217858 File Offset: 0x00216C58
		internal static Exception FillChapterAutoIncrement()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_FillChapterAutoIncrement"));
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00217874 File Offset: 0x00216C74
		internal static InvalidOperationException MissingDataReaderFieldType(int index)
		{
			return ADP.DataAdapter(Res.GetString("ADP_MissingDataReaderFieldType", new object[] { index }));
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x002178A4 File Offset: 0x00216CA4
		internal static InvalidOperationException OnlyOneTableForStartRecordOrMaxRecords()
		{
			return ADP.DataAdapter(Res.GetString("ADP_OnlyOneTableForStartRecordOrMaxRecords"));
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x002178C0 File Offset: 0x00216CC0
		internal static ArgumentNullException UpdateRequiresNonNullDataSet(string parameter)
		{
			return ADP.ArgumentNull(parameter);
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x002178D4 File Offset: 0x00216CD4
		internal static InvalidOperationException UpdateRequiresSourceTable(string defaultSrcTableName)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_UpdateRequiresSourceTable", new object[] { defaultSrcTableName }));
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x002178FC File Offset: 0x00216CFC
		internal static InvalidOperationException UpdateRequiresSourceTableName(string srcTable)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_UpdateRequiresSourceTableName", new object[] { srcTable }));
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00217924 File Offset: 0x00216D24
		internal static ArgumentNullException UpdateRequiresDataTable(string parameter)
		{
			return ADP.ArgumentNull(parameter);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00217938 File Offset: 0x00216D38
		internal static Exception UpdateConcurrencyViolation(StatementType statementType, int affected, int expected, DataRow[] dataRows)
		{
			string text;
			switch (statementType)
			{
			case StatementType.Update:
				text = "ADP_UpdateConcurrencyViolation_Update";
				break;
			case StatementType.Delete:
				text = "ADP_UpdateConcurrencyViolation_Delete";
				break;
			case StatementType.Batch:
				text = "ADP_UpdateConcurrencyViolation_Batch";
				break;
			default:
				throw ADP.InvalidStatementType(statementType);
			}
			DBConcurrencyException ex = new DBConcurrencyException(Res.GetString(text, new object[]
			{
				affected.ToString(CultureInfo.InvariantCulture),
				expected.ToString(CultureInfo.InvariantCulture)
			}), null, dataRows);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x002179B8 File Offset: 0x00216DB8
		internal static InvalidOperationException UpdateRequiresCommand(StatementType statementType, bool isRowUpdatingCommand)
		{
			string text;
			if (isRowUpdatingCommand)
			{
				text = "ADP_UpdateRequiresCommandClone";
			}
			else
			{
				switch (statementType)
				{
				case StatementType.Select:
					text = "ADP_UpdateRequiresCommandSelect";
					break;
				case StatementType.Insert:
					text = "ADP_UpdateRequiresCommandInsert";
					break;
				case StatementType.Update:
					text = "ADP_UpdateRequiresCommandUpdate";
					break;
				case StatementType.Delete:
					text = "ADP_UpdateRequiresCommandDelete";
					break;
				default:
					throw ADP.InvalidStatementType(statementType);
				}
			}
			return ADP.InvalidOperation(Res.GetString(text));
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00217A1C File Offset: 0x00216E1C
		internal static ArgumentException UpdateMismatchRowTable(int i)
		{
			return ADP.Argument(Res.GetString("ADP_UpdateMismatchRowTable", new object[] { i.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00217A50 File Offset: 0x00216E50
		internal static DataException RowUpdatedErrors()
		{
			return ADP.Data(Res.GetString("ADP_RowUpdatedErrors"));
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00217A6C File Offset: 0x00216E6C
		internal static DataException RowUpdatingErrors()
		{
			return ADP.Data(Res.GetString("ADP_RowUpdatingErrors"));
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00217A88 File Offset: 0x00216E88
		internal static InvalidOperationException ResultsNotAllowedDuringBatch()
		{
			return ADP.DataAdapter(Res.GetString("ADP_ResultsNotAllowedDuringBatch"));
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00217AA4 File Offset: 0x00216EA4
		internal static Exception InvalidCommandTimeout(int value)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidCommandTimeout", new object[] { value.ToString(CultureInfo.InvariantCulture) }), "CommandTimeout");
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00217ADC File Offset: 0x00216EDC
		internal static Exception DeriveParametersNotSupported(IDbCommand value)
		{
			return ADP.DataAdapter(Res.GetString("ADP_DeriveParametersNotSupported", new object[]
			{
				value.GetType().Name,
				value.CommandType.ToString()
			}));
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00217B24 File Offset: 0x00216F24
		internal static Exception UninitializedParameterSize(int index, Type dataType)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_UninitializedParameterSize", new object[]
			{
				index.ToString(CultureInfo.InvariantCulture),
				dataType.Name
			}));
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00217B60 File Offset: 0x00216F60
		internal static Exception PrepareParameterType(IDbCommand cmd)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_PrepareParameterType", new object[] { cmd.GetType().Name }));
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x00217B94 File Offset: 0x00216F94
		internal static Exception PrepareParameterSize(IDbCommand cmd)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_PrepareParameterSize", new object[] { cmd.GetType().Name }));
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00217BC8 File Offset: 0x00216FC8
		internal static Exception PrepareParameterScale(IDbCommand cmd, string type)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_PrepareParameterScale", new object[]
			{
				cmd.GetType().Name,
				type
			}));
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00217C00 File Offset: 0x00217000
		internal static Exception MismatchedAsyncResult(string expectedMethod, string gotMethod)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_MismatchedAsyncResult", new object[] { expectedMethod, gotMethod }));
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00217C2C File Offset: 0x0021702C
		internal static Exception ConnectionIsDisabled(Exception InnerException)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ConnectionIsDisabled"), InnerException);
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00217C4C File Offset: 0x0021704C
		internal static Exception ClosedConnectionError()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ClosedConnectionError"));
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00217C68 File Offset: 0x00217068
		internal static Exception ConnectionAlreadyOpen(ConnectionState state)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ConnectionAlreadyOpen", new object[] { ADP.ConnectionStateMsg(state) }));
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00217C98 File Offset: 0x00217098
		internal static Exception DelegatedTransactionPresent()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DelegatedTransactionPresent"));
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x00217CB4 File Offset: 0x002170B4
		internal static Exception TransactionPresent()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_TransactionPresent"));
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00217CD0 File Offset: 0x002170D0
		internal static Exception LocalTransactionPresent()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_LocalTransactionPresent"));
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x00217CEC File Offset: 0x002170EC
		internal static Exception OpenConnectionPropertySet(string property, ConnectionState state)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_OpenConnectionPropertySet", new object[]
			{
				property,
				ADP.ConnectionStateMsg(state)
			}));
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x00217D20 File Offset: 0x00217120
		internal static Exception EmptyDatabaseName()
		{
			return ADP.Argument(Res.GetString("ADP_EmptyDatabaseName"));
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x00217D3C File Offset: 0x0021713C
		internal static Exception DatabaseNameTooLong()
		{
			return ADP.Argument(Res.GetString("ADP_DatabaseNameTooLong"));
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00217D58 File Offset: 0x00217158
		internal static Exception InternalConnectionError(ADP.ConnectionError internalError)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InternalConnectionError", new object[] { (int)internalError }));
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x00217D88 File Offset: 0x00217188
		internal static Exception InternalError(ADP.InternalErrorCode internalError)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InternalProviderError", new object[] { (int)internalError }));
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00217DB8 File Offset: 0x002171B8
		internal static Exception InvalidConnectTimeoutValue()
		{
			return ADP.Argument(Res.GetString("ADP_InvalidConnectTimeoutValue"));
		}

		// Token: 0x06001028 RID: 4136 RVA: 0x00217DD4 File Offset: 0x002171D4
		internal static Exception DataReaderNoData()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DataReaderNoData"));
		}

		// Token: 0x06001029 RID: 4137 RVA: 0x00217DF0 File Offset: 0x002171F0
		internal static Exception DataReaderClosed(string method)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DataReaderClosed", new object[] { method }));
		}

		// Token: 0x0600102A RID: 4138 RVA: 0x00217E18 File Offset: 0x00217218
		internal static ArgumentOutOfRangeException InvalidSourceBufferIndex(int maxLen, long srcOffset, string parameterName)
		{
			return ADP.ArgumentOutOfRange(Res.GetString("ADP_InvalidSourceBufferIndex", new object[]
			{
				maxLen.ToString(CultureInfo.InvariantCulture),
				srcOffset.ToString(CultureInfo.InvariantCulture)
			}), parameterName);
		}

		// Token: 0x0600102B RID: 4139 RVA: 0x00217E5C File Offset: 0x0021725C
		internal static ArgumentOutOfRangeException InvalidDestinationBufferIndex(int maxLen, int dstOffset, string parameterName)
		{
			return ADP.ArgumentOutOfRange(Res.GetString("ADP_InvalidDestinationBufferIndex", new object[]
			{
				maxLen.ToString(CultureInfo.InvariantCulture),
				dstOffset.ToString(CultureInfo.InvariantCulture)
			}), parameterName);
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x00217EA0 File Offset: 0x002172A0
		internal static IndexOutOfRangeException InvalidBufferSizeOrIndex(int numBytes, int bufferIndex)
		{
			return ADP.IndexOutOfRange(Res.GetString("SQL_InvalidBufferSizeOrIndex", new object[]
			{
				numBytes.ToString(CultureInfo.InvariantCulture),
				bufferIndex.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x00217EE4 File Offset: 0x002172E4
		internal static Exception InvalidDataLength(long length)
		{
			return ADP.IndexOutOfRange(Res.GetString("SQL_InvalidDataLength", new object[] { length.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x00217F18 File Offset: 0x00217318
		internal static Exception StreamClosed(string method)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_StreamClosed", new object[] { method }));
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x00217F40 File Offset: 0x00217340
		internal static InvalidOperationException DynamicSQLJoinUnsupported()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DynamicSQLJoinUnsupported"));
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x00217F5C File Offset: 0x0021735C
		internal static InvalidOperationException DynamicSQLNoTableInfo()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DynamicSQLNoTableInfo"));
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x00217F78 File Offset: 0x00217378
		internal static InvalidOperationException DynamicSQLNoKeyInfoDelete()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DynamicSQLNoKeyInfoDelete"));
		}

		// Token: 0x06001032 RID: 4146 RVA: 0x00217F94 File Offset: 0x00217394
		internal static InvalidOperationException DynamicSQLNoKeyInfoUpdate()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DynamicSQLNoKeyInfoUpdate"));
		}

		// Token: 0x06001033 RID: 4147 RVA: 0x00217FB0 File Offset: 0x002173B0
		internal static InvalidOperationException DynamicSQLNoKeyInfoRowVersionDelete()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DynamicSQLNoKeyInfoRowVersionDelete"));
		}

		// Token: 0x06001034 RID: 4148 RVA: 0x00217FCC File Offset: 0x002173CC
		internal static InvalidOperationException DynamicSQLNoKeyInfoRowVersionUpdate()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DynamicSQLNoKeyInfoRowVersionUpdate"));
		}

		// Token: 0x06001035 RID: 4149 RVA: 0x00217FE8 File Offset: 0x002173E8
		internal static InvalidOperationException DynamicSQLNestedQuote(string name, string quote)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DynamicSQLNestedQuote", new object[] { name, quote }));
		}

		// Token: 0x06001036 RID: 4150 RVA: 0x00218014 File Offset: 0x00217414
		internal static InvalidOperationException NoQuoteChange()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NoQuoteChange"));
		}

		// Token: 0x06001037 RID: 4151 RVA: 0x00218030 File Offset: 0x00217430
		internal static InvalidOperationException ComputerNameEx(int lastError)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ComputerNameEx", new object[] { lastError }));
		}

		// Token: 0x06001038 RID: 4152 RVA: 0x00218060 File Offset: 0x00217460
		internal static InvalidOperationException MissingSourceCommand()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_MissingSourceCommand"));
		}

		// Token: 0x06001039 RID: 4153 RVA: 0x0021807C File Offset: 0x0021747C
		internal static InvalidOperationException MissingSourceCommandConnection()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_MissingSourceCommandConnection"));
		}

		// Token: 0x0600103A RID: 4154 RVA: 0x00218098 File Offset: 0x00217498
		internal static ArgumentException InvalidDataType(TypeCode typecode)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidDataType", new object[] { typecode.ToString() }));
		}

		// Token: 0x0600103B RID: 4155 RVA: 0x002180CC File Offset: 0x002174CC
		internal static ArgumentException UnknownDataType(Type dataType)
		{
			return ADP.Argument(Res.GetString("ADP_UnknownDataType", new object[] { dataType.FullName }));
		}

		// Token: 0x0600103C RID: 4156 RVA: 0x002180FC File Offset: 0x002174FC
		internal static ArgumentException DbTypeNotSupported(DbType type, Type enumtype)
		{
			return ADP.Argument(Res.GetString("ADP_DbTypeNotSupported", new object[]
			{
				type.ToString(),
				enumtype.Name
			}));
		}

		// Token: 0x0600103D RID: 4157 RVA: 0x00218138 File Offset: 0x00217538
		internal static ArgumentException UnknownDataTypeCode(Type dataType, TypeCode typeCode)
		{
			string text = "ADP_UnknownDataTypeCode";
			object[] array = new object[2];
			object[] array2 = array;
			int num = 0;
			int num2 = (int)typeCode;
			array2[num] = num2.ToString(CultureInfo.InvariantCulture);
			array[1] = dataType.FullName;
			return ADP.Argument(Res.GetString(text, array));
		}

		// Token: 0x0600103E RID: 4158 RVA: 0x00218178 File Offset: 0x00217578
		internal static ArgumentException InvalidOffsetValue(int value)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidOffsetValue", new object[] { value.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x0600103F RID: 4159 RVA: 0x002181AC File Offset: 0x002175AC
		internal static ArgumentException InvalidSizeValue(int value)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidSizeValue", new object[] { value.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x06001040 RID: 4160 RVA: 0x002181E0 File Offset: 0x002175E0
		internal static ArgumentException ParameterValueOutOfRange(decimal value)
		{
			return ADP.Argument(Res.GetString("ADP_ParameterValueOutOfRange", new object[] { value.ToString(null) }));
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x00218210 File Offset: 0x00217610
		internal static ArgumentException ParameterValueOutOfRange(SqlDecimal value)
		{
			return ADP.Argument(Res.GetString("ADP_ParameterValueOutOfRange", new object[] { value.ToString() }));
		}

		// Token: 0x06001042 RID: 4162 RVA: 0x00218244 File Offset: 0x00217644
		internal static ArgumentException VersionDoesNotSupportDataType(string typeName)
		{
			return ADP.Argument(Res.GetString("ADP_VersionDoesNotSupportDataType", new object[] { typeName }));
		}

		// Token: 0x06001043 RID: 4163 RVA: 0x0021826C File Offset: 0x0021766C
		internal static Exception ParameterConversionFailed(object value, Type destType, Exception inner)
		{
			string @string = Res.GetString("ADP_ParameterConversionFailed", new object[]
			{
				value.GetType().Name,
				destType.Name
			});
			Exception ex;
			if (inner is ArgumentException)
			{
				ex = new ArgumentException(@string, inner);
			}
			else if (inner is FormatException)
			{
				ex = new FormatException(@string, inner);
			}
			else if (inner is InvalidCastException)
			{
				ex = new InvalidCastException(@string, inner);
			}
			else if (inner is OverflowException)
			{
				ex = new OverflowException(@string, inner);
			}
			else
			{
				ex = inner;
			}
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06001044 RID: 4164 RVA: 0x002182F4 File Offset: 0x002176F4
		internal static Exception ParametersMappingIndex(int index, IDataParameterCollection collection)
		{
			return ADP.CollectionIndexInt32(index, collection.GetType(), collection.Count);
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x00218314 File Offset: 0x00217714
		internal static Exception ParametersSourceIndex(string parameterName, IDataParameterCollection collection, Type parameterType)
		{
			return ADP.CollectionIndexString(parameterType, "ParameterName", parameterName, collection.GetType());
		}

		// Token: 0x06001046 RID: 4166 RVA: 0x00218334 File Offset: 0x00217734
		internal static Exception ParameterNull(string parameter, IDataParameterCollection collection, Type parameterType)
		{
			return ADP.CollectionNullValue(parameter, collection.GetType(), parameterType);
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00218350 File Offset: 0x00217750
		internal static Exception InvalidParameterType(IDataParameterCollection collection, Type parameterType, object invalidValue)
		{
			return ADP.CollectionInvalidType(collection.GetType(), parameterType, invalidValue);
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x0021836C File Offset: 0x0021776C
		internal static Exception ParallelTransactionsNotSupported(IDbConnection obj)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ParallelTransactionsNotSupported", new object[] { obj.GetType().Name }));
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x002183A0 File Offset: 0x002177A0
		internal static Exception TransactionZombied(IDbTransaction obj)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_TransactionZombied", new object[] { obj.GetType().Name }));
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x002183D4 File Offset: 0x002177D4
		internal static Exception DbRecordReadOnly(string methodname)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DbRecordReadOnly", new object[] { methodname }));
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x002183FC File Offset: 0x002177FC
		internal static Exception OffsetOutOfRangeException()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_OffsetOutOfRangeException"));
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x00218418 File Offset: 0x00217818
		internal static Exception AmbigousCollectionName(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_AmbigousCollectionName", new object[] { collectionName }));
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00218440 File Offset: 0x00217840
		internal static Exception CollectionNameIsNotUnique(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_CollectionNameISNotUnique", new object[] { collectionName }));
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00218468 File Offset: 0x00217868
		internal static Exception DataTableDoesNotExist(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_DataTableDoesNotExist", new object[] { collectionName }));
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00218490 File Offset: 0x00217890
		internal static Exception IncorrectNumberOfDataSourceInformationRows()
		{
			return ADP.Argument(Res.GetString("MDF_IncorrectNumberOfDataSourceInformationRows"));
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x002184AC File Offset: 0x002178AC
		internal static ArgumentException InvalidRestrictionValue(string collectionName, string restrictionName, string restrictionValue)
		{
			return ADP.Argument(Res.GetString("MDF_InvalidRestrictionValue", new object[] { collectionName, restrictionName, restrictionValue }));
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x002184DC File Offset: 0x002178DC
		internal static Exception InvalidXml()
		{
			return ADP.Argument(Res.GetString("MDF_InvalidXml"));
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x002184F8 File Offset: 0x002178F8
		internal static Exception InvalidXmlMissingColumn(string collectionName, string columnName)
		{
			return ADP.Argument(Res.GetString("MDF_InvalidXmlMissingColumn", new object[] { collectionName, columnName }));
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x00218524 File Offset: 0x00217924
		internal static Exception InvalidXmlInvalidValue(string collectionName, string columnName)
		{
			return ADP.Argument(Res.GetString("MDF_InvalidXmlInvalidValue", new object[] { collectionName, columnName }));
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00218550 File Offset: 0x00217950
		internal static Exception MissingDataSourceInformationColumn()
		{
			return ADP.Argument(Res.GetString("MDF_MissingDataSourceInformationColumn"));
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x0021856C File Offset: 0x0021796C
		internal static Exception MissingRestrictionColumn()
		{
			return ADP.Argument(Res.GetString("MDF_MissingRestrictionColumn"));
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00218588 File Offset: 0x00217988
		internal static Exception MissingRestrictionRow()
		{
			return ADP.Argument(Res.GetString("MDF_MissingRestrictionRow"));
		}

		// Token: 0x06001057 RID: 4183 RVA: 0x002185A4 File Offset: 0x002179A4
		internal static Exception NoColumns()
		{
			return ADP.Argument(Res.GetString("MDF_NoColumns"));
		}

		// Token: 0x06001058 RID: 4184 RVA: 0x002185C0 File Offset: 0x002179C0
		internal static Exception QueryFailed(string collectionName, Exception e)
		{
			return ADP.InvalidOperation(Res.GetString("MDF_QueryFailed", new object[] { collectionName }), e);
		}

		// Token: 0x06001059 RID: 4185 RVA: 0x002185EC File Offset: 0x002179EC
		internal static Exception TooManyRestrictions(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_TooManyRestrictions", new object[] { collectionName }));
		}

		// Token: 0x0600105A RID: 4186 RVA: 0x00218614 File Offset: 0x00217A14
		internal static Exception UnableToBuildCollection(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_UnableToBuildCollection", new object[] { collectionName }));
		}

		// Token: 0x0600105B RID: 4187 RVA: 0x0021863C File Offset: 0x00217A3C
		internal static Exception UndefinedCollection(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_UndefinedCollection", new object[] { collectionName }));
		}

		// Token: 0x0600105C RID: 4188 RVA: 0x00218664 File Offset: 0x00217A64
		internal static Exception UndefinedPopulationMechanism(string populationMechanism)
		{
			return ADP.Argument(Res.GetString("MDF_UndefinedPopulationMechanism", new object[] { populationMechanism }));
		}

		// Token: 0x0600105D RID: 4189 RVA: 0x0021868C File Offset: 0x00217A8C
		internal static Exception UnsupportedVersion(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_UnsupportedVersion", new object[] { collectionName }));
		}

		// Token: 0x0600105E RID: 4190 RVA: 0x002186B4 File Offset: 0x00217AB4
		internal static InvalidOperationException InvalidDateTimeDigits(string dataTypeName)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InvalidDateTimeDigits", new object[] { dataTypeName }));
		}

		// Token: 0x0600105F RID: 4191 RVA: 0x002186DC File Offset: 0x00217ADC
		internal static Exception InvalidFormatValue()
		{
			return ADP.Argument(Res.GetString("ADP_InvalidFormatValue"));
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x002186F8 File Offset: 0x00217AF8
		internal static InvalidOperationException InvalidMaximumScale(string dataTypeName)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InvalidMaximumScale", new object[] { dataTypeName }));
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x00218720 File Offset: 0x00217B20
		internal static Exception LiteralValueIsInvalid(string dataTypeName)
		{
			return ADP.Argument(Res.GetString("ADP_LiteralValueIsInvalid", new object[] { dataTypeName }));
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x00218748 File Offset: 0x00217B48
		internal static Exception EvenLengthLiteralValue(string argumentName)
		{
			return ADP.Argument(Res.GetString("ADP_EvenLengthLiteralValue"), argumentName);
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00218768 File Offset: 0x00217B68
		internal static Exception HexDigitLiteralValue(string argumentName)
		{
			return ADP.Argument(Res.GetString("ADP_HexDigitLiteralValue"), argumentName);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x00218788 File Offset: 0x00217B88
		internal static InvalidOperationException QuotePrefixNotSet(string method)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_QuotePrefixNotSet", new object[] { method }));
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x002187B0 File Offset: 0x00217BB0
		internal static InvalidOperationException UnableToCreateBooleanLiteral()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_UnableToCreateBooleanLiteral"));
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x002187CC File Offset: 0x00217BCC
		internal static Exception UnsupportedNativeDataTypeOleDb(string dataTypeName)
		{
			return ADP.Argument(Res.GetString("ADP_UnsupportedNativeDataTypeOleDb", new object[] { dataTypeName }));
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x002187F4 File Offset: 0x00217BF4
		internal static Exception InvalidArgumentValue(string methodName)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidArgumentValue", new object[] { methodName }));
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0021881C File Offset: 0x00217C1C
		internal static bool CompareInsensitiveInvariant(string strvalue, string strconst)
		{
			return 0 == CultureInfo.InvariantCulture.CompareInfo.Compare(strvalue, strconst, CompareOptions.IgnoreCase);
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x00218840 File Offset: 0x00217C40
		internal static Delegate FindBuilder(MulticastDelegate mcd)
		{
			if (mcd != null)
			{
				Delegate[] invocationList = mcd.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					if (invocationList[i].Target is DbCommandBuilder)
					{
						return invocationList[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0021887C File Offset: 0x00217C7C
		internal static Transaction GetCurrentTransaction()
		{
			return Transaction.Current;
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x00218890 File Offset: 0x00217C90
		internal static IDtcTransaction GetOletxTransaction(Transaction transaction)
		{
			IDtcTransaction dtcTransaction = null;
			if (null != transaction)
			{
				dtcTransaction = TransactionInterop.GetDtcTransaction(transaction);
			}
			return dtcTransaction;
		}

		// Token: 0x0600106C RID: 4204 RVA: 0x002188B0 File Offset: 0x00217CB0
		[MethodImpl(MethodImplOptions.NoInlining)]
		internal static bool IsSysTxEqualSysEsTransaction()
		{
			return (!ContextUtil.IsInTransaction && null == Transaction.Current) || (ContextUtil.IsInTransaction && Transaction.Current == ContextUtil.SystemTransaction);
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x002188F0 File Offset: 0x00217CF0
		internal static bool NeedManualEnlistment()
		{
			if (ADP.IsWindowsNT)
			{
				bool flag = !InOutOfProcHelper.InProc;
				if ((flag && !ADP.IsSysTxEqualSysEsTransaction()) || (!flag && null != Transaction.Current))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600106E RID: 4206 RVA: 0x0021892C File Offset: 0x00217D2C
		internal static void TimerCurrent(out long ticks)
		{
			SafeNativeMethods.GetSystemTimeAsFileTime(out ticks);
		}

		// Token: 0x0600106F RID: 4207 RVA: 0x00218940 File Offset: 0x00217D40
		internal static long TimerCurrent()
		{
			long num = 0L;
			SafeNativeMethods.GetSystemTimeAsFileTime(out num);
			return num;
		}

		// Token: 0x06001070 RID: 4208 RVA: 0x00218958 File Offset: 0x00217D58
		internal static long TimerFromSeconds(int seconds)
		{
			checked
			{
				return unchecked((long)seconds) * 10000000L;
			}
		}

		// Token: 0x06001071 RID: 4209 RVA: 0x00218970 File Offset: 0x00217D70
		internal static bool TimerHasExpired(long timerExpire)
		{
			return ADP.TimerCurrent() > timerExpire;
		}

		// Token: 0x06001072 RID: 4210 RVA: 0x00218988 File Offset: 0x00217D88
		internal static long TimerRemaining(long timerExpire)
		{
			long num = ADP.TimerCurrent();
			return checked(timerExpire - num);
		}

		// Token: 0x06001073 RID: 4211 RVA: 0x002189A0 File Offset: 0x00217DA0
		internal static long TimerRemainingMilliseconds(long timerExpire)
		{
			return ADP.TimerToMilliseconds(ADP.TimerRemaining(timerExpire));
		}

		// Token: 0x06001074 RID: 4212 RVA: 0x002189BC File Offset: 0x00217DBC
		internal static long TimerRemainingSeconds(long timerExpire)
		{
			return ADP.TimerToSeconds(ADP.TimerRemaining(timerExpire));
		}

		// Token: 0x06001075 RID: 4213 RVA: 0x002189D8 File Offset: 0x00217DD8
		internal static long TimerToMilliseconds(long timerValue)
		{
			return timerValue / 10000L;
		}

		// Token: 0x06001076 RID: 4214 RVA: 0x002189F0 File Offset: 0x00217DF0
		private static long TimerToSeconds(long timerValue)
		{
			return timerValue / 10000000L;
		}

		// Token: 0x06001077 RID: 4215 RVA: 0x00218A08 File Offset: 0x00217E08
		internal static string MachineName()
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.AddPermission(new EnvironmentPermission(PermissionState.Unrestricted));
			permissionSet.AddPermission(new SecurityPermission(SecurityPermissionFlag.UnmanagedCode));
			permissionSet.Assert();
			string machineName;
			try
			{
				machineName = Environment.MachineName;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return machineName;
		}

		// Token: 0x06001078 RID: 4216 RVA: 0x00218A68 File Offset: 0x00217E68
		internal static string BuildQuotedString(string quotePrefix, string quoteSuffix, string unQuotedString)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!ADP.IsEmpty(quotePrefix))
			{
				stringBuilder.Append(quotePrefix);
			}
			if (!ADP.IsEmpty(quoteSuffix))
			{
				stringBuilder.Append(unQuotedString.Replace(quoteSuffix, quoteSuffix + quoteSuffix));
				stringBuilder.Append(quoteSuffix);
			}
			else
			{
				stringBuilder.Append(unQuotedString);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001079 RID: 4217 RVA: 0x00218AC0 File Offset: 0x00217EC0
		internal static byte[] ByteArrayFromString(string hexString, string dataTypeName)
		{
			if ((hexString.Length & 1) != 0)
			{
				throw ADP.LiteralValueIsInvalid(dataTypeName);
			}
			char[] array = hexString.ToCharArray();
			byte[] array2 = new byte[hexString.Length / 2];
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			for (int i = 0; i < hexString.Length; i += 2)
			{
				int num = ADP.hexDigits.IndexOf(char.ToLower(array[i], invariantCulture));
				int num2 = ADP.hexDigits.IndexOf(char.ToLower(array[i + 1], invariantCulture));
				if (num < 0 || num2 < 0)
				{
					throw ADP.LiteralValueIsInvalid(dataTypeName);
				}
				array2[i / 2] = (byte)((num << 4) | num2);
			}
			return array2;
		}

		// Token: 0x0600107A RID: 4218 RVA: 0x00218B58 File Offset: 0x00217F58
		internal static void EscapeSpecialCharacters(string unescapedString, StringBuilder escapedString)
		{
			foreach (char c in unescapedString)
			{
				if (".$^{[(|)*+?\\]".IndexOf(c) >= 0)
				{
					escapedString.Append("\\");
				}
				escapedString.Append(c);
			}
		}

		// Token: 0x0600107B RID: 4219 RVA: 0x00218BA4 File Offset: 0x00217FA4
		internal static string FixUpDecimalSeparator(string numericString, bool formatLiteral, string decimalSeparator, char[] exponentSymbols)
		{
			string text;
			if (numericString.IndexOfAny(exponentSymbols) == -1)
			{
				if (ADP.IsEmpty(decimalSeparator))
				{
					decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
				}
				if (formatLiteral)
				{
					text = numericString.Replace(".", decimalSeparator);
				}
				else
				{
					text = numericString.Replace(decimalSeparator, ".");
				}
			}
			else
			{
				text = numericString;
			}
			return text;
		}

		// Token: 0x0600107C RID: 4220 RVA: 0x00218BF8 File Offset: 0x00217FF8
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal static int GetCurrentThreadId()
		{
			return AppDomain.GetCurrentThreadId();
		}

		// Token: 0x0600107D RID: 4221 RVA: 0x00218C0C File Offset: 0x0021800C
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		internal static string GetFullPath(string filename)
		{
			return Path.GetFullPath(filename);
		}

		// Token: 0x0600107E RID: 4222 RVA: 0x00218C20 File Offset: 0x00218020
		internal static string GetComputerNameDnsFullyQualified()
		{
			int num = 256;
			string text;
			if (ADP.IsPlatformNT5)
			{
				SafeNativeMethods.GetComputerNameEx(3, null, ref num);
				StringBuilder stringBuilder = new StringBuilder(num);
				num = stringBuilder.Capacity;
				if (SafeNativeMethods.GetComputerNameEx(3, stringBuilder, ref num) == 0)
				{
					throw ADP.ComputerNameEx(Marshal.GetLastWin32Error());
				}
				text = stringBuilder.ToString();
			}
			else
			{
				text = ADP.MachineName();
			}
			return text;
		}

		// Token: 0x0600107F RID: 4223 RVA: 0x00218C78 File Offset: 0x00218078
		internal static Stream GetFileStream(string filename)
		{
			new FileIOPermission(FileIOPermissionAccess.Read, filename).Assert();
			Stream stream;
			try
			{
				stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return stream;
		}

		// Token: 0x06001080 RID: 4224 RVA: 0x00218CC0 File Offset: 0x002180C0
		internal static FileVersionInfo GetVersionInfo(string filename)
		{
			new FileIOPermission(FileIOPermissionAccess.Read, filename).Assert();
			FileVersionInfo versionInfo;
			try
			{
				versionInfo = FileVersionInfo.GetVersionInfo(filename);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return versionInfo;
		}

		// Token: 0x06001081 RID: 4225 RVA: 0x00218D08 File Offset: 0x00218108
		internal static Stream GetXmlStreamFromValues(string[] values, string errorString)
		{
			if (values.Length != 1)
			{
				throw ADP.ConfigWrongNumberOfValues(errorString);
			}
			return ADP.GetXmlStream(values[0], errorString);
		}

		// Token: 0x06001082 RID: 4226 RVA: 0x00218D2C File Offset: 0x0021812C
		internal static Stream GetXmlStream(string value, string errorString)
		{
			string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
			if (runtimeDirectory == null)
			{
				throw ADP.ConfigUnableToLoadXmlMetaDataFile(errorString);
			}
			StringBuilder stringBuilder = new StringBuilder(runtimeDirectory.Length + "config\\".Length + value.Length);
			stringBuilder.Append(runtimeDirectory);
			stringBuilder.Append("config\\");
			stringBuilder.Append(value);
			string text = stringBuilder.ToString();
			if (ADP.GetFullPath(text) != text)
			{
				throw ADP.ConfigUnableToLoadXmlMetaDataFile(errorString);
			}
			Stream fileStream;
			try
			{
				fileStream = ADP.GetFileStream(text);
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ADP.ConfigUnableToLoadXmlMetaDataFile(errorString);
			}
			return fileStream;
		}

		// Token: 0x06001083 RID: 4227 RVA: 0x00218DDC File Offset: 0x002181DC
		internal static object ClassesRootRegistryValue(string subkey, string queryvalue)
		{
			new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_CLASSES_ROOT\\" + subkey).Assert();
			object obj;
			try
			{
				using (RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(subkey, false))
				{
					obj = ((registryKey != null) ? registryKey.GetValue(queryvalue) : null);
				}
			}
			catch (SecurityException ex)
			{
				ADP.TraceExceptionWithoutRethrow(ex);
				obj = null;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return obj;
		}

		// Token: 0x06001084 RID: 4228 RVA: 0x00218E88 File Offset: 0x00218288
		internal static object LocalMachineRegistryValue(string subkey, string queryvalue)
		{
			new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\" + subkey).Assert();
			object obj;
			try
			{
				using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(subkey, false))
				{
					obj = ((registryKey != null) ? registryKey.GetValue(queryvalue) : null);
				}
			}
			catch (SecurityException ex)
			{
				ADP.TraceExceptionWithoutRethrow(ex);
				obj = null;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return obj;
		}

		// Token: 0x06001085 RID: 4229 RVA: 0x00218F34 File Offset: 0x00218334
		internal static void CheckVersionMDAC(bool ifodbcelseoledb)
		{
			string text;
			int num;
			int num2;
			int num3;
			try
			{
				text = (string)ADP.LocalMachineRegistryValue("Software\\Microsoft\\DataAccess", "FullInstallVer");
				if (ADP.IsEmpty(text))
				{
					string text2 = (string)ADP.ClassesRootRegistryValue("CLSID\\{2206CDB2-19C1-11D1-89E0-00C04FD7A829}\\InprocServer32", ADP.StrEmpty);
					FileVersionInfo versionInfo = ADP.GetVersionInfo(text2);
					num = versionInfo.FileMajorPart;
					num2 = versionInfo.FileMinorPart;
					num3 = versionInfo.FileBuildPart;
					text = versionInfo.FileVersion;
				}
				else
				{
					string[] array = text.Split(new char[] { '.' });
					num = int.Parse(array[0], NumberStyles.None, CultureInfo.InvariantCulture);
					num2 = int.Parse(array[1], NumberStyles.None, CultureInfo.InvariantCulture);
					num3 = int.Parse(array[2], NumberStyles.None, CultureInfo.InvariantCulture);
					int.Parse(array[3], NumberStyles.None, CultureInfo.InvariantCulture);
				}
			}
			catch (Exception ex)
			{
				if (!ADP.IsCatchableExceptionType(ex))
				{
					throw;
				}
				throw ODB.MDACNotAvailable(ex);
			}
			if (num >= 2 && (num != 2 || (num2 >= 60 && (num2 != 60 || num3 >= 6526))))
			{
				return;
			}
			if (ifodbcelseoledb)
			{
				throw ADP.DataAdapter(Res.GetString("Odbc_MDACWrongVersion", new object[] { text }));
			}
			throw ADP.DataAdapter(Res.GetString("OleDb_MDACWrongVersion", new object[] { text }));
		}

		// Token: 0x06001086 RID: 4230 RVA: 0x00219084 File Offset: 0x00218484
		internal static bool RemoveStringQuotes(string quotePrefix, string quoteSuffix, string quotedString, out string unquotedString)
		{
			int num;
			if (quotePrefix == null)
			{
				num = 0;
			}
			else
			{
				num = quotePrefix.Length;
			}
			int num2;
			if (quoteSuffix == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = quoteSuffix.Length;
			}
			if (num2 + num == 0)
			{
				unquotedString = quotedString;
				return true;
			}
			if (quotedString == null)
			{
				unquotedString = quotedString;
				return false;
			}
			int length = quotedString.Length;
			if (length < num + num2)
			{
				unquotedString = quotedString;
				return false;
			}
			if (num > 0 && !quotedString.StartsWith(quotePrefix, StringComparison.Ordinal))
			{
				unquotedString = quotedString;
				return false;
			}
			if (num2 > 0)
			{
				if (!quotedString.EndsWith(quoteSuffix, StringComparison.Ordinal))
				{
					unquotedString = quotedString;
					return false;
				}
				unquotedString = quotedString.Substring(num, length - (num + num2)).Replace(quoteSuffix + quoteSuffix, quoteSuffix);
			}
			else
			{
				unquotedString = quotedString.Substring(num, length - num);
			}
			return true;
		}

		// Token: 0x06001087 RID: 4231 RVA: 0x00219124 File Offset: 0x00218524
		internal static DataRow[] SelectAdapterRows(DataTable dataTable, bool sorted)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			DataRowCollection rows = dataTable.Rows;
			foreach (object obj in rows)
			{
				DataRow dataRow = (DataRow)obj;
				DataRowState rowState = dataRow.RowState;
				if (rowState != DataRowState.Added)
				{
					if (rowState != DataRowState.Deleted)
					{
						if (rowState == DataRowState.Modified)
						{
							num3++;
						}
					}
					else
					{
						num2++;
					}
				}
				else
				{
					num++;
				}
			}
			DataRow[] array = new DataRow[num + num2 + num3];
			if (sorted)
			{
				num3 = num + num2;
				num2 = num;
				num = 0;
				using (IEnumerator enumerator2 = rows.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						DataRow dataRow2 = (DataRow)obj2;
						DataRowState rowState2 = dataRow2.RowState;
						if (rowState2 != DataRowState.Added)
						{
							if (rowState2 != DataRowState.Deleted)
							{
								if (rowState2 == DataRowState.Modified)
								{
									array[num3++] = dataRow2;
								}
							}
							else
							{
								array[num2++] = dataRow2;
							}
						}
						else
						{
							array[num++] = dataRow2;
						}
					}
					return array;
				}
			}
			int num4 = 0;
			foreach (object obj3 in rows)
			{
				DataRow dataRow3 = (DataRow)obj3;
				if ((dataRow3.RowState & (DataRowState.Added | DataRowState.Deleted | DataRowState.Modified)) != (DataRowState)0)
				{
					array[num4++] = dataRow3;
					if (num4 == array.Length)
					{
						break;
					}
				}
			}
			return array;
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x002192D8 File Offset: 0x002186D8
		internal static int StringLength(string inputString)
		{
			if (inputString == null)
			{
				return 0;
			}
			return inputString.Length;
		}

		// Token: 0x06001089 RID: 4233 RVA: 0x002192F0 File Offset: 0x002186F0
		internal static void BuildSchemaTableInfoTableNames(string[] columnNameArray)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>(columnNameArray.Length);
			int num = columnNameArray.Length;
			int num2 = columnNameArray.Length - 1;
			while (0 <= num2)
			{
				string text = columnNameArray[num2];
				if (text != null && 0 < text.Length)
				{
					text = text.ToLower(CultureInfo.InvariantCulture);
					int num3;
					if (dictionary.TryGetValue(text, out num3))
					{
						num = Math.Min(num, num3);
					}
					dictionary[text] = num2;
				}
				else
				{
					columnNameArray[num2] = ADP.StrEmpty;
					num = num2;
				}
				num2--;
			}
			int num4 = 1;
			for (int i = num; i < columnNameArray.Length; i++)
			{
				string text2 = columnNameArray[i];
				if (text2.Length == 0)
				{
					columnNameArray[i] = "Column";
					num4 = ADP.GenerateUniqueName(dictionary, ref columnNameArray[i], i, num4);
				}
				else
				{
					text2 = text2.ToLower(CultureInfo.InvariantCulture);
					if (i != dictionary[text2])
					{
						ADP.GenerateUniqueName(dictionary, ref columnNameArray[i], i, 1);
					}
				}
			}
		}

		// Token: 0x0600108A RID: 4234 RVA: 0x002193C8 File Offset: 0x002187C8
		private static int GenerateUniqueName(Dictionary<string, int> hash, ref string columnName, int index, int uniqueIndex)
		{
			string text;
			string text2;
			for (;;)
			{
				text = columnName + uniqueIndex.ToString(CultureInfo.InvariantCulture);
				text2 = text.ToLower(CultureInfo.InvariantCulture);
				if (!hash.ContainsKey(text2))
				{
					break;
				}
				uniqueIndex++;
			}
			columnName = text;
			hash.Add(text2, index);
			return uniqueIndex;
		}

		// Token: 0x0600108B RID: 4235 RVA: 0x00219414 File Offset: 0x00218814
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal static IntPtr IntPtrOffset(IntPtr pbase, int offset)
		{
			checked
			{
				if (4 == ADP.PtrSize)
				{
					return (IntPtr)(pbase.ToInt32() + offset);
				}
				return (IntPtr)(pbase.ToInt64() + unchecked((long)offset));
			}
		}

		// Token: 0x0600108C RID: 4236 RVA: 0x00219448 File Offset: 0x00218848
		internal static int IntPtrToInt32(IntPtr value)
		{
			if (4 == ADP.PtrSize)
			{
				return (int)value;
			}
			long num = (long)value;
			num = Math.Min(2147483647L, num);
			num = Math.Max(-2147483648L, num);
			return (int)num;
		}

		// Token: 0x0600108D RID: 4237 RVA: 0x00219488 File Offset: 0x00218888
		internal static int SrcCompare(string strA, string strB)
		{
			if (!(strA == strB))
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x002194A4 File Offset: 0x002188A4
		internal static int DstCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x002194C4 File Offset: 0x002188C4
		internal static bool IsDirection(IDataParameter value, ParameterDirection condition)
		{
			return condition == (condition & value.Direction);
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x002194DC File Offset: 0x002188DC
		internal static bool IsEmpty(string str)
		{
			return str == null || 0 == str.Length;
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x002194F8 File Offset: 0x002188F8
		internal static bool IsEmptyArray(string[] array)
		{
			return array == null || 0 == array.Length;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00219510 File Offset: 0x00218910
		internal static bool IsNull(object value)
		{
			if (value == null || DBNull.Value == value)
			{
				return true;
			}
			INullable nullable = value as INullable;
			return nullable != null && nullable.IsNull;
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x0021953C File Offset: 0x0021893C
		internal static bool IsNull(object value, out bool isINullable)
		{
			INullable nullable = value as INullable;
			isINullable = null != nullable;
			return (isINullable && nullable.IsNull) || value == null || DBNull.Value == value;
		}

		// Token: 0x04000AAD RID: 2733
		internal const string Append = "Append";

		// Token: 0x04000AAE RID: 2734
		internal const string BeginExecuteNonQuery = "BeginExecuteNonQuery";

		// Token: 0x04000AAF RID: 2735
		internal const string BeginExecuteReader = "BeginExecuteReader";

		// Token: 0x04000AB0 RID: 2736
		internal const string BeginTransaction = "BeginTransaction";

		// Token: 0x04000AB1 RID: 2737
		internal const string BeginExecuteXmlReader = "BeginExecuteXmlReader";

		// Token: 0x04000AB2 RID: 2738
		internal const string ChangeDatabase = "ChangeDatabase";

		// Token: 0x04000AB3 RID: 2739
		internal const string Cancel = "Cancel";

		// Token: 0x04000AB4 RID: 2740
		internal const string Clone = "Clone";

		// Token: 0x04000AB5 RID: 2741
		internal const string CommitTransaction = "CommitTransaction";

		// Token: 0x04000AB6 RID: 2742
		internal const string CommandTimeout = "CommandTimeout";

		// Token: 0x04000AB7 RID: 2743
		internal const string ConnectionString = "ConnectionString";

		// Token: 0x04000AB8 RID: 2744
		internal const string DataSetColumn = "DataSetColumn";

		// Token: 0x04000AB9 RID: 2745
		internal const string DataSetTable = "DataSetTable";

		// Token: 0x04000ABA RID: 2746
		internal const string Delete = "Delete";

		// Token: 0x04000ABB RID: 2747
		internal const string DeleteCommand = "DeleteCommand";

		// Token: 0x04000ABC RID: 2748
		internal const string DeriveParameters = "DeriveParameters";

		// Token: 0x04000ABD RID: 2749
		internal const string EndExecuteNonQuery = "EndExecuteNonQuery";

		// Token: 0x04000ABE RID: 2750
		internal const string EndExecuteReader = "EndExecuteReader";

		// Token: 0x04000ABF RID: 2751
		internal const string EndExecuteXmlReader = "EndExecuteXmlReader";

		// Token: 0x04000AC0 RID: 2752
		internal const string ExecuteReader = "ExecuteReader";

		// Token: 0x04000AC1 RID: 2753
		internal const string ExecuteRow = "ExecuteRow";

		// Token: 0x04000AC2 RID: 2754
		internal const string ExecuteNonQuery = "ExecuteNonQuery";

		// Token: 0x04000AC3 RID: 2755
		internal const string ExecuteScalar = "ExecuteScalar";

		// Token: 0x04000AC4 RID: 2756
		internal const string ExecuteSqlScalar = "ExecuteSqlScalar";

		// Token: 0x04000AC5 RID: 2757
		internal const string ExecuteXmlReader = "ExecuteXmlReader";

		// Token: 0x04000AC6 RID: 2758
		internal const string Fill = "Fill";

		// Token: 0x04000AC7 RID: 2759
		internal const string FillPage = "FillPage";

		// Token: 0x04000AC8 RID: 2760
		internal const string FillSchema = "FillSchema";

		// Token: 0x04000AC9 RID: 2761
		internal const string GetBytes = "GetBytes";

		// Token: 0x04000ACA RID: 2762
		internal const string GetChars = "GetChars";

		// Token: 0x04000ACB RID: 2763
		internal const string GetOleDbSchemaTable = "GetOleDbSchemaTable";

		// Token: 0x04000ACC RID: 2764
		internal const string GetProperties = "GetProperties";

		// Token: 0x04000ACD RID: 2765
		internal const string GetSchema = "GetSchema";

		// Token: 0x04000ACE RID: 2766
		internal const string GetSchemaTable = "GetSchemaTable";

		// Token: 0x04000ACF RID: 2767
		internal const string GetServerTransactionLevel = "GetServerTransactionLevel";

		// Token: 0x04000AD0 RID: 2768
		internal const string Insert = "Insert";

		// Token: 0x04000AD1 RID: 2769
		internal const string Open = "Open";

		// Token: 0x04000AD2 RID: 2770
		internal const string Parameter = "Parameter";

		// Token: 0x04000AD3 RID: 2771
		internal const string ParameterBuffer = "buffer";

		// Token: 0x04000AD4 RID: 2772
		internal const string ParameterCount = "count";

		// Token: 0x04000AD5 RID: 2773
		internal const string ParameterDestinationType = "destinationType";

		// Token: 0x04000AD6 RID: 2774
		internal const string ParameterName = "ParameterName";

		// Token: 0x04000AD7 RID: 2775
		internal const string ParameterOffset = "offset";

		// Token: 0x04000AD8 RID: 2776
		internal const string ParameterSetPosition = "set_Position";

		// Token: 0x04000AD9 RID: 2777
		internal const string ParameterService = "Service";

		// Token: 0x04000ADA RID: 2778
		internal const string ParameterTimeout = "Timeout";

		// Token: 0x04000ADB RID: 2779
		internal const string ParameterUserData = "UserData";

		// Token: 0x04000ADC RID: 2780
		internal const string Prepare = "Prepare";

		// Token: 0x04000ADD RID: 2781
		internal const string QuoteIdentifier = "QuoteIdentifier";

		// Token: 0x04000ADE RID: 2782
		internal const string Read = "Read";

		// Token: 0x04000ADF RID: 2783
		internal const string Remove = "Remove";

		// Token: 0x04000AE0 RID: 2784
		internal const string RollbackTransaction = "RollbackTransaction";

		// Token: 0x04000AE1 RID: 2785
		internal const string SaveTransaction = "SaveTransaction";

		// Token: 0x04000AE2 RID: 2786
		internal const string SetProperties = "SetProperties";

		// Token: 0x04000AE3 RID: 2787
		internal const string SourceColumn = "SourceColumn";

		// Token: 0x04000AE4 RID: 2788
		internal const string SourceVersion = "SourceVersion";

		// Token: 0x04000AE5 RID: 2789
		internal const string SourceTable = "SourceTable";

		// Token: 0x04000AE6 RID: 2790
		internal const string UnquoteIdentifier = "UnquoteIdentifier";

		// Token: 0x04000AE7 RID: 2791
		internal const string Update = "Update";

		// Token: 0x04000AE8 RID: 2792
		internal const string UpdateCommand = "UpdateCommand";

		// Token: 0x04000AE9 RID: 2793
		internal const string UpdateRows = "UpdateRows";

		// Token: 0x04000AEA RID: 2794
		internal const CompareOptions compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

		// Token: 0x04000AEB RID: 2795
		internal const int DecimalMaxPrecision = 29;

		// Token: 0x04000AEC RID: 2796
		internal const int DecimalMaxPrecision28 = 28;

		// Token: 0x04000AED RID: 2797
		internal const int DefaultCommandTimeout = 30;

		// Token: 0x04000AEE RID: 2798
		internal const int DefaultConnectionTimeout = 15;

		// Token: 0x04000AEF RID: 2799
		internal const float FailoverTimeoutStep = 0.08f;

		// Token: 0x04000AF0 RID: 2800
		private static readonly Type StackOverflowType = typeof(StackOverflowException);

		// Token: 0x04000AF1 RID: 2801
		private static readonly Type OutOfMemoryType = typeof(OutOfMemoryException);

		// Token: 0x04000AF2 RID: 2802
		private static readonly Type ThreadAbortType = typeof(ThreadAbortException);

		// Token: 0x04000AF3 RID: 2803
		private static readonly Type NullReferenceType = typeof(NullReferenceException);

		// Token: 0x04000AF4 RID: 2804
		private static readonly Type AccessViolationType = typeof(AccessViolationException);

		// Token: 0x04000AF5 RID: 2805
		private static readonly Type SecurityType = typeof(SecurityException);

		// Token: 0x04000AF6 RID: 2806
		internal static readonly string StrEmpty = "";

		// Token: 0x04000AF7 RID: 2807
		internal static readonly IntPtr PtrZero = new IntPtr(0);

		// Token: 0x04000AF8 RID: 2808
		internal static readonly int PtrSize = IntPtr.Size;

		// Token: 0x04000AF9 RID: 2809
		internal static readonly IntPtr InvalidPtr = new IntPtr(-1);

		// Token: 0x04000AFA RID: 2810
		internal static readonly IntPtr RecordsUnaffected = new IntPtr(-1);

		// Token: 0x04000AFB RID: 2811
		internal static readonly HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		// Token: 0x04000AFC RID: 2812
		internal static readonly int CharSize = 2;

		// Token: 0x04000AFD RID: 2813
		internal static readonly bool IsWindowsNT = PlatformID.Win32NT == Environment.OSVersion.Platform;

		// Token: 0x04000AFE RID: 2814
		internal static readonly bool IsPlatformNT5 = ADP.IsWindowsNT && Environment.OSVersion.Version.Major >= 5;

		// Token: 0x04000AFF RID: 2815
		private static readonly string hexDigits = "0123456789abcdef";

		// Token: 0x02000107 RID: 263
		internal enum ConnectionError
		{
			// Token: 0x04000B01 RID: 2817
			BeginGetConnectionReturnsNull,
			// Token: 0x04000B02 RID: 2818
			GetConnectionReturnsNull,
			// Token: 0x04000B03 RID: 2819
			ConnectionOptionsMissing,
			// Token: 0x04000B04 RID: 2820
			CouldNotSwitchToClosedPreviouslyOpenedState
		}

		// Token: 0x02000108 RID: 264
		internal enum InternalErrorCode
		{
			// Token: 0x04000B06 RID: 2822
			UnpooledObjectHasOwner,
			// Token: 0x04000B07 RID: 2823
			UnpooledObjectHasWrongOwner,
			// Token: 0x04000B08 RID: 2824
			PushingObjectSecondTime,
			// Token: 0x04000B09 RID: 2825
			PooledObjectHasOwner,
			// Token: 0x04000B0A RID: 2826
			PooledObjectInPoolMoreThanOnce,
			// Token: 0x04000B0B RID: 2827
			CreateObjectReturnedNull,
			// Token: 0x04000B0C RID: 2828
			NewObjectCannotBePooled,
			// Token: 0x04000B0D RID: 2829
			NonPooledObjectUsedMoreThanOnce,
			// Token: 0x04000B0E RID: 2830
			AttemptingToPoolOnRestrictedToken,
			// Token: 0x04000B0F RID: 2831
			ConvertSidToStringSidWReturnedNull = 10,
			// Token: 0x04000B10 RID: 2832
			AttemptingToConstructReferenceCollectionOnStaticObject = 12,
			// Token: 0x04000B11 RID: 2833
			AttemptingToEnlistTwice,
			// Token: 0x04000B12 RID: 2834
			CreateReferenceCollectionReturnedNull,
			// Token: 0x04000B13 RID: 2835
			PooledObjectWithoutPool,
			// Token: 0x04000B14 RID: 2836
			UnexpectedWaitAnyResult,
			// Token: 0x04000B15 RID: 2837
			NameValuePairNext = 20,
			// Token: 0x04000B16 RID: 2838
			InvalidParserState1,
			// Token: 0x04000B17 RID: 2839
			InvalidParserState2,
			// Token: 0x04000B18 RID: 2840
			InvalidParserState3,
			// Token: 0x04000B19 RID: 2841
			InvalidBuffer = 30,
			// Token: 0x04000B1A RID: 2842
			UnimplementedSMIMethod = 40,
			// Token: 0x04000B1B RID: 2843
			InvalidSmiCall,
			// Token: 0x04000B1C RID: 2844
			SqlDependencyObtainProcessDispatcherFailureObjectHandle = 50,
			// Token: 0x04000B1D RID: 2845
			SqlDependencyProcessDispatcherFailureCreateInstance,
			// Token: 0x04000B1E RID: 2846
			SqlDependencyProcessDispatcherFailureAppDomain,
			// Token: 0x04000B1F RID: 2847
			SqlDependencyCommandHashIsNotAssociatedWithNotification,
			// Token: 0x04000B20 RID: 2848
			UnknownTransactionFailure = 60
		}
	}
}
