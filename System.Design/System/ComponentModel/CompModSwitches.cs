using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	internal static class CompModSwitches
	{
		public static BooleanSwitch CommonDesignerServices
		{
			get
			{
				if (CompModSwitches.commonDesignerServices == null)
				{
					CompModSwitches.commonDesignerServices = new BooleanSwitch("CommonDesignerServices", "Assert if any common designer service is not found.");
				}
				return CompModSwitches.commonDesignerServices;
			}
		}

		public static TraceSwitch DragDrop
		{
			get
			{
				if (CompModSwitches.dragDrop == null)
				{
					CompModSwitches.dragDrop = new TraceSwitch("DragDrop", "Debug OLEDragDrop support in Controls");
				}
				return CompModSwitches.dragDrop;
			}
		}

		public static TraceSwitch MSAA
		{
			get
			{
				if (CompModSwitches.msaa == null)
				{
					CompModSwitches.msaa = new TraceSwitch("MSAA", "Debug Microsoft Active Accessibility");
				}
				return CompModSwitches.msaa;
			}
		}

		public static TraceSwitch UserControlDesigner
		{
			get
			{
				if (CompModSwitches.userControlDesigner == null)
				{
					CompModSwitches.userControlDesigner = new TraceSwitch("UserControlDesigner", "User Control Designer : Trace service calls.");
				}
				return CompModSwitches.userControlDesigner;
			}
		}

		private static BooleanSwitch commonDesignerServices;

		private static TraceSwitch userControlDesigner;

		private static TraceSwitch dragDrop;

		private static TraceSwitch msaa;
	}
}
