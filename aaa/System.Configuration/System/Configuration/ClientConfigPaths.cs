using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using Microsoft.Win32;

namespace System.Configuration
{
	// Token: 0x02000017 RID: 23
	internal class ClientConfigPaths
	{
		// Token: 0x06000106 RID: 262 RVA: 0x0000AB1C File Offset: 0x00009B1C
		[SecurityPermission(SecurityAction.Assert, UnmanagedCode = true)]
		[FileIOPermission(SecurityAction.Assert, AllFiles = FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery)]
		private ClientConfigPaths(string exePath, bool includeUserConfig)
		{
			this._includesUserConfig = includeUserConfig;
			Assembly assembly = null;
			string text = null;
			string text2;
			if (exePath == null)
			{
				AppDomain currentDomain = AppDomain.CurrentDomain;
				AppDomainSetup setupInformation = currentDomain.SetupInformation;
				this._applicationConfigUri = setupInformation.ConfigurationFile;
				assembly = Assembly.GetEntryAssembly();
				if (assembly != null)
				{
					this._hasEntryAssembly = true;
					text2 = assembly.CodeBase;
					bool flag = false;
					if (StringUtil.StartsWithIgnoreCase(text2, "file:///"))
					{
						flag = true;
						text2 = text2.Substring("file:///".Length);
					}
					else if (StringUtil.StartsWithIgnoreCase(text2, "file://"))
					{
						flag = true;
						text2 = text2.Substring("file:".Length);
					}
					if (flag)
					{
						text2 = text2.Replace('/', '\\');
						text = text2;
					}
					else
					{
						text2 = assembly.EscapedCodeBase;
					}
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder(260);
					UnsafeNativeMethods.GetModuleFileName(new HandleRef(null, IntPtr.Zero), stringBuilder, stringBuilder.Capacity);
					text2 = Path.GetFullPath(stringBuilder.ToString());
					text = text2;
				}
			}
			else
			{
				text2 = Path.GetFullPath(exePath);
				if (!FileUtil.FileExists(text2, false))
				{
					throw ExceptionUtil.ParameterInvalid("exePath");
				}
				text = text2;
			}
			if (this._applicationConfigUri == null)
			{
				this._applicationConfigUri = text2 + ".config";
			}
			this._applicationUri = text2;
			if (exePath != null)
			{
				return;
			}
			if (!this._includesUserConfig)
			{
				return;
			}
			bool flag2 = StringUtil.StartsWithIgnoreCase(this._applicationConfigUri, "http://");
			this.SetNamesAndVersion(text, assembly, flag2);
			if (this.IsClickOnceDeployed(AppDomain.CurrentDomain))
			{
				string text3 = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
				string text4 = this.Validate(this._productVersion, false);
				if (Path.IsPathRooted(text3))
				{
					this._localConfigDirectory = this.CombineIfValid(text3, text4);
					this._localConfigFilename = this.CombineIfValid(this._localConfigDirectory, "user.config");
					return;
				}
			}
			else if (!flag2)
			{
				string text5 = this.Validate(this._companyName, true);
				string text6 = this.Validate(AppDomain.CurrentDomain.FriendlyName, true);
				string text7 = ((!string.IsNullOrEmpty(this._applicationUri)) ? this._applicationUri.ToLower(CultureInfo.InvariantCulture) : null);
				string text8 = ((!string.IsNullOrEmpty(text6)) ? text6 : this.Validate(this._productName, true));
				string typeAndHashSuffix = this.GetTypeAndHashSuffix(AppDomain.CurrentDomain, text7);
				string text9 = ((!string.IsNullOrEmpty(text8) && !string.IsNullOrEmpty(typeAndHashSuffix)) ? (text8 + typeAndHashSuffix) : null);
				string text10 = this.Validate(this._productVersion, false);
				string text11 = this.CombineIfValid(this.CombineIfValid(text5, text9), text10);
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
				if (Path.IsPathRooted(folderPath))
				{
					this._roamingConfigDirectory = this.CombineIfValid(folderPath, text11);
					this._roamingConfigFilename = this.CombineIfValid(this._roamingConfigDirectory, "user.config");
				}
				string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				if (Path.IsPathRooted(folderPath2))
				{
					this._localConfigDirectory = this.CombineIfValid(folderPath2, text11);
					this._localConfigFilename = this.CombineIfValid(this._localConfigDirectory, "user.config");
				}
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000AE08 File Offset: 0x00009E08
		internal static ClientConfigPaths GetPaths(string exePath, bool includeUserConfig)
		{
			ClientConfigPaths clientConfigPaths;
			if (exePath == null)
			{
				if (ClientConfigPaths.s_current == null || (includeUserConfig && !ClientConfigPaths.s_currentIncludesUserConfig))
				{
					ClientConfigPaths.s_current = new ClientConfigPaths(null, includeUserConfig);
					ClientConfigPaths.s_currentIncludesUserConfig = includeUserConfig;
				}
				clientConfigPaths = ClientConfigPaths.s_current;
			}
			else
			{
				clientConfigPaths = new ClientConfigPaths(exePath, includeUserConfig);
			}
			return clientConfigPaths;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000AE58 File Offset: 0x00009E58
		internal static void RefreshCurrent()
		{
			ClientConfigPaths.s_currentIncludesUserConfig = false;
			ClientConfigPaths.s_current = null;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000109 RID: 265 RVA: 0x0000AE6A File Offset: 0x00009E6A
		internal static ClientConfigPaths Current
		{
			get
			{
				return ClientConfigPaths.GetPaths(null, true);
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600010A RID: 266 RVA: 0x0000AE73 File Offset: 0x00009E73
		internal bool HasEntryAssembly
		{
			get
			{
				return this._hasEntryAssembly;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600010B RID: 267 RVA: 0x0000AE7B File Offset: 0x00009E7B
		internal string ApplicationUri
		{
			get
			{
				return this._applicationUri;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600010C RID: 268 RVA: 0x0000AE83 File Offset: 0x00009E83
		internal string ApplicationConfigUri
		{
			get
			{
				return this._applicationConfigUri;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600010D RID: 269 RVA: 0x0000AE8B File Offset: 0x00009E8B
		internal string RoamingConfigFilename
		{
			get
			{
				return this._roamingConfigFilename;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000AE93 File Offset: 0x00009E93
		internal string RoamingConfigDirectory
		{
			get
			{
				return this._roamingConfigDirectory;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600010F RID: 271 RVA: 0x0000AE9B File Offset: 0x00009E9B
		internal bool HasRoamingConfig
		{
			get
			{
				return this.RoamingConfigFilename != null || !this._includesUserConfig;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000AEB0 File Offset: 0x00009EB0
		internal string LocalConfigFilename
		{
			get
			{
				return this._localConfigFilename;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000111 RID: 273 RVA: 0x0000AEB8 File Offset: 0x00009EB8
		internal string LocalConfigDirectory
		{
			get
			{
				return this._localConfigDirectory;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000112 RID: 274 RVA: 0x0000AEC0 File Offset: 0x00009EC0
		internal bool HasLocalConfig
		{
			get
			{
				return this.LocalConfigFilename != null || !this._includesUserConfig;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000113 RID: 275 RVA: 0x0000AED5 File Offset: 0x00009ED5
		internal string ProductName
		{
			get
			{
				return this._productName;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000114 RID: 276 RVA: 0x0000AEDD File Offset: 0x00009EDD
		internal string ProductVersion
		{
			get
			{
				return this._productVersion;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000AEE5 File Offset: 0x00009EE5
		private static SecurityPermission ControlEvidencePermission
		{
			get
			{
				if (ClientConfigPaths.s_controlEvidencePerm == null)
				{
					ClientConfigPaths.s_controlEvidencePerm = new SecurityPermission(SecurityPermissionFlag.ControlEvidence);
				}
				return ClientConfigPaths.s_controlEvidencePerm;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000116 RID: 278 RVA: 0x0000AEFF File Offset: 0x00009EFF
		private static SecurityPermission SerializationFormatterPermission
		{
			get
			{
				if (ClientConfigPaths.s_serializationPerm == null)
				{
					ClientConfigPaths.s_serializationPerm = new SecurityPermission(SecurityPermissionFlag.SerializationFormatter);
				}
				return ClientConfigPaths.s_serializationPerm;
			}
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000AF1C File Offset: 0x00009F1C
		private string CombineIfValid(string path1, string path2)
		{
			string text = null;
			if (path1 != null && path2 != null)
			{
				try
				{
					string text2 = Path.Combine(path1, path2);
					if (text2.Length < 260)
					{
						text = text2;
					}
				}
				catch
				{
				}
			}
			return text;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x0000AF60 File Offset: 0x00009F60
		private string GetTypeAndHashSuffix(AppDomain appDomain, string exePath)
		{
			string text = null;
			string text2 = null;
			object evidenceInfo = ClientConfigPaths.GetEvidenceInfo(appDomain, exePath, out text2);
			if (evidenceInfo != null && !string.IsNullOrEmpty(text2))
			{
				MemoryStream memoryStream = new MemoryStream();
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				ClientConfigPaths.SerializationFormatterPermission.Assert();
				binaryFormatter.Serialize(memoryStream, evidenceInfo);
				memoryStream.Position = 0L;
				string hash = ClientConfigPaths.GetHash(memoryStream);
				if (!string.IsNullOrEmpty(hash))
				{
					text = "_" + text2 + "_" + hash;
				}
			}
			return text;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000AFD8 File Offset: 0x00009FD8
		private static object GetEvidenceInfo(AppDomain appDomain, string exePath, out string typeName)
		{
			ClientConfigPaths.ControlEvidencePermission.Assert();
			Evidence evidence = appDomain.Evidence;
			StrongName strongName = null;
			Url url = null;
			if (evidence != null)
			{
				IEnumerator hostEnumerator = evidence.GetHostEnumerator();
				while (hostEnumerator.MoveNext())
				{
					object obj = hostEnumerator.Current;
					if (obj is StrongName)
					{
						strongName = (StrongName)obj;
						break;
					}
					if (obj is Url)
					{
						url = (Url)obj;
					}
				}
			}
			object obj2 = null;
			if (strongName != null)
			{
				obj2 = ClientConfigPaths.MakeVersionIndependent(strongName);
				typeName = "StrongName";
			}
			else if (url != null)
			{
				obj2 = url.Value.ToUpperInvariant();
				typeName = "Url";
			}
			else if (exePath != null)
			{
				obj2 = exePath;
				typeName = "Path";
			}
			else
			{
				typeName = null;
			}
			return obj2;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000B084 File Offset: 0x0000A084
		private static string GetHash(Stream s)
		{
			byte[] array;
			using (SHA1 sha = new SHA1CryptoServiceProvider())
			{
				array = sha.ComputeHash(s);
			}
			return ClientConfigPaths.ToBase32StringSuitableForDirName(array);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000B0C4 File Offset: 0x0000A0C4
		private bool IsClickOnceDeployed(AppDomain appDomain)
		{
			ActivationContext activationContext = appDomain.ActivationContext;
			if (activationContext != null && activationContext.Form == ActivationContext.ContextForm.StoreBounded)
			{
				string fullName = activationContext.Identity.FullName;
				if (!string.IsNullOrEmpty(fullName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000B0FB File Offset: 0x0000A0FB
		private static StrongName MakeVersionIndependent(StrongName sn)
		{
			return new StrongName(sn.PublicKey, sn.Name, new Version(0, 0, 0, 0));
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000B118 File Offset: 0x0000A118
		private void SetNamesAndVersion(string applicationFilename, Assembly exeAssembly, bool isHttp)
		{
			Type type = null;
			if (exeAssembly != null)
			{
				object[] array = exeAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (array != null && array.Length > 0)
				{
					this._companyName = ((AssemblyCompanyAttribute)array[0]).Company;
					if (this._companyName != null)
					{
						this._companyName = this._companyName.Trim();
					}
				}
				array = exeAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (array != null && array.Length > 0)
				{
					this._productName = ((AssemblyProductAttribute)array[0]).Product;
					if (this._productName != null)
					{
						this._productName = this._productName.Trim();
					}
				}
				this._productVersion = exeAssembly.GetName().Version.ToString();
				if (this._productVersion != null)
				{
					this._productVersion = this._productVersion.Trim();
				}
			}
			if (!isHttp && (string.IsNullOrEmpty(this._companyName) || string.IsNullOrEmpty(this._productName) || string.IsNullOrEmpty(this._productVersion)))
			{
				string text = null;
				if (exeAssembly != null)
				{
					MethodInfo entryPoint = exeAssembly.EntryPoint;
					if (entryPoint != null)
					{
						type = entryPoint.ReflectedType;
						if (type != null)
						{
							text = type.Module.FullyQualifiedName;
						}
					}
				}
				if (text == null)
				{
					text = applicationFilename;
				}
				if (text != null)
				{
					FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(text);
					if (versionInfo != null)
					{
						if (string.IsNullOrEmpty(this._companyName))
						{
							this._companyName = versionInfo.CompanyName;
							if (this._companyName != null)
							{
								this._companyName = this._companyName.Trim();
							}
						}
						if (string.IsNullOrEmpty(this._productName))
						{
							this._productName = versionInfo.ProductName;
							if (this._productName != null)
							{
								this._productName = this._productName.Trim();
							}
						}
						if (string.IsNullOrEmpty(this._productVersion))
						{
							this._productVersion = versionInfo.ProductVersion;
							if (this._productVersion != null)
							{
								this._productVersion = this._productVersion.Trim();
							}
						}
					}
				}
			}
			if (string.IsNullOrEmpty(this._companyName) || string.IsNullOrEmpty(this._productName))
			{
				string text2 = null;
				if (type != null)
				{
					text2 = type.Namespace;
				}
				if (string.IsNullOrEmpty(this._productName))
				{
					if (text2 != null)
					{
						int num = text2.LastIndexOf(".", StringComparison.Ordinal);
						if (num != -1 && num < text2.Length - 1)
						{
							this._productName = text2.Substring(num + 1);
						}
						else
						{
							this._productName = text2;
						}
						this._productName = this._productName.Trim();
					}
					if (string.IsNullOrEmpty(this._productName) && type != null)
					{
						this._productName = type.Name.Trim();
					}
					if (this._productName == null)
					{
						this._productName = string.Empty;
					}
				}
				if (string.IsNullOrEmpty(this._companyName))
				{
					if (text2 != null)
					{
						int num2 = text2.IndexOf(".", StringComparison.Ordinal);
						if (num2 != -1)
						{
							this._companyName = text2.Substring(0, num2);
						}
						else
						{
							this._companyName = text2;
						}
						this._companyName = this._companyName.Trim();
					}
					if (string.IsNullOrEmpty(this._companyName))
					{
						this._companyName = this._productName;
					}
				}
			}
			if (string.IsNullOrEmpty(this._productVersion))
			{
				this._productVersion = "1.0.0.0";
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000B434 File Offset: 0x0000A434
		private static string ToBase32StringSuitableForDirName(byte[] buff)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = buff.Length;
			int num2 = 0;
			do
			{
				byte b = ((num2 < num) ? buff[num2++] : 0);
				byte b2 = ((num2 < num) ? buff[num2++] : 0);
				byte b3 = ((num2 < num) ? buff[num2++] : 0);
				byte b4 = ((num2 < num) ? buff[num2++] : 0);
				byte b5 = ((num2 < num) ? buff[num2++] : 0);
				stringBuilder.Append(ClientConfigPaths.s_Base32Char[(int)(b & 31)]);
				stringBuilder.Append(ClientConfigPaths.s_Base32Char[(int)(b2 & 31)]);
				stringBuilder.Append(ClientConfigPaths.s_Base32Char[(int)(b3 & 31)]);
				stringBuilder.Append(ClientConfigPaths.s_Base32Char[(int)(b4 & 31)]);
				stringBuilder.Append(ClientConfigPaths.s_Base32Char[(int)(b5 & 31)]);
				stringBuilder.Append(ClientConfigPaths.s_Base32Char[((b & 224) >> 5) | ((b4 & 96) >> 2)]);
				stringBuilder.Append(ClientConfigPaths.s_Base32Char[((b2 & 224) >> 5) | ((b5 & 96) >> 2)]);
				b3 = (byte)(b3 >> 5);
				if ((b4 & 128) != 0)
				{
					b3 |= 8;
				}
				if ((b5 & 128) != 0)
				{
					b3 |= 16;
				}
				stringBuilder.Append(ClientConfigPaths.s_Base32Char[(int)b3]);
			}
			while (num2 < num);
			return stringBuilder.ToString();
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000B584 File Offset: 0x0000A584
		private string Validate(string str, bool limitSize)
		{
			string text = str;
			if (!string.IsNullOrEmpty(text))
			{
				foreach (char c in Path.GetInvalidFileNameChars())
				{
					text = text.Replace(c, '_');
				}
				text = text.Replace(' ', '_');
				if (limitSize)
				{
					text = ((text.Length > 25) ? text.Substring(0, 25) : text);
				}
			}
			return text;
		}

		// Token: 0x040001D5 RID: 469
		internal const string UserConfigFilename = "user.config";

		// Token: 0x040001D6 RID: 470
		private const string ClickOnceDataDirectory = "DataDirectory";

		// Token: 0x040001D7 RID: 471
		private const string ConfigExtension = ".config";

		// Token: 0x040001D8 RID: 472
		private const int MAX_PATH = 260;

		// Token: 0x040001D9 RID: 473
		private const int MAX_LENGTH_TO_USE = 25;

		// Token: 0x040001DA RID: 474
		private const string FILE_URI_LOCAL = "file:///";

		// Token: 0x040001DB RID: 475
		private const string FILE_URI_UNC = "file://";

		// Token: 0x040001DC RID: 476
		private const string FILE_URI = "file:";

		// Token: 0x040001DD RID: 477
		private const string HTTP_URI = "http://";

		// Token: 0x040001DE RID: 478
		private const string StrongNameDesc = "StrongName";

		// Token: 0x040001DF RID: 479
		private const string UrlDesc = "Url";

		// Token: 0x040001E0 RID: 480
		private const string PathDesc = "Path";

		// Token: 0x040001E1 RID: 481
		private static char[] s_Base32Char = new char[]
		{
			'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
			'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
			'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3',
			'4', '5'
		};

		// Token: 0x040001E2 RID: 482
		private static volatile ClientConfigPaths s_current;

		// Token: 0x040001E3 RID: 483
		private static volatile bool s_currentIncludesUserConfig;

		// Token: 0x040001E4 RID: 484
		private static SecurityPermission s_serializationPerm;

		// Token: 0x040001E5 RID: 485
		private static SecurityPermission s_controlEvidencePerm;

		// Token: 0x040001E6 RID: 486
		private bool _hasEntryAssembly;

		// Token: 0x040001E7 RID: 487
		private bool _includesUserConfig;

		// Token: 0x040001E8 RID: 488
		private string _applicationUri;

		// Token: 0x040001E9 RID: 489
		private string _applicationConfigUri;

		// Token: 0x040001EA RID: 490
		private string _roamingConfigDirectory;

		// Token: 0x040001EB RID: 491
		private string _roamingConfigFilename;

		// Token: 0x040001EC RID: 492
		private string _localConfigDirectory;

		// Token: 0x040001ED RID: 493
		private string _localConfigFilename;

		// Token: 0x040001EE RID: 494
		private string _companyName;

		// Token: 0x040001EF RID: 495
		private string _productName;

		// Token: 0x040001F0 RID: 496
		private string _productVersion;
	}
}
