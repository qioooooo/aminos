using System;

namespace GeFanuc.iFixToolkit.Adapter
{
	public sealed class Globals
	{
		private Globals()
		{
		}

		public const short ALM_TEXT_SIZE = 133;

		public const short ALM_UNIQUEID_SIZE = 16;

		public const short ALM_EXT_TEXT_SIZE = 80;

		public const short ALM_MAX_VALUE_TEXT_SIZE = 80;

		public const short ALM_QUE_MAX_NAME = 16;

		public const short ALMID_BUFFER_SIZE = 168;

		public const short AEGU_TAG_SIZE = 34;

		public const short NAME_SIZE = 31;

		public const short TAGSIZ = 31;

		public const short BIG_TAGSIZ = 33;

		public const short NODE_NAME_SIZE = 9;

		public const short FIELDSIZ = 20;

		public const short NTF_SIZE = 62;

		public const short CDATE = 35;

		public const short MAX_DATE_LEN = 35;

		public const short CTIME = 9;

		public const short MAX_TIME_LEN = 9;

		public const short MAX_DURATION_LEN = 12;

		public const short LOGIN_NAMESIZE = 7;

		public const short NAMESIZE = 31;

		public const short GROUP_NAMESIZE = 31;

		public const short FILEPATHSIZE = 64;

		public const short BAD_EINDEX = -1;

		public const short TYPE_NTF = 1;

		public const short TYPE_DDE = 2;

		public const short EDA_SIG_VALUE = 444;

		public const short EDA_DDE_LOOKUP_OK = 1;

		public const short EDA_NO_DDE_LOOKUP = 0;

		public const short EDA_PREFIX_SIZE = 2;

		public const string EDA_FLOAT_PREFIX = "F_";

		public const string EDA_ASCII_PREFIX = "A_";

		public const short BAD_GHANDLE = 0;

		public const short BAD_GRPHANDLE = 0;

		public const int BAD_THANDLE = -1;

		public const short WRITE_ASCII_DATA = 0;

		public const short WRITE_FLOAT_DATA = 1;

		public const short WRITE_COMMAND = 2;

		public const short WRITE_DATA = 3;

		public const short WRITE_BINARY_DATA = 4;

		public const short BGN_SD_ORDER = 10;

		public const short FGN_SD_ORDER = 5;

		public const short BACKGROUND_TASK = 10;

		public const short FOREGROUND_TASK = 5;

		public const short EDA_LOOKUP = 1;

		public const short EDA_READ = 2;

		public const short EDA_WRITE = 3;

		public const short EDA_UWRITE = 4;

		public const short EDA_DEL_BLOCK = 5;

		public const short EDA_ADD_BLOCK = 6;

		public const short EDA_SAVE = 7;

		public const short EDA_RELOAD = 8;

		public const short EDA_RELOAD_INIT = 9;

		public const short EDA_RELOAD_TERM = 10;

		public const int EDA_READ_ASYNC = 254;

		public const short KEY_DENY_ACCESS = 1;

		public const short KEY_DENY_WRITES = 2;

		public const short KEY_DENY_READS = 4;

		public const short KEY_DENY_DBB = 8;

		public const int KEY_ALL_AREAS = 65535;

		public const long KEY_CONFIGURE_BLOCK = 2147539899L;

		public const int ALM_TYPER_DEFAULT = 0;

		public const int ALM_TYPER_FILE = 1;

		public const int ALM_TYPER_SUMMARY = 2;

		public const int ALM_TYPER_HISTORY = 4;

		public const int ALM_TYPER_PRINTER = 8;

		public const int ALM_TYPER_NETWORK = 16;

		public const long ALM_TYPER_NALL = 4294967295L;

		public const long ALM_TYPER_ALL = 4294967279L;

		public const short TYP_F = 1;

		public const short TYP_S = 2;

		public const short TYP_H = 4;

		public const short TYP_P = 8;

		public const short TYP_N = 16;

		public const short TYP_ALL = 15;

		public const short TYP_NALL = 31;

		public const short ALM_AREAS_MAX_PER_ALM = 15;

		public const short ALM_AREAS_MAX_LENGTH = 30;

		public const short TIME_TICK = 0;

		public const short TIME_SEC = 1;

		public const short TIME_MIN = 2;

		public const short TIME_HOUR = 3;

		public const short DTGSIZ = 8;

		public const short ADI_A = 1;

		public const short ADI_B = 2;

		public const short ADI_C = 4;

		public const short ADI_D = 8;

		public const short ADI_E = 16;

		public const short ADI_F = 32;

		public const short ADI_G = 64;

		public const short ADI_H = 128;

		public const short ADI_I = 256;

		public const short ADI_J = 512;

		public const short ADI_K = 1024;

		public const short ADI_L = 2048;

		public const short ADI_M = 4096;

		public const short ADI_N = 8192;

		public const short ADI_O = 16384;

		public const int ADI_P = 32768;

		public const short ALM_MSG_UNKNOWN = 0;

		public const short ALM_MSG_ALM = 1;

		public const short ALM_MSG_HARDWARE = 2;

		public const short ALM_MSG_NETWORK = 3;

		public const short ALM_MSG_SYSTEM_ALERT = 4;

		public const short ALM_MSG_USER = 5;

		public const short ALM_MSG_FLAG_ACK = 6;

		public const short ALM_MSG_FLAG_DEL = 7;

		public const short ALM_MSG_OPERATOR = 8;

		public const short ALM_MSG_RECIPE = 9;

		public const short ALM_MSG_EVENT = 10;

		public const short ALM_MSG_TEXT = 11;

		public const short ALM_MSG_TEXT_6X = 12;

		public const short ALM_MSG_AAM = 13;

		public const short ALM_MSG_SQL = 14;

		public const short ALM_MSG_SIGNED = 15;

		public const short EALM_DATE = 1;

		public const short EALM_TIME = 2;

		public const short EALM_TENTHS = 4;

		public const short AS_OK = 0;

		public const short AS_LOLO = 1;

		public const short AS_LO = 2;

		public const short AS_HI = 3;

		public const short AS_HIHI = 4;

		public const short AS_RATE = 5;

		public const short AS_COS = 6;

		public const short AS_CFN = 7;

		public const short AS_DEV = 8;

		public const short AS_FLT = 137;

		public const short AS_MANL = 10;

		public const short AS_DSAB = 11;

		public const short AS_ERROR = 12;

		public const short AS_ANY = 13;

		public const short AS_NEW = 14;

		public const short AS_TIME = 15;

		public const short AS_SQL_LOG = 16;

		public const short AS_SQL_CMD = 17;

		public const short AS_DATA_MATCH = 18;

		public const short AS_FIELD_READ = 19;

		public const short AS_FIELD_WRITE = 20;

		public const short AS_IOF = 192;

		public const short AS_OCD = 193;

		public const short AS_URNG = 66;

		public const short AS_ORNG = 67;

		public const short AS_RANG = 196;

		public const short AS_COMM = 197;

		public const short AS_DEVICE = 198;

		public const short AS_STATION = 199;

		public const short AS_ACCESS = 200;

		public const short AS_NODATA = 201;

		public const short AS_NOXDATA = 202;

		public const short HDA_MAX_SAMPLES = 5000;

		public const short HDA_MAX_TAGS_PER_GROUP = 8;

		public const int HDA_NO_HANDLE = 0;

		public const string HDA_DFLT_FIELD = "F_CV";

		public const short HDA_MODE_AVERAGE = 1;

		public const short HDA_MODE_HIGH = 2;

		public const short HDA_MODE_LOW = 3;

		public const short HDA_MODE_SAMPLE = 4;

		public const short HDA_MODE_RAW = 5;

		public const short HDA_MODE_INTERP = 6;

		public const short HDA_MODE_STDDEV = 7;

		public const short HDA_MODE_TOTAL = 8;

		public const short HDA_MODE_COUNT = 9;

		public const short HDA_MODE_RAWAVERAGE = 10;

		public const short HDA_MODE_RAWSTDDEV = 11;

		public const short HDA_MODE_RAWTOTAL = 12;

		public const int HDA_VAL_OK = 0;

		public const int HDA_VAL_BAD = 1;

		public const short IA_BAD = 128;

		public const int IA_OK = 0;

		public const int IA_LOLO = 1793;

		public const int IA_LO = 1538;

		public const int IA_HI = 1539;

		public const int IA_HIHI = 1796;

		public const int IA_RATE = 1541;

		public const int IA_COS = 1798;

		public const int IA_CFN = 1799;

		public const int IA_DEV = 1288;

		public const int IA_FLT = 2185;

		public const int IA_MANL = 4106;

		public const int IA_DSAB = 267;

		public const int IA_ERROR = 2060;

		public const int IA_ANY = 2061;

		public const int IA_NEW = 2062;

		public const int IA_TIME = 1807;

		public const int IA_SQL_LOG = 1808;

		public const int IA_SQL_CMD = 1553;

		public const int IA_DATA_MATCH = 1298;

		public const int IA_FIELD_READ = 1043;

		public const int IA_FIELD_WRITE = 1044;

		public const int IA_IOF = 4288;

		public const int IA_OCD = 4289;

		public const int IA_URNG = 4162;

		public const int IA_ORNG = 4163;

		public const int IA_RANG = 4292;

		public const int IA_COMM = 4293;

		public const int IA_DEVICE = 4294;

		public const int IA_STATION = 4295;

		public const int IA_ACCESS = 4296;

		public const int IA_NODATA = 4297;

		public const int IA_NOXDATA = 4298;

		public const string BASPATH = "BASPATH";

		public const string LOCPATH = "LOCPATH";

		public const string PDBPATH = "PDBPATH";

		public const string NLSPATH = "NLSPATH";

		public const string PICPATH = "PICPATH";

		public const string FSTPATH = "FASTPATH";

		public const string APPPATH = "APPPATH";

		public const string HTCPATH = "HTCPATH";

		public const string HTRDATA = "HTRDATA";

		public const string ALMPATH = "ALMPATH";

		public const string RCMPATH = "RCMPATH";

		public const string RCCPATH = "RCCPATH";
	}
}
