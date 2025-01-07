using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	public class DesignerOptions
	{
		[SRDescription("DesignerOptions_GridSizeDesc")]
		[SRCategory("DesignerOptions_LayoutSettings")]
		public virtual Size GridSize
		{
			get
			{
				return this.gridSize;
			}
			set
			{
				if (value.Width < 2)
				{
					value.Width = 2;
				}
				if (value.Height < 2)
				{
					value.Height = 2;
				}
				if (value.Width > 200)
				{
					value.Width = 200;
				}
				if (value.Height > 200)
				{
					value.Height = 200;
				}
				this.gridSize = value;
			}
		}

		[SRCategory("DesignerOptions_LayoutSettings")]
		[SRDescription("DesignerOptions_ShowGridDesc")]
		public virtual bool ShowGrid
		{
			get
			{
				return this.showGrid;
			}
			set
			{
				this.showGrid = value;
			}
		}

		[SRCategory("DesignerOptions_LayoutSettings")]
		[SRDescription("DesignerOptions_SnapToGridDesc")]
		public virtual bool SnapToGrid
		{
			get
			{
				return this.snapToGrid;
			}
			set
			{
				this.snapToGrid = value;
			}
		}

		[SRDescription("DesignerOptions_UseSnapLines")]
		[SRCategory("DesignerOptions_LayoutSettings")]
		public virtual bool UseSnapLines
		{
			get
			{
				return this.useSnapLines;
			}
			set
			{
				this.useSnapLines = value;
			}
		}

		[SRDescription("DesignerOptions_UseSmartTags")]
		[SRCategory("DesignerOptions_LayoutSettings")]
		public virtual bool UseSmartTags
		{
			get
			{
				return this.useSmartTags;
			}
			set
			{
				this.useSmartTags = value;
			}
		}

		[SRDescription("DesignerOptions_ObjectBoundSmartTagAutoShow")]
		[SRCategory("DesignerOptions_ObjectBoundSmartTagSettings")]
		[SRDisplayName("DesignerOptions_ObjectBoundSmartTagAutoShowDisplayName")]
		public virtual bool ObjectBoundSmartTagAutoShow
		{
			get
			{
				return this.objectBoundSmartTagAutoShow;
			}
			set
			{
				this.objectBoundSmartTagAutoShow = value;
			}
		}

		[SRDisplayName("DesignerOptions_CodeGenDisplay")]
		[SRCategory("DesignerOptions_CodeGenSettings")]
		[SRDescription("DesignerOptions_OptimizedCodeGen")]
		public virtual bool UseOptimizedCodeGeneration
		{
			get
			{
				return this.enableComponentCache;
			}
			set
			{
				this.enableComponentCache = value;
			}
		}

		[SRDisplayName("DesignerOptions_EnableInSituEditingDisplay")]
		[SRDescription("DesignerOptions_EnableInSituEditingDesc")]
		[Browsable(false)]
		[SRCategory("DesignerOptions_EnableInSituEditingCat")]
		public virtual bool EnableInSituEditing
		{
			get
			{
				return this.enableInSituEditing;
			}
			set
			{
				this.enableInSituEditing = value;
			}
		}

		private const int minGridSize = 2;

		private const int maxGridSize = 200;

		private bool showGrid = true;

		private bool snapToGrid = true;

		private Size gridSize = new Size(8, 8);

		private bool useSnapLines;

		private bool useSmartTags;

		private bool objectBoundSmartTagAutoShow = true;

		private bool enableComponentCache;

		private bool enableInSituEditing = true;
	}
}
