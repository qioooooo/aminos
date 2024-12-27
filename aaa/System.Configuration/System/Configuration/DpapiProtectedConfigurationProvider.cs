using System;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Xml;
using Microsoft.Win32;

namespace System.Configuration
{
	// Token: 0x0200005F RID: 95
	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	public sealed class DpapiProtectedConfigurationProvider : ProtectedConfigurationProvider
	{
		// Token: 0x0600039F RID: 927 RVA: 0x000129BC File Offset: 0x000119BC
		public override XmlNode Decrypt(XmlNode encryptedNode)
		{
			if (encryptedNode.NodeType != XmlNodeType.Element || encryptedNode.Name != "EncryptedData")
			{
				throw new ConfigurationErrorsException(SR.GetString("DPAPI_bad_data"));
			}
			XmlNode xmlNode = DpapiProtectedConfigurationProvider.TraverseToChild(encryptedNode, "CipherData", false);
			if (xmlNode == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("DPAPI_bad_data"));
			}
			XmlNode xmlNode2 = DpapiProtectedConfigurationProvider.TraverseToChild(xmlNode, "CipherValue", true);
			if (xmlNode2 == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("DPAPI_bad_data"));
			}
			string innerText = xmlNode2.InnerText;
			if (innerText == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("DPAPI_bad_data"));
			}
			string text = this.DecryptText(innerText);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml(text);
			return xmlDocument.DocumentElement;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00012A74 File Offset: 0x00011A74
		public override XmlNode Encrypt(XmlNode node)
		{
			string outerXml = node.OuterXml;
			string text = this.EncryptText(outerXml);
			string text2 = "<EncryptedData><CipherData><CipherValue>";
			string text3 = "</CipherValue></CipherData></EncryptedData>";
			string text4 = text2 + text + text3;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			xmlDocument.LoadXml(text4);
			return xmlDocument.DocumentElement;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00012AC8 File Offset: 0x00011AC8
		private string EncryptText(string clearText)
		{
			if (clearText == null || clearText.Length < 1)
			{
				return clearText;
			}
			SafeNativeMemoryHandle safeNativeMemoryHandle = new SafeNativeMemoryHandle();
			SafeNativeMemoryHandle safeNativeMemoryHandle2 = new SafeNativeMemoryHandle(true);
			SafeNativeMemoryHandle safeNativeMemoryHandle3 = new SafeNativeMemoryHandle();
			DATA_BLOB data_BLOB;
			DATA_BLOB data_BLOB2;
			DATA_BLOB data_BLOB3;
			data_BLOB.pbData = (data_BLOB2.pbData = (data_BLOB3.pbData = IntPtr.Zero));
			data_BLOB.cbData = (data_BLOB2.cbData = (data_BLOB3.cbData = 0));
			string text;
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					data_BLOB = DpapiProtectedConfigurationProvider.PrepareDataBlob(clearText);
					safeNativeMemoryHandle.SetDataHandle(data_BLOB.pbData);
					data_BLOB2 = DpapiProtectedConfigurationProvider.PrepareDataBlob(this._KeyEntropy);
					safeNativeMemoryHandle3.SetDataHandle(data_BLOB2.pbData);
				}
				CRYPTPROTECT_PROMPTSTRUCT cryptprotect_PROMPTSTRUCT = DpapiProtectedConfigurationProvider.PreparePromptStructure();
				uint num = 1U;
				if (this.UseMachineProtection)
				{
					num |= 4U;
				}
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					flag = UnsafeNativeMethods.CryptProtectData(ref data_BLOB, "", ref data_BLOB2, IntPtr.Zero, ref cryptprotect_PROMPTSTRUCT, num, ref data_BLOB3);
					safeNativeMemoryHandle2.SetDataHandle(data_BLOB3.pbData);
				}
				if (!flag || data_BLOB3.pbData == IntPtr.Zero)
				{
					data_BLOB3.pbData = IntPtr.Zero;
					Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
				}
				byte[] array = new byte[data_BLOB3.cbData];
				Marshal.Copy(data_BLOB3.pbData, array, 0, array.Length);
				text = Convert.ToBase64String(array);
			}
			finally
			{
				if (safeNativeMemoryHandle2 != null && !safeNativeMemoryHandle2.IsInvalid)
				{
					safeNativeMemoryHandle2.Dispose();
					data_BLOB3.pbData = IntPtr.Zero;
				}
				if (safeNativeMemoryHandle3 != null && !safeNativeMemoryHandle3.IsInvalid)
				{
					safeNativeMemoryHandle3.Dispose();
					data_BLOB2.pbData = IntPtr.Zero;
				}
				if (safeNativeMemoryHandle != null && !safeNativeMemoryHandle.IsInvalid)
				{
					safeNativeMemoryHandle.Dispose();
					data_BLOB.pbData = IntPtr.Zero;
				}
			}
			return text;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00012CA0 File Offset: 0x00011CA0
		private string DecryptText(string encText)
		{
			if (encText == null || encText.Length < 1)
			{
				return encText;
			}
			SafeNativeMemoryHandle safeNativeMemoryHandle = new SafeNativeMemoryHandle();
			SafeNativeMemoryHandle safeNativeMemoryHandle2 = new SafeNativeMemoryHandle(true);
			SafeNativeMemoryHandle safeNativeMemoryHandle3 = new SafeNativeMemoryHandle();
			DATA_BLOB data_BLOB;
			DATA_BLOB data_BLOB2;
			DATA_BLOB data_BLOB3;
			data_BLOB.pbData = (data_BLOB2.pbData = (data_BLOB3.pbData = IntPtr.Zero));
			data_BLOB.cbData = (data_BLOB2.cbData = (data_BLOB3.cbData = 0));
			string @string;
			try
			{
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					data_BLOB = DpapiProtectedConfigurationProvider.PrepareDataBlob(Convert.FromBase64String(encText));
					safeNativeMemoryHandle.SetDataHandle(data_BLOB.pbData);
					data_BLOB2 = DpapiProtectedConfigurationProvider.PrepareDataBlob(this._KeyEntropy);
					safeNativeMemoryHandle3.SetDataHandle(data_BLOB2.pbData);
				}
				CRYPTPROTECT_PROMPTSTRUCT cryptprotect_PROMPTSTRUCT = DpapiProtectedConfigurationProvider.PreparePromptStructure();
				uint num = 1U;
				string text = "";
				if (this.UseMachineProtection)
				{
					num |= 4U;
				}
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
				}
				finally
				{
					flag = UnsafeNativeMethods.CryptUnprotectData(ref data_BLOB, ref text, ref data_BLOB2, IntPtr.Zero, ref cryptprotect_PROMPTSTRUCT, num, ref data_BLOB3);
					safeNativeMemoryHandle2.SetDataHandle(data_BLOB3.pbData);
				}
				if (!flag || data_BLOB3.pbData == IntPtr.Zero)
				{
					data_BLOB3.pbData = IntPtr.Zero;
					Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
				}
				byte[] array = new byte[data_BLOB3.cbData];
				Marshal.Copy(data_BLOB3.pbData, array, 0, array.Length);
				@string = Encoding.Unicode.GetString(array);
			}
			finally
			{
				if (safeNativeMemoryHandle2 != null && !safeNativeMemoryHandle2.IsInvalid)
				{
					safeNativeMemoryHandle2.Dispose();
					data_BLOB3.pbData = IntPtr.Zero;
				}
				if (safeNativeMemoryHandle3 != null && !safeNativeMemoryHandle3.IsInvalid)
				{
					safeNativeMemoryHandle3.Dispose();
					data_BLOB2.pbData = IntPtr.Zero;
				}
				if (safeNativeMemoryHandle != null && !safeNativeMemoryHandle.IsInvalid)
				{
					safeNativeMemoryHandle.Dispose();
					data_BLOB.pbData = IntPtr.Zero;
				}
			}
			return @string;
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x00012E88 File Offset: 0x00011E88
		public bool UseMachineProtection
		{
			get
			{
				return this._UseMachineProtection;
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00012E90 File Offset: 0x00011E90
		public override void Initialize(string name, NameValueCollection configurationValues)
		{
			base.Initialize(name, configurationValues);
			this._UseMachineProtection = DpapiProtectedConfigurationProvider.GetBooleanValue(configurationValues, "useMachineProtection", true);
			this._KeyEntropy = configurationValues["keyEntropy"];
			configurationValues.Remove("keyEntropy");
			if (configurationValues.Count > 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Unrecognized_initialization_value", new object[] { configurationValues.GetKey(0) }));
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00012F00 File Offset: 0x00011F00
		private static XmlNode TraverseToChild(XmlNode node, string name, bool onlyChild)
		{
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (xmlNode.Name == name)
					{
						return xmlNode;
					}
					if (onlyChild)
					{
						return null;
					}
				}
			}
			return null;
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00012F78 File Offset: 0x00011F78
		private static DATA_BLOB PrepareDataBlob(byte[] buf)
		{
			if (buf == null)
			{
				buf = new byte[0];
			}
			DATA_BLOB data_BLOB = default(DATA_BLOB);
			data_BLOB.cbData = buf.Length;
			data_BLOB.pbData = Marshal.AllocHGlobal(data_BLOB.cbData);
			Marshal.Copy(buf, 0, data_BLOB.pbData, data_BLOB.cbData);
			return data_BLOB;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00012FCB File Offset: 0x00011FCB
		private static DATA_BLOB PrepareDataBlob(string s)
		{
			return DpapiProtectedConfigurationProvider.PrepareDataBlob((s != null) ? Encoding.Unicode.GetBytes(s) : new byte[0]);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00012FE8 File Offset: 0x00011FE8
		private static CRYPTPROTECT_PROMPTSTRUCT PreparePromptStructure()
		{
			return new CRYPTPROTECT_PROMPTSTRUCT
			{
				cbSize = Marshal.SizeOf(typeof(CRYPTPROTECT_PROMPTSTRUCT)),
				dwPromptFlags = 0,
				hwndApp = IntPtr.Zero,
				szPrompt = null
			};
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00013030 File Offset: 0x00012030
		private static bool GetBooleanValue(NameValueCollection configurationValues, string valueName, bool defaultValue)
		{
			string text = configurationValues[valueName];
			if (text == null)
			{
				return defaultValue;
			}
			configurationValues.Remove(valueName);
			if (text == "true")
			{
				return true;
			}
			if (text == "false")
			{
				return false;
			}
			throw new ConfigurationErrorsException(SR.GetString("Config_invalid_boolean_attribute", new object[] { valueName }));
		}

		// Token: 0x040002EA RID: 746
		private const int CRYPTPROTECT_UI_FORBIDDEN = 1;

		// Token: 0x040002EB RID: 747
		private const int CRYPTPROTECT_LOCAL_MACHINE = 4;

		// Token: 0x040002EC RID: 748
		private bool _UseMachineProtection = true;

		// Token: 0x040002ED RID: 749
		private string _KeyEntropy;
	}
}
