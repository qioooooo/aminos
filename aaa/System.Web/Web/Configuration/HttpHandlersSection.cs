using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001FB RID: 507
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpHandlersSection : ConfigurationSection
	{
		// Token: 0x06001BA5 RID: 7077 RVA: 0x0007F9D9 File Offset: 0x0007E9D9
		static HttpHandlersSection()
		{
			HttpHandlersSection._properties.Add(HttpHandlersSection._propHandlers);
		}

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x0007FA13 File Offset: 0x0007EA13
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpHandlersSection._properties;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x0007FA1A File Offset: 0x0007EA1A
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public HttpHandlerActionCollection Handlers
		{
			get
			{
				return (HttpHandlerActionCollection)base[HttpHandlersSection._propHandlers];
			}
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0007FA2C File Offset: 0x0007EA2C
		internal bool ValidateHandlers()
		{
			if (!this._validated)
			{
				lock (this)
				{
					if (!this._validated)
					{
						foreach (object obj in this.Handlers)
						{
							HttpHandlerAction httpHandlerAction = (HttpHandlerAction)obj;
							httpHandlerAction.InitValidateInternal();
						}
						this._validated = true;
					}
				}
			}
			return this._validated;
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0007FAC0 File Offset: 0x0007EAC0
		internal HttpHandlerAction FindMapping(string verb, VirtualPath path)
		{
			this.ValidateHandlers();
			for (int i = 0; i < this.Handlers.Count; i++)
			{
				HttpHandlerAction httpHandlerAction = this.Handlers[i];
				if (httpHandlerAction.IsMatch(verb, path))
				{
					return httpHandlerAction;
				}
			}
			return null;
		}

		// Token: 0x0400187A RID: 6266
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400187B RID: 6267
		private static readonly ConfigurationProperty _propHandlers = new ConfigurationProperty(null, typeof(HttpHandlerActionCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x0400187C RID: 6268
		private bool _validated;
	}
}
