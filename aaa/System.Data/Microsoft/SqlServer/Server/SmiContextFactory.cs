using System;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Permissions;

namespace Microsoft.SqlServer.Server
{
	// Token: 0x02000034 RID: 52
	internal sealed class SmiContextFactory
	{
		// Token: 0x060001DF RID: 479 RVA: 0x001CAD48 File Offset: 0x001CA148
		private SmiContextFactory()
		{
			if (InOutOfProcHelper.InProc)
			{
				Type type = Type.GetType("Microsoft.SqlServer.Server.InProcLink, SqlAccess, PublicKeyToken=89845dcd8080cc91");
				if (type == null)
				{
					throw SQL.ContextUnavailableOutOfProc();
				}
				FieldInfo staticField = this.GetStaticField(type, "Instance");
				if (staticField == null)
				{
					throw SQL.ContextUnavailableOutOfProc();
				}
				this._smiLink = (SmiLink)this.GetValue(staticField);
				FieldInfo staticField2 = this.GetStaticField(type, "BuildVersion");
				if (staticField2 != null)
				{
					uint num = (uint)this.GetValue(staticField2);
					this._majorVersion = (byte)(num >> 24);
					this._minorVersion = (byte)((num >> 16) & 255U);
					this._buildNum = (short)(num & 65535U);
					this._serverVersion = string.Format(null, "{0:00}.{1:00}.{2:0000}", new object[]
					{
						this._majorVersion,
						(short)this._minorVersion,
						this._buildNum
					});
				}
				else
				{
					this._serverVersion = string.Empty;
				}
				this._negotiatedSmiVersion = this._smiLink.NegotiateVersion(210UL);
				bool flag = false;
				int num2 = 0;
				while (!flag && num2 < this.__supportedSmiVersions.Length)
				{
					if (this.__supportedSmiVersions[num2] == this._negotiatedSmiVersion)
					{
						flag = true;
					}
					num2++;
				}
				if (!flag)
				{
					this._smiLink = null;
				}
				this._eventSinkForGetCurrentContext = new SmiEventSink_Default();
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x001CAEC0 File Offset: 0x001CA2C0
		internal ulong NegotiatedSmiVersion
		{
			get
			{
				if (this._smiLink == null)
				{
					throw SQL.ContextUnavailableOutOfProc();
				}
				return this._negotiatedSmiVersion;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x001CAEE4 File Offset: 0x001CA2E4
		internal string ServerVersion
		{
			get
			{
				if (this._smiLink == null)
				{
					throw SQL.ContextUnavailableOutOfProc();
				}
				return this._serverVersion;
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x001CAF08 File Offset: 0x001CA308
		internal SmiContext GetCurrentContext()
		{
			if (this._smiLink == null)
			{
				throw SQL.ContextUnavailableOutOfProc();
			}
			object currentContext = this._smiLink.GetCurrentContext(this._eventSinkForGetCurrentContext);
			this._eventSinkForGetCurrentContext.ProcessMessagesAndThrow();
			if (currentContext == null)
			{
				throw SQL.ContextUnavailableWhileInProc();
			}
			return (SmiContext)currentContext;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x001CAF50 File Offset: 0x001CA350
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		private object GetValue(FieldInfo fieldInfo)
		{
			return fieldInfo.GetValue(null);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x001CAF68 File Offset: 0x001CA368
		[ReflectionPermission(SecurityAction.Assert, MemberAccess = true)]
		private FieldInfo GetStaticField(Type aType, string fieldName)
		{
			return aType.GetField(fieldName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.GetField);
		}

		// Token: 0x04000566 RID: 1382
		internal const ulong YukonVersion = 100UL;

		// Token: 0x04000567 RID: 1383
		internal const ulong KatmaiVersion = 210UL;

		// Token: 0x04000568 RID: 1384
		internal const ulong LatestVersion = 210UL;

		// Token: 0x04000569 RID: 1385
		public static readonly SmiContextFactory Instance = new SmiContextFactory();

		// Token: 0x0400056A RID: 1386
		private readonly SmiLink _smiLink;

		// Token: 0x0400056B RID: 1387
		private readonly ulong _negotiatedSmiVersion;

		// Token: 0x0400056C RID: 1388
		private readonly byte _majorVersion;

		// Token: 0x0400056D RID: 1389
		private readonly byte _minorVersion;

		// Token: 0x0400056E RID: 1390
		private readonly short _buildNum;

		// Token: 0x0400056F RID: 1391
		private readonly string _serverVersion;

		// Token: 0x04000570 RID: 1392
		private readonly SmiEventSink_Default _eventSinkForGetCurrentContext;

		// Token: 0x04000571 RID: 1393
		private readonly ulong[] __supportedSmiVersions = new ulong[] { 100UL, 210UL };
	}
}
