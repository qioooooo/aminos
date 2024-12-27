using System;
using System.Data.Common;
using System.Text;

namespace System.Data.Odbc
{
	// Token: 0x020001BD RID: 445
	internal static class ODBC32
	{
		// Token: 0x0600196D RID: 6509 RVA: 0x0023EBF0 File Offset: 0x0023DFF0
		internal static string RetcodeToString(ODBC32.RetCode retcode)
		{
			switch (retcode)
			{
			case ODBC32.RetCode.INVALID_HANDLE:
				return "INVALID_HANDLE";
			case ODBC32.RetCode.ERROR:
				break;
			case ODBC32.RetCode.SUCCESS:
				return "SUCCESS";
			case ODBC32.RetCode.SUCCESS_WITH_INFO:
				return "SUCCESS_WITH_INFO";
			default:
				if (retcode == ODBC32.RetCode.NO_DATA)
				{
					return "NO_DATA";
				}
				break;
			}
			return "ERROR";
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0023EC3C File Offset: 0x0023E03C
		internal static OdbcErrorCollection GetDiagErrors(string source, OdbcHandle hrHandle, ODBC32.RetCode retcode)
		{
			OdbcErrorCollection odbcErrorCollection = new OdbcErrorCollection();
			ODBC32.GetDiagErrors(odbcErrorCollection, source, hrHandle, retcode);
			return odbcErrorCollection;
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0023EC5C File Offset: 0x0023E05C
		internal static void GetDiagErrors(OdbcErrorCollection errors, string source, OdbcHandle hrHandle, ODBC32.RetCode retcode)
		{
			if (retcode != ODBC32.RetCode.SUCCESS)
			{
				short num = 0;
				short num2 = 0;
				StringBuilder stringBuilder = new StringBuilder(1024);
				bool flag = true;
				while (flag)
				{
					num += 1;
					string text;
					int num3;
					retcode = hrHandle.GetDiagnosticRecord(num, out text, stringBuilder, out num3, out num2);
					if (ODBC32.RetCode.SUCCESS_WITH_INFO == retcode && stringBuilder.Capacity - 1 < (int)num2)
					{
						stringBuilder.Capacity = (int)(num2 + 1);
						retcode = hrHandle.GetDiagnosticRecord(num, out text, stringBuilder, out num3, out num2);
					}
					flag = retcode == ODBC32.RetCode.SUCCESS || retcode == ODBC32.RetCode.SUCCESS_WITH_INFO;
					if (flag)
					{
						errors.Add(new OdbcError(source, stringBuilder.ToString(), text, num3));
					}
				}
			}
		}

		// Token: 0x04000E3B RID: 3643
		internal const short SQL_COMMIT = 0;

		// Token: 0x04000E3C RID: 3644
		internal const short SQL_ROLLBACK = 1;

		// Token: 0x04000E3D RID: 3645
		private const int SIGNED_OFFSET = -20;

		// Token: 0x04000E3E RID: 3646
		private const int UNSIGNED_OFFSET = -22;

		// Token: 0x04000E3F RID: 3647
		internal const short SQL_ALL_TYPES = 0;

		// Token: 0x04000E40 RID: 3648
		internal const int SQL_NULL_DATA = -1;

		// Token: 0x04000E41 RID: 3649
		internal const int SQL_NO_TOTAL = -4;

		// Token: 0x04000E42 RID: 3650
		internal const int SQL_DEFAULT_PARAM = -5;

		// Token: 0x04000E43 RID: 3651
		internal const int COLUMN_NAME = 4;

		// Token: 0x04000E44 RID: 3652
		internal const int COLUMN_TYPE = 5;

		// Token: 0x04000E45 RID: 3653
		internal const int DATA_TYPE = 6;

		// Token: 0x04000E46 RID: 3654
		internal const int COLUMN_SIZE = 8;

		// Token: 0x04000E47 RID: 3655
		internal const int DECIMAL_DIGITS = 10;

		// Token: 0x04000E48 RID: 3656
		internal const int NUM_PREC_RADIX = 11;

		// Token: 0x04000E49 RID: 3657
		internal const int SQL_NTS = -3;

		// Token: 0x04000E4A RID: 3658
		internal const int SQL_CD_TRUE = 1;

		// Token: 0x04000E4B RID: 3659
		internal const int SQL_CD_FALSE = 0;

		// Token: 0x04000E4C RID: 3660
		internal const int SQL_DTC_DONE = 0;

		// Token: 0x04000E4D RID: 3661
		internal const int SQL_IS_POINTER = -4;

		// Token: 0x04000E4E RID: 3662
		internal const int SQL_IS_PTR = 1;

		// Token: 0x04000E4F RID: 3663
		internal const int MAX_CONNECTION_STRING_LENGTH = 1024;

		// Token: 0x04000E50 RID: 3664
		internal const short SQL_DIAG_SQLSTATE = 4;

		// Token: 0x04000E51 RID: 3665
		internal const short SQL_RESULT_COL = 3;

		// Token: 0x04000E52 RID: 3666
		internal static readonly IntPtr SQL_AUTOCOMMIT_OFF = ADP.PtrZero;

		// Token: 0x04000E53 RID: 3667
		internal static readonly IntPtr SQL_AUTOCOMMIT_ON = new IntPtr(1);

		// Token: 0x04000E54 RID: 3668
		internal static readonly IntPtr SQL_HANDLE_NULL = ADP.PtrZero;

		// Token: 0x04000E55 RID: 3669
		internal static readonly IntPtr SQL_OV_ODBC3 = new IntPtr(3);

		// Token: 0x04000E56 RID: 3670
		internal static readonly IntPtr SQL_CP_OFF = new IntPtr(0);

		// Token: 0x04000E57 RID: 3671
		internal static readonly IntPtr SQL_CP_ONE_PER_DRIVER = new IntPtr(1);

		// Token: 0x04000E58 RID: 3672
		internal static readonly IntPtr SQL_CP_ONE_PER_HENV = new IntPtr(2);

		// Token: 0x020001BE RID: 446
		internal enum SQL_HANDLE : short
		{
			// Token: 0x04000E5A RID: 3674
			ENV = 1,
			// Token: 0x04000E5B RID: 3675
			DBC,
			// Token: 0x04000E5C RID: 3676
			STMT,
			// Token: 0x04000E5D RID: 3677
			DESC
		}

		// Token: 0x020001BF RID: 447
		[Serializable]
		public enum RETCODE
		{
			// Token: 0x04000E5F RID: 3679
			SUCCESS,
			// Token: 0x04000E60 RID: 3680
			SUCCESS_WITH_INFO,
			// Token: 0x04000E61 RID: 3681
			ERROR = -1,
			// Token: 0x04000E62 RID: 3682
			INVALID_HANDLE = -2,
			// Token: 0x04000E63 RID: 3683
			NO_DATA = 100
		}

		// Token: 0x020001C0 RID: 448
		internal enum RetCode : short
		{
			// Token: 0x04000E65 RID: 3685
			SUCCESS,
			// Token: 0x04000E66 RID: 3686
			SUCCESS_WITH_INFO,
			// Token: 0x04000E67 RID: 3687
			ERROR = -1,
			// Token: 0x04000E68 RID: 3688
			INVALID_HANDLE = -2,
			// Token: 0x04000E69 RID: 3689
			NO_DATA = 100
		}

		// Token: 0x020001C1 RID: 449
		internal enum SQL_CONVERT : ushort
		{
			// Token: 0x04000E6B RID: 3691
			BIGINT = 53,
			// Token: 0x04000E6C RID: 3692
			BINARY,
			// Token: 0x04000E6D RID: 3693
			BIT,
			// Token: 0x04000E6E RID: 3694
			CHAR,
			// Token: 0x04000E6F RID: 3695
			DATE,
			// Token: 0x04000E70 RID: 3696
			DECIMAL,
			// Token: 0x04000E71 RID: 3697
			DOUBLE,
			// Token: 0x04000E72 RID: 3698
			FLOAT,
			// Token: 0x04000E73 RID: 3699
			INTEGER,
			// Token: 0x04000E74 RID: 3700
			LONGVARCHAR,
			// Token: 0x04000E75 RID: 3701
			NUMERIC,
			// Token: 0x04000E76 RID: 3702
			REAL,
			// Token: 0x04000E77 RID: 3703
			SMALLINT,
			// Token: 0x04000E78 RID: 3704
			TIME,
			// Token: 0x04000E79 RID: 3705
			TIMESTAMP,
			// Token: 0x04000E7A RID: 3706
			TINYINT,
			// Token: 0x04000E7B RID: 3707
			VARBINARY,
			// Token: 0x04000E7C RID: 3708
			VARCHAR,
			// Token: 0x04000E7D RID: 3709
			LONGVARBINARY
		}

		// Token: 0x020001C2 RID: 450
		[Flags]
		internal enum SQL_CVT
		{
			// Token: 0x04000E7F RID: 3711
			CHAR = 1,
			// Token: 0x04000E80 RID: 3712
			NUMERIC = 2,
			// Token: 0x04000E81 RID: 3713
			DECIMAL = 4,
			// Token: 0x04000E82 RID: 3714
			INTEGER = 8,
			// Token: 0x04000E83 RID: 3715
			SMALLINT = 16,
			// Token: 0x04000E84 RID: 3716
			FLOAT = 32,
			// Token: 0x04000E85 RID: 3717
			REAL = 64,
			// Token: 0x04000E86 RID: 3718
			DOUBLE = 128,
			// Token: 0x04000E87 RID: 3719
			VARCHAR = 256,
			// Token: 0x04000E88 RID: 3720
			LONGVARCHAR = 512,
			// Token: 0x04000E89 RID: 3721
			BINARY = 1024,
			// Token: 0x04000E8A RID: 3722
			VARBINARY = 2048,
			// Token: 0x04000E8B RID: 3723
			BIT = 4096,
			// Token: 0x04000E8C RID: 3724
			TINYINT = 8192,
			// Token: 0x04000E8D RID: 3725
			BIGINT = 16384,
			// Token: 0x04000E8E RID: 3726
			DATE = 32768,
			// Token: 0x04000E8F RID: 3727
			TIME = 65536,
			// Token: 0x04000E90 RID: 3728
			TIMESTAMP = 131072,
			// Token: 0x04000E91 RID: 3729
			LONGVARBINARY = 262144,
			// Token: 0x04000E92 RID: 3730
			INTERVAL_YEAR_MONTH = 524288,
			// Token: 0x04000E93 RID: 3731
			INTERVAL_DAY_TIME = 1048576,
			// Token: 0x04000E94 RID: 3732
			WCHAR = 2097152,
			// Token: 0x04000E95 RID: 3733
			WLONGVARCHAR = 4194304,
			// Token: 0x04000E96 RID: 3734
			WVARCHAR = 8388608,
			// Token: 0x04000E97 RID: 3735
			GUID = 16777216
		}

		// Token: 0x020001C3 RID: 451
		internal enum STMT : short
		{
			// Token: 0x04000E99 RID: 3737
			CLOSE,
			// Token: 0x04000E9A RID: 3738
			DROP,
			// Token: 0x04000E9B RID: 3739
			UNBIND,
			// Token: 0x04000E9C RID: 3740
			RESET_PARAMS
		}

		// Token: 0x020001C4 RID: 452
		internal enum SQL_IS
		{
			// Token: 0x04000E9E RID: 3742
			POINTER = -4,
			// Token: 0x04000E9F RID: 3743
			INTEGER = -6,
			// Token: 0x04000EA0 RID: 3744
			UINTEGER,
			// Token: 0x04000EA1 RID: 3745
			SMALLINT = -8
		}

		// Token: 0x020001C5 RID: 453
		internal enum SQL_TRANSACTION
		{
			// Token: 0x04000EA3 RID: 3747
			READ_UNCOMMITTED = 1,
			// Token: 0x04000EA4 RID: 3748
			READ_COMMITTED,
			// Token: 0x04000EA5 RID: 3749
			REPEATABLE_READ = 4,
			// Token: 0x04000EA6 RID: 3750
			SERIALIZABLE = 8,
			// Token: 0x04000EA7 RID: 3751
			SNAPSHOT = 16
		}

		// Token: 0x020001C6 RID: 454
		internal enum SQL_PARAM
		{
			// Token: 0x04000EA9 RID: 3753
			INPUT = 1,
			// Token: 0x04000EAA RID: 3754
			INPUT_OUTPUT,
			// Token: 0x04000EAB RID: 3755
			OUTPUT = 4,
			// Token: 0x04000EAC RID: 3756
			RETURN_VALUE
		}

		// Token: 0x020001C7 RID: 455
		internal enum SQL_API : ushort
		{
			// Token: 0x04000EAE RID: 3758
			SQLCOLUMNS = 40,
			// Token: 0x04000EAF RID: 3759
			SQLEXECDIRECT = 11,
			// Token: 0x04000EB0 RID: 3760
			SQLGETTYPEINFO = 47,
			// Token: 0x04000EB1 RID: 3761
			SQLPROCEDURECOLUMNS = 66,
			// Token: 0x04000EB2 RID: 3762
			SQLPROCEDURES,
			// Token: 0x04000EB3 RID: 3763
			SQLSTATISTICS = 53,
			// Token: 0x04000EB4 RID: 3764
			SQLTABLES
		}

		// Token: 0x020001C8 RID: 456
		internal enum SQL_DESC : short
		{
			// Token: 0x04000EB6 RID: 3766
			COUNT = 1001,
			// Token: 0x04000EB7 RID: 3767
			TYPE,
			// Token: 0x04000EB8 RID: 3768
			LENGTH,
			// Token: 0x04000EB9 RID: 3769
			OCTET_LENGTH_PTR,
			// Token: 0x04000EBA RID: 3770
			PRECISION,
			// Token: 0x04000EBB RID: 3771
			SCALE,
			// Token: 0x04000EBC RID: 3772
			DATETIME_INTERVAL_CODE,
			// Token: 0x04000EBD RID: 3773
			NULLABLE,
			// Token: 0x04000EBE RID: 3774
			INDICATOR_PTR,
			// Token: 0x04000EBF RID: 3775
			DATA_PTR,
			// Token: 0x04000EC0 RID: 3776
			NAME,
			// Token: 0x04000EC1 RID: 3777
			UNNAMED,
			// Token: 0x04000EC2 RID: 3778
			OCTET_LENGTH,
			// Token: 0x04000EC3 RID: 3779
			ALLOC_TYPE = 1099,
			// Token: 0x04000EC4 RID: 3780
			CONCISE_TYPE = 2,
			// Token: 0x04000EC5 RID: 3781
			DISPLAY_SIZE = 6,
			// Token: 0x04000EC6 RID: 3782
			UNSIGNED = 8,
			// Token: 0x04000EC7 RID: 3783
			UPDATABLE = 10,
			// Token: 0x04000EC8 RID: 3784
			AUTO_UNIQUE_VALUE,
			// Token: 0x04000EC9 RID: 3785
			TYPE_NAME = 14,
			// Token: 0x04000ECA RID: 3786
			TABLE_NAME,
			// Token: 0x04000ECB RID: 3787
			SCHEMA_NAME,
			// Token: 0x04000ECC RID: 3788
			CATALOG_NAME,
			// Token: 0x04000ECD RID: 3789
			BASE_COLUMN_NAME = 22,
			// Token: 0x04000ECE RID: 3790
			BASE_TABLE_NAME
		}

		// Token: 0x020001C9 RID: 457
		internal enum SQL_COLUMN
		{
			// Token: 0x04000ED0 RID: 3792
			COUNT,
			// Token: 0x04000ED1 RID: 3793
			NAME,
			// Token: 0x04000ED2 RID: 3794
			TYPE,
			// Token: 0x04000ED3 RID: 3795
			LENGTH,
			// Token: 0x04000ED4 RID: 3796
			PRECISION,
			// Token: 0x04000ED5 RID: 3797
			SCALE,
			// Token: 0x04000ED6 RID: 3798
			DISPLAY_SIZE,
			// Token: 0x04000ED7 RID: 3799
			NULLABLE,
			// Token: 0x04000ED8 RID: 3800
			UNSIGNED,
			// Token: 0x04000ED9 RID: 3801
			MONEY,
			// Token: 0x04000EDA RID: 3802
			UPDATABLE,
			// Token: 0x04000EDB RID: 3803
			AUTO_INCREMENT,
			// Token: 0x04000EDC RID: 3804
			CASE_SENSITIVE,
			// Token: 0x04000EDD RID: 3805
			SEARCHABLE,
			// Token: 0x04000EDE RID: 3806
			TYPE_NAME,
			// Token: 0x04000EDF RID: 3807
			TABLE_NAME,
			// Token: 0x04000EE0 RID: 3808
			OWNER_NAME,
			// Token: 0x04000EE1 RID: 3809
			QUALIFIER_NAME,
			// Token: 0x04000EE2 RID: 3810
			LABEL
		}

		// Token: 0x020001CA RID: 458
		internal enum SQL_SPECIALCOLS : ushort
		{
			// Token: 0x04000EE4 RID: 3812
			BEST_ROWID = 1,
			// Token: 0x04000EE5 RID: 3813
			ROWVER
		}

		// Token: 0x020001CB RID: 459
		internal enum SQL_SCOPE : ushort
		{
			// Token: 0x04000EE7 RID: 3815
			CURROW,
			// Token: 0x04000EE8 RID: 3816
			TRANSACTION,
			// Token: 0x04000EE9 RID: 3817
			SESSION
		}

		// Token: 0x020001CC RID: 460
		internal enum SQL_NULLABILITY : ushort
		{
			// Token: 0x04000EEB RID: 3819
			NO_NULLS,
			// Token: 0x04000EEC RID: 3820
			NULLABLE,
			// Token: 0x04000EED RID: 3821
			UNKNOWN
		}

		// Token: 0x020001CD RID: 461
		internal enum HANDLER
		{
			// Token: 0x04000EEF RID: 3823
			IGNORE,
			// Token: 0x04000EF0 RID: 3824
			THROW
		}

		// Token: 0x020001CE RID: 462
		internal enum SQL_C : short
		{
			// Token: 0x04000EF2 RID: 3826
			CHAR = 1,
			// Token: 0x04000EF3 RID: 3827
			WCHAR = -8,
			// Token: 0x04000EF4 RID: 3828
			SLONG = -16,
			// Token: 0x04000EF5 RID: 3829
			SSHORT,
			// Token: 0x04000EF6 RID: 3830
			REAL = 7,
			// Token: 0x04000EF7 RID: 3831
			DOUBLE,
			// Token: 0x04000EF8 RID: 3832
			BIT = -7,
			// Token: 0x04000EF9 RID: 3833
			UTINYINT = -28,
			// Token: 0x04000EFA RID: 3834
			SBIGINT = -25,
			// Token: 0x04000EFB RID: 3835
			UBIGINT = -27,
			// Token: 0x04000EFC RID: 3836
			BINARY = -2,
			// Token: 0x04000EFD RID: 3837
			TIMESTAMP = 11,
			// Token: 0x04000EFE RID: 3838
			TYPE_DATE = 91,
			// Token: 0x04000EFF RID: 3839
			TYPE_TIME,
			// Token: 0x04000F00 RID: 3840
			TYPE_TIMESTAMP,
			// Token: 0x04000F01 RID: 3841
			NUMERIC = 2,
			// Token: 0x04000F02 RID: 3842
			GUID = -11,
			// Token: 0x04000F03 RID: 3843
			DEFAULT = 99,
			// Token: 0x04000F04 RID: 3844
			ARD_TYPE = -99
		}

		// Token: 0x020001CF RID: 463
		internal enum SQL_TYPE : short
		{
			// Token: 0x04000F06 RID: 3846
			CHAR = 1,
			// Token: 0x04000F07 RID: 3847
			VARCHAR = 12,
			// Token: 0x04000F08 RID: 3848
			LONGVARCHAR = -1,
			// Token: 0x04000F09 RID: 3849
			WCHAR = -8,
			// Token: 0x04000F0A RID: 3850
			WVARCHAR = -9,
			// Token: 0x04000F0B RID: 3851
			WLONGVARCHAR = -10,
			// Token: 0x04000F0C RID: 3852
			DECIMAL = 3,
			// Token: 0x04000F0D RID: 3853
			NUMERIC = 2,
			// Token: 0x04000F0E RID: 3854
			SMALLINT = 5,
			// Token: 0x04000F0F RID: 3855
			INTEGER = 4,
			// Token: 0x04000F10 RID: 3856
			REAL = 7,
			// Token: 0x04000F11 RID: 3857
			FLOAT = 6,
			// Token: 0x04000F12 RID: 3858
			DOUBLE = 8,
			// Token: 0x04000F13 RID: 3859
			BIT = -7,
			// Token: 0x04000F14 RID: 3860
			TINYINT,
			// Token: 0x04000F15 RID: 3861
			BIGINT,
			// Token: 0x04000F16 RID: 3862
			BINARY = -2,
			// Token: 0x04000F17 RID: 3863
			VARBINARY = -3,
			// Token: 0x04000F18 RID: 3864
			LONGVARBINARY = -4,
			// Token: 0x04000F19 RID: 3865
			TYPE_DATE = 91,
			// Token: 0x04000F1A RID: 3866
			TYPE_TIME,
			// Token: 0x04000F1B RID: 3867
			TIMESTAMP = 11,
			// Token: 0x04000F1C RID: 3868
			TYPE_TIMESTAMP = 93,
			// Token: 0x04000F1D RID: 3869
			GUID = -11,
			// Token: 0x04000F1E RID: 3870
			SS_VARIANT = -150,
			// Token: 0x04000F1F RID: 3871
			SS_UDT = -151,
			// Token: 0x04000F20 RID: 3872
			SS_XML = -152,
			// Token: 0x04000F21 RID: 3873
			SS_UTCDATETIME = -153,
			// Token: 0x04000F22 RID: 3874
			SS_TIME_EX = -154
		}

		// Token: 0x020001D0 RID: 464
		internal enum SQL_ATTR
		{
			// Token: 0x04000F24 RID: 3876
			APP_ROW_DESC = 10010,
			// Token: 0x04000F25 RID: 3877
			APP_PARAM_DESC,
			// Token: 0x04000F26 RID: 3878
			IMP_ROW_DESC,
			// Token: 0x04000F27 RID: 3879
			IMP_PARAM_DESC,
			// Token: 0x04000F28 RID: 3880
			METADATA_ID,
			// Token: 0x04000F29 RID: 3881
			ODBC_VERSION = 200,
			// Token: 0x04000F2A RID: 3882
			CONNECTION_POOLING,
			// Token: 0x04000F2B RID: 3883
			AUTOCOMMIT = 102,
			// Token: 0x04000F2C RID: 3884
			TXN_ISOLATION = 108,
			// Token: 0x04000F2D RID: 3885
			CURRENT_CATALOG,
			// Token: 0x04000F2E RID: 3886
			LOGIN_TIMEOUT = 103,
			// Token: 0x04000F2F RID: 3887
			QUERY_TIMEOUT = 0,
			// Token: 0x04000F30 RID: 3888
			CONNECTION_DEAD = 1209,
			// Token: 0x04000F31 RID: 3889
			SQL_COPT_SS_ENLIST_IN_DTC = 1207
		}

		// Token: 0x020001D1 RID: 465
		internal enum SQL_INFO : ushort
		{
			// Token: 0x04000F33 RID: 3891
			DATA_SOURCE_NAME = 2,
			// Token: 0x04000F34 RID: 3892
			SERVER_NAME = 13,
			// Token: 0x04000F35 RID: 3893
			DRIVER_NAME = 6,
			// Token: 0x04000F36 RID: 3894
			DRIVER_VER,
			// Token: 0x04000F37 RID: 3895
			ODBC_VER = 10,
			// Token: 0x04000F38 RID: 3896
			SEARCH_PATTERN_ESCAPE = 14,
			// Token: 0x04000F39 RID: 3897
			DBMS_VER = 18,
			// Token: 0x04000F3A RID: 3898
			DBMS_NAME = 17,
			// Token: 0x04000F3B RID: 3899
			IDENTIFIER_CASE = 28,
			// Token: 0x04000F3C RID: 3900
			IDENTIFIER_QUOTE_CHAR,
			// Token: 0x04000F3D RID: 3901
			CATALOG_NAME_SEPARATOR = 41,
			// Token: 0x04000F3E RID: 3902
			DRIVER_ODBC_VER = 77,
			// Token: 0x04000F3F RID: 3903
			GROUP_BY = 88,
			// Token: 0x04000F40 RID: 3904
			KEYWORDS,
			// Token: 0x04000F41 RID: 3905
			ORDER_BY_COLUMNS_IN_SELECT,
			// Token: 0x04000F42 RID: 3906
			QUOTED_IDENTIFIER_CASE = 93,
			// Token: 0x04000F43 RID: 3907
			SQL_OJ_CAPABILITIES_30 = 115,
			// Token: 0x04000F44 RID: 3908
			SQL_OJ_CAPABILITIES_20 = 65003,
			// Token: 0x04000F45 RID: 3909
			SQL_SQL92_RELATIONAL_JOIN_OPERATORS = 161
		}
	}
}
