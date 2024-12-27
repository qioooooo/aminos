using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006DD RID: 1757
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public abstract class BaseChannelWithProperties : BaseChannelObjectWithProperties
	{
		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06003F29 RID: 16169 RVA: 0x000D84A8 File Offset: 0x000D74A8
		public override IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				ArrayList arrayList = new ArrayList();
				arrayList.Add(this);
				if (this.SinksWithProperties != null)
				{
					IServerChannelSink serverChannelSink = this.SinksWithProperties as IServerChannelSink;
					if (serverChannelSink != null)
					{
						while (serverChannelSink != null)
						{
							IDictionary properties = serverChannelSink.Properties;
							if (properties != null)
							{
								arrayList.Add(properties);
							}
							serverChannelSink = serverChannelSink.NextChannelSink;
						}
					}
					else
					{
						for (IClientChannelSink clientChannelSink = (IClientChannelSink)this.SinksWithProperties; clientChannelSink != null; clientChannelSink = clientChannelSink.NextChannelSink)
						{
							IDictionary properties2 = clientChannelSink.Properties;
							if (properties2 != null)
							{
								arrayList.Add(properties2);
							}
						}
					}
				}
				return new AggregateDictionary(arrayList);
			}
		}

		// Token: 0x04001FCE RID: 8142
		protected IChannelSinkBase SinksWithProperties;
	}
}
