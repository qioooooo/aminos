using System;

namespace System.Web.UI.Design
{
	public class TemplateModeChangedEventArgs : EventArgs
	{
		public TemplateModeChangedEventArgs(TemplateGroup newTemplateGroup)
		{
			this._newTemplateGroup = newTemplateGroup;
		}

		public TemplateGroup NewTemplateGroup
		{
			get
			{
				return this._newTemplateGroup;
			}
		}

		private TemplateGroup _newTemplateGroup;
	}
}
