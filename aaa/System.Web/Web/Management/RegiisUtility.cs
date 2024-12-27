using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Web.Configuration;

namespace System.Web.Management
{
	// Token: 0x020002C9 RID: 713
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class RegiisUtility : IRegiisUtility
	{
		// Token: 0x06002495 RID: 9365 RVA: 0x0009C360 File Offset: 0x0009B360
		public void RegisterSystemWebAssembly(int doReg, out IntPtr exception)
		{
			exception = IntPtr.Zero;
			try
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				RegistrationServices registrationServices = new RegistrationServices();
				if (doReg != 0)
				{
					if (!registrationServices.RegisterAssembly(executingAssembly, AssemblyRegistrationFlags.None))
					{
						exception = Marshal.StringToBSTR(new Exception(SR.GetString("Unable_To_Register_Assembly", new object[] { executingAssembly.FullName })).ToString());
					}
				}
				else if (!registrationServices.UnregisterAssembly(executingAssembly))
				{
					exception = Marshal.StringToBSTR(new Exception(SR.GetString("Unable_To_UnRegister_Assembly", new object[] { executingAssembly.FullName })).ToString());
				}
			}
			catch (Exception ex)
			{
				exception = Marshal.StringToBSTR(ex.ToString());
			}
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x0009C428 File Offset: 0x0009B428
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void RegisterAsnetMmcAssembly(int doReg, string typeName, string binaryDirectory, out IntPtr exception)
		{
			exception = IntPtr.Zero;
			try
			{
				Assembly assembly = Assembly.GetAssembly(Type.GetType(typeName, true));
				RegistrationServices registrationServices = new RegistrationServices();
				if (doReg != 0)
				{
					if (!registrationServices.RegisterAssembly(assembly, AssemblyRegistrationFlags.None))
					{
						exception = Marshal.StringToBSTR(new Exception(SR.GetString("Unable_To_Register_Assembly", new object[] { assembly.FullName })).ToString());
					}
					TypeLibConverter typeLibConverter = new TypeLibConverter();
					ConversionEventSink conversionEventSink = new ConversionEventSink();
					RegiisUtility.IRegisterCreateITypeLib registerCreateITypeLib = (RegiisUtility.IRegisterCreateITypeLib)typeLibConverter.ConvertAssemblyToTypeLib(assembly, Path.Combine(binaryDirectory, "AspNetMMCExt.tlb"), TypeLibExporterFlags.None, conversionEventSink);
					registerCreateITypeLib.SaveAllChanges();
				}
				else
				{
					if (!registrationServices.UnregisterAssembly(assembly))
					{
						exception = Marshal.StringToBSTR(new Exception(SR.GetString("Unable_To_UnRegister_Assembly", new object[] { assembly.FullName })).ToString());
					}
					try
					{
						File.Delete(Path.Combine(binaryDirectory, "AspNetMMCExt.tlb"));
					}
					catch
					{
					}
				}
			}
			catch (Exception ex)
			{
				exception = Marshal.StringToBSTR(ex.ToString());
			}
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x0009C54C File Offset: 0x0009B54C
		public void ProtectedConfigAction(long options, string firstArgument, string secondArgument, string providerName, string appPath, string site, string cspOrLocation, int keySize, out IntPtr exception)
		{
			exception = IntPtr.Zero;
			try
			{
				if ((options & 4294967296L) != 0L)
				{
					this.DoProtectSection(firstArgument, providerName, appPath, site, cspOrLocation, (options & 8796093022208L) != 0L);
				}
				else if ((options & 8589934592L) != 0L)
				{
					this.DoUnprotectSection(firstArgument, appPath, site, cspOrLocation, (options & 8796093022208L) != 0L);
				}
				else if ((options & 1125899906842624L) != 0L)
				{
					this.DoProtectSectionFile(firstArgument, secondArgument, providerName);
				}
				else if ((options & 2251799813685248L) != 0L)
				{
					this.DoUnprotectSectionFile(firstArgument, secondArgument);
				}
				else if ((options & 17179869184L) != 0L)
				{
					this.DoKeyCreate(firstArgument, cspOrLocation, options, keySize);
				}
				else if ((options & 34359738368L) != 0L)
				{
					this.DoKeyDelete(firstArgument, cspOrLocation, options);
				}
				else if ((options & 274877906944L) != 0L)
				{
					this.DoKeyExport(firstArgument, secondArgument, cspOrLocation, options);
				}
				else if ((options & 549755813888L) != 0L)
				{
					this.DoKeyImport(firstArgument, secondArgument, cspOrLocation, options);
				}
				else if ((options & 68719476736L) != 0L || (options & 137438953472L) != 0L)
				{
					this.DoKeyAclChange(firstArgument, secondArgument, cspOrLocation, options);
				}
				else
				{
					exception = Marshal.StringToBSTR(SR.GetString("Command_not_recognized"));
				}
			}
			catch (Exception ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				this.GetExceptionMessage(ex, stringBuilder);
				exception = Marshal.StringToBSTR(stringBuilder.ToString());
			}
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x0009C6FC File Offset: 0x0009B6FC
		private void GetExceptionMessage(Exception exception, StringBuilder sb)
		{
			if (sb.Length != 0)
			{
				sb.Append("\n\r");
			}
			if (exception is ConfigurationErrorsException)
			{
				using (IEnumerator enumerator = ((ConfigurationErrorsException)exception).Errors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						ConfigurationErrorsException ex = (ConfigurationErrorsException)obj;
						sb.Append(ex.Message);
						sb.Append("\n\r");
						if (ex.InnerException != null)
						{
							sb.Append("\n\r");
							sb.Append(ex.InnerException.Message);
							sb.Append("\n\r");
						}
					}
					return;
				}
			}
			sb.Append(exception.Message);
			sb.Append("\n\r");
			if (exception.InnerException != null)
			{
				this.GetExceptionMessage(exception.InnerException, sb);
			}
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x0009C7EC File Offset: 0x0009B7EC
		private void DoProtectSection(string configSection, string providerName, string appPath, string site, string location, bool useMachineConfig)
		{
			Configuration configuration;
			ConfigurationSection configSection2 = this.GetConfigSection(configSection, appPath, site, location, useMachineConfig, out configuration);
			if (configSection2 == null)
			{
				throw new Exception(SR.GetString("Configuration_Section_not_found", new object[] { configSection }));
			}
			configSection2.SectionInformation.ProtectSection(providerName);
			configuration.Save();
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x0009C83C File Offset: 0x0009B83C
		private void DoUnprotectSection(string configSection, string appPath, string site, string location, bool useMachineConfig)
		{
			Configuration configuration;
			ConfigurationSection configSection2 = this.GetConfigSection(configSection, appPath, site, location, useMachineConfig, out configuration);
			if (configSection2 == null)
			{
				throw new Exception(SR.GetString("Configuration_Section_not_found", new object[] { configSection }));
			}
			configSection2.SectionInformation.UnprotectSection();
			configuration.Save();
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x0009C88C File Offset: 0x0009B88C
		private void DoProtectSectionFile(string configSection, string dirName, string providerName)
		{
			Configuration configuration;
			ConfigurationSection configSectionFile = this.GetConfigSectionFile(configSection, dirName, out configuration);
			if (configSectionFile == null)
			{
				throw new Exception(SR.GetString("Configuration_Section_not_found", new object[] { configSection }));
			}
			configSectionFile.SectionInformation.ProtectSection(providerName);
			configuration.Save();
		}

		// Token: 0x0600249C RID: 9372 RVA: 0x0009C8D8 File Offset: 0x0009B8D8
		private void DoUnprotectSectionFile(string configSection, string dirName)
		{
			Configuration configuration;
			ConfigurationSection configSectionFile = this.GetConfigSectionFile(configSection, dirName, out configuration);
			if (configSectionFile == null)
			{
				throw new Exception(SR.GetString("Configuration_Section_not_found", new object[] { configSection }));
			}
			configSectionFile.SectionInformation.UnprotectSection();
			configuration.Save();
		}

		// Token: 0x0600249D RID: 9373 RVA: 0x0009C920 File Offset: 0x0009B920
		private ConfigurationSection GetConfigSectionFile(string configSection, string dirName, out Configuration config)
		{
			if (dirName == ".")
			{
				dirName = Environment.CurrentDirectory;
			}
			else
			{
				if (!Path.IsPathRooted(dirName))
				{
					dirName = Path.Combine(Environment.CurrentDirectory, dirName);
				}
				if (!Directory.Exists(dirName))
				{
					throw new Exception(SR.GetString("Configuration_for_physical_path_not_found", new object[] { dirName }));
				}
			}
			WebConfigurationFileMap webConfigurationFileMap = new WebConfigurationFileMap();
			string text = dirName.Replace('\\', '/');
			if (text.Length > 2 && text[1] == ':')
			{
				text = text.Substring(2);
			}
			else if (text.StartsWith("//", StringComparison.Ordinal))
			{
				text = "/";
			}
			webConfigurationFileMap.VirtualDirectories.Add(text, new VirtualDirectoryMapping(dirName, true));
			try
			{
				config = WebConfigurationManager.OpenMappedWebConfiguration(webConfigurationFileMap, text);
			}
			catch (Exception ex)
			{
				throw new Exception(SR.GetString("Configuration_for_physical_path_not_found", new object[] { dirName }), ex);
			}
			return config.GetSection(configSection);
		}

		// Token: 0x0600249E RID: 9374 RVA: 0x0009CA18 File Offset: 0x0009BA18
		private ConfigurationSection GetConfigSection(string configSection, string appPath, string site, string location, bool useMachineConfig, out Configuration config)
		{
			if (string.IsNullOrEmpty(appPath))
			{
				appPath = null;
			}
			if (string.IsNullOrEmpty(location))
			{
				location = null;
			}
			try
			{
				if (useMachineConfig)
				{
					config = WebConfigurationManager.OpenMachineConfiguration(location);
				}
				else
				{
					config = WebConfigurationManager.OpenWebConfiguration(appPath, site, location);
				}
			}
			catch (Exception ex)
			{
				if (useMachineConfig)
				{
					throw new Exception(SR.GetString("Configuration_for_machine_config_not_found"), ex);
				}
				throw new Exception(SR.GetString("Configuration_for_path_not_found", new object[]
				{
					appPath,
					string.IsNullOrEmpty(site) ? SR.GetString("DefaultSiteName") : site
				}), ex);
			}
			return config.GetSection(configSection);
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x0009CAC0 File Offset: 0x0009BAC0
		private void DoKeyCreate(string containerName, string csp, long options, int keySize)
		{
			if (containerName == null || containerName.Length < 1)
			{
				containerName = "NetFrameworkConfigurationKey";
			}
			uint num = (uint)UnsafeNativeMethods.DoesKeyContainerExist(containerName, csp, ((options & 17592186044416L) == 0L) ? 1 : 0);
			uint num2 = num;
			if (num2 == 0U)
			{
				throw new Exception(SR.GetString("RSA_Key_Container_already_exists"));
			}
			if (num2 == 2147942405U)
			{
				throw new Exception(SR.GetString("RSA_Key_Container_access_denied"));
			}
			if (num2 != 2148073494U)
			{
				Marshal.ThrowExceptionForHR((int)num);
				return;
			}
			RsaProtectedConfigurationProvider rsaProtectedConfigurationProvider = this.CreateRSAProvider(containerName, csp, options);
			try
			{
				rsaProtectedConfigurationProvider.AddKey(keySize, (options & 70368744177664L) != 0L);
			}
			catch
			{
				rsaProtectedConfigurationProvider.DeleteKey();
				throw;
			}
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x0009CB7C File Offset: 0x0009BB7C
		private void DoKeyDelete(string containerName, string csp, long options)
		{
			if (containerName == null || containerName.Length < 1)
			{
				containerName = "NetFrameworkConfigurationKey";
			}
			RegiisUtility.MakeSureContainerExists(containerName, csp, (options & 17592186044416L) == 0L);
			RsaProtectedConfigurationProvider rsaProtectedConfigurationProvider = this.CreateRSAProvider(containerName, csp, options);
			rsaProtectedConfigurationProvider.DeleteKey();
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x0009CBC4 File Offset: 0x0009BBC4
		private void DoKeyExport(string containerName, string fileName, string csp, long options)
		{
			if (!Path.IsPathRooted(fileName))
			{
				fileName = Path.Combine(Environment.CurrentDirectory, fileName);
			}
			if (!Directory.Exists(Path.GetDirectoryName(fileName)))
			{
				throw new DirectoryNotFoundException();
			}
			if (containerName == null || containerName.Length < 1)
			{
				containerName = "NetFrameworkConfigurationKey";
			}
			RegiisUtility.MakeSureContainerExists(containerName, csp, (options & 17592186044416L) == 0L);
			RsaProtectedConfigurationProvider rsaProtectedConfigurationProvider = this.CreateRSAProvider(containerName, csp, options);
			rsaProtectedConfigurationProvider.ExportKey(fileName, (options & 281474976710656L) != 0L);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x0009CC48 File Offset: 0x0009BC48
		private void DoKeyImport(string containerName, string fileName, string csp, long options)
		{
			if (!File.Exists(fileName))
			{
				throw new FileNotFoundException();
			}
			if (containerName == null || containerName.Length < 1)
			{
				containerName = "NetFrameworkConfigurationKey";
			}
			RsaProtectedConfigurationProvider rsaProtectedConfigurationProvider = this.CreateRSAProvider(containerName, csp, options);
			rsaProtectedConfigurationProvider.ImportKey(fileName, (options & 70368744177664L) != 0L);
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x0009CC9C File Offset: 0x0009BC9C
		private void DoKeyAclChange(string containerName, string account, string csp, long options)
		{
			if (containerName == null || containerName.Length < 1)
			{
				containerName = "NetFrameworkConfigurationKey";
			}
			RegiisUtility.MakeSureContainerExists(containerName, csp, (options & 17592186044416L) == 0L);
			int num = 0;
			if ((options & 68719476736L) != 0L)
			{
				num |= 1;
			}
			if ((options & 17592186044416L) == 0L)
			{
				num |= 2;
			}
			if ((options & 140737488355328L) != 0L)
			{
				num |= 4;
			}
			int num2 = UnsafeNativeMethods.ChangeAccessToKeyContainer(containerName, account, csp, num);
			if (num2 != 0)
			{
				Marshal.ThrowExceptionForHR(num2);
			}
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x0009CD24 File Offset: 0x0009BD24
		private RsaProtectedConfigurationProvider CreateRSAProvider(string containerName, string csp, long options)
		{
			RsaProtectedConfigurationProvider rsaProtectedConfigurationProvider = new RsaProtectedConfigurationProvider();
			rsaProtectedConfigurationProvider.Initialize("foo", new NameValueCollection
			{
				{ "keyContainerName", containerName },
				{ "cspProviderName", csp },
				{
					"useMachineContainer",
					((options & 17592186044416L) != 0L) ? "false" : "true"
				}
			});
			return rsaProtectedConfigurationProvider;
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x0009CD88 File Offset: 0x0009BD88
		private static void MakeSureContainerExists(string containerName, string csp, bool machineContainer)
		{
			uint num = (uint)UnsafeNativeMethods.DoesKeyContainerExist(containerName, csp, machineContainer ? 1 : 0);
			uint num2 = num;
			if (num2 == 0U)
			{
				return;
			}
			if (num2 == 2147942405U)
			{
				throw new Exception(SR.GetString("RSA_Key_Container_access_denied"));
			}
			if (num2 != 2148073494U)
			{
				Marshal.ThrowExceptionForHR((int)num);
				return;
			}
			throw new Exception(SR.GetString("RSA_Key_Container_not_found"));
		}

		// Token: 0x060024A6 RID: 9382 RVA: 0x0009CDE4 File Offset: 0x0009BDE4
		public void RemoveBrowserCaps(out IntPtr exception)
		{
			try
			{
				BrowserCapabilitiesCodeGenerator browserCapabilitiesCodeGenerator = new BrowserCapabilitiesCodeGenerator();
				browserCapabilitiesCodeGenerator.UninstallInternal();
				exception = IntPtr.Zero;
			}
			catch (Exception ex)
			{
				exception = Marshal.StringToBSTR(ex.Message);
			}
		}

		// Token: 0x04001C56 RID: 7254
		private const int WATSettingLocalOnly = 0;

		// Token: 0x04001C57 RID: 7255
		private const int WATSettingRequireSSL = 1;

		// Token: 0x04001C58 RID: 7256
		private const int WATSettingAuthSettings = 2;

		// Token: 0x04001C59 RID: 7257
		private const int WATSettingAuthMode = 3;

		// Token: 0x04001C5A RID: 7258
		private const int WATSettingMax = 4;

		// Token: 0x04001C5B RID: 7259
		private const int WATValueDoNothing = 0;

		// Token: 0x04001C5C RID: 7260
		private const int WATValueTrue = 1;

		// Token: 0x04001C5D RID: 7261
		private const int WATValueFalse = 2;

		// Token: 0x04001C5E RID: 7262
		private const int WATValueHosted = 3;

		// Token: 0x04001C5F RID: 7263
		private const int WATValueLocal = 4;

		// Token: 0x04001C60 RID: 7264
		private const int WATValueForms = 5;

		// Token: 0x04001C61 RID: 7265
		private const int WATValueWindows = 6;

		// Token: 0x04001C62 RID: 7266
		private const string DefaultRsaKeyContainerName = "NetFrameworkConfigurationKey";

		// Token: 0x04001C63 RID: 7267
		private const string NewLine = "\n\r";

		// Token: 0x04001C64 RID: 7268
		private const long DO_RSA_ENCRYPT = 4294967296L;

		// Token: 0x04001C65 RID: 7269
		private const long DO_RSA_DECRYPT = 8589934592L;

		// Token: 0x04001C66 RID: 7270
		private const long DO_RSA_ADD_KEY = 17179869184L;

		// Token: 0x04001C67 RID: 7271
		private const long DO_RSA_DEL_KEY = 34359738368L;

		// Token: 0x04001C68 RID: 7272
		private const long DO_RSA_ACL_KEY_ADD = 68719476736L;

		// Token: 0x04001C69 RID: 7273
		private const long DO_RSA_ACL_KEY_DEL = 137438953472L;

		// Token: 0x04001C6A RID: 7274
		private const long DO_RSA_EXPORT_KEY = 274877906944L;

		// Token: 0x04001C6B RID: 7275
		private const long DO_RSA_IMPORT_KEY = 549755813888L;

		// Token: 0x04001C6C RID: 7276
		private const long DO_RSA_PKM = 8796093022208L;

		// Token: 0x04001C6D RID: 7277
		private const long DO_RSA_PKU = 17592186044416L;

		// Token: 0x04001C6E RID: 7278
		private const long DO_RSA_EXPORTABLE = 70368744177664L;

		// Token: 0x04001C6F RID: 7279
		private const long DO_RSA_FULL_ACCESS = 140737488355328L;

		// Token: 0x04001C70 RID: 7280
		private const long DO_RSA_PRIVATE = 281474976710656L;

		// Token: 0x04001C71 RID: 7281
		private const long DO_RSA_ENCRYPT_FILE = 1125899906842624L;

		// Token: 0x04001C72 RID: 7282
		private const long DO_RSA_DECRYPT_FILE = 2251799813685248L;

		// Token: 0x020002CA RID: 714
		[Guid("00020406-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComVisible(false)]
		[ComImport]
		private interface IRegisterCreateITypeLib
		{
			// Token: 0x060024A8 RID: 9384
			void CreateTypeInfo();

			// Token: 0x060024A9 RID: 9385
			void SetName();

			// Token: 0x060024AA RID: 9386
			void SetVersion();

			// Token: 0x060024AB RID: 9387
			void SetGuid();

			// Token: 0x060024AC RID: 9388
			void SetDocString();

			// Token: 0x060024AD RID: 9389
			void SetHelpFileName();

			// Token: 0x060024AE RID: 9390
			void SetHelpContext();

			// Token: 0x060024AF RID: 9391
			void SetLcid();

			// Token: 0x060024B0 RID: 9392
			void SetLibFlags();

			// Token: 0x060024B1 RID: 9393
			void SaveAllChanges();
		}
	}
}
