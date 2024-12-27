using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.ProviderBase;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.SqlServer.Server;

namespace System.Data.SqlClient
{
	// Token: 0x0200031C RID: 796
	internal sealed class TdsParser
	{
		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x060029C5 RID: 10693 RVA: 0x00293E50 File Offset: 0x00293250
		internal int ObjectID
		{
			get
			{
				return this._objectID;
			}
		}

		// Token: 0x060029C6 RID: 10694 RVA: 0x00293E64 File Offset: 0x00293264
		internal TdsParser(bool MARS, bool fAsynchronous)
		{
			this._fMARS = MARS;
			this._fAsync = fAsynchronous;
			this._physicalStateObj = new TdsParserStateObject(this);
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x060029C7 RID: 10695 RVA: 0x00293ED0 File Offset: 0x002932D0
		internal bool AsyncOn
		{
			get
			{
				return this._fAsync;
			}
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x060029C8 RID: 10696 RVA: 0x00293EE4 File Offset: 0x002932E4
		internal SqlInternalConnectionTds Connection
		{
			get
			{
				return this._connHandler;
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x060029C9 RID: 10697 RVA: 0x00293EF8 File Offset: 0x002932F8
		// (set) Token: 0x060029CA RID: 10698 RVA: 0x00293F0C File Offset: 0x0029330C
		internal SqlInternalTransaction CurrentTransaction
		{
			get
			{
				return this._currentTransaction;
			}
			set
			{
				if ((this._currentTransaction == null && value != null) || (this._currentTransaction != null && value == null))
				{
					this._currentTransaction = value;
				}
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x060029CB RID: 10699 RVA: 0x00293F38 File Offset: 0x00293338
		internal int DefaultLCID
		{
			get
			{
				return this._defaultLCID;
			}
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x00293F4C File Offset: 0x0029334C
		// (set) Token: 0x060029CD RID: 10701 RVA: 0x00293F60 File Offset: 0x00293360
		internal EncryptionOptions EncryptionOptions
		{
			get
			{
				return this._encryptionOption;
			}
			set
			{
				this._encryptionOption = value;
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x00293F74 File Offset: 0x00293374
		internal SqlErrorCollection Errors
		{
			get
			{
				SqlErrorCollection errors;
				lock (this._ErrorCollectionLock)
				{
					if (this._errors == null)
					{
						this._errors = new SqlErrorCollection();
					}
					errors = this._errors;
				}
				return errors;
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060029CF RID: 10703 RVA: 0x00293FD0 File Offset: 0x002933D0
		internal bool IsYukonOrNewer
		{
			get
			{
				return this._isYukon;
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x00293FE4 File Offset: 0x002933E4
		internal bool IsKatmaiOrNewer
		{
			get
			{
				return this._isKatmai;
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x060029D1 RID: 10705 RVA: 0x00293FF8 File Offset: 0x002933F8
		internal bool MARSOn
		{
			get
			{
				return this._fMARS;
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x060029D2 RID: 10706 RVA: 0x0029400C File Offset: 0x0029340C
		// (set) Token: 0x060029D3 RID: 10707 RVA: 0x00294020 File Offset: 0x00293420
		internal SqlInternalTransaction PendingTransaction
		{
			get
			{
				return this._pendingTransaction;
			}
			set
			{
				this._pendingTransaction = value;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x060029D4 RID: 10708 RVA: 0x00294034 File Offset: 0x00293434
		internal string Server
		{
			get
			{
				return this._server;
			}
		}

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060029D5 RID: 10709 RVA: 0x00294048 File Offset: 0x00293448
		// (set) Token: 0x060029D6 RID: 10710 RVA: 0x0029405C File Offset: 0x0029345C
		internal TdsParserState State
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}

		// Token: 0x170006DF RID: 1759
		// (get) Token: 0x060029D7 RID: 10711 RVA: 0x00294070 File Offset: 0x00293470
		// (set) Token: 0x060029D8 RID: 10712 RVA: 0x00294084 File Offset: 0x00293484
		internal SqlStatistics Statistics
		{
			get
			{
				return this._statistics;
			}
			set
			{
				this._statistics = value;
			}
		}

		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060029D9 RID: 10713 RVA: 0x00294098 File Offset: 0x00293498
		internal SqlErrorCollection Warnings
		{
			get
			{
				SqlErrorCollection warnings;
				lock (this._ErrorCollectionLock)
				{
					if (this._warnings == null)
					{
						this._warnings = new SqlErrorCollection();
					}
					warnings = this._warnings;
				}
				return warnings;
			}
		}

		// Token: 0x060029DA RID: 10714 RVA: 0x002940F4 File Offset: 0x002934F4
		internal int IncrementNonTransactedOpenResultCount()
		{
			return Interlocked.Increment(ref this._nonTransactedOpenResultCount);
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x00294110 File Offset: 0x00293510
		internal void DecrementNonTransactedOpenResultCount()
		{
			Interlocked.Decrement(ref this._nonTransactedOpenResultCount);
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x0029412C File Offset: 0x0029352C
		internal void ProcessPendingAck(TdsParserStateObject stateObj)
		{
			if (stateObj._attentionSent)
			{
				this.ProcessAttention(stateObj);
			}
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x00294148 File Offset: 0x00293548
		internal void Connect(ServerInfo serverInfo, SqlInternalConnectionTds connHandler, bool ignoreSniOpenTimeout, long timerExpire, bool encrypt, bool trustServerCert, bool integratedSecurity, SqlConnection owningObject, bool withFailover)
		{
			if (this._state != TdsParserState.Closed)
			{
				return;
			}
			this._connHandler = connHandler;
			this._loginWithFailover = withFailover;
			uint snistatus = SNILoadHandle.SingletonInstance.SNIStatus;
			if (snistatus != 0U)
			{
				this.Errors.Add(this.ProcessSNIError(this._physicalStateObj));
				this._physicalStateObj.Dispose();
				this.ThrowExceptionAndWarning(this._physicalStateObj);
			}
			if (connHandler.ConnectionOptions.LocalDBInstance != null)
			{
				LocalDBAPI.CreateLocalDBInstance(connHandler.ConnectionOptions.LocalDBInstance);
			}
			if (integratedSecurity)
			{
				this.LoadSSPILibrary();
				this._sniSpnBuffer = new byte[SNINativeMethodWrapper.SniMaxComposedSpnLength];
				Bid.Trace("<sc.TdsParser.Connect|SEC> SSPI authentication\n");
			}
			else
			{
				this._sniSpnBuffer = null;
				Bid.Trace("<sc.TdsParser.Connect|SEC> SQL authentication\n");
			}
			byte[] array = null;
			bool multiSubnetFailover = this._connHandler.ConnectionOptions.MultiSubnetFailover;
			this._physicalStateObj.CreatePhysicalSNIHandle(serverInfo.ExtendedServerName, ignoreSniOpenTimeout, timerExpire, out array, this._sniSpnBuffer, false, this._fAsync, multiSubnetFailover);
			if (this._physicalStateObj.Status != 0U)
			{
				this.Errors.Add(this.ProcessSNIError(this._physicalStateObj));
				this._physicalStateObj.Dispose();
				Bid.Trace("<sc.TdsParser.Connect|ERR|SEC> Login failure\n");
				this.ThrowExceptionAndWarning(this._physicalStateObj);
			}
			this._server = serverInfo.ResolvedServerName;
			if (connHandler.PoolGroupProviderInfo != null)
			{
				connHandler.PoolGroupProviderInfo.AliasCheck((serverInfo.PreRoutingServerName == null) ? serverInfo.ResolvedServerName : serverInfo.PreRoutingServerName);
			}
			this._state = TdsParserState.OpenNotLoggedIn;
			this._physicalStateObj.SniContext = SniContext.Snix_PreLoginBeforeSuccessfullWrite;
			this._physicalStateObj.TimeoutTime = timerExpire;
			bool flag = false;
			this.SendPreLoginHandshake(array, encrypt);
			this._physicalStateObj.SniContext = SniContext.Snix_PreLogin;
			PreLoginHandshakeStatus preLoginHandshakeStatus = this.ConsumePreLoginHandshake(encrypt, trustServerCert, out flag);
			if (preLoginHandshakeStatus == PreLoginHandshakeStatus.SphinxFailure)
			{
				this._fMARS = false;
				this._physicalStateObj._sniPacket = null;
				this._physicalStateObj.SniContext = SniContext.Snix_Connect;
				this._physicalStateObj.CreatePhysicalSNIHandle(serverInfo.ExtendedServerName, ignoreSniOpenTimeout, timerExpire, out array, this._sniSpnBuffer, false, this._fAsync, multiSubnetFailover);
				if (this._physicalStateObj.Status != 0U)
				{
					this.Errors.Add(this.ProcessSNIError(this._physicalStateObj));
					Bid.Trace("<sc.TdsParser.Connect|ERR|SEC> Login failure\n");
					this.ThrowExceptionAndWarning(this._physicalStateObj);
				}
			}
			else if (preLoginHandshakeStatus == PreLoginHandshakeStatus.InstanceFailure)
			{
				this._physicalStateObj.Dispose();
				this._physicalStateObj.SniContext = SniContext.Snix_Connect;
				this._physicalStateObj.CreatePhysicalSNIHandle(serverInfo.ExtendedServerName, ignoreSniOpenTimeout, timerExpire, out array, this._sniSpnBuffer, true, this._fAsync, multiSubnetFailover);
				if (this._physicalStateObj.Status != 0U)
				{
					this.Errors.Add(this.ProcessSNIError(this._physicalStateObj));
					Bid.Trace("<sc.TdsParser.Connect|ERR|SEC> Login failure\n");
					this.ThrowExceptionAndWarning(this._physicalStateObj);
				}
				this.SendPreLoginHandshake(array, encrypt);
				preLoginHandshakeStatus = this.ConsumePreLoginHandshake(encrypt, trustServerCert, out flag);
				if (preLoginHandshakeStatus == PreLoginHandshakeStatus.InstanceFailure)
				{
					Bid.Trace("<sc.TdsParser.Connect|ERR|SEC> Login failure\n");
					throw SQL.InstanceFailure();
				}
			}
			if (this._fMARS && flag)
			{
				this._sessionPool = new TdsParserSessionPool(this);
				return;
			}
			this._fMARS = false;
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x00294448 File Offset: 0x00293848
		internal void RemoveEncryption()
		{
			uint num = SNINativeMethodWrapper.SNIRemoveProvider(this._physicalStateObj.Handle, SNINativeMethodWrapper.ProviderEnum.SSL_PROV);
			if (num != 0U)
			{
				this.Errors.Add(this.ProcessSNIError(this._physicalStateObj));
				this.ThrowExceptionAndWarning(this._physicalStateObj);
			}
			try
			{
			}
			finally
			{
				this._physicalStateObj._sniPacket.Dispose();
				this._physicalStateObj._sniPacket = new SNIPacket(this._physicalStateObj.Handle);
			}
		}

		// Token: 0x060029DF RID: 10719 RVA: 0x002944DC File Offset: 0x002938DC
		internal void EnableMars(string server)
		{
			if (this._fMARS)
			{
				this._pMarsPhysicalConObj = this._physicalStateObj;
				uint num = 0U;
				uint num2 = 0U;
				num = SNINativeMethodWrapper.SNIAddProvider(this._pMarsPhysicalConObj.Handle, SNINativeMethodWrapper.ProviderEnum.SMUX_PROV, ref num2);
				if (num != 0U)
				{
					this.Errors.Add(this.ProcessSNIError(this._physicalStateObj));
					this.ThrowExceptionAndWarning(this._physicalStateObj);
				}
				IntPtr zero = IntPtr.Zero;
				if (this._fAsync)
				{
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
					}
					finally
					{
						this._pMarsPhysicalConObj.IncrementPendingCallbacks();
						num = SNINativeMethodWrapper.SNIReadAsync(this._pMarsPhysicalConObj.Handle, ref zero);
						if (zero != IntPtr.Zero)
						{
							SNINativeMethodWrapper.SNIPacketRelease(zero);
						}
					}
					if (997U != num)
					{
						this.Errors.Add(this.ProcessSNIError(this._physicalStateObj));
						this.ThrowExceptionAndWarning(this._physicalStateObj);
					}
				}
				this._physicalStateObj = this.CreateSession();
			}
		}

		// Token: 0x060029E0 RID: 10720 RVA: 0x002945D8 File Offset: 0x002939D8
		internal TdsParserStateObject CreateSession()
		{
			TdsParserStateObject tdsParserStateObject = new TdsParserStateObject(this, this._pMarsPhysicalConObj.Handle, this._fAsync);
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.TdsParser.CreateSession|ADV> %d# created session %d\n", this.ObjectID, tdsParserStateObject.ObjectID);
			}
			return tdsParserStateObject;
		}

		// Token: 0x060029E1 RID: 10721 RVA: 0x0029461C File Offset: 0x00293A1C
		internal TdsParserStateObject GetSession(object owner)
		{
			TdsParserStateObject tdsParserStateObject;
			if (this.MARSOn)
			{
				tdsParserStateObject = this._sessionPool.GetSession(owner);
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.TdsParser.GetSession|ADV> %d# getting session %d from pool\n", this.ObjectID, tdsParserStateObject.ObjectID);
				}
			}
			else
			{
				tdsParserStateObject = this._physicalStateObj;
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.TdsParser.GetSession|ADV> %d# getting physical session %d\n", this.ObjectID, tdsParserStateObject.ObjectID);
				}
			}
			return tdsParserStateObject;
		}

		// Token: 0x060029E2 RID: 10722 RVA: 0x00294684 File Offset: 0x00293A84
		internal void PutSession(TdsParserStateObject session)
		{
			if (this.MARSOn)
			{
				this._sessionPool.PutSession(session);
			}
		}

		// Token: 0x060029E3 RID: 10723 RVA: 0x002946A8 File Offset: 0x00293AA8
		internal SNIHandle GetBestEffortCleanupTarget()
		{
			if (this._physicalStateObj != null)
			{
				return this._physicalStateObj.Handle;
			}
			return null;
		}

		// Token: 0x060029E4 RID: 10724 RVA: 0x002946CC File Offset: 0x00293ACC
		private void SendPreLoginHandshake(byte[] instanceName, bool encrypt)
		{
			this._physicalStateObj._outputMessageType = 18;
			int num = 26;
			byte[] array = new byte[1049];
			int num2 = 0;
			for (int i = 0; i < 5; i++)
			{
				int num3 = 0;
				this.WriteByte((byte)i, this._physicalStateObj);
				this.WriteByte((byte)(num & 65280), this._physicalStateObj);
				this.WriteByte((byte)(num & 255), this._physicalStateObj);
				switch (i)
				{
				case 0:
					array[num2++] = 16;
					array[num2++] = 0;
					array[num2++] = 0;
					array[num2++] = 0;
					array[num2++] = 0;
					array[num2++] = 0;
					num += 6;
					num3 = 6;
					break;
				case 1:
					if (this._encryptionOption == EncryptionOptions.NOT_SUP)
					{
						array[num2] = 2;
					}
					else if (encrypt)
					{
						array[num2] = 1;
						this._encryptionOption = EncryptionOptions.ON;
					}
					else
					{
						array[num2] = 0;
						this._encryptionOption = EncryptionOptions.OFF;
					}
					num2++;
					num++;
					num3 = 1;
					break;
				case 2:
				{
					int num4 = 0;
					while (instanceName[num4] != 0)
					{
						array[num2] = instanceName[num4];
						num2++;
						num4++;
					}
					array[num2] = 0;
					num2++;
					num4++;
					num += num4;
					num3 = num4;
					break;
				}
				case 3:
				{
					int currentThreadId = ADP.GetCurrentThreadId();
					array[num2++] = (byte)((ulong)(-16777216) & (ulong)((long)currentThreadId));
					array[num2++] = (byte)(16711680 & currentThreadId);
					array[num2++] = (byte)(65280 & currentThreadId);
					array[num2++] = (byte)(255 & currentThreadId);
					num += 4;
					num3 = 4;
					break;
				}
				case 4:
					array[num2++] = (this._fMARS ? 1 : 0);
					num++;
					num3++;
					break;
				}
				this.WriteByte((byte)(num3 & 65280), this._physicalStateObj);
				this.WriteByte((byte)(num3 & 255), this._physicalStateObj);
			}
			this.WriteByte(byte.MaxValue, this._physicalStateObj);
			this.WriteByteArray(array, num2, 0, this._physicalStateObj);
			this._physicalStateObj.WritePacket(1);
		}

		// Token: 0x060029E5 RID: 10725 RVA: 0x002948D4 File Offset: 0x00293CD4
		private PreLoginHandshakeStatus ConsumePreLoginHandshake(bool encrypt, bool trustServerCert, out bool marsCapable)
		{
			marsCapable = this._fMARS;
			bool flag = false;
			this._fAwaitingPreLogin = true;
			this._physicalStateObj.ReadNetworkPacket();
			this._fAwaitingPreLogin = false;
			if (this._physicalStateObj._inBytesRead == 0 || this._fPreLoginErrorOccurred)
			{
				if (encrypt)
				{
					this.Errors.Add(new SqlError(20, 0, 20, this._server, SQLMessage.EncryptionNotSupportedByServer(), "", 0));
					this._physicalStateObj.Dispose();
					this.ThrowExceptionAndWarning(this._physicalStateObj);
				}
				return PreLoginHandshakeStatus.SphinxFailure;
			}
			this._physicalStateObj.ProcessHeader();
			if (this._physicalStateObj._inBytesPacket > 32768 || this._physicalStateObj._inBytesPacket <= 0)
			{
				throw SQL.ParsingError();
			}
			byte[] array = new byte[this._physicalStateObj._inBytesPacket];
			this._physicalStateObj.ReadByteArray(array, 0, array.Length);
			if (array[0] == 170)
			{
				throw SQL.InvalidSQLServerVersionUnknown();
			}
			int num = 0;
			int num2 = (int)array[num++];
			bool flag2 = false;
			while (num2 != 255)
			{
				switch (num2)
				{
				case 0:
				{
					int num3 = ((int)array[num++] << 8) | (int)array[num++];
					byte b = array[num++];
					byte b2 = array[num++];
					byte b3 = array[num3];
					byte b4 = array[num3 + 1];
					byte b5 = array[num3 + 2];
					byte b6 = array[num3 + 3];
					flag = b3 >= 9;
					if (!flag)
					{
						marsCapable = false;
					}
					break;
				}
				case 1:
				{
					int num3 = ((int)array[num++] << 8) | (int)array[num++];
					byte b7 = array[num++];
					byte b8 = array[num++];
					EncryptionOptions encryptionOptions = (EncryptionOptions)array[num3];
					flag2 = encryptionOptions != EncryptionOptions.NOT_SUP;
					switch (this._encryptionOption)
					{
					case EncryptionOptions.OFF:
						if (encryptionOptions == EncryptionOptions.OFF)
						{
							this._encryptionOption = EncryptionOptions.LOGIN;
							goto IL_02D7;
						}
						if (encryptionOptions == EncryptionOptions.REQ)
						{
							this._encryptionOption = EncryptionOptions.ON;
							goto IL_02D7;
						}
						goto IL_02D7;
					case EncryptionOptions.NOT_SUP:
						if (encryptionOptions == EncryptionOptions.REQ)
						{
							this.Errors.Add(new SqlError(20, 0, 20, this._server, SQLMessage.EncryptionNotSupportedByClient(), "", 0));
							this._physicalStateObj.Dispose();
							this.ThrowExceptionAndWarning(this._physicalStateObj);
							goto IL_02D7;
						}
						goto IL_02D7;
					}
					if (encryptionOptions == EncryptionOptions.NOT_SUP)
					{
						this.Errors.Add(new SqlError(20, 0, 20, this._server, SQLMessage.EncryptionNotSupportedByServer(), "", 0));
						this._physicalStateObj.Dispose();
						this.ThrowExceptionAndWarning(this._physicalStateObj);
					}
					break;
				}
				case 2:
				{
					int num3 = ((int)array[num++] << 8) | (int)array[num++];
					byte b9 = array[num++];
					byte b10 = array[num++];
					byte b11 = 1;
					byte b12 = array[num3];
					if (b12 == b11)
					{
						return PreLoginHandshakeStatus.InstanceFailure;
					}
					break;
				}
				case 3:
					num += 4;
					break;
				case 4:
				{
					int num3 = ((int)array[num++] << 8) | (int)array[num++];
					byte b13 = array[num++];
					byte b14 = array[num++];
					marsCapable = array[num3] != 0;
					break;
				}
				default:
					num += 4;
					break;
				}
				IL_02D7:
				if (num >= array.Length)
				{
					break;
				}
				num2 = (int)array[num++];
			}
			if (this._encryptionOption == EncryptionOptions.ON || this._encryptionOption == EncryptionOptions.LOGIN)
			{
				if (!flag2)
				{
					this.Errors.Add(new SqlError(20, 0, 20, this._server, SQLMessage.EncryptionNotSupportedByServer(), "", 0));
					this._physicalStateObj.Dispose();
					this.ThrowExceptionAndWarning(this._physicalStateObj);
				}
				uint num4 = ((encrypt && !trustServerCert) ? 1U : 0U) | (flag ? 2U : 0U);
				uint num5 = SNINativeMethodWrapper.SNIAddProvider(this._physicalStateObj.Handle, SNINativeMethodWrapper.ProviderEnum.SSL_PROV, ref num4);
				if (num5 != 0U)
				{
					this.Errors.Add(this.ProcessSNIError(this._physicalStateObj));
					this.ThrowExceptionAndWarning(this._physicalStateObj);
				}
				num5 = SNINativeMethodWrapper.SNIWaitForSSLHandshakeToComplete(this._physicalStateObj.Handle, TdsParserStaticMethods.GetTimeoutMilliseconds(this._physicalStateObj.TimeoutTime));
				if (num5 != 0U)
				{
					this.Errors.Add(this.ProcessSNIError(this._physicalStateObj));
					this.ThrowExceptionAndWarning(this._physicalStateObj);
				}
				try
				{
				}
				finally
				{
					this._physicalStateObj._sniPacket.Dispose();
					this._physicalStateObj._sniPacket = new SNIPacket(this._physicalStateObj.Handle);
				}
			}
			return PreLoginHandshakeStatus.Successful;
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x00294D14 File Offset: 0x00294114
		internal void Deactivate(bool connectionIsDoomed)
		{
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.TdsParser.Deactivate|ADV> %d# deactivating\n", this.ObjectID);
			}
			if (Bid.StateDumpOn)
			{
				Bid.Trace("<sc.TdsParser.Deactivate|STATE> %d#, %s\n", this.ObjectID, this.TraceString());
			}
			if (this.MARSOn)
			{
				this._sessionPool.Deactivate();
			}
			if (!connectionIsDoomed && this._physicalStateObj != null)
			{
				if (this._physicalStateObj._pendingData)
				{
					this._physicalStateObj.CleanWire();
				}
				if (this._physicalStateObj.HasOpenResult)
				{
					this._physicalStateObj.DecrementOpenResultCount();
				}
			}
			SqlInternalTransaction currentTransaction = this.CurrentTransaction;
			if (currentTransaction != null && currentTransaction.HasParentTransaction)
			{
				currentTransaction.CloseFromConnection();
			}
			this.Statistics = null;
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x00294DC4 File Offset: 0x002941C4
		internal void Disconnect()
		{
			if (this._sessionPool != null)
			{
				this._sessionPool.Dispose();
			}
			if (this._state != TdsParserState.Closed)
			{
				this._state = TdsParserState.Closed;
				this._physicalStateObj.SniContext = SniContext.Snix_Close;
				if (this._fMARS)
				{
					try
					{
						this._physicalStateObj.Dispose();
						if (this._pMarsPhysicalConObj != null)
						{
							this._pMarsPhysicalConObj.Dispose();
						}
						return;
					}
					finally
					{
						this._pMarsPhysicalConObj = null;
					}
				}
				this._physicalStateObj.Dispose();
			}
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x00294E58 File Offset: 0x00294258
		private void FireInfoMessageEvent(TdsParserStateObject stateObj, SqlError error)
		{
			string text = null;
			if (this._state == TdsParserState.OpenLoggedIn)
			{
				text = this._connHandler.ServerVersion;
			}
			SqlException ex = SqlException.CreateException(new SqlErrorCollection { error }, text);
			this._connHandler.Connection.OnInfoMessage(new SqlInfoMessageEventArgs(ex));
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x00294EA8 File Offset: 0x002942A8
		internal void DisconnectTransaction(SqlInternalTransaction internalTransaction)
		{
			if (this._currentTransaction != null && this._currentTransaction == internalTransaction)
			{
				this._currentTransaction = null;
			}
		}

		// Token: 0x060029EA RID: 10730 RVA: 0x00294ED0 File Offset: 0x002942D0
		internal void RollbackOrphanedAPITransactions()
		{
			SqlInternalTransaction currentTransaction = this.CurrentTransaction;
			if (currentTransaction != null && currentTransaction.HasParentTransaction && currentTransaction.IsOrphaned)
			{
				currentTransaction.CloseFromConnection();
			}
		}

		// Token: 0x060029EB RID: 10731 RVA: 0x00294F00 File Offset: 0x00294300
		internal void ThrowExceptionAndWarning(TdsParserStateObject stateObj)
		{
			lock (this._ErrorCollectionLock)
			{
				SqlErrorCollection sqlErrorCollection = null;
				bool flag = this.AddSqlErrorToCollection(ref sqlErrorCollection, ref this._errors);
				flag |= this.AddSqlErrorToCollection(ref sqlErrorCollection, ref this._attentionErrors);
				flag |= this.AddSqlErrorToCollection(ref sqlErrorCollection, ref this._warnings);
				flag |= this.AddSqlErrorToCollection(ref sqlErrorCollection, ref this._attentionWarnings);
				if (flag)
				{
					this._state = TdsParserState.Broken;
				}
				if (sqlErrorCollection != null && sqlErrorCollection.Count > 0)
				{
					string text = null;
					if (this._state == TdsParserState.OpenLoggedIn)
					{
						text = this._connHandler.ServerVersion;
					}
					SqlException ex = SqlException.CreateException(sqlErrorCollection, text);
					this._connHandler.OnError(ex, flag);
				}
			}
		}

		// Token: 0x060029EC RID: 10732 RVA: 0x00294FC8 File Offset: 0x002943C8
		private bool AddSqlErrorToCollection(ref SqlErrorCollection temp, ref SqlErrorCollection InputCollection)
		{
			if (InputCollection == null)
			{
				return false;
			}
			bool flag = false;
			if (temp == null)
			{
				temp = new SqlErrorCollection();
			}
			for (int i = 0; i < InputCollection.Count; i++)
			{
				SqlError sqlError = InputCollection[i];
				temp.Add(sqlError);
				if (sqlError.Class >= 20)
				{
					flag = true;
				}
			}
			InputCollection = null;
			return flag && TdsParserState.Closed != this._state;
		}

		// Token: 0x060029ED RID: 10733 RVA: 0x0029502C File Offset: 0x0029442C
		internal static void ClearPoolCallback(object state)
		{
			if (state != null)
			{
				DbConnectionPoolGroup dbConnectionPoolGroup = (DbConnectionPoolGroup)state;
				dbConnectionPoolGroup.Clear();
			}
		}

		// Token: 0x060029EE RID: 10734 RVA: 0x0029504C File Offset: 0x0029444C
		internal SqlError ProcessSNIError(TdsParserStateObject stateObj)
		{
			SNINativeMethodWrapper.SNI_Error sni_Error = new SNINativeMethodWrapper.SNI_Error();
			SNINativeMethodWrapper.SNIGetLastError(sni_Error);
			if (sni_Error.sniError != 0U)
			{
				switch (sni_Error.sniError)
				{
				case 47U:
					throw SQL.MultiSubnetFailoverWithMoreThan64IPs();
				case 48U:
					throw SQL.MultiSubnetFailoverWithInstanceSpecified();
				case 49U:
					throw SQL.MultiSubnetFailoverWithNonTcpProtocol();
				}
			}
			if (this._state == TdsParserState.OpenLoggedIn && this._connHandler.Pool != null)
			{
				Bid.Trace("<sc.TdsParser.Connect|RSRC|CPOOL> %d# clearing pool(ProcessSNIError)\n", this.ObjectID);
				if (this._fAsync)
				{
					ThreadPool.QueueUserWorkItem(new WaitCallback(TdsParser.ClearPoolCallback), this._connHandler.Pool.PoolGroup);
				}
				else
				{
					this._connHandler.Pool.PoolGroup.Clear();
				}
			}
			int num = Array.IndexOf<char>(sni_Error.errorMessage, '\0');
			string text;
			if (num == -1)
			{
				text = string.Empty;
			}
			else
			{
				text = new string(sni_Error.errorMessage, 0, num);
			}
			string @string = Res.GetString(Enum.GetName(typeof(SniContext), stateObj.SniContext));
			string text2 = string.Format(null, "SNI_PN{0}", new object[] { (int)sni_Error.provider });
			string string2 = Res.GetString(text2);
			if (sni_Error.sniError == 0U)
			{
				int num2 = text.IndexOf(':');
				if (0 <= num2)
				{
					int num3 = text.Length;
					num3 -= 2;
					num2 += 2;
					num3 -= num2;
					if (num3 > 0)
					{
						text = text.Substring(num2, num3);
					}
				}
			}
			else
			{
				text = SQL.GetSNIErrorMessage((int)sni_Error.sniError);
				if ((ulong)sni_Error.sniError == (ulong)((long)SNINativeMethodWrapper.SNI_LocalDBErrorCode))
				{
					text += LocalDBAPI.GetLocalDBMessage((int)sni_Error.nativeError);
				}
			}
			text = string.Format(null, "{0} (provider: {1}, error: {2} - {3})", new object[]
			{
				@string,
				string2,
				(int)sni_Error.sniError,
				text
			});
			return new SqlError((int)sni_Error.nativeError, 0, 20, this._server, text, sni_Error.function, (int)sni_Error.lineNumber);
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x0029523C File Offset: 0x0029463C
		internal void CheckResetConnection(TdsParserStateObject stateObj)
		{
			if (this._fResetConnection && !stateObj._fResetConnectionSent)
			{
				try
				{
					if (this._fAsync && this._fMARS && !stateObj._fResetEventOwned)
					{
						stateObj._fResetEventOwned = this._resetConnectionEvent.WaitOne(TdsParserStaticMethods.GetTimeoutMilliseconds(stateObj.TimeoutTime), false);
						if (stateObj._fResetEventOwned && stateObj.TimeoutHasExpired)
						{
							stateObj._fResetEventOwned = !this._resetConnectionEvent.Set();
							stateObj.TimeoutTime = 0L;
						}
						if (!stateObj._fResetEventOwned)
						{
							stateObj.ResetBuffer();
							this.Errors.Add(new SqlError(-2, 0, 11, this._server, SQLMessage.Timeout(), "", 0));
							this.ThrowExceptionAndWarning(stateObj);
						}
					}
					if (this._fResetConnection)
					{
						if (this._fPreserveTransaction)
						{
							stateObj._outBuff[1] = stateObj._outBuff[1] | 16;
						}
						else
						{
							stateObj._outBuff[1] = stateObj._outBuff[1] | 8;
						}
						if (!this._fAsync || !this._fMARS)
						{
							this._fResetConnection = false;
							this._fPreserveTransaction = false;
						}
						else
						{
							stateObj._fResetConnectionSent = true;
						}
					}
					else if (this._fAsync && this._fMARS && stateObj._fResetEventOwned)
					{
						stateObj._fResetEventOwned = !this._resetConnectionEvent.Set();
					}
				}
				catch (Exception)
				{
					if (this._fAsync && this._fMARS && stateObj._fResetEventOwned)
					{
						stateObj._fResetConnectionSent = false;
						stateObj._fResetEventOwned = !this._resetConnectionEvent.Set();
					}
					throw;
				}
			}
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x0029540C File Offset: 0x0029480C
		internal void WriteByte(byte b, TdsParserStateObject stateObj)
		{
			if (stateObj._outBytesUsed == stateObj._outBuff.Length)
			{
				stateObj.WritePacket(0);
			}
			stateObj._outBuff[stateObj._outBytesUsed++] = b;
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x0029544C File Offset: 0x0029484C
		internal void WriteByteArray(byte[] b, int len, int offsetBuffer, TdsParserStateObject stateObj)
		{
			int num = offsetBuffer;
			while (len > 0)
			{
				if (stateObj._outBytesUsed + len <= stateObj._outBuff.Length)
				{
					Buffer.BlockCopy(b, num, stateObj._outBuff, stateObj._outBytesUsed, len);
					stateObj._outBytesUsed += len;
					return;
				}
				int num2 = stateObj._outBuff.Length - stateObj._outBytesUsed;
				Buffer.BlockCopy(b, num, stateObj._outBuff, stateObj._outBytesUsed, num2);
				num += num2;
				stateObj._outBytesUsed += num2;
				if (stateObj._outBytesUsed == stateObj._outBuff.Length)
				{
					stateObj.WritePacket(0);
				}
				len -= num2;
			}
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x00295500 File Offset: 0x00294900
		internal void WriteShort(int v, TdsParserStateObject stateObj)
		{
			if (stateObj._outBytesUsed + 2 > stateObj._outBuff.Length)
			{
				this.WriteByte((byte)(v & 255), stateObj);
				this.WriteByte((byte)((v >> 8) & 255), stateObj);
				return;
			}
			stateObj._outBuff[stateObj._outBytesUsed++] = (byte)(v & 255);
			stateObj._outBuff[stateObj._outBytesUsed++] = (byte)((v >> 8) & 255);
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x00295584 File Offset: 0x00294984
		internal void WriteUnsignedShort(ushort us, TdsParserStateObject stateObj)
		{
			this.WriteShort((int)((short)us), stateObj);
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x0029559C File Offset: 0x0029499C
		internal void WriteUnsignedInt(uint i, TdsParserStateObject stateObj)
		{
			this.WriteByteArray(BitConverter.GetBytes(i), 4, 0, stateObj);
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x002955B8 File Offset: 0x002949B8
		internal void WriteInt(int v, TdsParserStateObject stateObj)
		{
			this.WriteByteArray(BitConverter.GetBytes(v), 4, 0, stateObj);
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x002955D4 File Offset: 0x002949D4
		internal void WriteFloat(float v, TdsParserStateObject stateObj)
		{
			byte[] bytes = BitConverter.GetBytes(v);
			this.WriteByteArray(bytes, bytes.Length, 0, stateObj);
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x002955F4 File Offset: 0x002949F4
		internal void WriteLong(long v, TdsParserStateObject stateObj)
		{
			byte[] bytes = BitConverter.GetBytes(v);
			this.WriteByteArray(bytes, bytes.Length, 0, stateObj);
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x00295614 File Offset: 0x00294A14
		internal void WriteUnsignedLong(ulong uv, TdsParserStateObject stateObj)
		{
			byte[] bytes = BitConverter.GetBytes(uv);
			this.WriteByteArray(bytes, bytes.Length, 0, stateObj);
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x00295634 File Offset: 0x00294A34
		internal void WriteDouble(double v, TdsParserStateObject stateObj)
		{
			byte[] bytes = BitConverter.GetBytes(v);
			this.WriteByteArray(bytes, bytes.Length, 0, stateObj);
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x00295654 File Offset: 0x00294A54
		internal void SkipLongBytes(ulong num, TdsParserStateObject stateObj)
		{
			while (num > 0UL)
			{
				int num2 = ((num > 2147483647UL) ? int.MaxValue : ((int)num));
				stateObj.ReadByteArray(null, 0, num2);
				num -= (ulong)((long)num2);
			}
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x0029568C File Offset: 0x00294A8C
		public void SkipBytes(int num, TdsParserStateObject stateObj)
		{
			stateObj.ReadByteArray(null, 0, num);
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x002956A4 File Offset: 0x00294AA4
		internal void PrepareResetConnection(bool preserveTransaction)
		{
			this._fResetConnection = true;
			this._fPreserveTransaction = preserveTransaction;
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x002956C4 File Offset: 0x00294AC4
		internal bool Run(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj)
		{
			if (TdsParserState.Broken == this.State || this.State == TdsParserState.Closed)
			{
				return true;
			}
			bool flag = false;
			for (;;)
			{
				if (stateObj._internalTimeout)
				{
					runBehavior = RunBehavior.Attention;
				}
				if (TdsParserState.Broken == this.State || this.State == TdsParserState.Closed)
				{
					goto IL_06EF;
				}
				byte b = stateObj.ReadByte();
				if (b != 170 && b != 171 && b != 173 && b != 227 && b != 172 && b != 121 && b != 160 && b != 161 && b != 129 && b != 136 && b != 164 && b != 165 && b != 169 && b != 211 && b != 209 && b != 253 && b != 254 && b != 255 && b != 57 && b != 237 && b != 174 && b != 124 && b != 120 && b != 237)
				{
					break;
				}
				int tokenLength = this.GetTokenLength(b, stateObj);
				byte b2 = b;
				if (b2 <= 173)
				{
					if (b2 <= 129)
					{
						if (b2 != 121)
						{
							if (b2 == 129)
							{
								if (tokenLength != 65535)
								{
									stateObj._cleanupMetaData = this.ProcessMetaData(tokenLength, stateObj);
								}
								else if (cmdHandler != null)
								{
									stateObj._cleanupMetaData = cmdHandler.MetaData;
								}
								if (dataStream != null)
								{
									byte b3 = stateObj.PeekByte();
									dataStream.SetMetaData(stateObj._cleanupMetaData, 164 == b3 || 165 == b3);
								}
								else if (bulkCopyHandler != null)
								{
									bulkCopyHandler.SetMetaData(stateObj._cleanupMetaData);
								}
							}
						}
						else
						{
							int num = stateObj.ReadInt32();
							if (cmdHandler != null)
							{
								cmdHandler.OnReturnStatus(num);
							}
						}
					}
					else if (b2 != 136)
					{
						switch (b2)
						{
						case 164:
							if (dataStream != null)
							{
								dataStream.TableNames = this.ProcessTableName(tokenLength, stateObj);
							}
							else
							{
								this.SkipBytes(tokenLength, stateObj);
							}
							break;
						case 165:
							if (dataStream != null)
							{
								_SqlMetaDataSet sqlMetaDataSet = this.ProcessColInfo(dataStream.MetaData, dataStream, stateObj);
								dataStream.SetMetaData(sqlMetaDataSet, false);
								dataStream.BrowseModeInfoConsumed = true;
							}
							else
							{
								this.SkipBytes(tokenLength, stateObj);
							}
							break;
						case 169:
							this.SkipBytes(tokenLength, stateObj);
							break;
						case 170:
						case 171:
						{
							if (b == 170)
							{
								stateObj._errorTokenReceived = true;
							}
							SqlError sqlError = this.ProcessError(b, stateObj);
							if (RunBehavior.Clean != (RunBehavior.Clean & runBehavior))
							{
								if (this._connHandler != null && this._connHandler.Connection != null && this._connHandler.Connection.FireInfoMessageEventOnUserErrors && sqlError.Class <= 16)
								{
									this.FireInfoMessageEvent(stateObj, sqlError);
								}
								else if (sqlError.Class < 11)
								{
									this.Warnings.Add(sqlError);
								}
								else if (sqlError.Class <= 16)
								{
									this.Errors.Add(sqlError);
									if (dataStream != null && !dataStream.IsInitialized)
									{
										runBehavior = RunBehavior.UntilDone;
									}
								}
								else
								{
									this.Errors.Add(sqlError);
									runBehavior = RunBehavior.UntilDone;
								}
							}
							else if (sqlError.Class >= 20)
							{
								this.Errors.Add(sqlError);
							}
							break;
						}
						case 172:
						{
							SqlReturnValue sqlReturnValue = this.ProcessReturnValue(tokenLength, stateObj);
							if (cmdHandler != null)
							{
								cmdHandler.OnReturnValue(sqlReturnValue);
							}
							break;
						}
						case 173:
						{
							SqlLoginAck sqlLoginAck = this.ProcessLoginAck(stateObj);
							this._connHandler.OnLoginAck(sqlLoginAck);
							break;
						}
						}
					}
					else
					{
						if (stateObj._cleanupAltMetaDataSetArray == null)
						{
							stateObj._cleanupAltMetaDataSetArray = new _SqlMetaDataSetCollection();
						}
						_SqlMetaDataSet sqlMetaDataSet2 = this.ProcessAltMetaData(tokenLength, stateObj);
						stateObj._cleanupAltMetaDataSetArray.Add(sqlMetaDataSet2);
						if (dataStream != null)
						{
							dataStream.SetAltMetaDataSet(sqlMetaDataSet2, 136 != stateObj.PeekByte());
						}
					}
				}
				else if (b2 <= 227)
				{
					switch (b2)
					{
					case 209:
						if (bulkCopyHandler != null)
						{
							this.ProcessRow(stateObj._cleanupMetaData, bulkCopyHandler.CreateRowBuffer(), bulkCopyHandler.CreateIndexMap(), stateObj);
						}
						else if (RunBehavior.ReturnImmediately != (RunBehavior.ReturnImmediately & runBehavior))
						{
							this.SkipRow(stateObj._cleanupMetaData, stateObj);
						}
						if (this._statistics != null)
						{
							this._statistics.WaitForDoneAfterRow = true;
						}
						flag = true;
						break;
					case 210:
						break;
					case 211:
						if (RunBehavior.ReturnImmediately != (RunBehavior.ReturnImmediately & runBehavior))
						{
							int num2 = (int)stateObj.ReadUInt16();
							this.SkipRow(stateObj._cleanupAltMetaDataSetArray[num2], stateObj);
						}
						flag = true;
						break;
					default:
						if (b2 == 227)
						{
							SqlEnvChange[] array = this.ProcessEnvChange(tokenLength, stateObj);
							for (int i = 0; i < array.Length; i++)
							{
								if (array[i] != null && !this.Connection.IgnoreEnvChange)
								{
									switch (array[i].type)
									{
									case 8:
									case 11:
										this._currentTransaction = this._pendingTransaction;
										this._pendingTransaction = null;
										if (this._currentTransaction != null)
										{
											this._currentTransaction.TransactionId = array[i].newLongValue;
										}
										else
										{
											TransactionType transactionType = ((8 == array[i].type) ? TransactionType.LocalFromTSQL : TransactionType.Distributed);
											this._currentTransaction = new SqlInternalTransaction(this._connHandler, transactionType, null, array[i].newLongValue);
										}
										if (this._statistics != null && !this._statisticsIsInTransaction)
										{
											this._statistics.SafeIncrement(ref this._statistics._transactions);
										}
										this._statisticsIsInTransaction = true;
										this._retainedTransactionId = 0L;
										goto IL_056F;
									case 9:
									case 12:
									case 17:
										this._retainedTransactionId = 0L;
										break;
									case 10:
										break;
									case 13:
									case 14:
									case 15:
									case 16:
										goto IL_0561;
									default:
										goto IL_0561;
									}
									if (this._currentTransaction != null)
									{
										if (9 == array[i].type)
										{
											this._currentTransaction.Completed(TransactionState.Committed);
										}
										else if (10 == array[i].type)
										{
											if (this._currentTransaction.IsDistributed && this._currentTransaction.IsActive)
											{
												this._retainedTransactionId = array[i].oldLongValue;
											}
											this._currentTransaction.Completed(TransactionState.Aborted);
										}
										else
										{
											this._currentTransaction.Completed(TransactionState.Unknown);
										}
										this._currentTransaction = null;
									}
									this._statisticsIsInTransaction = false;
									goto IL_056F;
									IL_0561:
									this._connHandler.OnEnvChange(array[i]);
								}
								IL_056F:;
							}
						}
						break;
					}
				}
				else if (b2 != 237)
				{
					switch (b2)
					{
					case 253:
					case 254:
					case 255:
						this.ProcessDone(cmdHandler, dataStream, ref runBehavior, stateObj);
						if (b == 254 && cmdHandler != null)
						{
							cmdHandler.OnDoneProc();
						}
						break;
					}
				}
				else
				{
					this.ProcessSSPI(tokenLength);
				}
				if ((!stateObj._pendingData || RunBehavior.ReturnImmediately == (RunBehavior.ReturnImmediately & runBehavior)) && (stateObj._pendingData || !stateObj._attentionSent || stateObj._attentionReceived))
				{
					goto IL_06EF;
				}
			}
			this._state = TdsParserState.Broken;
			this._connHandler.BreakConnection();
			throw SQL.ParsingError();
			IL_06EF:
			if (!stateObj._pendingData && this.CurrentTransaction != null)
			{
				this.CurrentTransaction.Activate();
			}
			if (stateObj._attentionSent && stateObj._attentionReceived)
			{
				stateObj._attentionSent = false;
				stateObj._attentionReceived = false;
				if (RunBehavior.Clean != (RunBehavior.Clean & runBehavior) && !stateObj._internalTimeout)
				{
					this.Errors.Add(new SqlError(0, 0, 11, this._server, SQLMessage.OperationCancelled(), "", 0));
				}
			}
			if (this._errors != null || this._warnings != null)
			{
				this.ThrowExceptionAndWarning(stateObj);
			}
			return flag;
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x00295E4C File Offset: 0x0029524C
		private SqlEnvChange[] ProcessEnvChange(int tokenLength, TdsParserStateObject stateObj)
		{
			int num = 0;
			int num2 = 0;
			SqlEnvChange[] array = new SqlEnvChange[3];
			while (tokenLength > num)
			{
				if (num2 >= array.Length)
				{
					SqlEnvChange[] array2 = new SqlEnvChange[array.Length + 3];
					for (int i = 0; i < array.Length; i++)
					{
						array2[i] = array[i];
					}
					array = array2;
				}
				SqlEnvChange sqlEnvChange = new SqlEnvChange();
				sqlEnvChange.type = stateObj.ReadByte();
				array[num2] = sqlEnvChange;
				num2++;
				switch (sqlEnvChange.type)
				{
				case 1:
				case 2:
					this.ReadTwoStringFields(sqlEnvChange, stateObj);
					break;
				case 3:
					this.ReadTwoStringFields(sqlEnvChange, stateObj);
					if (sqlEnvChange.newValue == "iso_1")
					{
						this._defaultCodePage = 1252;
						this._defaultEncoding = Encoding.GetEncoding(this._defaultCodePage);
					}
					else
					{
						string text = sqlEnvChange.newValue.Substring(2);
						this._defaultCodePage = int.Parse(text, NumberStyles.Integer, CultureInfo.InvariantCulture);
						this._defaultEncoding = Encoding.GetEncoding(this._defaultCodePage);
					}
					break;
				case 4:
				{
					this.ReadTwoStringFields(sqlEnvChange, stateObj);
					int num3 = int.Parse(sqlEnvChange.newValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
					if (this._physicalStateObj.SetPacketSize(num3))
					{
						this._physicalStateObj._sniPacket.Dispose();
						uint num4 = (uint)num3;
						SNINativeMethodWrapper.SNISetInfo(this._physicalStateObj.Handle, SNINativeMethodWrapper.QTypes.SNI_QUERY_CONN_BUFSIZE, ref num4);
						this._physicalStateObj._sniPacket = new SNIPacket(this._physicalStateObj.Handle);
					}
					break;
				}
				case 5:
					this.ReadTwoStringFields(sqlEnvChange, stateObj);
					this._defaultLCID = int.Parse(sqlEnvChange.newValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
					break;
				case 6:
					this.ReadTwoStringFields(sqlEnvChange, stateObj);
					break;
				case 7:
					sqlEnvChange.newLength = (int)stateObj.ReadByte();
					if (sqlEnvChange.newLength == 5)
					{
						sqlEnvChange.newCollation = this.ProcessCollation(stateObj);
						this._defaultCollation = sqlEnvChange.newCollation;
						int codePage = this.GetCodePage(sqlEnvChange.newCollation, stateObj);
						if (codePage != this._defaultCodePage)
						{
							this._defaultCodePage = codePage;
							this._defaultEncoding = Encoding.GetEncoding(this._defaultCodePage);
						}
						this._defaultLCID = sqlEnvChange.newCollation.LCID;
					}
					sqlEnvChange.oldLength = stateObj.ReadByte();
					if (sqlEnvChange.oldLength == 5)
					{
						sqlEnvChange.oldCollation = this.ProcessCollation(stateObj);
					}
					sqlEnvChange.length = 3 + sqlEnvChange.newLength + (int)sqlEnvChange.oldLength;
					break;
				case 8:
				case 9:
				case 10:
				case 11:
				case 12:
				case 17:
					sqlEnvChange.newLength = (int)stateObj.ReadByte();
					if (sqlEnvChange.newLength > 0)
					{
						sqlEnvChange.newLongValue = stateObj.ReadInt64();
					}
					else
					{
						sqlEnvChange.newLongValue = 0L;
					}
					sqlEnvChange.oldLength = stateObj.ReadByte();
					if (sqlEnvChange.oldLength > 0)
					{
						sqlEnvChange.oldLongValue = stateObj.ReadInt64();
					}
					else
					{
						sqlEnvChange.oldLongValue = 0L;
					}
					sqlEnvChange.length = 3 + sqlEnvChange.newLength + (int)sqlEnvChange.oldLength;
					break;
				case 13:
					this.ReadTwoStringFields(sqlEnvChange, stateObj);
					break;
				case 15:
					sqlEnvChange.newLength = stateObj.ReadInt32();
					sqlEnvChange.newBinValue = new byte[sqlEnvChange.newLength];
					stateObj.ReadByteArray(sqlEnvChange.newBinValue, 0, sqlEnvChange.newLength);
					sqlEnvChange.oldLength = stateObj.ReadByte();
					sqlEnvChange.length = 5 + sqlEnvChange.newLength;
					break;
				case 16:
				case 18:
					this.ReadTwoBinaryFields(sqlEnvChange, stateObj);
					break;
				case 19:
					this.ReadTwoStringFields(sqlEnvChange, stateObj);
					break;
				case 20:
				{
					sqlEnvChange.newLength = (int)stateObj.ReadUInt16();
					byte b = stateObj.ReadByte();
					ushort num5 = stateObj.ReadUInt16();
					ushort num6 = stateObj.ReadUInt16();
					string text2 = stateObj.ReadString((int)num6);
					sqlEnvChange.newRoutingInfo = new RoutingInfo(b, num5, text2);
					ushort num7 = stateObj.ReadUInt16();
					for (int j = 0; j < (int)num7; j++)
					{
						stateObj.ReadByte();
					}
					sqlEnvChange.length = sqlEnvChange.newLength + (int)num7 + 5;
					break;
				}
				}
				num += sqlEnvChange.length;
			}
			return array;
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x00296250 File Offset: 0x00295650
		private void ReadTwoBinaryFields(SqlEnvChange env, TdsParserStateObject stateObj)
		{
			env.newLength = (int)stateObj.ReadByte();
			env.newBinValue = new byte[env.newLength];
			stateObj.ReadByteArray(env.newBinValue, 0, env.newLength);
			env.oldLength = stateObj.ReadByte();
			env.oldBinValue = new byte[(int)env.oldLength];
			stateObj.ReadByteArray(env.oldBinValue, 0, (int)env.oldLength);
			env.length = 3 + env.newLength + (int)env.oldLength;
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x002962D4 File Offset: 0x002956D4
		private void ReadTwoStringFields(SqlEnvChange env, TdsParserStateObject stateObj)
		{
			env.newLength = (int)stateObj.ReadByte();
			env.newValue = stateObj.ReadString(env.newLength);
			env.oldLength = stateObj.ReadByte();
			env.oldValue = stateObj.ReadString((int)env.oldLength);
			env.length = 3 + env.newLength * 2 + (int)(env.oldLength * 2);
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x00296338 File Offset: 0x00295738
		private void ProcessDone(SqlCommand cmd, SqlDataReader reader, ref RunBehavior run, TdsParserStateObject stateObj)
		{
			ushort num = stateObj.ReadUInt16();
			ushort num2 = stateObj.ReadUInt16();
			int num3;
			if (this._isYukon)
			{
				num3 = (int)stateObj.ReadInt64();
			}
			else
			{
				num3 = stateObj.ReadInt32();
				if (this._state == TdsParserState.OpenNotLoggedIn && stateObj._inBytesRead > stateObj._inBytesUsed && stateObj.PeekByte() == 0)
				{
					num3 = stateObj.ReadInt32();
				}
			}
			if (32 == (num & 32))
			{
				stateObj._attentionReceived = true;
			}
			if (cmd != null && 16 == (num & 16))
			{
				if (num2 != 193)
				{
					cmd.InternalRecordsAffected = num3;
				}
				if (stateObj._receivedColMetaData || num2 != 193)
				{
					cmd.OnStatementCompleted(num3);
				}
			}
			stateObj._receivedColMetaData = false;
			if (2 == (2 & num) && this._errors == null && !stateObj._errorTokenReceived && RunBehavior.Clean != (RunBehavior.Clean & run))
			{
				this.Errors.Add(new SqlError(0, 0, 11, this._server, SQLMessage.SevereError(), "", 0));
				if (reader != null && !reader.IsInitialized)
				{
					run = RunBehavior.UntilDone;
				}
			}
			if (256 == (256 & num) && RunBehavior.Clean != (RunBehavior.Clean & run))
			{
				this.Errors.Add(new SqlError(0, 0, 20, this._server, SQLMessage.SevereError(), "", 0));
				if (reader != null && !reader.IsInitialized)
				{
					run = RunBehavior.UntilDone;
				}
			}
			this.ProcessSqlStatistics(num2, num, num3);
			if (1 != (num & 1))
			{
				stateObj._errorTokenReceived = false;
				if (stateObj._inBytesUsed >= stateObj._inBytesRead)
				{
					stateObj._pendingData = false;
				}
			}
			if (!stateObj._pendingData && stateObj._hasOpenResult)
			{
				stateObj.DecrementOpenResultCount();
			}
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x002964C8 File Offset: 0x002958C8
		private void ProcessSqlStatistics(ushort curCmd, ushort status, int count)
		{
			if (this._statistics != null)
			{
				if (this._statistics.WaitForDoneAfterRow)
				{
					this._statistics.SafeIncrement(ref this._statistics._sumResultSets);
					this._statistics.WaitForDoneAfterRow = false;
				}
				if (16 != (status & 16))
				{
					count = 0;
				}
				if (curCmd <= 197)
				{
					if (curCmd == 32)
					{
						this._statistics.SafeIncrement(ref this._statistics._cursorOpens);
						return;
					}
					switch (curCmd)
					{
					case 193:
						this._statistics.SafeIncrement(ref this._statistics._selectCount);
						this._statistics.SafeAdd(ref this._statistics._selectRows, (long)count);
						return;
					case 194:
						return;
					case 195:
					case 196:
					case 197:
						break;
					default:
						return;
					}
				}
				else
				{
					switch (curCmd)
					{
					case 210:
						this._statisticsIsInTransaction = false;
						return;
					case 211:
						return;
					case 212:
						if (!this._statisticsIsInTransaction)
						{
							this._statistics.SafeIncrement(ref this._statistics._transactions);
						}
						this._statisticsIsInTransaction = true;
						return;
					case 213:
						this._statisticsIsInTransaction = false;
						return;
					default:
						if (curCmd != 279)
						{
							return;
						}
						break;
					}
				}
				this._statistics.SafeIncrement(ref this._statistics._iduCount);
				this._statistics.SafeAdd(ref this._statistics._iduRows, (long)count);
				if (!this._statisticsIsInTransaction)
				{
					this._statistics.SafeIncrement(ref this._statistics._transactions);
					return;
				}
			}
			else
			{
				switch (curCmd)
				{
				case 210:
				case 213:
					this._statisticsIsInTransaction = false;
					break;
				case 211:
					break;
				case 212:
					this._statisticsIsInTransaction = true;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x00296674 File Offset: 0x00295A74
		private SqlLoginAck ProcessLoginAck(TdsParserStateObject stateObj)
		{
			SqlLoginAck sqlLoginAck = new SqlLoginAck();
			this.SkipBytes(1, stateObj);
			byte[] array = new byte[4];
			stateObj.ReadByteArray(array, 0, array.Length);
			uint num = (uint)(((((((int)array[0] << 8) | (int)array[1]) << 8) | (int)array[2]) << 8) | (int)array[3]);
			uint num2 = num & 4278255615U;
			uint num3 = (num >> 16) & 255U;
			uint num4 = num2;
			if (num4 <= 1895825409U)
			{
				if (num4 != 117440512U)
				{
					if (num4 == 1895825409U)
					{
						if (num3 != 0U)
						{
							throw SQL.InvalidTDSVersion();
						}
						this._isShilohSP1 = true;
						goto IL_00DF;
					}
				}
				else
				{
					switch (num3)
					{
					case 0U:
						goto IL_00DF;
					case 1U:
						this._isShiloh = true;
						goto IL_00DF;
					default:
						throw SQL.InvalidTDSVersion();
					}
				}
			}
			else if (num4 != 1912602626U)
			{
				if (num4 == 1929379843U)
				{
					if (num3 != 10U)
					{
						throw SQL.InvalidTDSVersion();
					}
					this._isKatmai = true;
					goto IL_00DF;
				}
			}
			else
			{
				if (num3 != 9U)
				{
					throw SQL.InvalidTDSVersion();
				}
				this._isYukon = true;
				goto IL_00DF;
			}
			throw SQL.InvalidTDSVersion();
			IL_00DF:
			this._isYukon |= this._isKatmai;
			this._isShilohSP1 |= this._isYukon;
			this._isShiloh |= this._isShilohSP1;
			sqlLoginAck.isVersion8 = this._isShiloh;
			stateObj._outBytesUsed = stateObj._outputHeaderLen;
			byte b = stateObj.ReadByte();
			sqlLoginAck.programName = stateObj.ReadString((int)b);
			sqlLoginAck.majorVersion = stateObj.ReadByte();
			sqlLoginAck.minorVersion = stateObj.ReadByte();
			sqlLoginAck.buildNum = (short)(((int)stateObj.ReadByte() << 8) + (int)stateObj.ReadByte());
			this._state = TdsParserState.OpenLoggedIn;
			if (this._isYukon && this._fAsync && this._fMARS)
			{
				this._resetConnectionEvent = new AutoResetEvent(true);
			}
			if (this._connHandler.ConnectionOptions.UserInstance && ADP.IsEmpty(this._connHandler.InstanceName))
			{
				this.Errors.Add(new SqlError(0, 0, 20, this.Server, SQLMessage.UserInstanceFailure(), "", 0));
				this.ThrowExceptionAndWarning(this._physicalStateObj);
			}
			return sqlLoginAck;
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x00296878 File Offset: 0x00295C78
		internal SqlError ProcessError(byte token, TdsParserStateObject stateObj)
		{
			int num = stateObj.ReadInt32();
			byte b = stateObj.ReadByte();
			byte b2 = stateObj.ReadByte();
			int num2 = (int)stateObj.ReadUInt16();
			string text = stateObj.ReadString(num2);
			num2 = (int)stateObj.ReadByte();
			if (num2 != 0)
			{
				stateObj.ReadString(num2);
			}
			num2 = (int)stateObj.ReadByte();
			string text2 = stateObj.ReadString(num2);
			int num3;
			if (this._isYukon)
			{
				num3 = stateObj.ReadInt32();
			}
			else
			{
				num3 = (int)stateObj.ReadUInt16();
				if (this._state == TdsParserState.OpenNotLoggedIn && stateObj.PeekByte() == 0)
				{
					num3 = (num3 << 16) + (int)stateObj.ReadUInt16();
				}
			}
			return new SqlError(num, b, b2, this._server, text, text2, num3);
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x0029691C File Offset: 0x00295D1C
		internal SqlReturnValue ProcessReturnValue(int length, TdsParserStateObject stateObj)
		{
			SqlReturnValue sqlReturnValue = new SqlReturnValue();
			sqlReturnValue.length = length;
			if (this._isYukon)
			{
				sqlReturnValue.parmIndex = stateObj.ReadUInt16();
			}
			byte b = stateObj.ReadByte();
			if (b > 0)
			{
				sqlReturnValue.parameter = stateObj.ReadString((int)b);
			}
			stateObj.ReadByte();
			uint num;
			if (this.IsYukonOrNewer)
			{
				num = stateObj.ReadUInt32();
			}
			else
			{
				num = (uint)stateObj.ReadUInt16();
			}
			stateObj.ReadUInt16();
			byte b2 = stateObj.ReadByte();
			int num2;
			if (b2 == 241)
			{
				num2 = 65535;
			}
			else if (this.IsVarTimeTds(b2))
			{
				num2 = 0;
			}
			else if (b2 == 40)
			{
				num2 = 3;
			}
			else
			{
				num2 = this.GetTokenLength(b2, stateObj);
			}
			sqlReturnValue.metaType = MetaType.GetSqlDataType((int)b2, num, num2);
			sqlReturnValue.type = sqlReturnValue.metaType.SqlDbType;
			if (this._isShiloh)
			{
				sqlReturnValue.tdsType = sqlReturnValue.metaType.NullableType;
				sqlReturnValue.isNullable = true;
				if (num2 == 65535)
				{
					sqlReturnValue.metaType = MetaType.GetMaxMetaTypeFromMetaType(sqlReturnValue.metaType);
				}
			}
			else
			{
				if (sqlReturnValue.metaType.NullableType == b2)
				{
					sqlReturnValue.isNullable = true;
				}
				sqlReturnValue.tdsType = b2;
			}
			if (sqlReturnValue.type == SqlDbType.Decimal)
			{
				sqlReturnValue.precision = stateObj.ReadByte();
				sqlReturnValue.scale = stateObj.ReadByte();
			}
			if (sqlReturnValue.metaType.IsVarTime)
			{
				sqlReturnValue.scale = stateObj.ReadByte();
			}
			if (b2 == 240)
			{
				this.ProcessUDTMetaData(sqlReturnValue, stateObj);
			}
			if (sqlReturnValue.type == SqlDbType.Xml)
			{
				byte b3 = stateObj.ReadByte();
				if ((b3 & 1) != 0)
				{
					b = stateObj.ReadByte();
					if (b != 0)
					{
						sqlReturnValue.xmlSchemaCollectionDatabase = stateObj.ReadString((int)b);
					}
					b = stateObj.ReadByte();
					if (b != 0)
					{
						sqlReturnValue.xmlSchemaCollectionOwningSchema = stateObj.ReadString((int)b);
					}
					short num3 = stateObj.ReadInt16();
					if (num3 != 0)
					{
						sqlReturnValue.xmlSchemaCollectionName = stateObj.ReadString((int)num3);
					}
				}
			}
			else if (this._isShiloh && sqlReturnValue.metaType.IsCharType)
			{
				sqlReturnValue.collation = this.ProcessCollation(stateObj);
				int codePage = this.GetCodePage(sqlReturnValue.collation, stateObj);
				if (codePage == this._defaultCodePage)
				{
					sqlReturnValue.codePage = this._defaultCodePage;
					sqlReturnValue.encoding = this._defaultEncoding;
				}
				else
				{
					sqlReturnValue.codePage = codePage;
					sqlReturnValue.encoding = Encoding.GetEncoding(sqlReturnValue.codePage);
				}
			}
			bool flag = false;
			ulong num4 = this.ProcessColumnHeader(sqlReturnValue, stateObj, out flag);
			int num5 = ((num4 > 2147483647UL) ? int.MaxValue : ((int)num4));
			if (sqlReturnValue.metaType.IsPlp)
			{
				num5 = int.MaxValue;
			}
			if (flag)
			{
				this.GetNullSqlValue(sqlReturnValue.value, sqlReturnValue);
			}
			else
			{
				this.ReadSqlValue(sqlReturnValue.value, sqlReturnValue, num5, stateObj);
			}
			return sqlReturnValue;
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x00296BB8 File Offset: 0x00295FB8
		internal SqlCollation ProcessCollation(TdsParserStateObject stateObj)
		{
			return new SqlCollation
			{
				info = stateObj.ReadUInt32(),
				sortId = stateObj.ReadByte()
			};
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x00296BE4 File Offset: 0x00295FE4
		internal int GetCodePage(SqlCollation collation, TdsParserStateObject stateObj)
		{
			int num = 0;
			if (collation.sortId != 0)
			{
				num = (int)TdsEnums.CODE_PAGE_FROM_SORT_ID[(int)collation.sortId];
			}
			else
			{
				int num2 = collation.LCID;
				bool flag = false;
				try
				{
					num = CultureInfo.GetCultureInfo(num2).TextInfo.ANSICodePage;
					flag = true;
				}
				catch (ArgumentException ex)
				{
					ADP.TraceExceptionWithoutRethrow(ex);
				}
				if (!flag || num == 0)
				{
					CultureInfo cultureInfo = null;
					int num3 = num2;
					if (num3 <= 66578)
					{
						if (num3 == 2087)
						{
							goto IL_00CA;
						}
						if (num3 != 66564)
						{
							switch (num3)
							{
							case 66577:
							case 66578:
								break;
							default:
								goto IL_00E6;
							}
						}
					}
					else if (num3 <= 68612)
					{
						if (num3 != 67588 && num3 != 68612)
						{
							goto IL_00E6;
						}
					}
					else if (num3 != 69636 && num3 != 70660)
					{
						goto IL_00E6;
					}
					num2 &= 16383;
					try
					{
						cultureInfo = new CultureInfo(num2);
						flag = true;
						goto IL_00E6;
					}
					catch (ArgumentException ex2)
					{
						ADP.TraceExceptionWithoutRethrow(ex2);
						goto IL_00E6;
					}
					IL_00CA:
					try
					{
						cultureInfo = new CultureInfo(1063);
						flag = true;
					}
					catch (ArgumentException ex3)
					{
						ADP.TraceExceptionWithoutRethrow(ex3);
					}
					IL_00E6:
					if (!flag)
					{
						this.ThrowUnsupportedCollationEncountered(stateObj);
					}
					if (cultureInfo != null)
					{
						num = cultureInfo.TextInfo.ANSICodePage;
					}
				}
			}
			return num;
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x00296D40 File Offset: 0x00296140
		internal void ThrowUnsupportedCollationEncountered(TdsParserStateObject stateObj)
		{
			this.Errors.Add(new SqlError(0, 0, 11, this._server, SQLMessage.CultureIdError(), "", 0));
			if (stateObj != null)
			{
				stateObj.CleanWire();
				stateObj._pendingData = false;
			}
			this.ThrowExceptionAndWarning(stateObj);
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x00296D8C File Offset: 0x0029618C
		internal _SqlMetaDataSet ProcessAltMetaData(int cColumns, TdsParserStateObject stateObj)
		{
			_SqlMetaDataSet sqlMetaDataSet = new _SqlMetaDataSet(cColumns);
			int[] array = new int[cColumns];
			sqlMetaDataSet.id = stateObj.ReadUInt16();
			for (int i = (int)stateObj.ReadByte(); i > 0; i--)
			{
				this.SkipBytes(2, stateObj);
			}
			for (int j = 0; j < cColumns; j++)
			{
				_SqlMetaData sqlMetaData = sqlMetaDataSet[j];
				sqlMetaData.op = stateObj.ReadByte();
				sqlMetaData.operand = stateObj.ReadUInt16();
				this.CommonProcessMetaData(stateObj, sqlMetaData);
				if (ADP.IsEmpty(sqlMetaData.column))
				{
					byte op = sqlMetaData.op;
					if (op != 9)
					{
						switch (op)
						{
						case 48:
							sqlMetaData.column = "stdev";
							break;
						case 49:
							sqlMetaData.column = "stdevp";
							break;
						case 50:
							sqlMetaData.column = "var";
							break;
						case 51:
							sqlMetaData.column = "varp";
							break;
						default:
							switch (op)
							{
							case 75:
								sqlMetaData.column = "cnt";
								break;
							case 77:
								sqlMetaData.column = "sum";
								break;
							case 79:
								sqlMetaData.column = "avg";
								break;
							case 81:
								sqlMetaData.column = "min";
								break;
							case 82:
								sqlMetaData.column = "max";
								break;
							case 83:
								sqlMetaData.column = "any";
								break;
							case 86:
								sqlMetaData.column = "noop";
								break;
							}
							break;
						}
					}
					else
					{
						sqlMetaData.column = "cntb";
					}
				}
				array[j] = j;
			}
			sqlMetaDataSet.indexMap = array;
			sqlMetaDataSet.visibleColumns = cColumns;
			return sqlMetaDataSet;
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x00296F34 File Offset: 0x00296334
		internal _SqlMetaDataSet ProcessMetaData(int cColumns, TdsParserStateObject stateObj)
		{
			_SqlMetaDataSet sqlMetaDataSet = new _SqlMetaDataSet(cColumns);
			for (int i = 0; i < cColumns; i++)
			{
				this.CommonProcessMetaData(stateObj, sqlMetaDataSet[i]);
			}
			return sqlMetaDataSet;
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x00296F64 File Offset: 0x00296364
		private bool IsVarTimeTds(byte tdsType)
		{
			return tdsType == 41 || tdsType == 42 || tdsType == 43;
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x00296F84 File Offset: 0x00296384
		private void CommonProcessMetaData(TdsParserStateObject stateObj, _SqlMetaData col)
		{
			uint num;
			if (this.IsYukonOrNewer)
			{
				num = stateObj.ReadUInt32();
			}
			else
			{
				num = (uint)stateObj.ReadUInt16();
			}
			byte b = stateObj.ReadByte();
			col.updatability = (byte)((b & 11) >> 2);
			col.isNullable = 1 == (b & 1);
			col.isIdentity = 16 == (b & 16);
			stateObj.ReadByte();
			col.isColumnSet = 4 == (b & 4);
			byte b2 = stateObj.ReadByte();
			if (b2 == 241)
			{
				col.length = 65535;
			}
			else if (this.IsVarTimeTds(b2))
			{
				col.length = 0;
			}
			else if (b2 == 40)
			{
				col.length = 3;
			}
			else
			{
				col.length = this.GetTokenLength(b2, stateObj);
			}
			col.metaType = MetaType.GetSqlDataType((int)b2, num, col.length);
			col.type = col.metaType.SqlDbType;
			if (this._isShiloh)
			{
				col.tdsType = (col.isNullable ? col.metaType.NullableType : col.metaType.TDSType);
			}
			else
			{
				col.tdsType = b2;
			}
			int num2;
			if (this._isYukon)
			{
				if (240 == b2)
				{
					this.ProcessUDTMetaData(col, stateObj);
				}
				if (col.length == 65535)
				{
					col.metaType = MetaType.GetMaxMetaTypeFromMetaType(col.metaType);
					col.length = int.MaxValue;
					if (b2 == 241)
					{
						byte b3 = stateObj.ReadByte();
						if ((b3 & 1) != 0)
						{
							num2 = (int)stateObj.ReadByte();
							if (num2 != 0)
							{
								col.xmlSchemaCollectionDatabase = stateObj.ReadString(num2);
							}
							num2 = (int)stateObj.ReadByte();
							if (num2 != 0)
							{
								col.xmlSchemaCollectionOwningSchema = stateObj.ReadString(num2);
							}
							num2 = (int)stateObj.ReadInt16();
							if (num2 != 0)
							{
								col.xmlSchemaCollectionName = stateObj.ReadString(num2);
							}
						}
					}
				}
			}
			if (col.type == SqlDbType.Decimal)
			{
				col.precision = stateObj.ReadByte();
				col.scale = stateObj.ReadByte();
			}
			if (col.metaType.IsVarTime)
			{
				col.scale = stateObj.ReadByte();
				switch (col.metaType.SqlDbType)
				{
				case SqlDbType.Time:
					col.length = MetaType.GetTimeSizeFromScale(col.scale);
					break;
				case SqlDbType.DateTime2:
					col.length = 3 + MetaType.GetTimeSizeFromScale(col.scale);
					break;
				case SqlDbType.DateTimeOffset:
					col.length = 5 + MetaType.GetTimeSizeFromScale(col.scale);
					break;
				}
			}
			if (this._isShiloh && col.metaType.IsCharType && b2 != 241)
			{
				col.collation = this.ProcessCollation(stateObj);
				int codePage = this.GetCodePage(col.collation, stateObj);
				if (codePage == this._defaultCodePage)
				{
					col.codePage = this._defaultCodePage;
					col.encoding = this._defaultEncoding;
				}
				else
				{
					col.codePage = codePage;
					col.encoding = Encoding.GetEncoding(col.codePage);
				}
			}
			if (col.metaType.IsLong && !col.metaType.IsPlp)
			{
				if (this._isYukon)
				{
					int num3 = 65535;
					col.multiPartTableName = this.ProcessOneTable(stateObj, ref num3);
				}
				else
				{
					num2 = (int)stateObj.ReadUInt16();
					string text = stateObj.ReadString(num2);
					col.multiPartTableName = new MultiPartTableName(text);
				}
			}
			num2 = (int)stateObj.ReadByte();
			col.column = stateObj.ReadString(num2);
			stateObj._receivedColMetaData = true;
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x002972B8 File Offset: 0x002966B8
		private void ProcessUDTMetaData(SqlMetaDataPriv metaData, TdsParserStateObject stateObj)
		{
			metaData.length = (int)stateObj.ReadUInt16();
			int num = (int)stateObj.ReadByte();
			if (num != 0)
			{
				metaData.udtDatabaseName = stateObj.ReadString(num);
			}
			num = (int)stateObj.ReadByte();
			if (num != 0)
			{
				metaData.udtSchemaName = stateObj.ReadString(num);
			}
			num = (int)stateObj.ReadByte();
			if (num != 0)
			{
				metaData.udtTypeName = stateObj.ReadString(num);
			}
			num = (int)stateObj.ReadUInt16();
			if (num != 0)
			{
				metaData.udtAssemblyQualifiedName = stateObj.ReadString(num);
			}
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x00297330 File Offset: 0x00296730
		private void WriteUDTMetaData(object value, string database, string schema, string type, TdsParserStateObject stateObj)
		{
			if (ADP.IsEmpty(database))
			{
				this.WriteByte(0, stateObj);
			}
			else
			{
				this.WriteByte((byte)database.Length, stateObj);
				this.WriteString(database, stateObj);
			}
			if (ADP.IsEmpty(schema))
			{
				this.WriteByte(0, stateObj);
			}
			else
			{
				this.WriteByte((byte)schema.Length, stateObj);
				this.WriteString(schema, stateObj);
			}
			if (ADP.IsEmpty(type))
			{
				this.WriteByte(0, stateObj);
				return;
			}
			this.WriteByte((byte)type.Length, stateObj);
			this.WriteString(type, stateObj);
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x002973C0 File Offset: 0x002967C0
		internal MultiPartTableName[] ProcessTableName(int length, TdsParserStateObject stateObj)
		{
			int num = 0;
			MultiPartTableName[] array = new MultiPartTableName[1];
			while (length > 0)
			{
				MultiPartTableName multiPartTableName = this.ProcessOneTable(stateObj, ref length);
				if (num == 0)
				{
					array[num] = multiPartTableName;
				}
				else
				{
					MultiPartTableName[] array2 = new MultiPartTableName[array.Length + 1];
					Array.Copy(array, 0, array2, 0, array.Length);
					array2[array.Length] = multiPartTableName;
					array = array2;
				}
				num++;
			}
			return array;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x00297428 File Offset: 0x00296828
		private MultiPartTableName ProcessOneTable(TdsParserStateObject stateObj, ref int length)
		{
			MultiPartTableName multiPartTableName;
			if (this._isShilohSP1)
			{
				multiPartTableName = default(MultiPartTableName);
				byte b = stateObj.ReadByte();
				length--;
				if (b == 4)
				{
					ushort num = stateObj.ReadUInt16();
					length -= 2;
					multiPartTableName.ServerName = stateObj.ReadString((int)num);
					b -= 1;
					length -= (int)(num * 2);
				}
				if (b == 3)
				{
					ushort num = stateObj.ReadUInt16();
					length -= 2;
					multiPartTableName.CatalogName = stateObj.ReadString((int)num);
					length -= (int)(num * 2);
					b -= 1;
				}
				if (b == 2)
				{
					ushort num = stateObj.ReadUInt16();
					length -= 2;
					multiPartTableName.SchemaName = stateObj.ReadString((int)num);
					length -= (int)(num * 2);
					b -= 1;
				}
				if (b == 1)
				{
					ushort num = stateObj.ReadUInt16();
					length -= 2;
					multiPartTableName.TableName = stateObj.ReadString((int)num);
					length -= (int)(num * 2);
					b -= 1;
				}
			}
			else
			{
				ushort num = stateObj.ReadUInt16();
				length -= 2;
				string text = stateObj.ReadString((int)num);
				length -= (int)(num * 2);
				multiPartTableName = new MultiPartTableName(MultipartIdentifier.ParseMultipartIdentifier(text, "[\"", "]\"", "SQL_TDSParserTableName", false));
			}
			return multiPartTableName;
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x00297544 File Offset: 0x00296944
		private _SqlMetaDataSet ProcessColInfo(_SqlMetaDataSet columns, SqlDataReader reader, TdsParserStateObject stateObj)
		{
			for (int i = 0; i < columns.Length; i++)
			{
				_SqlMetaData sqlMetaData = columns[i];
				stateObj.ReadByte();
				sqlMetaData.tableNum = stateObj.ReadByte();
				byte b = stateObj.ReadByte();
				sqlMetaData.isDifferentName = 32 == (b & 32);
				sqlMetaData.isExpression = 4 == (b & 4);
				sqlMetaData.isKey = 8 == (b & 8);
				sqlMetaData.isHidden = 16 == (b & 16);
				if (sqlMetaData.isDifferentName)
				{
					byte b2 = stateObj.ReadByte();
					sqlMetaData.baseColumn = stateObj.ReadString((int)b2);
				}
				if (reader.TableNames != null && sqlMetaData.tableNum > 0)
				{
					sqlMetaData.multiPartTableName = reader.TableNames[(int)(sqlMetaData.tableNum - 1)];
				}
				if (sqlMetaData.isExpression)
				{
					sqlMetaData.updatability = 0;
				}
			}
			return columns;
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x0029761C File Offset: 0x00296A1C
		internal ulong ProcessColumnHeader(SqlMetaDataPriv col, TdsParserStateObject stateObj, out bool isNull)
		{
			if (col.metaType.IsLong && !col.metaType.IsPlp)
			{
				byte b = stateObj.ReadByte();
				if (b != 0)
				{
					this.SkipBytes((int)b, stateObj);
					this.SkipBytes(8, stateObj);
					isNull = false;
					return this.GetDataLength(col, stateObj);
				}
				isNull = true;
				return 0UL;
			}
			else
			{
				ulong dataLength = this.GetDataLength(col, stateObj);
				isNull = this.IsNull(col.metaType, dataLength);
				if (!isNull)
				{
					return dataLength;
				}
				return 0UL;
			}
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x00297690 File Offset: 0x00296A90
		internal int GetAltRowId(TdsParserStateObject stateObj)
		{
			stateObj.ReadByte();
			return (int)stateObj.ReadUInt16();
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x002976AC File Offset: 0x00296AAC
		private void ProcessRow(_SqlMetaDataSet columns, object[] buffer, int[] map, TdsParserStateObject stateObj)
		{
			SqlBuffer sqlBuffer = new SqlBuffer();
			for (int i = 0; i < columns.Length; i++)
			{
				_SqlMetaData sqlMetaData = columns[i];
				bool flag;
				ulong num = this.ProcessColumnHeader(sqlMetaData, stateObj, out flag);
				if (flag)
				{
					this.GetNullSqlValue(sqlBuffer, sqlMetaData);
					buffer[map[i]] = sqlBuffer.SqlValue;
				}
				else
				{
					this.ReadSqlValue(sqlBuffer, sqlMetaData, sqlMetaData.metaType.IsPlp ? int.MaxValue : ((int)num), stateObj);
					buffer[map[i]] = sqlBuffer.SqlValue;
					if (stateObj._longlen != 0UL)
					{
						throw new SqlTruncateException(Res.GetString("SqlMisc_TruncationMaxDataMessage"));
					}
				}
				sqlBuffer.Clear();
			}
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x00297750 File Offset: 0x00296B50
		internal object GetNullSqlValue(SqlBuffer nullVal, SqlMetaDataPriv md)
		{
			switch (md.type)
			{
			case SqlDbType.BigInt:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Int64);
				break;
			case SqlDbType.Binary:
			case SqlDbType.Image:
			case SqlDbType.VarBinary:
			case SqlDbType.Udt:
				nullVal.SqlBinary = SqlBinary.Null;
				break;
			case SqlDbType.Bit:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Boolean);
				break;
			case SqlDbType.Char:
			case SqlDbType.NChar:
			case SqlDbType.NText:
			case SqlDbType.NVarChar:
			case SqlDbType.Text:
			case SqlDbType.VarChar:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.String);
				break;
			case SqlDbType.DateTime:
			case SqlDbType.SmallDateTime:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.DateTime);
				break;
			case SqlDbType.Decimal:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Decimal);
				break;
			case SqlDbType.Float:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Double);
				break;
			case SqlDbType.Int:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Int32);
				break;
			case SqlDbType.Money:
			case SqlDbType.SmallMoney:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Money);
				break;
			case SqlDbType.Real:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Single);
				break;
			case SqlDbType.UniqueIdentifier:
				nullVal.SqlGuid = SqlGuid.Null;
				break;
			case SqlDbType.SmallInt:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Int16);
				break;
			case SqlDbType.TinyInt:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Byte);
				break;
			case SqlDbType.Variant:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Empty);
				break;
			case SqlDbType.Xml:
				nullVal.SqlCachedBuffer = SqlCachedBuffer.Null;
				break;
			case SqlDbType.Date:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Date);
				break;
			case SqlDbType.Time:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.Time);
				break;
			case SqlDbType.DateTime2:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.DateTime2);
				break;
			case SqlDbType.DateTimeOffset:
				nullVal.SetToNullOfType(SqlBuffer.StorageType.DateTimeOffset);
				break;
			}
			return nullVal;
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x002978D0 File Offset: 0x00296CD0
		internal void SkipRow(_SqlMetaDataSet columns, TdsParserStateObject stateObj)
		{
			this.SkipRow(columns, 0, stateObj);
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x002978E8 File Offset: 0x00296CE8
		internal void SkipRow(_SqlMetaDataSet columns, int startCol, TdsParserStateObject stateObj)
		{
			int i = startCol;
			while (i < columns.Length)
			{
				_SqlMetaData sqlMetaData = columns[i];
				if (!sqlMetaData.metaType.IsLong || sqlMetaData.metaType.IsPlp)
				{
					goto IL_003A;
				}
				byte b = stateObj.ReadByte();
				if (b != 0)
				{
					this.SkipBytes((int)(b + 8), stateObj);
					goto IL_003A;
				}
				IL_0042:
				i++;
				continue;
				IL_003A:
				this.SkipValue(sqlMetaData, stateObj);
				goto IL_0042;
			}
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x00297944 File Offset: 0x00296D44
		internal void SkipValue(SqlMetaDataPriv md, TdsParserStateObject stateObj)
		{
			if (md.metaType.IsPlp)
			{
				this.SkipPlpValue(ulong.MaxValue, stateObj);
				return;
			}
			int tokenLength = this.GetTokenLength(md.tdsType, stateObj);
			if (!this.IsNull(md.metaType, (ulong)((long)tokenLength)))
			{
				this.SkipBytes(tokenLength, stateObj);
			}
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x00297990 File Offset: 0x00296D90
		private bool IsNull(MetaType mt, ulong length)
		{
			if (mt.IsPlp)
			{
				return ulong.MaxValue == length;
			}
			return (65535UL == length && !mt.IsLong) || (0UL == length && !mt.IsCharType && !mt.IsBinType);
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x002979D8 File Offset: 0x00296DD8
		private void ReadSqlStringValue(SqlBuffer value, byte type, int length, Encoding encoding, bool isPlp, TdsParserStateObject stateObj)
		{
			if (type <= 99)
			{
				if (type <= 39)
				{
					if (type != 35 && type != 39)
					{
						return;
					}
				}
				else if (type != 47)
				{
					if (type != 99)
					{
						return;
					}
					goto IL_006B;
				}
			}
			else if (type <= 175)
			{
				if (type != 167 && type != 175)
				{
					return;
				}
			}
			else
			{
				if (type != 231 && type != 239)
				{
					return;
				}
				goto IL_006B;
			}
			if (encoding == null)
			{
				encoding = this._defaultEncoding;
			}
			value.SetToString(stateObj.ReadStringWithEncoding(length, encoding, isPlp));
			return;
			IL_006B:
			string text;
			if (isPlp)
			{
				char[] array = null;
				length = this.ReadPlpUnicodeChars(ref array, 0, length >> 1, stateObj);
				if (length > 0)
				{
					text = new string(array, 0, length);
				}
				else
				{
					text = ADP.StrEmpty;
				}
			}
			else
			{
				text = stateObj.ReadString(length >> 1);
			}
			value.SetToString(text);
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x00297A94 File Offset: 0x00296E94
		internal void ReadSqlValue(SqlBuffer value, SqlMetaDataPriv md, int length, TdsParserStateObject stateObj)
		{
			if (md.metaType.IsPlp)
			{
				length = int.MaxValue;
			}
			byte tdsType = md.tdsType;
			if (tdsType <= 108)
			{
				switch (tdsType)
				{
				case 34:
				case 37:
				case 45:
					break;
				case 35:
				case 39:
				case 47:
					goto IL_0138;
				case 36:
				case 38:
				case 44:
				case 46:
					goto IL_0183;
				case 40:
				case 41:
				case 42:
				case 43:
					this.ReadSqlDateTime(value, md.tdsType, length, md.scale, stateObj);
					return;
				default:
					if (tdsType == 99)
					{
						goto IL_0138;
					}
					switch (tdsType)
					{
					case 106:
					case 108:
						this.ReadSqlDecimal(value, length, md.precision, md.scale, stateObj);
						return;
					case 107:
						goto IL_0183;
					default:
						goto IL_0183;
					}
					break;
				}
			}
			else if (tdsType <= 175)
			{
				switch (tdsType)
				{
				case 165:
					break;
				case 166:
					goto IL_0183;
				case 167:
					goto IL_0138;
				default:
					switch (tdsType)
					{
					case 173:
						break;
					case 174:
						goto IL_0183;
					case 175:
						goto IL_0138;
					default:
						goto IL_0183;
					}
					break;
				}
			}
			else
			{
				if (tdsType == 231)
				{
					goto IL_0138;
				}
				switch (tdsType)
				{
				case 239:
					goto IL_0138;
				case 240:
					break;
				case 241:
				{
					SqlCachedBuffer sqlCachedBuffer = new SqlCachedBuffer(md, this, stateObj);
					value.SqlCachedBuffer = sqlCachedBuffer;
					return;
				}
				default:
					goto IL_0183;
				}
			}
			byte[] array = null;
			if (md.metaType.IsPlp)
			{
				stateObj.ReadPlpBytes(ref array, 0, length);
			}
			else
			{
				array = new byte[length];
				stateObj.ReadByteArray(array, 0, length);
			}
			value.SqlBinary = new SqlBinary(array, true);
			return;
			IL_0138:
			this.ReadSqlStringValue(value, md.tdsType, length, md.encoding, md.metaType.IsPlp, stateObj);
			return;
			IL_0183:
			this.ReadSqlValueInternal(value, md.tdsType, md.metaType.TypeId, length, stateObj);
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x00297C40 File Offset: 0x00297040
		private void ReadSqlDateTime(SqlBuffer value, byte tdsType, int length, byte scale, TdsParserStateObject stateObj)
		{
			stateObj.ReadByteArray(this.datetimeBuffer, 0, length);
			switch (tdsType)
			{
			case 40:
				value.SetToDate(this.datetimeBuffer);
				return;
			case 41:
				value.SetToTime(this.datetimeBuffer, length, scale);
				return;
			case 42:
				value.SetToDateTime2(this.datetimeBuffer, length, scale);
				return;
			case 43:
				value.SetToDateTimeOffset(this.datetimeBuffer, length, scale);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x00297CB4 File Offset: 0x002970B4
		internal void ReadSqlValueInternal(SqlBuffer value, byte tdsType, int typeId, int length, TdsParserStateObject stateObj)
		{
			if (tdsType <= 104)
			{
				if (tdsType <= 62)
				{
					switch (tdsType)
					{
					case 34:
					case 37:
						goto IL_01D8;
					case 35:
						return;
					case 36:
					{
						byte[] array = new byte[length];
						stateObj.ReadByteArray(array, 0, length);
						value.SqlGuid = new SqlGuid(array, true);
						return;
					}
					case 38:
						if (length != 1)
						{
							if (length == 2)
							{
								goto IL_00FE;
							}
							if (length == 4)
							{
								goto IL_010C;
							}
							goto IL_011A;
						}
						break;
					default:
						switch (tdsType)
						{
						case 45:
							goto IL_01D8;
						case 46:
						case 47:
						case 49:
						case 51:
						case 53:
						case 54:
						case 55:
						case 57:
							return;
						case 48:
							break;
						case 50:
							goto IL_00CB;
						case 52:
							goto IL_00FE;
						case 56:
							goto IL_010C;
						case 58:
							goto IL_0187;
						case 59:
							goto IL_012D;
						case 60:
							goto IL_014E;
						case 61:
							goto IL_01A2;
						case 62:
							goto IL_013B;
						default:
							return;
						}
						break;
					}
					value.Byte = stateObj.ReadByte();
					return;
					IL_00FE:
					value.Int16 = stateObj.ReadInt16();
					return;
					IL_010C:
					value.Int32 = stateObj.ReadInt32();
					return;
				}
				if (tdsType == 98)
				{
					this.ReadSqlVariant(value, length, stateObj);
					return;
				}
				if (tdsType != 104)
				{
					return;
				}
				IL_00CB:
				value.Boolean = stateObj.ReadByte() != 0;
				return;
			}
			if (tdsType <= 122)
			{
				switch (tdsType)
				{
				case 109:
					if (length == 4)
					{
						goto IL_012D;
					}
					goto IL_013B;
				case 110:
					if (length != 4)
					{
						goto IL_014E;
					}
					break;
				case 111:
					if (length == 4)
					{
						goto IL_0187;
					}
					goto IL_01A2;
				default:
					if (tdsType != 122)
					{
						return;
					}
					break;
				}
				value.SetToMoney((long)stateObj.ReadInt32());
				return;
			}
			if (tdsType != 127)
			{
				if (tdsType != 165 && tdsType != 173)
				{
					return;
				}
				goto IL_01D8;
			}
			IL_011A:
			value.Int64 = stateObj.ReadInt64();
			return;
			IL_012D:
			value.Single = stateObj.ReadSingle();
			return;
			IL_013B:
			value.Double = stateObj.ReadDouble();
			return;
			IL_014E:
			int num = stateObj.ReadInt32();
			uint num2 = stateObj.ReadUInt32();
			long num3 = ((long)num << 32) + (long)((ulong)num2);
			value.SetToMoney(num3);
			return;
			IL_0187:
			value.SetToDateTime((int)stateObj.ReadUInt16(), (int)stateObj.ReadUInt16() * SqlDateTime.SQLTicksPerMinute);
			return;
			IL_01A2:
			value.SetToDateTime(stateObj.ReadInt32(), (int)stateObj.ReadUInt32());
			return;
			IL_01D8:
			byte[] array2 = new byte[length];
			stateObj.ReadByteArray(array2, 0, length);
			value.SqlBinary = new SqlBinary(array2, true);
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x00297EC8 File Offset: 0x002972C8
		internal void ReadSqlVariant(SqlBuffer value, int lenTotal, TdsParserStateObject stateObj)
		{
			byte b = stateObj.ReadByte();
			byte b2 = stateObj.ReadByte();
			MetaType sqlDataType = MetaType.GetSqlDataType((int)b, 0U, 0);
			byte propBytes = sqlDataType.PropBytes;
			int num = (int)(2 + b2);
			int num2 = lenTotal - num;
			byte b3 = b;
			if (b3 > 127)
			{
				if (b3 <= 175)
				{
					switch (b3)
					{
					case 165:
						break;
					case 166:
						return;
					case 167:
						goto IL_016E;
					default:
						switch (b3)
						{
						case 173:
							break;
						case 174:
							return;
						case 175:
							goto IL_016E;
						default:
							return;
						}
						break;
					}
					stateObj.ReadUInt16();
					if (b2 > propBytes)
					{
						this.SkipBytes((int)(b2 - propBytes), stateObj);
						goto IL_011D;
					}
					goto IL_011D;
				}
				else if (b3 != 231 && b3 != 239)
				{
					return;
				}
				IL_016E:
				this.ProcessCollation(stateObj);
				stateObj.ReadUInt16();
				if (b2 > propBytes)
				{
					this.SkipBytes((int)(b2 - propBytes), stateObj);
				}
				this.ReadSqlStringValue(value, b, num2, null, false, stateObj);
				return;
			}
			if (b3 <= 108)
			{
				switch (b3)
				{
				case 36:
				case 48:
				case 50:
				case 52:
				case 56:
				case 58:
				case 59:
				case 60:
				case 61:
				case 62:
					break;
				case 37:
				case 38:
				case 39:
				case 44:
				case 45:
				case 46:
				case 47:
				case 49:
				case 51:
				case 53:
				case 54:
				case 55:
				case 57:
					return;
				case 40:
					this.ReadSqlDateTime(value, b, num2, 0, stateObj);
					return;
				case 41:
				case 42:
				case 43:
				{
					byte b4 = stateObj.ReadByte();
					if (b2 > propBytes)
					{
						this.SkipBytes((int)(b2 - propBytes), stateObj);
					}
					this.ReadSqlDateTime(value, b, num2, b4, stateObj);
					return;
				}
				default:
					switch (b3)
					{
					case 106:
					case 108:
					{
						byte b5 = stateObj.ReadByte();
						byte b6 = stateObj.ReadByte();
						if (b2 > propBytes)
						{
							this.SkipBytes((int)(b2 - propBytes), stateObj);
						}
						this.ReadSqlDecimal(value, 17, b5, b6, stateObj);
						return;
					}
					case 107:
						return;
					default:
						return;
					}
					break;
				}
			}
			else if (b3 != 122 && b3 != 127)
			{
				return;
			}
			IL_011D:
			this.ReadSqlValueInternal(value, b, 0, num2, stateObj);
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x002980A0 File Offset: 0x002974A0
		internal void WriteSqlVariantValue(object value, int length, int offset, TdsParserStateObject stateObj)
		{
			if (ADP.IsNull(value))
			{
				this.WriteInt(0, stateObj);
				this.WriteInt(0, stateObj);
				return;
			}
			MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(value);
			if (metaTypeFromValue.IsAnsiType)
			{
				length = this.GetEncodingCharLength((string)value, length, 0, this._defaultEncoding);
			}
			this.WriteInt((int)(2 + metaTypeFromValue.PropBytes) + length, stateObj);
			this.WriteInt((int)(2 + metaTypeFromValue.PropBytes) + length, stateObj);
			this.WriteByte(metaTypeFromValue.TDSType, stateObj);
			this.WriteByte(metaTypeFromValue.PropBytes, stateObj);
			byte tdstype = metaTypeFromValue.TDSType;
			if (tdstype <= 62)
			{
				if (tdstype == 36)
				{
					byte[] array = ((Guid)value).ToByteArray();
					this.WriteByteArray(array, length, 0, stateObj);
					return;
				}
				switch (tdstype)
				{
				case 41:
					this.WriteByte(metaTypeFromValue.Scale, stateObj);
					this.WriteTime((TimeSpan)value, metaTypeFromValue.Scale, length, stateObj);
					return;
				case 42:
					break;
				case 43:
					this.WriteByte(metaTypeFromValue.Scale, stateObj);
					this.WriteDateTimeOffset((DateTimeOffset)value, metaTypeFromValue.Scale, length, stateObj);
					break;
				default:
					switch (tdstype)
					{
					case 48:
						this.WriteByte((byte)value, stateObj);
						return;
					case 49:
					case 51:
					case 53:
					case 54:
					case 55:
					case 57:
					case 58:
						break;
					case 50:
						if ((bool)value)
						{
							this.WriteByte(1, stateObj);
							return;
						}
						this.WriteByte(0, stateObj);
						return;
					case 52:
						this.WriteShort((int)((short)value), stateObj);
						return;
					case 56:
						this.WriteInt((int)value, stateObj);
						return;
					case 59:
						this.WriteFloat((float)value, stateObj);
						return;
					case 60:
						this.WriteCurrency((decimal)value, 8, stateObj);
						return;
					case 61:
					{
						TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, 8);
						this.WriteInt(tdsDateTime.days, stateObj);
						this.WriteInt(tdsDateTime.time, stateObj);
						return;
					}
					case 62:
						this.WriteDouble((double)value, stateObj);
						return;
					default:
						return;
					}
					break;
				}
			}
			else if (tdstype <= 127)
			{
				if (tdstype == 108)
				{
					this.WriteByte(metaTypeFromValue.Precision, stateObj);
					this.WriteByte((byte)((decimal.GetBits((decimal)value)[3] & 16711680) >> 16), stateObj);
					this.WriteDecimal((decimal)value, stateObj);
					return;
				}
				if (tdstype != 127)
				{
					return;
				}
				this.WriteLong((long)value, stateObj);
				return;
			}
			else
			{
				switch (tdstype)
				{
				case 165:
				{
					byte[] array2 = (byte[])value;
					this.WriteShort(length, stateObj);
					this.WriteByteArray(array2, length, offset, stateObj);
					return;
				}
				case 166:
					break;
				case 167:
				{
					string text = (string)value;
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					this.WriteShort(length, stateObj);
					this.WriteEncodingChar(text, this._defaultEncoding, stateObj);
					return;
				}
				default:
				{
					if (tdstype != 231)
					{
						return;
					}
					string text2 = (string)value;
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					this.WriteShort(length, stateObj);
					length >>= 1;
					this.WriteString(text2, length, offset, stateObj);
					return;
				}
				}
			}
		}

		// Token: 0x06002A20 RID: 10784 RVA: 0x002983D4 File Offset: 0x002977D4
		internal void WriteSqlVariantDataRowValue(object value, TdsParserStateObject stateObj)
		{
			if (value == null || DBNull.Value == value)
			{
				this.WriteInt(0, stateObj);
				return;
			}
			MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(value);
			int num = 0;
			if (metaTypeFromValue.IsAnsiType)
			{
				num = this.GetEncodingCharLength((string)value, num, 0, this._defaultEncoding);
			}
			byte tdstype = metaTypeFromValue.TDSType;
			if (tdstype <= 62)
			{
				if (tdstype == 36)
				{
					byte[] array = ((Guid)value).ToByteArray();
					num = array.Length;
					this.WriteSqlVariantHeader(18, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
					this.WriteByteArray(array, num, 0, stateObj);
					return;
				}
				switch (tdstype)
				{
				case 41:
					this.WriteSqlVariantHeader(8, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
					this.WriteByte(metaTypeFromValue.Scale, stateObj);
					this.WriteTime((TimeSpan)value, metaTypeFromValue.Scale, 5, stateObj);
					return;
				case 42:
					break;
				case 43:
					this.WriteSqlVariantHeader(13, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
					this.WriteByte(metaTypeFromValue.Scale, stateObj);
					this.WriteDateTimeOffset((DateTimeOffset)value, metaTypeFromValue.Scale, 10, stateObj);
					break;
				default:
					switch (tdstype)
					{
					case 48:
						this.WriteSqlVariantHeader(3, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
						this.WriteByte((byte)value, stateObj);
						return;
					case 49:
					case 51:
					case 53:
					case 54:
					case 55:
					case 57:
					case 58:
						break;
					case 50:
						this.WriteSqlVariantHeader(3, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
						if ((bool)value)
						{
							this.WriteByte(1, stateObj);
							return;
						}
						this.WriteByte(0, stateObj);
						return;
					case 52:
						this.WriteSqlVariantHeader(4, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
						this.WriteShort((int)((short)value), stateObj);
						return;
					case 56:
						this.WriteSqlVariantHeader(6, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
						this.WriteInt((int)value, stateObj);
						return;
					case 59:
						this.WriteSqlVariantHeader(6, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
						this.WriteFloat((float)value, stateObj);
						return;
					case 60:
						this.WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
						this.WriteCurrency((decimal)value, 8, stateObj);
						return;
					case 61:
					{
						TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, 8);
						this.WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
						this.WriteInt(tdsDateTime.days, stateObj);
						this.WriteInt(tdsDateTime.time, stateObj);
						return;
					}
					case 62:
						this.WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
						this.WriteDouble((double)value, stateObj);
						return;
					default:
						return;
					}
					break;
				}
			}
			else if (tdstype <= 127)
			{
				if (tdstype == 108)
				{
					this.WriteSqlVariantHeader(21, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
					this.WriteByte(metaTypeFromValue.Precision, stateObj);
					this.WriteByte((byte)((decimal.GetBits((decimal)value)[3] & 16711680) >> 16), stateObj);
					this.WriteDecimal((decimal)value, stateObj);
					return;
				}
				if (tdstype != 127)
				{
					return;
				}
				this.WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
				this.WriteLong((long)value, stateObj);
				return;
			}
			else
			{
				switch (tdstype)
				{
				case 165:
				{
					byte[] array2 = (byte[])value;
					num = array2.Length;
					this.WriteSqlVariantHeader(4 + num, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
					this.WriteShort(num, stateObj);
					this.WriteByteArray(array2, num, 0, stateObj);
					return;
				}
				case 166:
					break;
				case 167:
				{
					string text = (string)value;
					num = text.Length;
					this.WriteSqlVariantHeader(9 + num, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					this.WriteShort(num, stateObj);
					this.WriteEncodingChar(text, this._defaultEncoding, stateObj);
					return;
				}
				default:
				{
					if (tdstype != 231)
					{
						return;
					}
					string text2 = (string)value;
					num = text2.Length * 2;
					this.WriteSqlVariantHeader(9 + num, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					this.WriteShort(num, stateObj);
					num >>= 1;
					this.WriteString(text2, num, 0, stateObj);
					return;
				}
				}
			}
		}

		// Token: 0x06002A21 RID: 10785 RVA: 0x00298810 File Offset: 0x00297C10
		internal void WriteSqlVariantHeader(int length, byte tdstype, byte propbytes, TdsParserStateObject stateObj)
		{
			this.WriteInt(length, stateObj);
			this.WriteByte(tdstype, stateObj);
			this.WriteByte(propbytes, stateObj);
		}

		// Token: 0x06002A22 RID: 10786 RVA: 0x00298838 File Offset: 0x00297C38
		private void WriteSqlMoney(SqlMoney value, int length, TdsParserStateObject stateObj)
		{
			int[] bits = decimal.GetBits(value.Value);
			bool flag = 0 != (bits[3] & int.MinValue);
			long num = (long)(((ulong)bits[1] << 32) | (ulong)bits[0]);
			if (flag)
			{
				num = -num;
			}
			if (length != 4)
			{
				this.WriteInt((int)(num >> 32), stateObj);
				this.WriteInt((int)num, stateObj);
				return;
			}
			decimal value2 = value.Value;
			if (value2 < TdsEnums.SQL_SMALL_MONEY_MIN || value2 > TdsEnums.SQL_SMALL_MONEY_MAX)
			{
				throw SQL.MoneyOverflow(value2.ToString(CultureInfo.InvariantCulture));
			}
			this.WriteInt((int)num, stateObj);
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x002988CC File Offset: 0x00297CCC
		private void WriteCurrency(decimal value, int length, TdsParserStateObject stateObj)
		{
			SqlMoney sqlMoney = new SqlMoney(value);
			int[] bits = decimal.GetBits(sqlMoney.Value);
			bool flag = 0 != (bits[3] & int.MinValue);
			long num = (long)(((ulong)bits[1] << 32) | (ulong)bits[0]);
			if (flag)
			{
				num = -num;
			}
			if (length != 4)
			{
				this.WriteInt((int)(num >> 32), stateObj);
				this.WriteInt((int)num, stateObj);
				return;
			}
			if (value < TdsEnums.SQL_SMALL_MONEY_MIN || value > TdsEnums.SQL_SMALL_MONEY_MAX)
			{
				throw SQL.MoneyOverflow(value.ToString(CultureInfo.InvariantCulture));
			}
			this.WriteInt((int)num, stateObj);
		}

		// Token: 0x06002A24 RID: 10788 RVA: 0x00298960 File Offset: 0x00297D60
		private void WriteDate(DateTime value, TdsParserStateObject stateObj)
		{
			int days = value.Subtract(DateTime.MinValue).Days;
			this.WriteByteArray(BitConverter.GetBytes(days), 3, 0, stateObj);
		}

		// Token: 0x06002A25 RID: 10789 RVA: 0x00298994 File Offset: 0x00297D94
		private void WriteTime(TimeSpan value, byte scale, int length, TdsParserStateObject stateObj)
		{
			if (0L > value.Ticks || value.Ticks >= 864000000000L)
			{
				throw SQL.TimeOverflow(value.ToString());
			}
			long num = value.Ticks / TdsEnums.TICKS_FROM_SCALE[(int)scale];
			this.WriteByteArray(BitConverter.GetBytes(num), length, 0, stateObj);
		}

		// Token: 0x06002A26 RID: 10790 RVA: 0x002989F4 File Offset: 0x00297DF4
		private void WriteDateTime2(DateTime value, byte scale, int length, TdsParserStateObject stateObj)
		{
			long num = value.TimeOfDay.Ticks / TdsEnums.TICKS_FROM_SCALE[(int)scale];
			this.WriteByteArray(BitConverter.GetBytes(num), length - 3, 0, stateObj);
			this.WriteDate(value, stateObj);
		}

		// Token: 0x06002A27 RID: 10791 RVA: 0x00298A34 File Offset: 0x00297E34
		private void WriteDateTimeOffset(DateTimeOffset value, byte scale, int length, TdsParserStateObject stateObj)
		{
			this.WriteDateTime2(value.UtcDateTime, scale, length - 2, stateObj);
			short num = (short)value.Offset.TotalMinutes;
			this.WriteByte((byte)(num & 255), stateObj);
			this.WriteByte((byte)((num >> 8) & 255), stateObj);
		}

		// Token: 0x06002A28 RID: 10792 RVA: 0x00298A88 File Offset: 0x00297E88
		private void ReadSqlDecimal(SqlBuffer value, int length, byte precision, byte scale, TdsParserStateObject stateObj)
		{
			bool flag = 1 == stateObj.ReadByte();
			checked
			{
				length--;
				int[] array = this.ReadDecimalBits(length, stateObj);
				value.SetToDecimal(precision, scale, flag, array);
			}
		}

		// Token: 0x06002A29 RID: 10793 RVA: 0x00298ABC File Offset: 0x00297EBC
		private int[] ReadDecimalBits(int length, TdsParserStateObject stateObj)
		{
			int[] array = stateObj._decimalBits;
			if (array == null)
			{
				array = new int[4];
			}
			else
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 0;
				}
			}
			int num = length >> 2;
			for (int i = 0; i < num; i++)
			{
				array[i] = stateObj.ReadInt32();
			}
			return array;
		}

		// Token: 0x06002A2A RID: 10794 RVA: 0x00298B08 File Offset: 0x00297F08
		internal static SqlDecimal AdjustSqlDecimalScale(SqlDecimal d, int newScale)
		{
			if ((int)d.Scale != newScale)
			{
				return SqlDecimal.AdjustScale(d, newScale - (int)d.Scale, false);
			}
			return d;
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x00298B34 File Offset: 0x00297F34
		internal static decimal AdjustDecimalScale(decimal value, int newScale)
		{
			int num = (decimal.GetBits(value)[3] & 16711680) >> 16;
			if (newScale != num)
			{
				SqlDecimal sqlDecimal = new SqlDecimal(value);
				sqlDecimal = SqlDecimal.AdjustScale(sqlDecimal, newScale - num, false);
				return sqlDecimal.Value;
			}
			return value;
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x00298B74 File Offset: 0x00297F74
		internal void WriteSqlDecimal(SqlDecimal d, TdsParserStateObject stateObj)
		{
			if (d.IsPositive)
			{
				this.WriteByte(1, stateObj);
			}
			else
			{
				this.WriteByte(0, stateObj);
			}
			int[] data = d.Data;
			this.WriteInt(data[0], stateObj);
			this.WriteInt(data[1], stateObj);
			this.WriteInt(data[2], stateObj);
			this.WriteInt(data[3], stateObj);
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x00298BCC File Offset: 0x00297FCC
		private void WriteDecimal(decimal value, TdsParserStateObject stateObj)
		{
			stateObj._decimalBits = decimal.GetBits(value);
			if ((ulong)(-2147483648) == (ulong)((long)stateObj._decimalBits[3] & (long)((ulong)(-2147483648))))
			{
				this.WriteByte(0, stateObj);
			}
			else
			{
				this.WriteByte(1, stateObj);
			}
			this.WriteInt(stateObj._decimalBits[0], stateObj);
			this.WriteInt(stateObj._decimalBits[1], stateObj);
			this.WriteInt(stateObj._decimalBits[2], stateObj);
			this.WriteInt(0, stateObj);
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x00298C44 File Offset: 0x00298044
		private void WriteIdentifier(string s, TdsParserStateObject stateObj)
		{
			if (s != null)
			{
				this.WriteByte(checked((byte)s.Length), stateObj);
				this.WriteString(s, stateObj);
				return;
			}
			this.WriteByte(0, stateObj);
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x00298C74 File Offset: 0x00298074
		private void WriteIdentifierWithShortLength(string s, TdsParserStateObject stateObj)
		{
			if (s != null)
			{
				this.WriteShort((int)(checked((short)s.Length)), stateObj);
				this.WriteString(s, stateObj);
				return;
			}
			this.WriteShort(0, stateObj);
		}

		// Token: 0x06002A30 RID: 10800 RVA: 0x00298CA4 File Offset: 0x002980A4
		private void WriteString(string s, TdsParserStateObject stateObj)
		{
			this.WriteString(s, s.Length, 0, stateObj);
		}

		// Token: 0x06002A31 RID: 10801 RVA: 0x00298CC0 File Offset: 0x002980C0
		internal void WriteCharArray(char[] carr, int length, int offset, TdsParserStateObject stateObj)
		{
			int num = ADP.CharSize * length;
			if (num < stateObj._outBuff.Length - stateObj._outBytesUsed)
			{
				TdsParser.CopyCharsToBytes(carr, offset, stateObj._outBuff, stateObj._outBytesUsed, length);
				stateObj._outBytesUsed += num;
				return;
			}
			if (stateObj._bTmp == null || stateObj._bTmp.Length < num)
			{
				stateObj._bTmp = new byte[num];
			}
			TdsParser.CopyCharsToBytes(carr, offset, stateObj._bTmp, 0, length);
			this.WriteByteArray(stateObj._bTmp, num, 0, stateObj);
		}

		// Token: 0x06002A32 RID: 10802 RVA: 0x00298D54 File Offset: 0x00298154
		internal void WriteString(string s, int length, int offset, TdsParserStateObject stateObj)
		{
			int num = ADP.CharSize * length;
			if (num < stateObj._outBuff.Length - stateObj._outBytesUsed)
			{
				TdsParser.CopyStringToBytes(s, offset, stateObj._outBuff, stateObj._outBytesUsed, length);
				stateObj._outBytesUsed += num;
				return;
			}
			if (stateObj._bTmp == null || stateObj._bTmp.Length < num)
			{
				stateObj._bTmp = new byte[num];
			}
			TdsParser.CopyStringToBytes(s, offset, stateObj._bTmp, 0, length);
			this.WriteByteArray(stateObj._bTmp, num, 0, stateObj);
		}

		// Token: 0x06002A33 RID: 10803 RVA: 0x00298DE8 File Offset: 0x002981E8
		private unsafe static void CopyCharsToBytes(char[] source, int sourceOffset, byte[] dest, int destOffset, int charLength)
		{
			if (charLength < 0)
			{
				throw ADP.InvalidDataLength((long)charLength);
			}
			int num;
			checked
			{
				if (sourceOffset + charLength > source.Length || sourceOffset < 0)
				{
					throw ADP.IndexOutOfRange(sourceOffset);
				}
				num = charLength * ADP.CharSize;
				if (destOffset + num > dest.Length || destOffset < 0)
				{
					throw ADP.IndexOutOfRange(destOffset);
				}
			}
			fixed (char* ptr = source)
			{
				char* ptr2 = ptr;
				ptr2 += sourceOffset;
				fixed (byte* ptr3 = dest)
				{
					byte* ptr4 = ptr3;
					ptr4 += destOffset;
					NativeOledbWrapper.MemoryCopy((IntPtr)((void*)ptr4), (IntPtr)((void*)ptr2), num);
				}
			}
		}

		// Token: 0x06002A34 RID: 10804 RVA: 0x00298E94 File Offset: 0x00298294
		private unsafe static void CopyStringToBytes(string source, int sourceOffset, byte[] dest, int destOffset, int charLength)
		{
			if (charLength < 0)
			{
				throw ADP.InvalidDataLength((long)charLength);
			}
			if (sourceOffset + charLength > source.Length || sourceOffset < 0)
			{
				throw ADP.IndexOutOfRange(sourceOffset);
			}
			int num = checked(charLength * ADP.CharSize);
			if (destOffset + num > dest.Length || destOffset < 0)
			{
				throw ADP.IndexOutOfRange(destOffset);
			}
			fixed (char* ptr = source)
			{
				char* ptr2 = ptr;
				ptr2 += sourceOffset;
				fixed (byte* ptr3 = dest)
				{
					byte* ptr4 = ptr3;
					ptr4 += destOffset;
					NativeOledbWrapper.MemoryCopy((IntPtr)((void*)ptr4), (IntPtr)((void*)ptr2), num);
				}
			}
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x00298F38 File Offset: 0x00298338
		private void WriteEncodingChar(string s, Encoding encoding, TdsParserStateObject stateObj)
		{
			this.WriteEncodingChar(s, s.Length, 0, encoding, stateObj);
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x00298F58 File Offset: 0x00298358
		private void WriteEncodingChar(string s, int numChars, int offset, Encoding encoding, TdsParserStateObject stateObj)
		{
			if (encoding == null)
			{
				encoding = this._defaultEncoding;
			}
			char[] array = s.ToCharArray(offset, numChars);
			byte[] bytes = encoding.GetBytes(array, 0, numChars);
			this.WriteByteArray(bytes, bytes.Length, 0, stateObj);
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x00298F94 File Offset: 0x00298394
		internal int GetEncodingCharLength(string value, int numChars, int charOffset, Encoding encoding)
		{
			if (value == null || value == ADP.StrEmpty)
			{
				return 0;
			}
			if (encoding == null)
			{
				if (this._defaultEncoding == null)
				{
					this.ThrowUnsupportedCollationEncountered(null);
				}
				encoding = this._defaultEncoding;
			}
			char[] array = value.ToCharArray(charOffset, numChars);
			return encoding.GetByteCount(array, 0, numChars);
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x00298FE4 File Offset: 0x002983E4
		internal ulong GetDataLength(SqlMetaDataPriv colmeta, TdsParserStateObject stateObj)
		{
			if (this._isYukon && colmeta.metaType.IsPlp)
			{
				return stateObj.ReadPlpLength(true);
			}
			return (ulong)((long)this.GetTokenLength(colmeta.tdsType, stateObj));
		}

		// Token: 0x06002A39 RID: 10809 RVA: 0x0029901C File Offset: 0x0029841C
		internal int GetTokenLength(byte token, TdsParserStateObject stateObj)
		{
			if (this._isYukon)
			{
				if (token == 240)
				{
					return -1;
				}
				if (token == 172)
				{
					return -1;
				}
				if (token == 241)
				{
					return (int)stateObj.ReadUInt16();
				}
			}
			int num = (int)(token & 48);
			if (num <= 16)
			{
				if (num != 0)
				{
					if (num != 16)
					{
						return 0;
					}
					return 0;
				}
			}
			else if (num != 32)
			{
				if (num == 48)
				{
					return (1 << ((token & 12) >> 2)) & 255;
				}
				return 0;
			}
			if ((token & 128) != 0)
			{
				return (int)stateObj.ReadUInt16();
			}
			if ((token & 12) == 0)
			{
				return stateObj.ReadInt32();
			}
			return (int)stateObj.ReadByte();
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x002990B0 File Offset: 0x002984B0
		private void ProcessAttention(TdsParserStateObject stateObj)
		{
			if (this._state == TdsParserState.Closed || this._state == TdsParserState.Broken)
			{
				return;
			}
			lock (this._ErrorCollectionLock)
			{
				this._attentionErrors = this._errors;
				this._attentionWarnings = this._warnings;
				this._errors = null;
				this._warnings = null;
				try
				{
					this.Run(RunBehavior.Attention, null, null, null, stateObj);
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					ADP.TraceExceptionWithoutRethrow(ex);
					this._state = TdsParserState.Broken;
					this._connHandler.BreakConnection();
					throw;
				}
				this._errors = this._attentionErrors;
				this._warnings = this._attentionWarnings;
				this._attentionErrors = null;
				this._attentionWarnings = null;
			}
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x0029919C File Offset: 0x0029859C
		internal void TdsLogin(SqlLogin rec)
		{
			this._physicalStateObj.SetTimeoutSeconds(rec.timeout);
			byte[] array = TdsParserStaticMethods.EncryptPassword(rec.password);
			byte[] array2 = TdsParserStaticMethods.EncryptPassword(rec.newPassword);
			this._physicalStateObj._outputMessageType = 16;
			int num = 94;
			string text = ".Net SqlClient Data Provider";
			byte[] array3;
			uint num2;
			checked
			{
				num += (rec.hostName.Length + rec.applicationName.Length + rec.serverName.Length + text.Length + rec.language.Length + rec.database.Length + rec.attachDBFilename.Length) * 2;
				array3 = null;
				num2 = 0U;
				if (!rec.useSSPI)
				{
					num += rec.userName.Length * 2 + array.Length + array2.Length;
				}
				else if (rec.useSSPI)
				{
					array3 = new byte[TdsParser.s_maxSSPILength];
					num2 = TdsParser.s_maxSSPILength;
					this._physicalStateObj.SniContext = SniContext.Snix_LoginSspi;
					this.SSPIData(null, 0U, array3, ref num2);
					if (num2 > 2147483647U)
					{
						throw SQL.InvalidSSPIPacketSize();
					}
					this._physicalStateObj.SniContext = SniContext.Snix_Login;
					num += (int)num2;
				}
			}
			try
			{
				this.WriteInt(num, this._physicalStateObj);
				this.WriteInt(1930035203, this._physicalStateObj);
				this.WriteInt(rec.packetSize, this._physicalStateObj);
				this.WriteInt(100663296, this._physicalStateObj);
				this.WriteInt(TdsParserStaticMethods.GetCurrentProcessId(), this._physicalStateObj);
				this.WriteInt(0, this._physicalStateObj);
				int num3 = 0;
				num3 |= 32;
				num3 |= 64;
				num3 |= 128;
				num3 |= 256;
				num3 |= 512;
				if (rec.useReplication)
				{
					num3 |= 12288;
				}
				if (rec.useSSPI)
				{
					num3 |= 32768;
				}
				if (rec.readOnlyIntent)
				{
					num3 |= 2097152;
				}
				if (!ADP.IsEmpty(rec.newPassword))
				{
					num3 |= 16777216;
				}
				if (rec.userInstance)
				{
					num3 |= 67108864;
				}
				this.WriteInt(num3, this._physicalStateObj);
				if (Bid.AdvancedOn)
				{
					Bid.Trace("<sc.TdsParser.TdsLogin|ADV> %d#, TDS Login7 flags = %d:\n", this.ObjectID, num3);
				}
				this.WriteInt(0, this._physicalStateObj);
				this.WriteInt(0, this._physicalStateObj);
				int num4 = 94;
				this.WriteShort(num4, this._physicalStateObj);
				this.WriteShort(rec.hostName.Length, this._physicalStateObj);
				num4 += rec.hostName.Length * 2;
				if (!rec.useSSPI)
				{
					this.WriteShort(num4, this._physicalStateObj);
					this.WriteShort(rec.userName.Length, this._physicalStateObj);
					num4 += rec.userName.Length * 2;
					this.WriteShort(num4, this._physicalStateObj);
					this.WriteShort(array.Length / 2, this._physicalStateObj);
					num4 += array.Length;
				}
				else
				{
					this.WriteShort(0, this._physicalStateObj);
					this.WriteShort(0, this._physicalStateObj);
					this.WriteShort(0, this._physicalStateObj);
					this.WriteShort(0, this._physicalStateObj);
				}
				this.WriteShort(num4, this._physicalStateObj);
				this.WriteShort(rec.applicationName.Length, this._physicalStateObj);
				num4 += rec.applicationName.Length * 2;
				this.WriteShort(num4, this._physicalStateObj);
				this.WriteShort(rec.serverName.Length, this._physicalStateObj);
				num4 += rec.serverName.Length * 2;
				this.WriteShort(num4, this._physicalStateObj);
				this.WriteShort(0, this._physicalStateObj);
				this.WriteShort(num4, this._physicalStateObj);
				this.WriteShort(text.Length, this._physicalStateObj);
				num4 += text.Length * 2;
				this.WriteShort(num4, this._physicalStateObj);
				this.WriteShort(rec.language.Length, this._physicalStateObj);
				num4 += rec.language.Length * 2;
				this.WriteShort(num4, this._physicalStateObj);
				this.WriteShort(rec.database.Length, this._physicalStateObj);
				num4 += rec.database.Length * 2;
				if (TdsParser.s_nicAddress == null)
				{
					TdsParser.s_nicAddress = TdsParserStaticMethods.GetNIC();
				}
				this.WriteByteArray(TdsParser.s_nicAddress, TdsParser.s_nicAddress.Length, 0, this._physicalStateObj);
				this.WriteShort(num4, this._physicalStateObj);
				if (rec.useSSPI)
				{
					this.WriteShort((int)num2, this._physicalStateObj);
					num4 += (int)num2;
				}
				else
				{
					this.WriteShort(0, this._physicalStateObj);
				}
				this.WriteShort(num4, this._physicalStateObj);
				this.WriteShort(rec.attachDBFilename.Length, this._physicalStateObj);
				num4 += rec.attachDBFilename.Length * 2;
				this.WriteShort(num4, this._physicalStateObj);
				this.WriteShort(array2.Length / 2, this._physicalStateObj);
				this.WriteInt(0, this._physicalStateObj);
				this.WriteString(rec.hostName, this._physicalStateObj);
				if (!rec.useSSPI)
				{
					this.WriteString(rec.userName, this._physicalStateObj);
					this._physicalStateObj._tracePasswordOffset = this._physicalStateObj._outBytesUsed;
					this._physicalStateObj._tracePasswordLength = array.Length;
					this.WriteByteArray(array, array.Length, 0, this._physicalStateObj);
				}
				this.WriteString(rec.applicationName, this._physicalStateObj);
				this.WriteString(rec.serverName, this._physicalStateObj);
				this.WriteString(text, this._physicalStateObj);
				this.WriteString(rec.language, this._physicalStateObj);
				this.WriteString(rec.database, this._physicalStateObj);
				if (rec.useSSPI)
				{
					this.WriteByteArray(array3, (int)num2, 0, this._physicalStateObj);
				}
				this.WriteString(rec.attachDBFilename, this._physicalStateObj);
				if (!rec.useSSPI)
				{
					this._physicalStateObj._traceChangePasswordOffset = this._physicalStateObj._outBytesUsed;
					this._physicalStateObj._traceChangePasswordLength = array2.Length;
					this.WriteByteArray(array2, array2.Length, 0, this._physicalStateObj);
				}
			}
			catch (Exception ex)
			{
				if (ADP.IsCatchableExceptionType(ex))
				{
					this._physicalStateObj._outputPacketNumber = 1;
					this._physicalStateObj.ResetBuffer();
				}
				throw;
			}
			this._physicalStateObj.WritePacket(1);
			this._physicalStateObj._pendingData = true;
		}

		// Token: 0x06002A3C RID: 10812 RVA: 0x00299800 File Offset: 0x00298C00
		private void SSPIData(byte[] receivedBuff, uint receivedLength, byte[] sendBuff, ref uint sendLength)
		{
			this.SNISSPIData(receivedBuff, receivedLength, sendBuff, ref sendLength);
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x00299818 File Offset: 0x00298C18
		private void SNISSPIData(byte[] receivedBuff, uint receivedLength, byte[] sendBuff, ref uint sendLength)
		{
			if (receivedBuff == null)
			{
				receivedLength = 0U;
			}
			if (SNINativeMethodWrapper.SNISecGenClientContext(this._physicalStateObj.Handle, receivedBuff, receivedLength, sendBuff, ref sendLength, this._sniSpnBuffer) != 0U)
			{
				this.SSPIError(SQLMessage.SSPIGenerateError(), "GenClientContext");
			}
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x00299858 File Offset: 0x00298C58
		private void ProcessSSPI(int receivedLength)
		{
			SniContext sniContext = this._physicalStateObj.SniContext;
			this._physicalStateObj.SniContext = SniContext.Snix_ProcessSspi;
			byte[] array = new byte[receivedLength];
			this._physicalStateObj.ReadByteArray(array, 0, receivedLength);
			byte[] array2 = new byte[TdsParser.s_maxSSPILength];
			uint num = TdsParser.s_maxSSPILength;
			this.SSPIData(array, (uint)receivedLength, array2, ref num);
			this.WriteByteArray(array2, (int)num, 0, this._physicalStateObj);
			this._physicalStateObj._outputMessageType = 17;
			this._physicalStateObj.WritePacket(1);
			this._physicalStateObj.SniContext = sniContext;
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x002998E8 File Offset: 0x00298CE8
		private void SSPIError(string error, string procedure)
		{
			this.Errors.Add(new SqlError(0, 0, 11, this._server, error, procedure, 0));
			this.ThrowExceptionAndWarning(this._physicalStateObj);
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x00299920 File Offset: 0x00298D20
		private void LoadSSPILibrary()
		{
			if (!TdsParser.s_fSSPILoaded)
			{
				lock (TdsParser.s_tdsParserLock)
				{
					if (!TdsParser.s_fSSPILoaded)
					{
						uint num = 0U;
						if (SNINativeMethodWrapper.SNISecInitPackage(ref num) != 0U)
						{
							this.SSPIError(SQLMessage.SSPIInitializeError(), "InitSSPIPackage");
						}
						TdsParser.s_maxSSPILength = num;
						TdsParser.s_fSSPILoaded = true;
					}
				}
			}
			if (TdsParser.s_maxSSPILength > 2147483647U)
			{
				throw SQL.InvalidSSPIPacketSize();
			}
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x002999AC File Offset: 0x00298DAC
		internal byte[] GetDTCAddress(int timeout, TdsParserStateObject stateObj)
		{
			byte[] array = null;
			using (SqlDataReader sqlDataReader = this.TdsExecuteTransactionManagerRequest(null, TdsEnums.TransactionManagerRequestType.GetDTCAddress, null, TdsEnums.TransactionManagerIsolationLevel.Unspecified, timeout, null, stateObj, false))
			{
				if (sqlDataReader != null && sqlDataReader.Read())
				{
					long bytes = sqlDataReader.GetBytes(0, 0L, null, 0, 0);
					if (bytes <= 2147483647L)
					{
						int num = (int)bytes;
						array = new byte[num];
						sqlDataReader.GetBytes(0, 0L, array, 0, num);
					}
				}
			}
			return array;
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x00299A2C File Offset: 0x00298E2C
		internal void PropagateDistributedTransaction(byte[] buffer, int timeout, TdsParserStateObject stateObj)
		{
			this.TdsExecuteTransactionManagerRequest(buffer, TdsEnums.TransactionManagerRequestType.Propagate, null, TdsEnums.TransactionManagerIsolationLevel.Unspecified, timeout, null, stateObj, false);
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x00299A48 File Offset: 0x00298E48
		internal SqlDataReader TdsExecuteTransactionManagerRequest(byte[] buffer, TdsEnums.TransactionManagerRequestType request, string transactionName, TdsEnums.TransactionManagerIsolationLevel isoLevel, int timeout, SqlInternalTransaction transaction, TdsParserStateObject stateObj, bool isDelegateControlRequest)
		{
			if (TdsParserState.Broken == this.State || this.State == TdsParserState.Closed)
			{
				return null;
			}
			bool flag = false;
			SqlDataReader sqlDataReader2;
			lock (this._connHandler)
			{
				try
				{
					if (this._isYukon && !this.MARSOn)
					{
						Monitor.Enter(this._physicalStateObj);
						flag = true;
					}
					if (!isDelegateControlRequest)
					{
						this._connHandler.ValidateTransaction();
					}
					stateObj._outputMessageType = 14;
					stateObj.SetTimeoutSeconds(timeout);
					stateObj.SniContext = SniContext.Snix_Execute;
					if (this._isYukon)
					{
						this.WriteMarsHeader(stateObj, this._currentTransaction);
					}
					this.WriteShort((int)((short)request), stateObj);
					bool flag2 = false;
					switch (request)
					{
					case TdsEnums.TransactionManagerRequestType.GetDTCAddress:
						this.WriteShort(0, stateObj);
						flag2 = true;
						break;
					case TdsEnums.TransactionManagerRequestType.Propagate:
						if (buffer != null)
						{
							this.WriteShort(buffer.Length, stateObj);
							this.WriteByteArray(buffer, buffer.Length, 0, stateObj);
						}
						else
						{
							this.WriteShort(0, stateObj);
						}
						break;
					case TdsEnums.TransactionManagerRequestType.Begin:
						if (this._currentTransaction != transaction)
						{
							this.PendingTransaction = transaction;
						}
						this.WriteByte((byte)isoLevel, stateObj);
						this.WriteByte((byte)(transactionName.Length * 2), stateObj);
						this.WriteString(transactionName, stateObj);
						break;
					case TdsEnums.TransactionManagerRequestType.Commit:
						this.WriteByte(0, stateObj);
						this.WriteByte(0, stateObj);
						break;
					case TdsEnums.TransactionManagerRequestType.Rollback:
						this.WriteByte((byte)(transactionName.Length * 2), stateObj);
						this.WriteString(transactionName, stateObj);
						this.WriteByte(0, stateObj);
						break;
					case TdsEnums.TransactionManagerRequestType.Save:
						this.WriteByte((byte)(transactionName.Length * 2), stateObj);
						this.WriteString(transactionName, stateObj);
						break;
					}
					stateObj.WritePacket(1);
					stateObj._pendingData = true;
					SqlDataReader sqlDataReader = null;
					stateObj.SniContext = SniContext.Snix_Read;
					if (flag2)
					{
						sqlDataReader = new SqlDataReader(null, CommandBehavior.Default);
						sqlDataReader.Bind(stateObj);
						_SqlMetaDataSet metaData = sqlDataReader.MetaData;
					}
					else
					{
						this.Run(RunBehavior.UntilDone, null, null, null, stateObj);
					}
					sqlDataReader2 = sqlDataReader;
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					this.FailureCleanup(stateObj, ex);
					throw;
				}
				finally
				{
					this._pendingTransaction = null;
					if (flag)
					{
						Monitor.Exit(this._physicalStateObj);
					}
				}
			}
			return sqlDataReader2;
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x00299CB4 File Offset: 0x002990B4
		internal void FailureCleanup(TdsParserStateObject stateObj, Exception e)
		{
			int outputPacketNumber = (int)stateObj._outputPacketNumber;
			if (Bid.TraceOn)
			{
				Bid.Trace("<sc.TdsParser.FailureCleanup|ERR> Exception caught on ExecuteXXX: '%ls' \n", e.ToString());
			}
			if (stateObj.HasOpenResult)
			{
				stateObj.DecrementOpenResultCount();
			}
			stateObj.ResetBuffer();
			stateObj._outputPacketNumber = 1;
			if (outputPacketNumber != 1 && this._state == TdsParserState.OpenLoggedIn)
			{
				stateObj.SendAttention();
				this.ProcessAttention(stateObj);
			}
			Bid.Trace("<sc.TdsParser.FailureCleanup|ERR> Exception rethrown. \n");
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x00299D20 File Offset: 0x00299120
		internal void TdsExecuteSQLBatch(string text, int timeout, SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj)
		{
			if (TdsParserState.Broken == this.State || this.State == TdsParserState.Closed)
			{
				return;
			}
			if (stateObj.BcpLock)
			{
				throw SQL.ConnectionLockedForBcpEvent();
			}
			bool flag = false;
			lock (this._connHandler)
			{
				try
				{
					if (this._isYukon && !this.MARSOn)
					{
						Monitor.Enter(this._physicalStateObj);
						flag = true;
					}
					this._connHandler.ValidateTransaction();
					stateObj.SetTimeoutSeconds(timeout);
					stateObj.SniContext = SniContext.Snix_Execute;
					if (this._isYukon)
					{
						this.WriteMarsHeader(stateObj, this.CurrentTransaction);
						if (notificationRequest != null)
						{
							this.WriteQueryNotificationHeader(notificationRequest, stateObj);
						}
					}
					stateObj._outputMessageType = 1;
					this.WriteString(text, text.Length, 0, stateObj);
					stateObj.ExecuteFlush();
					stateObj.SniContext = SniContext.Snix_Read;
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					this.FailureCleanup(stateObj, ex);
					throw;
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this._physicalStateObj);
					}
				}
			}
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x00299E60 File Offset: 0x00299260
		internal void TdsExecuteRPC(_SqlRPC[] rpcArray, int timeout, bool inSchema, SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj, bool isCommandProc)
		{
			if (TdsParserState.Broken == this.State || this.State == TdsParserState.Closed)
			{
				return;
			}
			bool flag = false;
			lock (this._connHandler)
			{
				try
				{
					if (this._isYukon && !this.MARSOn)
					{
						Monitor.Enter(this._physicalStateObj);
						flag = true;
					}
					this._connHandler.ValidateTransaction();
					stateObj.SetTimeoutSeconds(timeout);
					stateObj.SniContext = SniContext.Snix_Execute;
					if (this._isYukon)
					{
						this.WriteMarsHeader(stateObj, this.CurrentTransaction);
						if (notificationRequest != null)
						{
							this.WriteQueryNotificationHeader(notificationRequest, stateObj);
						}
					}
					stateObj._outputMessageType = 3;
					for (int i = 0; i < rpcArray.Length; i++)
					{
						_SqlRPC sqlRPC = rpcArray[i];
						if (sqlRPC.ProcID != 0 && this._isShiloh)
						{
							this.WriteShort(65535, stateObj);
							this.WriteShort((int)((short)sqlRPC.ProcID), stateObj);
						}
						else
						{
							int num = sqlRPC.rpcName.Length;
							this.WriteShort(num, stateObj);
							this.WriteString(sqlRPC.rpcName, num, 0, stateObj);
						}
						this.WriteShort((int)((short)sqlRPC.options), stateObj);
						SqlParameter[] parameters = sqlRPC.parameters;
						for (int j = 0; j < parameters.Length; j++)
						{
							SqlParameter sqlParameter = parameters[j];
							if (sqlParameter == null)
							{
								break;
							}
							sqlParameter.Validate(j, isCommandProc);
							MetaType internalMetaType = sqlParameter.InternalMetaType;
							if (internalMetaType.IsNewKatmaiType)
							{
								this.WriteSmiParameter(sqlParameter, j, 0 != (sqlRPC.paramoptions[j] & 2), stateObj);
							}
							else
							{
								if ((!this._isShiloh && !internalMetaType.Is70Supported) || (!this._isYukon && !internalMetaType.Is80Supported) || (!this._isKatmai && !internalMetaType.Is90Supported))
								{
									throw ADP.VersionDoesNotSupportDataType(internalMetaType.TypeName);
								}
								object obj;
								if (sqlParameter.Direction == ParameterDirection.Output)
								{
									bool paramaterIsSqlType = sqlParameter.ParamaterIsSqlType;
									sqlParameter.Value = null;
									obj = null;
									sqlParameter.ParamaterIsSqlType = paramaterIsSqlType;
								}
								else
								{
									obj = sqlParameter.GetCoercedValue();
								}
								bool flag3;
								bool flag2 = ADP.IsNull(obj, out flag3);
								string parameterNameFixed = sqlParameter.ParameterNameFixed;
								this.WriteParameterName(parameterNameFixed, stateObj);
								this.WriteByte(sqlRPC.paramoptions[j], stateObj);
								this.WriteByte(internalMetaType.NullableType, stateObj);
								if (internalMetaType.TDSType == 98)
								{
									this.WriteSqlVariantValue(flag3 ? MetaType.GetComValueFromSqlVariant(obj) : obj, sqlParameter.GetActualSize(), sqlParameter.Offset, stateObj);
								}
								else
								{
									int num2 = (internalMetaType.IsSizeInCharacters ? (sqlParameter.GetParameterSize() * 2) : sqlParameter.GetParameterSize());
									int num3;
									if (internalMetaType.TDSType != 240)
									{
										num3 = sqlParameter.GetActualSize();
									}
									else
									{
										num3 = 0;
									}
									int num4 = 0;
									int num5 = 0;
									if (internalMetaType.IsAnsiType)
									{
										if (!flag2)
										{
											string text;
											if (flag3)
											{
												if (obj is SqlString)
												{
													text = ((SqlString)obj).Value;
												}
												else
												{
													text = new string(((SqlChars)obj).Value);
												}
											}
											else
											{
												text = (string)obj;
											}
											num4 = this.GetEncodingCharLength(text, num3, sqlParameter.Offset, this._defaultEncoding);
										}
										if (internalMetaType.IsPlp)
										{
											this.WriteShort(65535, stateObj);
										}
										else
										{
											num5 = ((num2 > num4) ? num2 : num4);
											if (num5 == 0)
											{
												if (internalMetaType.IsNCharType)
												{
													num5 = 2;
												}
												else
												{
													num5 = 1;
												}
											}
											this.WriteParameterVarLen(internalMetaType, num5, false, stateObj);
										}
									}
									else if (internalMetaType.SqlDbType == SqlDbType.Timestamp)
									{
										this.WriteParameterVarLen(internalMetaType, 8, false, stateObj);
									}
									else if (internalMetaType.SqlDbType == SqlDbType.Udt)
									{
										byte[] array = null;
										bool flag4 = ADP.IsNull(obj);
										Format format = Format.Native;
										if (!flag4)
										{
											array = this._connHandler.Connection.GetBytes(obj, out format, out num5);
											num2 = array.Length;
											if (num2 < 0 || (num2 >= 65535 && num5 != -1))
											{
												throw new IndexOutOfRangeException();
											}
										}
										BitConverter.GetBytes((long)num2);
										if (ADP.IsEmpty(sqlParameter.UdtTypeName))
										{
											throw SQL.MustSetUdtTypeNameForUdtParams();
										}
										string[] array2 = SqlParameter.ParseTypeName(sqlParameter.UdtTypeName, true);
										if (!ADP.IsEmpty(array2[0]) && 255 < array2[0].Length)
										{
											throw ADP.ArgumentOutOfRange("names");
										}
										if (!ADP.IsEmpty(array2[1]) && 255 < array2[array2.Length - 2].Length)
										{
											throw ADP.ArgumentOutOfRange("names");
										}
										if (255 < array2[2].Length)
										{
											throw ADP.ArgumentOutOfRange("names");
										}
										this.WriteUDTMetaData(obj, array2[0], array2[1], array2[2], stateObj);
										if (!flag4)
										{
											this.WriteUnsignedLong((ulong)((long)array.Length), stateObj);
											if (array.Length > 0)
											{
												this.WriteInt(array.Length, stateObj);
												this.WriteByteArray(array, array.Length, 0, stateObj);
											}
											this.WriteInt(0, stateObj);
											goto IL_07D3;
										}
										this.WriteUnsignedLong(ulong.MaxValue, stateObj);
										goto IL_07D3;
									}
									else if (internalMetaType.IsPlp)
									{
										if (internalMetaType.SqlDbType != SqlDbType.Xml)
										{
											this.WriteShort(65535, stateObj);
										}
									}
									else if (!internalMetaType.IsVarTime && internalMetaType.SqlDbType != SqlDbType.Date)
									{
										num5 = ((num2 > num3) ? num2 : num3);
										if (num5 == 0 && this.IsYukonOrNewer)
										{
											if (internalMetaType.IsNCharType)
											{
												num5 = 2;
											}
											else
											{
												num5 = 1;
											}
										}
										this.WriteParameterVarLen(internalMetaType, num5, false, stateObj);
									}
									if (internalMetaType.SqlDbType == SqlDbType.Decimal)
									{
										byte actualPrecision = sqlParameter.GetActualPrecision();
										byte actualScale = sqlParameter.GetActualScale();
										if (!flag2)
										{
											if (flag3)
											{
												obj = TdsParser.AdjustSqlDecimalScale((SqlDecimal)obj, (int)actualScale);
												if (actualPrecision != 0 && actualPrecision < ((SqlDecimal)obj).Precision)
												{
													throw ADP.ParameterValueOutOfRange((SqlDecimal)obj);
												}
											}
											else
											{
												obj = TdsParser.AdjustDecimalScale((decimal)obj, (int)actualScale);
												SqlDecimal sqlDecimal = new SqlDecimal((decimal)obj);
												if (actualPrecision != 0 && actualPrecision < sqlDecimal.Precision)
												{
													throw ADP.ParameterValueOutOfRange((decimal)obj);
												}
											}
										}
										if (actualPrecision == 0)
										{
											if (this._isShiloh)
											{
												this.WriteByte(29, stateObj);
											}
											else
											{
												this.WriteByte(28, stateObj);
											}
										}
										else
										{
											this.WriteByte(actualPrecision, stateObj);
										}
										this.WriteByte(actualScale, stateObj);
									}
									else if (internalMetaType.IsVarTime)
									{
										this.WriteByte(sqlParameter.GetActualScale(), stateObj);
									}
									if (this._isYukon && internalMetaType.SqlDbType == SqlDbType.Xml)
									{
										if ((sqlParameter.XmlSchemaCollectionDatabase != null && sqlParameter.XmlSchemaCollectionDatabase != ADP.StrEmpty) || (sqlParameter.XmlSchemaCollectionOwningSchema != null && sqlParameter.XmlSchemaCollectionOwningSchema != ADP.StrEmpty) || (sqlParameter.XmlSchemaCollectionName != null && sqlParameter.XmlSchemaCollectionName != ADP.StrEmpty))
										{
											this.WriteByte(1, stateObj);
											if (sqlParameter.XmlSchemaCollectionDatabase != null && sqlParameter.XmlSchemaCollectionDatabase != ADP.StrEmpty)
											{
												int num = sqlParameter.XmlSchemaCollectionDatabase.Length;
												this.WriteByte((byte)num, stateObj);
												this.WriteString(sqlParameter.XmlSchemaCollectionDatabase, num, 0, stateObj);
											}
											else
											{
												this.WriteByte(0, stateObj);
											}
											if (sqlParameter.XmlSchemaCollectionOwningSchema != null && sqlParameter.XmlSchemaCollectionOwningSchema != ADP.StrEmpty)
											{
												int num = sqlParameter.XmlSchemaCollectionOwningSchema.Length;
												this.WriteByte((byte)num, stateObj);
												this.WriteString(sqlParameter.XmlSchemaCollectionOwningSchema, num, 0, stateObj);
											}
											else
											{
												this.WriteByte(0, stateObj);
											}
											if (sqlParameter.XmlSchemaCollectionName != null && sqlParameter.XmlSchemaCollectionName != ADP.StrEmpty)
											{
												int num = sqlParameter.XmlSchemaCollectionName.Length;
												this.WriteShort((int)((short)num), stateObj);
												this.WriteString(sqlParameter.XmlSchemaCollectionName, num, 0, stateObj);
											}
											else
											{
												this.WriteShort(0, stateObj);
											}
										}
										else
										{
											this.WriteByte(0, stateObj);
										}
									}
									else if (this._isShiloh && internalMetaType.IsCharType)
									{
										SqlCollation sqlCollation = ((sqlParameter.Collation != null) ? sqlParameter.Collation : this._defaultCollation);
										this.WriteUnsignedInt(sqlCollation.info, stateObj);
										this.WriteByte(sqlCollation.sortId, stateObj);
									}
									if (num4 == 0)
									{
										this.WriteParameterVarLen(internalMetaType, num3, flag2, stateObj);
									}
									else
									{
										this.WriteParameterVarLen(internalMetaType, num4, flag2, stateObj);
									}
									if (!flag2)
									{
										if (flag3)
										{
											this.WriteSqlValue(obj, internalMetaType, num3, num4, sqlParameter.Offset, stateObj);
										}
										else
										{
											this.WriteValue(obj, internalMetaType, sqlParameter.GetActualScale(), num3, num4, sqlParameter.Offset, stateObj);
										}
									}
								}
							}
							IL_07D3:;
						}
						if (i < rpcArray.Length - 1)
						{
							if (this._isYukon)
							{
								this.WriteByte(byte.MaxValue, stateObj);
							}
							else
							{
								this.WriteByte(128, stateObj);
							}
						}
					}
					stateObj.ExecuteFlush();
					stateObj.SniContext = SniContext.Snix_Read;
				}
				catch (Exception ex)
				{
					if (!ADP.IsCatchableExceptionType(ex))
					{
						throw;
					}
					this.FailureCleanup(stateObj, ex);
					throw;
				}
				finally
				{
					if (flag)
					{
						Monitor.Exit(this._physicalStateObj);
					}
				}
			}
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x0029A724 File Offset: 0x00299B24
		private void WriteParameterName(string parameterName, TdsParserStateObject stateObj)
		{
			if (!ADP.IsEmpty(parameterName))
			{
				int num = parameterName.Length & 255;
				this.WriteByte((byte)num, stateObj);
				this.WriteString(parameterName, num, 0, stateObj);
				return;
			}
			this.WriteByte(0, stateObj);
		}

		// Token: 0x06002A48 RID: 10824 RVA: 0x0029A764 File Offset: 0x00299B64
		private void WriteSmiParameter(SqlParameter param, int paramIndex, bool sendDefault, TdsParserStateObject stateObj)
		{
			ParameterPeekAheadValue parameterPeekAheadValue;
			SmiParameterMetaData smiParameterMetaData = param.MetaDataForSmi(out parameterPeekAheadValue);
			if (!this._isKatmai)
			{
				MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(smiParameterMetaData.SqlDbType, smiParameterMetaData.IsMultiValued);
				throw ADP.VersionDoesNotSupportDataType(metaTypeFromSqlDbType.TypeName);
			}
			object obj;
			ExtendedClrTypeCode extendedClrTypeCode;
			if (sendDefault)
			{
				if (SqlDbType.Structured == smiParameterMetaData.SqlDbType && smiParameterMetaData.IsMultiValued)
				{
					obj = TdsParser.__tvpEmptyValue;
					extendedClrTypeCode = ExtendedClrTypeCode.IEnumerableOfSqlDataRecord;
				}
				else
				{
					obj = null;
					extendedClrTypeCode = ExtendedClrTypeCode.DBNull;
				}
			}
			else if (param.Direction == ParameterDirection.Output)
			{
				bool paramaterIsSqlType = param.ParamaterIsSqlType;
				param.Value = null;
				obj = null;
				extendedClrTypeCode = ExtendedClrTypeCode.DBNull;
				param.ParamaterIsSqlType = paramaterIsSqlType;
			}
			else
			{
				obj = param.GetCoercedValue();
				extendedClrTypeCode = MetaDataUtilsSmi.DetermineExtendedTypeCodeForUseWithSqlDbType(smiParameterMetaData.SqlDbType, smiParameterMetaData.IsMultiValued, obj, null, 210UL);
			}
			if (Bid.AdvancedOn)
			{
				Bid.Trace("<sc.TdsParser.WriteSmiParameter|ADV> %d#, Sending parameter '%ls', default flag=%d, metadata:\n", this.ObjectID, paramIndex, sendDefault ? 1 : 0);
				Bid.PutStr(smiParameterMetaData.TraceString(3));
				Bid.Trace("\n");
			}
			this.WriteSmiParameterMetaData(smiParameterMetaData, sendDefault, stateObj);
			TdsParameterSetter tdsParameterSetter = new TdsParameterSetter(stateObj, smiParameterMetaData);
			ValueUtilsSmi.SetCompatibleValueV200(new SmiEventSink_Default(), tdsParameterSetter, 0, smiParameterMetaData, obj, extendedClrTypeCode, param.Offset, (0 < param.Size) ? param.Size : (-1), parameterPeekAheadValue);
		}

		// Token: 0x06002A49 RID: 10825 RVA: 0x0029A880 File Offset: 0x00299C80
		private void WriteSmiParameterMetaData(SmiParameterMetaData metaData, bool sendDefault, TdsParserStateObject stateObj)
		{
			byte b = 0;
			if (ParameterDirection.Output == metaData.Direction || ParameterDirection.InputOutput == metaData.Direction)
			{
				b |= 1;
			}
			if (sendDefault)
			{
				b |= 2;
			}
			this.WriteParameterName(metaData.Name, stateObj);
			this.WriteByte(b, stateObj);
			this.WriteSmiTypeInfo(metaData, stateObj);
		}

		// Token: 0x06002A4A RID: 10826 RVA: 0x0029A8CC File Offset: 0x00299CCC
		private void WriteSmiTypeInfo(SmiExtendedMetaData metaData, TdsParserStateObject stateObj)
		{
			checked
			{
				switch (metaData.SqlDbType)
				{
				case SqlDbType.BigInt:
					this.WriteByte(38, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.Binary:
					this.WriteByte(173, stateObj);
					this.WriteUnsignedShort((ushort)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.Bit:
					this.WriteByte(104, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.Char:
					this.WriteByte(175, stateObj);
					this.WriteUnsignedShort((ushort)metaData.MaxLength, stateObj);
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					return;
				case SqlDbType.DateTime:
					this.WriteByte(111, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.Decimal:
					this.WriteByte(108, stateObj);
					this.WriteByte((byte)MetaType.MetaDecimal.FixedLength, stateObj);
					this.WriteByte((metaData.Precision == 0) ? 1 : metaData.Precision, stateObj);
					this.WriteByte(metaData.Scale, stateObj);
					return;
				case SqlDbType.Float:
					this.WriteByte(109, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.Image:
					this.WriteByte(165, stateObj);
					this.WriteUnsignedShort(ushort.MaxValue, stateObj);
					return;
				case SqlDbType.Int:
					this.WriteByte(38, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.Money:
					this.WriteByte(110, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.NChar:
					this.WriteByte(239, stateObj);
					this.WriteUnsignedShort((ushort)(metaData.MaxLength * 2L), stateObj);
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					return;
				case SqlDbType.NText:
					this.WriteByte(231, stateObj);
					this.WriteUnsignedShort(ushort.MaxValue, stateObj);
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					return;
				case SqlDbType.NVarChar:
					this.WriteByte(231, stateObj);
					if (-1L == metaData.MaxLength)
					{
						this.WriteUnsignedShort(ushort.MaxValue, stateObj);
					}
					else
					{
						this.WriteUnsignedShort((ushort)(metaData.MaxLength * 2L), stateObj);
					}
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					return;
				case SqlDbType.Real:
					this.WriteByte(109, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.UniqueIdentifier:
					this.WriteByte(36, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.SmallDateTime:
					this.WriteByte(111, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.SmallInt:
					this.WriteByte(38, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.SmallMoney:
					this.WriteByte(110, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.Text:
					this.WriteByte(167, stateObj);
					this.WriteUnsignedShort(ushort.MaxValue, stateObj);
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					return;
				case SqlDbType.Timestamp:
					this.WriteByte(173, stateObj);
					this.WriteShort((int)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.TinyInt:
					this.WriteByte(38, stateObj);
					this.WriteByte((byte)metaData.MaxLength, stateObj);
					return;
				case SqlDbType.VarBinary:
					this.WriteByte(165, stateObj);
					this.WriteUnsignedShort(unchecked((ushort)metaData.MaxLength), stateObj);
					return;
				case SqlDbType.VarChar:
					this.WriteByte(167, stateObj);
					this.WriteUnsignedShort(unchecked((ushort)metaData.MaxLength), stateObj);
					this.WriteUnsignedInt(this._defaultCollation.info, stateObj);
					this.WriteByte(this._defaultCollation.sortId, stateObj);
					return;
				case SqlDbType.Variant:
					this.WriteByte(98, stateObj);
					this.WriteInt((int)metaData.MaxLength, stateObj);
					return;
				case (SqlDbType)24:
				case (SqlDbType)26:
				case (SqlDbType)27:
				case (SqlDbType)28:
					break;
				case SqlDbType.Xml:
					this.WriteByte(241, stateObj);
					if (ADP.IsEmpty(metaData.TypeSpecificNamePart1) && ADP.IsEmpty(metaData.TypeSpecificNamePart2) && ADP.IsEmpty(metaData.TypeSpecificNamePart3))
					{
						this.WriteByte(0, stateObj);
						return;
					}
					this.WriteByte(1, stateObj);
					this.WriteIdentifier(metaData.TypeSpecificNamePart1, stateObj);
					this.WriteIdentifier(metaData.TypeSpecificNamePart2, stateObj);
					this.WriteIdentifierWithShortLength(metaData.TypeSpecificNamePart3, stateObj);
					return;
				case SqlDbType.Udt:
					this.WriteByte(240, stateObj);
					this.WriteIdentifier(metaData.TypeSpecificNamePart1, stateObj);
					this.WriteIdentifier(metaData.TypeSpecificNamePart2, stateObj);
					this.WriteIdentifier(metaData.TypeSpecificNamePart3, stateObj);
					return;
				case SqlDbType.Structured:
					if (metaData.IsMultiValued)
					{
						this.WriteTvpTypeInfo(metaData, stateObj);
						return;
					}
					break;
				case SqlDbType.Date:
					this.WriteByte(40, stateObj);
					return;
				case SqlDbType.Time:
					this.WriteByte(41, stateObj);
					this.WriteByte(metaData.Scale, stateObj);
					return;
				case SqlDbType.DateTime2:
					this.WriteByte(42, stateObj);
					this.WriteByte(metaData.Scale, stateObj);
					return;
				case SqlDbType.DateTimeOffset:
					this.WriteByte(43, stateObj);
					this.WriteByte(metaData.Scale, stateObj);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x0029ADEC File Offset: 0x0029A1EC
		private void WriteTvpTypeInfo(SmiExtendedMetaData metaData, TdsParserStateObject stateObj)
		{
			this.WriteByte(243, stateObj);
			this.WriteIdentifier(metaData.TypeSpecificNamePart1, stateObj);
			this.WriteIdentifier(metaData.TypeSpecificNamePart2, stateObj);
			this.WriteIdentifier(metaData.TypeSpecificNamePart3, stateObj);
			if (metaData.FieldMetaData.Count == 0)
			{
				this.WriteUnsignedShort(ushort.MaxValue, stateObj);
			}
			else
			{
				this.WriteUnsignedShort(checked((ushort)metaData.FieldMetaData.Count), stateObj);
				SmiDefaultFieldsProperty smiDefaultFieldsProperty = (SmiDefaultFieldsProperty)metaData.ExtendedProperties[SmiPropertySelector.DefaultFields];
				for (int i = 0; i < metaData.FieldMetaData.Count; i++)
				{
					this.WriteTvpColumnMetaData(metaData.FieldMetaData[i], smiDefaultFieldsProperty[i], stateObj);
				}
				this.WriteTvpOrderUnique(metaData, stateObj);
			}
			this.WriteByte(0, stateObj);
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x0029AEAC File Offset: 0x0029A2AC
		private void WriteTvpColumnMetaData(SmiExtendedMetaData md, bool isDefault, TdsParserStateObject stateObj)
		{
			if (SqlDbType.Timestamp == md.SqlDbType)
			{
				this.WriteUnsignedInt(80U, stateObj);
			}
			else
			{
				this.WriteUnsignedInt(0U, stateObj);
			}
			ushort num = 1;
			if (isDefault)
			{
				num |= 512;
			}
			this.WriteUnsignedShort(num, stateObj);
			this.WriteSmiTypeInfo(md, stateObj);
			this.WriteIdentifier(null, stateObj);
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x0029AEFC File Offset: 0x0029A2FC
		private void WriteTvpOrderUnique(SmiExtendedMetaData metaData, TdsParserStateObject stateObj)
		{
			SmiOrderProperty smiOrderProperty = (SmiOrderProperty)metaData.ExtendedProperties[SmiPropertySelector.SortOrder];
			SmiUniqueKeyProperty smiUniqueKeyProperty = (SmiUniqueKeyProperty)metaData.ExtendedProperties[SmiPropertySelector.UniqueKey];
			List<TdsParser.TdsOrderUnique> list = new List<TdsParser.TdsOrderUnique>(metaData.FieldMetaData.Count);
			for (int i = 0; i < metaData.FieldMetaData.Count; i++)
			{
				byte b = 0;
				SmiOrderProperty.SmiColumnOrder smiColumnOrder = smiOrderProperty[i];
				if (smiColumnOrder.Order == SortOrder.Ascending)
				{
					b = 1;
				}
				else if (SortOrder.Descending == smiColumnOrder.Order)
				{
					b = 2;
				}
				if (smiUniqueKeyProperty[i])
				{
					b |= 4;
				}
				if (b != 0)
				{
					list.Add(new TdsParser.TdsOrderUnique(checked((short)(i + 1)), b));
				}
			}
			if (0 < list.Count)
			{
				this.WriteByte(16, stateObj);
				this.WriteShort(list.Count, stateObj);
				foreach (TdsParser.TdsOrderUnique tdsOrderUnique in list)
				{
					this.WriteShort((int)tdsOrderUnique.ColumnOrdinal, stateObj);
					this.WriteByte(tdsOrderUnique.Flags, stateObj);
				}
			}
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x0029B020 File Offset: 0x0029A420
		internal void WriteBulkCopyDone(TdsParserStateObject stateObj)
		{
			this.WriteByte(253, stateObj);
			this.WriteShort(0, stateObj);
			this.WriteShort(0, stateObj);
			this.WriteInt(0, stateObj);
			stateObj.WritePacket(1);
			stateObj._pendingData = true;
		}

		// Token: 0x06002A4F RID: 10831 RVA: 0x0029B060 File Offset: 0x0029A460
		internal void WriteBulkCopyMetaData(_SqlMetaDataSet metadataCollection, int count, TdsParserStateObject stateObj)
		{
			this.WriteByte(129, stateObj);
			this.WriteShort(count, stateObj);
			for (int i = 0; i < metadataCollection.Length; i++)
			{
				if (metadataCollection[i] != null)
				{
					_SqlMetaData sqlMetaData = metadataCollection[i];
					if (this.IsYukonOrNewer)
					{
						this.WriteInt(0, stateObj);
					}
					else
					{
						this.WriteShort(0, stateObj);
					}
					ushort num = (ushort)(sqlMetaData.updatability << 2);
					num |= (sqlMetaData.isNullable ? 1 : 0);
					num |= (sqlMetaData.isIdentity ? 16 : 0);
					this.WriteShort((int)num, stateObj);
					SqlDbType type = sqlMetaData.type;
					if (type != SqlDbType.Decimal)
					{
						switch (type)
						{
						case SqlDbType.Xml:
							this.WriteByteArray(TdsParser.s_xmlMetadataSubstituteSequence, TdsParser.s_xmlMetadataSubstituteSequence.Length, 0, stateObj);
							goto IL_01BF;
						case SqlDbType.Udt:
							this.WriteByte(165, stateObj);
							this.WriteTokenLength(165, sqlMetaData.length, stateObj);
							goto IL_01BF;
						case SqlDbType.Date:
							this.WriteByte(sqlMetaData.tdsType, stateObj);
							goto IL_01BF;
						case SqlDbType.Time:
						case SqlDbType.DateTime2:
						case SqlDbType.DateTimeOffset:
							this.WriteByte(sqlMetaData.tdsType, stateObj);
							this.WriteByte(sqlMetaData.scale, stateObj);
							goto IL_01BF;
						}
						this.WriteByte(sqlMetaData.tdsType, stateObj);
						this.WriteTokenLength(sqlMetaData.tdsType, sqlMetaData.length, stateObj);
						if (sqlMetaData.metaType.IsCharType && this._isShiloh)
						{
							this.WriteUnsignedInt(sqlMetaData.collation.info, stateObj);
							this.WriteByte(sqlMetaData.collation.sortId, stateObj);
						}
					}
					else
					{
						this.WriteByte(sqlMetaData.tdsType, stateObj);
						this.WriteTokenLength(sqlMetaData.tdsType, sqlMetaData.length, stateObj);
						this.WriteByte(sqlMetaData.precision, stateObj);
						this.WriteByte(sqlMetaData.scale, stateObj);
					}
					IL_01BF:
					if (sqlMetaData.metaType.IsLong && !sqlMetaData.metaType.IsPlp)
					{
						this.WriteShort(sqlMetaData.tableName.Length, stateObj);
						this.WriteString(sqlMetaData.tableName, stateObj);
					}
					this.WriteByte((byte)sqlMetaData.column.Length, stateObj);
					this.WriteString(sqlMetaData.column, stateObj);
				}
			}
		}

		// Token: 0x06002A50 RID: 10832 RVA: 0x0029B298 File Offset: 0x0029A698
		internal void WriteBulkCopyValue(object value, SqlMetaDataPriv metadata, TdsParserStateObject stateObj)
		{
			Encoding defaultEncoding = this._defaultEncoding;
			SqlCollation defaultCollation = this._defaultCollation;
			int defaultCodePage = this._defaultCodePage;
			int defaultLCID = this._defaultLCID;
			try
			{
				if (metadata.encoding != null)
				{
					this._defaultEncoding = metadata.encoding;
				}
				if (metadata.collation != null)
				{
					this._defaultCollation = metadata.collation;
					this._defaultLCID = this._defaultCollation.LCID;
				}
				this._defaultCodePage = metadata.codePage;
				MetaType metaType = metadata.metaType;
				ulong num = (ulong)((long)metadata.length);
				ulong num2 = 0UL;
				if (ADP.IsNull(value))
				{
					if (metaType.IsPlp && (metaType.NullableType != 240 || metaType.IsLong))
					{
						this.WriteLong(-1L, stateObj);
					}
					else if (!metaType.IsFixed && !metaType.IsLong && !metaType.IsVarTime)
					{
						this.WriteShort(65535, stateObj);
					}
					else
					{
						this.WriteByte(0, stateObj);
					}
				}
				else
				{
					byte nullableType = metaType.NullableType;
					ulong num3;
					if (nullableType <= 167)
					{
						switch (nullableType)
						{
						case 34:
							break;
						case 35:
							goto IL_0198;
						case 36:
							num3 = 16UL;
							goto IL_0312;
						default:
							if (nullableType == 99)
							{
								goto IL_029A;
							}
							switch (nullableType)
							{
							case 165:
								break;
							case 166:
								goto IL_030F;
							case 167:
								goto IL_0198;
							default:
								goto IL_030F;
							}
							break;
						}
					}
					else
					{
						switch (nullableType)
						{
						case 173:
							break;
						case 174:
							goto IL_030F;
						case 175:
							goto IL_0198;
						default:
							if (nullableType == 231)
							{
								goto IL_029A;
							}
							switch (nullableType)
							{
							case 239:
								goto IL_029A;
							case 240:
								break;
							case 241:
								if (value is XmlReader)
								{
									value = MetaType.GetStringFromXml((XmlReader)value);
								}
								num3 = (ulong)((long)((value is string) ? ((string)value).Length : ((SqlString)value).Value.Length) * 2L);
								goto IL_0312;
							default:
								goto IL_030F;
							}
							break;
						}
					}
					num3 = (ulong)((long)((value is byte[]) ? ((byte[])value).Length : ((SqlBinary)value).Length));
					goto IL_0312;
					IL_0198:
					if (this._defaultEncoding == null)
					{
						this.ThrowUnsupportedCollationEncountered(null);
					}
					if (value is string)
					{
						num3 = (ulong)((long)((string)value).Length);
						num2 = (ulong)((long)this._defaultEncoding.GetByteCount((string)value));
					}
					else
					{
						num3 = (ulong)((long)((SqlString)value).Value.Length);
						num2 = (ulong)((long)this._defaultEncoding.GetByteCount(((SqlString)value).Value));
					}
					if (num2 <= num)
					{
						goto IL_0312;
					}
					if (defaultEncoding == null)
					{
						this.ThrowUnsupportedCollationEncountered(null);
					}
					this._defaultEncoding = defaultEncoding;
					this._defaultCollation = defaultCollation;
					this._defaultCodePage = defaultCodePage;
					this._defaultLCID = defaultLCID;
					if (value is string)
					{
						num3 = (ulong)((long)((string)value).Length);
						num2 = (ulong)((long)this._defaultEncoding.GetByteCount((string)value));
						goto IL_0312;
					}
					num3 = (ulong)((long)((SqlString)value).Value.Length);
					num2 = (ulong)((long)this._defaultEncoding.GetByteCount(((SqlString)value).Value));
					goto IL_0312;
					IL_029A:
					num3 = (ulong)((long)((value is string) ? ((string)value).Length : ((SqlString)value).Value.Length) * 2L);
					goto IL_0312;
					IL_030F:
					num3 = num;
					IL_0312:
					if (metaType.IsLong)
					{
						SqlDbType sqlDbType = metaType.SqlDbType;
						if (sqlDbType <= SqlDbType.NVarChar)
						{
							if (sqlDbType != SqlDbType.Image)
							{
								switch (sqlDbType)
								{
								case SqlDbType.NText:
									break;
								case SqlDbType.NVarChar:
									goto IL_03A1;
								default:
									goto IL_03C5;
								}
							}
						}
						else
						{
							switch (sqlDbType)
							{
							case SqlDbType.Text:
								break;
							case SqlDbType.Timestamp:
							case SqlDbType.TinyInt:
								goto IL_03C5;
							case SqlDbType.VarBinary:
							case SqlDbType.VarChar:
								goto IL_03A1;
							default:
								if (sqlDbType != SqlDbType.Xml && sqlDbType != SqlDbType.Udt)
								{
									goto IL_03C5;
								}
								goto IL_03A1;
							}
						}
						this.WriteByteArray(TdsParser.s_longDataHeader, TdsParser.s_longDataHeader.Length, 0, stateObj);
						this.WriteTokenLength(metadata.tdsType, (num2 == 0UL) ? ((int)num3) : ((int)num2), stateObj);
						goto IL_03C5;
						IL_03A1:
						this.WriteUnsignedLong(18446744073709551614UL, stateObj);
					}
					else
					{
						this.WriteTokenLength(metadata.tdsType, (num2 == 0UL) ? ((int)num3) : ((int)num2), stateObj);
					}
					IL_03C5:
					if (DataStorage.IsSqlType(value.GetType()))
					{
						this.WriteSqlValue(value, metaType, (int)num3, (int)num2, 0, stateObj);
					}
					else if (metaType.SqlDbType != SqlDbType.Udt || metaType.IsLong)
					{
						this.WriteValue(value, metaType, metadata.scale, (int)num3, (int)num2, 0, stateObj);
					}
					else
					{
						this.WriteShort((int)num3, stateObj);
						this.WriteByteArray((byte[])value, (int)num3, 0, stateObj);
					}
				}
			}
			finally
			{
				this._defaultEncoding = defaultEncoding;
				this._defaultCollation = defaultCollation;
				this._defaultCodePage = defaultCodePage;
				this._defaultLCID = defaultLCID;
			}
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x0029B708 File Offset: 0x0029AB08
		private void WriteMarsHeader(TdsParserStateObject stateObj, SqlInternalTransaction transaction)
		{
			this.WriteUnsignedInt(22U, stateObj);
			this.WriteUnsignedInt(18U, stateObj);
			this.WriteShort(2, stateObj);
			if (transaction != null && 0L != transaction.TransactionId)
			{
				this.WriteLong(transaction.TransactionId, stateObj);
				this.WriteInt(stateObj.IncrementAndObtainOpenResultCount(transaction), stateObj);
				return;
			}
			this.WriteLong(this._retainedTransactionId, stateObj);
			this.WriteInt(stateObj.IncrementAndObtainOpenResultCount(null), stateObj);
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x0029B774 File Offset: 0x0029AB74
		private void WriteQueryNotificationHeader(SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj)
		{
			if (notificationRequest != null)
			{
				string userData = notificationRequest.UserData;
				string options = notificationRequest.Options;
				int timeout = notificationRequest.Timeout;
				if (userData == null)
				{
					throw ADP.ArgumentNull("CallbackId");
				}
				if (65535 < userData.Length)
				{
					throw ADP.ArgumentOutOfRange("CallbackId");
				}
				if (options == null)
				{
					throw ADP.ArgumentNull("Service");
				}
				if (65535 < options.Length)
				{
					throw ADP.ArgumentOutOfRange("Service");
				}
				if (-1 > timeout)
				{
					throw ADP.ArgumentOutOfRange("Timeout");
				}
				Bid.NotificationsTrace("<sc.TdsParser.WriteQueryNotificationHeader|DEP> NotificationRequest: userData: '%ls', options: '%ls', timeout: '%d'\n", notificationRequest.UserData, notificationRequest.Options, notificationRequest.Timeout);
				int num = 8 + userData.Length * 2 + 2 + options.Length * 2;
				if (timeout > 0)
				{
					num += 4;
				}
				int num2 = num + stateObj._outBytesUsed - 8;
				int outBytesUsed = stateObj._outBytesUsed;
				stateObj._outBytesUsed = 8;
				this.WriteInt(num2, stateObj);
				stateObj._outBytesUsed = outBytesUsed;
				this.WriteInt(num, stateObj);
				this.WriteShort(1, stateObj);
				this.WriteShort(userData.Length * 2, stateObj);
				this.WriteString(userData, stateObj);
				this.WriteShort(options.Length * 2, stateObj);
				this.WriteString(options, stateObj);
				if (timeout > 0)
				{
					this.WriteInt(timeout, stateObj);
				}
			}
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x0029B8A8 File Offset: 0x0029ACA8
		private void WriteTokenLength(byte token, int length, TdsParserStateObject stateObj)
		{
			int num = 0;
			if (this._isYukon)
			{
				if (240 == token)
				{
					num = 8;
				}
				else if (token == 241)
				{
					num = 8;
				}
			}
			if (num == 0)
			{
				int num2 = (int)(token & 48);
				if (num2 <= 16)
				{
					if (num2 != 0)
					{
						if (num2 != 16)
						{
							goto IL_0066;
						}
						num = 0;
						goto IL_0066;
					}
				}
				else if (num2 != 32)
				{
					if (num2 == 48)
					{
						num = 0;
						goto IL_0066;
					}
					goto IL_0066;
				}
				if ((token & 128) != 0)
				{
					num = 2;
				}
				else if ((token & 12) == 0)
				{
					num = 4;
				}
				else
				{
					num = 1;
				}
				IL_0066:
				int num3 = num;
				switch (num3)
				{
				case 1:
					this.WriteByte((byte)length, stateObj);
					return;
				case 2:
					this.WriteShort(length, stateObj);
					return;
				case 3:
					break;
				case 4:
					this.WriteInt(length, stateObj);
					return;
				default:
					if (num3 != 8)
					{
						return;
					}
					this.WriteShort(65535, stateObj);
					break;
				}
			}
		}

		// Token: 0x06002A54 RID: 10836 RVA: 0x0029B964 File Offset: 0x0029AD64
		private bool IsBOMNeeded(MetaType type, object value)
		{
			if (type.NullableType == 241)
			{
				Type type2 = value.GetType();
				if (type2 == typeof(SqlString))
				{
					if (!((SqlString)value).IsNull && ((SqlString)value).Value.Length > 0 && (((SqlString)value).Value[0] & 'ÿ') != 'ÿ')
					{
						return true;
					}
				}
				else if (type2 == typeof(string) && ((string)value).Length > 0)
				{
					if (value != null && (((string)value)[0] & 'ÿ') != 'ÿ')
					{
						return true;
					}
				}
				else if (type2 == typeof(SqlXml))
				{
					if (!((SqlXml)value).IsNull)
					{
						return true;
					}
				}
				else if (type2 == typeof(XmlReader))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002A55 RID: 10837 RVA: 0x0029BA4C File Offset: 0x0029AE4C
		private void WriteSqlValue(object value, MetaType type, int actualLength, int codePageByteSize, int offset, TdsParserStateObject stateObj)
		{
			byte nullableType = type.NullableType;
			if (nullableType <= 111)
			{
				switch (nullableType)
				{
				case 34:
					break;
				case 35:
					goto IL_0230;
				case 36:
				{
					byte[] array = ((SqlGuid)value).ToByteArray();
					this.WriteByteArray(array, actualLength, 0, stateObj);
					goto IL_03D1;
				}
				case 37:
					goto IL_03D1;
				case 38:
					if (type.FixedLength == 1)
					{
						this.WriteByte(((SqlByte)value).Value, stateObj);
						goto IL_03D1;
					}
					if (type.FixedLength == 2)
					{
						this.WriteShort((int)((SqlInt16)value).Value, stateObj);
						goto IL_03D1;
					}
					if (type.FixedLength == 4)
					{
						this.WriteInt(((SqlInt32)value).Value, stateObj);
						goto IL_03D1;
					}
					this.WriteLong(((SqlInt64)value).Value, stateObj);
					goto IL_03D1;
				default:
					if (nullableType == 99)
					{
						goto IL_0297;
					}
					switch (nullableType)
					{
					case 104:
						if (((SqlBoolean)value).Value)
						{
							this.WriteByte(1, stateObj);
							goto IL_03D1;
						}
						this.WriteByte(0, stateObj);
						goto IL_03D1;
					case 105:
					case 106:
					case 107:
						goto IL_03D1;
					case 108:
						this.WriteSqlDecimal((SqlDecimal)value, stateObj);
						goto IL_03D1;
					case 109:
						if (type.FixedLength == 4)
						{
							this.WriteFloat(((SqlSingle)value).Value, stateObj);
							goto IL_03D1;
						}
						this.WriteDouble(((SqlDouble)value).Value, stateObj);
						goto IL_03D1;
					case 110:
						this.WriteSqlMoney((SqlMoney)value, type.FixedLength, stateObj);
						goto IL_03D1;
					case 111:
					{
						SqlDateTime sqlDateTime = (SqlDateTime)value;
						if (type.FixedLength != 4)
						{
							this.WriteInt(sqlDateTime.DayTicks, stateObj);
							this.WriteInt(sqlDateTime.TimeTicks, stateObj);
							goto IL_03D1;
						}
						if (0 > sqlDateTime.DayTicks || sqlDateTime.DayTicks > 65535)
						{
							throw SQL.SmallDateTimeOverflow(sqlDateTime.ToString());
						}
						this.WriteShort(sqlDateTime.DayTicks, stateObj);
						this.WriteShort(sqlDateTime.TimeTicks / SqlDateTime.SQLTicksPerMinute, stateObj);
						goto IL_03D1;
					}
					default:
						goto IL_03D1;
					}
					break;
				}
			}
			else if (nullableType <= 175)
			{
				switch (nullableType)
				{
				case 165:
					break;
				case 166:
					goto IL_03D1;
				case 167:
					goto IL_0230;
				default:
					switch (nullableType)
					{
					case 173:
						break;
					case 174:
						goto IL_03D1;
					case 175:
						goto IL_0230;
					default:
						goto IL_03D1;
					}
					break;
				}
			}
			else
			{
				if (nullableType == 231)
				{
					goto IL_0297;
				}
				switch (nullableType)
				{
				case 239:
				case 241:
					goto IL_0297;
				case 240:
					throw SQL.UDTUnexpectedResult(value.GetType().AssemblyQualifiedName);
				default:
					goto IL_03D1;
				}
			}
			if (type.IsPlp)
			{
				this.WriteInt(actualLength, stateObj);
			}
			if (value is SqlBinary)
			{
				this.WriteByteArray(((SqlBinary)value).Value, actualLength, offset, stateObj);
				goto IL_03D1;
			}
			this.WriteByteArray(((SqlBytes)value).Value, actualLength, offset, stateObj);
			goto IL_03D1;
			IL_0230:
			if (type.IsPlp)
			{
				this.WriteInt(codePageByteSize, stateObj);
			}
			if (value is SqlChars)
			{
				string text = new string(((SqlChars)value).Value);
				this.WriteEncodingChar(text, actualLength, offset, this._defaultEncoding, stateObj);
				goto IL_03D1;
			}
			this.WriteEncodingChar(((SqlString)value).Value, actualLength, offset, this._defaultEncoding, stateObj);
			goto IL_03D1;
			IL_0297:
			if (type.IsPlp)
			{
				if (this.IsBOMNeeded(type, value))
				{
					this.WriteInt(actualLength + 2, stateObj);
					this.WriteShort(65279, stateObj);
				}
				else
				{
					this.WriteInt(actualLength, stateObj);
				}
			}
			if (actualLength != 0)
			{
				actualLength >>= 1;
			}
			if (value is SqlChars)
			{
				this.WriteCharArray(((SqlChars)value).Value, actualLength, offset, stateObj);
			}
			else
			{
				this.WriteString(((SqlString)value).Value, actualLength, offset, stateObj);
			}
			IL_03D1:
			if (type.IsPlp && actualLength > 0)
			{
				this.WriteInt(0, stateObj);
			}
		}

		// Token: 0x06002A56 RID: 10838 RVA: 0x0029BE40 File Offset: 0x0029B240
		private void WriteValue(object value, MetaType type, byte scale, int actualLength, int encodingByteSize, int offset, TdsParserStateObject stateObj)
		{
			byte nullableType = type.NullableType;
			if (nullableType <= 111)
			{
				switch (nullableType)
				{
				case 34:
					break;
				case 35:
					goto IL_01DE;
				case 36:
				{
					byte[] array = ((Guid)value).ToByteArray();
					this.WriteByteArray(array, actualLength, 0, stateObj);
					goto IL_03C2;
				}
				case 37:
				case 39:
					goto IL_03C2;
				case 38:
					if (type.FixedLength == 1)
					{
						this.WriteByte((byte)value, stateObj);
						goto IL_03C2;
					}
					if (type.FixedLength == 2)
					{
						this.WriteShort((int)((short)value), stateObj);
						goto IL_03C2;
					}
					if (type.FixedLength == 4)
					{
						this.WriteInt((int)value, stateObj);
						goto IL_03C2;
					}
					this.WriteLong((long)value, stateObj);
					goto IL_03C2;
				case 40:
					this.WriteDate((DateTime)value, stateObj);
					goto IL_03C2;
				case 41:
					if (scale > 7)
					{
						throw SQL.TimeScaleValueOutOfRange(scale);
					}
					this.WriteTime((TimeSpan)value, scale, actualLength, stateObj);
					goto IL_03C2;
				case 42:
					if (scale > 7)
					{
						throw SQL.TimeScaleValueOutOfRange(scale);
					}
					this.WriteDateTime2((DateTime)value, scale, actualLength, stateObj);
					goto IL_03C2;
				case 43:
					this.WriteDateTimeOffset((DateTimeOffset)value, scale, actualLength, stateObj);
					goto IL_03C2;
				default:
					if (nullableType == 99)
					{
						goto IL_022B;
					}
					switch (nullableType)
					{
					case 104:
						if ((bool)value)
						{
							this.WriteByte(1, stateObj);
							goto IL_03C2;
						}
						this.WriteByte(0, stateObj);
						goto IL_03C2;
					case 105:
					case 106:
					case 107:
						goto IL_03C2;
					case 108:
						this.WriteDecimal((decimal)value, stateObj);
						goto IL_03C2;
					case 109:
						if (type.FixedLength == 4)
						{
							this.WriteFloat((float)value, stateObj);
							goto IL_03C2;
						}
						this.WriteDouble((double)value, stateObj);
						goto IL_03C2;
					case 110:
						this.WriteCurrency((decimal)value, type.FixedLength, stateObj);
						goto IL_03C2;
					case 111:
					{
						TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, (byte)type.FixedLength);
						if (type.FixedLength != 4)
						{
							this.WriteInt(tdsDateTime.days, stateObj);
							this.WriteInt(tdsDateTime.time, stateObj);
							goto IL_03C2;
						}
						if (0 > tdsDateTime.days || tdsDateTime.days > 65535)
						{
							throw SQL.SmallDateTimeOverflow(MetaType.ToDateTime(tdsDateTime.days, tdsDateTime.time, 4).ToString(CultureInfo.InvariantCulture));
						}
						this.WriteShort(tdsDateTime.days, stateObj);
						this.WriteShort(tdsDateTime.time, stateObj);
						goto IL_03C2;
					}
					default:
						goto IL_03C2;
					}
					break;
				}
			}
			else if (nullableType <= 175)
			{
				switch (nullableType)
				{
				case 165:
					break;
				case 166:
					goto IL_03C2;
				case 167:
					goto IL_01DE;
				default:
					switch (nullableType)
					{
					case 173:
						break;
					case 174:
						goto IL_03C2;
					case 175:
						goto IL_01DE;
					default:
						goto IL_03C2;
					}
					break;
				}
			}
			else
			{
				if (nullableType == 231)
				{
					goto IL_022B;
				}
				switch (nullableType)
				{
				case 239:
				case 241:
					goto IL_022B;
				case 240:
					break;
				default:
					goto IL_03C2;
				}
			}
			byte[] array2 = (byte[])value;
			if (type.IsPlp)
			{
				this.WriteInt(actualLength, stateObj);
			}
			this.WriteByteArray(array2, actualLength, offset, stateObj);
			goto IL_03C2;
			IL_01DE:
			if (type.IsPlp)
			{
				this.WriteInt(encodingByteSize, stateObj);
			}
			if (value is byte[])
			{
				this.WriteByteArray((byte[])value, actualLength, 0, stateObj);
				goto IL_03C2;
			}
			this.WriteEncodingChar((string)value, actualLength, offset, this._defaultEncoding, stateObj);
			goto IL_03C2;
			IL_022B:
			if (type.IsPlp)
			{
				if (this.IsBOMNeeded(type, value))
				{
					this.WriteInt(actualLength + 2, stateObj);
					this.WriteShort(65279, stateObj);
				}
				else
				{
					this.WriteInt(actualLength, stateObj);
				}
			}
			if (value is byte[])
			{
				this.WriteByteArray((byte[])value, actualLength, 0, stateObj);
			}
			else
			{
				actualLength >>= 1;
				this.WriteString((string)value, actualLength, offset, stateObj);
			}
			IL_03C2:
			if (type.IsPlp && actualLength > 0)
			{
				this.WriteInt(0, stateObj);
			}
		}

		// Token: 0x06002A57 RID: 10839 RVA: 0x0029C228 File Offset: 0x0029B628
		internal void WriteParameterVarLen(MetaType type, int size, bool isNull, TdsParserStateObject stateObj)
		{
			if (type.IsLong)
			{
				if (isNull)
				{
					if (type.IsPlp)
					{
						this.WriteLong(-1L, stateObj);
						return;
					}
					this.WriteInt(-1, stateObj);
					return;
				}
				else
				{
					if (type.NullableType == 241)
					{
						this.WriteUnsignedLong(18446744073709551614UL, stateObj);
						return;
					}
					if (type.IsPlp)
					{
						this.WriteLong((long)size, stateObj);
						return;
					}
					this.WriteInt(size, stateObj);
					return;
				}
			}
			else if (type.IsVarTime)
			{
				if (isNull)
				{
					this.WriteByte(0, stateObj);
					return;
				}
				this.WriteByte((byte)size, stateObj);
				return;
			}
			else if (!type.IsFixed)
			{
				if (isNull)
				{
					this.WriteShort(65535, stateObj);
					return;
				}
				this.WriteShort(size, stateObj);
				return;
			}
			else
			{
				if (isNull)
				{
					this.WriteByte(0, stateObj);
					return;
				}
				this.WriteByte((byte)(type.FixedLength & 255), stateObj);
				return;
			}
		}

		// Token: 0x06002A58 RID: 10840 RVA: 0x0029C2F8 File Offset: 0x0029B6F8
		private int ReadPlpUnicodeCharsChunk(char[] buff, int offst, int len, TdsParserStateObject stateObj)
		{
			if (stateObj._longlenleft == 0UL)
			{
				return 0;
			}
			int num = len;
			if (stateObj._longlenleft >> 1 < (ulong)((long)len))
			{
				num = (int)(stateObj._longlenleft >> 1);
			}
			for (int i = 0; i < num; i++)
			{
				buff[offst + i] = stateObj.ReadChar();
			}
			stateObj._longlenleft -= (ulong)((ulong)((long)num) << 1);
			return num;
		}

		// Token: 0x06002A59 RID: 10841 RVA: 0x0029C358 File Offset: 0x0029B758
		internal int ReadPlpUnicodeChars(ref char[] buff, int offst, int len, TdsParserStateObject stateObj)
		{
			int num = 0;
			if (stateObj._longlen == 0UL)
			{
				return 0;
			}
			int i = len;
			if (buff == null && stateObj._longlen != 18446744073709551614UL)
			{
				buff = new char[Math.Min((int)stateObj._longlen, len)];
			}
			if (stateObj._longlenleft == 0UL)
			{
				stateObj.ReadPlpLength(false);
				if (stateObj._longlenleft == 0UL)
				{
					return 0;
				}
			}
			while (i > 0)
			{
				int num2 = (int)Math.Min(stateObj._longlenleft + 1UL >> 1, (ulong)((long)i));
				if (buff == null || buff.Length < offst + num2)
				{
					char[] array = new char[offst + num2];
					if (buff != null)
					{
						Buffer.BlockCopy(buff, 0, array, 0, offst * 2);
					}
					buff = array;
				}
				if (num2 > 0)
				{
					num2 = this.ReadPlpUnicodeCharsChunk(buff, offst, num2, stateObj);
					i -= num2;
					offst += num2;
					num += num2;
				}
				if (stateObj._longlenleft == 1UL && i > 0)
				{
					byte b = stateObj.ReadByte();
					stateObj._longlenleft -= 1UL;
					stateObj.ReadPlpLength(false);
					byte b2 = stateObj.ReadByte();
					stateObj._longlenleft -= 1UL;
					buff[offst] = (char)(((int)(b2 & byte.MaxValue) << 8) + (int)(b & byte.MaxValue));
					checked
					{
						offst++;
					}
					num2++;
					i--;
					num++;
				}
				if (stateObj._longlenleft == 0UL)
				{
					stateObj.ReadPlpLength(false);
				}
				if (stateObj._longlenleft == 0UL)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06002A5A RID: 10842 RVA: 0x0029C4C0 File Offset: 0x0029B8C0
		internal int ReadPlpAnsiChars(ref char[] buff, int offst, int len, SqlMetaDataPriv metadata, TdsParserStateObject stateObj)
		{
			int num = 0;
			if (stateObj._longlen == 0UL)
			{
				return 0;
			}
			int i = len;
			if (stateObj._longlenleft == 0UL)
			{
				stateObj.ReadPlpLength(false);
				if (stateObj._longlenleft == 0UL)
				{
					return 0;
				}
			}
			Encoding encoding = metadata.encoding;
			if (encoding == null)
			{
				if (this._defaultEncoding == null)
				{
					this.ThrowUnsupportedCollationEncountered(stateObj);
				}
				encoding = this._defaultEncoding;
			}
			while (i > 0)
			{
				int num2 = (int)Math.Min(stateObj._longlenleft, (ulong)((long)i));
				if (stateObj._bTmp == null || stateObj._bTmp.Length < num2)
				{
					stateObj._bTmp = new byte[num2];
				}
				num2 = stateObj.ReadPlpBytesChunk(stateObj._bTmp, 0, num2);
				int chars = encoding.GetChars(stateObj._bTmp, 0, num2, buff, offst);
				i -= chars;
				offst += chars;
				num += chars;
				if (stateObj._longlenleft == 0UL)
				{
					stateObj.ReadPlpLength(false);
				}
				if (stateObj._longlenleft == 0UL)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x0029C5C0 File Offset: 0x0029B9C0
		internal ulong SkipPlpValue(ulong cb, TdsParserStateObject stateObj)
		{
			ulong num = 0UL;
			if (stateObj._longlenleft == 0UL)
			{
				stateObj.ReadPlpLength(false);
			}
			while (num < cb && stateObj._longlenleft > 0UL)
			{
				int num2;
				if (stateObj._longlenleft > 2147483647UL)
				{
					num2 = int.MaxValue;
				}
				else
				{
					num2 = (int)stateObj._longlenleft;
				}
				num2 = ((cb - num < (ulong)((long)num2)) ? ((int)(cb - num)) : num2);
				this.SkipBytes(num2, stateObj);
				stateObj._longlenleft -= (ulong)((long)num2);
				num += (ulong)((long)num2);
				if (stateObj._longlenleft == 0UL)
				{
					stateObj.ReadPlpLength(false);
				}
			}
			return num;
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x0029C650 File Offset: 0x0029BA50
		internal ulong PlpBytesLeft(TdsParserStateObject stateObj)
		{
			if (stateObj._longlen != 0UL && stateObj._longlenleft == 0UL)
			{
				stateObj.ReadPlpLength(false);
			}
			return stateObj._longlenleft;
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x0029C680 File Offset: 0x0029BA80
		internal ulong PlpBytesTotalLength(TdsParserStateObject stateObj)
		{
			if (stateObj._longlen == 18446744073709551614UL)
			{
				return ulong.MaxValue;
			}
			if (stateObj._longlen == 18446744073709551615UL)
			{
				return 0UL;
			}
			return stateObj._longlen;
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x0029C6B0 File Offset: 0x0029BAB0
		internal string TraceString()
		{
			return string.Format(null, "\n\t         _physicalStateObj = {0}\n\t         _pMarsPhysicalConObj = {1}\n\t         _state = {2}\n\t         _server = {3}\n\t         _fResetConnection = {4}\n\t         _defaultCollation = {5}\n\t         _defaultCodePage = {6}\n\t         _defaultLCID = {7}\n\t         _defaultEncoding = {8}\n\t         _encryptionOption = {10}\n\t         _currentTransaction = {11}\n\t         _pendingTransaction = {12}\n\t         _retainedTransactionId = {13}\n\t         _nonTransactedOpenResultCount = {14}\n\t         _connHandler = {15}\n\t         _fAsync = {16}\n\t         _fMARS = {17}\n\t         _fAwaitingPreLogin = {18}\n\t         _fPreLoginErrorOccurred = {19}\n\t         _sessionPool = {20}\n\t         _isShiloh = {21}\n\t         _isShilohSP1 = {22}\n\t         _isYukon = {23}\n\t         _sniSpnBuffer = {24}\n\t         _errors = {25}\n\t         _warnings = {26}\n\t         _attentionErrors = {27}\n\t         _attentionWarnings = {28}\n\t         _statistics = {29}\n\t         _statisticsIsInTransaction = {30}\n\t         _fPreserveTransaction = {31}         _fParallel = {32}", new object[]
			{
				null == this._physicalStateObj,
				null == this._pMarsPhysicalConObj,
				this._state,
				this._server,
				this._fResetConnection,
				(this._defaultCollation == null) ? "(null)" : this._defaultCollation.TraceString(),
				this._defaultCodePage,
				this._defaultLCID,
				this.TraceObjectClass(this._defaultEncoding),
				"",
				this._encryptionOption,
				(this._currentTransaction == null) ? "(null)" : this._currentTransaction.TraceString(),
				(this._pendingTransaction == null) ? "(null)" : this._pendingTransaction.TraceString(),
				this._retainedTransactionId,
				this._nonTransactedOpenResultCount,
				(this._connHandler == null) ? "(null)" : this._connHandler.ObjectID.ToString(null),
				this._fAsync,
				this._fMARS,
				this._fAwaitingPreLogin,
				this._fPreLoginErrorOccurred,
				(this._sessionPool == null) ? "(null)" : this._sessionPool.TraceString(),
				this._isShiloh,
				this._isShilohSP1,
				this._isYukon,
				(this._sniSpnBuffer == null) ? "(null)" : this._sniSpnBuffer.Length.ToString(null),
				(this._errors == null) ? "(null)" : this._errors.Count.ToString(null),
				(this._warnings == null) ? "(null)" : this._warnings.Count.ToString(null),
				(this._attentionErrors == null) ? "(null)" : this._attentionErrors.Count.ToString(null),
				(this._attentionWarnings == null) ? "(null)" : this._attentionWarnings.Count.ToString(null),
				null == this._statistics,
				this._statisticsIsInTransaction,
				this._fPreserveTransaction,
				(this._connHandler == null) ? "(null)" : this._connHandler.ConnectionOptions.MultiSubnetFailover.ToString(null)
			});
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x0029C9A8 File Offset: 0x0029BDA8
		private string TraceObjectClass(object instance)
		{
			if (instance == null)
			{
				return "(null)";
			}
			return instance.GetType().ToString();
		}

		// Token: 0x04001B36 RID: 6966
		private const int ATTENTION_TIMEOUT = 5000;

		// Token: 0x04001B37 RID: 6967
		private const ulong _indeterminateSize = 18446744073709551615UL;

		// Token: 0x04001B38 RID: 6968
		private const string StateTraceFormatString = "\n\t         _physicalStateObj = {0}\n\t         _pMarsPhysicalConObj = {1}\n\t         _state = {2}\n\t         _server = {3}\n\t         _fResetConnection = {4}\n\t         _defaultCollation = {5}\n\t         _defaultCodePage = {6}\n\t         _defaultLCID = {7}\n\t         _defaultEncoding = {8}\n\t         _encryptionOption = {10}\n\t         _currentTransaction = {11}\n\t         _pendingTransaction = {12}\n\t         _retainedTransactionId = {13}\n\t         _nonTransactedOpenResultCount = {14}\n\t         _connHandler = {15}\n\t         _fAsync = {16}\n\t         _fMARS = {17}\n\t         _fAwaitingPreLogin = {18}\n\t         _fPreLoginErrorOccurred = {19}\n\t         _sessionPool = {20}\n\t         _isShiloh = {21}\n\t         _isShilohSP1 = {22}\n\t         _isYukon = {23}\n\t         _sniSpnBuffer = {24}\n\t         _errors = {25}\n\t         _warnings = {26}\n\t         _attentionErrors = {27}\n\t         _attentionWarnings = {28}\n\t         _statistics = {29}\n\t         _statisticsIsInTransaction = {30}\n\t         _fPreserveTransaction = {31}         _fParallel = {32}";

		// Token: 0x04001B39 RID: 6969
		private static int _objectTypeCount;

		// Token: 0x04001B3A RID: 6970
		internal readonly int _objectID = Interlocked.Increment(ref TdsParser._objectTypeCount);

		// Token: 0x04001B3B RID: 6971
		public static LocalDataStoreSlot ReliabilitySlot = null;

		// Token: 0x04001B3C RID: 6972
		internal TdsParserStateObject _physicalStateObj;

		// Token: 0x04001B3D RID: 6973
		internal TdsParserStateObject _pMarsPhysicalConObj;

		// Token: 0x04001B3E RID: 6974
		internal TdsParserState _state;

		// Token: 0x04001B3F RID: 6975
		private string _server = "";

		// Token: 0x04001B40 RID: 6976
		internal volatile bool _fResetConnection;

		// Token: 0x04001B41 RID: 6977
		internal volatile bool _fPreserveTransaction;

		// Token: 0x04001B42 RID: 6978
		private SqlCollation _defaultCollation;

		// Token: 0x04001B43 RID: 6979
		private int _defaultCodePage;

		// Token: 0x04001B44 RID: 6980
		private int _defaultLCID;

		// Token: 0x04001B45 RID: 6981
		internal Encoding _defaultEncoding;

		// Token: 0x04001B46 RID: 6982
		private static EncryptionOptions _sniSupportedEncryptionOption = SNILoadHandle.SingletonInstance.Options;

		// Token: 0x04001B47 RID: 6983
		private EncryptionOptions _encryptionOption = TdsParser._sniSupportedEncryptionOption;

		// Token: 0x04001B48 RID: 6984
		private SqlInternalTransaction _currentTransaction;

		// Token: 0x04001B49 RID: 6985
		private SqlInternalTransaction _pendingTransaction;

		// Token: 0x04001B4A RID: 6986
		private long _retainedTransactionId;

		// Token: 0x04001B4B RID: 6987
		private int _nonTransactedOpenResultCount;

		// Token: 0x04001B4C RID: 6988
		private SqlInternalConnectionTds _connHandler;

		// Token: 0x04001B4D RID: 6989
		private bool _fAsync;

		// Token: 0x04001B4E RID: 6990
		private bool _fMARS;

		// Token: 0x04001B4F RID: 6991
		internal volatile bool _fAwaitingPreLogin;

		// Token: 0x04001B50 RID: 6992
		internal bool _loginWithFailover;

		// Token: 0x04001B51 RID: 6993
		internal volatile bool _fPreLoginErrorOccurred;

		// Token: 0x04001B52 RID: 6994
		internal AutoResetEvent _resetConnectionEvent;

		// Token: 0x04001B53 RID: 6995
		private TdsParserSessionPool _sessionPool;

		// Token: 0x04001B54 RID: 6996
		private bool _isShiloh;

		// Token: 0x04001B55 RID: 6997
		private bool _isShilohSP1;

		// Token: 0x04001B56 RID: 6998
		private bool _isYukon;

		// Token: 0x04001B57 RID: 6999
		private bool _isKatmai;

		// Token: 0x04001B58 RID: 7000
		private byte[] _sniSpnBuffer;

		// Token: 0x04001B59 RID: 7001
		private SqlErrorCollection _errors;

		// Token: 0x04001B5A RID: 7002
		private SqlErrorCollection _warnings;

		// Token: 0x04001B5B RID: 7003
		private SqlErrorCollection _attentionErrors;

		// Token: 0x04001B5C RID: 7004
		private SqlErrorCollection _attentionWarnings;

		// Token: 0x04001B5D RID: 7005
		private object _ErrorCollectionLock = new object();

		// Token: 0x04001B5E RID: 7006
		private SqlStatistics _statistics;

		// Token: 0x04001B5F RID: 7007
		private bool _statisticsIsInTransaction;

		// Token: 0x04001B60 RID: 7008
		private byte[] datetimeBuffer = new byte[10];

		// Token: 0x04001B61 RID: 7009
		private static byte[] s_nicAddress;

		// Token: 0x04001B62 RID: 7010
		private static bool s_fSSPILoaded = false;

		// Token: 0x04001B63 RID: 7011
		private static volatile uint s_maxSSPILength = 0U;

		// Token: 0x04001B64 RID: 7012
		private static readonly byte[] s_longDataHeader = new byte[]
		{
			16, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue,
			byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue
		};

		// Token: 0x04001B65 RID: 7013
		private static object s_tdsParserLock = new object();

		// Token: 0x04001B66 RID: 7014
		private static readonly byte[] s_xmlMetadataSubstituteSequence = new byte[] { 231, byte.MaxValue, byte.MaxValue, 0, 0, 0, 0, 0 };

		// Token: 0x04001B67 RID: 7015
		private static readonly IEnumerable<SqlDataRecord> __tvpEmptyValue = new List<SqlDataRecord>().AsReadOnly();

		// Token: 0x0200031D RID: 797
		private class TdsOrderUnique
		{
			// Token: 0x06002A61 RID: 10849 RVA: 0x0029CA44 File Offset: 0x0029BE44
			internal TdsOrderUnique(short ordinal, byte flags)
			{
				this.ColumnOrdinal = ordinal;
				this.Flags = flags;
			}

			// Token: 0x04001B68 RID: 7016
			internal short ColumnOrdinal;

			// Token: 0x04001B69 RID: 7017
			internal byte Flags;
		}
	}
}
