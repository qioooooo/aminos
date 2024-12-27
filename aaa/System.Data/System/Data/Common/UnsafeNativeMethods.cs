using System;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Transactions;

namespace System.Data.Common
{
	// Token: 0x0200016B RID: 363
	[SuppressUnmanagedCodeSecurity]
	internal static class UnsafeNativeMethods
	{
		// Token: 0x06001677 RID: 5751
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLAllocHandle(ODBC32.SQL_HANDLE HandleType, IntPtr InputHandle, out IntPtr OutputHandle);

		// Token: 0x06001678 RID: 5752
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLAllocHandle(ODBC32.SQL_HANDLE HandleType, OdbcHandle InputHandle, out IntPtr OutputHandle);

		// Token: 0x06001679 RID: 5753
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLBindCol(OdbcStatementHandle StatementHandle, ushort ColumnNumber, ODBC32.SQL_C TargetType, HandleRef TargetValue, IntPtr BufferLength, IntPtr StrLen_or_Ind);

		// Token: 0x0600167A RID: 5754
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLBindCol(OdbcStatementHandle StatementHandle, ushort ColumnNumber, ODBC32.SQL_C TargetType, IntPtr TargetValue, IntPtr BufferLength, IntPtr StrLen_or_Ind);

		// Token: 0x0600167B RID: 5755
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLBindParameter(OdbcStatementHandle StatementHandle, ushort ParameterNumber, short ParamDirection, ODBC32.SQL_C SQLCType, short SQLType, IntPtr cbColDef, IntPtr ibScale, HandleRef rgbValue, IntPtr BufferLength, HandleRef StrLen_or_Ind);

		// Token: 0x0600167C RID: 5756
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLCancel(OdbcStatementHandle StatementHandle);

		// Token: 0x0600167D RID: 5757
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLCloseCursor(OdbcStatementHandle StatementHandle);

		// Token: 0x0600167E RID: 5758
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLColAttributeW(OdbcStatementHandle StatementHandle, short ColumnNumber, short FieldIdentifier, CNativeBuffer CharacterAttribute, short BufferLength, out short StringLength, out IntPtr NumericAttribute);

		// Token: 0x0600167F RID: 5759
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLColumnsW(OdbcStatementHandle StatementHandle, [MarshalAs(UnmanagedType.LPWStr)] [In] string CatalogName, short NameLen1, [MarshalAs(UnmanagedType.LPWStr)] [In] string SchemaName, short NameLen2, [MarshalAs(UnmanagedType.LPWStr)] [In] string TableName, short NameLen3, [MarshalAs(UnmanagedType.LPWStr)] [In] string ColumnName, short NameLen4);

		// Token: 0x06001680 RID: 5760
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLDisconnect(IntPtr ConnectionHandle);

		// Token: 0x06001681 RID: 5761
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLDriverConnectW(OdbcConnectionHandle hdbc, IntPtr hwnd, [MarshalAs(UnmanagedType.LPWStr)] [In] string connectionstring, short cbConnectionstring, IntPtr connectionstringout, short cbConnectionstringoutMax, out short cbConnectionstringout, short fDriverCompletion);

		// Token: 0x06001682 RID: 5762
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLEndTran(ODBC32.SQL_HANDLE HandleType, IntPtr Handle, short CompletionType);

		// Token: 0x06001683 RID: 5763
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLExecDirectW(OdbcStatementHandle StatementHandle, [MarshalAs(UnmanagedType.LPWStr)] [In] string StatementText, int TextLength);

		// Token: 0x06001684 RID: 5764
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLExecute(OdbcStatementHandle StatementHandle);

		// Token: 0x06001685 RID: 5765
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLFetch(OdbcStatementHandle StatementHandle);

		// Token: 0x06001686 RID: 5766
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLFreeHandle(ODBC32.SQL_HANDLE HandleType, IntPtr StatementHandle);

		// Token: 0x06001687 RID: 5767
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLFreeStmt(OdbcStatementHandle StatementHandle, ODBC32.STMT Option);

		// Token: 0x06001688 RID: 5768
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLGetConnectAttrW(OdbcConnectionHandle ConnectionHandle, ODBC32.SQL_ATTR Attribute, byte[] Value, int BufferLength, out int StringLength);

		// Token: 0x06001689 RID: 5769
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLGetData(OdbcStatementHandle StatementHandle, ushort ColumnNumber, ODBC32.SQL_C TargetType, CNativeBuffer TargetValue, IntPtr BufferLength, out IntPtr StrLen_or_Ind);

		// Token: 0x0600168A RID: 5770
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLGetDescFieldW(OdbcDescriptorHandle StatementHandle, short RecNumber, ODBC32.SQL_DESC FieldIdentifier, CNativeBuffer ValuePointer, int BufferLength, out int StringLength);

		// Token: 0x0600168B RID: 5771
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLGetDiagRecW(ODBC32.SQL_HANDLE HandleType, OdbcHandle Handle, short RecNumber, StringBuilder rchState, out int NativeError, StringBuilder MessageText, short BufferLength, out short TextLength);

		// Token: 0x0600168C RID: 5772
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLGetDiagFieldW(ODBC32.SQL_HANDLE HandleType, OdbcHandle Handle, short RecNumber, short DiagIdentifier, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder rchState, short BufferLength, out short StringLength);

		// Token: 0x0600168D RID: 5773
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLGetFunctions(OdbcConnectionHandle hdbc, ODBC32.SQL_API fFunction, out short pfExists);

		// Token: 0x0600168E RID: 5774
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLGetInfoW(OdbcConnectionHandle hdbc, ODBC32.SQL_INFO fInfoType, byte[] rgbInfoValue, short cbInfoValueMax, out short pcbInfoValue);

		// Token: 0x0600168F RID: 5775
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLGetInfoW(OdbcConnectionHandle hdbc, ODBC32.SQL_INFO fInfoType, byte[] rgbInfoValue, short cbInfoValueMax, IntPtr pcbInfoValue);

		// Token: 0x06001690 RID: 5776
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLGetStmtAttrW(OdbcStatementHandle StatementHandle, ODBC32.SQL_ATTR Attribute, out IntPtr Value, int BufferLength, out int StringLength);

		// Token: 0x06001691 RID: 5777
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLGetTypeInfo(OdbcStatementHandle StatementHandle, short fSqlType);

		// Token: 0x06001692 RID: 5778
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLMoreResults(OdbcStatementHandle StatementHandle);

		// Token: 0x06001693 RID: 5779
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLNumResultCols(OdbcStatementHandle StatementHandle, out short ColumnCount);

		// Token: 0x06001694 RID: 5780
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLPrepareW(OdbcStatementHandle StatementHandle, [MarshalAs(UnmanagedType.LPWStr)] [In] string StatementText, int TextLength);

		// Token: 0x06001695 RID: 5781
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLPrimaryKeysW(OdbcStatementHandle StatementHandle, [MarshalAs(UnmanagedType.LPWStr)] [In] string CatalogName, short NameLen1, [MarshalAs(UnmanagedType.LPWStr)] [In] string SchemaName, short NameLen2, [MarshalAs(UnmanagedType.LPWStr)] [In] string TableName, short NameLen3);

		// Token: 0x06001696 RID: 5782
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLProcedureColumnsW(OdbcStatementHandle StatementHandle, [MarshalAs(UnmanagedType.LPWStr)] [In] string CatalogName, short NameLen1, [MarshalAs(UnmanagedType.LPWStr)] [In] string SchemaName, short NameLen2, [MarshalAs(UnmanagedType.LPWStr)] [In] string ProcName, short NameLen3, [MarshalAs(UnmanagedType.LPWStr)] [In] string ColumnName, short NameLen4);

		// Token: 0x06001697 RID: 5783
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLProceduresW(OdbcStatementHandle StatementHandle, [MarshalAs(UnmanagedType.LPWStr)] [In] string CatalogName, short NameLen1, [MarshalAs(UnmanagedType.LPWStr)] [In] string SchemaName, short NameLen2, [MarshalAs(UnmanagedType.LPWStr)] [In] string ProcName, short NameLen3);

		// Token: 0x06001698 RID: 5784
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLRowCount(OdbcStatementHandle StatementHandle, out IntPtr RowCount);

		// Token: 0x06001699 RID: 5785
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLSetConnectAttrW(OdbcConnectionHandle ConnectionHandle, ODBC32.SQL_ATTR Attribute, IDtcTransaction Value, int StringLength);

		// Token: 0x0600169A RID: 5786
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLSetConnectAttrW(OdbcConnectionHandle ConnectionHandle, ODBC32.SQL_ATTR Attribute, string Value, int StringLength);

		// Token: 0x0600169B RID: 5787
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLSetConnectAttrW(OdbcConnectionHandle ConnectionHandle, ODBC32.SQL_ATTR Attribute, IntPtr Value, int StringLength);

		// Token: 0x0600169C RID: 5788
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLSetConnectAttrW(IntPtr ConnectionHandle, ODBC32.SQL_ATTR Attribute, IntPtr Value, int StringLength);

		// Token: 0x0600169D RID: 5789
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLSetDescFieldW(OdbcDescriptorHandle StatementHandle, short ColumnNumber, ODBC32.SQL_DESC FieldIdentifier, HandleRef CharacterAttribute, int BufferLength);

		// Token: 0x0600169E RID: 5790
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLSetDescFieldW(OdbcDescriptorHandle StatementHandle, short ColumnNumber, ODBC32.SQL_DESC FieldIdentifier, IntPtr CharacterAttribute, int BufferLength);

		// Token: 0x0600169F RID: 5791
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLSetEnvAttr(OdbcEnvironmentHandle EnvironmentHandle, ODBC32.SQL_ATTR Attribute, IntPtr Value, ODBC32.SQL_IS StringLength);

		// Token: 0x060016A0 RID: 5792
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLSetStmtAttrW(OdbcStatementHandle StatementHandle, int Attribute, IntPtr Value, int StringLength);

		// Token: 0x060016A1 RID: 5793
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLSpecialColumnsW(OdbcStatementHandle StatementHandle, ODBC32.SQL_SPECIALCOLS IdentifierType, [MarshalAs(UnmanagedType.LPWStr)] [In] string CatalogName, short NameLen1, [MarshalAs(UnmanagedType.LPWStr)] [In] string SchemaName, short NameLen2, [MarshalAs(UnmanagedType.LPWStr)] [In] string TableName, short NameLen3, ODBC32.SQL_SCOPE Scope, ODBC32.SQL_NULLABILITY Nullable);

		// Token: 0x060016A2 RID: 5794
		[DllImport("odbc32.dll", CharSet = CharSet.Unicode)]
		internal static extern ODBC32.RetCode SQLStatisticsW(OdbcStatementHandle StatementHandle, [MarshalAs(UnmanagedType.LPWStr)] [In] string CatalogName, short NameLen1, [MarshalAs(UnmanagedType.LPWStr)] [In] string SchemaName, short NameLen2, [MarshalAs(UnmanagedType.LPWStr)] [In] string TableName, short NameLen3, short Unique, short Reserved);

		// Token: 0x060016A3 RID: 5795
		[DllImport("odbc32.dll")]
		internal static extern ODBC32.RetCode SQLTablesW(OdbcStatementHandle StatementHandle, [MarshalAs(UnmanagedType.LPWStr)] [In] string CatalogName, short NameLen1, [MarshalAs(UnmanagedType.LPWStr)] [In] string SchemaName, short NameLen2, [MarshalAs(UnmanagedType.LPWStr)] [In] string TableName, short NameLen3, [MarshalAs(UnmanagedType.LPWStr)] [In] string TableType, short NameLen4);

		// Token: 0x060016A4 RID: 5796
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
		internal static extern OleDbHResult GetErrorInfo([In] int dwReserved, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IErrorInfo ppIErrorInfo);

		// Token: 0x060016A5 RID: 5797
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
		internal static extern uint GetEffectiveRightsFromAclW(byte[] pAcl, ref UnsafeNativeMethods.Trustee pTrustee, out uint pAccessMask);

		// Token: 0x060016A6 RID: 5798
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CheckTokenMembership(IntPtr tokenHandle, byte[] sidToCheck, out bool isMember);

		// Token: 0x060016A7 RID: 5799
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ConvertSidToStringSidW(IntPtr sid, out IntPtr stringSid);

		// Token: 0x060016A8 RID: 5800
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int CreateWellKnownSid(int sidType, byte[] domainSid, [Out] byte[] resultSid, ref uint resultSidLength);

		// Token: 0x060016A9 RID: 5801
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetTokenInformation(IntPtr tokenHandle, uint token_class, IntPtr tokenStruct, uint tokenInformationLength, ref uint tokenString);

		// Token: 0x060016AA RID: 5802
		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool IsTokenRestricted(IntPtr tokenHandle);

		// Token: 0x060016AB RID: 5803
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern int lstrlenW(IntPtr ptr);

		// Token: 0x060016AC RID: 5804
		[DllImport("kernel32.dll")]
		internal static extern void SetLastError(int dwErrCode);

		// Token: 0x0200016C RID: 364
		[SuppressUnmanagedCodeSecurity]
		[Guid("00000567-0000-0010-8000-00AA006D2EA4")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		internal interface ADORecordConstruction
		{
			// Token: 0x060016AD RID: 5805
			[return: MarshalAs(UnmanagedType.Interface)]
			object get_Row();
		}

		// Token: 0x0200016D RID: 365
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("00000283-0000-0010-8000-00AA006D2EA4")]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface ADORecordsetConstruction
		{
			// Token: 0x060016AE RID: 5806
			[return: MarshalAs(UnmanagedType.Interface)]
			object get_Rowset();

			// Token: 0x060016AF RID: 5807
			[Obsolete("not used", true)]
			void put_Rowset();

			// Token: 0x060016B0 RID: 5808
			IntPtr get_Chapter();
		}

		// Token: 0x0200016E RID: 366
		[Guid("0000050E-0000-0010-8000-00AA006D2EA4")]
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		internal interface Recordset15
		{
			// Token: 0x060016B1 RID: 5809
			[Obsolete("not used", true)]
			void get_Properties();

			// Token: 0x060016B2 RID: 5810
			[Obsolete("not used", true)]
			void get_AbsolutePosition();

			// Token: 0x060016B3 RID: 5811
			[Obsolete("not used", true)]
			void put_AbsolutePosition();

			// Token: 0x060016B4 RID: 5812
			[Obsolete("not used", true)]
			void putref_ActiveConnection();

			// Token: 0x060016B5 RID: 5813
			[Obsolete("not used", true)]
			void put_ActiveConnection();

			// Token: 0x060016B6 RID: 5814
			object get_ActiveConnection();

			// Token: 0x060016B7 RID: 5815
			[Obsolete("not used", true)]
			void get_BOF();

			// Token: 0x060016B8 RID: 5816
			[Obsolete("not used", true)]
			void get_Bookmark();

			// Token: 0x060016B9 RID: 5817
			[Obsolete("not used", true)]
			void put_Bookmark();

			// Token: 0x060016BA RID: 5818
			[Obsolete("not used", true)]
			void get_CacheSize();

			// Token: 0x060016BB RID: 5819
			[Obsolete("not used", true)]
			void put_CacheSize();

			// Token: 0x060016BC RID: 5820
			[Obsolete("not used", true)]
			void get_CursorType();

			// Token: 0x060016BD RID: 5821
			[Obsolete("not used", true)]
			void put_CursorType();

			// Token: 0x060016BE RID: 5822
			[Obsolete("not used", true)]
			void get_EOF();

			// Token: 0x060016BF RID: 5823
			[Obsolete("not used", true)]
			void get_Fields();

			// Token: 0x060016C0 RID: 5824
			[Obsolete("not used", true)]
			void get_LockType();

			// Token: 0x060016C1 RID: 5825
			[Obsolete("not used", true)]
			void put_LockType();

			// Token: 0x060016C2 RID: 5826
			[Obsolete("not used", true)]
			void get_MaxRecords();

			// Token: 0x060016C3 RID: 5827
			[Obsolete("not used", true)]
			void put_MaxRecords();

			// Token: 0x060016C4 RID: 5828
			[Obsolete("not used", true)]
			void get_RecordCount();

			// Token: 0x060016C5 RID: 5829
			[Obsolete("not used", true)]
			void putref_Source();

			// Token: 0x060016C6 RID: 5830
			[Obsolete("not used", true)]
			void put_Source();

			// Token: 0x060016C7 RID: 5831
			[Obsolete("not used", true)]
			void get_Source();

			// Token: 0x060016C8 RID: 5832
			[Obsolete("not used", true)]
			void AddNew();

			// Token: 0x060016C9 RID: 5833
			[Obsolete("not used", true)]
			void CancelUpdate();

			// Token: 0x060016CA RID: 5834
			[PreserveSig]
			OleDbHResult Close();

			// Token: 0x060016CB RID: 5835
			[Obsolete("not used", true)]
			void Delete();

			// Token: 0x060016CC RID: 5836
			[Obsolete("not used", true)]
			void GetRows();

			// Token: 0x060016CD RID: 5837
			[Obsolete("not used", true)]
			void Move();

			// Token: 0x060016CE RID: 5838
			[Obsolete("not used", true)]
			void MoveNext();

			// Token: 0x060016CF RID: 5839
			[Obsolete("not used", true)]
			void MovePrevious();

			// Token: 0x060016D0 RID: 5840
			[Obsolete("not used", true)]
			void MoveFirst();

			// Token: 0x060016D1 RID: 5841
			[Obsolete("not used", true)]
			void MoveLast();

			// Token: 0x060016D2 RID: 5842
			[Obsolete("not used", true)]
			void Open();

			// Token: 0x060016D3 RID: 5843
			[Obsolete("not used", true)]
			void Requery();

			// Token: 0x060016D4 RID: 5844
			[Obsolete("not used", true)]
			void _xResync();

			// Token: 0x060016D5 RID: 5845
			[Obsolete("not used", true)]
			void Update();

			// Token: 0x060016D6 RID: 5846
			[Obsolete("not used", true)]
			void get_AbsolutePage();

			// Token: 0x060016D7 RID: 5847
			[Obsolete("not used", true)]
			void put_AbsolutePage();

			// Token: 0x060016D8 RID: 5848
			[Obsolete("not used", true)]
			void get_EditMode();

			// Token: 0x060016D9 RID: 5849
			[Obsolete("not used", true)]
			void get_Filter();

			// Token: 0x060016DA RID: 5850
			[Obsolete("not used", true)]
			void put_Filter();

			// Token: 0x060016DB RID: 5851
			[Obsolete("not used", true)]
			void get_PageCount();

			// Token: 0x060016DC RID: 5852
			[Obsolete("not used", true)]
			void get_PageSize();

			// Token: 0x060016DD RID: 5853
			[Obsolete("not used", true)]
			void put_PageSize();

			// Token: 0x060016DE RID: 5854
			[Obsolete("not used", true)]
			void get_Sort();

			// Token: 0x060016DF RID: 5855
			[Obsolete("not used", true)]
			void put_Sort();

			// Token: 0x060016E0 RID: 5856
			[Obsolete("not used", true)]
			void get_Status();

			// Token: 0x060016E1 RID: 5857
			[Obsolete("not used", true)]
			void get_State();

			// Token: 0x060016E2 RID: 5858
			[Obsolete("not used", true)]
			void _xClone();

			// Token: 0x060016E3 RID: 5859
			[Obsolete("not used", true)]
			void UpdateBatch();

			// Token: 0x060016E4 RID: 5860
			[Obsolete("not used", true)]
			void CancelBatch();

			// Token: 0x060016E5 RID: 5861
			[Obsolete("not used", true)]
			void get_CursorLocation();

			// Token: 0x060016E6 RID: 5862
			[Obsolete("not used", true)]
			void put_CursorLocation();

			// Token: 0x060016E7 RID: 5863
			[PreserveSig]
			OleDbHResult NextRecordset(out object RecordsAffected, [MarshalAs(UnmanagedType.Interface)] out object ppiRs);
		}

		// Token: 0x0200016F RID: 367
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("00000562-0000-0010-8000-00AA006D2EA4")]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface _ADORecord
		{
			// Token: 0x060016E8 RID: 5864
			[Obsolete("not used", true)]
			void get_Properties();

			// Token: 0x060016E9 RID: 5865
			object get_ActiveConnection();

			// Token: 0x060016EA RID: 5866
			[Obsolete("not used", true)]
			void put_ActiveConnection();

			// Token: 0x060016EB RID: 5867
			[Obsolete("not used", true)]
			void putref_ActiveConnection();

			// Token: 0x060016EC RID: 5868
			[Obsolete("not used", true)]
			void get_State();

			// Token: 0x060016ED RID: 5869
			[Obsolete("not used", true)]
			void get_Source();

			// Token: 0x060016EE RID: 5870
			[Obsolete("not used", true)]
			void put_Source();

			// Token: 0x060016EF RID: 5871
			[Obsolete("not used", true)]
			void putref_Source();

			// Token: 0x060016F0 RID: 5872
			[Obsolete("not used", true)]
			void get_Mode();

			// Token: 0x060016F1 RID: 5873
			[Obsolete("not used", true)]
			void put_Mode();

			// Token: 0x060016F2 RID: 5874
			[Obsolete("not used", true)]
			void get_ParentURL();

			// Token: 0x060016F3 RID: 5875
			[Obsolete("not used", true)]
			void MoveRecord();

			// Token: 0x060016F4 RID: 5876
			[Obsolete("not used", true)]
			void CopyRecord();

			// Token: 0x060016F5 RID: 5877
			[Obsolete("not used", true)]
			void DeleteRecord();

			// Token: 0x060016F6 RID: 5878
			[Obsolete("not used", true)]
			void Open();

			// Token: 0x060016F7 RID: 5879
			[PreserveSig]
			OleDbHResult Close();
		}

		// Token: 0x02000170 RID: 368
		[Guid("0C733A8C-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface IAccessor
		{
			// Token: 0x060016F8 RID: 5880
			[Obsolete("not used", true)]
			void AddRefAccessor();

			// Token: 0x060016F9 RID: 5881
			[PreserveSig]
			OleDbHResult CreateAccessor([In] int dwAccessorFlags, [In] IntPtr cBindings, [In] SafeHandle rgBindings, [In] IntPtr cbRowSize, out IntPtr phAccessor, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.I4)] [In] [Out] int[] rgStatus);

			// Token: 0x060016FA RID: 5882
			[Obsolete("not used", true)]
			void GetBindings();

			// Token: 0x060016FB RID: 5883
			[PreserveSig]
			OleDbHResult ReleaseAccessor([In] IntPtr hAccessor, out int pcRefCount);
		}

		// Token: 0x02000171 RID: 369
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0C733A93-2A1C-11CE-ADE5-00AA0044773D")]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface IChapteredRowset
		{
			// Token: 0x060016FC RID: 5884
			[Obsolete("not used", true)]
			void AddRefChapter();

			// Token: 0x060016FD RID: 5885
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[PreserveSig]
			OleDbHResult ReleaseChapter([In] IntPtr hChapter, out int pcRefCount);
		}

		// Token: 0x02000172 RID: 370
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[Guid("0C733A11-2A1C-11CE-ADE5-00AA0044773D")]
		[ComImport]
		internal interface IColumnsInfo
		{
			// Token: 0x060016FE RID: 5886
			[PreserveSig]
			OleDbHResult GetColumnInfo(out IntPtr pcColumns, out IntPtr prgInfo, out IntPtr ppStringsBuffer);
		}

		// Token: 0x02000173 RID: 371
		[Guid("0C733A10-2A1C-11CE-ADE5-00AA0044773D")]
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IColumnsRowset
		{
			// Token: 0x060016FF RID: 5887
			[PreserveSig]
			OleDbHResult GetAvailableColumns(out IntPtr pcOptColumns, out IntPtr prgOptColumns);

			// Token: 0x06001700 RID: 5888
			[PreserveSig]
			OleDbHResult GetColumnsRowset([In] IntPtr pUnkOuter, [In] IntPtr cOptColumns, [In] SafeHandle rgOptColumns, [In] ref Guid riid, [In] int cPropertySets, [In] IntPtr rgPropertySets, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IRowset ppColRowset);
		}

		// Token: 0x02000174 RID: 372
		[Guid("0C733A26-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface ICommandPrepare
		{
			// Token: 0x06001701 RID: 5889
			[PreserveSig]
			OleDbHResult Prepare([In] int cExpectedRuns);
		}

		// Token: 0x02000175 RID: 373
		[Guid("0C733A79-2A1C-11CE-ADE5-00AA0044773D")]
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ICommandProperties
		{
			// Token: 0x06001702 RID: 5890
			[PreserveSig]
			OleDbHResult GetProperties([In] int cPropertyIDSets, [In] SafeHandle rgPropertyIDSets, out int pcPropertySets, out IntPtr prgPropertySets);

			// Token: 0x06001703 RID: 5891
			[PreserveSig]
			OleDbHResult SetProperties([In] int cPropertySets, [In] SafeHandle rgPropertySets);
		}

		// Token: 0x02000176 RID: 374
		[SuppressUnmanagedCodeSecurity]
		[Guid("0C733A27-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ICommandText
		{
			// Token: 0x06001704 RID: 5892
			[PreserveSig]
			OleDbHResult Cancel();

			// Token: 0x06001705 RID: 5893
			[PreserveSig]
			OleDbHResult Execute([In] IntPtr pUnkOuter, [In] ref Guid riid, [In] tagDBPARAMS pDBParams, out IntPtr pcRowsAffected, [MarshalAs(UnmanagedType.Interface)] out object ppRowset);

			// Token: 0x06001706 RID: 5894
			[Obsolete("not used", true)]
			void GetDBSession();

			// Token: 0x06001707 RID: 5895
			[Obsolete("not used", true)]
			void GetCommandText();

			// Token: 0x06001708 RID: 5896
			[PreserveSig]
			OleDbHResult SetCommandText([In] ref Guid rguidDialect, [MarshalAs(UnmanagedType.LPWStr)] [In] string pwszCommand);
		}

		// Token: 0x02000177 RID: 375
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0C733A64-2A1C-11CE-ADE5-00AA0044773D")]
		[ComImport]
		internal interface ICommandWithParameters
		{
			// Token: 0x06001709 RID: 5897
			[Obsolete("not used", true)]
			void GetParameterInfo();

			// Token: 0x0600170A RID: 5898
			[Obsolete("not used", true)]
			void MapParameterNames();

			// Token: 0x0600170B RID: 5899
			[PreserveSig]
			OleDbHResult SetParameterInfo([In] IntPtr cParams, [MarshalAs(UnmanagedType.LPArray)] [In] IntPtr[] rgParamOrdinals, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct)] [In] tagDBPARAMBINDINFO[] rgParamBindInfo);
		}

		// Token: 0x02000178 RID: 376
		[SuppressUnmanagedCodeSecurity]
		[Guid("2206CCB1-19C1-11D1-89E0-00C04FD7A829")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IDataInitialize
		{
		}

		// Token: 0x02000179 RID: 377
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[Guid("0C733A89-2A1C-11CE-ADE5-00AA0044773D")]
		[ComImport]
		internal interface IDBInfo
		{
			// Token: 0x0600170C RID: 5900
			[PreserveSig]
			OleDbHResult GetKeywords([MarshalAs(UnmanagedType.LPWStr)] out string ppwszKeywords);

			// Token: 0x0600170D RID: 5901
			[PreserveSig]
			OleDbHResult GetLiteralInfo([In] int cLiterals, [MarshalAs(UnmanagedType.LPArray)] [In] int[] rgLiterals, out int pcLiteralInfo, out IntPtr prgLiteralInfo, out IntPtr ppCharBuffer);
		}

		// Token: 0x0200017A RID: 378
		[Guid("0C733A8A-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface IDBProperties
		{
			// Token: 0x0600170E RID: 5902
			[PreserveSig]
			OleDbHResult GetProperties([In] int cPropertyIDSets, [In] SafeHandle rgPropertyIDSets, out int pcPropertySets, out IntPtr prgPropertySets);

			// Token: 0x0600170F RID: 5903
			[PreserveSig]
			OleDbHResult GetPropertyInfo([In] int cPropertyIDSets, [In] SafeHandle rgPropertyIDSets, out int pcPropertySets, out IntPtr prgPropertyInfoSets, out IntPtr ppDescBuffer);

			// Token: 0x06001710 RID: 5904
			[PreserveSig]
			OleDbHResult SetProperties([In] int cPropertySets, [In] SafeHandle rgPropertySets);
		}

		// Token: 0x0200017B RID: 379
		[Guid("0C733A7B-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface IDBSchemaRowset
		{
			// Token: 0x06001711 RID: 5905
			[PreserveSig]
			OleDbHResult GetRowset([In] IntPtr pUnkOuter, [In] ref Guid rguidSchema, [In] int cRestrictions, [MarshalAs(UnmanagedType.LPArray)] [In] object[] rgRestrictions, [In] ref Guid riid, [In] int cPropertySets, [In] IntPtr rgPropertySets, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IRowset ppRowset);

			// Token: 0x06001712 RID: 5906
			[PreserveSig]
			OleDbHResult GetSchemas(out int pcSchemas, out IntPtr rguidSchema, out IntPtr prgRestrictionSupport);
		}

		// Token: 0x0200017C RID: 380
		[Guid("1CF2B120-547D-101B-8E65-08002B2BD119")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface IErrorInfo
		{
			// Token: 0x06001713 RID: 5907
			[Obsolete("not used", true)]
			void GetGUID();

			// Token: 0x06001714 RID: 5908
			[PreserveSig]
			OleDbHResult GetSource([MarshalAs(UnmanagedType.BStr)] out string pBstrSource);

			// Token: 0x06001715 RID: 5909
			[PreserveSig]
			OleDbHResult GetDescription([MarshalAs(UnmanagedType.BStr)] out string pBstrDescription);
		}

		// Token: 0x0200017D RID: 381
		[SuppressUnmanagedCodeSecurity]
		[Guid("0C733A67-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IErrorRecords
		{
			// Token: 0x06001716 RID: 5910
			[Obsolete("not used", true)]
			void AddErrorRecord();

			// Token: 0x06001717 RID: 5911
			[Obsolete("not used", true)]
			void GetBasicErrorInfo();

			// Token: 0x06001718 RID: 5912
			[PreserveSig]
			OleDbHResult GetCustomErrorObject([In] int ulRecordNum, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.ISQLErrorInfo ppObject);

			// Token: 0x06001719 RID: 5913
			[return: MarshalAs(UnmanagedType.Interface)]
			UnsafeNativeMethods.IErrorInfo GetErrorInfo([In] int ulRecordNum, [In] int lcid);

			// Token: 0x0600171A RID: 5914
			[Obsolete("not used", true)]
			void GetErrorParameters();

			// Token: 0x0600171B RID: 5915
			int GetRecordCount();
		}

		// Token: 0x0200017E RID: 382
		[Guid("0C733A90-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface IMultipleResults
		{
			// Token: 0x0600171C RID: 5916
			[PreserveSig]
			OleDbHResult GetResult([In] IntPtr pUnkOuter, [In] IntPtr lResultFlag, [In] ref Guid riid, out IntPtr pcRowsAffected, [MarshalAs(UnmanagedType.Interface)] out object ppRowset);
		}

		// Token: 0x0200017F RID: 383
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[Guid("0C733A69-2A1C-11CE-ADE5-00AA0044773D")]
		[ComImport]
		internal interface IOpenRowset
		{
			// Token: 0x0600171D RID: 5917
			[PreserveSig]
			OleDbHResult OpenRowset([In] IntPtr pUnkOuter, [In] tagDBID pTableID, [In] IntPtr pIndexID, [In] ref Guid riid, [In] int cPropertySets, [In] IntPtr rgPropertySets, [MarshalAs(UnmanagedType.Interface)] out object ppRowset);
		}

		// Token: 0x02000180 RID: 384
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[Guid("0C733AB4-2A1C-11CE-ADE5-00AA0044773D")]
		[ComImport]
		internal interface IRow
		{
			// Token: 0x0600171E RID: 5918
			[PreserveSig]
			OleDbHResult GetColumns([In] IntPtr cColumns, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.Struct)] [In] [Out] tagDBCOLUMNACCESS[] rgColumns);
		}

		// Token: 0x02000181 RID: 385
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0C733A7C-2A1C-11CE-ADE5-00AA0044773D")]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface IRowset
		{
			// Token: 0x0600171F RID: 5919
			[Obsolete("not used", true)]
			void AddRefRows();

			// Token: 0x06001720 RID: 5920
			[PreserveSig]
			OleDbHResult GetData([In] IntPtr hRow, [In] IntPtr hAccessor, [In] IntPtr pData);

			// Token: 0x06001721 RID: 5921
			[PreserveSig]
			OleDbHResult GetNextRows([In] IntPtr hChapter, [In] IntPtr lRowsOffset, [In] IntPtr cRows, out IntPtr pcRowsObtained, [In] ref IntPtr pprghRows);

			// Token: 0x06001722 RID: 5922
			[PreserveSig]
			OleDbHResult ReleaseRows([In] IntPtr cRows, [In] SafeHandle rghRows, [In] IntPtr rgRowOptions, [In] IntPtr rgRefCounts, [In] IntPtr rgRowStatus);

			// Token: 0x06001723 RID: 5923
			[Obsolete("not used", true)]
			void RestartPosition();
		}

		// Token: 0x02000182 RID: 386
		[Guid("0C733A55-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[SuppressUnmanagedCodeSecurity]
		[ComImport]
		internal interface IRowsetInfo
		{
			// Token: 0x06001724 RID: 5924
			[PreserveSig]
			OleDbHResult GetProperties([In] int cPropertyIDSets, [In] SafeHandle rgPropertyIDSets, out int pcPropertySets, out IntPtr prgPropertySets);

			// Token: 0x06001725 RID: 5925
			[PreserveSig]
			OleDbHResult GetReferencedRowset([In] IntPtr iOrdinal, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out UnsafeNativeMethods.IRowset ppRowset);
		}

		// Token: 0x02000183 RID: 387
		[SuppressUnmanagedCodeSecurity]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0C733A74-2A1C-11CE-ADE5-00AA0044773D")]
		[ComImport]
		internal interface ISQLErrorInfo
		{
			// Token: 0x06001726 RID: 5926
			[return: MarshalAs(UnmanagedType.I4)]
			int GetSQLInfo([MarshalAs(UnmanagedType.BStr)] out string pbstrSQLState);
		}

		// Token: 0x02000184 RID: 388
		[SuppressUnmanagedCodeSecurity]
		[Guid("0C733A5F-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ITransactionLocal
		{
			// Token: 0x06001727 RID: 5927
			[Obsolete("not used", true)]
			void Commit();

			// Token: 0x06001728 RID: 5928
			[Obsolete("not used", true)]
			void Abort();

			// Token: 0x06001729 RID: 5929
			[Obsolete("not used", true)]
			void GetTransactionInfo();

			// Token: 0x0600172A RID: 5930
			[Obsolete("not used", true)]
			void GetOptionsObject();

			// Token: 0x0600172B RID: 5931
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[PreserveSig]
			OleDbHResult StartTransaction([In] int isoLevel, [In] int isoFlags, [In] IntPtr pOtherOptions, out int pulTransactionLevel);
		}

		// Token: 0x02000185 RID: 389
		// (Invoke) Token: 0x0600172D RID: 5933
		[SuppressUnmanagedCodeSecurity]
		internal delegate int IUnknownQueryInterface(IntPtr pThis, ref Guid riid, ref IntPtr ppInterface);

		// Token: 0x02000186 RID: 390
		// (Invoke) Token: 0x06001731 RID: 5937
		[SuppressUnmanagedCodeSecurity]
		internal delegate OleDbHResult IDataInitializeGetDataSource(IntPtr pThis, IntPtr pUnkOuter, int dwClsCtx, [MarshalAs(UnmanagedType.LPWStr)] string pwszInitializationString, ref Guid riid, ref DataSourceWrapper ppDataSource);

		// Token: 0x02000187 RID: 391
		// (Invoke) Token: 0x06001735 RID: 5941
		[SuppressUnmanagedCodeSecurity]
		internal delegate OleDbHResult IDBInitializeInitialize(IntPtr pThis);

		// Token: 0x02000188 RID: 392
		// (Invoke) Token: 0x06001739 RID: 5945
		[SuppressUnmanagedCodeSecurity]
		internal delegate OleDbHResult IDBCreateSessionCreateSession(IntPtr pThis, IntPtr pUnkOuter, ref Guid riid, ref SessionWrapper ppDBSession);

		// Token: 0x02000189 RID: 393
		// (Invoke) Token: 0x0600173D RID: 5949
		[SuppressUnmanagedCodeSecurity]
		internal delegate OleDbHResult IDBCreateCommandCreateCommand(IntPtr pThis, IntPtr pUnkOuter, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] ref object ppCommand);

		// Token: 0x0200018A RID: 394
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct Trustee
		{
			// Token: 0x06001740 RID: 5952 RVA: 0x00230E98 File Offset: 0x00230298
			internal Trustee(string name)
			{
				this._pMultipleTrustee = IntPtr.Zero;
				this._MultipleTrusteeOperation = 0;
				this._TrusteeForm = 1;
				this._TrusteeType = 1;
				this._name = name;
			}

			// Token: 0x04000CF5 RID: 3317
			internal IntPtr _pMultipleTrustee;

			// Token: 0x04000CF6 RID: 3318
			internal int _MultipleTrusteeOperation;

			// Token: 0x04000CF7 RID: 3319
			internal int _TrusteeForm;

			// Token: 0x04000CF8 RID: 3320
			internal int _TrusteeType;

			// Token: 0x04000CF9 RID: 3321
			[MarshalAs(UnmanagedType.LPTStr)]
			internal string _name;
		}
	}
}
