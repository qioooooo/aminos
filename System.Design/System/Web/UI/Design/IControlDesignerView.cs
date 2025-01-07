using System;
using System.ComponentModel.Design;
using System.Drawing;

namespace System.Web.UI.Design
{
	public interface IControlDesignerView
	{
		DesignerRegion ContainingRegion { get; }

		IDesigner NamingContainerDesigner { get; }

		bool SupportsRegions { get; }

		event ViewEventHandler ViewEvent;

		Rectangle GetBounds(DesignerRegion region);

		void Invalidate(Rectangle rectangle);

		void SetFlags(ViewFlags viewFlags, bool setFlag);

		void SetRegionContent(EditableDesignerRegion region, string content);

		void Update();
	}
}
