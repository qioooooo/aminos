using System;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI
{
	// Token: 0x020003E1 RID: 993
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DataSourceSelectArguments
	{
		// Token: 0x06003026 RID: 12326 RVA: 0x000D4EC8 File Offset: 0x000D3EC8
		public DataSourceSelectArguments()
			: this(string.Empty, 0, 0)
		{
		}

		// Token: 0x06003027 RID: 12327 RVA: 0x000D4ED7 File Offset: 0x000D3ED7
		public DataSourceSelectArguments(string sortExpression)
			: this(sortExpression, 0, 0)
		{
		}

		// Token: 0x06003028 RID: 12328 RVA: 0x000D4EE2 File Offset: 0x000D3EE2
		public DataSourceSelectArguments(int startRowIndex, int maximumRows)
			: this(string.Empty, startRowIndex, maximumRows)
		{
		}

		// Token: 0x06003029 RID: 12329 RVA: 0x000D4EF1 File Offset: 0x000D3EF1
		public DataSourceSelectArguments(string sortExpression, int startRowIndex, int maximumRows)
		{
			this.SortExpression = sortExpression;
			this.StartRowIndex = startRowIndex;
			this.MaximumRows = maximumRows;
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x0600302A RID: 12330 RVA: 0x000D4F15 File Offset: 0x000D3F15
		public static DataSourceSelectArguments Empty
		{
			get
			{
				return new DataSourceSelectArguments();
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x0600302B RID: 12331 RVA: 0x000D4F1C File Offset: 0x000D3F1C
		// (set) Token: 0x0600302C RID: 12332 RVA: 0x000D4F24 File Offset: 0x000D3F24
		public int MaximumRows
		{
			get
			{
				return this._maximumRows;
			}
			set
			{
				if (value == 0)
				{
					if (this._startRowIndex == 0)
					{
						this._requestedCapabilities &= ~DataSourceCapabilities.Page;
					}
				}
				else
				{
					this._requestedCapabilities |= DataSourceCapabilities.Page;
				}
				this._maximumRows = value;
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x0600302D RID: 12333 RVA: 0x000D4F57 File Offset: 0x000D3F57
		// (set) Token: 0x0600302E RID: 12334 RVA: 0x000D4F5F File Offset: 0x000D3F5F
		public bool RetrieveTotalRowCount
		{
			get
			{
				return this._retrieveTotalRowCount;
			}
			set
			{
				if (value)
				{
					this._requestedCapabilities |= DataSourceCapabilities.RetrieveTotalRowCount;
				}
				else
				{
					this._requestedCapabilities &= ~DataSourceCapabilities.RetrieveTotalRowCount;
				}
				this._retrieveTotalRowCount = value;
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x0600302F RID: 12335 RVA: 0x000D4F8A File Offset: 0x000D3F8A
		// (set) Token: 0x06003030 RID: 12336 RVA: 0x000D4FA5 File Offset: 0x000D3FA5
		public string SortExpression
		{
			get
			{
				if (this._sortExpression == null)
				{
					this._sortExpression = string.Empty;
				}
				return this._sortExpression;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this._requestedCapabilities &= ~DataSourceCapabilities.Sort;
				}
				else
				{
					this._requestedCapabilities |= DataSourceCapabilities.Sort;
				}
				this._sortExpression = value;
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06003031 RID: 12337 RVA: 0x000D4FD5 File Offset: 0x000D3FD5
		// (set) Token: 0x06003032 RID: 12338 RVA: 0x000D4FDD File Offset: 0x000D3FDD
		public int StartRowIndex
		{
			get
			{
				return this._startRowIndex;
			}
			set
			{
				if (value == 0)
				{
					if (this._maximumRows == 0)
					{
						this._requestedCapabilities &= ~DataSourceCapabilities.Page;
					}
				}
				else
				{
					this._requestedCapabilities |= DataSourceCapabilities.Page;
				}
				this._startRowIndex = value;
			}
		}

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06003033 RID: 12339 RVA: 0x000D5010 File Offset: 0x000D4010
		// (set) Token: 0x06003034 RID: 12340 RVA: 0x000D5018 File Offset: 0x000D4018
		public int TotalRowCount
		{
			get
			{
				return this._totalRowCount;
			}
			set
			{
				this._totalRowCount = value;
			}
		}

		// Token: 0x06003035 RID: 12341 RVA: 0x000D5021 File Offset: 0x000D4021
		public void AddSupportedCapabilities(DataSourceCapabilities capabilities)
		{
			this._supportedCapabilities |= capabilities;
		}

		// Token: 0x06003036 RID: 12342 RVA: 0x000D5031 File Offset: 0x000D4031
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(this._maximumRows.GetHashCode(), this._retrieveTotalRowCount.GetHashCode(), this._sortExpression.GetHashCode(), this._startRowIndex.GetHashCode(), this._totalRowCount.GetHashCode());
		}

		// Token: 0x06003037 RID: 12343 RVA: 0x000D5070 File Offset: 0x000D4070
		public override bool Equals(object obj)
		{
			DataSourceSelectArguments dataSourceSelectArguments = obj as DataSourceSelectArguments;
			return dataSourceSelectArguments != null && (dataSourceSelectArguments.MaximumRows == this._maximumRows && dataSourceSelectArguments.RetrieveTotalRowCount == this._retrieveTotalRowCount && dataSourceSelectArguments.SortExpression == this._sortExpression && dataSourceSelectArguments.StartRowIndex == this._startRowIndex) && dataSourceSelectArguments.TotalRowCount == this._totalRowCount;
		}

		// Token: 0x06003038 RID: 12344 RVA: 0x000D50D8 File Offset: 0x000D40D8
		public void RaiseUnsupportedCapabilitiesError(DataSourceView view)
		{
			DataSourceCapabilities dataSourceCapabilities = this._requestedCapabilities & ~this._supportedCapabilities;
			if ((dataSourceCapabilities & DataSourceCapabilities.Sort) != DataSourceCapabilities.None)
			{
				view.RaiseUnsupportedCapabilityError(DataSourceCapabilities.Sort);
			}
			if ((dataSourceCapabilities & DataSourceCapabilities.Page) != DataSourceCapabilities.None)
			{
				view.RaiseUnsupportedCapabilityError(DataSourceCapabilities.Page);
			}
			if ((dataSourceCapabilities & DataSourceCapabilities.RetrieveTotalRowCount) != DataSourceCapabilities.None)
			{
				view.RaiseUnsupportedCapabilityError(DataSourceCapabilities.RetrieveTotalRowCount);
			}
		}

		// Token: 0x0400220C RID: 8716
		private DataSourceCapabilities _requestedCapabilities;

		// Token: 0x0400220D RID: 8717
		private DataSourceCapabilities _supportedCapabilities;

		// Token: 0x0400220E RID: 8718
		private int _maximumRows;

		// Token: 0x0400220F RID: 8719
		private bool _retrieveTotalRowCount;

		// Token: 0x04002210 RID: 8720
		private string _sortExpression;

		// Token: 0x04002211 RID: 8721
		private int _startRowIndex;

		// Token: 0x04002212 RID: 8722
		private int _totalRowCount = -1;
	}
}
