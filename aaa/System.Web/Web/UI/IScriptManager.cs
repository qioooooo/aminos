using System;

namespace System.Web.UI
{
	// Token: 0x02000414 RID: 1044
	internal interface IScriptManager
	{
		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x060032B4 RID: 12980
		bool SupportsPartialRendering { get; }

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x060032B5 RID: 12981
		bool IsInAsyncPostBack { get; }

		// Token: 0x060032B6 RID: 12982
		void RegisterArrayDeclaration(Control control, string arrayName, string arrayValue);

		// Token: 0x060032B7 RID: 12983
		void RegisterClientScriptBlock(Control control, Type type, string key, string script, bool addScriptTags);

		// Token: 0x060032B8 RID: 12984
		void RegisterClientScriptInclude(Control control, Type type, string key, string url);

		// Token: 0x060032B9 RID: 12985
		void RegisterClientScriptResource(Control control, Type type, string resourceName);

		// Token: 0x060032BA RID: 12986
		void RegisterDispose(Control control, string disposeScript);

		// Token: 0x060032BB RID: 12987
		void RegisterExpandoAttribute(Control control, string controlId, string attributeName, string attributeValue, bool encode);

		// Token: 0x060032BC RID: 12988
		void RegisterHiddenField(Control control, string hiddenFieldName, string hiddenFieldValue);

		// Token: 0x060032BD RID: 12989
		void RegisterOnSubmitStatement(Control control, Type type, string key, string script);

		// Token: 0x060032BE RID: 12990
		void RegisterPostBackControl(Control control);

		// Token: 0x060032BF RID: 12991
		void RegisterStartupScript(Control control, Type type, string key, string script, bool addScriptTags);

		// Token: 0x060032C0 RID: 12992
		void SetFocusInternal(string clientID);
	}
}
