using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000020 RID: 32
	internal static class XmlILMethods
	{
		// Token: 0x0600013C RID: 316 RVA: 0x00009E88 File Offset: 0x00008E88
		static XmlILMethods()
		{
			XmlILMethods.StorageMethods[typeof(string)] = new XmlILStorageMethods(typeof(string));
			XmlILMethods.StorageMethods[typeof(bool)] = new XmlILStorageMethods(typeof(bool));
			XmlILMethods.StorageMethods[typeof(int)] = new XmlILStorageMethods(typeof(int));
			XmlILMethods.StorageMethods[typeof(long)] = new XmlILStorageMethods(typeof(long));
			XmlILMethods.StorageMethods[typeof(decimal)] = new XmlILStorageMethods(typeof(decimal));
			XmlILMethods.StorageMethods[typeof(double)] = new XmlILStorageMethods(typeof(double));
			XmlILMethods.StorageMethods[typeof(float)] = new XmlILStorageMethods(typeof(float));
			XmlILMethods.StorageMethods[typeof(DateTime)] = new XmlILStorageMethods(typeof(DateTime));
			XmlILMethods.StorageMethods[typeof(byte[])] = new XmlILStorageMethods(typeof(byte[]));
			XmlILMethods.StorageMethods[typeof(XmlQualifiedName)] = new XmlILStorageMethods(typeof(XmlQualifiedName));
			XmlILMethods.StorageMethods[typeof(TimeSpan)] = new XmlILStorageMethods(typeof(TimeSpan));
			XmlILMethods.StorageMethods[typeof(XPathItem)] = new XmlILStorageMethods(typeof(XPathItem));
			XmlILMethods.StorageMethods[typeof(XPathNavigator)] = new XmlILStorageMethods(typeof(XPathNavigator));
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000BBEC File Offset: 0x0000ABEC
		public static MethodInfo GetMethod(Type className, string methName)
		{
			return className.GetMethod(methName);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000BC04 File Offset: 0x0000AC04
		public static MethodInfo GetMethod(Type className, string methName, params Type[] args)
		{
			return className.GetMethod(methName, args);
		}

		// Token: 0x04000167 RID: 359
		public static readonly MethodInfo AncCreate = XmlILMethods.GetMethod(typeof(AncestorIterator), "Create");

		// Token: 0x04000168 RID: 360
		public static readonly MethodInfo AncNext = XmlILMethods.GetMethod(typeof(AncestorIterator), "MoveNext");

		// Token: 0x04000169 RID: 361
		public static readonly MethodInfo AncDOCreate = XmlILMethods.GetMethod(typeof(AncestorDocOrderIterator), "Create");

		// Token: 0x0400016A RID: 362
		public static readonly MethodInfo AncDONext = XmlILMethods.GetMethod(typeof(AncestorDocOrderIterator), "MoveNext");

		// Token: 0x0400016B RID: 363
		public static readonly MethodInfo AttrContentCreate = XmlILMethods.GetMethod(typeof(AttributeContentIterator), "Create");

		// Token: 0x0400016C RID: 364
		public static readonly MethodInfo AttrContentNext = XmlILMethods.GetMethod(typeof(AttributeContentIterator), "MoveNext");

		// Token: 0x0400016D RID: 365
		public static readonly MethodInfo AttrCreate = XmlILMethods.GetMethod(typeof(AttributeIterator), "Create");

		// Token: 0x0400016E RID: 366
		public static readonly MethodInfo AttrNext = XmlILMethods.GetMethod(typeof(AttributeIterator), "MoveNext");

		// Token: 0x0400016F RID: 367
		public static readonly MethodInfo ContentCreate = XmlILMethods.GetMethod(typeof(ContentIterator), "Create");

		// Token: 0x04000170 RID: 368
		public static readonly MethodInfo ContentNext = XmlILMethods.GetMethod(typeof(ContentIterator), "MoveNext");

		// Token: 0x04000171 RID: 369
		public static readonly MethodInfo ContentMergeCreate = XmlILMethods.GetMethod(typeof(ContentMergeIterator), "Create");

		// Token: 0x04000172 RID: 370
		public static readonly MethodInfo ContentMergeNext = XmlILMethods.GetMethod(typeof(ContentMergeIterator), "MoveNext");

		// Token: 0x04000173 RID: 371
		public static readonly MethodInfo DescCreate = XmlILMethods.GetMethod(typeof(DescendantIterator), "Create");

		// Token: 0x04000174 RID: 372
		public static readonly MethodInfo DescNext = XmlILMethods.GetMethod(typeof(DescendantIterator), "MoveNext");

		// Token: 0x04000175 RID: 373
		public static readonly MethodInfo DescMergeCreate = XmlILMethods.GetMethod(typeof(DescendantMergeIterator), "Create");

		// Token: 0x04000176 RID: 374
		public static readonly MethodInfo DescMergeNext = XmlILMethods.GetMethod(typeof(DescendantMergeIterator), "MoveNext");

		// Token: 0x04000177 RID: 375
		public static readonly MethodInfo DiffCreate = XmlILMethods.GetMethod(typeof(DifferenceIterator), "Create");

		// Token: 0x04000178 RID: 376
		public static readonly MethodInfo DiffNext = XmlILMethods.GetMethod(typeof(DifferenceIterator), "MoveNext");

		// Token: 0x04000179 RID: 377
		public static readonly MethodInfo DodMergeCreate = XmlILMethods.GetMethod(typeof(DodSequenceMerge), "Create");

		// Token: 0x0400017A RID: 378
		public static readonly MethodInfo DodMergeAdd = XmlILMethods.GetMethod(typeof(DodSequenceMerge), "AddSequence");

		// Token: 0x0400017B RID: 379
		public static readonly MethodInfo DodMergeSeq = XmlILMethods.GetMethod(typeof(DodSequenceMerge), "MergeSequences");

		// Token: 0x0400017C RID: 380
		public static readonly MethodInfo ElemContentCreate = XmlILMethods.GetMethod(typeof(ElementContentIterator), "Create");

		// Token: 0x0400017D RID: 381
		public static readonly MethodInfo ElemContentNext = XmlILMethods.GetMethod(typeof(ElementContentIterator), "MoveNext");

		// Token: 0x0400017E RID: 382
		public static readonly MethodInfo FollSibCreate = XmlILMethods.GetMethod(typeof(FollowingSiblingIterator), "Create");

		// Token: 0x0400017F RID: 383
		public static readonly MethodInfo FollSibNext = XmlILMethods.GetMethod(typeof(FollowingSiblingIterator), "MoveNext");

		// Token: 0x04000180 RID: 384
		public static readonly MethodInfo FollSibMergeCreate = XmlILMethods.GetMethod(typeof(FollowingSiblingMergeIterator), "Create");

		// Token: 0x04000181 RID: 385
		public static readonly MethodInfo FollSibMergeNext = XmlILMethods.GetMethod(typeof(FollowingSiblingMergeIterator), "MoveNext");

		// Token: 0x04000182 RID: 386
		public static readonly MethodInfo IdCreate = XmlILMethods.GetMethod(typeof(IdIterator), "Create");

		// Token: 0x04000183 RID: 387
		public static readonly MethodInfo IdNext = XmlILMethods.GetMethod(typeof(IdIterator), "MoveNext");

		// Token: 0x04000184 RID: 388
		public static readonly MethodInfo InterCreate = XmlILMethods.GetMethod(typeof(IntersectIterator), "Create");

		// Token: 0x04000185 RID: 389
		public static readonly MethodInfo InterNext = XmlILMethods.GetMethod(typeof(IntersectIterator), "MoveNext");

		// Token: 0x04000186 RID: 390
		public static readonly MethodInfo KindContentCreate = XmlILMethods.GetMethod(typeof(NodeKindContentIterator), "Create");

		// Token: 0x04000187 RID: 391
		public static readonly MethodInfo KindContentNext = XmlILMethods.GetMethod(typeof(NodeKindContentIterator), "MoveNext");

		// Token: 0x04000188 RID: 392
		public static readonly MethodInfo NmspCreate = XmlILMethods.GetMethod(typeof(NamespaceIterator), "Create");

		// Token: 0x04000189 RID: 393
		public static readonly MethodInfo NmspNext = XmlILMethods.GetMethod(typeof(NamespaceIterator), "MoveNext");

		// Token: 0x0400018A RID: 394
		public static readonly MethodInfo NodeRangeCreate = XmlILMethods.GetMethod(typeof(NodeRangeIterator), "Create");

		// Token: 0x0400018B RID: 395
		public static readonly MethodInfo NodeRangeNext = XmlILMethods.GetMethod(typeof(NodeRangeIterator), "MoveNext");

		// Token: 0x0400018C RID: 396
		public static readonly MethodInfo ParentCreate = XmlILMethods.GetMethod(typeof(ParentIterator), "Create");

		// Token: 0x0400018D RID: 397
		public static readonly MethodInfo ParentNext = XmlILMethods.GetMethod(typeof(ParentIterator), "MoveNext");

		// Token: 0x0400018E RID: 398
		public static readonly MethodInfo PrecCreate = XmlILMethods.GetMethod(typeof(PrecedingIterator), "Create");

		// Token: 0x0400018F RID: 399
		public static readonly MethodInfo PrecNext = XmlILMethods.GetMethod(typeof(PrecedingIterator), "MoveNext");

		// Token: 0x04000190 RID: 400
		public static readonly MethodInfo PreSibCreate = XmlILMethods.GetMethod(typeof(PrecedingSiblingIterator), "Create");

		// Token: 0x04000191 RID: 401
		public static readonly MethodInfo PreSibNext = XmlILMethods.GetMethod(typeof(PrecedingSiblingIterator), "MoveNext");

		// Token: 0x04000192 RID: 402
		public static readonly MethodInfo PreSibDOCreate = XmlILMethods.GetMethod(typeof(PrecedingSiblingDocOrderIterator), "Create");

		// Token: 0x04000193 RID: 403
		public static readonly MethodInfo PreSibDONext = XmlILMethods.GetMethod(typeof(PrecedingSiblingDocOrderIterator), "MoveNext");

		// Token: 0x04000194 RID: 404
		public static readonly MethodInfo SortKeyCreate = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "Create");

		// Token: 0x04000195 RID: 405
		public static readonly MethodInfo SortKeyDateTime = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "AddDateTimeSortKey");

		// Token: 0x04000196 RID: 406
		public static readonly MethodInfo SortKeyDecimal = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "AddDecimalSortKey");

		// Token: 0x04000197 RID: 407
		public static readonly MethodInfo SortKeyDouble = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "AddDoubleSortKey");

		// Token: 0x04000198 RID: 408
		public static readonly MethodInfo SortKeyEmpty = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "AddEmptySortKey");

		// Token: 0x04000199 RID: 409
		public static readonly MethodInfo SortKeyFinish = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "FinishSortKeys");

		// Token: 0x0400019A RID: 410
		public static readonly MethodInfo SortKeyInt = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "AddIntSortKey");

		// Token: 0x0400019B RID: 411
		public static readonly MethodInfo SortKeyInteger = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "AddIntegerSortKey");

		// Token: 0x0400019C RID: 412
		public static readonly MethodInfo SortKeyKeys = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "get_Keys");

		// Token: 0x0400019D RID: 413
		public static readonly MethodInfo SortKeyString = XmlILMethods.GetMethod(typeof(XmlSortKeyAccumulator), "AddStringSortKey");

		// Token: 0x0400019E RID: 414
		public static readonly MethodInfo UnionCreate = XmlILMethods.GetMethod(typeof(UnionIterator), "Create");

		// Token: 0x0400019F RID: 415
		public static readonly MethodInfo UnionNext = XmlILMethods.GetMethod(typeof(UnionIterator), "MoveNext");

		// Token: 0x040001A0 RID: 416
		public static readonly MethodInfo XPFollCreate = XmlILMethods.GetMethod(typeof(XPathFollowingIterator), "Create");

		// Token: 0x040001A1 RID: 417
		public static readonly MethodInfo XPFollNext = XmlILMethods.GetMethod(typeof(XPathFollowingIterator), "MoveNext");

		// Token: 0x040001A2 RID: 418
		public static readonly MethodInfo XPFollMergeCreate = XmlILMethods.GetMethod(typeof(XPathFollowingMergeIterator), "Create");

		// Token: 0x040001A3 RID: 419
		public static readonly MethodInfo XPFollMergeNext = XmlILMethods.GetMethod(typeof(XPathFollowingMergeIterator), "MoveNext");

		// Token: 0x040001A4 RID: 420
		public static readonly MethodInfo XPPrecCreate = XmlILMethods.GetMethod(typeof(XPathPrecedingIterator), "Create");

		// Token: 0x040001A5 RID: 421
		public static readonly MethodInfo XPPrecNext = XmlILMethods.GetMethod(typeof(XPathPrecedingIterator), "MoveNext");

		// Token: 0x040001A6 RID: 422
		public static readonly MethodInfo XPPrecDOCreate = XmlILMethods.GetMethod(typeof(XPathPrecedingDocOrderIterator), "Create");

		// Token: 0x040001A7 RID: 423
		public static readonly MethodInfo XPPrecDONext = XmlILMethods.GetMethod(typeof(XPathPrecedingDocOrderIterator), "MoveNext");

		// Token: 0x040001A8 RID: 424
		public static readonly MethodInfo XPPrecMergeCreate = XmlILMethods.GetMethod(typeof(XPathPrecedingMergeIterator), "Create");

		// Token: 0x040001A9 RID: 425
		public static readonly MethodInfo XPPrecMergeNext = XmlILMethods.GetMethod(typeof(XPathPrecedingMergeIterator), "MoveNext");

		// Token: 0x040001AA RID: 426
		public static readonly MethodInfo AddNewIndex = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "AddNewIndex");

		// Token: 0x040001AB RID: 427
		public static readonly MethodInfo ChangeTypeXsltArg = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "ChangeTypeXsltArgument", new Type[]
		{
			typeof(int),
			typeof(object),
			typeof(Type)
		});

		// Token: 0x040001AC RID: 428
		public static readonly MethodInfo ChangeTypeXsltResult = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "ChangeTypeXsltResult");

		// Token: 0x040001AD RID: 429
		public static readonly MethodInfo CompPos = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "ComparePosition");

		// Token: 0x040001AE RID: 430
		public static readonly MethodInfo Context = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "get_ExternalContext");

		// Token: 0x040001AF RID: 431
		public static readonly MethodInfo CreateCollation = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "CreateCollation");

		// Token: 0x040001B0 RID: 432
		public static readonly MethodInfo DocOrder = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "DocOrderDistinct");

		// Token: 0x040001B1 RID: 433
		public static readonly MethodInfo EndRtfConstr = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "EndRtfConstruction");

		// Token: 0x040001B2 RID: 434
		public static readonly MethodInfo EndSeqConstr = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "EndSequenceConstruction");

		// Token: 0x040001B3 RID: 435
		public static readonly MethodInfo FindIndex = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "FindIndex");

		// Token: 0x040001B4 RID: 436
		public static readonly MethodInfo GenId = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "GenerateId");

		// Token: 0x040001B5 RID: 437
		public static readonly MethodInfo GetAtomizedName = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "GetAtomizedName");

		// Token: 0x040001B6 RID: 438
		public static readonly MethodInfo GetCollation = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "GetCollation");

		// Token: 0x040001B7 RID: 439
		public static readonly MethodInfo GetEarly = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "GetEarlyBoundObject");

		// Token: 0x040001B8 RID: 440
		public static readonly MethodInfo GetNameFilter = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "GetNameFilter");

		// Token: 0x040001B9 RID: 441
		public static readonly MethodInfo GetOutput = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "get_Output");

		// Token: 0x040001BA RID: 442
		public static readonly MethodInfo GetGlobalValue = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "GetGlobalValue");

		// Token: 0x040001BB RID: 443
		public static readonly MethodInfo GetTypeFilter = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "GetTypeFilter");

		// Token: 0x040001BC RID: 444
		public static readonly MethodInfo GlobalComputed = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "IsGlobalComputed");

		// Token: 0x040001BD RID: 445
		public static readonly MethodInfo ItemMatchesCode = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "MatchesXmlType", new Type[]
		{
			typeof(XPathItem),
			typeof(XmlTypeCode)
		});

		// Token: 0x040001BE RID: 446
		public static readonly MethodInfo ItemMatchesType = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "MatchesXmlType", new Type[]
		{
			typeof(XPathItem),
			typeof(int)
		});

		// Token: 0x040001BF RID: 447
		public static readonly MethodInfo QNameEqualLit = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "IsQNameEqual", new Type[]
		{
			typeof(XPathNavigator),
			typeof(int),
			typeof(int)
		});

		// Token: 0x040001C0 RID: 448
		public static readonly MethodInfo QNameEqualNav = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "IsQNameEqual", new Type[]
		{
			typeof(XPathNavigator),
			typeof(XPathNavigator)
		});

		// Token: 0x040001C1 RID: 449
		public static readonly MethodInfo RtfConstr = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "TextRtfConstruction");

		// Token: 0x040001C2 RID: 450
		public static readonly MethodInfo SendMessage = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "SendMessage");

		// Token: 0x040001C3 RID: 451
		public static readonly MethodInfo SeqMatchesCode = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "MatchesXmlType", new Type[]
		{
			typeof(IList<XPathItem>),
			typeof(XmlTypeCode)
		});

		// Token: 0x040001C4 RID: 452
		public static readonly MethodInfo SeqMatchesType = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "MatchesXmlType", new Type[]
		{
			typeof(IList<XPathItem>),
			typeof(int)
		});

		// Token: 0x040001C5 RID: 453
		public static readonly MethodInfo SetGlobalValue = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "SetGlobalValue");

		// Token: 0x040001C6 RID: 454
		public static readonly MethodInfo StartRtfConstr = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "StartRtfConstruction");

		// Token: 0x040001C7 RID: 455
		public static readonly MethodInfo StartSeqConstr = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "StartSequenceConstruction");

		// Token: 0x040001C8 RID: 456
		public static readonly MethodInfo TagAndMappings = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "ParseTagName", new Type[]
		{
			typeof(string),
			typeof(int)
		});

		// Token: 0x040001C9 RID: 457
		public static readonly MethodInfo TagAndNamespace = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "ParseTagName", new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x040001CA RID: 458
		public static readonly MethodInfo ThrowException = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "ThrowException");

		// Token: 0x040001CB RID: 459
		public static readonly MethodInfo XsltLib = XmlILMethods.GetMethod(typeof(XmlQueryRuntime), "get_XsltFunctions");

		// Token: 0x040001CC RID: 460
		public static readonly MethodInfo GetDataSource = XmlILMethods.GetMethod(typeof(XmlQueryContext), "GetDataSource");

		// Token: 0x040001CD RID: 461
		public static readonly MethodInfo GetDefaultDataSource = XmlILMethods.GetMethod(typeof(XmlQueryContext), "get_DefaultDataSource");

		// Token: 0x040001CE RID: 462
		public static readonly MethodInfo GetParam = XmlILMethods.GetMethod(typeof(XmlQueryContext), "GetParameter");

		// Token: 0x040001CF RID: 463
		public static readonly MethodInfo InvokeXsltLate = XmlILMethods.GetMethod(typeof(XmlQueryContext), "InvokeXsltLateBoundFunction");

		// Token: 0x040001D0 RID: 464
		public static readonly MethodInfo IndexAdd = XmlILMethods.GetMethod(typeof(XmlILIndex), "Add");

		// Token: 0x040001D1 RID: 465
		public static readonly MethodInfo IndexLookup = XmlILMethods.GetMethod(typeof(XmlILIndex), "Lookup");

		// Token: 0x040001D2 RID: 466
		public static readonly MethodInfo ItemIsNode = XmlILMethods.GetMethod(typeof(XPathItem), "get_IsNode");

		// Token: 0x040001D3 RID: 467
		public static readonly MethodInfo Value = XmlILMethods.GetMethod(typeof(XPathItem), "get_Value");

		// Token: 0x040001D4 RID: 468
		public static readonly MethodInfo ValueAsAny = XmlILMethods.GetMethod(typeof(XPathItem), "ValueAs", new Type[]
		{
			typeof(Type),
			typeof(IXmlNamespaceResolver)
		});

		// Token: 0x040001D5 RID: 469
		public static readonly MethodInfo NavClone = XmlILMethods.GetMethod(typeof(XPathNavigator), "Clone");

		// Token: 0x040001D6 RID: 470
		public static readonly MethodInfo NavLocalName = XmlILMethods.GetMethod(typeof(XPathNavigator), "get_LocalName");

		// Token: 0x040001D7 RID: 471
		public static readonly MethodInfo NavMoveAttr = XmlILMethods.GetMethod(typeof(XPathNavigator), "MoveToAttribute", new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x040001D8 RID: 472
		public static readonly MethodInfo NavMoveId = XmlILMethods.GetMethod(typeof(XPathNavigator), "MoveToId");

		// Token: 0x040001D9 RID: 473
		public static readonly MethodInfo NavMoveParent = XmlILMethods.GetMethod(typeof(XPathNavigator), "MoveToParent");

		// Token: 0x040001DA RID: 474
		public static readonly MethodInfo NavMoveRoot = XmlILMethods.GetMethod(typeof(XPathNavigator), "MoveToRoot");

		// Token: 0x040001DB RID: 475
		public static readonly MethodInfo NavMoveTo = XmlILMethods.GetMethod(typeof(XPathNavigator), "MoveTo");

		// Token: 0x040001DC RID: 476
		public static readonly MethodInfo NavNmsp = XmlILMethods.GetMethod(typeof(XPathNavigator), "get_NamespaceURI");

		// Token: 0x040001DD RID: 477
		public static readonly MethodInfo NavPrefix = XmlILMethods.GetMethod(typeof(XPathNavigator), "get_Prefix");

		// Token: 0x040001DE RID: 478
		public static readonly MethodInfo NavSamePos = XmlILMethods.GetMethod(typeof(XPathNavigator), "IsSamePosition");

		// Token: 0x040001DF RID: 479
		public static readonly MethodInfo NavType = XmlILMethods.GetMethod(typeof(XPathNavigator), "get_NodeType");

		// Token: 0x040001E0 RID: 480
		public static readonly MethodInfo StartElemLitName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartElement", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(string)
		});

		// Token: 0x040001E1 RID: 481
		public static readonly MethodInfo StartElemLocName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartElementLocalName", new Type[] { typeof(string) });

		// Token: 0x040001E2 RID: 482
		public static readonly MethodInfo EndElemStackName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteEndElement");

		// Token: 0x040001E3 RID: 483
		public static readonly MethodInfo StartAttrLitName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartAttribute", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(string)
		});

		// Token: 0x040001E4 RID: 484
		public static readonly MethodInfo StartAttrLocName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartAttributeLocalName", new Type[] { typeof(string) });

		// Token: 0x040001E5 RID: 485
		public static readonly MethodInfo EndAttr = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteEndAttribute");

		// Token: 0x040001E6 RID: 486
		public static readonly MethodInfo Text = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteString");

		// Token: 0x040001E7 RID: 487
		public static readonly MethodInfo NoEntText = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteRaw", new Type[] { typeof(string) });

		// Token: 0x040001E8 RID: 488
		public static readonly MethodInfo StartTree = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "StartTree");

		// Token: 0x040001E9 RID: 489
		public static readonly MethodInfo EndTree = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "EndTree");

		// Token: 0x040001EA RID: 490
		public static readonly MethodInfo StartElemLitNameUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartElementUnchecked", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(string)
		});

		// Token: 0x040001EB RID: 491
		public static readonly MethodInfo StartElemLocNameUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartElementUnchecked", new Type[] { typeof(string) });

		// Token: 0x040001EC RID: 492
		public static readonly MethodInfo StartContentUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "StartElementContentUnchecked");

		// Token: 0x040001ED RID: 493
		public static readonly MethodInfo EndElemLitNameUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteEndElementUnchecked", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(string)
		});

		// Token: 0x040001EE RID: 494
		public static readonly MethodInfo EndElemLocNameUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteEndElementUnchecked", new Type[] { typeof(string) });

		// Token: 0x040001EF RID: 495
		public static readonly MethodInfo StartAttrLitNameUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartAttributeUnchecked", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(string)
		});

		// Token: 0x040001F0 RID: 496
		public static readonly MethodInfo StartAttrLocNameUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartAttributeUnchecked", new Type[] { typeof(string) });

		// Token: 0x040001F1 RID: 497
		public static readonly MethodInfo EndAttrUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteEndAttributeUnchecked");

		// Token: 0x040001F2 RID: 498
		public static readonly MethodInfo NamespaceDeclUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteNamespaceDeclarationUnchecked");

		// Token: 0x040001F3 RID: 499
		public static readonly MethodInfo TextUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStringUnchecked");

		// Token: 0x040001F4 RID: 500
		public static readonly MethodInfo NoEntTextUn = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteRawUnchecked");

		// Token: 0x040001F5 RID: 501
		public static readonly MethodInfo StartRoot = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartRoot");

		// Token: 0x040001F6 RID: 502
		public static readonly MethodInfo EndRoot = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteEndRoot");

		// Token: 0x040001F7 RID: 503
		public static readonly MethodInfo StartElemCopyName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartElementComputed", new Type[] { typeof(XPathNavigator) });

		// Token: 0x040001F8 RID: 504
		public static readonly MethodInfo StartElemMapName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartElementComputed", new Type[]
		{
			typeof(string),
			typeof(int)
		});

		// Token: 0x040001F9 RID: 505
		public static readonly MethodInfo StartElemNmspName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartElementComputed", new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x040001FA RID: 506
		public static readonly MethodInfo StartElemQName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartElementComputed", new Type[] { typeof(XmlQualifiedName) });

		// Token: 0x040001FB RID: 507
		public static readonly MethodInfo StartAttrCopyName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartAttributeComputed", new Type[] { typeof(XPathNavigator) });

		// Token: 0x040001FC RID: 508
		public static readonly MethodInfo StartAttrMapName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartAttributeComputed", new Type[]
		{
			typeof(string),
			typeof(int)
		});

		// Token: 0x040001FD RID: 509
		public static readonly MethodInfo StartAttrNmspName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartAttributeComputed", new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x040001FE RID: 510
		public static readonly MethodInfo StartAttrQName = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartAttributeComputed", new Type[] { typeof(XmlQualifiedName) });

		// Token: 0x040001FF RID: 511
		public static readonly MethodInfo NamespaceDecl = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteNamespaceDeclaration");

		// Token: 0x04000200 RID: 512
		public static readonly MethodInfo StartComment = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartComment");

		// Token: 0x04000201 RID: 513
		public static readonly MethodInfo CommentText = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteCommentString");

		// Token: 0x04000202 RID: 514
		public static readonly MethodInfo EndComment = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteEndComment");

		// Token: 0x04000203 RID: 515
		public static readonly MethodInfo StartPI = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteStartProcessingInstruction");

		// Token: 0x04000204 RID: 516
		public static readonly MethodInfo PIText = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteProcessingInstructionString");

		// Token: 0x04000205 RID: 517
		public static readonly MethodInfo EndPI = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteEndProcessingInstruction");

		// Token: 0x04000206 RID: 518
		public static readonly MethodInfo WriteItem = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "WriteItem");

		// Token: 0x04000207 RID: 519
		public static readonly MethodInfo CopyOf = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "XsltCopyOf");

		// Token: 0x04000208 RID: 520
		public static readonly MethodInfo StartCopy = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "StartCopy");

		// Token: 0x04000209 RID: 521
		public static readonly MethodInfo EndCopy = XmlILMethods.GetMethod(typeof(XmlQueryOutput), "EndCopy");

		// Token: 0x0400020A RID: 522
		public static readonly MethodInfo DecAdd = XmlILMethods.GetMethod(typeof(decimal), "Add");

		// Token: 0x0400020B RID: 523
		public static readonly MethodInfo DecCmp = XmlILMethods.GetMethod(typeof(decimal), "Compare", new Type[]
		{
			typeof(decimal),
			typeof(decimal)
		});

		// Token: 0x0400020C RID: 524
		public static readonly MethodInfo DecEq = XmlILMethods.GetMethod(typeof(decimal), "Equals", new Type[]
		{
			typeof(decimal),
			typeof(decimal)
		});

		// Token: 0x0400020D RID: 525
		public static readonly MethodInfo DecSub = XmlILMethods.GetMethod(typeof(decimal), "Subtract");

		// Token: 0x0400020E RID: 526
		public static readonly MethodInfo DecMul = XmlILMethods.GetMethod(typeof(decimal), "Multiply");

		// Token: 0x0400020F RID: 527
		public static readonly MethodInfo DecDiv = XmlILMethods.GetMethod(typeof(decimal), "Divide");

		// Token: 0x04000210 RID: 528
		public static readonly MethodInfo DecRem = XmlILMethods.GetMethod(typeof(decimal), "Remainder");

		// Token: 0x04000211 RID: 529
		public static readonly MethodInfo DecNeg = XmlILMethods.GetMethod(typeof(decimal), "Negate");

		// Token: 0x04000212 RID: 530
		public static readonly MethodInfo QNameEq = XmlILMethods.GetMethod(typeof(XmlQualifiedName), "Equals");

		// Token: 0x04000213 RID: 531
		public static readonly MethodInfo StrEq = XmlILMethods.GetMethod(typeof(string), "Equals", new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x04000214 RID: 532
		public static readonly MethodInfo StrCat2 = XmlILMethods.GetMethod(typeof(string), "Concat", new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x04000215 RID: 533
		public static readonly MethodInfo StrCat3 = XmlILMethods.GetMethod(typeof(string), "Concat", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(string)
		});

		// Token: 0x04000216 RID: 534
		public static readonly MethodInfo StrCat4 = XmlILMethods.GetMethod(typeof(string), "Concat", new Type[]
		{
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(string)
		});

		// Token: 0x04000217 RID: 535
		public static readonly MethodInfo StrCmp = XmlILMethods.GetMethod(typeof(string), "CompareOrdinal", new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x04000218 RID: 536
		public static readonly MethodInfo StrLen = XmlILMethods.GetMethod(typeof(string), "get_Length");

		// Token: 0x04000219 RID: 537
		public static readonly MethodInfo DblToDec = XmlILMethods.GetMethod(typeof(XsltConvert), "ToDecimal", new Type[] { typeof(double) });

		// Token: 0x0400021A RID: 538
		public static readonly MethodInfo DblToInt = XmlILMethods.GetMethod(typeof(XsltConvert), "ToInt", new Type[] { typeof(double) });

		// Token: 0x0400021B RID: 539
		public static readonly MethodInfo DblToLng = XmlILMethods.GetMethod(typeof(XsltConvert), "ToLong", new Type[] { typeof(double) });

		// Token: 0x0400021C RID: 540
		public static readonly MethodInfo DblToStr = XmlILMethods.GetMethod(typeof(XsltConvert), "ToString", new Type[] { typeof(double) });

		// Token: 0x0400021D RID: 541
		public static readonly MethodInfo DecToDbl = XmlILMethods.GetMethod(typeof(XsltConvert), "ToDouble", new Type[] { typeof(decimal) });

		// Token: 0x0400021E RID: 542
		public static readonly MethodInfo DTToStr = XmlILMethods.GetMethod(typeof(XsltConvert), "ToString", new Type[] { typeof(DateTime) });

		// Token: 0x0400021F RID: 543
		public static readonly MethodInfo IntToDbl = XmlILMethods.GetMethod(typeof(XsltConvert), "ToDouble", new Type[] { typeof(int) });

		// Token: 0x04000220 RID: 544
		public static readonly MethodInfo LngToDbl = XmlILMethods.GetMethod(typeof(XsltConvert), "ToDouble", new Type[] { typeof(long) });

		// Token: 0x04000221 RID: 545
		public static readonly MethodInfo StrToDbl = XmlILMethods.GetMethod(typeof(XsltConvert), "ToDouble", new Type[] { typeof(string) });

		// Token: 0x04000222 RID: 546
		public static readonly MethodInfo StrToDT = XmlILMethods.GetMethod(typeof(XsltConvert), "ToDateTime", new Type[] { typeof(string) });

		// Token: 0x04000223 RID: 547
		public static readonly MethodInfo ItemToBool = XmlILMethods.GetMethod(typeof(XsltConvert), "ToBoolean", new Type[] { typeof(XPathItem) });

		// Token: 0x04000224 RID: 548
		public static readonly MethodInfo ItemToDbl = XmlILMethods.GetMethod(typeof(XsltConvert), "ToDouble", new Type[] { typeof(XPathItem) });

		// Token: 0x04000225 RID: 549
		public static readonly MethodInfo ItemToStr = XmlILMethods.GetMethod(typeof(XsltConvert), "ToString", new Type[] { typeof(XPathItem) });

		// Token: 0x04000226 RID: 550
		public static readonly MethodInfo ItemToNode = XmlILMethods.GetMethod(typeof(XsltConvert), "ToNode", new Type[] { typeof(XPathItem) });

		// Token: 0x04000227 RID: 551
		public static readonly MethodInfo ItemToNodes = XmlILMethods.GetMethod(typeof(XsltConvert), "ToNodeSet", new Type[] { typeof(XPathItem) });

		// Token: 0x04000228 RID: 552
		public static readonly MethodInfo ItemsToBool = XmlILMethods.GetMethod(typeof(XsltConvert), "ToBoolean", new Type[] { typeof(IList<XPathItem>) });

		// Token: 0x04000229 RID: 553
		public static readonly MethodInfo ItemsToDbl = XmlILMethods.GetMethod(typeof(XsltConvert), "ToDouble", new Type[] { typeof(IList<XPathItem>) });

		// Token: 0x0400022A RID: 554
		public static readonly MethodInfo ItemsToNode = XmlILMethods.GetMethod(typeof(XsltConvert), "ToNode", new Type[] { typeof(IList<XPathItem>) });

		// Token: 0x0400022B RID: 555
		public static readonly MethodInfo ItemsToNodes = XmlILMethods.GetMethod(typeof(XsltConvert), "ToNodeSet", new Type[] { typeof(IList<XPathItem>) });

		// Token: 0x0400022C RID: 556
		public static readonly MethodInfo ItemsToStr = XmlILMethods.GetMethod(typeof(XsltConvert), "ToString", new Type[] { typeof(IList<XPathItem>) });

		// Token: 0x0400022D RID: 557
		public static readonly MethodInfo StrCatCat = XmlILMethods.GetMethod(typeof(StringConcat), "Concat");

		// Token: 0x0400022E RID: 558
		public static readonly MethodInfo StrCatClear = XmlILMethods.GetMethod(typeof(StringConcat), "Clear");

		// Token: 0x0400022F RID: 559
		public static readonly MethodInfo StrCatResult = XmlILMethods.GetMethod(typeof(StringConcat), "GetResult");

		// Token: 0x04000230 RID: 560
		public static readonly MethodInfo StrCatDelim = XmlILMethods.GetMethod(typeof(StringConcat), "set_Delimiter");

		// Token: 0x04000231 RID: 561
		public static readonly MethodInfo NavsToItems = XmlILMethods.GetMethod(typeof(XmlILStorageConverter), "NavigatorsToItems");

		// Token: 0x04000232 RID: 562
		public static readonly MethodInfo ItemsToNavs = XmlILMethods.GetMethod(typeof(XmlILStorageConverter), "ItemsToNavigators");

		// Token: 0x04000233 RID: 563
		public static readonly MethodInfo SetDod = XmlILMethods.GetMethod(typeof(XmlQueryNodeSequence), "set_IsDocOrderDistinct");

		// Token: 0x04000234 RID: 564
		public static readonly MethodInfo GetTypeFromHandle = XmlILMethods.GetMethod(typeof(Type), "GetTypeFromHandle");

		// Token: 0x04000235 RID: 565
		public static readonly MethodInfo InitializeArray = XmlILMethods.GetMethod(typeof(RuntimeHelpers), "InitializeArray");

		// Token: 0x04000236 RID: 566
		public static readonly Dictionary<Type, XmlILStorageMethods> StorageMethods = new Dictionary<Type, XmlILStorageMethods>();
	}
}
