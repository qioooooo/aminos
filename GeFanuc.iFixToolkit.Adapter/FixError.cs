using System;

namespace GeFanuc.iFixToolkit.Adapter
{
	public sealed class FixError
	{
		private FixError()
		{
		}

		public const short FE_OK = 0;

		public const short FE_DOS_FCN = 1;

		public const short FE_DOS_FNF = 2;

		public const short FE_DOS_PNF = 3;

		public const short FE_DOS_TMF = 4;

		public const short FE_DOS_ACC = 5;

		public const short FE_DOS_HND = 6;

		public const short FE_DOS_MCB = 7;

		public const short FE_DOS_MEM = 8;

		public const short FE_DOS_SPAWN = 9;

		public const short FE_DOS_DNF = 10;

		public const short FE_FIX_PROCESS_STILL_RUNNING = 89;

		public const short FE_VIRTUAL_MEMORY_LOW = 90;

		public const short FE_FIX_NOT_LOADED = 91;

		public const short FE_NT_WIN32_ERR = 92;

		public const short FE_DUP_PROC = 93;

		public const short FE_FIX_NOTSTARTED = 94;

		public const short FE_SYS_INTEG = 95;

		public const short FE_ERR_INTERN = 96;

		public const short FE_NO_EMS_MAPS = 97;

		public const short FE_NO_EMS_MEMORY = 98;

		public const short FE_MEMORY = 99;

		public const short FE_ERR = 100;

		public const short FE_OPEN = 101;

		public const short FE_CREATE = 102;

		public const short FE_UPDATE = 103;

		public const short FE_CLOSE = 104;

		public const short FE_READ = 105;

		public const short FE_WRITE = 106;

		public const short FE_DIR = 107;

		public const short FE_DEF = 108;

		public const short FE_VERERR = 109;

		public const short FE_FILEPATH = 110;

		public const short FE_DELETE = 111;

		public const short FE_FILE_NOTFOUND = 112;

		public const short FM_FILE_DELETED = 113;

		public const short FE_FILE_EXIST = 114;

		public const short FE_SEEK = 115;

		public const short FE_FKEY = 116;

		public const short FE_KEY_OPTION = 117;

		public const short FE_PRINTER_OFFLINE = 118;

		public const short FE_PRN_NOT_OPENED = 119;

		public const short FE_BACKUP_FAILED = 120;

		public const short FE_NO_DRIVE = 121;

		public const short FE_NO_DIR = 122;

		public const short FE_BAD_FILENAME = 123;

		public const short FE_FILE_RO = 124;

		public const short FE_FILE_NOT_EXIST = 125;

		public const short FE_SPECIFIX_OPTION = 126;

		public const short FE_DMACS_OPTION = 127;

		public const short FE_KEY_DEMO = 128;

		public const short FE_MISSING_KEY = 129;

		public const short FE_MISSING_SEC_FILE = 130;

		public const short FE_KEY_SEC_MISMATCH = 131;

		public const short FE_FATAL = 200;

		public const short FE_INTERNAL = 201;

		public const short FE_WARN = 202;

		public const short FE_ERROR = 203;

		public const short FE_MALLOC = 250;

		public const short FM_ASK_CLEAR_DEF = 251;

		public const short FM_RETURN = 300;

		public const short FM_FILNAM = 301;

		public const short FM_COMMAND = 302;

		public const short FM_INV_SEL = 304;

		public const short FM_HELP = 305;

		public const short FM_NO_DEF = 306;

		public const short FM_DIR_FILS = 307;

		public const short FM_PAGE = 308;

		public const short FM_PATH = 309;

		public const short FM_FILES = 310;

		public const short FM_ASK_SAVE = 311;

		public const short FM_VERSION = 312;

		public const short FM_FMMS = 313;

		public const short FM_LDBA = 314;

		public const short FM_GET_TAG = 315;

		public const short FM_BGN_EXIT = 316;

		public const short FM_MENU_INSERT = 317;

		public const short FM_WORKING = 318;

		public const short FM_FILESAVE = 319;

		public const short FM_IGNORE = 320;

		public const short FM_NEW_TAG = 321;

		public const short FM_EXIT = 322;

		public const short FM_DATA_EXIT = 323;

		public const short FM_MENU_OSTRIKE = 324;

		public const short FM_EXITING = 325;

		public const short FM_PASSWORD = 326;

		public const short FM_NO_FILES = 327;

		public const short FM_TOO_MANY_FILES = 328;

		public const short FM_NO_DIR_FOR_EXTEN = 329;

		public const short FM_FOR_NODE = 330;

		public const short FM_TRY_AGAIN = 331;

		public const short FM_USER_CANCEL = 332;

		public const short FM_USER_ANYWAY = 333;

		public const short FM_BY = 334;

		public const short FE_SAC_STOP = 402;

		public const short FE_DISABLE_SCAN_ON = 403;

		public const short FE_DISABLE_SCAN_OFF = 404;

		public const short FM_SAC_VERSION = 410;

		public const short FM_SAC_INIT = 411;

		public const short FM_SAC_WARM = 412;

		public const short FM_SAC_COLD = 413;

		public const short FM_SAC_OVER = 414;

		public const short FM_SAC_FAIL = 415;

		public const short FE_SAC_RELOAD = 416;

		public const short FE_SAC_NEXT = 417;

		public const short FE_SAC_OFF = 418;

		public const short FE_SAC_DBB = 419;

		public const short FE_SAC_LOOP = 420;

		public const short FE_SAC_BROK = 421;

		public const short FE_SAC_DATAFMT = 422;

		public const short FE_SAC_BADSEC = 423;

		public const short FE_PG_ERROR = 424;

		public const short FE_PG_RUN = 425;

		public const short FE_PG_STOP = 426;

		public const short FE_PG_ERRSTEP = 427;

		public const short FE_PG_MSGSTEP = 428;

		public const short FE_PG_CALL = 429;

		public const short FE_PG_STEP = 430;

		public const short FE_PG_DEBUG = 431;

		public const short FE_PG_OPTYP = 432;

		public const short FE_EV_ERROR = 433;

		public const short FE_EV_RUN = 434;

		public const short FE_EV_STOP = 435;

		public const short FE_EV_OPEN = 436;

		public const short FE_EV_CLOSE = 437;

		public const short FE_EV_OPTYP = 438;

		public const short FE_EV_ALARM = 439;

		public const short FE_SP_CLAMP = 440;

		public const short FE_TV_CLAMP = 441;

		public const short FE_SAC_SCANR = 442;

		public const short FE_SAC_ONSCAN = 443;

		public const short FE_SAC_NOEXCEPT = 444;

		public const short FE_SAC_BADCHAIN = 445;

		public const short FE_SAC_STOPPED = 446;

		public const short FE_SAC_RESTART = 447;

		public const short FE_SAC_PANIC = 448;

		public const short FE_TM_LOGIC = 449;

		public const short FE_PG_BADSTEP = 450;

		public const short FE_SD_WAIT = 451;

		public const short FM_SAC_ALM_RES = 452;

		public const short FM_SAC_ALM_SUS = 453;

		public const short FE_SAC_ERR_STP = 454;

		public const short FE_SAC_INP_PAT = 455;

		public const short FE_SAC_STP = 456;

		public const short FE_SAC_CUR_PAT = 457;

		public const short FE_SAC_NXT_STP = 458;

		public const short FE_SAC_IOERR_STP = 459;

		public const short FE_SAC_FOR_BIT = 460;

		public const short FE_SAC_RTN = 461;

		public const short FE_SAC_ADJUST = 462;

		public const short FE_SAC_NXT_BLK = 463;

		public const short FE_SAC_RESET = 464;

		public const short FE_SAC_IGNORE = 465;

		public const short FE_SAC_PER_DEF = 466;

		public const short FE_SAC_TIM_DEF = 467;

		public const short FE_SAC_SET_CON = 468;

		public const short FM_SAC_ILL_TAR = 469;

		public const short FM_SAC_BLK_MAX = 470;

		public const short FM_SAC_BLK_SEC = 471;

		public const short FM_SAC_BLK_RES = 472;

		public const short FM_SAC_TVC_RES = 473;

		public const short FE_ERR_FROM_DRV = 474;

		public const short FE_SAC_UNSUP_BLK = 475;

		public const short FE_EXCEPT_TIME = 476;

		public const short FM_SAC_REMOTE_ACK = 477;

		public const short FE_IO_ABORT = 500;

		public const short FE_IO_FULL = 501;

		public const short FE_IO_DUP = 502;

		public const short FE_IO_ERR = 510;

		public const short FE_IO_BUSY = 511;

		public const short FE_IO_WAIT = 512;

		public const short FE_IO_DCB = 513;

		public const short FE_IO_ADDR = 514;

		public const short FE_IO_OPT = 515;

		public const short FE_IO_SIG = 516;

		public const short FE_IO_EGU = 517;

		public const short FE_IO_COMM = 518;

		public const short FE_IO_DATAFMT = 519;

		public const short FE_IO_ACCEPTED = 520;

		public const short FE_IO_NOEXCEPT = 521;

		public const short FE_IO_DUP_EXCEPT = 522;

		public const short FE_IO_NODIT = 523;

		public const short FE_IO_DEV = 530;

		public const short FE_IO_SEP = 531;

		public const short FE_IO_1_ADDR = 532;

		public const short FE_IO_2_ADDR = 533;

		public const short FE_IO_3_ADDR = 534;

		public const short FE_IO_4_ADDR = 535;

		public const short FE_IO_5_ADDR = 536;

		public const short FE_IO_6_ADDR = 537;

		public const short FE_IO_7_ADDR = 538;

		public const short FE_IO_8_ADDR = 539;

		public const short FE_IO_9_ADDR = 540;

		public const short FE_IO_10_ADDR = 541;

		public const short FE_IOC_CREATE_DRIVER = 600;

		public const short FE_IOC_START_DRIVER = 601;

		public const short FE_IOC_STOP_DRIVER = 602;

		public const short FE_IOC_TERMINATE_DRIVER = 603;

		public const short FE_IOC_MSLOT_CREATE = 604;

		public const short FE_IOC_MSLOT_WRITE = 605;

		public const short FE_RTL_LOST = 1001;

		public const short FE_RTL_SIZE = 1002;

		public const short FE_RTL_LIMIT = 1003;

		public const short FE_RTL_VERSION = 1004;

		public const short FE_RTL_INTERNAL = 1005;

		public const short FM_RTL_WARM = 1006;

		public const short FM_RTL_LOAD = 1007;

		public const short FM_RTL_RELOAD = 1008;

		public const short FE_RTL_EXIST = 1009;

		public const short FE_NO_TDBSLOTS = 1010;

		public const short FE_UPL_MISMATCH = 1011;

		public const short FE_RTL_FILENAME = 1012;

		public const short FE_RTL_NOPATH = 1013;

		public const short FE_NO87 = 1014;

		public const short FE_TAG_GROUP_NOT_RESOLVED = 1072;

		public const short FE_I_WAS_A_VARIABLE = 1073;

		public const short FE_OLD_NO_SVC = 1101;

		public const short FE_NO_FN = 1102;

		public const short FE_NO_LIB = 1103;

		public const short FE_LIB_RES = 1104;

		public const short FE_NO_FCA = 1105;

		public const short FE_TSR_CONTINUE = 1106;

		public const short FE_NO_SVC = 1150;

		public const short FE_NO_SVC_TST = 1151;

		public const short FE_NO_SVC_FMMS = 1152;

		public const short FE_NO_SVC_LDBA = 1153;

		public const short FE_NO_SVC_DRV0A = 1154;

		public const short FE_NO_SVC_DRV0B = 1155;

		public const short FE_NO_SVC_DRV1A = 1156;

		public const short FE_NO_SVC_DRV1B = 1157;

		public const short FE_NO_SVC_DRV2A = 1158;

		public const short FE_NO_SVC_DRV2B = 1159;

		public const short FE_NO_SVC_DRV3A = 1160;

		public const short FE_NO_SVC_DRV3B = 1161;

		public const short FE_NO_SVC_DRV4A = 1162;

		public const short FE_NO_SVC_DRV4B = 1163;

		public const short FE_NO_SVC_DRV5A = 1164;

		public const short FE_NO_SVC_DRV5B = 1165;

		public const short FE_NO_SVC_DRV6A = 1166;

		public const short FE_NO_SVC_DRV6B = 1167;

		public const short FE_NO_SVC_DRV7A = 1168;

		public const short FE_NO_SVC_DRV7B = 1169;

		public const short FE_NO_SVC_NET = 1170;

		public const short FE_NO_SVC_RDBA = 1171;

		public const short FE_NO_SVC_SACAPI = 1172;

		public const short FE_NO_SVC_ALM = 1173;

		public const short FE_NO_SVC_NNT = 1174;

		public const short FE_NO_SVC_ART = 1175;

		public const short FE_NO_SVC_FIO = 1176;

		public const short FE_NO_SVC_SAM = 1177;

		public const short FE_NO_SVC_TIM = 1178;

		public const short FE_NO_SVC_KEY = 1179;

		public const short FE_NO_SVC_LOK = 1180;

		public const short FE_FMMS_LIB_RES = 1181;

		public const short FE_NO_SVC_DT = 1183;

		public const short FE_NO_SVC_MDBA = 1184;

		public const short FE_MDBA_LIB_RES = 1185;

		public const short FE_ILL_FDT = 1201;

		public const short FE_BLK_ID = 1202;

		public const short FE_FDT_IP = 1203;

		public const short FE_FDT_TSIZ = 1204;

		public const short FE_FDT_TBAD = 1205;

		public const short FE_FDT_BSIZ = 1206;

		public const short FE_FDT_NONE = 1207;

		public const short FE_FDT_TYPE = 1208;

		public const short FE_FDT_FLD = 1209;

		public const short FE_FDT_IPN = 1210;

		public const short FE_NO_INIT = 1211;

		public const short FE_FDT_DATA = 1212;

		public const short FE_FDT_DP = 1213;

		public const short FE_NO_CHECK = 1214;

		public const short FE_EXP_SYN = 1220;

		public const short FE_EXP_LPAR = 1221;

		public const short FE_EXP_RPAR = 1222;

		public const short FE_EXP_TERM = 1223;

		public const short FE_EXP_CMPLX = 1224;

		public const short FE_EXP_OPER = 1225;

		public const short FE_FLT_FLD = 1226;

		public const short FE_FDT_TAGL = 1227;

		public const short FE_FDT_BLOCK = 1228;

		public const short FE_SYN_SYM = 1229;

		public const short FE_PPARM_NUM = 1230;

		public const short FE_PPARM_DEF = 1231;

		public const short FE_LUVAR_NUM = 1232;

		public const short FE_LUVAR_TAG = 1233;

		public const short FE_TIME_NEG = 1234;

		public const short FE_RES_CSTEP = 1235;

		public const short FE_ALM_NOTFOUND = 1236;

		public const short FE_OPC_UNC_Q = 1250;

		public const short FE_STK1 = 1251;

		public const short FE_STK2 = 1252;

		public const short FE_STK3 = 1253;

		public const short FE_STK4 = 1254;

		public const short FE_STK5 = 1255;

		public const short FE_STK6 = 1256;

		public const short FE_STK7 = 1257;

		public const short FE_STK8 = 1258;

		public const short FE_STK9 = 1259;

		public const short FE_STK10 = 1260;

		public const short FE_STRL = 1301;

		public const short FE_NODE_INS = 1302;

		public const short FE_TAG_INS = 1303;

		public const short FE_FLD_INS = 1304;

		public const short FM_NODE_TITLE = 1305;

		public const short FM_TAG_TITLE = 1306;

		public const short FM_FIELD_TITLE = 1307;

		public const short FE_EMPTY_DATABASE = 1308;

		public const short FE_NO_FIELDS = 1309;

		public const short FE_NO_NODES = 1310;

		public const short FE_NODE_ERR = 1311;

		public const short FE_TAG_ERR = 1312;

		public const short FE_FIELD_ERR = 1313;

		public const short FE_FLDTYP_ERR = 1314;

		public const short FM_SYSTEM_TITLE = 1315;

		public const short FE_NO_DRIVERS = 1316;

		public const short FM_DRIVER_TITLE = 1317;

		public const short FE_ACE_NO_TIMERS = 1318;

		public const short FE_ACE_PRIORITY = 1319;

		public const short FE_BUF_TOO_SMALL = 1320;

		public const short FE_FLOATING_FORMAT = 1321;

		public const short FE_DBCHAR_INVALID = 1322;

		public const short FE_NODE_CMD_ARG = 1323;

		public const short FE_NODE_NOT_IN_REG = 1324;

		public const short FE_SCU_CMD_ARG = 1325;

		public const short FE_SCU_NOT_IN_REG = 1326;

		public const short FE_REG_ERR = 1327;

		public const short FE_PSP_ERR = 1328;

		public const short FE_QUE2SMALL = 1351;

		public const short FE_QUE2BIG = 1352;

		public const short FE_QUENULL = 1353;

		public const short FE_QUE_FULL = 1354;

		public const short FE_QUE_INUSE = 1355;

		public const short FE_QUE_HDRSIZE = 1356;

		public const short FE_QUE_ELEMSIZE = 1357;

		public const short FE_QUE_NOTINIT = 1358;

		public const short FE_NO_MENU = 1401;

		public const short FE_REFRESH = 1402;

		public const short FE_MENU_LIN = 1403;

		public const short FE_MENU_COL = 1404;

		public const short FE_MENU_NOTLOAD = 1405;

		public const short FW_MEN_SYSDOWN = 1450;

		public const short FW_BGD_STOP = 1451;

		public const short FE_HLPNONE = 1501;

		public const short FE_HLPNOENT = 1502;

		public const short FE_HLPNOMEM = 1503;

		public const short FE_HLPSTRL = 1504;

		public const short FE_CANT_SAVE_HELP = 1505;

		public const short FE_HLPNOENTB = 1506;

		public const short FM_HLPLOOK = 1550;

		public const short FM_ESC_UP_DN = 1551;

		public const short FM_HELP_TITLE = 1552;

		public const short FE_NET_LOWERR = 1600;

		public const short FE_NET_BAD_BUF_LEN = 1601;

		public const short FE_NET_BAD_CMD = 1603;

		public const short FE_NET_TMO = 1605;

		public const short FE_NET_MSG_INCOMPLETE = 1606;

		public const short FE_NET_ILLEGAL_BUFADDR = 1607;

		public const short FE_NET_BAD_LSN = 1608;

		public const short FE_NET_NO_RES = 1609;

		public const short FE_NET_TERM = 1610;

		public const short FE_NET_CANCELED = 1611;

		public const short FE_NET_DUP_NAME = 1613;

		public const short FE_NET_NAME_TABLE_FULL = 1614;

		public const short FE_NET_NAME_DEREG = 1615;

		public const short FE_NET_LSN_TABLE_FULL = 1617;

		public const short FE_NET_SESS_OPEN_RJCTD = 1618;

		public const short FE_NET_BAD_NUM = 1619;

		public const short FE_NET_NO_ANSWER = 1620;

		public const short FE_NET_BAD_NAME = 1621;

		public const short FE_NET_NAME_IN_USE = 1622;

		public const short FE_NET_NAME_DELETED = 1623;

		public const short FE_NET_ABNORMAL_END = 1624;

		public const short FE_NET_NAME_CONFLICT = 1625;

		public const short FE_NET_BAD_REM_DEV = 1626;

		public const short FE_NET_INTERFACE_BUSY = 1633;

		public const short FE_NET_2_MANY_OUT = 1634;

		public const short FE_NET_BAD_LANA_NUM = 1635;

		public const short FE_NET_CPLT_WHILE_CAN = 1636;

		public const short FE_NET_BAD_CANCEL = 1638;

		public const short FE_NET_LCL_NAME_IN_USE = 1648;

		public const short FE_NET_UNKNOWN = 1650;

		public const short FE_NET_DLL = 1651;

		public const short FE_NET_NO_RESET = 1652;

		public const short FE_NET_OS_RESOURCE_EX = 1653;

		public const short FE_NET_MAX_APPS = 1654;

		public const short FE_NET_NO_SAPS = 1655;

		public const short FE_NET_NO_OS_RESOURCES = 1656;

		public const short FE_NET_INVALID_ADDR = 1657;

		public const short FE_NET_INVDDID = 1659;

		public const short FE_NET_LOCKFAIL = 1660;

		public const short FE_NETBIOS_PROTOCOL_NAME = 1662;

		public const short FE_NET_NOT_LOADED = 1663;

		public const short FE_NET_SYS_ERR = 1664;

		public const short FE_NET_HOT_CARRIER_DETECT = 1665;

		public const short FE_NET_HOT_CARRIER_SENT = 1666;

		public const short FE_NET_NO_CARRIER = 1667;

		public const short FE_NET_BAD_AD = 1670;

		public const short FE_NET_NO_ADAPTER = 1671;

		public const short FE_NET_ALREADY_LOADED = 1678;

		public const short FE_NET_CABLE_FAULT = 1679;

		public const short FE_NET_CFG_FORM = 1683;

		public const short FE_NET_RES = 1685;

		public const short FE_NET_HANDLE = 1686;

		public const short FE_NET_NO_FN = 1687;

		public const short FE_NET_BINDING_BUF_TOO_SMALL = 1688;

		public const short FE_WINSOCK_ERROR = 1693;

		public const short FE_TCP_NAME = 1694;

		public const short FE_NET_WAIT = 1695;

		public const short FE_ILL_NETID = 1696;

		public const short FE_TCP_CFG = 1697;

		public const short FE_NET_CFG = 1698;

		public const short FE_NETW_INVALID = 1699;

		public const short FE_NO_DBB = 1701;

		public const short FE_NO_SLOTS = 1702;

		public const short FE_ILL_SCAN = 1703;

		public const short FE_ILL_ISCAN = 1704;

		public const short FE_ILL_SMOOTH = 1705;

		public const short FE_BADEGU = 1706;

		public const short FE_RANGE = 1707;

		public const short FE_DECIMAL = 1708;

		public const short FE_RW_UNALLOC = 1709;

		public const short FE_RW_LOCK = 1710;

		public const short FE_ILL_ADI = 1711;

		public const short FE_ILL_PRIO = 1712;

		public const short FE_ILL_ALM = 1713;

		public const short FE_NO_IO_SLOTS = 1714;

		public const short FE_TAG_DUP = 1715;

		public const short FE_OUT_INV = 1716;

		public const short FE_INV_COLD = 1717;

		public const short FE_FLD_SIZ = 1718;

		public const short FE_INV_DCT = 1719;

		public const short FE_INV_DIG = 1720;

		public const short FE_NO_PDATA_PUT = 1721;

		public const short FE_NO_REMOTE_SP = 1722;

		public const short FE_NOT_IN_LOCAL = 1723;

		public const short FE_MSG_LONG = 1724;

		public const short FE_TWA_OR_DIG = 1725;

		public const short FE_BAD_VAL = 1726;

		public const short FE_PG_NOWAIT = 1727;

		public const short FE_BAD_STMT = 1728;

		public const short FE_BAD_OPER = 1729;

		public const short FE_BAD_RAMP = 1730;

		public const short FE_BAD_SELMODE = 1731;

		public const short FE_NO_EXP = 1732;

		public const short FE_TWA_OR_TAG = 1733;

		public const short FE_BAD_EXP = 1734;

		public const short FE_INV_DAY = 1735;

		public const short FE_INV_OPCODE = 1736;

		public const short FE_INV_VAR = 1737;

		public const short FE_MONTH = 1738;

		public const short FE_DATE = 1739;

		public const short FE_YEAR = 1740;

		public const short FE_BAD_TWA = 1741;

		public const short FE_BAD_TIME = 1742;

		public const short FE_NOT_IN_MANL = 1743;

		public const short FE_NOT_YET = 1744;

		public const short FE_NOT_ENOUGH = 1745;

		public const short FE_TAG_ADD = 1747;

		public const short FE_CANT_ALLOC = 1748;

		public const short FE_NOPATH = 1749;

		public const short FE_TAG_NOTF = 1750;

		public const short FE_NEED_O_OR_C = 1751;

		public const short FE_BAD_BIT = 1752;

		public const short FE_BIT_REPEAT = 1753;

		public const short FE_NOT_PRIM = 1754;

		public const short FE_BAD_KEY = 1755;

		public const short FE_NOT_IN_CHAIN = 1756;

		public const short FE_IN_NCHNS = 1757;

		public const short FE_CHAIN_MAX = 1758;

		public const short FE_SPAN = 1759;

		public const short FE_BADINTERVAL = 1760;

		public const short FE_NOT_AT_ALL = 1761;

		public const short FE_PDB_SN = 1762;

		public const short FE_DBASE_BIG = 1763;

		public const short FE_WAIT_RELOAD = 1764;

		public const short FE_BAD_OPTION = 1765;

		public const short FE_BAD_CHAIN = 1766;

		public const short FE_INVLASCII = 1767;

		public const short FE_BITOVF = 1768;

		public const short FE_INVPREFIX = 1769;

		public const short FE_CHAROVF = 1770;

		public const short FE_INVTYPE = 1771;

		public const short FE_WILD_CARD = 1773;

		public const short FE_OUT_NOT_ALLOWED = 1774;

		public const short FE_VALID_MSG = 1775;

		public const short FE_ILL_OPER = 1776;

		public const short FE_BAD_PDB = 1777;

		public const short FE_DBB_SAVE_RELOAD = 1778;

		public const short FE_NO_ALM_VAL = 1779;

		public const short FE_NO_ALM_TYPE = 1780;

		public const short FE_NO_CONTACT = 1781;

		public const short FE_SIGNIF_FDIGITS = 1782;

		public const short FE_SIGNIF_DDIGITS = 1783;

		public const short FE_NOT_ON_SCAN = 1784;

		public const short FE_IN_CHAIN = 1785;

		public const short FE_BAD_RELOAD_STATE = 1786;

		public const short FE_BAD_SAVE_STATE = 1787;

		public const short FE_BAD_STOP_STATE = 1788;

		public const short FE_REM_BLK_TYP = 1789;

		public const short FE_BTK_CONFIG = 1790;

		public const short FE_BTK_CONFIG_ERR = 1791;

		public const short FE_BTK_BLOCK_MISMATCH = 1792;

		public const short FE_REMAP_THREAD = 1793;

		public const short FE_BLOCK_NO_10_CHAR = 1794;

		public const short FE_DELBLK_FAILED = 1795;

		public const short FM_WARN_NOTSIGNED = 1796;

		public const short FM_ERROR_NOTSIGNED = 1797;

		public const short FE_NOT_SIGNED = 1798;

		public const short FM_WARN_BYPASSED = 1799;

		public const short FM_WARN_BYPASSED2 = 1800;

		public const short FE_SES_HANDLELIST_FULL = 1802;

		public const short FE_SES_NO_SESS = 1803;

		public const short FE_SES_SESS_OK = 1804;

		public const short FE_SES_LOC_NAME = 1806;

		public const short FE_SES_TMO = 1807;

		public const short FE_SES_PLEN = 1808;

		public const short FE_SES_OPEN = 1809;

		public const short FE_SES_CFG_FORM = 1810;

		public const short FE_SM_RES = 1901;

		public const short FE_SM_LOC_NODE = 1903;

		public const short FE_SM_NO_LOAD = 1904;

		public const short FE_SM_VERS = 1905;

		public const short FE_SM_STATE = 1906;

		public const short FE_SM_NO_ENTRY = 1907;

		public const short FE_SM_HOST_REG = 1908;

		public const short FE_SM_LOADED = 1909;

		public const short FE_SM_REQ_OUT = 1910;

		public const short FE_SM_WAIT = 1911;

		public const short FE_SM_STAT_REQ = 1912;

		public const short FE_SM_NO_RSP = 1913;

		public const short FE_SM_NO_SESS = 1914;

		public const short FE_SM_SESS_DOWN = 1915;

		public const short FE_SM_RUNNING = 1916;

		public const short FE_SM_NOT_RUNNING = 1917;

		public const short FE_SM_OK_SESS = 1918;

		public const short FE_SM_NO_SESS2 = 1919;

		public const short FM_SM_NETINI = 1920;

		public const short FE_SM_SEND_VAL = 1921;

		public const short FE_SM_RCV_VAL = 1922;

		public const short FE_SM_SEND_LT_RCV = 1923;

		public const short FE_SM_TMO_MOD = 1924;

		public const short FE_SM_TIMERS = 1925;

		public const short FE_CFM_CORRECTED = 1951;

		public const short FE_CFM_NO_MEM = 1952;

		public const short FE_CFM_NO_OWN = 1953;

		public const short FE_CM_DYNAMIC_CONNECT = 1960;

		public const short FE_CM_PENDING_CONNECT = 1961;

		public const short FE_CM_DISABLED_CONNECT = 1962;

		public const short FE_CM_FAILED_CONNECT = 1963;

		public const short FE_CM_SHUTDOWN = 1964;

		public const short FE_CM_INVALID_LOGICAL = 1965;

		public const short FE_NNT_ACCESS_QUOTA = 2000;

		public const short FE_NNT_FULL = 2001;

		public const short FE_NNT_DUP_NAME = 2002;

		public const short FE_NNT_BAD_ID = 2003;

		public const short FE_NNT_ENTRY_IN_USE = 2004;

		public const short FE_NNT_NO_NAME = 2005;

		public const short FE_NNT_INITIALIZED = 2006;

		public const short FE_NR_TIMEOUT = 2007;

		public const short FE_NR_HANDLELIST_EMPTY = 2008;

		public const short FE_NNT_NO_LOAD = 2009;

		public const short FE_NNT_VERS = 2010;

		public const short FE_NNT_LOADED = 2011;

		public const short FE_NR_PLEN = 2012;

		public const short FE_NR_OPEN = 2013;

		public const short FE_NR_CFG_FORM = 2014;

		public const short FE_NNT_NO_FN = 2015;

		public const short FE_NBT_NO_LOAD = 2016;

		public const short FE_NBT_BAD_ID = 2017;

		public const short FE_LCT_NO_LOAD = 2019;

		public const short FM_NR_WAIT = 2020;

		public const short FE_LCT_BAD_ID = 2021;

		public const short FE_SKT_NO_LOAD = 2022;

		public const short FE_SKT_BAD_ID = 2023;

		public const short FE_BAD_NODENAME = 2040;

		public const short FE_NNT_EXTPTR_IN_USE = 2041;

		public const short FE_NDK_RES_ALREADY_OPEN = 2050;

		public const short FE_NDK_RES_NOT_OPEN = 2051;

		public const short FE_NDK_NCT_FULL = 2052;

		public const short FE_NDK_NCT_EMPTY = 2053;

		public const short FE_NDK_BUF_SIZE_OVERLAP = 2054;

		public const short FE_NDK_CALLBACK_NULL = 2055;

		public const short FE_NDK_INVALID_SERVICE = 2056;

		public const short FE_NDK_INVALID_MOD_ID = 2057;

		public const short FE_NDK_NETWORK_NOT_CFG = 2058;

		public const short FE_NDK_APPL_NOT_OPEN = 2059;

		public const short FE_NDK_INTERFACE_ERR = 2060;

		public const short FE_NDK_INVALID_OPTION = 2061;

		public const short FE_NDK_MULT_RECEIVES = 2062;

		public const short FE_NDK_INVALID_PROTO = 2063;

		public const short FE_NDK_NO_KEY_INFO = 2064;

		public const short FE_NDK_MULT_SYNCH_SEND = 2065;

		public const short FE_NDK_RECV_BAD_SEQ = 2066;

		public const short FE_NDK_RES_CLOSED_NETW = 2067;

		public const short FE_NDK_FAILOVER_NOT_ENABLED = 2068;

		public const short FE_NDK_FAILOVER_OCCURRED = 2069;

		public const short FE_NDK_FAILOVER_MANUAL = 2070;

		public const short FE_NDK_FAILOVER_DISABLED = 2071;

		public const short FE_NDK_FAILOVER_ENABLED = 2072;

		public const short FE_RDB_TOO_SMALL = 2101;

		public const short FE_RDB_RES = 2102;

		public const short FE_RDB_HANDLE = 2103;

		public const short FE_RDB_FORMAT = 2104;

		public const short FE_RDB_WAIT = 2105;

		public const short FE_RDB_NO_VARS_REQ = 2107;

		public const short FE_RDB_DONE = 2108;

		public const short FE_RDB_BAD_SIZE = 2109;

		public const short FE_RDB_CFG_FORM = 2110;

		public const short FE_RDB_BAD_VSP = 2111;

		public const short FE_RDB_LONG_TAG = 2112;

		public const short FE_RDB_LONG_FIELD = 2113;

		public const short FE_NDB_MAX_CALL = 2120;

		public const short FE_NO_MODEM_CONNECTION = 2151;

		public const short FE_REMOIU_NOT_OUTGOING = 2152;

		public const short FE_MDB_RES = 2153;

		public const short FE_MDB_QUEUE_FULL = 2154;

		public const short FE_MDB_HANDLE = 2155;

		public const short FE_MDB_FORMAT = 2156;

		public const short FE_MDB_WAIT = 2157;

		public const short FE_REMOIU_NOT_ACTIVE = 2158;

		public const short FE_MDB_ALREADY_CONNECTED = 2159;

		public const short FE_ILLEGAL_STATUS_TYPE = 2160;

		public const short FE_MDB_BAD_SIZE = 2161;

		public const short FE_ILLEGAL_MNT_VERSION = 2162;

		public const short FE_RECEIVE_COMPLETE = 2163;

		public const short FE_SRV_FORMAT = 2164;

		public const short FE_RESPONSE_BUFFER_SMALL = 2165;

		public const short FE_XMIT_MSG_TOO_LARGE = 2166;

		public const short FE_ILLEGAL_PASSWORD = 2167;

		public const short FE_PASSWRD_READ_ERR = 2168;

		public const short FE_INVALID_USER_ID = 2169;

		public const short FE_READ_ONLY_ACCESS = 2170;

		public const short FE_DEVICE_DISABLED = 2171;

		public const short FE_MDBA_LIMIT = 2172;

		public const short FE_SRV_CHDR = 2201;

		public const short FE_SRV_SIZE = 2202;

		public const short FE_SRV_VAP_TYPE = 2203;

		public const short FE_SRV_PROT = 2204;

		public const short FE_SRV_SES_DOWN = 2205;

		public const short FE_SRV_UPL_TYPE = 2206;

		public const short FE_SRV_RSP2BIG = 2207;

		public const short FE_SRV_SESS_ESTAB = 2208;

		public const short FE_SRV_NO_LDBA = 2209;

		public const short FE_SRV_NO_MEM = 2210;

		public const short FE_SRV_NO_NODES = 2211;

		public const short FE_SRV_NODE_DIS = 2212;

		public const short FE_SRV_NOT_RUNNING = 2213;

		public const short FM_SRV_NODE_DIS = 2230;

		public const short FE_SRV_SES_PROB = 2231;

		public const short FE_SRV_BAD_READ = 2232;

		public const short FE_SRV_ACC_WRITE = 2233;

		public const short FE_SRV_ACC_DEF_VAR = 2234;

		public const short FE_SRV_ACC_DEL_VAR = 2235;

		public const short FE_SRV_ACC_SAV_DB = 2236;

		public const short FE_SRV_ACC_REL_INI = 2237;

		public const short FE_SRV_ACC_REL_TER = 2238;

		public const short FE_FMS_RES = 2301;

		public const short FE_FMS_WAIT = 2302;

		public const short FE_FMS_HANDLE = 2303;

		public const short FE_FMS_NO_FN = 2304;

		public const short FE_FMS_TIMEOUT = 2305;

		public const short FE_FMS_HANDLELIST_EMPTY = 2306;

		public const short FE_FMS_TOO_SMALL = 2307;

		public const short FE_FMS_BAD_VSP = 2308;

		public const short FE_FMS_LIMIT = 2309;

		public const short FE_FMS_NO_RES = 2310;

		public const short FE_FMS_NO_NODE = 2311;

		public const short FE_FMS_MINIVIEW = 2312;

		public const short FE_INCOMPLETE = 2313;

		public const short FE_DDE_OP_PENDING = 2314;

		public const short FE_RAH_RES = 2401;

		public const short FE_HLA_HANDLE = 2501;

		public const short FE_HLA_ACCESS = 2502;

		public const short FE_HLA_BUFFER = 2503;

		public const short FE_LOK_LOGIN = 2601;

		public const short FE_LOK_PASSWORD = 2602;

		public const short FE_LOK_NOTLOAD = 2603;

		public const short FM_LOGIN = 2604;

		public const short FM_LOGOUT = 2605;

		public const short FM_AREA_MSG = 2606;

		public const short FM_APPL_MSG = 2607;

		public const short FM_SPPAS_MSG = 2608;

		public const short FM_PASS_PROMPT = 2609;

		public const short FE_LOCK_NOMATCH = 2610;

		public const short FE_LOCK_PASSWORD = 2611;

		public const short FE_LOCK_NOKEY = 2612;

		public const short FM_TRY_LOGIN = 2613;

		public const short FM_TGE_PROMPT = 2701;

		public const short FM_TGE_DIR = 2702;

		public const short FM_EDIT = 2703;

		public const short FE_INPUT = 2704;

		public const short FM_TGE_NEW = 2705;

		public const short FE_TG_VERSION_MISMATCH = 2706;

		public const short FE_TG_SYM_NOT_FOUND = 2707;

		public const short FE_TG_SYM_EXISTS = 2708;

		public const short FE_TG_SYMBOL_TOO_LONG = 2709;

		public const short FE_TG_SUBST_TOO_LONG = 2710;

		public const short FE_TG_DESC_TOO_LONG = 2711;

		public const short FE_TG_CANT_READ = 2712;

		public const short FE_TG_NEED_TG = 2713;

		public const short FE_ALM_INVALID_FORMAT = 2750;

		public const short FE_ALM_NO_DESTINATION = 2751;

		public const short FE_ALM_QUEUE_OVERFLOW = 2752;

		public const short FE_ALM_INVALID_API_PARAM = 2753;

		public const short FE_FTYPER_NOT_LOADED = 2775;

		public const short FE_CANT_OPEN_ALMFILE = 2776;

		public const short FE_CANT_CLOSE_ALMFILE = 2777;

		public const short FE_FTYPER_DISABLED = 2778;

		public const short FE_ALM_BAD_TIMESTAMP = 2779;

		public const short FE_AAD_BACKUP_WRITE = 2780;

		public const short FE_AAD_FILE_NOT_FOUND = 2781;

		public const short FE_AAD_INVALID_FORMAT = 2782;

		public const short FE_AAD_BAD_VERSION = 2783;

		public const short FE_AAD_READ_ERROR = 2784;

		public const short FE_AAD_WRITE_ERROR = 2785;

		public const short FE_AAD_FILE_NOT_OPEN = 2786;

		public const short FE_AAD_INVALID_AREA_HAN = 2787;

		public const short FE_AAD_INVALID_AREA_SIZE = 2788;

		public const short FE_AAD_AREA_NOT_FOUND = 2789;

		public const short FE_AAD_AREA_WAS_DELETED = 2790;

		public const short FE_AAD_WRONG_FILE_ID = 2791;

		public const short FE_AAD_INVALID_AREA_SYNTAX = 2792;

		public const short FE_SUM_ADI = 2800;

		public const short FE_SUM_COLOR = 2801;

		public const short FE_SUM_FIL = 2802;

		public const short FE_SUM_POS = 2803;

		public const short FE_SUM_LEN = 2804;

		public const short FM_SUM_DATE_IN = 2810;

		public const short FM_SUM_TIME_IN = 2811;

		public const short FM_SUM_DATE_LST = 2812;

		public const short FM_SUM_TIME_LST = 2813;

		public const short FM_SUM_SNODE = 2814;

		public const short FM_SUM_TAG = 2815;

		public const short FM_SUM_STAT = 2816;

		public const short FM_SUM_VALUE = 2817;

		public const short FM_SUM_DESC = 2818;

		public const short FM_SUM_PAGE = 2819;

		public const short FM_SUM_TIME = 2820;

		public const short FM_SUM_PRIORITY = 2821;

		public const short FM_SUM_NODE = 2822;

		public const short FM_SUM_TYPE = 2823;

		public const short FE_SUM_VERERR = 2824;

		public const short FM_SUM_ALMACK = 2825;

		public const short FM_SUM_STATUS = 2826;

		public const short FM_SUM_ENABLE = 2827;

		public const short FM_SUM_DISABLE = 2828;

		public const short FM_SUM_NEWALM = 2829;

		public const short FM_SUM_TOP = 2830;

		public const short FM_SUM_END = 2831;

		public const short FM_SUM_DELALM = 2832;

		public const short FM_SUM_CLRALM = 2833;

		public const short FM_SUM_ALMDEL = 2834;

		public const short FM_SUM_ALMCLR = 2835;

		public const short FE_SUM_ACK = 2836;

		public const short FM_SUM_MENU_TITLE = 2837;

		public const short FM_SUM_CURRENT_TIME = 2838;

		public const short FM_SUM_FSTATUS = 2839;

		public const short FM_SUM_ALARM_ACK = 2840;

		public const short FE_ALMSUM_INVALID_FILTER = 2841;

		public const short FE_ALMSUM_TAGNOTF = 2842;

		public const short FM_KME_PROMPT = 2900;

		public const short FM_KME_DIR = 2901;

		public const short FM_KME_PAGE = 2902;

		public const short FE_KME_KEYDEF = 2903;

		public const short FE_KME_KEYINV = 2904;

		public const short FE_KME_KEYFULL = 2905;

		public const short FM_KME_KEYINFO = 2906;

		public const short FM_KME_NEW = 2907;

		public const short FE_EMAN_NOT_RUNNING = 3000;

		public const short FE_EMAN_MEMORY = 3001;

		public const short FE_EMAN_EVENT_DESC = 3002;

		public const short FE_EMAN_SCANTIME_DESC = 3003;

		public const short FE_EMAN_ACTIVATION_FLAG = 3004;

		public const short FE_EMAN_DETECTION_FLAG = 3005;

		public const short FE_EMAN_HANDLE = 3006;

		public const short FE_EMAN_CALLBACK_ID = 3007;

		public const short FE_EMAN_PARENTHESIS = 3010;

		public const short FE_EMAN_UNBALANCED = 3011;

		public const short FE_EMAN_LOGIC = 3012;

		public const short FE_EMAN_TIME = 3013;

		public const short FE_EMAN_WEEKDAY = 3014;

		public const short FE_EMAN_DATE = 3015;

		public const short FE_EMAN_MONITOR = 3016;

		public const short FE_EMAN_TAG_VALUE = 3017;

		public const short FE_EMAN_COMPARATOR = 3018;

		public const short FE_EMAN_INCOMPATIBLE = 3019;

		public const short FE_EMAN_ALARM_EVENT = 3020;

		public const short FE_EMAN_ALARM_VALUE = 3021;

		public const short FE_EMAN_CHANGE_EVENT = 3022;

		public const short FE_EMAN_NODE_TAG_FIELD = 3023;

		public const short FE_EMAN_DEADBAND = 3024;

		public const short FE_EMAN_TIME_EXPRESSION = 3025;

		public const short FE_EMAN_WEEKDAY_VALUE = 3026;

		public const short FE_EMAN_DATE_EXPRESSION = 3027;

		public const short FE_EMAN_YEAR_VALUE = 3028;

		public const short FE_EMAN_DATE_RANGE = 3029;

		public const short FE_EMAN_LEAP_YEAR = 3030;

		public const short FE_EMAN_NODE_TAG = 3031;

		public const short FM_EMAN_STARTUP = 3050;

		public const short FM_EMAN_CLOSE = 3051;

		public const short FM_EMAN_SYS_FAIL = 3052;

		public const short FM_DCON_EM = 3053;

		public const short FE_PROG_NAME_PVIEW = 3100;

		public const short FE_PROG_NAME_VIEW = 3101;

		public const short FE_PROG_NAME_PDRAW = 3102;

		public const short FE_PROG_NAME_DRAW = 3103;

		public const short FE_PROG_NAME_DBM = 3104;

		public const short FE_PROG_NAME_DBB = 3105;

		public const short FE_PROG_NAME_MENU = 3106;

		public const short FE_PROG_NAME_NSD = 3107;

		public const short FE_PROG_NAME_SUM = 3108;

		public const short FE_PROG_NAME_SC = 3109;

		public const short FE_PROG_NAME_DC = 3110;

		public const short FE_PROG_NAME_REP = 3111;

		public const short FE_PROG_NAME_RCP = 3112;

		public const short FE_PROG_NAME_TGE = 3113;

		public const short FE_PROG_NAME_KME = 3114;

		public const short FE_PROG_NAME_SCU = 3115;

		public const short FE_PROG_NAME_NCU = 3116;

		public const short FE_PROG_NAME_PGU = 3117;

		public const short FE_PROG_NAME_LOG = 3118;

		public const short FE_PROG_NAME_SAC = 3119;

		public const short FE_PROG_NAME_SC1 = 3120;

		public const short FE_PROG_NAME_SC2 = 3121;

		public const short FE_PROG_NAME_ALM = 3122;

		public const short FE_PROG_NAME_DBS = 3123;

		public const short FE_PROG_NAME_SMON = 3124;

		public const short FE_PROG_NAME_SCHD = 3125;

		public const short FE_PROG_NAME_EM = 3127;

		public const short FE_PROG_NAME_HTA = 3128;

		public const short FE_PROG_NAME_HTC = 3129;

		public const short FE_PROG_NAME_HTD = 3130;

		public const short FE_PROG_NAME_DTU = 3131;

		public const short FE_PROG_TITLE_PVIEW = 3200;

		public const short FE_PROG_TITLE_VIEW = 3201;

		public const short FE_PROG_TITLE_PDRAW = 3202;

		public const short FE_PROG_TITLE_DRAW = 3203;

		public const short FE_PROG_TITLE_DBM = 3204;

		public const short FE_PROG_TITLE_DBB = 3205;

		public const short FE_PROG_TITLE_MENU = 3206;

		public const short FE_PROG_TITLE_NSD = 3207;

		public const short FE_PROG_TITLE_SUM = 3208;

		public const short FE_PROG_TITLE_SC = 3209;

		public const short FE_PROG_TITLE_DC = 3210;

		public const short FE_PROG_TITLE_REP = 3211;

		public const short FE_PROG_TITLE_RCP = 3212;

		public const short FE_PROG_TITLE_TGE = 3213;

		public const short FE_PROG_TITLE_KME = 3214;

		public const short FE_PROG_TITLE_SCU = 3215;

		public const short FE_PROG_TITLE_NCU = 3216;

		public const short FE_PROG_TITLE_PGU = 3217;

		public const short FE_PROG_TITLE_LOG = 3218;

		public const short FE_PROG_TITLE_SAC = 3219;

		public const short FE_PROG_TITLE_SC1 = 3220;

		public const short FE_PROG_TITLE_SC2 = 3221;

		public const short FE_PROG_TITLE_ALM = 3222;

		public const short FE_PROG_TITLE_DBS = 3223;

		public const short FE_PROG_TITLE_SMON = 3224;

		public const short FE_PROG_TITLE_SCHD = 3225;

		public const short FE_PROG_TITLE_EM = 3227;

		public const short FE_PROG_TITLE_HTA = 3228;

		public const short FE_PROG_TITLE_HTC = 3229;

		public const short FE_PROG_TITLE_HTD = 3230;

		public const short FE_PROG_TITLE_DTU = 3231;

		public const short FE_INIT_NOLOG = 3300;

		public const short FE_INIT_REENTER = 3301;

		public const short FE_OSX_TYPE = 3400;

		public const short FE_OSX_CREATE = 3401;

		public const short FE_OSX_NOWAY = 3402;

		public const short FE_OSX_NOTFOUND = 3403;

		public const short FE_OSX_BADCLOSE = 3404;

		public const short FE_OSX_NOTOURS = 3405;

		public const short FE_OSX_QCREATE = 3406;

		public const short FE_OSX_QMUTEX = 3407;

		public const short FE_OSX_QDOUBLE = 3408;

		public const short FE_OSX_QMAP = 3409;

		public const short FE_OSX_QFULL = 3410;

		public const short FE_OSX_QEMPTY = 3411;

		public const short FE_OSX_INDEX = 3412;

		public const short FE_OSX_TIMO = 3413;

		public const short FE_OSX_NAMESIZE = 3414;

		public const short FE_OSX_OBJSECURITY = 3415;

		public const short FE_ART_LOWERR = 3500;

		public const short FE_ART_STAT_TYPE = 3501;

		public const short FE_ART_UNKNOWN = 3502;

		public const short FE_ART_STAT_ID = 3503;

		public const short FE_ART_NAME_ID = 3504;

		public const short FE_ART_RES = 3505;

		public const short FE_ART_QCF_RES = 3506;

		public const short FE_NO_ARTIC_CFG = 3507;

		public const short FE_ART_HANDLE = 3508;

		public const short FE_ART_WAIT = 3509;

		public const short FE_ART_NOT_INUSE = 3510;

		public const short FE_ART_ATTACH = 3511;

		public const short FE_ART_LISTEN = 3512;

		public const short FE_ART_BAD_SESSION = 3513;

		public const short FE_ART_NO_RCV_OUT = 3514;

		public const short FE_ART_BAD_MSG = 3515;

		public const short FE_ART_RCV_STAT_CFG = 3516;

		public const short FE_ART_BUFSIZ = 3517;

		public const short FM_ART_SUPPORT = 3518;

		public const short FE_ART_NO_MORE_STREAMS = 3601;

		public const short FE_ERR_NO_MEM = 3602;

		public const short FE_ART_NO_STREAM = 3603;

		public const short FE_ART_STN_ALRDY_EXTS = 3604;

		public const short FE_ART_STRM_ALRDY_EXTS = 3605;

		public const short FE_ART_NO_STATION = 3606;

		public const short FE_ART_BADTYPE = 3607;

		public const short FE_ART_OPTION = 3609;

		public const short FE_ART_STAT = 3610;

		public const short FE_ART_GET_STAT = 3611;

		public const short FE_ART_ALREADY_OPEN = 3612;

		public const short FE_ART_NO_MORE_MBX = 3614;

		public const short FE_ART_NO_RICMON = 3615;

		public const short FE_ART_NOT_OPEN = 3617;

		public const short FE_ART_STATION_ABSENT = 3618;

		public const short FE_ART_NO_WRITE_BUFFER = 3619;

		public const short FE_ART_WRITE_TOO_BIG = 3620;

		public const short FE_ART_TOO_MANY_WRITE = 3621;

		public const short FE_ART_NO_MESSAGE = 3622;

		public const short FE_ART_TOO_MANY_READ = 3623;

		public const short FE_ART_READ_TOO_SMALL = 3624;

		public const short FE_ART_TIMEOUT = 3625;

		public const short FE_ART_NAME_IN_USE = 3626;

		public const short FE_ART_SVC_TIMEOUT = 3627;

		public const short FE_ART_CANT_QECB = 3628;

		public const short FE_ART_BAD_STRM_NAME = 3629;

		public const short FE_ART_NO_TIMER = 3630;

		public const short FE_ART_BAD_STN_NAME = 3631;

		public const short FE_ART_POOL_ALRDY_EXTS = 3632;

		public const short FE_ART_NO_MORE_POOLS = 3633;

		public const short FE_ART_NO_POOL = 3634;

		public const short FE_ART_NO_POOL_MEM = 3635;

		public const short FE_ART_POOL_SIZE = 3636;

		public const short FE_ART_TYPE_MISMATCH = 3637;

		public const short FE_ART_CANT_WAKE_SU = 3638;

		public const short FE_ART_BAD_POOL_BUF = 3639;

		public const short ILL_FLDTYP = 4038;

		public const short FE_FLOAT_ERR = 4039;

		public const short FE_FFR_NO_REPORT = 4100;

		public const short FE_FFR_FINISHED_MSG = 4101;

		public const short FE_FFR_FINISHED_TER = 4102;

		public const short FE_FFR_FINISHED_ERR = 4103;

		public const short FE_FFR_OPERATOR_ABORT = 4104;

		public const short FE_FFR_SHELL_CON = 4105;

		public const short FE_FFR_ERR_HEADER = 4120;

		public const short FE_FFR_NOERR_HEADER = 4121;

		public const short FE_FFR_ERR_ACC = 4122;

		public const short FE_FFR_ERR_FMT = 4123;

		public const short FE_FFR_NODE = 4124;

		public const short FE_FFR_TAG = 4125;

		public const short FE_FFR_FIELD = 4126;

		public const short FE_FFR_BAD_LINK = 4127;

		public const short FE_FFR_VAL_LONG = 4128;

		public const short FE_FFR_OPTION_LONG = 4129;

		public const short FE_NO_ALM_RESP = 4300;

		public const short FE_BAD_DDEA_ITEM = 4301;

		public const short FE_DDE_NO_SERVER = 4302;

		public const short FE_ART_STRM_MISMATCH = 4500;

		public const short FE_ART_ACK = 4501;

		public const short FE_ART_BAD_MBX = 4502;

		public const short FE_ART_MBX_ERR = 4503;

		public const short FE_ART_EMM_CTXNUM = 4504;

		public const short FE_ART_INTERNAL = 4505;

		public const short FE_FIO_DISK_SPACE_ERR = 4800;

		public const short FE_FIO_BACKUP_DEL_FAILED = 4801;

		public const short FE_FIO_SAVE_AND_BACKUP_FAILED = 4802;

		public const short FE_NET_INIT = 4900;

		public const short FE_FMS_INIT = 4901;

		public const short FE_RDB_INIT = 4902;

		public const short FE_ART_INIT = 4903;

		public const short FE_IPC_WAIT = 5000;

		public const short FE_IPC_TIMEOUT = 5001;

		public const short FE_IPC_UNKNOWN = 5002;

		public const short FE_IPC_PROTOCOL = 5003;

		public const short FM_NETDMACS_STARTUP = 5100;

		public const short FE_NETDMACS_SESS_OK = 5101;

		public const short FE_NETDMACS_SESS_FAIL = 5102;

		public const short FM_NETDMACS_CLOSE = 5103;

		public const short FM_NETDMACS_SYS_FAIL = 5104;

		public const short FE_NETDMACS_NOT_RUNNING = 5105;

		public const short FE_NETDMACS_ERR_MSG_1 = 5106;

		public const short FE_NETDMACS_ERR_MSG_2 = 5107;

		public const short FE_NETDMACS_ERR_MSG_3 = 5108;

		public const short FE_NETDMACS_ERR_MSG_4 = 5109;

		public const short FM_SAVE_CHANGES = 5210;

		public const short FE_FILE_VERSION = 5211;

		public const short FE_FILE_TYPE = 5212;

		public const short FE_FILE_READ = 5213;

		public const short FE_FILE_WRITE = 5214;

		public const short FE_FILE_OPEN = 5215;

		public const short FM_END_OF_LIST = 5220;

		public const short FM_YES = 5221;

		public const short FM_NO = 5222;

		public const short FM_CANCEL = 5223;

		public const short FE_NOMATCH = 5224;

		public const short FM_DISMISS = 5225;

		public const short FM_ASK_DELETE_FILE = 5226;

		public const short FM_ASK_OVERWRITE_FILE = 5227;

		public const short FM_FILE_SAVED = 5228;

		public const short FM_OUTFILE_NAME = 5240;

		public const short FM_OUTFILE_PACKAGE = 5241;

		public const short FM_OUTFILE_TIME = 5242;

		public const short FE_NO_INPUT = 5250;

		public const short FM_NODE_FILTER = 5300;

		public const short FM_NODENAME = 5301;

		public const short FM_TAGNAME = 5302;

		public const short FM_FILTER = 5303;

		public const short FE_FORMAT_NO_JUST = 5304;

		public const short FE_FORMAT_NO_NUM = 5305;

		public const short FE_FORMAT_BAD_LENGTH = 5306;

		public const short FE_FORMAT_NO_TYPE = 5307;

		public const short FE_FORMAT_VAL2SMALL = 5308;

		public const short FE_FORMAT_LEN2BIG = 5309;

		public const short FM_OVERVIEW = 5400;

		public const short FM_LCU_HEADING1 = 5500;

		public const short FM_LCU_HEADING2 = 5501;

		public const short FM_LCU_HEADING3 = 5502;

		public const short FM_LCU_HEADING4 = 5503;

		public const short FM_LCU_HEADING5 = 5504;

		public const short FM_LCU_HEADING6 = 5505;

		public const short FM_LCU_HEADING8 = 5506;

		public const short FM_LCU_QUERY1 = 5507;

		public const short FM_LCU_QUERY2 = 5508;

		public const short FM_LCU_QUERY3 = 5509;

		public const short FM_LCU_QUERY4 = 5510;

		public const short FM_LCU_ISSUE = 5511;

		public const short FM_LCU_CUSTOMER = 5512;

		public const short FM_LCU_ID = 5513;

		public const short FM_LCU_VERSION = 5514;

		public const short FM_LCU_CPU = 5515;

		public const short FM_LCU_UNITS = 5516;

		public const short FM_LCU_TERMINATION = 5517;

		public const short FM_LCU_SYSTEM = 5518;

		public const short FM_LCU_HARDWARE = 5519;

		public const short FM_LCU_CHECKSUM = 5520;

		public const short FE_LICENSE_INVALID = 5521;

		public const short FE_LICENSE_TERMINATED = 5522;

		public const short FE_LICENSE_CPU = 5523;

		public const short FE_LICENSE_DATE = 5524;

		public const short FE_LICENSE_COMMAND = 5525;

		public const short FE_LICENSE_NOT_LOADED = 5526;

		public const short FE_LCU_OK1 = 5527;

		public const short FM_LCU_LIST = 5528;

		public const short FM_LCU_ADD = 5529;

		public const short FM_LCU_MODIFY = 5530;

		public const short FM_LCU_REMOVE = 5531;

		public const short FM_LCU_SHOW = 5532;

		public const short FM_LCU_ENTER = 5533;

		public const short FM_LCU_HEADINGA = 5534;

		public const short FM_LCU_HEADINGB = 5535;

		public const short FM_LCU_HEADINGC = 5536;

		public const short FE_LICENSE_UNITS = 5537;

		public const short FM_LCU_QUERY5 = 5538;

		public const short FM_LCU_REMOVE1 = 5539;

		public const short FE_LICENSE_RELEASE = 5540;

		public const short FE_LICENSE_ALTERED = 5541;

		public const short FM_LCU_STARTUP = 5542;

		public const short FM_LCU_SERIAL = 5543;

		public const short FE_LICENSE_LOADED = 5544;

		public const short FM_LCU_HEADING7 = 5545;

		public const short FE_LCU_TIMEOUT = 5546;

		public const short FE_LICENSE_ISSUE = 5547;

		public const short FM_LCU_OPTIONS = 5548;

		public const short FE_LCU_PRODUCTNAME = 5549;

		public const short FM_XSCHD_NAME = 5600;

		public const short FE_XSCHD_COMMAND = 5601;

		public const short FE_XSCHD_TIME = 5602;

		public const short FE_XSCHD_DATE = 5603;

		public const short FE_XSCHD_NODE = 5604;

		public const short FE_XSCHD_TAG = 5605;

		public const short FM_XDCON_NAME = 5700;

		public const short FM_XDCON_SUMMARY_REMARK = 5701;

		public const short FM_XDCON_SNAPSHOT_REMARK = 5702;

		public const short FM_XDCON_SETPOINT_REMARK = 5703;

		public const short FM_XDCON_MANUAL_REMARK = 5704;

		public const short FM_XDCON_BATCH_ID = 5705;

		public const short FM_XDCON_BATCH_START = 5706;

		public const short FM_XDCON_BATCH_STOP = 5707;

		public const short FM_XDCON_BATCH_INTERVAL = 5708;

		public const short FM_XDCON_ACTION_NUM = 5709;

		public const short FM_XDCON_EVENT_NUM = 5710;

		public const short FM_XDCON_ACTION_REMARK = 5711;

		public const short FM_XDCON_EVENT_REMARK = 5712;

		public const short FM_XDCON_SUM_MODE = 5713;

		public const short FM_XDCON_SUM_FILE_SEL = 5714;

		public const short FM_XDCON_SNAP_FILE_SEL = 5715;

		public const short FM_XDCON_SET_FILE_SEL = 5716;

		public const short FM_XDCON_MANUAL_FILE_SEL = 5717;

		public const short FM_XDCON_EVENT_NAME_SEL = 5718;

		public const short FM_XDCON_EVENT_NAME_UNDEF = 5719;

		public const short FM_XDCON_SUMMARY_FILE_UNDEF = 5720;

		public const short FM_XDCON_SNAPSHOT_FILE_UNDEF = 5721;

		public const short FM_XDCON_SETPOINT_FILE_UNDEF = 5722;

		public const short FM_XDCON_MANUAL_FILE_UNDEF = 5723;

		public const short FM_XDCON_BATCH_FILE_UNDEF = 5724;

		public const short FM_XDCON_BATCH_FILE_SEL = 5725;

		public const short FM_XDCON_BATCH_EXISTS = 5726;

		public const short FM_XDCON_SUM_OUT_FILE_SEL = 5727;

		public const short FM_XDCON_SNAP_OUT_FILE_SEL = 5728;

		public const short FM_XDCON_SET_OUT_FILE_SEL = 5729;

		public const short FM_XDCON_MANUAL_OUT_FILE_SEL = 5730;

		public const short FM_XDCON_CONFIG_FILE_SEL = 5731;

		public const short FM_XDCON_BATCH_SUFFIX = 5732;

		public const short FM_XDCON_BATCH_SUFFIX_TAG = 5733;

		public const short FM_XDCON_BATCH_SUSPEND = 5734;

		public const short FM_XDCON_BATCH_RESUME = 5735;

		public const short FM_XDCON_ACTION_MSG1 = 5736;

		public const short FM_XDCON_ACTION_MSG2 = 5737;

		public const short FM_XDCON_ACTION_MSG3 = 5738;

		public const short FM_XDCON_ACTION_MSG4 = 5739;

		public const short FE_XDCON_DUPLICATE_EVENT = 5740;

		public const short FE_XDCON_INPUT_EVENT_DEF = 5741;

		public const short FE_XDCON_INPUT_SUMMARY_MODE = 5742;

		public const short FE_XDCON_INPUT_NODE_TAG_FIELD = 5743;

		public const short FE_XDCON_INPUT_FORMAT = 5744;

		public const short FE_XDCON_INPUT_FILE_NAME = 5745;

		public const short FE_XDCON_INPUT_BSTART_EVENT = 5746;

		public const short FE_XDCON_INPUT_BSTOP_EVENT = 5747;

		public const short FE_XDCON_INPUT_BSCAN_INTERVAL = 5748;

		public const short FE_XDCON_INPUT_ASTART_EVENT = 5749;

		public const short FE_XDCON_INPUT_ASTOP_EVENT = 5750;

		public const short FE_XDCON_INPUT_ASCAN_INTERVAL = 5751;

		public const short FE_XDCON_INPUT_EVENT_NAME = 5752;

		public const short FE_XDCON_INPUT_BATCH_ID = 5753;

		public const short FE_XDCON_INVALID_SUMMARY_MODE = 5761;

		public const short FE_XDCON_INVALID_FORMAT = 5762;

		public const short FE_XDCON_INVALID_NODE_TAG_FIELD = 5763;

		public const short FE_XDCON_INVALID_BSCAN_INTERVAL = 5764;

		public const short FE_XDCON_INVALID_ASCAN_INTERVAL = 5765;

		public const short FE_XDCON_INVALID_BSTART_EVENT = 5766;

		public const short FE_XDCON_INVALID_BSTOP_EVENT = 5767;

		public const short FE_XDCON_INVALID_BSUSPEND_EVENT = 5768;

		public const short FE_XDCON_INVALID_BRESUME_EVENT = 5769;

		public const short FE_XDCON_INVALID_ASTART_EVENT = 5770;

		public const short FE_XDCON_INVALID_ASTOP_EVENT = 5771;

		public const short FE_XDCON_NOTDEF_OUTFILE = 5772;

		public const short FM_XDCON_REMOVE_BATCH = 5773;

		public const short FM_XDCON_EXE_FILE_SEL = 5774;

		public const short FE_XDCON_SUM_TYPE_MISMATCH = 5775;

		public const short FM_XDCON_QUIT_XDCON = 5776;

		public const short FE_XDCON_FIELD_FORMAT_MISMATCH = 5777;

		public const short FM_XDCON_FILE_MISMATCH = 5778;

		public const short FM_XDCON_FILE_MISMATCH1 = 5779;

		public const short FM_XDCON_RENAME = 5780;

		public const short FM_XDCON_DELETE = 5781;

		public const short FE_XDCON_FILE_RENAME_EXISTS = 5782;

		public const short FE_XDCON_NO_ACTIONS = 5783;

		public const short FM_XFVIEW_NAME = 5800;

		public const short FM_XFVIEW_ID = 5801;

		public const short FM_XFVIEW_START_DATE = 5802;

		public const short FM_XFVIEW_START_TIME = 5803;

		public const short FM_XFVIEW_STOP_DATE = 5804;

		public const short FM_XFVIEW_STOP_TIME = 5805;

		public const short FE_XFVIEW_FORMAT = 5806;

		public const short FM_XDEMO_NAME = 5900;

		public const short FM_XNSD_NAME = 6030;

		public const short FM_XNSD_ESTABLISHED = 6031;

		public const short FM_XNSD_INACTIVE = 6032;

		public const short FE_BM_MAX_LIST_FULL = 6100;

		public const short FE_BM_MALLOC_FAIL = 6101;

		public const short FE_BM_TIMEOUT = 6102;

		public const short FE_BM_INSTALL = 6103;

		public const short FE_BM_UNKNOWN_BATCH = 6104;

		public const short FE_BM_MATCH = 6105;

		public const short FE_BM_START_ERROR = 6106;

		public const short FE_BM_STOP_ERROR = 6107;

		public const short FE_BM_SUSPEND_ERROR = 6108;

		public const short FE_BM_RESUME_ERROR = 6109;

		public const short FE_BM_NOT_RUNNING = 6110;

		public const short FE_BM_APPL_FORMAT = 6111;

		public const short FE_BM_SYSTEM_ERROR = 6112;

		public const short FE_BM_OPEN_ACT_FILE = 6113;

		public const short FE_BM_OUTPUT_FILE_TYPE = 6114;

		public const short FE_BM_OPEN_DEF_FILE = 6115;

		public const short FE_BM_ACT_TYPE = 6116;

		public const short FE_BM_READ_EVENT = 6117;

		public const short FE_BM_READ_ACTION = 6118;

		public const short FE_BM_IS_RUNNING = 6119;

		public const short FE_BM_MULTIPLE_BATCHES = 6120;

		public const short FE_BM_EXECUTE = 6121;

		public const short FE_BM_EXECUTE_TMP = 6122;

		public const short FE_BM_EXECUTE_PROC = 6123;

		public const short FE_BM_UNKNOWN_ACTION = 6124;

		public const short FE_BM_ACTION_STATE = 6125;

		public const short FE_BM_CONTROL_REC_MAPPING = 6126;

		public const short FE_FTB_NAME_TOO_BIG = 6127;

		public const short FE_FTB_NO_SLOTS = 6128;

		public const short FE_FTB_FILE_OPEN_ERROR = 6129;

		public const short FE_BM_BADBACKUP = 6130;

		public const short FE_BM_NOBACKUPPATH = 6131;

		public const short FM_BATCH_START = 6132;

		public const short FE_BM_OPEN_FILE = 6133;

		public const short FE_BM_INVALID_EVT = 6134;

		public const short FM_BM_INSTALL = 6135;

		public const short FM_BM_SUSPEND = 6136;

		public const short FM_BM_RESUME = 6137;

		public const short FE_BM_ACT_EVT = 6138;

		public const short FM_BM_START = 6139;

		public const short FM_BM_DELETE = 6140;

		public const short FE_BM_SUS_EVT = 6141;

		public const short FM_BM_STOP = 6142;

		public const short FM_BM_MODIFY = 6143;

		public const short FE_BM_ALLOC_EVT = 6144;

		public const short FE_BM_ALLOC_CHG = 6145;

		public const short FE_BM_EVT_CALL = 6146;

		public const short FE_BM_CHG_EVT = 6147;

		public const short FE_BM_OVERRUN = 6148;

		public const short FM_BM_ACT_START = 6149;

		public const short FM_BM_ACT_STOP = 6150;

		public const short FE_BM_SCAN_OVERRUN = 6151;

		public const short FE_BM_DQP_ERR = 6152;

		public const short FM_BM_DQP_OK = 6153;

		public const short FE_BM_VMS_ERR = 6154;

		public const short FE_BM_NO_SPACE = 6155;

		public const short FM_BM_BACKUP_FULL = 6156;

		public const short FE_BM_IO_ERR = 6157;

		public const short FE_BM_ALLOC_DATA = 6158;

		public const short FE_DECNET_OTHERS = 6200;

		public const short FE_DECNET_INSFMEM = 6201;

		public const short FE_DECNET_NOPRIV = 6202;

		public const short FE_DECNET_NOSUCHDEV = 6203;

		public const short FE_DECNET_CONNECFAIL = 6204;

		public const short FE_DECNET_DEVOFFLINE = 6205;

		public const short FE_DECNET_FILALRACC = 6206;

		public const short FE_DECNET_INVLOGIN = 6207;

		public const short FE_DECNET_IVDEVNAM = 6208;

		public const short FE_DECNET_LINKEXIT = 6209;

		public const short FE_DECNET_NOLINKS = 6210;

		public const short FE_DECNET_NOSUCHNODE = 6211;

		public const short FE_DECNET_NOSUCHOBJ = 6212;

		public const short FE_DECNET_NOSUCHUSER = 6213;

		public const short FE_DECNET_PROTOCOL = 6214;

		public const short FE_DECNET_REJECT = 6215;

		public const short FE_DECNET_REMRSRC = 6216;

		public const short FE_DECNET_SHUT = 6217;

		public const short FE_DECNET_THIRDPARTY = 6218;

		public const short FE_DECNET_TOOMUCHDATA = 6219;

		public const short FE_DECNET_UNREACHABLE = 6220;

		public const short FE_DECNET_DEVALLOC = 6221;

		public const short FE_DECNET_EXQUOTA = 6222;

		public const short FE_DECNET_LINKABORT = 6223;

		public const short FE_DECNET_LINKDISCON = 6224;

		public const short FE_DECNET_PATHLOST = 6225;

		public const short FE_DECNET_TIMEOUT = 6226;

		public const short FE_DECNET_FILNOTACC = 6227;

		public const short FE_DECNET_NOSOLICIT = 6228;

		public const short FE_DECNET_BADPARAM = 6229;

		public const short FE_DECNET_ILLCNTRFUNC = 6230;

		public const short FE_DECNET_NOMBX = 6231;

		public const short FM_XDBM_NAME = 6300;

		public const short FM_XDBM_QUIT_XDBM = 6301;

		public const short FM_XDBM_PDB_VERS = 6302;

		public const short FM_XDBM_BLK_VERS = 6303;

		public const short FM_XDBM_BLK_LESS = 6304;

		public const short FE_XDB_CREATEVA = 6305;

		public const short FE_XDB_DELETEVA = 6306;

		public const short FE_XDB_GETVM = 6307;

		public const short FE_XDB_FREEVM = 6308;

		public const short FE_XDB_MAPSEC = 6309;

		public const short FE_XDB_DELSEC = 6310;

		public const short FE_XDB_CRESEC = 6311;

		public const short FE_XDB_NOLIC = 6312;

		public const short FE_MALLOC_VMS = 6313;

		public const short FE_TIME_VMS = 6314;

		public const short FE_BLKMEM_EXPANSION = 6315;

		public const short FE_RDB_DCPATH = 6400;

		public const short FE_RDB_DCOPATH = 6401;

		public const short FE_RDB_DMACS = 6402;

		public const short FE_RDB_DATABASE = 6403;

		public const short FE_RDB_ARGS = 6404;

		public const short FE_RDB_QUALIFIER = 6405;

		public const short FE_RDB_DELIMETER1 = 6406;

		public const short FE_RDB_DELIMETER2 = 6407;

		public const short FE_RDB_IPC_SETUP = 6408;

		public const short FE_RDB_MALLOC_FAIL = 6409;

		public const short FE_RDB_RENAME = 6410;

		public const short FE_RDB_SCHEMA = 6411;

		public const short FE_RDB_TABLE = 6412;

		public const short FE_RDB_INSERT = 6413;

		public const short FE_RDB_COMMIT = 6414;

		public const short FE_RDB_IMMEDIATE = 6415;

		public const short FE_RDB_PREPARE = 6416;

		public const short FE_RDB_DESCRIBE = 6417;

		public const short FE_RDB_RELEASE = 6418;

		public const short FE_RDB_DATE = 6419;

		public const short FM_RDB_INSERT = 6420;

		public const short FM_RDB_COMMIT = 6421;

		public const short FM_RDB_BEGIN = 6423;

		public const short FM_RDB_SCHEMA = 6424;

		public const short FM_RDB_CREATE_TABLE = 6425;

		public const short FE_Q2_EXCEEDING_MAX_QUES = 6500;

		public const short FE_Q2_NO_HANDLE_AVAILABLE = 6501;

		public const short FE_Q2_NO_MEMORY_FOR_QUE = 6502;

		public const short FE_Q2_ITEM_SIZE_TOO_LARGE = 6503;

		public const short FE_Q2_BAD_ITEM_SIZE = 6504;

		public const short FE_Q2_BAD_ITEM_COUNT = 6505;

		public const short FE_Q2_MISMATCH_HANDLE_NAME = 6506;

		public const short FE_Q2_NO_MATCHING_HANDLE = 6507;

		public const short FE_Q2_INVALID_HANDLE = 6508;

		public const short FE_Q2_QUE_EXISTS = 6509;

		public const short FE_Q2_QUE_EMPTY = 6510;

		public const short FE_Q2_EMS_INITIALIZATION = 6511;

		public const short FE_Q2_NO_EMS = 6512;

		public const short FE_Q2_NO_EMS_MEMORY = 6513;

		public const short FE_Q2_NO_EMS_32K_WINDOW = 6514;

		public const short FE_Q2_NO_FCA_TABLE = 6515;

		public const short FE_Q2_SIZE_EMS_CONTEXT = 6516;

		public const short FE_Q2_MEMORY_TO_SAVE_CONTEXT = 6517;

		public const short FE_Q2_SAVE_CONTEXT = 6518;

		public const short FE_Q2_MAP_EMS_WINDOW = 6519;

		public const short FE_Q2_RESTORE_CONTEXT = 6520;

		public const short FE_Q2_FULL = 6521;

		public const short FE_Q2_LOCKED = 6522;

		public const short FE_LOCK_FAIL = 6600;

		public const short FE_UNLOCK_FAIL = 6601;

		public const short FE_SEC_NO_USER = 6700;

		public const short FE_SEC_BAD_PSWD = 6701;

		public const short FE_SEC_ACCESS = 6702;

		public const short FE_SEC_BADBASE = 6703;

		public const short FE_SEC_BAD_CAT = 6704;

		public const short FE_SEC_NO_REMOTE_USER = 6705;

		public const short FE_SEC_BAD_AREA = 6706;

		public const short FE_SEC_SHUT_MSG = 6707;

		public const short FE_SEC_LOGON_FAILURE = 6711;

		public const short FE_SEC_ACCOUNT_RESTRICTION = 6712;

		public const short FE_SEC_INVALID_LOGON_HOURS = 6713;

		public const short FE_SEC_INVALID_WORKSTATION = 6714;

		public const short FE_SEC_PASSWORD_EXPIRED = 6715;

		public const short FE_SEC_ACCOUNT_DISABLED = 6716;

		public const short FE_SEC_KEY_ERROR = 6717;

		public const short FE_SEC_ACCOUNT_EXPIRED = 6718;

		public const short FE_SEC_ACCOUNT_LOCKED_OUT = 6719;

		public const short FE_SEC_ERROR_PRIVILEGE_NOT_HELD = 6720;

		public const short FE_XNTF_SYNTAX_BAD = 6800;

		public const short FE_XNTF_SYNTAX_NO_NODE = 6801;

		public const short FE_XNTF_SYNTAX_BAD_NODESIZE = 6802;

		public const short FE_XNTF_SYNTAX_NO_TAG = 6803;

		public const short FE_XNTF_SYNTAX_BAD_TAGSIZE = 6804;

		public const short FE_XNTF_SYNTAX_NO_FIELD = 6805;

		public const short FE_XNTF_SYNTAX_BAD_FIELDSIZE = 6806;

		public const short FE_XNTF_SYNTAX_NT_BAD = 6807;

		public const short FE_XNTF_SYNTAX_NT_NO_NODE = 6808;

		public const short FE_XNTF_SYNTAX_NT_NO_TAG = 6809;

		public const short FE_XNTF_SYNTAX_NT_BAD_TAGSIZE = 6810;

		public const short FE_XNTF_SYNTAX_NF_BAD = 6811;

		public const short FE_XNTF_SYNTAX_NF_NO_NODE = 6812;

		public const short FE_XNTF_SYNTAX_NF_NO_FIELD = 6813;

		public const short FE_XNTF_SYNTAX_N_BAD = 6814;

		public const short FE_XNTF_SYNTAX_N_NO_NODE = 6815;

		public const short FE_IM_A_TAGGROUP = 6816;

		public const short FE_ASCII_DATA_MISMATCH = 6817;

		public const short FE_FLOAT_DATA_MISMATCH = 6818;

		public const short FE_GRAPH_DATA_MISMATCH = 6819;

		public const short FE_BINARY_DATA_MISMATCH = 6820;

		public const short FE_IM_A_VARIABLE = 6821;

		public const short FE_FIELD_SELECT = 6822;

		public const short FE_ALLOC_NODE_LIST = 6823;

		public const short FE_ALLOC_FIELD_LIST = 6824;

		public const short FE_GETTING_NODE_LIST = 6825;

		public const short FE_RETRIEVING_TAGS_FROM = 6826;

		public const short FE_NO_TAGS_IN_DATABASE = 6827;

		public const short FE_ALLOC_TAG_LIST = 6828;

		public const short FE_GETTING_TAG_LIST = 6829;

		public const short FE_TAGNAME_VERIFY_PROBLEM = 6830;

		public const short FE_SELECTED_NODE_TEXT = 6831;

		public const short FE_GETTING_FIELD_LIST = 6832;

		public const short FE_SELECTED_TAG_TEXT = 6833;

		public const short FE_GETTING_BLOCK_TYPE = 6834;

		public const short FE_TAG_SELECT = 6835;

		public const short FE_NEED_TO_SELECT_NODE = 6836;

		public const short FE_NO_BOS_TAGS_IN_DATABASE = 6837;

		public const short FE_XNTF_NODE_SELECT = 6840;

		public const short FE_IM_A_DDETAG = 6842;

		public const short FE_ICOUNTRY = 6845;

		public const short FE_XNTF_NO_NODES_MATCH = 6850;

		public const short FE_XNTF_NO_TAGS_MATCH = 6851;

		public const short FE_XNTF_NO_FIELDS_MATCH = 6852;

		public const short FE_XNTF_FILL_LISTBOX_NOTE = 6853;

		public const short FE_XNTF_ZERO_ELEMENTS = 6854;

		public const short FE_XNTF_ADD_TO_LISTBOX = 6855;

		public const short FE_TAG_PREFIX = 6860;

		public const short FE_DDE_APPL_CLIPPED = 6861;

		public const short FE_DDE_TOPIC_CLIPPED = 6862;

		public const short FE_DDE_ITEM_CLIPPED = 6863;

		public const short FE_VARIABLE_NAME = 6864;

		public const short FE_REFRESHING_TAG_LIST = 6870;

		public const short FE_REFRESH_READ = 6871;

		public const short FE_REFRESH_BAD_BYTE_CNT = 6872;

		public const short FE_REFRESH_WRITE_HEADER = 6873;

		public const short FE_REFRESH_WRITE_TMP = 6874;

		public const short FE_REFRESH_REMOVE_TMP = 6875;

		public const short FE_REFRESH_RENAME_TMP = 6876;

		public const short FE_XNTF_30CHAR_OVERFLOW = 6880;

		public const short FE_XNTF_VALIDATE_NODE = 6941;

		public const short FE_CS2_INIT = 7000;

		public const short FE_ADD_CCB = 7001;

		public const short FE_DAE_RDB_QUE_WAIT = 7010;

		public const short FE_DAE_RDB_QUE = 7011;

		public const short FE_DAE_RDB_REQUEST = 7012;

		public const short FE_DAE_RDB_RESPONSE = 7013;

		public const short FE_DAE_RDBEXT_MEM = 7014;

		public const short FE_DAE_INV_ADAPTER = 7015;

		public const short FE_DDM_NOT_CONNECT = 7020;

		public const short FE_DDM_VAR_NOTDEF = 7021;

		public const short FE_DDM_VAR_NOTPRIME = 7022;

		public const short FE_DDM_BAD_INDEX = 7023;

		public const short FE_DDM_BAD_OFFSET = 7024;

		public const short FE_DDM_NOT_PHYSADDR = 7025;

		public const short FE_UDCS_QUE_FULL = 7030;

		public const short FE_UDCS_BAD_FUNC = 7031;

		public const short FE_UDCS_BUF_SIZE = 7032;

		public const short FE_READING_INTL = 7039;

		public const short FE_INTERNATIONAL = 7040;

		public const short FE_TIMESEPARATOR = 7041;

		public const short FE_TIMEFORMAT = 7042;

		public const short FE_WIN2DATEFORMAT = 7043;

		public const short FE_WIN2DATESEPARATOR = 7044;

		public const short FE_SHORTDATEFORMAT = 7045;

		public const short FE_TIMELEADINGZERO = 7046;

		public const short FE_AMSTRING = 7047;

		public const short FE_PMSTRING = 7048;

		public const short FE_SEP_THOUS = 7049;

		public const short FE_SEP_DECIMAL = 7050;

		public const short FE_SEP_LIST = 7051;

		public const short FE_MEASURE_SYS = 7052;

		public const short FE_COUNTRY_NAME = 7053;

		public const short FE_LANG_NAME = 7054;

		public const short FE_COUNTRY_CODE = 7055;

		public const short FE_VARIABLE = 8000;

		public const short FE_UNIT = 8001;

		public const short FE_SQ_NOTFND = 8002;

		public const short FE_QFULL = 8003;

		public const short FE_QEMPTY = 8004;

		public const short FE_UTAG = 8005;

		public const short FE_NOT_DIGITAL = 8006;

		public const short FE_INDEX = 8007;

		public const short FE_TOMANY_SEQ = 8008;

		public const short FE_TOMANY_LINE = 8009;

		public const short FE_VAR_DUP = 8010;

		public const short FE_ONSCAN = 8011;

		public const short FE_ONAUTO = 8012;

		public const short FE_SQ_FOUND = 8013;

		public const short FE_NO_SQ_ACT = 8014;

		public const short FE_NOPHASE_STAT = 8015;

		public const short FE_SPAWN_DNLDR = 8016;

		public const short FE_NOSEQ_STAT = 8017;

		public const short FE_INV_CMD = 8018;

		public const short FE_NO_PH_ACT = 8019;

		public const short FE_BATCH_STARTING = 8020;

		public const short FE_UNIT_DUP = 8021;

		public const short FE_S2S_NOLOGIN = 8022;

		public const short FE_NO_SEQ_SIZE = 8023;

		public const short FE_NO_SEQ_DATA = 8024;

		public const short FE_NO_RT_SIZE = 8025;

		public const short FE_NO_RT_DATA = 8026;

		public const short FE_VAR_NORES = 8027;

		public const short FE_UVAR_NORES = 8028;

		public const short FE_S2S_NOT_RUNNING = 8029;

		public const short FE_BATCH_ON_LINE = 8030;

		public const short FE_NOT_DI = 8031;

		public const short FE_NOT_DO = 8032;

		public const short FE_VAR_BAD = 8033;

		public const short FE_UVAR_BAD = 8034;

		public const short FE_NO_PROC_IN_SLOT = 8035;

		public const short FE_SAC_NOT_RUNNING = 8036;

		public const short FE_UNIT_INUSE = 8037;

		public const short FE_LINE_INUSE = 8038;

		public const short FE_DEL_TRANSIENT = 8039;

		public const short FE_MOD_TRANSIENT = 8040;

		public const short FE_EXPORT_TYPE = 8041;

		public const short FE_BATCH_STOPPED = 8042;

		public const short FE_PROC_HOLD = 8043;

		public const short FE_PHASE_HOLD = 8044;

		public const short FE_ABORT_UPDATE = 8045;

		public const short FE_COMPLETE_UPDATE = 8046;

		public const short FE_OPERATE_DATA = 8047;

		public const short FE_UNIT_NOUPD = 8048;

		public const short FE_RERESOLVE = 8049;

		public const short FE_OPER_TRACE = 8050;

		public const short FE_INPROC_STAT = 8051;

		public const short FE_PRODNUM_RPT = 8052;

		public const short FE_BATCH_COMPLETE = 8053;

		public const short FE_NOLINE_STAT = 8054;

		public const short FE_PRESTART_UPDT = 8055;

		public const short FE_PROC_ROW = 8056;

		public const short FE_OPERATION = 8057;

		public const short FE_RECORD_STAT = 8058;

		public const short FE_INV_LOT = 8059;

		public const short FE_WEIGH_STAT = 8060;

		public const short FE_INGR_USED = 8061;

		public const short FE_IF_EXPRESS = 8062;

		public const short FE_TERMINATE_PHS = 8063;

		public const short FE_ON_HOLD = 8064;

		public const short FE_RUN_IT = 8065;

		public const short FE_TYPE_MISMAT = 8100;

		public const short FE_BAD_PARM = 8101;

		public const short FE_NUM_PARM = 8102;

		public const short FE_INV_PARM = 8103;

		public const short FE_INV_BATKEY = 8104;

		public const short FE_NUM_BKPARM = 8105;

		public const short FE_TYPE_BKPARM = 8106;

		public const short FE_INV_INGRNO = 8107;

		public const short FE_INV_IVARNO = 8108;

		public const short FE_INV_PTLNO = 8109;

		public const short FE_TASK_NORES = 8110;

		public const short FE_FBLK_TYPE = 8111;

		public const short FE_TAG_TYPE = 8112;

		public const short FE_LOOP_PREST = 8113;

		public const short FE_INV_CLASS = 8114;

		public const short FE_INV_UOM = 8115;

		public const short FE_VAR_SIZE4 = 8116;

		public const short FE_NEED_QUOTES = 8117;

		public const short FE_TASK_INPROC = 8118;

		public const short FE_INV_EXPR = 8119;

		public const short FE_INV_LKW = 8150;

		public const short FE_NO_LBLDATA = 8151;

		public const short FE_INV_EXPDATE = 8152;

		public const short FE_INV_LOTNO = 8153;

		public const short FE_INV_LOC = 8154;

		public const short FE_INV_SETI = 8155;

		public const short FE_LBL_SIZE = 8156;

		public const short FE_TK_UNALL = 8172;

		public const short FE_TK_XFER = 8173;

		public const short FE_TK_BKWD = 8174;

		public const short FE_TK_RES = 8175;

		public const short FE_TK_EVALPARM = 8176;

		public const short FE_END_BLK = 8177;

		public const short FE_SQ_ERROR = 8178;

		public const short FE_TK_UNDAL = 8179;

		public const short FE_INGRDATA_NOTF = 8180;

		public const short FE_SEQ_REPORT = 8181;

		public const short FE_LINE_POPQ = 8182;

		public const short FE_LN_BSTART = 8183;

		public const short FE_UNUSED_UNIT = 8184;

		public const short FE_TK_ERROR = 8185;

		public const short FE_TK_STEP = 8186;

		public const short FE_TK_OPEN = 8187;

		public const short FE_TK_CLOSE = 8188;

		public const short FE_TK_AUTO = 8189;

		public const short FE_TK_MANL = 8190;

		public const short FE_INV_LINE = 8200;

		public const short FE_INV_UNIT = 8201;

		public const short FE_INV_TASK = 8202;

		public const short FE_SEQ_NORES = 8203;

		public const short FE_MAX_SEQXFER = 8204;

		public const short FE_INV_PHASE = 8205;

		public const short FE_PHASE_ACTIVE = 8206;

		public const short FE_PHASE_INACTIVE = 8207;

		public const short FE_OPER_HOLD = 8208;

		public const short FE_SEQ_HOLD = 8209;

		public const short FE_TASK_HOLD = 8210;

		public const short FE_INV_ACTION = 8211;

		public const short FE_INV_SEQ = 8212;

		public const short FE_UNIT_HRA = 8213;

		public const short FE_PH_HOLD_AGAIN = 8214;

		public const short FE_PH_RESUME_AGAIN = 8215;

		public const short FE_SQ_HOLD_AGAIN = 8216;

		public const short FE_SQ_RESUME_AGAIN = 8217;

		public const short FE_PH_TK_HELD = 8218;

		public const short FE_PH_HOLD_RES = 8219;

		public const short FE_SEQ_ACCESS = 8220;

		public const short FE_UNIT_CMD = 8221;

		public const short FE_NO_BATCH = 8222;

		public const short FE_CANT_CHAIN_LNBLK = 8223;

		public const short FE_CANT_CHAIN_UNBLK = 8224;

		public const short FE_CANT_CHAIN_PHBLK = 8225;

		public const short FE_DUP_PROC_TAG = 8226;

		public const short FE_PROC_VERSION = 8227;

		public const short FE_RCP_FILETYPE = 8300;

		public const short FE_RCP_NOMATCH = 8301;

		public const short FE_RCP_ITEMUNDEF = 8302;

		public const short FE_RCP_TAGCORRUPT = 8303;

		public const short FE_RCP_BADSTRING = 8304;

		public const short FE_RCP_DINTERLOCK = 8305;

		public const short FE_RCP_UINTERLOCK = 8306;

		public const short FE_RCP_COMPSTATUS = 8307;

		public const short FE_ILLEGAL_RESPONSE = 8400;

		public const short FE_MSG_TMO = 8401;

		public const short FE_HARDWARE_ERROR = 8402;

		public const short FE_ML_COMMAND_ERROR = 8403;

		public const short FE_MODEM_FEATURE_UNSUPPORTED = 8404;

		public const short FE_MODEM_INITIALIZED = 8405;

		public const short FE_PORT_INITIALIZED = 8406;

		public const short FE_MODEM_NOT_INITIALIZED = 8407;

		public const short FE_PORT_NOT_INITIALIZED = 8408;

		public const short FE_NO_PHONE_NUMBER = 8409;

		public const short FE_MODEM_OK = 8420;

		public const short FE_CONNECT = 8421;

		public const short FE_RING = 8422;

		public const short FE_NO_CARRIER = 8423;

		public const short FE_MODEM_ERROR = 8424;

		public const short FE_CONNECT_1200 = 8425;

		public const short FE_NO_DIALTONE = 8426;

		public const short FE_BUSY = 8427;

		public const short FE_NO_ANSWER = 8428;

		public const short FE_CONNECT_2400 = 8430;

		public const short FE_CONNECT_4800 = 8431;

		public const short FE_CONNECT_9600 = 8432;

		public const short FE_CONNECT_19200 = 8434;

		public const short FE_CONNECT_1200_75 = 8442;

		public const short FE_CONNECT_75_1200 = 8443;

		public const short FE_CONNECT_38400 = 8448;

		public const short FE_LAST_MODEM_RESPONSE = 8460;

		public const short FE_ACCESS = 8470;

		public const short FE_LAB_CONNECT = 8471;

		public const short FE_BAD_API = 8472;

		public const short FE_BAD_GROUP = 8473;

		public const short FE_BAD_EGUTAG = 8474;

		public const short FE_BAD_DESC = 8475;

		public const short FE_BAD_LABDATA = 8476;

		public const short FE_BAD_USER = 8477;

		public const short FE_BAD_PARAM = 8478;

		public const short FE_INVALID_AALRM_BUF = 8480;

		public const short FE_NO_BYTES_RECEIVED = 8481;

		public const short FE_RECEIVED_ACK = 8482;

		public const short FE_INVALID_AALARM_CFG_FILE = 8483;

		public const short FE_PLANT3_PORT_TIMEOUT = 8484;

		public const short FE_PLANT3_PORT_ERROR = 8485;

		public const short FE_DISCONNECT_REMOIU_MSG = 8486;

		public const short FE_DISABLE_REMOIU_MSG = 8487;

		public const short FE_DISABLE_REMOIU_ERROR = 8488;

		public const short FE_REMOIU_NOT_RUNNING = 8489;

		public const short FE_XFER_TYPE = 8490;

		public const short FE_WRONG_NODE = 8491;

		public const short FE_XFER_ERROR = 8492;

		public const short FE_FTU_BUSY = 8493;

		public const short FE_BAD_REQ = 8494;

		public const short FE_TCP_NETW_NOT_READY = 8501;

		public const short FE_TCP_NOT_INIT = 8502;

		public const short FE_TCP_VER_MISMATCH = 8503;

		public const short FE_INVALID_PARAMETER = 8504;

		public const short FE_NETWORK_DOWN = 8505;

		public const short FE_BLOCK_IN_PROG = 8506;

		public const short FE_MAX_OUTSTANDING_CONN = 8507;

		public const short FE_MAX_OUTSTANDING_SEND = 8508;

		public const short FE_MAX_CONNECTIONS = 8509;

		public const short FE_TABLE_EMPTY = 8510;

		public const short FE_ADDNAME_FAILED = 8511;

		public const short FE_CONN_OUTSTANDING = 8512;

		public const short FE_NO_RECV_ALLOC_BUF = 8513;

		public const short FE_MAX_SENDBUF = 8514;

		public const short FE_TCP_NO_RES = 8515;

		public const short FE_TCP_NO_NETW_RCVBUF = 8516;

		public const short FE_TCP_NODE_NOTF = 8517;

		public const short FE_TCP_NODE_NOTF_W95 = 8518;

		public const short FE_LAN_FAILOVER_NOT_ENABLED = 8519;

		public const short FE_TCP_LAN_FAILOVER_OCCURRED = 8520;

		public const short FE_NB_LAN_FAILOVER_OCCURRED = 8521;

		public const short FE_LAN_FAILOVER_DISABLED = 8522;

		public const short FE_LAN_FAILOVER_ENABLED = 8523;

		public const short FE_TCP_PROTOCOL_NAME = 8524;

		public const short FE_TABLE_SIZE = 8525;

		public const short FE_ABORT_STARTUP = 8600;

		public const short FE_LOCAL_NODE_ALIAS = 8650;

		public const short FE_BAD_PARAMETER = 8700;

		public const short FE_VAR = 8750;

		public const short FE_KWIN_NOTOPEN = 8800;

		public const short FE_KWIN_NOARGUMENT = 8801;

		public const short FE_ESIG_INIT = 8850;

		public const short FE_ESIG_FIX32 = 8851;

		public const short FE_ESIG_CANCEL = 8852;

		public const short FE_ESIG_CV_CHANGE = 8853;

		public const short FE_ESIG_ACTION = 8854;

		public const short FE_ESIG_FIELD_TYPE = 8855;

		public const short FE_ESIG_READ = 8856;

		public const short FE_ESIG_NOT_ENAB = 8857;

		public const short FTK_OK = 11000;

		public const short FTK_MEMORY = 11001;

		public const short FTK_BAD_HDAGROUP = 11002;

		public const short FTK_BAD_HDANTF = 11003;

		public const short FTK_BAD_DATE = 11004;

		public const short FTK_BAD_TIME = 11005;

		public const short FTK_RANGE = 11006;

		public const short FTK_OPTION = 11007;

		public const short FTK_BADNTF = 11008;

		public const short FTK_BAD_LENGTH = 11009;

		public const short FTK_GROUP_FULL = 11010;

		public const short FTK_BAD_MHANDLE = 11011;

		public const short FTK_MORE_SAMPLES = 11012;

		public const short FTK_NO_NTFS = 11013;

		public const short FTK_NODENAME_NOT_DEFINED = 11014;

		public const short FTK_NOT_REGISTERED = 11015;

		public const short FTK_BAD_ORDER = 11016;

		public const short FTK_NO_MESSAGE = 11017;

		public const short FTK_FIX_NOT_RUNNING = 11018;

		public const short FTK_BAD_NAME = 11019;

		public const short FTK_BAD_PATH = 11020;

		public const short FTK_BAD_NODENAME = 11022;

		public const short FTK_NO_DATA = 11023;

		public const short FTK_ALREADY_REGISTERED = 11024;

		public const short FTK_CANNOT_REGISTER = 11025;

		public const short FTK_NO_USER = 11026;

		public const short FTK_NO_SECURITY = 11027;

		public const short FTK_INTERNAL_ERROR = 11028;

		public const short FTK_NO_FIX32 = 11029;

		public const short FTK_FIX32_PRIOR6 = 11030;

		public const short FTK_SEC_ACCESS = 11031;

		public const short FTK_SEC_LOGGED_IN = 11032;

		public const short FTK_SEC_BAD_AREA = 11033;

		public const short FTK_NAC_NORUN = 11034;

		public const short FTK_NAM_NORUN = 11035;

		public const short FTK_NAM_NOPAUSE = 11036;

		public const short FTK_IHSTATUS_API_TIMEOUT = 11037;

		public const short FTK_IHIST_ERRORCODES_SECTION = 11038;

		public const short FTK_IHIST_ERRORCODES_COUNT = 11039;

		public const short FTK_IHIST_ERRORCODES_VALUE = 11040;

		public const short FTK_CANT_FIND_IHIST = 11041;
	}
}
