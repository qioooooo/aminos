using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000032 RID: 50
	internal sealed class FileMonitor
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00005A18 File Offset: 0x00004A18
		internal FileMonitor(DirectoryMonitor dirMon, string fileNameLong, string fileNameShort, bool exists, FileAttributesData fad, byte[] dacl)
		{
			this.DirectoryMonitor = dirMon;
			this._fileNameLong = fileNameLong;
			this._fileNameShort = fileNameShort;
			this._exists = exists;
			this._fad = fad;
			this._dacl = dacl;
			this._targets = new HybridDictionary();
			this.Aliases = new HybridDictionary(true);
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x00005A6F File Offset: 0x00004A6F
		internal string FileNameLong
		{
			get
			{
				return this._fileNameLong;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00005A77 File Offset: 0x00004A77
		internal string FileNameShort
		{
			get
			{
				return this._fileNameShort;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060000FB RID: 251 RVA: 0x00005A7F File Offset: 0x00004A7F
		internal bool Exists
		{
			get
			{
				return this._exists;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00005A87 File Offset: 0x00004A87
		internal bool IsDirectory
		{
			get
			{
				return this.FileNameLong == null;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00005A92 File Offset: 0x00004A92
		// (set) Token: 0x060000FE RID: 254 RVA: 0x00005A9A File Offset: 0x00004A9A
		internal FileAction LastAction
		{
			get
			{
				return this._lastAction;
			}
			set
			{
				this._lastAction = value;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060000FF RID: 255 RVA: 0x00005AA3 File Offset: 0x00004AA3
		// (set) Token: 0x06000100 RID: 256 RVA: 0x00005AAB File Offset: 0x00004AAB
		internal DateTime UtcLastCompletion
		{
			get
			{
				return this._utcLastCompletion;
			}
			set
			{
				this._utcLastCompletion = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00005AB4 File Offset: 0x00004AB4
		internal FileAttributesData Attributes
		{
			get
			{
				return this._fad;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005ABC File Offset: 0x00004ABC
		internal byte[] Dacl
		{
			get
			{
				return this._dacl;
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00005AC4 File Offset: 0x00004AC4
		internal void ResetCachedAttributes()
		{
			this._fad = null;
			this._dacl = null;
		}

		// Token: 0x06000104 RID: 260 RVA: 0x00005AD4 File Offset: 0x00004AD4
		internal void UpdateCachedAttributes()
		{
			string text = Path.Combine(this.DirectoryMonitor.Directory, this.FileNameLong);
			FileAttributesData.GetFileAttributes(text, out this._fad);
			this._dacl = FileSecurity.GetDacl(text);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00005B11 File Offset: 0x00004B11
		internal void MakeExist(FindFileData ffd, byte[] dacl)
		{
			this._fileNameLong = ffd.FileNameLong;
			this._fileNameShort = ffd.FileNameShort;
			this._fad = ffd.FileAttributesData;
			this._dacl = dacl;
			this._exists = true;
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005B45 File Offset: 0x00004B45
		internal void MakeExtinct()
		{
			this._fad = null;
			this._dacl = null;
			this._exists = false;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x00005B5C File Offset: 0x00004B5C
		internal void RemoveFileNameShort()
		{
			this._fileNameShort = null;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005B65 File Offset: 0x00004B65
		internal ICollection Targets
		{
			get
			{
				return this._targets.Values;
			}
		}

		// Token: 0x06000109 RID: 265 RVA: 0x00005B74 File Offset: 0x00004B74
		internal void AddTarget(FileChangeEventHandler callback, string alias, bool newAlias)
		{
			FileMonitorTarget fileMonitorTarget = (FileMonitorTarget)this._targets[callback.Target];
			if (fileMonitorTarget != null)
			{
				fileMonitorTarget.AddRef();
			}
			else
			{
				this._targets.Add(callback.Target, new FileMonitorTarget(callback, alias));
			}
			if (newAlias)
			{
				this.Aliases[alias] = alias;
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00005BCC File Offset: 0x00004BCC
		internal int RemoveTarget(object callbackTarget)
		{
			FileMonitorTarget fileMonitorTarget = (FileMonitorTarget)this._targets[callbackTarget];
			if (fileMonitorTarget != null && fileMonitorTarget.Release() == 0)
			{
				this._targets.Remove(callbackTarget);
			}
			return this._targets.Count;
		}

		// Token: 0x04000DB0 RID: 3504
		internal readonly DirectoryMonitor DirectoryMonitor;

		// Token: 0x04000DB1 RID: 3505
		internal readonly HybridDictionary Aliases;

		// Token: 0x04000DB2 RID: 3506
		private string _fileNameLong;

		// Token: 0x04000DB3 RID: 3507
		private string _fileNameShort;

		// Token: 0x04000DB4 RID: 3508
		private HybridDictionary _targets;

		// Token: 0x04000DB5 RID: 3509
		private bool _exists;

		// Token: 0x04000DB6 RID: 3510
		private FileAttributesData _fad;

		// Token: 0x04000DB7 RID: 3511
		private byte[] _dacl;

		// Token: 0x04000DB8 RID: 3512
		private FileAction _lastAction;

		// Token: 0x04000DB9 RID: 3513
		private DateTime _utcLastCompletion;
	}
}
