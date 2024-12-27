using System;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x0200000D RID: 13
	public sealed class Globals
	{
		// Token: 0x06000042 RID: 66 RVA: 0x000025B7 File Offset: 0x000015B7
		private Globals()
		{
		}

		// Token: 0x040006E6 RID: 1766
		public const short ALM_TEXT_SIZE = 133;

		// Token: 0x040006E7 RID: 1767
		public const short ALM_UNIQUEID_SIZE = 16;

		// Token: 0x040006E8 RID: 1768
		public const short ALM_EXT_TEXT_SIZE = 80;

		// Token: 0x040006E9 RID: 1769
		public const short ALM_MAX_VALUE_TEXT_SIZE = 80;

		// Token: 0x040006EA RID: 1770
		public const short ALM_QUE_MAX_NAME = 16;

		// Token: 0x040006EB RID: 1771
		public const short ALMID_BUFFER_SIZE = 168;

		// Token: 0x040006EC RID: 1772
		public const short AEGU_TAG_SIZE = 34;

		// Token: 0x040006ED RID: 1773
		public const short NAME_SIZE = 31;

		// Token: 0x040006EE RID: 1774
		public const short TAGSIZ = 31;

		// Token: 0x040006EF RID: 1775
		public const short BIG_TAGSIZ = 33;

		// Token: 0x040006F0 RID: 1776
		public const short NODE_NAME_SIZE = 9;

		// Token: 0x040006F1 RID: 1777
		public const short FIELDSIZ = 20;

		// Token: 0x040006F2 RID: 1778
		public const short NTF_SIZE = 62;

		// Token: 0x040006F3 RID: 1779
		public const short CDATE = 35;

		// Token: 0x040006F4 RID: 1780
		public const short MAX_DATE_LEN = 35;

		// Token: 0x040006F5 RID: 1781
		public const short CTIME = 9;

		// Token: 0x040006F6 RID: 1782
		public const short MAX_TIME_LEN = 9;

		// Token: 0x040006F7 RID: 1783
		public const short MAX_DURATION_LEN = 12;

		// Token: 0x040006F8 RID: 1784
		public const short LOGIN_NAMESIZE = 7;

		// Token: 0x040006F9 RID: 1785
		public const short NAMESIZE = 31;

		// Token: 0x040006FA RID: 1786
		public const short GROUP_NAMESIZE = 31;

		// Token: 0x040006FB RID: 1787
		public const short FILEPATHSIZE = 64;

		// Token: 0x040006FC RID: 1788
		public const short BAD_EINDEX = -1;

		// Token: 0x040006FD RID: 1789
		public const short TYPE_NTF = 1;

		// Token: 0x040006FE RID: 1790
		public const short TYPE_DDE = 2;

		// Token: 0x040006FF RID: 1791
		public const short EDA_SIG_VALUE = 444;

		// Token: 0x04000700 RID: 1792
		public const short EDA_DDE_LOOKUP_OK = 1;

		// Token: 0x04000701 RID: 1793
		public const short EDA_NO_DDE_LOOKUP = 0;

		// Token: 0x04000702 RID: 1794
		public const short EDA_PREFIX_SIZE = 2;

		// Token: 0x04000703 RID: 1795
		public const string EDA_FLOAT_PREFIX = "F_";

		// Token: 0x04000704 RID: 1796
		public const string EDA_ASCII_PREFIX = "A_";

		// Token: 0x04000705 RID: 1797
		public const short BAD_GHANDLE = 0;

		// Token: 0x04000706 RID: 1798
		public const short BAD_GRPHANDLE = 0;

		// Token: 0x04000707 RID: 1799
		public const int BAD_THANDLE = -1;

		// Token: 0x04000708 RID: 1800
		public const short WRITE_ASCII_DATA = 0;

		// Token: 0x04000709 RID: 1801
		public const short WRITE_FLOAT_DATA = 1;

		// Token: 0x0400070A RID: 1802
		public const short WRITE_COMMAND = 2;

		// Token: 0x0400070B RID: 1803
		public const short WRITE_DATA = 3;

		// Token: 0x0400070C RID: 1804
		public const short WRITE_BINARY_DATA = 4;

		// Token: 0x0400070D RID: 1805
		public const short BGN_SD_ORDER = 10;

		// Token: 0x0400070E RID: 1806
		public const short FGN_SD_ORDER = 5;

		// Token: 0x0400070F RID: 1807
		public const short BACKGROUND_TASK = 10;

		// Token: 0x04000710 RID: 1808
		public const short FOREGROUND_TASK = 5;

		// Token: 0x04000711 RID: 1809
		public const short EDA_LOOKUP = 1;

		// Token: 0x04000712 RID: 1810
		public const short EDA_READ = 2;

		// Token: 0x04000713 RID: 1811
		public const short EDA_WRITE = 3;

		// Token: 0x04000714 RID: 1812
		public const short EDA_UWRITE = 4;

		// Token: 0x04000715 RID: 1813
		public const short EDA_DEL_BLOCK = 5;

		// Token: 0x04000716 RID: 1814
		public const short EDA_ADD_BLOCK = 6;

		// Token: 0x04000717 RID: 1815
		public const short EDA_SAVE = 7;

		// Token: 0x04000718 RID: 1816
		public const short EDA_RELOAD = 8;

		// Token: 0x04000719 RID: 1817
		public const short EDA_RELOAD_INIT = 9;

		// Token: 0x0400071A RID: 1818
		public const short EDA_RELOAD_TERM = 10;

		// Token: 0x0400071B RID: 1819
		public const int EDA_READ_ASYNC = 254;

		// Token: 0x0400071C RID: 1820
		public const short KEY_DENY_ACCESS = 1;

		// Token: 0x0400071D RID: 1821
		public const short KEY_DENY_WRITES = 2;

		// Token: 0x0400071E RID: 1822
		public const short KEY_DENY_READS = 4;

		// Token: 0x0400071F RID: 1823
		public const short KEY_DENY_DBB = 8;

		// Token: 0x04000720 RID: 1824
		public const int KEY_ALL_AREAS = 65535;

		// Token: 0x04000721 RID: 1825
		public const long KEY_CONFIGURE_BLOCK = 2147539899L;

		// Token: 0x04000722 RID: 1826
		public const int ALM_TYPER_DEFAULT = 0;

		// Token: 0x04000723 RID: 1827
		public const int ALM_TYPER_FILE = 1;

		// Token: 0x04000724 RID: 1828
		public const int ALM_TYPER_SUMMARY = 2;

		// Token: 0x04000725 RID: 1829
		public const int ALM_TYPER_HISTORY = 4;

		// Token: 0x04000726 RID: 1830
		public const int ALM_TYPER_PRINTER = 8;

		// Token: 0x04000727 RID: 1831
		public const int ALM_TYPER_NETWORK = 16;

		// Token: 0x04000728 RID: 1832
		public const long ALM_TYPER_NALL = 4294967295L;

		// Token: 0x04000729 RID: 1833
		public const long ALM_TYPER_ALL = 4294967279L;

		// Token: 0x0400072A RID: 1834
		public const short TYP_F = 1;

		// Token: 0x0400072B RID: 1835
		public const short TYP_S = 2;

		// Token: 0x0400072C RID: 1836
		public const short TYP_H = 4;

		// Token: 0x0400072D RID: 1837
		public const short TYP_P = 8;

		// Token: 0x0400072E RID: 1838
		public const short TYP_N = 16;

		// Token: 0x0400072F RID: 1839
		public const short TYP_ALL = 15;

		// Token: 0x04000730 RID: 1840
		public const short TYP_NALL = 31;

		// Token: 0x04000731 RID: 1841
		public const short ALM_AREAS_MAX_PER_ALM = 15;

		// Token: 0x04000732 RID: 1842
		public const short ALM_AREAS_MAX_LENGTH = 30;

		// Token: 0x04000733 RID: 1843
		public const short TIME_TICK = 0;

		// Token: 0x04000734 RID: 1844
		public const short TIME_SEC = 1;

		// Token: 0x04000735 RID: 1845
		public const short TIME_MIN = 2;

		// Token: 0x04000736 RID: 1846
		public const short TIME_HOUR = 3;

		// Token: 0x04000737 RID: 1847
		public const short DTGSIZ = 8;

		// Token: 0x04000738 RID: 1848
		public const short ADI_A = 1;

		// Token: 0x04000739 RID: 1849
		public const short ADI_B = 2;

		// Token: 0x0400073A RID: 1850
		public const short ADI_C = 4;

		// Token: 0x0400073B RID: 1851
		public const short ADI_D = 8;

		// Token: 0x0400073C RID: 1852
		public const short ADI_E = 16;

		// Token: 0x0400073D RID: 1853
		public const short ADI_F = 32;

		// Token: 0x0400073E RID: 1854
		public const short ADI_G = 64;

		// Token: 0x0400073F RID: 1855
		public const short ADI_H = 128;

		// Token: 0x04000740 RID: 1856
		public const short ADI_I = 256;

		// Token: 0x04000741 RID: 1857
		public const short ADI_J = 512;

		// Token: 0x04000742 RID: 1858
		public const short ADI_K = 1024;

		// Token: 0x04000743 RID: 1859
		public const short ADI_L = 2048;

		// Token: 0x04000744 RID: 1860
		public const short ADI_M = 4096;

		// Token: 0x04000745 RID: 1861
		public const short ADI_N = 8192;

		// Token: 0x04000746 RID: 1862
		public const short ADI_O = 16384;

		// Token: 0x04000747 RID: 1863
		public const int ADI_P = 32768;

		// Token: 0x04000748 RID: 1864
		public const short ALM_MSG_UNKNOWN = 0;

		// Token: 0x04000749 RID: 1865
		public const short ALM_MSG_ALM = 1;

		// Token: 0x0400074A RID: 1866
		public const short ALM_MSG_HARDWARE = 2;

		// Token: 0x0400074B RID: 1867
		public const short ALM_MSG_NETWORK = 3;

		// Token: 0x0400074C RID: 1868
		public const short ALM_MSG_SYSTEM_ALERT = 4;

		// Token: 0x0400074D RID: 1869
		public const short ALM_MSG_USER = 5;

		// Token: 0x0400074E RID: 1870
		public const short ALM_MSG_FLAG_ACK = 6;

		// Token: 0x0400074F RID: 1871
		public const short ALM_MSG_FLAG_DEL = 7;

		// Token: 0x04000750 RID: 1872
		public const short ALM_MSG_OPERATOR = 8;

		// Token: 0x04000751 RID: 1873
		public const short ALM_MSG_RECIPE = 9;

		// Token: 0x04000752 RID: 1874
		public const short ALM_MSG_EVENT = 10;

		// Token: 0x04000753 RID: 1875
		public const short ALM_MSG_TEXT = 11;

		// Token: 0x04000754 RID: 1876
		public const short ALM_MSG_TEXT_6X = 12;

		// Token: 0x04000755 RID: 1877
		public const short ALM_MSG_AAM = 13;

		// Token: 0x04000756 RID: 1878
		public const short ALM_MSG_SQL = 14;

		// Token: 0x04000757 RID: 1879
		public const short ALM_MSG_SIGNED = 15;

		// Token: 0x04000758 RID: 1880
		public const short EALM_DATE = 1;

		// Token: 0x04000759 RID: 1881
		public const short EALM_TIME = 2;

		// Token: 0x0400075A RID: 1882
		public const short EALM_TENTHS = 4;

		// Token: 0x0400075B RID: 1883
		public const short AS_OK = 0;

		// Token: 0x0400075C RID: 1884
		public const short AS_LOLO = 1;

		// Token: 0x0400075D RID: 1885
		public const short AS_LO = 2;

		// Token: 0x0400075E RID: 1886
		public const short AS_HI = 3;

		// Token: 0x0400075F RID: 1887
		public const short AS_HIHI = 4;

		// Token: 0x04000760 RID: 1888
		public const short AS_RATE = 5;

		// Token: 0x04000761 RID: 1889
		public const short AS_COS = 6;

		// Token: 0x04000762 RID: 1890
		public const short AS_CFN = 7;

		// Token: 0x04000763 RID: 1891
		public const short AS_DEV = 8;

		// Token: 0x04000764 RID: 1892
		public const short AS_FLT = 137;

		// Token: 0x04000765 RID: 1893
		public const short AS_MANL = 10;

		// Token: 0x04000766 RID: 1894
		public const short AS_DSAB = 11;

		// Token: 0x04000767 RID: 1895
		public const short AS_ERROR = 12;

		// Token: 0x04000768 RID: 1896
		public const short AS_ANY = 13;

		// Token: 0x04000769 RID: 1897
		public const short AS_NEW = 14;

		// Token: 0x0400076A RID: 1898
		public const short AS_TIME = 15;

		// Token: 0x0400076B RID: 1899
		public const short AS_SQL_LOG = 16;

		// Token: 0x0400076C RID: 1900
		public const short AS_SQL_CMD = 17;

		// Token: 0x0400076D RID: 1901
		public const short AS_DATA_MATCH = 18;

		// Token: 0x0400076E RID: 1902
		public const short AS_FIELD_READ = 19;

		// Token: 0x0400076F RID: 1903
		public const short AS_FIELD_WRITE = 20;

		// Token: 0x04000770 RID: 1904
		public const short AS_IOF = 192;

		// Token: 0x04000771 RID: 1905
		public const short AS_OCD = 193;

		// Token: 0x04000772 RID: 1906
		public const short AS_URNG = 66;

		// Token: 0x04000773 RID: 1907
		public const short AS_ORNG = 67;

		// Token: 0x04000774 RID: 1908
		public const short AS_RANG = 196;

		// Token: 0x04000775 RID: 1909
		public const short AS_COMM = 197;

		// Token: 0x04000776 RID: 1910
		public const short AS_DEVICE = 198;

		// Token: 0x04000777 RID: 1911
		public const short AS_STATION = 199;

		// Token: 0x04000778 RID: 1912
		public const short AS_ACCESS = 200;

		// Token: 0x04000779 RID: 1913
		public const short AS_NODATA = 201;

		// Token: 0x0400077A RID: 1914
		public const short AS_NOXDATA = 202;

		// Token: 0x0400077B RID: 1915
		public const short HDA_MAX_SAMPLES = 5000;

		// Token: 0x0400077C RID: 1916
		public const short HDA_MAX_TAGS_PER_GROUP = 8;

		// Token: 0x0400077D RID: 1917
		public const int HDA_NO_HANDLE = 0;

		// Token: 0x0400077E RID: 1918
		public const string HDA_DFLT_FIELD = "F_CV";

		// Token: 0x0400077F RID: 1919
		public const short HDA_MODE_AVERAGE = 1;

		// Token: 0x04000780 RID: 1920
		public const short HDA_MODE_HIGH = 2;

		// Token: 0x04000781 RID: 1921
		public const short HDA_MODE_LOW = 3;

		// Token: 0x04000782 RID: 1922
		public const short HDA_MODE_SAMPLE = 4;

		// Token: 0x04000783 RID: 1923
		public const short HDA_MODE_RAW = 5;

		// Token: 0x04000784 RID: 1924
		public const short HDA_MODE_INTERP = 6;

		// Token: 0x04000785 RID: 1925
		public const short HDA_MODE_STDDEV = 7;

		// Token: 0x04000786 RID: 1926
		public const short HDA_MODE_TOTAL = 8;

		// Token: 0x04000787 RID: 1927
		public const short HDA_MODE_COUNT = 9;

		// Token: 0x04000788 RID: 1928
		public const short HDA_MODE_RAWAVERAGE = 10;

		// Token: 0x04000789 RID: 1929
		public const short HDA_MODE_RAWSTDDEV = 11;

		// Token: 0x0400078A RID: 1930
		public const short HDA_MODE_RAWTOTAL = 12;

		// Token: 0x0400078B RID: 1931
		public const int HDA_VAL_OK = 0;

		// Token: 0x0400078C RID: 1932
		public const int HDA_VAL_BAD = 1;

		// Token: 0x0400078D RID: 1933
		public const short IA_BAD = 128;

		// Token: 0x0400078E RID: 1934
		public const int IA_OK = 0;

		// Token: 0x0400078F RID: 1935
		public const int IA_LOLO = 1793;

		// Token: 0x04000790 RID: 1936
		public const int IA_LO = 1538;

		// Token: 0x04000791 RID: 1937
		public const int IA_HI = 1539;

		// Token: 0x04000792 RID: 1938
		public const int IA_HIHI = 1796;

		// Token: 0x04000793 RID: 1939
		public const int IA_RATE = 1541;

		// Token: 0x04000794 RID: 1940
		public const int IA_COS = 1798;

		// Token: 0x04000795 RID: 1941
		public const int IA_CFN = 1799;

		// Token: 0x04000796 RID: 1942
		public const int IA_DEV = 1288;

		// Token: 0x04000797 RID: 1943
		public const int IA_FLT = 2185;

		// Token: 0x04000798 RID: 1944
		public const int IA_MANL = 4106;

		// Token: 0x04000799 RID: 1945
		public const int IA_DSAB = 267;

		// Token: 0x0400079A RID: 1946
		public const int IA_ERROR = 2060;

		// Token: 0x0400079B RID: 1947
		public const int IA_ANY = 2061;

		// Token: 0x0400079C RID: 1948
		public const int IA_NEW = 2062;

		// Token: 0x0400079D RID: 1949
		public const int IA_TIME = 1807;

		// Token: 0x0400079E RID: 1950
		public const int IA_SQL_LOG = 1808;

		// Token: 0x0400079F RID: 1951
		public const int IA_SQL_CMD = 1553;

		// Token: 0x040007A0 RID: 1952
		public const int IA_DATA_MATCH = 1298;

		// Token: 0x040007A1 RID: 1953
		public const int IA_FIELD_READ = 1043;

		// Token: 0x040007A2 RID: 1954
		public const int IA_FIELD_WRITE = 1044;

		// Token: 0x040007A3 RID: 1955
		public const int IA_IOF = 4288;

		// Token: 0x040007A4 RID: 1956
		public const int IA_OCD = 4289;

		// Token: 0x040007A5 RID: 1957
		public const int IA_URNG = 4162;

		// Token: 0x040007A6 RID: 1958
		public const int IA_ORNG = 4163;

		// Token: 0x040007A7 RID: 1959
		public const int IA_RANG = 4292;

		// Token: 0x040007A8 RID: 1960
		public const int IA_COMM = 4293;

		// Token: 0x040007A9 RID: 1961
		public const int IA_DEVICE = 4294;

		// Token: 0x040007AA RID: 1962
		public const int IA_STATION = 4295;

		// Token: 0x040007AB RID: 1963
		public const int IA_ACCESS = 4296;

		// Token: 0x040007AC RID: 1964
		public const int IA_NODATA = 4297;

		// Token: 0x040007AD RID: 1965
		public const int IA_NOXDATA = 4298;

		// Token: 0x040007AE RID: 1966
		public const string BASPATH = "BASPATH";

		// Token: 0x040007AF RID: 1967
		public const string LOCPATH = "LOCPATH";

		// Token: 0x040007B0 RID: 1968
		public const string PDBPATH = "PDBPATH";

		// Token: 0x040007B1 RID: 1969
		public const string NLSPATH = "NLSPATH";

		// Token: 0x040007B2 RID: 1970
		public const string PICPATH = "PICPATH";

		// Token: 0x040007B3 RID: 1971
		public const string FSTPATH = "FASTPATH";

		// Token: 0x040007B4 RID: 1972
		public const string APPPATH = "APPPATH";

		// Token: 0x040007B5 RID: 1973
		public const string HTCPATH = "HTCPATH";

		// Token: 0x040007B6 RID: 1974
		public const string HTRDATA = "HTRDATA";

		// Token: 0x040007B7 RID: 1975
		public const string ALMPATH = "ALMPATH";

		// Token: 0x040007B8 RID: 1976
		public const string RCMPATH = "RCMPATH";

		// Token: 0x040007B9 RID: 1977
		public const string RCCPATH = "RCCPATH";
	}
}
