using System;
using System.Globalization;
using System.Xml.XPath;

namespace System.Xml.Xsl.XsltOld
{
	// Token: 0x02000168 RID: 360
	internal class SortAction : CompiledAction
	{
		// Token: 0x06000F1B RID: 3867 RVA: 0x0004C308 File Offset: 0x0004B308
		private string ParseLang(string value)
		{
			if (value == null)
			{
				return null;
			}
			if (XmlComplianceUtil.IsValidLanguageID(value.ToCharArray(), 0, value.Length) || (value.Length != 0 && CultureInfo.GetCultureInfo(value) != null))
			{
				return value;
			}
			if (this.forwardCompatibility)
			{
				return null;
			}
			throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "lang", value });
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x0004C368 File Offset: 0x0004B368
		private XmlDataType ParseDataType(string value, InputScopeManager manager)
		{
			if (value == null)
			{
				return XmlDataType.Text;
			}
			if (value == "text")
			{
				return XmlDataType.Text;
			}
			if (value == "number")
			{
				return XmlDataType.Number;
			}
			string text;
			string text2;
			PrefixQName.ParseQualifiedName(value, out text, out text2);
			manager.ResolveXmlNamespace(text);
			if (text.Length == 0 && !this.forwardCompatibility)
			{
				throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "data-type", value });
			}
			return XmlDataType.Text;
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x0004C3DC File Offset: 0x0004B3DC
		private XmlSortOrder ParseOrder(string value)
		{
			if (value == null)
			{
				return XmlSortOrder.Ascending;
			}
			if (value == "ascending")
			{
				return XmlSortOrder.Ascending;
			}
			if (value == "descending")
			{
				return XmlSortOrder.Descending;
			}
			if (this.forwardCompatibility)
			{
				return XmlSortOrder.Ascending;
			}
			throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "order", value });
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x0004C434 File Offset: 0x0004B434
		private XmlCaseOrder ParseCaseOrder(string value)
		{
			if (value == null)
			{
				return XmlCaseOrder.None;
			}
			if (value == "upper-first")
			{
				return XmlCaseOrder.UpperFirst;
			}
			if (value == "lower-first")
			{
				return XmlCaseOrder.LowerFirst;
			}
			if (this.forwardCompatibility)
			{
				return XmlCaseOrder.None;
			}
			throw XsltException.Create("Xslt_InvalidAttrValue", new string[] { "case-order", value });
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x0004C48C File Offset: 0x0004B48C
		internal override void Compile(Compiler compiler)
		{
			base.CompileAttributes(compiler);
			base.CheckEmpty(compiler);
			if (this.selectKey == -1)
			{
				this.selectKey = compiler.AddQuery(".");
			}
			this.forwardCompatibility = compiler.ForwardCompatibility;
			this.manager = compiler.CloneScopeManager();
			this.lang = this.ParseLang(CompiledAction.PrecalculateAvt(ref this.langAvt));
			this.dataType = this.ParseDataType(CompiledAction.PrecalculateAvt(ref this.dataTypeAvt), this.manager);
			this.order = this.ParseOrder(CompiledAction.PrecalculateAvt(ref this.orderAvt));
			this.caseOrder = this.ParseCaseOrder(CompiledAction.PrecalculateAvt(ref this.caseOrderAvt));
			if (this.langAvt == null && this.dataTypeAvt == null && this.orderAvt == null && this.caseOrderAvt == null)
			{
				this.sort = new Sort(this.selectKey, this.lang, this.dataType, this.order, this.caseOrder);
			}
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x0004C584 File Offset: 0x0004B584
		internal override bool CompileAttribute(Compiler compiler)
		{
			string localName = compiler.Input.LocalName;
			string value = compiler.Input.Value;
			if (Keywords.Equals(localName, compiler.Atoms.Select))
			{
				this.selectKey = compiler.AddQuery(value);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Lang))
			{
				this.langAvt = Avt.CompileAvt(compiler, value);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.DataType))
			{
				this.dataTypeAvt = Avt.CompileAvt(compiler, value);
			}
			else if (Keywords.Equals(localName, compiler.Atoms.Order))
			{
				this.orderAvt = Avt.CompileAvt(compiler, value);
			}
			else
			{
				if (!Keywords.Equals(localName, compiler.Atoms.CaseOrder))
				{
					return false;
				}
				this.caseOrderAvt = Avt.CompileAvt(compiler, value);
			}
			return true;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x0004C65C File Offset: 0x0004B65C
		internal override void Execute(Processor processor, ActionFrame frame)
		{
			processor.AddSort((this.sort != null) ? this.sort : new Sort(this.selectKey, (this.langAvt == null) ? this.lang : this.ParseLang(this.langAvt.Evaluate(processor, frame)), (this.dataTypeAvt == null) ? this.dataType : this.ParseDataType(this.dataTypeAvt.Evaluate(processor, frame), this.manager), (this.orderAvt == null) ? this.order : this.ParseOrder(this.orderAvt.Evaluate(processor, frame)), (this.caseOrderAvt == null) ? this.caseOrder : this.ParseCaseOrder(this.caseOrderAvt.Evaluate(processor, frame))));
			frame.Finished();
		}

		// Token: 0x040009C7 RID: 2503
		private int selectKey = -1;

		// Token: 0x040009C8 RID: 2504
		private Avt langAvt;

		// Token: 0x040009C9 RID: 2505
		private Avt dataTypeAvt;

		// Token: 0x040009CA RID: 2506
		private Avt orderAvt;

		// Token: 0x040009CB RID: 2507
		private Avt caseOrderAvt;

		// Token: 0x040009CC RID: 2508
		private string lang;

		// Token: 0x040009CD RID: 2509
		private XmlDataType dataType = XmlDataType.Text;

		// Token: 0x040009CE RID: 2510
		private XmlSortOrder order = XmlSortOrder.Ascending;

		// Token: 0x040009CF RID: 2511
		private XmlCaseOrder caseOrder;

		// Token: 0x040009D0 RID: 2512
		private Sort sort;

		// Token: 0x040009D1 RID: 2513
		private bool forwardCompatibility;

		// Token: 0x040009D2 RID: 2514
		private InputScopeManager manager;
	}
}
