using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Xml.XmlConfiguration;
using System.Xml.XPath;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.Runtime;
using System.Xml.Xsl.Xslt;

namespace System.Xml.Xsl
{
	// Token: 0x02000171 RID: 369
	public sealed class XslCompiledTransform
	{
		// Token: 0x060013B1 RID: 5041 RVA: 0x000555C9 File Offset: 0x000545C9
		static XslCompiledTransform()
		{
			XslCompiledTransform.MemberAccessPermissionSet.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.MemberAccess));
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x000555E7 File Offset: 0x000545E7
		public XslCompiledTransform()
		{
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x000555EF File Offset: 0x000545EF
		public XslCompiledTransform(bool enableDebug)
		{
			this.enableDebug = enableDebug;
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x000555FE File Offset: 0x000545FE
		private void Reset()
		{
			this.compilerResults = null;
			this.outputSettings = null;
			this.qil = null;
			this.command = null;
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060013B5 RID: 5045 RVA: 0x0005561C File Offset: 0x0005461C
		internal CompilerErrorCollection Errors
		{
			get
			{
				if (this.compilerResults == null)
				{
					return null;
				}
				return this.compilerResults.Errors;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060013B6 RID: 5046 RVA: 0x00055633 File Offset: 0x00054633
		public XmlWriterSettings OutputSettings
		{
			get
			{
				return this.outputSettings;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060013B7 RID: 5047 RVA: 0x0005563B File Offset: 0x0005463B
		public TempFileCollection TemporaryFiles
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (this.compilerResults == null)
				{
					return null;
				}
				return this.compilerResults.TempFiles;
			}
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00055652 File Offset: 0x00054652
		public void Load(XmlReader stylesheet)
		{
			this.Reset();
			this.LoadInternal(stylesheet, XsltSettings.Default, XsltConfigSection.CreateDefaultResolver());
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0005566C File Offset: 0x0005466C
		public void Load(XmlReader stylesheet, XsltSettings settings, XmlResolver stylesheetResolver)
		{
			this.Reset();
			this.LoadInternal(stylesheet, settings, stylesheetResolver);
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0005567E File Offset: 0x0005467E
		public void Load(IXPathNavigable stylesheet)
		{
			this.Reset();
			this.LoadInternal(stylesheet, XsltSettings.Default, XsltConfigSection.CreateDefaultResolver());
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00055698 File Offset: 0x00054698
		public void Load(IXPathNavigable stylesheet, XsltSettings settings, XmlResolver stylesheetResolver)
		{
			this.Reset();
			this.LoadInternal(stylesheet, settings, stylesheetResolver);
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x000556AA File Offset: 0x000546AA
		public void Load(string stylesheetUri)
		{
			this.Reset();
			if (stylesheetUri == null)
			{
				throw new ArgumentNullException("stylesheetUri");
			}
			this.LoadInternal(stylesheetUri, XsltSettings.Default, XsltConfigSection.CreateDefaultResolver());
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x000556D2 File Offset: 0x000546D2
		public void Load(string stylesheetUri, XsltSettings settings, XmlResolver stylesheetResolver)
		{
			this.Reset();
			if (stylesheetUri == null)
			{
				throw new ArgumentNullException("stylesheetUri");
			}
			this.LoadInternal(stylesheetUri, settings, stylesheetResolver);
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x000556F4 File Offset: 0x000546F4
		private CompilerResults LoadInternal(object stylesheet, XsltSettings settings, XmlResolver stylesheetResolver)
		{
			if (stylesheet == null)
			{
				throw new ArgumentNullException("stylesheet");
			}
			if (settings == null)
			{
				settings = XsltSettings.Default;
			}
			this.CompileXsltToQil(stylesheet, settings, stylesheetResolver);
			CompilerError firstError = this.GetFirstError();
			if (firstError != null)
			{
				throw new XslLoadException(firstError);
			}
			if (!settings.CheckOnly)
			{
				this.CompileQilToMsil(settings);
			}
			return this.compilerResults;
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00055748 File Offset: 0x00054748
		private void CompileXsltToQil(object stylesheet, XsltSettings settings, XmlResolver stylesheetResolver)
		{
			this.compilerResults = new Compiler(settings, this.enableDebug, null).Compile(stylesheet, stylesheetResolver, out this.qil);
		}

		// Token: 0x060013C0 RID: 5056 RVA: 0x0005576C File Offset: 0x0005476C
		private CompilerError GetFirstError()
		{
			foreach (object obj in this.compilerResults.Errors)
			{
				CompilerError compilerError = (CompilerError)obj;
				if (!compilerError.IsWarning)
				{
					return compilerError;
				}
			}
			return null;
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x000557D4 File Offset: 0x000547D4
		private void CompileQilToMsil(XsltSettings settings)
		{
			this.command = new XmlILGenerator().Generate(this.qil, null);
			this.outputSettings = this.command.StaticData.DefaultWriterSettings;
			this.qil = null;
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0005580C File Offset: 0x0005480C
		public static CompilerErrorCollection CompileToType(XmlReader stylesheet, XsltSettings settings, XmlResolver stylesheetResolver, bool debug, TypeBuilder typeBuilder, string scriptAssemblyPath)
		{
			if (stylesheet == null)
			{
				throw new ArgumentNullException("stylesheet");
			}
			if (typeBuilder == null)
			{
				throw new ArgumentNullException("typeBuilder");
			}
			if (settings == null)
			{
				settings = XsltSettings.Default;
			}
			if (settings.EnableScript && scriptAssemblyPath == null)
			{
				throw new ArgumentNullException("scriptAssemblyPath");
			}
			if (scriptAssemblyPath != null)
			{
				scriptAssemblyPath = Path.GetFullPath(scriptAssemblyPath);
			}
			QilExpression qilExpression;
			CompilerErrorCollection errors = new Compiler(settings, debug, scriptAssemblyPath).Compile(stylesheet, stylesheetResolver, out qilExpression).Errors;
			if (!errors.HasErrors)
			{
				if (XslCompiledTransform.GeneratedCodeCtor == null)
				{
					XslCompiledTransform.GeneratedCodeCtor = typeof(GeneratedCodeAttribute).GetConstructor(new Type[]
					{
						typeof(string),
						typeof(string)
					});
				}
				typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(XslCompiledTransform.GeneratedCodeCtor, new object[]
				{
					typeof(XslCompiledTransform).FullName,
					"2.0.0.0"
				}));
				new XmlILGenerator().Generate(qilExpression, typeBuilder);
			}
			return errors;
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x00055904 File Offset: 0x00054904
		public void Load(Type compiledStylesheet)
		{
			this.Reset();
			if (compiledStylesheet == null)
			{
				throw new ArgumentNullException("compiledStylesheet");
			}
			object[] customAttributes = compiledStylesheet.GetCustomAttributes(typeof(GeneratedCodeAttribute), false);
			GeneratedCodeAttribute generatedCodeAttribute = ((customAttributes.Length > 0) ? ((GeneratedCodeAttribute)customAttributes[0]) : null);
			if (generatedCodeAttribute != null && generatedCodeAttribute.Tool == typeof(XslCompiledTransform).FullName && generatedCodeAttribute.Version == "2.0.0.0")
			{
				FieldInfo field = compiledStylesheet.GetField("staticData", BindingFlags.Static | BindingFlags.NonPublic);
				FieldInfo field2 = compiledStylesheet.GetField("ebTypes", BindingFlags.Static | BindingFlags.NonPublic);
				if (field != null && field2 != null)
				{
					if (XsltConfigSection.EnableMemberAccessForXslCompiledTransform)
					{
						new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
					}
					object obj = field.GetValue(null);
					byte[] array = obj as byte[];
					if (array != null)
					{
						lock (array)
						{
							obj = field.GetValue(null);
							if (obj == array)
							{
								MethodInfo method = compiledStylesheet.GetMethod("Execute", BindingFlags.Static | BindingFlags.NonPublic);
								Delegate @delegate = Delegate.CreateDelegate(typeof(ExecuteDelegate), method);
								obj = new XmlILCommand((ExecuteDelegate)@delegate, new XmlQueryStaticData(array, (Type[])field2.GetValue(null)));
								Thread.MemoryBarrier();
								field.SetValue(null, obj);
							}
						}
					}
					this.command = obj as XmlILCommand;
				}
			}
			if (this.command == null)
			{
				throw new ArgumentException(Res.GetString("Xslt_NotCompiledStylesheet", new object[] { compiledStylesheet.FullName }), "compiledStylesheet");
			}
			this.outputSettings = this.command.StaticData.DefaultWriterSettings;
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x00055AAC File Offset: 0x00054AAC
		public void Load(MethodInfo executeMethod, byte[] queryData, Type[] earlyBoundTypes)
		{
			this.Reset();
			if (executeMethod == null)
			{
				throw new ArgumentNullException("executeMethod");
			}
			if (queryData == null)
			{
				throw new ArgumentNullException("queryData");
			}
			if (!XsltConfigSection.EnableMemberAccessForXslCompiledTransform && executeMethod.DeclaringType != null && !executeMethod.DeclaringType.IsVisible)
			{
				new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Demand();
			}
			DynamicMethod dynamicMethod = executeMethod as DynamicMethod;
			Delegate @delegate = ((dynamicMethod != null) ? dynamicMethod.CreateDelegate(typeof(ExecuteDelegate)) : Delegate.CreateDelegate(typeof(ExecuteDelegate), executeMethod));
			this.command = new XmlILCommand((ExecuteDelegate)@delegate, new XmlQueryStaticData(queryData, earlyBoundTypes));
			this.outputSettings = this.command.StaticData.DefaultWriterSettings;
		}

		// Token: 0x060013C5 RID: 5061 RVA: 0x00055B5D File Offset: 0x00054B5D
		public void Transform(IXPathNavigable input, XmlWriter results)
		{
			this.CheckCommand();
			this.CheckInput(input);
			this.command.Execute(input, XsltConfigSection.CreateDefaultResolver(), null, results);
		}

		// Token: 0x060013C6 RID: 5062 RVA: 0x00055B7F File Offset: 0x00054B7F
		public void Transform(IXPathNavigable input, XsltArgumentList arguments, XmlWriter results)
		{
			this.CheckCommand();
			this.CheckInput(input);
			this.command.Execute(input, XsltConfigSection.CreateDefaultResolver(), arguments, results);
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00055BA1 File Offset: 0x00054BA1
		public void Transform(IXPathNavigable input, XsltArgumentList arguments, TextWriter results)
		{
			this.CheckCommand();
			this.CheckInput(input);
			this.command.Execute(input, XsltConfigSection.CreateDefaultResolver(), arguments, results);
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00055BC3 File Offset: 0x00054BC3
		public void Transform(IXPathNavigable input, XsltArgumentList arguments, Stream results)
		{
			this.CheckCommand();
			this.CheckInput(input);
			this.command.Execute(input, XsltConfigSection.CreateDefaultResolver(), arguments, results);
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00055BE5 File Offset: 0x00054BE5
		public void Transform(IXPathNavigable input, XsltArgumentList arguments, XmlWriter results, XmlResolver documentResolver)
		{
			this.CheckInput(input);
			this.CheckCommand();
			this.command.Execute(input, documentResolver, arguments, results);
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00055C04 File Offset: 0x00054C04
		public void Transform(XmlReader input, XmlWriter results)
		{
			this.CheckCommand();
			this.CheckInput(input);
			this.command.Execute(input, XsltConfigSection.CreateDefaultResolver(), null, results);
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00055C26 File Offset: 0x00054C26
		public void Transform(XmlReader input, XsltArgumentList arguments, XmlWriter results)
		{
			this.CheckCommand();
			this.CheckInput(input);
			this.command.Execute(input, XsltConfigSection.CreateDefaultResolver(), arguments, results);
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00055C48 File Offset: 0x00054C48
		public void Transform(XmlReader input, XsltArgumentList arguments, TextWriter results)
		{
			this.CheckCommand();
			this.CheckInput(input);
			this.command.Execute(input, XsltConfigSection.CreateDefaultResolver(), arguments, results);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00055C6A File Offset: 0x00054C6A
		public void Transform(XmlReader input, XsltArgumentList arguments, Stream results)
		{
			this.CheckCommand();
			this.CheckInput(input);
			this.command.Execute(input, XsltConfigSection.CreateDefaultResolver(), arguments, results);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00055C8C File Offset: 0x00054C8C
		public void Transform(XmlReader input, XsltArgumentList arguments, XmlWriter results, XmlResolver documentResolver)
		{
			this.CheckCommand();
			this.CheckInput(input);
			this.command.Execute(input, documentResolver, arguments, results);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00055CAC File Offset: 0x00054CAC
		public void Transform(string inputUri, string resultsFile)
		{
			this.CheckCommand();
			using (XmlReader xmlReader = this.CreateReader(inputUri))
			{
				if (resultsFile == null)
				{
					throw new ArgumentNullException("resultsFile");
				}
				using (FileStream fileStream = new FileStream(resultsFile, FileMode.Create, FileAccess.Write))
				{
					this.command.Execute(xmlReader, XsltConfigSection.CreateDefaultResolver(), null, fileStream);
				}
			}
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00055D28 File Offset: 0x00054D28
		public void Transform(string inputUri, XmlWriter results)
		{
			this.CheckCommand();
			using (XmlReader xmlReader = this.CreateReader(inputUri))
			{
				this.command.Execute(xmlReader, XsltConfigSection.CreateDefaultResolver(), null, results);
			}
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00055D74 File Offset: 0x00054D74
		public void Transform(string inputUri, XsltArgumentList arguments, XmlWriter results)
		{
			this.CheckCommand();
			using (XmlReader xmlReader = this.CreateReader(inputUri))
			{
				this.command.Execute(xmlReader, XsltConfigSection.CreateDefaultResolver(), arguments, results);
			}
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x00055DC0 File Offset: 0x00054DC0
		public void Transform(string inputUri, XsltArgumentList arguments, TextWriter results)
		{
			this.CheckCommand();
			using (XmlReader xmlReader = this.CreateReader(inputUri))
			{
				this.command.Execute(xmlReader, XsltConfigSection.CreateDefaultResolver(), arguments, results);
			}
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00055E0C File Offset: 0x00054E0C
		public void Transform(string inputUri, XsltArgumentList arguments, Stream results)
		{
			this.CheckCommand();
			using (XmlReader xmlReader = this.CreateReader(inputUri))
			{
				this.command.Execute(xmlReader, XsltConfigSection.CreateDefaultResolver(), arguments, results);
			}
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x00055E58 File Offset: 0x00054E58
		private void CheckCommand()
		{
			if (this.command == null)
			{
				throw new InvalidOperationException(Res.GetString("Xslt_NoStylesheetLoaded"));
			}
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00055E72 File Offset: 0x00054E72
		private void CheckInput(object input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00055E82 File Offset: 0x00054E82
		private XmlReader CreateReader(string inputUri)
		{
			if (inputUri == null)
			{
				throw new ArgumentNullException("inputUri");
			}
			return XmlReader.Create(inputUri);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00055E98 File Offset: 0x00054E98
		private QilExpression TestCompile(object stylesheet, XsltSettings settings, XmlResolver stylesheetResolver)
		{
			this.Reset();
			this.CompileXsltToQil(stylesheet, settings, stylesheetResolver);
			return this.qil;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00055EAF File Offset: 0x00054EAF
		private void TestGenerate(XsltSettings settings)
		{
			this.CompileQilToMsil(settings);
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00055EB8 File Offset: 0x00054EB8
		private void Transform(string inputUri, XsltArgumentList arguments, XmlWriter results, XmlResolver documentResolver)
		{
			this.command.Execute(inputUri, documentResolver, arguments, results);
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00055ECC File Offset: 0x00054ECC
		internal static void PrintQil(object qil, XmlWriter xw, bool printComments, bool printTypes, bool printLineInfo)
		{
			QilExpression qilExpression = (QilExpression)qil;
			QilXmlWriter.Options options = QilXmlWriter.Options.None;
			if (printComments)
			{
				options |= QilXmlWriter.Options.Annotations;
			}
			if (printTypes)
			{
				options |= QilXmlWriter.Options.TypeInfo;
			}
			if (printLineInfo)
			{
				options |= QilXmlWriter.Options.LineInfo;
			}
			QilXmlWriter qilXmlWriter = new QilXmlWriter(xw, options);
			qilXmlWriter.ToXml(qilExpression);
			xw.Flush();
		}

		// Token: 0x04000C32 RID: 3122
		private const string Version = "2.0.0.0";

		// Token: 0x04000C33 RID: 3123
		private static readonly PermissionSet MemberAccessPermissionSet = new PermissionSet(PermissionState.None);

		// Token: 0x04000C34 RID: 3124
		private bool enableDebug;

		// Token: 0x04000C35 RID: 3125
		private CompilerResults compilerResults;

		// Token: 0x04000C36 RID: 3126
		private XmlWriterSettings outputSettings;

		// Token: 0x04000C37 RID: 3127
		private QilExpression qil;

		// Token: 0x04000C38 RID: 3128
		private XmlILCommand command;

		// Token: 0x04000C39 RID: 3129
		private static ConstructorInfo GeneratedCodeCtor;
	}
}
