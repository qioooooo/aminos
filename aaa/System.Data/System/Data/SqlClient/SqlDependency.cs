using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Data.Sql;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;

namespace System.Data.SqlClient
{
	// Token: 0x020002E4 RID: 740
	public sealed class SqlDependency
	{
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06002697 RID: 9879 RVA: 0x0028378C File Offset: 0x00282B8C
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x002837A0 File Offset: 0x00282BA0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public SqlDependency()
			: this(null, null, 0)
		{
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x002837B8 File Offset: 0x00282BB8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public SqlDependency(SqlCommand command)
			: this(command, null, 0)
		{
		}

		// Token: 0x0600269A RID: 9882 RVA: 0x002837D0 File Offset: 0x00282BD0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public SqlDependency(SqlCommand command, string options, int timeout)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency|DEP> %d#, options: '%ls', timeout: '%d'", this.ObjectID, options, timeout);
			try
			{
				if (InOutOfProcHelper.InProc)
				{
					throw SQL.SqlDepCannotBeCreatedInProc();
				}
				if (timeout < 0)
				{
					throw SQL.InvalidSqlDependencyTimeout("timeout");
				}
				this._timeout = timeout;
				if (options != null)
				{
					this._options = options;
				}
				this.AddCommandInternal(command);
				SqlDependencyPerAppDomainDispatcher.SingletonInstance.AddDependencyEntry(this);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x0600269B RID: 9883 RVA: 0x002838C0 File Offset: 0x00282CC0
		[ResCategory("DataCategory_Data")]
		[ResDescription("SqlDependency_HasChanges")]
		public bool HasChanges
		{
			get
			{
				return this._dependencyFired;
			}
		}

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x0600269C RID: 9884 RVA: 0x002838D4 File Offset: 0x00282CD4
		[ResCategory("DataCategory_Data")]
		[ResDescription("SqlDependency_Id")]
		public string Id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x0600269D RID: 9885 RVA: 0x002838E8 File Offset: 0x00282CE8
		internal static string AppDomainKey
		{
			get
			{
				return SqlDependency._appDomainKey;
			}
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x0600269E RID: 9886 RVA: 0x002838FC File Offset: 0x00282CFC
		internal DateTime ExpirationTime
		{
			get
			{
				return this._expirationTime;
			}
		}

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x0600269F RID: 9887 RVA: 0x00283910 File Offset: 0x00282D10
		internal string Options
		{
			get
			{
				string text = null;
				if (this._options != null)
				{
					text = this._options;
				}
				return text;
			}
		}

		// Token: 0x17000616 RID: 1558
		// (get) Token: 0x060026A0 RID: 9888 RVA: 0x00283930 File Offset: 0x00282D30
		internal static SqlDependencyProcessDispatcher ProcessDispatcher
		{
			get
			{
				return SqlDependency._processDispatcher;
			}
		}

		// Token: 0x17000617 RID: 1559
		// (get) Token: 0x060026A1 RID: 9889 RVA: 0x00283944 File Offset: 0x00282D44
		internal List<string> ServerList
		{
			get
			{
				return this._serverList;
			}
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060026A2 RID: 9890 RVA: 0x00283958 File Offset: 0x00282D58
		internal int Timeout
		{
			get
			{
				return this._timeout;
			}
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x060026A3 RID: 9891 RVA: 0x0028396C File Offset: 0x00282D6C
		// (remove) Token: 0x060026A4 RID: 9892 RVA: 0x00283A48 File Offset: 0x00282E48
		[ResCategory("DataCategory_Data")]
		[ResDescription("SqlDependency_OnChange")]
		public event OnChangeEventHandler OnChange
		{
			add
			{
				IntPtr intPtr;
				Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.OnChange-Add|DEP> %d#", this.ObjectID);
				try
				{
					if (value != null)
					{
						SqlNotificationEventArgs sqlNotificationEventArgs = null;
						lock (this._eventHandlerLock)
						{
							if (this._dependencyFired)
							{
								Bid.NotificationsTrace("<sc.SqlDependency.OnChange-Add|DEP> Dependency already fired, firing new event.\n");
								sqlNotificationEventArgs = new SqlNotificationEventArgs(SqlNotificationType.Subscribe, SqlNotificationInfo.AlreadyChanged, SqlNotificationSource.Client);
							}
							else
							{
								Bid.NotificationsTrace("<sc.SqlDependency.OnChange-Add|DEP> Dependency has not fired, adding new event.\n");
								SqlDependency.EventContextPair eventContextPair = new SqlDependency.EventContextPair(value, this);
								if (this._eventList.Contains(eventContextPair))
								{
									throw SQL.SqlDependencyEventNoDuplicate();
								}
								this._eventList.Add(eventContextPair);
							}
						}
						if (sqlNotificationEventArgs != null)
						{
							value(this, sqlNotificationEventArgs);
						}
					}
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
			remove
			{
				IntPtr intPtr;
				Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.OnChange-Remove|DEP> %d#", this.ObjectID);
				try
				{
					if (value != null)
					{
						SqlDependency.EventContextPair eventContextPair = new SqlDependency.EventContextPair(value, this);
						lock (this._eventHandlerLock)
						{
							int num = this._eventList.IndexOf(eventContextPair);
							if (0 <= num)
							{
								this._eventList.RemoveAt(num);
							}
						}
					}
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x00283AE4 File Offset: 0x00282EE4
		[ResCategory("DataCategory_Data")]
		[ResDescription("SqlDependency_AddCommandDependency")]
		public void AddCommandDependency(SqlCommand command)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.AddCommandDependency|DEP> %d#", this.ObjectID);
			try
			{
				if (command == null)
				{
					throw ADP.ArgumentNull("command");
				}
				this.AddCommandInternal(command);
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x00283B40 File Offset: 0x00282F40
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		private static ObjectHandle CreateProcessDispatcher(_AppDomain masterDomain)
		{
			return masterDomain.CreateInstance(SqlDependency._assemblyName, SqlDependency._typeName);
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x00283B60 File Offset: 0x00282F60
		private static void ObtainProcessDispatcher()
		{
			byte[] data = SNINativeMethodWrapper.GetData();
			if (data != null)
			{
				Bid.NotificationsTrace("<sc.SqlDependency.ObtainProcessDispatcher|DEP> nativeStorage not null, obtaining existing dispatcher AppDomain and ProcessDispatcher.\n");
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				MemoryStream memoryStream = new MemoryStream(data);
				SqlDependency._processDispatcher = SqlDependency.GetDeserializedObject(binaryFormatter, memoryStream);
				Bid.NotificationsTrace("<sc.SqlDependency.ObtainProcessDispatcher|DEP> processDispatcher obtained, ID: %d\n", SqlDependency._processDispatcher.ObjectID);
				return;
			}
			Bid.NotificationsTrace("<sc.SqlDependency.ObtainProcessDispatcher|DEP> nativeStorage null, obtaining dispatcher AppDomain and creating ProcessDispatcher.\n");
			_AppDomain defaultAppDomain = SNINativeMethodWrapper.GetDefaultAppDomain();
			if (defaultAppDomain == null)
			{
				Bid.NotificationsTrace("<sc.SqlDependency.ObtainProcessDispatcher|DEP|ERR> ERROR - unable to obtain default AppDomain!\n");
				throw ADP.InternalError(ADP.InternalErrorCode.SqlDependencyProcessDispatcherFailureAppDomain);
			}
			ObjectHandle objectHandle = SqlDependency.CreateProcessDispatcher(defaultAppDomain);
			if (objectHandle == null)
			{
				Bid.NotificationsTrace("<sc.SqlDependency.ObtainProcessDispatcher|DEP|ERR> ERROR - AppDomain.CreateInstance returned null!\n");
				throw ADP.InternalError(ADP.InternalErrorCode.SqlDependencyProcessDispatcherFailureCreateInstance);
			}
			SqlDependencyProcessDispatcher sqlDependencyProcessDispatcher = (SqlDependencyProcessDispatcher)objectHandle.Unwrap();
			if (sqlDependencyProcessDispatcher != null)
			{
				SqlDependency._processDispatcher = sqlDependencyProcessDispatcher.SingletonProcessDispatcher;
				ObjRef objRef = SqlDependency.GetObjRef(SqlDependency._processDispatcher);
				BinaryFormatter binaryFormatter2 = new BinaryFormatter();
				MemoryStream memoryStream2 = new MemoryStream();
				SqlDependency.GetSerializedObject(objRef, binaryFormatter2, memoryStream2);
				SNINativeMethodWrapper.SetData(memoryStream2.GetBuffer());
				return;
			}
			Bid.NotificationsTrace("<sc.SqlDependency.ObtainProcessDispatcher|DEP|ERR> ERROR - ObjectHandle.Unwrap returned null!\n");
			throw ADP.InternalError(ADP.InternalErrorCode.SqlDependencyObtainProcessDispatcherFailureObjectHandle);
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x00283C58 File Offset: 0x00283058
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		private static ObjRef GetObjRef(SqlDependencyProcessDispatcher _processDispatcher)
		{
			return RemotingServices.Marshal(_processDispatcher);
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x00283C6C File Offset: 0x0028306C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private static void GetSerializedObject(ObjRef objRef, BinaryFormatter formatter, MemoryStream stream)
		{
			formatter.Serialize(stream, objRef);
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x00283C84 File Offset: 0x00283084
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private static SqlDependencyProcessDispatcher GetDeserializedObject(BinaryFormatter formatter, MemoryStream stream)
		{
			object obj = formatter.Deserialize(stream);
			return (SqlDependencyProcessDispatcher)obj;
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x00283CA0 File Offset: 0x002830A0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public static bool Start(string connectionString)
		{
			return SqlDependency.Start(connectionString, null, true);
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x00283CB8 File Offset: 0x002830B8
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public static bool Start(string connectionString, string queue)
		{
			return SqlDependency.Start(connectionString, queue, false);
		}

		// Token: 0x060026AD RID: 9901 RVA: 0x00283CD0 File Offset: 0x002830D0
		internal static bool Start(string connectionString, string queue, bool useDefaults)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.Start|DEP> AppDomainKey: '%ls', queue: '%ls'", SqlDependency.AppDomainKey, queue);
			bool flag4;
			try
			{
				if (InOutOfProcHelper.InProc)
				{
					throw SQL.SqlDepCannotBeCreatedInProc();
				}
				if (ADP.IsEmpty(connectionString))
				{
					if (connectionString == null)
					{
						throw ADP.ArgumentNull("connectionString");
					}
					throw ADP.Argument("connectionString");
				}
				else
				{
					if (!useDefaults && ADP.IsEmpty(queue))
					{
						useDefaults = true;
						queue = null;
					}
					SqlConnectionString sqlConnectionString = new SqlConnectionString(connectionString);
					sqlConnectionString.DemandPermission();
					if (sqlConnectionString.LocalDBInstance != null)
					{
						LocalDBAPI.DemandLocalDBPermissions();
					}
					bool flag = false;
					bool flag2 = false;
					lock (SqlDependency._startStopLock)
					{
						try
						{
							if (SqlDependency._processDispatcher == null)
							{
								SqlDependency.ObtainProcessDispatcher();
							}
							if (useDefaults)
							{
								string text = null;
								DbConnectionPoolIdentity dbConnectionPoolIdentity = null;
								string text2 = null;
								string text3 = null;
								string text4 = null;
								bool flag3 = false;
								RuntimeHelpers.PrepareConstrainedRegions();
								try
								{
									flag2 = SqlDependency._processDispatcher.StartWithDefault(connectionString, out text, out dbConnectionPoolIdentity, out text2, out text3, ref text4, SqlDependency._appDomainKey, SqlDependencyPerAppDomainDispatcher.SingletonInstance, out flag, out flag3);
									Bid.NotificationsTrace("<sc.SqlDependency.Start|DEP> Start (defaults) returned: '%d', with service: '%ls', server: '%ls', database: '%ls'\n", flag2, text4, text, text3);
									goto IL_015D;
								}
								finally
								{
									if (flag3 && !flag)
									{
										SqlDependency.IdentityUserNamePair identityUserNamePair = new SqlDependency.IdentityUserNamePair(dbConnectionPoolIdentity, text2);
										SqlDependency.DatabaseServicePair databaseServicePair = new SqlDependency.DatabaseServicePair(text3, text4);
										if (!SqlDependency.AddToServerUserHash(text, identityUserNamePair, databaseServicePair))
										{
											try
											{
												SqlDependency.Stop(connectionString, queue, useDefaults, true);
											}
											catch (Exception ex)
											{
												if (!ADP.IsCatchableExceptionType(ex))
												{
													throw;
												}
												ADP.TraceExceptionWithoutRethrow(ex);
												Bid.NotificationsTrace("<sc.SqlDependency.Start|DEP|ERR> Exception occurred from Stop() after duplicate was found on Start().\n");
											}
											throw SQL.SqlDependencyDuplicateStart();
										}
									}
								}
							}
							flag2 = SqlDependency._processDispatcher.Start(connectionString, queue, SqlDependency._appDomainKey, SqlDependencyPerAppDomainDispatcher.SingletonInstance);
							Bid.NotificationsTrace("<sc.SqlDependency.Start|DEP> Start (user provided queue) returned: '%d'\n", flag2);
							IL_015D:;
						}
						catch (Exception ex2)
						{
							if (!ADP.IsCatchableExceptionType(ex2))
							{
								throw;
							}
							ADP.TraceExceptionWithoutRethrow(ex2);
							Bid.NotificationsTrace("<sc.SqlDependency.Start|DEP|ERR> Exception occurred from _processDispatcher.Start(...), calling Invalidate(...).\n");
							throw;
						}
					}
					flag4 = flag2;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag4;
		}

		// Token: 0x060026AE RID: 9902 RVA: 0x00283EF0 File Offset: 0x002832F0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public static bool Stop(string connectionString)
		{
			return SqlDependency.Stop(connectionString, null, true, false);
		}

		// Token: 0x060026AF RID: 9903 RVA: 0x00283F08 File Offset: 0x00283308
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public static bool Stop(string connectionString, string queue)
		{
			return SqlDependency.Stop(connectionString, queue, false, false);
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x00283F20 File Offset: 0x00283320
		internal static bool Stop(string connectionString, string queue, bool useDefaults, bool startFailed)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.Stop|DEP> AppDomainKey: '%ls', queue: '%ls'", SqlDependency.AppDomainKey, queue);
			bool flag4;
			try
			{
				if (InOutOfProcHelper.InProc)
				{
					throw SQL.SqlDepCannotBeCreatedInProc();
				}
				if (ADP.IsEmpty(connectionString))
				{
					if (connectionString == null)
					{
						throw ADP.ArgumentNull("connectionString");
					}
					throw ADP.Argument("connectionString");
				}
				else
				{
					if (!useDefaults && ADP.IsEmpty(queue))
					{
						useDefaults = true;
						queue = null;
					}
					SqlConnectionString sqlConnectionString = new SqlConnectionString(connectionString);
					sqlConnectionString.DemandPermission();
					if (sqlConnectionString.LocalDBInstance != null)
					{
						LocalDBAPI.DemandLocalDBPermissions();
					}
					bool flag = false;
					lock (SqlDependency._startStopLock)
					{
						if (SqlDependency._processDispatcher != null)
						{
							try
							{
								string text = null;
								DbConnectionPoolIdentity dbConnectionPoolIdentity = null;
								string text2 = null;
								string text3 = null;
								string text4 = null;
								if (useDefaults)
								{
									bool flag2 = false;
									RuntimeHelpers.PrepareConstrainedRegions();
									try
									{
										flag = SqlDependency._processDispatcher.Stop(connectionString, out text, out dbConnectionPoolIdentity, out text2, out text3, ref text4, SqlDependency._appDomainKey, out flag2);
										goto IL_0106;
									}
									finally
									{
										if (flag2 && !startFailed)
										{
											SqlDependency.IdentityUserNamePair identityUserNamePair = new SqlDependency.IdentityUserNamePair(dbConnectionPoolIdentity, text2);
											SqlDependency.DatabaseServicePair databaseServicePair = new SqlDependency.DatabaseServicePair(text3, text4);
											SqlDependency.RemoveFromServerUserHash(text, identityUserNamePair, databaseServicePair);
										}
									}
								}
								bool flag3 = false;
								flag = SqlDependency._processDispatcher.Stop(connectionString, out text, out dbConnectionPoolIdentity, out text2, out text3, ref queue, SqlDependency._appDomainKey, out flag3);
								IL_0106:;
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
					flag4 = flag;
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag4;
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x002840C4 File Offset: 0x002834C4
		private static bool AddToServerUserHash(string server, SqlDependency.IdentityUserNamePair identityUser, SqlDependency.DatabaseServicePair databaseService)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.AddToServerUserHash|DEP> server: '%ls', database: '%ls', service: '%ls'", server, databaseService.Database, databaseService.Service);
			bool flag2;
			try
			{
				bool flag = false;
				lock (SqlDependency._serverUserHash)
				{
					Dictionary<SqlDependency.IdentityUserNamePair, List<SqlDependency.DatabaseServicePair>> dictionary;
					if (!SqlDependency._serverUserHash.ContainsKey(server))
					{
						Bid.NotificationsTrace("<sc.SqlDependency.AddToServerUserHash|DEP> Hash did not contain server, adding.\n");
						dictionary = new Dictionary<SqlDependency.IdentityUserNamePair, List<SqlDependency.DatabaseServicePair>>();
						SqlDependency._serverUserHash.Add(server, dictionary);
					}
					else
					{
						dictionary = SqlDependency._serverUserHash[server];
					}
					List<SqlDependency.DatabaseServicePair> list;
					if (!dictionary.ContainsKey(identityUser))
					{
						Bid.NotificationsTrace("<sc.SqlDependency.AddToServerUserHash|DEP> Hash contained server but not user, adding user.\n");
						list = new List<SqlDependency.DatabaseServicePair>();
						dictionary.Add(identityUser, list);
					}
					else
					{
						list = dictionary[identityUser];
					}
					if (!list.Contains(databaseService))
					{
						Bid.NotificationsTrace("<sc.SqlDependency.AddToServerUserHash|DEP> Adding database.\n");
						list.Add(databaseService);
						flag = true;
					}
					else
					{
						Bid.NotificationsTrace("<sc.SqlDependency.AddToServerUserHash|DEP|ERR> ERROR - hash already contained server, user, and database - we will throw!.\n");
					}
				}
				flag2 = flag;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return flag2;
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x002841D0 File Offset: 0x002835D0
		private static void RemoveFromServerUserHash(string server, SqlDependency.IdentityUserNamePair identityUser, SqlDependency.DatabaseServicePair databaseService)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.RemoveFromServerUserHash|DEP> server: '%ls', database: '%ls', service: '%ls'", server, databaseService.Database, databaseService.Service);
			try
			{
				lock (SqlDependency._serverUserHash)
				{
					if (SqlDependency._serverUserHash.ContainsKey(server))
					{
						Dictionary<SqlDependency.IdentityUserNamePair, List<SqlDependency.DatabaseServicePair>> dictionary = SqlDependency._serverUserHash[server];
						if (dictionary.ContainsKey(identityUser))
						{
							List<SqlDependency.DatabaseServicePair> list = dictionary[identityUser];
							int num = list.IndexOf(databaseService);
							if (num >= 0)
							{
								Bid.NotificationsTrace("<sc.SqlDependency.RemoveFromServerUserHash|DEP> Hash contained server, user, and database - removing database.\n");
								list.RemoveAt(num);
								if (list.Count == 0)
								{
									Bid.NotificationsTrace("<sc.SqlDependency.RemoveFromServerUserHash|DEP> databaseServiceList count 0, removing the list for this server and user.\n");
									dictionary.Remove(identityUser);
									if (dictionary.Count == 0)
									{
										Bid.NotificationsTrace("<sc.SqlDependency.RemoveFromServerUserHash|DEP> identityDatabaseHash count 0, removing the hash for this server.\n");
										SqlDependency._serverUserHash.Remove(server);
									}
								}
							}
							else
							{
								Bid.NotificationsTrace("<sc.SqlDependency.RemoveFromServerUserHash|DEP|ERR> ERROR - hash contained server and user but not database!\n");
							}
						}
						else
						{
							Bid.NotificationsTrace("<sc.SqlDependency.RemoveFromServerUserHash|DEP|ERR> ERROR - hash contained server but not user!\n");
						}
					}
					else
					{
						Bid.NotificationsTrace("<sc.SqlDependency.RemoveFromServerUserHash|DEP|ERR> ERROR - hash did not contain server!\n");
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x002842F0 File Offset: 0x002836F0
		internal static string GetDefaultComposedOptions(string server, string failoverServer, SqlDependency.IdentityUserNamePair identityUser, string database)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.GetDefaultComposedOptions|DEP> server: '%ls', failoverServer: '%ls', database: '%ls'", server, failoverServer, database);
			string text5;
			try
			{
				string text2;
				lock (SqlDependency._serverUserHash)
				{
					if (!SqlDependency._serverUserHash.ContainsKey(server))
					{
						if (SqlDependency._serverUserHash.Count == 0)
						{
							Bid.NotificationsTrace("<sc.SqlDependency.GetDefaultComposedOptions|DEP|ERR> ERROR - no start calls have been made, about to throw.\n");
							throw SQL.SqlDepDefaultOptionsButNoStart();
						}
						if (ADP.IsEmpty(failoverServer) || !SqlDependency._serverUserHash.ContainsKey(failoverServer))
						{
							Bid.NotificationsTrace("<sc.SqlDependency.GetDefaultComposedOptions|DEP|ERR> ERROR - not listening to this server, about to throw.\n");
							throw SQL.SqlDependencyNoMatchingServerStart();
						}
						Bid.NotificationsTrace("<sc.SqlDependency.GetDefaultComposedOptions|DEP> using failover server instead\n");
						server = failoverServer;
					}
					Dictionary<SqlDependency.IdentityUserNamePair, List<SqlDependency.DatabaseServicePair>> dictionary = SqlDependency._serverUserHash[server];
					List<SqlDependency.DatabaseServicePair> list = null;
					if (!dictionary.ContainsKey(identityUser))
					{
						if (dictionary.Count > 1)
						{
							Bid.NotificationsTrace("<sc.SqlDependency.GetDefaultComposedOptions|DEP|ERR> ERROR - not listening for this user, but listening to more than one other user, about to throw.\n");
							throw SQL.SqlDependencyNoMatchingServerStart();
						}
						using (Dictionary<SqlDependency.IdentityUserNamePair, List<SqlDependency.DatabaseServicePair>>.Enumerator enumerator = dictionary.GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								KeyValuePair<SqlDependency.IdentityUserNamePair, List<SqlDependency.DatabaseServicePair>> keyValuePair = enumerator.Current;
								list = keyValuePair.Value;
							}
							goto IL_00E7;
						}
					}
					list = dictionary[identityUser];
					IL_00E7:
					SqlDependency.DatabaseServicePair databaseServicePair = new SqlDependency.DatabaseServicePair(database, null);
					SqlDependency.DatabaseServicePair databaseServicePair2 = null;
					int num = list.IndexOf(databaseServicePair);
					if (num != -1)
					{
						databaseServicePair2 = list[num];
					}
					if (databaseServicePair2 != null)
					{
						database = SqlDependency.FixupServiceOrDatabaseName(databaseServicePair2.Database);
						string text = SqlDependency.FixupServiceOrDatabaseName(databaseServicePair2.Service);
						text2 = "Service=" + text + ";Local Database=" + database;
					}
					else
					{
						if (list.Count != 1)
						{
							Bid.NotificationsTrace("<sc.SqlDependency.GetDefaultComposedOptions|DEP|ERR> ERROR - SqlDependency.Start called multiple times for this server/user, but no matching database.\n");
							throw SQL.SqlDependencyNoMatchingServerDatabaseStart();
						}
						object[] array = list.ToArray();
						databaseServicePair2 = (SqlDependency.DatabaseServicePair)array[0];
						string text3 = SqlDependency.FixupServiceOrDatabaseName(databaseServicePair2.Database);
						string text4 = SqlDependency.FixupServiceOrDatabaseName(databaseServicePair2.Service);
						text2 = "Service=" + text4 + ";Local Database=" + text3;
					}
				}
				Bid.NotificationsTrace("<sc.SqlDependency.GetDefaultComposedOptions|DEP> resulting options: '%ls'.\n", text2);
				text5 = text2;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return text5;
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x00284504 File Offset: 0x00283904
		internal void AddToServerList(string server)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.AddToServerList|DEP> %d#, server: '%ls'", this.ObjectID, server);
			try
			{
				lock (this._serverList)
				{
					int num = this._serverList.BinarySearch(server, StringComparer.OrdinalIgnoreCase);
					if (0 > num)
					{
						Bid.NotificationsTrace("<sc.SqlDependency.AddToServerList|DEP> Server not present in hashtable, adding server: '%ls'.\n", server);
						num = ~num;
						this._serverList.Insert(num, server);
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x002845AC File Offset: 0x002839AC
		internal string ComputeHashAndAddToDispatcher(SqlCommand command)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.ComputeHashAndAddToDispatcher|DEP> %d#, SqlCommand: %d#", this.ObjectID, command.ObjectID);
			string text3;
			try
			{
				string text = this.ComputeCommandHash(command.Connection.ConnectionString, command);
				string text2 = SqlDependencyPerAppDomainDispatcher.SingletonInstance.AddCommandEntry(text, this);
				Bid.NotificationsTrace("<sc.SqlDependency.ComputeHashAndAddToDispatcher|DEP> computed id string: '%ls'.\n", text2);
				text3 = text2;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return text3;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x00284628 File Offset: 0x00283A28
		internal void Invalidate(SqlNotificationType type, SqlNotificationInfo info, SqlNotificationSource source)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.Invalidate|DEP> %d#", this.ObjectID);
			try
			{
				List<SqlDependency.EventContextPair> list = null;
				lock (this._eventHandlerLock)
				{
					if (this._dependencyFired && SqlNotificationInfo.AlreadyChanged != info && SqlNotificationSource.Client != source)
					{
						Bid.NotificationsTrace("<sc.SqlDependency.Invalidate|DEP|ERR> ERROR - notification received twice - we should never enter this state!");
					}
					else
					{
						this._dependencyFired = true;
						list = this._eventList;
						this._eventList = new List<SqlDependency.EventContextPair>();
					}
				}
				if (list != null)
				{
					Bid.NotificationsTrace("<sc.SqlDependency.Invalidate|DEP> Firing events.\n");
					foreach (SqlDependency.EventContextPair eventContextPair in list)
					{
						eventContextPair.Invoke(new SqlNotificationEventArgs(type, info, source));
					}
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x00284734 File Offset: 0x00283B34
		internal void StartTimer(SqlNotificationRequest notificationRequest)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.StartTimer|DEP> %d#", this.ObjectID);
			try
			{
				if (this._expirationTime == DateTime.MaxValue)
				{
					Bid.NotificationsTrace("<sc.SqlDependency.StartTimer|DEP> We've timed out, executing logic.\n");
					int num = 432000;
					if (this._timeout != 0)
					{
						num = this._timeout;
					}
					if (notificationRequest != null && notificationRequest.Timeout < num && notificationRequest.Timeout != 0)
					{
						num = notificationRequest.Timeout;
					}
					this._expirationTime = DateTime.UtcNow.AddSeconds((double)num);
					SqlDependencyPerAppDomainDispatcher.SingletonInstance.StartTimer(this);
				}
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x002847E8 File Offset: 0x00283BE8
		private void AddCommandInternal(SqlCommand cmd)
		{
			if (cmd != null)
			{
				IntPtr intPtr;
				Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.AddCommandInternal|DEP> %d#, SqlCommand: %d#", this.ObjectID, cmd.ObjectID);
				try
				{
					if (cmd.Notification != null)
					{
						if (cmd._sqlDep == null || cmd._sqlDep != this)
						{
							Bid.NotificationsTrace("<sc.SqlDependency.AddCommandInternal|DEP|ERR> ERROR - throwing command has existing SqlNotificationRequest exception.\n");
							throw SQL.SqlCommandHasExistingSqlNotificationRequest();
						}
					}
					else
					{
						bool flag = false;
						lock (this._eventHandlerLock)
						{
							if (!this._dependencyFired)
							{
								cmd.Notification = new SqlNotificationRequest();
								cmd.Notification.Timeout = this._timeout;
								if (this._options != null)
								{
									cmd.Notification.Options = this._options;
								}
								cmd._sqlDep = this;
							}
							else if (this._eventList.Count == 0)
							{
								Bid.NotificationsTrace("<sc.SqlDependency.AddCommandInternal|DEP|ERR> ERROR - firing events, though it is unexpected we have events at this point.\n");
								flag = true;
							}
						}
						if (flag)
						{
							this.Invalidate(SqlNotificationType.Subscribe, SqlNotificationInfo.AlreadyChanged, SqlNotificationSource.Client);
						}
					}
				}
				finally
				{
					Bid.ScopeLeave(ref intPtr);
				}
			}
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x00284904 File Offset: 0x00283D04
		private string ComputeCommandHash(string connectionString, SqlCommand command)
		{
			IntPtr intPtr;
			Bid.NotificationsScopeEnter(out intPtr, "<sc.SqlDependency.ComputeCommandHash|DEP> %d#, SqlCommand: %d#", this.ObjectID, command.ObjectID);
			string text2;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("{0};{1}", connectionString, command.CommandText);
				for (int i = 0; i < command.Parameters.Count; i++)
				{
					object value = command.Parameters[i].Value;
					if (value == null || value == DBNull.Value)
					{
						stringBuilder.Append("; NULL");
					}
					else
					{
						Type type = value.GetType();
						if (type == typeof(byte[]))
						{
							stringBuilder.Append(";");
							byte[] array = (byte[])value;
							for (int j = 0; j < array.Length; j++)
							{
								stringBuilder.Append(array[j].ToString("x2", CultureInfo.InvariantCulture));
							}
						}
						else if (type == typeof(char[]))
						{
							stringBuilder.Append((char[])value);
						}
						else if (type == typeof(XmlReader))
						{
							stringBuilder.Append(";");
							stringBuilder.Append(Guid.NewGuid().ToString());
						}
						else
						{
							stringBuilder.Append(";");
							stringBuilder.Append(value.ToString());
						}
					}
				}
				string text = stringBuilder.ToString();
				Bid.NotificationsTrace("<sc.SqlDependency.ComputeCommandHash|DEP> ComputeCommandHash result: '%ls'.\n", text);
				text2 = text;
			}
			finally
			{
				Bid.ScopeLeave(ref intPtr);
			}
			return text2;
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x00284A94 File Offset: 0x00283E94
		internal static string FixupServiceOrDatabaseName(string name)
		{
			if (!ADP.IsEmpty(name))
			{
				return "\"" + name.Replace("\"", "\"\"") + "\"";
			}
			return name;
		}

		// Token: 0x04001845 RID: 6213
		internal const Bid.ApiGroup NotificationsTracePoints = Bid.ApiGroup.Dependency;

		// Token: 0x04001846 RID: 6214
		private readonly string _id = Guid.NewGuid().ToString() + ";" + SqlDependency._appDomainKey;

		// Token: 0x04001847 RID: 6215
		private string _options;

		// Token: 0x04001848 RID: 6216
		private int _timeout;

		// Token: 0x04001849 RID: 6217
		private bool _dependencyFired;

		// Token: 0x0400184A RID: 6218
		private List<SqlDependency.EventContextPair> _eventList = new List<SqlDependency.EventContextPair>();

		// Token: 0x0400184B RID: 6219
		private object _eventHandlerLock = new object();

		// Token: 0x0400184C RID: 6220
		private DateTime _expirationTime = DateTime.MaxValue;

		// Token: 0x0400184D RID: 6221
		private List<string> _serverList = new List<string>();

		// Token: 0x0400184E RID: 6222
		private static object _startStopLock = new object();

		// Token: 0x0400184F RID: 6223
		private static readonly string _appDomainKey = Guid.NewGuid().ToString();

		// Token: 0x04001850 RID: 6224
		private static Dictionary<string, Dictionary<SqlDependency.IdentityUserNamePair, List<SqlDependency.DatabaseServicePair>>> _serverUserHash = new Dictionary<string, Dictionary<SqlDependency.IdentityUserNamePair, List<SqlDependency.DatabaseServicePair>>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04001851 RID: 6225
		private static SqlDependencyProcessDispatcher _processDispatcher = null;

		// Token: 0x04001852 RID: 6226
		private static readonly string _assemblyName = typeof(SqlDependencyProcessDispatcher).Assembly.FullName;

		// Token: 0x04001853 RID: 6227
		private static readonly string _typeName = typeof(SqlDependencyProcessDispatcher).FullName;

		// Token: 0x04001854 RID: 6228
		private readonly int _objectID = Interlocked.Increment(ref SqlDependency._objectTypeCount);

		// Token: 0x04001855 RID: 6229
		private static int _objectTypeCount;

		// Token: 0x020002E5 RID: 741
		internal class IdentityUserNamePair
		{
			// Token: 0x060026BC RID: 9916 RVA: 0x00284B40 File Offset: 0x00283F40
			internal IdentityUserNamePair(DbConnectionPoolIdentity identity, string userName)
			{
				this._identity = identity;
				this._userName = userName;
			}

			// Token: 0x17000619 RID: 1561
			// (get) Token: 0x060026BD RID: 9917 RVA: 0x00284B64 File Offset: 0x00283F64
			internal DbConnectionPoolIdentity Identity
			{
				get
				{
					return this._identity;
				}
			}

			// Token: 0x1700061A RID: 1562
			// (get) Token: 0x060026BE RID: 9918 RVA: 0x00284B78 File Offset: 0x00283F78
			internal string UserName
			{
				get
				{
					return this._userName;
				}
			}

			// Token: 0x060026BF RID: 9919 RVA: 0x00284B8C File Offset: 0x00283F8C
			public override bool Equals(object value)
			{
				SqlDependency.IdentityUserNamePair identityUserNamePair = (SqlDependency.IdentityUserNamePair)value;
				bool flag = false;
				if (identityUserNamePair == null)
				{
					flag = false;
				}
				else if (this == identityUserNamePair)
				{
					flag = true;
				}
				else if (this._identity != null)
				{
					if (this._identity.Equals(identityUserNamePair._identity))
					{
						flag = true;
					}
				}
				else if (this._userName == identityUserNamePair._userName)
				{
					flag = true;
				}
				return flag;
			}

			// Token: 0x060026C0 RID: 9920 RVA: 0x00284BE8 File Offset: 0x00283FE8
			public override int GetHashCode()
			{
				int num;
				if (this._identity != null)
				{
					num = this._identity.GetHashCode();
				}
				else
				{
					num = this._userName.GetHashCode();
				}
				return num;
			}

			// Token: 0x04001856 RID: 6230
			private DbConnectionPoolIdentity _identity;

			// Token: 0x04001857 RID: 6231
			private string _userName;
		}

		// Token: 0x020002E6 RID: 742
		private class DatabaseServicePair
		{
			// Token: 0x060026C1 RID: 9921 RVA: 0x00284C1C File Offset: 0x0028401C
			internal DatabaseServicePair(string database, string service)
			{
				this._database = database;
				this._service = service;
			}

			// Token: 0x1700061B RID: 1563
			// (get) Token: 0x060026C2 RID: 9922 RVA: 0x00284C40 File Offset: 0x00284040
			internal string Database
			{
				get
				{
					return this._database;
				}
			}

			// Token: 0x1700061C RID: 1564
			// (get) Token: 0x060026C3 RID: 9923 RVA: 0x00284C54 File Offset: 0x00284054
			internal string Service
			{
				get
				{
					return this._service;
				}
			}

			// Token: 0x060026C4 RID: 9924 RVA: 0x00284C68 File Offset: 0x00284068
			public override bool Equals(object value)
			{
				SqlDependency.DatabaseServicePair databaseServicePair = (SqlDependency.DatabaseServicePair)value;
				bool flag = false;
				if (databaseServicePair == null)
				{
					flag = false;
				}
				else if (this == databaseServicePair)
				{
					flag = true;
				}
				else if (this._database == databaseServicePair._database)
				{
					flag = true;
				}
				return flag;
			}

			// Token: 0x060026C5 RID: 9925 RVA: 0x00284CA4 File Offset: 0x002840A4
			public override int GetHashCode()
			{
				return this._database.GetHashCode();
			}

			// Token: 0x04001858 RID: 6232
			private string _database;

			// Token: 0x04001859 RID: 6233
			private string _service;
		}

		// Token: 0x020002E7 RID: 743
		internal class EventContextPair
		{
			// Token: 0x060026C6 RID: 9926 RVA: 0x00284CBC File Offset: 0x002840BC
			internal EventContextPair(OnChangeEventHandler eventHandler, SqlDependency dependency)
			{
				this._eventHandler = eventHandler;
				this._context = ExecutionContext.Capture();
				this._dependency = dependency;
			}

			// Token: 0x060026C7 RID: 9927 RVA: 0x00284CE8 File Offset: 0x002840E8
			public override bool Equals(object value)
			{
				SqlDependency.EventContextPair eventContextPair = (SqlDependency.EventContextPair)value;
				bool flag = false;
				if (eventContextPair == null)
				{
					flag = false;
				}
				else if (this == eventContextPair)
				{
					flag = true;
				}
				else if (this._eventHandler == eventContextPair._eventHandler)
				{
					flag = true;
				}
				return flag;
			}

			// Token: 0x060026C8 RID: 9928 RVA: 0x00284D24 File Offset: 0x00284124
			public override int GetHashCode()
			{
				return this._eventHandler.GetHashCode();
			}

			// Token: 0x060026C9 RID: 9929 RVA: 0x00284D3C File Offset: 0x0028413C
			internal void Invoke(SqlNotificationEventArgs args)
			{
				this._args = args;
				ExecutionContext.Run(this._context, SqlDependency.EventContextPair._contextCallback, this);
			}

			// Token: 0x060026CA RID: 9930 RVA: 0x00284D64 File Offset: 0x00284164
			private static void InvokeCallback(object eventContextPair)
			{
				SqlDependency.EventContextPair eventContextPair2 = (SqlDependency.EventContextPair)eventContextPair;
				eventContextPair2._eventHandler(eventContextPair2._dependency, eventContextPair2._args);
			}

			// Token: 0x0400185A RID: 6234
			private OnChangeEventHandler _eventHandler;

			// Token: 0x0400185B RID: 6235
			private ExecutionContext _context;

			// Token: 0x0400185C RID: 6236
			private SqlDependency _dependency;

			// Token: 0x0400185D RID: 6237
			private SqlNotificationEventArgs _args;

			// Token: 0x0400185E RID: 6238
			private static ContextCallback _contextCallback = new ContextCallback(SqlDependency.EventContextPair.InvokeCallback);
		}
	}
}
