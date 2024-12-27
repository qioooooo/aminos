using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Policy;
using System.Xml.XmlConfiguration;
using System.Xml.XPath;
using System.Xml.Xsl.XsltOld;
using System.Xml.Xsl.XsltOld.Debugger;

namespace System.Xml.Xsl
{
	// Token: 0x02000179 RID: 377
	[Obsolete("This class has been deprecated. Please use System.Xml.Xsl.XslCompiledTransform instead. http://go.microsoft.com/fwlink/?linkid=14202")]
	public sealed class XslTransform
	{
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x06001409 RID: 5129 RVA: 0x0005641D File Offset: 0x0005541D
		private XmlResolver _DocumentResolver
		{
			get
			{
				if (this.isDocumentResolverSet)
				{
					return this._documentResolver;
				}
				return XsltConfigSection.CreateDefaultResolver();
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x00056433 File Offset: 0x00055433
		public XslTransform()
		{
		}

		// Token: 0x170004D9 RID: 1241
		// (set) Token: 0x0600140B RID: 5131 RVA: 0x0005643B File Offset: 0x0005543B
		public XmlResolver XmlResolver
		{
			set
			{
				this._documentResolver = value;
				this.isDocumentResolverSet = true;
			}
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0005644B File Offset: 0x0005544B
		public void Load(XmlReader stylesheet)
		{
			this.Load(stylesheet, XsltConfigSection.CreateDefaultResolver());
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x00056459 File Offset: 0x00055459
		public void Load(XmlReader stylesheet, XmlResolver resolver)
		{
			this.Load(new XPathDocument(stylesheet, XmlSpace.Preserve), resolver);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x00056469 File Offset: 0x00055469
		public void Load(IXPathNavigable stylesheet)
		{
			this.Load(stylesheet, XsltConfigSection.CreateDefaultResolver());
		}

		// Token: 0x0600140F RID: 5135 RVA: 0x00056477 File Offset: 0x00055477
		public void Load(IXPathNavigable stylesheet, XmlResolver resolver)
		{
			if (stylesheet == null)
			{
				throw new ArgumentNullException("stylesheet");
			}
			this.Load(stylesheet.CreateNavigator(), resolver);
		}

		// Token: 0x06001410 RID: 5136 RVA: 0x00056494 File Offset: 0x00055494
		public void Load(XPathNavigator stylesheet)
		{
			if (stylesheet == null)
			{
				throw new ArgumentNullException("stylesheet");
			}
			this.Load(stylesheet, XsltConfigSection.CreateDefaultResolver());
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000564B0 File Offset: 0x000554B0
		public void Load(XPathNavigator stylesheet, XmlResolver resolver)
		{
			if (stylesheet == null)
			{
				throw new ArgumentNullException("stylesheet");
			}
			if (resolver == null)
			{
				resolver = new XmlNullResolver();
			}
			this.Compile(stylesheet, resolver, null);
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x000564D4 File Offset: 0x000554D4
		public void Load(string url)
		{
			XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl(url);
			Evidence evidence = XmlSecureResolver.CreateEvidenceForUrl(xmlTextReaderImpl.BaseURI);
			this.Compile(Compiler.LoadDocument(xmlTextReaderImpl).CreateNavigator(), XsltConfigSection.CreateDefaultResolver(), evidence);
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0005650C File Offset: 0x0005550C
		public void Load(string url, XmlResolver resolver)
		{
			XmlTextReaderImpl xmlTextReaderImpl = new XmlTextReaderImpl(url);
			xmlTextReaderImpl.XmlResolver = resolver;
			Evidence evidence = XmlSecureResolver.CreateEvidenceForUrl(xmlTextReaderImpl.BaseURI);
			if (resolver == null)
			{
				resolver = new XmlNullResolver();
			}
			this.Compile(Compiler.LoadDocument(xmlTextReaderImpl).CreateNavigator(), resolver, evidence);
		}

		// Token: 0x06001414 RID: 5140 RVA: 0x00056550 File Offset: 0x00055550
		public void Load(IXPathNavigable stylesheet, XmlResolver resolver, Evidence evidence)
		{
			if (stylesheet == null)
			{
				throw new ArgumentNullException("stylesheet");
			}
			this.Load(stylesheet.CreateNavigator(), resolver, evidence);
		}

		// Token: 0x06001415 RID: 5141 RVA: 0x0005656E File Offset: 0x0005556E
		public void Load(XmlReader stylesheet, XmlResolver resolver, Evidence evidence)
		{
			if (stylesheet == null)
			{
				throw new ArgumentNullException("stylesheet");
			}
			this.Load(new XPathDocument(stylesheet, XmlSpace.Preserve), resolver, evidence);
		}

		// Token: 0x06001416 RID: 5142 RVA: 0x0005658D File Offset: 0x0005558D
		public void Load(XPathNavigator stylesheet, XmlResolver resolver, Evidence evidence)
		{
			if (stylesheet == null)
			{
				throw new ArgumentNullException("stylesheet");
			}
			if (resolver == null)
			{
				resolver = new XmlNullResolver();
			}
			if (evidence == null)
			{
				evidence = new Evidence();
			}
			else
			{
				new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
			}
			this.Compile(stylesheet, resolver, evidence);
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x000565C8 File Offset: 0x000555C8
		private void CheckCommand()
		{
			if (this._CompiledStylesheet == null)
			{
				throw new InvalidOperationException(Res.GetString("Xslt_NoStylesheetLoaded"));
			}
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x000565E4 File Offset: 0x000555E4
		public XmlReader Transform(XPathNavigator input, XsltArgumentList args, XmlResolver resolver)
		{
			this.CheckCommand();
			Processor processor = new Processor(input, args, resolver, this._CompiledStylesheet, this._QueryStore, this._RootAction, this.debugger);
			return processor.StartReader();
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0005661E File Offset: 0x0005561E
		public XmlReader Transform(XPathNavigator input, XsltArgumentList args)
		{
			return this.Transform(input, args, this._DocumentResolver);
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x00056630 File Offset: 0x00055630
		public void Transform(XPathNavigator input, XsltArgumentList args, XmlWriter output, XmlResolver resolver)
		{
			this.CheckCommand();
			Processor processor = new Processor(input, args, resolver, this._CompiledStylesheet, this._QueryStore, this._RootAction, this.debugger);
			processor.Execute(output);
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0005666C File Offset: 0x0005566C
		public void Transform(XPathNavigator input, XsltArgumentList args, XmlWriter output)
		{
			this.Transform(input, args, output, this._DocumentResolver);
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x00056680 File Offset: 0x00055680
		public void Transform(XPathNavigator input, XsltArgumentList args, Stream output, XmlResolver resolver)
		{
			this.CheckCommand();
			Processor processor = new Processor(input, args, resolver, this._CompiledStylesheet, this._QueryStore, this._RootAction, this.debugger);
			processor.Execute(output);
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x000566BC File Offset: 0x000556BC
		public void Transform(XPathNavigator input, XsltArgumentList args, Stream output)
		{
			this.Transform(input, args, output, this._DocumentResolver);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x000566D0 File Offset: 0x000556D0
		public void Transform(XPathNavigator input, XsltArgumentList args, TextWriter output, XmlResolver resolver)
		{
			this.CheckCommand();
			Processor processor = new Processor(input, args, resolver, this._CompiledStylesheet, this._QueryStore, this._RootAction, this.debugger);
			processor.Execute(output);
		}

		// Token: 0x0600141F RID: 5151 RVA: 0x0005670C File Offset: 0x0005570C
		public void Transform(XPathNavigator input, XsltArgumentList args, TextWriter output)
		{
			this.CheckCommand();
			Processor processor = new Processor(input, args, this._DocumentResolver, this._CompiledStylesheet, this._QueryStore, this._RootAction, this.debugger);
			processor.Execute(output);
		}

		// Token: 0x06001420 RID: 5152 RVA: 0x0005674C File Offset: 0x0005574C
		public XmlReader Transform(IXPathNavigable input, XsltArgumentList args, XmlResolver resolver)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Transform(input.CreateNavigator(), args, resolver);
		}

		// Token: 0x06001421 RID: 5153 RVA: 0x0005676A File Offset: 0x0005576A
		public XmlReader Transform(IXPathNavigable input, XsltArgumentList args)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			return this.Transform(input.CreateNavigator(), args, this._DocumentResolver);
		}

		// Token: 0x06001422 RID: 5154 RVA: 0x0005678D File Offset: 0x0005578D
		public void Transform(IXPathNavigable input, XsltArgumentList args, TextWriter output, XmlResolver resolver)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			this.Transform(input.CreateNavigator(), args, output, resolver);
		}

		// Token: 0x06001423 RID: 5155 RVA: 0x000567AD File Offset: 0x000557AD
		public void Transform(IXPathNavigable input, XsltArgumentList args, TextWriter output)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			this.Transform(input.CreateNavigator(), args, output, this._DocumentResolver);
		}

		// Token: 0x06001424 RID: 5156 RVA: 0x000567D1 File Offset: 0x000557D1
		public void Transform(IXPathNavigable input, XsltArgumentList args, Stream output, XmlResolver resolver)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			this.Transform(input.CreateNavigator(), args, output, resolver);
		}

		// Token: 0x06001425 RID: 5157 RVA: 0x000567F1 File Offset: 0x000557F1
		public void Transform(IXPathNavigable input, XsltArgumentList args, Stream output)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			this.Transform(input.CreateNavigator(), args, output, this._DocumentResolver);
		}

		// Token: 0x06001426 RID: 5158 RVA: 0x00056815 File Offset: 0x00055815
		public void Transform(IXPathNavigable input, XsltArgumentList args, XmlWriter output, XmlResolver resolver)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			this.Transform(input.CreateNavigator(), args, output, resolver);
		}

		// Token: 0x06001427 RID: 5159 RVA: 0x00056835 File Offset: 0x00055835
		public void Transform(IXPathNavigable input, XsltArgumentList args, XmlWriter output)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			this.Transform(input.CreateNavigator(), args, output, this._DocumentResolver);
		}

		// Token: 0x06001428 RID: 5160 RVA: 0x0005685C File Offset: 0x0005585C
		public void Transform(string inputfile, string outputfile, XmlResolver resolver)
		{
			FileStream fileStream = null;
			try
			{
				XPathDocument xpathDocument = new XPathDocument(inputfile);
				fileStream = new FileStream(outputfile, FileMode.Create, FileAccess.ReadWrite);
				this.Transform(xpathDocument, null, fileStream, resolver);
			}
			finally
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}

		// Token: 0x06001429 RID: 5161 RVA: 0x000568A4 File Offset: 0x000558A4
		public void Transform(string inputfile, string outputfile)
		{
			this.Transform(inputfile, outputfile, this._DocumentResolver);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x000568B4 File Offset: 0x000558B4
		private void Compile(XPathNavigator stylesheet, XmlResolver resolver, Evidence evidence)
		{
			Compiler compiler = ((this.Debugger == null) ? new Compiler() : new DbgCompiler(this.Debugger));
			NavigatorInput navigatorInput = new NavigatorInput(stylesheet);
			compiler.Compile(navigatorInput, resolver, evidence);
			this._CompiledStylesheet = compiler.CompiledStylesheet;
			this._QueryStore = compiler.QueryStore;
			this._RootAction = compiler.RootAction;
		}

		// Token: 0x170004DA RID: 1242
		// (get) Token: 0x0600142B RID: 5163 RVA: 0x00056910 File Offset: 0x00055910
		internal IXsltDebugger Debugger
		{
			get
			{
				return this.debugger;
			}
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00056918 File Offset: 0x00055918
		internal XslTransform(object debugger)
		{
			if (debugger != null)
			{
				this.debugger = new XslTransform.DebuggerAddapter(debugger);
			}
		}

		// Token: 0x04000C43 RID: 3139
		private XmlResolver _documentResolver;

		// Token: 0x04000C44 RID: 3140
		private bool isDocumentResolverSet;

		// Token: 0x04000C45 RID: 3141
		private Stylesheet _CompiledStylesheet;

		// Token: 0x04000C46 RID: 3142
		private List<TheQuery> _QueryStore;

		// Token: 0x04000C47 RID: 3143
		private RootAction _RootAction;

		// Token: 0x04000C48 RID: 3144
		private IXsltDebugger debugger;

		// Token: 0x0200017A RID: 378
		private class DebuggerAddapter : IXsltDebugger
		{
			// Token: 0x0600142D RID: 5165 RVA: 0x00056930 File Offset: 0x00055930
			public DebuggerAddapter(object unknownDebugger)
			{
				this.unknownDebugger = unknownDebugger;
				BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
				Type type = unknownDebugger.GetType();
				this.getBltIn = type.GetMethod("GetBuiltInTemplatesUri", bindingFlags);
				this.onCompile = type.GetMethod("OnInstructionCompile", bindingFlags);
				this.onExecute = type.GetMethod("OnInstructionExecute", bindingFlags);
			}

			// Token: 0x0600142E RID: 5166 RVA: 0x0005698A File Offset: 0x0005598A
			public string GetBuiltInTemplatesUri()
			{
				if (this.getBltIn == null)
				{
					return null;
				}
				return (string)this.getBltIn.Invoke(this.unknownDebugger, new object[0]);
			}

			// Token: 0x0600142F RID: 5167 RVA: 0x000569B4 File Offset: 0x000559B4
			public void OnInstructionCompile(XPathNavigator styleSheetNavigator)
			{
				if (this.onCompile != null)
				{
					this.onCompile.Invoke(this.unknownDebugger, new object[] { styleSheetNavigator });
				}
			}

			// Token: 0x06001430 RID: 5168 RVA: 0x000569E8 File Offset: 0x000559E8
			public void OnInstructionExecute(IXsltProcessor xsltProcessor)
			{
				if (this.onExecute != null)
				{
					this.onExecute.Invoke(this.unknownDebugger, new object[] { xsltProcessor });
				}
			}

			// Token: 0x04000C49 RID: 3145
			private object unknownDebugger;

			// Token: 0x04000C4A RID: 3146
			private MethodInfo getBltIn;

			// Token: 0x04000C4B RID: 3147
			private MethodInfo onCompile;

			// Token: 0x04000C4C RID: 3148
			private MethodInfo onExecute;
		}
	}
}
