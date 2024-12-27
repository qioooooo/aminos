using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x020000F2 RID: 242
	internal static class CompModSwitches
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060009FE RID: 2558 RVA: 0x000260B9 File Offset: 0x000250B9
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

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x000260DB File Offset: 0x000250DB
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

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x000260FD File Offset: 0x000250FD
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

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0002611F File Offset: 0x0002511F
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

		// Token: 0x04000D4D RID: 3405
		private static BooleanSwitch commonDesignerServices;

		// Token: 0x04000D4E RID: 3406
		private static TraceSwitch userControlDesigner;

		// Token: 0x04000D4F RID: 3407
		private static TraceSwitch dragDrop;

		// Token: 0x04000D50 RID: 3408
		private static TraceSwitch msaa;
	}
}
