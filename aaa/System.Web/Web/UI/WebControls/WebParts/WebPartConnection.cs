using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000710 RID: 1808
	[ParseChildren(true, "Transformers")]
	[TypeConverter(typeof(ExpandableObjectConverter))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebPartConnection
	{
		// Token: 0x060057F8 RID: 22520 RVA: 0x00162ACF File Offset: 0x00161ACF
		public WebPartConnection()
		{
			this._isStatic = true;
			this._isShared = true;
		}

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x060057F9 RID: 22521 RVA: 0x00162AE8 File Offset: 0x00161AE8
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public WebPart Consumer
		{
			get
			{
				string consumerID = this.ConsumerID;
				if (consumerID.Length == 0)
				{
					throw new InvalidOperationException(SR.GetString("WebPartConnection_ConsumerIDNotSet"));
				}
				if (this._webPartManager != null)
				{
					return this._webPartManager.WebParts[consumerID];
				}
				return null;
			}
		}

		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x060057FA RID: 22522 RVA: 0x00162B30 File Offset: 0x00161B30
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ConsumerConnectionPoint ConsumerConnectionPoint
		{
			get
			{
				WebPart consumer = this.Consumer;
				if (consumer != null && this._webPartManager != null)
				{
					return this._webPartManager.GetConsumerConnectionPoint(consumer, this.ConsumerConnectionPointID);
				}
				return null;
			}
		}

		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x060057FB RID: 22523 RVA: 0x00162B63 File Offset: 0x00161B63
		// (set) Token: 0x060057FC RID: 22524 RVA: 0x00162B7E File Offset: 0x00161B7E
		[DefaultValue("default")]
		public string ConsumerConnectionPointID
		{
			get
			{
				if (string.IsNullOrEmpty(this._consumerConnectionPointID))
				{
					return ConnectionPoint.DefaultID;
				}
				return this._consumerConnectionPointID;
			}
			set
			{
				this._consumerConnectionPointID = value;
			}
		}

		// Token: 0x170016B8 RID: 5816
		// (get) Token: 0x060057FD RID: 22525 RVA: 0x00162B87 File Offset: 0x00161B87
		// (set) Token: 0x060057FE RID: 22526 RVA: 0x00162B9D File Offset: 0x00161B9D
		[DefaultValue("")]
		public string ConsumerID
		{
			get
			{
				if (this._consumerID == null)
				{
					return string.Empty;
				}
				return this._consumerID;
			}
			set
			{
				this._consumerID = value;
			}
		}

		// Token: 0x170016B9 RID: 5817
		// (get) Token: 0x060057FF RID: 22527 RVA: 0x00162BA6 File Offset: 0x00161BA6
		// (set) Token: 0x06005800 RID: 22528 RVA: 0x00162BAE File Offset: 0x00161BAE
		internal bool Deleted
		{
			get
			{
				return this._deleted;
			}
			set
			{
				this._deleted = value;
			}
		}

		// Token: 0x170016BA RID: 5818
		// (get) Token: 0x06005801 RID: 22529 RVA: 0x00162BB7 File Offset: 0x00161BB7
		// (set) Token: 0x06005802 RID: 22530 RVA: 0x00162BCD File Offset: 0x00161BCD
		[DefaultValue("")]
		public string ID
		{
			get
			{
				if (this._id == null)
				{
					return string.Empty;
				}
				return this._id;
			}
			set
			{
				this._id = value;
			}
		}

		// Token: 0x170016BB RID: 5819
		// (get) Token: 0x06005803 RID: 22531 RVA: 0x00162BD6 File Offset: 0x00161BD6
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x170016BC RID: 5820
		// (get) Token: 0x06005804 RID: 22532 RVA: 0x00162BDE File Offset: 0x00161BDE
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsShared
		{
			get
			{
				return this._isShared;
			}
		}

		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x06005805 RID: 22533 RVA: 0x00162BE6 File Offset: 0x00161BE6
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool IsStatic
		{
			get
			{
				return this._isStatic;
			}
		}

		// Token: 0x170016BE RID: 5822
		// (get) Token: 0x06005806 RID: 22534 RVA: 0x00162BF0 File Offset: 0x00161BF0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public WebPart Provider
		{
			get
			{
				string providerID = this.ProviderID;
				if (providerID.Length == 0)
				{
					throw new InvalidOperationException(SR.GetString("WebPartConnection_ProviderIDNotSet"));
				}
				if (this._webPartManager != null)
				{
					return this._webPartManager.WebParts[providerID];
				}
				return null;
			}
		}

		// Token: 0x170016BF RID: 5823
		// (get) Token: 0x06005807 RID: 22535 RVA: 0x00162C38 File Offset: 0x00161C38
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public ProviderConnectionPoint ProviderConnectionPoint
		{
			get
			{
				WebPart provider = this.Provider;
				if (provider != null && this._webPartManager != null)
				{
					return this._webPartManager.GetProviderConnectionPoint(provider, this.ProviderConnectionPointID);
				}
				return null;
			}
		}

		// Token: 0x170016C0 RID: 5824
		// (get) Token: 0x06005808 RID: 22536 RVA: 0x00162C6B File Offset: 0x00161C6B
		// (set) Token: 0x06005809 RID: 22537 RVA: 0x00162C86 File Offset: 0x00161C86
		[DefaultValue("default")]
		public string ProviderConnectionPointID
		{
			get
			{
				if (string.IsNullOrEmpty(this._providerConnectionPointID))
				{
					return ConnectionPoint.DefaultID;
				}
				return this._providerConnectionPointID;
			}
			set
			{
				this._providerConnectionPointID = value;
			}
		}

		// Token: 0x170016C1 RID: 5825
		// (get) Token: 0x0600580A RID: 22538 RVA: 0x00162C8F File Offset: 0x00161C8F
		// (set) Token: 0x0600580B RID: 22539 RVA: 0x00162CA5 File Offset: 0x00161CA5
		[DefaultValue("")]
		public string ProviderID
		{
			get
			{
				if (this._providerID == null)
				{
					return string.Empty;
				}
				return this._providerID;
			}
			set
			{
				this._providerID = value;
			}
		}

		// Token: 0x170016C2 RID: 5826
		// (get) Token: 0x0600580C RID: 22540 RVA: 0x00162CAE File Offset: 0x00161CAE
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public WebPartTransformer Transformer
		{
			get
			{
				if (this._transformers == null || this._transformers.Count == 0)
				{
					return null;
				}
				return this._transformers[0];
			}
		}

		// Token: 0x170016C3 RID: 5827
		// (get) Token: 0x0600580D RID: 22541 RVA: 0x00162CD3 File Offset: 0x00161CD3
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		public WebPartTransformerCollection Transformers
		{
			get
			{
				if (this._transformers == null)
				{
					this._transformers = new WebPartTransformerCollection();
				}
				return this._transformers;
			}
		}

		// Token: 0x0600580E RID: 22542 RVA: 0x00162CF0 File Offset: 0x00161CF0
		internal void Activate()
		{
			this.Transformers.SetReadOnly();
			WebPart provider = this.Provider;
			WebPart consumer = this.Consumer;
			Control control = provider.ToControl();
			Control control2 = consumer.ToControl();
			ProviderConnectionPoint providerConnectionPoint = this.ProviderConnectionPoint;
			if (!providerConnectionPoint.GetEnabled(control))
			{
				consumer.SetConnectErrorMessage(SR.GetString("WebPartConnection_DisabledConnectionPoint", new object[] { providerConnectionPoint.DisplayName, provider.DisplayTitle }));
				return;
			}
			ConsumerConnectionPoint consumerConnectionPoint = this.ConsumerConnectionPoint;
			if (!consumerConnectionPoint.GetEnabled(control2))
			{
				consumer.SetConnectErrorMessage(SR.GetString("WebPartConnection_DisabledConnectionPoint", new object[] { consumerConnectionPoint.DisplayName, consumer.DisplayTitle }));
				return;
			}
			if (!provider.IsClosed && !consumer.IsClosed)
			{
				WebPartTransformer transformer = this.Transformer;
				if (transformer == null)
				{
					if (providerConnectionPoint.InterfaceType != consumerConnectionPoint.InterfaceType)
					{
						consumer.SetConnectErrorMessage(SR.GetString("WebPartConnection_NoCommonInterface", new string[] { providerConnectionPoint.DisplayName, provider.DisplayTitle, consumerConnectionPoint.DisplayName, consumer.DisplayTitle }));
						return;
					}
					ConnectionInterfaceCollection secondaryInterfaces = providerConnectionPoint.GetSecondaryInterfaces(control);
					if (consumerConnectionPoint.SupportsConnection(control2, secondaryInterfaces))
					{
						object @object = providerConnectionPoint.GetObject(control);
						consumerConnectionPoint.SetObject(control2, @object);
						this._isActive = true;
						return;
					}
					consumer.SetConnectErrorMessage(SR.GetString("WebPartConnection_IncompatibleSecondaryInterfaces", new string[] { consumerConnectionPoint.DisplayName, consumer.DisplayTitle, providerConnectionPoint.DisplayName, provider.DisplayTitle }));
					return;
				}
				else
				{
					Type type = transformer.GetType();
					if (!this._webPartManager.AvailableTransformers.Contains(type))
					{
						string text;
						if (this._webPartManager.Context != null && this._webPartManager.Context.IsCustomErrorEnabled)
						{
							text = SR.GetString("WebPartConnection_TransformerNotAvailable");
						}
						else
						{
							text = SR.GetString("WebPartConnection_TransformerNotAvailableWithType", new object[] { type.FullName });
						}
						consumer.SetConnectErrorMessage(text);
					}
					Type consumerType = WebPartTransformerAttribute.GetConsumerType(type);
					Type providerType = WebPartTransformerAttribute.GetProviderType(type);
					if (providerConnectionPoint.InterfaceType == consumerType && providerType == consumerConnectionPoint.InterfaceType)
					{
						if (consumerConnectionPoint.SupportsConnection(control2, ConnectionInterfaceCollection.Empty))
						{
							object object2 = providerConnectionPoint.GetObject(control);
							object obj = transformer.Transform(object2);
							consumerConnectionPoint.SetObject(control2, obj);
							this._isActive = true;
							return;
						}
						consumer.SetConnectErrorMessage(SR.GetString("WebPartConnection_ConsumerRequiresSecondaryInterfaces", new object[] { consumerConnectionPoint.DisplayName, consumer.DisplayTitle }));
						return;
					}
					else
					{
						if (providerConnectionPoint.InterfaceType != consumerType)
						{
							string text2;
							if (this._webPartManager.Context != null && this._webPartManager.Context.IsCustomErrorEnabled)
							{
								text2 = SR.GetString("WebPartConnection_IncompatibleProviderTransformer", new object[] { providerConnectionPoint.DisplayName, provider.DisplayTitle });
							}
							else
							{
								text2 = SR.GetString("WebPartConnection_IncompatibleProviderTransformerWithType", new object[] { providerConnectionPoint.DisplayName, provider.DisplayTitle, type.FullName });
							}
							consumer.SetConnectErrorMessage(text2);
							return;
						}
						string text3;
						if (this._webPartManager.Context != null && this._webPartManager.Context.IsCustomErrorEnabled)
						{
							text3 = SR.GetString("WebPartConnection_IncompatibleConsumerTransformer", new object[] { consumerConnectionPoint.DisplayName, consumer.DisplayTitle });
						}
						else
						{
							text3 = SR.GetString("WebPartConnection_IncompatibleConsumerTransformerWithType", new object[] { type.FullName, consumerConnectionPoint.DisplayName, consumer.DisplayTitle });
						}
						consumer.SetConnectErrorMessage(text3);
					}
				}
			}
		}

		// Token: 0x0600580F RID: 22543 RVA: 0x001630BD File Offset: 0x001620BD
		internal bool ConflictsWith(WebPartConnection otherConnection)
		{
			return this.ConflictsWithConsumer(otherConnection) || this.ConflictsWithProvider(otherConnection);
		}

		// Token: 0x06005810 RID: 22544 RVA: 0x001630D1 File Offset: 0x001620D1
		internal bool ConflictsWithConsumer(WebPartConnection otherConnection)
		{
			return !this.ConsumerConnectionPoint.AllowsMultipleConnections && this.Consumer == otherConnection.Consumer && this.ConsumerConnectionPoint == otherConnection.ConsumerConnectionPoint;
		}

		// Token: 0x06005811 RID: 22545 RVA: 0x001630FE File Offset: 0x001620FE
		internal bool ConflictsWithProvider(WebPartConnection otherConnection)
		{
			return !this.ProviderConnectionPoint.AllowsMultipleConnections && this.Provider == otherConnection.Provider && this.ProviderConnectionPoint == otherConnection.ProviderConnectionPoint;
		}

		// Token: 0x06005812 RID: 22546 RVA: 0x0016312B File Offset: 0x0016212B
		internal void SetIsShared(bool isShared)
		{
			this._isShared = isShared;
		}

		// Token: 0x06005813 RID: 22547 RVA: 0x00163134 File Offset: 0x00162134
		internal void SetIsStatic(bool isStatic)
		{
			this._isStatic = isStatic;
		}

		// Token: 0x06005814 RID: 22548 RVA: 0x0016313D File Offset: 0x0016213D
		internal void SetTransformer(WebPartTransformer transformer)
		{
			if (this.Transformers.Count == 0)
			{
				this.Transformers.Add(transformer);
				return;
			}
			this.Transformers[0] = transformer;
		}

		// Token: 0x06005815 RID: 22549 RVA: 0x00163167 File Offset: 0x00162167
		internal void SetWebPartManager(WebPartManager webPartManager)
		{
			this._webPartManager = webPartManager;
		}

		// Token: 0x06005816 RID: 22550 RVA: 0x00163170 File Offset: 0x00162170
		public override string ToString()
		{
			return base.GetType().Name;
		}

		// Token: 0x04002FC5 RID: 12229
		private string _consumerConnectionPointID;

		// Token: 0x04002FC6 RID: 12230
		private string _consumerID;

		// Token: 0x04002FC7 RID: 12231
		private bool _deleted;

		// Token: 0x04002FC8 RID: 12232
		private string _id;

		// Token: 0x04002FC9 RID: 12233
		private bool _isActive;

		// Token: 0x04002FCA RID: 12234
		private bool _isShared;

		// Token: 0x04002FCB RID: 12235
		private bool _isStatic;

		// Token: 0x04002FCC RID: 12236
		private string _providerConnectionPointID;

		// Token: 0x04002FCD RID: 12237
		private string _providerID;

		// Token: 0x04002FCE RID: 12238
		private WebPartTransformerCollection _transformers;

		// Token: 0x04002FCF RID: 12239
		private WebPartManager _webPartManager;
	}
}
