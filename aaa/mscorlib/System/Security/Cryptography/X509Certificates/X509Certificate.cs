using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Util;
using System.Text;
using Microsoft.Win32;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008AC RID: 2220
	[ComVisible(true)]
	[Serializable]
	public class X509Certificate : IDeserializationCallback, ISerializable
	{
		// Token: 0x0600512C RID: 20780 RVA: 0x00123C0F File Offset: 0x00122C0F
		public X509Certificate()
		{
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x00123C22 File Offset: 0x00122C22
		public X509Certificate(byte[] data)
		{
			if (data != null && data.Length != 0)
			{
				this.LoadCertificateFromBlob(data, null, X509KeyStorageFlags.DefaultKeySet, false);
			}
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x00123C47 File Offset: 0x00122C47
		public X509Certificate(byte[] rawData, string password)
		{
			this.LoadCertificateFromBlob(rawData, password, X509KeyStorageFlags.DefaultKeySet, true);
		}

		// Token: 0x0600512F RID: 20783 RVA: 0x00123C64 File Offset: 0x00122C64
		public X509Certificate(byte[] rawData, SecureString password)
		{
			this.LoadCertificateFromBlob(rawData, password, X509KeyStorageFlags.DefaultKeySet, true);
		}

		// Token: 0x06005130 RID: 20784 RVA: 0x00123C81 File Offset: 0x00122C81
		public X509Certificate(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.LoadCertificateFromBlob(rawData, password, keyStorageFlags, true);
		}

		// Token: 0x06005131 RID: 20785 RVA: 0x00123C9E File Offset: 0x00122C9E
		public X509Certificate(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.LoadCertificateFromBlob(rawData, password, keyStorageFlags, true);
		}

		// Token: 0x06005132 RID: 20786 RVA: 0x00123CBB File Offset: 0x00122CBB
		public X509Certificate(string fileName)
		{
			this.LoadCertificateFromFile(fileName, null, X509KeyStorageFlags.DefaultKeySet);
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x00123CD7 File Offset: 0x00122CD7
		public X509Certificate(string fileName, string password)
		{
			this.LoadCertificateFromFile(fileName, password, X509KeyStorageFlags.DefaultKeySet);
		}

		// Token: 0x06005134 RID: 20788 RVA: 0x00123CF3 File Offset: 0x00122CF3
		public X509Certificate(string fileName, SecureString password)
		{
			this.LoadCertificateFromFile(fileName, password, X509KeyStorageFlags.DefaultKeySet);
		}

		// Token: 0x06005135 RID: 20789 RVA: 0x00123D0F File Offset: 0x00122D0F
		public X509Certificate(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.LoadCertificateFromFile(fileName, password, keyStorageFlags);
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x00123D2B File Offset: 0x00122D2B
		public X509Certificate(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.LoadCertificateFromFile(fileName, password, keyStorageFlags);
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x00123D48 File Offset: 0x00122D48
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public X509Certificate(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_InvalidHandle"), "handle");
			}
			X509Utils._DuplicateCertContext(handle, ref this.m_safeCertContext);
		}

		// Token: 0x06005138 RID: 20792 RVA: 0x00123D94 File Offset: 0x00122D94
		public X509Certificate(X509Certificate cert)
		{
			if (cert == null)
			{
				throw new ArgumentNullException("cert");
			}
			if (cert.m_safeCertContext.pCertContext != IntPtr.Zero)
			{
				X509Utils._DuplicateCertContext(cert.m_safeCertContext.pCertContext, ref this.m_safeCertContext);
			}
			GC.KeepAlive(cert.m_safeCertContext);
		}

		// Token: 0x06005139 RID: 20793 RVA: 0x00123DF8 File Offset: 0x00122DF8
		public X509Certificate(SerializationInfo info, StreamingContext context)
		{
			byte[] array = (byte[])info.GetValue("RawData", typeof(byte[]));
			if (array != null)
			{
				this.LoadCertificateFromBlob(array, null, X509KeyStorageFlags.DefaultKeySet, false);
			}
		}

		// Token: 0x0600513A RID: 20794 RVA: 0x00123E3E File Offset: 0x00122E3E
		public static X509Certificate CreateFromCertFile(string filename)
		{
			return new X509Certificate(filename);
		}

		// Token: 0x0600513B RID: 20795 RVA: 0x00123E46 File Offset: 0x00122E46
		public static X509Certificate CreateFromSignedFile(string filename)
		{
			return new X509Certificate(filename);
		}

		// Token: 0x17000E23 RID: 3619
		// (get) Token: 0x0600513C RID: 20796 RVA: 0x00123E4E File Offset: 0x00122E4E
		[ComVisible(false)]
		public IntPtr Handle
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return this.m_safeCertContext.pCertContext;
			}
		}

		// Token: 0x0600513D RID: 20797 RVA: 0x00123E5B File Offset: 0x00122E5B
		[Obsolete("This method has been deprecated.  Please use the Subject property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetName()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			return X509Utils._GetSubjectInfo(this.m_safeCertContext, 2U, true);
		}

		// Token: 0x0600513E RID: 20798 RVA: 0x00123E8C File Offset: 0x00122E8C
		[Obsolete("This method has been deprecated.  Please use the Issuer property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual string GetIssuerName()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			return X509Utils._GetIssuerName(this.m_safeCertContext, true);
		}

		// Token: 0x0600513F RID: 20799 RVA: 0x00123EBC File Offset: 0x00122EBC
		public virtual byte[] GetSerialNumber()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			if (this.m_serialNumber == null)
			{
				this.m_serialNumber = X509Utils._GetSerialNumber(this.m_safeCertContext);
			}
			return (byte[])this.m_serialNumber.Clone();
		}

		// Token: 0x06005140 RID: 20800 RVA: 0x00123F14 File Offset: 0x00122F14
		public virtual string GetSerialNumberString()
		{
			return this.SerialNumber;
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x00123F1C File Offset: 0x00122F1C
		public virtual byte[] GetKeyAlgorithmParameters()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			if (this.m_publicKeyParameters == null)
			{
				this.m_publicKeyParameters = X509Utils._GetPublicKeyParameters(this.m_safeCertContext);
			}
			return (byte[])this.m_publicKeyParameters.Clone();
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x00123F74 File Offset: 0x00122F74
		public virtual string GetKeyAlgorithmParametersString()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			return Hex.EncodeHexString(this.GetKeyAlgorithmParameters());
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x00123FA4 File Offset: 0x00122FA4
		public virtual string GetKeyAlgorithm()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			if (this.m_publicKeyOid == null)
			{
				this.m_publicKeyOid = X509Utils._GetPublicKeyOid(this.m_safeCertContext);
			}
			return this.m_publicKeyOid;
		}

		// Token: 0x06005144 RID: 20804 RVA: 0x00123FF4 File Offset: 0x00122FF4
		public virtual byte[] GetPublicKey()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			if (this.m_publicKeyValue == null)
			{
				this.m_publicKeyValue = X509Utils._GetPublicKeyValue(this.m_safeCertContext);
			}
			return (byte[])this.m_publicKeyValue.Clone();
		}

		// Token: 0x06005145 RID: 20805 RVA: 0x0012404C File Offset: 0x0012304C
		public virtual string GetPublicKeyString()
		{
			return Hex.EncodeHexString(this.GetPublicKey());
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x00124059 File Offset: 0x00123059
		public virtual byte[] GetRawCertData()
		{
			return this.RawData;
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x00124061 File Offset: 0x00123061
		public virtual string GetRawCertDataString()
		{
			return Hex.EncodeHexString(this.GetRawCertData());
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x0012406E File Offset: 0x0012306E
		public virtual byte[] GetCertHash()
		{
			this.SetThumbprint();
			return (byte[])this.m_thumbprint.Clone();
		}

		// Token: 0x06005149 RID: 20809 RVA: 0x00124086 File Offset: 0x00123086
		public virtual string GetCertHashString()
		{
			this.SetThumbprint();
			return Hex.EncodeHexString(this.m_thumbprint);
		}

		// Token: 0x0600514A RID: 20810 RVA: 0x0012409C File Offset: 0x0012309C
		public virtual string GetEffectiveDateString()
		{
			return this.NotBefore.ToString();
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x001240C0 File Offset: 0x001230C0
		public virtual string GetExpirationDateString()
		{
			return this.NotAfter.ToString();
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x001240E4 File Offset: 0x001230E4
		[ComVisible(false)]
		public override bool Equals(object obj)
		{
			if (!(obj is X509Certificate))
			{
				return false;
			}
			X509Certificate x509Certificate = (X509Certificate)obj;
			return this.Equals(x509Certificate);
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x0012410C File Offset: 0x0012310C
		public virtual bool Equals(X509Certificate other)
		{
			if (other == null)
			{
				return false;
			}
			if (this.m_safeCertContext.IsInvalid)
			{
				return other.m_safeCertContext.IsInvalid;
			}
			return this.Issuer.Equals(other.Issuer) && this.SerialNumber.Equals(other.SerialNumber);
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x00124164 File Offset: 0x00123164
		public override int GetHashCode()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				return 0;
			}
			this.SetThumbprint();
			int num = 0;
			int num2 = 0;
			while (num2 < this.m_thumbprint.Length && num2 < 4)
			{
				num = (num << 8) | (int)this.m_thumbprint[num2];
				num2++;
			}
			return num;
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001241AD File Offset: 0x001231AD
		public override string ToString()
		{
			return this.ToString(false);
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x001241B8 File Offset: 0x001231B8
		public virtual string ToString(bool fVerbose)
		{
			if (!fVerbose || this.m_safeCertContext.IsInvalid)
			{
				return base.GetType().FullName;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[Subject]" + Environment.NewLine + "  ");
			stringBuilder.Append(this.Subject);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Issuer]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.Issuer);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Serial Number]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.SerialNumber);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Not Before]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.NotBefore);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Not After]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.NotAfter);
			stringBuilder.Append(string.Concat(new string[]
			{
				Environment.NewLine,
				Environment.NewLine,
				"[Thumbprint]",
				Environment.NewLine,
				"  "
			}));
			stringBuilder.Append(this.GetCertHashString());
			stringBuilder.Append(Environment.NewLine);
			return stringBuilder.ToString();
		}

		// Token: 0x06005151 RID: 20817 RVA: 0x001243A6 File Offset: 0x001233A6
		public virtual string GetFormat()
		{
			return "X509";
		}

		// Token: 0x17000E24 RID: 3620
		// (get) Token: 0x06005152 RID: 20818 RVA: 0x001243B0 File Offset: 0x001233B0
		public string Issuer
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_issuerName == null)
				{
					this.m_issuerName = X509Utils._GetIssuerName(this.m_safeCertContext, false);
				}
				return this.m_issuerName;
			}
		}

		// Token: 0x17000E25 RID: 3621
		// (get) Token: 0x06005153 RID: 20819 RVA: 0x00124400 File Offset: 0x00123400
		public string Subject
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_subjectName == null)
				{
					this.m_subjectName = X509Utils._GetSubjectInfo(this.m_safeCertContext, 2U, false);
				}
				return this.m_subjectName;
			}
		}

		// Token: 0x06005154 RID: 20820 RVA: 0x00124450 File Offset: 0x00123450
		[ComVisible(false)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public virtual void Import(byte[] rawData)
		{
			this.Reset();
			this.LoadCertificateFromBlob(rawData, null, X509KeyStorageFlags.DefaultKeySet, false);
		}

		// Token: 0x06005155 RID: 20821 RVA: 0x00124462 File Offset: 0x00123462
		[ComVisible(false)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public virtual void Import(byte[] rawData, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			this.LoadCertificateFromBlob(rawData, password, keyStorageFlags, true);
		}

		// Token: 0x06005156 RID: 20822 RVA: 0x00124474 File Offset: 0x00123474
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public virtual void Import(byte[] rawData, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			this.LoadCertificateFromBlob(rawData, password, keyStorageFlags, true);
		}

		// Token: 0x06005157 RID: 20823 RVA: 0x00124486 File Offset: 0x00123486
		[ComVisible(false)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public virtual void Import(string fileName)
		{
			this.Reset();
			this.LoadCertificateFromFile(fileName, null, X509KeyStorageFlags.DefaultKeySet);
		}

		// Token: 0x06005158 RID: 20824 RVA: 0x00124497 File Offset: 0x00123497
		[ComVisible(false)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public virtual void Import(string fileName, string password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			this.LoadCertificateFromFile(fileName, password, keyStorageFlags);
		}

		// Token: 0x06005159 RID: 20825 RVA: 0x001244A8 File Offset: 0x001234A8
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public virtual void Import(string fileName, SecureString password, X509KeyStorageFlags keyStorageFlags)
		{
			this.Reset();
			this.LoadCertificateFromFile(fileName, password, keyStorageFlags);
		}

		// Token: 0x0600515A RID: 20826 RVA: 0x001244B9 File Offset: 0x001234B9
		[ComVisible(false)]
		public virtual byte[] Export(X509ContentType contentType)
		{
			return this.ExportHelper(contentType, null);
		}

		// Token: 0x0600515B RID: 20827 RVA: 0x001244C3 File Offset: 0x001234C3
		[ComVisible(false)]
		public virtual byte[] Export(X509ContentType contentType, string password)
		{
			return this.ExportHelper(contentType, password);
		}

		// Token: 0x0600515C RID: 20828 RVA: 0x001244CD File Offset: 0x001234CD
		public virtual byte[] Export(X509ContentType contentType, SecureString password)
		{
			return this.ExportHelper(contentType, password);
		}

		// Token: 0x0600515D RID: 20829 RVA: 0x001244D8 File Offset: 0x001234D8
		[ComVisible(false)]
		[PermissionSet(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[PermissionSet(SecurityAction.LinkDemand, Unrestricted = true)]
		public virtual void Reset()
		{
			this.m_subjectName = null;
			this.m_issuerName = null;
			this.m_serialNumber = null;
			this.m_publicKeyParameters = null;
			this.m_publicKeyValue = null;
			this.m_publicKeyOid = null;
			this.m_rawData = null;
			this.m_thumbprint = null;
			this.m_notBefore = DateTime.MinValue;
			this.m_notAfter = DateTime.MinValue;
			if (!this.m_safeCertContext.IsInvalid)
			{
				this.m_safeCertContext.Dispose();
				this.m_safeCertContext = SafeCertContextHandle.InvalidHandle;
			}
		}

		// Token: 0x0600515E RID: 20830 RVA: 0x00124556 File Offset: 0x00123556
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				info.AddValue("RawData", null);
				return;
			}
			info.AddValue("RawData", this.RawData);
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x00124583 File Offset: 0x00123583
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		// Token: 0x17000E26 RID: 3622
		// (get) Token: 0x06005160 RID: 20832 RVA: 0x00124585 File Offset: 0x00123585
		internal SafeCertContextHandle CertContext
		{
			get
			{
				return this.m_safeCertContext;
			}
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06005161 RID: 20833 RVA: 0x00124590 File Offset: 0x00123590
		private DateTime NotAfter
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_notAfter == DateTime.MinValue)
				{
					Win32Native.FILE_TIME file_TIME = default(Win32Native.FILE_TIME);
					X509Utils._GetDateNotAfter(this.m_safeCertContext, ref file_TIME);
					this.m_notAfter = DateTime.FromFileTime(file_TIME.ToTicks());
				}
				return this.m_notAfter;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06005162 RID: 20834 RVA: 0x00124600 File Offset: 0x00123600
		private DateTime NotBefore
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_notBefore == DateTime.MinValue)
				{
					Win32Native.FILE_TIME file_TIME = default(Win32Native.FILE_TIME);
					X509Utils._GetDateNotBefore(this.m_safeCertContext, ref file_TIME);
					this.m_notBefore = DateTime.FromFileTime(file_TIME.ToTicks());
				}
				return this.m_notBefore;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06005163 RID: 20835 RVA: 0x00124670 File Offset: 0x00123670
		private byte[] RawData
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_rawData == null)
				{
					this.m_rawData = X509Utils._GetCertRawData(this.m_safeCertContext);
				}
				return (byte[])this.m_rawData.Clone();
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06005164 RID: 20836 RVA: 0x001246C8 File Offset: 0x001236C8
		private string SerialNumber
		{
			get
			{
				if (this.m_safeCertContext.IsInvalid)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
				}
				if (this.m_serialNumber == null)
				{
					this.m_serialNumber = X509Utils._GetSerialNumber(this.m_safeCertContext);
				}
				return Hex.EncodeHexStringFromInt(this.m_serialNumber);
			}
		}

		// Token: 0x06005165 RID: 20837 RVA: 0x0012471B File Offset: 0x0012371B
		private void SetThumbprint()
		{
			if (this.m_safeCertContext.IsInvalid)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidHandle"), "m_safeCertContext");
			}
			if (this.m_thumbprint == null)
			{
				this.m_thumbprint = X509Utils._GetThumbprint(this.m_safeCertContext);
			}
		}

		// Token: 0x06005166 RID: 20838 RVA: 0x00124758 File Offset: 0x00123758
		private byte[] ExportHelper(X509ContentType contentType, object password)
		{
			switch (contentType)
			{
			case X509ContentType.Cert:
			case X509ContentType.SerializedCert:
				break;
			case X509ContentType.Pfx:
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.Open | KeyContainerPermissionFlags.Export);
				keyContainerPermission.Demand();
				break;
			}
			default:
				throw new CryptographicException(Environment.GetResourceString("Cryptography_X509_InvalidContentType"));
			}
			IntPtr intPtr = IntPtr.Zero;
			byte[] array = null;
			SafeCertStoreHandle safeCertStoreHandle = X509Utils.ExportCertToMemoryStore(this);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				intPtr = X509Utils.PasswordToCoTaskMemUni(password);
				array = X509Utils._ExportCertificatesToBlob(safeCertStoreHandle, contentType, intPtr);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.ZeroFreeCoTaskMemUnicode(intPtr);
				}
				safeCertStoreHandle.Dispose();
			}
			if (array == null)
			{
				throw new CryptographicException(Environment.GetResourceString("Cryptography_X509_ExportFailed"));
			}
			return array;
		}

		// Token: 0x06005167 RID: 20839 RVA: 0x00124804 File Offset: 0x00123804
		private void LoadCertificateFromBlob(byte[] rawData, object password, X509KeyStorageFlags keyStorageFlags, bool passwordProvided)
		{
			if (rawData == null || rawData.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EmptyOrNullArray"), "rawData");
			}
			X509ContentType x509ContentType = X509Utils.MapContentType(X509Utils._QueryCertBlobType(rawData));
			if (x509ContentType == X509ContentType.Pfx && (keyStorageFlags & X509KeyStorageFlags.PersistKeySet) == X509KeyStorageFlags.PersistKeySet)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.Create);
				keyContainerPermission.Demand();
			}
			if (x509ContentType == X509ContentType.Pfx)
			{
				X509Certificate.EnforceIterationCountLimit(rawData, false, passwordProvided);
			}
			uint num = X509Utils.MapKeyStorageFlags(keyStorageFlags);
			IntPtr intPtr = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				intPtr = X509Utils.PasswordToCoTaskMemUni(password);
				X509Utils._LoadCertFromBlob(rawData, intPtr, num, (keyStorageFlags & X509KeyStorageFlags.PersistKeySet) != X509KeyStorageFlags.DefaultKeySet, ref this.m_safeCertContext);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.ZeroFreeCoTaskMemUnicode(intPtr);
				}
			}
		}

		// Token: 0x06005168 RID: 20840 RVA: 0x001248B8 File Offset: 0x001238B8
		private void LoadCertificateFromFile(string fileName, object password, X509KeyStorageFlags keyStorageFlags)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}
			string fullPathInternal = Path.GetFullPathInternal(fileName);
			new FileIOPermission(FileIOPermissionAccess.Read, fullPathInternal).Demand();
			X509ContentType x509ContentType = X509Utils.MapContentType(X509Utils._QueryCertFileType(fileName));
			if (x509ContentType == X509ContentType.Pfx && (keyStorageFlags & X509KeyStorageFlags.PersistKeySet) == X509KeyStorageFlags.PersistKeySet)
			{
				KeyContainerPermission keyContainerPermission = new KeyContainerPermission(KeyContainerPermissionFlags.Create);
				keyContainerPermission.Demand();
			}
			uint num = X509Utils.MapKeyStorageFlags(keyStorageFlags);
			IntPtr intPtr = IntPtr.Zero;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				intPtr = X509Utils.PasswordToCoTaskMemUni(password);
				X509Utils._LoadCertFromFile(fileName, intPtr, num, (keyStorageFlags & X509KeyStorageFlags.PersistKeySet) != X509KeyStorageFlags.DefaultKeySet, ref this.m_safeCertContext);
			}
			finally
			{
				if (intPtr != IntPtr.Zero)
				{
					Marshal.ZeroFreeCoTaskMemUnicode(intPtr);
				}
			}
		}

		// Token: 0x06005169 RID: 20841 RVA: 0x00124968 File Offset: 0x00123968
		[SecurityTreatAsSafe]
		[SecurityCritical]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		private static long ReadSecuritySwitch()
		{
			long num = 0L;
			string environmentVariable = Environment.GetEnvironmentVariable("COMPlus_Pkcs12UnspecifiedPasswordIterationLimit");
			if (environmentVariable != null && long.TryParse(environmentVariable, out num))
			{
				return num;
			}
			if (X509Certificate.ReadSettingsFromRegistry(Registry.CurrentUser, ref num))
			{
				return num;
			}
			if (X509Certificate.ReadSettingsFromRegistry(Registry.LocalMachine, ref num))
			{
				return num;
			}
			return 600000L;
		}

		// Token: 0x0600516A RID: 20842 RVA: 0x001249B8 File Offset: 0x001239B8
		[SecurityTreatAsSafe]
		[SecurityCritical]
		[RegistryPermission(SecurityAction.Assert, Unrestricted = true)]
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		private static bool ReadSettingsFromRegistry(RegistryKey regKey, ref long value)
		{
			try
			{
				using (RegistryKey registryKey = regKey.OpenSubKey("SOFTWARE\\Microsoft\\.NETFramework", false))
				{
					if (registryKey != null)
					{
						object value2 = registryKey.GetValue("Pkcs12UnspecifiedPasswordIterationLimit");
						if (value2 != null)
						{
							value = Convert.ToInt64(value2, CultureInfo.InvariantCulture);
							return true;
						}
					}
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0600516B RID: 20843 RVA: 0x00124A28 File Offset: 0x00123A28
		internal static void EnforceIterationCountLimit(byte[] pkcs12, bool readingFromFile, bool passwordProvided)
		{
			if (readingFromFile || passwordProvided)
			{
				return;
			}
			long num = X509Certificate.s_pkcs12UnspecifiedPasswordIterationLimit;
			if (num == -1L)
			{
				return;
			}
			if (num < 0L)
			{
				num = 600000L;
			}
			checked
			{
				try
				{
					try
					{
						KdfWorkLimiter.SetIterationLimit((ulong)num);
						ulong iterationCount = X509Certificate.GetIterationCount(pkcs12);
						if (iterationCount > (ulong)num || KdfWorkLimiter.WasWorkLimitExceeded())
						{
							throw new CryptographicException();
						}
					}
					finally
					{
						KdfWorkLimiter.ResetIterationLimit();
					}
				}
				catch (Exception ex)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_X509_PfxWithoutPassword"), ex);
				}
			}
		}

		// Token: 0x0600516C RID: 20844 RVA: 0x00124AAC File Offset: 0x00123AAC
		private static ulong GetIterationCount(byte[] pkcs12)
		{
			ReadOnlyMemory<byte> readOnlyMemory = new ReadOnlyMemory<byte>(pkcs12);
			AsnValueReader asnValueReader = new AsnValueReader(pkcs12, AsnEncodingRules.BER);
			PfxAsn pfxAsn;
			PfxAsn.Decode(ref asnValueReader, readOnlyMemory, out pfxAsn);
			return pfxAsn.CountTotalIterations();
		}

		// Token: 0x040029C9 RID: 10697
		private const long DefaultPkcs12UnspecifiedPasswordIterationLimit = 600000L;

		// Token: 0x040029CA RID: 10698
		private const string m_format = "X509";

		// Token: 0x040029CB RID: 10699
		private string m_subjectName;

		// Token: 0x040029CC RID: 10700
		private string m_issuerName;

		// Token: 0x040029CD RID: 10701
		private byte[] m_serialNumber;

		// Token: 0x040029CE RID: 10702
		private byte[] m_publicKeyParameters;

		// Token: 0x040029CF RID: 10703
		private byte[] m_publicKeyValue;

		// Token: 0x040029D0 RID: 10704
		private string m_publicKeyOid;

		// Token: 0x040029D1 RID: 10705
		private byte[] m_rawData;

		// Token: 0x040029D2 RID: 10706
		private byte[] m_thumbprint;

		// Token: 0x040029D3 RID: 10707
		private DateTime m_notBefore;

		// Token: 0x040029D4 RID: 10708
		private DateTime m_notAfter;

		// Token: 0x040029D5 RID: 10709
		private SafeCertContextHandle m_safeCertContext = SafeCertContextHandle.InvalidHandle;

		// Token: 0x040029D6 RID: 10710
		private static long s_pkcs12UnspecifiedPasswordIterationLimit = X509Certificate.ReadSecuritySwitch();
	}
}
