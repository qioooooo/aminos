using System;

namespace System.Data.SqlClient
{
	// Token: 0x02000316 RID: 790
	internal static class TdsEnums
	{
		// Token: 0x040019B1 RID: 6577
		public const short SQL_SERVER_VERSION_SEVEN = 7;

		// Token: 0x040019B2 RID: 6578
		public const string SQL_PROVIDER_NAME = ".Net SqlClient Data Provider";

		// Token: 0x040019B3 RID: 6579
		public const string SDCI_MAPFILENAME = "SqlClientSSDebug";

		// Token: 0x040019B4 RID: 6580
		public const byte SDCI_MAX_MACHINENAME = 32;

		// Token: 0x040019B5 RID: 6581
		public const byte SDCI_MAX_DLLNAME = 16;

		// Token: 0x040019B6 RID: 6582
		public const byte SDCI_MAX_DATA = 255;

		// Token: 0x040019B7 RID: 6583
		public const int SQLDEBUG_OFF = 0;

		// Token: 0x040019B8 RID: 6584
		public const int SQLDEBUG_ON = 1;

		// Token: 0x040019B9 RID: 6585
		public const int SQLDEBUG_CONTEXT = 2;

		// Token: 0x040019BA RID: 6586
		public const string SP_SDIDEBUG = "sp_sdidebug";

		// Token: 0x040019BB RID: 6587
		public const SqlDbType SmallVarBinary = (SqlDbType)24;

		// Token: 0x040019BC RID: 6588
		public const string TCP = "tcp";

		// Token: 0x040019BD RID: 6589
		public const string NP = "np";

		// Token: 0x040019BE RID: 6590
		public const string RPC = "rpc";

		// Token: 0x040019BF RID: 6591
		public const string BV = "bv";

		// Token: 0x040019C0 RID: 6592
		public const string ADSP = "adsp";

		// Token: 0x040019C1 RID: 6593
		public const string SPX = "spx";

		// Token: 0x040019C2 RID: 6594
		public const string VIA = "via";

		// Token: 0x040019C3 RID: 6595
		public const string LPC = "lpc";

		// Token: 0x040019C4 RID: 6596
		public const string INIT_SSPI_PACKAGE = "InitSSPIPackage";

		// Token: 0x040019C5 RID: 6597
		public const string INIT_SESSION = "InitSession";

		// Token: 0x040019C6 RID: 6598
		public const string CONNECTION_GET_SVR_USER = "ConnectionGetSvrUser";

		// Token: 0x040019C7 RID: 6599
		public const string GEN_CLIENT_CONTEXT = "GenClientContext";

		// Token: 0x040019C8 RID: 6600
		public const byte SOFTFLUSH = 0;

		// Token: 0x040019C9 RID: 6601
		public const byte HARDFLUSH = 1;

		// Token: 0x040019CA RID: 6602
		public const byte IGNORE = 2;

		// Token: 0x040019CB RID: 6603
		public const int HEADER_LEN = 8;

		// Token: 0x040019CC RID: 6604
		public const int HEADER_LEN_FIELD_OFFSET = 2;

		// Token: 0x040019CD RID: 6605
		public const int YUKON_HEADER_LEN = 12;

		// Token: 0x040019CE RID: 6606
		public const int MARS_ID_OFFSET = 8;

		// Token: 0x040019CF RID: 6607
		public const int HEADERTYPE_MARS = 2;

		// Token: 0x040019D0 RID: 6608
		public const int SUCCEED = 1;

		// Token: 0x040019D1 RID: 6609
		public const int FAIL = 0;

		// Token: 0x040019D2 RID: 6610
		public const short TYPE_SIZE_LIMIT = 8000;

		// Token: 0x040019D3 RID: 6611
		public const int MIN_PACKET_SIZE = 512;

		// Token: 0x040019D4 RID: 6612
		public const int DEFAULT_LOGIN_PACKET_SIZE = 4096;

		// Token: 0x040019D5 RID: 6613
		public const int MAX_PRELOGIN_PAYLOAD_LENGTH = 1024;

		// Token: 0x040019D6 RID: 6614
		public const int MAX_PACKET_SIZE = 32768;

		// Token: 0x040019D7 RID: 6615
		public const int MAX_SERVER_USER_NAME = 256;

		// Token: 0x040019D8 RID: 6616
		public const byte MIN_ERROR_CLASS = 11;

		// Token: 0x040019D9 RID: 6617
		public const byte MAX_USER_CORRECTABLE_ERROR_CLASS = 16;

		// Token: 0x040019DA RID: 6618
		public const byte FATAL_ERROR_CLASS = 20;

		// Token: 0x040019DB RID: 6619
		public const byte MT_SQL = 1;

		// Token: 0x040019DC RID: 6620
		public const byte MT_LOGIN = 2;

		// Token: 0x040019DD RID: 6621
		public const byte MT_RPC = 3;

		// Token: 0x040019DE RID: 6622
		public const byte MT_TOKENS = 4;

		// Token: 0x040019DF RID: 6623
		public const byte MT_BINARY = 5;

		// Token: 0x040019E0 RID: 6624
		public const byte MT_ATTN = 6;

		// Token: 0x040019E1 RID: 6625
		public const byte MT_BULK = 7;

		// Token: 0x040019E2 RID: 6626
		public const byte MT_OPEN = 8;

		// Token: 0x040019E3 RID: 6627
		public const byte MT_CLOSE = 9;

		// Token: 0x040019E4 RID: 6628
		public const byte MT_ERROR = 10;

		// Token: 0x040019E5 RID: 6629
		public const byte MT_ACK = 11;

		// Token: 0x040019E6 RID: 6630
		public const byte MT_ECHO = 12;

		// Token: 0x040019E7 RID: 6631
		public const byte MT_LOGOUT = 13;

		// Token: 0x040019E8 RID: 6632
		public const byte MT_TRANS = 14;

		// Token: 0x040019E9 RID: 6633
		public const byte MT_OLEDB = 15;

		// Token: 0x040019EA RID: 6634
		public const byte MT_LOGIN7 = 16;

		// Token: 0x040019EB RID: 6635
		public const byte MT_SSPI = 17;

		// Token: 0x040019EC RID: 6636
		public const byte MT_PRELOGIN = 18;

		// Token: 0x040019ED RID: 6637
		public const byte ST_EOM = 1;

		// Token: 0x040019EE RID: 6638
		public const byte ST_AACK = 2;

		// Token: 0x040019EF RID: 6639
		public const byte ST_IGNORE = 2;

		// Token: 0x040019F0 RID: 6640
		public const byte ST_BATCH = 4;

		// Token: 0x040019F1 RID: 6641
		public const byte ST_RESET_CONNECTION = 8;

		// Token: 0x040019F2 RID: 6642
		public const byte ST_RESET_CONNECTION_PRESERVE_TRANSACTION = 16;

		// Token: 0x040019F3 RID: 6643
		public const byte SQLCOLFMT = 161;

		// Token: 0x040019F4 RID: 6644
		public const byte SQLPROCID = 124;

		// Token: 0x040019F5 RID: 6645
		public const byte SQLCOLNAME = 160;

		// Token: 0x040019F6 RID: 6646
		public const byte SQLTABNAME = 164;

		// Token: 0x040019F7 RID: 6647
		public const byte SQLCOLINFO = 165;

		// Token: 0x040019F8 RID: 6648
		public const byte SQLALTNAME = 167;

		// Token: 0x040019F9 RID: 6649
		public const byte SQLALTFMT = 168;

		// Token: 0x040019FA RID: 6650
		public const byte SQLERROR = 170;

		// Token: 0x040019FB RID: 6651
		public const byte SQLINFO = 171;

		// Token: 0x040019FC RID: 6652
		public const byte SQLRETURNVALUE = 172;

		// Token: 0x040019FD RID: 6653
		public const byte SQLRETURNSTATUS = 121;

		// Token: 0x040019FE RID: 6654
		public const byte SQLRETURNTOK = 219;

		// Token: 0x040019FF RID: 6655
		public const byte SQLCONTROL = 174;

		// Token: 0x04001A00 RID: 6656
		public const byte SQLALTCONTROL = 175;

		// Token: 0x04001A01 RID: 6657
		public const byte SQLROW = 209;

		// Token: 0x04001A02 RID: 6658
		public const byte SQLALTROW = 211;

		// Token: 0x04001A03 RID: 6659
		public const byte SQLDONE = 253;

		// Token: 0x04001A04 RID: 6660
		public const byte SQLDONEPROC = 254;

		// Token: 0x04001A05 RID: 6661
		public const byte SQLDONEINPROC = 255;

		// Token: 0x04001A06 RID: 6662
		public const byte SQLOFFSET = 120;

		// Token: 0x04001A07 RID: 6663
		public const byte SQLORDER = 169;

		// Token: 0x04001A08 RID: 6664
		public const byte SQLDEBUG_CMD = 96;

		// Token: 0x04001A09 RID: 6665
		public const byte SQLLOGINACK = 173;

		// Token: 0x04001A0A RID: 6666
		public const byte SQLENVCHANGE = 227;

		// Token: 0x04001A0B RID: 6667
		public const byte SQLSECLEVEL = 237;

		// Token: 0x04001A0C RID: 6668
		public const byte SQLROWCRC = 57;

		// Token: 0x04001A0D RID: 6669
		public const byte SQLCOLMETADATA = 129;

		// Token: 0x04001A0E RID: 6670
		public const byte SQLALTMETADATA = 136;

		// Token: 0x04001A0F RID: 6671
		public const byte SQLSSPI = 237;

		// Token: 0x04001A10 RID: 6672
		public const byte ENV_DATABASE = 1;

		// Token: 0x04001A11 RID: 6673
		public const byte ENV_LANG = 2;

		// Token: 0x04001A12 RID: 6674
		public const byte ENV_CHARSET = 3;

		// Token: 0x04001A13 RID: 6675
		public const byte ENV_PACKETSIZE = 4;

		// Token: 0x04001A14 RID: 6676
		public const byte ENV_LOCALEID = 5;

		// Token: 0x04001A15 RID: 6677
		public const byte ENV_COMPFLAGS = 6;

		// Token: 0x04001A16 RID: 6678
		public const byte ENV_COLLATION = 7;

		// Token: 0x04001A17 RID: 6679
		public const byte ENV_BEGINTRAN = 8;

		// Token: 0x04001A18 RID: 6680
		public const byte ENV_COMMITTRAN = 9;

		// Token: 0x04001A19 RID: 6681
		public const byte ENV_ROLLBACKTRAN = 10;

		// Token: 0x04001A1A RID: 6682
		public const byte ENV_ENLISTDTC = 11;

		// Token: 0x04001A1B RID: 6683
		public const byte ENV_DEFECTDTC = 12;

		// Token: 0x04001A1C RID: 6684
		public const byte ENV_LOGSHIPNODE = 13;

		// Token: 0x04001A1D RID: 6685
		public const byte ENV_PROMOTETRANSACTION = 15;

		// Token: 0x04001A1E RID: 6686
		public const byte ENV_TRANSACTIONMANAGERADDRESS = 16;

		// Token: 0x04001A1F RID: 6687
		public const byte ENV_TRANSACTIONENDED = 17;

		// Token: 0x04001A20 RID: 6688
		public const byte ENV_SPRESETCONNECTIONACK = 18;

		// Token: 0x04001A21 RID: 6689
		public const byte ENV_USERINSTANCE = 19;

		// Token: 0x04001A22 RID: 6690
		public const byte ENV_ROUTING = 20;

		// Token: 0x04001A23 RID: 6691
		public const int DONE_MORE = 1;

		// Token: 0x04001A24 RID: 6692
		public const int DONE_ERROR = 2;

		// Token: 0x04001A25 RID: 6693
		public const int DONE_INXACT = 4;

		// Token: 0x04001A26 RID: 6694
		public const int DONE_PROC = 8;

		// Token: 0x04001A27 RID: 6695
		public const int DONE_COUNT = 16;

		// Token: 0x04001A28 RID: 6696
		public const int DONE_ATTN = 32;

		// Token: 0x04001A29 RID: 6697
		public const int DONE_INPROC = 64;

		// Token: 0x04001A2A RID: 6698
		public const int DONE_RPCINBATCH = 128;

		// Token: 0x04001A2B RID: 6699
		public const int DONE_SRVERROR = 256;

		// Token: 0x04001A2C RID: 6700
		public const int DONE_FMTSENT = 32768;

		// Token: 0x04001A2D RID: 6701
		public const byte MAX_LOG_NAME = 30;

		// Token: 0x04001A2E RID: 6702
		public const byte MAX_PROG_NAME = 10;

		// Token: 0x04001A2F RID: 6703
		public const byte SEC_COMP_LEN = 8;

		// Token: 0x04001A30 RID: 6704
		public const byte MAX_PK_LEN = 6;

		// Token: 0x04001A31 RID: 6705
		public const byte MAX_NIC_SIZE = 6;

		// Token: 0x04001A32 RID: 6706
		public const byte SQLVARIANT_SIZE = 2;

		// Token: 0x04001A33 RID: 6707
		public const byte VERSION_SIZE = 4;

		// Token: 0x04001A34 RID: 6708
		public const int CLIENT_PROG_VER = 100663296;

		// Token: 0x04001A35 RID: 6709
		public const int YUKON_LOG_REC_FIXED_LEN = 94;

		// Token: 0x04001A36 RID: 6710
		public const int TEXT_TIME_STAMP_LEN = 8;

		// Token: 0x04001A37 RID: 6711
		public const int COLLATION_INFO_LEN = 4;

		// Token: 0x04001A38 RID: 6712
		public const int SPHINXORSHILOH_MAJOR = 7;

		// Token: 0x04001A39 RID: 6713
		public const int SPHINX_INCREMENT = 0;

		// Token: 0x04001A3A RID: 6714
		public const int SHILOH_INCREMENT = 1;

		// Token: 0x04001A3B RID: 6715
		public const int DEFAULT_MINOR = 0;

		// Token: 0x04001A3C RID: 6716
		public const int SHILOHSP1_MAJOR = 113;

		// Token: 0x04001A3D RID: 6717
		public const int YUKON_MAJOR = 114;

		// Token: 0x04001A3E RID: 6718
		public const int KATMAI_MAJOR = 115;

		// Token: 0x04001A3F RID: 6719
		public const int SHILOHSP1_INCREMENT = 0;

		// Token: 0x04001A40 RID: 6720
		public const int YUKON_INCREMENT = 9;

		// Token: 0x04001A41 RID: 6721
		public const int KATMAI_INCREMENT = 10;

		// Token: 0x04001A42 RID: 6722
		public const int SHILOHSP1_MINOR = 1;

		// Token: 0x04001A43 RID: 6723
		public const int YUKON_RTM_MINOR = 2;

		// Token: 0x04001A44 RID: 6724
		public const int KATMAI_MINOR = 3;

		// Token: 0x04001A45 RID: 6725
		public const int ORDER_68000 = 1;

		// Token: 0x04001A46 RID: 6726
		public const int USE_DB_ON = 1;

		// Token: 0x04001A47 RID: 6727
		public const int INIT_DB_FATAL = 1;

		// Token: 0x04001A48 RID: 6728
		public const int SET_LANG_ON = 1;

		// Token: 0x04001A49 RID: 6729
		public const int INIT_LANG_FATAL = 1;

		// Token: 0x04001A4A RID: 6730
		public const int ODBC_ON = 1;

		// Token: 0x04001A4B RID: 6731
		public const int SSPI_ON = 1;

		// Token: 0x04001A4C RID: 6732
		public const int REPL_ON = 3;

		// Token: 0x04001A4D RID: 6733
		public const int READONLY_INTENT_ON = 1;

		// Token: 0x04001A4E RID: 6734
		public const byte SQLLenMask = 48;

		// Token: 0x04001A4F RID: 6735
		public const byte SQLFixedLen = 48;

		// Token: 0x04001A50 RID: 6736
		public const byte SQLVarLen = 32;

		// Token: 0x04001A51 RID: 6737
		public const byte SQLZeroLen = 16;

		// Token: 0x04001A52 RID: 6738
		public const byte SQLVarCnt = 0;

		// Token: 0x04001A53 RID: 6739
		public const byte SQLDifferentName = 32;

		// Token: 0x04001A54 RID: 6740
		public const byte SQLExpression = 4;

		// Token: 0x04001A55 RID: 6741
		public const byte SQLKey = 8;

		// Token: 0x04001A56 RID: 6742
		public const byte SQLHidden = 16;

		// Token: 0x04001A57 RID: 6743
		public const byte Nullable = 1;

		// Token: 0x04001A58 RID: 6744
		public const byte Identity = 16;

		// Token: 0x04001A59 RID: 6745
		public const byte Updatability = 11;

		// Token: 0x04001A5A RID: 6746
		public const byte ClrFixedLen = 1;

		// Token: 0x04001A5B RID: 6747
		public const byte IsColumnSet = 4;

		// Token: 0x04001A5C RID: 6748
		public const uint VARLONGNULL = 4294967295U;

		// Token: 0x04001A5D RID: 6749
		public const int VARNULL = 65535;

		// Token: 0x04001A5E RID: 6750
		public const int MAXSIZE = 8000;

		// Token: 0x04001A5F RID: 6751
		public const byte FIXEDNULL = 0;

		// Token: 0x04001A60 RID: 6752
		public const ulong UDTNULL = 18446744073709551615UL;

		// Token: 0x04001A61 RID: 6753
		public const int SQLVOID = 31;

		// Token: 0x04001A62 RID: 6754
		public const int SQLTEXT = 35;

		// Token: 0x04001A63 RID: 6755
		public const int SQLVARBINARY = 37;

		// Token: 0x04001A64 RID: 6756
		public const int SQLINTN = 38;

		// Token: 0x04001A65 RID: 6757
		public const int SQLVARCHAR = 39;

		// Token: 0x04001A66 RID: 6758
		public const int SQLBINARY = 45;

		// Token: 0x04001A67 RID: 6759
		public const int SQLIMAGE = 34;

		// Token: 0x04001A68 RID: 6760
		public const int SQLCHAR = 47;

		// Token: 0x04001A69 RID: 6761
		public const int SQLINT1 = 48;

		// Token: 0x04001A6A RID: 6762
		public const int SQLBIT = 50;

		// Token: 0x04001A6B RID: 6763
		public const int SQLINT2 = 52;

		// Token: 0x04001A6C RID: 6764
		public const int SQLINT4 = 56;

		// Token: 0x04001A6D RID: 6765
		public const int SQLMONEY = 60;

		// Token: 0x04001A6E RID: 6766
		public const int SQLDATETIME = 61;

		// Token: 0x04001A6F RID: 6767
		public const int SQLFLT8 = 62;

		// Token: 0x04001A70 RID: 6768
		public const int SQLFLTN = 109;

		// Token: 0x04001A71 RID: 6769
		public const int SQLMONEYN = 110;

		// Token: 0x04001A72 RID: 6770
		public const int SQLDATETIMN = 111;

		// Token: 0x04001A73 RID: 6771
		public const int SQLFLT4 = 59;

		// Token: 0x04001A74 RID: 6772
		public const int SQLMONEY4 = 122;

		// Token: 0x04001A75 RID: 6773
		public const int SQLDATETIM4 = 58;

		// Token: 0x04001A76 RID: 6774
		public const int SQLDECIMALN = 106;

		// Token: 0x04001A77 RID: 6775
		public const int SQLNUMERICN = 108;

		// Token: 0x04001A78 RID: 6776
		public const int SQLUNIQUEID = 36;

		// Token: 0x04001A79 RID: 6777
		public const int SQLBIGCHAR = 175;

		// Token: 0x04001A7A RID: 6778
		public const int SQLBIGVARCHAR = 167;

		// Token: 0x04001A7B RID: 6779
		public const int SQLBIGBINARY = 173;

		// Token: 0x04001A7C RID: 6780
		public const int SQLBIGVARBINARY = 165;

		// Token: 0x04001A7D RID: 6781
		public const int SQLBITN = 104;

		// Token: 0x04001A7E RID: 6782
		public const int SQLNCHAR = 239;

		// Token: 0x04001A7F RID: 6783
		public const int SQLNVARCHAR = 231;

		// Token: 0x04001A80 RID: 6784
		public const int SQLNTEXT = 99;

		// Token: 0x04001A81 RID: 6785
		public const int SQLUDT = 240;

		// Token: 0x04001A82 RID: 6786
		public const int AOPCNTB = 9;

		// Token: 0x04001A83 RID: 6787
		public const int AOPSTDEV = 48;

		// Token: 0x04001A84 RID: 6788
		public const int AOPSTDEVP = 49;

		// Token: 0x04001A85 RID: 6789
		public const int AOPVAR = 50;

		// Token: 0x04001A86 RID: 6790
		public const int AOPVARP = 51;

		// Token: 0x04001A87 RID: 6791
		public const int AOPCNT = 75;

		// Token: 0x04001A88 RID: 6792
		public const int AOPSUM = 77;

		// Token: 0x04001A89 RID: 6793
		public const int AOPAVG = 79;

		// Token: 0x04001A8A RID: 6794
		public const int AOPMIN = 81;

		// Token: 0x04001A8B RID: 6795
		public const int AOPMAX = 82;

		// Token: 0x04001A8C RID: 6796
		public const int AOPANY = 83;

		// Token: 0x04001A8D RID: 6797
		public const int AOPNOOP = 86;

		// Token: 0x04001A8E RID: 6798
		public const int SQLTIMESTAMP = 80;

		// Token: 0x04001A8F RID: 6799
		public const int MAX_NUMERIC_LEN = 17;

		// Token: 0x04001A90 RID: 6800
		public const int DEFAULT_NUMERIC_PRECISION = 29;

		// Token: 0x04001A91 RID: 6801
		public const int SPHINX_DEFAULT_NUMERIC_PRECISION = 28;

		// Token: 0x04001A92 RID: 6802
		public const int MAX_NUMERIC_PRECISION = 38;

		// Token: 0x04001A93 RID: 6803
		public const int MAX_FLOAT_PRECISION = 53;

		// Token: 0x04001A94 RID: 6804
		public const int MAX_REAL_PRECISION = 24;

		// Token: 0x04001A95 RID: 6805
		public const byte UNKNOWN_PRECISION_SCALE = 255;

		// Token: 0x04001A96 RID: 6806
		public const int SQLINT8 = 127;

		// Token: 0x04001A97 RID: 6807
		public const int SQLVARIANT = 98;

		// Token: 0x04001A98 RID: 6808
		public const int SQLXMLTYPE = 241;

		// Token: 0x04001A99 RID: 6809
		public const int XMLUNICODEBOM = 65279;

		// Token: 0x04001A9A RID: 6810
		public const int SQLTABLE = 243;

		// Token: 0x04001A9B RID: 6811
		public const int SQLDATE = 40;

		// Token: 0x04001A9C RID: 6812
		public const int SQLTIME = 41;

		// Token: 0x04001A9D RID: 6813
		public const int SQLDATETIME2 = 42;

		// Token: 0x04001A9E RID: 6814
		public const int SQLDATETIMEOFFSET = 43;

		// Token: 0x04001A9F RID: 6815
		public const int DEFAULT_VARTIME_SCALE = 7;

		// Token: 0x04001AA0 RID: 6816
		public const ulong SQL_PLP_NULL = 18446744073709551615UL;

		// Token: 0x04001AA1 RID: 6817
		public const ulong SQL_PLP_UNKNOWNLEN = 18446744073709551614UL;

		// Token: 0x04001AA2 RID: 6818
		public const int SQL_PLP_CHUNK_TERMINATOR = 0;

		// Token: 0x04001AA3 RID: 6819
		public const ushort SQL_USHORTVARMAXLEN = 65535;

		// Token: 0x04001AA4 RID: 6820
		public const byte TVP_ROWCOUNT_ESTIMATE = 18;

		// Token: 0x04001AA5 RID: 6821
		public const byte TVP_ROW_TOKEN = 1;

		// Token: 0x04001AA6 RID: 6822
		public const byte TVP_END_TOKEN = 0;

		// Token: 0x04001AA7 RID: 6823
		public const ushort TVP_NOMETADATA_TOKEN = 65535;

		// Token: 0x04001AA8 RID: 6824
		public const byte TVP_ORDER_UNIQUE_TOKEN = 16;

		// Token: 0x04001AA9 RID: 6825
		public const int TVP_DEFAULT_COLUMN = 512;

		// Token: 0x04001AAA RID: 6826
		public const byte TVP_ORDERASC_FLAG = 1;

		// Token: 0x04001AAB RID: 6827
		public const byte TVP_ORDERDESC_FLAG = 2;

		// Token: 0x04001AAC RID: 6828
		public const byte TVP_UNIQUE_FLAG = 4;

		// Token: 0x04001AAD RID: 6829
		public const bool Is68K = false;

		// Token: 0x04001AAE RID: 6830
		public const bool TraceTDS = false;

		// Token: 0x04001AAF RID: 6831
		public const string SP_EXECUTESQL = "sp_executesql";

		// Token: 0x04001AB0 RID: 6832
		public const string SP_PREPEXEC = "sp_prepexec";

		// Token: 0x04001AB1 RID: 6833
		public const string SP_PREPARE = "sp_prepare";

		// Token: 0x04001AB2 RID: 6834
		public const string SP_EXECUTE = "sp_execute";

		// Token: 0x04001AB3 RID: 6835
		public const string SP_UNPREPARE = "sp_unprepare";

		// Token: 0x04001AB4 RID: 6836
		public const string SP_PARAMS = "sp_procedure_params_rowset";

		// Token: 0x04001AB5 RID: 6837
		public const string SP_PARAMS_MANAGED = "sp_procedure_params_managed";

		// Token: 0x04001AB6 RID: 6838
		public const string SP_PARAMS_MGD10 = "sp_procedure_params_100_managed";

		// Token: 0x04001AB7 RID: 6839
		public const ushort RPC_PROCID_CURSOR = 1;

		// Token: 0x04001AB8 RID: 6840
		public const ushort RPC_PROCID_CURSOROPEN = 2;

		// Token: 0x04001AB9 RID: 6841
		public const ushort RPC_PROCID_CURSORPREPARE = 3;

		// Token: 0x04001ABA RID: 6842
		public const ushort RPC_PROCID_CURSOREXECUTE = 4;

		// Token: 0x04001ABB RID: 6843
		public const ushort RPC_PROCID_CURSORPREPEXEC = 5;

		// Token: 0x04001ABC RID: 6844
		public const ushort RPC_PROCID_CURSORUNPREPARE = 6;

		// Token: 0x04001ABD RID: 6845
		public const ushort RPC_PROCID_CURSORFETCH = 7;

		// Token: 0x04001ABE RID: 6846
		public const ushort RPC_PROCID_CURSOROPTION = 8;

		// Token: 0x04001ABF RID: 6847
		public const ushort RPC_PROCID_CURSORCLOSE = 9;

		// Token: 0x04001AC0 RID: 6848
		public const ushort RPC_PROCID_EXECUTESQL = 10;

		// Token: 0x04001AC1 RID: 6849
		public const ushort RPC_PROCID_PREPARE = 11;

		// Token: 0x04001AC2 RID: 6850
		public const ushort RPC_PROCID_EXECUTE = 12;

		// Token: 0x04001AC3 RID: 6851
		public const ushort RPC_PROCID_PREPEXEC = 13;

		// Token: 0x04001AC4 RID: 6852
		public const ushort RPC_PROCID_PREPEXECRPC = 14;

		// Token: 0x04001AC5 RID: 6853
		public const ushort RPC_PROCID_UNPREPARE = 15;

		// Token: 0x04001AC6 RID: 6854
		public const string TRANS_BEGIN = "BEGIN TRANSACTION";

		// Token: 0x04001AC7 RID: 6855
		public const string TRANS_COMMIT = "COMMIT TRANSACTION";

		// Token: 0x04001AC8 RID: 6856
		public const string TRANS_ROLLBACK = "ROLLBACK TRANSACTION";

		// Token: 0x04001AC9 RID: 6857
		public const string TRANS_IF_ROLLBACK = "IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION";

		// Token: 0x04001ACA RID: 6858
		public const string TRANS_SAVE = "SAVE TRANSACTION";

		// Token: 0x04001ACB RID: 6859
		public const string TRANS_READ_COMMITTED = "SET TRANSACTION ISOLATION LEVEL READ COMMITTED";

		// Token: 0x04001ACC RID: 6860
		public const string TRANS_READ_UNCOMMITTED = "SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED";

		// Token: 0x04001ACD RID: 6861
		public const string TRANS_REPEATABLE_READ = "SET TRANSACTION ISOLATION LEVEL REPEATABLE READ";

		// Token: 0x04001ACE RID: 6862
		public const string TRANS_SERIALIZABLE = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE";

		// Token: 0x04001ACF RID: 6863
		public const string TRANS_SNAPSHOT = "SET TRANSACTION ISOLATION LEVEL SNAPSHOT";

		// Token: 0x04001AD0 RID: 6864
		public const byte SHILOH_RPCBATCHFLAG = 128;

		// Token: 0x04001AD1 RID: 6865
		public const byte YUKON_RPCBATCHFLAG = 255;

		// Token: 0x04001AD2 RID: 6866
		public const byte RPC_RECOMPILE = 1;

		// Token: 0x04001AD3 RID: 6867
		public const byte RPC_NOMETADATA = 2;

		// Token: 0x04001AD4 RID: 6868
		public const byte RPC_PARAM_BYREF = 1;

		// Token: 0x04001AD5 RID: 6869
		public const byte RPC_PARAM_DEFAULT = 2;

		// Token: 0x04001AD6 RID: 6870
		public const byte RPC_PARAM_IS_LOB_COOKIE = 8;

		// Token: 0x04001AD7 RID: 6871
		public const string PARAM_OUTPUT = "output";

		// Token: 0x04001AD8 RID: 6872
		public const int MAX_PARAMETER_NAME_LENGTH = 127;

		// Token: 0x04001AD9 RID: 6873
		public const string FMTONLY_ON = " SET FMTONLY ON;";

		// Token: 0x04001ADA RID: 6874
		public const string FMTONLY_OFF = " SET FMTONLY OFF;";

		// Token: 0x04001ADB RID: 6875
		public const string BROWSE_ON = " SET NO_BROWSETABLE ON;";

		// Token: 0x04001ADC RID: 6876
		public const string BROWSE_OFF = " SET NO_BROWSETABLE OFF;";

		// Token: 0x04001ADD RID: 6877
		public const string TABLE = "Table";

		// Token: 0x04001ADE RID: 6878
		public const int EXEC_THRESHOLD = 3;

		// Token: 0x04001ADF RID: 6879
		public const short TIMEOUT_EXPIRED = -2;

		// Token: 0x04001AE0 RID: 6880
		public const short ENCRYPTION_NOT_SUPPORTED = 20;

		// Token: 0x04001AE1 RID: 6881
		public const int LOGON_FAILED = 18456;

		// Token: 0x04001AE2 RID: 6882
		public const int PASSWORD_EXPIRED = 18488;

		// Token: 0x04001AE3 RID: 6883
		public const uint SNI_UNINITIALIZED = 4294967295U;

		// Token: 0x04001AE4 RID: 6884
		public const uint SNI_SUCCESS = 0U;

		// Token: 0x04001AE5 RID: 6885
		public const uint SNI_WAIT_TIMEOUT = 258U;

		// Token: 0x04001AE6 RID: 6886
		public const uint SNI_SUCCESS_IO_PENDING = 997U;

		// Token: 0x04001AE7 RID: 6887
		public const short SNI_WSAECONNRESET = 10054;

		// Token: 0x04001AE8 RID: 6888
		public const uint SNI_SSL_VALIDATE_CERTIFICATE = 1U;

		// Token: 0x04001AE9 RID: 6889
		public const uint SNI_SSL_USE_SCHANNEL_CACHE = 2U;

		// Token: 0x04001AEA RID: 6890
		public const string DEFAULT_ENGLISH_CODE_PAGE_STRING = "iso_1";

		// Token: 0x04001AEB RID: 6891
		public const short DEFAULT_ENGLISH_CODE_PAGE_VALUE = 1252;

		// Token: 0x04001AEC RID: 6892
		public const short CHARSET_CODE_PAGE_OFFSET = 2;

		// Token: 0x04001AED RID: 6893
		internal const int MAX_SERVERNAME = 255;

		// Token: 0x04001AEE RID: 6894
		internal const ushort SELECT = 193;

		// Token: 0x04001AEF RID: 6895
		internal const ushort INSERT = 195;

		// Token: 0x04001AF0 RID: 6896
		internal const ushort DELETE = 196;

		// Token: 0x04001AF1 RID: 6897
		internal const ushort UPDATE = 197;

		// Token: 0x04001AF2 RID: 6898
		internal const ushort ABORT = 210;

		// Token: 0x04001AF3 RID: 6899
		internal const ushort BEGINXACT = 212;

		// Token: 0x04001AF4 RID: 6900
		internal const ushort ENDXACT = 213;

		// Token: 0x04001AF5 RID: 6901
		internal const ushort BULKINSERT = 240;

		// Token: 0x04001AF6 RID: 6902
		internal const ushort OPENCURSOR = 32;

		// Token: 0x04001AF7 RID: 6903
		internal const ushort MERGE = 279;

		// Token: 0x04001AF8 RID: 6904
		internal const ushort MAXLEN_HOSTNAME = 128;

		// Token: 0x04001AF9 RID: 6905
		internal const ushort MAXLEN_USERNAME = 128;

		// Token: 0x04001AFA RID: 6906
		internal const ushort MAXLEN_PASSWORD = 128;

		// Token: 0x04001AFB RID: 6907
		internal const ushort MAXLEN_APPNAME = 128;

		// Token: 0x04001AFC RID: 6908
		internal const ushort MAXLEN_SERVERNAME = 128;

		// Token: 0x04001AFD RID: 6909
		internal const ushort MAXLEN_CLIENTINTERFACE = 128;

		// Token: 0x04001AFE RID: 6910
		internal const ushort MAXLEN_LANGUAGE = 128;

		// Token: 0x04001AFF RID: 6911
		internal const ushort MAXLEN_DATABASE = 128;

		// Token: 0x04001B00 RID: 6912
		internal const ushort MAXLEN_ATTACHDBFILE = 260;

		// Token: 0x04001B01 RID: 6913
		internal const ushort MAXLEN_NEWPASSWORD = 128;

		// Token: 0x04001B02 RID: 6914
		internal const int WHIDBEY_DATE_LENGTH = 10;

		// Token: 0x04001B03 RID: 6915
		public static readonly decimal SQL_SMALL_MONEY_MIN = new decimal(-214748.3648);

		// Token: 0x04001B04 RID: 6916
		public static readonly decimal SQL_SMALL_MONEY_MAX = new decimal(214748.3647);

		// Token: 0x04001B05 RID: 6917
		public static readonly string[] SQLDEBUG_MODE_NAMES = new string[] { "off", "on", "context" };

		// Token: 0x04001B06 RID: 6918
		public static readonly ushort[] CODE_PAGE_FROM_SORT_ID = new ushort[]
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			437, 437, 437, 437, 437, 0, 0, 0, 0, 0,
			850, 850, 850, 850, 850, 0, 0, 0, 0, 850,
			1252, 1252, 1252, 1252, 1252, 850, 850, 850, 850, 850,
			850, 850, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 1252, 1252, 1252, 1252, 1252, 0, 0, 0, 0,
			1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250,
			1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 1250, 0,
			0, 0, 0, 0, 1251, 1251, 1251, 1251, 1251, 0,
			0, 0, 1253, 1253, 1253, 0, 0, 0, 0, 0,
			1253, 1253, 1253, 0, 1253, 0, 0, 0, 1254, 1254,
			1254, 0, 0, 0, 0, 0, 1255, 1255, 1255, 0,
			0, 0, 0, 0, 1256, 1256, 1256, 0, 0, 0,
			0, 0, 1257, 1257, 1257, 1257, 1257, 1257, 1257, 1257,
			1257, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 1252, 1252, 1252, 1252, 0, 0, 0,
			0, 0, 932, 932, 949, 949, 950, 950, 936, 936,
			932, 949, 950, 936, 874, 874, 874, 0, 0, 0,
			1252, 1252, 1252, 1252, 1252, 1252, 1252, 1252, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0, 0, 0
		};

		// Token: 0x04001B07 RID: 6919
		internal static readonly long[] TICKS_FROM_SCALE = new long[] { 10000000L, 1000000L, 100000L, 10000L, 1000L, 100L, 10L, 1L };

		// Token: 0x04001B08 RID: 6920
		internal static readonly int[] WHIDBEY_TIME_LENGTH = new int[] { 8, 10, 11, 12, 13, 14, 15, 16 };

		// Token: 0x04001B09 RID: 6921
		internal static readonly int[] WHIDBEY_DATETIME2_LENGTH = new int[] { 19, 21, 22, 23, 24, 25, 26, 27 };

		// Token: 0x04001B0A RID: 6922
		internal static readonly int[] WHIDBEY_DATETIMEOFFSET_LENGTH = new int[] { 26, 28, 29, 30, 31, 32, 33, 34 };

		// Token: 0x02000317 RID: 791
		internal enum TransactionManagerRequestType
		{
			// Token: 0x04001B0C RID: 6924
			GetDTCAddress,
			// Token: 0x04001B0D RID: 6925
			Propagate,
			// Token: 0x04001B0E RID: 6926
			Begin = 5,
			// Token: 0x04001B0F RID: 6927
			Promote,
			// Token: 0x04001B10 RID: 6928
			Commit,
			// Token: 0x04001B11 RID: 6929
			Rollback,
			// Token: 0x04001B12 RID: 6930
			Save
		}

		// Token: 0x02000318 RID: 792
		internal enum TransactionManagerIsolationLevel
		{
			// Token: 0x04001B14 RID: 6932
			Unspecified,
			// Token: 0x04001B15 RID: 6933
			ReadUncommitted,
			// Token: 0x04001B16 RID: 6934
			ReadCommitted,
			// Token: 0x04001B17 RID: 6935
			RepeatableRead,
			// Token: 0x04001B18 RID: 6936
			Serializable,
			// Token: 0x04001B19 RID: 6937
			Snapshot
		}
	}
}
