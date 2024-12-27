using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Drawing
{
	// Token: 0x02000007 RID: 7
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
			this.resources = new ResourceManager("System.Drawing.Res", base.GetType().Assembly);
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

		// Token: 0x04000031 RID: 49
		internal const string CantTellPrinterName = "CantTellPrinterName";

		// Token: 0x04000032 RID: 50
		internal const string CantChangeImmutableObjects = "CantChangeImmutableObjects";

		// Token: 0x04000033 RID: 51
		internal const string CantMakeIconTransparent = "CantMakeIconTransparent";

		// Token: 0x04000034 RID: 52
		internal const string ColorNotSystemColor = "ColorNotSystemColor";

		// Token: 0x04000035 RID: 53
		internal const string DotNET_ComponentType = "DotNET_ComponentType";

		// Token: 0x04000036 RID: 54
		internal const string GdiplusAborted = "GdiplusAborted";

		// Token: 0x04000037 RID: 55
		internal const string GdiplusAccessDenied = "GdiplusAccessDenied";

		// Token: 0x04000038 RID: 56
		internal const string GdiplusCannotCreateGraphicsFromIndexedPixelFormat = "GdiplusCannotCreateGraphicsFromIndexedPixelFormat";

		// Token: 0x04000039 RID: 57
		internal const string GdiplusCannotSetPixelFromIndexedPixelFormat = "GdiplusCannotSetPixelFromIndexedPixelFormat";

		// Token: 0x0400003A RID: 58
		internal const string GdiplusDestPointsInvalidParallelogram = "GdiplusDestPointsInvalidParallelogram";

		// Token: 0x0400003B RID: 59
		internal const string GdiplusDestPointsInvalidLength = "GdiplusDestPointsInvalidLength";

		// Token: 0x0400003C RID: 60
		internal const string GdiplusFileNotFound = "GdiplusFileNotFound";

		// Token: 0x0400003D RID: 61
		internal const string GdiplusFontFamilyNotFound = "GdiplusFontFamilyNotFound";

		// Token: 0x0400003E RID: 62
		internal const string GdiplusFontStyleNotFound = "GdiplusFontStyleNotFound";

		// Token: 0x0400003F RID: 63
		internal const string GdiplusGenericError = "GdiplusGenericError";

		// Token: 0x04000040 RID: 64
		internal const string GdiplusInsufficientBuffer = "GdiplusInsufficientBuffer";

		// Token: 0x04000041 RID: 65
		internal const string GdiplusInvalidParameter = "GdiplusInvalidParameter";

		// Token: 0x04000042 RID: 66
		internal const string GdiplusInvalidRectangle = "GdiplusInvalidRectangle";

		// Token: 0x04000043 RID: 67
		internal const string GdiplusInvalidSize = "GdiplusInvalidSize";

		// Token: 0x04000044 RID: 68
		internal const string GdiplusOutOfMemory = "GdiplusOutOfMemory";

		// Token: 0x04000045 RID: 69
		internal const string GdiplusNotImplemented = "GdiplusNotImplemented";

		// Token: 0x04000046 RID: 70
		internal const string GdiplusNotInitialized = "GdiplusNotInitialized";

		// Token: 0x04000047 RID: 71
		internal const string GdiplusNotTrueTypeFont = "GdiplusNotTrueTypeFont";

		// Token: 0x04000048 RID: 72
		internal const string GdiplusNotTrueTypeFont_NoName = "GdiplusNotTrueTypeFont_NoName";

		// Token: 0x04000049 RID: 73
		internal const string GdiplusObjectBusy = "GdiplusObjectBusy";

		// Token: 0x0400004A RID: 74
		internal const string GdiplusOverflow = "GdiplusOverflow";

		// Token: 0x0400004B RID: 75
		internal const string GdiplusPropertyNotFoundError = "GdiplusPropertyNotFoundError";

		// Token: 0x0400004C RID: 76
		internal const string GdiplusPropertyNotSupportedError = "GdiplusPropertyNotSupportedError";

		// Token: 0x0400004D RID: 77
		internal const string GdiplusUnknown = "GdiplusUnknown";

		// Token: 0x0400004E RID: 78
		internal const string GdiplusUnknownImageFormat = "GdiplusUnknownImageFormat";

		// Token: 0x0400004F RID: 79
		internal const string GdiplusUnsupportedGdiplusVersion = "GdiplusUnsupportedGdiplusVersion";

		// Token: 0x04000050 RID: 80
		internal const string GdiplusWrongState = "GdiplusWrongState";

		// Token: 0x04000051 RID: 81
		internal const string GlobalAssemblyCache = "GlobalAssemblyCache";

		// Token: 0x04000052 RID: 82
		internal const string GraphicsBufferCurrentlyBusy = "GraphicsBufferCurrentlyBusy";

		// Token: 0x04000053 RID: 83
		internal const string GraphicsBufferQueryFail = "GraphicsBufferQueryFail";

		// Token: 0x04000054 RID: 84
		internal const string ToolboxItemLocked = "ToolboxItemLocked";

		// Token: 0x04000055 RID: 85
		internal const string ToolboxItemInvalidPropertyType = "ToolboxItemInvalidPropertyType";

		// Token: 0x04000056 RID: 86
		internal const string ToolboxItemValueNotSerializable = "ToolboxItemValueNotSerializable";

		// Token: 0x04000057 RID: 87
		internal const string ToolboxItemInvalidKey = "ToolboxItemInvalidKey";

		// Token: 0x04000058 RID: 88
		internal const string IllegalState = "IllegalState";

		// Token: 0x04000059 RID: 89
		internal const string InterpolationColorsColorBlendNotSet = "InterpolationColorsColorBlendNotSet";

		// Token: 0x0400005A RID: 90
		internal const string InterpolationColorsCommon = "InterpolationColorsCommon";

		// Token: 0x0400005B RID: 91
		internal const string InterpolationColorsInvalidColorBlendObject = "InterpolationColorsInvalidColorBlendObject";

		// Token: 0x0400005C RID: 92
		internal const string InterpolationColorsInvalidStartPosition = "InterpolationColorsInvalidStartPosition";

		// Token: 0x0400005D RID: 93
		internal const string InterpolationColorsInvalidEndPosition = "InterpolationColorsInvalidEndPosition";

		// Token: 0x0400005E RID: 94
		internal const string InterpolationColorsLength = "InterpolationColorsLength";

		// Token: 0x0400005F RID: 95
		internal const string InterpolationColorsLengthsDiffer = "InterpolationColorsLengthsDiffer";

		// Token: 0x04000060 RID: 96
		internal const string InvalidArgument = "InvalidArgument";

		// Token: 0x04000061 RID: 97
		internal const string InvalidBoundArgument = "InvalidBoundArgument";

		// Token: 0x04000062 RID: 98
		internal const string InvalidClassName = "InvalidClassName";

		// Token: 0x04000063 RID: 99
		internal const string InvalidColor = "InvalidColor";

		// Token: 0x04000064 RID: 100
		internal const string InvalidDashPattern = "InvalidDashPattern";

		// Token: 0x04000065 RID: 101
		internal const string InvalidEx2BoundArgument = "InvalidEx2BoundArgument";

		// Token: 0x04000066 RID: 102
		internal const string InvalidFrame = "InvalidFrame";

		// Token: 0x04000067 RID: 103
		internal const string InvalidGDIHandle = "InvalidGDIHandle";

		// Token: 0x04000068 RID: 104
		internal const string InvalidImage = "InvalidImage";

		// Token: 0x04000069 RID: 105
		internal const string InvalidLowBoundArgumentEx = "InvalidLowBoundArgumentEx";

		// Token: 0x0400006A RID: 106
		internal const string InvalidPermissionLevel = "InvalidPermissionLevel";

		// Token: 0x0400006B RID: 107
		internal const string InvalidPermissionState = "InvalidPermissionState";

		// Token: 0x0400006C RID: 108
		internal const string InvalidPictureType = "InvalidPictureType";

		// Token: 0x0400006D RID: 109
		internal const string InvalidPrinterException_InvalidPrinter = "InvalidPrinterException_InvalidPrinter";

		// Token: 0x0400006E RID: 110
		internal const string InvalidPrinterException_NoDefaultPrinter = "InvalidPrinterException_NoDefaultPrinter";

		// Token: 0x0400006F RID: 111
		internal const string InvalidPrinterHandle = "InvalidPrinterHandle";

		// Token: 0x04000070 RID: 112
		internal const string ValidRangeX = "ValidRangeX";

		// Token: 0x04000071 RID: 113
		internal const string ValidRangeY = "ValidRangeY";

		// Token: 0x04000072 RID: 114
		internal const string NativeHandle0 = "NativeHandle0";

		// Token: 0x04000073 RID: 115
		internal const string NoDefaultPrinter = "NoDefaultPrinter";

		// Token: 0x04000074 RID: 116
		internal const string NotImplemented = "NotImplemented";

		// Token: 0x04000075 RID: 117
		internal const string PDOCbeginPrintDescr = "PDOCbeginPrintDescr";

		// Token: 0x04000076 RID: 118
		internal const string PDOCdocumentNameDescr = "PDOCdocumentNameDescr";

		// Token: 0x04000077 RID: 119
		internal const string PDOCdocumentPageSettingsDescr = "PDOCdocumentPageSettingsDescr";

		// Token: 0x04000078 RID: 120
		internal const string PDOCendPrintDescr = "PDOCendPrintDescr";

		// Token: 0x04000079 RID: 121
		internal const string PDOCoriginAtMarginsDescr = "PDOCoriginAtMarginsDescr";

		// Token: 0x0400007A RID: 122
		internal const string PDOCprintControllerDescr = "PDOCprintControllerDescr";

		// Token: 0x0400007B RID: 123
		internal const string PDOCprintPageDescr = "PDOCprintPageDescr";

		// Token: 0x0400007C RID: 124
		internal const string PDOCprinterSettingsDescr = "PDOCprinterSettingsDescr";

		// Token: 0x0400007D RID: 125
		internal const string PDOCqueryPageSettingsDescr = "PDOCqueryPageSettingsDescr";

		// Token: 0x0400007E RID: 126
		internal const string PrintDocumentDesc = "PrintDocumentDesc";

		// Token: 0x0400007F RID: 127
		internal const string PrintingPermissionBadXml = "PrintingPermissionBadXml";

		// Token: 0x04000080 RID: 128
		internal const string PrintingPermissionAttributeInvalidPermissionLevel = "PrintingPermissionAttributeInvalidPermissionLevel";

		// Token: 0x04000081 RID: 129
		internal const string PropertyValueInvalidEntry = "PropertyValueInvalidEntry";

		// Token: 0x04000082 RID: 130
		internal const string PSizeNotCustom = "PSizeNotCustom";

		// Token: 0x04000083 RID: 131
		internal const string ResourceNotFound = "ResourceNotFound";

		// Token: 0x04000084 RID: 132
		internal const string TargetNotPrintingPermission = "TargetNotPrintingPermission";

		// Token: 0x04000085 RID: 133
		internal const string TextParseFailedFormat = "TextParseFailedFormat";

		// Token: 0x04000086 RID: 134
		internal const string TriStateCompareError = "TriStateCompareError";

		// Token: 0x04000087 RID: 135
		internal const string toStringIcon = "toStringIcon";

		// Token: 0x04000088 RID: 136
		internal const string toStringNone = "toStringNone";

		// Token: 0x04000089 RID: 137
		internal const string DCTypeInvalid = "DCTypeInvalid";

		// Token: 0x0400008A RID: 138
		private static SR loader;

		// Token: 0x0400008B RID: 139
		private ResourceManager resources;

		// Token: 0x0400008C RID: 140
		private static object s_InternalSyncObject;
	}
}
