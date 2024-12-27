using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Net.Configuration;
using System.Net.Mail;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005DC RID: 1500
	[Bindable(false)]
	[ParseChildren(true, "")]
	[TypeConverter(typeof(EmptyStringExpandableObjectConverter))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class MailDefinition : IStateManager
	{
		// Token: 0x17001235 RID: 4661
		// (get) Token: 0x06004978 RID: 18808 RVA: 0x0012B2F1 File Offset: 0x0012A2F1
		// (set) Token: 0x06004979 RID: 18809 RVA: 0x0012B307 File Offset: 0x0012A307
		[WebCategory("Behavior")]
		[UrlProperty("*.*")]
		[NotifyParentProperty(true)]
		[DefaultValue("")]
		[WebSysDescription("MailDefinition_BodyFileName")]
		[Editor("System.Web.UI.Design.WebControls.MailDefinitionBodyFileNameEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		public string BodyFileName
		{
			get
			{
				if (this._bodyFileName != null)
				{
					return this._bodyFileName;
				}
				return string.Empty;
			}
			set
			{
				this._bodyFileName = value;
			}
		}

		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x0600497A RID: 18810 RVA: 0x0012B310 File Offset: 0x0012A310
		// (set) Token: 0x0600497B RID: 18811 RVA: 0x0012B33D File Offset: 0x0012A33D
		[WebSysDescription("MailDefinition_CC")]
		[WebCategory("Behavior")]
		[NotifyParentProperty(true)]
		[DefaultValue("")]
		public string CC
		{
			get
			{
				object obj = this.ViewState["CC"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["CC"] = value;
			}
		}

		// Token: 0x17001237 RID: 4663
		// (get) Token: 0x0600497C RID: 18812 RVA: 0x0012B350 File Offset: 0x0012A350
		// (set) Token: 0x0600497D RID: 18813 RVA: 0x0012B37D File Offset: 0x0012A37D
		[DefaultValue("")]
		[NotifyParentProperty(true)]
		[WebSysDescription("MailDefinition_From")]
		[WebCategory("Behavior")]
		public string From
		{
			get
			{
				object obj = this.ViewState["From"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["From"] = value;
			}
		}

		// Token: 0x17001238 RID: 4664
		// (get) Token: 0x0600497E RID: 18814 RVA: 0x0012B390 File Offset: 0x0012A390
		[WebSysDescription("MailDefinition_EmbeddedObjects")]
		[WebCategory("Behavior")]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[DefaultValue(null)]
		[NotifyParentProperty(true)]
		public EmbeddedMailObjectsCollection EmbeddedObjects
		{
			get
			{
				if (this._embeddedObjects == null)
				{
					this._embeddedObjects = new EmbeddedMailObjectsCollection();
				}
				return this._embeddedObjects;
			}
		}

		// Token: 0x17001239 RID: 4665
		// (get) Token: 0x0600497F RID: 18815 RVA: 0x0012B3AC File Offset: 0x0012A3AC
		// (set) Token: 0x06004980 RID: 18816 RVA: 0x0012B3D5 File Offset: 0x0012A3D5
		[WebCategory("Behavior")]
		[DefaultValue(false)]
		[WebSysDescription("MailDefinition_IsBodyHtml")]
		[NotifyParentProperty(true)]
		public bool IsBodyHtml
		{
			get
			{
				object obj = this.ViewState["IsBodyHtml"];
				return obj != null && (bool)obj;
			}
			set
			{
				this.ViewState["IsBodyHtml"] = value;
			}
		}

		// Token: 0x1700123A RID: 4666
		// (get) Token: 0x06004981 RID: 18817 RVA: 0x0012B3F0 File Offset: 0x0012A3F0
		// (set) Token: 0x06004982 RID: 18818 RVA: 0x0012B419 File Offset: 0x0012A419
		[DefaultValue(MailPriority.Normal)]
		[NotifyParentProperty(true)]
		[WebCategory("Behavior")]
		[WebSysDescription("MailDefinition_Priority")]
		public MailPriority Priority
		{
			get
			{
				object obj = this.ViewState["Priority"];
				if (obj != null)
				{
					return (MailPriority)obj;
				}
				return MailPriority.Normal;
			}
			set
			{
				if (value < MailPriority.Normal || value > MailPriority.High)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.ViewState["Priority"] = value;
			}
		}

		// Token: 0x1700123B RID: 4667
		// (get) Token: 0x06004983 RID: 18819 RVA: 0x0012B444 File Offset: 0x0012A444
		// (set) Token: 0x06004984 RID: 18820 RVA: 0x0012B471 File Offset: 0x0012A471
		[WebCategory("Behavior")]
		[DefaultValue("")]
		[WebSysDescription("MailDefinition_Subject")]
		[NotifyParentProperty(true)]
		public string Subject
		{
			get
			{
				object obj = this.ViewState["Subject"];
				if (obj != null)
				{
					return (string)obj;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["Subject"] = value;
			}
		}

		// Token: 0x1700123C RID: 4668
		// (get) Token: 0x06004985 RID: 18821 RVA: 0x0012B484 File Offset: 0x0012A484
		internal string SubjectInternal
		{
			get
			{
				return (string)this.ViewState["Subject"];
			}
		}

		// Token: 0x1700123D RID: 4669
		// (get) Token: 0x06004986 RID: 18822 RVA: 0x0012B49B File Offset: 0x0012A49B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		private StateBag ViewState
		{
			get
			{
				if (this._viewState == null)
				{
					this._viewState = new StateBag(false);
					if (this._isTrackingViewState)
					{
						((IStateManager)this._viewState).TrackViewState();
					}
				}
				return this._viewState;
			}
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x0012B4CC File Offset: 0x0012A4CC
		public MailMessage CreateMailMessage(string recipients, IDictionary replacements, Control owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			string text = string.Empty;
			string bodyFileName = this.BodyFileName;
			if (!string.IsNullOrEmpty(bodyFileName))
			{
				string text2 = bodyFileName;
				if (!UrlPath.IsAbsolutePhysicalPath(text2))
				{
					text2 = UrlPath.Combine(owner.AppRelativeTemplateSourceDirectory, text2);
				}
				TextReader textReader = new StreamReader(owner.OpenFile(text2));
				try
				{
					text = textReader.ReadToEnd();
				}
				finally
				{
					textReader.Close();
				}
			}
			return this.CreateMailMessage(recipients, replacements, text, owner);
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x0012B54C File Offset: 0x0012A54C
		public MailMessage CreateMailMessage(string recipients, IDictionary replacements, string body, Control owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			string text = this.From;
			if (string.IsNullOrEmpty(text))
			{
				SmtpSection smtp = RuntimeConfig.GetConfig().Smtp;
				if (smtp == null || smtp.Network == null || string.IsNullOrEmpty(smtp.From))
				{
					throw new HttpException(SR.GetString("MailDefinition_NoFromAddressSpecified"));
				}
				text = smtp.From;
			}
			MailMessage mailMessage = null;
			MailMessage mailMessage2;
			try
			{
				mailMessage = new MailMessage(text, recipients);
				if (!string.IsNullOrEmpty(this.CC))
				{
					mailMessage.CC.Add(this.CC);
				}
				if (!string.IsNullOrEmpty(this.Subject))
				{
					mailMessage.Subject = this.Subject;
				}
				mailMessage.Priority = this.Priority;
				if (replacements != null && !string.IsNullOrEmpty(body))
				{
					foreach (object obj in replacements.Keys)
					{
						string text2 = obj as string;
						string text3 = replacements[obj] as string;
						if (text2 == null || text3 == null)
						{
							throw new ArgumentException(SR.GetString("MailDefinition_InvalidReplacements"));
						}
						text3 = text3.Replace("$", "$$");
						body = Regex.Replace(body, text2, text3, RegexOptions.IgnoreCase);
					}
				}
				if (this.EmbeddedObjects.Count > 0)
				{
					string text4 = (this.IsBodyHtml ? "text/html" : "text/plain");
					AlternateView alternateView = AlternateView.CreateAlternateViewFromString(body, null, text4);
					foreach (object obj2 in this.EmbeddedObjects)
					{
						EmbeddedMailObject embeddedMailObject = (EmbeddedMailObject)obj2;
						string text5 = embeddedMailObject.Path;
						if (string.IsNullOrEmpty(text5))
						{
							throw ExceptionUtil.PropertyNullOrEmpty("EmbeddedMailObject.Path");
						}
						if (!UrlPath.IsAbsolutePhysicalPath(text5))
						{
							VirtualPath virtualPath = VirtualPath.Combine(owner.TemplateControlVirtualDirectory, VirtualPath.Create(text5));
							text5 = virtualPath.AppRelativeVirtualPathString;
						}
						LinkedResource linkedResource = null;
						try
						{
							Stream stream = null;
							try
							{
								stream = owner.OpenFile(text5);
								linkedResource = new LinkedResource(stream);
							}
							catch
							{
								if (stream != null)
								{
									((IDisposable)stream).Dispose();
								}
								throw;
							}
							linkedResource.ContentId = embeddedMailObject.Name;
							linkedResource.ContentType.Name = UrlPath.GetFileName(text5);
							alternateView.LinkedResources.Add(linkedResource);
						}
						catch
						{
							if (linkedResource != null)
							{
								linkedResource.Dispose();
							}
							throw;
						}
					}
					mailMessage.AlternateViews.Add(alternateView);
				}
				else if (!string.IsNullOrEmpty(body))
				{
					mailMessage.Body = body;
				}
				mailMessage.IsBodyHtml = this.IsBodyHtml;
				mailMessage2 = mailMessage;
			}
			catch
			{
				if (mailMessage != null)
				{
					mailMessage.Dispose();
				}
				throw;
			}
			return mailMessage2;
		}

		// Token: 0x1700123E RID: 4670
		// (get) Token: 0x06004989 RID: 18825 RVA: 0x0012B870 File Offset: 0x0012A870
		bool IStateManager.IsTrackingViewState
		{
			get
			{
				return this._isTrackingViewState;
			}
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x0012B878 File Offset: 0x0012A878
		void IStateManager.LoadViewState(object savedState)
		{
			if (savedState != null)
			{
				((IStateManager)this.ViewState).LoadViewState(savedState);
			}
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x0012B889 File Offset: 0x0012A889
		object IStateManager.SaveViewState()
		{
			if (this._viewState != null)
			{
				return ((IStateManager)this._viewState).SaveViewState();
			}
			return null;
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x0012B8A0 File Offset: 0x0012A8A0
		void IStateManager.TrackViewState()
		{
			this._isTrackingViewState = true;
			if (this._viewState != null)
			{
				((IStateManager)this._viewState).TrackViewState();
			}
		}

		// Token: 0x04002B35 RID: 11061
		private bool _isTrackingViewState;

		// Token: 0x04002B36 RID: 11062
		private StateBag _viewState;

		// Token: 0x04002B37 RID: 11063
		private EmbeddedMailObjectsCollection _embeddedObjects;

		// Token: 0x04002B38 RID: 11064
		private string _bodyFileName;
	}
}
