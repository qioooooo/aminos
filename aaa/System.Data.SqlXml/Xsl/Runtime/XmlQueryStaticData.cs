using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Xsl.IlGen;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x020000B9 RID: 185
	internal class XmlQueryStaticData
	{
		// Token: 0x06000910 RID: 2320 RVA: 0x0002B5BC File Offset: 0x0002A5BC
		public XmlQueryStaticData(XmlWriterSettings defaultWriterSettings, IList<WhitespaceRule> whitespaceRules, StaticDataManager staticData)
		{
			this.defaultWriterSettings = defaultWriterSettings;
			this.whitespaceRules = whitespaceRules;
			this.names = staticData.Names;
			this.prefixMappingsList = staticData.PrefixMappingsList;
			this.filters = staticData.NameFilters;
			this.types = staticData.XmlTypes;
			this.collations = staticData.Collations;
			this.globalNames = staticData.GlobalNames;
			this.earlyBound = staticData.EarlyBound;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x0002B634 File Offset: 0x0002A634
		public XmlQueryStaticData(byte[] data, Type[] ebTypes)
		{
			MemoryStream memoryStream = new MemoryStream(data, false);
			XmlQueryDataReader xmlQueryDataReader = new XmlQueryDataReader(memoryStream);
			int num = xmlQueryDataReader.ReadInt32Encoded();
			if ((num & -256) > 0)
			{
				throw new NotSupportedException();
			}
			this.defaultWriterSettings = new XmlWriterSettings(xmlQueryDataReader);
			int num2 = xmlQueryDataReader.ReadInt32();
			if (num2 != 0)
			{
				this.whitespaceRules = new WhitespaceRule[num2];
				for (int i = 0; i < num2; i++)
				{
					this.whitespaceRules[i] = new WhitespaceRule(xmlQueryDataReader);
				}
			}
			num2 = xmlQueryDataReader.ReadInt32();
			if (num2 != 0)
			{
				this.names = new string[num2];
				for (int j = 0; j < num2; j++)
				{
					this.names[j] = xmlQueryDataReader.ReadString();
				}
			}
			num2 = xmlQueryDataReader.ReadInt32();
			if (num2 != 0)
			{
				this.prefixMappingsList = new StringPair[num2][];
				for (int k = 0; k < num2; k++)
				{
					int num3 = xmlQueryDataReader.ReadInt32();
					this.prefixMappingsList[k] = new StringPair[num3];
					for (int l = 0; l < num3; l++)
					{
						this.prefixMappingsList[k][l] = new StringPair(xmlQueryDataReader.ReadString(), xmlQueryDataReader.ReadString());
					}
				}
			}
			num2 = xmlQueryDataReader.ReadInt32();
			if (num2 != 0)
			{
				this.filters = new Int32Pair[num2];
				for (int m = 0; m < num2; m++)
				{
					this.filters[m] = new Int32Pair(xmlQueryDataReader.ReadInt32Encoded(), xmlQueryDataReader.ReadInt32Encoded());
				}
			}
			num2 = xmlQueryDataReader.ReadInt32();
			if (num2 != 0)
			{
				this.types = new XmlQueryType[num2];
				for (int n = 0; n < num2; n++)
				{
					this.types[n] = XmlQueryTypeFactory.Deserialize(xmlQueryDataReader);
				}
			}
			num2 = xmlQueryDataReader.ReadInt32();
			if (num2 != 0)
			{
				this.collations = new XmlCollation[num2];
				for (int num4 = 0; num4 < num2; num4++)
				{
					this.collations[num4] = new XmlCollation(xmlQueryDataReader);
				}
			}
			num2 = xmlQueryDataReader.ReadInt32();
			if (num2 != 0)
			{
				this.globalNames = new string[num2];
				for (int num5 = 0; num5 < num2; num5++)
				{
					this.globalNames[num5] = xmlQueryDataReader.ReadString();
				}
			}
			num2 = xmlQueryDataReader.ReadInt32();
			if (num2 != 0)
			{
				this.earlyBound = new EarlyBoundInfo[num2];
				for (int num6 = 0; num6 < num2; num6++)
				{
					this.earlyBound[num6] = new EarlyBoundInfo(xmlQueryDataReader.ReadString(), ebTypes[num6]);
				}
			}
			xmlQueryDataReader.Close();
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x0002B888 File Offset: 0x0002A888
		public void GetObjectData(out byte[] data, out Type[] ebTypes)
		{
			/*
An exception occurred when decompiling this method (06000912)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Xml.Xsl.Runtime.XmlQueryStaticData::GetObjectData(System.Byte[]&,System.Type[]&)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.VariableSlot.CloneVariableState(VariableSlot[] state) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 78
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.StackAnalysis(MethodDef methodDef) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 407
   at ICSharpCode.Decompiler.ILAst.ILAstBuilder.Build(MethodDef methodDef, Boolean optimize, DecompilerContext context) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstBuilder.cs:line 278
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 117
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000913 RID: 2323 RVA: 0x0002BBB8 File Offset: 0x0002ABB8
		public XmlWriterSettings DefaultWriterSettings
		{
			get
			{
				return this.defaultWriterSettings;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x0002BBC0 File Offset: 0x0002ABC0
		public IList<WhitespaceRule> WhitespaceRules
		{
			get
			{
				return this.whitespaceRules;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0002BBC8 File Offset: 0x0002ABC8
		public string[] Names
		{
			get
			{
				return this.names;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x0002BBD0 File Offset: 0x0002ABD0
		public StringPair[][] PrefixMappingsList
		{
			get
			{
				return this.prefixMappingsList;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x0002BBD8 File Offset: 0x0002ABD8
		public Int32Pair[] Filters
		{
			get
			{
				return this.filters;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x0002BBE0 File Offset: 0x0002ABE0
		public XmlQueryType[] Types
		{
			get
			{
				return this.types;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x0002BBE8 File Offset: 0x0002ABE8
		public XmlCollation[] Collations
		{
			get
			{
				return this.collations;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0002BBF0 File Offset: 0x0002ABF0
		public string[] GlobalNames
		{
			get
			{
				return this.globalNames;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0002BBF8 File Offset: 0x0002ABF8
		public EarlyBoundInfo[] EarlyBound
		{
			get
			{
				return this.earlyBound;
			}
		}

		// Token: 0x040005AD RID: 1453
		public const string DataFieldName = "staticData";

		// Token: 0x040005AE RID: 1454
		public const string TypesFieldName = "ebTypes";

		// Token: 0x040005AF RID: 1455
		private const int CurrentFormatVersion = 0;

		// Token: 0x040005B0 RID: 1456
		private XmlWriterSettings defaultWriterSettings;

		// Token: 0x040005B1 RID: 1457
		private IList<WhitespaceRule> whitespaceRules;

		// Token: 0x040005B2 RID: 1458
		private string[] names;

		// Token: 0x040005B3 RID: 1459
		private StringPair[][] prefixMappingsList;

		// Token: 0x040005B4 RID: 1460
		private Int32Pair[] filters;

		// Token: 0x040005B5 RID: 1461
		private XmlQueryType[] types;

		// Token: 0x040005B6 RID: 1462
		private XmlCollation[] collations;

		// Token: 0x040005B7 RID: 1463
		private string[] globalNames;

		// Token: 0x040005B8 RID: 1464
		private EarlyBoundInfo[] earlyBound;
	}
}
