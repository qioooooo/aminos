using System;
using System.Diagnostics;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x0200017E RID: 382
	internal class Keywords
	{
		// Token: 0x06000FB0 RID: 4016 RVA: 0x0004E388 File Offset: 0x0004D388
		internal Keywords(XmlNameTable nameTable)
		{
			this._NameTable = nameTable;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0004E398 File Offset: 0x0004D398
		internal void LookupKeywords()
		{
			this._AtomEmpty = this._NameTable.Add(string.Empty);
			this._AtomXsltNamespace = this._NameTable.Add("http://www.w3.org/1999/XSL/Transform");
			this._AtomApplyTemplates = this._NameTable.Add("apply-templates");
			this._AtomChoose = this._NameTable.Add("choose");
			this._AtomForEach = this._NameTable.Add("for-each");
			this._AtomIf = this._NameTable.Add("if");
			this._AtomOtherwise = this._NameTable.Add("otherwise");
			this._AtomStylesheet = this._NameTable.Add("stylesheet");
			this._AtomTemplate = this._NameTable.Add("template");
			this._AtomTransform = this._NameTable.Add("transform");
			this._AtomValueOf = this._NameTable.Add("value-of");
			this._AtomWhen = this._NameTable.Add("when");
			this._AtomMatch = this._NameTable.Add("match");
			this._AtomName = this._NameTable.Add("name");
			this._AtomSelect = this._NameTable.Add("select");
			this._AtomTest = this._NameTable.Add("test");
			this._AtomMsXsltNamespace = this._NameTable.Add("urn:schemas-microsoft-com:xslt");
			this._AtomScript = this._NameTable.Add("script");
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000FB2 RID: 4018 RVA: 0x0004E531 File Offset: 0x0004D531
		internal string Empty
		{
			get
			{
				return this._AtomEmpty;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x0004E539 File Offset: 0x0004D539
		internal string XsltNamespace
		{
			get
			{
				return this._AtomXsltNamespace;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000FB4 RID: 4020 RVA: 0x0004E541 File Offset: 0x0004D541
		internal string ApplyImports
		{
			get
			{
				if (this._AtomApplyImports == null)
				{
					this._AtomApplyImports = this._NameTable.Add("apply-imports");
				}
				return this._AtomApplyImports;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0004E567 File Offset: 0x0004D567
		internal string ApplyTemplates
		{
			get
			{
				return this._AtomApplyTemplates;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000FB6 RID: 4022 RVA: 0x0004E56F File Offset: 0x0004D56F
		internal string Attribute
		{
			get
			{
				if (this._AtomAttribute == null)
				{
					this._AtomAttribute = this._NameTable.Add("attribute");
				}
				return this._AtomAttribute;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000FB7 RID: 4023 RVA: 0x0004E595 File Offset: 0x0004D595
		internal string AttributeSet
		{
			get
			{
				if (this._AtomAttributeSet == null)
				{
					this._AtomAttributeSet = this._NameTable.Add("attribute-set");
				}
				return this._AtomAttributeSet;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000FB8 RID: 4024 RVA: 0x0004E5BB File Offset: 0x0004D5BB
		internal string CallTemplate
		{
			get
			{
				if (this._AtomCallTemplate == null)
				{
					this._AtomCallTemplate = this._NameTable.Add("call-template");
				}
				return this._AtomCallTemplate;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x0004E5E1 File Offset: 0x0004D5E1
		internal string Choose
		{
			get
			{
				return this._AtomChoose;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0004E5E9 File Offset: 0x0004D5E9
		internal string Comment
		{
			get
			{
				if (this._AtomComment == null)
				{
					this._AtomComment = this._NameTable.Add("comment");
				}
				return this._AtomComment;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x0004E60F File Offset: 0x0004D60F
		internal string Copy
		{
			get
			{
				if (this._AtomCopy == null)
				{
					this._AtomCopy = this._NameTable.Add("copy");
				}
				return this._AtomCopy;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0004E635 File Offset: 0x0004D635
		internal string CopyOf
		{
			get
			{
				if (this._AtomCopyOf == null)
				{
					this._AtomCopyOf = this._NameTable.Add("copy-of");
				}
				return this._AtomCopyOf;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x0004E65B File Offset: 0x0004D65B
		internal string DecimalFormat
		{
			get
			{
				if (this._AtomDecimalFormat == null)
				{
					this._AtomDecimalFormat = this._NameTable.Add("decimal-format");
				}
				return this._AtomDecimalFormat;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000FBE RID: 4030 RVA: 0x0004E681 File Offset: 0x0004D681
		internal string Element
		{
			get
			{
				if (this._AtomElement == null)
				{
					this._AtomElement = this._NameTable.Add("element");
				}
				return this._AtomElement;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0004E6A7 File Offset: 0x0004D6A7
		internal string Fallback
		{
			get
			{
				if (this._AtomFallback == null)
				{
					this._AtomFallback = this._NameTable.Add("fallback");
				}
				return this._AtomFallback;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0004E6CD File Offset: 0x0004D6CD
		internal string ForEach
		{
			get
			{
				return this._AtomForEach;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0004E6D5 File Offset: 0x0004D6D5
		internal string If
		{
			get
			{
				return this._AtomIf;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000FC2 RID: 4034 RVA: 0x0004E6DD File Offset: 0x0004D6DD
		internal string Import
		{
			get
			{
				if (this._AtomImport == null)
				{
					this._AtomImport = this._NameTable.Add("import");
				}
				return this._AtomImport;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000FC3 RID: 4035 RVA: 0x0004E703 File Offset: 0x0004D703
		internal string Include
		{
			get
			{
				if (this._AtomInclude == null)
				{
					this._AtomInclude = this._NameTable.Add("include");
				}
				return this._AtomInclude;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000FC4 RID: 4036 RVA: 0x0004E729 File Offset: 0x0004D729
		internal string Key
		{
			get
			{
				if (this._AtomKey == null)
				{
					this._AtomKey = this._NameTable.Add("key");
				}
				return this._AtomKey;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0004E74F File Offset: 0x0004D74F
		internal string Message
		{
			get
			{
				if (this._AtomMessage == null)
				{
					this._AtomMessage = this._NameTable.Add("message");
				}
				return this._AtomMessage;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000FC6 RID: 4038 RVA: 0x0004E775 File Offset: 0x0004D775
		internal string NamespaceAlias
		{
			get
			{
				if (this._AtomNamespaceAlias == null)
				{
					this._AtomNamespaceAlias = this._NameTable.Add("namespace-alias");
				}
				return this._AtomNamespaceAlias;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0004E79B File Offset: 0x0004D79B
		internal string Number
		{
			get
			{
				if (this._AtomNumber == null)
				{
					this._AtomNumber = this._NameTable.Add("number");
				}
				return this._AtomNumber;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0004E7C1 File Offset: 0x0004D7C1
		internal string Otherwise
		{
			get
			{
				return this._AtomOtherwise;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x0004E7C9 File Offset: 0x0004D7C9
		internal string Output
		{
			get
			{
				if (this._AtomOutput == null)
				{
					this._AtomOutput = this._NameTable.Add("output");
				}
				return this._AtomOutput;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x0004E7EF File Offset: 0x0004D7EF
		internal string Param
		{
			get
			{
				if (this._AtomParam == null)
				{
					this._AtomParam = this._NameTable.Add("param");
				}
				return this._AtomParam;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000FCB RID: 4043 RVA: 0x0004E815 File Offset: 0x0004D815
		internal string PreserveSpace
		{
			get
			{
				if (this._AtomPreserveSpace == null)
				{
					this._AtomPreserveSpace = this._NameTable.Add("preserve-space");
				}
				return this._AtomPreserveSpace;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000FCC RID: 4044 RVA: 0x0004E83B File Offset: 0x0004D83B
		internal string ProcessingInstruction
		{
			get
			{
				if (this._AtomProcessingInstruction == null)
				{
					this._AtomProcessingInstruction = this._NameTable.Add("processing-instruction");
				}
				return this._AtomProcessingInstruction;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x0004E861 File Offset: 0x0004D861
		internal string Sort
		{
			get
			{
				if (this._AtomSort == null)
				{
					this._AtomSort = this._NameTable.Add("sort");
				}
				return this._AtomSort;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x0004E887 File Offset: 0x0004D887
		internal string StripSpace
		{
			get
			{
				if (this._AtomStripSpace == null)
				{
					this._AtomStripSpace = this._NameTable.Add("strip-space");
				}
				return this._AtomStripSpace;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000FCF RID: 4047 RVA: 0x0004E8AD File Offset: 0x0004D8AD
		internal string Stylesheet
		{
			get
			{
				return this._AtomStylesheet;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0004E8B5 File Offset: 0x0004D8B5
		internal string Template
		{
			get
			{
				return this._AtomTemplate;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000FD1 RID: 4049 RVA: 0x0004E8BD File Offset: 0x0004D8BD
		internal string Text
		{
			get
			{
				if (this._AtomText == null)
				{
					this._AtomText = this._NameTable.Add("text");
				}
				return this._AtomText;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x0004E8E3 File Offset: 0x0004D8E3
		internal string Transform
		{
			get
			{
				return this._AtomTransform;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0004E8EB File Offset: 0x0004D8EB
		internal string ValueOf
		{
			get
			{
				return this._AtomValueOf;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0004E8F3 File Offset: 0x0004D8F3
		internal string Variable
		{
			get
			{
				if (this._AtomVariable == null)
				{
					this._AtomVariable = this._NameTable.Add("variable");
				}
				return this._AtomVariable;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x0004E919 File Offset: 0x0004D919
		internal string When
		{
			get
			{
				return this._AtomWhen;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000FD6 RID: 4054 RVA: 0x0004E921 File Offset: 0x0004D921
		internal string WithParam
		{
			get
			{
				if (this._AtomWithParam == null)
				{
					this._AtomWithParam = this._NameTable.Add("with-param");
				}
				return this._AtomWithParam;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x0004E947 File Offset: 0x0004D947
		internal string CaseOrder
		{
			get
			{
				if (this._AtomCaseOrder == null)
				{
					this._AtomCaseOrder = this._NameTable.Add("case-order");
				}
				return this._AtomCaseOrder;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000FD8 RID: 4056 RVA: 0x0004E96D File Offset: 0x0004D96D
		internal string CdataSectionElements
		{
			get
			{
				if (this._AtomCdataSectionElements == null)
				{
					this._AtomCdataSectionElements = this._NameTable.Add("cdata-section-elements");
				}
				return this._AtomCdataSectionElements;
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x0004E993 File Offset: 0x0004D993
		internal string Count
		{
			get
			{
				if (this._AtomCount == null)
				{
					this._AtomCount = this._NameTable.Add("count");
				}
				return this._AtomCount;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000FDA RID: 4058 RVA: 0x0004E9B9 File Offset: 0x0004D9B9
		internal string DataType
		{
			get
			{
				if (this._AtomDataType == null)
				{
					this._AtomDataType = this._NameTable.Add("data-type");
				}
				return this._AtomDataType;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x0004E9DF File Offset: 0x0004D9DF
		internal string DecimalSeparator
		{
			get
			{
				if (this._AtomDecimalSeparator == null)
				{
					this._AtomDecimalSeparator = this._NameTable.Add("decimal-separator");
				}
				return this._AtomDecimalSeparator;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x0004EA05 File Offset: 0x0004DA05
		internal string Digit
		{
			get
			{
				if (this._AtomDigit == null)
				{
					this._AtomDigit = this._NameTable.Add("digit");
				}
				return this._AtomDigit;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000FDD RID: 4061 RVA: 0x0004EA2B File Offset: 0x0004DA2B
		internal string DisableOutputEscaping
		{
			get
			{
				if (this._AtomDisableOutputEscaping == null)
				{
					this._AtomDisableOutputEscaping = this._NameTable.Add("disable-output-escaping");
				}
				return this._AtomDisableOutputEscaping;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x0004EA51 File Offset: 0x0004DA51
		internal string DoctypePublic
		{
			get
			{
				if (this._AtomDoctypePublic == null)
				{
					this._AtomDoctypePublic = this._NameTable.Add("doctype-public");
				}
				return this._AtomDoctypePublic;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000FDF RID: 4063 RVA: 0x0004EA77 File Offset: 0x0004DA77
		internal string DoctypeSystem
		{
			get
			{
				if (this._AtomDoctypeSystem == null)
				{
					this._AtomDoctypeSystem = this._NameTable.Add("doctype-system");
				}
				return this._AtomDoctypeSystem;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x0004EA9D File Offset: 0x0004DA9D
		internal string Elements
		{
			get
			{
				if (this._AtomElements == null)
				{
					this._AtomElements = this._NameTable.Add("elements");
				}
				return this._AtomElements;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000FE1 RID: 4065 RVA: 0x0004EAC3 File Offset: 0x0004DAC3
		internal string Encoding
		{
			get
			{
				if (this._AtomEncoding == null)
				{
					this._AtomEncoding = this._NameTable.Add("encoding");
				}
				return this._AtomEncoding;
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000FE2 RID: 4066 RVA: 0x0004EAE9 File Offset: 0x0004DAE9
		internal string ExcludeResultPrefixes
		{
			get
			{
				if (this._AtomExcludeResultPrefixes == null)
				{
					this._AtomExcludeResultPrefixes = this._NameTable.Add("exclude-result-prefixes");
				}
				return this._AtomExcludeResultPrefixes;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000FE3 RID: 4067 RVA: 0x0004EB0F File Offset: 0x0004DB0F
		internal string ExtensionElementPrefixes
		{
			get
			{
				if (this._AtomExtensionElementPrefixes == null)
				{
					this._AtomExtensionElementPrefixes = this._NameTable.Add("extension-element-prefixes");
				}
				return this._AtomExtensionElementPrefixes;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0004EB35 File Offset: 0x0004DB35
		internal string Format
		{
			get
			{
				if (this._AtomFormat == null)
				{
					this._AtomFormat = this._NameTable.Add("format");
				}
				return this._AtomFormat;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0004EB5B File Offset: 0x0004DB5B
		internal string From
		{
			get
			{
				if (this._AtomFrom == null)
				{
					this._AtomFrom = this._NameTable.Add("from");
				}
				return this._AtomFrom;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0004EB81 File Offset: 0x0004DB81
		internal string GroupingSeparator
		{
			get
			{
				if (this._AtomGroupingSeparator == null)
				{
					this._AtomGroupingSeparator = this._NameTable.Add("grouping-separator");
				}
				return this._AtomGroupingSeparator;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x0004EBA7 File Offset: 0x0004DBA7
		internal string GroupingSize
		{
			get
			{
				if (this._AtomGroupingSize == null)
				{
					this._AtomGroupingSize = this._NameTable.Add("grouping-size");
				}
				return this._AtomGroupingSize;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0004EBCD File Offset: 0x0004DBCD
		internal string Href
		{
			get
			{
				if (this._AtomHref == null)
				{
					this._AtomHref = this._NameTable.Add("href");
				}
				return this._AtomHref;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000FE9 RID: 4073 RVA: 0x0004EBF3 File Offset: 0x0004DBF3
		internal string Id
		{
			get
			{
				if (this._AtomId == null)
				{
					this._AtomId = this._NameTable.Add("id");
				}
				return this._AtomId;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000FEA RID: 4074 RVA: 0x0004EC19 File Offset: 0x0004DC19
		internal string Indent
		{
			get
			{
				if (this._AtomIndent == null)
				{
					this._AtomIndent = this._NameTable.Add("indent");
				}
				return this._AtomIndent;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000FEB RID: 4075 RVA: 0x0004EC3F File Offset: 0x0004DC3F
		internal string Infinity
		{
			get
			{
				if (this._AtomInfinity == null)
				{
					this._AtomInfinity = this._NameTable.Add("infinity");
				}
				return this._AtomInfinity;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0004EC65 File Offset: 0x0004DC65
		internal string Lang
		{
			get
			{
				if (this._AtomLang == null)
				{
					this._AtomLang = this._NameTable.Add("lang");
				}
				return this._AtomLang;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000FED RID: 4077 RVA: 0x0004EC8B File Offset: 0x0004DC8B
		internal string LetterValue
		{
			get
			{
				if (this._AtomLetterValue == null)
				{
					this._AtomLetterValue = this._NameTable.Add("letter-value");
				}
				return this._AtomLetterValue;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x0004ECB1 File Offset: 0x0004DCB1
		internal string Level
		{
			get
			{
				if (this._AtomLevel == null)
				{
					this._AtomLevel = this._NameTable.Add("level");
				}
				return this._AtomLevel;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000FEF RID: 4079 RVA: 0x0004ECD7 File Offset: 0x0004DCD7
		internal string Match
		{
			get
			{
				return this._AtomMatch;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000FF0 RID: 4080 RVA: 0x0004ECDF File Offset: 0x0004DCDF
		internal string MediaType
		{
			get
			{
				if (this._AtomMediaType == null)
				{
					this._AtomMediaType = this._NameTable.Add("media-type");
				}
				return this._AtomMediaType;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000FF1 RID: 4081 RVA: 0x0004ED05 File Offset: 0x0004DD05
		internal string Method
		{
			get
			{
				if (this._AtomMethod == null)
				{
					this._AtomMethod = this._NameTable.Add("method");
				}
				return this._AtomMethod;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000FF2 RID: 4082 RVA: 0x0004ED2B File Offset: 0x0004DD2B
		internal string MinusSign
		{
			get
			{
				if (this._AtomMinusSign == null)
				{
					this._AtomMinusSign = this._NameTable.Add("minus-sign");
				}
				return this._AtomMinusSign;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000FF3 RID: 4083 RVA: 0x0004ED51 File Offset: 0x0004DD51
		internal string Mode
		{
			get
			{
				if (this._AtomMode == null)
				{
					this._AtomMode = this._NameTable.Add("mode");
				}
				return this._AtomMode;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000FF4 RID: 4084 RVA: 0x0004ED77 File Offset: 0x0004DD77
		internal string Name
		{
			get
			{
				return this._AtomName;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000FF5 RID: 4085 RVA: 0x0004ED7F File Offset: 0x0004DD7F
		internal string Namespace
		{
			get
			{
				if (this._AtomNamespace == null)
				{
					this._AtomNamespace = this._NameTable.Add("namespace");
				}
				return this._AtomNamespace;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000FF6 RID: 4086 RVA: 0x0004EDA5 File Offset: 0x0004DDA5
		internal string NaN
		{
			get
			{
				if (this._AtomNaN == null)
				{
					this._AtomNaN = this._NameTable.Add("NaN");
				}
				return this._AtomNaN;
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000FF7 RID: 4087 RVA: 0x0004EDCB File Offset: 0x0004DDCB
		internal string OmitXmlDeclaration
		{
			get
			{
				if (this._AtomOmitXmlDeclaration == null)
				{
					this._AtomOmitXmlDeclaration = this._NameTable.Add("omit-xml-declaration");
				}
				return this._AtomOmitXmlDeclaration;
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000FF8 RID: 4088 RVA: 0x0004EDF1 File Offset: 0x0004DDF1
		internal string Order
		{
			get
			{
				if (this._AtomOrder == null)
				{
					this._AtomOrder = this._NameTable.Add("order");
				}
				return this._AtomOrder;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x0004EE17 File Offset: 0x0004DE17
		internal string PatternSeparator
		{
			get
			{
				if (this._AtomPatternSeparator == null)
				{
					this._AtomPatternSeparator = this._NameTable.Add("pattern-separator");
				}
				return this._AtomPatternSeparator;
			}
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x0004EE3D File Offset: 0x0004DE3D
		internal string Percent
		{
			get
			{
				if (this._AtomPercent == null)
				{
					this._AtomPercent = this._NameTable.Add("percent");
				}
				return this._AtomPercent;
			}
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0004EE63 File Offset: 0x0004DE63
		internal string PerMille
		{
			get
			{
				if (this._AtomPerMille == null)
				{
					this._AtomPerMille = this._NameTable.Add("per-mille");
				}
				return this._AtomPerMille;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x0004EE89 File Offset: 0x0004DE89
		internal string Priority
		{
			get
			{
				if (this._AtomPriority == null)
				{
					this._AtomPriority = this._NameTable.Add("priority");
				}
				return this._AtomPriority;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0004EEAF File Offset: 0x0004DEAF
		internal string ResultPrefix
		{
			get
			{
				if (this._AtomResultPrefix == null)
				{
					this._AtomResultPrefix = this._NameTable.Add("result-prefix");
				}
				return this._AtomResultPrefix;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x0004EED5 File Offset: 0x0004DED5
		internal string Select
		{
			get
			{
				return this._AtomSelect;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000FFF RID: 4095 RVA: 0x0004EEDD File Offset: 0x0004DEDD
		internal string Standalone
		{
			get
			{
				if (this._AtomStandalone == null)
				{
					this._AtomStandalone = this._NameTable.Add("standalone");
				}
				return this._AtomStandalone;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06001000 RID: 4096 RVA: 0x0004EF03 File Offset: 0x0004DF03
		internal string StylesheetPrefix
		{
			get
			{
				if (this._AtomStylesheetPrefix == null)
				{
					this._AtomStylesheetPrefix = this._NameTable.Add("stylesheet-prefix");
				}
				return this._AtomStylesheetPrefix;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001001 RID: 4097 RVA: 0x0004EF29 File Offset: 0x0004DF29
		internal string Terminate
		{
			get
			{
				if (this._AtomTerminate == null)
				{
					this._AtomTerminate = this._NameTable.Add("terminate");
				}
				return this._AtomTerminate;
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001002 RID: 4098 RVA: 0x0004EF4F File Offset: 0x0004DF4F
		internal string Test
		{
			get
			{
				return this._AtomTest;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001003 RID: 4099 RVA: 0x0004EF57 File Offset: 0x0004DF57
		internal string Use
		{
			get
			{
				if (this._AtomUse == null)
				{
					this._AtomUse = this._NameTable.Add("use");
				}
				return this._AtomUse;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001004 RID: 4100 RVA: 0x0004EF7D File Offset: 0x0004DF7D
		internal string UseAttributeSets
		{
			get
			{
				if (this._AtomUseAttributeSets == null)
				{
					this._AtomUseAttributeSets = this._NameTable.Add("use-attribute-sets");
				}
				return this._AtomUseAttributeSets;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06001005 RID: 4101 RVA: 0x0004EFA3 File Offset: 0x0004DFA3
		internal string Value
		{
			get
			{
				if (this._AtomValue == null)
				{
					this._AtomValue = this._NameTable.Add("value");
				}
				return this._AtomValue;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x0004EFC9 File Offset: 0x0004DFC9
		internal string Version
		{
			get
			{
				if (this._AtomVersion == null)
				{
					this._AtomVersion = this._NameTable.Add("version");
				}
				return this._AtomVersion;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06001007 RID: 4103 RVA: 0x0004EFEF File Offset: 0x0004DFEF
		internal string ZeroDigit
		{
			get
			{
				if (this._AtomZeroDigit == null)
				{
					this._AtomZeroDigit = this._NameTable.Add("zero-digit");
				}
				return this._AtomZeroDigit;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x0004F015 File Offset: 0x0004E015
		internal string HashDefault
		{
			get
			{
				if (this._AtomHashDefault == null)
				{
					this._AtomHashDefault = this._NameTable.Add("#default");
				}
				return this._AtomHashDefault;
			}
		}

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06001009 RID: 4105 RVA: 0x0004F03B File Offset: 0x0004E03B
		internal string No
		{
			get
			{
				if (this._AtomNo == null)
				{
					this._AtomNo = this._NameTable.Add("no");
				}
				return this._AtomNo;
			}
		}

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x0004F061 File Offset: 0x0004E061
		internal string Yes
		{
			get
			{
				if (this._AtomYes == null)
				{
					this._AtomYes = this._NameTable.Add("yes");
				}
				return this._AtomYes;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x0600100B RID: 4107 RVA: 0x0004F087 File Offset: 0x0004E087
		internal string MsXsltNamespace
		{
			get
			{
				return this._AtomMsXsltNamespace;
			}
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x0004F08F File Offset: 0x0004E08F
		internal string Script
		{
			get
			{
				return this._AtomScript;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x0600100D RID: 4109 RVA: 0x0004F097 File Offset: 0x0004E097
		internal string Language
		{
			get
			{
				if (this._AtomLanguage == null)
				{
					this._AtomLanguage = this._NameTable.Add("language");
				}
				return this._AtomLanguage;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600100E RID: 4110 RVA: 0x0004F0BD File Offset: 0x0004E0BD
		internal string ImplementsPrefix
		{
			get
			{
				if (this._AtomImplementsPrefix == null)
				{
					this._AtomImplementsPrefix = this._NameTable.Add("implements-prefix");
				}
				return this._AtomImplementsPrefix;
			}
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0004F0E3 File Offset: 0x0004E0E3
		internal static bool Equals(string strA, string strB)
		{
			return strA == strB;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0004F0E9 File Offset: 0x0004E0E9
		internal static bool Compare(string strA, string strB)
		{
			return string.Equals(strA, strB);
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0004F0F2 File Offset: 0x0004E0F2
		[Conditional("DEBUG")]
		private void CheckKeyword(string keyword)
		{
		}

		// Token: 0x04000A0B RID: 2571
		internal const string s_Xmlns = "xmlns";

		// Token: 0x04000A0C RID: 2572
		internal const string s_XsltNamespace = "http://www.w3.org/1999/XSL/Transform";

		// Token: 0x04000A0D RID: 2573
		internal const string s_XmlNamespace = "http://www.w3.org/XML/1998/namespace";

		// Token: 0x04000A0E RID: 2574
		internal const string s_XmlnsNamespace = "http://www.w3.org/2000/xmlns/";

		// Token: 0x04000A0F RID: 2575
		internal const string s_WdXslNamespace = "http://www.w3.org/TR/WD-xsl";

		// Token: 0x04000A10 RID: 2576
		internal const string s_Version10 = "1.0";

		// Token: 0x04000A11 RID: 2577
		internal const string s_ApplyImports = "apply-imports";

		// Token: 0x04000A12 RID: 2578
		internal const string s_ApplyTemplates = "apply-templates";

		// Token: 0x04000A13 RID: 2579
		internal const string s_Attribute = "attribute";

		// Token: 0x04000A14 RID: 2580
		internal const string s_AttributeSet = "attribute-set";

		// Token: 0x04000A15 RID: 2581
		internal const string s_CallTemplate = "call-template";

		// Token: 0x04000A16 RID: 2582
		internal const string s_Choose = "choose";

		// Token: 0x04000A17 RID: 2583
		internal const string s_Comment = "comment";

		// Token: 0x04000A18 RID: 2584
		internal const string s_Copy = "copy";

		// Token: 0x04000A19 RID: 2585
		internal const string s_CopyOf = "copy-of";

		// Token: 0x04000A1A RID: 2586
		internal const string s_DecimalFormat = "decimal-format";

		// Token: 0x04000A1B RID: 2587
		internal const string s_Element = "element";

		// Token: 0x04000A1C RID: 2588
		internal const string s_Fallback = "fallback";

		// Token: 0x04000A1D RID: 2589
		internal const string s_ForEach = "for-each";

		// Token: 0x04000A1E RID: 2590
		internal const string s_If = "if";

		// Token: 0x04000A1F RID: 2591
		internal const string s_Import = "import";

		// Token: 0x04000A20 RID: 2592
		internal const string s_Include = "include";

		// Token: 0x04000A21 RID: 2593
		internal const string s_Key = "key";

		// Token: 0x04000A22 RID: 2594
		internal const string s_Message = "message";

		// Token: 0x04000A23 RID: 2595
		internal const string s_NamespaceAlias = "namespace-alias";

		// Token: 0x04000A24 RID: 2596
		internal const string s_Number = "number";

		// Token: 0x04000A25 RID: 2597
		internal const string s_Otherwise = "otherwise";

		// Token: 0x04000A26 RID: 2598
		internal const string s_Output = "output";

		// Token: 0x04000A27 RID: 2599
		internal const string s_Param = "param";

		// Token: 0x04000A28 RID: 2600
		internal const string s_PreserveSpace = "preserve-space";

		// Token: 0x04000A29 RID: 2601
		internal const string s_ProcessingInstruction = "processing-instruction";

		// Token: 0x04000A2A RID: 2602
		internal const string s_Sort = "sort";

		// Token: 0x04000A2B RID: 2603
		internal const string s_StripSpace = "strip-space";

		// Token: 0x04000A2C RID: 2604
		internal const string s_Stylesheet = "stylesheet";

		// Token: 0x04000A2D RID: 2605
		internal const string s_Template = "template";

		// Token: 0x04000A2E RID: 2606
		internal const string s_Text = "text";

		// Token: 0x04000A2F RID: 2607
		internal const string s_Transform = "transform";

		// Token: 0x04000A30 RID: 2608
		internal const string s_ValueOf = "value-of";

		// Token: 0x04000A31 RID: 2609
		internal const string s_Variable = "variable";

		// Token: 0x04000A32 RID: 2610
		internal const string s_When = "when";

		// Token: 0x04000A33 RID: 2611
		internal const string s_WithParam = "with-param";

		// Token: 0x04000A34 RID: 2612
		internal const string s_CaseOrder = "case-order";

		// Token: 0x04000A35 RID: 2613
		internal const string s_CdataSectionElements = "cdata-section-elements";

		// Token: 0x04000A36 RID: 2614
		internal const string s_Count = "count";

		// Token: 0x04000A37 RID: 2615
		internal const string s_DataType = "data-type";

		// Token: 0x04000A38 RID: 2616
		internal const string s_DecimalSeparator = "decimal-separator";

		// Token: 0x04000A39 RID: 2617
		internal const string s_Digit = "digit";

		// Token: 0x04000A3A RID: 2618
		internal const string s_DisableOutputEscaping = "disable-output-escaping";

		// Token: 0x04000A3B RID: 2619
		internal const string s_DoctypePublic = "doctype-public";

		// Token: 0x04000A3C RID: 2620
		internal const string s_DoctypeSystem = "doctype-system";

		// Token: 0x04000A3D RID: 2621
		internal const string s_Elements = "elements";

		// Token: 0x04000A3E RID: 2622
		internal const string s_Encoding = "encoding";

		// Token: 0x04000A3F RID: 2623
		internal const string s_ExcludeResultPrefixes = "exclude-result-prefixes";

		// Token: 0x04000A40 RID: 2624
		internal const string s_ExtensionElementPrefixes = "extension-element-prefixes";

		// Token: 0x04000A41 RID: 2625
		internal const string s_Format = "format";

		// Token: 0x04000A42 RID: 2626
		internal const string s_From = "from";

		// Token: 0x04000A43 RID: 2627
		internal const string s_GroupingSeparator = "grouping-separator";

		// Token: 0x04000A44 RID: 2628
		internal const string s_GroupingSize = "grouping-size";

		// Token: 0x04000A45 RID: 2629
		internal const string s_Href = "href";

		// Token: 0x04000A46 RID: 2630
		internal const string s_Id = "id";

		// Token: 0x04000A47 RID: 2631
		internal const string s_Indent = "indent";

		// Token: 0x04000A48 RID: 2632
		internal const string s_Infinity = "infinity";

		// Token: 0x04000A49 RID: 2633
		internal const string s_Lang = "lang";

		// Token: 0x04000A4A RID: 2634
		internal const string s_LetterValue = "letter-value";

		// Token: 0x04000A4B RID: 2635
		internal const string s_Level = "level";

		// Token: 0x04000A4C RID: 2636
		internal const string s_Match = "match";

		// Token: 0x04000A4D RID: 2637
		internal const string s_MediaType = "media-type";

		// Token: 0x04000A4E RID: 2638
		internal const string s_Method = "method";

		// Token: 0x04000A4F RID: 2639
		internal const string s_MinusSign = "minus-sign";

		// Token: 0x04000A50 RID: 2640
		internal const string s_Mode = "mode";

		// Token: 0x04000A51 RID: 2641
		internal const string s_Name = "name";

		// Token: 0x04000A52 RID: 2642
		internal const string s_Namespace = "namespace";

		// Token: 0x04000A53 RID: 2643
		internal const string s_NaN = "NaN";

		// Token: 0x04000A54 RID: 2644
		internal const string s_OmitXmlDeclaration = "omit-xml-declaration";

		// Token: 0x04000A55 RID: 2645
		internal const string s_Order = "order";

		// Token: 0x04000A56 RID: 2646
		internal const string s_PatternSeparator = "pattern-separator";

		// Token: 0x04000A57 RID: 2647
		internal const string s_Percent = "percent";

		// Token: 0x04000A58 RID: 2648
		internal const string s_PerMille = "per-mille";

		// Token: 0x04000A59 RID: 2649
		internal const string s_Priority = "priority";

		// Token: 0x04000A5A RID: 2650
		internal const string s_ResultPrefix = "result-prefix";

		// Token: 0x04000A5B RID: 2651
		internal const string s_Select = "select";

		// Token: 0x04000A5C RID: 2652
		internal const string s_Space = "space";

		// Token: 0x04000A5D RID: 2653
		internal const string s_Standalone = "standalone";

		// Token: 0x04000A5E RID: 2654
		internal const string s_StylesheetPrefix = "stylesheet-prefix";

		// Token: 0x04000A5F RID: 2655
		internal const string s_Terminate = "terminate";

		// Token: 0x04000A60 RID: 2656
		internal const string s_Test = "test";

		// Token: 0x04000A61 RID: 2657
		internal const string s_Use = "use";

		// Token: 0x04000A62 RID: 2658
		internal const string s_UseAttributeSets = "use-attribute-sets";

		// Token: 0x04000A63 RID: 2659
		internal const string s_Value = "value";

		// Token: 0x04000A64 RID: 2660
		internal const string s_Version = "version";

		// Token: 0x04000A65 RID: 2661
		internal const string s_ZeroDigit = "zero-digit";

		// Token: 0x04000A66 RID: 2662
		internal const string s_Alphabetic = "alphabetic";

		// Token: 0x04000A67 RID: 2663
		internal const string s_Any = "any";

		// Token: 0x04000A68 RID: 2664
		internal const string s_Ascending = "ascending";

		// Token: 0x04000A69 RID: 2665
		internal const string s_Descending = "descending";

		// Token: 0x04000A6A RID: 2666
		internal const string s_HashDefault = "#default";

		// Token: 0x04000A6B RID: 2667
		internal const string s_Html = "html";

		// Token: 0x04000A6C RID: 2668
		internal const string s_LowerFirst = "lower-first";

		// Token: 0x04000A6D RID: 2669
		internal const string s_Multiple = "multiple";

		// Token: 0x04000A6E RID: 2670
		internal const string s_No = "no";

		// Token: 0x04000A6F RID: 2671
		internal const string s_Single = "single";

		// Token: 0x04000A70 RID: 2672
		internal const string s_Traditional = "traditional";

		// Token: 0x04000A71 RID: 2673
		internal const string s_UpperFirst = "upper-first";

		// Token: 0x04000A72 RID: 2674
		internal const string s_Xml = "xml";

		// Token: 0x04000A73 RID: 2675
		internal const string s_Yes = "yes";

		// Token: 0x04000A74 RID: 2676
		internal const string s_Vendor = "vendor";

		// Token: 0x04000A75 RID: 2677
		internal const string s_VendorUrl = "vendor-url";

		// Token: 0x04000A76 RID: 2678
		internal const string s_MsXsltNamespace = "urn:schemas-microsoft-com:xslt";

		// Token: 0x04000A77 RID: 2679
		internal const string s_Script = "script";

		// Token: 0x04000A78 RID: 2680
		internal const string s_Language = "language";

		// Token: 0x04000A79 RID: 2681
		internal const string s_ImplementsPrefix = "implements-prefix";

		// Token: 0x04000A7A RID: 2682
		private XmlNameTable _NameTable;

		// Token: 0x04000A7B RID: 2683
		private string _AtomEmpty;

		// Token: 0x04000A7C RID: 2684
		private string _AtomXsltNamespace;

		// Token: 0x04000A7D RID: 2685
		private string _AtomApplyImports;

		// Token: 0x04000A7E RID: 2686
		private string _AtomApplyTemplates;

		// Token: 0x04000A7F RID: 2687
		private string _AtomAttribute;

		// Token: 0x04000A80 RID: 2688
		private string _AtomAttributeSet;

		// Token: 0x04000A81 RID: 2689
		private string _AtomCallTemplate;

		// Token: 0x04000A82 RID: 2690
		private string _AtomChoose;

		// Token: 0x04000A83 RID: 2691
		private string _AtomComment;

		// Token: 0x04000A84 RID: 2692
		private string _AtomCopy;

		// Token: 0x04000A85 RID: 2693
		private string _AtomCopyOf;

		// Token: 0x04000A86 RID: 2694
		private string _AtomDecimalFormat;

		// Token: 0x04000A87 RID: 2695
		private string _AtomElement;

		// Token: 0x04000A88 RID: 2696
		private string _AtomFallback;

		// Token: 0x04000A89 RID: 2697
		private string _AtomForEach;

		// Token: 0x04000A8A RID: 2698
		private string _AtomIf;

		// Token: 0x04000A8B RID: 2699
		private string _AtomImport;

		// Token: 0x04000A8C RID: 2700
		private string _AtomInclude;

		// Token: 0x04000A8D RID: 2701
		private string _AtomKey;

		// Token: 0x04000A8E RID: 2702
		private string _AtomMessage;

		// Token: 0x04000A8F RID: 2703
		private string _AtomNamespaceAlias;

		// Token: 0x04000A90 RID: 2704
		private string _AtomNumber;

		// Token: 0x04000A91 RID: 2705
		private string _AtomOtherwise;

		// Token: 0x04000A92 RID: 2706
		private string _AtomOutput;

		// Token: 0x04000A93 RID: 2707
		private string _AtomParam;

		// Token: 0x04000A94 RID: 2708
		private string _AtomPreserveSpace;

		// Token: 0x04000A95 RID: 2709
		private string _AtomProcessingInstruction;

		// Token: 0x04000A96 RID: 2710
		private string _AtomSort;

		// Token: 0x04000A97 RID: 2711
		private string _AtomStripSpace;

		// Token: 0x04000A98 RID: 2712
		private string _AtomStylesheet;

		// Token: 0x04000A99 RID: 2713
		private string _AtomTemplate;

		// Token: 0x04000A9A RID: 2714
		private string _AtomText;

		// Token: 0x04000A9B RID: 2715
		private string _AtomTransform;

		// Token: 0x04000A9C RID: 2716
		private string _AtomValueOf;

		// Token: 0x04000A9D RID: 2717
		private string _AtomVariable;

		// Token: 0x04000A9E RID: 2718
		private string _AtomWhen;

		// Token: 0x04000A9F RID: 2719
		private string _AtomWithParam;

		// Token: 0x04000AA0 RID: 2720
		private string _AtomCaseOrder;

		// Token: 0x04000AA1 RID: 2721
		private string _AtomCdataSectionElements;

		// Token: 0x04000AA2 RID: 2722
		private string _AtomCount;

		// Token: 0x04000AA3 RID: 2723
		private string _AtomDataType;

		// Token: 0x04000AA4 RID: 2724
		private string _AtomDecimalSeparator;

		// Token: 0x04000AA5 RID: 2725
		private string _AtomDigit;

		// Token: 0x04000AA6 RID: 2726
		private string _AtomDisableOutputEscaping;

		// Token: 0x04000AA7 RID: 2727
		private string _AtomDoctypePublic;

		// Token: 0x04000AA8 RID: 2728
		private string _AtomDoctypeSystem;

		// Token: 0x04000AA9 RID: 2729
		private string _AtomElements;

		// Token: 0x04000AAA RID: 2730
		private string _AtomEncoding;

		// Token: 0x04000AAB RID: 2731
		private string _AtomExcludeResultPrefixes;

		// Token: 0x04000AAC RID: 2732
		private string _AtomExtensionElementPrefixes;

		// Token: 0x04000AAD RID: 2733
		private string _AtomFormat;

		// Token: 0x04000AAE RID: 2734
		private string _AtomFrom;

		// Token: 0x04000AAF RID: 2735
		private string _AtomGroupingSeparator;

		// Token: 0x04000AB0 RID: 2736
		private string _AtomGroupingSize;

		// Token: 0x04000AB1 RID: 2737
		private string _AtomHref;

		// Token: 0x04000AB2 RID: 2738
		private string _AtomId;

		// Token: 0x04000AB3 RID: 2739
		private string _AtomIndent;

		// Token: 0x04000AB4 RID: 2740
		private string _AtomInfinity;

		// Token: 0x04000AB5 RID: 2741
		private string _AtomLang;

		// Token: 0x04000AB6 RID: 2742
		private string _AtomLetterValue;

		// Token: 0x04000AB7 RID: 2743
		private string _AtomLevel;

		// Token: 0x04000AB8 RID: 2744
		private string _AtomMatch;

		// Token: 0x04000AB9 RID: 2745
		private string _AtomMediaType;

		// Token: 0x04000ABA RID: 2746
		private string _AtomMethod;

		// Token: 0x04000ABB RID: 2747
		private string _AtomMinusSign;

		// Token: 0x04000ABC RID: 2748
		private string _AtomMode;

		// Token: 0x04000ABD RID: 2749
		private string _AtomName;

		// Token: 0x04000ABE RID: 2750
		private string _AtomNamespace;

		// Token: 0x04000ABF RID: 2751
		private string _AtomNaN;

		// Token: 0x04000AC0 RID: 2752
		private string _AtomOmitXmlDeclaration;

		// Token: 0x04000AC1 RID: 2753
		private string _AtomOrder;

		// Token: 0x04000AC2 RID: 2754
		private string _AtomPatternSeparator;

		// Token: 0x04000AC3 RID: 2755
		private string _AtomPercent;

		// Token: 0x04000AC4 RID: 2756
		private string _AtomPerMille;

		// Token: 0x04000AC5 RID: 2757
		private string _AtomPriority;

		// Token: 0x04000AC6 RID: 2758
		private string _AtomResultPrefix;

		// Token: 0x04000AC7 RID: 2759
		private string _AtomSelect;

		// Token: 0x04000AC8 RID: 2760
		private string _AtomStandalone;

		// Token: 0x04000AC9 RID: 2761
		private string _AtomStylesheetPrefix;

		// Token: 0x04000ACA RID: 2762
		private string _AtomTerminate;

		// Token: 0x04000ACB RID: 2763
		private string _AtomTest;

		// Token: 0x04000ACC RID: 2764
		private string _AtomUse;

		// Token: 0x04000ACD RID: 2765
		private string _AtomUseAttributeSets;

		// Token: 0x04000ACE RID: 2766
		private string _AtomValue;

		// Token: 0x04000ACF RID: 2767
		private string _AtomVersion;

		// Token: 0x04000AD0 RID: 2768
		private string _AtomZeroDigit;

		// Token: 0x04000AD1 RID: 2769
		private string _AtomHashDefault;

		// Token: 0x04000AD2 RID: 2770
		private string _AtomNo;

		// Token: 0x04000AD3 RID: 2771
		private string _AtomYes;

		// Token: 0x04000AD4 RID: 2772
		private string _AtomMsXsltNamespace;

		// Token: 0x04000AD5 RID: 2773
		private string _AtomScript;

		// Token: 0x04000AD6 RID: 2774
		private string _AtomLanguage;

		// Token: 0x04000AD7 RID: 2775
		private string _AtomImplementsPrefix;
	}
}
