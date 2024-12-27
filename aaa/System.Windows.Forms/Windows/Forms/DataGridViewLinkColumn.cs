using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x0200037B RID: 891
	[ToolboxBitmap(typeof(DataGridViewLinkColumn), "DataGridViewLinkColumn.bmp")]
	public class DataGridViewLinkColumn : DataGridViewColumn
	{
		// Token: 0x0600369E RID: 13982 RVA: 0x000C3A56 File Offset: 0x000C2A56
		public DataGridViewLinkColumn()
			: base(new DataGridViewLinkCell())
		{
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x0600369F RID: 13983 RVA: 0x000C3A63 File Offset: 0x000C2A63
		// (set) Token: 0x060036A0 RID: 13984 RVA: 0x000C3A90 File Offset: 0x000C2A90
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_LinkColumnActiveLinkColorDescr")]
		public Color ActiveLinkColor
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).ActiveLinkColor;
			}
			set
			{
				if (!this.ActiveLinkColor.Equals(value))
				{
					((DataGridViewLinkCell)this.CellTemplate).ActiveLinkColorInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.ActiveLinkColorInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x000C3B30 File Offset: 0x000C2B30
		private bool ShouldSerializeActiveLinkColor()
		{
			return !this.ActiveLinkColor.Equals(LinkUtilities.IEActiveLinkColor);
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060036A2 RID: 13986 RVA: 0x000C3B5E File Offset: 0x000C2B5E
		// (set) Token: 0x060036A3 RID: 13987 RVA: 0x000C3B68 File Offset: 0x000C2B68
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				if (value != null && !(value is DataGridViewLinkCell))
				{
					throw new InvalidCastException(SR.GetString("DataGridViewTypeColumn_WrongCellTemplateType", new object[] { "System.Windows.Forms.DataGridViewLinkCell" }));
				}
				base.CellTemplate = value;
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060036A4 RID: 13988 RVA: 0x000C3BA7 File Offset: 0x000C2BA7
		// (set) Token: 0x060036A5 RID: 13989 RVA: 0x000C3BD4 File Offset: 0x000C2BD4
		[SRCategory("CatBehavior")]
		[SRDescription("DataGridView_LinkColumnLinkBehaviorDescr")]
		[DefaultValue(LinkBehavior.SystemDefault)]
		public LinkBehavior LinkBehavior
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).LinkBehavior;
			}
			set
			{
				if (!this.LinkBehavior.Equals(value))
				{
					((DataGridViewLinkCell)this.CellTemplate).LinkBehavior = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.LinkBehaviorInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060036A6 RID: 13990 RVA: 0x000C3C6E File Offset: 0x000C2C6E
		// (set) Token: 0x060036A7 RID: 13991 RVA: 0x000C3C98 File Offset: 0x000C2C98
		[SRDescription("DataGridView_LinkColumnLinkColorDescr")]
		[SRCategory("CatAppearance")]
		public Color LinkColor
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).LinkColor;
			}
			set
			{
				if (!this.LinkColor.Equals(value))
				{
					((DataGridViewLinkCell)this.CellTemplate).LinkColorInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.LinkColorInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x000C3D38 File Offset: 0x000C2D38
		private bool ShouldSerializeLinkColor()
		{
			return !this.LinkColor.Equals(LinkUtilities.IELinkColor);
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060036A9 RID: 13993 RVA: 0x000C3D66 File Offset: 0x000C2D66
		// (set) Token: 0x060036AA RID: 13994 RVA: 0x000C3D70 File Offset: 0x000C2D70
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_LinkColumnTextDescr")]
		[DefaultValue(null)]
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				if (!string.Equals(value, this.text, StringComparison.Ordinal))
				{
					this.text = value;
					if (base.DataGridView != null)
					{
						if (this.UseColumnTextForLinkValue)
						{
							base.DataGridView.OnColumnCommonChange(base.Index);
							return;
						}
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null && dataGridViewLinkCell.UseColumnTextForLinkValue)
							{
								base.DataGridView.OnColumnCommonChange(base.Index);
								return;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060036AB RID: 13995 RVA: 0x000C3E2A File Offset: 0x000C2E2A
		// (set) Token: 0x060036AC RID: 13996 RVA: 0x000C3E54 File Offset: 0x000C2E54
		[SRDescription("DataGridView_LinkColumnTrackVisitedStateDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool TrackVisitedState
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).TrackVisitedState;
			}
			set
			{
				if (this.TrackVisitedState != value)
				{
					((DataGridViewLinkCell)this.CellTemplate).TrackVisitedStateInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.TrackVisitedStateInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060036AD RID: 13997 RVA: 0x000C3EDF File Offset: 0x000C2EDF
		// (set) Token: 0x060036AE RID: 13998 RVA: 0x000C3F0C File Offset: 0x000C2F0C
		[SRDescription("DataGridView_LinkColumnUseColumnTextForLinkValueDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public bool UseColumnTextForLinkValue
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).UseColumnTextForLinkValue;
			}
			set
			{
				if (this.UseColumnTextForLinkValue != value)
				{
					((DataGridViewLinkCell)this.CellTemplate).UseColumnTextForLinkValueInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.UseColumnTextForLinkValueInternal = value;
							}
						}
						base.DataGridView.OnColumnCommonChange(base.Index);
					}
				}
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060036AF RID: 13999 RVA: 0x000C3F97 File Offset: 0x000C2F97
		// (set) Token: 0x060036B0 RID: 14000 RVA: 0x000C3FC4 File Offset: 0x000C2FC4
		[SRCategory("CatAppearance")]
		[SRDescription("DataGridView_LinkColumnVisitedLinkColorDescr")]
		public Color VisitedLinkColor
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new InvalidOperationException(SR.GetString("DataGridViewColumn_CellTemplateRequired"));
				}
				return ((DataGridViewLinkCell)this.CellTemplate).VisitedLinkColor;
			}
			set
			{
				if (!this.VisitedLinkColor.Equals(value))
				{
					((DataGridViewLinkCell)this.CellTemplate).VisitedLinkColorInternal = value;
					if (base.DataGridView != null)
					{
						DataGridViewRowCollection rows = base.DataGridView.Rows;
						int count = rows.Count;
						for (int i = 0; i < count; i++)
						{
							DataGridViewRow dataGridViewRow = rows.SharedRow(i);
							DataGridViewLinkCell dataGridViewLinkCell = dataGridViewRow.Cells[base.Index] as DataGridViewLinkCell;
							if (dataGridViewLinkCell != null)
							{
								dataGridViewLinkCell.VisitedLinkColorInternal = value;
							}
						}
						base.DataGridView.InvalidateColumn(base.Index);
					}
				}
			}
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x000C4064 File Offset: 0x000C3064
		private bool ShouldSerializeVisitedLinkColor()
		{
			return !this.VisitedLinkColor.Equals(LinkUtilities.IEVisitedLinkColor);
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x000C4094 File Offset: 0x000C3094
		public override object Clone()
		{
			Type type = base.GetType();
			DataGridViewLinkColumn dataGridViewLinkColumn;
			if (type == DataGridViewLinkColumn.columnType)
			{
				dataGridViewLinkColumn = new DataGridViewLinkColumn();
			}
			else
			{
				dataGridViewLinkColumn = (DataGridViewLinkColumn)Activator.CreateInstance(type);
			}
			if (dataGridViewLinkColumn != null)
			{
				base.CloneInternal(dataGridViewLinkColumn);
				dataGridViewLinkColumn.Text = this.text;
			}
			return dataGridViewLinkColumn;
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x000C40DC File Offset: 0x000C30DC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("DataGridViewLinkColumn { Name=");
			stringBuilder.Append(base.Name);
			stringBuilder.Append(", Index=");
			stringBuilder.Append(base.Index.ToString(CultureInfo.CurrentCulture));
			stringBuilder.Append(" }");
			return stringBuilder.ToString();
		}

		// Token: 0x04001BED RID: 7149
		private static Type columnType = typeof(DataGridViewLinkColumn);

		// Token: 0x04001BEE RID: 7150
		private string text;
	}
}
