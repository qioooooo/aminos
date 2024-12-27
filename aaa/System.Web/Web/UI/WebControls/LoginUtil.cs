using System;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Security.Principal;
using System.Web.Security;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004E3 RID: 1251
	internal static class LoginUtil
	{
		// Token: 0x06003CD8 RID: 15576 RVA: 0x00100058 File Offset: 0x000FF058
		internal static void ApplyStyleToLiteral(Literal literal, string text, Style style, bool setTableCellVisible)
		{
			bool flag = false;
			if (!string.IsNullOrEmpty(text))
			{
				literal.Text = text;
				if (style != null)
				{
					LoginUtil.SetTableCellStyle(literal, style);
				}
				flag = true;
			}
			if (setTableCellVisible)
			{
				LoginUtil.SetTableCellVisible(literal, flag);
				return;
			}
			literal.Visible = flag;
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x00100094 File Offset: 0x000FF094
		internal static void CopyBorderStyles(WebControl control, Style style)
		{
			if (style == null || style.IsEmpty)
			{
				return;
			}
			control.BorderStyle = style.BorderStyle;
			control.BorderColor = style.BorderColor;
			control.BorderWidth = style.BorderWidth;
			control.BackColor = style.BackColor;
			control.CssClass = style.CssClass;
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x001000E9 File Offset: 0x000FF0E9
		internal static void CopyStyleToInnerControl(WebControl control, Style style)
		{
			if (style == null || style.IsEmpty)
			{
				return;
			}
			control.ForeColor = style.ForeColor;
			control.Font.CopyFrom(style.Font);
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x00100114 File Offset: 0x000FF114
		internal static Table CreateChildTable(bool convertingToTemplate)
		{
			if (convertingToTemplate)
			{
				return new Table();
			}
			return new ChildTable(2);
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x00100128 File Offset: 0x000FF128
		private static MailMessage CreateMailMessage(string email, string userName, string password, MailDefinition mailDefinition, string defaultBody, Control owner)
		{
			ListDictionary listDictionary = new ListDictionary();
			if (mailDefinition.IsBodyHtml)
			{
				userName = HttpUtility.HtmlEncode(userName);
				password = HttpUtility.HtmlEncode(password);
			}
			listDictionary.Add("<%\\s*UserName\\s*%>", userName);
			listDictionary.Add("<%\\s*Password\\s*%>", password);
			if (string.IsNullOrEmpty(mailDefinition.BodyFileName) && defaultBody != null)
			{
				return mailDefinition.CreateMailMessage(email, listDictionary, defaultBody, owner);
			}
			return mailDefinition.CreateMailMessage(email, listDictionary, owner);
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x00100194 File Offset: 0x000FF194
		internal static MembershipProvider GetProvider(string providerName)
		{
			MembershipProvider membershipProvider;
			if (string.IsNullOrEmpty(providerName))
			{
				membershipProvider = Membership.Provider;
			}
			else
			{
				membershipProvider = Membership.Providers[providerName];
				if (membershipProvider == null)
				{
					throw new HttpException(SR.GetString("WebControl_CantFindProvider"));
				}
			}
			return membershipProvider;
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x001001D4 File Offset: 0x000FF1D4
		internal static IPrincipal GetUser(Control c)
		{
			IPrincipal principal = null;
			Page page = c.Page;
			if (page != null)
			{
				principal = page.User;
			}
			else
			{
				HttpContext httpContext = HttpContext.Current;
				if (httpContext != null)
				{
					principal = httpContext.User;
				}
			}
			return principal;
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x00100208 File Offset: 0x000FF208
		internal static string GetUserName(Control c)
		{
			string text = null;
			IPrincipal user = LoginUtil.GetUser(c);
			if (user != null)
			{
				IIdentity identity = user.Identity;
				if (identity != null)
				{
					text = identity.Name;
				}
			}
			return text;
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x00100234 File Offset: 0x000FF234
		internal static void SendPasswordMail(string email, string userName, string password, MailDefinition mailDefinition, string defaultSubject, string defaultBody, LoginUtil.OnSendingMailDelegate onSendingMailDelegate, LoginUtil.OnSendMailErrorDelegate onSendMailErrorDelegate, Control owner)
		{
			try
			{
				new MailAddress(email);
			}
			catch (Exception ex)
			{
				onSendMailErrorDelegate(new SendMailErrorEventArgs(ex)
				{
					Handled = true
				});
				return;
			}
			try
			{
				using (MailMessage mailMessage = LoginUtil.CreateMailMessage(email, userName, password, mailDefinition, defaultBody, owner))
				{
					if (mailDefinition.SubjectInternal == null && defaultSubject != null)
					{
						mailMessage.Subject = defaultSubject;
					}
					MailMessageEventArgs mailMessageEventArgs = new MailMessageEventArgs(mailMessage);
					onSendingMailDelegate(mailMessageEventArgs);
					if (!mailMessageEventArgs.Cancel)
					{
						SmtpClient smtpClient = new SmtpClient();
						smtpClient.Send(mailMessage);
					}
				}
			}
			catch (Exception ex2)
			{
				SendMailErrorEventArgs sendMailErrorEventArgs = new SendMailErrorEventArgs(ex2);
				onSendMailErrorDelegate(sendMailErrorEventArgs);
				if (!sendMailErrorEventArgs.Handled)
				{
					throw;
				}
			}
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x00100304 File Offset: 0x000FF304
		internal static void SetTableCellStyle(Control control, Style style)
		{
			Control parent = control.Parent;
			if (parent != null)
			{
				((TableCell)parent).ApplyStyle(style);
			}
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x00100328 File Offset: 0x000FF328
		internal static void SetTableCellVisible(Control control, bool visible)
		{
			Control parent = control.Parent;
			if (parent != null)
			{
				parent.Visible = visible;
			}
		}

		// Token: 0x04002748 RID: 10056
		private const string _userNameReplacementKey = "<%\\s*UserName\\s*%>";

		// Token: 0x04002749 RID: 10057
		private const string _passwordReplacementKey = "<%\\s*Password\\s*%>";

		// Token: 0x0400274A RID: 10058
		private const string _templateDesignerRegion = "0";

		// Token: 0x020004E4 RID: 1252
		// (Invoke) Token: 0x06003CE4 RID: 15588
		internal delegate void OnSendingMailDelegate(MailMessageEventArgs e);

		// Token: 0x020004E5 RID: 1253
		// (Invoke) Token: 0x06003CE8 RID: 15592
		internal delegate void OnSendMailErrorDelegate(SendMailErrorEventArgs e);

		// Token: 0x020004E8 RID: 1256
		internal sealed class DisappearingTableRow : TableRow
		{
			// Token: 0x06003CF8 RID: 15608 RVA: 0x00100520 File Offset: 0x000FF520
			protected internal override void Render(HtmlTextWriter writer)
			{
				bool flag = false;
				foreach (object obj in this.Cells)
				{
					TableCell tableCell = (TableCell)obj;
					if (tableCell.Visible)
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					base.Render(writer);
				}
			}
		}

		// Token: 0x020004E9 RID: 1257
		internal abstract class GenericContainer<ControlType> : WebControl where ControlType : WebControl
		{
			// Token: 0x06003CFA RID: 15610 RVA: 0x00100594 File Offset: 0x000FF594
			public GenericContainer(ControlType owner)
			{
				this._owner = owner;
			}

			// Token: 0x17000E2F RID: 3631
			// (get) Token: 0x06003CFB RID: 15611 RVA: 0x001005A3 File Offset: 0x000FF5A3
			// (set) Token: 0x06003CFC RID: 15612 RVA: 0x001005AB File Offset: 0x000FF5AB
			internal Table BorderTable
			{
				get
				{
					return this._borderTable;
				}
				set
				{
					this._borderTable = value;
				}
			}

			// Token: 0x17000E30 RID: 3632
			// (get) Token: 0x06003CFD RID: 15613
			protected abstract bool ConvertingToTemplate { get; }

			// Token: 0x17000E31 RID: 3633
			// (get) Token: 0x06003CFE RID: 15614 RVA: 0x001005B4 File Offset: 0x000FF5B4
			// (set) Token: 0x06003CFF RID: 15615 RVA: 0x001005BC File Offset: 0x000FF5BC
			internal Table LayoutTable
			{
				get
				{
					return this._layoutTable;
				}
				set
				{
					this._layoutTable = value;
				}
			}

			// Token: 0x17000E32 RID: 3634
			// (get) Token: 0x06003D00 RID: 15616 RVA: 0x001005C5 File Offset: 0x000FF5C5
			internal ControlType Owner
			{
				get
				{
					return this._owner;
				}
			}

			// Token: 0x17000E33 RID: 3635
			// (get) Token: 0x06003D01 RID: 15617 RVA: 0x001005CD File Offset: 0x000FF5CD
			// (set) Token: 0x06003D02 RID: 15618 RVA: 0x001005DF File Offset: 0x000FF5DF
			internal bool RenderDesignerRegion
			{
				get
				{
					return base.DesignMode && this._renderDesignerRegion;
				}
				set
				{
					this._renderDesignerRegion = value;
				}
			}

			// Token: 0x17000E34 RID: 3636
			// (get) Token: 0x06003D03 RID: 15619 RVA: 0x001005E8 File Offset: 0x000FF5E8
			private bool UsingDefaultTemplate
			{
				get
				{
					return this.BorderTable != null;
				}
			}

			// Token: 0x06003D04 RID: 15620 RVA: 0x001005F8 File Offset: 0x000FF5F8
			public sealed override void Focus()
			{
				throw new NotSupportedException(SR.GetString("NoFocusSupport", new object[] { base.GetType().Name }));
			}

			// Token: 0x06003D05 RID: 15621 RVA: 0x0010062C File Offset: 0x000FF62C
			private Control FindControl<RequiredType>(string id, bool required, string errorResourceKey)
			{
				Control control = this.FindControl(id);
				if (control is RequiredType)
				{
					return control;
				}
				if (required)
				{
					ControlType owner = this.Owner;
					if (!owner.DesignMode)
					{
						object[] array = new object[2];
						object[] array2 = array;
						int num = 0;
						ControlType owner2 = this.Owner;
						array2[num] = owner2.ID;
						array[1] = id;
						throw new HttpException(SR.GetString(errorResourceKey, array));
					}
				}
				return null;
			}

			// Token: 0x06003D06 RID: 15622 RVA: 0x00100694 File Offset: 0x000FF694
			protected Control FindOptionalControl<RequiredType>(string id)
			{
				return this.FindControl<RequiredType>(id, false, null);
			}

			// Token: 0x06003D07 RID: 15623 RVA: 0x0010069F File Offset: 0x000FF69F
			protected Control FindRequiredControl<RequiredType>(string id, string errorResourceKey)
			{
				return this.FindControl<RequiredType>(id, true, errorResourceKey);
			}

			// Token: 0x06003D08 RID: 15624 RVA: 0x001006AC File Offset: 0x000FF6AC
			protected internal sealed override void Render(HtmlTextWriter writer)
			{
				if (this.UsingDefaultTemplate)
				{
					if (!this.ConvertingToTemplate)
					{
						this.BorderTable.CopyBaseAttributes(this);
						if (base.ControlStyleCreated)
						{
							LoginUtil.CopyBorderStyles(this.BorderTable, base.ControlStyle);
							LoginUtil.CopyStyleToInnerControl(this.LayoutTable, base.ControlStyle);
						}
					}
					this.LayoutTable.Height = this.Height;
					this.LayoutTable.Width = this.Width;
					this.RenderContents(writer);
					return;
				}
				this.RenderContentsInUnitTable(writer);
			}

			// Token: 0x06003D09 RID: 15625 RVA: 0x00100730 File Offset: 0x000FF730
			private void RenderContentsInUnitTable(HtmlTextWriter writer)
			{
				LayoutTable layoutTable = new LayoutTable(1, 1, this.Page);
				if (this.RenderDesignerRegion)
				{
					layoutTable[0, 0].Attributes["_designerRegion"] = "0";
				}
				else
				{
					foreach (object obj in this.Controls)
					{
						Control control = (Control)obj;
						layoutTable[0, 0].Controls.Add(control);
					}
				}
				string id = this.Parent.ID;
				if (id != null && id.Length != 0)
				{
					layoutTable.ID = this.Parent.ClientID;
				}
				layoutTable.CopyBaseAttributes(this);
				layoutTable.ApplyStyle(base.ControlStyle);
				layoutTable.CellPadding = 0;
				layoutTable.CellSpacing = 0;
				layoutTable.RenderControl(writer);
			}

			// Token: 0x06003D0A RID: 15626 RVA: 0x0010081C File Offset: 0x000FF81C
			protected void VerifyControlNotPresent<RequiredType>(string id, string errorResourceKey)
			{
				Control control = this.FindOptionalControl<RequiredType>(id);
				if (control != null)
				{
					ControlType owner = this.Owner;
					if (!owner.DesignMode)
					{
						object[] array = new object[2];
						object[] array2 = array;
						int num = 0;
						ControlType owner2 = this.Owner;
						array2[num] = owner2.ID;
						array[1] = id;
						throw new HttpException(SR.GetString(errorResourceKey, array));
					}
				}
			}

			// Token: 0x0400274C RID: 10060
			private bool _renderDesignerRegion;

			// Token: 0x0400274D RID: 10061
			private ControlType _owner;

			// Token: 0x0400274E RID: 10062
			private Table _layoutTable;

			// Token: 0x0400274F RID: 10063
			private Table _borderTable;
		}
	}
}
