using System;
using HEDS;
using SafeNet.Sentinel.Internal;

namespace SafeNet.Sentinel
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public class AdminApi
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002148 File Offset: 0x00000348
		public AdminApi()
		{
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002198 File Offset: 0x00000398
		public AdminApi(string hostname)
		{
			this.m_hostname = hostname;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021F0 File Offset: 0x000003F0
		public AdminApi(string hostname, ushort port)
		{
			this.m_hostname = hostname;
			this.m_port = port;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002250 File Offset: 0x00000450
		public AdminApi(string hostname, ushort port, string password)
		{
			this.m_hostname = hostname;
			this.m_port = port;
			this.m_password = password;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022B4 File Offset: 0x000004B4
		public AdminApi(VendorCodeType vendorCodeType, string hostname)
		{
			this.m_vendorCode = vendorCodeType;
			this.m_hostname = hostname;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002314 File Offset: 0x00000514
		public AdminApi(VendorCodeType vendorCodeType, string hostname, ushort port)
		{
			this.m_vendorCode = vendorCodeType;
			this.m_hostname = hostname;
			this.m_port = port;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002378 File Offset: 0x00000578
		public AdminApi(VendorCodeType vendorCodeType, string hostname, ushort port, string password)
		{
			this.m_vendorCode = vendorCodeType;
			this.m_hostname = hostname;
			this.m_port = port;
			this.m_password = password;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023E4 File Offset: 0x000005E4
		public AdminStatus connect()
		{
			if (this.m_adminApiDsp != null)
			{
				this.m_connected = false;
				this.m_adminApiDsp.sntl_admin_context_delete();
				this.m_adminApiDsp = null;
			}
			this.m_adminApiDsp = new AdminApiDsp();
			AdminStatus adminStatus;
			if (this.m_adminApiDsp.setVendorCodeFlag(this.m_vendorCode != null))
			{
				if (this.m_searchPath != null)
				{
					this.m_adminApiDsp.setLibPath(this.m_searchPath);
					this.m_adminApiDsp.setLibPathApiDsp(this.m_searchPath);
				}
				adminStatus = this.m_adminApiDsp.sntl_admin_context_new_scope(this.getScopeTemplate(this.m_vendorCode, this.m_password, this.m_hostname, this.m_port));
			}
			else
			{
				if (this.m_searchPath != null)
				{
					this.m_adminApiDsp.setLibPath(this.m_searchPath);
				}
				adminStatus = this.m_adminApiDsp.sntl_admin_context_new(this.m_hostname, this.m_port, this.m_password);
			}
			if (adminStatus == AdminStatus.StatusOk)
			{
				this.m_connected = true;
			}
			return adminStatus;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000024FC File Offset: 0x000006FC
		public AdminStatus connect(string hostname, ushort port, string password)
		{
			this.m_vendorCode.clear();
			this.m_hostname = hostname;
			this.m_port = port;
			this.m_password = password;
			return this.connect();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002538 File Offset: 0x00000738
		public AdminStatus connect(VendorCodeType vendorCode, string hostname, ushort port, string password)
		{
			this.m_vendorCode = vendorCode;
			this.m_hostname = hostname;
			this.m_port = port;
			this.m_password = password;
			return this.connect();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002570 File Offset: 0x00000770
		public AdminStatus adminGet(string scope, string format, ref string info)
		{
			AdminStatus adminStatus = AdminStatus.ConnectMissing;
			if (this.m_connected)
			{
				adminStatus = this.m_adminApiDsp.sntl_admin_get(scope, format, ref info);
			}
			return adminStatus;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025A8 File Offset: 0x000007A8
		public AdminStatus adminSet(string action, ref string return_status)
		{
			AdminStatus adminStatus = AdminStatus.ConnectMissing;
			if (this.m_connected)
			{
				adminStatus = this.m_adminApiDsp.sntl_admin_set(action, ref return_status);
			}
			return adminStatus;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025DC File Offset: 0x000007DC
		public void SetLibPath(string path)
		{
			if (path != null)
			{
				string text = path;
				if (!text.EndsWith("\\"))
				{
					text += "\\";
				}
				this.m_searchPath = text;
			}
			else
			{
				this.m_searchPath = path;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002624 File Offset: 0x00000824
		private static void TestSignature()
		{
			if (AdminApiDsp.isRunningOnMono() <= 0)
			{
				if (!AdminApi.tested)
				{
					HedsCrypt hedsCrypt = new HedsCrypt();
					hedsCrypt.Init("<RSAKeyValue>  <Modulus>tPkMcaY3CO1MlQp+hShdu1MWrOkisuRmubklR4cxQt9JM1i6wPooMkeRXu62u/JyUk           IEe4Y45JFCZL5/dOBirs7dyMBM+a0umaANRQE1wvr+k7uQyXuTo8dNwFlZR4WShBD2           O/gv/QMfgYuJ0nm5P0IFGjJrx+K6oiMrRLBcg5E=  </Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
					HedsFile hedsFile = new HedsFile();
					hedsFile.hedsCrypt = hedsCrypt;
					int size = IntPtr.Size;
					string text;
					if (size != 4)
					{
						if (size != 8)
						{
							throw new PlatformNotSupportedException();
						}
						text = "apidsp_windows_x64.dll";
					}
					else
					{
						text = "apidsp_windows.dll";
					}
					hedsFile.strFileName = text;
					HedsFile.heds_status heds_status = hedsFile.CheckSignature(hedsFile.FindFile(), 1);
					if (heds_status != HedsFile.heds_status.HEDS_STATUS_OK)
					{
						throw new DllBrokenException(heds_status.ToString());
					}
					AdminApi.tested = true;
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026E0 File Offset: 0x000008E0
		private string getScopeTemplate(VendorCodeType vct, string password, string hostname, ushort port)
		{
			string text = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><haspscope><vendor_code>" + vct.ToString() + "</vendor_code>";
			text = text + "<host>" + hostname + "</host>";
			if (port != 0)
			{
				text = string.Concat(new object[] { text, "<port>", port, "</port>" });
			}
			if (password.Length > 0)
			{
				text = text + "<password>" + password + "</password>";
			}
			return text + "</haspscope>";
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002784 File Offset: 0x00000984
		~AdminApi()
		{
			this.m_adminApiDsp.sntl_admin_context_delete();
		}

		// Token: 0x04000002 RID: 2
		private string m_hostname = "";

		// Token: 0x04000003 RID: 3
		private string m_password = "";

		// Token: 0x04000004 RID: 4
		private ushort m_port = 0;

		// Token: 0x04000005 RID: 5
		private bool m_connected = false;

		// Token: 0x04000006 RID: 6
		private VendorCodeType m_vendorCode = null;

		// Token: 0x04000007 RID: 7
		private AdminApiDsp m_adminApiDsp = null;

		// Token: 0x04000008 RID: 8
		private string m_searchPath = null;

		// Token: 0x04000009 RID: 9
		private static bool tested;
	}
}
