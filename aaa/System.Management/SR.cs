using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Management
{
	// Token: 0x02000004 RID: 4
	internal sealed class SR
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002114 File Offset: 0x00001114
		private static object InternalSyncObject
		{
			get
			{
				if (SR.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref SR.s_InternalSyncObject, obj, null);
				}
				return SR.s_InternalSyncObject;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002140 File Offset: 0x00001140
		internal SR()
		{
			this.resources = new ResourceManager("System.Management", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00001164
		private static SR GetLoader()
		{
			if (SR.loader == null)
			{
				lock (SR.InternalSyncObject)
				{
					if (SR.loader == null)
					{
						SR.loader = new SR();
					}
				}
			}
			return SR.loader;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021B4 File Offset: 0x000011B4
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021B7 File Offset: 0x000011B7
		public static ResourceManager Resources
		{
			get
			{
				return SR.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000011C4
		public static string GetString(string name, params object[] args)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			string @string = sr.resources.GetString(name, SR.Culture);
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002248 File Offset: 0x00001248
		public static string GetString(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetString(name, SR.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002274 File Offset: 0x00001274
		public static object GetObject(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetObject(name, SR.Culture);
		}

		// Token: 0x04000002 RID: 2
		internal const string ASSEMBLY_NOT_REGISTERED = "ASSEMBLY_NOT_REGISTERED";

		// Token: 0x04000003 RID: 3
		internal const string FAILED_TO_BUILD_GENERATED_ASSEMBLY = "FAILED_TO_BUILD_GENERATED_ASSEMBLY";

		// Token: 0x04000004 RID: 4
		internal const string COMMENT_SHOULDSERIALIZE = "COMMENT_SHOULDSERIALIZE";

		// Token: 0x04000005 RID: 5
		internal const string COMMENT_ISPROPNULL = "COMMENT_ISPROPNULL";

		// Token: 0x04000006 RID: 6
		internal const string COMMENT_RESETPROP = "COMMENT_RESETPROP";

		// Token: 0x04000007 RID: 7
		internal const string COMMENT_DATECONVFUNC = "COMMENT_DATECONVFUNC";

		// Token: 0x04000008 RID: 8
		internal const string COMMENT_TIMESPANCONVFUNC = "COMMENT_TIMESPANCONVFUNC";

		// Token: 0x04000009 RID: 9
		internal const string COMMENT_ATTRIBPROP = "COMMENT_ATTRIBPROP";

		// Token: 0x0400000A RID: 10
		internal const string COMMENT_GETINSTANCES = "COMMENT_GETINSTANCES";

		// Token: 0x0400000B RID: 11
		internal const string COMMENT_CLASSBEGIN = "COMMENT_CLASSBEGIN";

		// Token: 0x0400000C RID: 12
		internal const string COMMENT_PRIVAUTOCOMMIT = "COMMENT_PRIVAUTOCOMMIT";

		// Token: 0x0400000D RID: 13
		internal const string COMMENT_CONSTRUCTORS = "COMMENT_CONSTRUCTORS";

		// Token: 0x0400000E RID: 14
		internal const string COMMENT_ORIGNAMESPACE = "COMMENT_ORIGNAMESPACE";

		// Token: 0x0400000F RID: 15
		internal const string COMMENT_CLASSNAME = "COMMENT_CLASSNAME";

		// Token: 0x04000010 RID: 16
		internal const string COMMENT_SYSOBJECT = "COMMENT_SYSOBJECT";

		// Token: 0x04000011 RID: 17
		internal const string COMMENT_LATEBOUNDOBJ = "COMMENT_LATEBOUNDOBJ";

		// Token: 0x04000012 RID: 18
		internal const string COMMENT_MGMTSCOPE = "COMMENT_MGMTSCOPE";

		// Token: 0x04000013 RID: 19
		internal const string COMMENT_AUTOCOMMITPROP = "COMMENT_AUTOCOMMITPROP";

		// Token: 0x04000014 RID: 20
		internal const string COMMENT_MGMTPATH = "COMMENT_MGMTPATH";

		// Token: 0x04000015 RID: 21
		internal const string COMMENT_PROPTYPECONVERTER = "COMMENT_PROPTYPECONVERTER";

		// Token: 0x04000016 RID: 22
		internal const string COMMENT_SYSPROPCLASS = "COMMENT_SYSPROPCLASS";

		// Token: 0x04000017 RID: 23
		internal const string COMMENT_ENUMIMPL = "COMMENT_ENUMIMPL";

		// Token: 0x04000018 RID: 24
		internal const string COMMENT_LATEBOUNDPROP = "COMMENT_LATEBOUNDPROP";

		// Token: 0x04000019 RID: 25
		internal const string COMMENT_CREATEDCLASS = "COMMENT_CREATEDCLASS";

		// Token: 0x0400001A RID: 26
		internal const string COMMENT_CREATEDWMINAMESPACE = "COMMENT_CREATEDWMINAMESPACE";

		// Token: 0x0400001B RID: 27
		internal const string COMMENT_STATICMANAGEMENTSCOPE = "COMMENT_STATICMANAGEMENTSCOPE";

		// Token: 0x0400001C RID: 28
		internal const string COMMENT_STATICSCOPEPROPERTY = "COMMENT_STATICSCOPEPROPERTY";

		// Token: 0x0400001D RID: 29
		internal const string COMMENT_TODATETIME = "COMMENT_TODATETIME";

		// Token: 0x0400001E RID: 30
		internal const string COMMENT_TODMTFDATETIME = "COMMENT_TODMTFDATETIME";

		// Token: 0x0400001F RID: 31
		internal const string COMMENT_TODMTFTIMEINTERVAL = "COMMENT_TODMTFTIMEINTERVAL";

		// Token: 0x04000020 RID: 32
		internal const string COMMENT_TOTIMESPAN = "COMMENT_TOTIMESPAN";

		// Token: 0x04000021 RID: 33
		internal const string COMMENT_EMBEDDEDOBJ = "COMMENT_EMBEDDEDOBJ";

		// Token: 0x04000022 RID: 34
		internal const string COMMENT_CURRENTOBJ = "COMMENT_CURRENTOBJ";

		// Token: 0x04000023 RID: 35
		internal const string COMMENT_FLAGFOREMBEDDED = "COMMENT_FLAGFOREMBEDDED";

		// Token: 0x04000024 RID: 36
		internal const string EMBEDDED_COMMENT1 = "EMBEDDED_COMMENT1";

		// Token: 0x04000025 RID: 37
		internal const string EMBEDDED_COMMENT2 = "EMBEDDED_COMMENT2";

		// Token: 0x04000026 RID: 38
		internal const string EMBEDDED_COMMENT3 = "EMBEDDED_COMMENT3";

		// Token: 0x04000027 RID: 39
		internal const string EMBEDDED_COMMENT4 = "EMBEDDED_COMMENT4";

		// Token: 0x04000028 RID: 40
		internal const string EMBEDDED_COMMENT5 = "EMBEDDED_COMMENT5";

		// Token: 0x04000029 RID: 41
		internal const string EMBEDDED_COMMENT6 = "EMBEDDED_COMMENT6";

		// Token: 0x0400002A RID: 42
		internal const string EMBEDDED_COMMENT7 = "EMBEDDED_COMMENT7";

		// Token: 0x0400002B RID: 43
		internal const string EMBEDED_VB_CODESAMP4 = "EMBEDED_VB_CODESAMP4";

		// Token: 0x0400002C RID: 44
		internal const string EMBEDED_VB_CODESAMP5 = "EMBEDED_VB_CODESAMP5";

		// Token: 0x0400002D RID: 45
		internal const string EMBEDDED_COMMENT8 = "EMBEDDED_COMMENT8";

		// Token: 0x0400002E RID: 46
		internal const string EMBEDED_CS_CODESAMP4 = "EMBEDED_CS_CODESAMP4";

		// Token: 0x0400002F RID: 47
		internal const string EMBEDED_CS_CODESAMP5 = "EMBEDED_CS_CODESAMP5";

		// Token: 0x04000030 RID: 48
		internal const string CLASSNOT_FOUND_EXCEPT = "CLASSNOT_FOUND_EXCEPT";

		// Token: 0x04000031 RID: 49
		internal const string NULLFILEPATH_EXCEPT = "NULLFILEPATH_EXCEPT";

		// Token: 0x04000032 RID: 50
		internal const string EMPTY_FILEPATH_EXCEPT = "EMPTY_FILEPATH_EXCEPT";

		// Token: 0x04000033 RID: 51
		internal const string NAMESPACE_NOTINIT_EXCEPT = "NAMESPACE_NOTINIT_EXCEPT";

		// Token: 0x04000034 RID: 52
		internal const string CLASSNAME_NOTINIT_EXCEPT = "CLASSNAME_NOTINIT_EXCEPT";

		// Token: 0x04000035 RID: 53
		internal const string UNABLE_TOCREATE_GEN_EXCEPT = "UNABLE_TOCREATE_GEN_EXCEPT";

		// Token: 0x04000036 RID: 54
		internal const string FORCE_UPDATE = "FORCE_UPDATE";

		// Token: 0x04000037 RID: 55
		internal const string FILETOWRITE_MOF = "FILETOWRITE_MOF";

		// Token: 0x04000038 RID: 56
		internal const string WMISCHEMA_INSTALLATIONSTART = "WMISCHEMA_INSTALLATIONSTART";

		// Token: 0x04000039 RID: 57
		internal const string REGESTRING_ASSEMBLY = "REGESTRING_ASSEMBLY";

		// Token: 0x0400003A RID: 58
		internal const string WMISCHEMA_INSTALLATIONEND = "WMISCHEMA_INSTALLATIONEND";

		// Token: 0x0400003B RID: 59
		internal const string MOFFILE_GENERATING = "MOFFILE_GENERATING";

		// Token: 0x0400003C RID: 60
		internal const string UNSUPPORTEDMEMBER_EXCEPT = "UNSUPPORTEDMEMBER_EXCEPT";

		// Token: 0x0400003D RID: 61
		internal const string CLASSINST_EXCEPT = "CLASSINST_EXCEPT";

		// Token: 0x0400003E RID: 62
		internal const string MEMBERCONFLILCT_EXCEPT = "MEMBERCONFLILCT_EXCEPT";

		// Token: 0x0400003F RID: 63
		internal const string NAMESPACE_ENSURE = "NAMESPACE_ENSURE";

		// Token: 0x04000040 RID: 64
		internal const string CLASS_ENSURE = "CLASS_ENSURE";

		// Token: 0x04000041 RID: 65
		internal const string CLASS_ENSURECREATE = "CLASS_ENSURECREATE";

		// Token: 0x04000042 RID: 66
		internal const string CLASS_NOTREPLACED_EXCEPT = "CLASS_NOTREPLACED_EXCEPT";

		// Token: 0x04000043 RID: 67
		internal const string NONCLS_COMPLIANT_EXCEPTION = "NONCLS_COMPLIANT_EXCEPTION";

		// Token: 0x04000044 RID: 68
		internal const string INVALID_QUERY = "INVALID_QUERY";

		// Token: 0x04000045 RID: 69
		internal const string INVALID_QUERY_DUP_TOKEN = "INVALID_QUERY_DUP_TOKEN";

		// Token: 0x04000046 RID: 70
		internal const string INVALID_QUERY_NULL_TOKEN = "INVALID_QUERY_NULL_TOKEN";

		// Token: 0x04000047 RID: 71
		internal const string WORKER_THREAD_WAKEUP_FAILED = "WORKER_THREAD_WAKEUP_FAILED";

		// Token: 0x04000048 RID: 72
		private static SR loader;

		// Token: 0x04000049 RID: 73
		private ResourceManager resources;

		// Token: 0x0400004A RID: 74
		private static object s_InternalSyncObject;
	}
}
