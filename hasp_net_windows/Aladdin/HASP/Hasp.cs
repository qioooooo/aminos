using System;
using Aladdin.HASP.Internal;
using HEDS;

namespace Aladdin.HASP
{
	public class Hasp : ICloneable, IDisposable, IComparable<Hasp>
	{
		public Hasp()
		{
			this.SetKey(HaspFeature.Default);
			this.isFeatureSet = true;
		}

		public Hasp(HaspFeature feature)
		{
			this.SetKey(feature);
			this.isFeatureSet = true;
		}

		private void SetKey(HaspFeature feature)
		{
			this.key = new Hasp.HaspBase(feature);
		}

		public Hasp(Hasp other)
		{
			if (other == null)
			{
				this.SetKey(HaspFeature.Default);
			}
			else
			{
				this.key = other.key;
				this.key.RefCount++;
				this.isFeatureSet = other.isFeatureSet;
			}
		}

		~Hasp()
		{
			this.Dispose(false);
		}

		public HaspStatus Decrypt(byte[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Decrypt(char[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Decrypt(double[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Decrypt(short[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Decrypt(int[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Decrypt(long[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Decrypt(float[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Decrypt(ref string data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.decrypt(this.key.handle, ref data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Encrypt(byte[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Encrypt(char[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Encrypt(double[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Encrypt(short[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Encrypt(int[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Encrypt(long[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Encrypt(float[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public HaspStatus Encrypt(ref string data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = this.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.encrypt(this.key.handle, ref data);
				}
			}
			return haspStatus;
		}

		public HaspFeature Feature
		{
			get
			{
				return this.key.feature;
			}
		}

		public HaspFile GetFile()
		{
			return this.GetFile(HaspFileId.Main);
		}

		public HaspFile GetFile(HaspFileId fileId)
		{
			return new HaspFile(fileId, this.IsValid() ? this : new Hasp());
		}

		public HaspFile GetFile(int fileId)
		{
			return new HaspFile(fileId, this.IsValid() ? this : new Hasp());
		}

		public static HaspStatus GetInfo(string query, string format, byte[] vendorCode, ref string info)
		{
			try
			{
				Hasp.TestSignature();
			}
			catch (DllBrokenException)
			{
				return HaspStatus.HaspDotNetDllBroken;
			}
			HaspStatus haspStatus;
			if (query == null || format == null || vendorCode == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				info = "";
				HaspStatus haspStatus2 = Hasp.Hasp_Prologue();
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.get_info(query, format, vendorCode, ref info);
				}
			}
			return haspStatus;
		}

		public static HaspStatus GetInfo(string query, string format, string vendorCode, ref string info)
		{
			try
			{
				Hasp.TestSignature();
			}
			catch (DllBrokenException)
			{
				return HaspStatus.HaspDotNetDllBroken;
			}
			HaspStatus haspStatus;
			if (query == null || format == null || vendorCode == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				info = "";
				HaspStatus haspStatus2 = Hasp.Hasp_Prologue();
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.get_info(query, format, vendorCode, ref info);
				}
			}
			return haspStatus;
		}

		public HaspLegacy Legacy
		{
			get
			{
				return new HaspLegacy(this.HasLegacy ? this : new Hasp());
			}
		}

		public HaspStatus GetRtc(ref DateTime rtc)
		{
			HaspStatus haspStatus = this.Hasp_Prologue(this.key);
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				long num = 0L;
				haspStatus = ApiDisp.get_rtc(this.key.handle, ref num);
				if (haspStatus != HaspStatus.StatusOk)
				{
					haspStatus2 = haspStatus;
				}
				else
				{
					DateTime dateTime = new DateTime(1970, 1, 1);
					rtc = new DateTime(num * 10L * 1000L * 1000L + dateTime.Ticks);
					haspStatus2 = haspStatus;
				}
			}
			return haspStatus2;
		}

		public HaspStatus GetSessionInfo(string format, ref string info)
		{
			HaspStatus haspStatus = this.Hasp_Prologue(this.key);
			info = "";
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.get_sessioninfo(this.key.handle, format, ref info);
			}
			return haspStatus2;
		}

		public static HaspStatus GetApiVersion(byte[] vendorCode, ref HaspVersion version)
		{
			try
			{
				Hasp.TestSignature();
			}
			catch (DllBrokenException)
			{
				return HaspStatus.HaspDotNetDllBroken;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			version = default(HaspVersion);
			HaspStatus haspStatus = Hasp.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus = ApiDisp.get_version(ref num, ref num2, ref num3, ref num4, vendorCode);
				if (haspStatus != HaspStatus.StatusOk)
				{
					haspStatus2 = haspStatus;
				}
				else
				{
					version = new HaspVersion(num, num2, num3, num4);
					haspStatus2 = HaspStatus.StatusOk;
				}
			}
			return haspStatus2;
		}

		public static HaspStatus GetApiVersion(string vendorCode, ref HaspVersion version)
		{
			try
			{
				Hasp.TestSignature();
			}
			catch (DllBrokenException)
			{
				return HaspStatus.HaspDotNetDllBroken;
			}
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			version = default(HaspVersion);
			HaspStatus haspStatus = Hasp.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus = ApiDisp.get_version(ref num, ref num2, ref num3, ref num4, vendorCode);
				if (haspStatus != HaspStatus.StatusOk)
				{
					haspStatus2 = haspStatus;
				}
				else
				{
					version = new HaspVersion(num, num2, num3, num4);
					haspStatus2 = HaspStatus.StatusOk;
				}
			}
			return haspStatus2;
		}

		public bool HasLegacy
		{
			get
			{
				return !this.isDisposed && this.key.feature.IsProgNum;
			}
		}

		public static string KeyInfo
		{
			get
			{
				return "<haspformat format=\"keyinfo\"/>";
			}
		}

		public HaspStatus Login(byte[] vendorCode)
		{
			HaspStatus haspStatus;
			if (this.isDisposed)
			{
				haspStatus = HaspStatus.InvalidObject;
			}
			else
			{
				try
				{
					Hasp.TestSignature();
				}
				catch (DllBrokenException)
				{
					return HaspStatus.HaspDotNetDllBroken;
				}
				if (this.key.IsLoggedIn)
				{
					haspStatus = HaspStatus.AlreadyLoggedIn;
				}
				else
				{
					HaspStatus haspStatus2 = ApiDisp.login(this.key.feature.Feature, vendorCode, ref this.key.handle);
					if (haspStatus2 != HaspStatus.StatusOk)
					{
						haspStatus = haspStatus2;
					}
					else
					{
						this.key.IsLoggedIn = true;
						haspStatus = HaspStatus.StatusOk;
					}
				}
			}
			return haspStatus;
		}

		public HaspStatus Login(string vendorCode)
		{
			HaspStatus haspStatus;
			if (this.isDisposed)
			{
				haspStatus = HaspStatus.InvalidObject;
			}
			else
			{
				try
				{
					Hasp.TestSignature();
				}
				catch (DllBrokenException)
				{
					return HaspStatus.HaspDotNetDllBroken;
				}
				if (this.key.IsLoggedIn)
				{
					haspStatus = HaspStatus.AlreadyLoggedIn;
				}
				else
				{
					HaspStatus haspStatus2 = ApiDisp.login(this.key.feature.Feature, vendorCode, ref this.key.handle);
					if (haspStatus2 != HaspStatus.StatusOk)
					{
						haspStatus = haspStatus2;
					}
					else
					{
						this.key.IsLoggedIn = true;
						haspStatus = HaspStatus.StatusOk;
					}
				}
			}
			return haspStatus;
		}

		public HaspStatus Login(byte[] vendorCode, string scope)
		{
			HaspStatus haspStatus;
			if (this.isDisposed)
			{
				haspStatus = HaspStatus.InvalidObject;
			}
			else
			{
				try
				{
					Hasp.TestSignature();
				}
				catch (DllBrokenException)
				{
					return HaspStatus.HaspDotNetDllBroken;
				}
				if (this.key.IsLoggedIn)
				{
					haspStatus = HaspStatus.AlreadyLoggedIn;
				}
				else if (vendorCode == null)
				{
					haspStatus = HaspStatus.InvalidParameter;
				}
				else
				{
					HaspStatus haspStatus2 = ApiDisp.login_scope(this.key.feature.Feature, scope, vendorCode, ref this.key.handle);
					if (haspStatus2 != HaspStatus.StatusOk)
					{
						haspStatus = haspStatus2;
					}
					else
					{
						this.key.IsLoggedIn = true;
						haspStatus = HaspStatus.StatusOk;
					}
				}
			}
			return haspStatus;
		}

		public HaspStatus Login(string vendorCode, string scope)
		{
			HaspStatus haspStatus;
			if (this.isDisposed)
			{
				haspStatus = HaspStatus.InvalidObject;
			}
			else
			{
				try
				{
					Hasp.TestSignature();
				}
				catch (DllBrokenException)
				{
					return HaspStatus.HaspDotNetDllBroken;
				}
				if (this.key.IsLoggedIn)
				{
					haspStatus = HaspStatus.AlreadyLoggedIn;
				}
				else if (vendorCode == null)
				{
					haspStatus = HaspStatus.InvalidParameter;
				}
				else
				{
					HaspStatus haspStatus2 = ApiDisp.login_scope(this.key.feature.Feature, scope, vendorCode, ref this.key.handle);
					if (haspStatus2 != HaspStatus.StatusOk)
					{
						haspStatus = haspStatus2;
					}
					else
					{
						this.key.IsLoggedIn = true;
						haspStatus = HaspStatus.StatusOk;
					}
				}
			}
			return haspStatus;
		}

		public HaspStatus Logout()
		{
			HaspStatus haspStatus;
			if (this.isDisposed)
			{
				haspStatus = HaspStatus.InvalidObject;
			}
			else if (!this.key.IsLoggedIn)
			{
				haspStatus = HaspStatus.AlreadyLoggedOut;
			}
			else
			{
				HaspStatus haspStatus2 = ApiDisp.logout(this.key.handle);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					this.key.IsLoggedIn = false;
					this.key.handle = 0;
					this.key.loggedOut = true;
					haspStatus = HaspStatus.StatusOk;
				}
			}
			return haspStatus;
		}

		public static string SessionInfo
		{
			get
			{
				return "<haspformat format=\"sessioninfo\"/>";
			}
		}

		public override string ToString()
		{
			return this.key.handle.ToString();
		}

		public static HaspStatus Update(string updateXml, ref string acknowledgeXml)
		{
			try
			{
				Hasp.TestSignature();
			}
			catch (DllBrokenException)
			{
				return HaspStatus.HaspDotNetDllBroken;
			}
			acknowledgeXml = "";
			HaspStatus haspStatus = Hasp.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.update(updateXml, ref acknowledgeXml);
			}
			return haspStatus2;
		}

		[Obsolete("Starting from Sentinel LDK version 6.0, the \u0093Detach\u0094 method has been deprecated.SafeNet recommends that user should use the \u0093Transfer\u0094 method to perform the detach/cancel actions. This method has been retained for backward compatibility.")]
		public static HaspStatus Detach(string detach_action, string scope, string vendor_code, string recipient, ref string info)
		{
			try
			{
				Hasp.TestSignature();
			}
			catch (DllBrokenException)
			{
				return HaspStatus.HaspDotNetDllBroken;
			}
			HaspStatus haspStatus = Hasp.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.detach(detach_action, scope, vendor_code, recipient, ref info);
			}
			return haspStatus2;
		}

		[Obsolete("Starting from Sentinel LDK version 6.0, the \u0093Detach\u0094 method has been deprecated.SafeNet recommends that user should use the \u0093Transfer\u0094 method to perform the detach/cancel actions. This method has been retained for backward compatibility.")]
		public static HaspStatus Detach(string detach_action, string scope, byte[] vendor_code, string recipient, ref string info)
		{
			try
			{
				Hasp.TestSignature();
			}
			catch (DllBrokenException)
			{
				return HaspStatus.HaspDotNetDllBroken;
			}
			HaspStatus haspStatus = Hasp.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.detach(detach_action, scope, vendor_code, recipient, ref info);
			}
			return haspStatus2;
		}

		public static HaspStatus Transfer(string action, string scope, string vendor_code, string recipient, ref string info)
		{
			try
			{
				Hasp.TestSignature();
			}
			catch (DllBrokenException)
			{
				return HaspStatus.HaspDotNetDllBroken;
			}
			HaspStatus haspStatus = Hasp.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.transfer(action, scope, vendor_code, recipient, ref info);
			}
			return haspStatus2;
		}

		public static HaspStatus Transfer(string action, string scope, byte[] vendor_code, string recipient, ref string info)
		{
			try
			{
				Hasp.TestSignature();
			}
			catch (DllBrokenException)
			{
				return HaspStatus.HaspDotNetDllBroken;
			}
			HaspStatus haspStatus = Hasp.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.transfer(action, scope, vendor_code, recipient, ref info);
			}
			return haspStatus2;
		}

		public static string UpdateInfo
		{
			get
			{
				return "<haspformat format=\"updateinfo\"/>";
			}
		}

		public static string Recipient
		{
			get
			{
				return "<haspformat root=\"location\">  <license_manager>    <attribute name=\"id\" />    <attribute name=\"time\" />    <element name=\"hostname\" />    <element name=\"version\" />    <element name=\"host_fingerprint\" />  </license_manager></haspformat>";
			}
		}

		public static string Fingerprint
		{
			get
			{
				return "<haspformat format=\"host_fingerprint\"/>";
			}
		}

		internal HaspStatus Hasp_Prologue(Hasp.HaspBase key)
		{
			HaspStatus haspStatus;
			if (this.isDisposed)
			{
				haspStatus = HaspStatus.InvalidObject;
			}
			else if (key.IsLoggedIn)
			{
				haspStatus = HaspStatus.StatusOk;
			}
			else if (key.loggedOut)
			{
				haspStatus = HaspStatus.AlreadyLoggedOut;
			}
			else
			{
				haspStatus = HaspStatus.InvalidHandle;
			}
			return haspStatus;
		}

		internal static HaspStatus Hasp_Prologue()
		{
			return HaspStatus.StatusOk;
		}

		public bool IsValid()
		{
			return this.isFeatureSet && !this.isDisposed;
		}

		public bool IsLoggedIn()
		{
			return this.key != null && this.IsValid() && this.key.IsLoggedIn;
		}

		public object Clone()
		{
			return new Hasp(this);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.isDisposed)
			{
				if (this.key != null)
				{
					this.key.RefCount--;
					if (this.key.RefCount == 0 && this.key.IsLoggedIn)
					{
						this.Logout();
					}
					this.key = null;
				}
				this.isDisposed = true;
			}
		}

		public Hasp Assign(Hasp other)
		{
			Hasp hasp;
			if (this.isDisposed)
			{
				hasp = null;
			}
			else
			{
				this.key.RefCount--;
				if (this.key.RefCount == 0 && this.key.IsLoggedIn)
				{
					this.Logout();
				}
				this.key = null;
				if (other == null)
				{
					hasp = null;
				}
				else
				{
					this.key = other.key;
					this.key.RefCount++;
					this.isFeatureSet = other.isFeatureSet;
					hasp = this;
				}
			}
			return hasp;
		}

		public int CompareTo(Hasp other)
		{
			int num;
			if (other == null)
			{
				num = 1;
			}
			else
			{
				int num2 = this.key.feature.CompareTo(other.key.feature);
				if (num2 != 0)
				{
					num = num2;
				}
				else
				{
					num = this.key.handle.CompareTo(other.key.handle);
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public static bool operator ==(Hasp left, object right)
		{
			bool flag;
			if (left == null && right == null)
			{
				flag = true;
			}
			else if (left == null || right == null)
			{
				flag = false;
			}
			else
			{
				Hasp hasp = right as Hasp;
				if (hasp == null)
				{
					HaspFile haspFile = right as HaspFile;
					flag = !(haspFile == null) && haspFile == left;
				}
				else
				{
					flag = left.CompareTo(hasp) == 0;
				}
			}
			return flag;
		}

		public static bool operator !=(Hasp left, object right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return this.key.handle;
		}

		private static void TestSignature()
		{
			if (ApiDisp.IsRunningOnMono() <= 0)
			{
				if (!Hasp.tested)
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
					HedsFile.heds_status heds_status = hedsFile.CheckSignature(hedsFile.FindFile(Hasp.libPath), 1);
					if (heds_status != HedsFile.heds_status.HEDS_STATUS_OK)
					{
						throw new DllBrokenException(heds_status.ToString());
					}
					Hasp.tested = true;
				}
			}
		}

		public HaspStatus SetLibPath(string path)
		{
			HaspStatus haspStatus;
			if (this.isDisposed)
			{
				haspStatus = HaspStatus.InvalidObject;
			}
			else
			{
				HaspStatus haspStatus2;
				if (path != null)
				{
					string text = path;
					if (!text.EndsWith("\\"))
					{
						text += "\\";
					}
					haspStatus2 = ApiDisp.set_lib_path(text);
				}
				else
				{
					haspStatus2 = ApiDisp.set_lib_path(path);
				}
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					if (path != null)
					{
						Hasp.libPath = path;
					}
					try
					{
						Hasp.TestSignature();
					}
					catch (DllBrokenException)
					{
						return HaspStatus.HaspDotNetDllBroken;
					}
					haspStatus = HaspStatus.StatusOk;
				}
			}
			return haspStatus;
		}

		internal Hasp.HaspBase key;

		private bool isFeatureSet = false;

		private bool isDisposed = false;

		private static string libPath;

		private static bool tested;

		internal class HaspBase
		{
			public HaspBase(HaspFeature feature)
			{
				this.feature = feature;
			}

			public bool IsLoggedIn
			{
				get
				{
					return this.loggedIn;
				}
				set
				{
					this.loggedIn = value;
				}
			}

			public int RefCount
			{
				get
				{
					return this.refCount;
				}
				set
				{
					this.refCount = value;
				}
			}

			public int handle = 0;

			public bool loggedOut = false;

			public HaspFeature feature;

			private bool loggedIn = false;

			private int refCount = 1;
		}
	}
}
