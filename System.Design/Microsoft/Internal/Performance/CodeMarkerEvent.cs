using System;

namespace Microsoft.Internal.Performance
{
	internal enum CodeMarkerEvent
	{
		perfFXDesignCreateComponentEnd = 7501,
		perfFXDesignPropertyBrowserPopulationStart,
		perfFXDesignPropertyBrowserPopulationEnd,
		perfFXDesignShowCode = 7505,
		perfFXDesignFromCodeToDesignStart,
		perfFXDesignFromCodeToDesign,
		perfFXDesignFlushStart,
		perfFXDesignFlushEnd,
		perfFXBindEventDesignToCode = 7511,
		perfFXGenerateCodeTreeEnd = 7513,
		perfFXIntegrateSerializedTreeEnd = 7515,
		perfFXOnLoadedStart,
		perfFXOnLoadedEnd,
		perfFXCreateEditorStart,
		perfFXCreateEditorEnd,
		perfFXParseEnd,
		perfFXPerformLoadStart,
		perfFXPerformLoadEnd,
		perfFXEmitMethodEnd,
		perfFXFormatMethodEnd,
		perfFXCodeGenerationEnd,
		perfFXGetDocumentType,
		perfFXDeserializeStart,
		perfFXDeserializeEnd,
		perfFXGetFileDocDataStart,
		perfFXGetFileDocDataEnd,
		perfFXCreateDesignerStart,
		perfFXCreateDesignerEnd,
		perfFXCreateDesignSurface,
		perfFXCreateDesignSurfaceEnd,
		perfFXNotifyStartupServices,
		perfFXNotifyStartupServicesEnd,
		perfFXGetGlobalObjects,
		perfFXGetGlobalObjectsEnd,
		perfFXDesignPropertyBrowserCreate,
		perfFXDesignPropertyBrowserCreateEnd,
		perfFXDesignPropertyBrowserLoadState,
		perfFXDesignPBOnSelectionChanged,
		perfFXDesignPBOnSelectionChangedEnd
	}
}
