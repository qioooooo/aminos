using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Globalization;
using System.IO;

namespace System.Data.Design
{
	// Token: 0x020000A8 RID: 168
	public class MethodSignatureGenerator
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x000122DC File Offset: 0x000112DC
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x000122E4 File Offset: 0x000112E4
		public CodeDomProvider CodeProvider
		{
			get
			{
				return this.codeProvider;
			}
			set
			{
				this.codeProvider = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060007D3 RID: 2003 RVA: 0x000122ED File Offset: 0x000112ED
		// (set) Token: 0x060007D4 RID: 2004 RVA: 0x000122F5 File Offset: 0x000112F5
		public Type ContainerParameterType
		{
			get
			{
				return this.containerParameterType;
			}
			set
			{
				if (value != typeof(DataSet) && value != typeof(DataTable))
				{
					throw new InternalException("Unsupported container parameter type.");
				}
				this.containerParameterType = value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060007D5 RID: 2005 RVA: 0x00012323 File Offset: 0x00011323
		// (set) Token: 0x060007D6 RID: 2006 RVA: 0x0001232B File Offset: 0x0001132B
		public bool IsGetMethod
		{
			get
			{
				return this.getMethod;
			}
			set
			{
				this.getMethod = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x00012334 File Offset: 0x00011334
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x0001233C File Offset: 0x0001133C
		public bool PagingMethod
		{
			get
			{
				return this.pagingMethod;
			}
			set
			{
				this.pagingMethod = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00012345 File Offset: 0x00011345
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x0001234D File Offset: 0x0001134D
		public ParameterGenerationOption ParameterOption
		{
			get
			{
				return this.parameterOption;
			}
			set
			{
				this.parameterOption = value;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x00012356 File Offset: 0x00011356
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0001235E File Offset: 0x0001135E
		public string TableClassName
		{
			get
			{
				return this.tableClassName;
			}
			set
			{
				this.tableClassName = value;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x00012367 File Offset: 0x00011367
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x0001236F File Offset: 0x0001136F
		public string DataSetClassName
		{
			get
			{
				return this.datasetClassName;
			}
			set
			{
				this.datasetClassName = value;
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00012378 File Offset: 0x00011378
		public void SetDesignTableContent(string designTableContent)
		{
			DesignDataSource designDataSource = new DesignDataSource();
			StringReader stringReader = new StringReader(designTableContent);
			designDataSource.ReadXmlSchema(stringReader);
			if (designDataSource.DesignTables == null || designDataSource.DesignTables.Count != 1)
			{
				throw new InternalException("Unexpected number of sources in deserialized DataSource.");
			}
			IEnumerator enumerator = designDataSource.DesignTables.GetEnumerator();
			enumerator.MoveNext();
			this.designTable = (DesignTable)enumerator.Current;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000123E0 File Offset: 0x000113E0
		public void SetMethodSourceContent(string methodSourceContent)
		{
			DesignDataSource designDataSource = new DesignDataSource();
			StringReader stringReader = new StringReader(methodSourceContent);
			designDataSource.ReadXmlSchema(stringReader);
			if (designDataSource.Sources == null || designDataSource.Sources.Count != 1)
			{
				throw new InternalException("Unexpected number of sources in deserialized DataSource.");
			}
			IEnumerator enumerator = designDataSource.Sources.GetEnumerator();
			enumerator.MoveNext();
			this.methodSource = (DbSource)enumerator.Current;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00012448 File Offset: 0x00011448
		public string GenerateMethodSignature()
		{
			if (this.codeProvider == null)
			{
				throw new ArgumentException("codeProvider");
			}
			if (this.methodSource == null)
			{
				throw new ArgumentException("MethodSource");
			}
			string text = null;
			CodeTypeDeclaration codeTypeDeclaration = this.GenerateMethodWrapper(out text);
			StringWriter stringWriter = new StringWriter(CultureInfo.CurrentCulture);
			this.codeProvider.GenerateCodeFromType(codeTypeDeclaration, stringWriter, null);
			string text2 = stringWriter.GetStringBuilder().ToString();
			string[] array = text2.Split(Environment.NewLine.ToCharArray());
			foreach (string text3 in array)
			{
				if (text3.Contains(text))
				{
					return text3.Trim().TrimEnd(new char[] { MethodSignatureGenerator.endOfStatement });
				}
			}
			return null;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001250C File Offset: 0x0001150C
		private CodeTypeDeclaration GenerateMethodWrapper(out string methodName)
		{
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration("Wrapper");
			codeTypeDeclaration.IsInterface = true;
			CodeMemberMethod codeMemberMethod = this.GenerateMethod();
			codeTypeDeclaration.Members.Add(codeMemberMethod);
			methodName = codeMemberMethod.Name;
			return codeTypeDeclaration;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00012548 File Offset: 0x00011548
		public CodeMemberMethod GenerateMethod()
		{
			if (this.codeProvider == null)
			{
				throw new ArgumentException("codeProvider");
			}
			if (this.methodSource == null)
			{
				throw new ArgumentException("MethodSource");
			}
			QueryGeneratorBase queryGeneratorBase;
			if (this.methodSource.QueryType == QueryType.Rowset && this.methodSource.CommandOperation == CommandOperation.Select)
			{
				queryGeneratorBase = new QueryGenerator(null);
				queryGeneratorBase.ContainerParameterTypeName = this.GetParameterTypeName();
				queryGeneratorBase.ContainerParameterName = this.GetParameterName();
				queryGeneratorBase.ContainerParameterType = this.containerParameterType;
			}
			else
			{
				queryGeneratorBase = new FunctionGenerator(null);
			}
			queryGeneratorBase.DeclarationOnly = true;
			queryGeneratorBase.CodeProvider = this.codeProvider;
			queryGeneratorBase.MethodSource = this.methodSource;
			queryGeneratorBase.MethodName = this.GetMethodName();
			queryGeneratorBase.ParameterOption = this.parameterOption;
			queryGeneratorBase.GeneratePagingMethod = this.pagingMethod;
			queryGeneratorBase.GenerateGetMethod = this.getMethod;
			return queryGeneratorBase.Generate();
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00012624 File Offset: 0x00011624
		public CodeTypeDeclaration GenerateUpdatingMethods()
		{
			if (this.designTable == null)
			{
				throw new InternalException("DesignTable should not be null.");
			}
			if (StringUtil.Empty(this.datasetClassName))
			{
				throw new InternalException("DatasetClassName should not be empty.");
			}
			CodeTypeDeclaration codeTypeDeclaration = new CodeTypeDeclaration("wrapper");
			codeTypeDeclaration.IsInterface = true;
			new QueryHandler(this.codeProvider, this.designTable)
			{
				DeclarationsOnly = true
			}.AddUpdateQueriesToDataComponent(codeTypeDeclaration, this.datasetClassName, this.codeProvider);
			return codeTypeDeclaration;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001269B File Offset: 0x0001169B
		private string GetParameterName()
		{
			if (this.containerParameterType == typeof(DataTable))
			{
				return "dataTable";
			}
			return "dataSet";
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x000126BC File Offset: 0x000116BC
		private string GetParameterTypeName()
		{
			if (StringUtil.Empty(this.datasetClassName))
			{
				throw new InternalException("DatasetClassName should not be empty.");
			}
			if (this.containerParameterType != typeof(DataTable))
			{
				return this.datasetClassName;
			}
			if (StringUtil.Empty(this.tableClassName))
			{
				throw new InternalException("TableClassName should not be empty.");
			}
			return CodeGenHelper.GetTypeName(this.codeProvider, this.datasetClassName, this.tableClassName);
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x0001272C File Offset: 0x0001172C
		private string GetMethodName()
		{
			if (this.methodSource.QueryType == QueryType.Rowset)
			{
				if (this.getMethod)
				{
					if (this.pagingMethod)
					{
						if (this.methodSource.GeneratorGetMethodNameForPaging != null)
						{
							return this.methodSource.GeneratorGetMethodNameForPaging;
						}
						return this.methodSource.GetMethodName + DataComponentNameHandler.PagingMethodSuffix;
					}
					else
					{
						if (this.methodSource.GeneratorGetMethodName != null)
						{
							return this.methodSource.GeneratorGetMethodName;
						}
						return this.methodSource.GetMethodName;
					}
				}
				else if (this.pagingMethod)
				{
					if (this.methodSource.GeneratorSourceNameForPaging != null)
					{
						return this.methodSource.GeneratorSourceNameForPaging;
					}
					return this.methodSource.Name + DataComponentNameHandler.PagingMethodSuffix;
				}
				else
				{
					if (this.methodSource.GeneratorSourceName != null)
					{
						return this.methodSource.GeneratorSourceName;
					}
					return this.methodSource.Name;
				}
			}
			else
			{
				if (this.methodSource.GeneratorSourceName != null)
				{
					return this.methodSource.GeneratorSourceName;
				}
				return this.methodSource.Name;
			}
		}

		// Token: 0x04000BC5 RID: 3013
		private static readonly char endOfStatement = ';';

		// Token: 0x04000BC6 RID: 3014
		private CodeDomProvider codeProvider;

		// Token: 0x04000BC7 RID: 3015
		private DbSource methodSource;

		// Token: 0x04000BC8 RID: 3016
		private Type containerParameterType = typeof(DataSet);

		// Token: 0x04000BC9 RID: 3017
		private bool pagingMethod;

		// Token: 0x04000BCA RID: 3018
		private bool getMethod;

		// Token: 0x04000BCB RID: 3019
		private ParameterGenerationOption parameterOption;

		// Token: 0x04000BCC RID: 3020
		private string tableClassName;

		// Token: 0x04000BCD RID: 3021
		private string datasetClassName;

		// Token: 0x04000BCE RID: 3022
		private DesignTable designTable;
	}
}
