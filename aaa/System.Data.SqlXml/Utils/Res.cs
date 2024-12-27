using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Xml.Utils
{
	// Token: 0x02000004 RID: 4
	internal sealed class Res
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002114 File Offset: 0x00001114
		private static object InternalSyncObject
		{
			get
			{
				if (Res.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Res.s_InternalSyncObject, obj, null);
				}
				return Res.s_InternalSyncObject;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002140 File Offset: 0x00001140
		internal Res()
		{
			this.resources = new ResourceManager("System.Xml.Utils", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00001164
		private static Res GetLoader()
		{
			if (Res.loader == null)
			{
				lock (Res.InternalSyncObject)
				{
					if (Res.loader == null)
					{
						Res.loader = new Res();
					}
				}
			}
			return Res.loader;
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
				return Res.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000011C4
		public static string GetString(string name, params object[] args)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			string @string = res.resources.GetString(name, Res.Culture);
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
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetString(name, Res.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002274 File Offset: 0x00001274
		public static object GetObject(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetObject(name, Res.Culture);
		}

		// Token: 0x04000002 RID: 2
		internal const string Xml_UserException = "Xml_UserException";

		// Token: 0x04000003 RID: 3
		internal const string Xml_ErrorFilePosition = "Xml_ErrorFilePosition";

		// Token: 0x04000004 RID: 4
		internal const string Xml_InvalidOperation = "Xml_InvalidOperation";

		// Token: 0x04000005 RID: 5
		internal const string Xml_EndOfInnerExceptionStack = "Xml_EndOfInnerExceptionStack";

		// Token: 0x04000006 RID: 6
		internal const string XPath_UnclosedString = "XPath_UnclosedString";

		// Token: 0x04000007 RID: 7
		internal const string XPath_ScientificNotation = "XPath_ScientificNotation";

		// Token: 0x04000008 RID: 8
		internal const string XPath_UnexpectedToken = "XPath_UnexpectedToken";

		// Token: 0x04000009 RID: 9
		internal const string XPath_NodeTestExpected = "XPath_NodeTestExpected";

		// Token: 0x0400000A RID: 10
		internal const string XPath_EofExpected = "XPath_EofExpected";

		// Token: 0x0400000B RID: 11
		internal const string XPath_TokenExpected = "XPath_TokenExpected";

		// Token: 0x0400000C RID: 12
		internal const string XPath_UnknownAxis = "XPath_UnknownAxis";

		// Token: 0x0400000D RID: 13
		internal const string XPath_InvalidAxisInPattern = "XPath_InvalidAxisInPattern";

		// Token: 0x0400000E RID: 14
		internal const string XPath_PredicateAfterDot = "XPath_PredicateAfterDot";

		// Token: 0x0400000F RID: 15
		internal const string XPath_PredicateAfterDotDot = "XPath_PredicateAfterDotDot";

		// Token: 0x04000010 RID: 16
		internal const string XPath_NArgsExpected = "XPath_NArgsExpected";

		// Token: 0x04000011 RID: 17
		internal const string XPath_NOrMArgsExpected = "XPath_NOrMArgsExpected";

		// Token: 0x04000012 RID: 18
		internal const string XPath_AtLeastNArgsExpected = "XPath_AtLeastNArgsExpected";

		// Token: 0x04000013 RID: 19
		internal const string XPath_AtMostMArgsExpected = "XPath_AtMostMArgsExpected";

		// Token: 0x04000014 RID: 20
		internal const string XPath_NodeSetArgumentExpected = "XPath_NodeSetArgumentExpected";

		// Token: 0x04000015 RID: 21
		internal const string XPath_NodeSetExpected = "XPath_NodeSetExpected";

		// Token: 0x04000016 RID: 22
		internal const string XPath_RtfInPathExpr = "XPath_RtfInPathExpr";

		// Token: 0x04000017 RID: 23
		internal const string Xslt_WarningAsError = "Xslt_WarningAsError";

		// Token: 0x04000018 RID: 24
		internal const string Xslt_CannotLoadStylesheet = "Xslt_CannotLoadStylesheet";

		// Token: 0x04000019 RID: 25
		internal const string Xslt_WrongStylesheetElement = "Xslt_WrongStylesheetElement";

		// Token: 0x0400001A RID: 26
		internal const string Xslt_WdXslNamespace = "Xslt_WdXslNamespace";

		// Token: 0x0400001B RID: 27
		internal const string Xslt_NotAtTop = "Xslt_NotAtTop";

		// Token: 0x0400001C RID: 28
		internal const string Xslt_UnexpectedElement = "Xslt_UnexpectedElement";

		// Token: 0x0400001D RID: 29
		internal const string Xslt_UnexpectedElementQ = "Xslt_UnexpectedElementQ";

		// Token: 0x0400001E RID: 30
		internal const string Xslt_NullNsAtTopLevel = "Xslt_NullNsAtTopLevel";

		// Token: 0x0400001F RID: 31
		internal const string Xslt_TextNodesNotAllowed = "Xslt_TextNodesNotAllowed";

		// Token: 0x04000020 RID: 32
		internal const string Xslt_NotEmptyContents = "Xslt_NotEmptyContents";

		// Token: 0x04000021 RID: 33
		internal const string Xslt_InvalidAttribute = "Xslt_InvalidAttribute";

		// Token: 0x04000022 RID: 34
		internal const string Xslt_MissingAttribute = "Xslt_MissingAttribute";

		// Token: 0x04000023 RID: 35
		internal const string Xslt_InvalidAttrValue = "Xslt_InvalidAttrValue";

		// Token: 0x04000024 RID: 36
		internal const string Xslt_BistateAttribute = "Xslt_BistateAttribute";

		// Token: 0x04000025 RID: 37
		internal const string Xslt_CharAttribute = "Xslt_CharAttribute";

		// Token: 0x04000026 RID: 38
		internal const string Xslt_CircularInclude = "Xslt_CircularInclude";

		// Token: 0x04000027 RID: 39
		internal const string Xslt_SingleRightBraceInAvt = "Xslt_SingleRightBraceInAvt";

		// Token: 0x04000028 RID: 40
		internal const string Xslt_VariableCntSel2 = "Xslt_VariableCntSel2";

		// Token: 0x04000029 RID: 41
		internal const string Xslt_DupTemplateName = "Xslt_DupTemplateName";

		// Token: 0x0400002A RID: 42
		internal const string Xslt_BothMatchNameAbsent = "Xslt_BothMatchNameAbsent";

		// Token: 0x0400002B RID: 43
		internal const string Xslt_InvalidVariable = "Xslt_InvalidVariable";

		// Token: 0x0400002C RID: 44
		internal const string Xslt_DupGlobalVariable = "Xslt_DupGlobalVariable";

		// Token: 0x0400002D RID: 45
		internal const string Xslt_DupLocalVariable = "Xslt_DupLocalVariable";

		// Token: 0x0400002E RID: 46
		internal const string Xslt_DupNsAlias = "Xslt_DupNsAlias";

		// Token: 0x0400002F RID: 47
		internal const string Xslt_EmptyAttrValue = "Xslt_EmptyAttrValue";

		// Token: 0x04000030 RID: 48
		internal const string Xslt_EmptyNsAlias = "Xslt_EmptyNsAlias";

		// Token: 0x04000031 RID: 49
		internal const string Xslt_UnknownXsltFunction = "Xslt_UnknownXsltFunction";

		// Token: 0x04000032 RID: 50
		internal const string Xslt_UnsupportedXsltFunction = "Xslt_UnsupportedXsltFunction";

		// Token: 0x04000033 RID: 51
		internal const string Xslt_NoAttributeSet = "Xslt_NoAttributeSet";

		// Token: 0x04000034 RID: 52
		internal const string Xslt_UndefinedKey = "Xslt_UndefinedKey";

		// Token: 0x04000035 RID: 53
		internal const string Xslt_CircularAttributeSet = "Xslt_CircularAttributeSet";

		// Token: 0x04000036 RID: 54
		internal const string Xslt_InvalidCallTemplate = "Xslt_InvalidCallTemplate";

		// Token: 0x04000037 RID: 55
		internal const string Xslt_InvalidPrefix = "Xslt_InvalidPrefix";

		// Token: 0x04000038 RID: 56
		internal const string Xslt_ScriptXsltNamespace = "Xslt_ScriptXsltNamespace";

		// Token: 0x04000039 RID: 57
		internal const string Xslt_ScriptInvalidLanguage = "Xslt_ScriptInvalidLanguage";

		// Token: 0x0400003A RID: 58
		internal const string Xslt_ScriptMixedLanguages = "Xslt_ScriptMixedLanguages";

		// Token: 0x0400003B RID: 59
		internal const string Xslt_ScriptCompileException = "Xslt_ScriptCompileException";

		// Token: 0x0400003C RID: 60
		internal const string Xslt_ScriptNotAtTop = "Xslt_ScriptNotAtTop";

		// Token: 0x0400003D RID: 61
		internal const string Xslt_AssemblyBothNameHrefAbsent = "Xslt_AssemblyBothNameHrefAbsent";

		// Token: 0x0400003E RID: 62
		internal const string Xslt_AssemblyBothNameHrefPresent = "Xslt_AssemblyBothNameHrefPresent";

		// Token: 0x0400003F RID: 63
		internal const string Xslt_ScriptAndExtensionClash = "Xslt_ScriptAndExtensionClash";

		// Token: 0x04000040 RID: 64
		internal const string Xslt_NoDecimalFormat = "Xslt_NoDecimalFormat";

		// Token: 0x04000041 RID: 65
		internal const string Xslt_DecimalFormatSignsNotDistinct = "Xslt_DecimalFormatSignsNotDistinct";

		// Token: 0x04000042 RID: 66
		internal const string Xslt_DecimalFormatRedefined = "Xslt_DecimalFormatRedefined";

		// Token: 0x04000043 RID: 67
		internal const string Xslt_UnknownExtensionElement = "Xslt_UnknownExtensionElement";

		// Token: 0x04000044 RID: 68
		internal const string Xslt_ModeWithoutMatch = "Xslt_ModeWithoutMatch";

		// Token: 0x04000045 RID: 69
		internal const string Xslt_PriorityWithoutMatch = "Xslt_PriorityWithoutMatch";

		// Token: 0x04000046 RID: 70
		internal const string Xslt_InvalidApplyImports = "Xslt_InvalidApplyImports";

		// Token: 0x04000047 RID: 71
		internal const string Xslt_DuplicateWithParam = "Xslt_DuplicateWithParam";

		// Token: 0x04000048 RID: 72
		internal const string Xslt_ReservedNS = "Xslt_ReservedNS";

		// Token: 0x04000049 RID: 73
		internal const string Xslt_XmlnsAttr = "Xslt_XmlnsAttr";

		// Token: 0x0400004A RID: 74
		internal const string Xslt_NoWhen = "Xslt_NoWhen";

		// Token: 0x0400004B RID: 75
		internal const string Xslt_WhenAfterOtherwise = "Xslt_WhenAfterOtherwise";

		// Token: 0x0400004C RID: 76
		internal const string Xslt_DupOtherwise = "Xslt_DupOtherwise";

		// Token: 0x0400004D RID: 77
		internal const string Xslt_AttributeRedefinition = "Xslt_AttributeRedefinition";

		// Token: 0x0400004E RID: 78
		internal const string Xslt_InvalidMethod = "Xslt_InvalidMethod";

		// Token: 0x0400004F RID: 79
		internal const string Xslt_InvalidEncoding = "Xslt_InvalidEncoding";

		// Token: 0x04000050 RID: 80
		internal const string Xslt_InvalidLanguage = "Xslt_InvalidLanguage";

		// Token: 0x04000051 RID: 81
		internal const string Xslt_InvalidLanguageTag = "Xslt_InvalidLanguageTag";

		// Token: 0x04000052 RID: 82
		internal const string Xslt_InvalidCompareOption = "Xslt_InvalidCompareOption";

		// Token: 0x04000053 RID: 83
		internal const string Xslt_KeyNotAllowed = "Xslt_KeyNotAllowed";

		// Token: 0x04000054 RID: 84
		internal const string Xslt_VariablesNotAllowed = "Xslt_VariablesNotAllowed";

		// Token: 0x04000055 RID: 85
		internal const string Xslt_CurrentNotAllowed = "Xslt_CurrentNotAllowed";

		// Token: 0x04000056 RID: 86
		internal const string Xslt_DocumentFuncProhibited = "Xslt_DocumentFuncProhibited";

		// Token: 0x04000057 RID: 87
		internal const string Xslt_ScriptsProhibited = "Xslt_ScriptsProhibited";

		// Token: 0x04000058 RID: 88
		internal const string Xslt_ItemNull = "Xslt_ItemNull";

		// Token: 0x04000059 RID: 89
		internal const string Xslt_NodeSetNotNode = "Xslt_NodeSetNotNode";

		// Token: 0x0400005A RID: 90
		internal const string Xslt_UnsupportedClrType = "Xslt_UnsupportedClrType";

		// Token: 0x0400005B RID: 91
		internal const string Coll_BadOptFormat = "Coll_BadOptFormat";

		// Token: 0x0400005C RID: 92
		internal const string Coll_Unsupported = "Coll_Unsupported";

		// Token: 0x0400005D RID: 93
		internal const string Coll_UnsupportedLanguage = "Coll_UnsupportedLanguage";

		// Token: 0x0400005E RID: 94
		internal const string Coll_UnsupportedOpt = "Coll_UnsupportedOpt";

		// Token: 0x0400005F RID: 95
		internal const string Coll_UnsupportedOptVal = "Coll_UnsupportedOptVal";

		// Token: 0x04000060 RID: 96
		internal const string Coll_UnsupportedSortOpt = "Coll_UnsupportedSortOpt";

		// Token: 0x04000061 RID: 97
		internal const string Qil_Validation = "Qil_Validation";

		// Token: 0x04000062 RID: 98
		internal const string XmlIl_TooManyParameters = "XmlIl_TooManyParameters";

		// Token: 0x04000063 RID: 99
		internal const string XmlIl_BadXmlState = "XmlIl_BadXmlState";

		// Token: 0x04000064 RID: 100
		internal const string XmlIl_BadXmlStateAttr = "XmlIl_BadXmlStateAttr";

		// Token: 0x04000065 RID: 101
		internal const string XmlIl_NmspAfterAttr = "XmlIl_NmspAfterAttr";

		// Token: 0x04000066 RID: 102
		internal const string XmlIl_NmspConflict = "XmlIl_NmspConflict";

		// Token: 0x04000067 RID: 103
		internal const string XmlIl_CantResolveEntity = "XmlIl_CantResolveEntity";

		// Token: 0x04000068 RID: 104
		internal const string XmlIl_NoDefaultDocument = "XmlIl_NoDefaultDocument";

		// Token: 0x04000069 RID: 105
		internal const string XmlIl_UnknownDocument = "XmlIl_UnknownDocument";

		// Token: 0x0400006A RID: 106
		internal const string XmlIl_UnknownParam = "XmlIl_UnknownParam";

		// Token: 0x0400006B RID: 107
		internal const string XmlIl_UnknownExtObj = "XmlIl_UnknownExtObj";

		// Token: 0x0400006C RID: 108
		internal const string XmlIl_CantStripNav = "XmlIl_CantStripNav";

		// Token: 0x0400006D RID: 109
		internal const string XmlIl_ExtensionError = "XmlIl_ExtensionError";

		// Token: 0x0400006E RID: 110
		internal const string XmlIl_TopLevelAttrNmsp = "XmlIl_TopLevelAttrNmsp";

		// Token: 0x0400006F RID: 111
		internal const string XmlIl_NoExtensionMethod = "XmlIl_NoExtensionMethod";

		// Token: 0x04000070 RID: 112
		internal const string XmlIl_AmbiguousExtensionMethod = "XmlIl_AmbiguousExtensionMethod";

		// Token: 0x04000071 RID: 113
		internal const string XmlIl_NonPublicExtensionMethod = "XmlIl_NonPublicExtensionMethod";

		// Token: 0x04000072 RID: 114
		internal const string XmlIl_GenericExtensionMethod = "XmlIl_GenericExtensionMethod";

		// Token: 0x04000073 RID: 115
		internal const string XmlIl_ByRefType = "XmlIl_ByRefType";

		// Token: 0x04000074 RID: 116
		internal const string XmlIl_DocumentLoadError = "XmlIl_DocumentLoadError";

		// Token: 0x04000075 RID: 117
		internal const string Xslt_CompileError = "Xslt_CompileError";

		// Token: 0x04000076 RID: 118
		internal const string Xslt_CompileError2 = "Xslt_CompileError2";

		// Token: 0x04000077 RID: 119
		internal const string Xslt_UnsuppFunction = "Xslt_UnsuppFunction";

		// Token: 0x04000078 RID: 120
		internal const string Xslt_NotFirstImport = "Xslt_NotFirstImport";

		// Token: 0x04000079 RID: 121
		internal const string Xslt_UnexpectedKeyword = "Xslt_UnexpectedKeyword";

		// Token: 0x0400007A RID: 122
		internal const string Xslt_InvalidContents = "Xslt_InvalidContents";

		// Token: 0x0400007B RID: 123
		internal const string Xslt_CantResolve = "Xslt_CantResolve";

		// Token: 0x0400007C RID: 124
		internal const string Xslt_SingleRightAvt = "Xslt_SingleRightAvt";

		// Token: 0x0400007D RID: 125
		internal const string Xslt_OpenBracesAvt = "Xslt_OpenBracesAvt";

		// Token: 0x0400007E RID: 126
		internal const string Xslt_OpenLiteralAvt = "Xslt_OpenLiteralAvt";

		// Token: 0x0400007F RID: 127
		internal const string Xslt_NestedAvt = "Xslt_NestedAvt";

		// Token: 0x04000080 RID: 128
		internal const string Xslt_EmptyAvtExpr = "Xslt_EmptyAvtExpr";

		// Token: 0x04000081 RID: 129
		internal const string Xslt_InvalidXPath = "Xslt_InvalidXPath";

		// Token: 0x04000082 RID: 130
		internal const string Xslt_InvalidQName = "Xslt_InvalidQName";

		// Token: 0x04000083 RID: 131
		internal const string Xslt_NoStylesheetLoaded = "Xslt_NoStylesheetLoaded";

		// Token: 0x04000084 RID: 132
		internal const string Xslt_TemplateNoAttrib = "Xslt_TemplateNoAttrib";

		// Token: 0x04000085 RID: 133
		internal const string Xslt_DupVarName = "Xslt_DupVarName";

		// Token: 0x04000086 RID: 134
		internal const string Xslt_WrongNumberArgs = "Xslt_WrongNumberArgs";

		// Token: 0x04000087 RID: 135
		internal const string Xslt_NoNodeSetConversion = "Xslt_NoNodeSetConversion";

		// Token: 0x04000088 RID: 136
		internal const string Xslt_NoNavigatorConversion = "Xslt_NoNavigatorConversion";

		// Token: 0x04000089 RID: 137
		internal const string Xslt_FunctionFailed = "Xslt_FunctionFailed";

		// Token: 0x0400008A RID: 138
		internal const string Xslt_InvalidFormat = "Xslt_InvalidFormat";

		// Token: 0x0400008B RID: 139
		internal const string Xslt_InvalidFormat1 = "Xslt_InvalidFormat1";

		// Token: 0x0400008C RID: 140
		internal const string Xslt_InvalidFormat2 = "Xslt_InvalidFormat2";

		// Token: 0x0400008D RID: 141
		internal const string Xslt_InvalidFormat3 = "Xslt_InvalidFormat3";

		// Token: 0x0400008E RID: 142
		internal const string Xslt_InvalidFormat4 = "Xslt_InvalidFormat4";

		// Token: 0x0400008F RID: 143
		internal const string Xslt_InvalidFormat5 = "Xslt_InvalidFormat5";

		// Token: 0x04000090 RID: 144
		internal const string Xslt_InvalidFormat8 = "Xslt_InvalidFormat8";

		// Token: 0x04000091 RID: 145
		internal const string Xslt_ScriptCompileErrors = "Xslt_ScriptCompileErrors";

		// Token: 0x04000092 RID: 146
		internal const string Xslt_ScriptInvalidPrefix = "Xslt_ScriptInvalidPrefix";

		// Token: 0x04000093 RID: 147
		internal const string Xslt_ScriptDub = "Xslt_ScriptDub";

		// Token: 0x04000094 RID: 148
		internal const string Xslt_ScriptEmpty = "Xslt_ScriptEmpty";

		// Token: 0x04000095 RID: 149
		internal const string Xslt_DupDecimalFormat = "Xslt_DupDecimalFormat";

		// Token: 0x04000096 RID: 150
		internal const string Xslt_CircularReference = "Xslt_CircularReference";

		// Token: 0x04000097 RID: 151
		internal const string Xslt_InvalidExtensionNamespace = "Xslt_InvalidExtensionNamespace";

		// Token: 0x04000098 RID: 152
		internal const string Xslt_InvalidModeAttribute = "Xslt_InvalidModeAttribute";

		// Token: 0x04000099 RID: 153
		internal const string Xslt_MultipleRoots = "Xslt_MultipleRoots";

		// Token: 0x0400009A RID: 154
		internal const string Xslt_ApplyImports = "Xslt_ApplyImports";

		// Token: 0x0400009B RID: 155
		internal const string Xslt_Terminate = "Xslt_Terminate";

		// Token: 0x0400009C RID: 156
		internal const string Xslt_InvalidPattern = "Xslt_InvalidPattern";

		// Token: 0x0400009D RID: 157
		internal const string Xslt_EmptyTagRequired = "Xslt_EmptyTagRequired";

		// Token: 0x0400009E RID: 158
		internal const string Xslt_WrongNamespace = "Xslt_WrongNamespace";

		// Token: 0x0400009F RID: 159
		internal const string Xslt_InvalidFormat6 = "Xslt_InvalidFormat6";

		// Token: 0x040000A0 RID: 160
		internal const string Xslt_InvalidFormat7 = "Xslt_InvalidFormat7";

		// Token: 0x040000A1 RID: 161
		internal const string Xslt_ScriptMixLang = "Xslt_ScriptMixLang";

		// Token: 0x040000A2 RID: 162
		internal const string Xslt_ScriptInvalidLang = "Xslt_ScriptInvalidLang";

		// Token: 0x040000A3 RID: 163
		internal const string Xslt_InvalidExtensionPermitions = "Xslt_InvalidExtensionPermitions";

		// Token: 0x040000A4 RID: 164
		internal const string Xslt_InvalidParamNamespace = "Xslt_InvalidParamNamespace";

		// Token: 0x040000A5 RID: 165
		internal const string Xslt_DuplicateParametr = "Xslt_DuplicateParametr";

		// Token: 0x040000A6 RID: 166
		internal const string Xslt_VariableCntSel = "Xslt_VariableCntSel";

		// Token: 0x040000A7 RID: 167
		private static Res loader;

		// Token: 0x040000A8 RID: 168
		private ResourceManager resources;

		// Token: 0x040000A9 RID: 169
		private static object s_InternalSyncObject;
	}
}
