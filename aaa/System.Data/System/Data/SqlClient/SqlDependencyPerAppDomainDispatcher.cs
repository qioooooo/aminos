using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Threading;

namespace System.Data.SqlClient
{
	// Token: 0x020002ED RID: 749
	internal class SqlDependencyPerAppDomainDispatcher : MarshalByRefObject
	{
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x00286ECC File Offset: 0x002862CC
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x060026F4 RID: 9972 RVA: 0x00286EE0 File Offset: 0x002862E0
		private SqlDependencyPerAppDomainDispatcher()
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher|DEP> %d#", this.ObjectID);
			try
			{
				this._dependencyIdToDependencyHash = new Dictionary<string, SqlDependency>();
				this._notificationIdToDependenciesHash = new Dictionary<string, SqlDependencyPerAppDomainDispatcher.DependencyList>();
				this._commandHashToNotificationId = new Dictionary<string, string>();
				this._timeoutTimer = new Timer(new TimerCallback(SqlDependencyPerAppDomainDispatcher.TimeoutTimerCallback), null, -1, -1);
				AppDomain.CurrentDomain.DomainUnload += this.UnloadEventHandler;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x00286F8C File Offset: 0x0028638C
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x00286F9C File Offset: 0x0028639C
		private void UnloadEventHandler(object sender, EventArgs e)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.UnloadEventHandler|DEP> %d#", this.ObjectID);
			try
			{
				SqlDependencyProcessDispatcher processDispatcher = SqlDependency.ProcessDispatcher;
				if (processDispatcher != null)
				{
					processDispatcher.QueueAppDomainUnloading(SqlDependency.AppDomainKey);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x00286FF8 File Offset: 0x002863F8
		internal void AddDependencyEntry(SqlDependency dep)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.AddDependencyEntry|DEP> %d#, SqlDependency: %d#", this.ObjectID, dep.ObjectID);
			try
			{
				lock (this)
				{
					this._dependencyIdToDependencyHash.Add(dep.Id, dep);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x00287080 File Offset: 0x00286480
		internal string AddCommandEntry(string commandHash, SqlDependency dep)
		{
			string text = string.Empty;
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> %d#, commandHash: '%ls', SqlDependency: %d#", this.ObjectID, commandHash, dep.ObjectID);
			try
			{
				lock (this)
				{
					if (!this._dependencyIdToDependencyHash.ContainsKey(dep.Id))
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> Dependency not present in depId->dep hash, must have been invalidated.\n");
					}
					else if (this._commandHashToNotificationId.TryGetValue(commandHash, out text))
					{
						SqlDependencyPerAppDomainDispatcher.DependencyList dependencyList = null;
						if (!this._notificationIdToDependenciesHash.TryGetValue(text, out dependencyList))
						{
							throw ADP.InternalError(ADP.InternalErrorCode.SqlDependencyCommandHashIsNotAssociatedWithNotification);
						}
						if (!dependencyList.Contains(dep))
						{
							Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> Dependency not present for commandHash, adding.\n");
							dependencyList.Add(dep);
						}
						else
						{
							Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> Dependency already present for commandHash.\n");
						}
					}
					else
					{
						text = string.Format(CultureInfo.InvariantCulture, "{0};{1}", new object[]
						{
							SqlDependency.AppDomainKey,
							Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture)
						});
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.AddCommandEntry|DEP> Creating new Dependencies list for commandHash.\n");
						SqlDependencyPerAppDomainDispatcher.DependencyList dependencyList2 = new SqlDependencyPerAppDomainDispatcher.DependencyList(commandHash);
						dependencyList2.Add(dep);
						try
						{
						}
						finally
						{
							this._commandHashToNotificationId.Add(commandHash, text);
							this._notificationIdToDependenciesHash.Add(text, dependencyList2);
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return text;
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x00287200 File Offset: 0x00286600
		internal void InvalidateCommandID(SqlNotification sqlNotification)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.InvalidateCommandID|DEP> %d#, commandHash: '%ls'", this.ObjectID, sqlNotification.Key);
			try
			{
				List<SqlDependency> list = null;
				lock (this)
				{
					list = this.LookupCommandEntryWithRemove(sqlNotification.Key);
					if (list != null)
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.InvalidateCommandID|DEP> commandHash found in hashtable.\n");
						using (List<SqlDependency>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								SqlDependency sqlDependency = enumerator.Current;
								this.LookupDependencyEntryWithRemove(sqlDependency.Id);
								this.RemoveDependencyFromCommandToDependenciesHash(sqlDependency);
							}
							goto IL_0085;
						}
					}
					Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.InvalidateCommandID|DEP> commandHash NOT found in hashtable.\n");
					IL_0085:;
				}
				if (list != null)
				{
					foreach (SqlDependency sqlDependency2 in list)
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.InvalidateCommandID|DEP> Dependency found in commandHash dependency ArrayList - calling invalidate.\n");
						try
						{
							sqlDependency2.Invalidate(sqlNotification.Type, sqlNotification.Info, sqlNotification.Source);
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(ex);
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x00287388 File Offset: 0x00286788
		internal void InvalidateServer(string server, SqlNotification sqlNotification)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.Invalidate|DEP> %d#, server: '%ls'", this.ObjectID, server);
			try
			{
				List<SqlDependency> list = new List<SqlDependency>();
				lock (this)
				{
					foreach (KeyValuePair<string, SqlDependency> keyValuePair in this._dependencyIdToDependencyHash)
					{
						SqlDependency value = keyValuePair.Value;
						if (value.ServerList.Contains(server))
						{
							list.Add(value);
						}
					}
					foreach (SqlDependency sqlDependency in list)
					{
						this.LookupDependencyEntryWithRemove(sqlDependency.Id);
						this.RemoveDependencyFromCommandToDependenciesHash(sqlDependency);
					}
				}
				foreach (SqlDependency sqlDependency2 in list)
				{
					try
					{
						sqlDependency2.Invalidate(sqlNotification.Type, sqlNotification.Info, sqlNotification.Source);
					}
					catch (Exception ex)
					{
						if (!ADP.IsCatchableExceptionType(ex))
						{
							throw;
						}
						ADP.TraceExceptionWithoutRethrow(ex);
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x00287548 File Offset: 0x00286948
		internal SqlDependency LookupDependencyEntry(string id)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntry|DEP> %d#, Key: '%ls'", this.ObjectID, id);
			SqlDependency sqlDependency2;
			try
			{
				if (id == null)
				{
					throw ADP.ArgumentNull("id");
				}
				if (ADP.IsEmpty(id))
				{
					throw SQL.SqlDependencyIdMismatch();
				}
				SqlDependency sqlDependency = null;
				lock (this)
				{
					if (this._dependencyIdToDependencyHash.ContainsKey(id))
					{
						sqlDependency = this._dependencyIdToDependencyHash[id];
					}
					else
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntry|DEP|ERR> ERROR - dependency ID mismatch - not throwing.\n");
					}
				}
				sqlDependency2 = sqlDependency;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return sqlDependency2;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x00287600 File Offset: 0x00286A00
		private void LookupDependencyEntryWithRemove(string id)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntryWithRemove|DEP> %d#, id: '%ls'", this.ObjectID, id);
			try
			{
				lock (this)
				{
					if (this._dependencyIdToDependencyHash.ContainsKey(id))
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntryWithRemove|DEP> Entry found in hashtable - removing.\n");
						this._dependencyIdToDependencyHash.Remove(id);
						if (this._dependencyIdToDependencyHash.Count == 0)
						{
							this._timeoutTimer.Change(-1, -1);
							this._SqlDependencyTimeOutTimerStarted = false;
						}
					}
					else
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntryWithRemove|DEP> Entry NOT found in hashtable.\n");
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x002876C4 File Offset: 0x00286AC4
		private List<SqlDependency> LookupCommandEntryWithRemove(string notificationId)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.LookupCommandEntryWithRemove|DEP> %d#, commandHash: '%ls'", this.ObjectID, notificationId);
			List<SqlDependency> list;
			try
			{
				SqlDependencyPerAppDomainDispatcher.DependencyList dependencyList = null;
				lock (this)
				{
					if (this._notificationIdToDependenciesHash.TryGetValue(notificationId, out dependencyList))
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntriesWithRemove|DEP> Entries found in hashtable - removing.\n");
						try
						{
							goto IL_0063;
						}
						finally
						{
							this._notificationIdToDependenciesHash.Remove(notificationId);
							this._commandHashToNotificationId.Remove(dependencyList.CommandHash);
						}
					}
					Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.LookupDependencyEntriesWithRemove|DEP> Entries NOT found in hashtable.\n");
					IL_0063:;
				}
				list = dependencyList;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return list;
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x00287798 File Offset: 0x00286B98
		private void RemoveDependencyFromCommandToDependenciesHash(SqlDependency dependency)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.RemoveDependencyFromCommandToDependenciesHash|DEP> %d#, SqlDependency: %d#", this.ObjectID, dependency.ObjectID);
			try
			{
				lock (this)
				{
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					foreach (KeyValuePair<string, SqlDependencyPerAppDomainDispatcher.DependencyList> keyValuePair in this._notificationIdToDependenciesHash)
					{
						SqlDependencyPerAppDomainDispatcher.DependencyList value = keyValuePair.Value;
						if (value.Remove(dependency))
						{
							Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.RemoveDependencyFromCommandToDependenciesHash|DEP> Removed SqlDependency: %d#, with ID: '%ls'.\n", dependency.ObjectID, dependency.Id);
							if (value.Count == 0)
							{
								list.Add(keyValuePair.Key);
								list2.Add(keyValuePair.Value.CommandHash);
							}
						}
					}
					for (int i = 0; i < list.Count; i++)
					{
						try
						{
						}
						finally
						{
							this._notificationIdToDependenciesHash.Remove(list[i]);
							this._commandHashToNotificationId.Remove(list2[i]);
						}
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x00287904 File Offset: 0x00286D04
		internal void StartTimer(SqlDependency dep)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.StartTimer|DEP> %d#, SqlDependency: %d#", this.ObjectID, dep.ObjectID);
			try
			{
				lock (this)
				{
					if (!this._SqlDependencyTimeOutTimerStarted)
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.StartTimer|DEP> Timer not yet started, starting.\n");
						this._timeoutTimer.Change(15000, 15000);
						this._nextTimeout = dep.ExpirationTime;
						this._SqlDependencyTimeOutTimerStarted = true;
					}
					else if (this._nextTimeout > dep.ExpirationTime)
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.StartTimer|DEP> Timer already started, resetting time.\n");
						this._nextTimeout = dep.ExpirationTime;
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x002879E0 File Offset: 0x00286DE0
		private static void TimeoutTimerCallback(object state)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependencyPerAppDomainDispatcher.TimeoutTimerCallback|DEP> AppDomainKey: '%ls'", SqlDependency.AppDomainKey);
			try
			{
				SqlDependency[] array;
				lock (SqlDependencyPerAppDomainDispatcher.SingletonInstance)
				{
					if (SqlDependencyPerAppDomainDispatcher.SingletonInstance._dependencyIdToDependencyHash.Count == 0)
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.TimeoutTimerCallback|DEP> No dependencies, exiting.\n");
						return;
					}
					if (SqlDependencyPerAppDomainDispatcher.SingletonInstance._nextTimeout > DateTime.UtcNow)
					{
						Bid.NotificationsTrace("<sc.SqlDependencyPerAppDomainDispatcher.TimeoutTimerCallback|DEP> No timeouts expired, exiting.\n");
						return;
					}
					array = new SqlDependency[SqlDependencyPerAppDomainDispatcher.SingletonInstance._dependencyIdToDependencyHash.Count];
					SqlDependencyPerAppDomainDispatcher.SingletonInstance._dependencyIdToDependencyHash.Values.CopyTo(array, 0);
				}
				DateTime utcNow = DateTime.UtcNow;
				DateTime dateTime = DateTime.MaxValue;
				int i = 0;
				while (i < array.Length)
				{
					if (array[i].ExpirationTime <= utcNow)
					{
						try
						{
							array[i].Invalidate(SqlNotificationType.Change, SqlNotificationInfo.Error, SqlNotificationSource.Timeout);
							goto IL_00FA;
						}
						catch (Exception ex)
						{
							if (!ADP.IsCatchableExceptionType(ex))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(ex);
							goto IL_00FA;
						}
						goto IL_00DD;
					}
					goto IL_00DD;
					IL_00FA:
					i++;
					continue;
					IL_00DD:
					if (array[i].ExpirationTime < dateTime)
					{
						dateTime = array[i].ExpirationTime;
					}
					array[i] = null;
					goto IL_00FA;
				}
				lock (SqlDependencyPerAppDomainDispatcher.SingletonInstance)
				{
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j] != null)
						{
							SqlDependencyPerAppDomainDispatcher.SingletonInstance._dependencyIdToDependencyHash.Remove(array[j].Id);
						}
					}
					if (dateTime < SqlDependencyPerAppDomainDispatcher.SingletonInstance._nextTimeout)
					{
						SqlDependencyPerAppDomainDispatcher.SingletonInstance._nextTimeout = dateTime;
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x0400188D RID: 6285
		internal static readonly SqlDependencyPerAppDomainDispatcher SingletonInstance = new SqlDependencyPerAppDomainDispatcher();

		// Token: 0x0400188E RID: 6286
		private Dictionary<string, SqlDependency> _dependencyIdToDependencyHash;

		// Token: 0x0400188F RID: 6287
		private Dictionary<string, SqlDependencyPerAppDomainDispatcher.DependencyList> _notificationIdToDependenciesHash;

		// Token: 0x04001890 RID: 6288
		private Dictionary<string, string> _commandHashToNotificationId;

		// Token: 0x04001891 RID: 6289
		private bool _SqlDependencyTimeOutTimerStarted;

		// Token: 0x04001892 RID: 6290
		private DateTime _nextTimeout;

		// Token: 0x04001893 RID: 6291
		private Timer _timeoutTimer;

		// Token: 0x04001894 RID: 6292
		private readonly int _objectID = Interlocked.Increment(ref SqlDependencyPerAppDomainDispatcher._objectTypeCount);

		// Token: 0x04001895 RID: 6293
		private static int _objectTypeCount;

		// Token: 0x020002EE RID: 750
		private sealed class DependencyList : List<SqlDependency>
		{
			// Token: 0x06002702 RID: 9986 RVA: 0x00287BD8 File Offset: 0x00286FD8
			internal DependencyList(string commandHash)
			{
				this.CommandHash = commandHash;
			}

			// Token: 0x04001896 RID: 6294
			public readonly string CommandHash;
		}
	}
}
