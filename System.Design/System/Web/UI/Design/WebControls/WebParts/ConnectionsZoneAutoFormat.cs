using System;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	internal sealed class ConnectionsZoneAutoFormat : BaseAutoFormat
	{
		public ConnectionsZoneAutoFormat(DataRow schemeData)
			: base(schemeData)
		{
			base.Style.Width = 225;
		}

		public override Control GetPreviewControl(Control runtimeControl)
		{
			ConnectionsZone connectionsZone = (ConnectionsZone)base.GetPreviewControl(runtimeControl);
			connectionsZone.ID = "AutoFormatPreviewControl";
			return connectionsZone;
		}

		internal const string PreviewControlID = "AutoFormatPreviewControl";
	}
}
