using System;

namespace System.Web.UI.Design
{
	[Obsolete("The recommended alternative is System.Web.UI.Design.IControlDesignerTag and System.Web.UI.Design.IControlDesignerView. http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface IControlDesignerBehavior
	{
		object DesignTimeElementView { get; }

		string DesignTimeHtml { get; set; }

		void OnTemplateModeChanged();
	}
}
