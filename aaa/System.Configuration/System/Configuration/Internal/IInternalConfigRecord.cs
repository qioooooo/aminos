using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Internal
{
	// Token: 0x0200000F RID: 15
	[ComVisible(false)]
	public interface IInternalConfigRecord
	{
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000072 RID: 114
		string ConfigPath { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000073 RID: 115
		string StreamName { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000074 RID: 116
		bool HasInitErrors { get; }

		// Token: 0x06000075 RID: 117
		void ThrowIfInitErrors();

		// Token: 0x06000076 RID: 118
		object GetSection(string configKey);

		// Token: 0x06000077 RID: 119
		object GetLkgSection(string configKey);

		// Token: 0x06000078 RID: 120
		void RefreshSection(string configKey);

		// Token: 0x06000079 RID: 121
		void Remove();
	}
}
