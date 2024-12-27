using System;
using System.Collections;
using System.Text;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x020001AF RID: 431
	internal class XsltOutput : CompiledAction
	{
		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00055BE9 File Offset: 0x00054BE9
		internal XsltOutput.OutputMethod Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x00055BF1 File Offset: 0x00054BF1
		internal bool OmitXmlDeclaration
		{
			get
			{
				return this.omitXmlDecl;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00055BF9 File Offset: 0x00054BF9
		internal bool HasStandalone
		{
			get
			{
				return this.standaloneSId != int.MaxValue;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x00055C0B File Offset: 0x00054C0B
		internal bool Standalone
		{
			get
			{
				return this.standalone;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x00055C13 File Offset: 0x00054C13
		internal string DoctypePublic
		{
			get
			{
				return this.doctypePublic;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x00055C1B File Offset: 0x00054C1B
		internal string DoctypeSystem
		{
			get
			{
				return this.doctypeSystem;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x00055C23 File Offset: 0x00054C23
		internal Hashtable CDataElements
		{
			get
			{
				return this.cdataElements;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x00055C2B File Offset: 0x00054C2B
		internal bool Indent
		{
			get
			{
				return this.indent;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x00055C33 File Offset: 0x00054C33
		internal Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x00055C3B File Offset: 0x00054C3B
		internal string MediaType
		{
			get
			{
				return this.mediaType;
			}
		}

		// Token: 0x060011D8 RID: 4568 RVA: 0x00055C44 File Offset: 0x00054C44
		internal XsltOutput CreateDerivedOutput(XsltOutput.OutputMethod method)
		{
			XsltOutput xsltOutput = (XsltOutput)base.MemberwiseClone();
			xsltOutput.method = method;
			if (method == XsltOutput.OutputMethod.Html && this.indentSId == 2147483647)
			{
				xsltOutput.indent = true;
			}
			return xsltOutput;
		}

		// Token: 0x060011D9 RID: 4569 RVA: 0x00055C7D File Offset: 0x00054C7D
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckEmpty(compiler);
		}

		// Token: 0x060011DA RID: 4570 RVA: 0x00055C90 File Offset: 0x00054C90
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Method))
			{
				if (compiler.Stylesheetid <= this.methodSId)
				{
					this.method = XsltOutput.ParseOutputMethod(value, compiler);
					this.methodSId = compiler.Stylesheetid;
					if (this.indentSId == 2147483647)
					{
						this.indent = this.method == XsltOutput.OutputMethod.Html;
					}
				}
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Version))
			{
				if (compiler.Stylesheetid <= this.versionSId)
				{
					this.version = value;
					this.versionSId = compiler.Stylesheetid;
				}
			}
			else
			{
				if (Keywords.Equals(localName, compiler.Atoms.Encoding))
				{
					if (compiler.Stylesheetid > this.encodingSId)
					{
						return true;
					}
					try
					{
						this.encoding = Encoding.GetEncoding(value);
						this.encodingSId = compiler.Stylesheetid;
						return true;
					}
					catch (NotSupportedException)
					{
						return true;
					}
					catch (ArgumentException)
					{
						return true;
					}
				}
				if (Keywords.Equals(localName, compiler.Atoms.OmitXmlDeclaration))
				{
					if (compiler.Stylesheetid <= this.omitXmlDeclSId)
					{
						this.omitXmlDecl = compiler.GetYesNo(value);
						this.omitXmlDeclSId = compiler.Stylesheetid;
					}
				}
				else if (Keywords.Equals(localName, compiler.Atoms.Standalone))
				{
					if (compiler.Stylesheetid <= this.standaloneSId)
					{
						this.standalone = compiler.GetYesNo(value);
						this.standaloneSId = compiler.Stylesheetid;
					}
				}
				else if (Keywords.Equals(localName, compiler.Atoms.DoctypePublic))
				{
					if (compiler.Stylesheetid <= this.doctypePublicSId)
					{
						this.doctypePublic = value;
						this.doctypePublicSId = compiler.Stylesheetid;
					}
				}
				else if (Keywords.Equals(localName, compiler.Atoms.DoctypeSystem))
				{
					if (compiler.Stylesheetid <= this.doctypeSystemSId)
					{
						this.doctypeSystem = value;
						this.doctypeSystemSId = compiler.Stylesheetid;
					}
				}
				else if (Keywords.Equals(localName, compiler.Atoms.Indent))
				{
					if (compiler.Stylesheetid <= this.indentSId)
					{
						this.indent = compiler.GetYesNo(value);
						this.indentSId = compiler.Stylesheetid;
					}
				}
				else if (Keywords.Equals(localName, compiler.Atoms.MediaType))
				{
					if (compiler.Stylesheetid <= this.mediaTypeSId)
					{
						this.mediaType = value;
						this.mediaTypeSId = compiler.Stylesheetid;
					}
				}
				else
				{
					if (!Keywords.Equals(localName, compiler.Atoms.CdataSectionElements))
					{
						return false;
					}
					string[] array = XmlConvert.SplitString(value);
					if (this.cdataElements == null)
					{
						this.cdataElements = new Hashtable(array.Length);
					}
					for (int i = 0; i < array.Length; i++)
					{
						XmlQualifiedName xmlQualifiedName = compiler.CreateXmlQName(array[i]);
						this.cdataElements[xmlQualifiedName] = xmlQualifiedName;
					}
				}
			}
			return true;
		}

		// Token: 0x060011DB RID: 4571 RVA: 0x00055F8C File Offset: 0x00054F8C
		internal override void Execute(Processor processor, ActionFrame frame)
		{
		}

		// Token: 0x060011DC RID: 4572 RVA: 0x00055F90 File Offset: 0x00054F90
		private static XsltOutput.OutputMethod ParseOutputMethod(string value, Compiler compiler)
		{
			XmlQualifiedName xmlQualifiedName = compiler.CreateXPathQName(value);
			if (xmlQualifiedName.Namespace.Length != 0)
			{
				return XsltOutput.OutputMethod.Other;
			}
			string name;
			if ((name = xmlQualifiedName.Name) != null)
			{
				if (name == "xml")
				{
					return XsltOutput.OutputMethod.Xml;
				}
				if (name == "html")
				{
					return XsltOutput.OutputMethod.Html;
				}
				if (name == "text")
				{
					return XsltOutput.OutputMethod.Text;
				}
			}
			if (compiler.ForwardCompatibility)
			{
				return XsltOutput.OutputMethod.Unknown;
			}
			throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "method", value });
		}

		// Token: 0x04000BE6 RID: 3046
		private XsltOutput.OutputMethod method = XsltOutput.OutputMethod.Unknown;

		// Token: 0x04000BE7 RID: 3047
		private int methodSId = int.MaxValue;

		// Token: 0x04000BE8 RID: 3048
		private Encoding encoding = Encoding.UTF8;

		// Token: 0x04000BE9 RID: 3049
		private int encodingSId = int.MaxValue;

		// Token: 0x04000BEA RID: 3050
		private string version;

		// Token: 0x04000BEB RID: 3051
		private int versionSId = int.MaxValue;

		// Token: 0x04000BEC RID: 3052
		private bool omitXmlDecl;

		// Token: 0x04000BED RID: 3053
		private int omitXmlDeclSId = int.MaxValue;

		// Token: 0x04000BEE RID: 3054
		private bool standalone;

		// Token: 0x04000BEF RID: 3055
		private int standaloneSId = int.MaxValue;

		// Token: 0x04000BF0 RID: 3056
		private string doctypePublic;

		// Token: 0x04000BF1 RID: 3057
		private int doctypePublicSId = int.MaxValue;

		// Token: 0x04000BF2 RID: 3058
		private string doctypeSystem;

		// Token: 0x04000BF3 RID: 3059
		private int doctypeSystemSId = int.MaxValue;

		// Token: 0x04000BF4 RID: 3060
		private bool indent;

		// Token: 0x04000BF5 RID: 3061
		private int indentSId = int.MaxValue;

		// Token: 0x04000BF6 RID: 3062
		private string mediaType = "text/html";

		// Token: 0x04000BF7 RID: 3063
		private int mediaTypeSId = int.MaxValue;

		// Token: 0x04000BF8 RID: 3064
		private Hashtable cdataElements;

		// Token: 0x020001B0 RID: 432
		internal enum OutputMethod
		{
			// Token: 0x04000BFA RID: 3066
			Xml,
			// Token: 0x04000BFB RID: 3067
			Html,
			// Token: 0x04000BFC RID: 3068
			Text,
			// Token: 0x04000BFD RID: 3069
			Other,
			// Token: 0x04000BFE RID: 3070
			Unknown
		}
	}
}
