using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005C8 RID: 1480
	[ValidationProperty("SelectedItem")]
	[SupportsEventValidation]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ListBox : ListControl, IPostBackDataHandler
	{
		// Token: 0x170011C7 RID: 4551
		// (get) Token: 0x06004822 RID: 18466 RVA: 0x00126B09 File Offset: 0x00125B09
		// (set) Token: 0x06004823 RID: 18467 RVA: 0x00126B11 File Offset: 0x00125B11
		[Browsable(false)]
		public override Color BorderColor
		{
			get
			{
				return base.BorderColor;
			}
			set
			{
				base.BorderColor = value;
			}
		}

		// Token: 0x170011C8 RID: 4552
		// (get) Token: 0x06004824 RID: 18468 RVA: 0x00126B1A File Offset: 0x00125B1A
		// (set) Token: 0x06004825 RID: 18469 RVA: 0x00126B22 File Offset: 0x00125B22
		[Browsable(false)]
		public override BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		// Token: 0x170011C9 RID: 4553
		// (get) Token: 0x06004826 RID: 18470 RVA: 0x00126B2B File Offset: 0x00125B2B
		// (set) Token: 0x06004827 RID: 18471 RVA: 0x00126B33 File Offset: 0x00125B33
		[Browsable(false)]
		public override Unit BorderWidth
		{
			get
			{
				return base.BorderWidth;
			}
			set
			{
				base.BorderWidth = value;
			}
		}

		// Token: 0x170011CA RID: 4554
		// (get) Token: 0x06004828 RID: 18472 RVA: 0x00126B3C File Offset: 0x00125B3C
		internal override bool IsMultiSelectInternal
		{
			get
			{
				return this.SelectionMode == ListSelectionMode.Multiple;
			}
		}

		// Token: 0x170011CB RID: 4555
		// (get) Token: 0x06004829 RID: 18473 RVA: 0x00126B48 File Offset: 0x00125B48
		// (set) Token: 0x0600482A RID: 18474 RVA: 0x00126B71 File Offset: 0x00125B71
		[WebSysDescription("ListBox_Rows")]
		[WebCategory("Appearance")]
		[DefaultValue(4)]
		public virtual int Rows
		{
			get
			{
				object obj = this.ViewState["Rows"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 4;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["Rows"] = value;
			}
		}

		// Token: 0x170011CC RID: 4556
		// (get) Token: 0x0600482B RID: 18475 RVA: 0x00126B98 File Offset: 0x00125B98
		// (set) Token: 0x0600482C RID: 18476 RVA: 0x00126BC1 File Offset: 0x00125BC1
		[WebCategory("Behavior")]
		[WebSysDescription("ListBox_SelectionMode")]
		[DefaultValue(ListSelectionMode.Single)]
		public virtual ListSelectionMode SelectionMode
		{
			get
			{
				object obj = this.ViewState["SelectionMode"];
				if (obj != null)
				{
					return (ListSelectionMode)obj;
				}
				return ListSelectionMode.Single;
			}
			set
			{
				if (value < ListSelectionMode.Single || value > ListSelectionMode.Multiple)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["SelectionMode"] = value;
			}
		}

		// Token: 0x0600482D RID: 18477 RVA: 0x00126BEC File Offset: 0x00125BEC
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Size, this.Rows.ToString(NumberFormatInfo.InvariantInfo));
			string uniqueID = this.UniqueID;
			if (uniqueID != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Name, uniqueID);
			}
			base.AddAttributesToRender(writer);
		}

		// Token: 0x0600482E RID: 18478 RVA: 0x00126C2E File Offset: 0x00125C2E
		public virtual int[] GetSelectedIndices()
		{
			return (int[])this.SelectedIndicesInternal.ToArray(typeof(int));
		}

		// Token: 0x0600482F RID: 18479 RVA: 0x00126C4A File Offset: 0x00125C4A
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Page != null && this.SelectionMode == ListSelectionMode.Multiple && this.Enabled)
			{
				this.Page.RegisterRequiresPostBack(this);
			}
		}

		// Token: 0x06004830 RID: 18480 RVA: 0x00126C78 File Offset: 0x00125C78
		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			return this.LoadPostData(postDataKey, postCollection);
		}

		// Token: 0x06004831 RID: 18481 RVA: 0x00126C84 File Offset: 0x00125C84
		protected virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			if (!base.IsEnabled)
			{
				return false;
			}
			string[] values = postCollection.GetValues(postDataKey);
			bool flag = false;
			this.EnsureDataBound();
			if (values != null)
			{
				if (this.SelectionMode == ListSelectionMode.Single)
				{
					base.ValidateEvent(postDataKey, values[0]);
					int num = this.Items.FindByValueInternal(values[0], false);
					if (this.SelectedIndex != num)
					{
						base.SetPostDataSelection(num);
						flag = true;
					}
				}
				else
				{
					int num2 = values.Length;
					ArrayList selectedIndicesInternal = this.SelectedIndicesInternal;
					ArrayList arrayList = new ArrayList(num2);
					for (int i = 0; i < num2; i++)
					{
						base.ValidateEvent(postDataKey, values[i]);
						arrayList.Add(this.Items.FindByValueInternal(values[i], false));
					}
					int num3 = 0;
					if (selectedIndicesInternal != null)
					{
						num3 = selectedIndicesInternal.Count;
					}
					if (num3 == num2)
					{
						for (int j = 0; j < num2; j++)
						{
							if ((int)arrayList[j] != (int)selectedIndicesInternal[j])
							{
								flag = true;
								break;
							}
						}
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						base.SelectInternal(arrayList);
					}
				}
			}
			else if (this.SelectedIndex != -1)
			{
				base.SetPostDataSelection(-1);
				flag = true;
			}
			return flag;
		}

		// Token: 0x06004832 RID: 18482 RVA: 0x00126DA4 File Offset: 0x00125DA4
		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			this.RaisePostDataChangedEvent();
		}

		// Token: 0x06004833 RID: 18483 RVA: 0x00126DAC File Offset: 0x00125DAC
		protected virtual void RaisePostDataChangedEvent()
		{
			if (this.AutoPostBack && !this.Page.IsPostBackEventControlRegistered)
			{
				this.Page.AutoPostBackControl = this;
				if (this.CausesValidation)
				{
					this.Page.Validate(this.ValidationGroup);
				}
			}
			this.OnSelectedIndexChanged(EventArgs.Empty);
		}
	}
}
