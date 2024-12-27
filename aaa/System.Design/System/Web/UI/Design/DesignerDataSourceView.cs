using System;
using System.Collections;

namespace System.Web.UI.Design
{
	// Token: 0x0200035B RID: 859
	public abstract class DesignerDataSourceView
	{
		// Token: 0x0600203E RID: 8254 RVA: 0x000B661A File Offset: 0x000B561A
		protected DesignerDataSourceView(IDataSourceDesigner owner, string viewName)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			if (viewName == null)
			{
				throw new ArgumentNullException("viewName");
			}
			this._owner = owner;
			this._name = viewName;
		}

		// Token: 0x170005AF RID: 1455
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x000B664C File Offset: 0x000B564C
		public virtual bool CanDelete
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B0 RID: 1456
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x000B664F File Offset: 0x000B564F
		public virtual bool CanInsert
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B1 RID: 1457
		// (get) Token: 0x06002041 RID: 8257 RVA: 0x000B6652 File Offset: 0x000B5652
		public virtual bool CanPage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06002042 RID: 8258 RVA: 0x000B6655 File Offset: 0x000B5655
		public virtual bool CanRetrieveTotalRowCount
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x000B6658 File Offset: 0x000B5658
		public virtual bool CanSort
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x000B665B File Offset: 0x000B565B
		public virtual bool CanUpdate
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x06002045 RID: 8261 RVA: 0x000B665E File Offset: 0x000B565E
		public IDataSourceDesigner DataSourceDesigner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x06002046 RID: 8262 RVA: 0x000B6666 File Offset: 0x000B5666
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x06002047 RID: 8263 RVA: 0x000B666E File Offset: 0x000B566E
		public virtual IDataSourceViewSchema Schema
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000B6671 File Offset: 0x000B5671
		public virtual IEnumerable GetDesignTimeData(int minimumRows, out bool isSampleData)
		{
			isSampleData = true;
			return DesignTimeData.GetDesignTimeDataSource(DesignTimeData.CreateDummyDataBoundDataTable(), minimumRows);
		}

		// Token: 0x040017CD RID: 6093
		private string _name;

		// Token: 0x040017CE RID: 6094
		private IDataSourceDesigner _owner;
	}
}
