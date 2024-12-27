using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
	// Token: 0x02000424 RID: 1060
	[SRDescription("DescriptionHelpProvider")]
	[ProvideProperty("HelpString", typeof(Control))]
	[ProvideProperty("HelpNavigator", typeof(Control))]
	[ToolboxItemFilter("System.Windows.Forms")]
	[ProvideProperty("HelpKeyword", typeof(Control))]
	[ProvideProperty("ShowHelp", typeof(Control))]
	public class HelpProvider : Component, IExtenderProvider
	{
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06003EE2 RID: 16098 RVA: 0x000E4EDA File Offset: 0x000E3EDA
		// (set) Token: 0x06003EE3 RID: 16099 RVA: 0x000E4EE2 File Offset: 0x000E3EE2
		[Editor("System.Windows.Forms.Design.HelpNamespaceEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		[Localizable(true)]
		[SRDescription("HelpProviderHelpNamespaceDescr")]
		[DefaultValue(null)]
		public virtual string HelpNamespace
		{
			get
			{
				return this.helpNamespace;
			}
			set
			{
				this.helpNamespace = value;
			}
		}

		// Token: 0x17000C15 RID: 3093
		// (get) Token: 0x06003EE4 RID: 16100 RVA: 0x000E4EEB File Offset: 0x000E3EEB
		// (set) Token: 0x06003EE5 RID: 16101 RVA: 0x000E4EF3 File Offset: 0x000E3EF3
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[SRCategory("CatData")]
		[Localizable(false)]
		[TypeConverter(typeof(StringConverter))]
		[DefaultValue(null)]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x06003EE6 RID: 16102 RVA: 0x000E4EFC File Offset: 0x000E3EFC
		public virtual bool CanExtend(object target)
		{
			return target is Control;
		}

		// Token: 0x06003EE7 RID: 16103 RVA: 0x000E4F07 File Offset: 0x000E3F07
		[Localizable(true)]
		[DefaultValue(null)]
		[SRDescription("HelpProviderHelpKeywordDescr")]
		public virtual string GetHelpKeyword(Control ctl)
		{
			return (string)this.keywords[ctl];
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x000E4F1C File Offset: 0x000E3F1C
		[Localizable(true)]
		[SRDescription("HelpProviderNavigatorDescr")]
		[DefaultValue(HelpNavigator.AssociateIndex)]
		public virtual HelpNavigator GetHelpNavigator(Control ctl)
		{
			object obj = this.navigators[ctl];
			if (obj != null)
			{
				return (HelpNavigator)obj;
			}
			return HelpNavigator.AssociateIndex;
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x000E4F45 File Offset: 0x000E3F45
		[SRDescription("HelpProviderHelpStringDescr")]
		[Localizable(true)]
		[DefaultValue(null)]
		public virtual string GetHelpString(Control ctl)
		{
			return (string)this.helpStrings[ctl];
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x000E4F58 File Offset: 0x000E3F58
		[Localizable(true)]
		[SRDescription("HelpProviderShowHelpDescr")]
		public virtual bool GetShowHelp(Control ctl)
		{
			object obj = this.showHelp[ctl];
			return obj != null && (bool)obj;
		}

		// Token: 0x06003EEB RID: 16107 RVA: 0x000E4F80 File Offset: 0x000E3F80
		private void OnControlHelp(object sender, HelpEventArgs hevent)
		{
			Control control = (Control)sender;
			string helpString = this.GetHelpString(control);
			string helpKeyword = this.GetHelpKeyword(control);
			HelpNavigator helpNavigator = this.GetHelpNavigator(control);
			if (!this.GetShowHelp(control))
			{
				return;
			}
			if (Control.MouseButtons != MouseButtons.None && helpString != null && helpString.Length > 0)
			{
				Help.ShowPopup(control, helpString, hevent.MousePos);
				hevent.Handled = true;
			}
			if (!hevent.Handled && this.helpNamespace != null)
			{
				if (helpKeyword != null && helpKeyword.Length > 0)
				{
					Help.ShowHelp(control, this.helpNamespace, helpNavigator, helpKeyword);
					hevent.Handled = true;
				}
				if (!hevent.Handled)
				{
					Help.ShowHelp(control, this.helpNamespace, helpNavigator);
					hevent.Handled = true;
				}
			}
			if (!hevent.Handled && helpString != null && helpString.Length > 0)
			{
				Help.ShowPopup(control, helpString, hevent.MousePos);
				hevent.Handled = true;
			}
			if (!hevent.Handled && this.helpNamespace != null)
			{
				Help.ShowHelp(control, this.helpNamespace);
				hevent.Handled = true;
			}
		}

		// Token: 0x06003EEC RID: 16108 RVA: 0x000E507C File Offset: 0x000E407C
		private void OnQueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
		{
			Control control = (Control)sender;
			e.HelpString = this.GetHelpString(control);
			e.HelpKeyword = this.GetHelpKeyword(control);
			e.HelpNamespace = this.HelpNamespace;
		}

		// Token: 0x06003EED RID: 16109 RVA: 0x000E50B6 File Offset: 0x000E40B6
		public virtual void SetHelpString(Control ctl, string helpString)
		{
			this.helpStrings[ctl] = helpString;
			if (helpString != null && helpString.Length > 0)
			{
				this.SetShowHelp(ctl, true);
			}
			this.UpdateEventBinding(ctl);
		}

		// Token: 0x06003EEE RID: 16110 RVA: 0x000E50E0 File Offset: 0x000E40E0
		public virtual void SetHelpKeyword(Control ctl, string keyword)
		{
			this.keywords[ctl] = keyword;
			if (keyword != null && keyword.Length > 0)
			{
				this.SetShowHelp(ctl, true);
			}
			this.UpdateEventBinding(ctl);
		}

		// Token: 0x06003EEF RID: 16111 RVA: 0x000E510C File Offset: 0x000E410C
		public virtual void SetHelpNavigator(Control ctl, HelpNavigator navigator)
		{
			if (!ClientUtils.IsEnumValid(navigator, (int)navigator, -2147483647, -2147483641))
			{
				throw new InvalidEnumArgumentException("navigator", (int)navigator, typeof(HelpNavigator));
			}
			this.navigators[ctl] = navigator;
			this.SetShowHelp(ctl, true);
			this.UpdateEventBinding(ctl);
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x000E5168 File Offset: 0x000E4168
		public virtual void SetShowHelp(Control ctl, bool value)
		{
			this.showHelp[ctl] = value;
			this.UpdateEventBinding(ctl);
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x000E5183 File Offset: 0x000E4183
		internal virtual bool ShouldSerializeShowHelp(Control ctl)
		{
			return this.showHelp.ContainsKey(ctl);
		}

		// Token: 0x06003EF2 RID: 16114 RVA: 0x000E5191 File Offset: 0x000E4191
		public virtual void ResetShowHelp(Control ctl)
		{
			this.showHelp.Remove(ctl);
		}

		// Token: 0x06003EF3 RID: 16115 RVA: 0x000E51A0 File Offset: 0x000E41A0
		private void UpdateEventBinding(Control ctl)
		{
			if (this.GetShowHelp(ctl) && !this.boundControls.ContainsKey(ctl))
			{
				ctl.HelpRequested += this.OnControlHelp;
				ctl.QueryAccessibilityHelp += this.OnQueryAccessibilityHelp;
				this.boundControls[ctl] = ctl;
				return;
			}
			if (!this.GetShowHelp(ctl) && this.boundControls.ContainsKey(ctl))
			{
				ctl.HelpRequested -= this.OnControlHelp;
				ctl.QueryAccessibilityHelp -= this.OnQueryAccessibilityHelp;
				this.boundControls.Remove(ctl);
			}
		}

		// Token: 0x06003EF4 RID: 16116 RVA: 0x000E5240 File Offset: 0x000E4240
		public override string ToString()
		{
			string text = base.ToString();
			return text + ", HelpNamespace: " + this.HelpNamespace;
		}

		// Token: 0x04001F05 RID: 7941
		private string helpNamespace;

		// Token: 0x04001F06 RID: 7942
		private Hashtable helpStrings = new Hashtable();

		// Token: 0x04001F07 RID: 7943
		private Hashtable showHelp = new Hashtable();

		// Token: 0x04001F08 RID: 7944
		private Hashtable boundControls = new Hashtable();

		// Token: 0x04001F09 RID: 7945
		private Hashtable keywords = new Hashtable();

		// Token: 0x04001F0A RID: 7946
		private Hashtable navigators = new Hashtable();

		// Token: 0x04001F0B RID: 7947
		private object userData;
	}
}
