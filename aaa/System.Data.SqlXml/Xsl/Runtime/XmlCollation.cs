using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000A4 RID: 164
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class XmlCollation
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x000274AE File Offset: 0x000264AE
		private XmlCollation()
			: this(null, CompareOptions.None)
		{
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000274B8 File Offset: 0x000264B8
		private XmlCollation(CultureInfo cultureInfo, CompareOptions compareOptions)
		{
			this.cultinfo = cultureInfo;
			this.compops = compareOptions;
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x000274CE File Offset: 0x000264CE
		internal static XmlCollation CodePointCollation
		{
			get
			{
				return XmlCollation.cp;
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x000274D8 File Offset: 0x000264D8
		internal static XmlCollation Create(string collationLiteral)
		{
			if (collationLiteral == "http://www.w3.org/2004/10/xpath-functions/collation/codepoint")
			{
				return XmlCollation.CodePointCollation;
			}
			XmlCollation xmlCollation = new XmlCollation();
			Uri uri = new Uri(collationLiteral);
			string leftPart = uri.GetLeftPart(UriPartial.Authority);
			if (leftPart == "http://collations.microsoft.com")
			{
				string text = uri.LocalPath.Substring(1);
				if (text.Length == 0)
				{
					goto IL_00AC;
				}
				try
				{
					xmlCollation.cultinfo = new CultureInfo(text);
					goto IL_00AC;
				}
				catch (ArgumentException)
				{
					throw new XslTransformException("Coll_UnsupportedLanguage", new string[] { text });
				}
			}
			if (!uri.IsBaseOf(new Uri("http://www.w3.org/2004/10/xpath-functions/collation/codepoint")))
			{
				throw new XslTransformException("Coll_Unsupported", new string[] { collationLiteral });
			}
			xmlCollation.compops = CompareOptions.Ordinal;
			IL_00AC:
			string query = uri.Query;
			string text2 = null;
			if (query.Length != 0)
			{
				foreach (string text3 in query.Substring(1).Split(new char[] { '&' }))
				{
					string[] array2 = text3.Split(new char[] { '=' });
					if (array2.Length != 2)
					{
						throw new XslTransformException("Coll_BadOptFormat", new string[] { text3 });
					}
					string text4 = array2[0].ToUpper(CultureInfo.InvariantCulture);
					string text5 = array2[1].ToUpper(CultureInfo.InvariantCulture);
					if (text4 == "SORT")
					{
						text2 = text5;
					}
					else
					{
						if (text5 == "1" || text5 == "TRUE")
						{
							string text6;
							switch (text6 = text4)
							{
							case "IGNORECASE":
								xmlCollation.compops |= CompareOptions.IgnoreCase;
								goto IL_0479;
							case "IGNOREKANATYPE":
								xmlCollation.compops |= CompareOptions.IgnoreKanaType;
								goto IL_0479;
							case "IGNORENONSPACE":
								xmlCollation.compops |= CompareOptions.IgnoreNonSpace;
								goto IL_0479;
							case "IGNORESYMBOLS":
								xmlCollation.compops |= CompareOptions.IgnoreSymbols;
								goto IL_0479;
							case "IGNOREWIDTH":
								xmlCollation.compops |= CompareOptions.IgnoreWidth;
								goto IL_0479;
							case "UPPERFIRST":
								xmlCollation.upperFirst = true;
								goto IL_0479;
							case "EMPTYGREATEST":
								xmlCollation.emptyGreatest = true;
								goto IL_0479;
							case "DESCENDINGORDER":
								xmlCollation.descendingOrder = true;
								goto IL_0479;
							}
							throw new XslTransformException("Coll_UnsupportedOpt", new string[] { array2[0] });
						}
						if (text5 == "0" || text5 == "FALSE")
						{
							string text7;
							switch (text7 = text4)
							{
							case "IGNORECASE":
								xmlCollation.compops &= ~CompareOptions.IgnoreCase;
								goto IL_0479;
							case "IGNOREKANATYPE":
								xmlCollation.compops &= ~CompareOptions.IgnoreKanaType;
								goto IL_0479;
							case "IGNORENONSPACE":
								xmlCollation.compops &= ~CompareOptions.IgnoreNonSpace;
								goto IL_0479;
							case "IGNORESYMBOLS":
								xmlCollation.compops &= ~CompareOptions.IgnoreSymbols;
								goto IL_0479;
							case "IGNOREWIDTH":
								xmlCollation.compops &= ~CompareOptions.IgnoreWidth;
								goto IL_0479;
							case "UPPERFIRST":
								xmlCollation.upperFirst = false;
								goto IL_0479;
							case "EMPTYGREATEST":
								xmlCollation.emptyGreatest = false;
								goto IL_0479;
							case "DESCENDINGORDER":
								xmlCollation.descendingOrder = false;
								goto IL_0479;
							}
							throw new XslTransformException("Coll_UnsupportedOpt", new string[] { array2[0] });
						}
						throw new XslTransformException("Coll_UnsupportedOptVal", new string[]
						{
							array2[0],
							array2[1]
						});
					}
					IL_0479:;
				}
			}
			if (xmlCollation.upperFirst && (xmlCollation.compops & CompareOptions.IgnoreCase) != CompareOptions.None)
			{
				xmlCollation.upperFirst = false;
			}
			if ((xmlCollation.compops & CompareOptions.Ordinal) != CompareOptions.None)
			{
				xmlCollation.compops = CompareOptions.Ordinal;
				xmlCollation.upperFirst = false;
			}
			if (text2 != null && xmlCollation.cultinfo != null)
			{
				int langID = XmlCollation.GetLangID(xmlCollation.cultinfo.LCID);
				string text8;
				switch (text8 = text2)
				{
				case "bopo":
					if (langID == 1028)
					{
						xmlCollation.cultinfo = new CultureInfo(197636);
						return xmlCollation;
					}
					return xmlCollation;
				case "strk":
					if (langID == 2052 || langID == 3076 || langID == 4100 || langID == 5124)
					{
						xmlCollation.cultinfo = new CultureInfo(XmlCollation.MakeLCID(xmlCollation.cultinfo.LCID, 2));
						return xmlCollation;
					}
					return xmlCollation;
				case "uni":
					if (langID == 1041 || langID == 1042)
					{
						xmlCollation.cultinfo = new CultureInfo(XmlCollation.MakeLCID(xmlCollation.cultinfo.LCID, 1));
						return xmlCollation;
					}
					return xmlCollation;
				case "phn":
					if (langID == 1031)
					{
						xmlCollation.cultinfo = new CultureInfo(66567);
						return xmlCollation;
					}
					return xmlCollation;
				case "tech":
					if (langID == 1038)
					{
						xmlCollation.cultinfo = new CultureInfo(66574);
						return xmlCollation;
					}
					return xmlCollation;
				case "mod":
					if (langID == 1079)
					{
						xmlCollation.cultinfo = new CultureInfo(66615);
						return xmlCollation;
					}
					return xmlCollation;
				case "pron":
				case "dict":
				case "trad":
					return xmlCollation;
				}
				throw new XslTransformException("Coll_UnsupportedSortOpt", new string[] { text2 });
			}
			return xmlCollation;
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00027BB8 File Offset: 0x00026BB8
		private int GetOptions()
		{
			int num = (int)this.compops;
			if (this.upperFirst)
			{
				num |= 4096;
			}
			if (this.emptyGreatest)
			{
				num |= 8192;
			}
			if (this.descendingOrder)
			{
				num |= 16384;
			}
			return num;
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00027C00 File Offset: 0x00026C00
		private void SetOptions(int options)
		{
			this.upperFirst = (options & 4096) != 0;
			this.emptyGreatest = (options & 8192) != 0;
			this.descendingOrder = (options & 16384) != 0;
			this.compops = (CompareOptions)(options & -28673);
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00027C54 File Offset: 0x00026C54
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			XmlCollation xmlCollation = obj as XmlCollation;
			return xmlCollation != null && this.GetOptions() == xmlCollation.GetOptions() && object.Equals(this.cultinfo, xmlCollation.cultinfo);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00027C94 File Offset: 0x00026C94
		public override int GetHashCode()
		{
			int num = this.GetOptions();
			if (this.cultinfo != null)
			{
				num ^= this.cultinfo.GetHashCode();
			}
			return num;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00027CBF File Offset: 0x00026CBF
		internal void GetObjectData(BinaryWriter writer)
		{
			/*
An exception occurred when decompiling this method (060007CA)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Xml.Xsl.Runtime.XmlCollation::GetObjectData(System.IO.BinaryWriter)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILExpression..ctor(ILCode code, Object operand) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstTypes.cs:line 633
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 1010
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.ConvertToAst(List`1 body, HashSet`1 ehs) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 959
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 280
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00027CEC File Offset: 0x00026CEC
		internal XmlCollation(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			this.cultinfo = ((num != -1) ? new CultureInfo(num) : null);
			this.SetOptions(reader.ReadInt32());
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x00027D25 File Offset: 0x00026D25
		internal bool EmptyGreatest
		{
			get
			{
				return this.emptyGreatest;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060007CD RID: 1997 RVA: 0x00027D2D File Offset: 0x00026D2D
		internal bool DescendingOrder
		{
			get
			{
				return this.descendingOrder;
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x00027D35 File Offset: 0x00026D35
		internal CultureInfo Culture
		{
			get
			{
				if (this.cultinfo == null)
				{
					return CultureInfo.CurrentCulture;
				}
				return this.cultinfo;
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00027D4C File Offset: 0x00026D4C
		internal XmlSortKey CreateSortKey(string s)
		{
			SortKey sortKey = this.Culture.CompareInfo.GetSortKey(s, this.compops);
			if (!this.upperFirst)
			{
				return new XmlStringSortKey(sortKey, this.descendingOrder);
			}
			byte[] keyData = sortKey.KeyData;
			if (this.upperFirst && keyData.Length != 0)
			{
				int num = 0;
				while (keyData[num] != 1)
				{
					num++;
				}
				do
				{
					num++;
				}
				while (keyData[num] != 1);
				do
				{
					num++;
					byte[] array = keyData;
					int num2 = num;
					array[num2] ^= byte.MaxValue;
				}
				while (keyData[num] != 254);
			}
			return new XmlStringSortKey(keyData, this.descendingOrder);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00027DE5 File Offset: 0x00026DE5
		private static int MakeLCID(int langid, int sortid)
		{
			return (langid & 65535) | ((sortid & 15) << 16);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00027DF6 File Offset: 0x00026DF6
		private static int GetLangID(int lcid)
		{
			return lcid & 65535;
		}

		// Token: 0x04000530 RID: 1328
		private const int deDE = 1031;

		// Token: 0x04000531 RID: 1329
		private const int huHU = 1038;

		// Token: 0x04000532 RID: 1330
		private const int jaJP = 1041;

		// Token: 0x04000533 RID: 1331
		private const int kaGE = 1079;

		// Token: 0x04000534 RID: 1332
		private const int koKR = 1042;

		// Token: 0x04000535 RID: 1333
		private const int zhTW = 1028;

		// Token: 0x04000536 RID: 1334
		private const int zhCN = 2052;

		// Token: 0x04000537 RID: 1335
		private const int zhHK = 3076;

		// Token: 0x04000538 RID: 1336
		private const int zhSG = 4100;

		// Token: 0x04000539 RID: 1337
		private const int zhMO = 5124;

		// Token: 0x0400053A RID: 1338
		private const int zhTWbopo = 197636;

		// Token: 0x0400053B RID: 1339
		private const int deDEphon = 66567;

		// Token: 0x0400053C RID: 1340
		private const int huHUtech = 66574;

		// Token: 0x0400053D RID: 1341
		private const int kaGEmode = 66615;

		// Token: 0x0400053E RID: 1342
		private const int strksort = 2;

		// Token: 0x0400053F RID: 1343
		private const int unicsort = 1;

		// Token: 0x04000540 RID: 1344
		private const string ignoreCaseStr = "IGNORECASE";

		// Token: 0x04000541 RID: 1345
		private const string ignoreKanatypeStr = "IGNOREKANATYPE";

		// Token: 0x04000542 RID: 1346
		private const string ignoreNonspaceStr = "IGNORENONSPACE";

		// Token: 0x04000543 RID: 1347
		private const string ignoreSymbolsStr = "IGNORESYMBOLS";

		// Token: 0x04000544 RID: 1348
		private const string ignoreWidthStr = "IGNOREWIDTH";

		// Token: 0x04000545 RID: 1349
		private const string upperFirstStr = "UPPERFIRST";

		// Token: 0x04000546 RID: 1350
		private const string emptyGreatestStr = "EMPTYGREATEST";

		// Token: 0x04000547 RID: 1351
		private const string descendingOrderStr = "DESCENDINGORDER";

		// Token: 0x04000548 RID: 1352
		private const string sortStr = "SORT";

		// Token: 0x04000549 RID: 1353
		private const int FlagUpperFirst = 4096;

		// Token: 0x0400054A RID: 1354
		private const int FlagEmptyGreatest = 8192;

		// Token: 0x0400054B RID: 1355
		private const int FlagDescendingOrder = 16384;

		// Token: 0x0400054C RID: 1356
		private const int CollationFlagsMask = 28672;

		// Token: 0x0400054D RID: 1357
		private const int LOCALE_CURRENT = -1;

		// Token: 0x0400054E RID: 1358
		private bool upperFirst;

		// Token: 0x0400054F RID: 1359
		private bool emptyGreatest;

		// Token: 0x04000550 RID: 1360
		private bool descendingOrder;

		// Token: 0x04000551 RID: 1361
		private CultureInfo cultinfo;

		// Token: 0x04000552 RID: 1362
		private CompareOptions compops;

		// Token: 0x04000553 RID: 1363
		private static XmlCollation cp = new XmlCollation(CultureInfo.InvariantCulture, CompareOptions.Ordinal);
	}
}
