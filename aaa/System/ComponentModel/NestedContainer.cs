using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000122 RID: 290
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class NestedContainer : Container, INestedContainer, IContainer, IDisposable
	{
		// Token: 0x0600093D RID: 2365 RVA: 0x0001F0F7 File Offset: 0x0001E0F7
		public NestedContainer(IComponent owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
			this._owner.Disposed += this.OnOwnerDisposed;
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x0001F12B File Offset: 0x0001E12B
		public IComponent Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0001F134 File Offset: 0x0001E134
		protected virtual string OwnerName
		{
			get
			{
				string text = null;
				if (this._owner != null && this._owner.Site != null)
				{
					INestedSite nestedSite = this._owner.Site as INestedSite;
					if (nestedSite != null)
					{
						text = nestedSite.FullName;
					}
					else
					{
						text = this._owner.Site.Name;
					}
				}
				return text;
			}
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x0001F187 File Offset: 0x0001E187
		protected override ISite CreateSite(IComponent component, string name)
		{
			if (component == null)
			{
				throw new ArgumentNullException("component");
			}
			return new NestedContainer.Site(component, this, name);
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x0001F19F File Offset: 0x0001E19F
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this._owner.Disposed -= this.OnOwnerDisposed;
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x0001F1C2 File Offset: 0x0001E1C2
		protected override object GetService(Type service)
		{
			if (service == typeof(INestedContainer))
			{
				return this;
			}
			return base.GetService(service);
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x0001F1DA File Offset: 0x0001E1DA
		private void OnOwnerDisposed(object sender, EventArgs e)
		{
			base.Dispose();
		}

		// Token: 0x04000A01 RID: 2561
		private IComponent _owner;

		// Token: 0x02000123 RID: 291
		private class Site : INestedSite, ISite, IServiceProvider
		{
			// Token: 0x06000944 RID: 2372 RVA: 0x0001F1E2 File Offset: 0x0001E1E2
			internal Site(IComponent component, NestedContainer container, string name)
			{
				this.component = component;
				this.container = container;
				this.name = name;
			}

			// Token: 0x170001EA RID: 490
			// (get) Token: 0x06000945 RID: 2373 RVA: 0x0001F1FF File Offset: 0x0001E1FF
			public IComponent Component
			{
				get
				{
					return this.component;
				}
			}

			// Token: 0x170001EB RID: 491
			// (get) Token: 0x06000946 RID: 2374 RVA: 0x0001F207 File Offset: 0x0001E207
			public IContainer Container
			{
				get
				{
					return this.container;
				}
			}

			// Token: 0x06000947 RID: 2375 RVA: 0x0001F20F File Offset: 0x0001E20F
			public object GetService(Type service)
			{
				if (service != typeof(ISite))
				{
					return this.container.GetService(service);
				}
				return this;
			}

			// Token: 0x170001EC RID: 492
			// (get) Token: 0x06000948 RID: 2376 RVA: 0x0001F22C File Offset: 0x0001E22C
			public bool DesignMode
			{
				get
				{
					IComponent owner = this.container.Owner;
					return owner != null && owner.Site != null && owner.Site.DesignMode;
				}
			}

			// Token: 0x170001ED RID: 493
			// (get) Token: 0x06000949 RID: 2377 RVA: 0x0001F260 File Offset: 0x0001E260
			public string FullName
			{
				get
				{
					if (this.name != null)
					{
						string ownerName = this.container.OwnerName;
						string text = this.name;
						if (ownerName != null)
						{
							text = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[] { ownerName, text });
						}
						return text;
					}
					return this.name;
				}
			}

			// Token: 0x170001EE RID: 494
			// (get) Token: 0x0600094A RID: 2378 RVA: 0x0001F2B3 File Offset: 0x0001E2B3
			// (set) Token: 0x0600094B RID: 2379 RVA: 0x0001F2BB File Offset: 0x0001E2BB
			public string Name
			{
				get
				{
					return this.name;
				}
				set
				{
					if (value == null || this.name == null || !value.Equals(this.name))
					{
						this.container.ValidateName(this.component, value);
						this.name = value;
					}
				}
			}

			// Token: 0x04000A02 RID: 2562
			private IComponent component;

			// Token: 0x04000A03 RID: 2563
			private NestedContainer container;

			// Token: 0x04000A04 RID: 2564
			private string name;
		}
	}
}
