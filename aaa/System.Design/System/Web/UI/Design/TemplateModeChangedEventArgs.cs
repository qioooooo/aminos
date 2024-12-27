using System;

namespace System.Web.UI.Design
{
	// Token: 0x0200039C RID: 924
	public class TemplateModeChangedEventArgs : EventArgs
	{
		// Token: 0x06002241 RID: 8769 RVA: 0x000BB9F1 File Offset: 0x000BA9F1
		public TemplateModeChangedEventArgs(TemplateGroup newTemplateGroup)
		{
			this._newTemplateGroup = newTemplateGroup;
		}

		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06002242 RID: 8770 RVA: 0x000BBA00 File Offset: 0x000BAA00
		public TemplateGroup NewTemplateGroup
		{
			get
			{
				return this._newTemplateGroup;
			}
		}

		// Token: 0x04001848 RID: 6216
		private TemplateGroup _newTemplateGroup;
	}
}
