using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200045A RID: 1114
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PostBackOptions
	{
		// Token: 0x060034D0 RID: 13520 RVA: 0x000E4BD0 File Offset: 0x000E3BD0
		public PostBackOptions(Control targetControl)
			: this(targetControl, null, null, false, false, false, true, false, null)
		{
		}

		// Token: 0x060034D1 RID: 13521 RVA: 0x000E4BEC File Offset: 0x000E3BEC
		public PostBackOptions(Control targetControl, string argument)
			: this(targetControl, argument, null, false, false, false, true, false, null)
		{
		}

		// Token: 0x060034D2 RID: 13522 RVA: 0x000E4C08 File Offset: 0x000E3C08
		public PostBackOptions(Control targetControl, string argument, string actionUrl, bool autoPostBack, bool requiresJavaScriptProtocol, bool trackFocus, bool clientSubmit, bool performValidation, string validationGroup)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}
			this._actionUrl = actionUrl;
			this._argument = argument;
			this._autoPostBack = autoPostBack;
			this._clientSubmit = clientSubmit;
			this._requiresJavaScriptProtocol = requiresJavaScriptProtocol;
			this._performValidation = performValidation;
			this._trackFocus = trackFocus;
			this._targetControl = targetControl;
			this._validationGroup = validationGroup;
		}

		// Token: 0x17000BCB RID: 3019
		// (get) Token: 0x060034D3 RID: 13523 RVA: 0x000E4C75 File Offset: 0x000E3C75
		// (set) Token: 0x060034D4 RID: 13524 RVA: 0x000E4C7D File Offset: 0x000E3C7D
		[DefaultValue("")]
		public string ActionUrl
		{
			get
			{
				return this._actionUrl;
			}
			set
			{
				this._actionUrl = value;
			}
		}

		// Token: 0x17000BCC RID: 3020
		// (get) Token: 0x060034D5 RID: 13525 RVA: 0x000E4C86 File Offset: 0x000E3C86
		// (set) Token: 0x060034D6 RID: 13526 RVA: 0x000E4C8E File Offset: 0x000E3C8E
		[DefaultValue("")]
		public string Argument
		{
			get
			{
				return this._argument;
			}
			set
			{
				this._argument = value;
			}
		}

		// Token: 0x17000BCD RID: 3021
		// (get) Token: 0x060034D7 RID: 13527 RVA: 0x000E4C97 File Offset: 0x000E3C97
		// (set) Token: 0x060034D8 RID: 13528 RVA: 0x000E4C9F File Offset: 0x000E3C9F
		[DefaultValue(false)]
		public bool AutoPostBack
		{
			get
			{
				return this._autoPostBack;
			}
			set
			{
				this._autoPostBack = value;
			}
		}

		// Token: 0x17000BCE RID: 3022
		// (get) Token: 0x060034D9 RID: 13529 RVA: 0x000E4CA8 File Offset: 0x000E3CA8
		// (set) Token: 0x060034DA RID: 13530 RVA: 0x000E4CB0 File Offset: 0x000E3CB0
		[DefaultValue(true)]
		public bool ClientSubmit
		{
			get
			{
				return this._clientSubmit;
			}
			set
			{
				this._clientSubmit = value;
			}
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x060034DB RID: 13531 RVA: 0x000E4CB9 File Offset: 0x000E3CB9
		// (set) Token: 0x060034DC RID: 13532 RVA: 0x000E4CC1 File Offset: 0x000E3CC1
		[DefaultValue(true)]
		public bool RequiresJavaScriptProtocol
		{
			get
			{
				return this._requiresJavaScriptProtocol;
			}
			set
			{
				this._requiresJavaScriptProtocol = value;
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x060034DD RID: 13533 RVA: 0x000E4CCA File Offset: 0x000E3CCA
		// (set) Token: 0x060034DE RID: 13534 RVA: 0x000E4CD2 File Offset: 0x000E3CD2
		[DefaultValue(false)]
		public bool PerformValidation
		{
			get
			{
				return this._performValidation;
			}
			set
			{
				this._performValidation = value;
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x060034DF RID: 13535 RVA: 0x000E4CDB File Offset: 0x000E3CDB
		// (set) Token: 0x060034E0 RID: 13536 RVA: 0x000E4CE3 File Offset: 0x000E3CE3
		[DefaultValue("")]
		public string ValidationGroup
		{
			get
			{
				return this._validationGroup;
			}
			set
			{
				this._validationGroup = value;
			}
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x060034E1 RID: 13537 RVA: 0x000E4CEC File Offset: 0x000E3CEC
		[DefaultValue(null)]
		public Control TargetControl
		{
			get
			{
				return this._targetControl;
			}
		}

		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x060034E2 RID: 13538 RVA: 0x000E4CF4 File Offset: 0x000E3CF4
		// (set) Token: 0x060034E3 RID: 13539 RVA: 0x000E4CFC File Offset: 0x000E3CFC
		[DefaultValue(false)]
		public bool TrackFocus
		{
			get
			{
				return this._trackFocus;
			}
			set
			{
				this._trackFocus = value;
			}
		}

		// Token: 0x040024FC RID: 9468
		private string _actionUrl;

		// Token: 0x040024FD RID: 9469
		private string _argument;

		// Token: 0x040024FE RID: 9470
		private string _validationGroup;

		// Token: 0x040024FF RID: 9471
		private bool _autoPostBack;

		// Token: 0x04002500 RID: 9472
		private bool _requiresJavaScriptProtocol;

		// Token: 0x04002501 RID: 9473
		private bool _performValidation;

		// Token: 0x04002502 RID: 9474
		private bool _trackFocus;

		// Token: 0x04002503 RID: 9475
		private bool _clientSubmit = true;

		// Token: 0x04002504 RID: 9476
		private Control _targetControl;
	}
}
