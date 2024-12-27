using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001DC RID: 476
	internal class TraceSection : ConfigurationElement
	{
		// Token: 0x06000F0D RID: 3853 RVA: 0x0002FD80 File Offset: 0x0002ED80
		static TraceSection()
		{
			TraceSection._properties.Add(TraceSection._propListeners);
			TraceSection._properties.Add(TraceSection._propAutoFlush);
			TraceSection._properties.Add(TraceSection._propIndentSize);
			TraceSection._properties.Add(TraceSection._propUseGlobalLock);
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x0002FE52 File Offset: 0x0002EE52
		[ConfigurationProperty("autoflush", DefaultValue = false)]
		public bool AutoFlush
		{
			get
			{
				return (bool)base[TraceSection._propAutoFlush];
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0002FE64 File Offset: 0x0002EE64
		[ConfigurationProperty("indentsize", DefaultValue = 4)]
		public int IndentSize
		{
			get
			{
				return (int)base[TraceSection._propIndentSize];
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000F10 RID: 3856 RVA: 0x0002FE76 File Offset: 0x0002EE76
		[ConfigurationProperty("listeners")]
		public ListenerElementsCollection Listeners
		{
			get
			{
				return (ListenerElementsCollection)base[TraceSection._propListeners];
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0002FE88 File Offset: 0x0002EE88
		[ConfigurationProperty("useGlobalLock", DefaultValue = true)]
		public bool UseGlobalLock
		{
			get
			{
				return (bool)base[TraceSection._propUseGlobalLock];
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x0002FE9A File Offset: 0x0002EE9A
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TraceSection._properties;
			}
		}

		// Token: 0x04000F3D RID: 3901
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000F3E RID: 3902
		private static readonly ConfigurationProperty _propListeners = new ConfigurationProperty("listeners", typeof(ListenerElementsCollection), new ListenerElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x04000F3F RID: 3903
		private static readonly ConfigurationProperty _propAutoFlush = new ConfigurationProperty("autoflush", typeof(bool), false, ConfigurationPropertyOptions.None);

		// Token: 0x04000F40 RID: 3904
		private static readonly ConfigurationProperty _propIndentSize = new ConfigurationProperty("indentsize", typeof(int), 4, ConfigurationPropertyOptions.None);

		// Token: 0x04000F41 RID: 3905
		private static readonly ConfigurationProperty _propUseGlobalLock = new ConfigurationProperty("useGlobalLock", typeof(bool), true, ConfigurationPropertyOptions.None);
	}
}
