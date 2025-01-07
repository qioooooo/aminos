using System;
using System.Collections;

namespace System.Web.UI.Design.WebControls
{
	public class BulletedListDesigner : ListControlDesigner
	{
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		protected override void PostFilterEvents(IDictionary events)
		{
			base.PostFilterEvents(events);
			events.Remove("SelectedIndexChanged");
		}
	}
}
