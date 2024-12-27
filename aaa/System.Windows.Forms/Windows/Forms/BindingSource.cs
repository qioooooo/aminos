using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x02000250 RID: 592
	[DefaultProperty("DataSource")]
	[DefaultEvent("CurrentChanged")]
	[ComplexBindingProperties("DataSource", "DataMember")]
	[Designer("System.Windows.Forms.Design.BindingSourceDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionBindingSource")]
	public class BindingSource : Component, IBindingListView, IBindingList, IList, ICollection, IEnumerable, ITypedList, ICancelAddNew, ISupportInitializeNotification, ISupportInitialize, ICurrencyManagerProvider
	{
		// Token: 0x06001E92 RID: 7826 RVA: 0x0003F7C6 File Offset: 0x0003E7C6
		public BindingSource()
			: this(null, string.Empty)
		{
		}

		// Token: 0x06001E93 RID: 7827 RVA: 0x0003F7D4 File Offset: 0x0003E7D4
		public BindingSource(object dataSource, string dataMember)
		{
			this.dataSource = dataSource;
			this.dataMember = dataMember;
			this._innerList = new ArrayList();
			this.currencyManager = new CurrencyManager(this);
			this.WireCurrencyManager(this.currencyManager);
			this.ResetList();
			this.listItemPropertyChangedHandler = new EventHandler(this.ListItem_PropertyChanged);
			this.WireDataSource();
		}

		// Token: 0x06001E94 RID: 7828 RVA: 0x0003F856 File Offset: 0x0003E856
		public BindingSource(