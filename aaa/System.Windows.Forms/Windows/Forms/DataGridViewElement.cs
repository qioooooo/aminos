using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000305 RID: 773
	public class DataGridViewElement
	{
		// Token: 0x06003147 RID: 12615 RVA: 0x000A987C File Offset: 0x000A887C
		public DataGridViewElement()
		{
			this.state = DataGridViewElementStates.Visible;
		}

		// Token: 0x06003148 RID: 12616 RVA: 0x000A988C File Offset: 0x000A888C
		internal DataGridViewElement(DataGridViewElement dgveTemplate)
		{
			this.state = dgveTemplate.State & (DataGridViewElementStates.Frozen | DataGridViewElementStates.ReadOnly | DataGridViewElementStates.Resizable | DataGridViewElementStates.ResizableSet | DataGridViewElementStates.Visible);
		}

		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06003149 RID: 12617 RVA: 0x000A98A3 File Offset: 0x000A88A3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public virtual DataGridViewElementStates State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x1700086A RID: 2154
		// (set) Token: 0x0600314A RID: 12618 RVA: 0x000A98AB File Offset: 0x000A88AB
		internal DataGridViewElementStates StateInternal
		{
			set
			{
				this.state = value;
			}
		}

		// Token: 0x0600314B RID: 12619 RVA: 0x000A98B4 File Offset: 0x000A88B4
		internal bool StateIncludes(DataGridViewElementStates elementState)
		{
			return (this.State & elementState) == elementState;
		}

		// Token: 0x0600314C RID: 12620 RVA: 0x000A98C1 File Offset: 0x000A88C1
		internal bool StateExcludes(DataGridViewElementStates elementState)
		{
			return (this.State & elementState) == DataGridViewElementStates.None;
		}

		// Token: 0x1700086B RID: 2155
		// (get) Token: 0x0600314D RID: 12621 RVA: 0x000A98CE File Offset: 0x000A88CE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public DataGridView DataGridView
		{
			get
			{
				return this.dataGridView;
			}
		}

		// Token: 0x1700086C RID: 2156
		// (set) Token: 0x0600314E RID: 12622 RVA: 0x000A98D6 File Offset: 0x000A88D6
		internal DataGridView DataGridViewInternal
		{
			set
			{
				if (this.DataGridView != value)
				{
					this.dataGridView = value;
					this.OnDataGridViewChanged();
				}
			}
		}

		// Token: 0x0600314F RID: 12623 RVA: 0x000A98EE File Offset: 0x000A88EE
		protected virtual void OnDataGridViewChanged()
		{
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x000A98F0 File Offset: 0x000A88F0
		protected void RaiseCellClick(DataGridViewCellEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnCellClickInternal(e);
			}
		}

		// Token: 0x06003151 RID: 12625 RVA: 0x000A9906 File Offset: 0x000A8906
		protected void RaiseCellContentClick(DataGridViewCellEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnCellContentClickInternal(e);
			}
		}

		// Token: 0x06003152 RID: 12626 RVA: 0x000A991C File Offset: 0x000A891C
		protected void RaiseCellContentDoubleClick(DataGridViewCellEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnCellContentDoubleClickInternal(e);
			}
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x000A9932 File Offset: 0x000A8932
		protected void RaiseCellValueChanged(DataGridViewCellEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnCellValueChangedInternal(e);
			}
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x000A9948 File Offset: 0x000A8948
		protected void RaiseDataError(DataGridViewDataErrorEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnDataErrorInternal(e);
			}
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x000A995E File Offset: 0x000A895E
		protected void RaiseMouseWheel(MouseEventArgs e)
		{
			if (this.dataGridView != null)
			{
				this.dataGridView.OnMouseWheelInternal(e);
			}
		}

		// Token: 0x04001A41 RID: 6721
		private DataGridViewElementStates state;

		// Token: 0x04001A42 RID: 6722
		private DataGridView dataGridView;
	}
}
