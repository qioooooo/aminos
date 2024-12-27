using System;
using System.Threading;

namespace System.Configuration.Internal
{
	// Token: 0x020000C3 RID: 195
	internal sealed class InternalConfigRoot : IInternalConfigRoot
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000756 RID: 1878 RVA: 0x0001FDC7 File Offset: 0x0001EDC7
		// (remove) Token: 0x06000757 RID: 1879 RVA: 0x0001FDE0 File Offset: 0x0001EDE0
		public event InternalConfigEventHandler ConfigChanged;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000758 RID: 1880 RVA: 0x0001FDF9 File Offset: 0x0001EDF9
		// (remove) Token: 0x06000759 RID: 1881 RVA: 0x0001FE12 File Offset: 0x0001EE12
		public event InternalConfigEventHandler ConfigRemoved;

		// Token: 0x0600075A RID: 1882 RVA: 0x0001FE2B File Offset: 0x0001EE2B
		internal InternalConfigRoot()
		{
		}

		// Token: 0x0600075B RID: 1883 RVA: 0x0001FE34 File Offset: 0x0001EE34
		void IInternalConfigRoot.Init(IInternalConfigHost host, bool isDesignTime)
		{
			this._host = host;
			this._isDesignTime = isDesignTime;
			this._hierarchyLock = new ReaderWriterLock();
			if (this._isDesignTime)
			{
				this._rootConfigRecord = MgmtConfigurationRecord.Create(this, null, string.Empty, null);
				return;
			}
			this._rootConfigRecord = (BaseConfigurationRecord)RuntimeConfigurationRecord.Create(this, null, string.Empty);
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0001FE8D File Offset: 0x0001EE8D
		internal IInternalConfigHost Host
		{
			get
			{
				return this._host;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0001FE95 File Offset: 0x0001EE95
		internal BaseConfigurationRecord RootConfigRecord
		{
			get
			{
				return this._rootConfigRecord;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0001FE9D File Offset: 0x0001EE9D
		bool IInternalConfigRoot.IsDesignTime
		{
			get
			{
				return this._isDesignTime;
			}
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0001FEA5 File Offset: 0x0001EEA5
		private void AcquireHierarchyLockForRead()
		{
			if (this._hierarchyLock.IsReaderLockHeld)
			{
				throw ExceptionUtil.UnexpectedError("System.Configuration.Internal.InternalConfigRoot::AcquireHierarchyLockForRead - reader lock already held by this thread");
			}
			if (this._hierarchyLock.IsWriterLockHeld)
			{
				throw ExceptionUtil.UnexpectedError("System.Configuration.Internal.InternalConfigRoot::AcquireHierarchyLockForRead - writer lock already held by this thread");
			}
			this._hierarchyLock.AcquireReaderLock(-1);
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0001FEE3 File Offset: 0x0001EEE3
		private void ReleaseHierarchyLockForRead()
		{
			if (this._hierarchyLock.IsReaderLockHeld)
			{
				this._hierarchyLock.ReleaseReaderLock();
			}
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0001FEFD File Offset: 0x0001EEFD
		private void AcquireHierarchyLockForWrite()
		{
			if (this._hierarchyLock.IsReaderLockHeld)
			{
				throw ExceptionUtil.UnexpectedError("System.Configuration.Internal.InternalConfigRoot::AcquireHierarchyLockForWrite - reader lock already held by this thread");
			}
			if (this._hierarchyLock.IsWriterLockHeld)
			{
				throw ExceptionUtil.UnexpectedError("System.Configuration.Internal.InternalConfigRoot::AcquireHierarchyLockForWrite - writer lock already held by this thread");
			}
			this._hierarchyLock.AcquireWriterLock(-1);
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0001FF3B File Offset: 0x0001EF3B
		private void ReleaseHierarchyLockForWrite()
		{
			if (this._hierarchyLock.IsWriterLockHeld)
			{
				this._hierarchyLock.ReleaseWriterLock();
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0001FF58 File Offset: 0x0001EF58
		private void hlFindConfigRecord(string[] parts, out int nextIndex, out BaseConfigurationRecord currentRecord)
		{
			currentRecord = this._rootConfigRecord;
			for (nextIndex = 0; nextIndex < parts.Length; nextIndex++)
			{
				BaseConfigurationRecord baseConfigurationRecord = currentRecord.hlGetChild(parts[nextIndex]);
				if (baseConfigurationRecord == null)
				{
					return;
				}
				currentRecord = baseConfigurationRecord;
			}
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0001FF94 File Offset: 0x0001EF94
		public object GetSection(string section, string configPath)
		{
			BaseConfigurationRecord baseConfigurationRecord = (BaseConfigurationRecord)this.GetUniqueConfigRecord(configPath);
			return baseConfigurationRecord.GetSection(section);
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0001FFB8 File Offset: 0x0001EFB8
		public string GetUniqueConfigPath(string configPath)
		{
			IInternalConfigRecord uniqueConfigRecord = this.GetUniqueConfigRecord(configPath);
			if (uniqueConfigRecord == null)
			{
				return null;
			}
			return uniqueConfigRecord.ConfigPath;
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0001FFD8 File Offset: 0x0001EFD8
		public IInternalConfigRecord GetUniqueConfigRecord(string configPath)
		{
			BaseConfigurationRecord baseConfigurationRecord = (BaseConfigurationRecord)this.GetConfigRecord(configPath);
			while (baseConfigurationRecord.IsEmpty)
			{
				BaseConfigurationRecord parent = baseConfigurationRecord.Parent;
				if (parent.IsRootConfig)
				{
					break;
				}
				baseConfigurationRecord = parent;
			}
			return baseConfigurationRecord;
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x00020010 File Offset: 0x0001F010
		public IInternalConfigRecord GetConfigRecord(string configPath)
		{
			if (!ConfigPathUtility.IsValid(configPath))
			{
				throw ExceptionUtil.ParameterInvalid("configPath");
			}
			string[] parts = ConfigPathUtility.GetParts(configPath);
			try
			{
				this.AcquireHierarchyLockForRead();
				int num;
				BaseConfigurationRecord baseConfigurationRecord;
				this.hlFindConfigRecord(parts, out num, out baseConfigurationRecord);
				if (num == parts.Length || !baseConfigurationRecord.hlNeedsChildFor(parts[num]))
				{
					return baseConfigurationRecord;
				}
			}
			finally
			{
				this.ReleaseHierarchyLockForRead();
			}
			IInternalConfigRecord internalConfigRecord;
			try
			{
				this.AcquireHierarchyLockForWrite();
				int num2;
				BaseConfigurationRecord baseConfigurationRecord2;
				this.hlFindConfigRecord(parts, out num2, out baseConfigurationRecord2);
				if (num2 == parts.Length)
				{
					internalConfigRecord = baseConfigurationRecord2;
				}
				else
				{
					string text = string.Join("/", parts, 0, num2);
					while (num2 < parts.Length && baseConfigurationRecord2.hlNeedsChildFor(parts[num2]))
					{
						string text2 = parts[num2];
						text = ConfigPathUtility.Combine(text, text2);
						BaseConfigurationRecord baseConfigurationRecord3;
						if (this._isDesignTime)
						{
							baseConfigurationRecord3 = MgmtConfigurationRecord.Create(this, baseConfigurationRecord2, text, null);
						}
						else
						{
							baseConfigurationRecord3 = (BaseConfigurationRecord)RuntimeConfigurationRecord.Create(this, baseConfigurationRecord2, text);
						}
						baseConfigurationRecord2.hlAddChild(text2, baseConfigurationRecord3);
						num2++;
						baseConfigurationRecord2 = baseConfigurationRecord3;
					}
					internalConfigRecord = baseConfigurationRecord2;
				}
			}
			finally
			{
				this.ReleaseHierarchyLockForWrite();
			}
			return internalConfigRecord;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x00020120 File Offset: 0x0001F120
		private void RemoveConfigImpl(string configPath, BaseConfigurationRecord configRecord)
		{
			if (!ConfigPathUtility.IsValid(configPath))
			{
				throw ExceptionUtil.ParameterInvalid("configPath");
			}
			string[] parts = ConfigPathUtility.GetParts(configPath);
			BaseConfigurationRecord baseConfigurationRecord;
			try
			{
				this.AcquireHierarchyLockForWrite();
				int num;
				this.hlFindConfigRecord(parts, out num, out baseConfigurationRecord);
				if (num != parts.Length || (configRecord != null && !object.ReferenceEquals(configRecord, baseConfigurationRecord)))
				{
					return;
				}
				baseConfigurationRecord.Parent.hlRemoveChild(parts[parts.Length - 1]);
			}
			finally
			{
				this.ReleaseHierarchyLockForWrite();
			}
			this.OnConfigRemoved(new InternalConfigEventArgs(configPath));
			baseConfigurationRecord.CloseRecursive();
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x000201AC File Offset: 0x0001F1AC
		public void RemoveConfig(string configPath)
		{
			this.RemoveConfigImpl(configPath, null);
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x000201B6 File Offset: 0x0001F1B6
		public void RemoveConfigRecord(BaseConfigurationRecord configRecord)
		{
			this.RemoveConfigImpl(configRecord.ConfigPath, configRecord);
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x000201C8 File Offset: 0x0001F1C8
		public void ClearResult(BaseConfigurationRecord configRecord, string configKey, bool forceEvaluation)
		{
			string[] parts = ConfigPathUtility.GetParts(configRecord.ConfigPath);
			try
			{
				this.AcquireHierarchyLockForRead();
				int num;
				BaseConfigurationRecord baseConfigurationRecord;
				this.hlFindConfigRecord(parts, out num, out baseConfigurationRecord);
				if (num == parts.Length && object.ReferenceEquals(configRecord, baseConfigurationRecord))
				{
					baseConfigurationRecord.hlClearResultRecursive(configKey, forceEvaluation);
				}
			}
			finally
			{
				this.ReleaseHierarchyLockForRead();
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x00020224 File Offset: 0x0001F224
		private void OnConfigRemoved(InternalConfigEventArgs e)
		{
			InternalConfigEventHandler configRemoved = this.ConfigRemoved;
			if (configRemoved != null)
			{
				configRemoved(this, e);
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00020243 File Offset: 0x0001F243
		internal void FireConfigChanged(string configPath)
		{
			this.OnConfigChanged(new InternalConfigEventArgs(configPath));
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x00020254 File Offset: 0x0001F254
		private void OnConfigChanged(InternalConfigEventArgs e)
		{
			InternalConfigEventHandler configChanged = this.ConfigChanged;
			if (configChanged != null)
			{
				configChanged(this, e);
			}
		}

		// Token: 0x04000423 RID: 1059
		private IInternalConfigHost _host;

		// Token: 0x04000424 RID: 1060
		private ReaderWriterLock _hierarchyLock;

		// Token: 0x04000425 RID: 1061
		private BaseConfigurationRecord _rootConfigRecord;

		// Token: 0x04000426 RID: 1062
		private bool _isDesignTime;
	}
}
