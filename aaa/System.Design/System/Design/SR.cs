using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Design
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
			this.resources = new ResourceManager("System.Design", base.GetType().Assembly);
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

		// Token: 0x04000036 RID: 54
		internal const string VerbEditorDefault = "VerbEditorDefault";

		// Token: 0x04000037 RID: 55
		internal const string WorkingDirectoryEditorLabel = "WorkingDirectoryEditorLabel";

		// Token: 0x04000038 RID: 56
		internal const string FSWPathEditorLabel = "FSWPathEditorLabel";

		// Token: 0x04000039 RID: 57
		internal const string BinaryEditorFileError = "BinaryEditorFileError";

		// Token: 0x0400003A RID: 58
		internal const string BinaryEditorTitle = "BinaryEditorTitle";

		// Token: 0x0400003B RID: 59
		internal const string BinaryEditorAllFiles = "BinaryEditorAllFiles";

		// Token: 0x0400003C RID: 60
		internal const string BinaryEditorSaveFile = "BinaryEditorSaveFile";

		// Token: 0x0400003D RID: 61
		internal const string BinaryEditorFileName = "BinaryEditorFileName";

		// Token: 0x0400003E RID: 62
		internal const string AnchorEditorAccName = "AnchorEditorAccName";

		// Token: 0x0400003F RID: 63
		internal const string AnchorEditorRightAccName = "AnchorEditorRightAccName";

		// Token: 0x04000040 RID: 64
		internal const string AnchorEditorLeftAccName = "AnchorEditorLeftAccName";

		// Token: 0x04000041 RID: 65
		internal const string AnchorEditorTopAccName = "AnchorEditorTopAccName";

		// Token: 0x04000042 RID: 66
		internal const string AnchorEditorBottomAccName = "AnchorEditorBottomAccName";

		// Token: 0x04000043 RID: 67
		internal const string CollectionEditorCaption = "CollectionEditorCaption";

		// Token: 0x04000044 RID: 68
		internal const string CollectionEditorProperties = "CollectionEditorProperties";

		// Token: 0x04000045 RID: 69
		internal const string CollectionEditorPropertiesMultiSelect = "CollectionEditorPropertiesMultiSelect";

		// Token: 0x04000046 RID: 70
		internal const string CollectionEditorPropertiesNone = "CollectionEditorPropertiesNone";

		// Token: 0x04000047 RID: 71
		internal const string CollectionEditorCantRemoveItem = "CollectionEditorCantRemoveItem";

		// Token: 0x04000048 RID: 72
		internal const string CollectionEditorUndoBatchDesc = "CollectionEditorUndoBatchDesc";

		// Token: 0x04000049 RID: 73
		internal const string CollectionEditorInheritedReadOnlySelection = "CollectionEditorInheritedReadOnlySelection";

		// Token: 0x0400004A RID: 74
		internal const string DockEditorAccName = "DockEditorAccName";

		// Token: 0x0400004B RID: 75
		internal const string DockEditorNoneAccName = "DockEditorNoneAccName";

		// Token: 0x0400004C RID: 76
		internal const string DockEditorRightAccName = "DockEditorRightAccName";

		// Token: 0x0400004D RID: 77
		internal const string DockEditorLeftAccName = "DockEditorLeftAccName";

		// Token: 0x0400004E RID: 78
		internal const string DockEditorTopAccName = "DockEditorTopAccName";

		// Token: 0x0400004F RID: 79
		internal const string DockEditorBottomAccName = "DockEditorBottomAccName";

		// Token: 0x04000050 RID: 80
		internal const string DockEditorFillAccName = "DockEditorFillAccName";

		// Token: 0x04000051 RID: 81
		internal const string DesignSurfaceNoRootComponent = "DesignSurfaceNoRootComponent";

		// Token: 0x04000052 RID: 82
		internal const string DesignSurfaceServiceIsFixed = "DesignSurfaceServiceIsFixed";

		// Token: 0x04000053 RID: 83
		internal const string DesignSurfaceFatalError = "DesignSurfaceFatalError";

		// Token: 0x04000054 RID: 84
		internal const string DesignSurfaceContainerDispose = "DesignSurfaceContainerDispose";

		// Token: 0x04000055 RID: 85
		internal const string DesignSurfaceDesignerNotLoaded = "DesignSurfaceDesignerNotLoaded";

		// Token: 0x04000056 RID: 86
		internal const string DesignSurfaceNoSupportedTechnology = "DesignSurfaceNoSupportedTechnology";

		// Token: 0x04000057 RID: 87
		internal const string DesignerHostUnloading = "DesignerHostUnloading";

		// Token: 0x04000058 RID: 88
		internal const string DesignerHostCyclicAdd = "DesignerHostCyclicAdd";

		// Token: 0x04000059 RID: 89
		internal const string DesignerHostNoTopLevelDesigner = "DesignerHostNoTopLevelDesigner";

		// Token: 0x0400005A RID: 90
		internal const string DesignerHostDuplicateName = "DesignerHostDuplicateName";

		// Token: 0x0400005B RID: 91
		internal const string DesignerHostFailedComponentCreate = "DesignerHostFailedComponentCreate";

		// Token: 0x0400005C RID: 92
		internal const string DesignerHostCantDestroyInheritedComponent = "DesignerHostCantDestroyInheritedComponent";

		// Token: 0x0400005D RID: 93
		internal const string DesignerHostDestroyComponentTransaction = "DesignerHostDestroyComponentTransaction";

		// Token: 0x0400005E RID: 94
		internal const string DesignerHostNoBaseClass = "DesignerHostNoBaseClass";

		// Token: 0x0400005F RID: 95
		internal const string DesignerHostLoaderSpecified = "DesignerHostLoaderSpecified";

		// Token: 0x04000060 RID: 96
		internal const string DesignerHostNestedTransaction = "DesignerHostNestedTransaction";

		// Token: 0x04000061 RID: 97
		internal const string DesignerHostGenericTransactionName = "DesignerHostGenericTransactionName";

		// Token: 0x04000062 RID: 98
		internal const string DesignerHostDesignerNeedsComponent = "DesignerHostDesignerNeedsComponent";

		// Token: 0x04000063 RID: 99
		internal const string DesignerOptionsMissingServiceContainer = "DesignerOptionsMissingServiceContainer";

		// Token: 0x04000064 RID: 100
		internal const string DesignerOptionsExistingOptionsService = "DesignerOptionsExistingOptionsService";

		// Token: 0x04000065 RID: 101
		internal const string DesignerOptionsUnableToCreateOptionService = "DesignerOptionsUnableToCreateOptionService";

		// Token: 0x04000066 RID: 102
		internal const string BasicDesignerLoaderAlreadyLoaded = "BasicDesignerLoaderAlreadyLoaded";

		// Token: 0x04000067 RID: 103
		internal const string BasicDesignerLoaderDifferentHost = "BasicDesignerLoaderDifferentHost";

		// Token: 0x04000068 RID: 104
		internal const string BasicDesignerLoaderMissingService = "BasicDesignerLoaderMissingService";

		// Token: 0x04000069 RID: 105
		internal const string BasicDesignerLoaderNotInitialized = "BasicDesignerLoaderNotInitialized";

		// Token: 0x0400006A RID: 106
		internal const string CodeDomDesignerLoaderNoLanguageSupport = "CodeDomDesignerLoaderNoLanguageSupport";

		// Token: 0x0400006B RID: 107
		internal const string CodeDomDesignerLoaderDocumentFailureTypeNotFound = "CodeDomDesignerLoaderDocumentFailureTypeNotFound";

		// Token: 0x0400006C RID: 108
		internal const string CodeDomDesignerLoaderDocumentFailureTypeNotDesignable = "CodeDomDesignerLoaderDocumentFailureTypeNotDesignable";

		// Token: 0x0400006D RID: 109
		internal const string CodeDomDesignerLoaderDocumentFailureTypeDesignerNotInstalled = "CodeDomDesignerLoaderDocumentFailureTypeDesignerNotInstalled";

		// Token: 0x0400006E RID: 110
		internal const string CodeDomDesignerLoaderNoRootSerializer = "CodeDomDesignerLoaderNoRootSerializer";

		// Token: 0x0400006F RID: 111
		internal const string CodeDomDesignerLoaderNoRootSerializerWithFailures = "CodeDomDesignerLoaderNoRootSerializerWithFailures";

		// Token: 0x04000070 RID: 112
		internal const string CodeDomDesignerLoaderInvalidIdentifier = "CodeDomDesignerLoaderInvalidIdentifier";

		// Token: 0x04000071 RID: 113
		internal const string CodeDomDesignerLoaderInvalidBlankIdentifier = "CodeDomDesignerLoaderInvalidBlankIdentifier";

		// Token: 0x04000072 RID: 114
		internal const string CodeDomDesignerLoaderDupComponentName = "CodeDomDesignerLoaderDupComponentName";

		// Token: 0x04000073 RID: 115
		internal const string CodeDomDesignerLoaderBadSerializationObject = "CodeDomDesignerLoaderBadSerializationObject";

		// Token: 0x04000074 RID: 116
		internal const string CodeDomDesignerLoaderPropModifiers = "CodeDomDesignerLoaderPropModifiers";

		// Token: 0x04000075 RID: 117
		internal const string CodeDomDesignerLoaderPropGenerateMember = "CodeDomDesignerLoaderPropGenerateMember";

		// Token: 0x04000076 RID: 118
		internal const string CodeDomDesignerLoaderNoTypeResolution = "CodeDomDesignerLoaderNoTypeResolution";

		// Token: 0x04000077 RID: 119
		internal const string CodeDomDesignerLoaderSerializerTypeNotFirstType = "CodeDomDesignerLoaderSerializerTypeNotFirstType";

		// Token: 0x04000078 RID: 120
		internal const string CodeDomComponentSerializationServiceUnknownStore = "CodeDomComponentSerializationServiceUnknownStore";

		// Token: 0x04000079 RID: 121
		internal const string CodeDomComponentSerializationServiceClosedStore = "CodeDomComponentSerializationServiceClosedStore";

		// Token: 0x0400007A RID: 122
		internal const string CodeDomComponentSerializationServiceDeserializationError = "CodeDomComponentSerializationServiceDeserializationError";

		// Token: 0x0400007B RID: 123
		internal const string DesignerActionPanel_CouldNotFindProperty = "DesignerActionPanel_CouldNotFindProperty";

		// Token: 0x0400007C RID: 124
		internal const string DesignerActionPanel_CouldNotFindMethod = "DesignerActionPanel_CouldNotFindMethod";

		// Token: 0x0400007D RID: 125
		internal const string DesignerActionPanel_CouldNotConvertValue = "DesignerActionPanel_CouldNotConvertValue";

		// Token: 0x0400007E RID: 126
		internal const string DesignerActionPanel_ErrorActivatingDropDown = "DesignerActionPanel_ErrorActivatingDropDown";

		// Token: 0x0400007F RID: 127
		internal const string DesignerActionPanel_ErrorSettingValue = "DesignerActionPanel_ErrorSettingValue";

		// Token: 0x04000080 RID: 128
		internal const string DesignerActionPanel_ErrorInvokingAction = "DesignerActionPanel_ErrorInvokingAction";

		// Token: 0x04000081 RID: 129
		internal const string DesignerActionPanel_DefaultPanelTitle = "DesignerActionPanel_DefaultPanelTitle";

		// Token: 0x04000082 RID: 130
		internal const string ExtenderProviderServiceDuplicateProvider = "ExtenderProviderServiceDuplicateProvider";

		// Token: 0x04000083 RID: 131
		internal const string EventBindingServiceMissingService = "EventBindingServiceMissingService";

		// Token: 0x04000084 RID: 132
		internal const string EventBindingServiceEventReadOnly = "EventBindingServiceEventReadOnly";

		// Token: 0x04000085 RID: 133
		internal const string EventBindingServiceBadArgType = "EventBindingServiceBadArgType";

		// Token: 0x04000086 RID: 134
		internal const string EventBindingServiceNoSite = "EventBindingServiceNoSite";

		// Token: 0x04000087 RID: 135
		internal const string EventBindingServiceSetValue = "EventBindingServiceSetValue";

		// Token: 0x04000088 RID: 136
		internal const string SerializationManagerDuplicateComponentDecl = "SerializationManagerDuplicateComponentDecl";

		// Token: 0x04000089 RID: 137
		internal const string SerializationManagerNoMatchingCtor = "SerializationManagerNoMatchingCtor";

		// Token: 0x0400008A RID: 138
		internal const string SerializationManagerNameInUse = "SerializationManagerNameInUse";

		// Token: 0x0400008B RID: 139
		internal const string SerializationManagerObjectHasName = "SerializationManagerObjectHasName";

		// Token: 0x0400008C RID: 140
		internal const string SerializationManagerAreadyInSession = "SerializationManagerAreadyInSession";

		// Token: 0x0400008D RID: 141
		internal const string SerializationManagerNoSession = "SerializationManagerNoSession";

		// Token: 0x0400008E RID: 142
		internal const string SerializationManagerWithinSession = "SerializationManagerWithinSession";

		// Token: 0x0400008F RID: 143
		internal const string UndoEngineMissingService = "UndoEngineMissingService";

		// Token: 0x04000090 RID: 144
		internal const string UndoEngineComponentChange0 = "UndoEngineComponentChange0";

		// Token: 0x04000091 RID: 145
		internal const string UndoEngineComponentChange1 = "UndoEngineComponentChange1";

		// Token: 0x04000092 RID: 146
		internal const string UndoEngineComponentChange2 = "UndoEngineComponentChange2";

		// Token: 0x04000093 RID: 147
		internal const string UndoEngineComponentAdd0 = "UndoEngineComponentAdd0";

		// Token: 0x04000094 RID: 148
		internal const string UndoEngineComponentAdd1 = "UndoEngineComponentAdd1";

		// Token: 0x04000095 RID: 149
		internal const string UndoEngineComponentRemove0 = "UndoEngineComponentRemove0";

		// Token: 0x04000096 RID: 150
		internal const string UndoEngineComponentRemove1 = "UndoEngineComponentRemove1";

		// Token: 0x04000097 RID: 151
		internal const string UndoEngineComponentRename = "UndoEngineComponentRename";

		// Token: 0x04000098 RID: 152
		internal const string BehaviorServiceResizeControl = "BehaviorServiceResizeControl";

		// Token: 0x04000099 RID: 153
		internal const string BehaviorServiceResizeControls = "BehaviorServiceResizeControls";

		// Token: 0x0400009A RID: 154
		internal const string BehaviorServiceMoveControl = "BehaviorServiceMoveControl";

		// Token: 0x0400009B RID: 155
		internal const string BehaviorServiceMoveControls = "BehaviorServiceMoveControls";

		// Token: 0x0400009C RID: 156
		internal const string BehaviorServiceCopyControl = "BehaviorServiceCopyControl";

		// Token: 0x0400009D RID: 157
		internal const string BehaviorServiceCopyControls = "BehaviorServiceCopyControls";

		// Token: 0x0400009E RID: 158
		internal const string MultilineStringEditorWatermark = "MultilineStringEditorWatermark";

		// Token: 0x0400009F RID: 159
		internal const string ComponentDesignerAddEvent = "ComponentDesignerAddEvent";

		// Token: 0x040000A0 RID: 160
		internal const string LocalizerManualReload = "LocalizerManualReload";

		// Token: 0x040000A1 RID: 161
		internal const string LocalizingCannotAdd = "LocalizingCannotAdd";

		// Token: 0x040000A2 RID: 162
		internal const string LocalizeDesigner_RegionWatermark = "LocalizeDesigner_RegionWatermark";

		// Token: 0x040000A3 RID: 163
		internal const string LocalizationProviderLocalizableDescr = "LocalizationProviderLocalizableDescr";

		// Token: 0x040000A4 RID: 164
		internal const string LocalizationProviderLanguageDescr = "LocalizationProviderLanguageDescr";

		// Token: 0x040000A5 RID: 165
		internal const string LocalizationProviderManualReload = "LocalizationProviderManualReload";

		// Token: 0x040000A6 RID: 166
		internal const string LocalizationProviderMissingService = "LocalizationProviderMissingService";

		// Token: 0x040000A7 RID: 167
		internal const string IntegerCollectionEditorTitle = "IntegerCollectionEditorTitle";

		// Token: 0x040000A8 RID: 168
		internal const string InheritanceServiceReadOnlyCollection = "InheritanceServiceReadOnlyCollection";

		// Token: 0x040000A9 RID: 169
		internal const string CancelCaption = "CancelCaption";

		// Token: 0x040000AA RID: 170
		internal const string OKCaption = "OKCaption";

		// Token: 0x040000AB RID: 171
		internal const string HelpCaption = "HelpCaption";

		// Token: 0x040000AC RID: 172
		internal const string DataFieldCollectionEditorTitle = "DataFieldCollectionEditorTitle";

		// Token: 0x040000AD RID: 173
		internal const string DataFieldCollectionAvailableFields = "DataFieldCollectionAvailableFields";

		// Token: 0x040000AE RID: 174
		internal const string DataFieldCollectionSelectedFields = "DataFieldCollectionSelectedFields";

		// Token: 0x040000AF RID: 175
		internal const string DataFieldCollection_MoveUp = "DataFieldCollection_MoveUp";

		// Token: 0x040000B0 RID: 176
		internal const string DataFieldCollection_MoveUpDesc = "DataFieldCollection_MoveUpDesc";

		// Token: 0x040000B1 RID: 177
		internal const string DataFieldCollection_MoveDown = "DataFieldCollection_MoveDown";

		// Token: 0x040000B2 RID: 178
		internal const string DataFieldCollection_MoveDownDesc = "DataFieldCollection_MoveDownDesc";

		// Token: 0x040000B3 RID: 179
		internal const string DataFieldCollection_MoveLeft = "DataFieldCollection_MoveLeft";

		// Token: 0x040000B4 RID: 180
		internal const string DataFieldCollection_MoveLeftDesc = "DataFieldCollection_MoveLeftDesc";

		// Token: 0x040000B5 RID: 181
		internal const string DataFieldCollection_MoveRight = "DataFieldCollection_MoveRight";

		// Token: 0x040000B6 RID: 182
		internal const string DataFieldCollection_MoveRightDesc = "DataFieldCollection_MoveRightDesc";

		// Token: 0x040000B7 RID: 183
		internal const string SerializerBadElementType = "SerializerBadElementType";

		// Token: 0x040000B8 RID: 184
		internal const string SerializerBadElementTypes = "SerializerBadElementTypes";

		// Token: 0x040000B9 RID: 185
		internal const string SerializerMissingService = "SerializerMissingService";

		// Token: 0x040000BA RID: 186
		internal const string SerializerNoSerializerForComponent = "SerializerNoSerializerForComponent";

		// Token: 0x040000BB RID: 187
		internal const string SerializerLostStatements = "SerializerLostStatements";

		// Token: 0x040000BC RID: 188
		internal const string SerializerTypeNotFound = "SerializerTypeNotFound";

		// Token: 0x040000BD RID: 189
		internal const string SerializerTypeAbstract = "SerializerTypeAbstract";

		// Token: 0x040000BE RID: 190
		internal const string SerializerUndeclaredName = "SerializerUndeclaredName";

		// Token: 0x040000BF RID: 191
		internal const string SerializerNoSuchEvent = "SerializerNoSuchEvent";

		// Token: 0x040000C0 RID: 192
		internal const string SerializerNoSuchField = "SerializerNoSuchField";

		// Token: 0x040000C1 RID: 193
		internal const string SerializerNoSuchProperty = "SerializerNoSuchProperty";

		// Token: 0x040000C2 RID: 194
		internal const string SerializerNullNestedProperty = "SerializerNullNestedProperty";

		// Token: 0x040000C3 RID: 195
		internal const string SerializerInvalidArrayRank = "SerializerInvalidArrayRank";

		// Token: 0x040000C4 RID: 196
		internal const string SerializerResourceException = "SerializerResourceException";

		// Token: 0x040000C5 RID: 197
		internal const string SerializerResourceExceptionInvariant = "SerializerResourceExceptionInvariant";

		// Token: 0x040000C6 RID: 198
		internal const string SerializerPropertyGenFailed = "SerializerPropertyGenFailed";

		// Token: 0x040000C7 RID: 199
		internal const string SerializerFieldTargetEvalFailed = "SerializerFieldTargetEvalFailed";

		// Token: 0x040000C8 RID: 200
		internal const string SerializerMemberTypeNotSerializable = "SerializerMemberTypeNotSerializable";

		// Token: 0x040000C9 RID: 201
		internal const string SerializerNoRootExpression = "SerializerNoRootExpression";

		// Token: 0x040000CA RID: 202
		internal const string AXAbout = "AXAbout";

		// Token: 0x040000CB RID: 203
		internal const string AXCannotLoadTypeLib = "AXCannotLoadTypeLib";

		// Token: 0x040000CC RID: 204
		internal const string AXCannotOverwriteFile = "AXCannotOverwriteFile";

		// Token: 0x040000CD RID: 205
		internal const string AXReadOnlyFile = "AXReadOnlyFile";

		// Token: 0x040000CE RID: 206
		internal const string AXCompilerError = "AXCompilerError";

		// Token: 0x040000CF RID: 207
		internal const string Ax_Control = "Ax_Control";

		// Token: 0x040000D0 RID: 208
		internal const string AXEdit = "AXEdit";

		// Token: 0x040000D1 RID: 209
		internal const string AxImportFailed = "AxImportFailed";

		// Token: 0x040000D2 RID: 210
		internal const string AXNoActiveXControls = "AXNoActiveXControls";

		// Token: 0x040000D3 RID: 211
		internal const string AXNotRegistered = "AXNotRegistered";

		// Token: 0x040000D4 RID: 212
		internal const string AXNotValidControl = "AXNotValidControl";

		// Token: 0x040000D5 RID: 213
		internal const string AxImpNoDefaultValue = "AxImpNoDefaultValue";

		// Token: 0x040000D6 RID: 214
		internal const string AxImpUnrecognizedDefaultValueType = "AxImpUnrecognizedDefaultValueType";

		// Token: 0x040000D7 RID: 215
		internal const string AXProperties = "AXProperties";

		// Token: 0x040000D8 RID: 216
		internal const string AXVerbPrefix = "AXVerbPrefix";

		// Token: 0x040000D9 RID: 217
		internal const string AdvancedBindingPropertyDescriptorDesc = "AdvancedBindingPropertyDescriptorDesc";

		// Token: 0x040000DA RID: 218
		internal const string AdvancedBindingPropertyDescName = "AdvancedBindingPropertyDescName";

		// Token: 0x040000DB RID: 219
		internal const string AutoAdjustMargins = "AutoAdjustMargins";

		// Token: 0x040000DC RID: 220
		internal const string BaseNodeName = "BaseNodeName";

		// Token: 0x040000DD RID: 221
		internal const string BindingFormattingDialogAllTreeNode = "BindingFormattingDialogAllTreeNode";

		// Token: 0x040000DE RID: 222
		internal const string BindingFormattingDialogBindingPickerAccName = "BindingFormattingDialogBindingPickerAccName";

		// Token: 0x040000DF RID: 223
		internal const string BindingFormattingDialogCommonTreeNode = "BindingFormattingDialogCommonTreeNode";

		// Token: 0x040000E0 RID: 224
		internal const string BindingFormattingDialogCustomFormat = "BindingFormattingDialogCustomFormat";

		// Token: 0x040000E1 RID: 225
		internal const string BindingFormattingDialogCustomFormatAccessibleDescription = "BindingFormattingDialogCustomFormatAccessibleDescription";

		// Token: 0x040000E2 RID: 226
		internal const string BindingFormattingDialogDataSourcePickerDropDownAccName = "BindingFormattingDialogDataSourcePickerDropDownAccName";

		// Token: 0x040000E3 RID: 227
		internal const string BindingFormattingDialogDecimalPlaces = "BindingFormattingDialogDecimalPlaces";

		// Token: 0x040000E4 RID: 228
		internal const string BindingFormattingDialogFormatTypeCurrency = "BindingFormattingDialogFormatTypeCurrency";

		// Token: 0x040000E5 RID: 229
		internal const string BindingFormattingDialogFormatTypeCurrencyExplanation = "BindingFormattingDialogFormatTypeCurrencyExplanation";

		// Token: 0x040000E6 RID: 230
		internal const string BindingFormattingDialogFormatTypeCustom = "BindingFormattingDialogFormatTypeCustom";

		// Token: 0x040000E7 RID: 231
		internal const string BindingFormattingDialogFormatTypeCustomExplanation = "BindingFormattingDialogFormatTypeCustomExplanation";

		// Token: 0x040000E8 RID: 232
		internal const string BindingFormattingDialogFormatTypeCustomInvalidFormat = "BindingFormattingDialogFormatTypeCustomInvalidFormat";

		// Token: 0x040000E9 RID: 233
		internal const string BindingFormattingDialogFormatTypeDateTime = "BindingFormattingDialogFormatTypeDateTime";

		// Token: 0x040000EA RID: 234
		internal const string BindingFormattingDialogFormatTypeDateTimeExplanation = "BindingFormattingDialogFormatTypeDateTimeExplanation";

		// Token: 0x040000EB RID: 235
		internal const string BindingFormattingDialogFormatTypeNoFormatting = "BindingFormattingDialogFormatTypeNoFormatting";

		// Token: 0x040000EC RID: 236
		internal const string BindingFormattingDialogFormatTypeNoFormattingExplanation = "BindingFormattingDialogFormatTypeNoFormattingExplanation";

		// Token: 0x040000ED RID: 237
		internal const string BindingFormattingDialogFormatTypeNumeric = "BindingFormattingDialogFormatTypeNumeric";

		// Token: 0x040000EE RID: 238
		internal const string BindingFormattingDialogFormatTypeNumericExplanation = "BindingFormattingDialogFormatTypeNumericExplanation";

		// Token: 0x040000EF RID: 239
		internal const string BindingFormattingDialogFormatTypeScientific = "BindingFormattingDialogFormatTypeScientific";

		// Token: 0x040000F0 RID: 240
		internal const string BindingFormattingDialogFormatTypeScientificExplanation = "BindingFormattingDialogFormatTypeScientificExplanation";

		// Token: 0x040000F1 RID: 241
		internal const string BindingFormattingDialogList = "BindingFormattingDialogList";

		// Token: 0x040000F2 RID: 242
		internal const string BindingFormattingDialogNullValue = "BindingFormattingDialogNullValue";

		// Token: 0x040000F3 RID: 243
		internal const string BindingFormattingDialogType = "BindingFormattingDialogType";

		// Token: 0x040000F4 RID: 244
		internal const string CellStyleBuilderPreview = "CellStyleBuilderPreview";

		// Token: 0x040000F5 RID: 245
		internal const string CellStyleBuilderPreviewText = "CellStyleBuilderPreviewText";

		// Token: 0x040000F6 RID: 246
		internal const string CellStyleBuilderTitle = "CellStyleBuilderTitle";

		// Token: 0x040000F7 RID: 247
		internal const string CellStyleBuilderNormalPreviewAccName = "CellStyleBuilderNormalPreviewAccName";

		// Token: 0x040000F8 RID: 248
		internal const string CellStyleBuilderSelectedPreviewAccName = "CellStyleBuilderSelectedPreviewAccName";

		// Token: 0x040000F9 RID: 249
		internal const string CommandSetAlignByPrimary = "CommandSetAlignByPrimary";

		// Token: 0x040000FA RID: 250
		internal const string CommandSetAlignToGrid = "CommandSetAlignToGrid";

		// Token: 0x040000FB RID: 251
		internal const string CommandSetBringToFront = "CommandSetBringToFront";

		// Token: 0x040000FC RID: 252
		internal const string CommandSetCutMultiple = "CommandSetCutMultiple";

		// Token: 0x040000FD RID: 253
		internal const string CommandSetDelete = "CommandSetDelete";

		// Token: 0x040000FE RID: 254
		internal const string CommandSetError = "CommandSetError";

		// Token: 0x040000FF RID: 255
		internal const string CommandSetFormatSpacing = "CommandSetFormatSpacing";

		// Token: 0x04000100 RID: 256
		internal const string CommandSetLockControls = "CommandSetLockControls";

		// Token: 0x04000101 RID: 257
		internal const string CommandSetPaste = "CommandSetPaste";

		// Token: 0x04000102 RID: 258
		internal const string CommandSetSendToBack = "CommandSetSendToBack";

		// Token: 0x04000103 RID: 259
		internal const string CommandSetSize = "CommandSetSize";

		// Token: 0x04000104 RID: 260
		internal const string CommandSetSizeToGrid = "CommandSetSizeToGrid";

		// Token: 0x04000105 RID: 261
		internal const string CommandSetUnknownSpacingCommand = "CommandSetUnknownSpacingCommand";

		// Token: 0x04000106 RID: 262
		internal const string CompositionDesignerWaterMark = "CompositionDesignerWaterMark";

		// Token: 0x04000107 RID: 263
		internal const string CompositionDesignerWaterMarkFirstLink = "CompositionDesignerWaterMarkFirstLink";

		// Token: 0x04000108 RID: 264
		internal const string CompositionDesignerWaterMarkSecondLink = "CompositionDesignerWaterMarkSecondLink";

		// Token: 0x04000109 RID: 265
		internal const string DataGridAdvancedBindingString = "DataGridAdvancedBindingString";

		// Token: 0x0400010A RID: 266
		internal const string DataGridNoneString = "DataGridNoneString";

		// Token: 0x0400010B RID: 267
		internal const string DataGridPopulateError = "DataGridPopulateError";

		// Token: 0x0400010C RID: 268
		internal const string DataGridAutoFormatString = "DataGridAutoFormatString";

		// Token: 0x0400010D RID: 269
		internal const string DataGridAutoFormatUndoTitle = "DataGridAutoFormatUndoTitle";

		// Token: 0x0400010E RID: 270
		internal const string DataGridAutoFormatSchemeName256Color1 = "DataGridAutoFormatSchemeName256Color1";

		// Token: 0x0400010F RID: 271
		internal const string DataGridAutoFormatSchemeName256Color2 = "DataGridAutoFormatSchemeName256Color2";

		// Token: 0x04000110 RID: 272
		internal const string DataGridAutoFormatSchemeNameClassic = "DataGridAutoFormatSchemeNameClassic";

		// Token: 0x04000111 RID: 273
		internal const string DataGridAutoFormatSchemeNameColorful1 = "DataGridAutoFormatSchemeNameColorful1";

		// Token: 0x04000112 RID: 274
		internal const string DataGridAutoFormatSchemeNameColorful2 = "DataGridAutoFormatSchemeNameColorful2";

		// Token: 0x04000113 RID: 275
		internal const string DataGridAutoFormatSchemeNameColorful3 = "DataGridAutoFormatSchemeNameColorful3";

		// Token: 0x04000114 RID: 276
		internal const string DataGridAutoFormatSchemeNameColorful4 = "DataGridAutoFormatSchemeNameColorful4";

		// Token: 0x04000115 RID: 277
		internal const string DataGridAutoFormatSchemeNameDefault = "DataGridAutoFormatSchemeNameDefault";

		// Token: 0x04000116 RID: 278
		internal const string DataGridAutoFormatSchemeNameProfessional1 = "DataGridAutoFormatSchemeNameProfessional1";

		// Token: 0x04000117 RID: 279
		internal const string DataGridAutoFormatSchemeNameProfessional2 = "DataGridAutoFormatSchemeNameProfessional2";

		// Token: 0x04000118 RID: 280
		internal const string DataGridAutoFormatSchemeNameProfessional3 = "DataGridAutoFormatSchemeNameProfessional3";

		// Token: 0x04000119 RID: 281
		internal const string DataGridAutoFormatSchemeNameProfessional4 = "DataGridAutoFormatSchemeNameProfessional4";

		// Token: 0x0400011A RID: 282
		internal const string DataGridAutoFormatSchemeNameSimple = "DataGridAutoFormatSchemeNameSimple";

		// Token: 0x0400011B RID: 283
		internal const string DataGridAutoFormatTableFirstColumn = "DataGridAutoFormatTableFirstColumn";

		// Token: 0x0400011C RID: 284
		internal const string DataGridAutoFormatTableSecondColumn = "DataGridAutoFormatTableSecondColumn";

		// Token: 0x0400011D RID: 285
		internal const string DataGridShowAllString = "DataGridShowAllString";

		// Token: 0x0400011E RID: 286
		internal const string DataSourceLocksItems = "DataSourceLocksItems";

		// Token: 0x0400011F RID: 287
		internal const string DesignBindingBadParseString = "DesignBindingBadParseString";

		// Token: 0x04000120 RID: 288
		internal const string DesignBindingContextRequiredWhenParsing = "DesignBindingContextRequiredWhenParsing";

		// Token: 0x04000121 RID: 289
		internal const string DesignBindingComponentNotFound = "DesignBindingComponentNotFound";

		// Token: 0x04000122 RID: 290
		internal const string DesignBindingPickerAccessibleName = "DesignBindingPickerAccessibleName";

		// Token: 0x04000123 RID: 291
		internal const string DesignBindingPickerAddProjDataSourceLabel = "DesignBindingPickerAddProjDataSourceLabel";

		// Token: 0x04000124 RID: 292
		internal const string DesignBindingPickerHelpGenAddDataSrc = "DesignBindingPickerHelpGenAddDataSrc";

		// Token: 0x04000125 RID: 293
		internal const string DesignBindingPickerHelpGenCurrentBinding = "DesignBindingPickerHelpGenCurrentBinding";

		// Token: 0x04000126 RID: 294
		internal const string DesignBindingPickerHelpGenPickBindSrc = "DesignBindingPickerHelpGenPickBindSrc";

		// Token: 0x04000127 RID: 295
		internal const string DesignBindingPickerHelpGenPickDataSrc = "DesignBindingPickerHelpGenPickDataSrc";

		// Token: 0x04000128 RID: 296
		internal const string DesignBindingPickerHelpGenPickMember = "DesignBindingPickerHelpGenPickMember";

		// Token: 0x04000129 RID: 297
		internal const string DesignBindingPickerHelpNodeBindSrcDM1 = "DesignBindingPickerHelpNodeBindSrcDM1";

		// Token: 0x0400012A RID: 298
		internal const string DesignBindingPickerHelpNodeBindSrcDS0 = "DesignBindingPickerHelpNodeBindSrcDS0";

		// Token: 0x0400012B RID: 299
		internal const string DesignBindingPickerHelpNodeBindSrcDS1 = "DesignBindingPickerHelpNodeBindSrcDS1";

		// Token: 0x0400012C RID: 300
		internal const string DesignBindingPickerHelpNodeBindSrcLM1 = "DesignBindingPickerHelpNodeBindSrcLM1";

		// Token: 0x0400012D RID: 301
		internal const string DesignBindingPickerHelpNodeFormInstDM1 = "DesignBindingPickerHelpNodeFormInstDM1";

		// Token: 0x0400012E RID: 302
		internal const string DesignBindingPickerHelpNodeFormInstDS0 = "DesignBindingPickerHelpNodeFormInstDS0";

		// Token: 0x0400012F RID: 303
		internal const string DesignBindingPickerHelpNodeFormInstDS1 = "DesignBindingPickerHelpNodeFormInstDS1";

		// Token: 0x04000130 RID: 304
		internal const string DesignBindingPickerHelpNodeFormInstLM0 = "DesignBindingPickerHelpNodeFormInstLM0";

		// Token: 0x04000131 RID: 305
		internal const string DesignBindingPickerHelpNodeFormInstLM1 = "DesignBindingPickerHelpNodeFormInstLM1";

		// Token: 0x04000132 RID: 306
		internal const string DesignBindingPickerHelpNodeInstances = "DesignBindingPickerHelpNodeInstances";

		// Token: 0x04000133 RID: 307
		internal const string DesignBindingPickerHelpNodeNone = "DesignBindingPickerHelpNodeNone";

		// Token: 0x04000134 RID: 308
		internal const string DesignBindingPickerHelpNodeOther = "DesignBindingPickerHelpNodeOther";

		// Token: 0x04000135 RID: 309
		internal const string DesignBindingPickerHelpNodeProject = "DesignBindingPickerHelpNodeProject";

		// Token: 0x04000136 RID: 310
		internal const string DesignBindingPickerHelpNodeProjectDM1 = "DesignBindingPickerHelpNodeProjectDM1";

		// Token: 0x04000137 RID: 311
		internal const string DesignBindingPickerHelpNodeProjectDS0 = "DesignBindingPickerHelpNodeProjectDS0";

		// Token: 0x04000138 RID: 312
		internal const string DesignBindingPickerHelpNodeProjectDS1 = "DesignBindingPickerHelpNodeProjectDS1";

		// Token: 0x04000139 RID: 313
		internal const string DesignBindingPickerHelpNodeProjectLM0 = "DesignBindingPickerHelpNodeProjectLM0";

		// Token: 0x0400013A RID: 314
		internal const string DesignBindingPickerHelpNodeProjectLM1 = "DesignBindingPickerHelpNodeProjectLM1";

		// Token: 0x0400013B RID: 315
		internal const string DesignBindingPickerHelpNodeProjectGroup = "DesignBindingPickerHelpNodeProjectGroup";

		// Token: 0x0400013C RID: 316
		internal const string DesignBindingPickerNodeNone = "DesignBindingPickerNodeNone";

		// Token: 0x0400013D RID: 317
		internal const string DesignBindingPickerNodeOther = "DesignBindingPickerNodeOther";

		// Token: 0x0400013E RID: 318
		internal const string DesignBindingPickerNodeProject = "DesignBindingPickerNodeProject";

		// Token: 0x0400013F RID: 319
		internal const string DesignBindingPickerNodeInstances = "DesignBindingPickerNodeInstances";

		// Token: 0x04000140 RID: 320
		internal const string DesignBindingPickerTreeViewAccessibleName = "DesignBindingPickerTreeViewAccessibleName";

		// Token: 0x04000141 RID: 321
		internal const string DesignerBatchCreateTool = "DesignerBatchCreateTool";

		// Token: 0x04000142 RID: 322
		internal const string DesignerCantParentType = "DesignerCantParentType";

		// Token: 0x04000143 RID: 323
		internal const string DesignerDefaultTab = "DesignerDefaultTab";

		// Token: 0x04000144 RID: 324
		internal const string UserControlTab = "UserControlTab";

		// Token: 0x04000145 RID: 325
		internal const string DesignerShortcutDockInParent = "DesignerShortcutDockInParent";

		// Token: 0x04000146 RID: 326
		internal const string DesignerShortcutUndockInParent = "DesignerShortcutUndockInParent";

		// Token: 0x04000147 RID: 327
		internal const string DesignerShortcutDockInForm = "DesignerShortcutDockInForm";

		// Token: 0x04000148 RID: 328
		internal const string DesignerShortcutDockInUserControl = "DesignerShortcutDockInUserControl";

		// Token: 0x04000149 RID: 329
		internal const string DesignerShortcutReparentControls = "DesignerShortcutReparentControls";

		// Token: 0x0400014A RID: 330
		internal const string DesignerShortcutHorizontalOrientation = "DesignerShortcutHorizontalOrientation";

		// Token: 0x0400014B RID: 331
		internal const string DesignerShortcutVerticalOrientation = "DesignerShortcutVerticalOrientation";

		// Token: 0x0400014C RID: 332
		internal const string DesignerNoUserControl = "DesignerNoUserControl";

		// Token: 0x0400014D RID: 333
		internal const string DesignerPropName = "DesignerPropName";

		// Token: 0x0400014E RID: 334
		internal const string DesignerBeginDragNotCalled = "DesignerBeginDragNotCalled";

		// Token: 0x0400014F RID: 335
		internal const string DesignerInheritedReadOnly = "DesignerInheritedReadOnly";

		// Token: 0x04000150 RID: 336
		internal const string DesignerInherited = "DesignerInherited";

		// Token: 0x04000151 RID: 337
		internal const string DesignerPropNotFound = "DesignerPropNotFound";

		// Token: 0x04000152 RID: 338
		internal const string DragDropDragComponents = "DragDropDragComponents";

		// Token: 0x04000153 RID: 339
		internal const string DragDropMoveComponent = "DragDropMoveComponent";

		// Token: 0x04000154 RID: 340
		internal const string DragDropMoveComponents = "DragDropMoveComponents";

		// Token: 0x04000155 RID: 341
		internal const string DragDropSizeComponent = "DragDropSizeComponent";

		// Token: 0x04000156 RID: 342
		internal const string DragDropSizeComponents = "DragDropSizeComponents";

		// Token: 0x04000157 RID: 343
		internal const string DragDropDropComponents = "DragDropDropComponents";

		// Token: 0x04000158 RID: 344
		internal const string DragDropSetDataError = "DragDropSetDataError";

		// Token: 0x04000159 RID: 345
		internal const string GenericFileFilter = "GenericFileFilter";

		// Token: 0x0400015A RID: 346
		internal const string GenericOpenFile = "GenericOpenFile";

		// Token: 0x0400015B RID: 347
		internal const string DataGridViewAdd = "DataGridViewAdd";

		// Token: 0x0400015C RID: 348
		internal const string DataGridViewAddColumn = "DataGridViewAddColumn";

		// Token: 0x0400015D RID: 349
		internal const string DataGridViewAddColumnDialogTitle = "DataGridViewAddColumnDialogTitle";

		// Token: 0x0400015E RID: 350
		internal const string DataGridViewAddColumnTransactionString = "DataGridViewAddColumnTransactionString";

		// Token: 0x0400015F RID: 351
		internal const string DataGridViewAddColumnVerb = "DataGridViewAddColumnVerb";

		// Token: 0x04000160 RID: 352
		internal const string DataGridViewBoundColumnProperties = "DataGridViewBoundColumnProperties";

		// Token: 0x04000161 RID: 353
		internal const string DataGridViewChooseDataSource = "DataGridViewChooseDataSource";

		// Token: 0x04000162 RID: 354
		internal const string DataGridViewColumnTypePropertyDescription = "DataGridViewColumnTypePropertyDescription";

		// Token: 0x04000163 RID: 355
		internal const string DataGridViewColumnCollectionTransaction = "DataGridViewColumnCollectionTransaction";

		// Token: 0x04000164 RID: 356
		internal const string DataGridViewDataSourceNoLongerValid = "DataGridViewDataSourceNoLongerValid";

		// Token: 0x04000165 RID: 357
		internal const string DataGridViewDeleteAccName = "DataGridViewDeleteAccName";

		// Token: 0x04000166 RID: 358
		internal const string DataGridViewDuplicateColumnName = "DataGridViewDuplicateColumnName";

		// Token: 0x04000167 RID: 359
		internal const string DataGridViewChooseDataSourceTransactionString = "DataGridViewChooseDataSourceTransactionString";

		// Token: 0x04000168 RID: 360
		internal const string DataGridViewDisableAddingTransactionString = "DataGridViewDisableAddingTransactionString";

		// Token: 0x04000169 RID: 361
		internal const string DataGridViewDisableColumnReorderingTransactionString = "DataGridViewDisableColumnReorderingTransactionString";

		// Token: 0x0400016A RID: 362
		internal const string DataGridViewDisableDeletingTransactionString = "DataGridViewDisableDeletingTransactionString";

		// Token: 0x0400016B RID: 363
		internal const string DataGridViewDisableEditingTransactionString = "DataGridViewDisableEditingTransactionString";

		// Token: 0x0400016C RID: 364
		internal const string DataGridViewEditColumnsTransactionString = "DataGridViewEditColumnsTransactionString";

		// Token: 0x0400016D RID: 365
		internal const string DataGridViewEnableAdding = "DataGridViewEnableAdding";

		// Token: 0x0400016E RID: 366
		internal const string DataGridViewEnableAddingTransactionString = "DataGridViewEnableAddingTransactionString";

		// Token: 0x0400016F RID: 367
		internal const string DataGridViewEnableDeleting = "DataGridViewEnableDeleting";

		// Token: 0x04000170 RID: 368
		internal const string DataGridViewEnableDeletingTransactionString = "DataGridViewEnableDeletingTransactionString";

		// Token: 0x04000171 RID: 369
		internal const string DataGridViewEnableEditing = "DataGridViewEnableEditing";

		// Token: 0x04000172 RID: 370
		internal const string DataGridViewEnableEditingTransactionString = "DataGridViewEnableEditingTransactionString";

		// Token: 0x04000173 RID: 371
		internal const string DataGridViewEditingTransactionString = "DataGridViewEditingTransactionString";

		// Token: 0x04000174 RID: 372
		internal const string DataGridViewEnableColumnReordering = "DataGridViewEnableColumnReordering";

		// Token: 0x04000175 RID: 373
		internal const string DataGridViewEnableColumnReorderingTransactionString = "DataGridViewEnableColumnReorderingTransactionString";

		// Token: 0x04000176 RID: 374
		internal const string DataGridView_Cancel = "DataGridView_Cancel";

		// Token: 0x04000177 RID: 375
		internal const string DataGridView_Close = "DataGridView_Close";

		// Token: 0x04000178 RID: 376
		internal const string DataGridViewEditColumnsVerb = "DataGridViewEditColumnsVerb";

		// Token: 0x04000179 RID: 377
		internal const string DataGridViewEditColumns = "DataGridViewEditColumns";

		// Token: 0x0400017A RID: 378
		internal const string DataGridViewFrozen = "DataGridViewFrozen";

		// Token: 0x0400017B RID: 379
		internal const string DataGridViewDataBoundColumn = "DataGridViewDataBoundColumn";

		// Token: 0x0400017C RID: 380
		internal const string DataGridViewDataSourceColumns = "DataGridViewDataSourceColumns";

		// Token: 0x0400017D RID: 381
		internal const string DataGridViewHeaderText = "DataGridViewHeaderText";

		// Token: 0x0400017E RID: 382
		internal const string DataGridViewHelp = "DataGridViewHelp";

		// Token: 0x0400017F RID: 383
		internal const string DataGridViewMoveDownAccName = "DataGridViewMoveDownAccName";

		// Token: 0x04000180 RID: 384
		internal const string DataGridViewMoveUpAccName = "DataGridViewMoveUpAccName";

		// Token: 0x04000181 RID: 385
		internal const string DataGridViewName = "DataGridViewName";

		// Token: 0x04000182 RID: 386
		internal const string DataGridViewNormalLabel = "DataGridViewNormalLabel";

		// Token: 0x04000183 RID: 387
		internal const string DataGridView_OK = "DataGridView_OK";

		// Token: 0x04000184 RID: 388
		internal const string DataGridViewProperties = "DataGridViewProperties";

		// Token: 0x04000185 RID: 389
		internal const string DataGridViewReadOnly = "DataGridViewReadOnly";

		// Token: 0x04000186 RID: 390
		internal const string DataGridViewSelectedColumns = "DataGridViewSelectedColumns";

		// Token: 0x04000187 RID: 391
		internal const string DataGridViewSelectedLabel = "DataGridViewSelectedLabel";

		// Token: 0x04000188 RID: 392
		internal const string DataGridViewType = "DataGridViewType";

		// Token: 0x04000189 RID: 393
		internal const string DataGridViewUnboundColumn = "DataGridViewUnboundColumn";

		// Token: 0x0400018A RID: 394
		internal const string DataGridViewUnboundColumnProperties = "DataGridViewUnboundColumnProperties";

		// Token: 0x0400018B RID: 395
		internal const string DataGridViewVisible = "DataGridViewVisible";

		// Token: 0x0400018C RID: 396
		internal const string FailedToCreateComponent = "FailedToCreateComponent";

		// Token: 0x0400018D RID: 397
		internal const string FormatStringDialogTitle = "FormatStringDialogTitle";

		// Token: 0x0400018E RID: 398
		internal const string HelpProviderEditorFilter = "HelpProviderEditorFilter";

		// Token: 0x0400018F RID: 399
		internal const string HelpProviderEditorTitle = "HelpProviderEditorTitle";

		// Token: 0x04000190 RID: 400
		internal const string imageFileDescription = "imageFileDescription";

		// Token: 0x04000191 RID: 401
		internal const string ImageListDesignerBadImageListImage = "ImageListDesignerBadImageListImage";

		// Token: 0x04000192 RID: 402
		internal const string ImageCollectionEditorFormText = "ImageCollectionEditorFormText";

		// Token: 0x04000193 RID: 403
		internal const string IntegerCollectionEditorCancelCaption = "IntegerCollectionEditorCancelCaption";

		// Token: 0x04000194 RID: 404
		internal const string IntegerCollectionEditorInstruction = "IntegerCollectionEditorInstruction";

		// Token: 0x04000195 RID: 405
		internal const string IntegerCollectionEditorOKCaption = "IntegerCollectionEditorOKCaption";

		// Token: 0x04000196 RID: 406
		internal const string IntegerCollectionEditorHelpCaption = "IntegerCollectionEditorHelpCaption";

		// Token: 0x04000197 RID: 407
		internal const string InvalidArgument = "InvalidArgument";

		// Token: 0x04000198 RID: 408
		internal const string InvalidArgumentType = "InvalidArgumentType";

		// Token: 0x04000199 RID: 409
		internal const string InvalidBoundArgument = "InvalidBoundArgument";

		// Token: 0x0400019A RID: 410
		internal const string LinkAreaEditorCancel = "LinkAreaEditorCancel";

		// Token: 0x0400019B RID: 411
		internal const string LinkAreaEditorCaption = "LinkAreaEditorCaption";

		// Token: 0x0400019C RID: 412
		internal const string LinkAreaEditorDescription = "LinkAreaEditorDescription";

		// Token: 0x0400019D RID: 413
		internal const string LinkAreaEditorOK = "LinkAreaEditorOK";

		// Token: 0x0400019E RID: 414
		internal const string ListViewItemBaseName = "ListViewItemBaseName";

		// Token: 0x0400019F RID: 415
		internal const string ListViewSubItemBaseName = "ListViewSubItemBaseName";

		// Token: 0x040001A0 RID: 416
		internal const string MaskDescriptorNullOrEmptyRequiredProperty = "MaskDescriptorNullOrEmptyRequiredProperty";

		// Token: 0x040001A1 RID: 417
		internal const string MaskDescriptorNull = "MaskDescriptorNull";

		// Token: 0x040001A2 RID: 418
		internal const string MaskDescriptorNotMaskFullErrorMsg = "MaskDescriptorNotMaskFullErrorMsg";

		// Token: 0x040001A3 RID: 419
		internal const string MaskDescriptorValidatingTypeNone = "MaskDescriptorValidatingTypeNone";

		// Token: 0x040001A4 RID: 420
		internal const string MaskDesignerDialogCustomEntry = "MaskDesignerDialogCustomEntry";

		// Token: 0x040001A5 RID: 421
		internal const string MaskDesignerDialogDataFormat = "MaskDesignerDialogDataFormat";

		// Token: 0x040001A6 RID: 422
		internal const string MaskDesignerDialogDlgCaption = "MaskDesignerDialogDlgCaption";

		// Token: 0x040001A7 RID: 423
		internal const string MaskDesignerDialogMaskDescription = "MaskDesignerDialogMaskDescription";

		// Token: 0x040001A8 RID: 424
		internal const string MaskDesignerDialogValidatingType = "MaskDesignerDialogValidatingType";

		// Token: 0x040001A9 RID: 425
		internal const string MaskedTextBoxDesignerVerbsSetMaskDesc = "MaskedTextBoxDesignerVerbsSetMaskDesc";

		// Token: 0x040001AA RID: 426
		internal const string MaskedTextBoxTextEditorErrorFormatString = "MaskedTextBoxTextEditorErrorFormatString";

		// Token: 0x040001AB RID: 427
		internal const string MaskedTextBoxHintAsciiCharacterExpected = "MaskedTextBoxHintAsciiCharacterExpected";

		// Token: 0x040001AC RID: 428
		internal const string MaskedTextBoxHintAlphanumericCharacterExpected = "MaskedTextBoxHintAlphanumericCharacterExpected";

		// Token: 0x040001AD RID: 429
		internal const string MaskedTextBoxHintDigitExpected = "MaskedTextBoxHintDigitExpected";

		// Token: 0x040001AE RID: 430
		internal const string MaskedTextBoxHintSignedDigitExpected = "MaskedTextBoxHintSignedDigitExpected";

		// Token: 0x040001AF RID: 431
		internal const string MaskedTextBoxHintLetterExpected = "MaskedTextBoxHintLetterExpected";

		// Token: 0x040001B0 RID: 432
		internal const string MaskedTextBoxHintPromptCharNotAllowed = "MaskedTextBoxHintPromptCharNotAllowed";

		// Token: 0x040001B1 RID: 433
		internal const string MaskedTextBoxHintUnavailableEditPosition = "MaskedTextBoxHintUnavailableEditPosition";

		// Token: 0x040001B2 RID: 434
		internal const string MaskedTextBoxHintNonEditPosition = "MaskedTextBoxHintNonEditPosition";

		// Token: 0x040001B3 RID: 435
		internal const string MaskedTextBoxHintPositionOutOfRange = "MaskedTextBoxHintPositionOutOfRange";

		// Token: 0x040001B4 RID: 436
		internal const string MaskedTextBoxHintInvalidInput = "MaskedTextBoxHintInvalidInput";

		// Token: 0x040001B5 RID: 437
		internal const string MenuCommandService_DuplicateCommand = "MenuCommandService_DuplicateCommand";

		// Token: 0x040001B6 RID: 438
		internal const string lockedDescr = "lockedDescr";

		// Token: 0x040001B7 RID: 439
		internal const string ParentControlDesignerDrawGridDescr = "ParentControlDesignerDrawGridDescr";

		// Token: 0x040001B8 RID: 440
		internal const string ParentControlDesignerSnapToGridDescr = "ParentControlDesignerSnapToGridDescr";

		// Token: 0x040001B9 RID: 441
		internal const string ParentControlDesignerGridSizeDescr = "ParentControlDesignerGridSizeDescr";

		// Token: 0x040001BA RID: 442
		internal const string ParentControlDesignerLanguageDescr = "ParentControlDesignerLanguageDescr";

		// Token: 0x040001BB RID: 443
		internal const string ParentControlDesignerLassoShortcutRedo = "ParentControlDesignerLassoShortcutRedo";

		// Token: 0x040001BC RID: 444
		internal const string PerformAutoAnchor = "PerformAutoAnchor";

		// Token: 0x040001BD RID: 445
		internal const string RtfFileFilter = "RtfFileFilter";

		// Token: 0x040001BE RID: 446
		internal const string RtfOpenFile = "RtfOpenFile";

		// Token: 0x040001BF RID: 447
		internal const string SelectedPathEditorLabel = "SelectedPathEditorLabel";

		// Token: 0x040001C0 RID: 448
		internal const string ShortcutKeys_InvalidKey = "ShortcutKeys_InvalidKey";

		// Token: 0x040001C1 RID: 449
		internal const string SoundPathWavFile = "SoundPathWavFile";

		// Token: 0x040001C2 RID: 450
		internal const string SoundPathEditorOpenFile = "SoundPathEditorOpenFile";

		// Token: 0x040001C3 RID: 451
		internal const string SoundPlayNowString = "SoundPlayNowString";

		// Token: 0x040001C4 RID: 452
		internal const string SplitContainerReplaceString = "SplitContainerReplaceString";

		// Token: 0x040001C5 RID: 453
		internal const string SplitContainerReplaceCaption = "SplitContainerReplaceCaption";

		// Token: 0x040001C6 RID: 454
		internal const string SplitterHorizontalOrientation = "SplitterHorizontalOrientation";

		// Token: 0x040001C7 RID: 455
		internal const string SplitterVerticalOrientation = "SplitterVerticalOrientation";

		// Token: 0x040001C8 RID: 456
		internal const string TabControlAdd = "TabControlAdd";

		// Token: 0x040001C9 RID: 457
		internal const string TabControlAddTab = "TabControlAddTab";

		// Token: 0x040001CA RID: 458
		internal const string TabControlRemoveTab = "TabControlRemoveTab";

		// Token: 0x040001CB RID: 459
		internal const string TabControlRemove = "TabControlRemove";

		// Token: 0x040001CC RID: 460
		internal const string TabControlInvalidTabPageType = "TabControlInvalidTabPageType";

		// Token: 0x040001CD RID: 461
		internal const string TableLayoutPanelRowColResize = "TableLayoutPanelRowColResize";

		// Token: 0x040001CE RID: 462
		internal const string TableLayoutPanelDesignerChangeSizeTypeUndoUnit = "TableLayoutPanelDesignerChangeSizeTypeUndoUnit";

		// Token: 0x040001CF RID: 463
		internal const string TableLayoutPanelDesignerClearAnchor = "TableLayoutPanelDesignerClearAnchor";

		// Token: 0x040001D0 RID: 464
		internal const string TableLayoutPanelDesignerClearDock = "TableLayoutPanelDesignerClearDock";

		// Token: 0x040001D1 RID: 465
		internal const string TableLayoutPanelDesignerAddColumn = "TableLayoutPanelDesignerAddColumn";

		// Token: 0x040001D2 RID: 466
		internal const string TableLayoutPanelDesignerAddRow = "TableLayoutPanelDesignerAddRow";

		// Token: 0x040001D3 RID: 467
		internal const string TableLayoutPanelDesignerRemoveColumn = "TableLayoutPanelDesignerRemoveColumn";

		// Token: 0x040001D4 RID: 468
		internal const string TableLayoutPanelDesignerRemoveRow = "TableLayoutPanelDesignerRemoveRow";

		// Token: 0x040001D5 RID: 469
		internal const string TableLayoutPanelDesignerEditRowAndCol = "TableLayoutPanelDesignerEditRowAndCol";

		// Token: 0x040001D6 RID: 470
		internal const string TableLayoutPanelDesignerRowMenu = "TableLayoutPanelDesignerRowMenu";

		// Token: 0x040001D7 RID: 471
		internal const string TableLayoutPanelDesignerColMenu = "TableLayoutPanelDesignerColMenu";

		// Token: 0x040001D8 RID: 472
		internal const string TableLayoutPanelDesignerAddMenu = "TableLayoutPanelDesignerAddMenu";

		// Token: 0x040001D9 RID: 473
		internal const string TableLayoutPanelDesignerInsertMenu = "TableLayoutPanelDesignerInsertMenu";

		// Token: 0x040001DA RID: 474
		internal const string TableLayoutPanelDesignerDeleteMenu = "TableLayoutPanelDesignerDeleteMenu";

		// Token: 0x040001DB RID: 475
		internal const string TableLayoutPanelDesignerLabelMenu = "TableLayoutPanelDesignerLabelMenu";

		// Token: 0x040001DC RID: 476
		internal const string TableLayoutPanelDesignerDontBoldLabel = "TableLayoutPanelDesignerDontBoldLabel";

		// Token: 0x040001DD RID: 477
		internal const string TableLayoutPanelDesignerAbsoluteMenu = "TableLayoutPanelDesignerAbsoluteMenu";

		// Token: 0x040001DE RID: 478
		internal const string TableLayoutPanelDesignerPercentageMenu = "TableLayoutPanelDesignerPercentageMenu";

		// Token: 0x040001DF RID: 479
		internal const string TableLayoutPanelDesignerAutoSizeMenu = "TableLayoutPanelDesignerAutoSizeMenu";

		// Token: 0x040001E0 RID: 480
		internal const string TableLayoutPanelDesignerContextMenuCut = "TableLayoutPanelDesignerContextMenuCut";

		// Token: 0x040001E1 RID: 481
		internal const string TableLayoutPanelDesignerContextMenuCopy = "TableLayoutPanelDesignerContextMenuCopy";

		// Token: 0x040001E2 RID: 482
		internal const string TableLayoutPanelDesignerContextMenuDelete = "TableLayoutPanelDesignerContextMenuDelete";

		// Token: 0x040001E3 RID: 483
		internal const string TableLayoutPanelDesignerAddColumnUndoUnit = "TableLayoutPanelDesignerAddColumnUndoUnit";

		// Token: 0x040001E4 RID: 484
		internal const string TableLayoutPanelDesignerAddRowUndoUnit = "TableLayoutPanelDesignerAddRowUndoUnit";

		// Token: 0x040001E5 RID: 485
		internal const string TableLayoutPanelDesignerRemoveColumnUndoUnit = "TableLayoutPanelDesignerRemoveColumnUndoUnit";

		// Token: 0x040001E6 RID: 486
		internal const string TableLayoutPanelDesignerRemoveRowUndoUnit = "TableLayoutPanelDesignerRemoveRowUndoUnit";

		// Token: 0x040001E7 RID: 487
		internal const string TableLayoutPanelDesignerControlsSwapped = "TableLayoutPanelDesignerControlsSwapped";

		// Token: 0x040001E8 RID: 488
		internal const string TableLayoutPanelDesignerInvalidColumnRowCount = "TableLayoutPanelDesignerInvalidColumnRowCount";

		// Token: 0x040001E9 RID: 489
		internal const string ToolStripTemplateNodeImageResetCaption = "ToolStripTemplateNodeImageResetCaption";

		// Token: 0x040001EA RID: 490
		internal const string ToolStripTemplateNodeImageResetString = "ToolStripTemplateNodeImageResetString";

		// Token: 0x040001EB RID: 491
		internal const string ToolStripItemPropertyChangeTransaction = "ToolStripItemPropertyChangeTransaction";

		// Token: 0x040001EC RID: 492
		internal const string ToolStripInsertItemsVerb = "ToolStripInsertItemsVerb";

		// Token: 0x040001ED RID: 493
		internal const string ToolStripSelectAllVerb = "ToolStripSelectAllVerb";

		// Token: 0x040001EE RID: 494
		internal const string ToolStripDropDownDesignerDropDownMenu = "ToolStripDropDownDesignerDropDownMenu";

		// Token: 0x040001EF RID: 495
		internal const string ToolStripMorphingItemTransaction = "ToolStripMorphingItemTransaction";

		// Token: 0x040001F0 RID: 496
		internal const string ToolStripCreatingNewItemTransaction = "ToolStripCreatingNewItemTransaction";

		// Token: 0x040001F1 RID: 497
		internal const string ToolStripInsertingIntoDropDownTransaction = "ToolStripInsertingIntoDropDownTransaction";

		// Token: 0x040001F2 RID: 498
		internal const string ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue = "ToolStripAllowItemReorderAndAllowDropCannotBeSetToTrue";

		// Token: 0x040001F3 RID: 499
		internal const string ToolStripSelectMenuItem = "ToolStripSelectMenuItem";

		// Token: 0x040001F4 RID: 500
		internal const string ToolStripPanelGlyphUnsupportedDock = "ToolStripPanelGlyphUnsupportedDock";

		// Token: 0x040001F5 RID: 501
		internal const string WindowsFormsAddEvent = "WindowsFormsAddEvent";

		// Token: 0x040001F6 RID: 502
		internal const string WindowsFormsCommandCenterX = "WindowsFormsCommandCenterX";

		// Token: 0x040001F7 RID: 503
		internal const string WindowsFormsCommandCenterY = "WindowsFormsCommandCenterY";

		// Token: 0x040001F8 RID: 504
		internal const string WindowsFormsTabOrderReadOnly = "WindowsFormsTabOrderReadOnly";

		// Token: 0x040001F9 RID: 505
		internal const string OK = "OK";

		// Token: 0x040001FA RID: 506
		internal const string Cancel = "Cancel";

		// Token: 0x040001FB RID: 507
		internal const string Value = "Value";

		// Token: 0x040001FC RID: 508
		internal const string None = "None";

		// Token: 0x040001FD RID: 509
		internal const string Default = "Default";

		// Token: 0x040001FE RID: 510
		internal const string Custom = "Custom";

		// Token: 0x040001FF RID: 511
		internal const string Edit = "Edit";

		// Token: 0x04000200 RID: 512
		internal const string None_lc = "None_lc";

		// Token: 0x04000201 RID: 513
		internal const string Control_ErrorRendering = "Control_ErrorRendering";

		// Token: 0x04000202 RID: 514
		internal const string Control_ErrorRenderingShort = "Control_ErrorRenderingShort";

		// Token: 0x04000203 RID: 515
		internal const string Control_Expressions = "Control_Expressions";

		// Token: 0x04000204 RID: 516
		internal const string Control_CanOnlyBePlacedInside = "Control_CanOnlyBePlacedInside";

		// Token: 0x04000205 RID: 517
		internal const string ControlDesigner_DesignTimeHtmlError = "ControlDesigner_DesignTimeHtmlError";

		// Token: 0x04000206 RID: 518
		internal const string ControlDesigner_UnhandledException = "ControlDesigner_UnhandledException";

		// Token: 0x04000207 RID: 519
		internal const string ControlDesigner_TransactedChangeRequiresServiceProvider = "ControlDesigner_TransactedChangeRequiresServiceProvider";

		// Token: 0x04000208 RID: 520
		internal const string ControlDesigner_CouldNotGetExpressionBuilder = "ControlDesigner_CouldNotGetExpressionBuilder";

		// Token: 0x04000209 RID: 521
		internal const string ControlDesigner_CouldNotGetDesignTimeResourceProviderFactory = "ControlDesigner_CouldNotGetDesignTimeResourceProviderFactory";

		// Token: 0x0400020A RID: 522
		internal const string ControlDesigner_ArgumentMustBeOfType = "ControlDesigner_ArgumentMustBeOfType";

		// Token: 0x0400020B RID: 523
		internal const string UnsettableComboBox_NotSet = "UnsettableComboBox_NotSet";

		// Token: 0x0400020C RID: 524
		internal const string ControlLocalizer_RequiresFilterService = "ControlLocalizer_RequiresFilterService";

		// Token: 0x0400020D RID: 525
		internal const string Wizard_NextButton = "Wizard_NextButton";

		// Token: 0x0400020E RID: 526
		internal const string Wizard_PreviousButton = "Wizard_PreviousButton";

		// Token: 0x0400020F RID: 527
		internal const string Wizard_CancelButton = "Wizard_CancelButton";

		// Token: 0x04000210 RID: 528
		internal const string Wizard_FinishButton = "Wizard_FinishButton";

		// Token: 0x04000211 RID: 529
		internal const string WizardAFmt_Scheme_Default = "WizardAFmt_Scheme_Default";

		// Token: 0x04000212 RID: 530
		internal const string WizardAFmt_Scheme_Classic = "WizardAFmt_Scheme_Classic";

		// Token: 0x04000213 RID: 531
		internal const string WizardAFmt_Scheme_Simple = "WizardAFmt_Scheme_Simple";

		// Token: 0x04000214 RID: 532
		internal const string WizardAFmt_Scheme_Professional = "WizardAFmt_Scheme_Professional";

		// Token: 0x04000215 RID: 533
		internal const string WizardAFmt_Scheme_Colorful = "WizardAFmt_Scheme_Colorful";

		// Token: 0x04000216 RID: 534
		internal const string Wizard_StepsView = "Wizard_StepsView";

		// Token: 0x04000217 RID: 535
		internal const string Wizard_StepsViewDescription = "Wizard_StepsViewDescription";

		// Token: 0x04000218 RID: 536
		internal const string CreateUserWizard_ConvertToCustomNavigationTemplate = "CreateUserWizard_ConvertToCustomNavigationTemplate";

		// Token: 0x04000219 RID: 537
		internal const string CreateUserWizard_CustomizeCreateUserStep = "CreateUserWizard_CustomizeCreateUserStep";

		// Token: 0x0400021A RID: 538
		internal const string CreateUserWizard_CustomizeCreateUserStepDescription = "CreateUserWizard_CustomizeCreateUserStepDescription";

		// Token: 0x0400021B RID: 539
		internal const string CreateUserWizard_CustomizeCompleteStep = "CreateUserWizard_CustomizeCompleteStep";

		// Token: 0x0400021C RID: 540
		internal const string CreateUserWizard_CustomizeCompleteStepDescription = "CreateUserWizard_CustomizeCompleteStepDescription";

		// Token: 0x0400021D RID: 541
		internal const string CreateUserWizard_ResetCreateUserStepVerb = "CreateUserWizard_ResetCreateUserStepVerb";

		// Token: 0x0400021E RID: 542
		internal const string CreateUserWizard_ResetCreateUserStepVerbDescription = "CreateUserWizard_ResetCreateUserStepVerbDescription";

		// Token: 0x0400021F RID: 543
		internal const string CreateUserWizard_ResetCompleteStepVerb = "CreateUserWizard_ResetCompleteStepVerb";

		// Token: 0x04000220 RID: 544
		internal const string CreateUserWizard_ResetCompleteStepVerbDescription = "CreateUserWizard_ResetCompleteStepVerbDescription";

		// Token: 0x04000221 RID: 545
		internal const string CreateUserWizard_NavigateToStep = "CreateUserWizard_NavigateToStep";

		// Token: 0x04000222 RID: 546
		internal const string CreateUserWizardAutoFormat_UserName = "CreateUserWizardAutoFormat_UserName";

		// Token: 0x04000223 RID: 547
		internal const string CreateUserWizardAutoFormat_HelpPageText = "CreateUserWizardAutoFormat_HelpPageText";

		// Token: 0x04000224 RID: 548
		internal const string CreateUserWizardStepCollectionEditor_Caption = "CreateUserWizardStepCollectionEditor_Caption";

		// Token: 0x04000225 RID: 549
		internal const string Wizard_ConvertToStartNavigationTemplate = "Wizard_ConvertToStartNavigationTemplate";

		// Token: 0x04000226 RID: 550
		internal const string Wizard_ConvertToStepNavigationTemplate = "Wizard_ConvertToStepNavigationTemplate";

		// Token: 0x04000227 RID: 551
		internal const string Wizard_ConvertToFinishNavigationTemplate = "Wizard_ConvertToFinishNavigationTemplate";

		// Token: 0x04000228 RID: 552
		internal const string Wizard_ConvertToSideBarTemplate = "Wizard_ConvertToSideBarTemplate";

		// Token: 0x04000229 RID: 553
		internal const string Wizard_ConvertToCustomNavigationTemplate = "Wizard_ConvertToCustomNavigationTemplate";

		// Token: 0x0400022A RID: 554
		internal const string Wizard_ConvertToTemplateDescription = "Wizard_ConvertToTemplateDescription";

		// Token: 0x0400022B RID: 555
		internal const string Wizard_ResetCustomNavigationTemplate = "Wizard_ResetCustomNavigationTemplate";

		// Token: 0x0400022C RID: 556
		internal const string Wizard_ResetStartNavigationTemplate = "Wizard_ResetStartNavigationTemplate";

		// Token: 0x0400022D RID: 557
		internal const string Wizard_ResetStepNavigationTemplate = "Wizard_ResetStepNavigationTemplate";

		// Token: 0x0400022E RID: 558
		internal const string Wizard_ResetFinishNavigationTemplate = "Wizard_ResetFinishNavigationTemplate";

		// Token: 0x0400022F RID: 559
		internal const string Wizard_ResetSideBarTemplate = "Wizard_ResetSideBarTemplate";

		// Token: 0x04000230 RID: 560
		internal const string Wizard_ResetDescription = "Wizard_ResetDescription";

		// Token: 0x04000231 RID: 561
		internal const string Wizard_StartWizardStepCollectionEditor = "Wizard_StartWizardStepCollectionEditor";

		// Token: 0x04000232 RID: 562
		internal const string Wizard_StartWizardStepCollectionEditorDescription = "Wizard_StartWizardStepCollectionEditorDescription";

		// Token: 0x04000233 RID: 563
		internal const string Wizard_OnViewChanged = "Wizard_OnViewChanged";

		// Token: 0x04000234 RID: 564
		internal const string Wizard_InvalidRegion = "Wizard_InvalidRegion";

		// Token: 0x04000235 RID: 565
		internal const string UIServiceHelper_ErrorCaption = "UIServiceHelper_ErrorCaption";

		// Token: 0x04000236 RID: 566
		internal const string Designer_DataBindingsVerb = "Designer_DataBindingsVerb";

		// Token: 0x04000237 RID: 567
		internal const string Designer_DataBindingsVerbDesc = "Designer_DataBindingsVerbDesc";

		// Token: 0x04000238 RID: 568
		internal const string MdbDataFileEditor_Ellipses = "MdbDataFileEditor_Ellipses";

		// Token: 0x04000239 RID: 569
		internal const string MdbDataFileEditor_Caption = "MdbDataFileEditor_Caption";

		// Token: 0x0400023A RID: 570
		internal const string MdbDataFileEditor_Filter = "MdbDataFileEditor_Filter";

		// Token: 0x0400023B RID: 571
		internal const string XmlDataFileEditor_Ellipses = "XmlDataFileEditor_Ellipses";

		// Token: 0x0400023C RID: 572
		internal const string XmlDataFileEditor_Caption = "XmlDataFileEditor_Caption";

		// Token: 0x0400023D RID: 573
		internal const string XmlDataFileEditor_Filter = "XmlDataFileEditor_Filter";

		// Token: 0x0400023E RID: 574
		internal const string XsdSchemaFileEditor_Ellipses = "XsdSchemaFileEditor_Ellipses";

		// Token: 0x0400023F RID: 575
		internal const string XsdSchemaFileEditor_Caption = "XsdSchemaFileEditor_Caption";

		// Token: 0x04000240 RID: 576
		internal const string XsdSchemaFileEditor_Filter = "XsdSchemaFileEditor_Filter";

		// Token: 0x04000241 RID: 577
		internal const string XslTransformFileEditor_Ellipses = "XslTransformFileEditor_Ellipses";

		// Token: 0x04000242 RID: 578
		internal const string XslTransformFileEditor_Caption = "XslTransformFileEditor_Caption";

		// Token: 0x04000243 RID: 579
		internal const string XslTransformFileEditor_Filter = "XslTransformFileEditor_Filter";

		// Token: 0x04000244 RID: 580
		internal const string UserControlFileEditor_Caption = "UserControlFileEditor_Caption";

		// Token: 0x04000245 RID: 581
		internal const string UserControlFileEditor_Filter = "UserControlFileEditor_Filter";

		// Token: 0x04000246 RID: 582
		internal const string ConnectionStringEditor_Title = "ConnectionStringEditor_Title";

		// Token: 0x04000247 RID: 583
		internal const string ConnectionStringEditor_HelpLabel = "ConnectionStringEditor_HelpLabel";

		// Token: 0x04000248 RID: 584
		internal const string ConnectionStringEditor_NewConnection = "ConnectionStringEditor_NewConnection";

		// Token: 0x04000249 RID: 585
		internal const string ConfigureDataSource_Title = "ConfigureDataSource_Title";

		// Token: 0x0400024A RID: 586
		internal const string DataSource_DebugService_FailedCall = "DataSource_DebugService_FailedCall";

		// Token: 0x0400024B RID: 587
		internal const string DataSource_CannotResumeEvents = "DataSource_CannotResumeEvents";

		// Token: 0x0400024C RID: 588
		internal const string DataSource_ConfigureTransactionDescription = "DataSource_ConfigureTransactionDescription";

		// Token: 0x0400024D RID: 589
		internal const string DataSourceDesigner_RefreshSchema = "DataSourceDesigner_RefreshSchema";

		// Token: 0x0400024E RID: 590
		internal const string DataSourceDesigner_RefreshSchemaNoHotkey = "DataSourceDesigner_RefreshSchemaNoHotkey";

		// Token: 0x0400024F RID: 591
		internal const string DataSourceDesigner_DataActionGroup = "DataSourceDesigner_DataActionGroup";

		// Token: 0x04000250 RID: 592
		internal const string DataSourceDesigner_ConfigureDataSourceVerb = "DataSourceDesigner_ConfigureDataSourceVerb";

		// Token: 0x04000251 RID: 593
		internal const string DataSourceDesigner_RefreshSchemaVerb = "DataSourceDesigner_RefreshSchemaVerb";

		// Token: 0x04000252 RID: 594
		internal const string DataSourceDesigner_ConfigureDataSourceVerbDesc = "DataSourceDesigner_ConfigureDataSourceVerbDesc";

		// Token: 0x04000253 RID: 595
		internal const string DataSourceDesigner_RefreshSchemaVerbDesc = "DataSourceDesigner_RefreshSchemaVerbDesc";

		// Token: 0x04000254 RID: 596
		internal const string HierarchicalDataBoundControlDesigner_SampleRoot = "HierarchicalDataBoundControlDesigner_SampleRoot";

		// Token: 0x04000255 RID: 597
		internal const string HierarchicalDataBoundControlDesigner_SampleParent = "HierarchicalDataBoundControlDesigner_SampleParent";

		// Token: 0x04000256 RID: 598
		internal const string HierarchicalDataBoundControlDesigner_SampleLeaf = "HierarchicalDataBoundControlDesigner_SampleLeaf";

		// Token: 0x04000257 RID: 599
		internal const string SqlDataSourceQueryConverter_Text = "SqlDataSourceQueryConverter_Text";

		// Token: 0x04000258 RID: 600
		internal const string SqlDataSourceDesigner_EditQueryTransactionDescription = "SqlDataSourceDesigner_EditQueryTransactionDescription";

		// Token: 0x04000259 RID: 601
		internal const string SqlDataSourceDesigner_DeleteQuery = "SqlDataSourceDesigner_DeleteQuery";

		// Token: 0x0400025A RID: 602
		internal const string SqlDataSourceDesigner_InsertQuery = "SqlDataSourceDesigner_InsertQuery";

		// Token: 0x0400025B RID: 603
		internal const string SqlDataSourceDesigner_SelectQuery = "SqlDataSourceDesigner_SelectQuery";

		// Token: 0x0400025C RID: 604
		internal const string SqlDataSourceDesigner_SelectCountQuery = "SqlDataSourceDesigner_SelectCountQuery";

		// Token: 0x0400025D RID: 605
		internal const string SqlDataSourceDesigner_UpdateQuery = "SqlDataSourceDesigner_UpdateQuery";

		// Token: 0x0400025E RID: 606
		internal const string SqlDataSourceDesigner_CannotGetSchema = "SqlDataSourceDesigner_CannotGetSchema";

		// Token: 0x0400025F RID: 607
		internal const string SqlDataSourceDesigner_CouldNotCreateConnection = "SqlDataSourceDesigner_CouldNotCreateConnection";

		// Token: 0x04000260 RID: 608
		internal const string SqlDataSourceDesigner_NoCommand = "SqlDataSourceDesigner_NoCommand";

		// Token: 0x04000261 RID: 609
		internal const string SqlDataSourceDesigner_InferStoredProcedureNotSupported = "SqlDataSourceDesigner_InferStoredProcedureNotSupported";

		// Token: 0x04000262 RID: 610
		internal const string SqlDataSourceDesigner_InferStoredProcedureError = "SqlDataSourceDesigner_InferStoredProcedureError";

		// Token: 0x04000263 RID: 611
		internal const string SqlDataSourceDesigner_RefreshSchemaRequiresSettings = "SqlDataSourceDesigner_RefreshSchemaRequiresSettings";

		// Token: 0x04000264 RID: 612
		internal const string SqlDataSource_General_PreviewLabel = "SqlDataSource_General_PreviewLabel";

		// Token: 0x04000265 RID: 613
		internal const string SqlDataSourceRefreshSchemaForm_Title = "SqlDataSourceRefreshSchemaForm_Title";

		// Token: 0x04000266 RID: 614
		internal const string SqlDataSourceRefreshSchemaForm_HelpLabel = "SqlDataSourceRefreshSchemaForm_HelpLabel";

		// Token: 0x04000267 RID: 615
		internal const string SqlDataSourceRefreshSchemaForm_ParametersLabel = "SqlDataSourceRefreshSchemaForm_ParametersLabel";

		// Token: 0x04000268 RID: 616
		internal const string SqlDataSourceConnectionPanel_ProviderNotFound = "SqlDataSourceConnectionPanel_ProviderNotFound";

		// Token: 0x04000269 RID: 617
		internal const string SqlDataSourceConnectionPanel_CouldNotGetConnectionSchema = "SqlDataSourceConnectionPanel_CouldNotGetConnectionSchema";

		// Token: 0x0400026A RID: 618
		internal const string SqlDataSourceSaveConfiguredConnectionPanel_HelpLabel = "SqlDataSourceSaveConfiguredConnectionPanel_HelpLabel";

		// Token: 0x0400026B RID: 619
		internal const string SqlDataSourceSaveConfiguredConnectionPanel_NameTextBoxDescription = "SqlDataSourceSaveConfiguredConnectionPanel_NameTextBoxDescription";

		// Token: 0x0400026C RID: 620
		internal const string SqlDataSourceSaveConfiguredConnectionPanel_SaveLabel = "SqlDataSourceSaveConfiguredConnectionPanel_SaveLabel";

		// Token: 0x0400026D RID: 621
		internal const string SqlDataSourceSaveConfiguredConnectionPanel_SaveCheckBox = "SqlDataSourceSaveConfiguredConnectionPanel_SaveCheckBox";

		// Token: 0x0400026E RID: 622
		internal const string SqlDataSourceSaveConfiguredConnectionPanel_PanelCaption = "SqlDataSourceSaveConfiguredConnectionPanel_PanelCaption";

		// Token: 0x0400026F RID: 623
		internal const string SqlDataSourceSaveConfiguredConnectionPanel_DuplicateName = "SqlDataSourceSaveConfiguredConnectionPanel_DuplicateName";

		// Token: 0x04000270 RID: 624
		internal const string SqlDataSourceSaveConfiguredConnectionPanel_CouldNotSaveConnection = "SqlDataSourceSaveConfiguredConnectionPanel_CouldNotSaveConnection";

		// Token: 0x04000271 RID: 625
		internal const string SqlDataSourceDataConnectionChooserPanel_PanelCaption = "SqlDataSourceDataConnectionChooserPanel_PanelCaption";

		// Token: 0x04000272 RID: 626
		internal const string SqlDataSourceDataConnectionChooserPanel_NewConnectionButton = "SqlDataSourceDataConnectionChooserPanel_NewConnectionButton";

		// Token: 0x04000273 RID: 627
		internal const string SqlDataSourceDataConnectionChooserPanel_ChooseLabel = "SqlDataSourceDataConnectionChooserPanel_ChooseLabel";

		// Token: 0x04000274 RID: 628
		internal const string SqlDataSourceDataConnectionChooserPanel_ConnectionStringLabel = "SqlDataSourceDataConnectionChooserPanel_ConnectionStringLabel";

		// Token: 0x04000275 RID: 629
		internal const string SqlDataSourceDataConnectionChooserPanel_CustomConnectionName = "SqlDataSourceDataConnectionChooserPanel_CustomConnectionName";

		// Token: 0x04000276 RID: 630
		internal const string SqlDataSourceDataConnectionChooserPanel_DetailsButtonName = "SqlDataSourceDataConnectionChooserPanel_DetailsButtonName";

		// Token: 0x04000277 RID: 631
		internal const string SqlDataSourceDataConnectionChooserPanel_DetailsButtonDesc = "SqlDataSourceDataConnectionChooserPanel_DetailsButtonDesc";

		// Token: 0x04000278 RID: 632
		internal const string SqlDataSourceQueryEditorForm_CommandLabel = "SqlDataSourceQueryEditorForm_CommandLabel";

		// Token: 0x04000279 RID: 633
		internal const string SqlDataSourceQueryEditorForm_InferParametersButton = "SqlDataSourceQueryEditorForm_InferParametersButton";

		// Token: 0x0400027A RID: 634
		internal const string SqlDataSourceQueryEditorForm_QueryBuilderButton = "SqlDataSourceQueryEditorForm_QueryBuilderButton";

		// Token: 0x0400027B RID: 635
		internal const string SqlDataSourceQueryEditorForm_Caption = "SqlDataSourceQueryEditorForm_Caption";

		// Token: 0x0400027C RID: 636
		internal const string SqlDataSourceQueryEditorForm_InferNeedsCommand = "SqlDataSourceQueryEditorForm_InferNeedsCommand";

		// Token: 0x0400027D RID: 637
		internal const string SqlDataSourceQueryEditorForm_QueryBuilderNeedsConnectionString = "SqlDataSourceQueryEditorForm_QueryBuilderNeedsConnectionString";

		// Token: 0x0400027E RID: 638
		internal const string SqlDataSourceConfigureFilterForm_ColumnLabel = "SqlDataSourceConfigureFilterForm_ColumnLabel";

		// Token: 0x0400027F RID: 639
		internal const string SqlDataSourceConfigureFilterForm_OperatorLabel = "SqlDataSourceConfigureFilterForm_OperatorLabel";

		// Token: 0x04000280 RID: 640
		internal const string SqlDataSourceConfigureFilterForm_ExpressionLabel = "SqlDataSourceConfigureFilterForm_ExpressionLabel";

		// Token: 0x04000281 RID: 641
		internal const string SqlDataSourceConfigureFilterForm_ValueLabel = "SqlDataSourceConfigureFilterForm_ValueLabel";

		// Token: 0x04000282 RID: 642
		internal const string SqlDataSourceConfigureFilterForm_ExpressionColumnHeader = "SqlDataSourceConfigureFilterForm_ExpressionColumnHeader";

		// Token: 0x04000283 RID: 643
		internal const string SqlDataSourceConfigureFilterForm_ValueColumnHeader = "SqlDataSourceConfigureFilterForm_ValueColumnHeader";

		// Token: 0x04000284 RID: 644
		internal const string SqlDataSourceConfigureFilterForm_ParameterPropertiesGroup = "SqlDataSourceConfigureFilterForm_ParameterPropertiesGroup";

		// Token: 0x04000285 RID: 645
		internal const string SqlDataSourceConfigureFilterForm_SourceLabel = "SqlDataSourceConfigureFilterForm_SourceLabel";

		// Token: 0x04000286 RID: 646
		internal const string SqlDataSourceConfigureFilterForm_WhereLabel = "SqlDataSourceConfigureFilterForm_WhereLabel";

		// Token: 0x04000287 RID: 647
		internal const string SqlDataSourceConfigureFilterForm_AddButton = "SqlDataSourceConfigureFilterForm_AddButton";

		// Token: 0x04000288 RID: 648
		internal const string SqlDataSourceConfigureFilterForm_HelpLabel = "SqlDataSourceConfigureFilterForm_HelpLabel";

		// Token: 0x04000289 RID: 649
		internal const string SqlDataSourceConfigureFilterForm_RemoveButton = "SqlDataSourceConfigureFilterForm_RemoveButton";

		// Token: 0x0400028A RID: 650
		internal const string SqlDataSourceConfigureFilterForm_Caption = "SqlDataSourceConfigureFilterForm_Caption";

		// Token: 0x0400028B RID: 651
		internal const string SqlDataSourceConfigureFilterForm_ParameterEditor_DefaultValue = "SqlDataSourceConfigureFilterForm_ParameterEditor_DefaultValue";

		// Token: 0x0400028C RID: 652
		internal const string SqlDataSourceConfigureFilterForm_StaticParameterEditor_ValueLabel = "SqlDataSourceConfigureFilterForm_StaticParameterEditor_ValueLabel";

		// Token: 0x0400028D RID: 653
		internal const string SqlDataSourceConfigureFilterForm_CookieParameterEditor_CookieNameLabel = "SqlDataSourceConfigureFilterForm_CookieParameterEditor_CookieNameLabel";

		// Token: 0x0400028E RID: 654
		internal const string SqlDataSourceConfigureFilterForm_ControlParameterEditor_ControlIDLabel = "SqlDataSourceConfigureFilterForm_ControlParameterEditor_ControlIDLabel";

		// Token: 0x0400028F RID: 655
		internal const string SqlDataSourceConfigureFilterForm_FormParameterEditor_FormFieldLabel = "SqlDataSourceConfigureFilterForm_FormParameterEditor_FormFieldLabel";

		// Token: 0x04000290 RID: 656
		internal const string SqlDataSourceConfigureFilterForm_QueryStringParameterEditor_QueryStringFieldLabel = "SqlDataSourceConfigureFilterForm_QueryStringParameterEditor_QueryStringFieldLabel";

		// Token: 0x04000291 RID: 657
		internal const string SqlDataSourceConfigureFilterForm_SessionParameterEditor_SessionFieldLabel = "SqlDataSourceConfigureFilterForm_SessionParameterEditor_SessionFieldLabel";

		// Token: 0x04000292 RID: 658
		internal const string SqlDataSourceConfigureFilterForm_ProfileParameterEditor_PropertyNameLabel = "SqlDataSourceConfigureFilterForm_ProfileParameterEditor_PropertyNameLabel";

		// Token: 0x04000293 RID: 659
		internal const string SqlDataSourceConfigureSortForm_HelpLabel = "SqlDataSourceConfigureSortForm_HelpLabel";

		// Token: 0x04000294 RID: 660
		internal const string SqlDataSourceConfigureSortForm_SortByLabel = "SqlDataSourceConfigureSortForm_SortByLabel";

		// Token: 0x04000295 RID: 661
		internal const string SqlDataSourceConfigureSortForm_ThenByLabel = "SqlDataSourceConfigureSortForm_ThenByLabel";

		// Token: 0x04000296 RID: 662
		internal const string SqlDataSourceConfigureSortForm_AscendingLabel = "SqlDataSourceConfigureSortForm_AscendingLabel";

		// Token: 0x04000297 RID: 663
		internal const string SqlDataSourceConfigureSortForm_DescendingLabel = "SqlDataSourceConfigureSortForm_DescendingLabel";

		// Token: 0x04000298 RID: 664
		internal const string SqlDataSourceConfigureSortForm_Caption = "SqlDataSourceConfigureSortForm_Caption";

		// Token: 0x04000299 RID: 665
		internal const string SqlDataSourceConfigureSortForm_SortDirection1 = "SqlDataSourceConfigureSortForm_SortDirection1";

		// Token: 0x0400029A RID: 666
		internal const string SqlDataSourceConfigureSortForm_SortDirection2 = "SqlDataSourceConfigureSortForm_SortDirection2";

		// Token: 0x0400029B RID: 667
		internal const string SqlDataSourceConfigureSortForm_SortDirection3 = "SqlDataSourceConfigureSortForm_SortDirection3";

		// Token: 0x0400029C RID: 668
		internal const string SqlDataSourceConfigureSortForm_SortColumn1 = "SqlDataSourceConfigureSortForm_SortColumn1";

		// Token: 0x0400029D RID: 669
		internal const string SqlDataSourceConfigureSortForm_SortColumn2 = "SqlDataSourceConfigureSortForm_SortColumn2";

		// Token: 0x0400029E RID: 670
		internal const string SqlDataSourceConfigureSortForm_SortColumn3 = "SqlDataSourceConfigureSortForm_SortColumn3";

		// Token: 0x0400029F RID: 671
		internal const string SqlDataSourceConfigureSortForm_SortNone = "SqlDataSourceConfigureSortForm_SortNone";

		// Token: 0x040002A0 RID: 672
		internal const string SqlDataSourceConfigureParametersPanel_PanelCaption = "SqlDataSourceConfigureParametersPanel_PanelCaption";

		// Token: 0x040002A1 RID: 673
		internal const string SqlDataSourceConfigureParametersPanel_HelpLabel = "SqlDataSourceConfigureParametersPanel_HelpLabel";

		// Token: 0x040002A2 RID: 674
		internal const string SqlDataSourceSummaryPanel_PanelCaption = "SqlDataSourceSummaryPanel_PanelCaption";

		// Token: 0x040002A3 RID: 675
		internal const string SqlDataSourceSummaryPanel_TestQueryButton = "SqlDataSourceSummaryPanel_TestQueryButton";

		// Token: 0x040002A4 RID: 676
		internal const string SqlDataSourceSummaryPanel_HelpLabel = "SqlDataSourceSummaryPanel_HelpLabel";

		// Token: 0x040002A5 RID: 677
		internal const string SqlDataSourceSummaryPanel_ResultsAccessibleName = "SqlDataSourceSummaryPanel_ResultsAccessibleName";

		// Token: 0x040002A6 RID: 678
		internal const string SqlDataSourceSummaryPanel_CouldNotCreateConnection = "SqlDataSourceSummaryPanel_CouldNotCreateConnection";

		// Token: 0x040002A7 RID: 679
		internal const string SqlDataSourceSummaryPanel_CannotExecuteQueryNoTables = "SqlDataSourceSummaryPanel_CannotExecuteQueryNoTables";

		// Token: 0x040002A8 RID: 680
		internal const string SqlDataSourceSummaryPanel_CannotExecuteQuery = "SqlDataSourceSummaryPanel_CannotExecuteQuery";

		// Token: 0x040002A9 RID: 681
		internal const string SqlDataSourceConfigureSelectPanel_PanelCaption = "SqlDataSourceConfigureSelectPanel_PanelCaption";

		// Token: 0x040002AA RID: 682
		internal const string SqlDataSourceConfigureSelectPanel_RetrieveDataLabel = "SqlDataSourceConfigureSelectPanel_RetrieveDataLabel";

		// Token: 0x040002AB RID: 683
		internal const string SqlDataSourceConfigureSelectPanel_TableLabel = "SqlDataSourceConfigureSelectPanel_TableLabel";

		// Token: 0x040002AC RID: 684
		internal const string SqlDataSourceConfigureSelectPanel_CustomSqlLabel = "SqlDataSourceConfigureSelectPanel_CustomSqlLabel";

		// Token: 0x040002AD RID: 685
		internal const string SqlDataSourceConfigureSelectPanel_TableNameLabel = "SqlDataSourceConfigureSelectPanel_TableNameLabel";

		// Token: 0x040002AE RID: 686
		internal const string SqlDataSourceConfigureSelectPanel_FieldsLabel = "SqlDataSourceConfigureSelectPanel_FieldsLabel";

		// Token: 0x040002AF RID: 687
		internal const string SqlDataSourceConfigureSelectPanel_SortButton = "SqlDataSourceConfigureSelectPanel_SortButton";

		// Token: 0x040002B0 RID: 688
		internal const string SqlDataSourceConfigureSelectPanel_FilterLabel = "SqlDataSourceConfigureSelectPanel_FilterLabel";

		// Token: 0x040002B1 RID: 689
		internal const string SqlDataSourceConfigureSelectPanel_SelectDistinctLabel = "SqlDataSourceConfigureSelectPanel_SelectDistinctLabel";

		// Token: 0x040002B2 RID: 690
		internal const string SqlDataSourceConfigureSelectPanel_AdvancedOptions = "SqlDataSourceConfigureSelectPanel_AdvancedOptions";

		// Token: 0x040002B3 RID: 691
		internal const string SqlDataSourceConfigureSelectPanel_CouldNotGetTableSchema = "SqlDataSourceConfigureSelectPanel_CouldNotGetTableSchema";

		// Token: 0x040002B4 RID: 692
		internal const string SqlDataSourceAdvancedOptionsForm_HelpLabel = "SqlDataSourceAdvancedOptionsForm_HelpLabel";

		// Token: 0x040002B5 RID: 693
		internal const string SqlDataSourceAdvancedOptionsForm_GenerateCheckBox = "SqlDataSourceAdvancedOptionsForm_GenerateCheckBox";

		// Token: 0x040002B6 RID: 694
		internal const string SqlDataSourceAdvancedOptionsForm_GenerateHelpLabel = "SqlDataSourceAdvancedOptionsForm_GenerateHelpLabel";

		// Token: 0x040002B7 RID: 695
		internal const string SqlDataSourceAdvancedOptionsForm_OptimisticCheckBox = "SqlDataSourceAdvancedOptionsForm_OptimisticCheckBox";

		// Token: 0x040002B8 RID: 696
		internal const string SqlDataSourceAdvancedOptionsForm_OptimisticLabel = "SqlDataSourceAdvancedOptionsForm_OptimisticLabel";

		// Token: 0x040002B9 RID: 697
		internal const string SqlDataSourceAdvancedOptionsForm_Caption = "SqlDataSourceAdvancedOptionsForm_Caption";

		// Token: 0x040002BA RID: 698
		internal const string SqlDataSourceCustomCommandEditor_QueryBuilderButton = "SqlDataSourceCustomCommandEditor_QueryBuilderButton";

		// Token: 0x040002BB RID: 699
		internal const string SqlDataSourceCustomCommandEditor_SqlLabel = "SqlDataSourceCustomCommandEditor_SqlLabel";

		// Token: 0x040002BC RID: 700
		internal const string SqlDataSourceCustomCommandEditor_StoredProcedureLabel = "SqlDataSourceCustomCommandEditor_StoredProcedureLabel";

		// Token: 0x040002BD RID: 701
		internal const string SqlDataSourceCustomCommandEditor_NoConnectionString = "SqlDataSourceCustomCommandEditor_NoConnectionString";

		// Token: 0x040002BE RID: 702
		internal const string SqlDataSourceCustomCommandEditor_CouldNotGetStoredProcedureSchema = "SqlDataSourceCustomCommandEditor_CouldNotGetStoredProcedureSchema";

		// Token: 0x040002BF RID: 703
		internal const string SqlDataSourceCustomCommandPanel_HelpLabel = "SqlDataSourceCustomCommandPanel_HelpLabel";

		// Token: 0x040002C0 RID: 704
		internal const string SqlDataSourceCustomCommandPanel_PanelCaption = "SqlDataSourceCustomCommandPanel_PanelCaption";

		// Token: 0x040002C1 RID: 705
		internal const string SqlDataSourceParameterValueEditorForm_HelpLabel = "SqlDataSourceParameterValueEditorForm_HelpLabel";

		// Token: 0x040002C2 RID: 706
		internal const string SqlDataSourceParameterValueEditorForm_ParametersGridAccessibleName = "SqlDataSourceParameterValueEditorForm_ParametersGridAccessibleName";

		// Token: 0x040002C3 RID: 707
		internal const string SqlDataSourceParameterValueEditorForm_Caption = "SqlDataSourceParameterValueEditorForm_Caption";

		// Token: 0x040002C4 RID: 708
		internal const string SqlDataSourceParameterValueEditorForm_DbTypeColumnHeader = "SqlDataSourceParameterValueEditorForm_DbTypeColumnHeader";

		// Token: 0x040002C5 RID: 709
		internal const string SqlDataSourceParameterValueEditorForm_ParameterColumnHeader = "SqlDataSourceParameterValueEditorForm_ParameterColumnHeader";

		// Token: 0x040002C6 RID: 710
		internal const string SqlDataSourceParameterValueEditorForm_TypeColumnHeader = "SqlDataSourceParameterValueEditorForm_TypeColumnHeader";

		// Token: 0x040002C7 RID: 711
		internal const string SqlDataSourceParameterValueEditorForm_ValueColumnHeader = "SqlDataSourceParameterValueEditorForm_ValueColumnHeader";

		// Token: 0x040002C8 RID: 712
		internal const string SqlDataSourceParameterValueEditorForm_InvalidParameter = "SqlDataSourceParameterValueEditorForm_InvalidParameter";

		// Token: 0x040002C9 RID: 713
		internal const string AccessDataSourceConnectionChooserPanel_PanelCaption = "AccessDataSourceConnectionChooserPanel_PanelCaption";

		// Token: 0x040002CA RID: 714
		internal const string AccessDataSourceConnectionChooserPanel_DataFileLabel = "AccessDataSourceConnectionChooserPanel_DataFileLabel";

		// Token: 0x040002CB RID: 715
		internal const string AccessDataSourceConnectionChooserPanel_HelpLabel = "AccessDataSourceConnectionChooserPanel_HelpLabel";

		// Token: 0x040002CC RID: 716
		internal const string AccessDataSourceConnectionChooserPanel_BrowseButton = "AccessDataSourceConnectionChooserPanel_BrowseButton";

		// Token: 0x040002CD RID: 717
		internal const string AccessDataSourceConnectionChooserPanel_FileNotFound = "AccessDataSourceConnectionChooserPanel_FileNotFound";

		// Token: 0x040002CE RID: 718
		internal const string XmlDataSourceConfigureDataSourceForm_HelpLabel = "XmlDataSourceConfigureDataSourceForm_HelpLabel";

		// Token: 0x040002CF RID: 719
		internal const string XmlDataSourceConfigureDataSourceForm_DataFileLabel = "XmlDataSourceConfigureDataSourceForm_DataFileLabel";

		// Token: 0x040002D0 RID: 720
		internal const string XmlDataSourceConfigureDataSourceForm_TransformFileLabel = "XmlDataSourceConfigureDataSourceForm_TransformFileLabel";

		// Token: 0x040002D1 RID: 721
		internal const string XmlDataSourceConfigureDataSourceForm_TransformFileHelpLabel = "XmlDataSourceConfigureDataSourceForm_TransformFileHelpLabel";

		// Token: 0x040002D2 RID: 722
		internal const string XmlDataSourceConfigureDataSourceForm_XPathExpressionLabel = "XmlDataSourceConfigureDataSourceForm_XPathExpressionLabel";

		// Token: 0x040002D3 RID: 723
		internal const string XmlDataSourceConfigureDataSourceForm_XPathExpressionHelpLabel = "XmlDataSourceConfigureDataSourceForm_XPathExpressionHelpLabel";

		// Token: 0x040002D4 RID: 724
		internal const string XmlDataSourceConfigureDataSourceForm_Browse = "XmlDataSourceConfigureDataSourceForm_Browse";

		// Token: 0x040002D5 RID: 725
		internal const string ObjectDataSourceDesigner_CannotGetSchema = "ObjectDataSourceDesigner_CannotGetSchema";

		// Token: 0x040002D6 RID: 726
		internal const string ObjectDataSourceDesigner_CannotGetType = "ObjectDataSourceDesigner_CannotGetType";

		// Token: 0x040002D7 RID: 727
		internal const string ObjectDataSource_General_MethodSignatureLabel = "ObjectDataSource_General_MethodSignatureLabel";

		// Token: 0x040002D8 RID: 728
		internal const string ObjectDataSourceConfigureParametersPanel_PanelCaption = "ObjectDataSourceConfigureParametersPanel_PanelCaption";

		// Token: 0x040002D9 RID: 729
		internal const string ObjectDataSourceConfigureParametersPanel_HelpLabel = "ObjectDataSourceConfigureParametersPanel_HelpLabel";

		// Token: 0x040002DA RID: 730
		internal const string ObjectDataSourceChooseMethodsPanel_PanelCaption = "ObjectDataSourceChooseMethodsPanel_PanelCaption";

		// Token: 0x040002DB RID: 731
		internal const string ObjectDataSourceChooseMethodsPanel_IncompatibleDataObjectTypes = "ObjectDataSourceChooseMethodsPanel_IncompatibleDataObjectTypes";

		// Token: 0x040002DC RID: 732
		internal const string ObjectDataSourceMethodEditor_DeleteHelpLabel = "ObjectDataSourceMethodEditor_DeleteHelpLabel";

		// Token: 0x040002DD RID: 733
		internal const string ObjectDataSourceMethodEditor_InsertHelpLabel = "ObjectDataSourceMethodEditor_InsertHelpLabel";

		// Token: 0x040002DE RID: 734
		internal const string ObjectDataSourceMethodEditor_SelectHelpLabel = "ObjectDataSourceMethodEditor_SelectHelpLabel";

		// Token: 0x040002DF RID: 735
		internal const string ObjectDataSourceMethodEditor_UpdateHelpLabel = "ObjectDataSourceMethodEditor_UpdateHelpLabel";

		// Token: 0x040002E0 RID: 736
		internal const string ObjectDataSourceMethodEditor_MethodLabel = "ObjectDataSourceMethodEditor_MethodLabel";

		// Token: 0x040002E1 RID: 737
		internal const string ObjectDataSourceMethodEditor_SignatureFormat = "ObjectDataSourceMethodEditor_SignatureFormat";

		// Token: 0x040002E2 RID: 738
		internal const string ObjectDataSourceMethodEditor_NoMethod = "ObjectDataSourceMethodEditor_NoMethod";

		// Token: 0x040002E3 RID: 739
		internal const string ObjectDataSourceChooseTypePanel_PanelCaption = "ObjectDataSourceChooseTypePanel_PanelCaption";

		// Token: 0x040002E4 RID: 740
		internal const string ObjectDataSourceChooseTypePanel_HelpLabel = "ObjectDataSourceChooseTypePanel_HelpLabel";

		// Token: 0x040002E5 RID: 741
		internal const string ObjectDataSourceChooseTypePanel_NameLabel = "ObjectDataSourceChooseTypePanel_NameLabel";

		// Token: 0x040002E6 RID: 742
		internal const string ObjectDataSourceChooseTypePanel_ExampleLabel = "ObjectDataSourceChooseTypePanel_ExampleLabel";

		// Token: 0x040002E7 RID: 743
		internal const string ObjectDataSourceChooseTypePanel_FilterCheckBox = "ObjectDataSourceChooseTypePanel_FilterCheckBox";

		// Token: 0x040002E8 RID: 744
		internal const string ParameterCollectionEditor_InvalidParameters = "ParameterCollectionEditor_InvalidParameters";

		// Token: 0x040002E9 RID: 745
		internal const string ParameterCollectionEditorForm_Caption = "ParameterCollectionEditorForm_Caption";

		// Token: 0x040002EA RID: 746
		internal const string ParameterEditorUserControl_ParametersLabel = "ParameterEditorUserControl_ParametersLabel";

		// Token: 0x040002EB RID: 747
		internal const string ParameterEditorUserControl_PropertiesLabel = "ParameterEditorUserControl_PropertiesLabel";

		// Token: 0x040002EC RID: 748
		internal const string ParameterEditorUserControl_AddButton = "ParameterEditorUserControl_AddButton";

		// Token: 0x040002ED RID: 749
		internal const string ParameterEditorUserControl_SourceLabel = "ParameterEditorUserControl_SourceLabel";

		// Token: 0x040002EE RID: 750
		internal const string ParameterEditorUserControl_ParameterNameColumnHeader = "ParameterEditorUserControl_ParameterNameColumnHeader";

		// Token: 0x040002EF RID: 751
		internal const string ParameterEditorUserControl_ParameterValueColumnHeader = "ParameterEditorUserControl_ParameterValueColumnHeader";

		// Token: 0x040002F0 RID: 752
		internal const string ParameterEditorUserControl_MoveParameterUp = "ParameterEditorUserControl_MoveParameterUp";

		// Token: 0x040002F1 RID: 753
		internal const string ParameterEditorUserControl_MoveParameterDown = "ParameterEditorUserControl_MoveParameterDown";

		// Token: 0x040002F2 RID: 754
		internal const string ParameterEditorUserControl_DeleteParameter = "ParameterEditorUserControl_DeleteParameter";

		// Token: 0x040002F3 RID: 755
		internal const string ParameterEditorUserControl_ControlParameterExpressionUnknown = "ParameterEditorUserControl_ControlParameterExpressionUnknown";

		// Token: 0x040002F4 RID: 756
		internal const string ParameterEditorUserControl_CookieParameterExpressionUnknown = "ParameterEditorUserControl_CookieParameterExpressionUnknown";

		// Token: 0x040002F5 RID: 757
		internal const string ParameterEditorUserControl_FormParameterExpressionUnknown = "ParameterEditorUserControl_FormParameterExpressionUnknown";

		// Token: 0x040002F6 RID: 758
		internal const string ParameterEditorUserControl_QueryStringParameterExpressionUnknown = "ParameterEditorUserControl_QueryStringParameterExpressionUnknown";

		// Token: 0x040002F7 RID: 759
		internal const string ParameterEditorUserControl_SessionParameterExpressionUnknown = "ParameterEditorUserControl_SessionParameterExpressionUnknown";

		// Token: 0x040002F8 RID: 760
		internal const string ParameterEditorUserControl_ProfileParameterExpressionUnknown = "ParameterEditorUserControl_ProfileParameterExpressionUnknown";

		// Token: 0x040002F9 RID: 761
		internal const string ParameterEditorUserControl_ShowAdvancedProperties = "ParameterEditorUserControl_ShowAdvancedProperties";

		// Token: 0x040002FA RID: 762
		internal const string ParameterEditorUserControl_HideAdvancedPropertiesLabel = "ParameterEditorUserControl_HideAdvancedPropertiesLabel";

		// Token: 0x040002FB RID: 763
		internal const string ParameterEditorUserControl_AdvancedProperties = "ParameterEditorUserControl_AdvancedProperties";

		// Token: 0x040002FC RID: 764
		internal const string ParameterEditorUserControl_ParameterDefaultValue = "ParameterEditorUserControl_ParameterDefaultValue";

		// Token: 0x040002FD RID: 765
		internal const string ParameterEditorUserControl_ControlParameterControlID = "ParameterEditorUserControl_ControlParameterControlID";

		// Token: 0x040002FE RID: 766
		internal const string ParameterEditorUserControl_CookieParameterCookieName = "ParameterEditorUserControl_CookieParameterCookieName";

		// Token: 0x040002FF RID: 767
		internal const string ParameterEditorUserControl_FormParameterFormField = "ParameterEditorUserControl_FormParameterFormField";

		// Token: 0x04000300 RID: 768
		internal const string ParameterEditorUserControl_SessionParameterSessionField = "ParameterEditorUserControl_SessionParameterSessionField";

		// Token: 0x04000301 RID: 769
		internal const string ParameterEditorUserControl_QueryStringParameterQueryStringField = "ParameterEditorUserControl_QueryStringParameterQueryStringField";

		// Token: 0x04000302 RID: 770
		internal const string ParameterEditorUserControl_ProfilePropertyName = "ParameterEditorUserControl_ProfilePropertyName";

		// Token: 0x04000303 RID: 771
		internal const string DBDlg_Text = "DBDlg_Text";

		// Token: 0x04000304 RID: 772
		internal const string DBDlg_Inst = "DBDlg_Inst";

		// Token: 0x04000305 RID: 773
		internal const string DBDlg_BindableProps = "DBDlg_BindableProps";

		// Token: 0x04000306 RID: 774
		internal const string DBDlg_ShowAll = "DBDlg_ShowAll";

		// Token: 0x04000307 RID: 775
		internal const string DBDlg_TwoWay = "DBDlg_TwoWay";

		// Token: 0x04000308 RID: 776
		internal const string DBDlg_OK = "DBDlg_OK";

		// Token: 0x04000309 RID: 777
		internal const string DBDlg_Cancel = "DBDlg_Cancel";

		// Token: 0x0400030A RID: 778
		internal const string DBDlg_Help = "DBDlg_Help";

		// Token: 0x0400030B RID: 779
		internal const string DBDlg_BindingGroup = "DBDlg_BindingGroup";

		// Token: 0x0400030C RID: 780
		internal const string DBDlg_FieldBinding = "DBDlg_FieldBinding";

		// Token: 0x0400030D RID: 781
		internal const string DBDlg_Field = "DBDlg_Field";

		// Token: 0x0400030E RID: 782
		internal const string DBDlg_Format = "DBDlg_Format";

		// Token: 0x0400030F RID: 783
		internal const string DBDlg_Sample = "DBDlg_Sample";

		// Token: 0x04000310 RID: 784
		internal const string DBDlg_CustomBinding = "DBDlg_CustomBinding";

		// Token: 0x04000311 RID: 785
		internal const string DBDlg_Expr = "DBDlg_Expr";

		// Token: 0x04000312 RID: 786
		internal const string DBDlg_RefreshSchema = "DBDlg_RefreshSchema";

		// Token: 0x04000313 RID: 787
		internal const string DBDlg_Unbound = "DBDlg_Unbound";

		// Token: 0x04000314 RID: 788
		internal const string DBDlg_Fmt_None = "DBDlg_Fmt_None";

		// Token: 0x04000315 RID: 789
		internal const string DBDlg_Fmt_General = "DBDlg_Fmt_General";

		// Token: 0x04000316 RID: 790
		internal const string DBDlg_Fmt_ShortDate = "DBDlg_Fmt_ShortDate";

		// Token: 0x04000317 RID: 791
		internal const string DBDlg_Fmt_LongDate = "DBDlg_Fmt_LongDate";

		// Token: 0x04000318 RID: 792
		internal const string DBDlg_Fmt_ShortTime = "DBDlg_Fmt_ShortTime";

		// Token: 0x04000319 RID: 793
		internal const string DBDlg_Fmt_LongTime = "DBDlg_Fmt_LongTime";

		// Token: 0x0400031A RID: 794
		internal const string DBDlg_Fmt_DateTime = "DBDlg_Fmt_DateTime";

		// Token: 0x0400031B RID: 795
		internal const string DBDlg_Fmt_FullDate = "DBDlg_Fmt_FullDate";

		// Token: 0x0400031C RID: 796
		internal const string DBDlg_Fmt_Decimal = "DBDlg_Fmt_Decimal";

		// Token: 0x0400031D RID: 797
		internal const string DBDlg_Fmt_Numeric = "DBDlg_Fmt_Numeric";

		// Token: 0x0400031E RID: 798
		internal const string DBDlg_Fmt_Fixed = "DBDlg_Fmt_Fixed";

		// Token: 0x0400031F RID: 799
		internal const string DBDlg_Fmt_Currency = "DBDlg_Fmt_Currency";

		// Token: 0x04000320 RID: 800
		internal const string DBDlg_Fmt_Scientific = "DBDlg_Fmt_Scientific";

		// Token: 0x04000321 RID: 801
		internal const string DBDlg_Fmt_Hexadecimal = "DBDlg_Fmt_Hexadecimal";

		// Token: 0x04000322 RID: 802
		internal const string DBDlg_InvalidFormat = "DBDlg_InvalidFormat";

		// Token: 0x04000323 RID: 803
		internal const string ExpressionBindingsDialog_Text = "ExpressionBindingsDialog_Text";

		// Token: 0x04000324 RID: 804
		internal const string ExpressionBindingsDialog_None = "ExpressionBindingsDialog_None";

		// Token: 0x04000325 RID: 805
		internal const string ExpressionBindingsDialog_Inst = "ExpressionBindingsDialog_Inst";

		// Token: 0x04000326 RID: 806
		internal const string ExpressionBindingsDialog_BindableProps = "ExpressionBindingsDialog_BindableProps";

		// Token: 0x04000327 RID: 807
		internal const string ExpressionBindingsDialog_OK = "ExpressionBindingsDialog_OK";

		// Token: 0x04000328 RID: 808
		internal const string ExpressionBindingsDialog_Cancel = "ExpressionBindingsDialog_Cancel";

		// Token: 0x04000329 RID: 809
		internal const string ExpressionBindingsDialog_ExpressionType = "ExpressionBindingsDialog_ExpressionType";

		// Token: 0x0400032A RID: 810
		internal const string ExpressionBindingsDialog_Properties = "ExpressionBindingsDialog_Properties";

		// Token: 0x0400032B RID: 811
		internal const string ExpressionBindingsDialog_UndefinedExpressionPrefix = "ExpressionBindingsDialog_UndefinedExpressionPrefix";

		// Token: 0x0400032C RID: 812
		internal const string ExpressionBindingsDialog_GeneratedExpression = "ExpressionBindingsDialog_GeneratedExpression";

		// Token: 0x0400032D RID: 813
		internal const string BDL_PrivateDataSource = "BDL_PrivateDataSource";

		// Token: 0x0400032E RID: 814
		internal const string BDL_PrivateDataSourceT = "BDL_PrivateDataSourceT";

		// Token: 0x0400032F RID: 815
		internal const string BDL_TemplateModePropBuilder = "BDL_TemplateModePropBuilder";

		// Token: 0x04000330 RID: 816
		internal const string BDL_PropertyBuilder = "BDL_PropertyBuilder";

		// Token: 0x04000331 RID: 817
		internal const string BDL_PropertyBuilderVerb = "BDL_PropertyBuilderVerb";

		// Token: 0x04000332 RID: 818
		internal const string BDL_PropertyBuilderDesc = "BDL_PropertyBuilderDesc";

		// Token: 0x04000333 RID: 819
		internal const string BDL_BehaviorGroup = "BDL_BehaviorGroup";

		// Token: 0x04000334 RID: 820
		internal const string BDLAF_Title = "BDLAF_Title";

		// Token: 0x04000335 RID: 821
		internal const string BDLAF_SchemeName = "BDLAF_SchemeName";

		// Token: 0x04000336 RID: 822
		internal const string BDLAF_Preview = "BDLAF_Preview";

		// Token: 0x04000337 RID: 823
		internal const string BDLAF_OK = "BDLAF_OK";

		// Token: 0x04000338 RID: 824
		internal const string BDLAF_Cancel = "BDLAF_Cancel";

		// Token: 0x04000339 RID: 825
		internal const string BDLAF_Help = "BDLAF_Help";

		// Token: 0x0400033A RID: 826
		internal const string BDLAF_Column1 = "BDLAF_Column1";

		// Token: 0x0400033B RID: 827
		internal const string BDLAF_Column2 = "BDLAF_Column2";

		// Token: 0x0400033C RID: 828
		internal const string BDLAF_Header = "BDLAF_Header";

		// Token: 0x0400033D RID: 829
		internal const string BDLAF_Footer = "BDLAF_Footer";

		// Token: 0x0400033E RID: 830
		internal const string BDLAF_Apply = "BDLAF_Apply";

		// Token: 0x0400033F RID: 831
		internal const string BDLAF_AutoFormats = "BDLAF_AutoFormats";

		// Token: 0x04000340 RID: 832
		internal const string BDLAF_Skins = "BDLAF_Skins";

		// Token: 0x04000341 RID: 833
		internal const string BDLAF_DefaultSkin = "BDLAF_DefaultSkin";

		// Token: 0x04000342 RID: 834
		internal const string BDLAF_NoSkin = "BDLAF_NoSkin";

		// Token: 0x04000343 RID: 835
		internal const string BDLAF_Couldnotgeneratepreview = "BDLAF_Couldnotgeneratepreview";

		// Token: 0x04000344 RID: 836
		internal const string BDLAF_RemoveFormatting = "BDLAF_RemoveFormatting";

		// Token: 0x04000345 RID: 837
		internal const string BDLScheme_Empty = "BDLScheme_Empty";

		// Token: 0x04000346 RID: 838
		internal const string BDLScheme_Colorful1 = "BDLScheme_Colorful1";

		// Token: 0x04000347 RID: 839
		internal const string BDLScheme_Colorful2 = "BDLScheme_Colorful2";

		// Token: 0x04000348 RID: 840
		internal const string BDLScheme_Colorful3 = "BDLScheme_Colorful3";

		// Token: 0x04000349 RID: 841
		internal const string BDLScheme_Colorful4 = "BDLScheme_Colorful4";

		// Token: 0x0400034A RID: 842
		internal const string BDLScheme_Colorful5 = "BDLScheme_Colorful5";

		// Token: 0x0400034B RID: 843
		internal const string BDLScheme_Professional1 = "BDLScheme_Professional1";

		// Token: 0x0400034C RID: 844
		internal const string BDLScheme_Professional2 = "BDLScheme_Professional2";

		// Token: 0x0400034D RID: 845
		internal const string BDLScheme_Professional3 = "BDLScheme_Professional3";

		// Token: 0x0400034E RID: 846
		internal const string BDLScheme_Simple1 = "BDLScheme_Simple1";

		// Token: 0x0400034F RID: 847
		internal const string BDLScheme_Simple2 = "BDLScheme_Simple2";

		// Token: 0x04000350 RID: 848
		internal const string BDLScheme_Simple3 = "BDLScheme_Simple3";

		// Token: 0x04000351 RID: 849
		internal const string BDLScheme_Classic1 = "BDLScheme_Classic1";

		// Token: 0x04000352 RID: 850
		internal const string BDLScheme_Classic2 = "BDLScheme_Classic2";

		// Token: 0x04000353 RID: 851
		internal const string BDLScheme_Consistent1 = "BDLScheme_Consistent1";

		// Token: 0x04000354 RID: 852
		internal const string BDLScheme_Consistent2 = "BDLScheme_Consistent2";

		// Token: 0x04000355 RID: 853
		internal const string BDLScheme_Consistent3 = "BDLScheme_Consistent3";

		// Token: 0x04000356 RID: 854
		internal const string BDLScheme_Consistent4 = "BDLScheme_Consistent4";

		// Token: 0x04000357 RID: 855
		internal const string BDLBor_Text = "BDLBor_Text";

		// Token: 0x04000358 RID: 856
		internal const string BDLBor_Desc = "BDLBor_Desc";

		// Token: 0x04000359 RID: 857
		internal const string BDLBor_CellMarginsGroup = "BDLBor_CellMarginsGroup";

		// Token: 0x0400035A RID: 858
		internal const string BDLBor_CellPadding = "BDLBor_CellPadding";

		// Token: 0x0400035B RID: 859
		internal const string BDLBor_CellSpacing = "BDLBor_CellSpacing";

		// Token: 0x0400035C RID: 860
		internal const string BDLBor_BorderLinesGroup = "BDLBor_BorderLinesGroup";

		// Token: 0x0400035D RID: 861
		internal const string BDLBor_GridLines = "BDLBor_GridLines";

		// Token: 0x0400035E RID: 862
		internal const string BDLBor_GL_Horz = "BDLBor_GL_Horz";

		// Token: 0x0400035F RID: 863
		internal const string BDLBor_GL_Vert = "BDLBor_GL_Vert";

		// Token: 0x04000360 RID: 864
		internal const string BDLBor_GL_Both = "BDLBor_GL_Both";

		// Token: 0x04000361 RID: 865
		internal const string BDLBor_GL_None = "BDLBor_GL_None";

		// Token: 0x04000362 RID: 866
		internal const string BDLBor_BorderColor = "BDLBor_BorderColor";

		// Token: 0x04000363 RID: 867
		internal const string BDLBor_BorderWidth = "BDLBor_BorderWidth";

		// Token: 0x04000364 RID: 868
		internal const string BDLBor_ChooseColorButton = "BDLBor_ChooseColorButton";

		// Token: 0x04000365 RID: 869
		internal const string BDLBor_ChooseColorDesc = "BDLBor_ChooseColorDesc";

		// Token: 0x04000366 RID: 870
		internal const string BDLBor_BorderWidthValueDesc = "BDLBor_BorderWidthValueDesc";

		// Token: 0x04000367 RID: 871
		internal const string BDLBor_BorderWidthValueName = "BDLBor_BorderWidthValueName";

		// Token: 0x04000368 RID: 872
		internal const string BDLBor_BorderWidthUnitDesc = "BDLBor_BorderWidthUnitDesc";

		// Token: 0x04000369 RID: 873
		internal const string BDLBor_BorderWidthUnitName = "BDLBor_BorderWidthUnitName";

		// Token: 0x0400036A RID: 874
		internal const string BDLFmt_Text = "BDLFmt_Text";

		// Token: 0x0400036B RID: 875
		internal const string BDLFmt_Desc = "BDLFmt_Desc";

		// Token: 0x0400036C RID: 876
		internal const string BDLFmt_Objects = "BDLFmt_Objects";

		// Token: 0x0400036D RID: 877
		internal const string BDLFmt_AppearanceGroup = "BDLFmt_AppearanceGroup";

		// Token: 0x0400036E RID: 878
		internal const string BDLFmt_ForeColor = "BDLFmt_ForeColor";

		// Token: 0x0400036F RID: 879
		internal const string BDLFmt_BackColor = "BDLFmt_BackColor";

		// Token: 0x04000370 RID: 880
		internal const string BDLFmt_FontName = "BDLFmt_FontName";

		// Token: 0x04000371 RID: 881
		internal const string BDLFmt_FontSize = "BDLFmt_FontSize";

		// Token: 0x04000372 RID: 882
		internal const string BDLFmt_FS_Smaller = "BDLFmt_FS_Smaller";

		// Token: 0x04000373 RID: 883
		internal const string BDLFmt_FS_Larger = "BDLFmt_FS_Larger";

		// Token: 0x04000374 RID: 884
		internal const string BDLFmt_FS_XXSmall = "BDLFmt_FS_XXSmall";

		// Token: 0x04000375 RID: 885
		internal const string BDLFmt_FS_XSmall = "BDLFmt_FS_XSmall";

		// Token: 0x04000376 RID: 886
		internal const string BDLFmt_FS_Small = "BDLFmt_FS_Small";

		// Token: 0x04000377 RID: 887
		internal const string BDLFmt_FS_Medium = "BDLFmt_FS_Medium";

		// Token: 0x04000378 RID: 888
		internal const string BDLFmt_FS_Large = "BDLFmt_FS_Large";

		// Token: 0x04000379 RID: 889
		internal const string BDLFmt_FS_XLarge = "BDLFmt_FS_XLarge";

		// Token: 0x0400037A RID: 890
		internal const string BDLFmt_FS_XXLarge = "BDLFmt_FS_XXLarge";

		// Token: 0x0400037B RID: 891
		internal const string BDLFmt_FS_Custom = "BDLFmt_FS_Custom";

		// Token: 0x0400037C RID: 892
		internal const string BDLFmt_FontBold = "BDLFmt_FontBold";

		// Token: 0x0400037D RID: 893
		internal const string BDLFmt_FontItalic = "BDLFmt_FontItalic";

		// Token: 0x0400037E RID: 894
		internal const string BDLFmt_FontUnderline = "BDLFmt_FontUnderline";

		// Token: 0x0400037F RID: 895
		internal const string BDLFmt_FontStrikeout = "BDLFmt_FontStrikeout";

		// Token: 0x04000380 RID: 896
		internal const string BDLFmt_FontOverline = "BDLFmt_FontOverline";

		// Token: 0x04000381 RID: 897
		internal const string BDLFmt_AlignmentGroup = "BDLFmt_AlignmentGroup";

		// Token: 0x04000382 RID: 898
		internal const string BDLFmt_HorzAlign = "BDLFmt_HorzAlign";

		// Token: 0x04000383 RID: 899
		internal const string BDLFmt_HA_Left = "BDLFmt_HA_Left";

		// Token: 0x04000384 RID: 900
		internal const string BDLFmt_HA_Center = "BDLFmt_HA_Center";

		// Token: 0x04000385 RID: 901
		internal const string BDLFmt_HA_Right = "BDLFmt_HA_Right";

		// Token: 0x04000386 RID: 902
		internal const string BDLFmt_HA_Justify = "BDLFmt_HA_Justify";

		// Token: 0x04000387 RID: 903
		internal const string BDLFmt_VertAlign = "BDLFmt_VertAlign";

		// Token: 0x04000388 RID: 904
		internal const string BDLFmt_VA_Top = "BDLFmt_VA_Top";

		// Token: 0x04000389 RID: 905
		internal const string BDLFmt_VA_Middle = "BDLFmt_VA_Middle";

		// Token: 0x0400038A RID: 906
		internal const string BDLFmt_VA_Bottom = "BDLFmt_VA_Bottom";

		// Token: 0x0400038B RID: 907
		internal const string BDLFmt_LayoutGroup = "BDLFmt_LayoutGroup";

		// Token: 0x0400038C RID: 908
		internal const string BDLFmt_Width = "BDLFmt_Width";

		// Token: 0x0400038D RID: 909
		internal const string BDLFmt_AllowWrapping = "BDLFmt_AllowWrapping";

		// Token: 0x0400038E RID: 910
		internal const string BDLFmt_Node_EntireDG = "BDLFmt_Node_EntireDG";

		// Token: 0x0400038F RID: 911
		internal const string BDLFmt_Node_EntireDL = "BDLFmt_Node_EntireDL";

		// Token: 0x04000390 RID: 912
		internal const string BDLFmt_Node_Header = "BDLFmt_Node_Header";

		// Token: 0x04000391 RID: 913
		internal const string BDLFmt_Node_Footer = "BDLFmt_Node_Footer";

		// Token: 0x04000392 RID: 914
		internal const string BDLFmt_Node_Pager = "BDLFmt_Node_Pager";

		// Token: 0x04000393 RID: 915
		internal const string BDLFmt_Node_Items = "BDLFmt_Node_Items";

		// Token: 0x04000394 RID: 916
		internal const string BDLFmt_Node_Separators = "BDLFmt_Node_Separators";

		// Token: 0x04000395 RID: 917
		internal const string BDLFmt_Node_NormalItems = "BDLFmt_Node_NormalItems";

		// Token: 0x04000396 RID: 918
		internal const string BDLFmt_Node_AltItems = "BDLFmt_Node_AltItems";

		// Token: 0x04000397 RID: 919
		internal const string BDLFmt_Node_SelItems = "BDLFmt_Node_SelItems";

		// Token: 0x04000398 RID: 920
		internal const string BDLFmt_Node_EditItems = "BDLFmt_Node_EditItems";

		// Token: 0x04000399 RID: 921
		internal const string BDLFmt_Node_Columns = "BDLFmt_Node_Columns";

		// Token: 0x0400039A RID: 922
		internal const string BDLFmt_ChooseColorButton = "BDLFmt_ChooseColorButton";

		// Token: 0x0400039B RID: 923
		internal const string BDLFmt_ChooseForeColorDesc = "BDLFmt_ChooseForeColorDesc";

		// Token: 0x0400039C RID: 924
		internal const string BDLFmt_ChooseBackColorDesc = "BDLFmt_ChooseBackColorDesc";

		// Token: 0x0400039D RID: 925
		internal const string BDLFmt_FontSizeValueDesc = "BDLFmt_FontSizeValueDesc";

		// Token: 0x0400039E RID: 926
		internal const string BDLFmt_FontSizeValueName = "BDLFmt_FontSizeValueName";

		// Token: 0x0400039F RID: 927
		internal const string BDLFmt_FontSizeUnitDesc = "BDLFmt_FontSizeUnitDesc";

		// Token: 0x040003A0 RID: 928
		internal const string BDLFmt_FontSizeUnitName = "BDLFmt_FontSizeUnitName";

		// Token: 0x040003A1 RID: 929
		internal const string BDLFmt_WidthValueDesc = "BDLFmt_WidthValueDesc";

		// Token: 0x040003A2 RID: 930
		internal const string BDLFmt_WidthValueName = "BDLFmt_WidthValueName";

		// Token: 0x040003A3 RID: 931
		internal const string BDLFmt_WidthUnitDesc = "BDLFmt_WidthUnitDesc";

		// Token: 0x040003A4 RID: 932
		internal const string BDLFmt_WidthUnitName = "BDLFmt_WidthUnitName";

		// Token: 0x040003A5 RID: 933
		internal const string CalAFmt_Title = "CalAFmt_Title";

		// Token: 0x040003A6 RID: 934
		internal const string CalAFmt_SchemeName = "CalAFmt_SchemeName";

		// Token: 0x040003A7 RID: 935
		internal const string CalAFmt_Preview = "CalAFmt_Preview";

		// Token: 0x040003A8 RID: 936
		internal const string CalAFmt_OK = "CalAFmt_OK";

		// Token: 0x040003A9 RID: 937
		internal const string CalAFmt_Cancel = "CalAFmt_Cancel";

		// Token: 0x040003AA RID: 938
		internal const string CalAFmt_Help = "CalAFmt_Help";

		// Token: 0x040003AB RID: 939
		internal const string CalAFmt_Scheme_Default = "CalAFmt_Scheme_Default";

		// Token: 0x040003AC RID: 940
		internal const string CalAFmt_Scheme_Simple = "CalAFmt_Scheme_Simple";

		// Token: 0x040003AD RID: 941
		internal const string CalAFmt_Scheme_Professional1 = "CalAFmt_Scheme_Professional1";

		// Token: 0x040003AE RID: 942
		internal const string CalAFmt_Scheme_Professional2 = "CalAFmt_Scheme_Professional2";

		// Token: 0x040003AF RID: 943
		internal const string CalAFmt_Scheme_Classic = "CalAFmt_Scheme_Classic";

		// Token: 0x040003B0 RID: 944
		internal const string CalAFmt_Scheme_Colorful1 = "CalAFmt_Scheme_Colorful1";

		// Token: 0x040003B1 RID: 945
		internal const string CalAFmt_Scheme_Colorful2 = "CalAFmt_Scheme_Colorful2";

		// Token: 0x040003B2 RID: 946
		internal const string CreateDataSource_Title = "CreateDataSource_Title";

		// Token: 0x040003B3 RID: 947
		internal const string CreateDataSource_Caption = "CreateDataSource_Caption";

		// Token: 0x040003B4 RID: 948
		internal const string CreateDataSource_Description = "CreateDataSource_Description";

		// Token: 0x040003B5 RID: 949
		internal const string CreateDataSource_SelectType = "CreateDataSource_SelectType";

		// Token: 0x040003B6 RID: 950
		internal const string CreateDataSource_SelectTypeDesc = "CreateDataSource_SelectTypeDesc";

		// Token: 0x040003B7 RID: 951
		internal const string CreateDataSource_ID = "CreateDataSource_ID";

		// Token: 0x040003B8 RID: 952
		internal const string CreateDataSource_NameNotValid = "CreateDataSource_NameNotValid";

		// Token: 0x040003B9 RID: 953
		internal const string CreateDataSource_NameNotUnique = "CreateDataSource_NameNotUnique";

		// Token: 0x040003BA RID: 954
		internal const string DataSourceIDChromeConverter_NoDataSource = "DataSourceIDChromeConverter_NoDataSource";

		// Token: 0x040003BB RID: 955
		internal const string DataSourceIDChromeConverter_NewDataSource = "DataSourceIDChromeConverter_NewDataSource";

		// Token: 0x040003BC RID: 956
		internal const string DCFAdd_Title = "DCFAdd_Title";

		// Token: 0x040003BD RID: 957
		internal const string DCFAdd_ChooseField = "DCFAdd_ChooseField";

		// Token: 0x040003BE RID: 958
		internal const string DCFAdd_HeaderText = "DCFAdd_HeaderText";

		// Token: 0x040003BF RID: 959
		internal const string DCFAdd_DataField = "DCFAdd_DataField";

		// Token: 0x040003C0 RID: 960
		internal const string DCFAdd_ButtonType = "DCFAdd_ButtonType";

		// Token: 0x040003C1 RID: 961
		internal const string DCFAdd_CommandName = "DCFAdd_CommandName";

		// Token: 0x040003C2 RID: 962
		internal const string DCFAdd_Text = "DCFAdd_Text";

		// Token: 0x040003C3 RID: 963
		internal const string DCFAdd_CommandButtons = "DCFAdd_CommandButtons";

		// Token: 0x040003C4 RID: 964
		internal const string DCFAdd_EditUpdate = "DCFAdd_EditUpdate";

		// Token: 0x040003C5 RID: 965
		internal const string DCFAdd_Delete = "DCFAdd_Delete";

		// Token: 0x040003C6 RID: 966
		internal const string DCFAdd_NewInsert = "DCFAdd_NewInsert";

		// Token: 0x040003C7 RID: 967
		internal const string DCFAdd_Select = "DCFAdd_Select";

		// Token: 0x040003C8 RID: 968
		internal const string DCFAdd_ShowCancel = "DCFAdd_ShowCancel";

		// Token: 0x040003C9 RID: 969
		internal const string DCFAdd_DeleteDesc = "DCFAdd_DeleteDesc";

		// Token: 0x040003CA RID: 970
		internal const string DCFAdd_SelectDesc = "DCFAdd_SelectDesc";

		// Token: 0x040003CB RID: 971
		internal const string DCFAdd_ShowCancelDesc = "DCFAdd_ShowCancelDesc";

		// Token: 0x040003CC RID: 972
		internal const string DCFAdd_EditUpdateDesc = "DCFAdd_EditUpdateDesc";

		// Token: 0x040003CD RID: 973
		internal const string DCFAdd_NewInsertDesc = "DCFAdd_NewInsertDesc";

		// Token: 0x040003CE RID: 974
		internal const string DCFAdd_ReadOnly = "DCFAdd_ReadOnly";

		// Token: 0x040003CF RID: 975
		internal const string DCFAdd_ImageMode = "DCFAdd_ImageMode";

		// Token: 0x040003D0 RID: 976
		internal const string DCFAdd_DataMode = "DCFAdd_DataMode";

		// Token: 0x040003D1 RID: 977
		internal const string DCFAdd_LinkMode = "DCFAdd_LinkMode";

		// Token: 0x040003D2 RID: 978
		internal const string DCFAdd_LinkFormatString = "DCFAdd_LinkFormatString";

		// Token: 0x040003D3 RID: 979
		internal const string DCFAdd_ExampleFormatString = "DCFAdd_ExampleFormatString";

		// Token: 0x040003D4 RID: 980
		internal const string DCFAdd_HyperlinkText = "DCFAdd_HyperlinkText";

		// Token: 0x040003D5 RID: 981
		internal const string DCFAdd_HyperlinkURL = "DCFAdd_HyperlinkURL";

		// Token: 0x040003D6 RID: 982
		internal const string DCFAdd_SpecifyText = "DCFAdd_SpecifyText";

		// Token: 0x040003D7 RID: 983
		internal const string DCFAdd_BindText = "DCFAdd_BindText";

		// Token: 0x040003D8 RID: 984
		internal const string DCFAdd_TextFormatString = "DCFAdd_TextFormatString";

		// Token: 0x040003D9 RID: 985
		internal const string DCFAdd_TextFormatStringExample = "DCFAdd_TextFormatStringExample";

		// Token: 0x040003DA RID: 986
		internal const string DCFAdd_SpecifyURL = "DCFAdd_SpecifyURL";

		// Token: 0x040003DB RID: 987
		internal const string DCFAdd_BindURL = "DCFAdd_BindURL";

		// Token: 0x040003DC RID: 988
		internal const string DCFAdd_URLFormatString = "DCFAdd_URLFormatString";

		// Token: 0x040003DD RID: 989
		internal const string DCFAdd_URLFormatStringExample = "DCFAdd_URLFormatStringExample";

		// Token: 0x040003DE RID: 990
		internal const string DCFEditor_Text = "DCFEditor_Text";

		// Token: 0x040003DF RID: 991
		internal const string DCFEditor_AutoGen = "DCFEditor_AutoGen";

		// Token: 0x040003E0 RID: 992
		internal const string DCFEditor_AvailableFields = "DCFEditor_AvailableFields";

		// Token: 0x040003E1 RID: 993
		internal const string DCFEditor_SelectedFields = "DCFEditor_SelectedFields";

		// Token: 0x040003E2 RID: 994
		internal const string DCFEditor_FieldProps = "DCFEditor_FieldProps";

		// Token: 0x040003E3 RID: 995
		internal const string DCFEditor_FieldPropsFormat = "DCFEditor_FieldPropsFormat";

		// Token: 0x040003E4 RID: 996
		internal const string DCFEditor_Add = "DCFEditor_Add";

		// Token: 0x040003E5 RID: 997
		internal const string DCFEditor_MoveFieldUpName = "DCFEditor_MoveFieldUpName";

		// Token: 0x040003E6 RID: 998
		internal const string DCFEditor_MoveFieldDownName = "DCFEditor_MoveFieldDownName";

		// Token: 0x040003E7 RID: 999
		internal const string DCFEditor_DeleteFieldName = "DCFEditor_DeleteFieldName";

		// Token: 0x040003E8 RID: 1000
		internal const string DCFEditor_MoveFieldUpDesc = "DCFEditor_MoveFieldUpDesc";

		// Token: 0x040003E9 RID: 1001
		internal const string DCFEditor_MoveFieldDownDesc = "DCFEditor_MoveFieldDownDesc";

		// Token: 0x040003EA RID: 1002
		internal const string DCFEditor_DeleteFieldDesc = "DCFEditor_DeleteFieldDesc";

		// Token: 0x040003EB RID: 1003
		internal const string DCFEditor_Templatize = "DCFEditor_Templatize";

		// Token: 0x040003EC RID: 1004
		internal const string DCFEditor_Node_AllFields = "DCFEditor_Node_AllFields";

		// Token: 0x040003ED RID: 1005
		internal const string DCFEditor_Node_Bound = "DCFEditor_Node_Bound";

		// Token: 0x040003EE RID: 1006
		internal const string DCFEditor_Node_Button = "DCFEditor_Node_Button";

		// Token: 0x040003EF RID: 1007
		internal const string DCFEditor_Node_Command = "DCFEditor_Node_Command";

		// Token: 0x040003F0 RID: 1008
		internal const string DCFEditor_Node_CheckBox = "DCFEditor_Node_CheckBox";

		// Token: 0x040003F1 RID: 1009
		internal const string DCFEditor_Node_HyperLink = "DCFEditor_Node_HyperLink";

		// Token: 0x040003F2 RID: 1010
		internal const string DCFEditor_Node_Template = "DCFEditor_Node_Template";

		// Token: 0x040003F3 RID: 1011
		internal const string DCFEditor_Node_Select = "DCFEditor_Node_Select";

		// Token: 0x040003F4 RID: 1012
		internal const string DCFEditor_Node_Edit = "DCFEditor_Node_Edit";

		// Token: 0x040003F5 RID: 1013
		internal const string DCFEditor_Node_Insert = "DCFEditor_Node_Insert";

		// Token: 0x040003F6 RID: 1014
		internal const string DCFEditor_Node_Delete = "DCFEditor_Node_Delete";

		// Token: 0x040003F7 RID: 1015
		internal const string DCFEditor_Node_Image = "DCFEditor_Node_Image";

		// Token: 0x040003F8 RID: 1016
		internal const string DCFEditor_Button = "DCFEditor_Button";

		// Token: 0x040003F9 RID: 1017
		internal const string DCFEditor_HyperLink = "DCFEditor_HyperLink";

		// Token: 0x040003FA RID: 1018
		internal const string DesignTimeSiteMapProvider_RootNodeText = "DesignTimeSiteMapProvider_RootNodeText";

		// Token: 0x040003FB RID: 1019
		internal const string DesignTimeSiteMapProvider_ParentNodeText = "DesignTimeSiteMapProvider_ParentNodeText";

		// Token: 0x040003FC RID: 1020
		internal const string DesignTimeSiteMapProvider_SiblingNodeText = "DesignTimeSiteMapProvider_SiblingNodeText";

		// Token: 0x040003FD RID: 1021
		internal const string DesignTimeSiteMapProvider_CurrentNodeText = "DesignTimeSiteMapProvider_CurrentNodeText";

		// Token: 0x040003FE RID: 1022
		internal const string DesignTimeSiteMapProvider_ChildNodeText = "DesignTimeSiteMapProvider_ChildNodeText";

		// Token: 0x040003FF RID: 1023
		internal const string DesignTimeSiteMapProvider_Duplicate_Url = "DesignTimeSiteMapProvider_Duplicate_Url";

		// Token: 0x04000400 RID: 1024
		internal const string DGGen_Text = "DGGen_Text";

		// Token: 0x04000401 RID: 1025
		internal const string DGGen_Desc = "DGGen_Desc";

		// Token: 0x04000402 RID: 1026
		internal const string DGGen_DataGroup = "DGGen_DataGroup";

		// Token: 0x04000403 RID: 1027
		internal const string DGGen_DataSource = "DGGen_DataSource";

		// Token: 0x04000404 RID: 1028
		internal const string DGGen_DataMember = "DGGen_DataMember";

		// Token: 0x04000405 RID: 1029
		internal const string DGGen_DSUnbound = "DGGen_DSUnbound";

		// Token: 0x04000406 RID: 1030
		internal const string DGGen_DataKey = "DGGen_DataKey";

		// Token: 0x04000407 RID: 1031
		internal const string DGGen_DKNone = "DGGen_DKNone";

		// Token: 0x04000408 RID: 1032
		internal const string DGGen_DMNone = "DGGen_DMNone";

		// Token: 0x04000409 RID: 1033
		internal const string DGGen_HeaderFooterGroup = "DGGen_HeaderFooterGroup";

		// Token: 0x0400040A RID: 1034
		internal const string DGGen_ShowHeader = "DGGen_ShowHeader";

		// Token: 0x0400040B RID: 1035
		internal const string DGGen_ShowFooter = "DGGen_ShowFooter";

		// Token: 0x0400040C RID: 1036
		internal const string DGGen_BehaviorGroup = "DGGen_BehaviorGroup";

		// Token: 0x0400040D RID: 1037
		internal const string DGGen_AllowSorting = "DGGen_AllowSorting";

		// Token: 0x0400040E RID: 1038
		internal const string DGGen_AutoColumnInfo = "DGGen_AutoColumnInfo";

		// Token: 0x0400040F RID: 1039
		internal const string DGGen_CustomColumnInfo = "DGGen_CustomColumnInfo";

		// Token: 0x04000410 RID: 1040
		internal const string DGPg_Text = "DGPg_Text";

		// Token: 0x04000411 RID: 1041
		internal const string DGPg_Desc = "DGPg_Desc";

		// Token: 0x04000412 RID: 1042
		internal const string DGPg_PagingGroup = "DGPg_PagingGroup";

		// Token: 0x04000413 RID: 1043
		internal const string DGPg_AllowPaging = "DGPg_AllowPaging";

		// Token: 0x04000414 RID: 1044
		internal const string DGPg_AllowCustomPaging = "DGPg_AllowCustomPaging";

		// Token: 0x04000415 RID: 1045
		internal const string DGPg_PageSize = "DGPg_PageSize";

		// Token: 0x04000416 RID: 1046
		internal const string DGPg_Rows = "DGPg_Rows";

		// Token: 0x04000417 RID: 1047
		internal const string DGPg_NavigationGroup = "DGPg_NavigationGroup";

		// Token: 0x04000418 RID: 1048
		internal const string DGPg_Visible = "DGPg_Visible";

		// Token: 0x04000419 RID: 1049
		internal const string DGPg_Position = "DGPg_Position";

		// Token: 0x0400041A RID: 1050
		internal const string DGPg_Pos_Top = "DGPg_Pos_Top";

		// Token: 0x0400041B RID: 1051
		internal const string DGPg_Pos_Bottom = "DGPg_Pos_Bottom";

		// Token: 0x0400041C RID: 1052
		internal const string DGPg_Pos_TopBottom = "DGPg_Pos_TopBottom";

		// Token: 0x0400041D RID: 1053
		internal const string DGPg_Mode = "DGPg_Mode";

		// Token: 0x0400041E RID: 1054
		internal const string DGPg_Mode_Buttons = "DGPg_Mode_Buttons";

		// Token: 0x0400041F RID: 1055
		internal const string DGPg_Mode_Numbers = "DGPg_Mode_Numbers";

		// Token: 0x04000420 RID: 1056
		internal const string DGPg_NextPage = "DGPg_NextPage";

		// Token: 0x04000421 RID: 1057
		internal const string DGPg_PrevPage = "DGPg_PrevPage";

		// Token: 0x04000422 RID: 1058
		internal const string DGPg_ButtonCount = "DGPg_ButtonCount";

		// Token: 0x04000423 RID: 1059
		internal const string DGCol_Text = "DGCol_Text";

		// Token: 0x04000424 RID: 1060
		internal const string DGCol_Desc = "DGCol_Desc";

		// Token: 0x04000425 RID: 1061
		internal const string DGCol_AutoGen = "DGCol_AutoGen";

		// Token: 0x04000426 RID: 1062
		internal const string DGCol_ColListGroup = "DGCol_ColListGroup";

		// Token: 0x04000427 RID: 1063
		internal const string DGCol_AvailableCols = "DGCol_AvailableCols";

		// Token: 0x04000428 RID: 1064
		internal const string DGCol_SelectedCols = "DGCol_SelectedCols";

		// Token: 0x04000429 RID: 1065
		internal const string DGCol_ColumnPropsGroup1 = "DGCol_ColumnPropsGroup1";

		// Token: 0x0400042A RID: 1066
		internal const string DGCol_ColumnPropsGroup2 = "DGCol_ColumnPropsGroup2";

		// Token: 0x0400042B RID: 1067
		internal const string DGCol_HeaderText = "DGCol_HeaderText";

		// Token: 0x0400042C RID: 1068
		internal const string DGCol_HeaderImage = "DGCol_HeaderImage";

		// Token: 0x0400042D RID: 1069
		internal const string DGCol_FooterText = "DGCol_FooterText";

		// Token: 0x0400042E RID: 1070
		internal const string DGCol_SortExpr = "DGCol_SortExpr";

		// Token: 0x0400042F RID: 1071
		internal const string DGCol_Visible = "DGCol_Visible";

		// Token: 0x04000430 RID: 1072
		internal const string DGCol_Templatize = "DGCol_Templatize";

		// Token: 0x04000431 RID: 1073
		internal const string DGCol_Node = "DGCol_Node";

		// Token: 0x04000432 RID: 1074
		internal const string DGCol_Node_DataFields = "DGCol_Node_DataFields";

		// Token: 0x04000433 RID: 1075
		internal const string DGCol_Node_AllFields = "DGCol_Node_AllFields";

		// Token: 0x04000434 RID: 1076
		internal const string DGCol_Node_Bound = "DGCol_Node_Bound";

		// Token: 0x04000435 RID: 1077
		internal const string DGCol_Node_Button = "DGCol_Node_Button";

		// Token: 0x04000436 RID: 1078
		internal const string DGCol_Node_Select = "DGCol_Node_Select";

		// Token: 0x04000437 RID: 1079
		internal const string DGCol_Node_Edit = "DGCol_Node_Edit";

		// Token: 0x04000438 RID: 1080
		internal const string DGCol_Node_Delete = "DGCol_Node_Delete";

		// Token: 0x04000439 RID: 1081
		internal const string DGCol_Node_HyperLink = "DGCol_Node_HyperLink";

		// Token: 0x0400043A RID: 1082
		internal const string DGCol_Node_Template = "DGCol_Node_Template";

		// Token: 0x0400043B RID: 1083
		internal const string DGCol_DFC_DataField = "DGCol_DFC_DataField";

		// Token: 0x0400043C RID: 1084
		internal const string DGCol_DFC_DataFormat = "DGCol_DFC_DataFormat";

		// Token: 0x0400043D RID: 1085
		internal const string DGCol_DFC_ReadOnly = "DGCol_DFC_ReadOnly";

		// Token: 0x0400043E RID: 1086
		internal const string DGCol_BC_Text = "DGCol_BC_Text";

		// Token: 0x0400043F RID: 1087
		internal const string DGCol_BC_DataTextField = "DGCol_BC_DataTextField";

		// Token: 0x04000440 RID: 1088
		internal const string DGCol_BC_DataTextFormat = "DGCol_BC_DataTextFormat";

		// Token: 0x04000441 RID: 1089
		internal const string DGCol_BC_Command = "DGCol_BC_Command";

		// Token: 0x04000442 RID: 1090
		internal const string DGCol_BC_ButtonType = "DGCol_BC_ButtonType";

		// Token: 0x04000443 RID: 1091
		internal const string DGCol_BC_BT_Link = "DGCol_BC_BT_Link";

		// Token: 0x04000444 RID: 1092
		internal const string DGCol_BC_BT_Push = "DGCol_BC_BT_Push";

		// Token: 0x04000445 RID: 1093
		internal const string DGCol_HC_Text = "DGCol_HC_Text";

		// Token: 0x04000446 RID: 1094
		internal const string DGCol_HC_DataTextField = "DGCol_HC_DataTextField";

		// Token: 0x04000447 RID: 1095
		internal const string DGCol_HC_DataTextFormat = "DGCol_HC_DataTextFormat";

		// Token: 0x04000448 RID: 1096
		internal const string DGCol_HC_URL = "DGCol_HC_URL";

		// Token: 0x04000449 RID: 1097
		internal const string DGCol_HC_DataURLField = "DGCol_HC_DataURLField";

		// Token: 0x0400044A RID: 1098
		internal const string DGCol_HC_DataURLFormat = "DGCol_HC_DataURLFormat";

		// Token: 0x0400044B RID: 1099
		internal const string DGCol_HC_Target = "DGCol_HC_Target";

		// Token: 0x0400044C RID: 1100
		internal const string DGCol_EC_Edit = "DGCol_EC_Edit";

		// Token: 0x0400044D RID: 1101
		internal const string DGCol_EC_Update = "DGCol_EC_Update";

		// Token: 0x0400044E RID: 1102
		internal const string DGCol_EC_Cancel = "DGCol_EC_Cancel";

		// Token: 0x0400044F RID: 1103
		internal const string DGCol_EC_ButtonType = "DGCol_EC_ButtonType";

		// Token: 0x04000450 RID: 1104
		internal const string DGCol_EC_BT_Link = "DGCol_EC_BT_Link";

		// Token: 0x04000451 RID: 1105
		internal const string DGCol_EC_BT_Push = "DGCol_EC_BT_Push";

		// Token: 0x04000452 RID: 1106
		internal const string DGCol_Button = "DGCol_Button";

		// Token: 0x04000453 RID: 1107
		internal const string DGCol_SelectButton = "DGCol_SelectButton";

		// Token: 0x04000454 RID: 1108
		internal const string DGCol_DeleteButton = "DGCol_DeleteButton";

		// Token: 0x04000455 RID: 1109
		internal const string DGCol_EditButton = "DGCol_EditButton";

		// Token: 0x04000456 RID: 1110
		internal const string DGCol_UpdateButton = "DGCol_UpdateButton";

		// Token: 0x04000457 RID: 1111
		internal const string DGCol_CancelButton = "DGCol_CancelButton";

		// Token: 0x04000458 RID: 1112
		internal const string DGCol_HyperLink = "DGCol_HyperLink";

		// Token: 0x04000459 RID: 1113
		internal const string DGCol_URLPFilter = "DGCol_URLPFilter";

		// Token: 0x0400045A RID: 1114
		internal const string DGCol_URLPCaption = "DGCol_URLPCaption";

		// Token: 0x0400045B RID: 1115
		internal const string DGCol_AddColButtonDesc = "DGCol_AddColButtonDesc";

		// Token: 0x0400045C RID: 1116
		internal const string DGCol_MoveColumnUpButtonDesc = "DGCol_MoveColumnUpButtonDesc";

		// Token: 0x0400045D RID: 1117
		internal const string DGCol_MoveColumnDownButtonDesc = "DGCol_MoveColumnDownButtonDesc";

		// Token: 0x0400045E RID: 1118
		internal const string DGCol_DeleteColumnButtonDesc = "DGCol_DeleteColumnButtonDesc";

		// Token: 0x0400045F RID: 1119
		internal const string DGCol_HeaderImagePickerDesc = "DGCol_HeaderImagePickerDesc";

		// Token: 0x04000460 RID: 1120
		internal const string DataList_NoTemplatesInst = "DataList_NoTemplatesInst";

		// Token: 0x04000461 RID: 1121
		internal const string DataList_NoTemplatesInst2 = "DataList_NoTemplatesInst2";

		// Token: 0x04000462 RID: 1122
		internal const string DataList_HeaderFooterTemplates = "DataList_HeaderFooterTemplates";

		// Token: 0x04000463 RID: 1123
		internal const string DataList_ItemTemplates = "DataList_ItemTemplates";

		// Token: 0x04000464 RID: 1124
		internal const string DataList_SeparatorTemplate = "DataList_SeparatorTemplate";

		// Token: 0x04000465 RID: 1125
		internal const string DataList_RefreshSchemaTransaction = "DataList_RefreshSchemaTransaction";

		// Token: 0x04000466 RID: 1126
		internal const string DataList_RegenerateTemplates = "DataList_RegenerateTemplates";

		// Token: 0x04000467 RID: 1127
		internal const string DataList_ClearTemplates = "DataList_ClearTemplates";

		// Token: 0x04000468 RID: 1128
		internal const string DataList_ClearTemplatesCaption = "DataList_ClearTemplatesCaption";

		// Token: 0x04000469 RID: 1129
		internal const string DLGen_Text = "DLGen_Text";

		// Token: 0x0400046A RID: 1130
		internal const string DLGen_Desc = "DLGen_Desc";

		// Token: 0x0400046B RID: 1131
		internal const string DLGen_DataGroup = "DLGen_DataGroup";

		// Token: 0x0400046C RID: 1132
		internal const string DLGen_DataSource = "DLGen_DataSource";

		// Token: 0x0400046D RID: 1133
		internal const string DLGen_DataMember = "DLGen_DataMember";

		// Token: 0x0400046E RID: 1134
		internal const string DLGen_DSUnbound = "DLGen_DSUnbound";

		// Token: 0x0400046F RID: 1135
		internal const string DLGen_DataKey = "DLGen_DataKey";

		// Token: 0x04000470 RID: 1136
		internal const string DLGen_DKNone = "DLGen_DKNone";

		// Token: 0x04000471 RID: 1137
		internal const string DLGen_DMNone = "DLGen_DMNone";

		// Token: 0x04000472 RID: 1138
		internal const string DLGen_HeaderFooterGroup = "DLGen_HeaderFooterGroup";

		// Token: 0x04000473 RID: 1139
		internal const string DLGen_ShowHeader = "DLGen_ShowHeader";

		// Token: 0x04000474 RID: 1140
		internal const string DLGen_ShowFooter = "DLGen_ShowFooter";

		// Token: 0x04000475 RID: 1141
		internal const string DLGen_RepeatLayoutGroup = "DLGen_RepeatLayoutGroup";

		// Token: 0x04000476 RID: 1142
		internal const string DLGen_RepeatColumns = "DLGen_RepeatColumns";

		// Token: 0x04000477 RID: 1143
		internal const string DLGen_RepeatDirection = "DLGen_RepeatDirection";

		// Token: 0x04000478 RID: 1144
		internal const string DLGen_RD_Horz = "DLGen_RD_Horz";

		// Token: 0x04000479 RID: 1145
		internal const string DLGen_RD_Vert = "DLGen_RD_Vert";

		// Token: 0x0400047A RID: 1146
		internal const string DLGen_RepeatLayout = "DLGen_RepeatLayout";

		// Token: 0x0400047B RID: 1147
		internal const string DLGen_RL_Table = "DLGen_RL_Table";

		// Token: 0x0400047C RID: 1148
		internal const string DLGen_RL_Flow = "DLGen_RL_Flow";

		// Token: 0x0400047D RID: 1149
		internal const string DLGen_ExtractRows = "DLGen_ExtractRows";

		// Token: 0x0400047E RID: 1150
		internal const string DLGen_Templates = "DLGen_Templates";

		// Token: 0x0400047F RID: 1151
		internal const string DVScheme_Empty = "DVScheme_Empty";

		// Token: 0x04000480 RID: 1152
		internal const string DVScheme_Colorful1 = "DVScheme_Colorful1";

		// Token: 0x04000481 RID: 1153
		internal const string DVScheme_Colorful2 = "DVScheme_Colorful2";

		// Token: 0x04000482 RID: 1154
		internal const string DVScheme_Colorful3 = "DVScheme_Colorful3";

		// Token: 0x04000483 RID: 1155
		internal const string DVScheme_Colorful4 = "DVScheme_Colorful4";

		// Token: 0x04000484 RID: 1156
		internal const string DVScheme_Colorful5 = "DVScheme_Colorful5";

		// Token: 0x04000485 RID: 1157
		internal const string DVScheme_Professional1 = "DVScheme_Professional1";

		// Token: 0x04000486 RID: 1158
		internal const string DVScheme_Professional2 = "DVScheme_Professional2";

		// Token: 0x04000487 RID: 1159
		internal const string DVScheme_Professional3 = "DVScheme_Professional3";

		// Token: 0x04000488 RID: 1160
		internal const string DVScheme_Simple1 = "DVScheme_Simple1";

		// Token: 0x04000489 RID: 1161
		internal const string DVScheme_Simple2 = "DVScheme_Simple2";

		// Token: 0x0400048A RID: 1162
		internal const string DVScheme_Simple3 = "DVScheme_Simple3";

		// Token: 0x0400048B RID: 1163
		internal const string DVScheme_Classic1 = "DVScheme_Classic1";

		// Token: 0x0400048C RID: 1164
		internal const string DVScheme_Classic2 = "DVScheme_Classic2";

		// Token: 0x0400048D RID: 1165
		internal const string DVScheme_Consistent1 = "DVScheme_Consistent1";

		// Token: 0x0400048E RID: 1166
		internal const string DVScheme_Consistent2 = "DVScheme_Consistent2";

		// Token: 0x0400048F RID: 1167
		internal const string DVScheme_Consistent3 = "DVScheme_Consistent3";

		// Token: 0x04000490 RID: 1168
		internal const string DVScheme_Consistent4 = "DVScheme_Consistent4";

		// Token: 0x04000491 RID: 1169
		internal const string FVScheme_Empty = "FVScheme_Empty";

		// Token: 0x04000492 RID: 1170
		internal const string FVScheme_Colorful1 = "FVScheme_Colorful1";

		// Token: 0x04000493 RID: 1171
		internal const string FVScheme_Colorful2 = "FVScheme_Colorful2";

		// Token: 0x04000494 RID: 1172
		internal const string FVScheme_Colorful3 = "FVScheme_Colorful3";

		// Token: 0x04000495 RID: 1173
		internal const string FVScheme_Colorful4 = "FVScheme_Colorful4";

		// Token: 0x04000496 RID: 1174
		internal const string FVScheme_Colorful5 = "FVScheme_Colorful5";

		// Token: 0x04000497 RID: 1175
		internal const string FVScheme_Professional1 = "FVScheme_Professional1";

		// Token: 0x04000498 RID: 1176
		internal const string FVScheme_Professional2 = "FVScheme_Professional2";

		// Token: 0x04000499 RID: 1177
		internal const string FVScheme_Professional3 = "FVScheme_Professional3";

		// Token: 0x0400049A RID: 1178
		internal const string FVScheme_Simple1 = "FVScheme_Simple1";

		// Token: 0x0400049B RID: 1179
		internal const string FVScheme_Simple2 = "FVScheme_Simple2";

		// Token: 0x0400049C RID: 1180
		internal const string FVScheme_Simple3 = "FVScheme_Simple3";

		// Token: 0x0400049D RID: 1181
		internal const string FVScheme_Classic1 = "FVScheme_Classic1";

		// Token: 0x0400049E RID: 1182
		internal const string FVScheme_Classic2 = "FVScheme_Classic2";

		// Token: 0x0400049F RID: 1183
		internal const string FVScheme_Consistent1 = "FVScheme_Consistent1";

		// Token: 0x040004A0 RID: 1184
		internal const string FVScheme_Consistent2 = "FVScheme_Consistent2";

		// Token: 0x040004A1 RID: 1185
		internal const string FVScheme_Consistent3 = "FVScheme_Consistent3";

		// Token: 0x040004A2 RID: 1186
		internal const string FVScheme_Consistent4 = "FVScheme_Consistent4";

		// Token: 0x040004A3 RID: 1187
		internal const string Repeater_NoTemplatesInst = "Repeater_NoTemplatesInst";

		// Token: 0x040004A4 RID: 1188
		internal const string BaseDataBoundControl_CreateDataSourceTransaction = "BaseDataBoundControl_CreateDataSourceTransaction";

		// Token: 0x040004A5 RID: 1189
		internal const string BaseDataBoundControl_ConfigureDataVerb = "BaseDataBoundControl_ConfigureDataVerb";

		// Token: 0x040004A6 RID: 1190
		internal const string BaseDataBoundControl_ConfigureDataVerbDesc = "BaseDataBoundControl_ConfigureDataVerbDesc";

		// Token: 0x040004A7 RID: 1191
		internal const string BaseDataBoundControl_DataActionGroup = "BaseDataBoundControl_DataActionGroup";

		// Token: 0x040004A8 RID: 1192
		internal const string ExpressionEditor_ExpressionBound = "ExpressionEditor_ExpressionBound";

		// Token: 0x040004A9 RID: 1193
		internal const string AppSettingExpressionEditor_AppSetting = "AppSettingExpressionEditor_AppSetting";

		// Token: 0x040004AA RID: 1194
		internal const string ConnectionStringsExpressionEditor_ConnectionName = "ConnectionStringsExpressionEditor_ConnectionName";

		// Token: 0x040004AB RID: 1195
		internal const string ConnectionStringsExpressionEditor_ConnectionType = "ConnectionStringsExpressionEditor_ConnectionType";

		// Token: 0x040004AC RID: 1196
		internal const string ExpressionEditor_Expression = "ExpressionEditor_Expression";

		// Token: 0x040004AD RID: 1197
		internal const string ResourceExpressionEditorSheet_ClassKey = "ResourceExpressionEditorSheet_ClassKey";

		// Token: 0x040004AE RID: 1198
		internal const string ResourceExpressionEditorSheet_ResourceKey = "ResourceExpressionEditorSheet_ResourceKey";

		// Token: 0x040004AF RID: 1199
		internal const string ResourceExpressionEditorSheet_InvalidResourceKey = "ResourceExpressionEditorSheet_InvalidResourceKey";

		// Token: 0x040004B0 RID: 1200
		internal const string ControlDesigner_WndProcException = "ControlDesigner_WndProcException";

		// Token: 0x040004B1 RID: 1201
		internal const string DataBoundControl_SchemaRefreshedWarning = "DataBoundControl_SchemaRefreshedWarning";

		// Token: 0x040004B2 RID: 1202
		internal const string DataBoundControl_SchemaRefreshedWarningNoDataSource = "DataBoundControl_SchemaRefreshedWarningNoDataSource";

		// Token: 0x040004B3 RID: 1203
		internal const string DataBoundControl_SchemaRefreshedCaption = "DataBoundControl_SchemaRefreshedCaption";

		// Token: 0x040004B4 RID: 1204
		internal const string DataBoundControl_GridView = "DataBoundControl_GridView";

		// Token: 0x040004B5 RID: 1205
		internal const string DataBoundControl_DetailsView = "DataBoundControl_DetailsView";

		// Token: 0x040004B6 RID: 1206
		internal const string DataBoundControl_FormView = "DataBoundControl_FormView";

		// Token: 0x040004B7 RID: 1207
		internal const string DataBoundControl_Column = "DataBoundControl_Column";

		// Token: 0x040004B8 RID: 1208
		internal const string DataBoundControl_Row = "DataBoundControl_Row";

		// Token: 0x040004B9 RID: 1209
		internal const string DataBoundControlActionList_SetDataSourceIDTransaction = "DataBoundControlActionList_SetDataSourceIDTransaction";

		// Token: 0x040004BA RID: 1210
		internal const string GridView_EditFieldsTransaction = "GridView_EditFieldsTransaction";

		// Token: 0x040004BB RID: 1211
		internal const string GridView_AddNewFieldTransaction = "GridView_AddNewFieldTransaction";

		// Token: 0x040004BC RID: 1212
		internal const string GridView_EnableEditingTransaction = "GridView_EnableEditingTransaction";

		// Token: 0x040004BD RID: 1213
		internal const string GridView_EnableDeletingTransaction = "GridView_EnableDeletingTransaction";

		// Token: 0x040004BE RID: 1214
		internal const string GridView_EnableSortingTransaction = "GridView_EnableSortingTransaction";

		// Token: 0x040004BF RID: 1215
		internal const string GridView_EnableSelectionTransaction = "GridView_EnableSelectionTransaction";

		// Token: 0x040004C0 RID: 1216
		internal const string GridView_EnablePagingTransaction = "GridView_EnablePagingTransaction";

		// Token: 0x040004C1 RID: 1217
		internal const string GridView_MoveLeftTransaction = "GridView_MoveLeftTransaction";

		// Token: 0x040004C2 RID: 1218
		internal const string GridView_MoveRightTransaction = "GridView_MoveRightTransaction";

		// Token: 0x040004C3 RID: 1219
		internal const string GridView_RemoveFieldTransaction = "GridView_RemoveFieldTransaction";

		// Token: 0x040004C4 RID: 1220
		internal const string GridView_SchemaRefreshedTransaction = "GridView_SchemaRefreshedTransaction";

		// Token: 0x040004C5 RID: 1221
		internal const string GridView_EditFieldsVerb = "GridView_EditFieldsVerb";

		// Token: 0x040004C6 RID: 1222
		internal const string GridView_AddNewFieldVerb = "GridView_AddNewFieldVerb";

		// Token: 0x040004C7 RID: 1223
		internal const string GridView_RemoveFieldVerb = "GridView_RemoveFieldVerb";

		// Token: 0x040004C8 RID: 1224
		internal const string GridView_MoveFieldLeftVerb = "GridView_MoveFieldLeftVerb";

		// Token: 0x040004C9 RID: 1225
		internal const string GridView_MoveFieldRightVerb = "GridView_MoveFieldRightVerb";

		// Token: 0x040004CA RID: 1226
		internal const string GridView_EditFieldsDesc = "GridView_EditFieldsDesc";

		// Token: 0x040004CB RID: 1227
		internal const string GridView_AddNewFieldDesc = "GridView_AddNewFieldDesc";

		// Token: 0x040004CC RID: 1228
		internal const string GridView_RemoveFieldDesc = "GridView_RemoveFieldDesc";

		// Token: 0x040004CD RID: 1229
		internal const string GridView_MoveFieldLeftDesc = "GridView_MoveFieldLeftDesc";

		// Token: 0x040004CE RID: 1230
		internal const string GridView_MoveFieldRightDesc = "GridView_MoveFieldRightDesc";

		// Token: 0x040004CF RID: 1231
		internal const string GridView_Field = "GridView_Field";

		// Token: 0x040004D0 RID: 1232
		internal const string GridView_EnablePaging = "GridView_EnablePaging";

		// Token: 0x040004D1 RID: 1233
		internal const string GridView_EnableSorting = "GridView_EnableSorting";

		// Token: 0x040004D2 RID: 1234
		internal const string GridView_EnableEditing = "GridView_EnableEditing";

		// Token: 0x040004D3 RID: 1235
		internal const string GridView_EnableDeleting = "GridView_EnableDeleting";

		// Token: 0x040004D4 RID: 1236
		internal const string GridView_EnableSelection = "GridView_EnableSelection";

		// Token: 0x040004D5 RID: 1237
		internal const string GridView_EnablePagingDesc = "GridView_EnablePagingDesc";

		// Token: 0x040004D6 RID: 1238
		internal const string GridView_EnableSortingDesc = "GridView_EnableSortingDesc";

		// Token: 0x040004D7 RID: 1239
		internal const string GridView_EnableEditingDesc = "GridView_EnableEditingDesc";

		// Token: 0x040004D8 RID: 1240
		internal const string GridView_EnableDeletingDesc = "GridView_EnableDeletingDesc";

		// Token: 0x040004D9 RID: 1241
		internal const string GridView_EnableSelectionDesc = "GridView_EnableSelectionDesc";

		// Token: 0x040004DA RID: 1242
		internal const string DataControls_SchemaRefreshedTransaction = "DataControls_SchemaRefreshedTransaction";

		// Token: 0x040004DB RID: 1243
		internal const string DetailsView_EditFieldsTransaction = "DetailsView_EditFieldsTransaction";

		// Token: 0x040004DC RID: 1244
		internal const string DetailsView_AddNewFieldTransaction = "DetailsView_AddNewFieldTransaction";

		// Token: 0x040004DD RID: 1245
		internal const string DetailsView_EnableEditingTransaction = "DetailsView_EnableEditingTransaction";

		// Token: 0x040004DE RID: 1246
		internal const string DetailsView_EnableDeletingTransaction = "DetailsView_EnableDeletingTransaction";

		// Token: 0x040004DF RID: 1247
		internal const string DetailsView_EnableInsertingTransaction = "DetailsView_EnableInsertingTransaction";

		// Token: 0x040004E0 RID: 1248
		internal const string DetailsView_EnablePagingTransaction = "DetailsView_EnablePagingTransaction";

		// Token: 0x040004E1 RID: 1249
		internal const string DetailsView_MoveUpTransaction = "DetailsView_MoveUpTransaction";

		// Token: 0x040004E2 RID: 1250
		internal const string DetailsView_MoveDownTransaction = "DetailsView_MoveDownTransaction";

		// Token: 0x040004E3 RID: 1251
		internal const string DetailsView_RemoveFieldTransaction = "DetailsView_RemoveFieldTransaction";

		// Token: 0x040004E4 RID: 1252
		internal const string DetailsView_EditFieldsVerb = "DetailsView_EditFieldsVerb";

		// Token: 0x040004E5 RID: 1253
		internal const string DetailsView_AddNewFieldVerb = "DetailsView_AddNewFieldVerb";

		// Token: 0x040004E6 RID: 1254
		internal const string DetailsView_RemoveFieldVerb = "DetailsView_RemoveFieldVerb";

		// Token: 0x040004E7 RID: 1255
		internal const string DetailsView_MoveFieldUpVerb = "DetailsView_MoveFieldUpVerb";

		// Token: 0x040004E8 RID: 1256
		internal const string DetailsView_MoveFieldDownVerb = "DetailsView_MoveFieldDownVerb";

		// Token: 0x040004E9 RID: 1257
		internal const string DetailsView_Field = "DetailsView_Field";

		// Token: 0x040004EA RID: 1258
		internal const string DetailsView_EnablePaging = "DetailsView_EnablePaging";

		// Token: 0x040004EB RID: 1259
		internal const string DetailsView_EnableEditing = "DetailsView_EnableEditing";

		// Token: 0x040004EC RID: 1260
		internal const string DetailsView_EnableDeleting = "DetailsView_EnableDeleting";

		// Token: 0x040004ED RID: 1261
		internal const string DetailsView_EnableInserting = "DetailsView_EnableInserting";

		// Token: 0x040004EE RID: 1262
		internal const string DetailsView_EditFieldsDesc = "DetailsView_EditFieldsDesc";

		// Token: 0x040004EF RID: 1263
		internal const string DetailsView_AddNewFieldDesc = "DetailsView_AddNewFieldDesc";

		// Token: 0x040004F0 RID: 1264
		internal const string DetailsView_RemoveFieldDesc = "DetailsView_RemoveFieldDesc";

		// Token: 0x040004F1 RID: 1265
		internal const string DetailsView_MoveFieldUpDesc = "DetailsView_MoveFieldUpDesc";

		// Token: 0x040004F2 RID: 1266
		internal const string DetailsView_MoveFieldDownDesc = "DetailsView_MoveFieldDownDesc";

		// Token: 0x040004F3 RID: 1267
		internal const string DetailsView_EnablePagingDesc = "DetailsView_EnablePagingDesc";

		// Token: 0x040004F4 RID: 1268
		internal const string DetailsView_EnableEditingDesc = "DetailsView_EnableEditingDesc";

		// Token: 0x040004F5 RID: 1269
		internal const string DetailsView_EnableDeletingDesc = "DetailsView_EnableDeletingDesc";

		// Token: 0x040004F6 RID: 1270
		internal const string DetailsView_EnableInsertingDesc = "DetailsView_EnableInsertingDesc";

		// Token: 0x040004F7 RID: 1271
		internal const string FormView_EnablePagingTransaction = "FormView_EnablePagingTransaction";

		// Token: 0x040004F8 RID: 1272
		internal const string FormView_EnablePaging = "FormView_EnablePaging";

		// Token: 0x040004F9 RID: 1273
		internal const string FormView_EnablePagingDesc = "FormView_EnablePagingDesc";

		// Token: 0x040004FA RID: 1274
		internal const string FormView_EnableDynamicData = "FormView_EnableDynamicData";

		// Token: 0x040004FB RID: 1275
		internal const string FormView_EnableDynamicDataDesc = "FormView_EnableDynamicDataDesc";

		// Token: 0x040004FC RID: 1276
		internal const string FormView_SchemaRefreshedWarning = "FormView_SchemaRefreshedWarning";

		// Token: 0x040004FD RID: 1277
		internal const string FormView_SchemaRefreshedWarningNoDataSource = "FormView_SchemaRefreshedWarningNoDataSource";

		// Token: 0x040004FE RID: 1278
		internal const string FormView_SchemaRefreshedWarningGenerate = "FormView_SchemaRefreshedWarningGenerate";

		// Token: 0x040004FF RID: 1279
		internal const string FormView_SchemaRefreshedCaption = "FormView_SchemaRefreshedCaption";

		// Token: 0x04000500 RID: 1280
		internal const string FormView_Edit = "FormView_Edit";

		// Token: 0x04000501 RID: 1281
		internal const string FormView_Update = "FormView_Update";

		// Token: 0x04000502 RID: 1282
		internal const string FormView_Cancel = "FormView_Cancel";

		// Token: 0x04000503 RID: 1283
		internal const string FormView_Delete = "FormView_Delete";

		// Token: 0x04000504 RID: 1284
		internal const string FormView_New = "FormView_New";

		// Token: 0x04000505 RID: 1285
		internal const string FormView_Insert = "FormView_Insert";

		// Token: 0x04000506 RID: 1286
		internal const string ListControlCreateDataSource_Title = "ListControlCreateDataSource_Title";

		// Token: 0x04000507 RID: 1287
		internal const string ListControlCreateDataSource_Caption = "ListControlCreateDataSource_Caption";

		// Token: 0x04000508 RID: 1288
		internal const string ListControlCreateDataSource_Description = "ListControlCreateDataSource_Description";

		// Token: 0x04000509 RID: 1289
		internal const string ListControlCreateDataSource_SelectDataSource = "ListControlCreateDataSource_SelectDataSource";

		// Token: 0x0400050A RID: 1290
		internal const string ListControlCreateDataSource_SelectDataTextField = "ListControlCreateDataSource_SelectDataTextField";

		// Token: 0x0400050B RID: 1291
		internal const string ListControlCreateDataSource_SelectDataValueField = "ListControlCreateDataSource_SelectDataValueField";

		// Token: 0x0400050C RID: 1292
		internal const string ListControl_ConfigureDataVerb = "ListControl_ConfigureDataVerb";

		// Token: 0x0400050D RID: 1293
		internal const string ListControlDesigner_ConnectToDataSource = "ListControlDesigner_ConnectToDataSource";

		// Token: 0x0400050E RID: 1294
		internal const string ListControl_EnableAutoPostBack = "ListControl_EnableAutoPostBack";

		// Token: 0x0400050F RID: 1295
		internal const string ListControl_EnableAutoPostBackDesc = "ListControl_EnableAutoPostBackDesc";

		// Token: 0x04000510 RID: 1296
		internal const string ListControl_EditItems = "ListControl_EditItems";

		// Token: 0x04000511 RID: 1297
		internal const string ListControl_EditItemsDesc = "ListControl_EditItemsDesc";

		// Token: 0x04000512 RID: 1298
		internal const string ListControlDesigner_EditItems = "ListControlDesigner_EditItems";

		// Token: 0x04000513 RID: 1299
		internal const string ContainerControlDesigner_RegionWatermark = "ContainerControlDesigner_RegionWatermark";

		// Token: 0x04000514 RID: 1300
		internal const string ContentPlaceHolder_Invalid_RootComponent = "ContentPlaceHolder_Invalid_RootComponent";

		// Token: 0x04000515 RID: 1301
		internal const string Content_CreateBlankContent = "Content_CreateBlankContent";

		// Token: 0x04000516 RID: 1302
		internal const string Content_ClearRegion = "Content_ClearRegion";

		// Token: 0x04000517 RID: 1303
		internal const string SiteMapPathAFmt_Scheme_Default = "SiteMapPathAFmt_Scheme_Default";

		// Token: 0x04000518 RID: 1304
		internal const string SiteMapPathAFmt_Scheme_Colorful = "SiteMapPathAFmt_Scheme_Colorful";

		// Token: 0x04000519 RID: 1305
		internal const string SiteMapPathAFmt_Scheme_Simple = "SiteMapPathAFmt_Scheme_Simple";

		// Token: 0x0400051A RID: 1306
		internal const string SiteMapPathAFmt_Scheme_Professional = "SiteMapPathAFmt_Scheme_Professional";

		// Token: 0x0400051B RID: 1307
		internal const string SiteMapPathAFmt_Scheme_Classic = "SiteMapPathAFmt_Scheme_Classic";

		// Token: 0x0400051C RID: 1308
		internal const string ImageGeneratorUrlEditor_Filter = "ImageGeneratorUrlEditor_Filter";

		// Token: 0x0400051D RID: 1309
		internal const string WebControls_ConvertToTemplate = "WebControls_ConvertToTemplate";

		// Token: 0x0400051E RID: 1310
		internal const string WebControls_ConvertToTemplateDescription = "WebControls_ConvertToTemplateDescription";

		// Token: 0x0400051F RID: 1311
		internal const string WebControls_ConvertToTemplateDescriptionViews = "WebControls_ConvertToTemplateDescriptionViews";

		// Token: 0x04000520 RID: 1312
		internal const string WebControls_Reset = "WebControls_Reset";

		// Token: 0x04000521 RID: 1313
		internal const string WebControls_ResetDescription = "WebControls_ResetDescription";

		// Token: 0x04000522 RID: 1314
		internal const string WebControls_ResetDescriptionViews = "WebControls_ResetDescriptionViews";

		// Token: 0x04000523 RID: 1315
		internal const string WebControls_Views = "WebControls_Views";

		// Token: 0x04000524 RID: 1316
		internal const string WebControls_ViewsDescription = "WebControls_ViewsDescription";

		// Token: 0x04000525 RID: 1317
		internal const string ChangePassword_ChangePasswordView = "ChangePassword_ChangePasswordView";

		// Token: 0x04000526 RID: 1318
		internal const string ChangePassword_SuccessView = "ChangePassword_SuccessView";

		// Token: 0x04000527 RID: 1319
		internal const string ChangePasswordAutoFormat_UserName = "ChangePasswordAutoFormat_UserName";

		// Token: 0x04000528 RID: 1320
		internal const string ChangePasswordAutoFormat_HelpPageText = "ChangePasswordAutoFormat_HelpPageText";

		// Token: 0x04000529 RID: 1321
		internal const string ChangePasswordScheme_Empty = "ChangePasswordScheme_Empty";

		// Token: 0x0400052A RID: 1322
		internal const string ChangePasswordScheme_Classic = "ChangePasswordScheme_Classic";

		// Token: 0x0400052B RID: 1323
		internal const string ChangePasswordScheme_Elegant = "ChangePasswordScheme_Elegant";

		// Token: 0x0400052C RID: 1324
		internal const string ChangePasswordScheme_Simple = "ChangePasswordScheme_Simple";

		// Token: 0x0400052D RID: 1325
		internal const string ChangePasswordScheme_Professional = "ChangePasswordScheme_Professional";

		// Token: 0x0400052E RID: 1326
		internal const string ChangePasswordScheme_Colorful = "ChangePasswordScheme_Colorful";

		// Token: 0x0400052F RID: 1327
		internal const string Login_LaunchWebAdmin = "Login_LaunchWebAdmin";

		// Token: 0x04000530 RID: 1328
		internal const string Login_LaunchWebAdminDescription = "Login_LaunchWebAdminDescription";

		// Token: 0x04000531 RID: 1329
		internal const string LoginScheme_Empty = "LoginScheme_Empty";

		// Token: 0x04000532 RID: 1330
		internal const string LoginScheme_Classic = "LoginScheme_Classic";

		// Token: 0x04000533 RID: 1331
		internal const string LoginScheme_Elegant = "LoginScheme_Elegant";

		// Token: 0x04000534 RID: 1332
		internal const string LoginScheme_Simple = "LoginScheme_Simple";

		// Token: 0x04000535 RID: 1333
		internal const string LoginScheme_Professional = "LoginScheme_Professional";

		// Token: 0x04000536 RID: 1334
		internal const string LoginScheme_Colorful = "LoginScheme_Colorful";

		// Token: 0x04000537 RID: 1335
		internal const string LoginAutoFormat_UserName = "LoginAutoFormat_UserName";

		// Token: 0x04000538 RID: 1336
		internal const string LoginAutoFormat_HelpPageText = "LoginAutoFormat_HelpPageText";

		// Token: 0x04000539 RID: 1337
		internal const string CreateUserWizardScheme_Empty = "CreateUserWizardScheme_Empty";

		// Token: 0x0400053A RID: 1338
		internal const string CreateUserWizardScheme_Classic = "CreateUserWizardScheme_Classic";

		// Token: 0x0400053B RID: 1339
		internal const string CreateUserWizardScheme_Elegant = "CreateUserWizardScheme_Elegant";

		// Token: 0x0400053C RID: 1340
		internal const string CreateUserWizardScheme_Simple = "CreateUserWizardScheme_Simple";

		// Token: 0x0400053D RID: 1341
		internal const string CreateUserWizardScheme_Professional = "CreateUserWizardScheme_Professional";

		// Token: 0x0400053E RID: 1342
		internal const string CreateUserWizardScheme_Colorful = "CreateUserWizardScheme_Colorful";

		// Token: 0x0400053F RID: 1343
		internal const string LoginStatus_LoggedOutView = "LoginStatus_LoggedOutView";

		// Token: 0x04000540 RID: 1344
		internal const string LoginStatus_LoggedInView = "LoginStatus_LoggedInView";

		// Token: 0x04000541 RID: 1345
		internal const string LoginView_EditRoleGroups = "LoginView_EditRoleGroups";

		// Token: 0x04000542 RID: 1346
		internal const string LoginView_EditRoleGroupsDescription = "LoginView_EditRoleGroupsDescription";

		// Token: 0x04000543 RID: 1347
		internal const string LoginView_EditRoleGroupsTransactionDescription = "LoginView_EditRoleGroupsTransactionDescription";

		// Token: 0x04000544 RID: 1348
		internal const string LoginView_ErrorRendering = "LoginView_ErrorRendering";

		// Token: 0x04000545 RID: 1349
		internal const string LoginView_AnonymousTemplateEmpty = "LoginView_AnonymousTemplateEmpty";

		// Token: 0x04000546 RID: 1350
		internal const string LoginView_LoggedInTemplateEmpty = "LoginView_LoggedInTemplateEmpty";

		// Token: 0x04000547 RID: 1351
		internal const string LoginView_RoleGroupTemplateEmpty = "LoginView_RoleGroupTemplateEmpty";

		// Token: 0x04000548 RID: 1352
		internal const string LoginView_NoTemplateInst = "LoginView_NoTemplateInst";

		// Token: 0x04000549 RID: 1353
		internal const string UserControlDesignerHost_ComponentAlreadyExists = "UserControlDesignerHost_ComponentAlreadyExists";

		// Token: 0x0400054A RID: 1354
		internal const string MenuDesigner_DataActionGroup = "MenuDesigner_DataActionGroup";

		// Token: 0x0400054B RID: 1355
		internal const string MenuDesigner_EditBindingsTransactionDescription = "MenuDesigner_EditBindingsTransactionDescription";

		// Token: 0x0400054C RID: 1356
		internal const string MenuDesigner_EditMenuItemsTransactionDescription = "MenuDesigner_EditMenuItemsTransactionDescription";

		// Token: 0x0400054D RID: 1357
		internal const string MenuDesigner_EditBindings = "MenuDesigner_EditBindings";

		// Token: 0x0400054E RID: 1358
		internal const string MenuDesigner_EditBindingsDescription = "MenuDesigner_EditBindingsDescription";

		// Token: 0x0400054F RID: 1359
		internal const string MenuDesigner_EditMenuItems = "MenuDesigner_EditMenuItems";

		// Token: 0x04000550 RID: 1360
		internal const string MenuDesigner_EditMenuItemsDescription = "MenuDesigner_EditMenuItemsDescription";

		// Token: 0x04000551 RID: 1361
		internal const string MenuDesigner_CreateLineImages = "MenuDesigner_CreateLineImages";

		// Token: 0x04000552 RID: 1362
		internal const string MenuDesigner_Empty = "MenuDesigner_Empty";

		// Token: 0x04000553 RID: 1363
		internal const string MenuDesigner_EmptyDataBinding = "MenuDesigner_EmptyDataBinding";

		// Token: 0x04000554 RID: 1364
		internal const string MenuDesigner_Error = "MenuDesigner_Error";

		// Token: 0x04000555 RID: 1365
		internal const string MenuDesigner_EditNodesTransactionDescription = "MenuDesigner_EditNodesTransactionDescription";

		// Token: 0x04000556 RID: 1366
		internal const string MenuDesigner_EditNodes = "MenuDesigner_EditNodes";

		// Token: 0x04000557 RID: 1367
		internal const string MenuDesigner_ViewsDescription = "MenuDesigner_ViewsDescription";

		// Token: 0x04000558 RID: 1368
		internal const string MenuDesigner_ConvertToDynamicTemplate = "MenuDesigner_ConvertToDynamicTemplate";

		// Token: 0x04000559 RID: 1369
		internal const string MenuDesigner_ConvertToDynamicTemplateDescription = "MenuDesigner_ConvertToDynamicTemplateDescription";

		// Token: 0x0400055A RID: 1370
		internal const string MenuDesigner_ResetDynamicTemplate = "MenuDesigner_ResetDynamicTemplate";

		// Token: 0x0400055B RID: 1371
		internal const string MenuDesigner_ResetDynamicTemplateDescription = "MenuDesigner_ResetDynamicTemplateDescription";

		// Token: 0x0400055C RID: 1372
		internal const string MenuDesigner_ConvertToStaticTemplate = "MenuDesigner_ConvertToStaticTemplate";

		// Token: 0x0400055D RID: 1373
		internal const string MenuDesigner_ConvertToStaticTemplateDescription = "MenuDesigner_ConvertToStaticTemplateDescription";

		// Token: 0x0400055E RID: 1374
		internal const string MenuDesigner_ResetStaticTemplate = "MenuDesigner_ResetStaticTemplate";

		// Token: 0x0400055F RID: 1375
		internal const string MenuDesigner_ResetStaticTemplateDescription = "MenuDesigner_ResetStaticTemplateDescription";

		// Token: 0x04000560 RID: 1376
		internal const string Menu_StaticView = "Menu_StaticView";

		// Token: 0x04000561 RID: 1377
		internal const string Menu_DynamicView = "Menu_DynamicView";

		// Token: 0x04000562 RID: 1378
		internal const string MenuItemCollectionEditor_AddRoot = "MenuItemCollectionEditor_AddRoot";

		// Token: 0x04000563 RID: 1379
		internal const string MenuItemCollectionEditor_AddChild = "MenuItemCollectionEditor_AddChild";

		// Token: 0x04000564 RID: 1380
		internal const string MenuItemCollectionEditor_Remove = "MenuItemCollectionEditor_Remove";

		// Token: 0x04000565 RID: 1381
		internal const string MenuItemCollectionEditor_MoveDown = "MenuItemCollectionEditor_MoveDown";

		// Token: 0x04000566 RID: 1382
		internal const string MenuItemCollectionEditor_MoveUp = "MenuItemCollectionEditor_MoveUp";

		// Token: 0x04000567 RID: 1383
		internal const string MenuItemCollectionEditor_Indent = "MenuItemCollectionEditor_Indent";

		// Token: 0x04000568 RID: 1384
		internal const string MenuItemCollectionEditor_Unindent = "MenuItemCollectionEditor_Unindent";

		// Token: 0x04000569 RID: 1385
		internal const string MenuItemCollectionEditor_OK = "MenuItemCollectionEditor_OK";

		// Token: 0x0400056A RID: 1386
		internal const string MenuItemCollectionEditor_Cancel = "MenuItemCollectionEditor_Cancel";

		// Token: 0x0400056B RID: 1387
		internal const string MenuItemCollectionEditor_Nodes = "MenuItemCollectionEditor_Nodes";

		// Token: 0x0400056C RID: 1388
		internal const string MenuItemCollectionEditor_Properties = "MenuItemCollectionEditor_Properties";

		// Token: 0x0400056D RID: 1389
		internal const string MenuItemCollectionEditor_PropertyGrid = "MenuItemCollectionEditor_PropertyGrid";

		// Token: 0x0400056E RID: 1390
		internal const string MenuItemCollectionEditor_Title = "MenuItemCollectionEditor_Title";

		// Token: 0x0400056F RID: 1391
		internal const string MenuItemCollectionEditor_NewNodeText = "MenuItemCollectionEditor_NewNodeText";

		// Token: 0x04000570 RID: 1392
		internal const string MenuItemCollectionEditor_CantSelect = "MenuItemCollectionEditor_CantSelect";

		// Token: 0x04000571 RID: 1393
		internal const string MenuBindingsEditor_Apply = "MenuBindingsEditor_Apply";

		// Token: 0x04000572 RID: 1394
		internal const string MenuBindingsEditor_AddBinding = "MenuBindingsEditor_AddBinding";

		// Token: 0x04000573 RID: 1395
		internal const string MenuBindingsEditor_AutoGenerateBindings = "MenuBindingsEditor_AutoGenerateBindings";

		// Token: 0x04000574 RID: 1396
		internal const string MenuBindingsEditor_Bindings = "MenuBindingsEditor_Bindings";

		// Token: 0x04000575 RID: 1397
		internal const string MenuBindingsEditor_BindingProperties = "MenuBindingsEditor_BindingProperties";

		// Token: 0x04000576 RID: 1398
		internal const string MenuBindingsEditor_Cancel = "MenuBindingsEditor_Cancel";

		// Token: 0x04000577 RID: 1399
		internal const string MenuBindingsEditor_EmptyBindingText = "MenuBindingsEditor_EmptyBindingText";

		// Token: 0x04000578 RID: 1400
		internal const string MenuBindingsEditor_OK = "MenuBindingsEditor_OK";

		// Token: 0x04000579 RID: 1401
		internal const string MenuBindingsEditor_Schema = "MenuBindingsEditor_Schema";

		// Token: 0x0400057A RID: 1402
		internal const string MenuBindingsEditor_Title = "MenuBindingsEditor_Title";

		// Token: 0x0400057B RID: 1403
		internal const string MenuBindingsEditor_MoveBindingUpName = "MenuBindingsEditor_MoveBindingUpName";

		// Token: 0x0400057C RID: 1404
		internal const string MenuBindingsEditor_MoveBindingUpDescription = "MenuBindingsEditor_MoveBindingUpDescription";

		// Token: 0x0400057D RID: 1405
		internal const string MenuBindingsEditor_MoveBindingDownName = "MenuBindingsEditor_MoveBindingDownName";

		// Token: 0x0400057E RID: 1406
		internal const string MenuBindingsEditor_MoveBindingDownDescription = "MenuBindingsEditor_MoveBindingDownDescription";

		// Token: 0x0400057F RID: 1407
		internal const string MenuBindingsEditor_DeleteBindingName = "MenuBindingsEditor_DeleteBindingName";

		// Token: 0x04000580 RID: 1408
		internal const string MenuBindingsEditor_DeleteBindingDescription = "MenuBindingsEditor_DeleteBindingDescription";

		// Token: 0x04000581 RID: 1409
		internal const string MenuScheme_Empty = "MenuScheme_Empty";

		// Token: 0x04000582 RID: 1410
		internal const string MenuScheme_Classic = "MenuScheme_Classic";

		// Token: 0x04000583 RID: 1411
		internal const string MenuScheme_Professional = "MenuScheme_Professional";

		// Token: 0x04000584 RID: 1412
		internal const string MenuScheme_Colorful = "MenuScheme_Colorful";

		// Token: 0x04000585 RID: 1413
		internal const string MenuScheme_Simple = "MenuScheme_Simple";

		// Token: 0x04000586 RID: 1414
		internal const string PagerScheme_Empty = "PagerScheme_Empty";

		// Token: 0x04000587 RID: 1415
		internal const string PagerScheme_Classic = "PagerScheme_Classic";

		// Token: 0x04000588 RID: 1416
		internal const string PagerScheme_Professional = "PagerScheme_Professional";

		// Token: 0x04000589 RID: 1417
		internal const string PagerScheme_Colorful = "PagerScheme_Colorful";

		// Token: 0x0400058A RID: 1418
		internal const string PagerScheme_Simple = "PagerScheme_Simple";

		// Token: 0x0400058B RID: 1419
		internal const string PasswordRecoveryScheme_Empty = "PasswordRecoveryScheme_Empty";

		// Token: 0x0400058C RID: 1420
		internal const string PasswordRecoveryScheme_Classic = "PasswordRecoveryScheme_Classic";

		// Token: 0x0400058D RID: 1421
		internal const string PasswordRecoveryScheme_Elegant = "PasswordRecoveryScheme_Elegant";

		// Token: 0x0400058E RID: 1422
		internal const string PasswordRecoveryScheme_Simple = "PasswordRecoveryScheme_Simple";

		// Token: 0x0400058F RID: 1423
		internal const string PasswordRecoveryScheme_Professional = "PasswordRecoveryScheme_Professional";

		// Token: 0x04000590 RID: 1424
		internal const string PasswordRecoveryScheme_Colorful = "PasswordRecoveryScheme_Colorful";

		// Token: 0x04000591 RID: 1425
		internal const string PasswordRecovery_QuestionView = "PasswordRecovery_QuestionView";

		// Token: 0x04000592 RID: 1426
		internal const string PasswordRecovery_SuccessView = "PasswordRecovery_SuccessView";

		// Token: 0x04000593 RID: 1427
		internal const string PasswordRecovery_UserNameView = "PasswordRecovery_UserNameView";

		// Token: 0x04000594 RID: 1428
		internal const string PasswordRecoveryAutoFormat_UserName = "PasswordRecoveryAutoFormat_UserName";

		// Token: 0x04000595 RID: 1429
		internal const string PasswordRecoveryAutoFormat_HelpPageText = "PasswordRecoveryAutoFormat_HelpPageText";

		// Token: 0x04000596 RID: 1430
		internal const string MailFilePicker_Caption = "MailFilePicker_Caption";

		// Token: 0x04000597 RID: 1431
		internal const string MailFilePicker_Filter = "MailFilePicker_Filter";

		// Token: 0x04000598 RID: 1432
		internal const string Xml_Inst = "Xml_Inst";

		// Token: 0x04000599 RID: 1433
		internal const string MailDefinitionBodyFileNameEditor_DefaultCaption = "MailDefinitionBodyFileNameEditor_DefaultCaption";

		// Token: 0x0400059A RID: 1434
		internal const string MailDefinitionBodyFileNameEditor_DefaultFilter = "MailDefinitionBodyFileNameEditor_DefaultFilter";

		// Token: 0x0400059B RID: 1435
		internal const string UrlPicker_DefaultCaption = "UrlPicker_DefaultCaption";

		// Token: 0x0400059C RID: 1436
		internal const string UrlPicker_DefaultFilter = "UrlPicker_DefaultFilter";

		// Token: 0x0400059D RID: 1437
		internal const string UrlPicker_ImageCaption = "UrlPicker_ImageCaption";

		// Token: 0x0400059E RID: 1438
		internal const string UrlPicker_ImageFilter = "UrlPicker_ImageFilter";

		// Token: 0x0400059F RID: 1439
		internal const string UrlPicker_XmlCaption = "UrlPicker_XmlCaption";

		// Token: 0x040005A0 RID: 1440
		internal const string UrlPicker_XmlFilter = "UrlPicker_XmlFilter";

		// Token: 0x040005A1 RID: 1441
		internal const string UrlPicker_XslCaption = "UrlPicker_XslCaption";

		// Token: 0x040005A2 RID: 1442
		internal const string UrlPicker_XslFilter = "UrlPicker_XslFilter";

		// Token: 0x040005A3 RID: 1443
		internal const string XMLFilePicker_Caption = "XMLFilePicker_Caption";

		// Token: 0x040005A4 RID: 1444
		internal const string XMLFilePicker_Filter = "XMLFilePicker_Filter";

		// Token: 0x040005A5 RID: 1445
		internal const string DataBindingGlyph_ToolTip = "DataBindingGlyph_ToolTip";

		// Token: 0x040005A6 RID: 1446
		internal const string ExpressionBindingGlyph_ToolTip = "ExpressionBindingGlyph_ToolTip";

		// Token: 0x040005A7 RID: 1447
		internal const string ImplicitExpressionBindingGlyph_ToolTip = "ImplicitExpressionBindingGlyph_ToolTip";

		// Token: 0x040005A8 RID: 1448
		internal const string TemplateEdit_Tip = "TemplateEdit_Tip";

		// Token: 0x040005A9 RID: 1449
		internal const string RegexEditor_Title = "RegexEditor_Title";

		// Token: 0x040005AA RID: 1450
		internal const string RegexEditor_StdExp = "RegexEditor_StdExp";

		// Token: 0x040005AB RID: 1451
		internal const string RegexEditor_Validate = "RegexEditor_Validate";

		// Token: 0x040005AC RID: 1452
		internal const string RegexEditor_SampleInput = "RegexEditor_SampleInput";

		// Token: 0x040005AD RID: 1453
		internal const string RegexEditor_TestExpression = "RegexEditor_TestExpression";

		// Token: 0x040005AE RID: 1454
		internal const string RegexEditor_ValidationExpression = "RegexEditor_ValidationExpression";

		// Token: 0x040005AF RID: 1455
		internal const string RegexEditor_InputValid = "RegexEditor_InputValid";

		// Token: 0x040005B0 RID: 1456
		internal const string RegexEditor_InputInvalid = "RegexEditor_InputInvalid";

		// Token: 0x040005B1 RID: 1457
		internal const string RegexEditor_BadExpression = "RegexEditor_BadExpression";

		// Token: 0x040005B2 RID: 1458
		internal const string RegexEditor_Help = "RegexEditor_Help";

		// Token: 0x040005B3 RID: 1459
		internal const string RegexCanned_Custom = "RegexCanned_Custom";

		// Token: 0x040005B4 RID: 1460
		internal const string RegexCanned_Zip = "RegexCanned_Zip";

		// Token: 0x040005B5 RID: 1461
		internal const string RegexCanned_SocialSecurity = "RegexCanned_SocialSecurity";

		// Token: 0x040005B6 RID: 1462
		internal const string RegexCanned_USPhone = "RegexCanned_USPhone";

		// Token: 0x040005B7 RID: 1463
		internal const string RegexCanned_Email = "RegexCanned_Email";

		// Token: 0x040005B8 RID: 1464
		internal const string RegexCanned_URL = "RegexCanned_URL";

		// Token: 0x040005B9 RID: 1465
		internal const string RegexCanned_FrZip = "RegexCanned_FrZip";

		// Token: 0x040005BA RID: 1466
		internal const string RegexCanned_FrPhone = "RegexCanned_FrPhone";

		// Token: 0x040005BB RID: 1467
		internal const string RegexCanned_DeZip = "RegexCanned_DeZip";

		// Token: 0x040005BC RID: 1468
		internal const string RegexCanned_DePhone = "RegexCanned_DePhone";

		// Token: 0x040005BD RID: 1469
		internal const string RegexCanned_JpnZip = "RegexCanned_JpnZip";

		// Token: 0x040005BE RID: 1470
		internal const string RegexCanned_JpnPhone = "RegexCanned_JpnPhone";

		// Token: 0x040005BF RID: 1471
		internal const string RegexCanned_PrcZip = "RegexCanned_PrcZip";

		// Token: 0x040005C0 RID: 1472
		internal const string RegexCanned_PrcPhone = "RegexCanned_PrcPhone";

		// Token: 0x040005C1 RID: 1473
		internal const string RegexCanned_PrcSocialSecurity = "RegexCanned_PrcSocialSecurity";

		// Token: 0x040005C2 RID: 1474
		internal const string RegexCanned_Zip_Format = "RegexCanned_Zip_Format";

		// Token: 0x040005C3 RID: 1475
		internal const string RegexCanned_SocialSecurity_Format = "RegexCanned_SocialSecurity_Format";

		// Token: 0x040005C4 RID: 1476
		internal const string RegexCanned_USPhone_Format = "RegexCanned_USPhone_Format";

		// Token: 0x040005C5 RID: 1477
		internal const string RegexCanned_FrZip_Format = "RegexCanned_FrZip_Format";

		// Token: 0x040005C6 RID: 1478
		internal const string RegexCanned_FrPhone_Format = "RegexCanned_FrPhone_Format";

		// Token: 0x040005C7 RID: 1479
		internal const string RegexCanned_DeZip_Format = "RegexCanned_DeZip_Format";

		// Token: 0x040005C8 RID: 1480
		internal const string RegexCanned_DePhone_Format = "RegexCanned_DePhone_Format";

		// Token: 0x040005C9 RID: 1481
		internal const string RegexCanned_JpnZip_Format = "RegexCanned_JpnZip_Format";

		// Token: 0x040005CA RID: 1482
		internal const string RegexCanned_JpnPhone_Format = "RegexCanned_JpnPhone_Format";

		// Token: 0x040005CB RID: 1483
		internal const string RegexCanned_PrcZip_Format = "RegexCanned_PrcZip_Format";

		// Token: 0x040005CC RID: 1484
		internal const string RegexCanned_PrcPhone_Format = "RegexCanned_PrcPhone_Format";

		// Token: 0x040005CD RID: 1485
		internal const string RegexCanned_PrcSocialSecurity_Format = "RegexCanned_PrcSocialSecurity_Format";

		// Token: 0x040005CE RID: 1486
		internal const string TemplateEditableDesignerRegion_CannotSetSupportsDataBinding = "TemplateEditableDesignerRegion_CannotSetSupportsDataBinding";

		// Token: 0x040005CF RID: 1487
		internal const string TemplateDefinition_InvalidTemplateProperty = "TemplateDefinition_InvalidTemplateProperty";

		// Token: 0x040005D0 RID: 1488
		internal const string WrongType = "WrongType";

		// Token: 0x040005D1 RID: 1489
		internal const string Toolbox_OnWebformsPage = "Toolbox_OnWebformsPage";

		// Token: 0x040005D2 RID: 1490
		internal const string Toolbox_BadAttributeType = "Toolbox_BadAttributeType";

		// Token: 0x040005D3 RID: 1491
		internal const string TreeViewImageGenerator_ExpandImage = "TreeViewImageGenerator_ExpandImage";

		// Token: 0x040005D4 RID: 1492
		internal const string TreeViewImageGenerator_CollapseImage = "TreeViewImageGenerator_CollapseImage";

		// Token: 0x040005D5 RID: 1493
		internal const string TreeViewImageGenerator_NoExpandImage = "TreeViewImageGenerator_NoExpandImage";

		// Token: 0x040005D6 RID: 1494
		internal const string TreeViewImageGenerator_Preview = "TreeViewImageGenerator_Preview";

		// Token: 0x040005D7 RID: 1495
		internal const string TreeViewImageGenerator_Properties = "TreeViewImageGenerator_Properties";

		// Token: 0x040005D8 RID: 1496
		internal const string TreeViewImageGenerator_SampleRoot = "TreeViewImageGenerator_SampleRoot";

		// Token: 0x040005D9 RID: 1497
		internal const string TreeViewImageGenerator_SampleParent = "TreeViewImageGenerator_SampleParent";

		// Token: 0x040005DA RID: 1498
		internal const string TreeViewImageGenerator_SampleLeaf = "TreeViewImageGenerator_SampleLeaf";

		// Token: 0x040005DB RID: 1499
		internal const string TreeViewImageGenerator_FolderName = "TreeViewImageGenerator_FolderName";

		// Token: 0x040005DC RID: 1500
		internal const string TreeViewImageGenerator_DefaultFolderName = "TreeViewImageGenerator_DefaultFolderName";

		// Token: 0x040005DD RID: 1501
		internal const string TreeViewImageGenerator_Title = "TreeViewImageGenerator_Title";

		// Token: 0x040005DE RID: 1502
		internal const string TreeViewImageGenerator_LineColor = "TreeViewImageGenerator_LineColor";

		// Token: 0x040005DF RID: 1503
		internal const string TreeViewImageGenerator_LineStyle = "TreeViewImageGenerator_LineStyle";

		// Token: 0x040005E0 RID: 1504
		internal const string TreeViewImageGenerator_LineWidth = "TreeViewImageGenerator_LineWidth";

		// Token: 0x040005E1 RID: 1505
		internal const string TreeViewImageGenerator_LineImageHeight = "TreeViewImageGenerator_LineImageHeight";

		// Token: 0x040005E2 RID: 1506
		internal const string TreeViewImageGenerator_LineImageWidth = "TreeViewImageGenerator_LineImageWidth";

		// Token: 0x040005E3 RID: 1507
		internal const string TreeViewImageGenerator_LineImagesGenerated = "TreeViewImageGenerator_LineImagesGenerated";

		// Token: 0x040005E4 RID: 1508
		internal const string TreeViewImageGenerator_MissingFolderName = "TreeViewImageGenerator_MissingFolderName";

		// Token: 0x040005E5 RID: 1509
		internal const string TreeViewImageGenerator_NonExistentFolderName = "TreeViewImageGenerator_NonExistentFolderName";

		// Token: 0x040005E6 RID: 1510
		internal const string TreeViewImageGenerator_ProgressBarName = "TreeViewImageGenerator_ProgressBarName";

		// Token: 0x040005E7 RID: 1511
		internal const string TreeViewImageGenerator_ImagePickerFilter = "TreeViewImageGenerator_ImagePickerFilter";

		// Token: 0x040005E8 RID: 1512
		internal const string TreeViewImageGenerator_TransparentColor = "TreeViewImageGenerator_TransparentColor";

		// Token: 0x040005E9 RID: 1513
		internal const string TreeViewImageGenerator_ErrorCreatingFolder = "TreeViewImageGenerator_ErrorCreatingFolder";

		// Token: 0x040005EA RID: 1514
		internal const string TreeViewImageGenerator_InvalidFolderName = "TreeViewImageGenerator_InvalidFolderName";

		// Token: 0x040005EB RID: 1515
		internal const string TreeViewImageGenerator_DocumentExists = "TreeViewImageGenerator_DocumentExists";

		// Token: 0x040005EC RID: 1516
		internal const string TreeViewImageGenerator_ErrorWriting = "TreeViewImageGenerator_ErrorWriting";

		// Token: 0x040005ED RID: 1517
		internal const string TreeViewImageGenerator_InvalidValue = "TreeViewImageGenerator_InvalidValue";

		// Token: 0x040005EE RID: 1518
		internal const string TreeViewImageGenerator_CouldNotOpenImage = "TreeViewImageGenerator_CouldNotOpenImage";

		// Token: 0x040005EF RID: 1519
		internal const string TreeViewImageGenerator_Yes = "TreeViewImageGenerator_Yes";

		// Token: 0x040005F0 RID: 1520
		internal const string TreeViewImageGenerator_No = "TreeViewImageGenerator_No";

		// Token: 0x040005F1 RID: 1521
		internal const string TreeViewImageGenerator_YesToAll = "TreeViewImageGenerator_YesToAll";

		// Token: 0x040005F2 RID: 1522
		internal const string TreeViewImageGenerator_HelpText = "TreeViewImageGenerator_HelpText";

		// Token: 0x040005F3 RID: 1523
		internal const string TreeNodeCollectionEditor_AddRoot = "TreeNodeCollectionEditor_AddRoot";

		// Token: 0x040005F4 RID: 1524
		internal const string TreeNodeCollectionEditor_AddChild = "TreeNodeCollectionEditor_AddChild";

		// Token: 0x040005F5 RID: 1525
		internal const string TreeNodeCollectionEditor_Remove = "TreeNodeCollectionEditor_Remove";

		// Token: 0x040005F6 RID: 1526
		internal const string TreeNodeCollectionEditor_MoveDown = "TreeNodeCollectionEditor_MoveDown";

		// Token: 0x040005F7 RID: 1527
		internal const string TreeNodeCollectionEditor_MoveUp = "TreeNodeCollectionEditor_MoveUp";

		// Token: 0x040005F8 RID: 1528
		internal const string TreeNodeCollectionEditor_Indent = "TreeNodeCollectionEditor_Indent";

		// Token: 0x040005F9 RID: 1529
		internal const string TreeNodeCollectionEditor_Unindent = "TreeNodeCollectionEditor_Unindent";

		// Token: 0x040005FA RID: 1530
		internal const string TreeNodeCollectionEditor_OK = "TreeNodeCollectionEditor_OK";

		// Token: 0x040005FB RID: 1531
		internal const string TreeNodeCollectionEditor_Cancel = "TreeNodeCollectionEditor_Cancel";

		// Token: 0x040005FC RID: 1532
		internal const string TreeNodeCollectionEditor_Nodes = "TreeNodeCollectionEditor_Nodes";

		// Token: 0x040005FD RID: 1533
		internal const string TreeNodeCollectionEditor_Properties = "TreeNodeCollectionEditor_Properties";

		// Token: 0x040005FE RID: 1534
		internal const string TreeNodeCollectionEditor_Title = "TreeNodeCollectionEditor_Title";

		// Token: 0x040005FF RID: 1535
		internal const string TreeNodeCollectionEditor_NewNodeText = "TreeNodeCollectionEditor_NewNodeText";

		// Token: 0x04000600 RID: 1536
		internal const string TreeViewBindingsEditor_Apply = "TreeViewBindingsEditor_Apply";

		// Token: 0x04000601 RID: 1537
		internal const string TreeViewBindingsEditor_AddBinding = "TreeViewBindingsEditor_AddBinding";

		// Token: 0x04000602 RID: 1538
		internal const string TreeViewBindingsEditor_AutoGenerateBindings = "TreeViewBindingsEditor_AutoGenerateBindings";

		// Token: 0x04000603 RID: 1539
		internal const string TreeViewBindingsEditor_Bindings = "TreeViewBindingsEditor_Bindings";

		// Token: 0x04000604 RID: 1540
		internal const string TreeViewBindingsEditor_BindingProperties = "TreeViewBindingsEditor_BindingProperties";

		// Token: 0x04000605 RID: 1541
		internal const string TreeViewBindingsEditor_Cancel = "TreeViewBindingsEditor_Cancel";

		// Token: 0x04000606 RID: 1542
		internal const string TreeViewBindingsEditor_EmptyBindingText = "TreeViewBindingsEditor_EmptyBindingText";

		// Token: 0x04000607 RID: 1543
		internal const string TreeViewBindingsEditor_OK = "TreeViewBindingsEditor_OK";

		// Token: 0x04000608 RID: 1544
		internal const string TreeViewBindingsEditor_Schema = "TreeViewBindingsEditor_Schema";

		// Token: 0x04000609 RID: 1545
		internal const string TreeViewBindingsEditor_Title = "TreeViewBindingsEditor_Title";

		// Token: 0x0400060A RID: 1546
		internal const string TreeViewDesigner_CreateLineImagesTransactionDescription = "TreeViewDesigner_CreateLineImagesTransactionDescription";

		// Token: 0x0400060B RID: 1547
		internal const string TreeViewDesigner_DataActionGroup = "TreeViewDesigner_DataActionGroup";

		// Token: 0x0400060C RID: 1548
		internal const string TreeViewDesigner_EditBindingsTransactionDescription = "TreeViewDesigner_EditBindingsTransactionDescription";

		// Token: 0x0400060D RID: 1549
		internal const string TreeViewDesigner_EditNodesTransactionDescription = "TreeViewDesigner_EditNodesTransactionDescription";

		// Token: 0x0400060E RID: 1550
		internal const string TreeViewDesigner_EditNodesDescription = "TreeViewDesigner_EditNodesDescription";

		// Token: 0x0400060F RID: 1551
		internal const string TreeViewDesigner_EditBindings = "TreeViewDesigner_EditBindings";

		// Token: 0x04000610 RID: 1552
		internal const string TreeViewDesigner_EditBindingsDescription = "TreeViewDesigner_EditBindingsDescription";

		// Token: 0x04000611 RID: 1553
		internal const string TreeViewDesigner_EditNodes = "TreeViewDesigner_EditNodes";

		// Token: 0x04000612 RID: 1554
		internal const string TreeViewDesigner_CreateLineImages = "TreeViewDesigner_CreateLineImages";

		// Token: 0x04000613 RID: 1555
		internal const string TreeViewDesigner_CreateLineImagesDescription = "TreeViewDesigner_CreateLineImagesDescription";

		// Token: 0x04000614 RID: 1556
		internal const string TreeViewDesigner_Empty = "TreeViewDesigner_Empty";

		// Token: 0x04000615 RID: 1557
		internal const string TreeViewDesigner_EmptyDataBinding = "TreeViewDesigner_EmptyDataBinding";

		// Token: 0x04000616 RID: 1558
		internal const string TreeViewDesigner_Error = "TreeViewDesigner_Error";

		// Token: 0x04000617 RID: 1559
		internal const string TreeViewDesigner_ShowLines = "TreeViewDesigner_ShowLines";

		// Token: 0x04000618 RID: 1560
		internal const string TreeViewDesigner_ShowLinesDescription = "TreeViewDesigner_ShowLinesDescription";

		// Token: 0x04000619 RID: 1561
		internal const string TreeViewBindingsEditor_MoveBindingUpName = "TreeViewBindingsEditor_MoveBindingUpName";

		// Token: 0x0400061A RID: 1562
		internal const string TreeViewBindingsEditor_MoveBindingUpDescription = "TreeViewBindingsEditor_MoveBindingUpDescription";

		// Token: 0x0400061B RID: 1563
		internal const string TreeViewBindingsEditor_MoveBindingDownName = "TreeViewBindingsEditor_MoveBindingDownName";

		// Token: 0x0400061C RID: 1564
		internal const string TreeViewBindingsEditor_MoveBindingDownDescription = "TreeViewBindingsEditor_MoveBindingDownDescription";

		// Token: 0x0400061D RID: 1565
		internal const string TreeViewBindingsEditor_DeleteBindingName = "TreeViewBindingsEditor_DeleteBindingName";

		// Token: 0x0400061E RID: 1566
		internal const string TreeViewBindingsEditor_DeleteBindingDescription = "TreeViewBindingsEditor_DeleteBindingDescription";

		// Token: 0x0400061F RID: 1567
		internal const string TVScheme_Empty = "TVScheme_Empty";

		// Token: 0x04000620 RID: 1568
		internal const string TVScheme_XP_File_Explorer = "TVScheme_XP_File_Explorer";

		// Token: 0x04000621 RID: 1569
		internal const string TVScheme_MSDN = "TVScheme_MSDN";

		// Token: 0x04000622 RID: 1570
		internal const string TVScheme_Windows_Help = "TVScheme_Windows_Help";

		// Token: 0x04000623 RID: 1571
		internal const string TVScheme_Simple = "TVScheme_Simple";

		// Token: 0x04000624 RID: 1572
		internal const string TVScheme_Simple2 = "TVScheme_Simple2";

		// Token: 0x04000625 RID: 1573
		internal const string TVScheme_BulletedList = "TVScheme_BulletedList";

		// Token: 0x04000626 RID: 1574
		internal const string TVScheme_BulletedList2 = "TVScheme_BulletedList2";

		// Token: 0x04000627 RID: 1575
		internal const string TVScheme_BulletedList3 = "TVScheme_BulletedList3";

		// Token: 0x04000628 RID: 1576
		internal const string TVScheme_BulletedList4 = "TVScheme_BulletedList4";

		// Token: 0x04000629 RID: 1577
		internal const string TVScheme_BulletedList5 = "TVScheme_BulletedList5";

		// Token: 0x0400062A RID: 1578
		internal const string TVScheme_BulletedList6 = "TVScheme_BulletedList6";

		// Token: 0x0400062B RID: 1579
		internal const string TVScheme_Arrows = "TVScheme_Arrows";

		// Token: 0x0400062C RID: 1580
		internal const string TVScheme_Arrows2 = "TVScheme_Arrows2";

		// Token: 0x0400062D RID: 1581
		internal const string TVScheme_TOC = "TVScheme_TOC";

		// Token: 0x0400062E RID: 1582
		internal const string TVScheme_News = "TVScheme_News";

		// Token: 0x0400062F RID: 1583
		internal const string TVScheme_Contacts = "TVScheme_Contacts";

		// Token: 0x04000630 RID: 1584
		internal const string TVScheme_Inbox = "TVScheme_Inbox";

		// Token: 0x04000631 RID: 1585
		internal const string TVScheme_Events = "TVScheme_Events";

		// Token: 0x04000632 RID: 1586
		internal const string TVScheme_FAQ = "TVScheme_FAQ";

		// Token: 0x04000633 RID: 1587
		internal const string UserControlDesigner_MissingID = "UserControlDesigner_MissingID";

		// Token: 0x04000634 RID: 1588
		internal const string UserControlDesigner_EditUserControl = "UserControlDesigner_EditUserControl";

		// Token: 0x04000635 RID: 1589
		internal const string UserControlDesigner_Refresh = "UserControlDesigner_Refresh";

		// Token: 0x04000636 RID: 1590
		internal const string UserControlDesigner_NotFound = "UserControlDesigner_NotFound";

		// Token: 0x04000637 RID: 1591
		internal const string UserControlDesigner_CyclicError = "UserControlDesigner_CyclicError";

		// Token: 0x04000638 RID: 1592
		internal const string WebPartScheme_Empty = "WebPartScheme_Empty";

		// Token: 0x04000639 RID: 1593
		internal const string WebPartScheme_Professional = "WebPartScheme_Professional";

		// Token: 0x0400063A RID: 1594
		internal const string WebPartScheme_Simple = "WebPartScheme_Simple";

		// Token: 0x0400063B RID: 1595
		internal const string WebPartScheme_Classic = "WebPartScheme_Classic";

		// Token: 0x0400063C RID: 1596
		internal const string WebPartScheme_Colorful = "WebPartScheme_Colorful";

		// Token: 0x0400063D RID: 1597
		internal const string CatalogZoneDesigner_OnlyCatalogParts = "CatalogZoneDesigner_OnlyCatalogParts";

		// Token: 0x0400063E RID: 1598
		internal const string CatalogZoneDesigner_Empty = "CatalogZoneDesigner_Empty";

		// Token: 0x0400063F RID: 1599
		internal const string DesignerCatalogPartChrome_TypeCatalogPart = "DesignerCatalogPartChrome_TypeCatalogPart";

		// Token: 0x04000640 RID: 1600
		internal const string DesignerEditorPartChrome_TypeEditorPart = "DesignerEditorPartChrome_TypeEditorPart";

		// Token: 0x04000641 RID: 1601
		internal const string EditorZoneDesigner_OnlyEditorParts = "EditorZoneDesigner_OnlyEditorParts";

		// Token: 0x04000642 RID: 1602
		internal const string EditorZoneDesigner_Empty = "EditorZoneDesigner_Empty";

		// Token: 0x04000643 RID: 1603
		internal const string DeclarativeCatalogPartDesigner_Empty = "DeclarativeCatalogPartDesigner_Empty";

		// Token: 0x04000644 RID: 1604
		internal const string ToolZoneDesigner_ViewInBrowseMode = "ToolZoneDesigner_ViewInBrowseMode";

		// Token: 0x04000645 RID: 1605
		internal const string ToolZoneDesigner_ViewInBrowseModeDesc = "ToolZoneDesigner_ViewInBrowseModeDesc";

		// Token: 0x04000646 RID: 1606
		internal const string WebPartZoneAutoFormat_SampleWebPartTitle = "WebPartZoneAutoFormat_SampleWebPartTitle";

		// Token: 0x04000647 RID: 1607
		internal const string WebPartZoneAutoFormat_SampleWebPartContents = "WebPartZoneAutoFormat_SampleWebPartContents";

		// Token: 0x04000648 RID: 1608
		internal const string CatalogZone_SampleWebPartTitle = "CatalogZone_SampleWebPartTitle";

		// Token: 0x04000649 RID: 1609
		internal const string WebPartZoneDesigner_Empty = "WebPartZoneDesigner_Empty";

		// Token: 0x0400064A RID: 1610
		internal const string WebPartZoneDesigner_Error = "WebPartZoneDesigner_Error";

		// Token: 0x0400064B RID: 1611
		internal const string RTL = "RTL";

		// Token: 0x0400064C RID: 1612
		internal const string Sample_Column = "Sample_Column";

		// Token: 0x0400064D RID: 1613
		internal const string Sample_Databound_Column = "Sample_Databound_Column";

		// Token: 0x0400064E RID: 1614
		internal const string Sample_Databound_Text = "Sample_Databound_Text";

		// Token: 0x0400064F RID: 1615
		internal const string Sample_Databound_Text_Alt = "Sample_Databound_Text_Alt";

		// Token: 0x04000650 RID: 1616
		internal const string Sample_Unbound_Text = "Sample_Unbound_Text";

		// Token: 0x04000651 RID: 1617
		internal const string DesignTimeData_BadDataMember = "DesignTimeData_BadDataMember";

		// Token: 0x04000652 RID: 1618
		internal const string TrayLineUpIcons = "TrayLineUpIcons";

		// Token: 0x04000653 RID: 1619
		internal const string TrayAutoArrange = "TrayAutoArrange";

		// Token: 0x04000654 RID: 1620
		internal const string TrayShowLargeIcons = "TrayShowLargeIcons";

		// Token: 0x04000655 RID: 1621
		internal const string StringDictionaryEditorTitle = "StringDictionaryEditorTitle";

		// Token: 0x04000656 RID: 1622
		internal const string StartFileNameEditorTitle = "StartFileNameEditorTitle";

		// Token: 0x04000657 RID: 1623
		internal const string StartFileNameEditorAllFiles = "StartFileNameEditorAllFiles";

		// Token: 0x04000658 RID: 1624
		internal const string ToolStripItemCollectionEditorVerb = "ToolStripItemCollectionEditorVerb";

		// Token: 0x04000659 RID: 1625
		internal const string ToolStripDropDownItemCollectionEditorVerb = "ToolStripDropDownItemCollectionEditorVerb";

		// Token: 0x0400065A RID: 1626
		internal const string ToolStripItemCollectionEditorLabelNone = "ToolStripItemCollectionEditorLabelNone";

		// Token: 0x0400065B RID: 1627
		internal const string ToolStripItemCollectionEditorLabelMultipleItems = "ToolStripItemCollectionEditorLabelMultipleItems";

		// Token: 0x0400065C RID: 1628
		internal const string ContextMenuViewCode = "ContextMenuViewCode";

		// Token: 0x0400065D RID: 1629
		internal const string ContextMenuDocumentOutline = "ContextMenuDocumentOutline";

		// Token: 0x0400065E RID: 1630
		internal const string ContextMenuBringToFront = "ContextMenuBringToFront";

		// Token: 0x0400065F RID: 1631
		internal const string ContextMenuSendToBack = "ContextMenuSendToBack";

		// Token: 0x04000660 RID: 1632
		internal const string ContextMenuAlignToGrid = "ContextMenuAlignToGrid";

		// Token: 0x04000661 RID: 1633
		internal const string ContextMenuLockControls = "ContextMenuLockControls";

		// Token: 0x04000662 RID: 1634
		internal const string ContextMenuSelect = "ContextMenuSelect";

		// Token: 0x04000663 RID: 1635
		internal const string ContextMenuCut = "ContextMenuCut";

		// Token: 0x04000664 RID: 1636
		internal const string ContextMenuCopy = "ContextMenuCopy";

		// Token: 0x04000665 RID: 1637
		internal const string ContextMenuPaste = "ContextMenuPaste";

		// Token: 0x04000666 RID: 1638
		internal const string ContextMenuDelete = "ContextMenuDelete";

		// Token: 0x04000667 RID: 1639
		internal const string ContextMenuProperties = "ContextMenuProperties";

		// Token: 0x04000668 RID: 1640
		internal const string ToolStripItemContextMenuSetImage = "ToolStripItemContextMenuSetImage";

		// Token: 0x04000669 RID: 1641
		internal const string ToolStripItemContextMenuConvertTo = "ToolStripItemContextMenuConvertTo";

		// Token: 0x0400066A RID: 1642
		internal const string ToolStripItemContextMenuInsert = "ToolStripItemContextMenuInsert";

		// Token: 0x0400066B RID: 1643
		internal const string ToolStripActionList_Name = "ToolStripActionList_Name";

		// Token: 0x0400066C RID: 1644
		internal const string ToolStripActionList_NameDesc = "ToolStripActionList_NameDesc";

		// Token: 0x0400066D RID: 1645
		internal const string ToolStripActionList_Behavior = "ToolStripActionList_Behavior";

		// Token: 0x0400066E RID: 1646
		internal const string ToolStripActionList_Visible = "ToolStripActionList_Visible";

		// Token: 0x0400066F RID: 1647
		internal const string ToolStripActionList_VisibleDesc = "ToolStripActionList_VisibleDesc";

		// Token: 0x04000670 RID: 1648
		internal const string ToolStripActionList_ShowItemToolTips = "ToolStripActionList_ShowItemToolTips";

		// Token: 0x04000671 RID: 1649
		internal const string ToolStripActionList_ShowItemToolTipsDesc = "ToolStripActionList_ShowItemToolTipsDesc";

		// Token: 0x04000672 RID: 1650
		internal const string ToolStripActionList_AllowItemReorder = "ToolStripActionList_AllowItemReorder";

		// Token: 0x04000673 RID: 1651
		internal const string ToolStripActionList_AllowItemReorderDesc = "ToolStripActionList_AllowItemReorderDesc";

		// Token: 0x04000674 RID: 1652
		internal const string ToolStripActionList_CanOverflow = "ToolStripActionList_CanOverflow";

		// Token: 0x04000675 RID: 1653
		internal const string ToolStripActionList_CanOverflowDesc = "ToolStripActionList_CanOverflowDesc";

		// Token: 0x04000676 RID: 1654
		internal const string ToolStripActionList_Layout = "ToolStripActionList_Layout";

		// Token: 0x04000677 RID: 1655
		internal const string ToolStripActionList_Dock = "ToolStripActionList_Dock";

		// Token: 0x04000678 RID: 1656
		internal const string ToolStripActionList_DockDesc = "ToolStripActionList_DockDesc";

		// Token: 0x04000679 RID: 1657
		internal const string ToolStripActionList_Raft = "ToolStripActionList_Raft";

		// Token: 0x0400067A RID: 1658
		internal const string ToolStripActionList_RaftDesc = "ToolStripActionList_RaftDesc";

		// Token: 0x0400067B RID: 1659
		internal const string ToolStripActionList_RenderMode = "ToolStripActionList_RenderMode";

		// Token: 0x0400067C RID: 1660
		internal const string ToolStripActionList_RenderModeDesc = "ToolStripActionList_RenderModeDesc";

		// Token: 0x0400067D RID: 1661
		internal const string ToolStripActionList_GripStyle = "ToolStripActionList_GripStyle";

		// Token: 0x0400067E RID: 1662
		internal const string ToolStripActionList_GripStyleDesc = "ToolStripActionList_GripStyleDesc";

		// Token: 0x0400067F RID: 1663
		internal const string ToolStripActionList_Stretch = "ToolStripActionList_Stretch";

		// Token: 0x04000680 RID: 1664
		internal const string ToolStripActionList_StretchDesc = "ToolStripActionList_StretchDesc";

		// Token: 0x04000681 RID: 1665
		internal const string ToolStripActionList_SizingGrip = "ToolStripActionList_SizingGrip";

		// Token: 0x04000682 RID: 1666
		internal const string ToolStripActionList_SizingGripDesc = "ToolStripActionList_SizingGripDesc";

		// Token: 0x04000683 RID: 1667
		internal const string ToolStripContainerActionList_Show = "ToolStripContainerActionList_Show";

		// Token: 0x04000684 RID: 1668
		internal const string ToolStripContainerActionList_Visible = "ToolStripContainerActionList_Visible";

		// Token: 0x04000685 RID: 1669
		internal const string ToolStripContainerActionList_Top = "ToolStripContainerActionList_Top";

		// Token: 0x04000686 RID: 1670
		internal const string ToolStripContainerActionList_TopDesc = "ToolStripContainerActionList_TopDesc";

		// Token: 0x04000687 RID: 1671
		internal const string ToolStripContainerActionList_Bottom = "ToolStripContainerActionList_Bottom";

		// Token: 0x04000688 RID: 1672
		internal const string ToolStripContainerActionList_BottomDesc = "ToolStripContainerActionList_BottomDesc";

		// Token: 0x04000689 RID: 1673
		internal const string ToolStripContainerActionList_Left = "ToolStripContainerActionList_Left";

		// Token: 0x0400068A RID: 1674
		internal const string ToolStripContainerActionList_LeftDesc = "ToolStripContainerActionList_LeftDesc";

		// Token: 0x0400068B RID: 1675
		internal const string ToolStripContainerActionList_Right = "ToolStripContainerActionList_Right";

		// Token: 0x0400068C RID: 1676
		internal const string ToolStripContainerActionList_RightDesc = "ToolStripContainerActionList_RightDesc";

		// Token: 0x0400068D RID: 1677
		internal const string ContextMenuStripActionList_ShowImageMargin = "ContextMenuStripActionList_ShowImageMargin";

		// Token: 0x0400068E RID: 1678
		internal const string ContextMenuStripActionList_ShowImageMarginDesc = "ContextMenuStripActionList_ShowImageMarginDesc";

		// Token: 0x0400068F RID: 1679
		internal const string ContextMenuStripActionList_ShowCheckMargin = "ContextMenuStripActionList_ShowCheckMargin";

		// Token: 0x04000690 RID: 1680
		internal const string ContextMenuStripActionList_ShowCheckMarginDesc = "ContextMenuStripActionList_ShowCheckMarginDesc";

		// Token: 0x04000691 RID: 1681
		internal const string ContextMenuStripActionList_ShowShortCuts = "ContextMenuStripActionList_ShowShortCuts";

		// Token: 0x04000692 RID: 1682
		internal const string ContextMenuStripActionList_ShowShortCutsDesc = "ContextMenuStripActionList_ShowShortCutsDesc";

		// Token: 0x04000693 RID: 1683
		internal const string ToolStripDesignerTransactionAddingItem = "ToolStripDesignerTransactionAddingItem";

		// Token: 0x04000694 RID: 1684
		internal const string ToolStripDesignerTransactionRemovingItem = "ToolStripDesignerTransactionRemovingItem";

		// Token: 0x04000695 RID: 1685
		internal const string ToolStripDesignerSelectToolStripTransaction = "ToolStripDesignerSelectToolStripTransaction";

		// Token: 0x04000696 RID: 1686
		internal const string ToolStripDesignerStandardItemsVerb = "ToolStripDesignerStandardItemsVerb";

		// Token: 0x04000697 RID: 1687
		internal const string ToolStripDesignerEmbedVerb = "ToolStripDesignerEmbedVerb";

		// Token: 0x04000698 RID: 1688
		internal const string ToolStripDesignerStandardItemsVerbDesc = "ToolStripDesignerStandardItemsVerbDesc";

		// Token: 0x04000699 RID: 1689
		internal const string ToolStripDesignerEmbedVerbDesc = "ToolStripDesignerEmbedVerbDesc";

		// Token: 0x0400069A RID: 1690
		internal const string ToolStripDesignerInsertItemsVerb = "ToolStripDesignerInsertItemsVerb";

		// Token: 0x0400069B RID: 1691
		internal const string ToolStripAddingItem = "ToolStripAddingItem";

		// Token: 0x0400069C RID: 1692
		internal const string ToolStripDesignerSelectAllVerb = "ToolStripDesignerSelectAllVerb";

		// Token: 0x0400069D RID: 1693
		internal const string ToolStripSeparatorError = "ToolStripSeparatorError";

		// Token: 0x0400069E RID: 1694
		internal const string ToolStripCircularReferenceError = "ToolStripCircularReferenceError";

		// Token: 0x0400069F RID: 1695
		internal const string ToolStripDesignerTemplateNodeEnterText = "ToolStripDesignerTemplateNodeEnterText";

		// Token: 0x040006A0 RID: 1696
		internal const string ToolStripDesignerTemplateNodeSplitButtonToolTip = "ToolStripDesignerTemplateNodeSplitButtonToolTip";

		// Token: 0x040006A1 RID: 1697
		internal const string ToolStripDesignerTemplateNodeLabelToolTip = "ToolStripDesignerTemplateNodeLabelToolTip";

		// Token: 0x040006A2 RID: 1698
		internal const string ToolStripDesignerTemplateNodeSplitButtonStatusStripToolTip = "ToolStripDesignerTemplateNodeSplitButtonStatusStripToolTip";

		// Token: 0x040006A3 RID: 1699
		internal const string ToolStripDesignerFailedToLoadItemType = "ToolStripDesignerFailedToLoadItemType";

		// Token: 0x040006A4 RID: 1700
		internal const string ToolStripDesignerToolStripItemsOnly = "ToolStripDesignerToolStripItemsOnly";

		// Token: 0x040006A5 RID: 1701
		internal const string StandardMenuTitle = "StandardMenuTitle";

		// Token: 0x040006A6 RID: 1702
		internal const string StandardMenuStripTitle = "StandardMenuStripTitle";

		// Token: 0x040006A7 RID: 1703
		internal const string StandardMenuFile = "StandardMenuFile";

		// Token: 0x040006A8 RID: 1704
		internal const string StandardMenuNew = "StandardMenuNew";

		// Token: 0x040006A9 RID: 1705
		internal const string StandardMenuOpen = "StandardMenuOpen";

		// Token: 0x040006AA RID: 1706
		internal const string StandardMenuSave = "StandardMenuSave";

		// Token: 0x040006AB RID: 1707
		internal const string StandardMenuSaveAs = "StandardMenuSaveAs";

		// Token: 0x040006AC RID: 1708
		internal const string StandardMenuPrint = "StandardMenuPrint";

		// Token: 0x040006AD RID: 1709
		internal const string StandardMenuPrintPreview = "StandardMenuPrintPreview";

		// Token: 0x040006AE RID: 1710
		internal const string StandardMenuExit = "StandardMenuExit";

		// Token: 0x040006AF RID: 1711
		internal const string StandardMenuEdit = "StandardMenuEdit";

		// Token: 0x040006B0 RID: 1712
		internal const string StandardMenuUndo = "StandardMenuUndo";

		// Token: 0x040006B1 RID: 1713
		internal const string StandardMenuRedo = "StandardMenuRedo";

		// Token: 0x040006B2 RID: 1714
		internal const string StandardMenuCut = "StandardMenuCut";

		// Token: 0x040006B3 RID: 1715
		internal const string StandardToolCut = "StandardToolCut";

		// Token: 0x040006B4 RID: 1716
		internal const string StandardMenuCopy = "StandardMenuCopy";

		// Token: 0x040006B5 RID: 1717
		internal const string StandardMenuPaste = "StandardMenuPaste";

		// Token: 0x040006B6 RID: 1718
		internal const string StandardMenuDelete = "StandardMenuDelete";

		// Token: 0x040006B7 RID: 1719
		internal const string StandardMenuSelectAll = "StandardMenuSelectAll";

		// Token: 0x040006B8 RID: 1720
		internal const string StandardMenuTools = "StandardMenuTools";

		// Token: 0x040006B9 RID: 1721
		internal const string StandardMenuCustomize = "StandardMenuCustomize";

		// Token: 0x040006BA RID: 1722
		internal const string StandardMenuOptions = "StandardMenuOptions";

		// Token: 0x040006BB RID: 1723
		internal const string StandardMenuHelp = "StandardMenuHelp";

		// Token: 0x040006BC RID: 1724
		internal const string StandardToolHelp = "StandardToolHelp";

		// Token: 0x040006BD RID: 1725
		internal const string StandardMenuContents = "StandardMenuContents";

		// Token: 0x040006BE RID: 1726
		internal const string StandardMenuIndex = "StandardMenuIndex";

		// Token: 0x040006BF RID: 1727
		internal const string StandardMenuSearch = "StandardMenuSearch";

		// Token: 0x040006C0 RID: 1728
		internal const string StandardMenuAbout = "StandardMenuAbout";

		// Token: 0x040006C1 RID: 1729
		internal const string StandardMenuCreateDesc = "StandardMenuCreateDesc";

		// Token: 0x040006C2 RID: 1730
		internal const string CG_DataSetGeneratorFail_InputFileEmpty = "CG_DataSetGeneratorFail_InputFileEmpty";

		// Token: 0x040006C3 RID: 1731
		internal const string CG_DataSetGeneratorFail_DatasetNull = "CG_DataSetGeneratorFail_DatasetNull";

		// Token: 0x040006C4 RID: 1732
		internal const string CG_DataSetGeneratorFail_CodeGeneratorNull = "CG_DataSetGeneratorFail_CodeGeneratorNull";

		// Token: 0x040006C5 RID: 1733
		internal const string CG_DataSetGeneratorFail_CodeNamespaceNull = "CG_DataSetGeneratorFail_CodeNamespaceNull";

		// Token: 0x040006C6 RID: 1734
		internal const string CG_DataSetGeneratorFail_UnableToConvertToDataSet = "CG_DataSetGeneratorFail_UnableToConvertToDataSet";

		// Token: 0x040006C7 RID: 1735
		internal const string CG_DataSetGeneratorFail_FailToGenerateCode = "CG_DataSetGeneratorFail_FailToGenerateCode";

		// Token: 0x040006C8 RID: 1736
		internal const string CG_TypeCantBeNull = "CG_TypeCantBeNull";

		// Token: 0x040006C9 RID: 1737
		internal const string CG_NoCtor0 = "CG_NoCtor0";

		// Token: 0x040006CA RID: 1738
		internal const string CG_NoCtor1 = "CG_NoCtor1";

		// Token: 0x040006CB RID: 1739
		internal const string CG_MainSelectCommandNotSet = "CG_MainSelectCommandNotSet";

		// Token: 0x040006CC RID: 1740
		internal const string CG_UnableToReadExtProperties = "CG_UnableToReadExtProperties";

		// Token: 0x040006CD RID: 1741
		internal const string CG_UnableToConvertSqlDbTypeToSqlType = "CG_UnableToConvertSqlDbTypeToSqlType";

		// Token: 0x040006CE RID: 1742
		internal const string CG_UnableToConvertDbTypeToUrtType = "CG_UnableToConvertDbTypeToUrtType";

		// Token: 0x040006CF RID: 1743
		internal const string CG_RowColumnPropertyNameFixup = "CG_RowColumnPropertyNameFixup";

		// Token: 0x040006D0 RID: 1744
		internal const string CG_DataSourceClassNameFixup = "CG_DataSourceClassNameFixup";

		// Token: 0x040006D1 RID: 1745
		internal const string CG_TablePropertyNameFixup = "CG_TablePropertyNameFixup";

		// Token: 0x040006D2 RID: 1746
		internal const string CG_TableSourceNameFixup = "CG_TableSourceNameFixup";

		// Token: 0x040006D3 RID: 1747
		internal const string CG_EmptyDSName = "CG_EmptyDSName";

		// Token: 0x040006D4 RID: 1748
		internal const string CG_ColumnIsDBNull = "CG_ColumnIsDBNull";

		// Token: 0x040006D5 RID: 1749
		internal const string CG_ParameterIsDBNull = "CG_ParameterIsDBNull";

		// Token: 0x040006D6 RID: 1750
		internal const string CG_TableAdapterManagerNeedsSameConnString = "CG_TableAdapterManagerNeedsSameConnString";

		// Token: 0x040006D7 RID: 1751
		internal const string CG_TableAdapterManagerHasNoConnection = "CG_TableAdapterManagerHasNoConnection";

		// Token: 0x040006D8 RID: 1752
		internal const string CG_TableAdapterManagerNotSupportTransaction = "CG_TableAdapterManagerNotSupportTransaction";

		// Token: 0x040006D9 RID: 1753
		internal const string DTDS_CouldNotDeserializeConnection = "DTDS_CouldNotDeserializeConnection";

		// Token: 0x040006DA RID: 1754
		internal const string DTDS_CouldNotDeserializeXmlElement = "DTDS_CouldNotDeserializeXmlElement";

		// Token: 0x040006DB RID: 1755
		internal const string DTDS_NameIsRequired = "DTDS_NameIsRequired";

		// Token: 0x040006DC RID: 1756
		internal const string DTDS_NameConflict = "DTDS_NameConflict";

		// Token: 0x040006DD RID: 1757
		internal const string DTDS_TableNotMatch = "DTDS_TableNotMatch";

		// Token: 0x040006DE RID: 1758
		internal const string DD_E_TableDirectValidForOleDbOnly = "DD_E_TableDirectValidForOleDbOnly";

		// Token: 0x040006DF RID: 1759
		internal const string CM_NameNotEmptyExcption = "CM_NameNotEmptyExcption";

		// Token: 0x040006E0 RID: 1760
		internal const string CM_NameTooLongExcption = "CM_NameTooLongExcption";

		// Token: 0x040006E1 RID: 1761
		internal const string CM_NameInvalid = "CM_NameInvalid";

		// Token: 0x040006E2 RID: 1762
		internal const string CM_NameExist = "CM_NameExist";

		// Token: 0x040006E3 RID: 1763
		internal const string PropertiesCategoryName = "PropertiesCategoryName";

		// Token: 0x040006E4 RID: 1764
		internal const string LinksCategoryName = "LinksCategoryName";

		// Token: 0x040006E5 RID: 1765
		internal const string ItemsCategoryName = "ItemsCategoryName";

		// Token: 0x040006E6 RID: 1766
		internal const string DataCategoryName = "DataCategoryName";

		// Token: 0x040006E7 RID: 1767
		internal const string ImageListActionListImageSizeDisplayName = "ImageListActionListImageSizeDisplayName";

		// Token: 0x040006E8 RID: 1768
		internal const string ImageListActionListImageSizeDescription = "ImageListActionListImageSizeDescription";

		// Token: 0x040006E9 RID: 1769
		internal const string ImageListActionListColorDepthDisplayName = "ImageListActionListColorDepthDisplayName";

		// Token: 0x040006EA RID: 1770
		internal const string ImageListActionListColorDepthDescription = "ImageListActionListColorDepthDescription";

		// Token: 0x040006EB RID: 1771
		internal const string ImageListActionListChooseImagesDisplayName = "ImageListActionListChooseImagesDisplayName";

		// Token: 0x040006EC RID: 1772
		internal const string ImageListActionListChooseImagesDescription = "ImageListActionListChooseImagesDescription";

		// Token: 0x040006ED RID: 1773
		internal const string ListControlUnboundActionListEditItemsDisplayName = "ListControlUnboundActionListEditItemsDisplayName";

		// Token: 0x040006EE RID: 1774
		internal const string ListControlUnboundActionListEditItemsDescription = "ListControlUnboundActionListEditItemsDescription";

		// Token: 0x040006EF RID: 1775
		internal const string ListViewActionListEditItemsDisplayName = "ListViewActionListEditItemsDisplayName";

		// Token: 0x040006F0 RID: 1776
		internal const string ListViewActionListEditItemsDescription = "ListViewActionListEditItemsDescription";

		// Token: 0x040006F1 RID: 1777
		internal const string ListViewActionListEditColumnsDisplayName = "ListViewActionListEditColumnsDisplayName";

		// Token: 0x040006F2 RID: 1778
		internal const string ListViewActionListEditColumnsDescription = "ListViewActionListEditColumnsDescription";

		// Token: 0x040006F3 RID: 1779
		internal const string ListViewActionListEditGroupsDisplayName = "ListViewActionListEditGroupsDisplayName";

		// Token: 0x040006F4 RID: 1780
		internal const string ListViewActionListEditGroupsDescription = "ListViewActionListEditGroupsDescription";

		// Token: 0x040006F5 RID: 1781
		internal const string ListViewActionListViewDisplayName = "ListViewActionListViewDisplayName";

		// Token: 0x040006F6 RID: 1782
		internal const string ListViewActionListViewDescription = "ListViewActionListViewDescription";

		// Token: 0x040006F7 RID: 1783
		internal const string ListViewActionListSmallImagesDisplayName = "ListViewActionListSmallImagesDisplayName";

		// Token: 0x040006F8 RID: 1784
		internal const string ListViewActionListSmallImagesDescription = "ListViewActionListSmallImagesDescription";

		// Token: 0x040006F9 RID: 1785
		internal const string ListViewActionListLargeImagesDisplayName = "ListViewActionListLargeImagesDisplayName";

		// Token: 0x040006FA RID: 1786
		internal const string ListViewActionListLargeImagesDescription = "ListViewActionListLargeImagesDescription";

		// Token: 0x040006FB RID: 1787
		internal const string BoundModeHeader = "BoundModeHeader";

		// Token: 0x040006FC RID: 1788
		internal const string UnBoundModeHeader = "UnBoundModeHeader";

		// Token: 0x040006FD RID: 1789
		internal const string BoundModeDisplayName = "BoundModeDisplayName";

		// Token: 0x040006FE RID: 1790
		internal const string BoundModeDescription = "BoundModeDescription";

		// Token: 0x040006FF RID: 1791
		internal const string DataSourceDisplayName = "DataSourceDisplayName";

		// Token: 0x04000700 RID: 1792
		internal const string DataSourceDescription = "DataSourceDescription";

		// Token: 0x04000701 RID: 1793
		internal const string DisplayMemberDisplayName = "DisplayMemberDisplayName";

		// Token: 0x04000702 RID: 1794
		internal const string DisplayMemberDescription = "DisplayMemberDescription";

		// Token: 0x04000703 RID: 1795
		internal const string ValueMemberDisplayName = "ValueMemberDisplayName";

		// Token: 0x04000704 RID: 1796
		internal const string ValueMemberDescription = "ValueMemberDescription";

		// Token: 0x04000705 RID: 1797
		internal const string BoundSelectedValueDisplayName = "BoundSelectedValueDisplayName";

		// Token: 0x04000706 RID: 1798
		internal const string BoundSelectedValueDescription = "BoundSelectedValueDescription";

		// Token: 0x04000707 RID: 1799
		internal const string EditItemDisplayName = "EditItemDisplayName";

		// Token: 0x04000708 RID: 1800
		internal const string EditItemDescription = "EditItemDescription";

		// Token: 0x04000709 RID: 1801
		internal const string ChooseImageDisplayName = "ChooseImageDisplayName";

		// Token: 0x0400070A RID: 1802
		internal const string ChooseImageDescription = "ChooseImageDescription";

		// Token: 0x0400070B RID: 1803
		internal const string SizeModeDisplayName = "SizeModeDisplayName";

		// Token: 0x0400070C RID: 1804
		internal const string SizeModeDescription = "SizeModeDescription";

		// Token: 0x0400070D RID: 1805
		internal const string EditLinesDisplayName = "EditLinesDisplayName";

		// Token: 0x0400070E RID: 1806
		internal const string EditLinesDescription = "EditLinesDescription";

		// Token: 0x0400070F RID: 1807
		internal const string MultiLineDisplayName = "MultiLineDisplayName";

		// Token: 0x04000710 RID: 1808
		internal const string MultiLineDescription = "MultiLineDescription";

		// Token: 0x04000711 RID: 1809
		internal const string ChooseIconDisplayName = "ChooseIconDisplayName";

		// Token: 0x04000712 RID: 1810
		internal const string InvokeNodesDialogDisplayName = "InvokeNodesDialogDisplayName";

		// Token: 0x04000713 RID: 1811
		internal const string InvokeNodesDialogDescription = "InvokeNodesDialogDescription";

		// Token: 0x04000714 RID: 1812
		internal const string ImageListDisplayName = "ImageListDisplayName";

		// Token: 0x04000715 RID: 1813
		internal const string ImageListDescription = "ImageListDescription";

		// Token: 0x04000716 RID: 1814
		internal const string DesignerOptions_LayoutSettings = "DesignerOptions_LayoutSettings";

		// Token: 0x04000717 RID: 1815
		internal const string DesignerOptions_ObjectBoundSmartTagSettings = "DesignerOptions_ObjectBoundSmartTagSettings";

		// Token: 0x04000718 RID: 1816
		internal const string DesignerOptions_GridSizeDesc = "DesignerOptions_GridSizeDesc";

		// Token: 0x04000719 RID: 1817
		internal const string DesignerOptions_ShowGridDesc = "DesignerOptions_ShowGridDesc";

		// Token: 0x0400071A RID: 1818
		internal const string DesignerOptions_SnapToGridDesc = "DesignerOptions_SnapToGridDesc";

		// Token: 0x0400071B RID: 1819
		internal const string DesignerOptions_UseSnapLines = "DesignerOptions_UseSnapLines";

		// Token: 0x0400071C RID: 1820
		internal const string DesignerOptions_UseSmartTags = "DesignerOptions_UseSmartTags";

		// Token: 0x0400071D RID: 1821
		internal const string DesignerOptions_ObjectBoundSmartTagAutoShow = "DesignerOptions_ObjectBoundSmartTagAutoShow";

		// Token: 0x0400071E RID: 1822
		internal const string DesignerOptions_ObjectBoundSmartTagAutoShowDisplayName = "DesignerOptions_ObjectBoundSmartTagAutoShowDisplayName";

		// Token: 0x0400071F RID: 1823
		internal const string DesignerOptions_CodeGenSettings = "DesignerOptions_CodeGenSettings";

		// Token: 0x04000720 RID: 1824
		internal const string DesignerOptions_OptimizedCodeGen = "DesignerOptions_OptimizedCodeGen";

		// Token: 0x04000721 RID: 1825
		internal const string DesignerOptions_CodeGenDisplay = "DesignerOptions_CodeGenDisplay";

		// Token: 0x04000722 RID: 1826
		internal const string DesignerOptions_EnableInSituEditingDisplay = "DesignerOptions_EnableInSituEditingDisplay";

		// Token: 0x04000723 RID: 1827
		internal const string DesignerOptions_EnableInSituEditingCat = "DesignerOptions_EnableInSituEditingCat";

		// Token: 0x04000724 RID: 1828
		internal const string DesignerOptions_EnableInSituEditingDesc = "DesignerOptions_EnableInSituEditingDesc";

		// Token: 0x04000725 RID: 1829
		internal const string ClassComments1 = "ClassComments1";

		// Token: 0x04000726 RID: 1830
		internal const string ClassComments2 = "ClassComments2";

		// Token: 0x04000727 RID: 1831
		internal const string ClassComments3 = "ClassComments3";

		// Token: 0x04000728 RID: 1832
		internal const string ClassComments4 = "ClassComments4";

		// Token: 0x04000729 RID: 1833
		internal const string ClassDocComment = "ClassDocComment";

		// Token: 0x0400072A RID: 1834
		internal const string StringPropertyComment = "StringPropertyComment";

		// Token: 0x0400072B RID: 1835
		internal const string StringPropertyTruncatedComment = "StringPropertyTruncatedComment";

		// Token: 0x0400072C RID: 1836
		internal const string CulturePropertyComment1 = "CulturePropertyComment1";

		// Token: 0x0400072D RID: 1837
		internal const string CulturePropertyComment2 = "CulturePropertyComment2";

		// Token: 0x0400072E RID: 1838
		internal const string ResMgrPropertyComment = "ResMgrPropertyComment";

		// Token: 0x0400072F RID: 1839
		internal const string MismatchedResourceName = "MismatchedResourceName";

		// Token: 0x04000730 RID: 1840
		internal const string InvalidIdentifier = "InvalidIdentifier";

		// Token: 0x04000731 RID: 1841
		private static SR loader;

		// Token: 0x04000732 RID: 1842
		private ResourceManager resources;

		// Token: 0x04000733 RID: 1843
		private static object s_InternalSyncObject;
	}
}
