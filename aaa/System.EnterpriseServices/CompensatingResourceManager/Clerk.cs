using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000AF RID: 175
	public sealed class Clerk
	{
		// Token: 0x06000428 RID: 1064 RVA: 0x0000D29F File Offset: 0x0000C29F
		internal Clerk(CrmLogControl logControl)
		{
			this._control = logControl;
			this._monitor = this._control.GetMonitor();
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0000D2C0 File Offset: 0x0000C2C0
		private void ValidateCompensator(Type compensator)
		{
			if (!compensator.IsSubclassOf(typeof(Compensator)))
			{
				throw new ArgumentException(Resource.FormatString("CRM_CompensatorDerive"));
			}
			if (!new RegistrationServices().TypeRequiresRegistration(compensator))
			{
				throw new ArgumentException(Resource.FormatString("CRM_CompensatorConstructor"));
			}
			ServicedComponent servicedComponent = (ServicedComponent)Activator.CreateInstance(compensator);
			if (servicedComponent == null)
			{
				throw new ArgumentException(Resource.FormatString("CRM_CompensatorActivate"));
			}
			ServicedComponent.DisposeObject(servicedComponent);
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x0000D331 File Offset: 0x0000C331
		private void Init(string compensator, string description, CompensatorOptions flags)
		{
			this._control = new CrmLogControl();
			this._control.RegisterCompensator(compensator, description, flags);
			this._monitor = this._control.GetMonitor();
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x0000D360 File Offset: 0x0000C360
		public Clerk(Type compensator, string description, CompensatorOptions flags)
		{
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			securityPermission.Assert();
			Platform.Assert(Platform.W2K, "CRM");
			this.ValidateCompensator(compensator);
			string text = "{" + Marshal.GenerateGuidForType(compensator) + "}";
			this.Init(text, description, flags);
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x0000D3C0 File Offset: 0x0000C3C0
		public Clerk(string compensator, string description, CompensatorOptions flags)
		{
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			securityPermission.Assert();
			this.Init(compensator, description, flags);
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x0000D3EF File Offset: 0x0000C3EF
		public string TransactionUOW
		{
			get
			{
				return this._control.GetTransactionUOW();
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x0000D3FC File Offset: 0x0000C3FC
		public void ForceLog()
		{
			this._control.ForceLog();
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x0000D409 File Offset: 0x0000C409
		public void ForgetLogRecord()
		{
			this._control.ForgetLogRecord();
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x0000D416 File Offset: 0x0000C416
		public void ForceTransactionToAbort()
		{
			this._control.ForceTransactionToAbort();
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0000D424 File Offset: 0x0000C424
		public void WriteLogRecord(object record)
		{
			byte[] array = Packager.Serialize(record);
			this._control.WriteLogRecord(array);
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0000D444 File Offset: 0x0000C444
		public int LogRecordCount
		{
			get
			{
				return this._monitor.GetCount();
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000433 RID: 1075 RVA: 0x0000D451 File Offset: 0x0000C451
		private TransactionState TransactionState
		{
			get
			{
				return (TransactionState)this._monitor.GetTransactionState();
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0000D460 File Offset: 0x0000C460
		~Clerk()
		{
			if (this._monitor != null)
			{
				this._monitor.Dispose();
			}
			if (this._control != null)
			{
				this._control.Dispose();
			}
		}

		// Token: 0x040001E2 RID: 482
		private CrmLogControl _control;

		// Token: 0x040001E3 RID: 483
		private CrmMonitorLogRecords _monitor;
	}
}
