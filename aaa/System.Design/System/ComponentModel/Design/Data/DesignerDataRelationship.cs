using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000146 RID: 326
	public sealed class DesignerDataRelationship
	{
		// Token: 0x06000C91 RID: 3217 RVA: 0x0003098A File Offset: 0x0002F98A
		public DesignerDataRelationship(string name, ICollection parentColumns, DesignerDataTable childTable, ICollection childColumns)
		{
			this._childColumns = childColumns;
			this._childTable = childTable;
			this._name = name;
			this._parentColumns = parentColumns;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x000309AF File Offset: 0x0002F9AF
		public ICollection ChildColumns
		{
			get
			{
				return this._childColumns;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x000309B7 File Offset: 0x0002F9B7
		public DesignerDataTable ChildTable
		{
			get
			{
				return this._childTable;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x000309BF File Offset: 0x0002F9BF
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x000309C7 File Offset: 0x0002F9C7
		public ICollection ParentColumns
		{
			get
			{
				return this._parentColumns;
			}
		}

		// Token: 0x04000EAA RID: 3754
		private ICollection _childColumns;

		// Token: 0x04000EAB RID: 3755
		private DesignerDataTable _childTable;

		// Token: 0x04000EAC RID: 3756
		private string _name;

		// Token: 0x04000EAD RID: 3757
		private ICollection _parentColumns;
	}
}
