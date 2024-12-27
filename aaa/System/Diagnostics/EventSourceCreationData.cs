using System;

namespace System.Diagnostics
{
	// Token: 0x0200075B RID: 1883
	public class EventSourceCreationData
	{
		// Token: 0x060039B0 RID: 14768 RVA: 0x000F4942 File Offset: 0x000F3942
		private EventSourceCreationData()
		{
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x000F4960 File Offset: 0x000F3960
		public EventSourceCreationData(string source, string logName)
		{
			this._source = source;
			this._logName = logName;
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x000F498C File Offset: 0x000F398C
		internal EventSourceCreationData(string source, string logName, string machineName)
		{
			this._source = source;
			this._logName = logName;
			this._machineName = machineName;
		}

		// Token: 0x060039B3 RID: 14771 RVA: 0x000F49C0 File Offset: 0x000F39C0
		private EventSourceCreationData(string source, string logName, string machineName, string messageResourceFile, string parameterResourceFile, string categoryResourceFile, short categoryCount)
		{
			this._source = source;
			this._logName = logName;
			this._machineName = machineName;
			this._messageResourceFile = messageResourceFile;
			this._parameterResourceFile = parameterResourceFile;
			this._categoryResourceFile = categoryResourceFile;
			this.CategoryCount = (int)categoryCount;
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x060039B4 RID: 14772 RVA: 0x000F4A1E File Offset: 0x000F3A1E
		// (set) Token: 0x060039B5 RID: 14773 RVA: 0x000F4A26 File Offset: 0x000F3A26
		public string LogName
		{
			get
			{
				return this._logName;
			}
			set
			{
				this._logName = value;
			}
		}

		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x060039B6 RID: 14774 RVA: 0x000F4A2F File Offset: 0x000F3A2F
		// (set) Token: 0x060039B7 RID: 14775 RVA: 0x000F4A37 File Offset: 0x000F3A37
		public string MachineName
		{
			get
			{
				return this._machineName;
			}
			set
			{
				this._machineName = value;
			}
		}

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x000F4A40 File Offset: 0x000F3A40
		// (set) Token: 0x060039B9 RID: 14777 RVA: 0x000F4A48 File Offset: 0x000F3A48
		public string Source
		{
			get
			{
				return this._source;
			}
			set
			{
				this._source = value;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x060039BA RID: 14778 RVA: 0x000F4A51 File Offset: 0x000F3A51
		// (set) Token: 0x060039BB RID: 14779 RVA: 0x000F4A59 File Offset: 0x000F3A59
		public string MessageResourceFile
		{
			get
			{
				return this._messageResourceFile;
			}
			set
			{
				this._messageResourceFile = value;
			}
		}

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x060039BC RID: 14780 RVA: 0x000F4A62 File Offset: 0x000F3A62
		// (set) Token: 0x060039BD RID: 14781 RVA: 0x000F4A6A File Offset: 0x000F3A6A
		public string ParameterResourceFile
		{
			get
			{
				return this._parameterResourceFile;
			}
			set
			{
				this._parameterResourceFile = value;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x060039BE RID: 14782 RVA: 0x000F4A73 File Offset: 0x000F3A73
		// (set) Token: 0x060039BF RID: 14783 RVA: 0x000F4A7B File Offset: 0x000F3A7B
		public string CategoryResourceFile
		{
			get
			{
				return this._categoryResourceFile;
			}
			set
			{
				this._categoryResourceFile = value;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x000F4A84 File Offset: 0x000F3A84
		// (set) Token: 0x060039C1 RID: 14785 RVA: 0x000F4A8C File Offset: 0x000F3A8C
		public int CategoryCount
		{
			get
			{
				return this._categoryCount;
			}
			set
			{
				if (value > 65535 || value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._categoryCount = value;
			}
		}

		// Token: 0x040032C6 RID: 12998
		private string _logName = "Application";

		// Token: 0x040032C7 RID: 12999
		private string _machineName = ".";

		// Token: 0x040032C8 RID: 13000
		private string _source;

		// Token: 0x040032C9 RID: 13001
		private string _messageResourceFile;

		// Token: 0x040032CA RID: 13002
		private string _parameterResourceFile;

		// Token: 0x040032CB RID: 13003
		private string _categoryResourceFile;

		// Token: 0x040032CC RID: 13004
		private int _categoryCount;
	}
}
