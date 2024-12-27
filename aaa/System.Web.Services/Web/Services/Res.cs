using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Web.Services
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
			this.resources = new ResourceManager("System.Web.Services", base.GetType().Assembly);
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
		internal const string NonClsCompliantException = "NonClsCompliantException";

		// Token: 0x04000003 RID: 3
		internal const string WebConfigInvalidExtensionPriority = "WebConfigInvalidExtensionPriority";

		// Token: 0x04000004 RID: 4
		internal const string ConfigKeyNotFoundInElementCollection = "ConfigKeyNotFoundInElementCollection";

		// Token: 0x04000005 RID: 5
		internal const string ConfigKeysDoNotMatch = "ConfigKeysDoNotMatch";

		// Token: 0x04000006 RID: 6
		internal const string Invalid_priority_group_value = "Invalid_priority_group_value";

		// Token: 0x04000007 RID: 7
		internal const string WebSchemaNotFound = "WebSchemaNotFound";

		// Token: 0x04000008 RID: 8
		internal const string WebReflectionError = "WebReflectionError";

		// Token: 0x04000009 RID: 9
		internal const string WebInvalidMethodName = "WebInvalidMethodName";

		// Token: 0x0400000A RID: 10
		internal const string WebInvalidMethodNameCase = "WebInvalidMethodNameCase";

		// Token: 0x0400000B RID: 11
		internal const string WebInvalidRequestFormat = "WebInvalidRequestFormat";

		// Token: 0x0400000C RID: 12
		internal const string WebInvalidRequestFormatDetails = "WebInvalidRequestFormatDetails";

		// Token: 0x0400000D RID: 13
		internal const string WebMethodStatic = "WebMethodStatic";

		// Token: 0x0400000E RID: 14
		internal const string WebMethodMissingParams = "WebMethodMissingParams";

		// Token: 0x0400000F RID: 15
		internal const string WebBadOutParameter = "WebBadOutParameter";

		// Token: 0x04000010 RID: 16
		internal const string WebInOutParameter = "WebInOutParameter";

		// Token: 0x04000011 RID: 17
		internal const string WebAsyncMissingEnd = "WebAsyncMissingEnd";

		// Token: 0x04000012 RID: 18
		internal const string WebMissingPath = "WebMissingPath";

		// Token: 0x04000013 RID: 19
		internal const string WebResponseKnownError = "WebResponseKnownError";

		// Token: 0x04000014 RID: 20
		internal const string WebResponseUnknownError = "WebResponseUnknownError";

		// Token: 0x04000015 RID: 21
		internal const string WebResponseUnknownErrorEmptyBody = "WebResponseUnknownErrorEmptyBody";

		// Token: 0x04000016 RID: 22
		internal const string WebResponseContent = "WebResponseContent";

		// Token: 0x04000017 RID: 23
		internal const string WebBadStreamState = "WebBadStreamState";

		// Token: 0x04000018 RID: 24
		internal const string WebResponseBadXml = "WebResponseBadXml";

		// Token: 0x04000019 RID: 25
		internal const string WebCannotUnderstandHeader = "WebCannotUnderstandHeader";

		// Token: 0x0400001A RID: 26
		internal const string WebMissingHeader = "WebMissingHeader";

		// Token: 0x0400001B RID: 27
		internal const string WebNoReturnValue = "WebNoReturnValue";

		// Token: 0x0400001C RID: 28
		internal const string WebCannotAccessValue = "WebCannotAccessValue";

		// Token: 0x0400001D RID: 29
		internal const string WebCannotAccessValueStage = "WebCannotAccessValueStage";

		// Token: 0x0400001E RID: 30
		internal const string WebInvalidBindingPlacement = "WebInvalidBindingPlacement";

		// Token: 0x0400001F RID: 31
		internal const string WebInvalidBindingName = "WebInvalidBindingName";

		// Token: 0x04000020 RID: 32
		internal const string WebBothMethodAttrs = "WebBothMethodAttrs";

		// Token: 0x04000021 RID: 33
		internal const string WebBothServiceAttrs = "WebBothServiceAttrs";

		// Token: 0x04000022 RID: 34
		internal const string WebOneWayOutParameters = "WebOneWayOutParameters";

		// Token: 0x04000023 RID: 35
		internal const string WebOneWayReturnValue = "WebOneWayReturnValue";

		// Token: 0x04000024 RID: 36
		internal const string WebReflectionErrorMethod = "WebReflectionErrorMethod";

		// Token: 0x04000025 RID: 37
		internal const string WebMultiDimArray = "WebMultiDimArray";

		// Token: 0x04000026 RID: 38
		internal const string WebHeaderMissing = "WebHeaderMissing";

		// Token: 0x04000027 RID: 39
		internal const string WebHeaderStatic = "WebHeaderStatic";

		// Token: 0x04000028 RID: 40
		internal const string WebHeaderRead = "WebHeaderRead";

		// Token: 0x04000029 RID: 41
		internal const string WebHeaderWrite = "WebHeaderWrite";

		// Token: 0x0400002A RID: 42
		internal const string WebHeaderType = "WebHeaderType";

		// Token: 0x0400002B RID: 43
		internal const string WebHeaderOneWayOut = "WebHeaderOneWayOut";

		// Token: 0x0400002C RID: 44
		internal const string WebHeaderInvalidMustUnderstand = "WebHeaderInvalidMustUnderstand";

		// Token: 0x0400002D RID: 45
		internal const string WebMultiplyDeclaredHeaderTypes = "WebMultiplyDeclaredHeaderTypes";

		// Token: 0x0400002E RID: 46
		internal const string WebHttpHeader = "WebHttpHeader";

		// Token: 0x0400002F RID: 47
		internal const string WebRequestContent = "WebRequestContent";

		// Token: 0x04000030 RID: 48
		internal const string WebRequestUnableToRead = "WebRequestUnableToRead";

		// Token: 0x04000031 RID: 49
		internal const string WebRequestUnableToProcess = "WebRequestUnableToProcess";

		// Token: 0x04000032 RID: 50
		internal const string WebMissingParameter = "WebMissingParameter";

		// Token: 0x04000033 RID: 51
		internal const string WebUnrecognizedRequestFormat = "WebUnrecognizedRequestFormat";

		// Token: 0x04000034 RID: 52
		internal const string WebUnrecognizedRequestFormatUrl = "WebUnrecognizedRequestFormatUrl";

		// Token: 0x04000035 RID: 53
		internal const string WebTimeout = "WebTimeout";

		// Token: 0x04000036 RID: 54
		internal const string WebMissingHelpContext = "WebMissingHelpContext";

		// Token: 0x04000037 RID: 55
		internal const string WebMissingCustomAttribute = "WebMissingCustomAttribute";

		// Token: 0x04000038 RID: 56
		internal const string WebMissingClientProtocol = "WebMissingClientProtocol";

		// Token: 0x04000039 RID: 57
		internal const string WebResolveMissingClientProtocol = "WebResolveMissingClientProtocol";

		// Token: 0x0400003A RID: 58
		internal const string WebPathNotFound = "WebPathNotFound";

		// Token: 0x0400003B RID: 59
		internal const string WebMissingResource = "WebMissingResource";

		// Token: 0x0400003C RID: 60
		internal const string WebContractReferenceName = "WebContractReferenceName";

		// Token: 0x0400003D RID: 61
		internal const string WebShemaReferenceName = "WebShemaReferenceName";

		// Token: 0x0400003E RID: 62
		internal const string WebDiscoveryDocumentReferenceName = "WebDiscoveryDocumentReferenceName";

		// Token: 0x0400003F RID: 63
		internal const string WebMissingDocument = "WebMissingDocument";

		// Token: 0x04000040 RID: 64
		internal const string WebInvalidContentType = "WebInvalidContentType";

		// Token: 0x04000041 RID: 65
		internal const string WebInvalidFormat = "WebInvalidFormat";

		// Token: 0x04000042 RID: 66
		internal const string WebInvalidEnvelopeNamespace = "WebInvalidEnvelopeNamespace";

		// Token: 0x04000043 RID: 67
		internal const string WebResultNotXml = "WebResultNotXml";

		// Token: 0x04000044 RID: 68
		internal const string WebDescriptionMissingItem = "WebDescriptionMissingItem";

		// Token: 0x04000045 RID: 69
		internal const string WebDescriptionMissing = "WebDescriptionMissing";

		// Token: 0x04000046 RID: 70
		internal const string WebDescriptionPartElementRequired = "WebDescriptionPartElementRequired";

		// Token: 0x04000047 RID: 71
		internal const string WebDescriptionPartTypeRequired = "WebDescriptionPartTypeRequired";

		// Token: 0x04000048 RID: 72
		internal const string WebDescriptionPartElementWarning = "WebDescriptionPartElementWarning";

		// Token: 0x04000049 RID: 73
		internal const string WebDescriptionPartTypeWarning = "WebDescriptionPartTypeWarning";

		// Token: 0x0400004A RID: 74
		internal const string WebDescriptionMissingBodyUseAttribute = "WebDescriptionMissingBodyUseAttribute";

		// Token: 0x0400004B RID: 75
		internal const string WebDescriptionTooManyMessages = "WebDescriptionTooManyMessages";

		// Token: 0x0400004C RID: 76
		internal const string WebDescriptionHeaderAndBodyUseMismatch = "WebDescriptionHeaderAndBodyUseMismatch";

		// Token: 0x0400004D RID: 77
		internal const string WebQNamePrefixUndefined = "WebQNamePrefixUndefined";

		// Token: 0x0400004E RID: 78
		internal const string WebNegativeValue = "WebNegativeValue";

		// Token: 0x0400004F RID: 79
		internal const string WebEmptyRef = "WebEmptyRef";

		// Token: 0x04000050 RID: 80
		internal const string WebNullRef = "WebNullRef";

		// Token: 0x04000051 RID: 81
		internal const string WebRefInvalidAttribute = "WebRefInvalidAttribute";

		// Token: 0x04000052 RID: 82
		internal const string WebRefInvalidAttribute2 = "WebRefInvalidAttribute2";

		// Token: 0x04000053 RID: 83
		internal const string WebInvalidDocType = "WebInvalidDocType";

		// Token: 0x04000054 RID: 84
		internal const string WebDiscoRefReport = "WebDiscoRefReport";

		// Token: 0x04000055 RID: 85
		internal const string WebTextMatchMissingPattern = "WebTextMatchMissingPattern";

		// Token: 0x04000056 RID: 86
		internal const string WebTextMatchIgnoredTypeWarning = "WebTextMatchIgnoredTypeWarning";

		// Token: 0x04000057 RID: 87
		internal const string WebTextMatchBadCaptureIndex = "WebTextMatchBadCaptureIndex";

		// Token: 0x04000058 RID: 88
		internal const string WebTextMatchBadGroupIndex = "WebTextMatchBadGroupIndex";

		// Token: 0x04000059 RID: 89
		internal const string WebServiceDescriptionIgnoredOptional = "WebServiceDescriptionIgnoredOptional";

		// Token: 0x0400005A RID: 90
		internal const string WebServiceDescriptionIgnoredRequired = "WebServiceDescriptionIgnoredRequired";

		// Token: 0x0400005B RID: 91
		internal const string WebDuplicateServiceDescription = "WebDuplicateServiceDescription";

		// Token: 0x0400005C RID: 92
		internal const string WebDuplicateFormatExtension = "WebDuplicateFormatExtension";

		// Token: 0x0400005D RID: 93
		internal const string WebDuplicateOperationMessage = "WebDuplicateOperationMessage";

		// Token: 0x0400005E RID: 94
		internal const string WebDuplicateImport = "WebDuplicateImport";

		// Token: 0x0400005F RID: 95
		internal const string WebDuplicateMessage = "WebDuplicateMessage";

		// Token: 0x04000060 RID: 96
		internal const string WebDuplicatePort = "WebDuplicatePort";

		// Token: 0x04000061 RID: 97
		internal const string WebDuplicatePortType = "WebDuplicatePortType";

		// Token: 0x04000062 RID: 98
		internal const string WebDuplicateBinding = "WebDuplicateBinding";

		// Token: 0x04000063 RID: 99
		internal const string WebDuplicateService = "WebDuplicateService";

		// Token: 0x04000064 RID: 100
		internal const string WebDuplicateMessagePart = "WebDuplicateMessagePart";

		// Token: 0x04000065 RID: 101
		internal const string WebDuplicateOperationBinding = "WebDuplicateOperationBinding";

		// Token: 0x04000066 RID: 102
		internal const string WebDuplicateFaultBinding = "WebDuplicateFaultBinding";

		// Token: 0x04000067 RID: 103
		internal const string WebDuplicateOperation = "WebDuplicateOperation";

		// Token: 0x04000068 RID: 104
		internal const string WebDuplicateOperationFault = "WebDuplicateOperationFault";

		// Token: 0x04000069 RID: 105
		internal const string WebDuplicateUnknownElement = "WebDuplicateUnknownElement";

		// Token: 0x0400006A RID: 106
		internal const string WebUnknownEncodingStyle = "WebUnknownEncodingStyle";

		// Token: 0x0400006B RID: 107
		internal const string WebSoap11EncodingStyleNotSupported1 = "WebSoap11EncodingStyleNotSupported1";

		// Token: 0x0400006C RID: 108
		internal const string WebNullAsyncResultInBegin = "WebNullAsyncResultInBegin";

		// Token: 0x0400006D RID: 109
		internal const string WebNullAsyncResultInEnd = "WebNullAsyncResultInEnd";

		// Token: 0x0400006E RID: 110
		internal const string WebAsyncTransaction = "WebAsyncTransaction";

		// Token: 0x0400006F RID: 111
		internal const string WebConfigExtensionError = "WebConfigExtensionError";

		// Token: 0x04000070 RID: 112
		internal const string WebExtensionError = "WebExtensionError";

		// Token: 0x04000071 RID: 113
		internal const string WebChangeTypeFailed = "WebChangeTypeFailed";

		// Token: 0x04000072 RID: 114
		internal const string WebBadEnum = "WebBadEnum";

		// Token: 0x04000073 RID: 115
		internal const string WebBadHex = "WebBadHex";

		// Token: 0x04000074 RID: 116
		internal const string WebClientBindingAttributeRequired = "WebClientBindingAttributeRequired";

		// Token: 0x04000075 RID: 117
		internal const string WebHeaderInvalidRelay = "WebHeaderInvalidRelay";

		// Token: 0x04000076 RID: 118
		internal const string WebVirtualDisoRoot = "WebVirtualDisoRoot";

		// Token: 0x04000077 RID: 119
		internal const string WebRefDuplicateSchema = "WebRefDuplicateSchema";

		// Token: 0x04000078 RID: 120
		internal const string WebRefDuplicateService = "WebRefDuplicateService";

		// Token: 0x04000079 RID: 121
		internal const string WebWsiContentTypeEncoding = "WebWsiContentTypeEncoding";

		// Token: 0x0400007A RID: 122
		internal const string WebWsiViolation = "WebWsiViolation";

		// Token: 0x0400007B RID: 123
		internal const string WebNullReaderForMessage = "WebNullReaderForMessage";

		// Token: 0x0400007C RID: 124
		internal const string WebNullWriterForMessage = "WebNullWriterForMessage";

		// Token: 0x0400007D RID: 125
		internal const string NeedConcreteType = "NeedConcreteType";

		// Token: 0x0400007E RID: 126
		internal const string WebUnknownElement = "WebUnknownElement";

		// Token: 0x0400007F RID: 127
		internal const string WebUnknownElement1 = "WebUnknownElement1";

		// Token: 0x04000080 RID: 128
		internal const string WebUnknownElement2 = "WebUnknownElement2";

		// Token: 0x04000081 RID: 129
		internal const string WebUnknownAttribute = "WebUnknownAttribute";

		// Token: 0x04000082 RID: 130
		internal const string WebUnknownAttribute2 = "WebUnknownAttribute2";

		// Token: 0x04000083 RID: 131
		internal const string WebUnknownAttribute3 = "WebUnknownAttribute3";

		// Token: 0x04000084 RID: 132
		internal const string WebUnreferencedObject = "WebUnreferencedObject";

		// Token: 0x04000085 RID: 133
		internal const string WebSuppressedExceptionMessage = "WebSuppressedExceptionMessage";

		// Token: 0x04000086 RID: 134
		internal const string WebServiceContext = "WebServiceContext";

		// Token: 0x04000087 RID: 135
		internal const string WebServiceSession = "WebServiceSession";

		// Token: 0x04000088 RID: 136
		internal const string WebServiceServer = "WebServiceServer";

		// Token: 0x04000089 RID: 137
		internal const string WebServiceUser = "WebServiceUser";

		// Token: 0x0400008A RID: 138
		internal const string WebServiceSoapVersion = "WebServiceSoapVersion";

		// Token: 0x0400008B RID: 139
		internal const string ClientProtocolAllowAutoRedirect = "ClientProtocolAllowAutoRedirect";

		// Token: 0x0400008C RID: 140
		internal const string ClientProtocolCookieContainer = "ClientProtocolCookieContainer";

		// Token: 0x0400008D RID: 141
		internal const string ClientProtocolPreAuthenticate = "ClientProtocolPreAuthenticate";

		// Token: 0x0400008E RID: 142
		internal const string ClientProtocolClientCertificates = "ClientProtocolClientCertificates";

		// Token: 0x0400008F RID: 143
		internal const string ClientProtocolUrl = "ClientProtocolUrl";

		// Token: 0x04000090 RID: 144
		internal const string ClientProtocolEncoding = "ClientProtocolEncoding";

		// Token: 0x04000091 RID: 145
		internal const string ClientProtocolTimeout = "ClientProtocolTimeout";

		// Token: 0x04000092 RID: 146
		internal const string ClientProtocolUserAgent = "ClientProtocolUserAgent";

		// Token: 0x04000093 RID: 147
		internal const string ClientProtocolUsername = "ClientProtocolUsername";

		// Token: 0x04000094 RID: 148
		internal const string ClientProtocolPassword = "ClientProtocolPassword";

		// Token: 0x04000095 RID: 149
		internal const string ClientProtocolDomain = "ClientProtocolDomain";

		// Token: 0x04000096 RID: 150
		internal const string ClientProtocolProxyName = "ClientProtocolProxyName";

		// Token: 0x04000097 RID: 151
		internal const string ClientProtocolProxyPort = "ClientProtocolProxyPort";

		// Token: 0x04000098 RID: 152
		internal const string ClientProtocolSoapVersion = "ClientProtocolSoapVersion";

		// Token: 0x04000099 RID: 153
		internal const string ClientProtocolEnableDecompression = "ClientProtocolEnableDecompression";

		// Token: 0x0400009A RID: 154
		internal const string XmlLang = "XmlLang";

		// Token: 0x0400009B RID: 155
		internal const string HelpGeneratorHttpGetTitle = "HelpGeneratorHttpGetTitle";

		// Token: 0x0400009C RID: 156
		internal const string HelpGeneratorHttpGetText = "HelpGeneratorHttpGetText";

		// Token: 0x0400009D RID: 157
		internal const string HelpGeneratorHttpPostTitle = "HelpGeneratorHttpPostTitle";

		// Token: 0x0400009E RID: 158
		internal const string HelpGeneratorHttpPostText = "HelpGeneratorHttpPostText";

		// Token: 0x0400009F RID: 159
		internal const string HelpGeneratorSoapTitle = "HelpGeneratorSoapTitle";

		// Token: 0x040000A0 RID: 160
		internal const string HelpGeneratorSoap1_2Title = "HelpGeneratorSoap1_2Title";

		// Token: 0x040000A1 RID: 161
		internal const string HelpGeneratorSoapText = "HelpGeneratorSoapText";

		// Token: 0x040000A2 RID: 162
		internal const string HelpGeneratorSoap1_2Text = "HelpGeneratorSoap1_2Text";

		// Token: 0x040000A3 RID: 163
		internal const string HelpGeneratorInvokeButton = "HelpGeneratorInvokeButton";

		// Token: 0x040000A4 RID: 164
		internal const string HelpGeneratorParameter = "HelpGeneratorParameter";

		// Token: 0x040000A5 RID: 165
		internal const string HelpGeneratorValue = "HelpGeneratorValue";

		// Token: 0x040000A6 RID: 166
		internal const string HelpGeneratorTestHeader = "HelpGeneratorTestHeader";

		// Token: 0x040000A7 RID: 167
		internal const string HelpGeneratorTestText = "HelpGeneratorTestText";

		// Token: 0x040000A8 RID: 168
		internal const string HelpGeneratorNoTestFormRemote = "HelpGeneratorNoTestFormRemote";

		// Token: 0x040000A9 RID: 169
		internal const string HelpGeneratorLinkBack = "HelpGeneratorLinkBack";

		// Token: 0x040000AA RID: 170
		internal const string HelpGeneratorEnableHttpPostHeader = "HelpGeneratorEnableHttpPostHeader";

		// Token: 0x040000AB RID: 171
		internal const string HelpGeneratorEnableHttpPostInstructions = "HelpGeneratorEnableHttpPostInstructions";

		// Token: 0x040000AC RID: 172
		internal const string HelpGeneratorOperationsIntro = "HelpGeneratorOperationsIntro";

		// Token: 0x040000AD RID: 173
		internal const string HelpGeneratorWebService = "HelpGeneratorWebService";

		// Token: 0x040000AE RID: 174
		internal const string HelpGeneratorNoHttpGetTest = "HelpGeneratorNoHttpGetTest";

		// Token: 0x040000AF RID: 175
		internal const string HelpGeneratorNoHttpPostTest = "HelpGeneratorNoHttpPostTest";

		// Token: 0x040000B0 RID: 176
		internal const string HelpGeneratorNoTestNonPrimitive = "HelpGeneratorNoTestNonPrimitive";

		// Token: 0x040000B1 RID: 177
		internal const string HelpGeneratorMethodNotFound = "HelpGeneratorMethodNotFound";

		// Token: 0x040000B2 RID: 178
		internal const string HelpGeneratorMethodNotFoundText = "HelpGeneratorMethodNotFoundText";

		// Token: 0x040000B3 RID: 179
		internal const string HelpGeneratorStyleBODY = "HelpGeneratorStyleBODY";

		// Token: 0x040000B4 RID: 180
		internal const string HelpGeneratorStylecontent = "HelpGeneratorStylecontent";

		// Token: 0x040000B5 RID: 181
		internal const string HelpGeneratorStyleAlink = "HelpGeneratorStyleAlink";

		// Token: 0x040000B6 RID: 182
		internal const string HelpGeneratorStyleAvisited = "HelpGeneratorStyleAvisited";

		// Token: 0x040000B7 RID: 183
		internal const string HelpGeneratorStyleAactive = "HelpGeneratorStyleAactive";

		// Token: 0x040000B8 RID: 184
		internal const string HelpGeneratorStyleAhover = "HelpGeneratorStyleAhover";

		// Token: 0x040000B9 RID: 185
		internal const string HelpGeneratorStyleP = "HelpGeneratorStyleP";

		// Token: 0x040000BA RID: 186
		internal const string HelpGeneratorStylepre = "HelpGeneratorStylepre";

		// Token: 0x040000BB RID: 187
		internal const string HelpGeneratorStyletd = "HelpGeneratorStyletd";

		// Token: 0x040000BC RID: 188
		internal const string HelpGeneratorStyleh2 = "HelpGeneratorStyleh2";

		// Token: 0x040000BD RID: 189
		internal const string HelpGeneratorStyleh3 = "HelpGeneratorStyleh3";

		// Token: 0x040000BE RID: 190
		internal const string HelpGeneratorStyleul = "HelpGeneratorStyleul";

		// Token: 0x040000BF RID: 191
		internal const string HelpGeneratorStyleol = "HelpGeneratorStyleol";

		// Token: 0x040000C0 RID: 192
		internal const string HelpGeneratorStyleli = "HelpGeneratorStyleli";

		// Token: 0x040000C1 RID: 193
		internal const string HelpGeneratorStylefontvalue = "HelpGeneratorStylefontvalue";

		// Token: 0x040000C2 RID: 194
		internal const string HelpGeneratorStylefontkey = "HelpGeneratorStylefontkey";

		// Token: 0x040000C3 RID: 195
		internal const string HelpGeneratorStylefontError = "HelpGeneratorStylefontError";

		// Token: 0x040000C4 RID: 196
		internal const string HelpGeneratorStyleheading1 = "HelpGeneratorStyleheading1";

		// Token: 0x040000C5 RID: 197
		internal const string HelpGeneratorStylebutton = "HelpGeneratorStylebutton";

		// Token: 0x040000C6 RID: 198
		internal const string HelpGeneratorStylefrmheader = "HelpGeneratorStylefrmheader";

		// Token: 0x040000C7 RID: 199
		internal const string HelpGeneratorStylefrmtext = "HelpGeneratorStylefrmtext";

		// Token: 0x040000C8 RID: 200
		internal const string HelpGeneratorStylefrmInput = "HelpGeneratorStylefrmInput";

		// Token: 0x040000C9 RID: 201
		internal const string HelpGeneratorStyleintro = "HelpGeneratorStyleintro";

		// Token: 0x040000CA RID: 202
		internal const string HelpGeneratorImplementation = "HelpGeneratorImplementation";

		// Token: 0x040000CB RID: 203
		internal const string HelpGeneratorDefaultNamespaceWarning1 = "HelpGeneratorDefaultNamespaceWarning1";

		// Token: 0x040000CC RID: 204
		internal const string HelpGeneratorDefaultNamespaceWarning2 = "HelpGeneratorDefaultNamespaceWarning2";

		// Token: 0x040000CD RID: 205
		internal const string HelpGeneratorDefaultNamespaceHelp1 = "HelpGeneratorDefaultNamespaceHelp1";

		// Token: 0x040000CE RID: 206
		internal const string HelpGeneratorDefaultNamespaceHelp2 = "HelpGeneratorDefaultNamespaceHelp2";

		// Token: 0x040000CF RID: 207
		internal const string HelpGeneratorDefaultNamespaceHelp3 = "HelpGeneratorDefaultNamespaceHelp3";

		// Token: 0x040000D0 RID: 208
		internal const string HelpGeneratorDefaultNamespaceHelp4 = "HelpGeneratorDefaultNamespaceHelp4";

		// Token: 0x040000D1 RID: 209
		internal const string HelpGeneratorDefaultNamespaceHelp5 = "HelpGeneratorDefaultNamespaceHelp5";

		// Token: 0x040000D2 RID: 210
		internal const string HelpGeneratorDefaultNamespaceHelp6 = "HelpGeneratorDefaultNamespaceHelp6";

		// Token: 0x040000D3 RID: 211
		internal const string HelpGeneratorServiceConformance = "HelpGeneratorServiceConformance";

		// Token: 0x040000D4 RID: 212
		internal const string HelpGeneratorServiceConformanceDetails = "HelpGeneratorServiceConformanceDetails";

		// Token: 0x040000D5 RID: 213
		internal const string HelpGeneratorServiceConformanceConfig = "HelpGeneratorServiceConformanceConfig";

		// Token: 0x040000D6 RID: 214
		internal const string HelpGeneratorRecommendation = "HelpGeneratorRecommendation";

		// Token: 0x040000D7 RID: 215
		internal const string Rxxxx = "Rxxxx";

		// Token: 0x040000D8 RID: 216
		internal const string HelpGeneratorServiceConformanceRxxxx = "HelpGeneratorServiceConformanceRxxxx";

		// Token: 0x040000D9 RID: 217
		internal const string HelpGeneratorServiceConformanceRxxxx_r = "HelpGeneratorServiceConformanceRxxxx_r";

		// Token: 0x040000DA RID: 218
		internal const string HelpGeneratorServiceConformanceR2028 = "HelpGeneratorServiceConformanceR2028";

		// Token: 0x040000DB RID: 219
		internal const string HelpGeneratorServiceConformanceR2026 = "HelpGeneratorServiceConformanceR2026";

		// Token: 0x040000DC RID: 220
		internal const string HelpGeneratorServiceConformanceR2705 = "HelpGeneratorServiceConformanceR2705";

		// Token: 0x040000DD RID: 221
		internal const string HelpGeneratorServiceConformanceR2705_r = "HelpGeneratorServiceConformanceR2705_r";

		// Token: 0x040000DE RID: 222
		internal const string HelpGeneratorServiceConformanceR2706 = "HelpGeneratorServiceConformanceR2706";

		// Token: 0x040000DF RID: 223
		internal const string HelpGeneratorServiceConformanceR2706_r = "HelpGeneratorServiceConformanceR2706_r";

		// Token: 0x040000E0 RID: 224
		internal const string HelpGeneratorServiceConformanceR2007 = "HelpGeneratorServiceConformanceR2007";

		// Token: 0x040000E1 RID: 225
		internal const string HelpGeneratorServiceConformanceR2007_r = "HelpGeneratorServiceConformanceR2007_r";

		// Token: 0x040000E2 RID: 226
		internal const string HelpGeneratorServiceConformanceR2803 = "HelpGeneratorServiceConformanceR2803";

		// Token: 0x040000E3 RID: 227
		internal const string HelpGeneratorServiceConformanceR2803_r = "HelpGeneratorServiceConformanceR2803_r";

		// Token: 0x040000E4 RID: 228
		internal const string HelpGeneratorServiceConformanceR2105 = "HelpGeneratorServiceConformanceR2105";

		// Token: 0x040000E5 RID: 229
		internal const string HelpGeneratorServiceConformanceR2105_r = "HelpGeneratorServiceConformanceR2105_r";

		// Token: 0x040000E6 RID: 230
		internal const string HelpGeneratorServiceConformanceR1014 = "HelpGeneratorServiceConformanceR1014";

		// Token: 0x040000E7 RID: 231
		internal const string HelpGeneratorServiceConformanceR1014_r = "HelpGeneratorServiceConformanceR1014_r";

		// Token: 0x040000E8 RID: 232
		internal const string HelpGeneratorServiceConformanceR2201 = "HelpGeneratorServiceConformanceR2201";

		// Token: 0x040000E9 RID: 233
		internal const string HelpGeneratorServiceConformanceR2210 = "HelpGeneratorServiceConformanceR2210";

		// Token: 0x040000EA RID: 234
		internal const string HelpGeneratorServiceConformanceR2210_r = "HelpGeneratorServiceConformanceR2210_r";

		// Token: 0x040000EB RID: 235
		internal const string HelpGeneratorServiceConformanceR2306 = "HelpGeneratorServiceConformanceR2306";

		// Token: 0x040000EC RID: 236
		internal const string HelpGeneratorServiceConformanceR2203 = "HelpGeneratorServiceConformanceR2203";

		// Token: 0x040000ED RID: 237
		internal const string HelpGeneratorServiceConformanceR2204 = "HelpGeneratorServiceConformanceR2204";

		// Token: 0x040000EE RID: 238
		internal const string HelpGeneratorServiceConformanceR2205 = "HelpGeneratorServiceConformanceR2205";

		// Token: 0x040000EF RID: 239
		internal const string HelpGeneratorServiceConformanceR2303 = "HelpGeneratorServiceConformanceR2303";

		// Token: 0x040000F0 RID: 240
		internal const string HelpGeneratorServiceConformanceR2304 = "HelpGeneratorServiceConformanceR2304";

		// Token: 0x040000F1 RID: 241
		internal const string HelpGeneratorServiceConformanceR2304_r = "HelpGeneratorServiceConformanceR2304_r";

		// Token: 0x040000F2 RID: 242
		internal const string HelpGeneratorServiceConformanceR2701 = "HelpGeneratorServiceConformanceR2701";

		// Token: 0x040000F3 RID: 243
		internal const string HelpGeneratorServiceConformanceR2702 = "HelpGeneratorServiceConformanceR2702";

		// Token: 0x040000F4 RID: 244
		internal const string HelpGeneratorServiceConformanceR2710 = "HelpGeneratorServiceConformanceR2710";

		// Token: 0x040000F5 RID: 245
		internal const string HelpGeneratorServiceConformanceR2710_r = "HelpGeneratorServiceConformanceR2710_r";

		// Token: 0x040000F6 RID: 246
		internal const string HelpGeneratorServiceConformanceR2716 = "HelpGeneratorServiceConformanceR2716";

		// Token: 0x040000F7 RID: 247
		internal const string HelpGeneratorServiceConformanceR2717 = "HelpGeneratorServiceConformanceR2717";

		// Token: 0x040000F8 RID: 248
		internal const string HelpGeneratorServiceConformanceR2726 = "HelpGeneratorServiceConformanceR2726";

		// Token: 0x040000F9 RID: 249
		internal const string HelpGeneratorServiceConformanceR2718 = "HelpGeneratorServiceConformanceR2718";

		// Token: 0x040000FA RID: 250
		internal const string HelpGeneratorServiceConformanceR2720 = "HelpGeneratorServiceConformanceR2720";

		// Token: 0x040000FB RID: 251
		internal const string HelpGeneratorServiceConformanceR2749 = "HelpGeneratorServiceConformanceR2749";

		// Token: 0x040000FC RID: 252
		internal const string HelpGeneratorServiceConformanceR2721 = "HelpGeneratorServiceConformanceR2721";

		// Token: 0x040000FD RID: 253
		internal const string HelpGeneratorServiceConformanceR2754 = "HelpGeneratorServiceConformanceR2754";

		// Token: 0x040000FE RID: 254
		internal const string HelpGeneratorServiceConformanceHelp = "HelpGeneratorServiceConformanceHelp";

		// Token: 0x040000FF RID: 255
		internal const string BindingMissingAttribute = "BindingMissingAttribute";

		// Token: 0x04000100 RID: 256
		internal const string BindingInvalidAttribute = "BindingInvalidAttribute";

		// Token: 0x04000101 RID: 257
		internal const string OperationFlowNotification = "OperationFlowNotification";

		// Token: 0x04000102 RID: 258
		internal const string OperationFlowSolicitResponse = "OperationFlowSolicitResponse";

		// Token: 0x04000103 RID: 259
		internal const string PortTypeOperationMissing = "PortTypeOperationMissing";

		// Token: 0x04000104 RID: 260
		internal const string BindingOperationMissing = "BindingOperationMissing";

		// Token: 0x04000105 RID: 261
		internal const string BindingMultipleParts = "BindingMultipleParts";

		// Token: 0x04000106 RID: 262
		internal const string ElementEncodedExtension = "ElementEncodedExtension";

		// Token: 0x04000107 RID: 263
		internal const string InputElement = "InputElement";

		// Token: 0x04000108 RID: 264
		internal const string OutputElement = "OutputElement";

		// Token: 0x04000109 RID: 265
		internal const string Fault = "Fault";

		// Token: 0x0400010A RID: 266
		internal const string HeaderFault = "HeaderFault";

		// Token: 0x0400010B RID: 267
		internal const string Binding = "Binding";

		// Token: 0x0400010C RID: 268
		internal const string Operation = "Operation";

		// Token: 0x0400010D RID: 269
		internal const string OperationBinding = "OperationBinding";

		// Token: 0x0400010E RID: 270
		internal const string FaultBinding = "FaultBinding";

		// Token: 0x0400010F RID: 271
		internal const string Description = "Description";

		// Token: 0x04000110 RID: 272
		internal const string Element = "Element";

		// Token: 0x04000111 RID: 273
		internal const string Port = "Port";

		// Token: 0x04000112 RID: 274
		internal const string Message = "Message";

		// Token: 0x04000113 RID: 275
		internal const string Part = "Part";

		// Token: 0x04000114 RID: 276
		internal const string OperationMissingBinding = "OperationMissingBinding";

		// Token: 0x04000115 RID: 277
		internal const string UriValueRelative = "UriValueRelative";

		// Token: 0x04000116 RID: 278
		internal const string HelpGeneratorLanguageConfig = "HelpGeneratorLanguageConfig";

		// Token: 0x04000117 RID: 279
		internal const string HelpGeneratorInternalError = "HelpGeneratorInternalError";

		// Token: 0x04000118 RID: 280
		internal const string OperationOverload = "OperationOverload";

		// Token: 0x04000119 RID: 281
		internal const string WireSignature = "WireSignature";

		// Token: 0x0400011A RID: 282
		internal const string WireSignatureEmpty = "WireSignatureEmpty";

		// Token: 0x0400011B RID: 283
		internal const string WsdlInstanceValidation = "WsdlInstanceValidation";

		// Token: 0x0400011C RID: 284
		internal const string WsdlInstanceValidationDetails = "WsdlInstanceValidationDetails";

		// Token: 0x0400011D RID: 285
		internal const string WhenUsingAMessageStyleOfParametersAsDocument0 = "WhenUsingAMessageStyleOfParametersAsDocument0";

		// Token: 0x0400011E RID: 286
		internal const string UnsupportedMessageStyle1 = "UnsupportedMessageStyle1";

		// Token: 0x0400011F RID: 287
		internal const string TheMethodsAndUseTheSameSoapActionWhenTheService3 = "TheMethodsAndUseTheSameSoapActionWhenTheService3";

		// Token: 0x04000120 RID: 288
		internal const string TheMethodDoesNotHaveARequestElementEither1 = "TheMethodDoesNotHaveARequestElementEither1";

		// Token: 0x04000121 RID: 289
		internal const string TheMethodsAndUseTheSameRequestElementXmlns4 = "TheMethodsAndUseTheSameRequestElementXmlns4";

		// Token: 0x04000122 RID: 290
		internal const string TheMethodsAndUseTheSameRequestElementAndSoapActionXmlns6 = "TheMethodsAndUseTheSameRequestElementAndSoapActionXmlns6";

		// Token: 0x04000123 RID: 291
		internal const string TheRootElementForTheRequestCouldNotBeDetermined0 = "TheRootElementForTheRequestCouldNotBeDetermined0";

		// Token: 0x04000124 RID: 292
		internal const string TheRequestElementXmlnsWasNotRecognized2 = "TheRequestElementXmlnsWasNotRecognized2";

		// Token: 0x04000125 RID: 293
		internal const string ServiceDescriptionWasNotFound0 = "ServiceDescriptionWasNotFound0";

		// Token: 0x04000126 RID: 294
		internal const string internalError0 = "internalError0";

		// Token: 0x04000127 RID: 295
		internal const string DiscoveryIsNotPossibleBecauseTypeIsMissing1 = "DiscoveryIsNotPossibleBecauseTypeIsMissing1";

		// Token: 0x04000128 RID: 296
		internal const string TheBindingNamedFromNamespaceWasNotFoundIn3 = "TheBindingNamedFromNamespaceWasNotFoundIn3";

		// Token: 0x04000129 RID: 297
		internal const string Missing2 = "Missing2";

		// Token: 0x0400012A RID: 298
		internal const string MissingHttpOperationElement0 = "MissingHttpOperationElement0";

		// Token: 0x0400012B RID: 299
		internal const string MessageHasNoParts1 = "MessageHasNoParts1";

		// Token: 0x0400012C RID: 300
		internal const string DuplicateInputOutputNames0 = "DuplicateInputOutputNames0";

		// Token: 0x0400012D RID: 301
		internal const string MissingBinding0 = "MissingBinding0";

		// Token: 0x0400012E RID: 302
		internal const string MissingInputBinding0 = "MissingInputBinding0";

		// Token: 0x0400012F RID: 303
		internal const string MissingOutputBinding0 = "MissingOutputBinding0";

		// Token: 0x04000130 RID: 304
		internal const string UnableToImportOperation1 = "UnableToImportOperation1";

		// Token: 0x04000131 RID: 305
		internal const string UnableToImportBindingFromNamespace2 = "UnableToImportBindingFromNamespace2";

		// Token: 0x04000132 RID: 306
		internal const string TheOperationFromNamespaceHadInvalidSyntax3 = "TheOperationFromNamespaceHadInvalidSyntax3";

		// Token: 0x04000133 RID: 307
		internal const string TheOperationBindingFromNamespaceHadInvalid3 = "TheOperationBindingFromNamespaceHadInvalid3";

		// Token: 0x04000134 RID: 308
		internal const string IfAppSettingBaseUrlArgumentIsSpecifiedThen0 = "IfAppSettingBaseUrlArgumentIsSpecifiedThen0";

		// Token: 0x04000135 RID: 309
		internal const string MissingMessagePartForMessageFromNamespace3 = "MissingMessagePartForMessageFromNamespace3";

		// Token: 0x04000136 RID: 310
		internal const string MissingMessage2 = "MissingMessage2";

		// Token: 0x04000137 RID: 311
		internal const string OnlyXmlElementsOrTypesDerivingFromServiceDescriptionFormatExtension0 = "OnlyXmlElementsOrTypesDerivingFromServiceDescriptionFormatExtension0";

		// Token: 0x04000138 RID: 312
		internal const string OnlyOperationInputOrOperationOutputTypes = "OnlyOperationInputOrOperationOutputTypes";

		// Token: 0x04000139 RID: 313
		internal const string ProtocolWithNameIsNotRecognized1 = "ProtocolWithNameIsNotRecognized1";

		// Token: 0x0400013A RID: 314
		internal const string BothAndUseTheMessageNameUseTheMessageName3 = "BothAndUseTheMessageNameUseTheMessageName3";

		// Token: 0x0400013B RID: 315
		internal const string MissingSoapOperationBinding0 = "MissingSoapOperationBinding0";

		// Token: 0x0400013C RID: 316
		internal const string OnlyOneWebServiceBindingAttributeMayBeSpecified1 = "OnlyOneWebServiceBindingAttributeMayBeSpecified1";

		// Token: 0x0400013D RID: 317
		internal const string ContractOverride = "ContractOverride";

		// Token: 0x0400013E RID: 318
		internal const string TypeIsMissingWebServiceBindingAttributeThat2 = "TypeIsMissingWebServiceBindingAttributeThat2";

		// Token: 0x0400013F RID: 319
		internal const string MultipleBindingsWithSameName2 = "MultipleBindingsWithSameName2";

		// Token: 0x04000140 RID: 320
		internal const string UnknownWebServicesProtocolInConfigFile1 = "UnknownWebServicesProtocolInConfigFile1";

		// Token: 0x04000141 RID: 321
		internal const string RequiredXmlFormatExtensionAttributeIsMissing1 = "RequiredXmlFormatExtensionAttributeIsMissing1";

		// Token: 0x04000142 RID: 322
		internal const string TheSyntaxOfTypeMayNotBeExtended1 = "TheSyntaxOfTypeMayNotBeExtended1";

		// Token: 0x04000143 RID: 323
		internal const string InternalConfigurationError0 = "InternalConfigurationError0";

		// Token: 0x04000144 RID: 324
		internal const string ThereIsNoSoapTransportImporterThatUnderstands1 = "ThereIsNoSoapTransportImporterThatUnderstands1";

		// Token: 0x04000145 RID: 325
		internal const string MissingSoapBodyInputBinding0 = "MissingSoapBodyInputBinding0";

		// Token: 0x04000146 RID: 326
		internal const string MissingSoapBodyOutputBinding0 = "MissingSoapBodyOutputBinding0";

		// Token: 0x04000147 RID: 327
		internal const string TheOperationStyleRpcButBothMessagesAreNot0 = "TheOperationStyleRpcButBothMessagesAreNot0";

		// Token: 0x04000148 RID: 328
		internal const string TheCombinationOfStyleRpcWithUseLiteralIsNot0 = "TheCombinationOfStyleRpcWithUseLiteralIsNot0";

		// Token: 0x04000149 RID: 329
		internal const string TheEncodingIsNotSupported1 = "TheEncodingIsNotSupported1";

		// Token: 0x0400014A RID: 330
		internal const string SpecifyingAnElementForUseEncodedMessageParts0 = "SpecifyingAnElementForUseEncodedMessageParts0";

		// Token: 0x0400014B RID: 331
		internal const string EachMessagePartInAnUseEncodedMessageMustSpecify0 = "EachMessagePartInAnUseEncodedMessageMustSpecify0";

		// Token: 0x0400014C RID: 332
		internal const string SpecifyingATypeForUseLiteralMessagesIs0 = "SpecifyingATypeForUseLiteralMessagesIs0";

		// Token: 0x0400014D RID: 333
		internal const string SpecifyingATypeForUseLiteralMessagesIsAny = "SpecifyingATypeForUseLiteralMessagesIsAny";

		// Token: 0x0400014E RID: 334
		internal const string EachMessagePartInAUseLiteralMessageMustSpecify0 = "EachMessagePartInAUseLiteralMessageMustSpecify0";

		// Token: 0x0400014F RID: 335
		internal const string EachMessagePartInRpcUseLiteralMessageMustSpecify0 = "EachMessagePartInRpcUseLiteralMessageMustSpecify0";

		// Token: 0x04000150 RID: 336
		internal const string NoInputMIMEFormatsWereRecognized0 = "NoInputMIMEFormatsWereRecognized0";

		// Token: 0x04000151 RID: 337
		internal const string NoInputHTTPFormatsWereRecognized0 = "NoInputHTTPFormatsWereRecognized0";

		// Token: 0x04000152 RID: 338
		internal const string NoOutputMIMEFormatsWereRecognized0 = "NoOutputMIMEFormatsWereRecognized0";

		// Token: 0x04000153 RID: 339
		internal const string MissingMatchElement0 = "MissingMatchElement0";

		// Token: 0x04000154 RID: 340
		internal const string SolicitResponseIsNotSupported0 = "SolicitResponseIsNotSupported0";

		// Token: 0x04000155 RID: 341
		internal const string RequestResponseIsNotSupported0 = "RequestResponseIsNotSupported0";

		// Token: 0x04000156 RID: 342
		internal const string OneWayIsNotSupported0 = "OneWayIsNotSupported0";

		// Token: 0x04000157 RID: 343
		internal const string NotificationIsNotSupported0 = "NotificationIsNotSupported0";

		// Token: 0x04000158 RID: 344
		internal const string SyntaxErrorInWSDLDocumentMessageDoesNotHave1 = "SyntaxErrorInWSDLDocumentMessageDoesNotHave1";

		// Token: 0x04000159 RID: 345
		internal const string WebMissingBodyElement = "WebMissingBodyElement";

		// Token: 0x0400015A RID: 346
		internal const string WebMissingEnvelopeElement = "WebMissingEnvelopeElement";

		// Token: 0x0400015B RID: 347
		internal const string UnableToHandleRequestActionNotRecognized1 = "UnableToHandleRequestActionNotRecognized1";

		// Token: 0x0400015C RID: 348
		internal const string UnableToHandleRequestActionRequired0 = "UnableToHandleRequestActionRequired0";

		// Token: 0x0400015D RID: 349
		internal const string UnableToHandleRequest0 = "UnableToHandleRequest0";

		// Token: 0x0400015E RID: 350
		internal const string FailedToHandleRequest0 = "FailedToHandleRequest0";

		// Token: 0x0400015F RID: 351
		internal const string CodeGenSupportReferenceParameters = "CodeGenSupportReferenceParameters";

		// Token: 0x04000160 RID: 352
		internal const string CodeGenSupportParameterAttributes = "CodeGenSupportParameterAttributes";

		// Token: 0x04000161 RID: 353
		internal const string CodeGenSupportReturnTypeAttributes = "CodeGenSupportReturnTypeAttributes";

		// Token: 0x04000162 RID: 354
		internal const string TheBinding0FromNamespace1WasIgnored2 = "TheBinding0FromNamespace1WasIgnored2";

		// Token: 0x04000163 RID: 355
		internal const string TheOperation0FromNamespace1WasIgnored2 = "TheOperation0FromNamespace1WasIgnored2";

		// Token: 0x04000164 RID: 356
		internal const string TheOperationBinding0FromNamespace1WasIgnored = "TheOperationBinding0FromNamespace1WasIgnored";

		// Token: 0x04000165 RID: 357
		internal const string NoMethodsWereFoundInTheWSDLForThisProtocol = "NoMethodsWereFoundInTheWSDLForThisProtocol";

		// Token: 0x04000166 RID: 358
		internal const string UnexpectedFlush = "UnexpectedFlush";

		// Token: 0x04000167 RID: 359
		internal const string ThereWasAnErrorDuringAsyncProcessing = "ThereWasAnErrorDuringAsyncProcessing";

		// Token: 0x04000168 RID: 360
		internal const string CanTCallTheEndMethodOfAnAsyncCallMoreThan = "CanTCallTheEndMethodOfAnAsyncCallMoreThan";

		// Token: 0x04000169 RID: 361
		internal const string AsyncDuplicateUserState = "AsyncDuplicateUserState";

		// Token: 0x0400016A RID: 362
		internal const string StreamDoesNotSeek = "StreamDoesNotSeek";

		// Token: 0x0400016B RID: 363
		internal const string StreamDoesNotRead = "StreamDoesNotRead";

		// Token: 0x0400016C RID: 364
		internal const string ElementTypeMustBeObjectOrSoapReflectedException = "ElementTypeMustBeObjectOrSoapReflectedException";

		// Token: 0x0400016D RID: 365
		internal const string ElementTypeMustBeObjectOrSoapExtensionOrSoapReflectedException = "ElementTypeMustBeObjectOrSoapExtensionOrSoapReflectedException";

		// Token: 0x0400016E RID: 366
		internal const string ProtocolDoesNotAsyncSerialize = "ProtocolDoesNotAsyncSerialize";

		// Token: 0x0400016F RID: 367
		internal const string ThereWasAnErrorDownloading0 = "ThereWasAnErrorDownloading0";

		// Token: 0x04000170 RID: 368
		internal const string TheHTMLDocumentDoesNotContainDiscoveryInformation = "TheHTMLDocumentDoesNotContainDiscoveryInformation";

		// Token: 0x04000171 RID: 369
		internal const string TheDocumentWasNotRecognizedAsAKnownDocumentType = "TheDocumentWasNotRecognizedAsAKnownDocumentType";

		// Token: 0x04000172 RID: 370
		internal const string TheDocumentWasUnderstoodButContainsErrors = "TheDocumentWasUnderstoodButContainsErrors";

		// Token: 0x04000173 RID: 371
		internal const string TheWSDLDocumentContainsLinksThatCouldNotBeResolved = "TheWSDLDocumentContainsLinksThatCouldNotBeResolved";

		// Token: 0x04000174 RID: 372
		internal const string TheSchemaDocumentContainsLinksThatCouldNotBeResolved = "TheSchemaDocumentContainsLinksThatCouldNotBeResolved";

		// Token: 0x04000175 RID: 373
		internal const string CanTSpecifyElementOnEncodedMessagePartsPart = "CanTSpecifyElementOnEncodedMessagePartsPart";

		// Token: 0x04000176 RID: 374
		internal const string CanTMergeMessage = "CanTMergeMessage";

		// Token: 0x04000177 RID: 375
		internal const string CanTMergePortType = "CanTMergePortType";

		// Token: 0x04000178 RID: 376
		internal const string CanTMergeBinding = "CanTMergeBinding";

		// Token: 0x04000179 RID: 377
		internal const string CanTMergeTypes = "CanTMergeTypes";

		// Token: 0x0400017A RID: 378
		internal const string CanTMergeService = "CanTMergeService";

		// Token: 0x0400017B RID: 379
		internal const string indexMustBeBetweenAnd0Inclusive = "indexMustBeBetweenAnd0Inclusive";

		// Token: 0x0400017C RID: 380
		internal const string BPConformanceSoapEncodedMethod = "BPConformanceSoapEncodedMethod";

		// Token: 0x0400017D RID: 381
		internal const string BPConformanceHeaderFault = "BPConformanceHeaderFault";

		// Token: 0x0400017E RID: 382
		internal const string WsdlGenRpcLitAnonimousType = "WsdlGenRpcLitAnonimousType";

		// Token: 0x0400017F RID: 383
		internal const string WsdlGenRpcLitAccessorNamespace = "WsdlGenRpcLitAccessorNamespace";

		// Token: 0x04000180 RID: 384
		internal const string StackTraceEnd = "StackTraceEnd";

		// Token: 0x04000181 RID: 385
		internal const string CodeRemarks = "CodeRemarks";

		// Token: 0x04000182 RID: 386
		internal const string CodegenWarningDetails = "CodegenWarningDetails";

		// Token: 0x04000183 RID: 387
		internal const string ValidationError = "ValidationError";

		// Token: 0x04000184 RID: 388
		internal const string SchemaValidationError = "SchemaValidationError";

		// Token: 0x04000185 RID: 389
		internal const string SchemaValidationWarning = "SchemaValidationWarning";

		// Token: 0x04000186 RID: 390
		internal const string SchemaSyntaxErrorDetails = "SchemaSyntaxErrorDetails";

		// Token: 0x04000187 RID: 391
		internal const string SchemaSyntaxErrorItemDetails = "SchemaSyntaxErrorItemDetails";

		// Token: 0x04000188 RID: 392
		internal const string InitFailed = "InitFailed";

		// Token: 0x04000189 RID: 393
		internal const string XmlSchemaElementReference = "XmlSchemaElementReference";

		// Token: 0x0400018A RID: 394
		internal const string XmlSchemaAttributeReference = "XmlSchemaAttributeReference";

		// Token: 0x0400018B RID: 395
		internal const string XmlSchemaItem = "XmlSchemaItem";

		// Token: 0x0400018C RID: 396
		internal const string XmlSchemaNamedItem = "XmlSchemaNamedItem";

		// Token: 0x0400018D RID: 397
		internal const string XmlSchemaContentDef = "XmlSchemaContentDef";

		// Token: 0x0400018E RID: 398
		internal const string XmlSchema = "XmlSchema";

		// Token: 0x0400018F RID: 399
		internal const string TraceCallEnter = "TraceCallEnter";

		// Token: 0x04000190 RID: 400
		internal const string TraceCallEnterDetails = "TraceCallEnterDetails";

		// Token: 0x04000191 RID: 401
		internal const string TraceCallExit = "TraceCallExit";

		// Token: 0x04000192 RID: 402
		internal const string TraceExceptionThrown = "TraceExceptionThrown";

		// Token: 0x04000193 RID: 403
		internal const string TraceExceptionCought = "TraceExceptionCought";

		// Token: 0x04000194 RID: 404
		internal const string TraceExceptionIgnored = "TraceExceptionIgnored";

		// Token: 0x04000195 RID: 405
		internal const string TraceExceptionDetails = "TraceExceptionDetails";

		// Token: 0x04000196 RID: 406
		internal const string TracePostWorkItemIn = "TracePostWorkItemIn";

		// Token: 0x04000197 RID: 407
		internal const string TracePostWorkItemOut = "TracePostWorkItemOut";

		// Token: 0x04000198 RID: 408
		internal const string TraceUserHostName = "TraceUserHostName";

		// Token: 0x04000199 RID: 409
		internal const string TraceUserHostAddress = "TraceUserHostAddress";

		// Token: 0x0400019A RID: 410
		internal const string TraceUrl = "TraceUrl";

		// Token: 0x0400019B RID: 411
		internal const string TraceUrlReferrer = "TraceUrlReferrer";

		// Token: 0x0400019C RID: 412
		internal const string TraceCreateSerializer = "TraceCreateSerializer";

		// Token: 0x0400019D RID: 413
		internal const string TraceWriteRequest = "TraceWriteRequest";

		// Token: 0x0400019E RID: 414
		internal const string TraceWriteResponse = "TraceWriteResponse";

		// Token: 0x0400019F RID: 415
		internal const string TraceWriteHeaders = "TraceWriteHeaders";

		// Token: 0x040001A0 RID: 416
		internal const string TraceReadRequest = "TraceReadRequest";

		// Token: 0x040001A1 RID: 417
		internal const string TraceReadResponse = "TraceReadResponse";

		// Token: 0x040001A2 RID: 418
		internal const string TraceReadHeaders = "TraceReadHeaders";

		// Token: 0x040001A3 RID: 419
		private static Res loader;

		// Token: 0x040001A4 RID: 420
		private ResourceManager resources;

		// Token: 0x040001A5 RID: 421
		private static object s_InternalSyncObject;
	}
}
