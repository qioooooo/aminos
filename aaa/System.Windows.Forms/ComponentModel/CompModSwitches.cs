using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x02000039 RID: 57
	internal static class CompModSwitches
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00006218 File Offset: 0x00005218
		public static TraceSwitch ActiveX
		{
			get
			{
				if (CompModSwitches.activeX == null)
				{
					CompModSwitches.activeX = new TraceSwitch("ActiveX", "Debug ActiveX sourcing");
				}
				return CompModSwitches.activeX;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000623A File Offset: 0x0000523A
		public static TraceSwitch DataCursor
		{
			get
			{
				if (CompModSwitches.dataCursor == null)
				{
					CompModSwitches.dataCursor = new TraceSwitch("Microsoft.WFC.Data.DataCursor", "DataCursor");
				}
				return CompModSwitches.dataCursor;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000625C File Offset: 0x0000525C
		public static TraceSwitch DataGridCursor
		{
			get
			{
				if (CompModSwitches.dataGridCursor == null)
				{
					CompModSwitches.dataGridCursor = new TraceSwitch("DataGridCursor", "DataGrid cursor tracing");
				}
				return CompModSwitches.dataGridCursor;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000627E File Offset: 0x0000527E
		public static TraceSwitch DataGridEditing
		{
			get
			{
				if (CompModSwitches.dataGridEditing == null)
				{
					CompModSwitches.dataGridEditing = new TraceSwitch("DataGridEditing", "DataGrid edit related tracing");
				}
				return CompModSwitches.dataGridEditing;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000182 RID: 386 RVA: 0x000062A0 File Offset: 0x000052A0
		public static TraceSwitch DataGridKeys
		{
			get
			{
				if (CompModSwitches.dataGridKeys == null)
				{
					CompModSwitches.dataGridKeys = new TraceSwitch("DataGridKeys", "DataGrid keystroke management tracing");
				}
				return CompModSwitches.dataGridKeys;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000183 RID: 387 RVA: 0x000062C2 File Offset: 0x000052C2
		public static TraceSwitch DataGridLayout
		{
			get
			{
				if (CompModSwitches.dataGridLayout == null)
				{
					CompModSwitches.dataGridLayout = new TraceSwitch("DataGridLayout", "DataGrid layout tracing");
				}
				return CompModSwitches.dataGridLayout;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000184 RID: 388 RVA: 0x000062E4 File Offset: 0x000052E4
		public static TraceSwitch DataGridPainting
		{
			get
			{
				if (CompModSwitches.dataGridPainting == null)
				{
					CompModSwitches.dataGridPainting = new TraceSwitch("DataGridPainting", "DataGrid Painting related tracing");
				}
				return CompModSwitches.dataGridPainting;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00006306 File Offset: 0x00005306
		public static TraceSwitch DataGridParents
		{
			get
			{
				if (CompModSwitches.dataGridParents == null)
				{
					CompModSwitches.dataGridParents = new TraceSwitch("DataGridParents", "DataGrid parent rows");
				}
				return CompModSwitches.dataGridParents;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00006328 File Offset: 0x00005328
		public static TraceSwitch DataGridScrolling
		{
			get
			{
				if (CompModSwitches.dataGridScrolling == null)
				{
					CompModSwitches.dataGridScrolling = new TraceSwitch("DataGridScrolling", "DataGrid scrolling");
				}
				return CompModSwitches.dataGridScrolling;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000634A File Offset: 0x0000534A
		public static TraceSwitch DataGridSelection
		{
			get
			{
				if (CompModSwitches.dataGridSelection == null)
				{
					CompModSwitches.dataGridSelection = new TraceSwitch("DataGridSelection", "DataGrid selection management tracing");
				}
				return CompModSwitches.dataGridSelection;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000636C File Offset: 0x0000536C
		public static TraceSwitch DataObject
		{
			get
			{
				if (CompModSwitches.dataObject == null)
				{
					CompModSwitches.dataObject = new TraceSwitch("DataObject", "Enable tracing for the DataObject class.");
				}
				return CompModSwitches.dataObject;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000638E File Offset: 0x0000538E
		public static TraceSwitch DataView
		{
			get
			{
				if (CompModSwitches.dataView == null)
				{
					CompModSwitches.dataView = new TraceSwitch("DataView", "DataView");
				}
				return CompModSwitches.dataView;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000063B0 File Offset: 0x000053B0
		public static TraceSwitch DebugGridView
		{
			get
			{
				if (CompModSwitches.debugGridView == null)
				{
					CompModSwitches.debugGridView = new TraceSwitch("PSDEBUGGRIDVIEW", "Debug PropertyGridView");
				}
				return CompModSwitches.debugGridView;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000063D2 File Offset: 0x000053D2
		public static TraceSwitch DGCaptionPaint
		{
			get
			{
				if (CompModSwitches.dgCaptionPaint == null)
				{
					CompModSwitches.dgCaptionPaint = new TraceSwitch("DGCaptionPaint", "DataGridCaption");
				}
				return CompModSwitches.dgCaptionPaint;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000063F4 File Offset: 0x000053F4
		public static TraceSwitch DGEditColumnEditing
		{
			get
			{
				if (CompModSwitches.dgEditColumnEditing == null)
				{
					CompModSwitches.dgEditColumnEditing = new TraceSwitch("DGEditColumnEditing", "Editing related tracing");
				}
				return CompModSwitches.dgEditColumnEditing;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00006416 File Offset: 0x00005416
		public static TraceSwitch DGRelationShpRowLayout
		{
			get
			{
				if (CompModSwitches.dgRelationShpRowLayout == null)
				{
					CompModSwitches.dgRelationShpRowLayout = new TraceSwitch("DGRelationShpRowLayout", "Relationship row layout");
				}
				return CompModSwitches.dgRelationShpRowLayout;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00006438 File Offset: 0x00005438
		public static TraceSwitch DGRelationShpRowPaint
		{
			get
			{
				if (CompModSwitches.dgRelationShpRowPaint == null)
				{
					CompModSwitches.dgRelationShpRowPaint = new TraceSwitch("DGRelationShpRowPaint", "Relationship row painting");
				}
				return CompModSwitches.dgRelationShpRowPaint;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600018F RID: 399 RVA: 0x0000645A File Offset: 0x0000545A
		public static TraceSwitch DGRowPaint
		{
			get
			{
				if (CompModSwitches.dgRowPaint == null)
				{
					CompModSwitches.dgRowPaint = new TraceSwitch("DGRowPaint", "DataGrid Simple Row painting stuff");
				}
				return CompModSwitches.dgRowPaint;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000190 RID: 400 RVA: 0x0000647C File Offset: 0x0000547C
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000191 RID: 401 RVA: 0x0000649E File Offset: 0x0000549E
		public static TraceSwitch FlowLayout
		{
			get
			{
				if (CompModSwitches.flowLayout == null)
				{
					CompModSwitches.flowLayout = new TraceSwitch("FlowLayout", "Debug flow layout");
				}
				return CompModSwitches.flowLayout;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000064C0 File Offset: 0x000054C0
		public static TraceSwitch ImeMode
		{
			get
			{
				if (CompModSwitches.imeMode == null)
				{
					CompModSwitches.imeMode = new TraceSwitch("ImeMode", "Debug IME Mode");
				}
				return CompModSwitches.imeMode;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000193 RID: 403 RVA: 0x000064E2 File Offset: 0x000054E2
		public static TraceSwitch LayoutPerformance
		{
			get
			{
				if (CompModSwitches.layoutPerformance == null)
				{
					CompModSwitches.layoutPerformance = new TraceSwitch("LayoutPerformance", "Tracks layout events which impact performance.");
				}
				return CompModSwitches.layoutPerformance;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00006504 File Offset: 0x00005504
		public static TraceSwitch LayoutSuspendResume
		{
			get
			{
				if (CompModSwitches.layoutSuspendResume == null)
				{
					CompModSwitches.layoutSuspendResume = new TraceSwitch("LayoutSuspendResume", "Tracks SuspendLayout/ResumeLayout.");
				}
				return CompModSwitches.layoutSuspendResume;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00006526 File Offset: 0x00005526
		public static BooleanSwitch LifetimeTracing
		{
			get
			{
				if (CompModSwitches.lifetimeTracing == null)
				{
					CompModSwitches.lifetimeTracing = new BooleanSwitch("LifetimeTracing", "Track lifetime events. This will cause objects to track the stack at creation and dispose.");
				}
				return CompModSwitches.lifetimeTracing;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00006548 File Offset: 0x00005548
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000656A File Offset: 0x0000556A
		public static TraceSwitch MSOComponentManager
		{
			get
			{
				if (CompModSwitches.msoComponentManager == null)
				{
					CompModSwitches.msoComponentManager = new TraceSwitch("MSOComponentManager", "Debug MSO Component Manager support");
				}
				return CompModSwitches.msoComponentManager;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000658C File Offset: 0x0000558C
		public static TraceSwitch RichLayout
		{
			get
			{
				if (CompModSwitches.richLayout == null)
				{
					CompModSwitches.richLayout = new TraceSwitch("RichLayout", "Debug layout in RichControls");
				}
				return CompModSwitches.richLayout;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000199 RID: 409 RVA: 0x000065AE File Offset: 0x000055AE
		public static TraceSwitch SetBounds
		{
			get
			{
				if (CompModSwitches.setBounds == null)
				{
					CompModSwitches.setBounds = new TraceSwitch("SetBounds", "Trace changes to control size/position.");
				}
				return CompModSwitches.setBounds;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000065D0 File Offset: 0x000055D0
		public static TraceSwitch HandleLeak
		{
			get
			{
				if (CompModSwitches.handleLeak == null)
				{
					CompModSwitches.handleLeak = new TraceSwitch("HANDLELEAK", "HandleCollector: Track Win32 Handle Leaks");
				}
				return CompModSwitches.handleLeak;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000065F2 File Offset: 0x000055F2
		public static BooleanSwitch TraceCollect
		{
			get
			{
				if (CompModSwitches.traceCollect == null)
				{
					CompModSwitches.traceCollect = new BooleanSwitch("TRACECOLLECT", "HandleCollector: Trace HandleCollector operations");
				}
				return CompModSwitches.traceCollect;
			}
		}

		// Token: 0x04000B41 RID: 2881
		private static TraceSwitch activeX;

		// Token: 0x04000B42 RID: 2882
		private static TraceSwitch flowLayout;

		// Token: 0x04000B43 RID: 2883
		private static TraceSwitch dataCursor;

		// Token: 0x04000B44 RID: 2884
		private static TraceSwitch dataGridCursor;

		// Token: 0x04000B45 RID: 2885
		private static TraceSwitch dataGridEditing;

		// Token: 0x04000B46 RID: 2886
		private static TraceSwitch dataGridKeys;

		// Token: 0x04000B47 RID: 2887
		private static TraceSwitch dataGridLayout;

		// Token: 0x04000B48 RID: 2888
		private static TraceSwitch dataGridPainting;

		// Token: 0x04000B49 RID: 2889
		private static TraceSwitch dataGridParents;

		// Token: 0x04000B4A RID: 2890
		private static TraceSwitch dataGridScrolling;

		// Token: 0x04000B4B RID: 2891
		private static TraceSwitch dataGridSelection;

		// Token: 0x04000B4C RID: 2892
		private static TraceSwitch dataObject;

		// Token: 0x04000B4D RID: 2893
		private static TraceSwitch dataView;

		// Token: 0x04000B4E RID: 2894
		private static TraceSwitch debugGridView;

		// Token: 0x04000B4F RID: 2895
		private static TraceSwitch dgCaptionPaint;

		// Token: 0x04000B50 RID: 2896
		private static TraceSwitch dgEditColumnEditing;

		// Token: 0x04000B51 RID: 2897
		private static TraceSwitch dgRelationShpRowLayout;

		// Token: 0x04000B52 RID: 2898
		private static TraceSwitch dgRelationShpRowPaint;

		// Token: 0x04000B53 RID: 2899
		private static TraceSwitch dgRowPaint;

		// Token: 0x04000B54 RID: 2900
		private static TraceSwitch dragDrop;

		// Token: 0x04000B55 RID: 2901
		private static TraceSwitch imeMode;

		// Token: 0x04000B56 RID: 2902
		private static TraceSwitch msaa;

		// Token: 0x04000B57 RID: 2903
		private static TraceSwitch msoComponentManager;

		// Token: 0x04000B58 RID: 2904
		private static TraceSwitch layoutPerformance;

		// Token: 0x04000B59 RID: 2905
		private static TraceSwitch layoutSuspendResume;

		// Token: 0x04000B5A RID: 2906
		private static TraceSwitch richLayout;

		// Token: 0x04000B5B RID: 2907
		private static TraceSwitch setBounds;

		// Token: 0x04000B5C RID: 2908
		private static BooleanSwitch lifetimeTracing;

		// Token: 0x04000B5D RID: 2909
		private static TraceSwitch handleLeak;

		// Token: 0x04000B5E RID: 2910
		private static BooleanSwitch traceCollect;
	}
}
