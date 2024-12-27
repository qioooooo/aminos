using System;
using System.Collections;
using System.Configuration;
using System.Data.OracleClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Transactions;

namespace System.Data.Common
{
	// Token: 0x02000018 RID: 24
	internal sealed class ADP
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x00056F14 File Offset: 0x00056314
		internal static int SrcCompare(string strA, string strB)
		{
			if (!(strA == strB))
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00056F30 File Offset: 0x00056330
		internal static int DstCompare(string strA, string strB)
		{
			return CultureInfo.CurrentCulture.CompareInfo.Compare(strA, strB, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00056F50 File Offset: 0x00056350
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

		// Token: 0x060000DB RID: 219 RVA: 0x00056FE0 File Offset: 0x000563E0
		internal static void CheckArgumentLength(string value, string parameterName)
		{
			ADP.CheckArgumentNull(value, parameterName);
			if (value.Length == 0)
			{
				throw ADP.Argument(Res.GetString("ADP_EmptyString", new object[] { parameterName }));
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00057018 File Offset: 0x00056418
		internal static bool CompareInsensitiveInvariant(string strvalue, string strconst)
		{
			return 0 == CultureInfo.InvariantCulture.CompareInfo.Compare(strvalue, strconst, CompareOptions.IgnoreCase);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0005703C File Offset: 0x0005643C
		internal static bool IsEmptyArray(string[] array)
		{
			return array == null || 0 == array.Length;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00057054 File Offset: 0x00056454
		internal static Exception CollectionNullValue(string parameter, Type collection, Type itemType)
		{
			return ADP.ArgumentNull(parameter, Res.GetString("ADP_CollectionNullValue", new object[] { collection.Name, itemType.Name }));
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0005708C File Offset: 0x0005648C
		internal static Exception CollectionIndexInt32(int index, Type collection, int count)
		{
			return ADP.IndexOutOfRange(Res.GetString("ADP_CollectionIndexInt32", new object[]
			{
				index.ToString(CultureInfo.InvariantCulture),
				collection.Name,
				count.ToString(CultureInfo.InvariantCulture)
			}));
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000570D8 File Offset: 0x000564D8
		internal static Exception CollectionIndexString(Type itemType, string propertyName, string propertyValue, Type collection)
		{
			return ADP.IndexOutOfRange(Res.GetString("ADP_CollectionIndexString", new object[] { itemType.Name, propertyName, propertyValue, collection.Name }));
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00057118 File Offset: 0x00056518
		internal static Exception CollectionInvalidType(Type collection, Type itemType, object invalidValue)
		{
			return ADP.InvalidCast(Res.GetString("ADP_CollectionInvalidType", new object[]
			{
				collection.Name,
				itemType.Name,
				invalidValue.GetType().Name
			}));
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0005715C File Offset: 0x0005655C
		internal static ArgumentException CollectionRemoveInvalidObject(Type itemType, ICollection collection)
		{
			return ADP.Argument(Res.GetString("ADP_CollectionRemoveInvalidObject", new object[]
			{
				itemType.Name,
				collection.GetType().Name
			}));
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00057198 File Offset: 0x00056598
		internal static Exception ConnectionAlreadyOpen(ConnectionState state)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ConnectionAlreadyOpen", new object[] { ADP.ConnectionStateMsg(state) }));
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x000571C8 File Offset: 0x000565C8
		internal static ArgumentException ConnectionStringSyntax(int index)
		{
			return ADP.Argument(Res.GetString("ADP_ConnectionStringSyntax", new object[] { index }));
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000571F8 File Offset: 0x000565F8
		internal static Exception InvalidDataDirectory()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InvalidDataDirectory"));
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00057214 File Offset: 0x00056614
		internal static ArgumentException InvalidKeyname(string parameterName)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidKey"), parameterName);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00057234 File Offset: 0x00056634
		internal static ArgumentException InvalidValue(string parameterName)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidValue"), parameterName);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00057254 File Offset: 0x00056654
		internal static Exception DataReaderClosed(string method)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DataReaderClosed", new object[] { method }));
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0005727C File Offset: 0x0005667C
		internal static Exception InvalidXMLBadVersion()
		{
			return ADP.Argument(Res.GetString("ADP_InvalidXMLBadVersion"));
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00057298 File Offset: 0x00056698
		internal static Exception NotAPermissionElement()
		{
			return ADP.Argument(Res.GetString("ADP_NotAPermissionElement"));
		}

		// Token: 0x060000EB RID: 235 RVA: 0x000572B4 File Offset: 0x000566B4
		internal static Exception PermissionTypeMismatch()
		{
			return ADP.Argument(Res.GetString("ADP_PermissionTypeMismatch"));
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000572D0 File Offset: 0x000566D0
		internal static Exception InternalConnectionError(ADP.ConnectionError internalError)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InternalConnectionError", new object[] { (int)internalError }));
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00057300 File Offset: 0x00056700
		internal static Exception InternalError(ADP.InternalErrorCode internalError)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InternalProviderError", new object[] { (int)internalError }));
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00057330 File Offset: 0x00056730
		internal static Exception InvalidConnectionOptionValue(string key, Exception inner)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidConnectionOptionValue", new object[] { key }), inner);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0005735C File Offset: 0x0005675C
		internal static Exception InvalidEnumerationValue(Type type, int value)
		{
			return ADP.ArgumentOutOfRange(Res.GetString("ADP_InvalidEnumerationValue", new object[]
			{
				type.Name,
				value.ToString(CultureInfo.InvariantCulture)
			}), type.Name);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000573A0 File Offset: 0x000567A0
		internal static Exception InvalidDataRowVersion(DataRowVersion value)
		{
			return ADP.InvalidEnumerationValue(typeof(DataRowVersion), (int)value);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x000573C0 File Offset: 0x000567C0
		internal static Exception InvalidKeyRestrictionBehavior(KeyRestrictionBehavior value)
		{
			return ADP.InvalidEnumerationValue(typeof(KeyRestrictionBehavior), (int)value);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000573E0 File Offset: 0x000567E0
		internal static Exception InvalidOffsetValue(int value)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidOffsetValue", new object[] { value.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00057414 File Offset: 0x00056814
		internal static Exception InvalidParameterDirection(ParameterDirection value)
		{
			return ADP.InvalidEnumerationValue(typeof(ParameterDirection), (int)value);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00057434 File Offset: 0x00056834
		internal static Exception InvalidParameterType(IDataParameterCollection collection, Type parameterType, object invalidValue)
		{
			return ADP.CollectionInvalidType(collection.GetType(), parameterType, invalidValue);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00057450 File Offset: 0x00056850
		internal static Exception InvalidPermissionState(PermissionState value)
		{
			return ADP.InvalidEnumerationValue(typeof(PermissionState), (int)value);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00057470 File Offset: 0x00056870
		internal static Exception InvalidUpdateRowSource(UpdateRowSource value)
		{
			return ADP.InvalidEnumerationValue(typeof(UpdateRowSource), (int)value);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00057490 File Offset: 0x00056890
		internal static Exception MethodNotImplemented(string methodName)
		{
			NotImplementedException ex = new NotImplementedException(methodName);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x000574AC File Offset: 0x000568AC
		internal static Exception NoConnectionString()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NoConnectionString"));
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000574C8 File Offset: 0x000568C8
		internal static Exception ParameterNull(string parameter, IDataParameterCollection collection, Type parameterType)
		{
			return ADP.CollectionNullValue(parameter, collection.GetType(), parameterType);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000574E4 File Offset: 0x000568E4
		internal static Exception ParametersIsNotParent(Type parameterType, IDataParameterCollection collection)
		{
			return ADP.Argument(Res.GetString("ADP_CollectionIsNotParent", new object[]
			{
				parameterType.Name,
				collection.GetType().Name
			}));
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00057520 File Offset: 0x00056920
		internal static ArgumentException ParametersIsParent(Type parameterType, IDataParameterCollection collection)
		{
			return ADP.Argument(Res.GetString("ADP_CollectionIsNotParent", new object[]
			{
				parameterType.Name,
				collection.GetType().Name
			}));
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0005755C File Offset: 0x0005695C
		internal static Exception ParametersMappingIndex(int index, IDataParameterCollection collection)
		{
			return ADP.CollectionIndexInt32(index, collection.GetType(), collection.Count);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0005757C File Offset: 0x0005697C
		internal static Exception ParametersSourceIndex(string parameterName, IDataParameterCollection collection, Type parameterType)
		{
			return ADP.CollectionIndexString(parameterType, "ParameterName", parameterName, collection.GetType());
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0005759C File Offset: 0x0005699C
		internal static Exception PooledOpenTimeout()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_PooledOpenTimeout"));
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000575B8 File Offset: 0x000569B8
		internal static Exception OpenConnectionPropertySet(string property, ConnectionState state)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_OpenConnectionPropertySet", new object[]
			{
				property,
				ADP.ConnectionStateMsg(state)
			}));
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000575EC File Offset: 0x000569EC
		internal static Exception AmbigousCollectionName(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_AmbigousCollectionName", new object[] { collectionName }));
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00057614 File Offset: 0x00056A14
		internal static Exception CollectionNameIsNotUnique(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_CollectionNameISNotUnique", new object[] { collectionName }));
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0005763C File Offset: 0x00056A3C
		internal static Exception DataTableDoesNotExist(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_DataTableDoesNotExist", new object[] { collectionName }));
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00057664 File Offset: 0x00056A64
		internal static Exception IncorrectNumberOfDataSourceInformationRows()
		{
			return ADP.Argument(Res.GetString("MDF_IncorrectNumberOfDataSourceInformationRows"));
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00057680 File Offset: 0x00056A80
		internal static Exception InvalidXml()
		{
			return ADP.Argument(Res.GetString("MDF_InvalidXml"));
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0005769C File Offset: 0x00056A9C
		internal static Exception InvalidXmlMissingColumn(string collectionName, string columnName)
		{
			return ADP.Argument(Res.GetString("MDF_InvalidXmlMissingColumn", new object[] { collectionName, columnName }));
		}

		// Token: 0x06000106 RID: 262 RVA: 0x000576C8 File Offset: 0x00056AC8
		internal static Exception InvalidXmlInvalidValue(string collectionName, string columnName)
		{
			return ADP.Argument(Res.GetString("MDF_InvalidXmlInvalidValue", new object[] { collectionName, columnName }));
		}

		// Token: 0x06000107 RID: 263 RVA: 0x000576F4 File Offset: 0x00056AF4
		internal static Exception MissingDataSourceInformationColumn()
		{
			return ADP.Argument(Res.GetString("MDF_MissingDataSourceInformationColumn"));
		}

		// Token: 0x06000108 RID: 264 RVA: 0x00057710 File Offset: 0x00056B10
		internal static Exception MissingRestrictionColumn()
		{
			return ADP.Argument(Res.GetString("MDF_MissingRestrictionColumn"));
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0005772C File Offset: 0x00056B2C
		internal static Exception MissingRestrictionRow()
		{
			return ADP.Argument(Res.GetString("MDF_MissingRestrictionRow"));
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00057748 File Offset: 0x00056B48
		internal static Exception NoColumns()
		{
			return ADP.Argument(Res.GetString("MDF_NoColumns"));
		}

		// Token: 0x0600010B RID: 267 RVA: 0x00057764 File Offset: 0x00056B64
		internal static Exception QueryFailed(string collectionName, Exception e)
		{
			return ADP.InvalidOperation(Res.GetString("MDF_QueryFailed", new object[] { collectionName }), e);
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00057790 File Offset: 0x00056B90
		internal static Exception TooManyRestrictions(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_TooManyRestrictions", new object[] { collectionName }));
		}

		// Token: 0x0600010D RID: 269 RVA: 0x000577B8 File Offset: 0x00056BB8
		internal static Exception UndefinedCollection(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_UndefinedCollection", new object[] { collectionName }));
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000577E0 File Offset: 0x00056BE0
		internal static Exception UndefinedPopulationMechanism(string populationMechanism)
		{
			return ADP.Argument(Res.GetString("MDF_UndefinedPopulationMechanism", new object[] { populationMechanism }));
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00057808 File Offset: 0x00056C08
		internal static Exception UnsupportedVersion(string collectionName)
		{
			return ADP.Argument(Res.GetString("MDF_UnsupportedVersion", new object[] { collectionName }));
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00057830 File Offset: 0x00056C30
		private ADP()
		{
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00057844 File Offset: 0x00056C44
		private static void TraceException(string trace, Exception e)
		{
			if (e != null)
			{
				Bid.Trace(trace, e.ToString());
			}
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00057860 File Offset: 0x00056C60
		internal static Exception TraceException(Exception e)
		{
			ADP.TraceExceptionAsReturnValue(e);
			return e;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00057874 File Offset: 0x00056C74
		internal static void TraceExceptionAsReturnValue(Exception e)
		{
			ADP.TraceException("<oc|ERR|THROW> '%ls'\n", e);
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0005788C File Offset: 0x00056C8C
		internal static void TraceExceptionForCapture(Exception e)
		{
			ADP.TraceException("<comm.ADP.TraceException|ERR|CATCH> '%ls'\n", e);
		}

		// Token: 0x06000115 RID: 277 RVA: 0x000578A4 File Offset: 0x00056CA4
		internal static void TraceExceptionWithoutRethrow(Exception e)
		{
			ADP.TraceException("<oc|ERR|CATCH> '%ls'\n", e);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x000578BC File Offset: 0x00056CBC
		internal static ArgumentException Argument(string error)
		{
			ArgumentException ex = new ArgumentException(error);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000578D8 File Offset: 0x00056CD8
		internal static ArgumentException Argument(string error, string parameter)
		{
			ArgumentException ex = new ArgumentException(error, parameter);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000578F4 File Offset: 0x00056CF4
		internal static ArgumentException Argument(string error, Exception inner)
		{
			ArgumentException ex = new ArgumentException(error, inner);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00057910 File Offset: 0x00056D10
		internal static ArgumentNullException ArgumentNull(string parameter)
		{
			ArgumentNullException ex = new ArgumentNullException(parameter);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0005792C File Offset: 0x00056D2C
		internal static ArgumentNullException ArgumentNull(string parameter, string error)
		{
			ArgumentNullException ex = new ArgumentNullException(parameter, error);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00057948 File Offset: 0x00056D48
		internal static ArgumentOutOfRangeException ArgumentOutOfRange(string argName, string message)
		{
			ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException(argName, message);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00057964 File Offset: 0x00056D64
		internal static ConfigurationException Configuration(string message)
		{
			ConfigurationException ex = new ConfigurationErrorsException(message);
			ADP.TraceExceptionAsReturnValue(ex);
			return ex;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00057980 File Offset: 0x00056D80
		internal static Exception ProviderException(string error)
		{
			return ADP.InvalidOperation(error);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00057994 File Offset: 0x00056D94
		internal static Exception IndexOutOfRange(string error)
		{
			return ADP.TraceException(new IndexOutOfRangeException(error));
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000579AC File Offset: 0x00056DAC
		internal static Exception InvalidCast()
		{
			return ADP.TraceException(new InvalidCastException());
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000579C4 File Offset: 0x00056DC4
		internal static Exception InvalidCast(string error)
		{
			return ADP.TraceException(new InvalidCastException(error));
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000579DC File Offset: 0x00056DDC
		internal static Exception InvalidOperation(string error)
		{
			return ADP.TraceException(new InvalidOperationException(error));
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000579F4 File Offset: 0x00056DF4
		internal static Exception InvalidOperation(string error, Exception inner)
		{
			return ADP.TraceException(new InvalidOperationException(error, inner));
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00057A10 File Offset: 0x00056E10
		internal static Exception NotSupported()
		{
			return ADP.TraceException(new NotSupportedException());
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00057A28 File Offset: 0x00056E28
		internal static Exception NotSupported(string message)
		{
			return ADP.TraceException(new NotSupportedException(message));
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00057A40 File Offset: 0x00056E40
		internal static Exception ObjectDisposed(string name)
		{
			return ADP.TraceException(new ObjectDisposedException(name));
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00057A58 File Offset: 0x00056E58
		internal static Exception OracleError(OciErrorHandle errorHandle, int rc)
		{
			return ADP.TraceException(OracleException.CreateException(errorHandle, rc));
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00057A74 File Offset: 0x00056E74
		internal static Exception OracleError(int rc, OracleInternalConnection internalConnection)
		{
			return ADP.TraceException(OracleException.CreateException(rc, internalConnection));
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00057A90 File Offset: 0x00056E90
		internal static Exception Overflow(string error)
		{
			return ADP.TraceException(new OverflowException(error));
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00057AA8 File Offset: 0x00056EA8
		internal static Exception Simple(string message)
		{
			return ADP.TraceException(new Exception(message));
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00057AC0 File Offset: 0x00056EC0
		internal static Exception BadBindValueType(Type valueType, OracleType oracleType)
		{
			return ADP.InvalidCast(Res.GetString("ADP_BadBindValueType", new object[]
			{
				valueType.ToString(),
				oracleType.ToString()
			}));
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00057AFC File Offset: 0x00056EFC
		internal static Exception UnsupportedOracleDateTimeBinding(OracleType dtType)
		{
			return ADP.ArgumentOutOfRange("", Res.GetString("ADP_BadBindValueType", new object[]
			{
				typeof(OracleDateTime).ToString(),
				dtType.ToString()
			}));
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00057B48 File Offset: 0x00056F48
		internal static Exception BadOracleClientImageFormat(Exception e)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_BadOracleClientImageFormat"), e);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00057B68 File Offset: 0x00056F68
		internal static Exception BadOracleClientVersion()
		{
			return ADP.Simple(Res.GetString("ADP_BadOracleClientVersion"));
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00057B84 File Offset: 0x00056F84
		internal static Exception BufferExceeded(string argName)
		{
			return ADP.ArgumentOutOfRange(argName, Res.GetString("ADP_BufferExceeded"));
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00057BA4 File Offset: 0x00056FA4
		internal static Exception CannotDeriveOverloaded()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_CannotDeriveOverloaded"));
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00057BC0 File Offset: 0x00056FC0
		internal static Exception CannotOpenLobWithDifferentMode(OracleLobOpenMode newmode, OracleLobOpenMode current)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_CannotOpenLobWithDifferentMode", new object[]
			{
				newmode.ToString(),
				current.ToString()
			}));
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00057C00 File Offset: 0x00057000
		internal static Exception ChangeDatabaseNotSupported()
		{
			return ADP.NotSupported(Res.GetString("ADP_ChangeDatabaseNotSupported"));
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00057C1C File Offset: 0x0005701C
		internal static Exception ClosedConnectionError()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ClosedConnectionError"));
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00057C38 File Offset: 0x00057038
		internal static Exception ClosedDataReaderError()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ClosedDataReaderError"));
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00057C54 File Offset: 0x00057054
		internal static Exception CommandTextRequired(string method)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_CommandTextRequired", new object[] { method }));
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00057C7C File Offset: 0x0005707C
		internal static ConfigurationException ConfigUnableToLoadXmlMetaDataFile(string settingName)
		{
			return ADP.Configuration(Res.GetString("ADP_ConfigUnableToLoadXmlMetaDataFile", new object[] { settingName }));
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00057CA4 File Offset: 0x000570A4
		internal static ConfigurationException ConfigWrongNumberOfValues(string settingName)
		{
			return ADP.Configuration(Res.GetString("ADP_ConfigWrongNumberOfValues", new object[] { settingName }));
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00057CCC File Offset: 0x000570CC
		internal static Exception ConnectionRequired(string method)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_ConnectionRequired", new object[] { method }));
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00057CF4 File Offset: 0x000570F4
		internal static Exception CouldNotCreateEnvironment(string methodname, int rc)
		{
			return ADP.Simple(Res.GetString("ADP_CouldNotCreateEnvironment", new object[]
			{
				methodname,
				rc.ToString(CultureInfo.CurrentCulture)
			}));
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00057D2C File Offset: 0x0005712C
		internal static ArgumentException ConvertFailed(Type fromType, Type toType, Exception innerException)
		{
			return ADP.Argument(Res.GetString("ADP_ConvertFailed", new object[] { fromType.FullName, toType.FullName }), innerException);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00057D64 File Offset: 0x00057164
		internal static Exception DataIsNull()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DataIsNull"));
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00057D80 File Offset: 0x00057180
		internal static Exception DataReaderNoData()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DataReaderNoData"));
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00057D9C File Offset: 0x0005719C
		internal static Exception DeriveParametersNotSupported(IDbCommand value)
		{
			return ADP.ProviderException(Res.GetString("ADP_DeriveParametersNotSupported", new object[]
			{
				value.GetType().Name,
				value.CommandType.ToString()
			}));
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00057DE4 File Offset: 0x000571E4
		internal static Exception DistribTxRequiresOracle9i()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DistribTxRequiresOracle9i"));
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00057E00 File Offset: 0x00057200
		internal static Exception DistribTxRequiresOracleServicesForMTS(Exception inner)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_DistribTxRequiresOracleServicesForMTS"), inner);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00057E20 File Offset: 0x00057220
		internal static Exception IdentifierIsNotQuoted()
		{
			return ADP.Argument(Res.GetString("ADP_IdentifierIsNotQuoted"));
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00057E3C File Offset: 0x0005723C
		internal static Exception InputRefCursorNotSupported(string parameterName)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InputRefCursorNotSupported", new object[] { parameterName }));
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00057E64 File Offset: 0x00057264
		internal static Exception InvalidCommandType(CommandType cmdType)
		{
			string text = "ADP_InvalidCommandType";
			object[] array = new object[1];
			object[] array2 = array;
			int num = 0;
			int num2 = (int)cmdType;
			array2[num] = num2.ToString(CultureInfo.CurrentCulture);
			return ADP.Argument(Res.GetString(text, array));
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00057E9C File Offset: 0x0005729C
		internal static Exception InvalidConnectionOptionLength(string key, int maxLength)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidConnectionOptionLength", new object[] { key, maxLength }));
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00057ED0 File Offset: 0x000572D0
		internal static Exception InvalidConnectionOptionValue(string key)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidConnectionOptionValue", new object[] { key }));
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00057EF8 File Offset: 0x000572F8
		internal static Exception InvalidDataLength(long length)
		{
			return ADP.IndexOutOfRange(Res.GetString("ADP_InvalidDataLength", new object[] { length.ToString(CultureInfo.CurrentCulture) }));
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00057F2C File Offset: 0x0005732C
		internal static Exception InvalidDataType(TypeCode tc)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidDataType", new object[] { tc.ToString() }));
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00057F60 File Offset: 0x00057360
		internal static Exception InvalidDataTypeForValue(Type dataType, TypeCode tc)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidDataTypeForValue", new object[]
			{
				dataType.ToString(),
				tc.ToString()
			}));
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00057F9C File Offset: 0x0005739C
		internal static Exception InvalidDbType(DbType dbType)
		{
			return ADP.ArgumentOutOfRange("dbType", Res.GetString("ADP_InvalidDbType", new object[] { dbType.ToString() }));
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00057FD4 File Offset: 0x000573D4
		internal static Exception InvalidDestinationBufferIndex(int maxLen, int dstOffset, string parameterName)
		{
			return ADP.ArgumentOutOfRange(parameterName, Res.GetString("ADP_InvalidDestinationBufferIndex", new object[]
			{
				maxLen.ToString(CultureInfo.CurrentCulture),
				dstOffset.ToString(CultureInfo.CurrentCulture)
			}));
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00058018 File Offset: 0x00057418
		internal static Exception InvalidLobType(OracleType oracleType)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_InvalidLobType", new object[] { oracleType.ToString() }));
		}

		// Token: 0x0600014A RID: 330 RVA: 0x0005804C File Offset: 0x0005744C
		internal static Exception InvalidMinMaxPoolSizeValues()
		{
			return ADP.Argument(Res.GetString("ADP_InvalidMinMaxPoolSizeValues"));
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00058068 File Offset: 0x00057468
		internal static Exception InvalidOracleType(OracleType oracleType)
		{
			return ADP.ArgumentOutOfRange("oracleType", Res.GetString("ADP_InvalidOracleType", new object[] { oracleType.ToString() }));
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000580A0 File Offset: 0x000574A0
		internal static Exception InvalidSeekOrigin(SeekOrigin origin)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidSeekOrigin", new object[] { origin.ToString() }));
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000580D4 File Offset: 0x000574D4
		internal static Exception InvalidSizeValue(int value)
		{
			return ADP.Argument(Res.GetString("ADP_InvalidSizeValue", new object[] { value.ToString(CultureInfo.InvariantCulture) }));
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00058108 File Offset: 0x00057508
		internal static ArgumentException KeywordNotSupported(string keyword)
		{
			return ADP.Argument(Res.GetString("ADP_KeywordNotSupported", new object[] { keyword }));
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00058130 File Offset: 0x00057530
		internal static Exception InvalidSourceBufferIndex(int maxLen, long srcOffset, string parameterName)
		{
			return ADP.ArgumentOutOfRange(parameterName, Res.GetString("ADP_InvalidSourceBufferIndex", new object[]
			{
				maxLen.ToString(CultureInfo.CurrentCulture),
				srcOffset.ToString(CultureInfo.CurrentCulture)
			}));
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00058174 File Offset: 0x00057574
		internal static Exception InvalidSourceOffset(string argName, long minValue, long maxValue)
		{
			return ADP.ArgumentOutOfRange(argName, Res.GetString("ADP_InvalidSourceOffset", new object[]
			{
				minValue.ToString(CultureInfo.CurrentCulture),
				maxValue.ToString(CultureInfo.CurrentCulture)
			}));
		}

		// Token: 0x06000151 RID: 337 RVA: 0x000581B8 File Offset: 0x000575B8
		internal static Exception LobAmountExceeded(string argName)
		{
			return ADP.ArgumentOutOfRange(argName, Res.GetString("ADP_LobAmountExceeded"));
		}

		// Token: 0x06000152 RID: 338 RVA: 0x000581D8 File Offset: 0x000575D8
		internal static Exception LobAmountMustBeEven(string argName)
		{
			return ADP.ArgumentOutOfRange(argName, Res.GetString("ADP_LobAmountMustBeEven"));
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000581F8 File Offset: 0x000575F8
		internal static Exception LobPositionMustBeEven()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_LobPositionMustBeEven"));
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00058214 File Offset: 0x00057614
		internal static Exception LobWriteInvalidOnNull()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_LobWriteInvalidOnNull"));
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00058230 File Offset: 0x00057630
		internal static Exception LobWriteRequiresTransaction()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_LobWriteRequiresTransaction"));
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0005824C File Offset: 0x0005764C
		internal static Exception MonthOutOfRange()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_MonthOutOfRange"));
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00058268 File Offset: 0x00057668
		internal static Exception MustBePositive(string argName)
		{
			return ADP.ArgumentOutOfRange(argName, Res.GetString("ADP_MustBePositive"));
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00058288 File Offset: 0x00057688
		internal static Exception NoCommandText()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NoCommandText"));
		}

		// Token: 0x06000159 RID: 345 RVA: 0x000582A4 File Offset: 0x000576A4
		internal static Exception NoData()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NoData"));
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000582C0 File Offset: 0x000576C0
		internal static Exception NoLocalTransactionInDistributedContext()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NoLocalTransactionInDistributedContext"));
		}

		// Token: 0x0600015B RID: 347 RVA: 0x000582DC File Offset: 0x000576DC
		internal static Exception NoOptimizedDirectTableAccess()
		{
			return ADP.Argument(Res.GetString("ADP_NoOptimizedDirectTableAccess"));
		}

		// Token: 0x0600015C RID: 348 RVA: 0x000582F8 File Offset: 0x000576F8
		internal static Exception NoParallelTransactions()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_NoParallelTransactions"));
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00058314 File Offset: 0x00057714
		internal static Exception OpenConnectionRequired(string method, ConnectionState state)
		{
			return ADP.InvalidOperation(Res.GetString("ADP_OpenConnectionRequired", new object[]
			{
				method,
				"ConnectionState",
				state.ToString()
			}));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00058354 File Offset: 0x00057754
		internal static Exception OperationFailed(string method, int rc)
		{
			return ADP.Simple(Res.GetString("ADP_OperationFailed", new object[] { method, rc }));
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00058388 File Offset: 0x00057788
		internal static Exception OperationResultedInOverflow()
		{
			return ADP.Overflow(Res.GetString("ADP_OperationResultedInOverflow"));
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000583A4 File Offset: 0x000577A4
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

		// Token: 0x06000161 RID: 353 RVA: 0x0005842C File Offset: 0x0005782C
		internal static Exception ParameterSizeIsTooLarge(string parameterName)
		{
			return ADP.Simple(Res.GetString("ADP_ParameterSizeIsTooLarge", new object[] { parameterName }));
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00058454 File Offset: 0x00057854
		internal static Exception ParameterSizeIsMissing(string parameterName, Type dataType)
		{
			return ADP.Simple(Res.GetString("ADP_ParameterSizeIsMissing", new object[] { parameterName, dataType.Name }));
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00058488 File Offset: 0x00057888
		internal static Exception ReadOnlyLob()
		{
			return ADP.NotSupported(Res.GetString("ADP_ReadOnlyLob"));
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000584A4 File Offset: 0x000578A4
		internal static Exception SeekBeyondEnd(string parameter)
		{
			return ADP.ArgumentOutOfRange(parameter, Res.GetString("ADP_SeekBeyondEnd"));
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000584C4 File Offset: 0x000578C4
		internal static Exception SyntaxErrorExpectedCommaAfterColumn()
		{
			return ADP.TraceException(ADP.InvalidOperation(Res.GetString("ADP_SyntaxErrorExpectedCommaAfterColumn")));
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000584E8 File Offset: 0x000578E8
		internal static Exception SyntaxErrorExpectedCommaAfterTable()
		{
			return ADP.TraceException(ADP.InvalidOperation(Res.GetString("ADP_SyntaxErrorExpectedCommaAfterTable")));
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0005850C File Offset: 0x0005790C
		internal static Exception SyntaxErrorExpectedIdentifier()
		{
			return ADP.TraceException(ADP.InvalidOperation(Res.GetString("ADP_SyntaxErrorExpectedIdentifier")));
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00058530 File Offset: 0x00057930
		internal static Exception SyntaxErrorExpectedNextPart()
		{
			return ADP.TraceException(ADP.InvalidOperation(Res.GetString("ADP_SyntaxErrorExpectedNextPart")));
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00058554 File Offset: 0x00057954
		internal static Exception SyntaxErrorMissingParenthesis()
		{
			return ADP.TraceException(ADP.InvalidOperation(Res.GetString("ADP_SyntaxErrorMissingParenthesis")));
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00058578 File Offset: 0x00057978
		internal static Exception SyntaxErrorTooManyNameParts()
		{
			return ADP.TraceException(ADP.InvalidOperation(Res.GetString("ADP_SyntaxErrorTooManyNameParts")));
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0005859C File Offset: 0x0005799C
		internal static Exception TransactionCompleted()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_TransactionCompleted"));
		}

		// Token: 0x0600016C RID: 364 RVA: 0x000585B8 File Offset: 0x000579B8
		internal static Exception TransactionConnectionMismatch()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_TransactionConnectionMismatch"));
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000585D4 File Offset: 0x000579D4
		internal static Exception TransactionPresent()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_TransactionPresent"));
		}

		// Token: 0x0600016E RID: 366 RVA: 0x000585F0 File Offset: 0x000579F0
		internal static Exception TransactionRequired()
		{
			return ADP.InvalidOperation(Res.GetString("ADP_TransactionRequired_Execute"));
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0005860C File Offset: 0x00057A0C
		internal static Exception TypeNotSupported(OCI.DATATYPE ociType)
		{
			return ADP.NotSupported(Res.GetString("ADP_TypeNotSupported", new object[] { ociType.ToString() }));
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00058640 File Offset: 0x00057A40
		internal static Exception UnknownDataTypeCode(Type dataType, TypeCode tc)
		{
			return ADP.Simple(Res.GetString("ADP_UnknownDataTypeCode", new object[]
			{
				dataType.ToString(),
				tc.ToString()
			}));
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0005867C File Offset: 0x00057A7C
		internal static Exception UnsupportedIsolationLevel()
		{
			return ADP.Argument(Res.GetString("ADP_UnsupportedIsolationLevel"));
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00058698 File Offset: 0x00057A98
		internal static Exception WriteByteForBinaryLobsOnly()
		{
			return ADP.NotSupported(Res.GetString("ADP_WriteByteForBinaryLobsOnly"));
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000586B4 File Offset: 0x00057AB4
		internal static Exception WrongType(Type got, Type expected)
		{
			return ADP.Argument(Res.GetString("ADP_WrongType", new object[]
			{
				got.ToString(),
				expected.ToString()
			}));
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000586EC File Offset: 0x00057AEC
		public static void CheckArgumentNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw ADP.ArgumentNull(parameterName);
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00058704 File Offset: 0x00057B04
		internal static bool IsCatchableExceptionType(Exception e)
		{
			Type type = e.GetType();
			return type != ADP.StackOverflowType && type != ADP.OutOfMemoryType && type != ADP.ThreadAbortType && type != ADP.NullReferenceType && type != ADP.AccessViolationType && !ADP.SecurityType.IsAssignableFrom(type);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00058750 File Offset: 0x00057B50
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

		// Token: 0x06000177 RID: 375 RVA: 0x0005878C File Offset: 0x00057B8C
		internal static IntPtr IntPtrOffset(IntPtr pbase, int offset)
		{
			if (4 == ADP.PtrSize)
			{
				return (IntPtr)(pbase.ToInt32() + offset);
			}
			return (IntPtr)(pbase.ToInt64() + (long)offset);
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000587C0 File Offset: 0x00057BC0
		internal static bool IsDirection(IDataParameter value, ParameterDirection condition)
		{
			return condition == (condition & value.Direction);
		}

		// Token: 0x06000179 RID: 377 RVA: 0x000587D8 File Offset: 0x00057BD8
		internal static bool IsDirection(ParameterDirection value, ParameterDirection condition)
		{
			return condition == (condition & value);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x000587EC File Offset: 0x00057BEC
		internal static bool IsEmpty(string str)
		{
			return str == null || 0 == str.Length;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00058808 File Offset: 0x00057C08
		internal static bool IsNull(object value)
		{
			if (value == null || DBNull.Value == value)
			{
				return true;
			}
			INullable nullable = value as INullable;
			return nullable != null && nullable.IsNull;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00058834 File Offset: 0x00057C34
		internal static Transaction GetCurrentTransaction()
		{
			return Transaction.Current;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00058848 File Offset: 0x00057C48
		internal static IDtcTransaction GetOletxTransaction(Transaction transaction)
		{
			IDtcTransaction dtcTransaction = null;
			if (null != transaction)
			{
				dtcTransaction = TransactionInterop.GetDtcTransaction(transaction);
			}
			return dtcTransaction;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00058868 File Offset: 0x00057C68
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.PathDiscovery)]
		internal static string GetFullPath(string filename)
		{
			return Path.GetFullPath(filename);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0005887C File Offset: 0x00057C7C
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

		// Token: 0x06000180 RID: 384 RVA: 0x000588C4 File Offset: 0x00057CC4
		internal static Stream GetXmlStreamFromValues(string[] values, string errorString)
		{
			if (values.Length != 1)
			{
				throw ADP.ConfigWrongNumberOfValues(errorString);
			}
			return ADP.GetXmlStream(values[0], errorString);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000588E8 File Offset: 0x00057CE8
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

		// Token: 0x04000154 RID: 340
		internal const string Parameter = "Parameter";

		// Token: 0x04000155 RID: 341
		internal const string ParameterName = "ParameterName";

		// Token: 0x04000156 RID: 342
		internal const string ConnectionString = "ConnectionString";

		// Token: 0x04000157 RID: 343
		internal const CompareOptions compareOptions = CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth;

		// Token: 0x04000158 RID: 344
		internal static readonly bool IsWindowsNT = PlatformID.Win32NT == Environment.OSVersion.Platform;

		// Token: 0x04000159 RID: 345
		internal static readonly bool IsPlatformNT5 = ADP.IsWindowsNT && Environment.OSVersion.Version.Major >= 5;

		// Token: 0x0400015A RID: 346
		private static readonly Type StackOverflowType = typeof(StackOverflowException);

		// Token: 0x0400015B RID: 347
		private static readonly Type OutOfMemoryType = typeof(OutOfMemoryException);

		// Token: 0x0400015C RID: 348
		private static readonly Type ThreadAbortType = typeof(ThreadAbortException);

		// Token: 0x0400015D RID: 349
		private static readonly Type NullReferenceType = typeof(NullReferenceException);

		// Token: 0x0400015E RID: 350
		private static readonly Type AccessViolationType = typeof(AccessViolationException);

		// Token: 0x0400015F RID: 351
		private static readonly Type SecurityType = typeof(SecurityException);

		// Token: 0x04000160 RID: 352
		internal static readonly Type ArgumentNullExceptionType = typeof(ArgumentNullException);

		// Token: 0x04000161 RID: 353
		internal static readonly Type FormatExceptionType = typeof(FormatException);

		// Token: 0x04000162 RID: 354
		internal static readonly Type OverflowExceptionType = typeof(OverflowException);

		// Token: 0x04000163 RID: 355
		internal static readonly string NullString = Res.GetString("SqlMisc_NullString");

		// Token: 0x04000164 RID: 356
		internal static readonly int CharSize = 2;

		// Token: 0x04000165 RID: 357
		internal static readonly byte[] EmptyByteArray = new byte[0];

		// Token: 0x04000166 RID: 358
		internal static readonly int PtrSize = IntPtr.Size;

		// Token: 0x04000167 RID: 359
		internal static readonly string StrEmpty = "";

		// Token: 0x04000168 RID: 360
		internal static readonly HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

		// Token: 0x02000019 RID: 25
		internal enum ConnectionError
		{
			// Token: 0x0400016A RID: 362
			BeginGetConnectionReturnsNull,
			// Token: 0x0400016B RID: 363
			GetConnectionReturnsNull,
			// Token: 0x0400016C RID: 364
			ConnectionOptionsMissing,
			// Token: 0x0400016D RID: 365
			CouldNotSwitchToClosedPreviouslyOpenedState
		}

		// Token: 0x0200001A RID: 26
		internal enum InternalErrorCode
		{
			// Token: 0x0400016F RID: 367
			UnpooledObjectHasOwner,
			// Token: 0x04000170 RID: 368
			UnpooledObjectHasWrongOwner,
			// Token: 0x04000171 RID: 369
			PushingObjectSecondTime,
			// Token: 0x04000172 RID: 370
			PooledObjectHasOwner,
			// Token: 0x04000173 RID: 371
			PooledObjectInPoolMoreThanOnce,
			// Token: 0x04000174 RID: 372
			CreateObjectReturnedNull,
			// Token: 0x04000175 RID: 373
			NewObjectCannotBePooled,
			// Token: 0x04000176 RID: 374
			NonPooledObjectUsedMoreThanOnce,
			// Token: 0x04000177 RID: 375
			AttemptingToPoolOnRestrictedToken,
			// Token: 0x04000178 RID: 376
			ConvertSidToStringSidWReturnedNull = 10,
			// Token: 0x04000179 RID: 377
			AttemptingToConstructReferenceCollectionOnStaticObject = 12,
			// Token: 0x0400017A RID: 378
			AttemptingToEnlistTwice,
			// Token: 0x0400017B RID: 379
			CreateReferenceCollectionReturnedNull,
			// Token: 0x0400017C RID: 380
			PooledObjectWithoutPool,
			// Token: 0x0400017D RID: 381
			UnexpectedWaitAnyResult,
			// Token: 0x0400017E RID: 382
			NameValuePairNext = 20,
			// Token: 0x0400017F RID: 383
			InvalidParserState1,
			// Token: 0x04000180 RID: 384
			InvalidParserState2,
			// Token: 0x04000181 RID: 385
			InvalidBuffer = 30,
			// Token: 0x04000182 RID: 386
			InvalidLongBuffer,
			// Token: 0x04000183 RID: 387
			InvalidNumberOfRows
		}
	}
}
