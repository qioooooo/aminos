using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.ServiceProcess.Design;
using System.Text;

namespace System.ServiceProcess
{
	// Token: 0x02000030 RID: 48
	public class ServiceProcessInstaller : ComponentInstaller
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000ED RID: 237 RVA: 0x000059BA File Offset: 0x000049BA
		public override string HelpText
		{
			get
			{
				if (ServiceProcessInstaller.helpPrinted)
				{
					return base.HelpText;
				}
				ServiceProcessInstaller.helpPrinted = true;
				return Res.GetString("HelpText") + "\r\n" + base.HelpText;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000EE RID: 238 RVA: 0x000059EA File Offset: 0x000049EA
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00005A00 File Offset: 0x00004A00
		[Browsable(false)]
		public string Password
		{
			get
			{
				if (!this.haveLoginInfo)
				{
					this.GetLoginInfo();
				}
				return this.password;
			}
			set
			{
				this.haveLoginInfo = false;
				this.password = value;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005A10 File Offset: 0x00004A10
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00005A26 File Offset: 0x00004A26
		[DefaultValue(ServiceAccount.User)]
		[ServiceProcessDescription("ServiceProcessInstallerAccount")]
		public ServiceAccount Account
		{
			get
			{
				if (!this.haveLoginInfo)
				{
					this.GetLoginInfo();
				}
				return this.serviceAccount;
			}
			set
			{
				this.haveLoginInfo = false;
				this.serviceAccount = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00005A36 File Offset: 0x00004A36
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00005A4C File Offset: 0x00004A4C
		[Browsable(false)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string Username
		{
			get
			{
				if (!this.haveLoginInfo)
				{
					this.GetLoginInfo();
				}
				return this.username;
			}
			set
			{
				this.haveLoginInfo = false;
				this.username = value;
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00005A5C File Offset: 0x00004A5C
		private static bool AccountHasRight(IntPtr policyHandle, byte[] accountSid, string rightName)
		{
			IntPtr intPtr = (IntPtr)0;
			int num = 0;
			int num2 = NativeMethods.LsaEnumerateAccountRights(policyHandle, accountSid, out intPtr, out num);
			if (num2 == -1073741772)
			{
				return false;
			}
			if (num2 != 0)
			{
				throw new Win32Exception(SafeNativeMethods.LsaNtStatusToWinError(num2));
			}
			bool flag = false;
			try
			{
				IntPtr intPtr2 = intPtr;
				for (int i = 0; i < num; i++)
				{
					NativeMethods.LSA_UNICODE_STRING_withPointer lsa_UNICODE_STRING_withPointer = new NativeMethods.LSA_UNICODE_STRING_withPointer();
					Marshal.PtrToStructure(intPtr2, lsa_UNICODE_STRING_withPointer);
					char[] array = new char[(int)lsa_UNICODE_STRING_withPointer.length];
					Marshal.Copy(lsa_UNICODE_STRING_withPointer.pwstr, array, 0, array.Length);
					string text = new string(array, 0, array.Length);
					if (string.Compare(text, rightName, StringComparison.Ordinal) == 0)
					{
						flag = true;
						break;
					}
					intPtr2 = (IntPtr)((long)intPtr2 + (long)Marshal.SizeOf(typeof(NativeMethods.LSA_UNICODE_STRING)));
				}
			}
			finally
			{
				SafeNativeMethods.LsaFreeMemory(intPtr);
			}
			return flag;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005B34 File Offset: 0x00004B34
		public override void CopyFromComponent(IComponent comp)
		{
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005B38 File Offset: 0x00004B38
		private byte[] GetAccountSid(string accountName)
		{
			byte[] array = new byte[256];
			int[] array2 = new int[] { array.Length };
			char[] array3 = new char[1024];
			int[] array4 = new int[] { array3.Length };
			int[] array5 = new int[1];
			if (accountName.Substring(0, 2) == ".\\")
			{
				StringBuilder stringBuilder = new StringBuilder(32);
				int num = 32;
				if (!NativeMethods.GetComputerName(stringBuilder, ref num))
				{
					throw new Win32Exception();
				}
				accountName = stringBuilder + accountName.Substring(1);
			}
			if (!NativeMethods.LookupAccountName(null, accountName, array, array2, array3, array4, array5))
			{
				throw new Win32Exception();
			}
			byte[] array6 = new byte[array2[0]];
			Array.Copy(array, 0, array6, 0, array2[0]);
			return array6;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005C00 File Offset: 0x00004C00
		private void GetLoginInfo()
		{
			if (base.Context != null && !base.DesignMode)
			{
				if (this.haveLoginInfo)
				{
					return;
				}
				this.haveLoginInfo = true;
				if (this.serviceAccount == ServiceAccount.User)
				{
					if (base.Context.Parameters.ContainsKey("username"))
					{
						this.username = base.Context.Parameters["username"];
					}
					if (base.Context.Parameters.ContainsKey("password"))
					{
						this.password = base.Context.Parameters["password"];
					}
					if (this.username == null || this.username.Length == 0 || this.password == null)
					{
						if (!base.Context.Parameters.ContainsKey("unattended"))
						{
							using (ServiceInstallerDialog serviceInstallerDialog = new ServiceInstallerDialog())
							{
								if (this.username != null)
								{
									serviceInstallerDialog.Username = this.username;
								}
								serviceInstallerDialog.ShowDialog();
								switch (serviceInstallerDialog.Result)
								{
								case ServiceInstallerDialogResult.OK:
									this.username = serviceInstallerDialog.Username;
									this.password = serviceInstallerDialog.Password;
									break;
								case ServiceInstallerDialogResult.UseSystem:
									this.username = null;
									this.password = null;
									this.serviceAccount = ServiceAccount.LocalSystem;
									break;
								case ServiceInstallerDialogResult.Canceled:
									throw new InvalidOperationException(Res.GetString("UserCanceledInstall", new object[] { base.Context.Parameters["assemblypath"] }));
								}
								return;
							}
						}
						throw new InvalidOperationException(Res.GetString("UnattendedCannotPrompt", new object[] { base.Context.Parameters["assemblypath"] }));
					}
				}
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005DC4 File Offset: 0x00004DC4
		private static void GrantAccountRight(IntPtr policyHandle, byte[] accountSid, string rightName)
		{
			NativeMethods.LSA_UNICODE_STRING lsa_UNICODE_STRING = new NativeMethods.LSA_UNICODE_STRING();
			lsa_UNICODE_STRING.buffer = rightName;
			lsa_UNICODE_STRING.length = (short)(lsa_UNICODE_STRING.buffer.Length * 2);
			lsa_UNICODE_STRING.maximumLength = (short)(lsa_UNICODE_STRING.buffer.Length * 2);
			int num = NativeMethods.LsaAddAccountRights(policyHandle, accountSid, lsa_UNICODE_STRING, 1);
			if (num != 0)
			{
				throw new Win32Exception(SafeNativeMethods.LsaNtStatusToWinError(num));
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005E20 File Offset: 0x00004E20
		public override void Install(IDictionary stateSaver)
		{
			try
			{
				ServiceInstaller.CheckEnvironment();
				try
				{
					if (!this.haveLoginInfo)
					{
						try
						{
							this.GetLoginInfo();
						}
						catch
						{
							stateSaver["hadServiceLogonRight"] = true;
							throw;
						}
					}
				}
				finally
				{
					stateSaver["Account"] = this.Account;
					if (this.Account == ServiceAccount.User)
					{
						stateSaver["Username"] = this.Username;
					}
				}
				if (this.Account == ServiceAccount.User)
				{
					IntPtr intPtr = this.OpenSecurityPolicy();
					bool flag = true;
					try
					{
						byte[] accountSid = this.GetAccountSid(this.Username);
						flag = ServiceProcessInstaller.AccountHasRight(intPtr, accountSid, "SeServiceLogonRight");
						if (!flag)
						{
							ServiceProcessInstaller.GrantAccountRight(intPtr, accountSid, "SeServiceLogonRight");
						}
					}
					finally
					{
						stateSaver["hadServiceLogonRight"] = flag;
						SafeNativeMethods.LsaClose(intPtr);
					}
				}
			}
			finally
			{
				base.Install(stateSaver);
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005F20 File Offset: 0x00004F20
		private IntPtr OpenSecurityPolicy()
		{
			NativeMethods.LSA_OBJECT_ATTRIBUTES lsa_OBJECT_ATTRIBUTES = new NativeMethods.LSA_OBJECT_ATTRIBUTES();
			GCHandle gchandle = GCHandle.Alloc(lsa_OBJECT_ATTRIBUTES, GCHandleType.Pinned);
			IntPtr intPtr3;
			try
			{
				IntPtr intPtr = gchandle.AddrOfPinnedObject();
				IntPtr intPtr2;
				int num = NativeMethods.LsaOpenPolicy(null, intPtr, 2064, out intPtr2);
				if (num != 0)
				{
					throw new Win32Exception(SafeNativeMethods.LsaNtStatusToWinError(num));
				}
				intPtr3 = intPtr2;
			}
			finally
			{
				gchandle.Free();
			}
			return intPtr3;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005F84 File Offset: 0x00004F84
		private static void RemoveAccountRight(IntPtr policyHandle, byte[] accountSid, string rightName)
		{
			NativeMethods.LSA_UNICODE_STRING lsa_UNICODE_STRING = new NativeMethods.LSA_UNICODE_STRING();
			lsa_UNICODE_STRING.buffer = rightName;
			lsa_UNICODE_STRING.length = (short)(lsa_UNICODE_STRING.buffer.Length * 2);
			lsa_UNICODE_STRING.maximumLength = lsa_UNICODE_STRING.length;
			int num = NativeMethods.LsaRemoveAccountRights(policyHandle, accountSid, false, lsa_UNICODE_STRING, 1);
			if (num != 0)
			{
				throw new Win32Exception(SafeNativeMethods.LsaNtStatusToWinError(num));
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005FD8 File Offset: 0x00004FD8
		public override void Rollback(IDictionary savedState)
		{
			try
			{
				if ((ServiceAccount)savedState["Account"] == ServiceAccount.User && !(bool)savedState["hadServiceLogonRight"])
				{
					string text = (string)savedState["Username"];
					IntPtr intPtr = this.OpenSecurityPolicy();
					try
					{
						byte[] accountSid = this.GetAccountSid(text);
						ServiceProcessInstaller.RemoveAccountRight(intPtr, accountSid, "SeServiceLogonRight");
					}
					finally
					{
						SafeNativeMethods.LsaClose(intPtr);
					}
				}
			}
			finally
			{
				base.Rollback(savedState);
			}
		}

		// Token: 0x04000221 RID: 545
		private ServiceAccount serviceAccount = ServiceAccount.User;

		// Token: 0x04000222 RID: 546
		private bool haveLoginInfo;

		// Token: 0x04000223 RID: 547
		private string password;

		// Token: 0x04000224 RID: 548
		private string username;

		// Token: 0x04000225 RID: 549
		private static bool helpPrinted;
	}
}
