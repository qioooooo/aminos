using System;
using System.Security.Permissions;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.Devices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class Mouse
	{
		public bool ButtonsSwapped
		{
			get
			{
				if (SystemInformation.MousePresent)
				{
					return SystemInformation.MouseButtonsSwapped;
				}
				throw ExceptionUtils.GetInvalidOperationException("Mouse_NoMouseIsPresent", new string[0]);
			}
		}

		public bool WheelExists
		{
			get
			{
				if (SystemInformation.MousePresent)
				{
					return SystemInformation.MouseWheelPresent;
				}
				throw ExceptionUtils.GetInvalidOperationException("Mouse_NoMouseIsPresent", new string[0]);
			}
		}

		public int WheelScrollLines
		{
			get
			{
				if (this.WheelExists)
				{
					return SystemInformation.MouseWheelScrollLines;
				}
				throw ExceptionUtils.GetInvalidOperationException("Mouse_NoWheelIsPresent", new string[0]);
			}
		}
	}
}
