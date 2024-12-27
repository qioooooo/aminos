using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Windows.Forms
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
			this.resources = new ResourceManager("System.Windows.Forms", base.GetType().Assembly);
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
		internal const string AboutBoxDesc = "AboutBoxDesc";

		// Token: 0x04000032 RID: 50
		internal const string AccDGCollapse = "AccDGCollapse";

		// Token: 0x04000033 RID: 51
		internal const string AccDGEdit = "AccDGEdit";

		// Token: 0x04000034 RID: 52
		internal const string AccDGExpand = "AccDGExpand";

		// Token: 0x04000035 RID: 53
		internal const string AccDGNavigate = "AccDGNavigate";

		// Token: 0x04000036 RID: 54
		internal const string AccDGNavigateBack = "AccDGNavigateBack";

		// Token: 0x04000037 RID: 55
		internal const string AccDGNewRow = "AccDGNewRow";

		// Token: 0x04000038 RID: 56
		internal const string AccDGParentRow = "AccDGParentRow";

		// Token: 0x04000039 RID: 57
		internal const string AccDGParentRows = "AccDGParentRows";

		// Token: 0x0400003A RID: 58
		internal const string AccessibleActionCheck = "AccessibleActionCheck";

		// Token: 0x0400003B RID: 59
		internal const string AccessibleActionClick = "AccessibleActionClick";

		// Token: 0x0400003C RID: 60
		internal const string AccessibleActionCollapse = "AccessibleActionCollapse";

		// Token: 0x0400003D RID: 61
		internal const string AccessibleActionExpand = "AccessibleActionExpand";

		// Token: 0x0400003E RID: 62
		internal const string AccessibleActionPress = "AccessibleActionPress";

		// Token: 0x0400003F RID: 63
		internal const string AccessibleActionUncheck = "AccessibleActionUncheck";

		// Token: 0x04000040 RID: 64
		internal const string AddDifferentThreads = "AddDifferentThreads";

		// Token: 0x04000041 RID: 65
		internal const string ApplicationCannotChangeThreadExceptionMode = "ApplicationCannotChangeThreadExceptionMode";

		// Token: 0x04000042 RID: 66
		internal const string ApplicationCannotChangeApplicationExceptionMode = "ApplicationCannotChangeApplicationExceptionMode";

		// Token: 0x04000043 RID: 67
		internal const string ApplyCaption = "ApplyCaption";

		// Token: 0x04000044 RID: 68
		internal const string ArraysNotSameSize = "ArraysNotSameSize";

		// Token: 0x04000045 RID: 69
		internal const string AutoCompleteFailure = "AutoCompleteFailure";

		// Token: 0x04000046 RID: 70
		internal const string AutoCompleteFailureListItems = "AutoCompleteFailureListItems";

		// Token: 0x04000047 RID: 71
		internal const string AXAddInvalidEvent = "AXAddInvalidEvent";

		// Token: 0x04000048 RID: 72
		internal const string AXDuplicateControl = "AXDuplicateControl";

		// Token: 0x04000049 RID: 73
		internal const string AXEditProperties = "AXEditProperties";

		// Token: 0x0400004A RID: 74
		internal const string AXFontUnitNotPoint = "AXFontUnitNotPoint";

		// Token: 0x0400004B RID: 75
		internal const string AxInterfaceNotSupported = "AxInterfaceNotSupported";

		// Token: 0x0400004C RID: 76
		internal const string AXInvalidArgument = "AXInvalidArgument";

		// Token: 0x0400004D RID: 77
		internal const string AXInvalidMethodInvoke = "AXInvalidMethodInvoke";

		// Token: 0x0400004E RID: 78
		internal const string AXInvalidPropertyGet = "AXInvalidPropertyGet";

		// Token: 0x0400004F RID: 79
		internal const string AXInvalidPropertySet = "AXInvalidPropertySet";

		// Token: 0x04000050 RID: 80
		internal const string AXMTAThread = "AXMTAThread";

		// Token: 0x04000051 RID: 81
		internal const string AXNoConnectionPoint = "AXNoConnectionPoint";

		// Token: 0x04000052 RID: 82
		internal const string AXNoConnectionPointContainer = "AXNoConnectionPointContainer";

		// Token: 0x04000053 RID: 83
		internal const string AXNoEventInterface = "AXNoEventInterface";

		// Token: 0x04000054 RID: 84
		internal const string AXNohWnd = "AXNohWnd";

		// Token: 0x04000055 RID: 85
		internal const string AXNoLicenseToUse = "AXNoLicenseToUse";

		// Token: 0x04000056 RID: 86
		internal const string AXNoSinkAdvise = "AXNoSinkAdvise";

		// Token: 0x04000057 RID: 87
		internal const string AXNoSinkImplementation = "AXNoSinkImplementation";

		// Token: 0x04000058 RID: 88
		internal const string AXNoThreadInfo = "AXNoThreadInfo";

		// Token: 0x04000059 RID: 89
		internal const string AXNotImplemented = "AXNotImplemented";

		// Token: 0x0400005A RID: 90
		internal const string AXNoTopLevelContainerControl = "AXNoTopLevelContainerControl";

		// Token: 0x0400005B RID: 91
		internal const string AXOcxStateLoaded = "AXOcxStateLoaded";

		// Token: 0x0400005C RID: 92
		internal const string AXProperties = "AXProperties";

		// Token: 0x0400005D RID: 93
		internal const string AXSingleThreaded = "AXSingleThreaded";

		// Token: 0x0400005E RID: 94
		internal const string AXTopLevelSource = "AXTopLevelSource";

		// Token: 0x0400005F RID: 95
		internal const string AXUnknownError = "AXUnknownError";

		// Token: 0x04000060 RID: 96
		internal const string AXUnknownImage = "AXUnknownImage";

		// Token: 0x04000061 RID: 97
		internal const string AXWindowlessControl = "AXWindowlessControl";

		// Token: 0x04000062 RID: 98
		internal const string BadDataSourceForComplexBinding = "BadDataSourceForComplexBinding";

		// Token: 0x04000063 RID: 99
		internal const string BindingManagerBadIndex = "BindingManagerBadIndex";

		// Token: 0x04000064 RID: 100
		internal const string BindingNavigatorAddNewItemPropDescr = "BindingNavigatorAddNewItemPropDescr";

		// Token: 0x04000065 RID: 101
		internal const string BindingNavigatorAddNewItemText = "BindingNavigatorAddNewItemText";

		// Token: 0x04000066 RID: 102
		internal const string BindingNavigatorBindingSourcePropDescr = "BindingNavigatorBindingSourcePropDescr";

		// Token: 0x04000067 RID: 103
		internal const string BindingNavigatorCountItemFormat = "BindingNavigatorCountItemFormat";

		// Token: 0x04000068 RID: 104
		internal const string BindingNavigatorCountItemFormatPropDescr = "BindingNavigatorCountItemFormatPropDescr";

		// Token: 0x04000069 RID: 105
		internal const string BindingNavigatorCountItemPropDescr = "BindingNavigatorCountItemPropDescr";

		// Token: 0x0400006A RID: 106
		internal const string BindingNavigatorCountItemTip = "BindingNavigatorCountItemTip";

		// Token: 0x0400006B RID: 107
		internal const string BindingNavigatorDeleteItemPropDescr = "BindingNavigatorDeleteItemPropDescr";

		// Token: 0x0400006C RID: 108
		internal const string BindingNavigatorDeleteItemText = "BindingNavigatorDeleteItemText";

		// Token: 0x0400006D RID: 109
		internal const string BindingNavigatorMoveFirstItemPropDescr = "BindingNavigatorMoveFirstItemPropDescr";

		// Token: 0x0400006E RID: 110
		internal const string BindingNavigatorMoveFirstItemText = "BindingNavigatorMoveFirstItemText";

		// Token: 0x0400006F RID: 111
		internal const string BindingNavigatorMoveLastItemPropDescr = "BindingNavigatorMoveLastItemPropDescr";

		// Token: 0x04000070 RID: 112
		internal const string BindingNavigatorMoveLastItemText = "BindingNavigatorMoveLastItemText";

		// Token: 0x04000071 RID: 113
		internal const string BindingNavigatorMoveNextItemPropDescr = "BindingNavigatorMoveNextItemPropDescr";

		// Token: 0x04000072 RID: 114
		internal const string BindingNavigatorMoveNextItemText = "BindingNavigatorMoveNextItemText";

		// Token: 0x04000073 RID: 115
		internal const string BindingNavigatorMovePreviousItemPropDescr = "BindingNavigatorMovePreviousItemPropDescr";

		// Token: 0x04000074 RID: 116
		internal const string BindingNavigatorMovePreviousItemText = "BindingNavigatorMovePreviousItemText";

		// Token: 0x04000075 RID: 117
		internal const string BindingNavigatorPositionAccessibleName = "BindingNavigatorPositionAccessibleName";

		// Token: 0x04000076 RID: 118
		internal const string BindingNavigatorPositionItemPropDescr = "BindingNavigatorPositionItemPropDescr";

		// Token: 0x04000077 RID: 119
		internal const string BindingNavigatorPositionItemTip = "BindingNavigatorPositionItemTip";

		// Token: 0x04000078 RID: 120
		internal const string BindingNavigatorRefreshItemsEventDescr = "BindingNavigatorRefreshItemsEventDescr";

		// Token: 0x04000079 RID: 121
		internal const string BindingNavigatorToolStripName = "BindingNavigatorToolStripName";

		// Token: 0x0400007A RID: 122
		internal const string BindingsCollectionAdd1 = "BindingsCollectionAdd1";

		// Token: 0x0400007B RID: 123
		internal const string BindingsCollectionAdd2 = "BindingsCollectionAdd2";

		// Token: 0x0400007C RID: 124
		internal const string BindingsCollectionBadIndex = "BindingsCollectionBadIndex";

		// Token: 0x0400007D RID: 125
		internal const string BindingsCollectionDup = "BindingsCollectionDup";

		// Token: 0x0400007E RID: 126
		internal const string BindingsCollectionForeign = "BindingsCollectionForeign";

		// Token: 0x0400007F RID: 127
		internal const string BindingSourceAddingNewEventHandlerDescr = "BindingSourceAddingNewEventHandlerDescr";

		// Token: 0x04000080 RID: 128
		internal const string BindingSourceAllowNewDescr = "BindingSourceAllowNewDescr";

		// Token: 0x04000081 RID: 129
		internal const string BindingSourceBadSortString = "BindingSourceBadSortString";

		// Token: 0x04000082 RID: 130
		internal const string BindingSourceBindingCompleteEventHandlerDescr = "BindingSourceBindingCompleteEventHandlerDescr";

		// Token: 0x04000083 RID: 131
		internal const string BindingSourceBindingListWrapperAddToReadOnlyList = "BindingSourceBindingListWrapperAddToReadOnlyList";

		// Token: 0x04000084 RID: 132
		internal const string BindingSourceBindingListWrapperNeedAParameterlessConstructor = "BindingSourceBindingListWrapperNeedAParameterlessConstructor";

		// Token: 0x04000085 RID: 133
		internal const string BindingSourceBindingListWrapperNeedToSetAllowNew = "BindingSourceBindingListWrapperNeedToSetAllowNew";

		// Token: 0x04000086 RID: 134
		internal const string BindingSourceCurrentChangedEventHandlerDescr = "BindingSourceCurrentChangedEventHandlerDescr";

		// Token: 0x04000087 RID: 135
		internal const string BindingSourceCurrentItemChangedEventHandlerDescr = "BindingSourceCurrentItemChangedEventHandlerDescr";

		// Token: 0x04000088 RID: 136
		internal const string BindingSourceDataErrorEventHandlerDescr = "BindingSourceDataErrorEventHandlerDescr";

		// Token: 0x04000089 RID: 137
		internal const string BindingSourceDataMemberChangedEventHandlerDescr = "BindingSourceDataMemberChangedEventHandlerDescr";

		// Token: 0x0400008A RID: 138
		internal const string BindingSourceDataMemberDescr = "BindingSourceDataMemberDescr";

		// Token: 0x0400008B RID: 139
		internal const string BindingSourceDataSourceChangedEventHandlerDescr = "BindingSourceDataSourceChangedEventHandlerDescr";

		// Token: 0x0400008C RID: 140
		internal const string BindingSourceDataSourceDescr = "BindingSourceDataSourceDescr";

		// Token: 0x0400008D RID: 141
		internal const string BindingSourceFilterDescr = "BindingSourceFilterDescr";

		// Token: 0x0400008E RID: 142
		internal const string BindingSourceInstanceError = "BindingSourceInstanceError";

		// Token: 0x0400008F RID: 143
		internal const string BindingSourceItemChangedEventModeDescr = "BindingSourceItemChangedEventModeDescr";

		// Token: 0x04000090 RID: 144
		internal const string BindingSourceItemTypeIsValueType = "BindingSourceItemTypeIsValueType";

		// Token: 0x04000091 RID: 145
		internal const string BindingSourceItemTypeMismatchOnAdd = "BindingSourceItemTypeMismatchOnAdd";

		// Token: 0x04000092 RID: 146
		internal const string BindingSourceListChangedEventHandlerDescr = "BindingSourceListChangedEventHandlerDescr";

		// Token: 0x04000093 RID: 147
		internal const string BindingSourcePositionChangedEventHandlerDescr = "BindingSourcePositionChangedEventHandlerDescr";

		// Token: 0x04000094 RID: 148
		internal const string BindingSourceRecursionDetected = "BindingSourceRecursionDetected";

		// Token: 0x04000095 RID: 149
		internal const string BindingSourceRemoveCurrentNoCurrentItem = "BindingSourceRemoveCurrentNoCurrentItem";

		// Token: 0x04000096 RID: 150
		internal const string BindingSourceRemoveCurrentNotAllowed = "BindingSourceRemoveCurrentNotAllowed";

		// Token: 0x04000097 RID: 151
		internal const string BindingSourceSortDescr = "BindingSourceSortDescr";

		// Token: 0x04000098 RID: 152
		internal const string BindingSourceSortStringPropertyNotInIBindingList = "BindingSourceSortStringPropertyNotInIBindingList";

		// Token: 0x04000099 RID: 153
		internal const string BlinkRateMustBeZeroOrMore = "BlinkRateMustBeZeroOrMore";

		// Token: 0x0400009A RID: 154
		internal const string borderStyleDescr = "borderStyleDescr";

		// Token: 0x0400009B RID: 155
		internal const string ButtonAutoEllipsisDescr = "ButtonAutoEllipsisDescr";

		// Token: 0x0400009C RID: 156
		internal const string ButtonBorderColorDescr = "ButtonBorderColorDescr";

		// Token: 0x0400009D RID: 157
		internal const string ButtonBorderSizeDescr = "ButtonBorderSizeDescr";

		// Token: 0x0400009E RID: 158
		internal const string ButtonCheckedBackColorDescr = "ButtonCheckedBackColorDescr";

		// Token: 0x0400009F RID: 159
		internal const string ButtonDialogResultDescr = "ButtonDialogResultDescr";

		// Token: 0x040000A0 RID: 160
		internal const string ButtonFlatAppearance = "ButtonFlatAppearance";

		// Token: 0x040000A1 RID: 161
		internal const string ButtonFlatAppearanceInvalidBorderColor = "ButtonFlatAppearanceInvalidBorderColor";

		// Token: 0x040000A2 RID: 162
		internal const string ButtonFlatStyleDescr = "ButtonFlatStyleDescr";

		// Token: 0x040000A3 RID: 163
		internal const string ButtonImageAlignDescr = "ButtonImageAlignDescr";

		// Token: 0x040000A4 RID: 164
		internal const string ButtonImageDescr = "ButtonImageDescr";

		// Token: 0x040000A5 RID: 165
		internal const string ButtonImageIndexDescr = "ButtonImageIndexDescr";

		// Token: 0x040000A6 RID: 166
		internal const string ButtonImageListDescr = "ButtonImageListDescr";

		// Token: 0x040000A7 RID: 167
		internal const string ButtonMouseDownBackColorDescr = "ButtonMouseDownBackColorDescr";

		// Token: 0x040000A8 RID: 168
		internal const string ButtonMouseOverBackColorDescr = "ButtonMouseOverBackColorDescr";

		// Token: 0x040000A9 RID: 169
		internal const string ButtonTextAlignDescr = "ButtonTextAlignDescr";

		// Token: 0x040000AA RID: 170
		internal const string ButtonTextImageRelationDescr = "ButtonTextImageRelationDescr";

		// Token: 0x040000AB RID: 171
		internal const string ButtonUseMnemonicDescr = "ButtonUseMnemonicDescr";

		// Token: 0x040000AC RID: 172
		internal const string ButtonUseVisualStyleBackColorDescr = "ButtonUseVisualStyleBackColorDescr";

		// Token: 0x040000AD RID: 173
		internal const string CancelCaption = "CancelCaption";

		// Token: 0x040000AE RID: 174
		internal const string CannotActivateControl = "CannotActivateControl";

		// Token: 0x040000AF RID: 175
		internal const string CannotChangePrintedDocument = "CannotChangePrintedDocument";

		// Token: 0x040000B0 RID: 176
		internal const string CannotConvertDoubleToDate = "CannotConvertDoubleToDate";

		// Token: 0x040000B1 RID: 177
		internal const string CannotConvertIntToFloat = "CannotConvertIntToFloat";

		// Token: 0x040000B2 RID: 178
		internal const string CantNestMessageLoops = "CantNestMessageLoops";

		// Token: 0x040000B3 RID: 179
		internal const string CantShowMBServiceWithHelp = "CantShowMBServiceWithHelp";

		// Token: 0x040000B4 RID: 180
		internal const string CantShowMBServiceWithOwner = "CantShowMBServiceWithOwner";

		// Token: 0x040000B5 RID: 181
		internal const string CantShowModalOnNonInteractive = "CantShowModalOnNonInteractive";

		// Token: 0x040000B6 RID: 182
		internal const string CatAccessibility = "CatAccessibility";

		// Token: 0x040000B7 RID: 183
		internal const string CatAction = "CatAction";

		// Token: 0x040000B8 RID: 184
		internal const string CatAppearance = "CatAppearance";

		// Token: 0x040000B9 RID: 185
		internal const string CatAsynchronous = "CatAsynchronous";

		// Token: 0x040000BA RID: 186
		internal const string CatBehavior = "CatBehavior";

		// Token: 0x040000BB RID: 187
		internal const string CatColors = "CatColors";

		// Token: 0x040000BC RID: 188
		internal const string CatData = "CatData";

		// Token: 0x040000BD RID: 189
		internal const string CatDisplay = "CatDisplay";

		// Token: 0x040000BE RID: 190
		internal const string CatDragDrop = "CatDragDrop";

		// Token: 0x040000BF RID: 191
		internal const string CatFocus = "CatFocus";

		// Token: 0x040000C0 RID: 192
		internal const string CatFolderBrowsing = "CatFolderBrowsing";

		// Token: 0x040000C1 RID: 193
		internal const string CatItems = "CatItems";

		// Token: 0x040000C2 RID: 194
		internal const string CatKey = "CatKey";

		// Token: 0x040000C3 RID: 195
		internal const string CatLayout = "CatLayout";

		// Token: 0x040000C4 RID: 196
		internal const string CatMouse = "CatMouse";

		// Token: 0x040000C5 RID: 197
		internal const string CatPrivate = "CatPrivate";

		// Token: 0x040000C6 RID: 198
		internal const string CatPropertyChanged = "CatPropertyChanged";

		// Token: 0x040000C7 RID: 199
		internal const string CatWindowStyle = "CatWindowStyle";

		// Token: 0x040000C8 RID: 200
		internal const string CDallowFullOpenDescr = "CDallowFullOpenDescr";

		// Token: 0x040000C9 RID: 201
		internal const string CDanyColorDescr = "CDanyColorDescr";

		// Token: 0x040000CA RID: 202
		internal const string CDcolorDescr = "CDcolorDescr";

		// Token: 0x040000CB RID: 203
		internal const string CDcustomColorsDescr = "CDcustomColorsDescr";

		// Token: 0x040000CC RID: 204
		internal const string CDfullOpenDescr = "CDfullOpenDescr";

		// Token: 0x040000CD RID: 205
		internal const string CDshowHelpDescr = "CDshowHelpDescr";

		// Token: 0x040000CE RID: 206
		internal const string CDsolidColorOnlyDescr = "CDsolidColorOnlyDescr";

		// Token: 0x040000CF RID: 207
		internal const string CheckBoxAppearanceDescr = "CheckBoxAppearanceDescr";

		// Token: 0x040000D0 RID: 208
		internal const string CheckBoxAutoCheckDescr = "CheckBoxAutoCheckDescr";

		// Token: 0x040000D1 RID: 209
		internal const string CheckBoxCheckAlignDescr = "CheckBoxCheckAlignDescr";

		// Token: 0x040000D2 RID: 210
		internal const string CheckBoxCheckedDescr = "CheckBoxCheckedDescr";

		// Token: 0x040000D3 RID: 211
		internal const string CheckBoxCheckStateDescr = "CheckBoxCheckStateDescr";

		// Token: 0x040000D4 RID: 212
		internal const string CheckBoxOnAppearanceChangedDescr = "CheckBoxOnAppearanceChangedDescr";

		// Token: 0x040000D5 RID: 213
		internal const string CheckBoxOnCheckedChangedDescr = "CheckBoxOnCheckedChangedDescr";

		// Token: 0x040000D6 RID: 214
		internal const string CheckBoxOnCheckStateChangedDescr = "CheckBoxOnCheckStateChangedDescr";

		// Token: 0x040000D7 RID: 215
		internal const string CheckBoxThreeStateDescr = "CheckBoxThreeStateDescr";

		// Token: 0x040000D8 RID: 216
		internal const string CheckedListBoxCheckedIndexCollectionIsReadOnly = "CheckedListBoxCheckedIndexCollectionIsReadOnly";

		// Token: 0x040000D9 RID: 217
		internal const string CheckedListBoxCheckedItemCollectionIsReadOnly = "CheckedListBoxCheckedItemCollectionIsReadOnly";

		// Token: 0x040000DA RID: 218
		internal const string CheckedListBoxCheckOnClickDescr = "CheckedListBoxCheckOnClickDescr";

		// Token: 0x040000DB RID: 219
		internal const string CheckedListBoxInvalidSelectionMode = "CheckedListBoxInvalidSelectionMode";

		// Token: 0x040000DC RID: 220
		internal const string CheckedListBoxItemCheckDescr = "CheckedListBoxItemCheckDescr";

		// Token: 0x040000DD RID: 221
		internal const string CheckedListBoxThreeDCheckBoxesDescr = "CheckedListBoxThreeDCheckBoxesDescr";

		// Token: 0x040000DE RID: 222
		internal const string CircularOwner = "CircularOwner";

		// Token: 0x040000DF RID: 223
		internal const string Clipboard_InvalidPath = "Clipboard_InvalidPath";

		// Token: 0x040000E0 RID: 224
		internal const string ClipboardOperationFailed = "ClipboardOperationFailed";

		// Token: 0x040000E1 RID: 225
		internal const string ClipboardSecurityException = "ClipboardSecurityException";

		// Token: 0x040000E2 RID: 226
		internal const string CloseCaption = "CloseCaption";

		// Token: 0x040000E3 RID: 227
		internal const string ClosingWhileCreatingHandle = "ClosingWhileCreatingHandle";

		// Token: 0x040000E4 RID: 228
		internal const string collectionChangedEventDescr = "collectionChangedEventDescr";

		// Token: 0x040000E5 RID: 229
		internal const string collectionChangingEventDescr = "collectionChangingEventDescr";

		// Token: 0x040000E6 RID: 230
		internal const string CollectionEmptyException = "CollectionEmptyException";

		// Token: 0x040000E7 RID: 231
		internal const string ColumnAlignment = "ColumnAlignment";

		// Token: 0x040000E8 RID: 232
		internal const string ColumnCaption = "ColumnCaption";

		// Token: 0x040000E9 RID: 233
		internal const string ColumnHeaderBadDisplayIndex = "ColumnHeaderBadDisplayIndex";

		// Token: 0x040000EA RID: 234
		internal const string ColumnHeaderCollectionInvalidArgument = "ColumnHeaderCollectionInvalidArgument";

		// Token: 0x040000EB RID: 235
		internal const string ColumnHeaderDisplayIndexDescr = "ColumnHeaderDisplayIndexDescr";

		// Token: 0x040000EC RID: 236
		internal const string ColumnHeaderNameDescr = "ColumnHeaderNameDescr";

		// Token: 0x040000ED RID: 237
		internal const string ColumnWidth = "ColumnWidth";

		// Token: 0x040000EE RID: 238
		internal const string COM2BadHandlerType = "COM2BadHandlerType";

		// Token: 0x040000EF RID: 239
		internal const string COM2NamesAndValuesNotEqual = "COM2NamesAndValuesNotEqual";

		// Token: 0x040000F0 RID: 240
		internal const string COM2ReadonlyProperty = "COM2ReadonlyProperty";

		// Token: 0x040000F1 RID: 241
		internal const string COM2UnhandledVT = "COM2UnhandledVT";

		// Token: 0x040000F2 RID: 242
		internal const string ComboBoxAutoCompleteCustomSourceDescr = "ComboBoxAutoCompleteCustomSourceDescr";

		// Token: 0x040000F3 RID: 243
		internal const string ComboBoxAutoCompleteModeDescr = "ComboBoxAutoCompleteModeDescr";

		// Token: 0x040000F4 RID: 244
		internal const string ComboBoxAutoCompleteModeOnlyNoneAllowed = "ComboBoxAutoCompleteModeOnlyNoneAllowed";

		// Token: 0x040000F5 RID: 245
		internal const string ComboBoxAutoCompleteSourceDescr = "ComboBoxAutoCompleteSourceDescr";

		// Token: 0x040000F6 RID: 246
		internal const string ComboBoxAutoCompleteSourceOnlyListItemsAllowed = "ComboBoxAutoCompleteSourceOnlyListItemsAllowed";

		// Token: 0x040000F7 RID: 247
		internal const string ComboBoxDataSourceWithSort = "ComboBoxDataSourceWithSort";

		// Token: 0x040000F8 RID: 248
		internal const string ComboBoxDrawModeDescr = "ComboBoxDrawModeDescr";

		// Token: 0x040000F9 RID: 249
		internal const string ComboBoxDropDownHeightDescr = "ComboBoxDropDownHeightDescr";

		// Token: 0x040000FA RID: 250
		internal const string ComboBoxDropDownStyleChangedDescr = "ComboBoxDropDownStyleChangedDescr";

		// Token: 0x040000FB RID: 251
		internal const string ComboBoxDropDownWidthDescr = "ComboBoxDropDownWidthDescr";

		// Token: 0x040000FC RID: 252
		internal const string ComboBoxDroppedDownDescr = "ComboBoxDroppedDownDescr";

		// Token: 0x040000FD RID: 253
		internal const string ComboBoxFlatStyleDescr = "ComboBoxFlatStyleDescr";

		// Token: 0x040000FE RID: 254
		internal const string ComboBoxIntegralHeightDescr = "ComboBoxIntegralHeightDescr";

		// Token: 0x040000FF RID: 255
		internal const string ComboBoxItemHeightDescr = "ComboBoxItemHeightDescr";

		// Token: 0x04000100 RID: 256
		internal const string ComboBoxItemOverflow = "ComboBoxItemOverflow";

		// Token: 0x04000101 RID: 257
		internal const string ComboBoxItemsDescr = "ComboBoxItemsDescr";

		// Token: 0x04000102 RID: 258
		internal const string ComboBoxMaxDropDownItemsDescr = "ComboBoxMaxDropDownItemsDescr";

		// Token: 0x04000103 RID: 259
		internal const string ComboBoxMaxLengthDescr = "ComboBoxMaxLengthDescr";

		// Token: 0x04000104 RID: 260
		internal const string ComboBoxOnDropDownClosedDescr = "ComboBoxOnDropDownClosedDescr";

		// Token: 0x04000105 RID: 261
		internal const string ComboBoxOnDropDownDescr = "ComboBoxOnDropDownDescr";

		// Token: 0x04000106 RID: 262
		internal const string ComboBoxOnTextUpdateDescr = "ComboBoxOnTextUpdateDescr";

		// Token: 0x04000107 RID: 263
		internal const string ComboBoxPreferredHeightDescr = "ComboBoxPreferredHeightDescr";

		// Token: 0x04000108 RID: 264
		internal const string ComboBoxSelectedIndexDescr = "ComboBoxSelectedIndexDescr";

		// Token: 0x04000109 RID: 265
		internal const string ComboBoxSelectedItemDescr = "ComboBoxSelectedItemDescr";

		// Token: 0x0400010A RID: 266
		internal const string ComboBoxSelectedTextDescr = "ComboBoxSelectedTextDescr";

		// Token: 0x0400010B RID: 267
		internal const string ComboBoxSelectionLengthDescr = "ComboBoxSelectionLengthDescr";

		// Token: 0x0400010C RID: 268
		internal const string ComboBoxSelectionStartDescr = "ComboBoxSelectionStartDescr";

		// Token: 0x0400010D RID: 269
		internal const string ComboBoxSortedDescr = "ComboBoxSortedDescr";

		// Token: 0x0400010E RID: 270
		internal const string ComboBoxSortWithDataSource = "ComboBoxSortWithDataSource";

		// Token: 0x0400010F RID: 271
		internal const string ComboBoxStyleDescr = "ComboBoxStyleDescr";

		// Token: 0x04000110 RID: 272
		internal const string CommandIdNotAllocated = "CommandIdNotAllocated";

		// Token: 0x04000111 RID: 273
		internal const string CommonDialogHelpRequested = "CommonDialogHelpRequested";

		// Token: 0x04000112 RID: 274
		internal const string ComponentEditorFormBadComponent = "ComponentEditorFormBadComponent";

		// Token: 0x04000113 RID: 275
		internal const string ComponentEditorFormProperties = "ComponentEditorFormProperties";

		// Token: 0x04000114 RID: 276
		internal const string ComponentEditorFormPropertiesNoName = "ComponentEditorFormPropertiesNoName";

		// Token: 0x04000115 RID: 277
		internal const string ComponentManagerProxyOutOfMemory = "ComponentManagerProxyOutOfMemory";

		// Token: 0x04000116 RID: 278
		internal const string Config_base_unrecognized_attribute = "Config_base_unrecognized_attribute";

		// Token: 0x04000117 RID: 279
		internal const string ConnPointAdviseFailed = "ConnPointAdviseFailed";

		// Token: 0x04000118 RID: 280
		internal const string ConnPointCouldNotCreate = "ConnPointCouldNotCreate";

		// Token: 0x04000119 RID: 281
		internal const string ConnPointSinkIF = "ConnPointSinkIF";

		// Token: 0x0400011A RID: 282
		internal const string ConnPointSourceIF = "ConnPointSourceIF";

		// Token: 0x0400011B RID: 283
		internal const string ConnPointUnhandledType = "ConnPointUnhandledType";

		// Token: 0x0400011C RID: 284
		internal const string ContainerControlActiveControlDescr = "ContainerControlActiveControlDescr";

		// Token: 0x0400011D RID: 285
		internal const string ContainerControlAutoScaleModeDescr = "ContainerControlAutoScaleModeDescr";

		// Token: 0x0400011E RID: 286
		internal const string ContainerControlAutoValidate = "ContainerControlAutoValidate";

		// Token: 0x0400011F RID: 287
		internal const string ContainerControlBindingContextDescr = "ContainerControlBindingContextDescr";

		// Token: 0x04000120 RID: 288
		internal const string ContainerControlInvalidAutoScaleDimensions = "ContainerControlInvalidAutoScaleDimensions";

		// Token: 0x04000121 RID: 289
		internal const string ContainerControlOnAutoValidateChangedDescr = "ContainerControlOnAutoValidateChangedDescr";

		// Token: 0x04000122 RID: 290
		internal const string ContainerControlParentFormDescr = "ContainerControlParentFormDescr";

		// Token: 0x04000123 RID: 291
		internal const string ContextMenuCollapseDescr = "ContextMenuCollapseDescr";

		// Token: 0x04000124 RID: 292
		internal const string ContextMenuImageListDescr = "ContextMenuImageListDescr";

		// Token: 0x04000125 RID: 293
		internal const string ContextMenuInvalidParent = "ContextMenuInvalidParent";

		// Token: 0x04000126 RID: 294
		internal const string ContextMenuIsImageMarginPresentDescr = "ContextMenuIsImageMarginPresentDescr";

		// Token: 0x04000127 RID: 295
		internal const string ContextMenuSourceControlDescr = "ContextMenuSourceControlDescr";

		// Token: 0x04000128 RID: 296
		internal const string ContextMenuStripSourceControlDescr = "ContextMenuStripSourceControlDescr";

		// Token: 0x04000129 RID: 297
		internal const string ControlAccessibileObjectInvalid = "ControlAccessibileObjectInvalid";

		// Token: 0x0400012A RID: 298
		internal const string ControlAccessibilityObjectDescr = "ControlAccessibilityObjectDescr";

		// Token: 0x0400012B RID: 299
		internal const string ControlAccessibleDefaultActionDescr = "ControlAccessibleDefaultActionDescr";

		// Token: 0x0400012C RID: 300
		internal const string ControlAccessibleDescriptionDescr = "ControlAccessibleDescriptionDescr";

		// Token: 0x0400012D RID: 301
		internal const string ControlAccessibleNameDescr = "ControlAccessibleNameDescr";

		// Token: 0x0400012E RID: 302
		internal const string ControlAccessibleRoleDescr = "ControlAccessibleRoleDescr";

		// Token: 0x0400012F RID: 303
		internal const string ControlAllowDropDescr = "ControlAllowDropDescr";

		// Token: 0x04000130 RID: 304
		internal const string ControlAllowTransparencyDescr = "ControlAllowTransparencyDescr";

		// Token: 0x04000131 RID: 305
		internal const string ControlAnchorDescr = "ControlAnchorDescr";

		// Token: 0x04000132 RID: 306
		internal const string ControlArrayCannotAddComponentArray = "ControlArrayCannotAddComponentArray";

		// Token: 0x04000133 RID: 307
		internal const string ControlArrayCannotPerformAddCopy = "ControlArrayCannotPerformAddCopy";

		// Token: 0x04000134 RID: 308
		internal const string ControlArrayCloningException = "ControlArrayCloningException";

		// Token: 0x04000135 RID: 309
		internal const string ControlArrayDuplicateException = "ControlArrayDuplicateException";

		// Token: 0x04000136 RID: 310
		internal const string ControlArrayValidationException = "ControlArrayValidationException";

		// Token: 0x04000137 RID: 311
		internal const string ControlAutoRelocateDescr = "ControlAutoRelocateDescr";

		// Token: 0x04000138 RID: 312
		internal const string ControlAutoSizeDescr = "ControlAutoSizeDescr";

		// Token: 0x04000139 RID: 313
		internal const string ControlAutoSizeModeDescr = "ControlAutoSizeModeDescr";

		// Token: 0x0400013A RID: 314
		internal const string ControlBackColorDescr = "ControlBackColorDescr";

		// Token: 0x0400013B RID: 315
		internal const string ControlBackgroundImageDescr = "ControlBackgroundImageDescr";

		// Token: 0x0400013C RID: 316
		internal const string ControlBackgroundImageLayoutDescr = "ControlBackgroundImageLayoutDescr";

		// Token: 0x0400013D RID: 317
		internal const string ControlBadAsyncResult = "ControlBadAsyncResult";

		// Token: 0x0400013E RID: 318
		internal const string ControlBadControl = "ControlBadControl";

		// Token: 0x0400013F RID: 319
		internal const string ControlBindingContextDescr = "ControlBindingContextDescr";

		// Token: 0x04000140 RID: 320
		internal const string ControlBindingsDescr = "ControlBindingsDescr";

		// Token: 0x04000141 RID: 321
		internal const string ControlBottomDescr = "ControlBottomDescr";

		// Token: 0x04000142 RID: 322
		internal const string ControlBoundsDescr = "ControlBoundsDescr";

		// Token: 0x04000143 RID: 323
		internal const string ControlCanFocusDescr = "ControlCanFocusDescr";

		// Token: 0x04000144 RID: 324
		internal const string ControlCannotBeNull = "ControlCannotBeNull";

		// Token: 0x04000145 RID: 325
		internal const string ControlCanSelectDescr = "ControlCanSelectDescr";

		// Token: 0x04000146 RID: 326
		internal const string ControlCaptureDescr = "ControlCaptureDescr";

		// Token: 0x04000147 RID: 327
		internal const string ControlCausesValidationDescr = "ControlCausesValidationDescr";

		// Token: 0x04000148 RID: 328
		internal const string ControlCheckForIllegalCrossThreadCalls = "ControlCheckForIllegalCrossThreadCalls";

		// Token: 0x04000149 RID: 329
		internal const string ControlClientRectangleDescr = "ControlClientRectangleDescr";

		// Token: 0x0400014A RID: 330
		internal const string ControlClientSizeDescr = "ControlClientSizeDescr";

		// Token: 0x0400014B RID: 331
		internal const string ControlCompanyNameDescr = "ControlCompanyNameDescr";

		// Token: 0x0400014C RID: 332
		internal const string ControlContainsFocusDescr = "ControlContainsFocusDescr";

		// Token: 0x0400014D RID: 333
		internal const string ControlContextMenuDescr = "ControlContextMenuDescr";

		// Token: 0x0400014E RID: 334
		internal const string ControlContextMenuStripChangedDescr = "ControlContextMenuStripChangedDescr";

		// Token: 0x0400014F RID: 335
		internal const string ControlControlsDescr = "ControlControlsDescr";

		// Token: 0x04000150 RID: 336
		internal const string ControlCreatedDescr = "ControlCreatedDescr";

		// Token: 0x04000151 RID: 337
		internal const string ControlCursorDescr = "ControlCursorDescr";

		// Token: 0x04000152 RID: 338
		internal const string ControlDisplayRectangleDescr = "ControlDisplayRectangleDescr";

		// Token: 0x04000153 RID: 339
		internal const string ControlDisposedDescr = "ControlDisposedDescr";

		// Token: 0x04000154 RID: 340
		internal const string ControlDisposingDescr = "ControlDisposingDescr";

		// Token: 0x04000155 RID: 341
		internal const string ControlDockDescr = "ControlDockDescr";

		// Token: 0x04000156 RID: 342
		internal const string ControlDoubleBufferedDescr = "ControlDoubleBufferedDescr";

		// Token: 0x04000157 RID: 343
		internal const string ControlEnabledDescr = "ControlEnabledDescr";

		// Token: 0x04000158 RID: 344
		internal const string ControlFocusedDescr = "ControlFocusedDescr";

		// Token: 0x04000159 RID: 345
		internal const string ControlFontDescr = "ControlFontDescr";

		// Token: 0x0400015A RID: 346
		internal const string ControlForeColorDescr = "ControlForeColorDescr";

		// Token: 0x0400015B RID: 347
		internal const string ControlHandleCreatedDescr = "ControlHandleCreatedDescr";

		// Token: 0x0400015C RID: 348
		internal const string ControlHandleDescr = "ControlHandleDescr";

		// Token: 0x0400015D RID: 349
		internal const string ControlHasChildrenDescr = "ControlHasChildrenDescr";

		// Token: 0x0400015E RID: 350
		internal const string ControlHeightDescr = "ControlHeightDescr";

		// Token: 0x0400015F RID: 351
		internal const string ControlIMEModeDescr = "ControlIMEModeDescr";

		// Token: 0x04000160 RID: 352
		internal const string ControlInvalidLastScalingFactor = "ControlInvalidLastScalingFactor";

		// Token: 0x04000161 RID: 353
		internal const string ControlInvokeRequiredDescr = "ControlInvokeRequiredDescr";

		// Token: 0x04000162 RID: 354
		internal const string ControlIsAccessibleDescr = "ControlIsAccessibleDescr";

		// Token: 0x04000163 RID: 355
		internal const string ControlIsKeyLockedNumCapsScrollLockKeysSupportedOnly = "ControlIsKeyLockedNumCapsScrollLockKeysSupportedOnly";

		// Token: 0x04000164 RID: 356
		internal const string ControlLeftDescr = "ControlLeftDescr";

		// Token: 0x04000165 RID: 357
		internal const string ControlLocationDescr = "ControlLocationDescr";

		// Token: 0x04000166 RID: 358
		internal const string ControlMarginDescr = "ControlMarginDescr";

		// Token: 0x04000167 RID: 359
		internal const string ControlMaximumSizeDescr = "ControlMaximumSizeDescr";

		// Token: 0x04000168 RID: 360
		internal const string ControlMetaFileDCWrapperSizeInvalid = "ControlMetaFileDCWrapperSizeInvalid";

		// Token: 0x04000169 RID: 361
		internal const string ControlMinimumSizeDescr = "ControlMinimumSizeDescr";

		// Token: 0x0400016A RID: 362
		internal const string ControlNotChild = "ControlNotChild";

		// Token: 0x0400016B RID: 363
		internal const string ControlOnAutoSizeChangedDescr = "ControlOnAutoSizeChangedDescr";

		// Token: 0x0400016C RID: 364
		internal const string ControlOnBackColorChangedDescr = "ControlOnBackColorChangedDescr";

		// Token: 0x0400016D RID: 365
		internal const string ControlOnBackgroundImageChangedDescr = "ControlOnBackgroundImageChangedDescr";

		// Token: 0x0400016E RID: 366
		internal const string ControlOnBackgroundImageLayoutChangedDescr = "ControlOnBackgroundImageLayoutChangedDescr";

		// Token: 0x0400016F RID: 367
		internal const string ControlOnBindingContextChangedDescr = "ControlOnBindingContextChangedDescr";

		// Token: 0x04000170 RID: 368
		internal const string ControlOnCausesValidationChangedDescr = "ControlOnCausesValidationChangedDescr";

		// Token: 0x04000171 RID: 369
		internal const string ControlOnChangeUICuesDescr = "ControlOnChangeUICuesDescr";

		// Token: 0x04000172 RID: 370
		internal const string ControlOnClickDescr = "ControlOnClickDescr";

		// Token: 0x04000173 RID: 371
		internal const string ControlOnClientSizeChangedDescr = "ControlOnClientSizeChangedDescr";

		// Token: 0x04000174 RID: 372
		internal const string ControlOnContextMenuChangedDescr = "ControlOnContextMenuChangedDescr";

		// Token: 0x04000175 RID: 373
		internal const string ControlOnControlAddedDescr = "ControlOnControlAddedDescr";

		// Token: 0x04000176 RID: 374
		internal const string ControlOnControlRemovedDescr = "ControlOnControlRemovedDescr";

		// Token: 0x04000177 RID: 375
		internal const string ControlOnCreateHandleDescr = "ControlOnCreateHandleDescr";

		// Token: 0x04000178 RID: 376
		internal const string ControlOnCursorChangedDescr = "ControlOnCursorChangedDescr";

		// Token: 0x04000179 RID: 377
		internal const string ControlOnDestroyHandleDescr = "ControlOnDestroyHandleDescr";

		// Token: 0x0400017A RID: 378
		internal const string ControlOnDockChangedDescr = "ControlOnDockChangedDescr";

		// Token: 0x0400017B RID: 379
		internal const string ControlOnDoubleClickDescr = "ControlOnDoubleClickDescr";

		// Token: 0x0400017C RID: 380
		internal const string ControlOnDragDropDescr = "ControlOnDragDropDescr";

		// Token: 0x0400017D RID: 381
		internal const string ControlOnDragEnterDescr = "ControlOnDragEnterDescr";

		// Token: 0x0400017E RID: 382
		internal const string ControlOnDragLeaveDescr = "ControlOnDragLeaveDescr";

		// Token: 0x0400017F RID: 383
		internal const string ControlOnDragOverDescr = "ControlOnDragOverDescr";

		// Token: 0x04000180 RID: 384
		internal const string ControlOnEnabledChangedDescr = "ControlOnEnabledChangedDescr";

		// Token: 0x04000181 RID: 385
		internal const string ControlOnEnterDescr = "ControlOnEnterDescr";

		// Token: 0x04000182 RID: 386
		internal const string ControlOnFontChangedDescr = "ControlOnFontChangedDescr";

		// Token: 0x04000183 RID: 387
		internal const string ControlOnForeColorChangedDescr = "ControlOnForeColorChangedDescr";

		// Token: 0x04000184 RID: 388
		internal const string ControlOnGiveFeedbackDescr = "ControlOnGiveFeedbackDescr";

		// Token: 0x04000185 RID: 389
		internal const string ControlOnGotFocusDescr = "ControlOnGotFocusDescr";

		// Token: 0x04000186 RID: 390
		internal const string ControlOnHelpDescr = "ControlOnHelpDescr";

		// Token: 0x04000187 RID: 391
		internal const string ControlOnImeModeChangedDescr = "ControlOnImeModeChangedDescr";

		// Token: 0x04000188 RID: 392
		internal const string ControlOnInvalidateDescr = "ControlOnInvalidateDescr";

		// Token: 0x04000189 RID: 393
		internal const string ControlOnKeyDownDescr = "ControlOnKeyDownDescr";

		// Token: 0x0400018A RID: 394
		internal const string ControlOnKeyPressDescr = "ControlOnKeyPressDescr";

		// Token: 0x0400018B RID: 395
		internal const string ControlOnKeyUpDescr = "ControlOnKeyUpDescr";

		// Token: 0x0400018C RID: 396
		internal const string ControlOnLayoutDescr = "ControlOnLayoutDescr";

		// Token: 0x0400018D RID: 397
		internal const string ControlOnLeaveDescr = "ControlOnLeaveDescr";

		// Token: 0x0400018E RID: 398
		internal const string ControlOnLocationChangedDescr = "ControlOnLocationChangedDescr";

		// Token: 0x0400018F RID: 399
		internal const string ControlOnLostFocusDescr = "ControlOnLostFocusDescr";

		// Token: 0x04000190 RID: 400
		internal const string ControlOnMarginChangedDescr = "ControlOnMarginChangedDescr";

		// Token: 0x04000191 RID: 401
		internal const string ControlOnMouseCaptureChangedDescr = "ControlOnMouseCaptureChangedDescr";

		// Token: 0x04000192 RID: 402
		internal const string ControlOnMouseClickDescr = "ControlOnMouseClickDescr";

		// Token: 0x04000193 RID: 403
		internal const string ControlOnMouseDoubleClickDescr = "ControlOnMouseDoubleClickDescr";

		// Token: 0x04000194 RID: 404
		internal const string ControlOnMouseDownDescr = "ControlOnMouseDownDescr";

		// Token: 0x04000195 RID: 405
		internal const string ControlOnMouseEnterDescr = "ControlOnMouseEnterDescr";

		// Token: 0x04000196 RID: 406
		internal const string ControlOnMouseHoverDescr = "ControlOnMouseHoverDescr";

		// Token: 0x04000197 RID: 407
		internal const string ControlOnMouseLeaveDescr = "ControlOnMouseLeaveDescr";

		// Token: 0x04000198 RID: 408
		internal const string ControlOnMouseMoveDescr = "ControlOnMouseMoveDescr";

		// Token: 0x04000199 RID: 409
		internal const string ControlOnMouseUpDescr = "ControlOnMouseUpDescr";

		// Token: 0x0400019A RID: 410
		internal const string ControlOnMouseWheelDescr = "ControlOnMouseWheelDescr";

		// Token: 0x0400019B RID: 411
		internal const string ControlOnMoveDescr = "ControlOnMoveDescr";

		// Token: 0x0400019C RID: 412
		internal const string ControlOnPaddingChangedDescr = "ControlOnPaddingChangedDescr";

		// Token: 0x0400019D RID: 413
		internal const string ControlOnPaintDescr = "ControlOnPaintDescr";

		// Token: 0x0400019E RID: 414
		internal const string ControlOnParentChangedDescr = "ControlOnParentChangedDescr";

		// Token: 0x0400019F RID: 415
		internal const string ControlOnQueryAccessibilityHelpDescr = "ControlOnQueryAccessibilityHelpDescr";

		// Token: 0x040001A0 RID: 416
		internal const string ControlOnQueryContinueDragDescr = "ControlOnQueryContinueDragDescr";

		// Token: 0x040001A1 RID: 417
		internal const string ControlOnResizeBeginDescr = "ControlOnResizeBeginDescr";

		// Token: 0x040001A2 RID: 418
		internal const string ControlOnResizeDescr = "ControlOnResizeDescr";

		// Token: 0x040001A3 RID: 419
		internal const string ControlOnResizeEndDescr = "ControlOnResizeEndDescr";

		// Token: 0x040001A4 RID: 420
		internal const string ControlOnRightToLeftChangedDescr = "ControlOnRightToLeftChangedDescr";

		// Token: 0x040001A5 RID: 421
		internal const string ControlOnRightToLeftLayoutChangedDescr = "ControlOnRightToLeftLayoutChangedDescr";

		// Token: 0x040001A6 RID: 422
		internal const string ControlOnSizeChangedDescr = "ControlOnSizeChangedDescr";

		// Token: 0x040001A7 RID: 423
		internal const string ControlOnStyleChangedDescr = "ControlOnStyleChangedDescr";

		// Token: 0x040001A8 RID: 424
		internal const string ControlOnSystemColorsChangedDescr = "ControlOnSystemColorsChangedDescr";

		// Token: 0x040001A9 RID: 425
		internal const string ControlOnTabIndexChangedDescr = "ControlOnTabIndexChangedDescr";

		// Token: 0x040001AA RID: 426
		internal const string ControlOnTabStopChangedDescr = "ControlOnTabStopChangedDescr";

		// Token: 0x040001AB RID: 427
		internal const string ControlOnTextChangedDescr = "ControlOnTextChangedDescr";

		// Token: 0x040001AC RID: 428
		internal const string ControlOnValidatedDescr = "ControlOnValidatedDescr";

		// Token: 0x040001AD RID: 429
		internal const string ControlOnValidatingDescr = "ControlOnValidatingDescr";

		// Token: 0x040001AE RID: 430
		internal const string ControlOnVisibleChangedDescr = "ControlOnVisibleChangedDescr";

		// Token: 0x040001AF RID: 431
		internal const string ControlPaddingDescr = "ControlPaddingDescr";

		// Token: 0x040001B0 RID: 432
		internal const string ControlParentDescr = "ControlParentDescr";

		// Token: 0x040001B1 RID: 433
		internal const string ControlProductNameDescr = "ControlProductNameDescr";

		// Token: 0x040001B2 RID: 434
		internal const string ControlProductVersionDescr = "ControlProductVersionDescr";

		// Token: 0x040001B3 RID: 435
		internal const string ControlRecreatingHandleDescr = "ControlRecreatingHandleDescr";

		// Token: 0x040001B4 RID: 436
		internal const string ControlRegionChangedDescr = "ControlRegionChangedDescr";

		// Token: 0x040001B5 RID: 437
		internal const string ControlRegionDescr = "ControlRegionDescr";

		// Token: 0x040001B6 RID: 438
		internal const string ControlResizeRedrawDescr = "ControlResizeRedrawDescr";

		// Token: 0x040001B7 RID: 439
		internal const string ControlRightDescr = "ControlRightDescr";

		// Token: 0x040001B8 RID: 440
		internal const string ControlRightToLeftDescr = "ControlRightToLeftDescr";

		// Token: 0x040001B9 RID: 441
		internal const string ControlRightToLeftLayoutDescr = "ControlRightToLeftLayoutDescr";

		// Token: 0x040001BA RID: 442
		internal const string ControlSizeDescr = "ControlSizeDescr";

		// Token: 0x040001BB RID: 443
		internal const string ControlTabIndexDescr = "ControlTabIndexDescr";

		// Token: 0x040001BC RID: 444
		internal const string ControlTabStopDescr = "ControlTabStopDescr";

		// Token: 0x040001BD RID: 445
		internal const string ControlTagDescr = "ControlTagDescr";

		// Token: 0x040001BE RID: 446
		internal const string ControlTextDescr = "ControlTextDescr";

		// Token: 0x040001BF RID: 447
		internal const string ControlTopDescr = "ControlTopDescr";

		// Token: 0x040001C0 RID: 448
		internal const string ControlTopLevelControlDescr = "ControlTopLevelControlDescr";

		// Token: 0x040001C1 RID: 449
		internal const string ControlUnsupportedProperty = "ControlUnsupportedProperty";

		// Token: 0x040001C2 RID: 450
		internal const string ControlUserPreferenceChangedDescr = "ControlUserPreferenceChangedDescr";

		// Token: 0x040001C3 RID: 451
		internal const string ControlUserPreferenceChangingDescr = "ControlUserPreferenceChangingDescr";

		// Token: 0x040001C4 RID: 452
		internal const string ControlUseWaitCursorDescr = "ControlUseWaitCursorDescr";

		// Token: 0x040001C5 RID: 453
		internal const string ControlVisibleDescr = "ControlVisibleDescr";

		// Token: 0x040001C6 RID: 454
		internal const string ControlWidthDescr = "ControlWidthDescr";

		// Token: 0x040001C7 RID: 455
		internal const string ControlWindowTargetDescr = "ControlWindowTargetDescr";

		// Token: 0x040001C8 RID: 456
		internal const string ControlWithScrollbarsPositionDescr = "ControlWithScrollbarsPositionDescr";

		// Token: 0x040001C9 RID: 457
		internal const string ControlWithScrollbarsVirtualSizeDescr = "ControlWithScrollbarsVirtualSizeDescr";

		// Token: 0x040001CA RID: 458
		internal const string CurrencyManagerCantAddNew = "CurrencyManagerCantAddNew";

		// Token: 0x040001CB RID: 459
		internal const string CursorCannotCovertToBytes = "CursorCannotCovertToBytes";

		// Token: 0x040001CC RID: 460
		internal const string CursorCannotCovertToString = "CursorCannotCovertToString";

		// Token: 0x040001CD RID: 461
		internal const string CursorNonSerializableHandle = "CursorNonSerializableHandle";

		// Token: 0x040001CE RID: 462
		internal const string DataBindingAddNewNotSupportedOnPropertyManager = "DataBindingAddNewNotSupportedOnPropertyManager";

		// Token: 0x040001CF RID: 463
		internal const string DataBindingCycle = "DataBindingCycle";

		// Token: 0x040001D0 RID: 464
		internal const string DataBindingPushDataException = "DataBindingPushDataException";

		// Token: 0x040001D1 RID: 465
		internal const string DataBindingRemoveAtNotSupportedOnPropertyManager = "DataBindingRemoveAtNotSupportedOnPropertyManager";

		// Token: 0x040001D2 RID: 466
		internal const string DataGridAllowSortingDescr = "DataGridAllowSortingDescr";

		// Token: 0x040001D3 RID: 467
		internal const string DataGridAlternatingBackColorDescr = "DataGridAlternatingBackColorDescr";

		// Token: 0x040001D4 RID: 468
		internal const string DataGridBackButtonClickDescr = "DataGridBackButtonClickDescr";

		// Token: 0x040001D5 RID: 469
		internal const string DataGridBackgroundColorDescr = "DataGridBackgroundColorDescr";

		// Token: 0x040001D6 RID: 470
		internal const string DataGridBeginInit = "DataGridBeginInit";

		// Token: 0x040001D7 RID: 471
		internal const string DataGridBoolColumnAllowNullValue = "DataGridBoolColumnAllowNullValue";

		// Token: 0x040001D8 RID: 472
		internal const string DataGridBorderStyleDescr = "DataGridBorderStyleDescr";

		// Token: 0x040001D9 RID: 473
		internal const string DataGridCaptionBackButtonToolTip = "DataGridCaptionBackButtonToolTip";

		// Token: 0x040001DA RID: 474
		internal const string DataGridCaptionBackColorDescr = "DataGridCaptionBackColorDescr";

		// Token: 0x040001DB RID: 475
		internal const string DataGridCaptionDetailsButtonToolTip = "DataGridCaptionDetailsButtonToolTip";

		// Token: 0x040001DC RID: 476
		internal const string DataGridCaptionFontDescr = "DataGridCaptionFontDescr";

		// Token: 0x040001DD RID: 477
		internal const string DataGridCaptionForeColorDescr = "DataGridCaptionForeColorDescr";

		// Token: 0x040001DE RID: 478
		internal const string DataGridCaptionTextDescr = "DataGridCaptionTextDescr";

		// Token: 0x040001DF RID: 479
		internal const string DataGridCaptionVisibleDescr = "DataGridCaptionVisibleDescr";

		// Token: 0x040001E0 RID: 480
		internal const string DataGridColumnCollectionMissing = "DataGridColumnCollectionMissing";

		// Token: 0x040001E1 RID: 481
		internal const string DataGridColumnHeadersVisibleDescr = "DataGridColumnHeadersVisibleDescr";

		// Token: 0x040001E2 RID: 482
		internal const string DataGridColumnListManagerPosition = "DataGridColumnListManagerPosition";

		// Token: 0x040001E3 RID: 483
		internal const string DataGridColumnNoPropertyDescriptor = "DataGridColumnNoPropertyDescriptor";

		// Token: 0x040001E4 RID: 484
		internal const string DataGridColumnStyleDuplicateMappingName = "DataGridColumnStyleDuplicateMappingName";

		// Token: 0x040001E5 RID: 485
		internal const string DataGridColumnUnbound = "DataGridColumnUnbound";

		// Token: 0x040001E6 RID: 486
		internal const string DataGridColumnWidth = "DataGridColumnWidth";

		// Token: 0x040001E7 RID: 487
		internal const string DataGridCurrentCellDescr = "DataGridCurrentCellDescr";

		// Token: 0x040001E8 RID: 488
		internal const string DataGridDataMemberDescr = "DataGridDataMemberDescr";

		// Token: 0x040001E9 RID: 489
		internal const string DataGridDataSourceDescr = "DataGridDataSourceDescr";

		// Token: 0x040001EA RID: 490
		internal const string DataGridDefaultColumnCollectionChanged = "DataGridDefaultColumnCollectionChanged";

		// Token: 0x040001EB RID: 491
		internal const string DataGridDefaultTableSet = "DataGridDefaultTableSet";

		// Token: 0x040001EC RID: 492
		internal const string DataGridDownButtonClickDescr = "DataGridDownButtonClickDescr";

		// Token: 0x040001ED RID: 493
		internal const string DataGridEmptyColor = "DataGridEmptyColor";

		// Token: 0x040001EE RID: 494
		internal const string DataGridErrorMessageBoxCaption = "DataGridErrorMessageBoxCaption";

		// Token: 0x040001EF RID: 495
		internal const string DataGridExceptionInPaint = "DataGridExceptionInPaint";

		// Token: 0x040001F0 RID: 496
		internal const string DataGridFailedToGetRegionInfo = "DataGridFailedToGetRegionInfo";

		// Token: 0x040001F1 RID: 497
		internal const string DataGridFirstVisibleColumnDescr = "DataGridFirstVisibleColumnDescr";

		// Token: 0x040001F2 RID: 498
		internal const string DataGridFlatModeDescr = "DataGridFlatModeDescr";

		// Token: 0x040001F3 RID: 499
		internal const string DataGridGridLineColorDescr = "DataGridGridLineColorDescr";

		// Token: 0x040001F4 RID: 500
		internal const string DataGridGridLineStyleDescr = "DataGridGridLineStyleDescr";

		// Token: 0x040001F5 RID: 501
		internal const string DataGridGridTablesDescr = "DataGridGridTablesDescr";

		// Token: 0x040001F6 RID: 502
		internal const string DataGridHeaderBackColorDescr = "DataGridHeaderBackColorDescr";

		// Token: 0x040001F7 RID: 503
		internal const string DataGridHeaderFontDescr = "DataGridHeaderFontDescr";

		// Token: 0x040001F8 RID: 504
		internal const string DataGridHeaderForeColorDescr = "DataGridHeaderForeColorDescr";

		// Token: 0x040001F9 RID: 505
		internal const string DataGridHorizScrollBarDescr = "DataGridHorizScrollBarDescr";

		// Token: 0x040001FA RID: 506
		internal const string DataGridLinkColorDescr = "DataGridLinkColorDescr";

		// Token: 0x040001FB RID: 507
		internal const string DataGridLinkHoverColorDescr = "DataGridLinkHoverColorDescr";

		// Token: 0x040001FC RID: 508
		internal const string DataGridListManagerDescr = "DataGridListManagerDescr";

		// Token: 0x040001FD RID: 509
		internal const string DataGridNavigateEventDescr = "DataGridNavigateEventDescr";

		// Token: 0x040001FE RID: 510
		internal const string DataGridNavigationModeDescr = "DataGridNavigationModeDescr";

		// Token: 0x040001FF RID: 511
		internal const string DataGridNodeClickEventDescr = "DataGridNodeClickEventDescr";

		// Token: 0x04000200 RID: 512
		internal const string DataGridNullText = "DataGridNullText";

		// Token: 0x04000201 RID: 513
		internal const string DataGridOnBackgroundColorChangedDescr = "DataGridOnBackgroundColorChangedDescr";

		// Token: 0x04000202 RID: 514
		internal const string DataGridOnBorderStyleChangedDescr = "DataGridOnBorderStyleChangedDescr";

		// Token: 0x04000203 RID: 515
		internal const string DataGridOnCaptionVisibleChangedDescr = "DataGridOnCaptionVisibleChangedDescr";

		// Token: 0x04000204 RID: 516
		internal const string DataGridOnCurrentCellChangedDescr = "DataGridOnCurrentCellChangedDescr";

		// Token: 0x04000205 RID: 517
		internal const string DataGridOnDataSourceChangedDescr = "DataGridOnDataSourceChangedDescr";

		// Token: 0x04000206 RID: 518
		internal const string DataGridOnFlatModeChangedDescr = "DataGridOnFlatModeChangedDescr";

		// Token: 0x04000207 RID: 519
		internal const string DataGridOnNavigationModeChangedDescr = "DataGridOnNavigationModeChangedDescr";

		// Token: 0x04000208 RID: 520
		internal const string DataGridOnParentRowsLabelStyleChangedDescr = "DataGridOnParentRowsLabelStyleChangedDescr";

		// Token: 0x04000209 RID: 521
		internal const string DataGridOnParentRowsVisibleChangedDescr = "DataGridOnParentRowsVisibleChangedDescr";

		// Token: 0x0400020A RID: 522
		internal const string DataGridOnReadOnlyChangedDescr = "DataGridOnReadOnlyChangedDescr";

		// Token: 0x0400020B RID: 523
		internal const string DataGridParentRowsBackColorDescr = "DataGridParentRowsBackColorDescr";

		// Token: 0x0400020C RID: 524
		internal const string DataGridParentRowsForeColorDescr = "DataGridParentRowsForeColorDescr";

		// Token: 0x0400020D RID: 525
		internal const string DataGridParentRowsLabelStyleDescr = "DataGridParentRowsLabelStyleDescr";

		// Token: 0x0400020E RID: 526
		internal const string DataGridParentRowsVisibleDescr = "DataGridParentRowsVisibleDescr";

		// Token: 0x0400020F RID: 527
		internal const string DataGridPreferredColumnWidthDescr = "DataGridPreferredColumnWidthDescr";

		// Token: 0x04000210 RID: 528
		internal const string DataGridPreferredRowHeightDescr = "DataGridPreferredRowHeightDescr";

		// Token: 0x04000211 RID: 529
		internal const string DataGridPushedIncorrectValueIntoColumn = "DataGridPushedIncorrectValueIntoColumn";

		// Token: 0x04000212 RID: 530
		internal const string DataGridReadOnlyDescr = "DataGridReadOnlyDescr";

		// Token: 0x04000213 RID: 531
		internal const string DataGridRowHeadersVisibleDescr = "DataGridRowHeadersVisibleDescr";

		// Token: 0x04000214 RID: 532
		internal const string DataGridRowHeaderWidthDescr = "DataGridRowHeaderWidthDescr";

		// Token: 0x04000215 RID: 533
		internal const string DataGridRowRowHeight = "DataGridRowRowHeight";

		// Token: 0x04000216 RID: 534
		internal const string DataGridRowRowNumber = "DataGridRowRowNumber";

		// Token: 0x04000217 RID: 535
		internal const string DataGridScrollEventDescr = "DataGridScrollEventDescr";

		// Token: 0x04000218 RID: 536
		internal const string DataGridSelectedIndexDescr = "DataGridSelectedIndexDescr";

		// Token: 0x04000219 RID: 537
		internal const string DataGridSelectionBackColorDescr = "DataGridSelectionBackColorDescr";

		// Token: 0x0400021A RID: 538
		internal const string DataGridSelectionForeColorDescr = "DataGridSelectionForeColorDescr";

		// Token: 0x0400021B RID: 539
		internal const string DataGridSetListManager = "DataGridSetListManager";

		// Token: 0x0400021C RID: 540
		internal const string DataGridSetSelectIndex = "DataGridSetSelectIndex";

		// Token: 0x0400021D RID: 541
		internal const string DataGridSettingCurrentCellNotGood = "DataGridSettingCurrentCellNotGood";

		// Token: 0x0400021E RID: 542
		internal const string DataGridTableCollectionMissingTable = "DataGridTableCollectionMissingTable";

		// Token: 0x0400021F RID: 543
		internal const string DataGridTableStyleCollectionAddedParentedTableStyle = "DataGridTableStyleCollectionAddedParentedTableStyle";

		// Token: 0x04000220 RID: 544
		internal const string DataGridTableStyleDuplicateMappingName = "DataGridTableStyleDuplicateMappingName";

		// Token: 0x04000221 RID: 545
		internal const string DataGridTableStyleTransparentAlternatingBackColorNotAllowed = "DataGridTableStyleTransparentAlternatingBackColorNotAllowed";

		// Token: 0x04000222 RID: 546
		internal const string DataGridTableStyleTransparentBackColorNotAllowed = "DataGridTableStyleTransparentBackColorNotAllowed";

		// Token: 0x04000223 RID: 547
		internal const string DataGridTableStyleTransparentHeaderBackColorNotAllowed = "DataGridTableStyleTransparentHeaderBackColorNotAllowed";

		// Token: 0x04000224 RID: 548
		internal const string DataGridTableStyleTransparentSelectionBackColorNotAllowed = "DataGridTableStyleTransparentSelectionBackColorNotAllowed";

		// Token: 0x04000225 RID: 549
		internal const string DataGridToolTipEmptyIcon = "DataGridToolTipEmptyIcon";

		// Token: 0x04000226 RID: 550
		internal const string DataGridTransparentAlternatingBackColorNotAllowed = "DataGridTransparentAlternatingBackColorNotAllowed";

		// Token: 0x04000227 RID: 551
		internal const string DataGridTransparentBackColorNotAllowed = "DataGridTransparentBackColorNotAllowed";

		// Token: 0x04000228 RID: 552
		internal const string DataGridTransparentCaptionBackColorNotAllowed = "DataGridTransparentCaptionBackColorNotAllowed";

		// Token: 0x04000229 RID: 553
		internal const string DataGridTransparentHeaderBackColorNotAllowed = "DataGridTransparentHeaderBackColorNotAllowed";

		// Token: 0x0400022A RID: 554
		internal const string DataGridTransparentParentRowsBackColorNotAllowed = "DataGridTransparentParentRowsBackColorNotAllowed";

		// Token: 0x0400022B RID: 555
		internal const string DataGridTransparentSelectionBackColorNotAllowed = "DataGridTransparentSelectionBackColorNotAllowed";

		// Token: 0x0400022C RID: 556
		internal const string DataGridUnbound = "DataGridUnbound";

		// Token: 0x0400022D RID: 557
		internal const string DataGridVertScrollBarDescr = "DataGridVertScrollBarDescr";

		// Token: 0x0400022E RID: 558
		internal const string DataGridView_AccButtonCellDefaultAction = "DataGridView_AccButtonCellDefaultAction";

		// Token: 0x0400022F RID: 559
		internal const string DataGridView_AccCellDefaultAction = "DataGridView_AccCellDefaultAction";

		// Token: 0x04000230 RID: 560
		internal const string DataGridView_AccCheckBoxCellDefaultActionCheck = "DataGridView_AccCheckBoxCellDefaultActionCheck";

		// Token: 0x04000231 RID: 561
		internal const string DataGridView_AccCheckBoxCellDefaultActionUncheck = "DataGridView_AccCheckBoxCellDefaultActionUncheck";

		// Token: 0x04000232 RID: 562
		internal const string DataGridView_AccColumnHeaderCellDefaultAction = "DataGridView_AccColumnHeaderCellDefaultAction";

		// Token: 0x04000233 RID: 563
		internal const string DataGridView_AccColumnHeaderCellSelectDefaultAction = "DataGridView_AccColumnHeaderCellSelectDefaultAction";

		// Token: 0x04000234 RID: 564
		internal const string DataGridView_AccDataGridViewCellName = "DataGridView_AccDataGridViewCellName";

		// Token: 0x04000235 RID: 565
		internal const string DataGridView_AccEditingControlAccName = "DataGridView_AccEditingControlAccName";

		// Token: 0x04000236 RID: 566
		internal const string DataGridView_AccEditingPanelAccName = "DataGridView_AccEditingPanelAccName";

		// Token: 0x04000237 RID: 567
		internal const string DataGridView_AccHorizontalScrollBarAccName = "DataGridView_AccHorizontalScrollBarAccName";

		// Token: 0x04000238 RID: 568
		internal const string DataGridView_AccLinkCellDefaultAction = "DataGridView_AccLinkCellDefaultAction";

		// Token: 0x04000239 RID: 569
		internal const string DataGridView_AccNullValue = "DataGridView_AccNullValue";

		// Token: 0x0400023A RID: 570
		internal const string DataGridView_AccRowCreateNew = "DataGridView_AccRowCreateNew";

		// Token: 0x0400023B RID: 571
		internal const string DataGridView_AccRowName = "DataGridView_AccRowName";

		// Token: 0x0400023C RID: 572
		internal const string DataGridView_AccSelectedCellsName = "DataGridView_AccSelectedCellsName";

		// Token: 0x0400023D RID: 573
		internal const string DataGridView_AccSelectedRowCellsName = "DataGridView_AccSelectedRowCellsName";

		// Token: 0x0400023E RID: 574
		internal const string DataGridView_AccTopLeftColumnHeaderCellDefaultAction = "DataGridView_AccTopLeftColumnHeaderCellDefaultAction";

		// Token: 0x0400023F RID: 575
		internal const string DataGridView_AccTopLeftColumnHeaderCellName = "DataGridView_AccTopLeftColumnHeaderCellName";

		// Token: 0x04000240 RID: 576
		internal const string DataGridView_AccTopLeftColumnHeaderCellNameRTL = "DataGridView_AccTopLeftColumnHeaderCellNameRTL";

		// Token: 0x04000241 RID: 577
		internal const string DataGridView_AccTopRow = "DataGridView_AccTopRow";

		// Token: 0x04000242 RID: 578
		internal const string DataGridView_AccVerticalScrollBarAccName = "DataGridView_AccVerticalScrollBarAccName";

		// Token: 0x04000243 RID: 579
		internal const string DataGridView_AColumnHasNoCellTemplate = "DataGridView_AColumnHasNoCellTemplate";

		// Token: 0x04000244 RID: 580
		internal const string DataGridView_AdvancedCellBorderStyleInvalid = "DataGridView_AdvancedCellBorderStyleInvalid";

		// Token: 0x04000245 RID: 581
		internal const string DataGridView_AllowUserToAddRowsDescr = "DataGridView_AllowUserToAddRowsDescr";

		// Token: 0x04000246 RID: 582
		internal const string DataGridView_AllowUserToDeleteRowsDescr = "DataGridView_AllowUserToDeleteRowsDescr";

		// Token: 0x04000247 RID: 583
		internal const string DataGridView_AllowUserToOrderColumnsDescr = "DataGridView_AllowUserToOrderColumnsDescr";

		// Token: 0x04000248 RID: 584
		internal const string DataGridView_AllowUserToResizeColumnsDescr = "DataGridView_AllowUserToResizeColumnsDescr";

		// Token: 0x04000249 RID: 585
		internal const string DataGridView_AllowUserToResizeRowsDescr = "DataGridView_AllowUserToResizeRowsDescr";

		// Token: 0x0400024A RID: 586
		internal const string DataGridView_AlternatingRowsDefaultCellStyleDescr = "DataGridView_AlternatingRowsDefaultCellStyleDescr";

		// Token: 0x0400024B RID: 587
		internal const string DataGridView_AtLeastOneColumnIsNull = "DataGridView_AtLeastOneColumnIsNull";

		// Token: 0x0400024C RID: 588
		internal const string DataGridView_AtLeastOneRowIsNull = "DataGridView_AtLeastOneRowIsNull";

		// Token: 0x0400024D RID: 589
		internal const string DataGridView_AutoSizeColumnsModeDescr = "DataGridView_AutoSizeColumnsModeDescr";

		// Token: 0x0400024E RID: 590
		internal const string DataGridView_AutoSizeRowsModeDescr = "DataGridView_AutoSizeRowsModeDescr";

		// Token: 0x0400024F RID: 591
		internal const string DataGridView_BeginEditNotReentrant = "DataGridView_BeginEditNotReentrant";

		// Token: 0x04000250 RID: 592
		internal const string DataGridView_BorderStyleDescr = "DataGridView_BorderStyleDescr";

		// Token: 0x04000251 RID: 593
		internal const string DataGridView_ButtonColumnFlatStyleDescr = "DataGridView_ButtonColumnFlatStyleDescr";

		// Token: 0x04000252 RID: 594
		internal const string DataGridView_ButtonColumnTextDescr = "DataGridView_ButtonColumnTextDescr";

		// Token: 0x04000253 RID: 595
		internal const string DataGridView_ButtonColumnUseColumnTextForButtonValueDescr = "DataGridView_ButtonColumnUseColumnTextForButtonValueDescr";

		// Token: 0x04000254 RID: 596
		internal const string DataGridView_CancelRowEditDescr = "DataGridView_CancelRowEditDescr";

		// Token: 0x04000255 RID: 597
		internal const string DataGridView_CannotAddAutoFillColumn = "DataGridView_CannotAddAutoFillColumn";

		// Token: 0x04000256 RID: 598
		internal const string DataGridView_CannotAddAutoSizedColumn = "DataGridView_CannotAddAutoSizedColumn";

		// Token: 0x04000257 RID: 599
		internal const string DataGridView_CannotAddFrozenColumn = "DataGridView_CannotAddFrozenColumn";

		// Token: 0x04000258 RID: 600
		internal const string DataGridView_CannotAddFrozenRow = "DataGridView_CannotAddFrozenRow";

		// Token: 0x04000259 RID: 601
		internal const string DataGridView_CannotAddIdenticalColumns = "DataGridView_CannotAddIdenticalColumns";

		// Token: 0x0400025A RID: 602
		internal const string DataGridView_CannotAddIdenticalRows = "DataGridView_CannotAddIdenticalRows";

		// Token: 0x0400025B RID: 603
		internal const string DataGridView_CannotAddNonFrozenColumn = "DataGridView_CannotAddNonFrozenColumn";

		// Token: 0x0400025C RID: 604
		internal const string DataGridView_CannotAddNonFrozenRow = "DataGridView_CannotAddNonFrozenRow";

		// Token: 0x0400025D RID: 605
		internal const string DataGridView_CannotAddUntypedColumn = "DataGridView_CannotAddUntypedColumn";

		// Token: 0x0400025E RID: 606
		internal const string DataGridView_CannotAlterAutoFillColumnParameter = "DataGridView_CannotAlterAutoFillColumnParameter";

		// Token: 0x0400025F RID: 607
		internal const string DataGridView_CannotAlterDisplayIndexWithinAdjustments = "DataGridView_CannotAlterDisplayIndexWithinAdjustments";

		// Token: 0x04000260 RID: 608
		internal const string DataGridView_CannotAutoFillFrozenColumns = "DataGridView_CannotAutoFillFrozenColumns";

		// Token: 0x04000261 RID: 609
		internal const string DataGridView_CannotAutoSizeColumnsInvisibleColumnHeaders = "DataGridView_CannotAutoSizeColumnsInvisibleColumnHeaders";

		// Token: 0x04000262 RID: 610
		internal const string DataGridView_CannotAutoSizeInvisibleColumnHeader = "DataGridView_CannotAutoSizeInvisibleColumnHeader";

		// Token: 0x04000263 RID: 611
		internal const string DataGridView_CannotAutoSizeRowInvisibleRowHeader = "DataGridView_CannotAutoSizeRowInvisibleRowHeader";

		// Token: 0x04000264 RID: 612
		internal const string DataGridView_CannotAutoSizeRowsInvisibleRowHeader = "DataGridView_CannotAutoSizeRowsInvisibleRowHeader";

		// Token: 0x04000265 RID: 613
		internal const string DataGridView_CannotMakeAutoSizedColumnVisible = "DataGridView_CannotMakeAutoSizedColumnVisible";

		// Token: 0x04000266 RID: 614
		internal const string DataGridView_CannotMoveFrozenColumn = "DataGridView_CannotMoveFrozenColumn";

		// Token: 0x04000267 RID: 615
		internal const string DataGridView_CannotMoveNonFrozenColumn = "DataGridView_CannotMoveNonFrozenColumn";

		// Token: 0x04000268 RID: 616
		internal const string DataGridView_CannotSetColumnCountOnDataBoundDataGridView = "DataGridView_CannotSetColumnCountOnDataBoundDataGridView";

		// Token: 0x04000269 RID: 617
		internal const string DataGridView_CannotSetRowCountOnDataBoundDataGridView = "DataGridView_CannotSetRowCountOnDataBoundDataGridView";

		// Token: 0x0400026A RID: 618
		internal const string DataGridView_CannotSortDataBoundDataGridViewBoundToNonIBindingList = "DataGridView_CannotSortDataBoundDataGridViewBoundToNonIBindingList";

		// Token: 0x0400026B RID: 619
		internal const string DataGridView_CannotThrowNullException = "DataGridView_CannotThrowNullException";

		// Token: 0x0400026C RID: 620
		internal const string DataGridView_CannotUseAComparerToSortDataGridViewWhenDataBound = "DataGridView_CannotUseAComparerToSortDataGridViewWhenDataBound";

		// Token: 0x0400026D RID: 621
		internal const string DataGridView_CellBeginEditDescr = "DataGridView_CellBeginEditDescr";

		// Token: 0x0400026E RID: 622
		internal const string DataGridView_CellBorderStyleChangedDescr = "DataGridView_CellBorderStyleChangedDescr";

		// Token: 0x0400026F RID: 623
		internal const string DataGridView_CellBorderStyleDescr = "DataGridView_CellBorderStyleDescr";

		// Token: 0x04000270 RID: 624
		internal const string DataGridView_CellChangeCannotBeCommittedOrAborted = "DataGridView_CellChangeCannotBeCommittedOrAborted";

		// Token: 0x04000271 RID: 625
		internal const string DataGridView_CellClickDescr = "DataGridView_CellClickDescr";

		// Token: 0x04000272 RID: 626
		internal const string DataGridView_CellContentClick = "DataGridView_CellContentClick";

		// Token: 0x04000273 RID: 627
		internal const string DataGridView_CellContentDoubleClick = "DataGridView_CellContentDoubleClick";

		// Token: 0x04000274 RID: 628
		internal const string DataGridView_CellContextMenuStripChanged = "DataGridView_CellContextMenuStripChanged";

		// Token: 0x04000275 RID: 629
		internal const string DataGridView_CellContextMenuStripNeeded = "DataGridView_CellContextMenuStripNeeded";

		// Token: 0x04000276 RID: 630
		internal const string DataGridView_CellDoesNotBelongToDataGridView = "DataGridView_CellDoesNotBelongToDataGridView";

		// Token: 0x04000277 RID: 631
		internal const string DataGridView_CellDoesNotYetBelongToDataGridView = "DataGridView_CellDoesNotYetBelongToDataGridView";

		// Token: 0x04000278 RID: 632
		internal const string DataGridView_CellDoubleClickDescr = "DataGridView_CellDoubleClickDescr";

		// Token: 0x04000279 RID: 633
		internal const string DataGridView_CellEndEditDescr = "DataGridView_CellEndEditDescr";

		// Token: 0x0400027A RID: 634
		internal const string DataGridView_CellEnterDescr = "DataGridView_CellEnterDescr";

		// Token: 0x0400027B RID: 635
		internal const string DataGridView_CellErrorTextChangedDescr = "DataGridView_CellErrorTextChangedDescr";

		// Token: 0x0400027C RID: 636
		internal const string DataGridView_CellErrorTextNeededDescr = "DataGridView_CellErrorTextNeededDescr";

		// Token: 0x0400027D RID: 637
		internal const string DataGridView_CellFormattingDescr = "DataGridView_CellFormattingDescr";

		// Token: 0x0400027E RID: 638
		internal const string DataGridView_CellLeaveDescr = "DataGridView_CellLeaveDescr";

		// Token: 0x0400027F RID: 639
		internal const string DataGridView_CellMouseClickDescr = "DataGridView_CellMouseClickDescr";

		// Token: 0x04000280 RID: 640
		internal const string DataGridView_CellMouseDoubleClickDescr = "DataGridView_CellMouseDoubleClickDescr";

		// Token: 0x04000281 RID: 641
		internal const string DataGridView_CellMouseDownDescr = "DataGridView_CellMouseDownDescr";

		// Token: 0x04000282 RID: 642
		internal const string DataGridView_CellMouseEnterDescr = "DataGridView_CellMouseEnterDescr";

		// Token: 0x04000283 RID: 643
		internal const string DataGridView_CellMouseLeaveDescr = "DataGridView_CellMouseLeaveDescr";

		// Token: 0x04000284 RID: 644
		internal const string DataGridView_CellMouseMoveDescr = "DataGridView_CellMouseMoveDescr";

		// Token: 0x04000285 RID: 645
		internal const string DataGridView_CellMouseUpDescr = "DataGridView_CellMouseUpDescr";

		// Token: 0x04000286 RID: 646
		internal const string DataGridView_CellNeedsDataGridViewForInheritedStyle = "DataGridView_CellNeedsDataGridViewForInheritedStyle";

		// Token: 0x04000287 RID: 647
		internal const string DataGridView_CellPaintingDescr = "DataGridView_CellPaintingDescr";

		// Token: 0x04000288 RID: 648
		internal const string DataGridView_CellParsingDescr = "DataGridView_CellParsingDescr";

		// Token: 0x04000289 RID: 649
		internal const string DataGridView_CellStateChangedDescr = "DataGridView_CellStateChangedDescr";

		// Token: 0x0400028A RID: 650
		internal const string DataGridView_CellStyleChangedDescr = "DataGridView_CellStyleChangedDescr";

		// Token: 0x0400028B RID: 651
		internal const string DataGridView_CellStyleContentChangedDescr = "DataGridView_CellStyleContentChangedDescr";

		// Token: 0x0400028C RID: 652
		internal const string DataGridView_CellToolTipTextChangedDescr = "DataGridView_CellToolTipTextChangedDescr";

		// Token: 0x0400028D RID: 653
		internal const string DataGridView_CellToolTipTextDescr = "DataGridView_CellToolTipTextDescr";

		// Token: 0x0400028E RID: 654
		internal const string DataGridView_CellToolTipTextNeededDescr = "DataGridView_CellToolTipTextNeededDescr";

		// Token: 0x0400028F RID: 655
		internal const string DataGridView_CellValidatedDescr = "DataGridView_CellValidatedDescr";

		// Token: 0x04000290 RID: 656
		internal const string DataGridView_CellValidatingDescr = "DataGridView_CellValidatingDescr";

		// Token: 0x04000291 RID: 657
		internal const string DataGridView_CellValueChangedDescr = "DataGridView_CellValueChangedDescr";

		// Token: 0x04000292 RID: 658
		internal const string DataGridView_CellValueNeededDescr = "DataGridView_CellValueNeededDescr";

		// Token: 0x04000293 RID: 659
		internal const string DataGridView_CellValuePushedDescr = "DataGridView_CellValuePushedDescr";

		// Token: 0x04000294 RID: 660
		internal const string DataGridView_CheckBoxColumnFalseValueDescr = "DataGridView_CheckBoxColumnFalseValueDescr";

		// Token: 0x04000295 RID: 661
		internal const string DataGridView_CheckBoxColumnFlatStyleDescr = "DataGridView_CheckBoxColumnFlatStyleDescr";

		// Token: 0x04000296 RID: 662
		internal const string DataGridView_CheckBoxColumnIndeterminateValueDescr = "DataGridView_CheckBoxColumnIndeterminateValueDescr";

		// Token: 0x04000297 RID: 663
		internal const string DataGridView_CheckBoxColumnThreeStateDescr = "DataGridView_CheckBoxColumnThreeStateDescr";

		// Token: 0x04000298 RID: 664
		internal const string DataGridView_CheckBoxColumnTrueValueDescr = "DataGridView_CheckBoxColumnTrueValueDescr";

		// Token: 0x04000299 RID: 665
		internal const string DataGridView_ClipboardCopyModeDescr = "DataGridView_ClipboardCopyModeDescr";

		// Token: 0x0400029A RID: 666
		internal const string DataGridView_ColumnAddedDescr = "DataGridView_ColumnAddedDescr";

		// Token: 0x0400029B RID: 667
		internal const string DataGridView_ColumnAlreadyBelongsToDataGridView = "DataGridView_ColumnAlreadyBelongsToDataGridView";

		// Token: 0x0400029C RID: 668
		internal const string DataGridView_ColumnBoundToAReadOnlyFieldMustRemainReadOnly = "DataGridView_ColumnBoundToAReadOnlyFieldMustRemainReadOnly";

		// Token: 0x0400029D RID: 669
		internal const string DataGridView_ColumnContextMenuStripChangedDescr = "DataGridView_ColumnContextMenuStripChangedDescr";

		// Token: 0x0400029E RID: 670
		internal const string DataGridView_ColumnContextMenuStripDescr = "DataGridView_ColumnContextMenuStripDescr";

		// Token: 0x0400029F RID: 671
		internal const string DataGridView_ColumnDataPropertyNameChangedDescr = "DataGridView_ColumnDataPropertyNameChangedDescr";

		// Token: 0x040002A0 RID: 672
		internal const string DataGridView_ColumnDataPropertyNameDescr = "DataGridView_ColumnDataPropertyNameDescr";

		// Token: 0x040002A1 RID: 673
		internal const string DataGridView_ColumnDefaultCellStyleChangedDescr = "DataGridView_ColumnDefaultCellStyleChangedDescr";

		// Token: 0x040002A2 RID: 674
		internal const string DataGridView_ColumnDefaultCellStyleDescr = "DataGridView_ColumnDefaultCellStyleDescr";

		// Token: 0x040002A3 RID: 675
		internal const string DataGridView_ColumnDisplayIndexChangedDescr = "DataGridView_ColumnDisplayIndexChangedDescr";

		// Token: 0x040002A4 RID: 676
		internal const string DataGridView_ColumnDividerDoubleClickDescr = "DataGridView_ColumnDividerDoubleClickDescr";

		// Token: 0x040002A5 RID: 677
		internal const string DataGridView_ColumnDividerWidthChangedDescr = "DataGridView_ColumnDividerWidthChangedDescr";

		// Token: 0x040002A6 RID: 678
		internal const string DataGridView_ColumnDividerWidthDescr = "DataGridView_ColumnDividerWidthDescr";

		// Token: 0x040002A7 RID: 679
		internal const string DataGridView_ColumnDoesNotBelongToDataGridView = "DataGridView_ColumnDoesNotBelongToDataGridView";

		// Token: 0x040002A8 RID: 680
		internal const string DataGridView_ColumnFrozenDescr = "DataGridView_ColumnFrozenDescr";

		// Token: 0x040002A9 RID: 681
		internal const string DataGridView_ColumnHeaderCellChangedDescr = "DataGridView_ColumnHeaderCellChangedDescr";

		// Token: 0x040002AA RID: 682
		internal const string DataGridView_ColumnHeaderMouseClickDescr = "DataGridView_ColumnHeaderMouseClickDescr";

		// Token: 0x040002AB RID: 683
		internal const string DataGridView_ColumnHeaderMouseDoubleClickDescr = "DataGridView_ColumnHeaderMouseDoubleClickDescr";

		// Token: 0x040002AC RID: 684
		internal const string DataGridView_ColumnHeadersBorderStyleChangedDescr = "DataGridView_ColumnHeadersBorderStyleChangedDescr";

		// Token: 0x040002AD RID: 685
		internal const string DataGridView_ColumnHeadersBorderStyleDescr = "DataGridView_ColumnHeadersBorderStyleDescr";

		// Token: 0x040002AE RID: 686
		internal const string DataGridView_ColumnHeadersCannotBeInvisible = "DataGridView_ColumnHeadersCannotBeInvisible";

		// Token: 0x040002AF RID: 687
		internal const string DataGridView_ColumnHeadersDefaultCellStyleDescr = "DataGridView_ColumnHeadersDefaultCellStyleDescr";

		// Token: 0x040002B0 RID: 688
		internal const string DataGridView_ColumnHeadersHeightDescr = "DataGridView_ColumnHeadersHeightDescr";

		// Token: 0x040002B1 RID: 689
		internal const string DataGridView_ColumnHeadersHeightSizeModeChangedDescr = "DataGridView_ColumnHeadersHeightSizeModeChangedDescr";

		// Token: 0x040002B2 RID: 690
		internal const string DataGridView_ColumnHeadersHeightSizeModeDescr = "DataGridView_ColumnHeadersHeightSizeModeDescr";

		// Token: 0x040002B3 RID: 691
		internal const string DataGridView_ColumnHeaderTextDescr = "DataGridView_ColumnHeaderTextDescr";

		// Token: 0x040002B4 RID: 692
		internal const string DataGridView_ColumnMinimumWidthChangedDescr = "DataGridView_ColumnMinimumWidthChangedDescr";

		// Token: 0x040002B5 RID: 693
		internal const string DataGridView_ColumnMinimumWidthDescr = "DataGridView_ColumnMinimumWidthDescr";

		// Token: 0x040002B6 RID: 694
		internal const string DataGridView_ColumnNameChangedDescr = "DataGridView_ColumnNameChangedDescr";

		// Token: 0x040002B7 RID: 695
		internal const string DataGridView_ColumnNameDescr = "DataGridView_ColumnNameDescr";

		// Token: 0x040002B8 RID: 696
		internal const string DataGridView_ColumnNeedsToBeDataBoundWhenSortingDataBoundDataGridView = "DataGridView_ColumnNeedsToBeDataBoundWhenSortingDataBoundDataGridView";

		// Token: 0x040002B9 RID: 697
		internal const string DataGridView_ColumnReadOnlyDescr = "DataGridView_ColumnReadOnlyDescr";

		// Token: 0x040002BA RID: 698
		internal const string DataGridView_ColumnRemovedDescr = "DataGridView_ColumnRemovedDescr";

		// Token: 0x040002BB RID: 699
		internal const string DataGridView_ColumnResizableDescr = "DataGridView_ColumnResizableDescr";

		// Token: 0x040002BC RID: 700
		internal const string DataGridView_ColumnSortModeDescr = "DataGridView_ColumnSortModeDescr";

		// Token: 0x040002BD RID: 701
		internal const string DataGridView_ColumnStateChangedDescr = "DataGridView_ColumnStateChangedDescr";

		// Token: 0x040002BE RID: 702
		internal const string DataGridView_ColumnToolTipTextChangedDescr = "DataGridView_ColumnToolTipTextChangedDescr";

		// Token: 0x040002BF RID: 703
		internal const string DataGridView_ColumnToolTipTextDescr = "DataGridView_ColumnToolTipTextDescr";

		// Token: 0x040002C0 RID: 704
		internal const string DataGridView_ColumnVisibleDescr = "DataGridView_ColumnVisibleDescr";

		// Token: 0x040002C1 RID: 705
		internal const string DataGridView_ColumnWidthChangedDescr = "DataGridView_ColumnWidthChangedDescr";

		// Token: 0x040002C2 RID: 706
		internal const string DataGridView_ColumnWidthDescr = "DataGridView_ColumnWidthDescr";

		// Token: 0x040002C3 RID: 707
		internal const string DataGridView_ComboBoxColumnAutoCompleteDescr = "DataGridView_ComboBoxColumnAutoCompleteDescr";

		// Token: 0x040002C4 RID: 708
		internal const string DataGridView_ComboBoxColumnDataSourceDescr = "DataGridView_ComboBoxColumnDataSourceDescr";

		// Token: 0x040002C5 RID: 709
		internal const string DataGridView_ComboBoxColumnDisplayMemberDescr = "DataGridView_ComboBoxColumnDisplayMemberDescr";

		// Token: 0x040002C6 RID: 710
		internal const string DataGridView_ComboBoxColumnDisplayStyleDescr = "DataGridView_ComboBoxColumnDisplayStyleDescr";

		// Token: 0x040002C7 RID: 711
		internal const string DataGridView_ComboBoxColumnDisplayStyleForCurrentCellOnlyDescr = "DataGridView_ComboBoxColumnDisplayStyleForCurrentCellOnlyDescr";

		// Token: 0x040002C8 RID: 712
		internal const string DataGridView_ComboBoxColumnDropDownWidthDescr = "DataGridView_ComboBoxColumnDropDownWidthDescr";

		// Token: 0x040002C9 RID: 713
		internal const string DataGridView_ComboBoxColumnFlatStyleDescr = "DataGridView_ComboBoxColumnFlatStyleDescr";

		// Token: 0x040002CA RID: 714
		internal const string DataGridView_ComboBoxColumnItemsDescr = "DataGridView_ComboBoxColumnItemsDescr";

		// Token: 0x040002CB RID: 715
		internal const string DataGridView_ComboBoxColumnMaxDropDownItemsDescr = "DataGridView_ComboBoxColumnMaxDropDownItemsDescr";

		// Token: 0x040002CC RID: 716
		internal const string DataGridView_ComboBoxColumnSortedDescr = "DataGridView_ComboBoxColumnSortedDescr";

		// Token: 0x040002CD RID: 717
		internal const string DataGridView_ComboBoxColumnValueMemberDescr = "DataGridView_ComboBoxColumnValueMemberDescr";

		// Token: 0x040002CE RID: 718
		internal const string DataGridView_CommitFailedCannotCompleteOperation = "DataGridView_CommitFailedCannotCompleteOperation";

		// Token: 0x040002CF RID: 719
		internal const string DataGridView_CurrencyManagerRowCannotBeInvisible = "DataGridView_CurrencyManagerRowCannotBeInvisible";

		// Token: 0x040002D0 RID: 720
		internal const string DataGridView_CurrentCellCannotBeInvisible = "DataGridView_CurrentCellCannotBeInvisible";

		// Token: 0x040002D1 RID: 721
		internal const string DataGridView_CurrentCellChangedDescr = "DataGridView_CurrentCellChangedDescr";

		// Token: 0x040002D2 RID: 722
		internal const string DataGridView_CurrentCellDirtyStateChangedDescr = "DataGridView_CurrentCellDirtyStateChangedDescr";

		// Token: 0x040002D3 RID: 723
		internal const string DataGridView_CustomCellBorderStyleInvalid = "DataGridView_CustomCellBorderStyleInvalid";

		// Token: 0x040002D4 RID: 724
		internal const string DataGridView_DataBindingCompleteDescr = "DataGridView_DataBindingCompleteDescr";

		// Token: 0x040002D5 RID: 725
		internal const string DataGridView_DataErrorDescr = "DataGridView_DataErrorDescr";

		// Token: 0x040002D6 RID: 726
		internal const string DataGridView_DefaultCellStyleDescr = "DataGridView_DefaultCellStyleDescr";

		// Token: 0x040002D7 RID: 727
		internal const string DataGridView_DefaultValuesNeededDescr = "DataGridView_DefaultValuesNeededDescr";

		// Token: 0x040002D8 RID: 728
		internal const string DataGridView_DisabledClipboardCopy = "DataGridView_DisabledClipboardCopy";

		// Token: 0x040002D9 RID: 729
		internal const string DataGridView_EditingControlShowingDescr = "DataGridView_EditingControlShowingDescr";

		// Token: 0x040002DA RID: 730
		internal const string DataGridView_EditModeChangedDescr = "DataGridView_EditModeChangedDescr";

		// Token: 0x040002DB RID: 731
		internal const string DataGridView_EditModeDescr = "DataGridView_EditModeDescr";

		// Token: 0x040002DC RID: 732
		internal const string DataGridView_EmptyColor = "DataGridView_EmptyColor";

		// Token: 0x040002DD RID: 733
		internal const string DataGridView_EnableHeadersVisualStylesDescr = "DataGridView_EnableHeadersVisualStylesDescr";

		// Token: 0x040002DE RID: 734
		internal const string DataGridView_ErrorMessageCaption = "DataGridView_ErrorMessageCaption";

		// Token: 0x040002DF RID: 735
		internal const string DataGridView_ErrorMessageText_NoException = "DataGridView_ErrorMessageText_NoException";

		// Token: 0x040002E0 RID: 736
		internal const string DataGridView_ErrorMessageText_WithException = "DataGridView_ErrorMessageText_WithException";

		// Token: 0x040002E1 RID: 737
		internal const string DataGridView_FirstDisplayedCellCannotBeAHeaderOrSharedCell = "DataGridView_FirstDisplayedCellCannotBeAHeaderOrSharedCell";

		// Token: 0x040002E2 RID: 738
		internal const string DataGridView_FirstDisplayedCellCannotBeInvisible = "DataGridView_FirstDisplayedCellCannotBeInvisible";

		// Token: 0x040002E3 RID: 739
		internal const string DataGridView_FirstDisplayedScrollingColumnCannotBeFrozen = "DataGridView_FirstDisplayedScrollingColumnCannotBeFrozen";

		// Token: 0x040002E4 RID: 740
		internal const string DataGridView_FirstDisplayedScrollingColumnCannotBeInvisible = "DataGridView_FirstDisplayedScrollingColumnCannotBeInvisible";

		// Token: 0x040002E5 RID: 741
		internal const string DataGridView_FirstDisplayedScrollingRowCannotBeFrozen = "DataGridView_FirstDisplayedScrollingRowCannotBeFrozen";

		// Token: 0x040002E6 RID: 742
		internal const string DataGridView_FirstDisplayedScrollingRowCannotBeInvisible = "DataGridView_FirstDisplayedScrollingRowCannotBeInvisible";

		// Token: 0x040002E7 RID: 743
		internal const string DataGridView_ForbiddenOperationInEventHandler = "DataGridView_ForbiddenOperationInEventHandler";

		// Token: 0x040002E8 RID: 744
		internal const string DataGridView_FrozenColumnsPreventFirstDisplayedScrollingColumn = "DataGridView_FrozenColumnsPreventFirstDisplayedScrollingColumn";

		// Token: 0x040002E9 RID: 745
		internal const string DataGridView_FrozenRowsPreventFirstDisplayedScrollingRow = "DataGridView_FrozenRowsPreventFirstDisplayedScrollingRow";

		// Token: 0x040002EA RID: 746
		internal const string DataGridView_HeaderCellReadOnlyProperty = "DataGridView_HeaderCellReadOnlyProperty";

		// Token: 0x040002EB RID: 747
		internal const string DataGridView_IBindingListNeedsToSupportSorting = "DataGridView_IBindingListNeedsToSupportSorting";

		// Token: 0x040002EC RID: 748
		internal const string DataGridView_InvalidDataGridViewElementStateCombination = "DataGridView_InvalidDataGridViewElementStateCombination";

		// Token: 0x040002ED RID: 749
		internal const string DataGridView_InvalidDataGridViewPaintPartsCombination = "DataGridView_InvalidDataGridViewPaintPartsCombination";

		// Token: 0x040002EE RID: 750
		internal const string DataGridView_InvalidEditingControl = "DataGridView_InvalidEditingControl";

		// Token: 0x040002EF RID: 751
		internal const string DataGridView_InvalidOperationInVirtualMode = "DataGridView_InvalidOperationInVirtualMode";

		// Token: 0x040002F0 RID: 752
		internal const string DataGridView_InvalidOperationOnSharedCell = "DataGridView_InvalidOperationOnSharedCell";

		// Token: 0x040002F1 RID: 753
		internal const string DataGridView_InvalidOperationOnSharedRow = "DataGridView_InvalidOperationOnSharedRow";

		// Token: 0x040002F2 RID: 754
		internal const string DataGridView_InvalidPropertyGetOnSharedCell = "DataGridView_InvalidPropertyGetOnSharedCell";

		// Token: 0x040002F3 RID: 755
		internal const string DataGridView_InvalidPropertyGetOnSharedRow = "DataGridView_InvalidPropertyGetOnSharedRow";

		// Token: 0x040002F4 RID: 756
		internal const string DataGridView_InvalidPropertySetOnSharedRow = "DataGridView_InvalidPropertySetOnSharedRow";

		// Token: 0x040002F5 RID: 757
		internal const string DataGridView_LinkColumnActiveLinkColorDescr = "DataGridView_LinkColumnActiveLinkColorDescr";

		// Token: 0x040002F6 RID: 758
		internal const string DataGridView_LinkColumnLinkBehaviorDescr = "DataGridView_LinkColumnLinkBehaviorDescr";

		// Token: 0x040002F7 RID: 759
		internal const string DataGridView_LinkColumnLinkColorDescr = "DataGridView_LinkColumnLinkColorDescr";

		// Token: 0x040002F8 RID: 760
		internal const string DataGridView_LinkColumnTextDescr = "DataGridView_LinkColumnTextDescr";

		// Token: 0x040002F9 RID: 761
		internal const string DataGridView_LinkColumnTrackVisitedStateDescr = "DataGridView_LinkColumnTrackVisitedStateDescr";

		// Token: 0x040002FA RID: 762
		internal const string DataGridView_LinkColumnUseColumnTextForLinkValueDescr = "DataGridView_LinkColumnUseColumnTextForLinkValueDescr";

		// Token: 0x040002FB RID: 763
		internal const string DataGridView_LinkColumnVisitedLinkColorDescr = "DataGridView_LinkColumnVisitedLinkColorDescr";

		// Token: 0x040002FC RID: 764
		internal const string DataGridView_MultiSelectDescr = "DataGridView_MultiSelectDescr";

		// Token: 0x040002FD RID: 765
		internal const string DataGridView_NeedAutoSizingCriteria = "DataGridView_NeedAutoSizingCriteria";

		// Token: 0x040002FE RID: 766
		internal const string DataGridView_NeedColumnAutoSizingCriteria = "DataGridView_NeedColumnAutoSizingCriteria";

		// Token: 0x040002FF RID: 767
		internal const string DataGridView_NewRowNeededDescr = "DataGridView_NewRowNeededDescr";

		// Token: 0x04000300 RID: 768
		internal const string DataGridView_NoCurrentCell = "DataGridView_NoCurrentCell";

		// Token: 0x04000301 RID: 769
		internal const string DataGridView_NoRoomForDisplayedColumns = "DataGridView_NoRoomForDisplayedColumns";

		// Token: 0x04000302 RID: 770
		internal const string DataGridView_NoRoomForDisplayedRows = "DataGridView_NoRoomForDisplayedRows";

		// Token: 0x04000303 RID: 771
		internal const string DataGridView_OperationDisabledInVirtualMode = "DataGridView_OperationDisabledInVirtualMode";

		// Token: 0x04000304 RID: 772
		internal const string DataGridView_PreviousModesHasWrongLength = "DataGridView_PreviousModesHasWrongLength";

		// Token: 0x04000305 RID: 773
		internal const string DataGridView_PropertyMustBeZero = "DataGridView_PropertyMustBeZero";

		// Token: 0x04000306 RID: 774
		internal const string DataGridView_ReadOnlyCollection = "DataGridView_ReadOnlyCollection";

		// Token: 0x04000307 RID: 775
		internal const string DataGridView_ReadOnlyDescr = "DataGridView_ReadOnlyDescr";

		// Token: 0x04000308 RID: 776
		internal const string DataGridView_RowAlreadyBelongsToDataGridView = "DataGridView_RowAlreadyBelongsToDataGridView";

		// Token: 0x04000309 RID: 777
		internal const string DataGridView_RowContextMenuStripChangedDescr = "DataGridView_RowContextMenuStripChangedDescr";

		// Token: 0x0400030A RID: 778
		internal const string DataGridView_RowContextMenuStripDescr = "DataGridView_RowContextMenuStripDescr";

		// Token: 0x0400030B RID: 779
		internal const string DataGridView_RowContextMenuStripNeededDescr = "DataGridView_RowContextMenuStripNeededDescr";

		// Token: 0x0400030C RID: 780
		internal const string DataGridView_RowDefaultCellStyleChangedDescr = "DataGridView_RowDefaultCellStyleChangedDescr";

		// Token: 0x0400030D RID: 781
		internal const string DataGridView_RowDefaultCellStyleDescr = "DataGridView_RowDefaultCellStyleDescr";

		// Token: 0x0400030E RID: 782
		internal const string DataGridView_RowDirtyStateNeededDescr = "DataGridView_RowDirtyStateNeededDescr";

		// Token: 0x0400030F RID: 783
		internal const string DataGridView_RowDividerDoubleClickDescr = "DataGridView_RowDividerDoubleClickDescr";

		// Token: 0x04000310 RID: 784
		internal const string DataGridView_RowDividerHeightChangedDescr = "DataGridView_RowDividerHeightChangedDescr";

		// Token: 0x04000311 RID: 785
		internal const string DataGridView_RowDividerHeightDescr = "DataGridView_RowDividerHeightDescr";

		// Token: 0x04000312 RID: 786
		internal const string DataGridView_RowDoesNotBelongToDataGridView = "DataGridView_RowDoesNotBelongToDataGridView";

		// Token: 0x04000313 RID: 787
		internal const string DataGridView_RowDoesNotYetBelongToDataGridView = "DataGridView_RowDoesNotYetBelongToDataGridView";

		// Token: 0x04000314 RID: 788
		internal const string DataGridView_RowEnterDescr = "DataGridView_RowEnterDescr";

		// Token: 0x04000315 RID: 789
		internal const string DataGridView_RowErrorTextChangedDescr = "DataGridView_RowErrorTextChangedDescr";

		// Token: 0x04000316 RID: 790
		internal const string DataGridView_RowErrorTextDescr = "DataGridView_RowErrorTextDescr";

		// Token: 0x04000317 RID: 791
		internal const string DataGridView_RowErrorTextNeededDescr = "DataGridView_RowErrorTextNeededDescr";

		// Token: 0x04000318 RID: 792
		internal const string DataGridView_RowHeaderCellAccDefaultAction = "DataGridView_RowHeaderCellAccDefaultAction";

		// Token: 0x04000319 RID: 793
		internal const string DataGridView_RowHeaderCellChangedDescr = "DataGridView_RowHeaderCellChangedDescr";

		// Token: 0x0400031A RID: 794
		internal const string DataGridView_RowHeaderMouseClickDescr = "DataGridView_RowHeaderMouseClickDescr";

		// Token: 0x0400031B RID: 795
		internal const string DataGridView_RowHeaderMouseDoubleClickDescr = "DataGridView_RowHeaderMouseDoubleClickDescr";

		// Token: 0x0400031C RID: 796
		internal const string DataGridView_RowHeadersBorderStyleChangedDescr = "DataGridView_RowHeadersBorderStyleChangedDescr";

		// Token: 0x0400031D RID: 797
		internal const string DataGridView_RowHeadersBorderStyleDescr = "DataGridView_RowHeadersBorderStyleDescr";

		// Token: 0x0400031E RID: 798
		internal const string DataGridView_RowHeadersCannotBeInvisible = "DataGridView_RowHeadersCannotBeInvisible";

		// Token: 0x0400031F RID: 799
		internal const string DataGridView_RowHeadersDefaultCellStyleDescr = "DataGridView_RowHeadersDefaultCellStyleDescr";

		// Token: 0x04000320 RID: 800
		internal const string DataGridView_RowHeadersWidthDescr = "DataGridView_RowHeadersWidthDescr";

		// Token: 0x04000321 RID: 801
		internal const string DataGridView_RowHeadersWidthSizeModeChangedDescr = "DataGridView_RowHeadersWidthSizeModeChangedDescr";

		// Token: 0x04000322 RID: 802
		internal const string DataGridView_RowHeadersWidthSizeModeDescr = "DataGridView_RowHeadersWidthSizeModeDescr";

		// Token: 0x04000323 RID: 803
		internal const string DataGridView_RowHeightChangedDescr = "DataGridView_RowHeightChangedDescr";

		// Token: 0x04000324 RID: 804
		internal const string DataGridView_RowHeightDescr = "DataGridView_RowHeightDescr";

		// Token: 0x04000325 RID: 805
		internal const string DataGridView_RowHeightInfoNeededDescr = "DataGridView_RowHeightInfoNeededDescr";

		// Token: 0x04000326 RID: 806
		internal const string DataGridView_RowHeightInfoPushedDescr = "DataGridView_RowHeightInfoPushedDescr";

		// Token: 0x04000327 RID: 807
		internal const string DataGridView_RowLeaveDescr = "DataGridView_RowLeaveDescr";

		// Token: 0x04000328 RID: 808
		internal const string DataGridView_RowMinimumHeightChangedDescr = "DataGridView_RowMinimumHeightChangedDescr";

		// Token: 0x04000329 RID: 809
		internal const string DataGridView_RowMinimumHeightDescr = "DataGridView_RowMinimumHeightDescr";

		// Token: 0x0400032A RID: 810
		internal const string DataGridView_RowMustBeUnshared = "DataGridView_RowMustBeUnshared";

		// Token: 0x0400032B RID: 811
		internal const string DataGridView_RowPostPaintDescr = "DataGridView_RowPostPaintDescr";

		// Token: 0x0400032C RID: 812
		internal const string DataGridView_RowPrePaintDescr = "DataGridView_RowPrePaintDescr";

		// Token: 0x0400032D RID: 813
		internal const string DataGridView_RowReadOnlyDescr = "DataGridView_RowReadOnlyDescr";

		// Token: 0x0400032E RID: 814
		internal const string DataGridView_RowResizableDescr = "DataGridView_RowResizableDescr";

		// Token: 0x0400032F RID: 815
		internal const string DataGridView_RowsAddedDescr = "DataGridView_RowsAddedDescr";

		// Token: 0x04000330 RID: 816
		internal const string DataGridView_RowsDefaultCellStyleDescr = "DataGridView_RowsDefaultCellStyleDescr";

		// Token: 0x04000331 RID: 817
		internal const string DataGridView_RowsRemovedDescr = "DataGridView_RowsRemovedDescr";

		// Token: 0x04000332 RID: 818
		internal const string DataGridView_RowStateChangedDescr = "DataGridView_RowStateChangedDescr";

		// Token: 0x04000333 RID: 819
		internal const string DataGridView_RowTemplateDescr = "DataGridView_RowTemplateDescr";

		// Token: 0x04000334 RID: 820
		internal const string DataGridView_RowUnsharedDescr = "DataGridView_RowUnsharedDescr";

		// Token: 0x04000335 RID: 821
		internal const string DataGridView_RowValidatedDescr = "DataGridView_RowValidatedDescr";

		// Token: 0x04000336 RID: 822
		internal const string DataGridView_RowValidatingDescr = "DataGridView_RowValidatingDescr";

		// Token: 0x04000337 RID: 823
		internal const string DataGridView_ScrollBarsDescr = "DataGridView_ScrollBarsDescr";

		// Token: 0x04000338 RID: 824
		internal const string DataGridView_ScrollDescr = "DataGridView_ScrollDescr";

		// Token: 0x04000339 RID: 825
		internal const string DataGridView_SelectionChangedDescr = "DataGridView_SelectionChangedDescr";

		// Token: 0x0400033A RID: 826
		internal const string DataGridView_SelectionModeAndSortModeClash = "DataGridView_SelectionModeAndSortModeClash";

		// Token: 0x0400033B RID: 827
		internal const string DataGridView_SelectionModeDescr = "DataGridView_SelectionModeDescr";

		// Token: 0x0400033C RID: 828
		internal const string DataGridView_SelectionModeReset = "DataGridView_SelectionModeReset";

		// Token: 0x0400033D RID: 829
		internal const string DataGridView_SetCurrentCellAddressCoreNotReentrant = "DataGridView_SetCurrentCellAddressCoreNotReentrant";

		// Token: 0x0400033E RID: 830
		internal const string DataGridView_ShowCellErrorsDescr = "DataGridView_ShowCellErrorsDescr";

		// Token: 0x0400033F RID: 831
		internal const string DataGridView_ShowCellToolTipsDescr = "DataGridView_ShowCellToolTipsDescr";

		// Token: 0x04000340 RID: 832
		internal const string DataGridView_ShowEditingIconDescr = "DataGridView_ShowEditingIconDescr";

		// Token: 0x04000341 RID: 833
		internal const string DataGridView_ShowRowErrorsDescr = "DataGridView_ShowRowErrorsDescr";

		// Token: 0x04000342 RID: 834
		internal const string DataGridView_SizeTooLarge = "DataGridView_SizeTooLarge";

		// Token: 0x04000343 RID: 835
		internal const string DataGridView_SortCompareDescr = "DataGridView_SortCompareDescr";

		// Token: 0x04000344 RID: 836
		internal const string DataGridView_SortedDescr = "DataGridView_SortedDescr";

		// Token: 0x04000345 RID: 837
		internal const string DataGridView_StandardTabDescr = "DataGridView_StandardTabDescr";

		// Token: 0x04000346 RID: 838
		internal const string DataGridView_TextBoxColumnMaxInputLengthDescr = "DataGridView_TextBoxColumnMaxInputLengthDescr";

		// Token: 0x04000347 RID: 839
		internal const string DataGridView_TransparentColor = "DataGridView_TransparentColor";

		// Token: 0x04000348 RID: 840
		internal const string DataGridView_UserAddedRowDescr = "DataGridView_UserAddedRowDescr";

		// Token: 0x04000349 RID: 841
		internal const string DataGridView_UserDeletedRowDescr = "DataGridView_UserDeletedRowDescr";

		// Token: 0x0400034A RID: 842
		internal const string DataGridView_UserDeletingRowDescr = "DataGridView_UserDeletingRowDescr";

		// Token: 0x0400034B RID: 843
		internal const string DataGridView_WeightSumCannotExceedLongMaxValue = "DataGridView_WeightSumCannotExceedLongMaxValue";

		// Token: 0x0400034C RID: 844
		internal const string DataGridView_WrongType = "DataGridView_WrongType";

		// Token: 0x0400034D RID: 845
		internal const string DataGridViewAlternatingRowsDefaultCellStyleChangedDescr = "DataGridViewAlternatingRowsDefaultCellStyleChangedDescr";

		// Token: 0x0400034E RID: 846
		internal const string DataGridViewAutoSizeColumnModeChangedDescr = "DataGridViewAutoSizeColumnModeChangedDescr";

		// Token: 0x0400034F RID: 847
		internal const string DataGridViewAutoSizeColumnsModeChangedDescr = "DataGridViewAutoSizeColumnsModeChangedDescr";

		// Token: 0x04000350 RID: 848
		internal const string DataGridViewAutoSizeRowsModeChangedDescr = "DataGridViewAutoSizeRowsModeChangedDescr";

		// Token: 0x04000351 RID: 849
		internal const string DataGridViewBackgroundColorChangedDescr = "DataGridViewBackgroundColorChangedDescr";

		// Token: 0x04000352 RID: 850
		internal const string DataGridViewBackgroundColorDescr = "DataGridViewBackgroundColorDescr";

		// Token: 0x04000353 RID: 851
		internal const string DataGridViewBand_CannotSelect = "DataGridViewBand_CannotSelect";

		// Token: 0x04000354 RID: 852
		internal const string DataGridViewBand_MinimumHeightSmallerThanOne = "DataGridViewBand_MinimumHeightSmallerThanOne";

		// Token: 0x04000355 RID: 853
		internal const string DataGridViewBand_MinimumWidthSmallerThanOne = "DataGridViewBand_MinimumWidthSmallerThanOne";

		// Token: 0x04000356 RID: 854
		internal const string DataGridViewBand_NewRowCannotBeInvisible = "DataGridViewBand_NewRowCannotBeInvisible";

		// Token: 0x04000357 RID: 855
		internal const string DataGridViewBeginInit = "DataGridViewBeginInit";

		// Token: 0x04000358 RID: 856
		internal const string DataGridViewBorderStyleChangedDescr = "DataGridViewBorderStyleChangedDescr";

		// Token: 0x04000359 RID: 857
		internal const string DataGridViewCell_CannotSetReadOnlyState = "DataGridViewCell_CannotSetReadOnlyState";

		// Token: 0x0400035A RID: 858
		internal const string DataGridViewCell_CannotSetSelectedState = "DataGridViewCell_CannotSetSelectedState";

		// Token: 0x0400035B RID: 859
		internal const string DataGridViewCell_FormattedValueHasWrongType = "DataGridViewCell_FormattedValueHasWrongType";

		// Token: 0x0400035C RID: 860
		internal const string DataGridViewCell_FormattedValueTypeNull = "DataGridViewCell_FormattedValueTypeNull";

		// Token: 0x0400035D RID: 861
		internal const string DataGridViewCell_ValueTypeNull = "DataGridViewCell_ValueTypeNull";

		// Token: 0x0400035E RID: 862
		internal const string DataGridViewCellAccessibleObject_OwnerAlreadySet = "DataGridViewCellAccessibleObject_OwnerAlreadySet";

		// Token: 0x0400035F RID: 863
		internal const string DataGridViewCellAccessibleObject_OwnerNotSet = "DataGridViewCellAccessibleObject_OwnerNotSet";

		// Token: 0x04000360 RID: 864
		internal const string DataGridViewCellCollection_AtLeastOneCellIsNull = "DataGridViewCellCollection_AtLeastOneCellIsNull";

		// Token: 0x04000361 RID: 865
		internal const string DataGridViewCellCollection_CannotAddIdenticalCells = "DataGridViewCellCollection_CannotAddIdenticalCells";

		// Token: 0x04000362 RID: 866
		internal const string DataGridViewCellCollection_CellAlreadyBelongsToDataGridView = "DataGridViewCellCollection_CellAlreadyBelongsToDataGridView";

		// Token: 0x04000363 RID: 867
		internal const string DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow = "DataGridViewCellCollection_CellAlreadyBelongsToDataGridViewRow";

		// Token: 0x04000364 RID: 868
		internal const string DataGridViewCellCollection_CellNotFound = "DataGridViewCellCollection_CellNotFound";

		// Token: 0x04000365 RID: 869
		internal const string DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView = "DataGridViewCellCollection_OwningRowAlreadyBelongsToDataGridView";

		// Token: 0x04000366 RID: 870
		internal const string DataGridViewCellStyle_TransparentColorNotAllowed = "DataGridViewCellStyle_TransparentColorNotAllowed";

		// Token: 0x04000367 RID: 871
		internal const string DataGridViewCellStyleAlignmentDescr = "DataGridViewCellStyleAlignmentDescr";

		// Token: 0x04000368 RID: 872
		internal const string DataGridViewCheckBoxCell_ClipboardChecked = "DataGridViewCheckBoxCell_ClipboardChecked";

		// Token: 0x04000369 RID: 873
		internal const string DataGridViewCheckBoxCell_ClipboardFalse = "DataGridViewCheckBoxCell_ClipboardFalse";

		// Token: 0x0400036A RID: 874
		internal const string DataGridViewCheckBoxCell_ClipboardIndeterminate = "DataGridViewCheckBoxCell_ClipboardIndeterminate";

		// Token: 0x0400036B RID: 875
		internal const string DataGridViewCheckBoxCell_ClipboardTrue = "DataGridViewCheckBoxCell_ClipboardTrue";

		// Token: 0x0400036C RID: 876
		internal const string DataGridViewCheckBoxCell_ClipboardUnchecked = "DataGridViewCheckBoxCell_ClipboardUnchecked";

		// Token: 0x0400036D RID: 877
		internal const string DataGridViewCheckBoxCell_InvalidValueType = "DataGridViewCheckBoxCell_InvalidValueType";

		// Token: 0x0400036E RID: 878
		internal const string DataGridViewColumn_AutoSizeCriteriaCannotUseInvisibleHeaders = "DataGridViewColumn_AutoSizeCriteriaCannotUseInvisibleHeaders";

		// Token: 0x0400036F RID: 879
		internal const string DataGridViewColumn_AutoSizeModeDescr = "DataGridViewColumn_AutoSizeModeDescr";

		// Token: 0x04000370 RID: 880
		internal const string DataGridViewColumn_CellTemplateRequired = "DataGridViewColumn_CellTemplateRequired";

		// Token: 0x04000371 RID: 881
		internal const string DataGridViewColumn_DisplayIndexExceedsColumnCount = "DataGridViewColumn_DisplayIndexExceedsColumnCount";

		// Token: 0x04000372 RID: 882
		internal const string DataGridViewColumn_DisplayIndexNegative = "DataGridViewColumn_DisplayIndexNegative";

		// Token: 0x04000373 RID: 883
		internal const string DataGridViewColumn_DisplayIndexTooLarge = "DataGridViewColumn_DisplayIndexTooLarge";

		// Token: 0x04000374 RID: 884
		internal const string DataGridViewColumn_DisplayIndexTooNegative = "DataGridViewColumn_DisplayIndexTooNegative";

		// Token: 0x04000375 RID: 885
		internal const string DataGridViewColumn_FillWeightDescr = "DataGridViewColumn_FillWeightDescr";

		// Token: 0x04000376 RID: 886
		internal const string DataGridViewColumn_FrozenColumnCannotAutoFill = "DataGridViewColumn_FrozenColumnCannotAutoFill";

		// Token: 0x04000377 RID: 887
		internal const string DataGridViewColumn_SortModeAndSelectionModeClash = "DataGridViewColumn_SortModeAndSelectionModeClash";

		// Token: 0x04000378 RID: 888
		internal const string DataGridViewColumnCollection_ColumnNotFound = "DataGridViewColumnCollection_ColumnNotFound";

		// Token: 0x04000379 RID: 889
		internal const string DataGridViewColumnHeaderCell_SortModeAndSortGlyphDirectionClash = "DataGridViewColumnHeaderCell_SortModeAndSortGlyphDirectionClash";

		// Token: 0x0400037A RID: 890
		internal const string DataGridViewColumnHeadersDefaultCellStyleChangedDescr = "DataGridViewColumnHeadersDefaultCellStyleChangedDescr";

		// Token: 0x0400037B RID: 891
		internal const string DataGridViewColumnHeadersHeightChangedDescr = "DataGridViewColumnHeadersHeightChangedDescr";

		// Token: 0x0400037C RID: 892
		internal const string DataGridViewColumnHeadersVisibleDescr = "DataGridViewColumnHeadersVisibleDescr";

		// Token: 0x0400037D RID: 893
		internal const string DataGridViewColumnSortModeChangedDescr = "DataGridViewColumnSortModeChangedDescr";

		// Token: 0x0400037E RID: 894
		internal const string DataGridViewComboBoxCell_DropDownWidthOutOfRange = "DataGridViewComboBoxCell_DropDownWidthOutOfRange";

		// Token: 0x0400037F RID: 895
		internal const string DataGridViewComboBoxCell_FieldNotFound = "DataGridViewComboBoxCell_FieldNotFound";

		// Token: 0x04000380 RID: 896
		internal const string DataGridViewComboBoxCell_InvalidValue = "DataGridViewComboBoxCell_InvalidValue";

		// Token: 0x04000381 RID: 897
		internal const string DataGridViewComboBoxCell_MaxDropDownItemsOutOfRange = "DataGridViewComboBoxCell_MaxDropDownItemsOutOfRange";

		// Token: 0x04000382 RID: 898
		internal const string DataGridViewDataMemberChangedDescr = "DataGridViewDataMemberChangedDescr";

		// Token: 0x04000383 RID: 899
		internal const string DataGridViewDataMemberDescr = "DataGridViewDataMemberDescr";

		// Token: 0x04000384 RID: 900
		internal const string DataGridViewDataSourceChangedDescr = "DataGridViewDataSourceChangedDescr";

		// Token: 0x04000385 RID: 901
		internal const string DataGridViewDataSourceDescr = "DataGridViewDataSourceDescr";

		// Token: 0x04000386 RID: 902
		internal const string DataGridViewDefaultCellStyleChangedDescr = "DataGridViewDefaultCellStyleChangedDescr";

		// Token: 0x04000387 RID: 903
		internal const string DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange = "DataGridViewElementPaintingEventArgs_ColumnIndexOutOfRange";

		// Token: 0x04000388 RID: 904
		internal const string DataGridViewElementPaintingEventArgs_RowIndexOutOfRange = "DataGridViewElementPaintingEventArgs_RowIndexOutOfRange";

		// Token: 0x04000389 RID: 905
		internal const string DataGridViewGridColorDescr = "DataGridViewGridColorDescr";

		// Token: 0x0400038A RID: 906
		internal const string DataGridViewImageColumn_DescriptionDescr = "DataGridViewImageColumn_DescriptionDescr";

		// Token: 0x0400038B RID: 907
		internal const string DataGridViewImageColumn_IconDescr = "DataGridViewImageColumn_IconDescr";

		// Token: 0x0400038C RID: 908
		internal const string DataGridViewImageColumn_ImageDescr = "DataGridViewImageColumn_ImageDescr";

		// Token: 0x0400038D RID: 909
		internal const string DataGridViewImageColumn_ImageLayoutDescr = "DataGridViewImageColumn_ImageLayoutDescr";

		// Token: 0x0400038E RID: 910
		internal const string DataGridViewImageColumn_PaddingDescr = "DataGridViewImageColumn_PaddingDescr";

		// Token: 0x0400038F RID: 911
		internal const string DataGridViewImageColumn_ValuesAreIconsDescr = "DataGridViewImageColumn_ValuesAreIconsDescr";

		// Token: 0x04000390 RID: 912
		internal const string DataGridViewOnAllowUserToAddRowsChangedDescr = "DataGridViewOnAllowUserToAddRowsChangedDescr";

		// Token: 0x04000391 RID: 913
		internal const string DataGridViewOnAllowUserToDeleteRowsChangedDescr = "DataGridViewOnAllowUserToDeleteRowsChangedDescr";

		// Token: 0x04000392 RID: 914
		internal const string DataGridViewOnAllowUserToOrderColumnsChangedDescr = "DataGridViewOnAllowUserToOrderColumnsChangedDescr";

		// Token: 0x04000393 RID: 915
		internal const string DataGridViewOnAllowUserToResizeColumnsChangedDescr = "DataGridViewOnAllowUserToResizeColumnsChangedDescr";

		// Token: 0x04000394 RID: 916
		internal const string DataGridViewOnAllowUserToResizeRowsChangedDescr = "DataGridViewOnAllowUserToResizeRowsChangedDescr";

		// Token: 0x04000395 RID: 917
		internal const string DataGridViewOnGridColorChangedDescr = "DataGridViewOnGridColorChangedDescr";

		// Token: 0x04000396 RID: 918
		internal const string DataGridViewOnMultiSelectChangedDescr = "DataGridViewOnMultiSelectChangedDescr";

		// Token: 0x04000397 RID: 919
		internal const string DataGridViewOnReadOnlyChangedDescr = "DataGridViewOnReadOnlyChangedDescr";

		// Token: 0x04000398 RID: 920
		internal const string DataGridViewRowAccessibleObject_OwnerAlreadySet = "DataGridViewRowAccessibleObject_OwnerAlreadySet";

		// Token: 0x04000399 RID: 921
		internal const string DataGridViewRowAccessibleObject_OwnerNotSet = "DataGridViewRowAccessibleObject_OwnerNotSet";

		// Token: 0x0400039A RID: 922
		internal const string DataGridViewRowCollection_AddUnboundRow = "DataGridViewRowCollection_AddUnboundRow";

		// Token: 0x0400039B RID: 923
		internal const string DataGridViewRowCollection_CannotAddOrInsertSelectedRow = "DataGridViewRowCollection_CannotAddOrInsertSelectedRow";

		// Token: 0x0400039C RID: 924
		internal const string DataGridViewRowCollection_CannotDeleteNewRow = "DataGridViewRowCollection_CannotDeleteNewRow";

		// Token: 0x0400039D RID: 925
		internal const string DataGridViewRowCollection_CantClearRowCollectionWithWrongSource = "DataGridViewRowCollection_CantClearRowCollectionWithWrongSource";

		// Token: 0x0400039E RID: 926
		internal const string DataGridViewRowCollection_CantRemoveRowsWithWrongSource = "DataGridViewRowCollection_CantRemoveRowsWithWrongSource";

		// Token: 0x0400039F RID: 927
		internal const string DataGridViewRowCollection_CountOutOfRange = "DataGridViewRowCollection_CountOutOfRange";

		// Token: 0x040003A0 RID: 928
		internal const string DataGridViewRowCollection_EnumFinished = "DataGridViewRowCollection_EnumFinished";

		// Token: 0x040003A1 RID: 929
		internal const string DataGridViewRowCollection_EnumNotStarted = "DataGridViewRowCollection_EnumNotStarted";

		// Token: 0x040003A2 RID: 930
		internal const string DataGridViewRowCollection_IndexDestinationOutOfRange = "DataGridViewRowCollection_IndexDestinationOutOfRange";

		// Token: 0x040003A3 RID: 931
		internal const string DataGridViewRowCollection_IndexSourceOutOfRange = "DataGridViewRowCollection_IndexSourceOutOfRange";

		// Token: 0x040003A4 RID: 932
		internal const string DataGridViewRowCollection_NoColumns = "DataGridViewRowCollection_NoColumns";

		// Token: 0x040003A5 RID: 933
		internal const string DataGridViewRowCollection_NoInsertionAfterNewRow = "DataGridViewRowCollection_NoInsertionAfterNewRow";

		// Token: 0x040003A6 RID: 934
		internal const string DataGridViewRowCollection_NoRowToDuplicate = "DataGridViewRowCollection_NoRowToDuplicate";

		// Token: 0x040003A7 RID: 935
		internal const string DataGridViewRowCollection_RowIndexOutOfRange = "DataGridViewRowCollection_RowIndexOutOfRange";

		// Token: 0x040003A8 RID: 936
		internal const string DataGridViewRowCollection_RowTemplateTooManyCells = "DataGridViewRowCollection_RowTemplateTooManyCells";

		// Token: 0x040003A9 RID: 937
		internal const string DataGridViewRowCollection_TooManyCells = "DataGridViewRowCollection_TooManyCells";

		// Token: 0x040003AA RID: 938
		internal const string DataGridViewRowHeadersDefaultCellStyleChangedDescr = "DataGridViewRowHeadersDefaultCellStyleChangedDescr";

		// Token: 0x040003AB RID: 939
		internal const string DataGridViewRowHeadersVisibleDescr = "DataGridViewRowHeadersVisibleDescr";

		// Token: 0x040003AC RID: 940
		internal const string DataGridViewRowHeadersWidthChangedDescr = "DataGridViewRowHeadersWidthChangedDescr";

		// Token: 0x040003AD RID: 941
		internal const string DataGridViewRowsDefaultCellStyleChangedDescr = "DataGridViewRowsDefaultCellStyleChangedDescr";

		// Token: 0x040003AE RID: 942
		internal const string DataGridViewTopRowAccessibleObject_OwnerAlreadySet = "DataGridViewTopRowAccessibleObject_OwnerAlreadySet";

		// Token: 0x040003AF RID: 943
		internal const string DataGridViewTopRowAccessibleObject_OwnerNotSet = "DataGridViewTopRowAccessibleObject_OwnerNotSet";

		// Token: 0x040003B0 RID: 944
		internal const string DataGridViewTypeColumn_WrongCellTemplateType = "DataGridViewTypeColumn_WrongCellTemplateType";

		// Token: 0x040003B1 RID: 945
		internal const string DataGridViewVirtualModeDescr = "DataGridViewVirtualModeDescr";

		// Token: 0x040003B2 RID: 946
		internal const string DataGridVisibleColumnCountDescr = "DataGridVisibleColumnCountDescr";

		// Token: 0x040003B3 RID: 947
		internal const string DataGridVisibleRowCountDescr = "DataGridVisibleRowCountDescr";

		// Token: 0x040003B4 RID: 948
		internal const string DataObjectDibNotSupported = "DataObjectDibNotSupported";

		// Token: 0x040003B5 RID: 949
		internal const string DataSourceDataMemberPropNotFound = "DataSourceDataMemberPropNotFound";

		// Token: 0x040003B6 RID: 950
		internal const string DataSourceLocksItems = "DataSourceLocksItems";

		// Token: 0x040003B7 RID: 951
		internal const string DataStreamWrite = "DataStreamWrite";

		// Token: 0x040003B8 RID: 952
		internal const string DateTimePickerCalendarFontDescr = "DateTimePickerCalendarFontDescr";

		// Token: 0x040003B9 RID: 953
		internal const string DateTimePickerCalendarForeColorDescr = "DateTimePickerCalendarForeColorDescr";

		// Token: 0x040003BA RID: 954
		internal const string DateTimePickerCalendarMonthBackgroundDescr = "DateTimePickerCalendarMonthBackgroundDescr";

		// Token: 0x040003BB RID: 955
		internal const string DateTimePickerCalendarTitleBackColorDescr = "DateTimePickerCalendarTitleBackColorDescr";

		// Token: 0x040003BC RID: 956
		internal const string DateTimePickerCalendarTitleForeColorDescr = "DateTimePickerCalendarTitleForeColorDescr";

		// Token: 0x040003BD RID: 957
		internal const string DateTimePickerCalendarTrailingForeColorDescr = "DateTimePickerCalendarTrailingForeColorDescr";

		// Token: 0x040003BE RID: 958
		internal const string DateTimePickerCheckedDescr = "DateTimePickerCheckedDescr";

		// Token: 0x040003BF RID: 959
		internal const string DateTimePickerCustomFormatDescr = "DateTimePickerCustomFormatDescr";

		// Token: 0x040003C0 RID: 960
		internal const string DateTimePickerDropDownAlignDescr = "DateTimePickerDropDownAlignDescr";

		// Token: 0x040003C1 RID: 961
		internal const string DateTimePickerFormatDescr = "DateTimePickerFormatDescr";

		// Token: 0x040003C2 RID: 962
		internal const string DateTimePickerMaxDate = "DateTimePickerMaxDate";

		// Token: 0x040003C3 RID: 963
		internal const string DateTimePickerMaxDateDescr = "DateTimePickerMaxDateDescr";

		// Token: 0x040003C4 RID: 964
		internal const string DateTimePickerMinDate = "DateTimePickerMinDate";

		// Token: 0x040003C5 RID: 965
		internal const string DateTimePickerMinDateDescr = "DateTimePickerMinDateDescr";

		// Token: 0x040003C6 RID: 966
		internal const string DateTimePickerOnCloseUpDescr = "DateTimePickerOnCloseUpDescr";

		// Token: 0x040003C7 RID: 967
		internal const string DateTimePickerOnDropDownDescr = "DateTimePickerOnDropDownDescr";

		// Token: 0x040003C8 RID: 968
		internal const string DateTimePickerOnFormatChangedDescr = "DateTimePickerOnFormatChangedDescr";

		// Token: 0x040003C9 RID: 969
		internal const string DateTimePickerShowNoneDescr = "DateTimePickerShowNoneDescr";

		// Token: 0x040003CA RID: 970
		internal const string DateTimePickerShowUpDownDescr = "DateTimePickerShowUpDownDescr";

		// Token: 0x040003CB RID: 971
		internal const string DateTimePickerValueDescr = "DateTimePickerValueDescr";

		// Token: 0x040003CC RID: 972
		internal const string DebuggingExceptionOnly = "DebuggingExceptionOnly";

		// Token: 0x040003CD RID: 973
		internal const string DefManifestNotFound = "DefManifestNotFound";

		// Token: 0x040003CE RID: 974
		internal const string DescriptionBindingNavigator = "DescriptionBindingNavigator";

		// Token: 0x040003CF RID: 975
		internal const string DescriptionBindingSource = "DescriptionBindingSource";

		// Token: 0x040003D0 RID: 976
		internal const string DescriptionButton = "DescriptionButton";

		// Token: 0x040003D1 RID: 977
		internal const string DescriptionCheckBox = "DescriptionCheckBox";

		// Token: 0x040003D2 RID: 978
		internal const string DescriptionCheckedListBox = "DescriptionCheckedListBox";

		// Token: 0x040003D3 RID: 979
		internal const string DescriptionColorDialog = "DescriptionColorDialog";

		// Token: 0x040003D4 RID: 980
		internal const string DescriptionComboBox = "DescriptionComboBox";

		// Token: 0x040003D5 RID: 981
		internal const string DescriptionContextMenuStrip = "DescriptionContextMenuStrip";

		// Token: 0x040003D6 RID: 982
		internal const string DescriptionDataGridView = "DescriptionDataGridView";

		// Token: 0x040003D7 RID: 983
		internal const string DescriptionDateTimePicker = "DescriptionDateTimePicker";

		// Token: 0x040003D8 RID: 984
		internal const string DescriptionDomainUpDown = "DescriptionDomainUpDown";

		// Token: 0x040003D9 RID: 985
		internal const string DescriptionErrorProvider = "DescriptionErrorProvider";

		// Token: 0x040003DA RID: 986
		internal const string DescriptionFlowLayoutPanel = "DescriptionFlowLayoutPanel";

		// Token: 0x040003DB RID: 987
		internal const string DescriptionFolderBrowserDialog = "DescriptionFolderBrowserDialog";

		// Token: 0x040003DC RID: 988
		internal const string DescriptionFontDialog = "DescriptionFontDialog";

		// Token: 0x040003DD RID: 989
		internal const string DescriptionGroupBox = "DescriptionGroupBox";

		// Token: 0x040003DE RID: 990
		internal const string DescriptionHelpProvider = "DescriptionHelpProvider";

		// Token: 0x040003DF RID: 991
		internal const string DescriptionHScrollBar = "DescriptionHScrollBar";

		// Token: 0x040003E0 RID: 992
		internal const string DescriptionImageList = "DescriptionImageList";

		// Token: 0x040003E1 RID: 993
		internal const string DescriptionLabel = "DescriptionLabel";

		// Token: 0x040003E2 RID: 994
		internal const string DescriptionLinkLabel = "DescriptionLinkLabel";

		// Token: 0x040003E3 RID: 995
		internal const string DescriptionListBox = "DescriptionListBox";

		// Token: 0x040003E4 RID: 996
		internal const string DescriptionListView = "DescriptionListView";

		// Token: 0x040003E5 RID: 997
		internal const string DescriptionMaskedTextBox = "DescriptionMaskedTextBox";

		// Token: 0x040003E6 RID: 998
		internal const string DescriptionMenuStrip = "DescriptionMenuStrip";

		// Token: 0x040003E7 RID: 999
		internal const string DescriptionMonthCalendar = "DescriptionMonthCalendar";

		// Token: 0x040003E8 RID: 1000
		internal const string DescriptionNotifyIcon = "DescriptionNotifyIcon";

		// Token: 0x040003E9 RID: 1001
		internal const string DescriptionNumericUpDown = "DescriptionNumericUpDown";

		// Token: 0x040003EA RID: 1002
		internal const string DescriptionOpenFileDialog = "DescriptionOpenFileDialog";

		// Token: 0x040003EB RID: 1003
		internal const string DescriptionPageSetupDialog = "DescriptionPageSetupDialog";

		// Token: 0x040003EC RID: 1004
		internal const string DescriptionPanel = "DescriptionPanel";

		// Token: 0x040003ED RID: 1005
		internal const string DescriptionPictureBox = "DescriptionPictureBox";

		// Token: 0x040003EE RID: 1006
		internal const string DescriptionPrintDialog = "DescriptionPrintDialog";

		// Token: 0x040003EF RID: 1007
		internal const string DescriptionPrintPreviewControl = "DescriptionPrintPreviewControl";

		// Token: 0x040003F0 RID: 1008
		internal const string DescriptionPrintPreviewDialog = "DescriptionPrintPreviewDialog";

		// Token: 0x040003F1 RID: 1009
		internal const string DescriptionProgressBar = "DescriptionProgressBar";

		// Token: 0x040003F2 RID: 1010
		internal const string DescriptionPropertyGrid = "DescriptionPropertyGrid";

		// Token: 0x040003F3 RID: 1011
		internal const string DescriptionRadioButton = "DescriptionRadioButton";

		// Token: 0x040003F4 RID: 1012
		internal const string DescriptionRichTextBox = "DescriptionRichTextBox";

		// Token: 0x040003F5 RID: 1013
		internal const string DescriptionSaveFileDialog = "DescriptionSaveFileDialog";

		// Token: 0x040003F6 RID: 1014
		internal const string DescriptionSplitContainer = "DescriptionSplitContainer";

		// Token: 0x040003F7 RID: 1015
		internal const string DescriptionSplitter = "DescriptionSplitter";

		// Token: 0x040003F8 RID: 1016
		internal const string DescriptionStatusStrip = "DescriptionStatusStrip";

		// Token: 0x040003F9 RID: 1017
		internal const string DescriptionTabControl = "DescriptionTabControl";

		// Token: 0x040003FA RID: 1018
		internal const string DescriptionTableLayoutPanel = "DescriptionTableLayoutPanel";

		// Token: 0x040003FB RID: 1019
		internal const string DescriptionTextBox = "DescriptionTextBox";

		// Token: 0x040003FC RID: 1020
		internal const string DescriptionTimer = "DescriptionTimer";

		// Token: 0x040003FD RID: 1021
		internal const string DescriptionToolStrip = "DescriptionToolStrip";

		// Token: 0x040003FE RID: 1022
		internal const string DescriptionToolTip = "DescriptionToolTip";

		// Token: 0x040003FF RID: 1023
		internal const string DescriptionTrackBar = "DescriptionTrackBar";

		// Token: 0x04000400 RID: 1024
		internal const string DescriptionTreeView = "DescriptionTreeView";

		// Token: 0x04000401 RID: 1025
		internal const string DescriptionVScrollBar = "DescriptionVScrollBar";

		// Token: 0x04000402 RID: 1026
		internal const string DescriptionWebBrowser = "DescriptionWebBrowser";

		// Token: 0x04000403 RID: 1027
		internal const string DispInvokeFailed = "DispInvokeFailed";

		// Token: 0x04000404 RID: 1028
		internal const string DomainUpDownItemsDescr = "DomainUpDownItemsDescr";

		// Token: 0x04000405 RID: 1029
		internal const string DomainUpDownOnSelectedItemChangedDescr = "DomainUpDownOnSelectedItemChangedDescr";

		// Token: 0x04000406 RID: 1030
		internal const string DomainUpDownSelectedIndexDescr = "DomainUpDownSelectedIndexDescr";

		// Token: 0x04000407 RID: 1031
		internal const string DomainUpDownSelectedItemDescr = "DomainUpDownSelectedItemDescr";

		// Token: 0x04000408 RID: 1032
		internal const string DomainUpDownSortedDescr = "DomainUpDownSortedDescr";

		// Token: 0x04000409 RID: 1033
		internal const string DomainUpDownWrapDescr = "DomainUpDownWrapDescr";

		// Token: 0x0400040A RID: 1034
		internal const string DragDropRegFailed = "DragDropRegFailed";

		// Token: 0x0400040B RID: 1035
		internal const string drawItemEventDescr = "drawItemEventDescr";

		// Token: 0x0400040C RID: 1036
		internal const string ErrorBadInputLanguage = "ErrorBadInputLanguage";

		// Token: 0x0400040D RID: 1037
		internal const string ErrorCollectionFull = "ErrorCollectionFull";

		// Token: 0x0400040E RID: 1038
		internal const string ErrorCreatingHandle = "ErrorCreatingHandle";

		// Token: 0x0400040F RID: 1039
		internal const string ErrorNoMarshalingThread = "ErrorNoMarshalingThread";

		// Token: 0x04000410 RID: 1040
		internal const string ErrorPropertyPageFailed = "ErrorPropertyPageFailed";

		// Token: 0x04000411 RID: 1041
		internal const string ErrorProviderBlinkRateDescr = "ErrorProviderBlinkRateDescr";

		// Token: 0x04000412 RID: 1042
		internal const string ErrorProviderBlinkStyleDescr = "ErrorProviderBlinkStyleDescr";

		// Token: 0x04000413 RID: 1043
		internal const string ErrorProviderContainerControlDescr = "ErrorProviderContainerControlDescr";

		// Token: 0x04000414 RID: 1044
		internal const string ErrorProviderDataMemberDescr = "ErrorProviderDataMemberDescr";

		// Token: 0x04000415 RID: 1045
		internal const string ErrorProviderDataSourceDescr = "ErrorProviderDataSourceDescr";

		// Token: 0x04000416 RID: 1046
		internal const string ErrorProviderErrorDescr = "ErrorProviderErrorDescr";

		// Token: 0x04000417 RID: 1047
		internal const string ErrorProviderIconAlignmentDescr = "ErrorProviderIconAlignmentDescr";

		// Token: 0x04000418 RID: 1048
		internal const string ErrorProviderIconDescr = "ErrorProviderIconDescr";

		// Token: 0x04000419 RID: 1049
		internal const string ErrorProviderIconPaddingDescr = "ErrorProviderIconPaddingDescr";

		// Token: 0x0400041A RID: 1050
		internal const string ErrorSettingWindowRegion = "ErrorSettingWindowRegion";

		// Token: 0x0400041B RID: 1051
		internal const string ErrorTypeConverterFailed = "ErrorTypeConverterFailed";

		// Token: 0x0400041C RID: 1052
		internal const string ExceptionCreatingCompEditorControl = "ExceptionCreatingCompEditorControl";

		// Token: 0x0400041D RID: 1053
		internal const string ExceptionCreatingObject = "ExceptionCreatingObject";

		// Token: 0x0400041E RID: 1054
		internal const string ExDlgCancel = "ExDlgCancel";

		// Token: 0x0400041F RID: 1055
		internal const string ExDlgCaption = "ExDlgCaption";

		// Token: 0x04000420 RID: 1056
		internal const string ExDlgCaption2 = "ExDlgCaption2";

		// Token: 0x04000421 RID: 1057
		internal const string ExDlgContinue = "ExDlgContinue";

		// Token: 0x04000422 RID: 1058
		internal const string ExDlgContinueErrorText = "ExDlgContinueErrorText";

		// Token: 0x04000423 RID: 1059
		internal const string ExDlgDetailsText = "ExDlgDetailsText";

		// Token: 0x04000424 RID: 1060
		internal const string ExDlgErrorText = "ExDlgErrorText";

		// Token: 0x04000425 RID: 1061
		internal const string ExDlgHelp = "ExDlgHelp";

		// Token: 0x04000426 RID: 1062
		internal const string ExDlgMsgExceptionSection = "ExDlgMsgExceptionSection";

		// Token: 0x04000427 RID: 1063
		internal const string ExDlgMsgFooterNonSwitchable = "ExDlgMsgFooterNonSwitchable";

		// Token: 0x04000428 RID: 1064
		internal const string ExDlgMsgFooterSwitchable = "ExDlgMsgFooterSwitchable";

		// Token: 0x04000429 RID: 1065
		internal const string ExDlgMsgHeaderNonSwitchable = "ExDlgMsgHeaderNonSwitchable";

		// Token: 0x0400042A RID: 1066
		internal const string ExDlgMsgHeaderSwitchable = "ExDlgMsgHeaderSwitchable";

		// Token: 0x0400042B RID: 1067
		internal const string ExDlgMsgJITDebuggingSection = "ExDlgMsgJITDebuggingSection";

		// Token: 0x0400042C RID: 1068
		internal const string ExDlgMsgLoadedAssembliesEntry = "ExDlgMsgLoadedAssembliesEntry";

		// Token: 0x0400042D RID: 1069
		internal const string ExDlgMsgLoadedAssembliesSection = "ExDlgMsgLoadedAssembliesSection";

		// Token: 0x0400042E RID: 1070
		internal const string ExDlgMsgSectionSeperator = "ExDlgMsgSectionSeperator";

		// Token: 0x0400042F RID: 1071
		internal const string ExDlgMsgSeperator = "ExDlgMsgSeperator";

		// Token: 0x04000430 RID: 1072
		internal const string ExDlgOk = "ExDlgOk";

		// Token: 0x04000431 RID: 1073
		internal const string ExDlgQuit = "ExDlgQuit";

		// Token: 0x04000432 RID: 1074
		internal const string ExDlgSecurityContinueErrorText = "ExDlgSecurityContinueErrorText";

		// Token: 0x04000433 RID: 1075
		internal const string ExDlgSecurityErrorText = "ExDlgSecurityErrorText";

		// Token: 0x04000434 RID: 1076
		internal const string ExDlgShowDetails = "ExDlgShowDetails";

		// Token: 0x04000435 RID: 1077
		internal const string ExDlgWarningText = "ExDlgWarningText";

		// Token: 0x04000436 RID: 1078
		internal const string ExternalException = "ExternalException";

		// Token: 0x04000437 RID: 1079
		internal const string FDaddExtensionDescr = "FDaddExtensionDescr";

		// Token: 0x04000438 RID: 1080
		internal const string FDcheckFileExistsDescr = "FDcheckFileExistsDescr";

		// Token: 0x04000439 RID: 1081
		internal const string FDcheckPathExistsDescr = "FDcheckPathExistsDescr";

		// Token: 0x0400043A RID: 1082
		internal const string FDdefaultExtDescr = "FDdefaultExtDescr";

		// Token: 0x0400043B RID: 1083
		internal const string FDdereferenceLinksDescr = "FDdereferenceLinksDescr";

		// Token: 0x0400043C RID: 1084
		internal const string FDfileNameDescr = "FDfileNameDescr";

		// Token: 0x0400043D RID: 1085
		internal const string FDFileNamesDescr = "FDFileNamesDescr";

		// Token: 0x0400043E RID: 1086
		internal const string FDfileOkDescr = "FDfileOkDescr";

		// Token: 0x0400043F RID: 1087
		internal const string FDfilterDescr = "FDfilterDescr";

		// Token: 0x04000440 RID: 1088
		internal const string FDfilterIndexDescr = "FDfilterIndexDescr";

		// Token: 0x04000441 RID: 1089
		internal const string FDinitialDirDescr = "FDinitialDirDescr";

		// Token: 0x04000442 RID: 1090
		internal const string FDrestoreDirectoryDescr = "FDrestoreDirectoryDescr";

		// Token: 0x04000443 RID: 1091
		internal const string FDshowHelpDescr = "FDshowHelpDescr";

		// Token: 0x04000444 RID: 1092
		internal const string FDsupportMultiDottedExtensionsDescr = "FDsupportMultiDottedExtensionsDescr";

		// Token: 0x04000445 RID: 1093
		internal const string FDtitleDescr = "FDtitleDescr";

		// Token: 0x04000446 RID: 1094
		internal const string FDvalidateNamesDescr = "FDvalidateNamesDescr";

		// Token: 0x04000447 RID: 1095
		internal const string FileDialogBufferTooSmall = "FileDialogBufferTooSmall";

		// Token: 0x04000448 RID: 1096
		internal const string FileDialogCreatePrompt = "FileDialogCreatePrompt";

		// Token: 0x04000449 RID: 1097
		internal const string FileDialogFileNotFound = "FileDialogFileNotFound";

		// Token: 0x0400044A RID: 1098
		internal const string FileDialogInvalidFileName = "FileDialogInvalidFileName";

		// Token: 0x0400044B RID: 1099
		internal const string FileDialogInvalidFilter = "FileDialogInvalidFilter";

		// Token: 0x0400044C RID: 1100
		internal const string FileDialogInvalidFilterIndex = "FileDialogInvalidFilterIndex";

		// Token: 0x0400044D RID: 1101
		internal const string FileDialogOverwritePrompt = "FileDialogOverwritePrompt";

		// Token: 0x0400044E RID: 1102
		internal const string FileDialogSubLassFailure = "FileDialogSubLassFailure";

		// Token: 0x0400044F RID: 1103
		internal const string FilterRequiresIBindingListView = "FilterRequiresIBindingListView";

		// Token: 0x04000450 RID: 1104
		internal const string FindKeyMayNotBeEmptyOrNull = "FindKeyMayNotBeEmptyOrNull";

		// Token: 0x04000451 RID: 1105
		internal const string FlowPanelFlowDirectionDescr = "FlowPanelFlowDirectionDescr";

		// Token: 0x04000452 RID: 1106
		internal const string FlowPanelWrapContentsDescr = "FlowPanelWrapContentsDescr";

		// Token: 0x04000453 RID: 1107
		internal const string FnDallowScriptChangeDescr = "FnDallowScriptChangeDescr";

		// Token: 0x04000454 RID: 1108
		internal const string FnDallowSimulationsDescr = "FnDallowSimulationsDescr";

		// Token: 0x04000455 RID: 1109
		internal const string FnDallowVectorFontsDescr = "FnDallowVectorFontsDescr";

		// Token: 0x04000456 RID: 1110
		internal const string FnDallowVerticalFontsDescr = "FnDallowVerticalFontsDescr";

		// Token: 0x04000457 RID: 1111
		internal const string FnDapplyDescr = "FnDapplyDescr";

		// Token: 0x04000458 RID: 1112
		internal const string FnDcolorDescr = "FnDcolorDescr";

		// Token: 0x04000459 RID: 1113
		internal const string FnDfixedPitchOnlyDescr = "FnDfixedPitchOnlyDescr";

		// Token: 0x0400045A RID: 1114
		internal const string FnDfontDescr = "FnDfontDescr";

		// Token: 0x0400045B RID: 1115
		internal const string FnDfontMustExistDescr = "FnDfontMustExistDescr";

		// Token: 0x0400045C RID: 1116
		internal const string FnDmaxSizeDescr = "FnDmaxSizeDescr";

		// Token: 0x0400045D RID: 1117
		internal const string FnDminSizeDescr = "FnDminSizeDescr";

		// Token: 0x0400045E RID: 1118
		internal const string FnDscriptsOnlyDescr = "FnDscriptsOnlyDescr";

		// Token: 0x0400045F RID: 1119
		internal const string FnDshowApplyDescr = "FnDshowApplyDescr";

		// Token: 0x04000460 RID: 1120
		internal const string FnDshowColorDescr = "FnDshowColorDescr";

		// Token: 0x04000461 RID: 1121
		internal const string FnDshowEffectsDescr = "FnDshowEffectsDescr";

		// Token: 0x04000462 RID: 1122
		internal const string FnDshowHelpDescr = "FnDshowHelpDescr";

		// Token: 0x04000463 RID: 1123
		internal const string FolderBrowserDialogDescription = "FolderBrowserDialogDescription";

		// Token: 0x04000464 RID: 1124
		internal const string FolderBrowserDialogNoRootFolder = "FolderBrowserDialogNoRootFolder";

		// Token: 0x04000465 RID: 1125
		internal const string FolderBrowserDialogRootFolder = "FolderBrowserDialogRootFolder";

		// Token: 0x04000466 RID: 1126
		internal const string FolderBrowserDialogSelectedPath = "FolderBrowserDialogSelectedPath";

		// Token: 0x04000467 RID: 1127
		internal const string FolderBrowserDialogShowNewFolderButton = "FolderBrowserDialogShowNewFolderButton";

		// Token: 0x04000468 RID: 1128
		internal const string FormAcceptButtonDescr = "FormAcceptButtonDescr";

		// Token: 0x04000469 RID: 1129
		internal const string FormActiveMDIChildDescr = "FormActiveMDIChildDescr";

		// Token: 0x0400046A RID: 1130
		internal const string FormatControlFormatDescr = "FormatControlFormatDescr";

		// Token: 0x0400046B RID: 1131
		internal const string Formatter_CantConvert = "Formatter_CantConvert";

		// Token: 0x0400046C RID: 1132
		internal const string Formatter_CantConvertNull = "Formatter_CantConvertNull";

		// Token: 0x0400046D RID: 1133
		internal const string FormAutoScaleDescr = "FormAutoScaleDescr";

		// Token: 0x0400046E RID: 1134
		internal const string FormAutoScrollDescr = "FormAutoScrollDescr";

		// Token: 0x0400046F RID: 1135
		internal const string FormAutoScrollMarginDescr = "FormAutoScrollMarginDescr";

		// Token: 0x04000470 RID: 1136
		internal const string FormAutoScrollMinSizeDescr = "FormAutoScrollMinSizeDescr";

		// Token: 0x04000471 RID: 1137
		internal const string FormAutoScrollPositionDescr = "FormAutoScrollPositionDescr";

		// Token: 0x04000472 RID: 1138
		internal const string FormBorderStyleDescr = "FormBorderStyleDescr";

		// Token: 0x04000473 RID: 1139
		internal const string FormCancelButtonDescr = "FormCancelButtonDescr";

		// Token: 0x04000474 RID: 1140
		internal const string FormControlBoxDescr = "FormControlBoxDescr";

		// Token: 0x04000475 RID: 1141
		internal const string FormDesktopBoundsDescr = "FormDesktopBoundsDescr";

		// Token: 0x04000476 RID: 1142
		internal const string FormDesktopLocationDescr = "FormDesktopLocationDescr";

		// Token: 0x04000477 RID: 1143
		internal const string FormDialogResultDescr = "FormDialogResultDescr";

		// Token: 0x04000478 RID: 1144
		internal const string FormHelpButtonClickedDescr = "FormHelpButtonClickedDescr";

		// Token: 0x04000479 RID: 1145
		internal const string FormHelpButtonDescr = "FormHelpButtonDescr";

		// Token: 0x0400047A RID: 1146
		internal const string FormIconDescr = "FormIconDescr";

		// Token: 0x0400047B RID: 1147
		internal const string FormIsMDIChildDescr = "FormIsMDIChildDescr";

		// Token: 0x0400047C RID: 1148
		internal const string FormIsMDIContainerDescr = "FormIsMDIContainerDescr";

		// Token: 0x0400047D RID: 1149
		internal const string FormKeyPreviewDescr = "FormKeyPreviewDescr";

		// Token: 0x0400047E RID: 1150
		internal const string FormMaximizeBoxDescr = "FormMaximizeBoxDescr";

		// Token: 0x0400047F RID: 1151
		internal const string FormMaximumSizeDescr = "FormMaximumSizeDescr";

		// Token: 0x04000480 RID: 1152
		internal const string FormMDIChildrenDescr = "FormMDIChildrenDescr";

		// Token: 0x04000481 RID: 1153
		internal const string FormMDIParentAndChild = "FormMDIParentAndChild";

		// Token: 0x04000482 RID: 1154
		internal const string FormMDIParentCannotAdd = "FormMDIParentCannotAdd";

		// Token: 0x04000483 RID: 1155
		internal const string FormMDIParentDescr = "FormMDIParentDescr";

		// Token: 0x04000484 RID: 1156
		internal const string FormMenuDescr = "FormMenuDescr";

		// Token: 0x04000485 RID: 1157
		internal const string FormMenuStripDescr = "FormMenuStripDescr";

		// Token: 0x04000486 RID: 1158
		internal const string FormMergedMenuDescr = "FormMergedMenuDescr";

		// Token: 0x04000487 RID: 1159
		internal const string FormMinimizeBoxDescr = "FormMinimizeBoxDescr";

		// Token: 0x04000488 RID: 1160
		internal const string FormMinimumSizeDescr = "FormMinimumSizeDescr";

		// Token: 0x04000489 RID: 1161
		internal const string FormModalDescr = "FormModalDescr";

		// Token: 0x0400048A RID: 1162
		internal const string FormOnActivateDescr = "FormOnActivateDescr";

		// Token: 0x0400048B RID: 1163
		internal const string FormOnClosedDescr = "FormOnClosedDescr";

		// Token: 0x0400048C RID: 1164
		internal const string FormOnClosingDescr = "FormOnClosingDescr";

		// Token: 0x0400048D RID: 1165
		internal const string FormOnDeactivateDescr = "FormOnDeactivateDescr";

		// Token: 0x0400048E RID: 1166
		internal const string FormOnFormClosedDescr = "FormOnFormClosedDescr";

		// Token: 0x0400048F RID: 1167
		internal const string FormOnFormClosingDescr = "FormOnFormClosingDescr";

		// Token: 0x04000490 RID: 1168
		internal const string FormOnInputLangChangeDescr = "FormOnInputLangChangeDescr";

		// Token: 0x04000491 RID: 1169
		internal const string FormOnInputLangChangeRequestDescr = "FormOnInputLangChangeRequestDescr";

		// Token: 0x04000492 RID: 1170
		internal const string FormOnLoadDescr = "FormOnLoadDescr";

		// Token: 0x04000493 RID: 1171
		internal const string FormOnMaximizedBoundsChangedDescr = "FormOnMaximizedBoundsChangedDescr";

		// Token: 0x04000494 RID: 1172
		internal const string FormOnMaximumSizeChangedDescr = "FormOnMaximumSizeChangedDescr";

		// Token: 0x04000495 RID: 1173
		internal const string FormOnMDIChildActivateDescr = "FormOnMDIChildActivateDescr";

		// Token: 0x04000496 RID: 1174
		internal const string FormOnMenuCompleteDescr = "FormOnMenuCompleteDescr";

		// Token: 0x04000497 RID: 1175
		internal const string FormOnMenuStartDescr = "FormOnMenuStartDescr";

		// Token: 0x04000498 RID: 1176
		internal const string FormOnMinimumSizeChangedDescr = "FormOnMinimumSizeChangedDescr";

		// Token: 0x04000499 RID: 1177
		internal const string FormOnResizeBeginDescr = "FormOnResizeBeginDescr";

		// Token: 0x0400049A RID: 1178
		internal const string FormOnResizeEndDescr = "FormOnResizeEndDescr";

		// Token: 0x0400049B RID: 1179
		internal const string FormOnShownDescr = "FormOnShownDescr";

		// Token: 0x0400049C RID: 1180
		internal const string FormOpacityDescr = "FormOpacityDescr";

		// Token: 0x0400049D RID: 1181
		internal const string FormOwnedFormsDescr = "FormOwnedFormsDescr";

		// Token: 0x0400049E RID: 1182
		internal const string FormOwnerDescr = "FormOwnerDescr";

		// Token: 0x0400049F RID: 1183
		internal const string FormShowIconDescr = "FormShowIconDescr";

		// Token: 0x040004A0 RID: 1184
		internal const string FormShowInTaskbarDescr = "FormShowInTaskbarDescr";

		// Token: 0x040004A1 RID: 1185
		internal const string FormSizeGripStyleDescr = "FormSizeGripStyleDescr";

		// Token: 0x040004A2 RID: 1186
		internal const string FormStartPositionDescr = "FormStartPositionDescr";

		// Token: 0x040004A3 RID: 1187
		internal const string FormTopMostDescr = "FormTopMostDescr";

		// Token: 0x040004A4 RID: 1188
		internal const string FormTransparencyKeyDescr = "FormTransparencyKeyDescr";

		// Token: 0x040004A5 RID: 1189
		internal const string FormWindowStateDescr = "FormWindowStateDescr";

		// Token: 0x040004A6 RID: 1190
		internal const string GridItemDisposed = "GridItemDisposed";

		// Token: 0x040004A7 RID: 1191
		internal const string GridItemNotExpandable = "GridItemNotExpandable";

		// Token: 0x040004A8 RID: 1192
		internal const string GridPanelCellPositionDescr = "GridPanelCellPositionDescr";

		// Token: 0x040004A9 RID: 1193
		internal const string GridPanelColumnDescr = "GridPanelColumnDescr";

		// Token: 0x040004AA RID: 1194
		internal const string GridPanelColumnsDescr = "GridPanelColumnsDescr";

		// Token: 0x040004AB RID: 1195
		internal const string GridPanelColumnStylesDescr = "GridPanelColumnStylesDescr";

		// Token: 0x040004AC RID: 1196
		internal const string GridPanelGetAlignmentDescr = "GridPanelGetAlignmentDescr";

		// Token: 0x040004AD RID: 1197
		internal const string GridPanelGetBoxStretchDescr = "GridPanelGetBoxStretchDescr";

		// Token: 0x040004AE RID: 1198
		internal const string GridPanelGetColumnSpanDescr = "GridPanelGetColumnSpanDescr";

		// Token: 0x040004AF RID: 1199
		internal const string GridPanelGetRowSpanDescr = "GridPanelGetRowSpanDescr";

		// Token: 0x040004B0 RID: 1200
		internal const string GridPanelRowDescr = "GridPanelRowDescr";

		// Token: 0x040004B1 RID: 1201
		internal const string GridPanelRowsDescr = "GridPanelRowsDescr";

		// Token: 0x040004B2 RID: 1202
		internal const string GridPanelRowStylesDescr = "GridPanelRowStylesDescr";

		// Token: 0x040004B3 RID: 1203
		internal const string HandleAlreadyExists = "HandleAlreadyExists";

		// Token: 0x040004B4 RID: 1204
		internal const string HelpCaption = "HelpCaption";

		// Token: 0x040004B5 RID: 1205
		internal const string HelpInvalidURL = "HelpInvalidURL";

		// Token: 0x040004B6 RID: 1206
		internal const string HelpProviderHelpKeywordDescr = "HelpProviderHelpKeywordDescr";

		// Token: 0x040004B7 RID: 1207
		internal const string HelpProviderHelpNamespaceDescr = "HelpProviderHelpNamespaceDescr";

		// Token: 0x040004B8 RID: 1208
		internal const string HelpProviderHelpStringDescr = "HelpProviderHelpStringDescr";

		// Token: 0x040004B9 RID: 1209
		internal const string HelpProviderNavigatorDescr = "HelpProviderNavigatorDescr";

		// Token: 0x040004BA RID: 1210
		internal const string HelpProviderShowHelpDescr = "HelpProviderShowHelpDescr";

		// Token: 0x040004BB RID: 1211
		internal const string HtmlDocumentInvalidDomain = "HtmlDocumentInvalidDomain";

		// Token: 0x040004BC RID: 1212
		internal const string HtmlElementAttributeNotSupported = "HtmlElementAttributeNotSupported";

		// Token: 0x040004BD RID: 1213
		internal const string HtmlElementMethodNotSupported = "HtmlElementMethodNotSupported";

		// Token: 0x040004BE RID: 1214
		internal const string HtmlElementPropertyNotSupported = "HtmlElementPropertyNotSupported";

		// Token: 0x040004BF RID: 1215
		internal const string ICurrencyManagerProviderDescr = "ICurrencyManagerProviderDescr";

		// Token: 0x040004C0 RID: 1216
		internal const string IllegalCrossThreadCall = "IllegalCrossThreadCall";

		// Token: 0x040004C1 RID: 1217
		internal const string IllegalCrossThreadCallWithStack = "IllegalCrossThreadCallWithStack";

		// Token: 0x040004C2 RID: 1218
		internal const string IllegalState = "IllegalState";

		// Token: 0x040004C3 RID: 1219
		internal const string ImageListAddFailed = "ImageListAddFailed";

		// Token: 0x040004C4 RID: 1220
		internal const string ImageListAllowMirroringDescr = "ImageListAllowMirroringDescr";

		// Token: 0x040004C5 RID: 1221
		internal const string ImageListBadImage = "ImageListBadImage";

		// Token: 0x040004C6 RID: 1222
		internal const string ImageListBitmap = "ImageListBitmap";

		// Token: 0x040004C7 RID: 1223
		internal const string ImageListCantRecreate = "ImageListCantRecreate";

		// Token: 0x040004C8 RID: 1224
		internal const string ImageListColorDepthDescr = "ImageListColorDepthDescr";

		// Token: 0x040004C9 RID: 1225
		internal const string ImageListCreateFailed = "ImageListCreateFailed";

		// Token: 0x040004CA RID: 1226
		internal const string ImageListEntryType = "ImageListEntryType";

		// Token: 0x040004CB RID: 1227
		internal const string ImageListGetFailed = "ImageListGetFailed";

		// Token: 0x040004CC RID: 1228
		internal const string ImageListHandleCreatedDescr = "ImageListHandleCreatedDescr";

		// Token: 0x040004CD RID: 1229
		internal const string ImageListHandleDescr = "ImageListHandleDescr";

		// Token: 0x040004CE RID: 1230
		internal const string ImageListImagesDescr = "ImageListImagesDescr";

		// Token: 0x040004CF RID: 1231
		internal const string ImageListImageStreamDescr = "ImageListImageStreamDescr";

		// Token: 0x040004D0 RID: 1232
		internal const string ImageListImageTooShort = "ImageListImageTooShort";

		// Token: 0x040004D1 RID: 1233
		internal const string ImageListOnRecreateHandleDescr = "ImageListOnRecreateHandleDescr";

		// Token: 0x040004D2 RID: 1234
		internal const string ImageListRemoveFailed = "ImageListRemoveFailed";

		// Token: 0x040004D3 RID: 1235
		internal const string ImageListReplaceFailed = "ImageListReplaceFailed";

		// Token: 0x040004D4 RID: 1236
		internal const string ImageListRightToLeftInvalidArgument = "ImageListRightToLeftInvalidArgument";

		// Token: 0x040004D5 RID: 1237
		internal const string ImageListSizeDescr = "ImageListSizeDescr";

		// Token: 0x040004D6 RID: 1238
		internal const string ImageListStreamerLoadFailed = "ImageListStreamerLoadFailed";

		// Token: 0x040004D7 RID: 1239
		internal const string ImageListStreamerSaveFailed = "ImageListStreamerSaveFailed";

		// Token: 0x040004D8 RID: 1240
		internal const string ImageListStripBadWidth = "ImageListStripBadWidth";

		// Token: 0x040004D9 RID: 1241
		internal const string ImageListTransparentColorDescr = "ImageListTransparentColorDescr";

		// Token: 0x040004DA RID: 1242
		internal const string IndexOutOfRange = "IndexOutOfRange";

		// Token: 0x040004DB RID: 1243
		internal const string Invalid_boolean_attribute = "Invalid_boolean_attribute";

		// Token: 0x040004DC RID: 1244
		internal const string InvalidArgument = "InvalidArgument";

		// Token: 0x040004DD RID: 1245
		internal const string InvalidBoundArgument = "InvalidBoundArgument";

		// Token: 0x040004DE RID: 1246
		internal const string InvalidCrossThreadControlCall = "InvalidCrossThreadControlCall";

		// Token: 0x040004DF RID: 1247
		internal const string InvalidExBoundArgument = "InvalidExBoundArgument";

		// Token: 0x040004E0 RID: 1248
		internal const string InvalidFileFormat = "InvalidFileFormat";

		// Token: 0x040004E1 RID: 1249
		internal const string InvalidFileType = "InvalidFileType";

		// Token: 0x040004E2 RID: 1250
		internal const string InvalidGDIHandle = "InvalidGDIHandle";

		// Token: 0x040004E3 RID: 1251
		internal const string InvalidHighBoundArgument = "InvalidHighBoundArgument";

		// Token: 0x040004E4 RID: 1252
		internal const string InvalidHighBoundArgumentEx = "InvalidHighBoundArgumentEx";

		// Token: 0x040004E5 RID: 1253
		internal const string InvalidLowBoundArgument = "InvalidLowBoundArgument";

		// Token: 0x040004E6 RID: 1254
		internal const string InvalidLowBoundArgumentEx = "InvalidLowBoundArgumentEx";

		// Token: 0x040004E7 RID: 1255
		internal const string InvalidNullArgument = "InvalidNullArgument";

		// Token: 0x040004E8 RID: 1256
		internal const string InvalidNullItemInCollection = "InvalidNullItemInCollection";

		// Token: 0x040004E9 RID: 1257
		internal const string InvalidPictureFormat = "InvalidPictureFormat";

		// Token: 0x040004EA RID: 1258
		internal const string InvalidPictureType = "InvalidPictureType";

		// Token: 0x040004EB RID: 1259
		internal const string InvalidResXBasePathOperation = "InvalidResXBasePathOperation";

		// Token: 0x040004EC RID: 1260
		internal const string InvalidResXFile = "InvalidResXFile";

		// Token: 0x040004ED RID: 1261
		internal const string InvalidResXFileReaderWriterTypes = "InvalidResXFileReaderWriterTypes";

		// Token: 0x040004EE RID: 1262
		internal const string InvalidResXNoType = "InvalidResXNoType";

		// Token: 0x040004EF RID: 1263
		internal const string InvalidResXResourceNoName = "InvalidResXResourceNoName";

		// Token: 0x040004F0 RID: 1264
		internal const string InvalidSendKeysKeyword = "InvalidSendKeysKeyword";

		// Token: 0x040004F1 RID: 1265
		internal const string InvalidSendKeysRepeat = "InvalidSendKeysRepeat";

		// Token: 0x040004F2 RID: 1266
		internal const string InvalidSendKeysString = "InvalidSendKeysString";

		// Token: 0x040004F3 RID: 1267
		internal const string InvalidSingleMonthSize = "InvalidSingleMonthSize";

		// Token: 0x040004F4 RID: 1268
		internal const string InvalidWndClsName = "InvalidWndClsName";

		// Token: 0x040004F5 RID: 1269
		internal const string InvocationException = "InvocationException";

		// Token: 0x040004F6 RID: 1270
		internal const string IsMirroredDescr = "IsMirroredDescr";

		// Token: 0x040004F7 RID: 1271
		internal const string ISupportInitializeDescr = "ISupportInitializeDescr";

		// Token: 0x040004F8 RID: 1272
		internal const string KeysConverterInvalidKeyCombination = "KeysConverterInvalidKeyCombination";

		// Token: 0x040004F9 RID: 1273
		internal const string KeysConverterInvalidKeyName = "KeysConverterInvalidKeyName";

		// Token: 0x040004FA RID: 1274
		internal const string LabelAutoEllipsisDescr = "LabelAutoEllipsisDescr";

		// Token: 0x040004FB RID: 1275
		internal const string LabelAutoSizeDescr = "LabelAutoSizeDescr";

		// Token: 0x040004FC RID: 1276
		internal const string LabelBackgroundImageDescr = "LabelBackgroundImageDescr";

		// Token: 0x040004FD RID: 1277
		internal const string LabelBorderDescr = "LabelBorderDescr";

		// Token: 0x040004FE RID: 1278
		internal const string LabelOnTextAlignChangedDescr = "LabelOnTextAlignChangedDescr";

		// Token: 0x040004FF RID: 1279
		internal const string LabelPreferredHeightDescr = "LabelPreferredHeightDescr";

		// Token: 0x04000500 RID: 1280
		internal const string LabelPreferredWidthDescr = "LabelPreferredWidthDescr";

		// Token: 0x04000501 RID: 1281
		internal const string LabelTextAlignDescr = "LabelTextAlignDescr";

		// Token: 0x04000502 RID: 1282
		internal const string LabelUseMnemonicDescr = "LabelUseMnemonicDescr";

		// Token: 0x04000503 RID: 1283
		internal const string LayoutEngineUnsupportedType = "LayoutEngineUnsupportedType";

		// Token: 0x04000504 RID: 1284
		internal const string LinkLabelActiveLinkColorDescr = "LinkLabelActiveLinkColorDescr";

		// Token: 0x04000505 RID: 1285
		internal const string LinkLabelAreaLength = "LinkLabelAreaLength";

		// Token: 0x04000506 RID: 1286
		internal const string LinkLabelAreaStart = "LinkLabelAreaStart";

		// Token: 0x04000507 RID: 1287
		internal const string LinkLabelBadLink = "LinkLabelBadLink";

		// Token: 0x04000508 RID: 1288
		internal const string LinkLabelDisabledLinkColorDescr = "LinkLabelDisabledLinkColorDescr";

		// Token: 0x04000509 RID: 1289
		internal const string LinkLabelLinkAreaDescr = "LinkLabelLinkAreaDescr";

		// Token: 0x0400050A RID: 1290
		internal const string LinkLabelLinkBehaviorDescr = "LinkLabelLinkBehaviorDescr";

		// Token: 0x0400050B RID: 1291
		internal const string LinkLabelLinkClickedDescr = "LinkLabelLinkClickedDescr";

		// Token: 0x0400050C RID: 1292
		internal const string LinkLabelLinkColorDescr = "LinkLabelLinkColorDescr";

		// Token: 0x0400050D RID: 1293
		internal const string LinkLabelLinkVisitedDescr = "LinkLabelLinkVisitedDescr";

		// Token: 0x0400050E RID: 1294
		internal const string LinkLabelOverlap = "LinkLabelOverlap";

		// Token: 0x0400050F RID: 1295
		internal const string LinkLabelVisitedLinkColorDescr = "LinkLabelVisitedLinkColorDescr";

		// Token: 0x04000510 RID: 1296
		internal const string ListBindingBindField = "ListBindingBindField";

		// Token: 0x04000511 RID: 1297
		internal const string ListBindingBindProperty = "ListBindingBindProperty";

		// Token: 0x04000512 RID: 1298
		internal const string ListBindingBindPropertyReadOnly = "ListBindingBindPropertyReadOnly";

		// Token: 0x04000513 RID: 1299
		internal const string ListBindingFormatFailed = "ListBindingFormatFailed";

		// Token: 0x04000514 RID: 1300
		internal const string ListBoxBorderDescr = "ListBoxBorderDescr";

		// Token: 0x04000515 RID: 1301
		internal const string ListBoxCantInsertIntoIntegerCollection = "ListBoxCantInsertIntoIntegerCollection";

		// Token: 0x04000516 RID: 1302
		internal const string ListBoxColumnWidthDescr = "ListBoxColumnWidthDescr";

		// Token: 0x04000517 RID: 1303
		internal const string ListBoxCustomTabOffsetsDescr = "ListBoxCustomTabOffsetsDescr";

		// Token: 0x04000518 RID: 1304
		internal const string ListBoxDrawModeDescr = "ListBoxDrawModeDescr";

		// Token: 0x04000519 RID: 1305
		internal const string ListBoxHorizontalExtentDescr = "ListBoxHorizontalExtentDescr";

		// Token: 0x0400051A RID: 1306
		internal const string ListBoxHorizontalScrollbarDescr = "ListBoxHorizontalScrollbarDescr";

		// Token: 0x0400051B RID: 1307
		internal const string ListBoxIntegralHeightDescr = "ListBoxIntegralHeightDescr";

		// Token: 0x0400051C RID: 1308
		internal const string ListBoxInvalidSelectionMode = "ListBoxInvalidSelectionMode";

		// Token: 0x0400051D RID: 1309
		internal const string ListBoxItemHeightDescr = "ListBoxItemHeightDescr";

		// Token: 0x0400051E RID: 1310
		internal const string ListBoxItemOverflow = "ListBoxItemOverflow";

		// Token: 0x0400051F RID: 1311
		internal const string ListBoxItemsDescr = "ListBoxItemsDescr";

		// Token: 0x04000520 RID: 1312
		internal const string ListBoxMultiColumnDescr = "ListBoxMultiColumnDescr";

		// Token: 0x04000521 RID: 1313
		internal const string ListBoxPreferredHeightDescr = "ListBoxPreferredHeightDescr";

		// Token: 0x04000522 RID: 1314
		internal const string ListBoxScrollIsVisibleDescr = "ListBoxScrollIsVisibleDescr";

		// Token: 0x04000523 RID: 1315
		internal const string ListBoxSelectedIndexCollectionIsReadOnly = "ListBoxSelectedIndexCollectionIsReadOnly";

		// Token: 0x04000524 RID: 1316
		internal const string ListBoxSelectedIndexDescr = "ListBoxSelectedIndexDescr";

		// Token: 0x04000525 RID: 1317
		internal const string ListBoxSelectedIndicesDescr = "ListBoxSelectedIndicesDescr";

		// Token: 0x04000526 RID: 1318
		internal const string ListBoxSelectedItemDescr = "ListBoxSelectedItemDescr";

		// Token: 0x04000527 RID: 1319
		internal const string ListBoxSelectedItemsDescr = "ListBoxSelectedItemsDescr";

		// Token: 0x04000528 RID: 1320
		internal const string ListBoxSelectedObjectCollectionIsReadOnly = "ListBoxSelectedObjectCollectionIsReadOnly";

		// Token: 0x04000529 RID: 1321
		internal const string ListBoxSelectionModeDescr = "ListBoxSelectionModeDescr";

		// Token: 0x0400052A RID: 1322
		internal const string ListBoxSortedDescr = "ListBoxSortedDescr";

		// Token: 0x0400052B RID: 1323
		internal const string ListBoxTopIndexDescr = "ListBoxTopIndexDescr";

		// Token: 0x0400052C RID: 1324
		internal const string ListBoxUseTabStopsDescr = "ListBoxUseTabStopsDescr";

		// Token: 0x0400052D RID: 1325
		internal const string ListBoxVarHeightMultiCol = "ListBoxVarHeightMultiCol";

		// Token: 0x0400052E RID: 1326
		internal const string ListControlDataSourceDescr = "ListControlDataSourceDescr";

		// Token: 0x0400052F RID: 1327
		internal const string ListControlDisplayMemberDescr = "ListControlDisplayMemberDescr";

		// Token: 0x04000530 RID: 1328
		internal const string ListControlEmptyValueMemberInSettingSelectedValue = "ListControlEmptyValueMemberInSettingSelectedValue";

		// Token: 0x04000531 RID: 1329
		internal const string ListControlFormatDescr = "ListControlFormatDescr";

		// Token: 0x04000532 RID: 1330
		internal const string ListControlFormatInfoChangedDescr = "ListControlFormatInfoChangedDescr";

		// Token: 0x04000533 RID: 1331
		internal const string ListControlFormatStringChangedDescr = "ListControlFormatStringChangedDescr";

		// Token: 0x04000534 RID: 1332
		internal const string ListControlFormatStringDescr = "ListControlFormatStringDescr";

		// Token: 0x04000535 RID: 1333
		internal const string ListControlFormattingEnabledChangedDescr = "ListControlFormattingEnabledChangedDescr";

		// Token: 0x04000536 RID: 1334
		internal const string ListControlFormattingEnabledDescr = "ListControlFormattingEnabledDescr";

		// Token: 0x04000537 RID: 1335
		internal const string ListControlOnDataSourceChangedDescr = "ListControlOnDataSourceChangedDescr";

		// Token: 0x04000538 RID: 1336
		internal const string ListControlOnDisplayMemberChangedDescr = "ListControlOnDisplayMemberChangedDescr";

		// Token: 0x04000539 RID: 1337
		internal const string ListControlOnSelectedValueChangedDescr = "ListControlOnSelectedValueChangedDescr";

		// Token: 0x0400053A RID: 1338
		internal const string ListControlOnValueMemberChangedDescr = "ListControlOnValueMemberChangedDescr";

		// Token: 0x0400053B RID: 1339
		internal const string ListControlSelectedValueDescr = "ListControlSelectedValueDescr";

		// Token: 0x0400053C RID: 1340
		internal const string ListControlValueMemberDescr = "ListControlValueMemberDescr";

		// Token: 0x0400053D RID: 1341
		internal const string ListControlWrongDisplayMember = "ListControlWrongDisplayMember";

		// Token: 0x0400053E RID: 1342
		internal const string ListControlWrongValueMember = "ListControlWrongValueMember";

		// Token: 0x0400053F RID: 1343
		internal const string ListEnumCurrentOutOfRange = "ListEnumCurrentOutOfRange";

		// Token: 0x04000540 RID: 1344
		internal const string ListEnumVersionMismatch = "ListEnumVersionMismatch";

		// Token: 0x04000541 RID: 1345
		internal const string ListManagerBadPosition = "ListManagerBadPosition";

		// Token: 0x04000542 RID: 1346
		internal const string ListManagerEmptyList = "ListManagerEmptyList";

		// Token: 0x04000543 RID: 1347
		internal const string ListManagerNoValue = "ListManagerNoValue";

		// Token: 0x04000544 RID: 1348
		internal const string ListManagerSetDataSource = "ListManagerSetDataSource";

		// Token: 0x04000545 RID: 1349
		internal const string ListViewActivationDescr = "ListViewActivationDescr";

		// Token: 0x04000546 RID: 1350
		internal const string ListViewActivationMustBeOnWhenHotTrackingIsOn = "ListViewActivationMustBeOnWhenHotTrackingIsOn";

		// Token: 0x04000547 RID: 1351
		internal const string ListViewAddColumnFailed = "ListViewAddColumnFailed";

		// Token: 0x04000548 RID: 1352
		internal const string ListViewAddItemFailed = "ListViewAddItemFailed";

		// Token: 0x04000549 RID: 1353
		internal const string ListViewAfterLabelEditDescr = "ListViewAfterLabelEditDescr";

		// Token: 0x0400054A RID: 1354
		internal const string ListViewAlignmentDescr = "ListViewAlignmentDescr";

		// Token: 0x0400054B RID: 1355
		internal const string ListViewAllowColumnReorderDescr = "ListViewAllowColumnReorderDescr";

		// Token: 0x0400054C RID: 1356
		internal const string ListViewAutoArrangeDescr = "ListViewAutoArrangeDescr";

		// Token: 0x0400054D RID: 1357
		internal const string ListViewBackgroundImageTiledDescr = "ListViewBackgroundImageTiledDescr";

		// Token: 0x0400054E RID: 1358
		internal const string ListViewBadListViewSubItem = "ListViewBadListViewSubItem";

		// Token: 0x0400054F RID: 1359
		internal const string ListViewBeforeLabelEditDescr = "ListViewBeforeLabelEditDescr";

		// Token: 0x04000550 RID: 1360
		internal const string ListViewBeginEditFailed = "ListViewBeginEditFailed";

		// Token: 0x04000551 RID: 1361
		internal const string ListViewCacheVirtualItemsEventDescr = "ListViewCacheVirtualItemsEventDescr";

		// Token: 0x04000552 RID: 1362
		internal const string ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode = "ListViewCantAccessCheckedItemsCollectionWhenInVirtualMode";

		// Token: 0x04000553 RID: 1363
		internal const string ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode = "ListViewCantAccessSelectedItemsCollectionWhenInVirtualMode";

		// Token: 0x04000554 RID: 1364
		internal const string ListViewCantAddItemsToAVirtualListView = "ListViewCantAddItemsToAVirtualListView";

		// Token: 0x04000555 RID: 1365
		internal const string ListViewCantGetEnumeratorInVirtualMode = "ListViewCantGetEnumeratorInVirtualMode";

		// Token: 0x04000556 RID: 1366
		internal const string ListViewCantModifyTheItemCollInAVirtualListView = "ListViewCantModifyTheItemCollInAVirtualListView";

		// Token: 0x04000557 RID: 1367
		internal const string ListViewCantRemoveItemsFromAVirtualListView = "ListViewCantRemoveItemsFromAVirtualListView";

		// Token: 0x04000558 RID: 1368
		internal const string ListViewCantSetViewToTileViewInVirtualMode = "ListViewCantSetViewToTileViewInVirtualMode";

		// Token: 0x04000559 RID: 1369
		internal const string ListViewCantSetVirtualModeWhenInTileView = "ListViewCantSetVirtualModeWhenInTileView";

		// Token: 0x0400055A RID: 1370
		internal const string ListViewCheckBoxesDescr = "ListViewCheckBoxesDescr";

		// Token: 0x0400055B RID: 1371
		internal const string ListViewCheckBoxesNotSupportedInTileView = "ListViewCheckBoxesNotSupportedInTileView";

		// Token: 0x0400055C RID: 1372
		internal const string ListViewColumnClickDescr = "ListViewColumnClickDescr";

		// Token: 0x0400055D RID: 1373
		internal const string ListViewColumnInfoSet = "ListViewColumnInfoSet";

		// Token: 0x0400055E RID: 1374
		internal const string ListViewColumnReorderedDscr = "ListViewColumnReorderedDscr";

		// Token: 0x0400055F RID: 1375
		internal const string ListViewColumnsDescr = "ListViewColumnsDescr";

		// Token: 0x04000560 RID: 1376
		internal const string ListViewColumnWidthChangedDscr = "ListViewColumnWidthChangedDscr";

		// Token: 0x04000561 RID: 1377
		internal const string ListViewColumnWidthChangingDscr = "ListViewColumnWidthChangingDscr";

		// Token: 0x04000562 RID: 1378
		internal const string ListViewDrawColumnHeaderEventDescr = "ListViewDrawColumnHeaderEventDescr";

		// Token: 0x04000563 RID: 1379
		internal const string ListViewDrawItemEventDescr = "ListViewDrawItemEventDescr";

		// Token: 0x04000564 RID: 1380
		internal const string ListViewDrawSubItemEventDescr = "ListViewDrawSubItemEventDescr";

		// Token: 0x04000565 RID: 1381
		internal const string ListViewFindNearestItemWorksOnlyInIconView = "ListViewFindNearestItemWorksOnlyInIconView";

		// Token: 0x04000566 RID: 1382
		internal const string ListViewFocusedItemDescr = "ListViewFocusedItemDescr";

		// Token: 0x04000567 RID: 1383
		internal const string ListViewFullRowSelectDescr = "ListViewFullRowSelectDescr";

		// Token: 0x04000568 RID: 1384
		internal const string ListViewGetTopItem = "ListViewGetTopItem";

		// Token: 0x04000569 RID: 1385
		internal const string ListViewGridLinesDescr = "ListViewGridLinesDescr";

		// Token: 0x0400056A RID: 1386
		internal const string ListViewGroupDefaultGroup = "ListViewGroupDefaultGroup";

		// Token: 0x0400056B RID: 1387
		internal const string ListViewGroupDefaultHeader = "ListViewGroupDefaultHeader";

		// Token: 0x0400056C RID: 1388
		internal const string ListViewGroupNameDescr = "ListViewGroupNameDescr";

		// Token: 0x0400056D RID: 1389
		internal const string ListViewGroupsDescr = "ListViewGroupsDescr";

		// Token: 0x0400056E RID: 1390
		internal const string ListViewHeaderStyleDescr = "ListViewHeaderStyleDescr";

		// Token: 0x0400056F RID: 1391
		internal const string ListViewHideSelectionDescr = "ListViewHideSelectionDescr";

		// Token: 0x04000570 RID: 1392
		internal const string ListViewHotTrackingDescr = "ListViewHotTrackingDescr";

		// Token: 0x04000571 RID: 1393
		internal const string ListViewHoverMustBeOnWhenHotTrackingIsOn = "ListViewHoverMustBeOnWhenHotTrackingIsOn";

		// Token: 0x04000572 RID: 1394
		internal const string ListViewHoverSelectDescr = "ListViewHoverSelectDescr";

		// Token: 0x04000573 RID: 1395
		internal const string ListViewIndentCountCantBeNegative = "ListViewIndentCountCantBeNegative";

		// Token: 0x04000574 RID: 1396
		internal const string ListViewInsertionMarkDescr = "ListViewInsertionMarkDescr";

		// Token: 0x04000575 RID: 1397
		internal const string ListViewItemCheckedDescr = "ListViewItemCheckedDescr";

		// Token: 0x04000576 RID: 1398
		internal const string ListViewItemClickDescr = "ListViewItemClickDescr";

		// Token: 0x04000577 RID: 1399
		internal const string ListViewItemDragDescr = "ListViewItemDragDescr";

		// Token: 0x04000578 RID: 1400
		internal const string ListViewItemImageIndexDescr = "ListViewItemImageIndexDescr";

		// Token: 0x04000579 RID: 1401
		internal const string ListViewItemImageKeyDescr = "ListViewItemImageKeyDescr";

		// Token: 0x0400057A RID: 1402
		internal const string ListViewItemIndentCountDescr = "ListViewItemIndentCountDescr";

		// Token: 0x0400057B RID: 1403
		internal const string ListViewItemMouseHoverDescr = "ListViewItemMouseHoverDescr";

		// Token: 0x0400057C RID: 1404
		internal const string ListViewItemsDescr = "ListViewItemsDescr";

		// Token: 0x0400057D RID: 1405
		internal const string ListViewItemSelectionChangedDescr = "ListViewItemSelectionChangedDescr";

		// Token: 0x0400057E RID: 1406
		internal const string ListViewItemSorterDescr = "ListViewItemSorterDescr";

		// Token: 0x0400057F RID: 1407
		internal const string ListViewItemStateImageIndexDescr = "ListViewItemStateImageIndexDescr";

		// Token: 0x04000580 RID: 1408
		internal const string ListViewItemStateImageKeyDescr = "ListViewItemStateImageKeyDescr";

		// Token: 0x04000581 RID: 1409
		internal const string ListViewItemSubItemsDescr = "ListViewItemSubItemsDescr";

		// Token: 0x04000582 RID: 1410
		internal const string ListViewLabelEditDescr = "ListViewLabelEditDescr";

		// Token: 0x04000583 RID: 1411
		internal const string ListViewLabelWrapDescr = "ListViewLabelWrapDescr";

		// Token: 0x04000584 RID: 1412
		internal const string ListViewLargeImageListDescr = "ListViewLargeImageListDescr";

		// Token: 0x04000585 RID: 1413
		internal const string ListViewMultiSelectDescr = "ListViewMultiSelectDescr";

		// Token: 0x04000586 RID: 1414
		internal const string ListViewOwnerDrawDescr = "ListViewOwnerDrawDescr";

		// Token: 0x04000587 RID: 1415
		internal const string ListViewRetrieveVirtualItemEventDescr = "ListViewRetrieveVirtualItemEventDescr";

		// Token: 0x04000588 RID: 1416
		internal const string ListViewScrollableDescr = "ListViewScrollableDescr";

		// Token: 0x04000589 RID: 1417
		internal const string ListViewSearchForVirtualItemDescr = "ListViewSearchForVirtualItemDescr";

		// Token: 0x0400058A RID: 1418
		internal const string ListViewSelectedIndexChangedDescr = "ListViewSelectedIndexChangedDescr";

		// Token: 0x0400058B RID: 1419
		internal const string ListViewSelectedItemsDescr = "ListViewSelectedItemsDescr";

		// Token: 0x0400058C RID: 1420
		internal const string ListViewSetTopItem = "ListViewSetTopItem";

		// Token: 0x0400058D RID: 1421
		internal const string ListViewShowGroupsDescr = "ListViewShowGroupsDescr";

		// Token: 0x0400058E RID: 1422
		internal const string ListViewShowItemToolTipsDescr = "ListViewShowItemToolTipsDescr";

		// Token: 0x0400058F RID: 1423
		internal const string ListViewSmallImageListDescr = "ListViewSmallImageListDescr";

		// Token: 0x04000590 RID: 1424
		internal const string ListViewSortingDescr = "ListViewSortingDescr";

		// Token: 0x04000591 RID: 1425
		internal const string ListViewSortNotAllowedInVirtualListView = "ListViewSortNotAllowedInVirtualListView";

		// Token: 0x04000592 RID: 1426
		internal const string ListViewStartIndexCannotBeLargerThanEndIndex = "ListViewStartIndexCannotBeLargerThanEndIndex";

		// Token: 0x04000593 RID: 1427
		internal const string ListViewStateImageListDescr = "ListViewStateImageListDescr";

		// Token: 0x04000594 RID: 1428
		internal const string ListViewSubItemCollectionInvalidArgument = "ListViewSubItemCollectionInvalidArgument";

		// Token: 0x04000595 RID: 1429
		internal const string ListViewTileSizeDescr = "ListViewTileSizeDescr";

		// Token: 0x04000596 RID: 1430
		internal const string ListViewTileSizeMustBePositive = "ListViewTileSizeMustBePositive";

		// Token: 0x04000597 RID: 1431
		internal const string ListViewTileViewDoesNotSupportCheckBoxes = "ListViewTileViewDoesNotSupportCheckBoxes";

		// Token: 0x04000598 RID: 1432
		internal const string ListViewTopItemDescr = "ListViewTopItemDescr";

		// Token: 0x04000599 RID: 1433
		internal const string ListViewViewDescr = "ListViewViewDescr";

		// Token: 0x0400059A RID: 1434
		internal const string ListViewVirtualItemRequired = "ListViewVirtualItemRequired";

		// Token: 0x0400059B RID: 1435
		internal const string ListViewVirtualItemsSelectionRangeChangedDescr = "ListViewVirtualItemsSelectionRangeChangedDescr";

		// Token: 0x0400059C RID: 1436
		internal const string ListViewVirtualItemStateChangedDescr = "ListViewVirtualItemStateChangedDescr";

		// Token: 0x0400059D RID: 1437
		internal const string ListViewVirtualListSizeDescr = "ListViewVirtualListSizeDescr";

		// Token: 0x0400059E RID: 1438
		internal const string ListViewVirtualListSizeInvalidArgument = "ListViewVirtualListSizeInvalidArgument";

		// Token: 0x0400059F RID: 1439
		internal const string ListViewVirtualListViewRequiresNoCheckedItems = "ListViewVirtualListViewRequiresNoCheckedItems";

		// Token: 0x040005A0 RID: 1440
		internal const string ListViewVirtualListViewRequiresNoItems = "ListViewVirtualListViewRequiresNoItems";

		// Token: 0x040005A1 RID: 1441
		internal const string ListViewVirtualListViewRequiresNoSelectedItems = "ListViewVirtualListViewRequiresNoSelectedItems";

		// Token: 0x040005A2 RID: 1442
		internal const string ListViewVirtualModeCantAccessSubItem = "ListViewVirtualModeCantAccessSubItem";

		// Token: 0x040005A3 RID: 1443
		internal const string ListViewVirtualModeDescr = "ListViewVirtualModeDescr";

		// Token: 0x040005A4 RID: 1444
		internal const string LoadDLLError = "LoadDLLError";

		// Token: 0x040005A5 RID: 1445
		internal const string LoadTextError = "LoadTextError";

		// Token: 0x040005A6 RID: 1446
		internal const string MainMenuCollapseDescr = "MainMenuCollapseDescr";

		// Token: 0x040005A7 RID: 1447
		internal const string MainMenuImageListDescr = "MainMenuImageListDescr";

		// Token: 0x040005A8 RID: 1448
		internal const string MainMenuIsImageMarginPresentDescr = "MainMenuIsImageMarginPresentDescr";

		// Token: 0x040005A9 RID: 1449
		internal const string MainMenuToStringNoForm = "MainMenuToStringNoForm";

		// Token: 0x040005AA RID: 1450
		internal const string MaskedTextBoxAllowPromptAsInputDescr = "MaskedTextBoxAllowPromptAsInputDescr";

		// Token: 0x040005AB RID: 1451
		internal const string MaskedTextBoxAsciiOnlyDescr = "MaskedTextBoxAsciiOnlyDescr";

		// Token: 0x040005AC RID: 1452
		internal const string MaskedTextBoxBeepOnErrorDescr = "MaskedTextBoxBeepOnErrorDescr";

		// Token: 0x040005AD RID: 1453
		internal const string MaskedTextBoxCultureDescr = "MaskedTextBoxCultureDescr";

		// Token: 0x040005AE RID: 1454
		internal const string MaskedTextBoxCutCopyMaskFormat = "MaskedTextBoxCutCopyMaskFormat";

		// Token: 0x040005AF RID: 1455
		internal const string MaskedTextBoxHidePromptOnLeaveDescr = "MaskedTextBoxHidePromptOnLeaveDescr";

		// Token: 0x040005B0 RID: 1456
		internal const string MaskedTextBoxIncompleteMsg = "MaskedTextBoxIncompleteMsg";

		// Token: 0x040005B1 RID: 1457
		internal const string MaskedTextBoxInsertKeyModeDescr = "MaskedTextBoxInsertKeyModeDescr";

		// Token: 0x040005B2 RID: 1458
		internal const string MaskedTextBoxInvalidCharError = "MaskedTextBoxInvalidCharError";

		// Token: 0x040005B3 RID: 1459
		internal const string MaskedTextBoxIsOverwriteModeChangedDescr = "MaskedTextBoxIsOverwriteModeChangedDescr";

		// Token: 0x040005B4 RID: 1460
		internal const string MaskedTextBoxMaskChangedDescr = "MaskedTextBoxMaskChangedDescr";

		// Token: 0x040005B5 RID: 1461
		internal const string MaskedTextBoxMaskDescr = "MaskedTextBoxMaskDescr";

		// Token: 0x040005B6 RID: 1462
		internal const string MaskedTextBoxMaskInputRejectedDescr = "MaskedTextBoxMaskInputRejectedDescr";

		// Token: 0x040005B7 RID: 1463
		internal const string MaskedTextBoxMaskInvalidChar = "MaskedTextBoxMaskInvalidChar";

		// Token: 0x040005B8 RID: 1464
		internal const string MaskedTextBoxPasswordAndPromptCharError = "MaskedTextBoxPasswordAndPromptCharError";

		// Token: 0x040005B9 RID: 1465
		internal const string MaskedTextBoxPasswordCharDescr = "MaskedTextBoxPasswordCharDescr";

		// Token: 0x040005BA RID: 1466
		internal const string MaskedTextBoxPromptCharDescr = "MaskedTextBoxPromptCharDescr";

		// Token: 0x040005BB RID: 1467
		internal const string MaskedTextBoxRejectInputOnFirstFailureDescr = "MaskedTextBoxRejectInputOnFirstFailureDescr";

		// Token: 0x040005BC RID: 1468
		internal const string MaskedTextBoxResetOnPrompt = "MaskedTextBoxResetOnPrompt";

		// Token: 0x040005BD RID: 1469
		internal const string MaskedTextBoxResetOnSpace = "MaskedTextBoxResetOnSpace";

		// Token: 0x040005BE RID: 1470
		internal const string MaskedTextBoxSkipLiterals = "MaskedTextBoxSkipLiterals";

		// Token: 0x040005BF RID: 1471
		internal const string MaskedTextBoxTextMaskFormat = "MaskedTextBoxTextMaskFormat";

		// Token: 0x040005C0 RID: 1472
		internal const string MaskedTextBoxTypeValidationCompletedDescr = "MaskedTextBoxTypeValidationCompletedDescr";

		// Token: 0x040005C1 RID: 1473
		internal const string MaskedTextBoxTypeValidationSucceeded = "MaskedTextBoxTypeValidationSucceeded";

		// Token: 0x040005C2 RID: 1474
		internal const string MaskedTextBoxUseSystemPasswordCharDescr = "MaskedTextBoxUseSystemPasswordCharDescr";

		// Token: 0x040005C3 RID: 1475
		internal const string MaskedTextBoxValidatedValueChangedDescr = "MaskedTextBoxValidatedValueChangedDescr";

		// Token: 0x040005C4 RID: 1476
		internal const string MaskedTextBoxValidatingTypeDescr = "MaskedTextBoxValidatingTypeDescr";

		// Token: 0x040005C5 RID: 1477
		internal const string MDIChildAddToNonMDIParent = "MDIChildAddToNonMDIParent";

		// Token: 0x040005C6 RID: 1478
		internal const string MDIContainerMustBeTopLevel = "MDIContainerMustBeTopLevel";

		// Token: 0x040005C7 RID: 1479
		internal const string MDIMenuMoreWindows = "MDIMenuMoreWindows";

		// Token: 0x040005C8 RID: 1480
		internal const string MDIParentNotContainer = "MDIParentNotContainer";

		// Token: 0x040005C9 RID: 1481
		internal const string measureItemEventDescr = "measureItemEventDescr";

		// Token: 0x040005CA RID: 1482
		internal const string MenuBadMenuItem = "MenuBadMenuItem";

		// Token: 0x040005CB RID: 1483
		internal const string MenuImageMarginColorDescr = "MenuImageMarginColorDescr";

		// Token: 0x040005CC RID: 1484
		internal const string MenuIsParentDescr = "MenuIsParentDescr";

		// Token: 0x040005CD RID: 1485
		internal const string MenuItemAlreadyExists = "MenuItemAlreadyExists";

		// Token: 0x040005CE RID: 1486
		internal const string MenuItemCheckedDescr = "MenuItemCheckedDescr";

		// Token: 0x040005CF RID: 1487
		internal const string MenuItemDefaultDescr = "MenuItemDefaultDescr";

		// Token: 0x040005D0 RID: 1488
		internal const string MenuItemEnabledDescr = "MenuItemEnabledDescr";

		// Token: 0x040005D1 RID: 1489
		internal const string MenuItemImageDescr = "MenuItemImageDescr";

		// Token: 0x040005D2 RID: 1490
		internal const string MenuItemImageListDescr = "MenuItemImageListDescr";

		// Token: 0x040005D3 RID: 1491
		internal const string MenuItemImageMarginColorDescr = "MenuItemImageMarginColorDescr";

		// Token: 0x040005D4 RID: 1492
		internal const string MenuItemInvalidCheckProperty = "MenuItemInvalidCheckProperty";

		// Token: 0x040005D5 RID: 1493
		internal const string MenuItemMDIListDescr = "MenuItemMDIListDescr";

		// Token: 0x040005D6 RID: 1494
		internal const string MenuItemMergeOrderDescr = "MenuItemMergeOrderDescr";

		// Token: 0x040005D7 RID: 1495
		internal const string MenuItemMergeTypeDescr = "MenuItemMergeTypeDescr";

		// Token: 0x040005D8 RID: 1496
		internal const string MenuItemOnClickDescr = "MenuItemOnClickDescr";

		// Token: 0x040005D9 RID: 1497
		internal const string MenuItemOnInitDescr = "MenuItemOnInitDescr";

		// Token: 0x040005DA RID: 1498
		internal const string MenuItemOnSelectDescr = "MenuItemOnSelectDescr";

		// Token: 0x040005DB RID: 1499
		internal const string MenuItemOwnerDrawDescr = "MenuItemOwnerDrawDescr";

		// Token: 0x040005DC RID: 1500
		internal const string MenuItemRadioCheckDescr = "MenuItemRadioCheckDescr";

		// Token: 0x040005DD RID: 1501
		internal const string MenuItemShortCutDescr = "MenuItemShortCutDescr";

		// Token: 0x040005DE RID: 1502
		internal const string MenuItemShowShortCutDescr = "MenuItemShowShortCutDescr";

		// Token: 0x040005DF RID: 1503
		internal const string MenuItemTextDescr = "MenuItemTextDescr";

		// Token: 0x040005E0 RID: 1504
		internal const string MenuItemVisibleDescr = "MenuItemVisibleDescr";

		// Token: 0x040005E1 RID: 1505
		internal const string MenuMDIListItemDescr = "MenuMDIListItemDescr";

		// Token: 0x040005E2 RID: 1506
		internal const string MenuMenuItemsDescr = "MenuMenuItemsDescr";

		// Token: 0x040005E3 RID: 1507
		internal const string MenuMergeWithSelf = "MenuMergeWithSelf";

		// Token: 0x040005E4 RID: 1508
		internal const string MenuRightToLeftDescr = "MenuRightToLeftDescr";

		// Token: 0x040005E5 RID: 1509
		internal const string MenuStripMdiWindowListItem = "MenuStripMdiWindowListItem";

		// Token: 0x040005E6 RID: 1510
		internal const string MenuStripMenuActivateDescr = "MenuStripMenuActivateDescr";

		// Token: 0x040005E7 RID: 1511
		internal const string MenuStripMenuDeactivateDescr = "MenuStripMenuDeactivateDescr";

		// Token: 0x040005E8 RID: 1512
		internal const string MonthCalendarAnnuallyBoldedDatesDescr = "MonthCalendarAnnuallyBoldedDatesDescr";

		// Token: 0x040005E9 RID: 1513
		internal const string MonthCalendarDimensionsDescr = "MonthCalendarDimensionsDescr";

		// Token: 0x040005EA RID: 1514
		internal const string MonthCalendarFirstDayOfWeekDescr = "MonthCalendarFirstDayOfWeekDescr";

		// Token: 0x040005EB RID: 1515
		internal const string MonthCalendarForeColorDescr = "MonthCalendarForeColorDescr";

		// Token: 0x040005EC RID: 1516
		internal const string MonthCalendarInvalidDimensions = "MonthCalendarInvalidDimensions";

		// Token: 0x040005ED RID: 1517
		internal const string MonthCalendarMaxDateDescr = "MonthCalendarMaxDateDescr";

		// Token: 0x040005EE RID: 1518
		internal const string MonthCalendarMaxSelCount = "MonthCalendarMaxSelCount";

		// Token: 0x040005EF RID: 1519
		internal const string MonthCalendarMaxSelectionCountDescr = "MonthCalendarMaxSelectionCountDescr";

		// Token: 0x040005F0 RID: 1520
		internal const string MonthCalendarMinDateDescr = "MonthCalendarMinDateDescr";

		// Token: 0x040005F1 RID: 1521
		internal const string MonthCalendarMonthBackColorDescr = "MonthCalendarMonthBackColorDescr";

		// Token: 0x040005F2 RID: 1522
		internal const string MonthCalendarMonthlyBoldedDatesDescr = "MonthCalendarMonthlyBoldedDatesDescr";

		// Token: 0x040005F3 RID: 1523
		internal const string MonthCalendarOnDateChangedDescr = "MonthCalendarOnDateChangedDescr";

		// Token: 0x040005F4 RID: 1524
		internal const string MonthCalendarOnDateSelectedDescr = "MonthCalendarOnDateSelectedDescr";

		// Token: 0x040005F5 RID: 1525
		internal const string MonthCalendarRange = "MonthCalendarRange";

		// Token: 0x040005F6 RID: 1526
		internal const string MonthCalendarScrollChangeDescr = "MonthCalendarScrollChangeDescr";

		// Token: 0x040005F7 RID: 1527
		internal const string MonthCalendarSelectionEndDescr = "MonthCalendarSelectionEndDescr";

		// Token: 0x040005F8 RID: 1528
		internal const string MonthCalendarSelectionRangeDescr = "MonthCalendarSelectionRangeDescr";

		// Token: 0x040005F9 RID: 1529
		internal const string MonthCalendarSelectionStartDescr = "MonthCalendarSelectionStartDescr";

		// Token: 0x040005FA RID: 1530
		internal const string MonthCalendarShowTodayCircleDescr = "MonthCalendarShowTodayCircleDescr";

		// Token: 0x040005FB RID: 1531
		internal const string MonthCalendarShowTodayDescr = "MonthCalendarShowTodayDescr";

		// Token: 0x040005FC RID: 1532
		internal const string MonthCalendarShowWeekNumbersDescr = "MonthCalendarShowWeekNumbersDescr";

		// Token: 0x040005FD RID: 1533
		internal const string MonthCalendarSingleMonthSizeDescr = "MonthCalendarSingleMonthSizeDescr";

		// Token: 0x040005FE RID: 1534
		internal const string MonthCalendarTitleBackColorDescr = "MonthCalendarTitleBackColorDescr";

		// Token: 0x040005FF RID: 1535
		internal const string MonthCalendarTitleForeColorDescr = "MonthCalendarTitleForeColorDescr";

		// Token: 0x04000600 RID: 1536
		internal const string MonthCalendarTodayDateDescr = "MonthCalendarTodayDateDescr";

		// Token: 0x04000601 RID: 1537
		internal const string MonthCalendarTodayDateSetDescr = "MonthCalendarTodayDateSetDescr";

		// Token: 0x04000602 RID: 1538
		internal const string MonthCalendarTrailingForeColorDescr = "MonthCalendarTrailingForeColorDescr";

		// Token: 0x04000603 RID: 1539
		internal const string NoAllowNewOnReadOnlyList = "NoAllowNewOnReadOnlyList";

		// Token: 0x04000604 RID: 1540
		internal const string NoAllowRemoveOnReadOnlyList = "NoAllowRemoveOnReadOnlyList";

		// Token: 0x04000605 RID: 1541
		internal const string NoDefaultConstructor = "NoDefaultConstructor";

		// Token: 0x04000606 RID: 1542
		internal const string NoMoreColumns = "NoMoreColumns";

		// Token: 0x04000607 RID: 1543
		internal const string NonTopLevelCantHaveOwner = "NonTopLevelCantHaveOwner";

		// Token: 0x04000608 RID: 1544
		internal const string NotAvailable = "NotAvailable";

		// Token: 0x04000609 RID: 1545
		internal const string NotifyIconBalloonTipIconDescr = "NotifyIconBalloonTipIconDescr";

		// Token: 0x0400060A RID: 1546
		internal const string NotifyIconBalloonTipTextDescr = "NotifyIconBalloonTipTextDescr";

		// Token: 0x0400060B RID: 1547
		internal const string NotifyIconBalloonTipTitleDescr = "NotifyIconBalloonTipTitleDescr";

		// Token: 0x0400060C RID: 1548
		internal const string NotifyIconEmptyOrNullTipText = "NotifyIconEmptyOrNullTipText";

		// Token: 0x0400060D RID: 1549
		internal const string NotifyIconIconDescr = "NotifyIconIconDescr";

		// Token: 0x0400060E RID: 1550
		internal const string NotifyIconMenuDescr = "NotifyIconMenuDescr";

		// Token: 0x0400060F RID: 1551
		internal const string NotifyIconMouseClickDescr = "NotifyIconMouseClickDescr";

		// Token: 0x04000610 RID: 1552
		internal const string NotifyIconMouseDoubleClickDescr = "NotifyIconMouseDoubleClickDescr";

		// Token: 0x04000611 RID: 1553
		internal const string NotifyIconOnBalloonTipClickedDescr = "NotifyIconOnBalloonTipClickedDescr";

		// Token: 0x04000612 RID: 1554
		internal const string NotifyIconOnBalloonTipClosedDescr = "NotifyIconOnBalloonTipClosedDescr";

		// Token: 0x04000613 RID: 1555
		internal const string NotifyIconOnBalloonTipShownDescr = "NotifyIconOnBalloonTipShownDescr";

		// Token: 0x04000614 RID: 1556
		internal const string NotifyIconTextDescr = "NotifyIconTextDescr";

		// Token: 0x04000615 RID: 1557
		internal const string NotifyIconVisDescr = "NotifyIconVisDescr";

		// Token: 0x04000616 RID: 1558
		internal const string NotSerializableType = "NotSerializableType";

		// Token: 0x04000617 RID: 1559
		internal const string NotSupported = "NotSupported";

		// Token: 0x04000618 RID: 1560
		internal const string NumericUpDownAccelerationCollectionAtLeastOneEntryIsNull = "NumericUpDownAccelerationCollectionAtLeastOneEntryIsNull";

		// Token: 0x04000619 RID: 1561
		internal const string NumericUpDownAccelerationCompareException = "NumericUpDownAccelerationCompareException";

		// Token: 0x0400061A RID: 1562
		internal const string NumericUpDownDecimalPlacesDescr = "NumericUpDownDecimalPlacesDescr";

		// Token: 0x0400061B RID: 1563
		internal const string NumericUpDownHexadecimalDescr = "NumericUpDownHexadecimalDescr";

		// Token: 0x0400061C RID: 1564
		internal const string NumericUpDownIncrementDescr = "NumericUpDownIncrementDescr";

		// Token: 0x0400061D RID: 1565
		internal const string NumericUpDownLessThanZeroError = "NumericUpDownLessThanZeroError";

		// Token: 0x0400061E RID: 1566
		internal const string NumericUpDownMaximumDescr = "NumericUpDownMaximumDescr";

		// Token: 0x0400061F RID: 1567
		internal const string NumericUpDownMinimumDescr = "NumericUpDownMinimumDescr";

		// Token: 0x04000620 RID: 1568
		internal const string NumericUpDownOnValueChangedDescr = "NumericUpDownOnValueChangedDescr";

		// Token: 0x04000621 RID: 1569
		internal const string NumericUpDownThousandsSeparatorDescr = "NumericUpDownThousandsSeparatorDescr";

		// Token: 0x04000622 RID: 1570
		internal const string NumericUpDownValueDescr = "NumericUpDownValueDescr";

		// Token: 0x04000623 RID: 1571
		internal const string ObjectDisposed = "ObjectDisposed";

		// Token: 0x04000624 RID: 1572
		internal const string ObjectHasParent = "ObjectHasParent";

		// Token: 0x04000625 RID: 1573
		internal const string OFDcheckFileExistsDescr = "OFDcheckFileExistsDescr";

		// Token: 0x04000626 RID: 1574
		internal const string OFDmultiSelectDescr = "OFDmultiSelectDescr";

		// Token: 0x04000627 RID: 1575
		internal const string OFDreadOnlyCheckedDescr = "OFDreadOnlyCheckedDescr";

		// Token: 0x04000628 RID: 1576
		internal const string OFDshowReadOnlyDescr = "OFDshowReadOnlyDescr";

		// Token: 0x04000629 RID: 1577
		internal const string OKCaption = "OKCaption";

		// Token: 0x0400062A RID: 1578
		internal const string OnlyOneControl = "OnlyOneControl";

		// Token: 0x0400062B RID: 1579
		internal const string OperationRequiresIBindingList = "OperationRequiresIBindingList";

		// Token: 0x0400062C RID: 1580
		internal const string OperationRequiresIBindingListView = "OperationRequiresIBindingListView";

		// Token: 0x0400062D RID: 1581
		internal const string OutOfMemory = "OutOfMemory";

		// Token: 0x0400062E RID: 1582
		internal const string OwnsSelfOrOwner = "OwnsSelfOrOwner";

		// Token: 0x0400062F RID: 1583
		internal const string PaddingAllDescr = "PaddingAllDescr";

		// Token: 0x04000630 RID: 1584
		internal const string PaddingBottomDescr = "PaddingBottomDescr";

		// Token: 0x04000631 RID: 1585
		internal const string PaddingLeftDescr = "PaddingLeftDescr";

		// Token: 0x04000632 RID: 1586
		internal const string PaddingRightDescr = "PaddingRightDescr";

		// Token: 0x04000633 RID: 1587
		internal const string PaddingTopDescr = "PaddingTopDescr";

		// Token: 0x04000634 RID: 1588
		internal const string PanelBorderStyleDescr = "PanelBorderStyleDescr";

		// Token: 0x04000635 RID: 1589
		internal const string PBRSDocCommentPaneTitle = "PBRSDocCommentPaneTitle";

		// Token: 0x04000636 RID: 1590
		internal const string PBRSErrorInvalidPropertyValue = "PBRSErrorInvalidPropertyValue";

		// Token: 0x04000637 RID: 1591
		internal const string PBRSErrorTitle = "PBRSErrorTitle";

		// Token: 0x04000638 RID: 1592
		internal const string PBRSFormatExceptionMessage = "PBRSFormatExceptionMessage";

		// Token: 0x04000639 RID: 1593
		internal const string PBRSToolTipAlphabetic = "PBRSToolTipAlphabetic";

		// Token: 0x0400063A RID: 1594
		internal const string PBRSToolTipCategorized = "PBRSToolTipCategorized";

		// Token: 0x0400063B RID: 1595
		internal const string PBRSToolTipEvents = "PBRSToolTipEvents";

		// Token: 0x0400063C RID: 1596
		internal const string PBRSToolTipProperties = "PBRSToolTipProperties";

		// Token: 0x0400063D RID: 1597
		internal const string PBRSToolTipPropertyPages = "PBRSToolTipPropertyPages";

		// Token: 0x0400063E RID: 1598
		internal const string PDallowCurrentPageDescr = "PDallowCurrentPageDescr";

		// Token: 0x0400063F RID: 1599
		internal const string PDallowPagesDescr = "PDallowPagesDescr";

		// Token: 0x04000640 RID: 1600
		internal const string PDallowPrintToFileDescr = "PDallowPrintToFileDescr";

		// Token: 0x04000641 RID: 1601
		internal const string PDallowSelectionDescr = "PDallowSelectionDescr";

		// Token: 0x04000642 RID: 1602
		internal const string PDcantShowWithoutPrinter = "PDcantShowWithoutPrinter";

		// Token: 0x04000643 RID: 1603
		internal const string PDdocumentDescr = "PDdocumentDescr";

		// Token: 0x04000644 RID: 1604
		internal const string PDpageOutOfRange = "PDpageOutOfRange";

		// Token: 0x04000645 RID: 1605
		internal const string PDprinterSettingsDescr = "PDprinterSettingsDescr";

		// Token: 0x04000646 RID: 1606
		internal const string PDprintToFileDescr = "PDprintToFileDescr";

		// Token: 0x04000647 RID: 1607
		internal const string PDshowHelpDescr = "PDshowHelpDescr";

		// Token: 0x04000648 RID: 1608
		internal const string PDshowNetworkDescr = "PDshowNetworkDescr";

		// Token: 0x04000649 RID: 1609
		internal const string PDuseEXDialog = "PDuseEXDialog";

		// Token: 0x0400064A RID: 1610
		internal const string PictureBoxBorderStyleDescr = "PictureBoxBorderStyleDescr";

		// Token: 0x0400064B RID: 1611
		internal const string PictureBoxCancelAsyncDescr = "PictureBoxCancelAsyncDescr";

		// Token: 0x0400064C RID: 1612
		internal const string PictureBoxErrorImageDescr = "PictureBoxErrorImageDescr";

		// Token: 0x0400064D RID: 1613
		internal const string PictureBoxImageDescr = "PictureBoxImageDescr";

		// Token: 0x0400064E RID: 1614
		internal const string PictureBoxImageLocationDescr = "PictureBoxImageLocationDescr";

		// Token: 0x0400064F RID: 1615
		internal const string PictureBoxInitialImageDescr = "PictureBoxInitialImageDescr";

		// Token: 0x04000650 RID: 1616
		internal const string PictureBoxLoad0Descr = "PictureBoxLoad0Descr";

		// Token: 0x04000651 RID: 1617
		internal const string PictureBoxLoad1Descr = "PictureBoxLoad1Descr";

		// Token: 0x04000652 RID: 1618
		internal const string PictureBoxLoadAsync0Descr = "PictureBoxLoadAsync0Descr";

		// Token: 0x04000653 RID: 1619
		internal const string PictureBoxLoadAsync1Descr = "PictureBoxLoadAsync1Descr";

		// Token: 0x04000654 RID: 1620
		internal const string PictureBoxLoadCompletedDescr = "PictureBoxLoadCompletedDescr";

		// Token: 0x04000655 RID: 1621
		internal const string PictureBoxLoadProgressChangedDescr = "PictureBoxLoadProgressChangedDescr";

		// Token: 0x04000656 RID: 1622
		internal const string PictureBoxLoadProgressDescr = "PictureBoxLoadProgressDescr";

		// Token: 0x04000657 RID: 1623
		internal const string PictureBoxNoImageLocation = "PictureBoxNoImageLocation";

		// Token: 0x04000658 RID: 1624
		internal const string PictureBoxOnSizeModeChangedDescr = "PictureBoxOnSizeModeChangedDescr";

		// Token: 0x04000659 RID: 1625
		internal const string PictureBoxSizeModeDescr = "PictureBoxSizeModeDescr";

		// Token: 0x0400065A RID: 1626
		internal const string PictureBoxWaitOnLoadDescr = "PictureBoxWaitOnLoadDescr";

		// Token: 0x0400065B RID: 1627
		internal const string PopupControlBadParentArgument = "PopupControlBadParentArgument";

		// Token: 0x0400065C RID: 1628
		internal const string PreviewKeyDownDescr = "PreviewKeyDownDescr";

		// Token: 0x0400065D RID: 1629
		internal const string PrintControllerWithStatusDialog_Cancel = "PrintControllerWithStatusDialog_Cancel";

		// Token: 0x0400065E RID: 1630
		internal const string PrintControllerWithStatusDialog_Canceling = "PrintControllerWithStatusDialog_Canceling";

		// Token: 0x0400065F RID: 1631
		internal const string PrintControllerWithStatusDialog_DialogTitlePreview = "PrintControllerWithStatusDialog_DialogTitlePreview";

		// Token: 0x04000660 RID: 1632
		internal const string PrintControllerWithStatusDialog_DialogTitlePrint = "PrintControllerWithStatusDialog_DialogTitlePrint";

		// Token: 0x04000661 RID: 1633
		internal const string PrintControllerWithStatusDialog_NowPrinting = "PrintControllerWithStatusDialog_NowPrinting";

		// Token: 0x04000662 RID: 1634
		internal const string PrintPreviewAntiAliasDescr = "PrintPreviewAntiAliasDescr";

		// Token: 0x04000663 RID: 1635
		internal const string PrintPreviewAutoZoomDescr = "PrintPreviewAutoZoomDescr";

		// Token: 0x04000664 RID: 1636
		internal const string PrintPreviewColumnsDescr = "PrintPreviewColumnsDescr";

		// Token: 0x04000665 RID: 1637
		internal const string PrintPreviewControlZoomNegative = "PrintPreviewControlZoomNegative";

		// Token: 0x04000666 RID: 1638
		internal const string PrintPreviewDialog_Close = "PrintPreviewDialog_Close";

		// Token: 0x04000667 RID: 1639
		internal const string PrintPreviewDialog_FourPages = "PrintPreviewDialog_FourPages";

		// Token: 0x04000668 RID: 1640
		internal const string PrintPreviewDialog_OnePage = "PrintPreviewDialog_OnePage";

		// Token: 0x04000669 RID: 1641
		internal const string PrintPreviewDialog_Page = "PrintPreviewDialog_Page";

		// Token: 0x0400066A RID: 1642
		internal const string PrintPreviewDialog_Print = "PrintPreviewDialog_Print";

		// Token: 0x0400066B RID: 1643
		internal const string PrintPreviewDialog_PrintPreview = "PrintPreviewDialog_PrintPreview";

		// Token: 0x0400066C RID: 1644
		internal const string PrintPreviewDialog_SixPages = "PrintPreviewDialog_SixPages";

		// Token: 0x0400066D RID: 1645
		internal const string PrintPreviewDialog_ThreePages = "PrintPreviewDialog_ThreePages";

		// Token: 0x0400066E RID: 1646
		internal const string PrintPreviewDialog_TwoPages = "PrintPreviewDialog_TwoPages";

		// Token: 0x0400066F RID: 1647
		internal const string PrintPreviewDialog_Zoom = "PrintPreviewDialog_Zoom";

		// Token: 0x04000670 RID: 1648
		internal const string PrintPreviewDialog_Zoom10 = "PrintPreviewDialog_Zoom10";

		// Token: 0x04000671 RID: 1649
		internal const string PrintPreviewDialog_Zoom100 = "PrintPreviewDialog_Zoom100";

		// Token: 0x04000672 RID: 1650
		internal const string PrintPreviewDialog_Zoom150 = "PrintPreviewDialog_Zoom150";

		// Token: 0x04000673 RID: 1651
		internal const string PrintPreviewDialog_Zoom200 = "PrintPreviewDialog_Zoom200";

		// Token: 0x04000674 RID: 1652
		internal const string PrintPreviewDialog_Zoom25 = "PrintPreviewDialog_Zoom25";

		// Token: 0x04000675 RID: 1653
		internal const string PrintPreviewDialog_Zoom50 = "PrintPreviewDialog_Zoom50";

		// Token: 0x04000676 RID: 1654
		internal const string PrintPreviewDialog_Zoom500 = "PrintPreviewDialog_Zoom500";

		// Token: 0x04000677 RID: 1655
		internal const string PrintPreviewDialog_Zoom75 = "PrintPreviewDialog_Zoom75";

		// Token: 0x04000678 RID: 1656
		internal const string PrintPreviewDialog_ZoomAuto = "PrintPreviewDialog_ZoomAuto";

		// Token: 0x04000679 RID: 1657
		internal const string PrintPreviewDocumentDescr = "PrintPreviewDocumentDescr";

		// Token: 0x0400067A RID: 1658
		internal const string PrintPreviewExceptionPrinting = "PrintPreviewExceptionPrinting";

		// Token: 0x0400067B RID: 1659
		internal const string PrintPreviewNoPages = "PrintPreviewNoPages";

		// Token: 0x0400067C RID: 1660
		internal const string PrintPreviewPrintPreviewControlDescr = "PrintPreviewPrintPreviewControlDescr";

		// Token: 0x0400067D RID: 1661
		internal const string PrintPreviewRowsDescr = "PrintPreviewRowsDescr";

		// Token: 0x0400067E RID: 1662
		internal const string PrintPreviewStartPageDescr = "PrintPreviewStartPageDescr";

		// Token: 0x0400067F RID: 1663
		internal const string PrintPreviewZoomDescr = "PrintPreviewZoomDescr";

		// Token: 0x04000680 RID: 1664
		internal const string ProfessionalColorsButtonCheckedGradientBeginDescr = "ProfessionalColorsButtonCheckedGradientBeginDescr";

		// Token: 0x04000681 RID: 1665
		internal const string ProfessionalColorsButtonCheckedGradientEndDescr = "ProfessionalColorsButtonCheckedGradientEndDescr";

		// Token: 0x04000682 RID: 1666
		internal const string ProfessionalColorsButtonCheckedGradientMiddleDescr = "ProfessionalColorsButtonCheckedGradientMiddleDescr";

		// Token: 0x04000683 RID: 1667
		internal const string ProfessionalColorsButtonCheckedHighlightBorderDescr = "ProfessionalColorsButtonCheckedHighlightBorderDescr";

		// Token: 0x04000684 RID: 1668
		internal const string ProfessionalColorsButtonCheckedHighlightDescr = "ProfessionalColorsButtonCheckedHighlightDescr";

		// Token: 0x04000685 RID: 1669
		internal const string ProfessionalColorsButtonPressedBorderDescr = "ProfessionalColorsButtonPressedBorderDescr";

		// Token: 0x04000686 RID: 1670
		internal const string ProfessionalColorsButtonPressedGradientBeginDescr = "ProfessionalColorsButtonPressedGradientBeginDescr";

		// Token: 0x04000687 RID: 1671
		internal const string ProfessionalColorsButtonPressedGradientEndDescr = "ProfessionalColorsButtonPressedGradientEndDescr";

		// Token: 0x04000688 RID: 1672
		internal const string ProfessionalColorsButtonPressedGradientMiddleDescr = "ProfessionalColorsButtonPressedGradientMiddleDescr";

		// Token: 0x04000689 RID: 1673
		internal const string ProfessionalColorsButtonPressedHighlightBorderDescr = "ProfessionalColorsButtonPressedHighlightBorderDescr";

		// Token: 0x0400068A RID: 1674
		internal const string ProfessionalColorsButtonPressedHighlightDescr = "ProfessionalColorsButtonPressedHighlightDescr";

		// Token: 0x0400068B RID: 1675
		internal const string ProfessionalColorsButtonSelectedBorderDescr = "ProfessionalColorsButtonSelectedBorderDescr";

		// Token: 0x0400068C RID: 1676
		internal const string ProfessionalColorsButtonSelectedGradientBeginDescr = "ProfessionalColorsButtonSelectedGradientBeginDescr";

		// Token: 0x0400068D RID: 1677
		internal const string ProfessionalColorsButtonSelectedGradientEndDescr = "ProfessionalColorsButtonSelectedGradientEndDescr";

		// Token: 0x0400068E RID: 1678
		internal const string ProfessionalColorsButtonSelectedGradientMiddleDescr = "ProfessionalColorsButtonSelectedGradientMiddleDescr";

		// Token: 0x0400068F RID: 1679
		internal const string ProfessionalColorsButtonSelectedHighlightBorderDescr = "ProfessionalColorsButtonSelectedHighlightBorderDescr";

		// Token: 0x04000690 RID: 1680
		internal const string ProfessionalColorsButtonSelectedHighlightDescr = "ProfessionalColorsButtonSelectedHighlightDescr";

		// Token: 0x04000691 RID: 1681
		internal const string ProfessionalColorsCheckBackgroundDescr = "ProfessionalColorsCheckBackgroundDescr";

		// Token: 0x04000692 RID: 1682
		internal const string ProfessionalColorsCheckPressedBackgroundDescr = "ProfessionalColorsCheckPressedBackgroundDescr";

		// Token: 0x04000693 RID: 1683
		internal const string ProfessionalColorsCheckSelectedBackgroundDescr = "ProfessionalColorsCheckSelectedBackgroundDescr";

		// Token: 0x04000694 RID: 1684
		internal const string ProfessionalColorsGripDarkDescr = "ProfessionalColorsGripDarkDescr";

		// Token: 0x04000695 RID: 1685
		internal const string ProfessionalColorsGripLightDescr = "ProfessionalColorsGripLightDescr";

		// Token: 0x04000696 RID: 1686
		internal const string ProfessionalColorsImageMarginGradientBeginDescr = "ProfessionalColorsImageMarginGradientBeginDescr";

		// Token: 0x04000697 RID: 1687
		internal const string ProfessionalColorsImageMarginGradientEndDescr = "ProfessionalColorsImageMarginGradientEndDescr";

		// Token: 0x04000698 RID: 1688
		internal const string ProfessionalColorsImageMarginGradientMiddleDescr = "ProfessionalColorsImageMarginGradientMiddleDescr";

		// Token: 0x04000699 RID: 1689
		internal const string ProfessionalColorsImageMarginRevealedGradientBeginDescr = "ProfessionalColorsImageMarginRevealedGradientBeginDescr";

		// Token: 0x0400069A RID: 1690
		internal const string ProfessionalColorsImageMarginRevealedGradientEndDescr = "ProfessionalColorsImageMarginRevealedGradientEndDescr";

		// Token: 0x0400069B RID: 1691
		internal const string ProfessionalColorsImageMarginRevealedGradientMiddleDescr = "ProfessionalColorsImageMarginRevealedGradientMiddleDescr";

		// Token: 0x0400069C RID: 1692
		internal const string ProfessionalColorsMenuBorderDescr = "ProfessionalColorsMenuBorderDescr";

		// Token: 0x0400069D RID: 1693
		internal const string ProfessionalColorsMenuItemBorderDescr = "ProfessionalColorsMenuItemBorderDescr";

		// Token: 0x0400069E RID: 1694
		internal const string ProfessionalColorsMenuItemPressedGradientBeginDescr = "ProfessionalColorsMenuItemPressedGradientBeginDescr";

		// Token: 0x0400069F RID: 1695
		internal const string ProfessionalColorsMenuItemPressedGradientEndDescr = "ProfessionalColorsMenuItemPressedGradientEndDescr";

		// Token: 0x040006A0 RID: 1696
		internal const string ProfessionalColorsMenuItemPressedGradientMiddleDescr = "ProfessionalColorsMenuItemPressedGradientMiddleDescr";

		// Token: 0x040006A1 RID: 1697
		internal const string ProfessionalColorsMenuItemSelectedDescr = "ProfessionalColorsMenuItemSelectedDescr";

		// Token: 0x040006A2 RID: 1698
		internal const string ProfessionalColorsMenuItemSelectedGradientBeginDescr = "ProfessionalColorsMenuItemSelectedGradientBeginDescr";

		// Token: 0x040006A3 RID: 1699
		internal const string ProfessionalColorsMenuItemSelectedGradientEndDescr = "ProfessionalColorsMenuItemSelectedGradientEndDescr";

		// Token: 0x040006A4 RID: 1700
		internal const string ProfessionalColorsMenuStripGradientBeginDescr = "ProfessionalColorsMenuStripGradientBeginDescr";

		// Token: 0x040006A5 RID: 1701
		internal const string ProfessionalColorsMenuStripGradientEndDescr = "ProfessionalColorsMenuStripGradientEndDescr";

		// Token: 0x040006A6 RID: 1702
		internal const string ProfessionalColorsOverflowButtonGradientBeginDescr = "ProfessionalColorsOverflowButtonGradientBeginDescr";

		// Token: 0x040006A7 RID: 1703
		internal const string ProfessionalColorsOverflowButtonGradientEndDescr = "ProfessionalColorsOverflowButtonGradientEndDescr";

		// Token: 0x040006A8 RID: 1704
		internal const string ProfessionalColorsOverflowButtonGradientMiddleDescr = "ProfessionalColorsOverflowButtonGradientMiddleDescr";

		// Token: 0x040006A9 RID: 1705
		internal const string ProfessionalColorsRaftingContainerGradientBeginDescr = "ProfessionalColorsRaftingContainerGradientBeginDescr";

		// Token: 0x040006AA RID: 1706
		internal const string ProfessionalColorsRaftingContainerGradientEndDescr = "ProfessionalColorsRaftingContainerGradientEndDescr";

		// Token: 0x040006AB RID: 1707
		internal const string ProfessionalColorsSeparatorDarkDescr = "ProfessionalColorsSeparatorDarkDescr";

		// Token: 0x040006AC RID: 1708
		internal const string ProfessionalColorsSeparatorLightDescr = "ProfessionalColorsSeparatorLightDescr";

		// Token: 0x040006AD RID: 1709
		internal const string ProfessionalColorsStatusStripGradientBeginDescr = "ProfessionalColorsStatusStripGradientBeginDescr";

		// Token: 0x040006AE RID: 1710
		internal const string ProfessionalColorsStatusStripGradientEndDescr = "ProfessionalColorsStatusStripGradientEndDescr";

		// Token: 0x040006AF RID: 1711
		internal const string ProfessionalColorsToolStripBorderDescr = "ProfessionalColorsToolStripBorderDescr";

		// Token: 0x040006B0 RID: 1712
		internal const string ProfessionalColorsToolStripContentPanelGradientBeginDescr = "ProfessionalColorsToolStripContentPanelGradientBeginDescr";

		// Token: 0x040006B1 RID: 1713
		internal const string ProfessionalColorsToolStripContentPanelGradientEndDescr = "ProfessionalColorsToolStripContentPanelGradientEndDescr";

		// Token: 0x040006B2 RID: 1714
		internal const string ProfessionalColorsToolStripDropDownBackgroundDescr = "ProfessionalColorsToolStripDropDownBackgroundDescr";

		// Token: 0x040006B3 RID: 1715
		internal const string ProfessionalColorsToolStripGradientBeginDescr = "ProfessionalColorsToolStripGradientBeginDescr";

		// Token: 0x040006B4 RID: 1716
		internal const string ProfessionalColorsToolStripGradientEndDescr = "ProfessionalColorsToolStripGradientEndDescr";

		// Token: 0x040006B5 RID: 1717
		internal const string ProfessionalColorsToolStripGradientMiddleDescr = "ProfessionalColorsToolStripGradientMiddleDescr";

		// Token: 0x040006B6 RID: 1718
		internal const string ProfessionalColorsToolStripPanelGradientBeginDescr = "ProfessionalColorsToolStripPanelGradientBeginDescr";

		// Token: 0x040006B7 RID: 1719
		internal const string ProfessionalColorsToolStripPanelGradientEndDescr = "ProfessionalColorsToolStripPanelGradientEndDescr";

		// Token: 0x040006B8 RID: 1720
		internal const string ProgressBarIncrementMarqueeException = "ProgressBarIncrementMarqueeException";

		// Token: 0x040006B9 RID: 1721
		internal const string ProgressBarMarqueeAnimationSpeed = "ProgressBarMarqueeAnimationSpeed";

		// Token: 0x040006BA RID: 1722
		internal const string ProgressBarMaximumDescr = "ProgressBarMaximumDescr";

		// Token: 0x040006BB RID: 1723
		internal const string ProgressBarMinimumDescr = "ProgressBarMinimumDescr";

		// Token: 0x040006BC RID: 1724
		internal const string ProgressBarPerformStepMarqueeException = "ProgressBarPerformStepMarqueeException";

		// Token: 0x040006BD RID: 1725
		internal const string ProgressBarStepDescr = "ProgressBarStepDescr";

		// Token: 0x040006BE RID: 1726
		internal const string ProgressBarStyleDescr = "ProgressBarStyleDescr";

		// Token: 0x040006BF RID: 1727
		internal const string ProgressBarValueDescr = "ProgressBarValueDescr";

		// Token: 0x040006C0 RID: 1728
		internal const string ProgressBarValueMarqueeException = "ProgressBarValueMarqueeException";

		// Token: 0x040006C1 RID: 1729
		internal const string PropertyCategoryAppearance = "PropertyCategoryAppearance";

		// Token: 0x040006C2 RID: 1730
		internal const string PropertyCategoryBehavior = "PropertyCategoryBehavior";

		// Token: 0x040006C3 RID: 1731
		internal const string PropertyCategoryData = "PropertyCategoryData";

		// Token: 0x040006C4 RID: 1732
		internal const string PropertyCategoryDDE = "PropertyCategoryDDE";

		// Token: 0x040006C5 RID: 1733
		internal const string PropertyCategoryFont = "PropertyCategoryFont";

		// Token: 0x040006C6 RID: 1734
		internal const string PropertyCategoryList = "PropertyCategoryList";

		// Token: 0x040006C7 RID: 1735
		internal const string PropertyCategoryMisc = "PropertyCategoryMisc";

		// Token: 0x040006C8 RID: 1736
		internal const string PropertyCategoryPosition = "PropertyCategoryPosition";

		// Token: 0x040006C9 RID: 1737
		internal const string PropertyCategoryScale = "PropertyCategoryScale";

		// Token: 0x040006CA RID: 1738
		internal const string PropertyCategoryText = "PropertyCategoryText";

		// Token: 0x040006CB RID: 1739
		internal const string PropertyGridBadTabIndex = "PropertyGridBadTabIndex";

		// Token: 0x040006CC RID: 1740
		internal const string PropertyGridCanShowCommandsDesc = "PropertyGridCanShowCommandsDesc";

		// Token: 0x040006CD RID: 1741
		internal const string PropertyGridCategoryForeColorDesc = "PropertyGridCategoryForeColorDesc";

		// Token: 0x040006CE RID: 1742
		internal const string PropertyGridCommandsActiveLinkColorDesc = "PropertyGridCommandsActiveLinkColorDesc";

		// Token: 0x040006CF RID: 1743
		internal const string PropertyGridCommandsBackColorDesc = "PropertyGridCommandsBackColorDesc";

		// Token: 0x040006D0 RID: 1744
		internal const string PropertyGridCommandsDisabledLinkColorDesc = "PropertyGridCommandsDisabledLinkColorDesc";

		// Token: 0x040006D1 RID: 1745
		internal const string PropertyGridCommandsForeColorDesc = "PropertyGridCommandsForeColorDesc";

		// Token: 0x040006D2 RID: 1746
		internal const string PropertyGridCommandsLinkColorDesc = "PropertyGridCommandsLinkColorDesc";

		// Token: 0x040006D3 RID: 1747
		internal const string PropertyGridCommandsVisibleIfAvailable = "PropertyGridCommandsVisibleIfAvailable";

		// Token: 0x040006D4 RID: 1748
		internal const string PropertyGridDefaultAccessibleName = "PropertyGridDefaultAccessibleName";

		// Token: 0x040006D5 RID: 1749
		internal const string PropertyGridDropDownButtonAccessibleName = "PropertyGridDropDownButtonAccessibleName";

		// Token: 0x040006D6 RID: 1750
		internal const string PropertyGridExceptionInfo = "PropertyGridExceptionInfo";

		// Token: 0x040006D7 RID: 1751
		internal const string PropertyGridExceptionWhilePaintingLabel = "PropertyGridExceptionWhilePaintingLabel";

		// Token: 0x040006D8 RID: 1752
		internal const string PropertyGridHelpBackColorDesc = "PropertyGridHelpBackColorDesc";

		// Token: 0x040006D9 RID: 1753
		internal const string PropertyGridHelpForeColorDesc = "PropertyGridHelpForeColorDesc";

		// Token: 0x040006DA RID: 1754
		internal const string PropertyGridHelpVisibleDesc = "PropertyGridHelpVisibleDesc";

		// Token: 0x040006DB RID: 1755
		internal const string PropertyGridInternalNoProp = "PropertyGridInternalNoProp";

		// Token: 0x040006DC RID: 1756
		internal const string PropertyGridInvalidGridEntry = "PropertyGridInvalidGridEntry";

		// Token: 0x040006DD RID: 1757
		internal const string PropertyGridLargeButtonsDesc = "PropertyGridLargeButtonsDesc";

		// Token: 0x040006DE RID: 1758
		internal const string PropertyGridLineColorDesc = "PropertyGridLineColorDesc";

		// Token: 0x040006DF RID: 1759
		internal const string PropertyGridNoBitmap = "PropertyGridNoBitmap";

		// Token: 0x040006E0 RID: 1760
		internal const string PropertyGridPropertySortChangedDescr = "PropertyGridPropertySortChangedDescr";

		// Token: 0x040006E1 RID: 1761
		internal const string PropertyGridPropertySortDesc = "PropertyGridPropertySortDesc";

		// Token: 0x040006E2 RID: 1762
		internal const string PropertyGridPropertyTabchangedDescr = "PropertyGridPropertyTabchangedDescr";

		// Token: 0x040006E3 RID: 1763
		internal const string PropertyGridPropertyTabCollectionReadOnly = "PropertyGridPropertyTabCollectionReadOnly";

		// Token: 0x040006E4 RID: 1764
		internal const string PropertyGridPropertyValueChangedDescr = "PropertyGridPropertyValueChangedDescr";

		// Token: 0x040006E5 RID: 1765
		internal const string PropertyGridRemotedObject = "PropertyGridRemotedObject";

		// Token: 0x040006E6 RID: 1766
		internal const string PropertyGridRemoveStaticTabs = "PropertyGridRemoveStaticTabs";

		// Token: 0x040006E7 RID: 1767
		internal const string PropertyGridResetValue = "PropertyGridResetValue";

		// Token: 0x040006E8 RID: 1768
		internal const string PropertyGridSelectedGridItemChangedDescr = "PropertyGridSelectedGridItemChangedDescr";

		// Token: 0x040006E9 RID: 1769
		internal const string PropertyGridSelectedObjectDesc = "PropertyGridSelectedObjectDesc";

		// Token: 0x040006EA RID: 1770
		internal const string PropertyGridSelectedObjectsChangedDescr = "PropertyGridSelectedObjectsChangedDescr";

		// Token: 0x040006EB RID: 1771
		internal const string PropertyGridSet = "PropertyGridSet";

		// Token: 0x040006EC RID: 1772
		internal const string PropertyGridSetNull = "PropertyGridSetNull";

		// Token: 0x040006ED RID: 1773
		internal const string PropertyGridSetValue = "PropertyGridSetValue";

		// Token: 0x040006EE RID: 1774
		internal const string PropertyGridTabName = "PropertyGridTabName";

		// Token: 0x040006EF RID: 1775
		internal const string PropertyGridTabScope = "PropertyGridTabScope";

		// Token: 0x040006F0 RID: 1776
		internal const string PropertyGridTitle = "PropertyGridTitle";

		// Token: 0x040006F1 RID: 1777
		internal const string PropertyGridToolbarAccessibleName = "PropertyGridToolbarAccessibleName";

		// Token: 0x040006F2 RID: 1778
		internal const string PropertyGridToolbarVisibleDesc = "PropertyGridToolbarVisibleDesc";

		// Token: 0x040006F3 RID: 1779
		internal const string PropertyGridViewBackColorDesc = "PropertyGridViewBackColorDesc";

		// Token: 0x040006F4 RID: 1780
		internal const string PropertyGridViewEditorCreatedInvalidObject = "PropertyGridViewEditorCreatedInvalidObject";

		// Token: 0x040006F5 RID: 1781
		internal const string PropertyGridViewForeColorDesc = "PropertyGridViewForeColorDesc";

		// Token: 0x040006F6 RID: 1782
		internal const string PropertyManagerPropDoesNotExist = "PropertyManagerPropDoesNotExist";

		// Token: 0x040006F7 RID: 1783
		internal const string PropertyValueInvalidEntry = "PropertyValueInvalidEntry";

		// Token: 0x040006F8 RID: 1784
		internal const string PSDallowMarginsDescr = "PSDallowMarginsDescr";

		// Token: 0x040006F9 RID: 1785
		internal const string PSDallowOrientationDescr = "PSDallowOrientationDescr";

		// Token: 0x040006FA RID: 1786
		internal const string PSDallowPaperDescr = "PSDallowPaperDescr";

		// Token: 0x040006FB RID: 1787
		internal const string PSDallowPrinterDescr = "PSDallowPrinterDescr";

		// Token: 0x040006FC RID: 1788
		internal const string PSDcantShowWithoutPage = "PSDcantShowWithoutPage";

		// Token: 0x040006FD RID: 1789
		internal const string PSDenableMetricDescr = "PSDenableMetricDescr";

		// Token: 0x040006FE RID: 1790
		internal const string PSDminMarginsDescr = "PSDminMarginsDescr";

		// Token: 0x040006FF RID: 1791
		internal const string PSDpageSettingsDescr = "PSDpageSettingsDescr";

		// Token: 0x04000700 RID: 1792
		internal const string PSDprinterSettingsDescr = "PSDprinterSettingsDescr";

		// Token: 0x04000701 RID: 1793
		internal const string PSDshowHelpDescr = "PSDshowHelpDescr";

		// Token: 0x04000702 RID: 1794
		internal const string PSDshowNetworkDescr = "PSDshowNetworkDescr";

		// Token: 0x04000703 RID: 1795
		internal const string RadioButtonAppearanceDescr = "RadioButtonAppearanceDescr";

		// Token: 0x04000704 RID: 1796
		internal const string RadioButtonAutoCheckDescr = "RadioButtonAutoCheckDescr";

		// Token: 0x04000705 RID: 1797
		internal const string RadioButtonCheckAlignDescr = "RadioButtonCheckAlignDescr";

		// Token: 0x04000706 RID: 1798
		internal const string RadioButtonCheckedDescr = "RadioButtonCheckedDescr";

		// Token: 0x04000707 RID: 1799
		internal const string RadioButtonOnAppearanceChangedDescr = "RadioButtonOnAppearanceChangedDescr";

		// Token: 0x04000708 RID: 1800
		internal const string RadioButtonOnCheckedChangedDescr = "RadioButtonOnCheckedChangedDescr";

		// Token: 0x04000709 RID: 1801
		internal const string RadioButtonOnStartPageChangedDescr = "RadioButtonOnStartPageChangedDescr";

		// Token: 0x0400070A RID: 1802
		internal const string RadioButtonOnTextAlignChangedDescr = "RadioButtonOnTextAlignChangedDescr";

		// Token: 0x0400070B RID: 1803
		internal const string ReadonlyControlsCollection = "ReadonlyControlsCollection";

		// Token: 0x0400070C RID: 1804
		internal const string RegisterCFFailed = "RegisterCFFailed";

		// Token: 0x0400070D RID: 1805
		internal const string RelatedListManagerChild = "RelatedListManagerChild";

		// Token: 0x0400070E RID: 1806
		internal const string RestartNotSupported = "RestartNotSupported";

		// Token: 0x0400070F RID: 1807
		internal const string ResXResourceWriterSaved = "ResXResourceWriterSaved";

		// Token: 0x04000710 RID: 1808
		internal const string RichControlLresult = "RichControlLresult";

		// Token: 0x04000711 RID: 1809
		internal const string RichTextBox_IDCut = "RichTextBox_IDCut";

		// Token: 0x04000712 RID: 1810
		internal const string RichTextBox_IDDelete = "RichTextBox_IDDelete";

		// Token: 0x04000713 RID: 1811
		internal const string RichTextBox_IDDragDrop = "RichTextBox_IDDragDrop";

		// Token: 0x04000714 RID: 1812
		internal const string RichTextBox_IDPaste = "RichTextBox_IDPaste";

		// Token: 0x04000715 RID: 1813
		internal const string RichTextBox_IDTyping = "RichTextBox_IDTyping";

		// Token: 0x04000716 RID: 1814
		internal const string RichTextBox_IDUnknown = "RichTextBox_IDUnknown";

		// Token: 0x04000717 RID: 1815
		internal const string RichTextBoxAutoWordSelection = "RichTextBoxAutoWordSelection";

		// Token: 0x04000718 RID: 1816
		internal const string RichTextBoxBulletIndent = "RichTextBoxBulletIndent";

		// Token: 0x04000719 RID: 1817
		internal const string RichTextBoxCanRedoDescr = "RichTextBoxCanRedoDescr";

		// Token: 0x0400071A RID: 1818
		internal const string RichTextBoxContentsResized = "RichTextBoxContentsResized";

		// Token: 0x0400071B RID: 1819
		internal const string RichTextBoxDetectURLs = "RichTextBoxDetectURLs";

		// Token: 0x0400071C RID: 1820
		internal const string RichTextBoxEnableAutoDragDrop = "RichTextBoxEnableAutoDragDrop";

		// Token: 0x0400071D RID: 1821
		internal const string RichTextBoxHScroll = "RichTextBoxHScroll";

		// Token: 0x0400071E RID: 1822
		internal const string RichTextBoxIMEChange = "RichTextBoxIMEChange";

		// Token: 0x0400071F RID: 1823
		internal const string RichTextBoxLinkClick = "RichTextBoxLinkClick";

		// Token: 0x04000720 RID: 1824
		internal const string RichTextBoxProtected = "RichTextBoxProtected";

		// Token: 0x04000721 RID: 1825
		internal const string RichTextBoxRedoActionNameDescr = "RichTextBoxRedoActionNameDescr";

		// Token: 0x04000722 RID: 1826
		internal const string RichTextBoxRightMargin = "RichTextBoxRightMargin";

		// Token: 0x04000723 RID: 1827
		internal const string RichTextBoxRTF = "RichTextBoxRTF";

		// Token: 0x04000724 RID: 1828
		internal const string RichTextBoxScrollBars = "RichTextBoxScrollBars";

		// Token: 0x04000725 RID: 1829
		internal const string RichTextBoxSelAlignment = "RichTextBoxSelAlignment";

		// Token: 0x04000726 RID: 1830
		internal const string RichTextBoxSelBackColor = "RichTextBoxSelBackColor";

		// Token: 0x04000727 RID: 1831
		internal const string RichTextBoxSelBullet = "RichTextBoxSelBullet";

		// Token: 0x04000728 RID: 1832
		internal const string RichTextBoxSelChange = "RichTextBoxSelChange";

		// Token: 0x04000729 RID: 1833
		internal const string RichTextBoxSelCharOffset = "RichTextBoxSelCharOffset";

		// Token: 0x0400072A RID: 1834
		internal const string RichTextBoxSelColor = "RichTextBoxSelColor";

		// Token: 0x0400072B RID: 1835
		internal const string RichTextBoxSelFont = "RichTextBoxSelFont";

		// Token: 0x0400072C RID: 1836
		internal const string RichTextBoxSelHangingIndent = "RichTextBoxSelHangingIndent";

		// Token: 0x0400072D RID: 1837
		internal const string RichTextBoxSelIndent = "RichTextBoxSelIndent";

		// Token: 0x0400072E RID: 1838
		internal const string RichTextBoxSelMargin = "RichTextBoxSelMargin";

		// Token: 0x0400072F RID: 1839
		internal const string RichTextBoxSelProtected = "RichTextBoxSelProtected";

		// Token: 0x04000730 RID: 1840
		internal const string RichTextBoxSelRightIndent = "RichTextBoxSelRightIndent";

		// Token: 0x04000731 RID: 1841
		internal const string RichTextBoxSelRTF = "RichTextBoxSelRTF";

		// Token: 0x04000732 RID: 1842
		internal const string RichTextBoxSelTabs = "RichTextBoxSelTabs";

		// Token: 0x04000733 RID: 1843
		internal const string RichTextBoxSelText = "RichTextBoxSelText";

		// Token: 0x04000734 RID: 1844
		internal const string RichTextBoxSelTypeDescr = "RichTextBoxSelTypeDescr";

		// Token: 0x04000735 RID: 1845
		internal const string RichTextBoxUndoActionNameDescr = "RichTextBoxUndoActionNameDescr";

		// Token: 0x04000736 RID: 1846
		internal const string RichTextBoxVScroll = "RichTextBoxVScroll";

		// Token: 0x04000737 RID: 1847
		internal const string RichTextBoxZoomFactor = "RichTextBoxZoomFactor";

		// Token: 0x04000738 RID: 1848
		internal const string RichTextFindEndInvalid = "RichTextFindEndInvalid";

		// Token: 0x04000739 RID: 1849
		internal const string RTL = "RTL";

		// Token: 0x0400073A RID: 1850
		internal const string SafeTopLevelCaptionFormat = "SafeTopLevelCaptionFormat";

		// Token: 0x0400073B RID: 1851
		internal const string SaveFileDialogCreatePrompt = "SaveFileDialogCreatePrompt";

		// Token: 0x0400073C RID: 1852
		internal const string SaveFileDialogOverWritePrompt = "SaveFileDialogOverWritePrompt";

		// Token: 0x0400073D RID: 1853
		internal const string SaveTextError = "SaveTextError";

		// Token: 0x0400073E RID: 1854
		internal const string ScrollableControlHorizontalScrollDescr = "ScrollableControlHorizontalScrollDescr";

		// Token: 0x0400073F RID: 1855
		internal const string ScrollableControlRaiseMouseEnterLeaveEventsForScrollBarsDescr = "ScrollableControlRaiseMouseEnterLeaveEventsForScrollBarsDescr";

		// Token: 0x04000740 RID: 1856
		internal const string ScrollableControlVerticalScrollDescr = "ScrollableControlVerticalScrollDescr";

		// Token: 0x04000741 RID: 1857
		internal const string ScrollBarEnableDescr = "ScrollBarEnableDescr";

		// Token: 0x04000742 RID: 1858
		internal const string ScrollBarLargeChangeDescr = "ScrollBarLargeChangeDescr";

		// Token: 0x04000743 RID: 1859
		internal const string ScrollBarMaximumDescr = "ScrollBarMaximumDescr";

		// Token: 0x04000744 RID: 1860
		internal const string ScrollBarMinimumDescr = "ScrollBarMinimumDescr";

		// Token: 0x04000745 RID: 1861
		internal const string ScrollBarOnScrollDescr = "ScrollBarOnScrollDescr";

		// Token: 0x04000746 RID: 1862
		internal const string ScrollBarSmallChangeDescr = "ScrollBarSmallChangeDescr";

		// Token: 0x04000747 RID: 1863
		internal const string ScrollBarValueDescr = "ScrollBarValueDescr";

		// Token: 0x04000748 RID: 1864
		internal const string ScrollBarVisibleDescr = "ScrollBarVisibleDescr";

		// Token: 0x04000749 RID: 1865
		internal const string SecurityAboutDialog = "SecurityAboutDialog";

		// Token: 0x0400074A RID: 1866
		internal const string SecurityApplication = "SecurityApplication";

		// Token: 0x0400074B RID: 1867
		internal const string SecurityAsmNameColumn = "SecurityAsmNameColumn";

		// Token: 0x0400074C RID: 1868
		internal const string SecurityAssembliesTab = "SecurityAssembliesTab";

		// Token: 0x0400074D RID: 1869
		internal const string SecurityBugReportLabel = "SecurityBugReportLabel";

		// Token: 0x0400074E RID: 1870
		internal const string SecurityBugReportTab = "SecurityBugReportTab";

		// Token: 0x0400074F RID: 1871
		internal const string SecurityClose = "SecurityClose";

		// Token: 0x04000750 RID: 1872
		internal const string SecurityCodeBaseColumn = "SecurityCodeBaseColumn";

		// Token: 0x04000751 RID: 1873
		internal const string SecurityFileVersionColumn = "SecurityFileVersionColumn";

		// Token: 0x04000752 RID: 1874
		internal const string SecurityIncludeAppInfo = "SecurityIncludeAppInfo";

		// Token: 0x04000753 RID: 1875
		internal const string SecurityIncludeSysInfo = "SecurityIncludeSysInfo";

		// Token: 0x04000754 RID: 1876
		internal const string SecurityInfoTab = "SecurityInfoTab";

		// Token: 0x04000755 RID: 1877
		internal const string SecurityRestrictedText = "SecurityRestrictedText";

		// Token: 0x04000756 RID: 1878
		internal const string SecurityRestrictedWindowTextMixedZone = "SecurityRestrictedWindowTextMixedZone";

		// Token: 0x04000757 RID: 1879
		internal const string SecurityRestrictedWindowTextMultipleSites = "SecurityRestrictedWindowTextMultipleSites";

		// Token: 0x04000758 RID: 1880
		internal const string SecurityRestrictedWindowTextUnknownSite = "SecurityRestrictedWindowTextUnknownSite";

		// Token: 0x04000759 RID: 1881
		internal const string SecurityRestrictedWindowTextUnknownZone = "SecurityRestrictedWindowTextUnknownZone";

		// Token: 0x0400075A RID: 1882
		internal const string SecuritySaveBug = "SecuritySaveBug";

		// Token: 0x0400075B RID: 1883
		internal const string SecuritySaveFilter = "SecuritySaveFilter";

		// Token: 0x0400075C RID: 1884
		internal const string SecuritySubmitBug = "SecuritySubmitBug";

		// Token: 0x0400075D RID: 1885
		internal const string SecuritySubmitBugUrl = "SecuritySubmitBugUrl";

		// Token: 0x0400075E RID: 1886
		internal const string SecuritySwitchDescrColumn = "SecuritySwitchDescrColumn";

		// Token: 0x0400075F RID: 1887
		internal const string SecuritySwitchesTab = "SecuritySwitchesTab";

		// Token: 0x04000760 RID: 1888
		internal const string SecuritySwitchLabel = "SecuritySwitchLabel";

		// Token: 0x04000761 RID: 1889
		internal const string SecuritySwitchNameColumn = "SecuritySwitchNameColumn";

		// Token: 0x04000762 RID: 1890
		internal const string SecurityToolTipCaption = "SecurityToolTipCaption";

		// Token: 0x04000763 RID: 1891
		internal const string SecurityToolTipMainText = "SecurityToolTipMainText";

		// Token: 0x04000764 RID: 1892
		internal const string SecurityToolTipSourceInformation = "SecurityToolTipSourceInformation";

		// Token: 0x04000765 RID: 1893
		internal const string SecurityToolTipTextFormat = "SecurityToolTipTextFormat";

		// Token: 0x04000766 RID: 1894
		internal const string SecurityUnrestrictedText = "SecurityUnrestrictedText";

		// Token: 0x04000767 RID: 1895
		internal const string SecurityVersionColumn = "SecurityVersionColumn";

		// Token: 0x04000768 RID: 1896
		internal const string selectedIndexChangedEventDescr = "selectedIndexChangedEventDescr";

		// Token: 0x04000769 RID: 1897
		internal const string selectedIndexDescr = "selectedIndexDescr";

		// Token: 0x0400076A RID: 1898
		internal const string SelectedNotEqualActual = "SelectedNotEqualActual";

		// Token: 0x0400076B RID: 1899
		internal const string selectionChangeCommittedEventDescr = "selectionChangeCommittedEventDescr";

		// Token: 0x0400076C RID: 1900
		internal const string SelTabCountRange = "SelTabCountRange";

		// Token: 0x0400076D RID: 1901
		internal const string SendKeysGroupDelimError = "SendKeysGroupDelimError";

		// Token: 0x0400076E RID: 1902
		internal const string SendKeysHookFailed = "SendKeysHookFailed";

		// Token: 0x0400076F RID: 1903
		internal const string SendKeysKeywordDelimError = "SendKeysKeywordDelimError";

		// Token: 0x04000770 RID: 1904
		internal const string SendKeysNestingError = "SendKeysNestingError";

		// Token: 0x04000771 RID: 1905
		internal const string SendKeysNoMessageLoop = "SendKeysNoMessageLoop";

		// Token: 0x04000772 RID: 1906
		internal const string SerializationException = "SerializationException";

		// Token: 0x04000773 RID: 1907
		internal const string ShowDialogOnDisabled = "ShowDialogOnDisabled";

		// Token: 0x04000774 RID: 1908
		internal const string ShowDialogOnModal = "ShowDialogOnModal";

		// Token: 0x04000775 RID: 1909
		internal const string ShowDialogOnNonTopLevel = "ShowDialogOnNonTopLevel";

		// Token: 0x04000776 RID: 1910
		internal const string ShowDialogOnVisible = "ShowDialogOnVisible";

		// Token: 0x04000777 RID: 1911
		internal const string SortRequiresIBindingList = "SortRequiresIBindingList";

		// Token: 0x04000778 RID: 1912
		internal const string SplitContainerFixedPanelDescr = "SplitContainerFixedPanelDescr";

		// Token: 0x04000779 RID: 1913
		internal const string SplitContainerIsSplitterFixedDescr = "SplitContainerIsSplitterFixedDescr";

		// Token: 0x0400077A RID: 1914
		internal const string SplitContainerOrientationDescr = "SplitContainerOrientationDescr";

		// Token: 0x0400077B RID: 1915
		internal const string SplitContainerPanel1CollapsedDescr = "SplitContainerPanel1CollapsedDescr";

		// Token: 0x0400077C RID: 1916
		internal const string SplitContainerPanel1Descr = "SplitContainerPanel1Descr";

		// Token: 0x0400077D RID: 1917
		internal const string SplitContainerPanel1MinSizeDescr = "SplitContainerPanel1MinSizeDescr";

		// Token: 0x0400077E RID: 1918
		internal const string SplitContainerPanel2CollapsedDescr = "SplitContainerPanel2CollapsedDescr";

		// Token: 0x0400077F RID: 1919
		internal const string SplitContainerPanel2Descr = "SplitContainerPanel2Descr";

		// Token: 0x04000780 RID: 1920
		internal const string SplitContainerPanel2MinSizeDescr = "SplitContainerPanel2MinSizeDescr";

		// Token: 0x04000781 RID: 1921
		internal const string SplitContainerPanelHeight = "SplitContainerPanelHeight";

		// Token: 0x04000782 RID: 1922
		internal const string SplitContainerPanelWidth = "SplitContainerPanelWidth";

		// Token: 0x04000783 RID: 1923
		internal const string SplitContainerSplitterDistanceDescr = "SplitContainerSplitterDistanceDescr";

		// Token: 0x04000784 RID: 1924
		internal const string SplitContainerSplitterIncrementDescr = "SplitContainerSplitterIncrementDescr";

		// Token: 0x04000785 RID: 1925
		internal const string SplitContainerSplitterRectangleDescr = "SplitContainerSplitterRectangleDescr";

		// Token: 0x04000786 RID: 1926
		internal const string SplitContainerSplitterWidthDescr = "SplitContainerSplitterWidthDescr";

		// Token: 0x04000787 RID: 1927
		internal const string SplitterBorderStyleDescr = "SplitterBorderStyleDescr";

		// Token: 0x04000788 RID: 1928
		internal const string SplitterDistanceNotAllowed = "SplitterDistanceNotAllowed";

		// Token: 0x04000789 RID: 1929
		internal const string SplitterInvalidDockEnum = "SplitterInvalidDockEnum";

		// Token: 0x0400078A RID: 1930
		internal const string SplitterMinExtraDescr = "SplitterMinExtraDescr";

		// Token: 0x0400078B RID: 1931
		internal const string SplitterMinSizeDescr = "SplitterMinSizeDescr";

		// Token: 0x0400078C RID: 1932
		internal const string SplitterSplitPositionDescr = "SplitterSplitPositionDescr";

		// Token: 0x0400078D RID: 1933
		internal const string SplitterSplitterMovedDescr = "SplitterSplitterMovedDescr";

		// Token: 0x0400078E RID: 1934
		internal const string SplitterSplitterMovingDescr = "SplitterSplitterMovingDescr";

		// Token: 0x0400078F RID: 1935
		internal const string StatusBarAddFailed = "StatusBarAddFailed";

		// Token: 0x04000790 RID: 1936
		internal const string StatusBarBadStatusBarPanel = "StatusBarBadStatusBarPanel";

		// Token: 0x04000791 RID: 1937
		internal const string StatusBarDrawItem = "StatusBarDrawItem";

		// Token: 0x04000792 RID: 1938
		internal const string StatusBarOnPanelClickDescr = "StatusBarOnPanelClickDescr";

		// Token: 0x04000793 RID: 1939
		internal const string StatusBarPanelAlignmentDescr = "StatusBarPanelAlignmentDescr";

		// Token: 0x04000794 RID: 1940
		internal const string StatusBarPanelAutoSizeDescr = "StatusBarPanelAutoSizeDescr";

		// Token: 0x04000795 RID: 1941
		internal const string StatusBarPanelBorderStyleDescr = "StatusBarPanelBorderStyleDescr";

		// Token: 0x04000796 RID: 1942
		internal const string StatusBarPanelIconDescr = "StatusBarPanelIconDescr";

		// Token: 0x04000797 RID: 1943
		internal const string StatusBarPanelMinWidthDescr = "StatusBarPanelMinWidthDescr";

		// Token: 0x04000798 RID: 1944
		internal const string StatusBarPanelNameDescr = "StatusBarPanelNameDescr";

		// Token: 0x04000799 RID: 1945
		internal const string StatusBarPanelsDescr = "StatusBarPanelsDescr";

		// Token: 0x0400079A RID: 1946
		internal const string StatusBarPanelStyleDescr = "StatusBarPanelStyleDescr";

		// Token: 0x0400079B RID: 1947
		internal const string StatusBarPanelTextDescr = "StatusBarPanelTextDescr";

		// Token: 0x0400079C RID: 1948
		internal const string StatusBarPanelToolTipTextDescr = "StatusBarPanelToolTipTextDescr";

		// Token: 0x0400079D RID: 1949
		internal const string StatusBarPanelWidthDescr = "StatusBarPanelWidthDescr";

		// Token: 0x0400079E RID: 1950
		internal const string StatusBarShowPanelsDescr = "StatusBarShowPanelsDescr";

		// Token: 0x0400079F RID: 1951
		internal const string StatusBarSizingGripDescr = "StatusBarSizingGripDescr";

		// Token: 0x040007A0 RID: 1952
		internal const string StatusStripPanelBorderSidesDescr = "StatusStripPanelBorderSidesDescr";

		// Token: 0x040007A1 RID: 1953
		internal const string StatusStripPanelBorderStyleDescr = "StatusStripPanelBorderStyleDescr";

		// Token: 0x040007A2 RID: 1954
		internal const string StatusStripSizingGripDescr = "StatusStripSizingGripDescr";

		// Token: 0x040007A3 RID: 1955
		internal const string SystemInformationFeatureNotSupported = "SystemInformationFeatureNotSupported";

		// Token: 0x040007A4 RID: 1956
		internal const string TabBaseAlignmentDescr = "TabBaseAlignmentDescr";

		// Token: 0x040007A5 RID: 1957
		internal const string TabBaseAppearanceDescr = "TabBaseAppearanceDescr";

		// Token: 0x040007A6 RID: 1958
		internal const string TabBaseDrawModeDescr = "TabBaseDrawModeDescr";

		// Token: 0x040007A7 RID: 1959
		internal const string TabBaseHotTrackDescr = "TabBaseHotTrackDescr";

		// Token: 0x040007A8 RID: 1960
		internal const string TabBaseImageListDescr = "TabBaseImageListDescr";

		// Token: 0x040007A9 RID: 1961
		internal const string TabBaseItemSizeDescr = "TabBaseItemSizeDescr";

		// Token: 0x040007AA RID: 1962
		internal const string TabBaseMultilineDescr = "TabBaseMultilineDescr";

		// Token: 0x040007AB RID: 1963
		internal const string TabBasePaddingDescr = "TabBasePaddingDescr";

		// Token: 0x040007AC RID: 1964
		internal const string TabBaseRowCountDescr = "TabBaseRowCountDescr";

		// Token: 0x040007AD RID: 1965
		internal const string TabBaseShowToolTipsDescr = "TabBaseShowToolTipsDescr";

		// Token: 0x040007AE RID: 1966
		internal const string TabBaseSizeModeDescr = "TabBaseSizeModeDescr";

		// Token: 0x040007AF RID: 1967
		internal const string TabBaseTabCountDescr = "TabBaseTabCountDescr";

		// Token: 0x040007B0 RID: 1968
		internal const string TabControlDeselectedEventDescr = "TabControlDeselectedEventDescr";

		// Token: 0x040007B1 RID: 1969
		internal const string TabControlDeselectingEventDescr = "TabControlDeselectingEventDescr";

		// Token: 0x040007B2 RID: 1970
		internal const string TabControlInvalidTabPageType = "TabControlInvalidTabPageType";

		// Token: 0x040007B3 RID: 1971
		internal const string TabControlSelectedEventDescr = "TabControlSelectedEventDescr";

		// Token: 0x040007B4 RID: 1972
		internal const string TabControlSelectedTabDescr = "TabControlSelectedTabDescr";

		// Token: 0x040007B5 RID: 1973
		internal const string TabControlSelectingEventDescr = "TabControlSelectingEventDescr";

		// Token: 0x040007B6 RID: 1974
		internal const string TABCONTROLTabPageNotOnTabControl = "TABCONTROLTabPageNotOnTabControl";

		// Token: 0x040007B7 RID: 1975
		internal const string TABCONTROLTabPageOnTabPage = "TABCONTROLTabPageOnTabPage";

		// Token: 0x040007B8 RID: 1976
		internal const string TabControlTabsDescr = "TabControlTabsDescr";

		// Token: 0x040007B9 RID: 1977
		internal const string TabItemImageIndexDescr = "TabItemImageIndexDescr";

		// Token: 0x040007BA RID: 1978
		internal const string TabItemToolTipTextDescr = "TabItemToolTipTextDescr";

		// Token: 0x040007BB RID: 1979
		internal const string TabItemUseVisualStyleBackColorDescr = "TabItemUseVisualStyleBackColorDescr";

		// Token: 0x040007BC RID: 1980
		internal const string TableBeginMustBeCalledPrior = "TableBeginMustBeCalledPrior";

		// Token: 0x040007BD RID: 1981
		internal const string TableBeginNotCalled = "TableBeginNotCalled";

		// Token: 0x040007BE RID: 1982
		internal const string TableLayoutPanelCellBorderStyleDescr = "TableLayoutPanelCellBorderStyleDescr";

		// Token: 0x040007BF RID: 1983
		internal const string TableLayoutPanelFullDesc = "TableLayoutPanelFullDesc";

		// Token: 0x040007C0 RID: 1984
		internal const string TableLayoutPanelGrowStyleDescr = "TableLayoutPanelGrowStyleDescr";

		// Token: 0x040007C1 RID: 1985
		internal const string TableLayoutPanelOnPaintCellDescr = "TableLayoutPanelOnPaintCellDescr";

		// Token: 0x040007C2 RID: 1986
		internal const string TableLayoutPanelSpanDesc = "TableLayoutPanelSpanDesc";

		// Token: 0x040007C3 RID: 1987
		internal const string TableLayoutSettingSettingsIsNotSupported = "TableLayoutSettingSettingsIsNotSupported";

		// Token: 0x040007C4 RID: 1988
		internal const string TableLayoutSettingsGetCellPositionDescr = "TableLayoutSettingsGetCellPositionDescr";

		// Token: 0x040007C5 RID: 1989
		internal const string TableLayoutSettingsSetCellPositionDescr = "TableLayoutSettingsSetCellPositionDescr";

		// Token: 0x040007C6 RID: 1990
		internal const string TablePrintLayoutFromDifferentDocument = "TablePrintLayoutFromDifferentDocument";

		// Token: 0x040007C7 RID: 1991
		internal const string TextBoxAcceptsReturnDescr = "TextBoxAcceptsReturnDescr";

		// Token: 0x040007C8 RID: 1992
		internal const string TextBoxAcceptsTabDescr = "TextBoxAcceptsTabDescr";

		// Token: 0x040007C9 RID: 1993
		internal const string TextBoxAutoCompleteCustomSourceDescr = "TextBoxAutoCompleteCustomSourceDescr";

		// Token: 0x040007CA RID: 1994
		internal const string TextBoxAutoCompleteModeDescr = "TextBoxAutoCompleteModeDescr";

		// Token: 0x040007CB RID: 1995
		internal const string TextBoxAutoCompleteSourceDescr = "TextBoxAutoCompleteSourceDescr";

		// Token: 0x040007CC RID: 1996
		internal const string TextBoxAutoCompleteSourceNoItems = "TextBoxAutoCompleteSourceNoItems";

		// Token: 0x040007CD RID: 1997
		internal const string TextBoxAutoSizeDescr = "TextBoxAutoSizeDescr";

		// Token: 0x040007CE RID: 1998
		internal const string TextBoxBaseOnAcceptsTabChangedDescr = "TextBoxBaseOnAcceptsTabChangedDescr";

		// Token: 0x040007CF RID: 1999
		internal const string TextBoxBaseOnAutoSizeChangedDescr = "TextBoxBaseOnAutoSizeChangedDescr";

		// Token: 0x040007D0 RID: 2000
		internal const string TextBoxBaseOnBorderStyleChangedDescr = "TextBoxBaseOnBorderStyleChangedDescr";

		// Token: 0x040007D1 RID: 2001
		internal const string TextBoxBaseOnHideSelectionChangedDescr = "TextBoxBaseOnHideSelectionChangedDescr";

		// Token: 0x040007D2 RID: 2002
		internal const string TextBoxBaseOnModifiedChangedDescr = "TextBoxBaseOnModifiedChangedDescr";

		// Token: 0x040007D3 RID: 2003
		internal const string TextBoxBaseOnMultilineChangedDescr = "TextBoxBaseOnMultilineChangedDescr";

		// Token: 0x040007D4 RID: 2004
		internal const string TextBoxBaseOnReadOnlyChangedDescr = "TextBoxBaseOnReadOnlyChangedDescr";

		// Token: 0x040007D5 RID: 2005
		internal const string TextBoxBorderDescr = "TextBoxBorderDescr";

		// Token: 0x040007D6 RID: 2006
		internal const string TextBoxCanUndoDescr = "TextBoxCanUndoDescr";

		// Token: 0x040007D7 RID: 2007
		internal const string TextBoxCharacterCasingDescr = "TextBoxCharacterCasingDescr";

		// Token: 0x040007D8 RID: 2008
		internal const string TextBoxHideSelectionDescr = "TextBoxHideSelectionDescr";

		// Token: 0x040007D9 RID: 2009
		internal const string TextBoxLinesDescr = "TextBoxLinesDescr";

		// Token: 0x040007DA RID: 2010
		internal const string TextBoxMaxLengthDescr = "TextBoxMaxLengthDescr";

		// Token: 0x040007DB RID: 2011
		internal const string TextBoxModifiedDescr = "TextBoxModifiedDescr";

		// Token: 0x040007DC RID: 2012
		internal const string TextBoxMultilineDescr = "TextBoxMultilineDescr";

		// Token: 0x040007DD RID: 2013
		internal const string TextBoxPasswordCharDescr = "TextBoxPasswordCharDescr";

		// Token: 0x040007DE RID: 2014
		internal const string TextBoxPreferredHeightDescr = "TextBoxPreferredHeightDescr";

		// Token: 0x040007DF RID: 2015
		internal const string TextBoxReadOnlyDescr = "TextBoxReadOnlyDescr";

		// Token: 0x040007E0 RID: 2016
		internal const string TextBoxScrollBarsDescr = "TextBoxScrollBarsDescr";

		// Token: 0x040007E1 RID: 2017
		internal const string TextBoxSelectedTextDescr = "TextBoxSelectedTextDescr";

		// Token: 0x040007E2 RID: 2018
		internal const string TextBoxSelectionLengthDescr = "TextBoxSelectionLengthDescr";

		// Token: 0x040007E3 RID: 2019
		internal const string TextBoxSelectionStartDescr = "TextBoxSelectionStartDescr";

		// Token: 0x040007E4 RID: 2020
		internal const string TextBoxShortcutsEnabledDescr = "TextBoxShortcutsEnabledDescr";

		// Token: 0x040007E5 RID: 2021
		internal const string TextBoxTextAlignDescr = "TextBoxTextAlignDescr";

		// Token: 0x040007E6 RID: 2022
		internal const string TextBoxUseSystemPasswordCharDescr = "TextBoxUseSystemPasswordCharDescr";

		// Token: 0x040007E7 RID: 2023
		internal const string TextBoxWordWrapDescr = "TextBoxWordWrapDescr";

		// Token: 0x040007E8 RID: 2024
		internal const string TextParseFailedFormat = "TextParseFailedFormat";

		// Token: 0x040007E9 RID: 2025
		internal const string ThreadMustBeSTA = "ThreadMustBeSTA";

		// Token: 0x040007EA RID: 2026
		internal const string ThreadNoLongerValid = "ThreadNoLongerValid";

		// Token: 0x040007EB RID: 2027
		internal const string ThreadNotPumpingMessages = "ThreadNotPumpingMessages";

		// Token: 0x040007EC RID: 2028
		internal const string TimerEnabledDescr = "TimerEnabledDescr";

		// Token: 0x040007ED RID: 2029
		internal const string TimerIntervalDescr = "TimerIntervalDescr";

		// Token: 0x040007EE RID: 2030
		internal const string TimerInvalidInterval = "TimerInvalidInterval";

		// Token: 0x040007EF RID: 2031
		internal const string TimerTimerDescr = "TimerTimerDescr";

		// Token: 0x040007F0 RID: 2032
		internal const string ToolBarAppearanceDescr = "ToolBarAppearanceDescr";

		// Token: 0x040007F1 RID: 2033
		internal const string ToolBarAutoSizeDescr = "ToolBarAutoSizeDescr";

		// Token: 0x040007F2 RID: 2034
		internal const string ToolBarBadToolBarButton = "ToolBarBadToolBarButton";

		// Token: 0x040007F3 RID: 2035
		internal const string ToolBarBorderStyleDescr = "ToolBarBorderStyleDescr";

		// Token: 0x040007F4 RID: 2036
		internal const string ToolBarButtonClickDescr = "ToolBarButtonClickDescr";

		// Token: 0x040007F5 RID: 2037
		internal const string ToolBarButtonDropDownDescr = "ToolBarButtonDropDownDescr";

		// Token: 0x040007F6 RID: 2038
		internal const string ToolBarButtonEnabledDescr = "ToolBarButtonEnabledDescr";

		// Token: 0x040007F7 RID: 2039
		internal const string ToolBarButtonImageIndexDescr = "ToolBarButtonImageIndexDescr";

		// Token: 0x040007F8 RID: 2040
		internal const string ToolBarButtonInvalidDropDownMenuType = "ToolBarButtonInvalidDropDownMenuType";

		// Token: 0x040007F9 RID: 2041
		internal const string ToolBarButtonMenuDescr = "ToolBarButtonMenuDescr";

		// Token: 0x040007FA RID: 2042
		internal const string ToolBarButtonNotFound = "ToolBarButtonNotFound";

		// Token: 0x040007FB RID: 2043
		internal const string ToolBarButtonPartialPushDescr = "ToolBarButtonPartialPushDescr";

		// Token: 0x040007FC RID: 2044
		internal const string ToolBarButtonPushedDescr = "ToolBarButtonPushedDescr";

		// Token: 0x040007FD RID: 2045
		internal const string ToolBarButtonsDescr = "ToolBarButtonsDescr";

		// Token: 0x040007FE RID: 2046
		internal const string ToolBarButtonSizeDescr = "ToolBarButtonSizeDescr";

		// Token: 0x040007FF RID: 2047
		internal const string ToolBarButtonStyleDescr = "ToolBarButtonStyleDescr";

		// Token: 0x04000800 RID: 2048
		internal const string ToolBarButtonTextDescr = "ToolBarButtonTextDescr";

		// Token: 0x04000801 RID: 2049
		internal const string ToolBarButtonToolTipTextDescr = "ToolBarButtonToolTipTextDescr";

		// Token: 0x04000802 RID: 2050
		internal const string ToolBarButtonVisibleDescr = "ToolBarButtonVisibleDescr";

		// Token: 0x04000803 RID: 2051
		internal const string ToolBarDividerDescr = "ToolBarDividerDescr";

		// Token: 0x04000804 RID: 2052
		internal const string ToolBarDropDownArrowsDescr = "ToolBarDropDownArrowsDescr";

		// Token: 0x04000805 RID: 2053
		internal const string ToolBarImageListDescr = "ToolBarImageListDescr";

		// Token: 0x04000806 RID: 2054
		internal const string ToolBarImageSizeDescr = "ToolBarImageSizeDescr";

		// Token: 0x04000807 RID: 2055
		internal const string ToolBarShowToolTipsDescr = "ToolBarShowToolTipsDescr";

		// Token: 0x04000808 RID: 2056
		internal const string ToolBarTextAlignDescr = "ToolBarTextAlignDescr";

		// Token: 0x04000809 RID: 2057
		internal const string ToolBarWrappableDescr = "ToolBarWrappableDescr";

		// Token: 0x0400080A RID: 2058
		internal const string ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue = "ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue";

		// Token: 0x0400080B RID: 2059
		internal const string ToolStripAllowItemReorderDescr = "ToolStripAllowItemReorderDescr";

		// Token: 0x0400080C RID: 2060
		internal const string ToolStripAllowMergeDescr = "ToolStripAllowMergeDescr";

		// Token: 0x0400080D RID: 2061
		internal const string ToolStripBackColorDescr = "ToolStripBackColorDescr";

		// Token: 0x0400080E RID: 2062
		internal const string ToolStripButtonCheckedDescr = "ToolStripButtonCheckedDescr";

		// Token: 0x0400080F RID: 2063
		internal const string ToolStripButtonCheckOnClickDescr = "ToolStripButtonCheckOnClickDescr";

		// Token: 0x04000810 RID: 2064
		internal const string ToolStripCanOnlyPositionItsOwnItems = "ToolStripCanOnlyPositionItsOwnItems";

		// Token: 0x04000811 RID: 2065
		internal const string ToolStripCanOverflowDescr = "ToolStripCanOverflowDescr";

		// Token: 0x04000812 RID: 2066
		internal const string ToolStripCollectionMustInsertAndRemove = "ToolStripCollectionMustInsertAndRemove";

		// Token: 0x04000813 RID: 2067
		internal const string ToolStripContainerBottomToolStripPanelDescr = "ToolStripContainerBottomToolStripPanelDescr";

		// Token: 0x04000814 RID: 2068
		internal const string ToolStripContainerBottomToolStripPanelVisibleDescr = "ToolStripContainerBottomToolStripPanelVisibleDescr";

		// Token: 0x04000815 RID: 2069
		internal const string ToolStripContainerContentPanelDescr = "ToolStripContainerContentPanelDescr";

		// Token: 0x04000816 RID: 2070
		internal const string ToolStripContainerDesc = "ToolStripContainerDesc";

		// Token: 0x04000817 RID: 2071
		internal const string ToolStripContainerLeftToolStripPanelDescr = "ToolStripContainerLeftToolStripPanelDescr";

		// Token: 0x04000818 RID: 2072
		internal const string ToolStripContainerLeftToolStripPanelVisibleDescr = "ToolStripContainerLeftToolStripPanelVisibleDescr";

		// Token: 0x04000819 RID: 2073
		internal const string ToolStripContainerRightToolStripPanelDescr = "ToolStripContainerRightToolStripPanelDescr";

		// Token: 0x0400081A RID: 2074
		internal const string ToolStripContainerRightToolStripPanelVisibleDescr = "ToolStripContainerRightToolStripPanelVisibleDescr";

		// Token: 0x0400081B RID: 2075
		internal const string ToolStripContainerTopToolStripPanelDescr = "ToolStripContainerTopToolStripPanelDescr";

		// Token: 0x0400081C RID: 2076
		internal const string ToolStripContainerTopToolStripPanelVisibleDescr = "ToolStripContainerTopToolStripPanelVisibleDescr";

		// Token: 0x0400081D RID: 2077
		internal const string ToolStripContainerUseContentPanel = "ToolStripContainerUseContentPanel";

		// Token: 0x0400081E RID: 2078
		internal const string ToolStripContentPanelOnLoadDescr = "ToolStripContentPanelOnLoadDescr";

		// Token: 0x0400081F RID: 2079
		internal const string ToolStripDefaultDropDownDirectionDescr = "ToolStripDefaultDropDownDirectionDescr";

		// Token: 0x04000820 RID: 2080
		internal const string ToolStripDoesntSupportAutoScroll = "ToolStripDoesntSupportAutoScroll";

		// Token: 0x04000821 RID: 2081
		internal const string ToolStripDropDownAutoCloseDescr = "ToolStripDropDownAutoCloseDescr";

		// Token: 0x04000822 RID: 2082
		internal const string ToolStripDropDownButtonShowDropDownArrowDescr = "ToolStripDropDownButtonShowDropDownArrowDescr";

		// Token: 0x04000823 RID: 2083
		internal const string ToolStripDropDownClosedDecr = "ToolStripDropDownClosedDecr";

		// Token: 0x04000824 RID: 2084
		internal const string ToolStripDropDownClosingDecr = "ToolStripDropDownClosingDecr";

		// Token: 0x04000825 RID: 2085
		internal const string ToolStripDropDownDescr = "ToolStripDropDownDescr";

		// Token: 0x04000826 RID: 2086
		internal const string ToolStripDropDownItemDropDownDirectionDescr = "ToolStripDropDownItemDropDownDirectionDescr";

		// Token: 0x04000827 RID: 2087
		internal const string ToolStripDropDownItemsDescr = "ToolStripDropDownItemsDescr";

		// Token: 0x04000828 RID: 2088
		internal const string ToolStripDropDownMenuShowCheckMarginDescr = "ToolStripDropDownMenuShowCheckMarginDescr";

		// Token: 0x04000829 RID: 2089
		internal const string ToolStripDropDownMenuShowImageMarginDescr = "ToolStripDropDownMenuShowImageMarginDescr";

		// Token: 0x0400082A RID: 2090
		internal const string ToolStripDropDownOpenedDescr = "ToolStripDropDownOpenedDescr";

		// Token: 0x0400082B RID: 2091
		internal const string ToolStripDropDownOpeningDescr = "ToolStripDropDownOpeningDescr";

		// Token: 0x0400082C RID: 2092
		internal const string ToolStripDropDownPreferredWidthDescr = "ToolStripDropDownPreferredWidthDescr";

		// Token: 0x0400082D RID: 2093
		internal const string ToolStripGripAccessibleName = "ToolStripGripAccessibleName";

		// Token: 0x0400082E RID: 2094
		internal const string ToolStripDropDownsCantBeRafted = "ToolStripDropDownsCantBeRafted";

		// Token: 0x0400082F RID: 2095
		internal const string ToolStripGripDisplayStyleDescr = "ToolStripGripDisplayStyleDescr";

		// Token: 0x04000830 RID: 2096
		internal const string ToolStripGripMargin = "ToolStripGripMargin";

		// Token: 0x04000831 RID: 2097
		internal const string ToolStripGripStyleDescr = "ToolStripGripStyleDescr";

		// Token: 0x04000832 RID: 2098
		internal const string ToolStripImageListDescr = "ToolStripImageListDescr";

		// Token: 0x04000833 RID: 2099
		internal const string ToolStripImageScalingSizeDescr = "ToolStripImageScalingSizeDescr";

		// Token: 0x04000834 RID: 2100
		internal const string ToolStripItemAccessibilityObjectDescr = "ToolStripItemAccessibilityObjectDescr";

		// Token: 0x04000835 RID: 2101
		internal const string ToolStripItemAccessibleDefaultActionDescr = "ToolStripItemAccessibleDefaultActionDescr";

		// Token: 0x04000836 RID: 2102
		internal const string ToolStripItemAccessibleDescriptionDescr = "ToolStripItemAccessibleDescriptionDescr";

		// Token: 0x04000837 RID: 2103
		internal const string ToolStripItemAccessibleNameDescr = "ToolStripItemAccessibleNameDescr";

		// Token: 0x04000838 RID: 2104
		internal const string ToolStripItemAccessibleRoleDescr = "ToolStripItemAccessibleRoleDescr";

		// Token: 0x04000839 RID: 2105
		internal const string ToolStripItemAddedDescr = "ToolStripItemAddedDescr";

		// Token: 0x0400083A RID: 2106
		internal const string ToolStripItemAlignment = "ToolStripItemAlignment";

		// Token: 0x0400083B RID: 2107
		internal const string ToolStripItemAlignmentDescr = "ToolStripItemAlignmentDescr";

		// Token: 0x0400083C RID: 2108
		internal const string ToolStripItemAllowDropDescr = "ToolStripItemAllowDropDescr";

		// Token: 0x0400083D RID: 2109
		internal const string ToolStripItemAutoSizeDescr = "ToolStripItemAutoSizeDescr";

		// Token: 0x0400083E RID: 2110
		internal const string ToolStripItemAutoToolTipDescr = "ToolStripItemAutoToolTipDescr";

		// Token: 0x0400083F RID: 2111
		internal const string ToolStripItemAvailableDescr = "ToolStripItemAvailableDescr";

		// Token: 0x04000840 RID: 2112
		internal const string ToolStripItemBackColorDescr = "ToolStripItemBackColorDescr";

		// Token: 0x04000841 RID: 2113
		internal const string ToolStripItemCircularReference = "ToolStripItemCircularReference";

		// Token: 0x04000842 RID: 2114
		internal const string ToolStripItemCollectionIsReadOnly = "ToolStripItemCollectionIsReadOnly";

		// Token: 0x04000843 RID: 2115
		internal const string ToolStripItemDisplayStyleDescr = "ToolStripItemDisplayStyleDescr";

		// Token: 0x04000844 RID: 2116
		internal const string ToolStripItemDoubleClickedEnabledDescr = "ToolStripItemDoubleClickedEnabledDescr";

		// Token: 0x04000845 RID: 2117
		internal const string ToolStripItemDrawModeDescr = "ToolStripItemDrawModeDescr";

		// Token: 0x04000846 RID: 2118
		internal const string ToolStripItemEnabledChangedDescr = "ToolStripItemEnabledChangedDescr";

		// Token: 0x04000847 RID: 2119
		internal const string ToolStripItemEnabledDescr = "ToolStripItemEnabledDescr";

		// Token: 0x04000848 RID: 2120
		internal const string ToolStripItemFontDescr = "ToolStripItemFontDescr";

		// Token: 0x04000849 RID: 2121
		internal const string ToolStripItemForeColorDescr = "ToolStripItemForeColorDescr";

		// Token: 0x0400084A RID: 2122
		internal const string ToolStripItemImageAlignDescr = "ToolStripItemImageAlignDescr";

		// Token: 0x0400084B RID: 2123
		internal const string ToolStripItemImageDescr = "ToolStripItemImageDescr";

		// Token: 0x0400084C RID: 2124
		internal const string ToolStripItemImageIndexDescr = "ToolStripItemImageIndexDescr";

		// Token: 0x0400084D RID: 2125
		internal const string ToolStripItemImageKeyDescr = "ToolStripItemImageKeyDescr";

		// Token: 0x0400084E RID: 2126
		internal const string ToolStripItemImageList = "ToolStripItemImageList";

		// Token: 0x0400084F RID: 2127
		internal const string ToolStripItemImageScalingDescr = "ToolStripItemImageScalingDescr";

		// Token: 0x04000850 RID: 2128
		internal const string ToolStripItemImageTransparentColorDescr = "ToolStripItemImageTransparentColorDescr";

		// Token: 0x04000851 RID: 2129
		internal const string ToolStripItemMarginDescr = "ToolStripItemMarginDescr";

		// Token: 0x04000852 RID: 2130
		internal const string ToolStripItemOnAvailableChangedDescr = "ToolStripItemOnAvailableChangedDescr";

		// Token: 0x04000853 RID: 2131
		internal const string ToolStripItemOnBackColorChangedDescr = "ToolStripItemOnBackColorChangedDescr";

		// Token: 0x04000854 RID: 2132
		internal const string ToolStripItemOnClickDescr = "ToolStripItemOnClickDescr";

		// Token: 0x04000855 RID: 2133
		internal const string ToolStripItemOnDragDropDescr = "ToolStripItemOnDragDropDescr";

		// Token: 0x04000856 RID: 2134
		internal const string ToolStripItemOnDragEnterDescr = "ToolStripItemOnDragEnterDescr";

		// Token: 0x04000857 RID: 2135
		internal const string ToolStripItemOnDragLeaveDescr = "ToolStripItemOnDragLeaveDescr";

		// Token: 0x04000858 RID: 2136
		internal const string ToolStripItemOnDragOverDescr = "ToolStripItemOnDragOverDescr";

		// Token: 0x04000859 RID: 2137
		internal const string ToolStripItemOnForeColorChangedDescr = "ToolStripItemOnForeColorChangedDescr";

		// Token: 0x0400085A RID: 2138
		internal const string ToolStripItemOnGiveFeedbackDescr = "ToolStripItemOnGiveFeedbackDescr";

		// Token: 0x0400085B RID: 2139
		internal const string ToolStripItemOnGotFocusDescr = "ToolStripItemOnGotFocusDescr";

		// Token: 0x0400085C RID: 2140
		internal const string ToolStripItemOnLocationChangedDescr = "ToolStripItemOnLocationChangedDescr";

		// Token: 0x0400085D RID: 2141
		internal const string ToolStripItemOnLostFocusDescr = "ToolStripItemOnLostFocusDescr";

		// Token: 0x0400085E RID: 2142
		internal const string ToolStripItemOnMouseDownDescr = "ToolStripItemOnMouseDownDescr";

		// Token: 0x0400085F RID: 2143
		internal const string ToolStripItemOnMouseEnterDescr = "ToolStripItemOnMouseEnterDescr";

		// Token: 0x04000860 RID: 2144
		internal const string ToolStripItemOnMouseHoverDescr = "ToolStripItemOnMouseHoverDescr";

		// Token: 0x04000861 RID: 2145
		internal const string ToolStripItemOnMouseLeaveDescr = "ToolStripItemOnMouseLeaveDescr";

		// Token: 0x04000862 RID: 2146
		internal const string ToolStripItemOnMouseMoveDescr = "ToolStripItemOnMouseMoveDescr";

		// Token: 0x04000863 RID: 2147
		internal const string ToolStripItemOnMouseUpDescr = "ToolStripItemOnMouseUpDescr";

		// Token: 0x04000864 RID: 2148
		internal const string ToolStripItemOnPaintDescr = "ToolStripItemOnPaintDescr";

		// Token: 0x04000865 RID: 2149
		internal const string ToolStripItemOnQueryAccessibilityHelpDescr = "ToolStripItemOnQueryAccessibilityHelpDescr";

		// Token: 0x04000866 RID: 2150
		internal const string ToolStripItemOnQueryContinueDragDescr = "ToolStripItemOnQueryContinueDragDescr";

		// Token: 0x04000867 RID: 2151
		internal const string ToolStripItemOnRightToLeftChangedDescr = "ToolStripItemOnRightToLeftChangedDescr";

		// Token: 0x04000868 RID: 2152
		internal const string ToolStripItemOnTextChangedDescr = "ToolStripItemOnTextChangedDescr";

		// Token: 0x04000869 RID: 2153
		internal const string ToolStripItemOnVisibleChangedDescr = "ToolStripItemOnVisibleChangedDescr";

		// Token: 0x0400086A RID: 2154
		internal const string ToolStripItemOverflow = "ToolStripItemOverflow";

		// Token: 0x0400086B RID: 2155
		internal const string ToolStripItemOverflowDescr = "ToolStripItemOverflowDescr";

		// Token: 0x0400086C RID: 2156
		internal const string ToolStripItemOwnerChangedDescr = "ToolStripItemOwnerChangedDescr";

		// Token: 0x0400086D RID: 2157
		internal const string ToolStripItemPaddingDescr = "ToolStripItemPaddingDescr";

		// Token: 0x0400086E RID: 2158
		internal const string ToolStripItemRemovedDescr = "ToolStripItemRemovedDescr";

		// Token: 0x0400086F RID: 2159
		internal const string ToolStripItemRightToLeftAutoMirrorImageDescr = "ToolStripItemRightToLeftAutoMirrorImageDescr";

		// Token: 0x04000870 RID: 2160
		internal const string ToolStripItemRightToLeftDescr = "ToolStripItemRightToLeftDescr";

		// Token: 0x04000871 RID: 2161
		internal const string ToolStripItemsDescr = "ToolStripItemsDescr";

		// Token: 0x04000872 RID: 2162
		internal const string ToolStripItemSize = "ToolStripItemSize";

		// Token: 0x04000873 RID: 2163
		internal const string ToolStripItemSizeDescr = "ToolStripItemSizeDescr";

		// Token: 0x04000874 RID: 2164
		internal const string ToolStripItemTagDescr = "ToolStripItemTagDescr";

		// Token: 0x04000875 RID: 2165
		internal const string ToolStripItemTextAlignDescr = "ToolStripItemTextAlignDescr";

		// Token: 0x04000876 RID: 2166
		internal const string ToolStripItemTextDescr = "ToolStripItemTextDescr";

		// Token: 0x04000877 RID: 2167
		internal const string ToolStripItemTextImageRelationDescr = "ToolStripItemTextImageRelationDescr";

		// Token: 0x04000878 RID: 2168
		internal const string ToolStripItemToolTipTextDescr = "ToolStripItemToolTipTextDescr";

		// Token: 0x04000879 RID: 2169
		internal const string ToolStripItemVisibleDescr = "ToolStripItemVisibleDescr";

		// Token: 0x0400087A RID: 2170
		internal const string ToolStripLabelActiveLinkColorDescr = "ToolStripLabelActiveLinkColorDescr";

		// Token: 0x0400087B RID: 2171
		internal const string ToolStripLabelIsLinkDescr = "ToolStripLabelIsLinkDescr";

		// Token: 0x0400087C RID: 2172
		internal const string ToolStripLabelLinkBehaviorDescr = "ToolStripLabelLinkBehaviorDescr";

		// Token: 0x0400087D RID: 2173
		internal const string ToolStripLabelLinkColorDescr = "ToolStripLabelLinkColorDescr";

		// Token: 0x0400087E RID: 2174
		internal const string ToolStripLabelLinkVisitedDescr = "ToolStripLabelLinkVisitedDescr";

		// Token: 0x0400087F RID: 2175
		internal const string ToolStripLabelVisitedLinkColorDescr = "ToolStripLabelVisitedLinkColorDescr";

		// Token: 0x04000880 RID: 2176
		internal const string ToolStripLayoutCompleteDescr = "ToolStripLayoutCompleteDescr";

		// Token: 0x04000881 RID: 2177
		internal const string ToolStripLayoutStyle = "ToolStripLayoutStyle";

		// Token: 0x04000882 RID: 2178
		internal const string ToolStripLayoutStyleChangedDescr = "ToolStripLayoutStyleChangedDescr";

		// Token: 0x04000883 RID: 2179
		internal const string ToolStripMenuItemShortcutKeyDisplayStringDescr = "ToolStripMenuItemShortcutKeyDisplayStringDescr";

		// Token: 0x04000884 RID: 2180
		internal const string ToolStripMergeActionDescr = "ToolStripMergeActionDescr";

		// Token: 0x04000885 RID: 2181
		internal const string ToolStripMergeImpossibleIdentical = "ToolStripMergeImpossibleIdentical";

		// Token: 0x04000886 RID: 2182
		internal const string ToolStripMergeIndexDescr = "ToolStripMergeIndexDescr";

		// Token: 0x04000887 RID: 2183
		internal const string ToolStripMustSupplyItsOwnComboBox = "ToolStripMustSupplyItsOwnComboBox";

		// Token: 0x04000888 RID: 2184
		internal const string ToolStripMustSupplyItsOwnTextBox = "ToolStripMustSupplyItsOwnTextBox";

		// Token: 0x04000889 RID: 2185
		internal const string ToolStripOnBeginDrag = "ToolStripOnBeginDrag";

		// Token: 0x0400088A RID: 2186
		internal const string ToolStripOnEndDrag = "ToolStripOnEndDrag";

		// Token: 0x0400088B RID: 2187
		internal const string ToolStripOptions = "ToolStripOptions";

		// Token: 0x0400088C RID: 2188
		internal const string ToolStripPaintGripDescr = "ToolStripPaintGripDescr";

		// Token: 0x0400088D RID: 2189
		internal const string ToolStripPanelRowsDescr = "ToolStripPanelRowsDescr";

		// Token: 0x0400088E RID: 2190
		internal const string ToolStripPanelRowControlCollectionIncorrectIndexLength = "ToolStripPanelRowControlCollectionIncorrectIndexLength";

		// Token: 0x0400088F RID: 2191
		internal const string ToolStripRendererChanged = "ToolStripRendererChanged";

		// Token: 0x04000890 RID: 2192
		internal const string ToolStripRenderModeDescr = "ToolStripRenderModeDescr";

		// Token: 0x04000891 RID: 2193
		internal const string ToolStripRenderModeUseRendererPropertyInstead = "ToolStripRenderModeUseRendererPropertyInstead";

		// Token: 0x04000892 RID: 2194
		internal const string ToolStripSaveSettingsDescr = "ToolStripSaveSettingsDescr";

		// Token: 0x04000893 RID: 2195
		internal const string ToolStripSettingsKeyDescr = "ToolStripSettingsKeyDescr";

		// Token: 0x04000894 RID: 2196
		internal const string ToolStripShowDropDownInvalidOperation = "ToolStripShowDropDownInvalidOperation";

		// Token: 0x04000895 RID: 2197
		internal const string ToolStripShowItemToolTipsDescr = "ToolStripShowItemToolTipsDescr";

		// Token: 0x04000896 RID: 2198
		internal const string ToolStripSplitButtonDropDownButtonWidthDescr = "ToolStripSplitButtonDropDownButtonWidthDescr";

		// Token: 0x04000897 RID: 2199
		internal const string ToolStripSplitButtonOnButtonClickDescr = "ToolStripSplitButtonOnButtonClickDescr";

		// Token: 0x04000898 RID: 2200
		internal const string ToolStripSplitButtonOnButtonDoubleClickDescr = "ToolStripSplitButtonOnButtonDoubleClickDescr";

		// Token: 0x04000899 RID: 2201
		internal const string ToolStripSplitButtonOnDefaultItemChangedDescr = "ToolStripSplitButtonOnDefaultItemChangedDescr";

		// Token: 0x0400089A RID: 2202
		internal const string ToolStripSplitButtonSplitterWidthDescr = "ToolStripSplitButtonSplitterWidthDescr";

		// Token: 0x0400089B RID: 2203
		internal const string ToolStripSplitStackLayoutContainerMustBeAToolStrip = "ToolStripSplitStackLayoutContainerMustBeAToolStrip";

		// Token: 0x0400089C RID: 2204
		internal const string ToolStripStatusLabelBorderSidesDescr = "ToolStripStatusLabelBorderSidesDescr";

		// Token: 0x0400089D RID: 2205
		internal const string ToolStripStatusLabelBorderStyleDescr = "ToolStripStatusLabelBorderStyleDescr";

		// Token: 0x0400089E RID: 2206
		internal const string ToolStripStatusLabelSpringDescr = "ToolStripStatusLabelSpringDescr";

		// Token: 0x0400089F RID: 2207
		internal const string ToolStripStretchDescr = "ToolStripStretchDescr";

		// Token: 0x040008A0 RID: 2208
		internal const string ToolStripTextBoxTextBoxTextAlignChangedDescr = "ToolStripTextBoxTextBoxTextAlignChangedDescr";

		// Token: 0x040008A1 RID: 2209
		internal const string ToolStripTextDirectionDescr = "ToolStripTextDirectionDescr";

		// Token: 0x040008A2 RID: 2210
		internal const string ToolTipActiveDescr = "ToolTipActiveDescr";

		// Token: 0x040008A3 RID: 2211
		internal const string ToolTipAddFailed = "ToolTipAddFailed";

		// Token: 0x040008A4 RID: 2212
		internal const string ToolTipAutomaticDelayDescr = "ToolTipAutomaticDelayDescr";

		// Token: 0x040008A5 RID: 2213
		internal const string ToolTipAutoPopDelayDescr = "ToolTipAutoPopDelayDescr";

		// Token: 0x040008A6 RID: 2214
		internal const string ToolTipBackColorDescr = "ToolTipBackColorDescr";

		// Token: 0x040008A7 RID: 2215
		internal const string ToolTipDrawEventDescr = "ToolTipDrawEventDescr";

		// Token: 0x040008A8 RID: 2216
		internal const string ToolTipEmptyColor = "ToolTipEmptyColor";

		// Token: 0x040008A9 RID: 2217
		internal const string ToolTipForeColorDescr = "ToolTipForeColorDescr";

		// Token: 0x040008AA RID: 2218
		internal const string ToolTipInitialDelayDescr = "ToolTipInitialDelayDescr";

		// Token: 0x040008AB RID: 2219
		internal const string ToolTipIsBalloonDescr = "ToolTipIsBalloonDescr";

		// Token: 0x040008AC RID: 2220
		internal const string ToolTipOwnerDrawDescr = "ToolTipOwnerDrawDescr";

		// Token: 0x040008AD RID: 2221
		internal const string ToolTipPopupEventDescr = "ToolTipPopupEventDescr";

		// Token: 0x040008AE RID: 2222
		internal const string ToolTipReshowDelayDescr = "ToolTipReshowDelayDescr";

		// Token: 0x040008AF RID: 2223
		internal const string ToolTipShowAlwaysDescr = "ToolTipShowAlwaysDescr";

		// Token: 0x040008B0 RID: 2224
		internal const string ToolTipStripAmpersandsDescr = "ToolTipStripAmpersandsDescr";

		// Token: 0x040008B1 RID: 2225
		internal const string ToolTipTitleDescr = "ToolTipTitleDescr";

		// Token: 0x040008B2 RID: 2226
		internal const string ToolTipToolTipDescr = "ToolTipToolTipDescr";

		// Token: 0x040008B3 RID: 2227
		internal const string ToolTipToolTipIconDescr = "ToolTipToolTipIconDescr";

		// Token: 0x040008B4 RID: 2228
		internal const string ToolTipUseAnimationDescr = "ToolTipUseAnimationDescr";

		// Token: 0x040008B5 RID: 2229
		internal const string ToolTipUseFadingDescr = "ToolTipUseFadingDescr";

		// Token: 0x040008B6 RID: 2230
		internal const string TooManyResumeUpdateMenuHandles = "TooManyResumeUpdateMenuHandles";

		// Token: 0x040008B7 RID: 2231
		internal const string TopLevelControlAdd = "TopLevelControlAdd";

		// Token: 0x040008B8 RID: 2232
		internal const string TopLevelNotAllowedIfActiveX = "TopLevelNotAllowedIfActiveX";

		// Token: 0x040008B9 RID: 2233
		internal const string TopLevelParentedControl = "TopLevelParentedControl";

		// Token: 0x040008BA RID: 2234
		internal const string toStringAlt = "toStringAlt";

		// Token: 0x040008BB RID: 2235
		internal const string toStringBack = "toStringBack";

		// Token: 0x040008BC RID: 2236
		internal const string toStringControl = "toStringControl";

		// Token: 0x040008BD RID: 2237
		internal const string toStringDefault = "toStringDefault";

		// Token: 0x040008BE RID: 2238
		internal const string toStringDelete = "toStringDelete";

		// Token: 0x040008BF RID: 2239
		internal const string toStringEnd = "toStringEnd";

		// Token: 0x040008C0 RID: 2240
		internal const string toStringEnter = "toStringEnter";

		// Token: 0x040008C1 RID: 2241
		internal const string toStringHome = "toStringHome";

		// Token: 0x040008C2 RID: 2242
		internal const string toStringInsert = "toStringInsert";

		// Token: 0x040008C3 RID: 2243
		internal const string toStringNone = "toStringNone";

		// Token: 0x040008C4 RID: 2244
		internal const string toStringPageDown = "toStringPageDown";

		// Token: 0x040008C5 RID: 2245
		internal const string toStringPageUp = "toStringPageUp";

		// Token: 0x040008C6 RID: 2246
		internal const string toStringShift = "toStringShift";

		// Token: 0x040008C7 RID: 2247
		internal const string TrackBarAutoSizeDescr = "TrackBarAutoSizeDescr";

		// Token: 0x040008C8 RID: 2248
		internal const string TrackBarLargeChangeDescr = "TrackBarLargeChangeDescr";

		// Token: 0x040008C9 RID: 2249
		internal const string TrackBarLargeChangeError = "TrackBarLargeChangeError";

		// Token: 0x040008CA RID: 2250
		internal const string TrackBarMaximumDescr = "TrackBarMaximumDescr";

		// Token: 0x040008CB RID: 2251
		internal const string TrackBarMinimumDescr = "TrackBarMinimumDescr";

		// Token: 0x040008CC RID: 2252
		internal const string TrackBarOnScrollDescr = "TrackBarOnScrollDescr";

		// Token: 0x040008CD RID: 2253
		internal const string TrackBarOrientationDescr = "TrackBarOrientationDescr";

		// Token: 0x040008CE RID: 2254
		internal const string TrackBarSmallChangeDescr = "TrackBarSmallChangeDescr";

		// Token: 0x040008CF RID: 2255
		internal const string TrackBarSmallChangeError = "TrackBarSmallChangeError";

		// Token: 0x040008D0 RID: 2256
		internal const string TrackBarTickFrequencyDescr = "TrackBarTickFrequencyDescr";

		// Token: 0x040008D1 RID: 2257
		internal const string TrackBarTickStyleDescr = "TrackBarTickStyleDescr";

		// Token: 0x040008D2 RID: 2258
		internal const string TrackBarValueDescr = "TrackBarValueDescr";

		// Token: 0x040008D3 RID: 2259
		internal const string TransparentBackColorNotAllowed = "TransparentBackColorNotAllowed";

		// Token: 0x040008D4 RID: 2260
		internal const string TrayIcon_TextTooLong = "TrayIcon_TextTooLong";

		// Token: 0x040008D5 RID: 2261
		internal const string TreeNodeBackColorDescr = "TreeNodeBackColorDescr";

		// Token: 0x040008D6 RID: 2262
		internal const string TreeNodeBeginEditFailed = "TreeNodeBeginEditFailed";

		// Token: 0x040008D7 RID: 2263
		internal const string TreeNodeCheckedDescr = "TreeNodeCheckedDescr";

		// Token: 0x040008D8 RID: 2264
		internal const string TreeNodeCollectionBadTreeNode = "TreeNodeCollectionBadTreeNode";

		// Token: 0x040008D9 RID: 2265
		internal const string TreeNodeForeColorDescr = "TreeNodeForeColorDescr";

		// Token: 0x040008DA RID: 2266
		internal const string TreeNodeImageIndexDescr = "TreeNodeImageIndexDescr";

		// Token: 0x040008DB RID: 2267
		internal const string TreeNodeImageKeyDescr = "TreeNodeImageKeyDescr";

		// Token: 0x040008DC RID: 2268
		internal const string TreeNodeIndexDescr = "TreeNodeIndexDescr";

		// Token: 0x040008DD RID: 2269
		internal const string TreeNodeNodeFontDescr = "TreeNodeNodeFontDescr";

		// Token: 0x040008DE RID: 2270
		internal const string TreeNodeNodeNameDescr = "TreeNodeNodeNameDescr";

		// Token: 0x040008DF RID: 2271
		internal const string TreeNodeNoParent = "TreeNodeNoParent";

		// Token: 0x040008E0 RID: 2272
		internal const string TreeNodeSelectedImageIndexDescr = "TreeNodeSelectedImageIndexDescr";

		// Token: 0x040008E1 RID: 2273
		internal const string TreeNodeSelectedImageKeyDescr = "TreeNodeSelectedImageKeyDescr";

		// Token: 0x040008E2 RID: 2274
		internal const string TreeNodeStateImageIndexDescr = "TreeNodeStateImageIndexDescr";

		// Token: 0x040008E3 RID: 2275
		internal const string TreeNodeStateImageKeyDescr = "TreeNodeStateImageKeyDescr";

		// Token: 0x040008E4 RID: 2276
		internal const string TreeNodeTextDescr = "TreeNodeTextDescr";

		// Token: 0x040008E5 RID: 2277
		internal const string TreeNodeToolTipTextDescr = "TreeNodeToolTipTextDescr";

		// Token: 0x040008E6 RID: 2278
		internal const string TreeViewAfterCheckDescr = "TreeViewAfterCheckDescr";

		// Token: 0x040008E7 RID: 2279
		internal const string TreeViewAfterCollapseDescr = "TreeViewAfterCollapseDescr";

		// Token: 0x040008E8 RID: 2280
		internal const string TreeViewAfterEditDescr = "TreeViewAfterEditDescr";

		// Token: 0x040008E9 RID: 2281
		internal const string TreeViewAfterExpandDescr = "TreeViewAfterExpandDescr";

		// Token: 0x040008EA RID: 2282
		internal const string TreeViewAfterSelectDescr = "TreeViewAfterSelectDescr";

		// Token: 0x040008EB RID: 2283
		internal const string TreeViewBeforeCheckDescr = "TreeViewBeforeCheckDescr";

		// Token: 0x040008EC RID: 2284
		internal const string TreeViewBeforeCollapseDescr = "TreeViewBeforeCollapseDescr";

		// Token: 0x040008ED RID: 2285
		internal const string TreeViewBeforeEditDescr = "TreeViewBeforeEditDescr";

		// Token: 0x040008EE RID: 2286
		internal const string TreeViewBeforeExpandDescr = "TreeViewBeforeExpandDescr";

		// Token: 0x040008EF RID: 2287
		internal const string TreeViewBeforeSelectDescr = "TreeViewBeforeSelectDescr";

		// Token: 0x040008F0 RID: 2288
		internal const string TreeViewCheckBoxesDescr = "TreeViewCheckBoxesDescr";

		// Token: 0x040008F1 RID: 2289
		internal const string TreeViewDrawModeDescr = "TreeViewDrawModeDescr";

		// Token: 0x040008F2 RID: 2290
		internal const string TreeViewDrawNodeEventDescr = "TreeViewDrawNodeEventDescr";

		// Token: 0x040008F3 RID: 2291
		internal const string TreeViewFullRowSelectDescr = "TreeViewFullRowSelectDescr";

		// Token: 0x040008F4 RID: 2292
		internal const string TreeViewHideSelectionDescr = "TreeViewHideSelectionDescr";

		// Token: 0x040008F5 RID: 2293
		internal const string TreeViewHotTrackingDescr = "TreeViewHotTrackingDescr";

		// Token: 0x040008F6 RID: 2294
		internal const string TreeViewImageIndexDescr = "TreeViewImageIndexDescr";

		// Token: 0x040008F7 RID: 2295
		internal const string TreeViewImageKeyDescr = "TreeViewImageKeyDescr";

		// Token: 0x040008F8 RID: 2296
		internal const string TreeViewImageListDescr = "TreeViewImageListDescr";

		// Token: 0x040008F9 RID: 2297
		internal const string TreeViewIndentDescr = "TreeViewIndentDescr";

		// Token: 0x040008FA RID: 2298
		internal const string TreeViewItemHeightDescr = "TreeViewItemHeightDescr";

		// Token: 0x040008FB RID: 2299
		internal const string TreeViewLabelEditDescr = "TreeViewLabelEditDescr";

		// Token: 0x040008FC RID: 2300
		internal const string TreeViewLineColorDescr = "TreeViewLineColorDescr";

		// Token: 0x040008FD RID: 2301
		internal const string TreeViewNodeMouseClickDescr = "TreeViewNodeMouseClickDescr";

		// Token: 0x040008FE RID: 2302
		internal const string TreeViewNodeMouseDoubleClickDescr = "TreeViewNodeMouseDoubleClickDescr";

		// Token: 0x040008FF RID: 2303
		internal const string TreeViewNodeMouseHoverDescr = "TreeViewNodeMouseHoverDescr";

		// Token: 0x04000900 RID: 2304
		internal const string TreeViewNodesDescr = "TreeViewNodesDescr";

		// Token: 0x04000901 RID: 2305
		internal const string TreeViewNodeSorterDescr = "TreeViewNodeSorterDescr";

		// Token: 0x04000902 RID: 2306
		internal const string TreeViewPathSeparatorDescr = "TreeViewPathSeparatorDescr";

		// Token: 0x04000903 RID: 2307
		internal const string TreeViewScrollableDescr = "TreeViewScrollableDescr";

		// Token: 0x04000904 RID: 2308
		internal const string TreeViewSelectedImageIndexDescr = "TreeViewSelectedImageIndexDescr";

		// Token: 0x04000905 RID: 2309
		internal const string TreeViewSelectedImageKeyDescr = "TreeViewSelectedImageKeyDescr";

		// Token: 0x04000906 RID: 2310
		internal const string TreeViewSelectedNodeDescr = "TreeViewSelectedNodeDescr";

		// Token: 0x04000907 RID: 2311
		internal const string TreeViewShowLinesDescr = "TreeViewShowLinesDescr";

		// Token: 0x04000908 RID: 2312
		internal const string TreeViewShowPlusMinusDescr = "TreeViewShowPlusMinusDescr";

		// Token: 0x04000909 RID: 2313
		internal const string TreeViewShowRootLinesDescr = "TreeViewShowRootLinesDescr";

		// Token: 0x0400090A RID: 2314
		internal const string TreeViewShowShowNodeToolTipsDescr = "TreeViewShowShowNodeToolTipsDescr";

		// Token: 0x0400090B RID: 2315
		internal const string TreeViewSortedDescr = "TreeViewSortedDescr";

		// Token: 0x0400090C RID: 2316
		internal const string TreeViewStateImageListDescr = "TreeViewStateImageListDescr";

		// Token: 0x0400090D RID: 2317
		internal const string TreeViewTopNodeDescr = "TreeViewTopNodeDescr";

		// Token: 0x0400090E RID: 2318
		internal const string TreeViewVisibleCountDescr = "TreeViewVisibleCountDescr";

		// Token: 0x0400090F RID: 2319
		internal const string TrustManager_WarningIconAccessibleDescription_HighRisk = "TrustManager_WarningIconAccessibleDescription_HighRisk";

		// Token: 0x04000910 RID: 2320
		internal const string TrustManager_WarningIconAccessibleDescription_LowRisk = "TrustManager_WarningIconAccessibleDescription_LowRisk";

		// Token: 0x04000911 RID: 2321
		internal const string TrustManager_WarningIconAccessibleDescription_MediumRisk = "TrustManager_WarningIconAccessibleDescription_MediumRisk";

		// Token: 0x04000912 RID: 2322
		internal const string TrustManagerBadXml = "TrustManagerBadXml";

		// Token: 0x04000913 RID: 2323
		internal const string TrustManagerMoreInfo_InstallTitle = "TrustManagerMoreInfo_InstallTitle";

		// Token: 0x04000914 RID: 2324
		internal const string TrustManagerMoreInfo_InternetSource = "TrustManagerMoreInfo_InternetSource";

		// Token: 0x04000915 RID: 2325
		internal const string TrustManagerMoreInfo_KnownPublisher = "TrustManagerMoreInfo_KnownPublisher";

		// Token: 0x04000916 RID: 2326
		internal const string TrustManagerMoreInfo_LocalComputerSource = "TrustManagerMoreInfo_LocalComputerSource";

		// Token: 0x04000917 RID: 2327
		internal const string TrustManagerMoreInfo_LocalNetworkSource = "TrustManagerMoreInfo_LocalNetworkSource";

		// Token: 0x04000918 RID: 2328
		internal const string TrustManagerMoreInfo_Location = "TrustManagerMoreInfo_Location";

		// Token: 0x04000919 RID: 2329
		internal const string TrustManagerMoreInfo_RunTitle = "TrustManagerMoreInfo_RunTitle";

		// Token: 0x0400091A RID: 2330
		internal const string TrustManagerMoreInfo_SafeAccess = "TrustManagerMoreInfo_SafeAccess";

		// Token: 0x0400091B RID: 2331
		internal const string TrustManagerMoreInfo_UnknownPublisher = "TrustManagerMoreInfo_UnknownPublisher";

		// Token: 0x0400091C RID: 2332
		internal const string TrustManagerMoreInfo_UnsafeAccess = "TrustManagerMoreInfo_UnsafeAccess";

		// Token: 0x0400091D RID: 2333
		internal const string TrustManagerMoreInfo_UntrustedSitesSource = "TrustManagerMoreInfo_UntrustedSitesSource";

		// Token: 0x0400091E RID: 2334
		internal const string TrustManagerMoreInfo_TrustedSitesSource = "TrustManagerMoreInfo_TrustedSitesSource";

		// Token: 0x0400091F RID: 2335
		internal const string TrustManagerMoreInfo_WithoutShortcut = "TrustManagerMoreInfo_WithoutShortcut";

		// Token: 0x04000920 RID: 2336
		internal const string TrustManagerMoreInfo_WithShortcut = "TrustManagerMoreInfo_WithShortcut";

		// Token: 0x04000921 RID: 2337
		internal const string TrustManagerPromptUI_AccessibleDescription_InstallBlocked = "TrustManagerPromptUI_AccessibleDescription_InstallBlocked";

		// Token: 0x04000922 RID: 2338
		internal const string TrustManagerPromptUI_AccessibleDescription_InstallConfirmation = "TrustManagerPromptUI_AccessibleDescription_InstallConfirmation";

		// Token: 0x04000923 RID: 2339
		internal const string TrustManagerPromptUI_AccessibleDescription_InstallWithElevatedPermissions = "TrustManagerPromptUI_AccessibleDescription_InstallWithElevatedPermissions";

		// Token: 0x04000924 RID: 2340
		internal const string TrustManagerPromptUI_AccessibleDescription_RunBlocked = "TrustManagerPromptUI_AccessibleDescription_RunBlocked";

		// Token: 0x04000925 RID: 2341
		internal const string TrustManagerPromptUI_AccessibleDescription_RunConfirmation = "TrustManagerPromptUI_AccessibleDescription_RunConfirmation";

		// Token: 0x04000926 RID: 2342
		internal const string TrustManagerPromptUI_AccessibleDescription_RunWithElevatedPermissions = "TrustManagerPromptUI_AccessibleDescription_RunWithElevatedPermissions";

		// Token: 0x04000927 RID: 2343
		internal const string TrustManagerPromptUI_BlockedApp = "TrustManagerPromptUI_BlockedApp";

		// Token: 0x04000928 RID: 2344
		internal const string TrustManagerPromptUI_InstalledAppBlockedWarning = "TrustManagerPromptUI_InstalledAppBlockedWarning";

		// Token: 0x04000929 RID: 2345
		internal const string TrustManagerPromptUI_InstallFromLocalMachineWarning = "TrustManagerPromptUI_InstallFromLocalMachineWarning";

		// Token: 0x0400092A RID: 2346
		internal const string TrustManagerPromptUI_InstallQuestion = "TrustManagerPromptUI_InstallQuestion";

		// Token: 0x0400092B RID: 2347
		internal const string TrustManagerPromptUI_InstallTitle = "TrustManagerPromptUI_InstallTitle";

		// Token: 0x0400092C RID: 2348
		internal const string TrustManagerPromptUI_InstallWarning = "TrustManagerPromptUI_InstallWarning";

		// Token: 0x0400092D RID: 2349
		internal const string TrustManagerPromptUI_MoreInformation = "TrustManagerPromptUI_MoreInformation";

		// Token: 0x0400092E RID: 2350
		internal const string TrustManagerPromptUI_MoreInformationAccessibleDescription = "TrustManagerPromptUI_MoreInformationAccessibleDescription";

		// Token: 0x0400092F RID: 2351
		internal const string TrustManagerPromptUI_MoreInformationAccessibleName = "TrustManagerPromptUI_MoreInformationAccessibleName";

		// Token: 0x04000930 RID: 2352
		internal const string TrustManagerPromptUI_NoPublisherInstallQuestion = "TrustManagerPromptUI_NoPublisherInstallQuestion";

		// Token: 0x04000931 RID: 2353
		internal const string TrustManagerPromptUI_NoPublisherRunQuestion = "TrustManagerPromptUI_NoPublisherRunQuestion";

		// Token: 0x04000932 RID: 2354
		internal const string TrustManagerPromptUI_Close = "TrustManagerPromptUI_Close";

		// Token: 0x04000933 RID: 2355
		internal const string TrustManagerPromptUI_DoNotInstall = "TrustManagerPromptUI_DoNotInstall";

		// Token: 0x04000934 RID: 2356
		internal const string TrustManagerPromptUI_DoNotRun = "TrustManagerPromptUI_DoNotRun";

		// Token: 0x04000935 RID: 2357
		internal const string TrustManagerPromptUI_Run = "TrustManagerPromptUI_Run";

		// Token: 0x04000936 RID: 2358
		internal const string TrustManagerPromptUI_RunAppBlockedWarning = "TrustManagerPromptUI_RunAppBlockedWarning";

		// Token: 0x04000937 RID: 2359
		internal const string TrustManagerPromptUI_RunFromLocalMachineWarning = "TrustManagerPromptUI_RunFromLocalMachineWarning";

		// Token: 0x04000938 RID: 2360
		internal const string TrustManagerPromptUI_RunQuestion = "TrustManagerPromptUI_RunQuestion";

		// Token: 0x04000939 RID: 2361
		internal const string TrustManagerPromptUI_RunTitle = "TrustManagerPromptUI_RunTitle";

		// Token: 0x0400093A RID: 2362
		internal const string TrustManagerPromptUI_RunWarning = "TrustManagerPromptUI_RunWarning";

		// Token: 0x0400093B RID: 2363
		internal const string TrustManagerPromptUI_UnknownPublisher = "TrustManagerPromptUI_UnknownPublisher";

		// Token: 0x0400093C RID: 2364
		internal const string TrustManagerPromptUI_WarningAccessibleDescription = "TrustManagerPromptUI_WarningAccessibleDescription";

		// Token: 0x0400093D RID: 2365
		internal const string TrustManagerPromptUI_WarningAccessibleName = "TrustManagerPromptUI_WarningAccessibleName";

		// Token: 0x0400093E RID: 2366
		internal const string TypedControlCollectionShouldBeOfType = "TypedControlCollectionShouldBeOfType";

		// Token: 0x0400093F RID: 2367
		internal const string TypedControlCollectionShouldBeOfTypes = "TypedControlCollectionShouldBeOfTypes";

		// Token: 0x04000940 RID: 2368
		internal const string TYPEINFOPROCESSORGetDocumentationFailed = "TYPEINFOPROCESSORGetDocumentationFailed";

		// Token: 0x04000941 RID: 2369
		internal const string TYPEINFOPROCESSORGetRefTypeInfoFailed = "TYPEINFOPROCESSORGetRefTypeInfoFailed";

		// Token: 0x04000942 RID: 2370
		internal const string TYPEINFOPROCESSORGetTypeAttrFailed = "TYPEINFOPROCESSORGetTypeAttrFailed";

		// Token: 0x04000943 RID: 2371
		internal const string TypeLoadException = "TypeLoadException";

		// Token: 0x04000944 RID: 2372
		internal const string TypeLoadExceptionShort = "TypeLoadExceptionShort";

		// Token: 0x04000945 RID: 2373
		internal const string UnableToInitComponent = "UnableToInitComponent";

		// Token: 0x04000946 RID: 2374
		internal const string UnableToSetPanelText = "UnableToSetPanelText";

		// Token: 0x04000947 RID: 2375
		internal const string UnknownAttr = "UnknownAttr";

		// Token: 0x04000948 RID: 2376
		internal const string UnknownInputLanguageLayout = "UnknownInputLanguageLayout";

		// Token: 0x04000949 RID: 2377
		internal const string UnsafeNativeMethodsNotImplemented = "UnsafeNativeMethodsNotImplemented";

		// Token: 0x0400094A RID: 2378
		internal const string UpDownBaseAlignmentDescr = "UpDownBaseAlignmentDescr";

		// Token: 0x0400094B RID: 2379
		internal const string UpDownBaseBorderStyleDescr = "UpDownBaseBorderStyleDescr";

		// Token: 0x0400094C RID: 2380
		internal const string UpDownBaseDownButtonAccName = "UpDownBaseDownButtonAccName";

		// Token: 0x0400094D RID: 2381
		internal const string UpDownBaseInterceptArrowKeysDescr = "UpDownBaseInterceptArrowKeysDescr";

		// Token: 0x0400094E RID: 2382
		internal const string UpDownBasePreferredHeightDescr = "UpDownBasePreferredHeightDescr";

		// Token: 0x0400094F RID: 2383
		internal const string UpDownBaseReadOnlyDescr = "UpDownBaseReadOnlyDescr";

		// Token: 0x04000950 RID: 2384
		internal const string UpDownBaseTextAlignDescr = "UpDownBaseTextAlignDescr";

		// Token: 0x04000951 RID: 2385
		internal const string UpDownBaseUpButtonAccName = "UpDownBaseUpButtonAccName";

		// Token: 0x04000952 RID: 2386
		internal const string UseCompatibleTextRenderingDescr = "UseCompatibleTextRenderingDescr";

		// Token: 0x04000953 RID: 2387
		internal const string UserControlBorderStyleDescr = "UserControlBorderStyleDescr";

		// Token: 0x04000954 RID: 2388
		internal const string UserControlOnLoadDescr = "UserControlOnLoadDescr";

		// Token: 0x04000955 RID: 2389
		internal const string valueChangedEventDescr = "valueChangedEventDescr";

		// Token: 0x04000956 RID: 2390
		internal const string VisualStyleHandleCreationFailed = "VisualStyleHandleCreationFailed";

		// Token: 0x04000957 RID: 2391
		internal const string VisualStyleNotActive = "VisualStyleNotActive";

		// Token: 0x04000958 RID: 2392
		internal const string VisualStylesDisabledInClientArea = "VisualStylesDisabledInClientArea";

		// Token: 0x04000959 RID: 2393
		internal const string VisualStylesInvalidCombination = "VisualStylesInvalidCombination";

		// Token: 0x0400095A RID: 2394
		internal const string WebBrowserAllowDropNotSupported = "WebBrowserAllowDropNotSupported";

		// Token: 0x0400095B RID: 2395
		internal const string WebBrowserAllowNavigationDescr = "WebBrowserAllowNavigationDescr";

		// Token: 0x0400095C RID: 2396
		internal const string WebBrowserAllowWebBrowserDropDescr = "WebBrowserAllowWebBrowserDropDescr";

		// Token: 0x0400095D RID: 2397
		internal const string WebBrowserBackgroundImageLayoutNotSupported = "WebBrowserBackgroundImageLayoutNotSupported";

		// Token: 0x0400095E RID: 2398
		internal const string WebBrowserBackgroundImageNotSupported = "WebBrowserBackgroundImageNotSupported";

		// Token: 0x0400095F RID: 2399
		internal const string WebBrowserCanGoBackChangedDescr = "WebBrowserCanGoBackChangedDescr";

		// Token: 0x04000960 RID: 2400
		internal const string WebBrowserCanGoForwardChangedDescr = "WebBrowserCanGoForwardChangedDescr";

		// Token: 0x04000961 RID: 2401
		internal const string WebBrowserCursorNotSupported = "WebBrowserCursorNotSupported";

		// Token: 0x04000962 RID: 2402
		internal const string WebBrowserDocumentCompletedDescr = "WebBrowserDocumentCompletedDescr";

		// Token: 0x04000963 RID: 2403
		internal const string WebBrowserDocumentTitleChangedDescr = "WebBrowserDocumentTitleChangedDescr";

		// Token: 0x04000964 RID: 2404
		internal const string WebBrowserEnabledNotSupported = "WebBrowserEnabledNotSupported";

		// Token: 0x04000965 RID: 2405
		internal const string WebBrowserEncryptionLevelChangedDescr = "WebBrowserEncryptionLevelChangedDescr";

		// Token: 0x04000966 RID: 2406
		internal const string WebBrowserFileDownloadDescr = "WebBrowserFileDownloadDescr";

		// Token: 0x04000967 RID: 2407
		internal const string WebBrowserInIENotSupported = "WebBrowserInIENotSupported";

		// Token: 0x04000968 RID: 2408
		internal const string WebBrowserIsOfflineDescr = "WebBrowserIsOfflineDescr";

		// Token: 0x04000969 RID: 2409
		internal const string WebBrowserIsWebBrowserContextMenuEnabledDescr = "WebBrowserIsWebBrowserContextMenuEnabledDescr";

		// Token: 0x0400096A RID: 2410
		internal const string WebBrowserNavigateAbsoluteUri = "WebBrowserNavigateAbsoluteUri";

		// Token: 0x0400096B RID: 2411
		internal const string WebBrowserNavigatedDescr = "WebBrowserNavigatedDescr";

		// Token: 0x0400096C RID: 2412
		internal const string WebBrowserNavigatingDescr = "WebBrowserNavigatingDescr";

		// Token: 0x0400096D RID: 2413
		internal const string WebBrowserNewWindowDescr = "WebBrowserNewWindowDescr";

		// Token: 0x0400096E RID: 2414
		internal const string WebBrowserNoCastToIWebBrowser2 = "WebBrowserNoCastToIWebBrowser2";

		// Token: 0x0400096F RID: 2415
		internal const string WebBrowserObjectForScriptingComVisibleOnly = "WebBrowserObjectForScriptingComVisibleOnly";

		// Token: 0x04000970 RID: 2416
		internal const string WebBrowserProgressChangedDescr = "WebBrowserProgressChangedDescr";

		// Token: 0x04000971 RID: 2417
		internal const string WebBrowserRightToLeftNotSupported = "WebBrowserRightToLeftNotSupported";

		// Token: 0x04000972 RID: 2418
		internal const string WebBrowserScriptErrorsSuppressedDescr = "WebBrowserScriptErrorsSuppressedDescr";

		// Token: 0x04000973 RID: 2419
		internal const string WebBrowserScrollBarsEnabledDescr = "WebBrowserScrollBarsEnabledDescr";

		// Token: 0x04000974 RID: 2420
		internal const string WebBrowserSecurityLevelDescr = "WebBrowserSecurityLevelDescr";

		// Token: 0x04000975 RID: 2421
		internal const string WebBrowserStatusTextChangedDescr = "WebBrowserStatusTextChangedDescr";

		// Token: 0x04000976 RID: 2422
		internal const string WebBrowserTextNotSupported = "WebBrowserTextNotSupported";

		// Token: 0x04000977 RID: 2423
		internal const string WebBrowserUrlDescr = "WebBrowserUrlDescr";

		// Token: 0x04000978 RID: 2424
		internal const string WebBrowserUseWaitCursorNotSupported = "WebBrowserUseWaitCursorNotSupported";

		// Token: 0x04000979 RID: 2425
		internal const string WebBrowserWebBrowserShortcutsEnabledDescr = "WebBrowserWebBrowserShortcutsEnabledDescr";

		// Token: 0x0400097A RID: 2426
		internal const string WidthGreaterThanMinWidth = "WidthGreaterThanMinWidth";

		// Token: 0x0400097B RID: 2427
		internal const string Win32WindowAlreadyCreated = "Win32WindowAlreadyCreated";

		// Token: 0x0400097C RID: 2428
		internal const string WindowsFormsSetEvent = "WindowsFormsSetEvent";

		// Token: 0x0400097D RID: 2429
		private static SR loader;

		// Token: 0x0400097E RID: 2430
		private ResourceManager resources;

		// Token: 0x0400097F RID: 2431
		private static object s_InternalSyncObject;
	}
}
