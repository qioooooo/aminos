﻿using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Xml
{
	internal sealed class Res
	{
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

		internal Res()
		{
			this.resources = new ResourceManager("System.Xml", base.GetType().Assembly);
		}

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

		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		public static ResourceManager Resources
		{
			get
			{
				return Res.GetLoader().resources;
			}
		}

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

		public static string GetString(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetString(name, Res.Culture);
		}

		public static object GetObject(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetObject(name, Res.Culture);
		}

		internal const string Xml_UserException = "Xml_UserException";

		internal const string Xml_DefaultException = "Xml_DefaultException";

		internal const string Xml_InvalidOperation = "Xml_InvalidOperation";

		internal const string Xml_StackOverflow = "Xml_StackOverflow";

		internal const string Xml_ErrorFilePosition = "Xml_ErrorFilePosition";

		internal const string Xslt_NoStylesheetLoaded = "Xslt_NoStylesheetLoaded";

		internal const string Xslt_NotCompiledStylesheet = "Xslt_NotCompiledStylesheet";

		internal const string Xml_UnclosedQuote = "Xml_UnclosedQuote";

		internal const string Xml_UnexpectedEOF = "Xml_UnexpectedEOF";

		internal const string Xml_UnexpectedEOF1 = "Xml_UnexpectedEOF1";

		internal const string Xml_UnexpectedEOFInElementContent = "Xml_UnexpectedEOFInElementContent";

		internal const string Xml_BadStartNameChar = "Xml_BadStartNameChar";

		internal const string Xml_BadNameChar = "Xml_BadNameChar";

		internal const string Xml_BadNameCharWithPos = "Xml_BadNameCharWithPos";

		internal const string Xml_BadDecimalEntity = "Xml_BadDecimalEntity";

		internal const string Xml_BadHexEntity = "Xml_BadHexEntity";

		internal const string Xml_MissingByteOrderMark = "Xml_MissingByteOrderMark";

		internal const string Xml_UnknownEncoding = "Xml_UnknownEncoding";

		internal const string Xml_InternalError = "Xml_InternalError";

		internal const string Xml_InvalidCharInThisEncoding = "Xml_InvalidCharInThisEncoding";

		internal const string Xml_ErrorPosition = "Xml_ErrorPosition";

		internal const string Xml_UnexpectedTokenEx = "Xml_UnexpectedTokenEx";

		internal const string Xml_UnexpectedTokens2 = "Xml_UnexpectedTokens2";

		internal const string Xml_ExpectingWhiteSpace = "Xml_ExpectingWhiteSpace";

		internal const string Xml_TagMismatch = "Xml_TagMismatch";

		internal const string Xml_UnexpectedEndTag = "Xml_UnexpectedEndTag";

		internal const string Xml_UnknownNs = "Xml_UnknownNs";

		internal const string Xml_BadAttributeChar = "Xml_BadAttributeChar";

		internal const string Xml_MissingRoot = "Xml_MissingRoot";

		internal const string Xml_MultipleRoots = "Xml_MultipleRoots";

		internal const string Xml_InvalidRootData = "Xml_InvalidRootData";

		internal const string Xml_XmlDeclNotFirst = "Xml_XmlDeclNotFirst";

		internal const string Xml_InvalidXmlDecl = "Xml_InvalidXmlDecl";

		internal const string Xml_InvalidNodeType = "Xml_InvalidNodeType";

		internal const string Xml_InvalidPIName = "Xml_InvalidPIName";

		internal const string Xml_InvalidXmlSpace = "Xml_InvalidXmlSpace";

		internal const string Xml_InvalidVersionNumber = "Xml_InvalidVersionNumber";

		internal const string Xml_DupAttributeName = "Xml_DupAttributeName";

		internal const string Xml_BadDTDLocation = "Xml_BadDTDLocation";

		internal const string Xml_ElementNotFound = "Xml_ElementNotFound";

		internal const string Xml_ElementNotFoundNs = "Xml_ElementNotFoundNs";

		internal const string Xml_PartialContentNodeTypeNotSupportedEx = "Xml_PartialContentNodeTypeNotSupportedEx";

		internal const string Xml_MultipleDTDsProvided = "Xml_MultipleDTDsProvided";

		internal const string Xml_CanNotBindToReservedNamespace = "Xml_CanNotBindToReservedNamespace";

		internal const string Xml_XmlnsBelongsToReservedNs = "Xml_XmlnsBelongsToReservedNs";

		internal const string Xml_InvalidCharacter = "Xml_InvalidCharacter";

		internal const string Xml_ExpectDtdMarkup = "Xml_ExpectDtdMarkup";

		internal const string Xml_InvalidBinHexValue = "Xml_InvalidBinHexValue";

		internal const string Xml_InvalidBinHexValueOddCount = "Xml_InvalidBinHexValueOddCount";

		internal const string Xml_InvalidTextDecl = "Xml_InvalidTextDecl";

		internal const string Xml_InvalidBase64Value = "Xml_InvalidBase64Value";

		internal const string Xml_ExpectExternalOrPublicId = "Xml_ExpectExternalOrPublicId";

		internal const string Xml_ExpectExternalIdOrEntityValue = "Xml_ExpectExternalIdOrEntityValue";

		internal const string Xml_ExpectAttType = "Xml_ExpectAttType";

		internal const string Xml_ExpectIgnoreOrInclude = "Xml_ExpectIgnoreOrInclude";

		internal const string Xml_ExpectSubOrClose = "Xml_ExpectSubOrClose";

		internal const string Xml_ExpectExternalOrClose = "Xml_ExpectExternalOrClose";

		internal const string Xml_ExpectOp = "Xml_ExpectOp";

		internal const string Xml_ExpectNoWhitespace = "Xml_ExpectNoWhitespace";

		internal const string Xml_ExpectPcData = "Xml_ExpectPcData";

		internal const string Xml_UndeclaredParEntity = "Xml_UndeclaredParEntity";

		internal const string Xml_UndeclaredEntity = "Xml_UndeclaredEntity";

		internal const string Xml_RecursiveParEntity = "Xml_RecursiveParEntity";

		internal const string Xml_RecursiveGenEntity = "Xml_RecursiveGenEntity";

		internal const string Xml_ExternalEntityInAttValue = "Xml_ExternalEntityInAttValue";

		internal const string Xml_UnparsedEntityRef = "Xml_UnparsedEntityRef";

		internal const string Xml_InvalidConditionalSection = "Xml_InvalidConditionalSection";

		internal const string Xml_UnclosedConditionalSection = "Xml_UnclosedConditionalSection";

		internal const string Xml_InvalidParEntityRef = "Xml_InvalidParEntityRef";

		internal const string Xml_InvalidContentModel = "Xml_InvalidContentModel";

		internal const string Xml_InvalidXmlDocument = "Xml_InvalidXmlDocument";

		internal const string Xml_FragmentId = "Xml_FragmentId";

		internal const string Xml_ColonInLocalName = "Xml_ColonInLocalName";

		internal const string Xml_InvalidAttributeType = "Xml_InvalidAttributeType";

		internal const string Xml_InvalidAttributeType1 = "Xml_InvalidAttributeType1";

		internal const string Xml_UnexpectedCDataEnd = "Xml_UnexpectedCDataEnd";

		internal const string Xml_EnumerationRequired = "Xml_EnumerationRequired";

		internal const string Xml_NotSameNametable = "Xml_NotSameNametable";

		internal const string Xml_NametableMismatch = "Xml_NametableMismatch";

		internal const string Xml_NoDTDPresent = "Xml_NoDTDPresent";

		internal const string Xml_MultipleValidaitonTypes = "Xml_MultipleValidaitonTypes";

		internal const string Xml_BadNamespaceDecl = "Xml_BadNamespaceDecl";

		internal const string Xml_ErrorParsingEntityName = "Xml_ErrorParsingEntityName";

		internal const string Xml_NoValidation = "Xml_NoValidation";

		internal const string Xml_WhitespaceHandling = "Xml_WhitespaceHandling";

		internal const string Xml_InvalidResetStateCall = "Xml_InvalidResetStateCall";

		internal const string Xml_EntityHandling = "Xml_EntityHandling";

		internal const string Xml_InvalidNmToken = "Xml_InvalidNmToken";

		internal const string Xml_EntityRefNesting = "Xml_EntityRefNesting";

		internal const string Xml_CannotResolveEntity = "Xml_CannotResolveEntity";

		internal const string Xml_CannotResolveExternalSubset = "Xml_CannotResolveExternalSubset";

		internal const string Xml_CannotResolveUrl = "Xml_CannotResolveUrl";

		internal const string Xml_CDATAEndInText = "Xml_CDATAEndInText";

		internal const string Xml_ExternalEntityInStandAloneDocument = "Xml_ExternalEntityInStandAloneDocument";

		internal const string Xml_DtdAfterRootElement = "Xml_DtdAfterRootElement";

		internal const string Xml_ReadOnlyProperty = "Xml_ReadOnlyProperty";

		internal const string Xml_DtdIsProhibited = "Xml_DtdIsProhibited";

		internal const string Xml_DtdIsProhibitedEx = "Xml_DtdIsProhibitedEx";

		internal const string Xml_AttlistDuplEnumValue = "Xml_AttlistDuplEnumValue";

		internal const string Xml_AttlistDuplNotationValue = "Xml_AttlistDuplNotationValue";

		internal const string Xml_EncodingSwitchAfterResetState = "Xml_EncodingSwitchAfterResetState";

		internal const string Xml_ReadSubtreeNotOnElement = "Xml_ReadSubtreeNotOnElement";

		internal const string Xml_DtdNotAllowedInFragment = "Xml_DtdNotAllowedInFragment";

		internal const string Xml_CannotStartDocumentOnFragment = "Xml_CannotStartDocumentOnFragment";

		internal const string Xml_InvalidWhitespaceCharacter = "Xml_InvalidWhitespaceCharacter";

		internal const string Xml_IncompatibleConformanceLevel = "Xml_IncompatibleConformanceLevel";

		internal const string Xml_BinaryXmlReadAsText = "Xml_BinaryXmlReadAsText";

		internal const string Xml_UnexpectedNodeType = "Xml_UnexpectedNodeType";

		internal const string Xml_ErrorOpeningExternalDtd = "Xml_ErrorOpeningExternalDtd";

		internal const string Xml_ErrorOpeningExternalEntity = "Xml_ErrorOpeningExternalEntity";

		internal const string Xml_ReadBinaryContentNotSupported = "Xml_ReadBinaryContentNotSupported";

		internal const string Xml_ReadValueChunkNotSupported = "Xml_ReadValueChunkNotSupported";

		internal const string Xml_InvalidReadContentAs = "Xml_InvalidReadContentAs";

		internal const string Xml_InvalidReadElementContentAs = "Xml_InvalidReadElementContentAs";

		internal const string Xml_MixedReadElementContentAs = "Xml_MixedReadElementContentAs";

		internal const string Xml_MixingReadValueChunkWithBinary = "Xml_MixingReadValueChunkWithBinary";

		internal const string Xml_MixingBinaryContentMethods = "Xml_MixingBinaryContentMethods";

		internal const string Xml_MixingV1StreamingWithV2Binary = "Xml_MixingV1StreamingWithV2Binary";

		internal const string Xml_InvalidReadValueChunk = "Xml_InvalidReadValueChunk";

		internal const string Xml_ReadContentAsFormatException = "Xml_ReadContentAsFormatException";

		internal const string Xml_DoubleBaseUri = "Xml_DoubleBaseUri";

		internal const string Xml_NotEnoughSpaceForSurrogatePair = "Xml_NotEnoughSpaceForSurrogatePair";

		internal const string Xml_EmptyUrl = "Xml_EmptyUrl";

		internal const string Xml_UnexpectedNodeInSimpleContent = "Xml_UnexpectedNodeInSimpleContent";

		internal const string Xml_UnsupportedClass = "Xml_UnsupportedClass";

		internal const string Xml_NullResolver = "Xml_NullResolver";

		internal const string Xml_UntrustedCodeSettingResolver = "Xml_UntrustedCodeSettingResolver";

		internal const string Xml_InvalidQuote = "Xml_InvalidQuote";

		internal const string Xml_UndefPrefix = "Xml_UndefPrefix";

		internal const string Xml_PrefixForEmptyNs = "Xml_PrefixForEmptyNs";

		internal const string Xml_NoNamespaces = "Xml_NoNamespaces";

		internal const string Xml_InvalidCDataChars = "Xml_InvalidCDataChars";

		internal const string Xml_InvalidCommentChars = "Xml_InvalidCommentChars";

		internal const string Xml_NotTheFirst = "Xml_NotTheFirst";

		internal const string Xml_InvalidPiChars = "Xml_InvalidPiChars";

		internal const string Xml_UndefNamespace = "Xml_UndefNamespace";

		internal const string Xml_EmptyName = "Xml_EmptyName";

		internal const string Xml_EmptyLocalName = "Xml_EmptyLocalName";

		internal const string Xml_InvalidNameChars = "Xml_InvalidNameChars";

		internal const string Xml_InvalidNameCharsDetail = "Xml_InvalidNameCharsDetail";

		internal const string Xml_NoStartTag = "Xml_NoStartTag";

		internal const string Xml_Closed = "Xml_Closed";

		internal const string Xml_ClosedOrError = "Xml_ClosedOrError";

		internal const string Xml_WrongToken = "Xml_WrongToken";

		internal const string Xml_InvalidPrefix = "Xml_InvalidPrefix";

		internal const string Xml_XmlPrefix = "Xml_XmlPrefix";

		internal const string Xml_XmlnsPrefix = "Xml_XmlnsPrefix";

		internal const string Xml_NamespaceDeclXmlXmlns = "Xml_NamespaceDeclXmlXmlns";

		internal const string Xml_NonWhitespace = "Xml_NonWhitespace";

		internal const string Xml_DupXmlDecl = "Xml_DupXmlDecl";

		internal const string Xml_CannotWriteXmlDecl = "Xml_CannotWriteXmlDecl";

		internal const string Xml_NoRoot = "Xml_NoRoot";

		internal const string Xml_InvalidIndentation = "Xml_InvalidIndentation";

		internal const string Xml_NotInWriteState = "Xml_NotInWriteState";

		internal const string Xml_InvalidPosition = "Xml_InvalidPosition";

		internal const string Xml_IncompleteEntity = "Xml_IncompleteEntity";

		internal const string Xml_IncompleteDtdContent = "Xml_IncompleteDtdContent";

		internal const string Xml_InvalidSurrogateHighChar = "Xml_InvalidSurrogateHighChar";

		internal const string Xml_InvalidSurrogateMissingLowChar = "Xml_InvalidSurrogateMissingLowChar";

		internal const string Xml_InvalidSurrogatePairWithArgs = "Xml_InvalidSurrogatePairWithArgs";

		internal const string Xml_SurrogatePairSplit = "Xml_SurrogatePairSplit";

		internal const string Xml_NoMultipleRoots = "Xml_NoMultipleRoots";

		internal const string Xml_RedefinePrefix = "Xml_RedefinePrefix";

		internal const string Xml_DtdAlreadyWritten = "Xml_DtdAlreadyWritten";

		internal const string XmlBadName = "XmlBadName";

		internal const string XmlNoNameAllowed = "XmlNoNameAllowed";

		internal const string Xml_InvalidCharsInIndent = "Xml_InvalidCharsInIndent";

		internal const string Xml_IndentCharsNotWhitespace = "Xml_IndentCharsNotWhitespace";

		internal const string Xml_ConformanceLevelFragment = "Xml_ConformanceLevelFragment";

		internal const string XmlDocument_ValidateInvalidNodeType = "XmlDocument_ValidateInvalidNodeType";

		internal const string XmlDocument_NodeNotFromDocument = "XmlDocument_NodeNotFromDocument";

		internal const string XmlDocument_NoNodeSchemaInfo = "XmlDocument_NoNodeSchemaInfo";

		internal const string XmlDocument_NoSchemaInfo = "XmlDocument_NoSchemaInfo";

		internal const string Sch_DefaultException = "Sch_DefaultException";

		internal const string Sch_ParEntityRefNesting = "Sch_ParEntityRefNesting";

		internal const string Sch_DupElementDecl = "Sch_DupElementDecl";

		internal const string Sch_IdAttrDeclared = "Sch_IdAttrDeclared";

		internal const string Sch_RootMatchDocType = "Sch_RootMatchDocType";

		internal const string Sch_DupId = "Sch_DupId";

		internal const string Sch_UndeclaredElement = "Sch_UndeclaredElement";

		internal const string Sch_UndeclaredAttribute = "Sch_UndeclaredAttribute";

		internal const string Sch_UndeclaredNotation = "Sch_UndeclaredNotation";

		internal const string Sch_UndeclaredId = "Sch_UndeclaredId";

		internal const string Sch_SchemaRootExpected = "Sch_SchemaRootExpected";

		internal const string Sch_XSDSchemaRootExpected = "Sch_XSDSchemaRootExpected";

		internal const string Sch_UnsupportedAttribute = "Sch_UnsupportedAttribute";

		internal const string Sch_UnsupportedElement = "Sch_UnsupportedElement";

		internal const string Sch_MissAttribute = "Sch_MissAttribute";

		internal const string Sch_AnnotationLocation = "Sch_AnnotationLocation";

		internal const string Sch_DataTypeTextOnly = "Sch_DataTypeTextOnly";

		internal const string Sch_UnknownModel = "Sch_UnknownModel";

		internal const string Sch_UnknownOrder = "Sch_UnknownOrder";

		internal const string Sch_UnknownContent = "Sch_UnknownContent";

		internal const string Sch_UnknownRequired = "Sch_UnknownRequired";

		internal const string Sch_UnknownDtType = "Sch_UnknownDtType";

		internal const string Sch_MixedMany = "Sch_MixedMany";

		internal const string Sch_GroupDisabled = "Sch_GroupDisabled";

		internal const string Sch_MissDtvalue = "Sch_MissDtvalue";

		internal const string Sch_MissDtvaluesAttribute = "Sch_MissDtvaluesAttribute";

		internal const string Sch_DupDtType = "Sch_DupDtType";

		internal const string Sch_DupAttribute = "Sch_DupAttribute";

		internal const string Sch_RequireEnumeration = "Sch_RequireEnumeration";

		internal const string Sch_DefaultIdValue = "Sch_DefaultIdValue";

		internal const string Sch_ElementNotAllowed = "Sch_ElementNotAllowed";

		internal const string Sch_ElementMissing = "Sch_ElementMissing";

		internal const string Sch_ManyMaxOccurs = "Sch_ManyMaxOccurs";

		internal const string Sch_MaxOccursInvalid = "Sch_MaxOccursInvalid";

		internal const string Sch_MinOccursInvalid = "Sch_MinOccursInvalid";

		internal const string Sch_DtMaxLengthInvalid = "Sch_DtMaxLengthInvalid";

		internal const string Sch_DtMinLengthInvalid = "Sch_DtMinLengthInvalid";

		internal const string Sch_DupDtMaxLength = "Sch_DupDtMaxLength";

		internal const string Sch_DupDtMinLength = "Sch_DupDtMinLength";

		internal const string Sch_DtMinMaxLength = "Sch_DtMinMaxLength";

		internal const string Sch_DupElement = "Sch_DupElement";

		internal const string Sch_DupGroupParticle = "Sch_DupGroupParticle";

		internal const string Sch_InvalidValue = "Sch_InvalidValue";

		internal const string Sch_InvalidValueDetailed = "Sch_InvalidValueDetailed";

		internal const string Sch_MissRequiredAttribute = "Sch_MissRequiredAttribute";

		internal const string Sch_FixedAttributeValue = "Sch_FixedAttributeValue";

		internal const string Sch_FixedElementValue = "Sch_FixedElementValue";

		internal const string Sch_AttributeValueDataTypeDetailed = "Sch_AttributeValueDataTypeDetailed";

		internal const string Sch_AttributeDefaultDataType = "Sch_AttributeDefaultDataType";

		internal const string Sch_IncludeLocation = "Sch_IncludeLocation";

		internal const string Sch_ImportLocation = "Sch_ImportLocation";

		internal const string Sch_RedefineLocation = "Sch_RedefineLocation";

		internal const string Sch_InvalidBlockDefaultValue = "Sch_InvalidBlockDefaultValue";

		internal const string Sch_InvalidFinalDefaultValue = "Sch_InvalidFinalDefaultValue";

		internal const string Sch_InvalidElementBlockValue = "Sch_InvalidElementBlockValue";

		internal const string Sch_InvalidElementFinalValue = "Sch_InvalidElementFinalValue";

		internal const string Sch_InvalidSimpleTypeFinalValue = "Sch_InvalidSimpleTypeFinalValue";

		internal const string Sch_InvalidComplexTypeBlockValue = "Sch_InvalidComplexTypeBlockValue";

		internal const string Sch_InvalidComplexTypeFinalValue = "Sch_InvalidComplexTypeFinalValue";

		internal const string Sch_DupIdentityConstraint = "Sch_DupIdentityConstraint";

		internal const string Sch_DupGlobalElement = "Sch_DupGlobalElement";

		internal const string Sch_DupGlobalAttribute = "Sch_DupGlobalAttribute";

		internal const string Sch_DupSimpleType = "Sch_DupSimpleType";

		internal const string Sch_DupComplexType = "Sch_DupComplexType";

		internal const string Sch_DupGroup = "Sch_DupGroup";

		internal const string Sch_DupAttributeGroup = "Sch_DupAttributeGroup";

		internal const string Sch_DupNotation = "Sch_DupNotation";

		internal const string Sch_DefaultFixedAttributes = "Sch_DefaultFixedAttributes";

		internal const string Sch_FixedInRef = "Sch_FixedInRef";

		internal const string Sch_FixedDefaultInRef = "Sch_FixedDefaultInRef";

		internal const string Sch_DupXsdElement = "Sch_DupXsdElement";

		internal const string Sch_ForbiddenAttribute = "Sch_ForbiddenAttribute";

		internal const string Sch_AttributeIgnored = "Sch_AttributeIgnored";

		internal const string Sch_ElementRef = "Sch_ElementRef";

		internal const string Sch_TypeMutualExclusive = "Sch_TypeMutualExclusive";

		internal const string Sch_ElementNameRef = "Sch_ElementNameRef";

		internal const string Sch_AttributeNameRef = "Sch_AttributeNameRef";

		internal const string Sch_TextNotAllowed = "Sch_TextNotAllowed";

		internal const string Sch_UndeclaredType = "Sch_UndeclaredType";

		internal const string Sch_UndeclaredSimpleType = "Sch_UndeclaredSimpleType";

		internal const string Sch_UndeclaredEquivClass = "Sch_UndeclaredEquivClass";

		internal const string Sch_AttListPresence = "Sch_AttListPresence";

		internal const string Sch_NotationValue = "Sch_NotationValue";

		internal const string Sch_EnumerationValue = "Sch_EnumerationValue";

		internal const string Sch_EmptyAttributeValue = "Sch_EmptyAttributeValue";

		internal const string Sch_InvalidLanguageId = "Sch_InvalidLanguageId";

		internal const string Sch_XmlSpace = "Sch_XmlSpace";

		internal const string Sch_InvalidXsdAttributeValue = "Sch_InvalidXsdAttributeValue";

		internal const string Sch_InvalidXsdAttributeDatatypeValue = "Sch_InvalidXsdAttributeDatatypeValue";

		internal const string Sch_ElementValueDataTypeDetailed = "Sch_ElementValueDataTypeDetailed";

		internal const string Sch_InvalidElementDefaultValue = "Sch_InvalidElementDefaultValue";

		internal const string Sch_NonDeterministic = "Sch_NonDeterministic";

		internal const string Sch_NonDeterministicAnyEx = "Sch_NonDeterministicAnyEx";

		internal const string Sch_NonDeterministicAnyAny = "Sch_NonDeterministicAnyAny";

		internal const string Sch_StandAlone = "Sch_StandAlone";

		internal const string Sch_XmlNsAttribute = "Sch_XmlNsAttribute";

		internal const string Sch_AllElement = "Sch_AllElement";

		internal const string Sch_MismatchTargetNamespaceInclude = "Sch_MismatchTargetNamespaceInclude";

		internal const string Sch_MismatchTargetNamespaceImport = "Sch_MismatchTargetNamespaceImport";

		internal const string Sch_MismatchTargetNamespaceEx = "Sch_MismatchTargetNamespaceEx";

		internal const string Sch_XsiTypeNotFound = "Sch_XsiTypeNotFound";

		internal const string Sch_XsiTypeAbstract = "Sch_XsiTypeAbstract";

		internal const string Sch_ListFromNonatomic = "Sch_ListFromNonatomic";

		internal const string Sch_UnionFromUnion = "Sch_UnionFromUnion";

		internal const string Sch_DupLengthFacet = "Sch_DupLengthFacet";

		internal const string Sch_DupMinLengthFacet = "Sch_DupMinLengthFacet";

		internal const string Sch_DupMaxLengthFacet = "Sch_DupMaxLengthFacet";

		internal const string Sch_DupWhiteSpaceFacet = "Sch_DupWhiteSpaceFacet";

		internal const string Sch_DupMaxInclusiveFacet = "Sch_DupMaxInclusiveFacet";

		internal const string Sch_DupMaxExclusiveFacet = "Sch_DupMaxExclusiveFacet";

		internal const string Sch_DupMinInclusiveFacet = "Sch_DupMinInclusiveFacet";

		internal const string Sch_DupMinExclusiveFacet = "Sch_DupMinExclusiveFacet";

		internal const string Sch_DupTotalDigitsFacet = "Sch_DupTotalDigitsFacet";

		internal const string Sch_DupFractionDigitsFacet = "Sch_DupFractionDigitsFacet";

		internal const string Sch_LengthFacetProhibited = "Sch_LengthFacetProhibited";

		internal const string Sch_MinLengthFacetProhibited = "Sch_MinLengthFacetProhibited";

		internal const string Sch_MaxLengthFacetProhibited = "Sch_MaxLengthFacetProhibited";

		internal const string Sch_PatternFacetProhibited = "Sch_PatternFacetProhibited";

		internal const string Sch_EnumerationFacetProhibited = "Sch_EnumerationFacetProhibited";

		internal const string Sch_WhiteSpaceFacetProhibited = "Sch_WhiteSpaceFacetProhibited";

		internal const string Sch_MaxInclusiveFacetProhibited = "Sch_MaxInclusiveFacetProhibited";

		internal const string Sch_MaxExclusiveFacetProhibited = "Sch_MaxExclusiveFacetProhibited";

		internal const string Sch_MinInclusiveFacetProhibited = "Sch_MinInclusiveFacetProhibited";

		internal const string Sch_MinExclusiveFacetProhibited = "Sch_MinExclusiveFacetProhibited";

		internal const string Sch_TotalDigitsFacetProhibited = "Sch_TotalDigitsFacetProhibited";

		internal const string Sch_FractionDigitsFacetProhibited = "Sch_FractionDigitsFacetProhibited";

		internal const string Sch_LengthFacetInvalid = "Sch_LengthFacetInvalid";

		internal const string Sch_MinLengthFacetInvalid = "Sch_MinLengthFacetInvalid";

		internal const string Sch_MaxLengthFacetInvalid = "Sch_MaxLengthFacetInvalid";

		internal const string Sch_MaxInclusiveFacetInvalid = "Sch_MaxInclusiveFacetInvalid";

		internal const string Sch_MaxExclusiveFacetInvalid = "Sch_MaxExclusiveFacetInvalid";

		internal const string Sch_MinInclusiveFacetInvalid = "Sch_MinInclusiveFacetInvalid";

		internal const string Sch_MinExclusiveFacetInvalid = "Sch_MinExclusiveFacetInvalid";

		internal const string Sch_TotalDigitsFacetInvalid = "Sch_TotalDigitsFacetInvalid";

		internal const string Sch_FractionDigitsFacetInvalid = "Sch_FractionDigitsFacetInvalid";

		internal const string Sch_PatternFacetInvalid = "Sch_PatternFacetInvalid";

		internal const string Sch_EnumerationFacetInvalid = "Sch_EnumerationFacetInvalid";

		internal const string Sch_InvalidWhiteSpace = "Sch_InvalidWhiteSpace";

		internal const string Sch_UnknownFacet = "Sch_UnknownFacet";

		internal const string Sch_LengthAndMinMax = "Sch_LengthAndMinMax";

		internal const string Sch_MinLengthGtMaxLength = "Sch_MinLengthGtMaxLength";

		internal const string Sch_FractionDigitsGtTotalDigits = "Sch_FractionDigitsGtTotalDigits";

		internal const string Sch_LengthConstraintFailed = "Sch_LengthConstraintFailed";

		internal const string Sch_MinLengthConstraintFailed = "Sch_MinLengthConstraintFailed";

		internal const string Sch_MaxLengthConstraintFailed = "Sch_MaxLengthConstraintFailed";

		internal const string Sch_PatternConstraintFailed = "Sch_PatternConstraintFailed";

		internal const string Sch_EnumerationConstraintFailed = "Sch_EnumerationConstraintFailed";

		internal const string Sch_MaxInclusiveConstraintFailed = "Sch_MaxInclusiveConstraintFailed";

		internal const string Sch_MaxExclusiveConstraintFailed = "Sch_MaxExclusiveConstraintFailed";

		internal const string Sch_MinInclusiveConstraintFailed = "Sch_MinInclusiveConstraintFailed";

		internal const string Sch_MinExclusiveConstraintFailed = "Sch_MinExclusiveConstraintFailed";

		internal const string Sch_TotalDigitsConstraintFailed = "Sch_TotalDigitsConstraintFailed";

		internal const string Sch_FractionDigitsConstraintFailed = "Sch_FractionDigitsConstraintFailed";

		internal const string Sch_UnionFailedEx = "Sch_UnionFailedEx";

		internal const string Sch_NotationRequired = "Sch_NotationRequired";

		internal const string Sch_DupNotationAttribute = "Sch_DupNotationAttribute";

		internal const string Sch_MissingPublicSystemAttribute = "Sch_MissingPublicSystemAttribute";

		internal const string Sch_NotationAttributeOnEmptyElement = "Sch_NotationAttributeOnEmptyElement";

		internal const string Sch_RefNotInScope = "Sch_RefNotInScope";

		internal const string Sch_UndeclaredIdentityConstraint = "Sch_UndeclaredIdentityConstraint";

		internal const string Sch_RefInvalidIdentityConstraint = "Sch_RefInvalidIdentityConstraint";

		internal const string Sch_RefInvalidCardin = "Sch_RefInvalidCardin";

		internal const string Sch_ReftoKeyref = "Sch_ReftoKeyref";

		internal const string Sch_EmptyXPath = "Sch_EmptyXPath";

		internal const string Sch_UnresolvedPrefix = "Sch_UnresolvedPrefix";

		internal const string Sch_UnresolvedKeyref = "Sch_UnresolvedKeyref";

		internal const string Sch_ICXpathError = "Sch_ICXpathError";

		internal const string Sch_SelectorAttr = "Sch_SelectorAttr";

		internal const string Sch_FieldSimpleTypeExpected = "Sch_FieldSimpleTypeExpected";

		internal const string Sch_FieldSingleValueExpected = "Sch_FieldSingleValueExpected";

		internal const string Sch_MissingKey = "Sch_MissingKey";

		internal const string Sch_DuplicateKey = "Sch_DuplicateKey";

		internal const string Sch_TargetNamespaceXsi = "Sch_TargetNamespaceXsi";

		internal const string Sch_UndeclaredEntity = "Sch_UndeclaredEntity";

		internal const string Sch_UnparsedEntityRef = "Sch_UnparsedEntityRef";

		internal const string Sch_MaxOccursInvalidXsd = "Sch_MaxOccursInvalidXsd";

		internal const string Sch_MinOccursInvalidXsd = "Sch_MinOccursInvalidXsd";

		internal const string Sch_MaxInclusiveExclusive = "Sch_MaxInclusiveExclusive";

		internal const string Sch_MinInclusiveExclusive = "Sch_MinInclusiveExclusive";

		internal const string Sch_MinInclusiveGtMaxInclusive = "Sch_MinInclusiveGtMaxInclusive";

		internal const string Sch_MinExclusiveGtMaxExclusive = "Sch_MinExclusiveGtMaxExclusive";

		internal const string Sch_MinInclusiveGtMaxExclusive = "Sch_MinInclusiveGtMaxExclusive";

		internal const string Sch_MinExclusiveGtMaxInclusive = "Sch_MinExclusiveGtMaxInclusive";

		internal const string Sch_SimpleTypeRestriction = "Sch_SimpleTypeRestriction";

		internal const string Sch_InvalidFacetPosition = "Sch_InvalidFacetPosition";

		internal const string Sch_AttributeMutuallyExclusive = "Sch_AttributeMutuallyExclusive";

		internal const string Sch_AnyAttributeLastChild = "Sch_AnyAttributeLastChild";

		internal const string Sch_ComplexTypeContentModel = "Sch_ComplexTypeContentModel";

		internal const string Sch_ComplexContentContentModel = "Sch_ComplexContentContentModel";

		internal const string Sch_NotNormalizedString = "Sch_NotNormalizedString";

		internal const string Sch_NotTokenString = "Sch_NotTokenString";

		internal const string Sch_FractionDigitsNotOnDecimal = "Sch_FractionDigitsNotOnDecimal";

		internal const string Sch_ContentInNill = "Sch_ContentInNill";

		internal const string Sch_NoElementSchemaFound = "Sch_NoElementSchemaFound";

		internal const string Sch_NoAttributeSchemaFound = "Sch_NoAttributeSchemaFound";

		internal const string Sch_InvalidNamespace = "Sch_InvalidNamespace";

		internal const string Sch_InvalidTargetNamespaceAttribute = "Sch_InvalidTargetNamespaceAttribute";

		internal const string Sch_InvalidNamespaceAttribute = "Sch_InvalidNamespaceAttribute";

		internal const string Sch_InvalidSchemaLocation = "Sch_InvalidSchemaLocation";

		internal const string Sch_ImportTargetNamespace = "Sch_ImportTargetNamespace";

		internal const string Sch_ImportTargetNamespaceNull = "Sch_ImportTargetNamespaceNull";

		internal const string Sch_GroupDoubleRedefine = "Sch_GroupDoubleRedefine";

		internal const string Sch_ComponentRedefineNotFound = "Sch_ComponentRedefineNotFound";

		internal const string Sch_GroupRedefineNotFound = "Sch_GroupRedefineNotFound";

		internal const string Sch_AttrGroupDoubleRedefine = "Sch_AttrGroupDoubleRedefine";

		internal const string Sch_AttrGroupRedefineNotFound = "Sch_AttrGroupRedefineNotFound";

		internal const string Sch_ComplexTypeDoubleRedefine = "Sch_ComplexTypeDoubleRedefine";

		internal const string Sch_ComplexTypeRedefineNotFound = "Sch_ComplexTypeRedefineNotFound";

		internal const string Sch_SimpleToComplexTypeRedefine = "Sch_SimpleToComplexTypeRedefine";

		internal const string Sch_SimpleTypeDoubleRedefine = "Sch_SimpleTypeDoubleRedefine";

		internal const string Sch_ComplexToSimpleTypeRedefine = "Sch_ComplexToSimpleTypeRedefine";

		internal const string Sch_SimpleTypeRedefineNotFound = "Sch_SimpleTypeRedefineNotFound";

		internal const string Sch_MinMaxGroupRedefine = "Sch_MinMaxGroupRedefine";

		internal const string Sch_MultipleGroupSelfRef = "Sch_MultipleGroupSelfRef";

		internal const string Sch_MultipleAttrGroupSelfRef = "Sch_MultipleAttrGroupSelfRef";

		internal const string Sch_InvalidTypeRedefine = "Sch_InvalidTypeRedefine";

		internal const string Sch_InvalidElementRef = "Sch_InvalidElementRef";

		internal const string Sch_MinGtMax = "Sch_MinGtMax";

		internal const string Sch_DupSelector = "Sch_DupSelector";

		internal const string Sch_IdConstraintNoSelector = "Sch_IdConstraintNoSelector";

		internal const string Sch_IdConstraintNoFields = "Sch_IdConstraintNoFields";

		internal const string Sch_IdConstraintNoRefer = "Sch_IdConstraintNoRefer";

		internal const string Sch_SelectorBeforeFields = "Sch_SelectorBeforeFields";

		internal const string Sch_NoSimpleTypeContent = "Sch_NoSimpleTypeContent";

		internal const string Sch_SimpleTypeRestRefBase = "Sch_SimpleTypeRestRefBase";

		internal const string Sch_SimpleTypeRestRefBaseNone = "Sch_SimpleTypeRestRefBaseNone";

		internal const string Sch_SimpleTypeListRefBase = "Sch_SimpleTypeListRefBase";

		internal const string Sch_SimpleTypeListRefBaseNone = "Sch_SimpleTypeListRefBaseNone";

		internal const string Sch_SimpleTypeUnionNoBase = "Sch_SimpleTypeUnionNoBase";

		internal const string Sch_NoRestOrExtQName = "Sch_NoRestOrExtQName";

		internal const string Sch_NoRestOrExt = "Sch_NoRestOrExt";

		internal const string Sch_NoGroupParticle = "Sch_NoGroupParticle";

		internal const string Sch_InvalidAllMin = "Sch_InvalidAllMin";

		internal const string Sch_InvalidAllMax = "Sch_InvalidAllMax";

		internal const string Sch_InvalidFacet = "Sch_InvalidFacet";

		internal const string Sch_AbstractElement = "Sch_AbstractElement";

		internal const string Sch_XsiTypeBlockedEx = "Sch_XsiTypeBlockedEx";

		internal const string Sch_InvalidXsiNill = "Sch_InvalidXsiNill";

		internal const string Sch_SubstitutionNotAllowed = "Sch_SubstitutionNotAllowed";

		internal const string Sch_SubstitutionBlocked = "Sch_SubstitutionBlocked";

		internal const string Sch_InvalidElementInEmptyEx = "Sch_InvalidElementInEmptyEx";

		internal const string Sch_InvalidElementInTextOnlyEx = "Sch_InvalidElementInTextOnlyEx";

		internal const string Sch_InvalidTextInElement = "Sch_InvalidTextInElement";

		internal const string Sch_InvalidElementContent = "Sch_InvalidElementContent";

		internal const string Sch_InvalidElementContentComplex = "Sch_InvalidElementContentComplex";

		internal const string Sch_IncompleteContent = "Sch_IncompleteContent";

		internal const string Sch_IncompleteContentComplex = "Sch_IncompleteContentComplex";

		internal const string Sch_InvalidTextInElementExpecting = "Sch_InvalidTextInElementExpecting";

		internal const string Sch_InvalidElementContentExpecting = "Sch_InvalidElementContentExpecting";

		internal const string Sch_InvalidElementContentExpectingComplex = "Sch_InvalidElementContentExpectingComplex";

		internal const string Sch_IncompleteContentExpecting = "Sch_IncompleteContentExpecting";

		internal const string Sch_IncompleteContentExpectingComplex = "Sch_IncompleteContentExpectingComplex";

		internal const string Sch_InvalidElementSubstitution = "Sch_InvalidElementSubstitution";

		internal const string Sch_ElementNameAndNamespace = "Sch_ElementNameAndNamespace";

		internal const string Sch_ElementName = "Sch_ElementName";

		internal const string Sch_ContinuationString = "Sch_ContinuationString";

		internal const string Sch_AnyElementNS = "Sch_AnyElementNS";

		internal const string Sch_AnyElement = "Sch_AnyElement";

		internal const string Sch_InvalidTextInEmpty = "Sch_InvalidTextInEmpty";

		internal const string Sch_InvalidWhitespaceInEmpty = "Sch_InvalidWhitespaceInEmpty";

		internal const string Sch_InvalidPIComment = "Sch_InvalidPIComment";

		internal const string Sch_InvalidAttributeRef = "Sch_InvalidAttributeRef";

		internal const string Sch_OptionalDefaultAttribute = "Sch_OptionalDefaultAttribute";

		internal const string Sch_AttributeCircularRef = "Sch_AttributeCircularRef";

		internal const string Sch_IdentityConstraintCircularRef = "Sch_IdentityConstraintCircularRef";

		internal const string Sch_SubstitutionCircularRef = "Sch_SubstitutionCircularRef";

		internal const string Sch_InvalidAnyAttribute = "Sch_InvalidAnyAttribute";

		internal const string Sch_DupIdAttribute = "Sch_DupIdAttribute";

		internal const string Sch_InvalidAllElementMax = "Sch_InvalidAllElementMax";

		internal const string Sch_InvalidAny = "Sch_InvalidAny";

		internal const string Sch_InvalidAnyDetailed = "Sch_InvalidAnyDetailed";

		internal const string Sch_InvalidExamplar = "Sch_InvalidExamplar";

		internal const string Sch_NoExamplar = "Sch_NoExamplar";

		internal const string Sch_InvalidSubstitutionMember = "Sch_InvalidSubstitutionMember";

		internal const string Sch_RedefineNoSchema = "Sch_RedefineNoSchema";

		internal const string Sch_ProhibitedAttribute = "Sch_ProhibitedAttribute";

		internal const string Sch_TypeCircularRef = "Sch_TypeCircularRef";

		internal const string Sch_TwoIdAttrUses = "Sch_TwoIdAttrUses";

		internal const string Sch_AttrUseAndWildId = "Sch_AttrUseAndWildId";

		internal const string Sch_MoreThanOneWildId = "Sch_MoreThanOneWildId";

		internal const string Sch_BaseFinalExtension = "Sch_BaseFinalExtension";

		internal const string Sch_NotSimpleContent = "Sch_NotSimpleContent";

		internal const string Sch_NotComplexContent = "Sch_NotComplexContent";

		internal const string Sch_BaseFinalRestriction = "Sch_BaseFinalRestriction";

		internal const string Sch_BaseFinalList = "Sch_BaseFinalList";

		internal const string Sch_BaseFinalUnion = "Sch_BaseFinalUnion";

		internal const string Sch_UndefBaseRestriction = "Sch_UndefBaseRestriction";

		internal const string Sch_UndefBaseExtension = "Sch_UndefBaseExtension";

		internal const string Sch_DifContentType = "Sch_DifContentType";

		internal const string Sch_InvalidContentRestriction = "Sch_InvalidContentRestriction";

		internal const string Sch_InvalidContentRestrictionDetailed = "Sch_InvalidContentRestrictionDetailed";

		internal const string Sch_InvalidBaseToEmpty = "Sch_InvalidBaseToEmpty";

		internal const string Sch_InvalidBaseToMixed = "Sch_InvalidBaseToMixed";

		internal const string Sch_DupAttributeUse = "Sch_DupAttributeUse";

		internal const string Sch_InvalidParticleRestriction = "Sch_InvalidParticleRestriction";

		internal const string Sch_InvalidParticleRestrictionDetailed = "Sch_InvalidParticleRestrictionDetailed";

		internal const string Sch_ForbiddenDerivedParticleForAll = "Sch_ForbiddenDerivedParticleForAll";

		internal const string Sch_ForbiddenDerivedParticleForElem = "Sch_ForbiddenDerivedParticleForElem";

		internal const string Sch_ForbiddenDerivedParticleForChoice = "Sch_ForbiddenDerivedParticleForChoice";

		internal const string Sch_ForbiddenDerivedParticleForSeq = "Sch_ForbiddenDerivedParticleForSeq";

		internal const string Sch_ElementFromElement = "Sch_ElementFromElement";

		internal const string Sch_ElementFromAnyRule1 = "Sch_ElementFromAnyRule1";

		internal const string Sch_ElementFromAnyRule2 = "Sch_ElementFromAnyRule2";

		internal const string Sch_AnyFromAnyRule1 = "Sch_AnyFromAnyRule1";

		internal const string Sch_AnyFromAnyRule2 = "Sch_AnyFromAnyRule2";

		internal const string Sch_AnyFromAnyRule3 = "Sch_AnyFromAnyRule3";

		internal const string Sch_GroupBaseFromAny1 = "Sch_GroupBaseFromAny1";

		internal const string Sch_GroupBaseFromAny2 = "Sch_GroupBaseFromAny2";

		internal const string Sch_ElementFromGroupBase1 = "Sch_ElementFromGroupBase1";

		internal const string Sch_ElementFromGroupBase2 = "Sch_ElementFromGroupBase2";

		internal const string Sch_ElementFromGroupBase3 = "Sch_ElementFromGroupBase3";

		internal const string Sch_GroupBaseRestRangeInvalid = "Sch_GroupBaseRestRangeInvalid";

		internal const string Sch_GroupBaseRestNoMap = "Sch_GroupBaseRestNoMap";

		internal const string Sch_GroupBaseRestNotEmptiable = "Sch_GroupBaseRestNotEmptiable";

		internal const string Sch_SeqFromAll = "Sch_SeqFromAll";

		internal const string Sch_SeqFromChoice = "Sch_SeqFromChoice";

		internal const string Sch_UndefGroupRef = "Sch_UndefGroupRef";

		internal const string Sch_GroupCircularRef = "Sch_GroupCircularRef";

		internal const string Sch_AllRefNotRoot = "Sch_AllRefNotRoot";

		internal const string Sch_AllRefMinMax = "Sch_AllRefMinMax";

		internal const string Sch_NotAllAlone = "Sch_NotAllAlone";

		internal const string Sch_AttributeGroupCircularRef = "Sch_AttributeGroupCircularRef";

		internal const string Sch_UndefAttributeGroupRef = "Sch_UndefAttributeGroupRef";

		internal const string Sch_InvalidAttributeExtension = "Sch_InvalidAttributeExtension";

		internal const string Sch_InvalidAnyAttributeRestriction = "Sch_InvalidAnyAttributeRestriction";

		internal const string Sch_AttributeRestrictionProhibited = "Sch_AttributeRestrictionProhibited";

		internal const string Sch_AttributeRestrictionInvalid = "Sch_AttributeRestrictionInvalid";

		internal const string Sch_AttributeFixedInvalid = "Sch_AttributeFixedInvalid";

		internal const string Sch_AttributeUseInvalid = "Sch_AttributeUseInvalid";

		internal const string Sch_AttributeRestrictionInvalidFromWildcard = "Sch_AttributeRestrictionInvalidFromWildcard";

		internal const string Sch_NoDerivedAttribute = "Sch_NoDerivedAttribute";

		internal const string Sch_UnexpressibleAnyAttribute = "Sch_UnexpressibleAnyAttribute";

		internal const string Sch_RefInvalidAttribute = "Sch_RefInvalidAttribute";

		internal const string Sch_ElementCircularRef = "Sch_ElementCircularRef";

		internal const string Sch_RefInvalidElement = "Sch_RefInvalidElement";

		internal const string Sch_ElementCannotHaveValue = "Sch_ElementCannotHaveValue";

		internal const string Sch_ElementInMixedWithFixed = "Sch_ElementInMixedWithFixed";

		internal const string Sch_ElementTypeCollision = "Sch_ElementTypeCollision";

		internal const string Sch_InvalidIncludeLocation = "Sch_InvalidIncludeLocation";

		internal const string Sch_CannotLoadSchema = "Sch_CannotLoadSchema";

		internal const string Sch_CannotLoadSchemaLocation = "Sch_CannotLoadSchemaLocation";

		internal const string Sch_LengthGtBaseLength = "Sch_LengthGtBaseLength";

		internal const string Sch_MinLengthGtBaseMinLength = "Sch_MinLengthGtBaseMinLength";

		internal const string Sch_MaxLengthGtBaseMaxLength = "Sch_MaxLengthGtBaseMaxLength";

		internal const string Sch_MaxMinLengthBaseLength = "Sch_MaxMinLengthBaseLength";

		internal const string Sch_MaxInclusiveMismatch = "Sch_MaxInclusiveMismatch";

		internal const string Sch_MaxExclusiveMismatch = "Sch_MaxExclusiveMismatch";

		internal const string Sch_MinInclusiveMismatch = "Sch_MinInclusiveMismatch";

		internal const string Sch_MinExclusiveMismatch = "Sch_MinExclusiveMismatch";

		internal const string Sch_MinExlIncMismatch = "Sch_MinExlIncMismatch";

		internal const string Sch_MinExlMaxExlMismatch = "Sch_MinExlMaxExlMismatch";

		internal const string Sch_MinIncMaxExlMismatch = "Sch_MinIncMaxExlMismatch";

		internal const string Sch_MinIncExlMismatch = "Sch_MinIncExlMismatch";

		internal const string Sch_MaxIncExlMismatch = "Sch_MaxIncExlMismatch";

		internal const string Sch_MaxExlIncMismatch = "Sch_MaxExlIncMismatch";

		internal const string Sch_TotalDigitsMismatch = "Sch_TotalDigitsMismatch";

		internal const string Sch_FacetBaseFixed = "Sch_FacetBaseFixed";

		internal const string Sch_WhiteSpaceRestriction1 = "Sch_WhiteSpaceRestriction1";

		internal const string Sch_WhiteSpaceRestriction2 = "Sch_WhiteSpaceRestriction2";

		internal const string Sch_UnSpecifiedDefaultAttributeInExternalStandalone = "Sch_UnSpecifiedDefaultAttributeInExternalStandalone";

		internal const string Sch_StandAloneNormalization = "Sch_StandAloneNormalization";

		internal const string Sch_XsiNilAndFixed = "Sch_XsiNilAndFixed";

		internal const string Sch_MixSchemaTypes = "Sch_MixSchemaTypes";

		internal const string Sch_XSDSchemaOnly = "Sch_XSDSchemaOnly";

		internal const string Sch_InvalidPublicAttribute = "Sch_InvalidPublicAttribute";

		internal const string Sch_InvalidSystemAttribute = "Sch_InvalidSystemAttribute";

		internal const string Sch_TypeAfterConstraints = "Sch_TypeAfterConstraints";

		internal const string Sch_XsiNilAndType = "Sch_XsiNilAndType";

		internal const string Sch_DupSimpleTypeChild = "Sch_DupSimpleTypeChild";

		internal const string Sch_InvalidIdAttribute = "Sch_InvalidIdAttribute";

		internal const string Sch_InvalidNameAttributeEx = "Sch_InvalidNameAttributeEx";

		internal const string Sch_InvalidAttribute = "Sch_InvalidAttribute";

		internal const string Sch_EmptyChoice = "Sch_EmptyChoice";

		internal const string Sch_DerivedNotFromBase = "Sch_DerivedNotFromBase";

		internal const string Sch_NeedSimpleTypeChild = "Sch_NeedSimpleTypeChild";

		internal const string Sch_InvalidCollection = "Sch_InvalidCollection";

		internal const string Sch_UnrefNS = "Sch_UnrefNS";

		internal const string Sch_InvalidSimpleTypeRestriction = "Sch_InvalidSimpleTypeRestriction";

		internal const string Sch_MultipleRedefine = "Sch_MultipleRedefine";

		internal const string Sch_NullValue = "Sch_NullValue";

		internal const string Sch_ComplexContentModel = "Sch_ComplexContentModel";

		internal const string Sch_SchemaNotPreprocessed = "Sch_SchemaNotPreprocessed";

		internal const string Sch_SchemaNotRemoved = "Sch_SchemaNotRemoved";

		internal const string Sch_ComponentAlreadySeenForNS = "Sch_ComponentAlreadySeenForNS";

		internal const string Sch_DefaultAttributeNotApplied = "Sch_DefaultAttributeNotApplied";

		internal const string Sch_NotXsiAttribute = "Sch_NotXsiAttribute";

		internal const string Sch_XsdDateTimeCompare = "Sch_XsdDateTimeCompare";

		internal const string Sch_InvalidNullCast = "Sch_InvalidNullCast";

		internal const string Sch_SchemaDoesNotExist = "Sch_SchemaDoesNotExist";

		internal const string Sch_InvalidDateTimeOption = "Sch_InvalidDateTimeOption";

		internal const string Sch_InvalidStartTransition = "Sch_InvalidStartTransition";

		internal const string Sch_InvalidStateTransition = "Sch_InvalidStateTransition";

		internal const string Sch_InvalidEndValidation = "Sch_InvalidEndValidation";

		internal const string Sch_InvalidEndElementCall = "Sch_InvalidEndElementCall";

		internal const string Sch_InvalidEndElementCallTyped = "Sch_InvalidEndElementCallTyped";

		internal const string Sch_InvalidEndElementMultiple = "Sch_InvalidEndElementMultiple";

		internal const string Sch_DuplicateAttribute = "Sch_DuplicateAttribute";

		internal const string Sch_InvalidPartialValidationType = "Sch_InvalidPartialValidationType";

		internal const string Sch_SchemaElementNameMismatch = "Sch_SchemaElementNameMismatch";

		internal const string Sch_SchemaAttributeNameMismatch = "Sch_SchemaAttributeNameMismatch";

		internal const string Sch_ValidateAttributeInvalidCall = "Sch_ValidateAttributeInvalidCall";

		internal const string Sch_ValidateElementInvalidCall = "Sch_ValidateElementInvalidCall";

		internal const string Sch_EnumNotStarted = "Sch_EnumNotStarted";

		internal const string Sch_EnumFinished = "Sch_EnumFinished";

		internal const string SchInf_schema = "SchInf_schema";

		internal const string SchInf_entity = "SchInf_entity";

		internal const string SchInf_simplecontent = "SchInf_simplecontent";

		internal const string SchInf_extension = "SchInf_extension";

		internal const string SchInf_particle = "SchInf_particle";

		internal const string SchInf_ct = "SchInf_ct";

		internal const string SchInf_seq = "SchInf_seq";

		internal const string SchInf_noseq = "SchInf_noseq";

		internal const string SchInf_noct = "SchInf_noct";

		internal const string SchInf_UnknownParticle = "SchInf_UnknownParticle";

		internal const string SchInf_schematype = "SchInf_schematype";

		internal const string SchInf_NoElement = "SchInf_NoElement";

		internal const string Xp_UnclosedString = "Xp_UnclosedString";

		internal const string Xp_ExprExpected = "Xp_ExprExpected";

		internal const string Xp_InvalidArgumentType = "Xp_InvalidArgumentType";

		internal const string Xp_InvalidNumArgs = "Xp_InvalidNumArgs";

		internal const string Xp_InvalidName = "Xp_InvalidName";

		internal const string Xp_InvalidToken = "Xp_InvalidToken";

		internal const string Xp_NodeSetExpected = "Xp_NodeSetExpected";

		internal const string Xp_NotSupported = "Xp_NotSupported";

		internal const string Xp_InvalidPattern = "Xp_InvalidPattern";

		internal const string Xp_InvalidKeyPattern = "Xp_InvalidKeyPattern";

		internal const string Xp_BadQueryObject = "Xp_BadQueryObject";

		internal const string Xp_UndefinedXsltContext = "Xp_UndefinedXsltContext";

		internal const string Xp_NoContext = "Xp_NoContext";

		internal const string Xp_UndefVar = "Xp_UndefVar";

		internal const string Xp_UndefFunc = "Xp_UndefFunc";

		internal const string Xp_FunctionFailed = "Xp_FunctionFailed";

		internal const string Xp_CurrentNotAllowed = "Xp_CurrentNotAllowed";

		internal const string Xdom_DualDocumentTypeNode = "Xdom_DualDocumentTypeNode";

		internal const string Xdom_DualDocumentElementNode = "Xdom_DualDocumentElementNode";

		internal const string Xdom_DualDeclarationNode = "Xdom_DualDeclarationNode";

		internal const string Xdom_Import = "Xdom_Import";

		internal const string Xdom_Import_NullNode = "Xdom_Import_NullNode";

		internal const string Xdom_NoRootEle = "Xdom_NoRootEle";

		internal const string Xdom_Attr_Name = "Xdom_Attr_Name";

		internal const string Xdom_AttrCol_Object = "Xdom_AttrCol_Object";

		internal const string Xdom_AttrCol_Insert = "Xdom_AttrCol_Insert";

		internal const string Xdom_NamedNode_Context = "Xdom_NamedNode_Context";

		internal const string Xdom_Version = "Xdom_Version";

		internal const string Xdom_standalone = "Xdom_standalone";

		internal const string Xdom_Ele_Prefix = "Xdom_Ele_Prefix";

		internal const string Xdom_Ent_Innertext = "Xdom_Ent_Innertext";

		internal const string Xdom_EntRef_SetVal = "Xdom_EntRef_SetVal";

		internal const string Xdom_WS_Char = "Xdom_WS_Char";

		internal const string Xdom_Node_SetVal = "Xdom_Node_SetVal";

		internal const string Xdom_Empty_LocalName = "Xdom_Empty_LocalName";

		internal const string Xdom_Set_InnerXml = "Xdom_Set_InnerXml";

		internal const string Xdom_Attr_InUse = "Xdom_Attr_InUse";

		internal const string Xdom_Enum_ElementList = "Xdom_Enum_ElementList";

		internal const string Xdom_Invalid_NT_String = "Xdom_Invalid_NT_String";

		internal const string Xdom_InvalidCharacter_EntityReference = "Xdom_InvalidCharacter_EntityReference";

		internal const string Xdom_IndexOutOfRange = "Xdom_IndexOutOfRange";

		internal const string Xpn_BadPosition = "Xpn_BadPosition";

		internal const string Xpn_MissingParent = "Xpn_MissingParent";

		internal const string Xpn_NoContent = "Xpn_NoContent";

		internal const string Xdom_Load_NoDocument = "Xdom_Load_NoDocument";

		internal const string Xdom_Load_NoReader = "Xdom_Load_NoReader";

		internal const string Xdom_Node_Null_Doc = "Xdom_Node_Null_Doc";

		internal const string Xdom_Node_Insert_Child = "Xdom_Node_Insert_Child";

		internal const string Xdom_Node_Insert_Contain = "Xdom_Node_Insert_Contain";

		internal const string Xdom_Node_Insert_Path = "Xdom_Node_Insert_Path";

		internal const string Xdom_Node_Insert_Context = "Xdom_Node_Insert_Context";

		internal const string Xdom_Node_Insert_Location = "Xdom_Node_Insert_Location";

		internal const string Xdom_Node_Insert_TypeConflict = "Xdom_Node_Insert_TypeConflict";

		internal const string Xdom_Node_Remove_Contain = "Xdom_Node_Remove_Contain";

		internal const string Xdom_Node_Remove_Child = "Xdom_Node_Remove_Child";

		internal const string Xdom_Node_Modify_ReadOnly = "Xdom_Node_Modify_ReadOnly";

		internal const string Xdom_TextNode_SplitText = "Xdom_TextNode_SplitText";

		internal const string Xdom_Attr_Reserved_XmlNS = "Xdom_Attr_Reserved_XmlNS";

		internal const string Xdom_Node_Cloning = "Xdom_Node_Cloning";

		internal const string Xnr_ResolveEntity = "Xnr_ResolveEntity";

		internal const string XmlMissingType = "XmlMissingType";

		internal const string XmlUnsupportedType = "XmlUnsupportedType";

		internal const string XmlSerializerUnsupportedType = "XmlSerializerUnsupportedType";

		internal const string XmlSerializerUnsupportedMember = "XmlSerializerUnsupportedMember";

		internal const string XmlUnsupportedTypeKind = "XmlUnsupportedTypeKind";

		internal const string XmlUnsupportedSoapTypeKind = "XmlUnsupportedSoapTypeKind";

		internal const string XmlUnsupportedIDictionary = "XmlUnsupportedIDictionary";

		internal const string XmlUnsupportedIDictionaryDetails = "XmlUnsupportedIDictionaryDetails";

		internal const string XmlDuplicateTypeName = "XmlDuplicateTypeName";

		internal const string XmlSerializableNameMissing1 = "XmlSerializableNameMissing1";

		internal const string XmlConstructorInaccessible = "XmlConstructorInaccessible";

		internal const string XmlTypeInaccessible = "XmlTypeInaccessible";

		internal const string XmlTypeStatic = "XmlTypeStatic";

		internal const string XmlNoDefaultAccessors = "XmlNoDefaultAccessors";

		internal const string XmlNoAddMethod = "XmlNoAddMethod";

		internal const string XmlAttributeSetAgain = "XmlAttributeSetAgain";

		internal const string XmlIllegalWildcard = "XmlIllegalWildcard";

		internal const string XmlIllegalArrayElement = "XmlIllegalArrayElement";

		internal const string XmlIllegalForm = "XmlIllegalForm";

		internal const string XmlBareTextMember = "XmlBareTextMember";

		internal const string XmlBareAttributeMember = "XmlBareAttributeMember";

		internal const string XmlReflectionError = "XmlReflectionError";

		internal const string XmlTypeReflectionError = "XmlTypeReflectionError";

		internal const string XmlPropertyReflectionError = "XmlPropertyReflectionError";

		internal const string XmlFieldReflectionError = "XmlFieldReflectionError";

		internal const string XmlInvalidDataTypeUsage = "XmlInvalidDataTypeUsage";

		internal const string XmlInvalidXsdDataType = "XmlInvalidXsdDataType";

		internal const string XmlDataTypeMismatch = "XmlDataTypeMismatch";

		internal const string XmlIllegalTypeContext = "XmlIllegalTypeContext";

		internal const string XmlUdeclaredXsdType = "XmlUdeclaredXsdType";

		internal const string XmlAnyElementNamespace = "XmlAnyElementNamespace";

		internal const string XmlInvalidConstantAttribute = "XmlInvalidConstantAttribute";

		internal const string XmlIllegalDefault = "XmlIllegalDefault";

		internal const string XmlIllegalAttributesArrayAttribute = "XmlIllegalAttributesArrayAttribute";

		internal const string XmlIllegalElementsArrayAttribute = "XmlIllegalElementsArrayAttribute";

		internal const string XmlIllegalArrayArrayAttribute = "XmlIllegalArrayArrayAttribute";

		internal const string XmlIllegalAttribute = "XmlIllegalAttribute";

		internal const string XmlIllegalType = "XmlIllegalType";

		internal const string XmlIllegalAttrOrText = "XmlIllegalAttrOrText";

		internal const string XmlIllegalSoapAttribute = "XmlIllegalSoapAttribute";

		internal const string XmlIllegalAttrOrTextInterface = "XmlIllegalAttrOrTextInterface";

		internal const string XmlIllegalAttributeFlagsArray = "XmlIllegalAttributeFlagsArray";

		internal const string XmlIllegalAnyElement = "XmlIllegalAnyElement";

		internal const string XmlInvalidIsNullable = "XmlInvalidIsNullable";

		internal const string XmlInvalidNotNullable = "XmlInvalidNotNullable";

		internal const string XmlInvalidFormUnqualified = "XmlInvalidFormUnqualified";

		internal const string XmlDuplicateNamespace = "XmlDuplicateNamespace";

		internal const string XmlElementHasNoName = "XmlElementHasNoName";

		internal const string XmlAttributeHasNoName = "XmlAttributeHasNoName";

		internal const string XmlElementImportedTwice = "XmlElementImportedTwice";

		internal const string XmlHiddenMember = "XmlHiddenMember";

		internal const string XmlInvalidXmlOverride = "XmlInvalidXmlOverride";

		internal const string XmlMembersDeriveError = "XmlMembersDeriveError";

		internal const string XmlTypeUsedTwice = "XmlTypeUsedTwice";

		internal const string XmlMissingGroup = "XmlMissingGroup";

		internal const string XmlMissingAttributeGroup = "XmlMissingAttributeGroup";

		internal const string XmlMissingDataType = "XmlMissingDataType";

		internal const string XmlInvalidEncoding = "XmlInvalidEncoding";

		internal const string XmlMissingElement = "XmlMissingElement";

		internal const string XmlMissingAttribute = "XmlMissingAttribute";

		internal const string XmlMissingMethodEnum = "XmlMissingMethodEnum";

		internal const string XmlNoAttributeHere = "XmlNoAttributeHere";

		internal const string XmlNeedAttributeHere = "XmlNeedAttributeHere";

		internal const string XmlElementNameMismatch = "XmlElementNameMismatch";

		internal const string XmlUnsupportedDefaultType = "XmlUnsupportedDefaultType";

		internal const string XmlUnsupportedDefaultValue = "XmlUnsupportedDefaultValue";

		internal const string XmlInvalidDefaultValue = "XmlInvalidDefaultValue";

		internal const string XmlInvalidDefaultEnumValue = "XmlInvalidDefaultEnumValue";

		internal const string XmlUnknownNode = "XmlUnknownNode";

		internal const string XmlUnknownConstant = "XmlUnknownConstant";

		internal const string XmlSerializeError = "XmlSerializeError";

		internal const string XmlSerializeErrorDetails = "XmlSerializeErrorDetails";

		internal const string XmlCompilerError = "XmlCompilerError";

		internal const string XmlSchemaDuplicateNamespace = "XmlSchemaDuplicateNamespace";

		internal const string XmlSchemaCompiled = "XmlSchemaCompiled";

		internal const string XmlInvalidSchemaExtension = "XmlInvalidSchemaExtension";

		internal const string XmlInvalidArrayDimentions = "XmlInvalidArrayDimentions";

		internal const string XmlInvalidArrayTypeName = "XmlInvalidArrayTypeName";

		internal const string XmlInvalidArrayTypeNamespace = "XmlInvalidArrayTypeNamespace";

		internal const string XmlMissingArrayType = "XmlMissingArrayType";

		internal const string XmlEmptyArrayType = "XmlEmptyArrayType";

		internal const string XmlInvalidArraySyntax = "XmlInvalidArraySyntax";

		internal const string XmlInvalidArrayTypeSyntax = "XmlInvalidArrayTypeSyntax";

		internal const string XmlMismatchedArrayBrackets = "XmlMismatchedArrayBrackets";

		internal const string XmlInvalidArrayLength = "XmlInvalidArrayLength";

		internal const string XmlMissingHref = "XmlMissingHref";

		internal const string XmlInvalidHref = "XmlInvalidHref";

		internal const string XmlUnknownType = "XmlUnknownType";

		internal const string XmlAbstractType = "XmlAbstractType";

		internal const string XmlMappingsScopeMismatch = "XmlMappingsScopeMismatch";

		internal const string XmlMethodTypeNameConflict = "XmlMethodTypeNameConflict";

		internal const string XmlCannotReconcileAccessor = "XmlCannotReconcileAccessor";

		internal const string XmlCannotReconcileAttributeAccessor = "XmlCannotReconcileAttributeAccessor";

		internal const string XmlCannotReconcileAccessorDefault = "XmlCannotReconcileAccessorDefault";

		internal const string XmlInvalidTypeAttributes = "XmlInvalidTypeAttributes";

		internal const string XmlInvalidAttributeUse = "XmlInvalidAttributeUse";

		internal const string XmlTypesDuplicate = "XmlTypesDuplicate";

		internal const string XmlInvalidSoapArray = "XmlInvalidSoapArray";

		internal const string XmlCannotIncludeInSchema = "XmlCannotIncludeInSchema";

		internal const string XmlSoapCannotIncludeInSchema = "XmlSoapCannotIncludeInSchema";

		internal const string XmlInvalidSerializable = "XmlInvalidSerializable";

		internal const string XmlInvalidUseOfType = "XmlInvalidUseOfType";

		internal const string XmlUnxpectedType = "XmlUnxpectedType";

		internal const string XmlUnknownAnyElement = "XmlUnknownAnyElement";

		internal const string XmlMultipleAttributeOverrides = "XmlMultipleAttributeOverrides";

		internal const string XmlInvalidEnumAttribute = "XmlInvalidEnumAttribute";

		internal const string XmlInvalidReturnPosition = "XmlInvalidReturnPosition";

		internal const string XmlInvalidElementAttribute = "XmlInvalidElementAttribute";

		internal const string XmlInvalidVoid = "XmlInvalidVoid";

		internal const string XmlInvalidContent = "XmlInvalidContent";

		internal const string XmlInvalidSchemaElementType = "XmlInvalidSchemaElementType";

		internal const string XmlInvalidSubstitutionGroupUse = "XmlInvalidSubstitutionGroupUse";

		internal const string XmlElementMissingType = "XmlElementMissingType";

		internal const string XmlInvalidAnyAttributeUse = "XmlInvalidAnyAttributeUse";

		internal const string XmlSoapInvalidAttributeUse = "XmlSoapInvalidAttributeUse";

		internal const string XmlSoapInvalidChoice = "XmlSoapInvalidChoice";

		internal const string XmlSoapUnsupportedGroupRef = "XmlSoapUnsupportedGroupRef";

		internal const string XmlSoapUnsupportedGroupRepeat = "XmlSoapUnsupportedGroupRepeat";

		internal const string XmlSoapUnsupportedGroupNested = "XmlSoapUnsupportedGroupNested";

		internal const string XmlSoapUnsupportedGroupAny = "XmlSoapUnsupportedGroupAny";

		internal const string XmlInvalidEnumContent = "XmlInvalidEnumContent";

		internal const string XmlInvalidAttributeType = "XmlInvalidAttributeType";

		internal const string XmlInvalidBaseType = "XmlInvalidBaseType";

		internal const string XmlPrimitiveBaseType = "XmlPrimitiveBaseType";

		internal const string XmlInvalidIdentifier = "XmlInvalidIdentifier";

		internal const string XmlGenError = "XmlGenError";

		internal const string XmlInvalidXmlns = "XmlInvalidXmlns";

		internal const string XmlCircularReference = "XmlCircularReference";

		internal const string XmlCircularReference2 = "XmlCircularReference2";

		internal const string XmlAnonymousBaseType = "XmlAnonymousBaseType";

		internal const string XmlMissingSchema = "XmlMissingSchema";

		internal const string XmlNoSerializableMembers = "XmlNoSerializableMembers";

		internal const string XmlIllegalOverride = "XmlIllegalOverride";

		internal const string XmlReadOnlyCollection = "XmlReadOnlyCollection";

		internal const string XmlRpcNestedValueType = "XmlRpcNestedValueType";

		internal const string XmlRpcRefsInValueType = "XmlRpcRefsInValueType";

		internal const string XmlRpcArrayOfValueTypes = "XmlRpcArrayOfValueTypes";

		internal const string XmlDuplicateElementName = "XmlDuplicateElementName";

		internal const string XmlDuplicateAttributeName = "XmlDuplicateAttributeName";

		internal const string XmlBadBaseElement = "XmlBadBaseElement";

		internal const string XmlBadBaseType = "XmlBadBaseType";

		internal const string XmlUndefinedAlias = "XmlUndefinedAlias";

		internal const string XmlChoiceIdentifierType = "XmlChoiceIdentifierType";

		internal const string XmlChoiceIdentifierArrayType = "XmlChoiceIdentifierArrayType";

		internal const string XmlChoiceIdentifierTypeEnum = "XmlChoiceIdentifierTypeEnum";

		internal const string XmlChoiceIdentiferMemberMissing = "XmlChoiceIdentiferMemberMissing";

		internal const string XmlChoiceIdentiferAmbiguous = "XmlChoiceIdentiferAmbiguous";

		internal const string XmlChoiceIdentiferMissing = "XmlChoiceIdentiferMissing";

		internal const string XmlChoiceMissingValue = "XmlChoiceMissingValue";

		internal const string XmlChoiceMissingAnyValue = "XmlChoiceMissingAnyValue";

		internal const string XmlChoiceMismatchChoiceException = "XmlChoiceMismatchChoiceException";

		internal const string XmlArrayItemAmbiguousTypes = "XmlArrayItemAmbiguousTypes";

		internal const string XmlUnsupportedInterface = "XmlUnsupportedInterface";

		internal const string XmlUnsupportedInterfaceDetails = "XmlUnsupportedInterfaceDetails";

		internal const string XmlUnsupportedRank = "XmlUnsupportedRank";

		internal const string XmlUnsupportedInheritance = "XmlUnsupportedInheritance";

		internal const string XmlIllegalMultipleText = "XmlIllegalMultipleText";

		internal const string XmlIllegalMultipleTextMembers = "XmlIllegalMultipleTextMembers";

		internal const string XmlIllegalArrayTextAttribute = "XmlIllegalArrayTextAttribute";

		internal const string XmlIllegalTypedTextAttribute = "XmlIllegalTypedTextAttribute";

		internal const string XmlIllegalSimpleContentExtension = "XmlIllegalSimpleContentExtension";

		internal const string XmlInvalidCast = "XmlInvalidCast";

		internal const string XmlInvalidCastWithId = "XmlInvalidCastWithId";

		internal const string XmlInvalidArrayRef = "XmlInvalidArrayRef";

		internal const string XmlInvalidNullCast = "XmlInvalidNullCast";

		internal const string XmlMultipleXmlns = "XmlMultipleXmlns";

		internal const string XmlMultipleXmlnsMembers = "XmlMultipleXmlnsMembers";

		internal const string XmlXmlnsInvalidType = "XmlXmlnsInvalidType";

		internal const string XmlSoleXmlnsAttribute = "XmlSoleXmlnsAttribute";

		internal const string XmlConstructorHasSecurityAttributes = "XmlConstructorHasSecurityAttributes";

		internal const string XmlPropertyHasSecurityAttributes = "XmlPropertyHasSecurityAttributes";

		internal const string XmlMethodHasSecurityAttributes = "XmlMethodHasSecurityAttributes";

		internal const string XmlDefaultAccessorHasSecurityAttributes = "XmlDefaultAccessorHasSecurityAttributes";

		internal const string XmlInvalidChoiceIdentifierValue = "XmlInvalidChoiceIdentifierValue";

		internal const string XmlAnyElementDuplicate = "XmlAnyElementDuplicate";

		internal const string XmlChoiceIdDuplicate = "XmlChoiceIdDuplicate";

		internal const string XmlChoiceIdentifierMismatch = "XmlChoiceIdentifierMismatch";

		internal const string XmlUnsupportedRedefine = "XmlUnsupportedRedefine";

		internal const string XmlDuplicateElementInScope = "XmlDuplicateElementInScope";

		internal const string XmlDuplicateElementInScope1 = "XmlDuplicateElementInScope1";

		internal const string XmlNoPartialTrust = "XmlNoPartialTrust";

		internal const string XmlInvalidEncodingNotEncoded1 = "XmlInvalidEncodingNotEncoded1";

		internal const string XmlInvalidEncoding3 = "XmlInvalidEncoding3";

		internal const string XmlInvalidSpecifiedType = "XmlInvalidSpecifiedType";

		internal const string XmlUnsupportedOpenGenericType = "XmlUnsupportedOpenGenericType";

		internal const string XmlMismatchSchemaObjects = "XmlMismatchSchemaObjects";

		internal const string XmlCircularTypeReference = "XmlCircularTypeReference";

		internal const string XmlCircularGroupReference = "XmlCircularGroupReference";

		internal const string XmlRpcLitElementNamespace = "XmlRpcLitElementNamespace";

		internal const string XmlRpcLitElementNullable = "XmlRpcLitElementNullable";

		internal const string XmlRpcLitElements = "XmlRpcLitElements";

		internal const string XmlRpcLitArrayElement = "XmlRpcLitArrayElement";

		internal const string XmlRpcLitAttributeAttributes = "XmlRpcLitAttributeAttributes";

		internal const string XmlRpcLitAttributes = "XmlRpcLitAttributes";

		internal const string XmlSequenceMembers = "XmlSequenceMembers";

		internal const string XmlRpcLitXmlns = "XmlRpcLitXmlns";

		internal const string XmlDuplicateNs = "XmlDuplicateNs";

		internal const string XmlAnonymousInclude = "XmlAnonymousInclude";

		internal const string XmlSchemaIncludeLocation = "XmlSchemaIncludeLocation";

		internal const string XmlSerializableSchemaError = "XmlSerializableSchemaError";

		internal const string XmlGetSchemaMethodName = "XmlGetSchemaMethodName";

		internal const string XmlGetSchemaMethodMissing = "XmlGetSchemaMethodMissing";

		internal const string XmlGetSchemaMethodReturnType = "XmlGetSchemaMethodReturnType";

		internal const string XmlGetSchemaEmptyTypeName = "XmlGetSchemaEmptyTypeName";

		internal const string XmlGetSchemaTypeMissing = "XmlGetSchemaTypeMissing";

		internal const string XmlGetSchemaInclude = "XmlGetSchemaInclude";

		internal const string XmlSerializableAttributes = "XmlSerializableAttributes";

		internal const string XmlSerializableMergeItem = "XmlSerializableMergeItem";

		internal const string XmlSerializableBadDerivation = "XmlSerializableBadDerivation";

		internal const string XmlSerializableMissingClrType = "XmlSerializableMissingClrType";

		internal const string XmlCircularDerivation = "XmlCircularDerivation";

		internal const string XmlSerializerAccessDenied = "XmlSerializerAccessDenied";

		internal const string XmlIdentityAccessDenied = "XmlIdentityAccessDenied";

		internal const string XmlMelformMapping = "XmlMelformMapping";

		internal const string XmlSerializableWriteLess = "XmlSerializableWriteLess";

		internal const string XmlSerializableWriteMore = "XmlSerializableWriteMore";

		internal const string XmlSerializableReadMore = "XmlSerializableReadMore";

		internal const string XmlSerializableReadLess = "XmlSerializableReadLess";

		internal const string XmlSerializableIllegalOperation = "XmlSerializableIllegalOperation";

		internal const string XmlSchemaSyntaxErrorDetails = "XmlSchemaSyntaxErrorDetails";

		internal const string XmlSchemaElementReference = "XmlSchemaElementReference";

		internal const string XmlSchemaAttributeReference = "XmlSchemaAttributeReference";

		internal const string XmlSchemaItem = "XmlSchemaItem";

		internal const string XmlSchemaNamedItem = "XmlSchemaNamedItem";

		internal const string XmlSchemaContentDef = "XmlSchemaContentDef";

		internal const string XmlSchema = "XmlSchema";

		internal const string XmlSerializerCompileFailed = "XmlSerializerCompileFailed";

		internal const string XmlSerializableRootDupName = "XmlSerializableRootDupName";

		internal const string XmlDropDefaultAttribute = "XmlDropDefaultAttribute";

		internal const string XmlDropAttributeValue = "XmlDropAttributeValue";

		internal const string XmlDropArrayAttributeValue = "XmlDropArrayAttributeValue";

		internal const string XmlDropNonPrimitiveAttributeValue = "XmlDropNonPrimitiveAttributeValue";

		internal const string XmlNotKnownDefaultValue = "XmlNotKnownDefaultValue";

		internal const string XmlRemarks = "XmlRemarks";

		internal const string XmlCodegenWarningDetails = "XmlCodegenWarningDetails";

		internal const string XmlExtensionComment = "XmlExtensionComment";

		internal const string XmlExtensionDuplicateDefinition = "XmlExtensionDuplicateDefinition";

		internal const string XmlImporterExtensionBadLocalTypeName = "XmlImporterExtensionBadLocalTypeName";

		internal const string XmlImporterExtensionBadTypeName = "XmlImporterExtensionBadTypeName";

		internal const string XmlConfigurationDuplicateExtension = "XmlConfigurationDuplicateExtension";

		internal const string XmlPregenMissingDirectory = "XmlPregenMissingDirectory";

		internal const string XmlPregenMissingTempDirectory = "XmlPregenMissingTempDirectory";

		internal const string XmlPregenTypeDynamic = "XmlPregenTypeDynamic";

		internal const string XmlSerializerExpiredDetails = "XmlSerializerExpiredDetails";

		internal const string XmlSerializerExpired = "XmlSerializerExpired";

		internal const string XmlPregenAssemblyDynamic = "XmlPregenAssemblyDynamic";

		internal const string XmlNotSerializable = "XmlNotSerializable";

		internal const string XmlPregenOrphanType = "XmlPregenOrphanType";

		internal const string XmlPregenCannotLoad = "XmlPregenCannotLoad";

		internal const string XmlPregenInvalidXmlSerializerAssemblyAttribute = "XmlPregenInvalidXmlSerializerAssemblyAttribute";

		internal const string XmlSequenceInconsistent = "XmlSequenceInconsistent";

		internal const string XmlSequenceUnique = "XmlSequenceUnique";

		internal const string XmlSequenceHierarchy = "XmlSequenceHierarchy";

		internal const string XmlSequenceMatch = "XmlSequenceMatch";

		internal const string XmlDisallowNegativeValues = "XmlDisallowNegativeValues";

		internal const string XmlInternalError = "XmlInternalError";

		internal const string XmlInternalErrorDetails = "XmlInternalErrorDetails";

		internal const string XmlInternalErrorMethod = "XmlInternalErrorMethod";

		internal const string XmlInternalErrorReaderAdvance = "XmlInternalErrorReaderAdvance";

		internal const string XmlNonCLSCompliantException = "XmlNonCLSCompliantException";

		internal const string XmlConvert_BadFormat = "XmlConvert_BadFormat";

		internal const string XmlConvert_Overflow = "XmlConvert_Overflow";

		internal const string XmlConvert_TypeBadMapping = "XmlConvert_TypeBadMapping";

		internal const string XmlConvert_TypeBadMapping2 = "XmlConvert_TypeBadMapping2";

		internal const string XmlConvert_TypeListBadMapping = "XmlConvert_TypeListBadMapping";

		internal const string XmlConvert_TypeListBadMapping2 = "XmlConvert_TypeListBadMapping2";

		internal const string XmlConvert_TypeToString = "XmlConvert_TypeToString";

		internal const string XmlConvert_TypeFromString = "XmlConvert_TypeFromString";

		internal const string XmlConvert_TypeNoPrefix = "XmlConvert_TypeNoPrefix";

		internal const string XmlConvert_TypeNoNamespace = "XmlConvert_TypeNoNamespace";

		internal const string RefSyntaxNotSupportedForElements0 = "RefSyntaxNotSupportedForElements0";

		internal const string XPathDocument_MissingSchemas = "XPathDocument_MissingSchemas";

		internal const string XPathDocument_NotEnoughSchemaInfo = "XPathDocument_NotEnoughSchemaInfo";

		internal const string XPathDocument_ValidateInvalidNodeType = "XPathDocument_ValidateInvalidNodeType";

		internal const string XPathDocument_SchemaSetNotAllowed = "XPathDocument_SchemaSetNotAllowed";

		internal const string XmlBin_MissingEndCDATA = "XmlBin_MissingEndCDATA";

		internal const string XmlBin_InvalidQNameID = "XmlBin_InvalidQNameID";

		internal const string XmlBinary_UnexpectedToken = "XmlBinary_UnexpectedToken";

		internal const string XmlBinary_InvalidSqlDecimal = "XmlBinary_InvalidSqlDecimal";

		internal const string XmlBinary_InvalidSignature = "XmlBinary_InvalidSignature";

		internal const string XmlBinary_InvalidProtocolVersion = "XmlBinary_InvalidProtocolVersion";

		internal const string XmlBinary_UnsupportedCodePage = "XmlBinary_UnsupportedCodePage";

		internal const string XmlBinary_InvalidStandalone = "XmlBinary_InvalidStandalone";

		internal const string XmlBinary_NoParserContext = "XmlBinary_NoParserContext";

		internal const string XmlBinary_ListsOfValuesNotSupported = "XmlBinary_ListsOfValuesNotSupported";

		internal const string XmlBinary_CastNotSupported = "XmlBinary_CastNotSupported";

		internal const string XmlBinary_NoRemapPrefix = "XmlBinary_NoRemapPrefix";

		internal const string XmlBinary_AttrWithNsNoPrefix = "XmlBinary_AttrWithNsNoPrefix";

		internal const string XmlBinary_ValueTooBig = "XmlBinary_ValueTooBig";

		internal const string SqlTypes_ArithOverflow = "SqlTypes_ArithOverflow";

		internal const string SqlTypes_ArithTruncation = "SqlTypes_ArithTruncation";

		internal const string SqlTypes_DivideByZero = "SqlTypes_DivideByZero";

		internal const string Enc_InvalidByteInEncoding = "Enc_InvalidByteInEncoding";

		internal const string Arg_ExpectingXmlTextReader = "Arg_ExpectingXmlTextReader";

		internal const string Arg_CannotCreateNode = "Arg_CannotCreateNode";

		internal const string Xml_BadComment = "Xml_BadComment";

		internal const string Xml_NumEntityOverflow = "Xml_NumEntityOverflow";

		internal const string Xml_UnexpectedCharacter = "Xml_UnexpectedCharacter";

		internal const string Xml_UnexpectedToken1 = "Xml_UnexpectedToken1";

		internal const string Xml_TagMismatchFileName = "Xml_TagMismatchFileName";

		internal const string Xml_ReservedNs = "Xml_ReservedNs";

		internal const string Xml_BadElementData = "Xml_BadElementData";

		internal const string Xml_UnexpectedElement = "Xml_UnexpectedElement";

		internal const string Xml_TagNotInTheSameEntity = "Xml_TagNotInTheSameEntity";

		internal const string Xml_InvalidPartialContentData = "Xml_InvalidPartialContentData";

		internal const string Xml_CanNotStartWithXmlInNamespace = "Xml_CanNotStartWithXmlInNamespace";

		internal const string Xml_UnparsedEntity = "Xml_UnparsedEntity";

		internal const string Xml_InvalidContentForThisNode = "Xml_InvalidContentForThisNode";

		internal const string Xml_MissingEncodingDecl = "Xml_MissingEncodingDecl";

		internal const string Xml_InvalidSurrogatePair = "Xml_InvalidSurrogatePair";

		internal const string Sch_ErrorPosition = "Sch_ErrorPosition";

		internal const string Sch_ReservedNsDecl = "Sch_ReservedNsDecl";

		internal const string Sch_NotInSchemaCollection = "Sch_NotInSchemaCollection";

		internal const string Sch_NotationNotAttr = "Sch_NotationNotAttr";

		internal const string Sch_InvalidContent = "Sch_InvalidContent";

		internal const string Sch_InvalidContentExpecting = "Sch_InvalidContentExpecting";

		internal const string Sch_InvalidTextWhiteSpace = "Sch_InvalidTextWhiteSpace";

		internal const string Sch_XSCHEMA = "Sch_XSCHEMA";

		internal const string Sch_DubSchema = "Sch_DubSchema";

		internal const string Xp_TokenExpected = "Xp_TokenExpected";

		internal const string Xp_NodeTestExpected = "Xp_NodeTestExpected";

		internal const string Xp_NumberExpected = "Xp_NumberExpected";

		internal const string Xp_QueryExpected = "Xp_QueryExpected";

		internal const string Xp_InvalidArgument = "Xp_InvalidArgument";

		internal const string Xp_FunctionExpected = "Xp_FunctionExpected";

		internal const string Xp_InvalidPatternString = "Xp_InvalidPatternString";

		internal const string Xp_BadQueryString = "Xp_BadQueryString";

		internal const string XdomXpNav_NullParam = "XdomXpNav_NullParam";

		internal const string Xdom_Load_NodeType = "Xdom_Load_NodeType";

		internal const string XmlMissingMethod = "XmlMissingMethod";

		internal const string XmlIncludeSerializableError = "XmlIncludeSerializableError";

		internal const string XmlCompilerDynModule = "XmlCompilerDynModule";

		internal const string XmlInvalidSchemaType = "XmlInvalidSchemaType";

		internal const string XmlInvalidAnyUse = "XmlInvalidAnyUse";

		internal const string XmlSchemaSyntaxError = "XmlSchemaSyntaxError";

		internal const string XmlDuplicateChoiceElement = "XmlDuplicateChoiceElement";

		internal const string XmlConvert_BadTimeSpan = "XmlConvert_BadTimeSpan";

		internal const string XmlConvert_BadBoolean = "XmlConvert_BadBoolean";

		internal const string XmlConvert_BadUri = "XmlConvert_BadUri";

		internal const string Xml_UnexpectedToken = "Xml_UnexpectedToken";

		internal const string Xml_PartialContentNodeTypeNotSupported = "Xml_PartialContentNodeTypeNotSupported";

		internal const string Sch_AttributeValueDataType = "Sch_AttributeValueDataType";

		internal const string Sch_ElementValueDataType = "Sch_ElementValueDataType";

		internal const string Sch_NonDeterministicAny = "Sch_NonDeterministicAny";

		internal const string Sch_MismatchTargetNamespace = "Sch_MismatchTargetNamespace";

		internal const string Sch_UnionFailed = "Sch_UnionFailed";

		internal const string Sch_XsiTypeBlocked = "Sch_XsiTypeBlocked";

		internal const string Sch_InvalidElementInEmpty = "Sch_InvalidElementInEmpty";

		internal const string Sch_InvalidElementInTextOnly = "Sch_InvalidElementInTextOnly";

		internal const string Sch_InvalidNameAttribute = "Sch_InvalidNameAttribute";

		private static Res loader;

		private ResourceManager resources;

		private static object s_InternalSyncObject;
	}
}
