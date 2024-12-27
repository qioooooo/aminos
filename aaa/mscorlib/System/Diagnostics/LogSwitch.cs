using System;

namespace System.Diagnostics
{
	// Token: 0x020002AE RID: 686
	[Serializable]
	internal class LogSwitch
	{
		// Token: 0x06001B13 RID: 6931 RVA: 0x00047127 File Offset: 0x00046127
		private LogSwitch()
		{
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x00047130 File Offset: 0x00046130
		public LogSwitch(string name, string description, LogSwitch parent)
		{
			if (name == null || parent == null)
			{
				throw new ArgumentNullException((name == null) ? "name" : "parent");
			}
			if (name.Length == 0)
			{
				throw new ArgumentOutOfRangeException("Name", Environment.GetResourceString("Argument_StringZeroLength"));
			}
			this.strName = name;
			this.strDescription = description;
			this.iLevel = LoggingLevels.ErrorLevel;
			this.iOldLevel = this.iLevel;
			parent.AddChildSwitch(this);
			this.ParentSwitch = parent;
			this.ChildSwitch = null;
			this.iNumChildren = 0;
			this.iChildArraySize = 0;
			Log.m_Hashtable.Add(this.strName, this);
			Log.AddLogSwitch(this);
			Log.iNumOfSwitches++;
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x000471EC File Offset: 0x000461EC
		internal LogSwitch(string name, string description)
		{
			this.strName = name;
			this.strDescription = description;
			this.iLevel = LoggingLevels.ErrorLevel;
			this.iOldLevel = this.iLevel;
			this.ParentSwitch = null;
			this.ChildSwitch = null;
			this.iNumChildren = 0;
			this.iChildArraySize = 0;
			Log.m_Hashtable.Add(this.strName, this);
			Log.AddLogSwitch(this);
			Log.iNumOfSwitches++;
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06001B16 RID: 6934 RVA: 0x00047260 File Offset: 0x00046260
		public virtual string Name
		{
			get
			{
				return this.strName;
			}
		}

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06001B17 RID: 6935 RVA: 0x00047268 File Offset: 0x00046268
		public virtual string Description
		{
			get
			{
				return this.strDescription;
			}
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06001B18 RID: 6936 RVA: 0x00047270 File Offset: 0x00046270
		public virtual LogSwitch Parent
		{
			get
			{
				return this.ParentSwitch;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06001B19 RID: 6937 RVA: 0x00047278 File Offset: 0x00046278
		// (set) Token: 0x06001B1A RID: 6938 RVA: 0x00047280 File Offset: 0x00046280
		public virtual LoggingLevels MinimumLevel
		{
			get
			{
				return this.iLevel;
			}
			set
			{
				this.iLevel = value;
				this.iOldLevel = value;
				string text = ((this.ParentSwitch != null) ? this.ParentSwitch.Name : "");
				if (Debugger.IsAttached)
				{
					Log.ModifyLogSwitch((int)this.iLevel, this.strName, text);
				}
				Log.InvokeLogSwitchLevelHandlers(this, this.iLevel);
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x000472DB File Offset: 0x000462DB
		public virtual bool CheckLevel(LoggingLevels level)
		{
			return this.iLevel <= level || (this.ParentSwitch != null && this.ParentSwitch.CheckLevel(level));
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x000472FE File Offset: 0x000462FE
		public static LogSwitch GetSwitch(string name)
		{
			return (LogSwitch)Log.m_Hashtable[name];
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x00047310 File Offset: 0x00046310
		private void AddChildSwitch(LogSwitch child)
		{
			if (this.iChildArraySize <= this.iNumChildren)
			{
				int num;
				if (this.iChildArraySize == 0)
				{
					num = 10;
				}
				else
				{
					num = this.iChildArraySize * 3 / 2;
				}
				LogSwitch[] array = new LogSwitch[num];
				if (this.iNumChildren > 0)
				{
					Array.Copy(this.ChildSwitch, array, this.iNumChildren);
				}
				this.iChildArraySize = num;
				this.ChildSwitch = array;
			}
			this.ChildSwitch[this.iNumChildren++] = child;
		}

		// Token: 0x04000A33 RID: 2611
		internal string strName;

		// Token: 0x04000A34 RID: 2612
		internal string strDescription;

		// Token: 0x04000A35 RID: 2613
		private LogSwitch ParentSwitch;

		// Token: 0x04000A36 RID: 2614
		private LogSwitch[] ChildSwitch;

		// Token: 0x04000A37 RID: 2615
		internal LoggingLevels iLevel;

		// Token: 0x04000A38 RID: 2616
		internal LoggingLevels iOldLevel;

		// Token: 0x04000A39 RID: 2617
		private int iNumChildren;

		// Token: 0x04000A3A RID: 2618
		private int iChildArraySize;
	}
}
