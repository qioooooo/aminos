using System;
using System.CodeDom;
using System.Collections;
using System.Globalization;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x0200017E RID: 382
	internal class MasterPageCodeDomTreeGenerator : TemplateControlCodeDomTreeGenerator
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x0004996C File Offset: 0x0004896C
		private MasterPageParser Parser
		{
			get
			{
				return this._masterPageParser;
			}
		}

		// Token: 0x0600109F RID: 4255 RVA: 0x00049974 File Offset: 0x00048974
		internal MasterPageCodeDomTreeGenerator(MasterPageParser parser)
			: base(parser)
		{
			this._masterPageParser = parser;
		}

		// Token: 0x060010A0 RID: 4256 RVA: 0x00049984 File Offset: 0x00048984
		protected override void BuildDefaultConstructor()
		{
			base.BuildDefaultConstructor();
			foreach (object obj in ((IEnumerable)this.Parser.PlaceHolderList))
			{
				string text = (string)obj;
				this.BuildAddContentPlaceHolderNames(this._ctor, text);
			}
		}

		// Token: 0x060010A1 RID: 4257 RVA: 0x000499F0 File Offset: 0x000489F0
		private void BuildAddContentPlaceHolderNames(CodeMemberMethod method, string placeHolderID)
		{
			CodePropertyReferenceExpression codePropertyReferenceExpression = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "ContentPlaceHolders");
			CodeExpressionStatement codeExpressionStatement = new CodeExpressionStatement();
			codeExpressionStatement.Expression = new CodeMethodInvokeExpression(codePropertyReferenceExpression, "Add", new CodeExpression[]
			{
				new CodePrimitiveExpression(placeHolderID.ToLower(CultureInfo.InvariantCulture))
			});
			method.Statements.Add(codeExpressionStatement);
		}

		// Token: 0x060010A2 RID: 4258 RVA: 0x00049A4C File Offset: 0x00048A4C
		protected override void BuildMiscClassMembers()
		{
			base.BuildMiscClassMembers();
			if (this.Parser.MasterPageType != null)
			{
				base.BuildStronglyTypedProperty("Master", this.Parser.MasterPageType);
			}
		}

		// Token: 0x0400165F RID: 5727
		private const string _masterPropertyName = "Master";

		// Token: 0x04001660 RID: 5728
		protected MasterPageParser _masterPageParser;
	}
}
