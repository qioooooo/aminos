using System;
using System.Deployment.Application.Manifest;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace System.Deployment.Application
{
	// Token: 0x020000C6 RID: 198
	internal static class ShellExposure
	{
		// Token: 0x060004DC RID: 1244 RVA: 0x00019644 File Offset: 0x00018644
		public static void UpdateSubscriptionShellExposure(SubscriptionState subState)
		{
			using (subState.SubscriptionStore.AcquireStoreWriterLock())
			{
				ShellExposure.ShellExposureInformation shellExposureInformation = ShellExposure.ShellExposureInformation.CreateShellExposureInformation(subState.SubscriptionId);
				ShellExposure.UpdateShortcuts(subState, ref shellExposureInformation);
				ShellExposure.UpdateShellExtensions(subState, ref shellExposureInformation);
				ShellExposure.UpdateArpEntry(subState, shellExposureInformation);
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001969C File Offset: 0x0001869C
		public static void RemoveSubscriptionShellExposure(SubscriptionState subState)
		{
			using (subState.SubscriptionStore.AcquireStoreWriterLock())
			{
				DefinitionIdentity subscriptionId = subState.SubscriptionId;
				bool flag = false;
				ShellExposure.ShellExposureInformation shellExposureInformation = ShellExposure.ShellExposureInformation.CreateShellExposureInformation(subscriptionId);
				if (shellExposureInformation == null)
				{
					flag = true;
				}
				else
				{
					ShellExposure.RemoveShortcuts(shellExposureInformation);
				}
				ShellExposure.RemoveArpEntry(subscriptionId);
				if (flag)
				{
					throw new DeploymentException(ExceptionTypes.Subscription, Resources.GetString("Ex_ShortcutRemovalFailureDueToInvalidPublisherProduct"));
				}
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00019708 File Offset: 0x00018708
		public static void RemoveShellExtensions(DefinitionIdentity subId, AssemblyManifest appManifest, string productName)
		{
			foreach (FileAssociation fileAssociation in appManifest.FileAssociations)
			{
				ShellExposure.RemoveFileAssociation(fileAssociation, subId, productName);
			}
			NativeMethods.SHChangeNotify(134217728, 0U, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001974C File Offset: 0x0001874C
		public static void ParseAppShortcut(string shortcutFile, out DefinitionIdentity subId, out Uri providerUri)
		{
			FileInfo fileInfo = new FileInfo(shortcutFile);
			if (fileInfo.Length > 65536L)
			{
				throw new DeploymentException(ExceptionTypes.InvalidShortcut, Resources.GetString("Ex_ShortcutTooLarge"));
			}
			using (StreamReader streamReader = new StreamReader(shortcutFile, Encoding.Unicode))
			{
				string text;
				try
				{
					text = streamReader.ReadToEnd();
				}
				catch (IOException ex)
				{
					throw new DeploymentException(ExceptionTypes.InvalidShortcut, Resources.GetString("Ex_InvalidShortcutFormat"), ex);
				}
				if (text == null)
				{
					throw new DeploymentException(ExceptionTypes.InvalidShortcut, Resources.GetString("Ex_InvalidShortcutFormat"));
				}
				int num = text.IndexOf('#');
				if (num < 0)
				{
					throw new DeploymentException(ExceptionTypes.InvalidShortcut, Resources.GetString("Ex_InvalidShortcutFormat"));
				}
				try
				{
					subId = new DefinitionIdentity(text.Substring(num + 1));
				}
				catch (COMException ex2)
				{
					throw new DeploymentException(ExceptionTypes.InvalidShortcut, Resources.GetString("Ex_InvalidShortcutFormat"), ex2);
				}
				catch (SEHException ex3)
				{
					throw new DeploymentException(ExceptionTypes.InvalidShortcut, Resources.GetString("Ex_InvalidShortcutFormat"), ex3);
				}
				try
				{
					providerUri = new Uri(text.Substring(0, num));
				}
				catch (UriFormatException ex4)
				{
					throw new DeploymentException(ExceptionTypes.InvalidShortcut, Resources.GetString("Ex_InvalidShortcutFormat"), ex4);
				}
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001988C File Offset: 0x0001888C
		private static void MoveDeleteFile(string filePath)
		{
			if (!global::System.IO.File.Exists(filePath))
			{
				return;
			}
			string text = filePath;
			string text2 = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			try
			{
				global::System.IO.File.Move(filePath, text2);
				text = text2;
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			try
			{
				global::System.IO.File.Delete(text);
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00019908 File Offset: 0x00018908
		private static void MoveDeleteEmptyFolder(string folderPath)
		{
			if (!Directory.Exists(folderPath))
			{
				return;
			}
			string[] files = Directory.GetFiles(folderPath);
			if (files.Length > 0)
			{
				return;
			}
			string text = folderPath;
			string text2 = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			try
			{
				Directory.Move(folderPath, text2);
				text = text2;
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			try
			{
				Directory.Delete(text);
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x00019994 File Offset: 0x00018994
		private static void UpdateShortcuts(SubscriptionState subState, ref ShellExposure.ShellExposureInformation shellExposureInformation)
		{
			string text = string.Format("{0}#{1}", subState.DeploymentProviderUri.AbsoluteUri, subState.SubscriptionId.ToString());
			Description effectiveDescription = subState.EffectiveDescription;
			if (shellExposureInformation != null)
			{
				bool flag = true;
				bool flag2 = true;
				bool flag3 = true;
				bool flag4 = true;
				if (string.Compare(effectiveDescription.FilteredPublisher, shellExposureInformation.AppVendor, StringComparison.Ordinal) == 0)
				{
					flag = false;
					if (Utilities.CompareWithNullEqEmpty(effectiveDescription.FilteredSuiteName, shellExposureInformation.AppSuiteName, StringComparison.Ordinal) == 0)
					{
						flag2 = false;
						if (string.Compare(effectiveDescription.FilteredProduct, shellExposureInformation.AppProduct, StringComparison.Ordinal) == 0)
						{
							flag3 = false;
							if (string.Compare(text, shellExposureInformation.ShortcutAppId, StringComparison.Ordinal) == 0)
							{
								flag4 = false;
							}
						}
					}
				}
				if (!flag && !flag2 && !flag3 && !flag4 && global::System.IO.File.Exists(shellExposureInformation.ApplicationShortcutPath))
				{
					return;
				}
				if (flag3)
				{
					ShellExposure.UnpinShortcut(shellExposureInformation.ApplicationShortcutPath);
					ShellExposure.MoveDeleteFile(shellExposureInformation.ApplicationShortcutPath);
					ShellExposure.MoveDeleteFile(shellExposureInformation.SupportShortcutPath);
					ShellExposure.MoveDeleteFile(shellExposureInformation.DesktopShortcutPath);
				}
				if (flag2)
				{
					ShellExposure.MoveDeleteEmptyFolder(shellExposureInformation.ApplicationFolderPath);
				}
				if (flag)
				{
					ShellExposure.MoveDeleteEmptyFolder(shellExposureInformation.ApplicationRootFolderPath);
				}
				if (flag || flag2 || flag3)
				{
					shellExposureInformation = ShellExposure.ShellExposureInformation.CreateShellExposureInformation(effectiveDescription.FilteredPublisher, effectiveDescription.FilteredSuiteName, effectiveDescription.FilteredProduct, text);
				}
				else
				{
					shellExposureInformation.ShortcutAppId = text;
				}
			}
			else
			{
				shellExposureInformation = ShellExposure.ShellExposureInformation.CreateShellExposureInformation(effectiveDescription.FilteredPublisher, effectiveDescription.FilteredSuiteName, effectiveDescription.FilteredProduct, text);
			}
			try
			{
				Directory.CreateDirectory(shellExposureInformation.ApplicationFolderPath);
				ShellExposure.GenerateAppShortcut(subState, shellExposureInformation);
				ShellExposure.GenerateSupportShortcut(subState, shellExposureInformation);
			}
			catch (Exception)
			{
				ShellExposure.RemoveShortcuts(shellExposureInformation);
				throw;
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00019B28 File Offset: 0x00018B28
		private static void GenerateAppShortcut(SubscriptionState subState, ShellExposure.ShellExposureInformation shellExposureInformation)
		{
			using (StreamWriter streamWriter = new StreamWriter(shellExposureInformation.ApplicationShortcutPath, false, Encoding.Unicode))
			{
				streamWriter.Write("{0}#{1}", subState.DeploymentProviderUri.AbsoluteUri, subState.SubscriptionId.ToString());
			}
			if (subState.CurrentDeploymentManifest.Deployment.CreateDesktopShortcut)
			{
				using (StreamWriter streamWriter2 = new StreamWriter(shellExposureInformation.DesktopShortcutPath, false, Encoding.Unicode))
				{
					streamWriter2.Write("{0}#{1}", subState.DeploymentProviderUri.AbsoluteUri, subState.SubscriptionId.ToString());
				}
			}
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00019BE4 File Offset: 0x00018BE4
		private static void GenerateSupportShortcut(SubscriptionState subState, ShellExposure.ShellExposureInformation shellExposureInformation)
		{
			Description effectiveDescription = subState.EffectiveDescription;
			if (effectiveDescription.SupportUri != null)
			{
				using (StreamWriter streamWriter = new StreamWriter(shellExposureInformation.SupportShortcutPath, false, Encoding.ASCII))
				{
					streamWriter.WriteLine("[Default]");
					streamWriter.WriteLine("BASEURL=" + effectiveDescription.SupportUri.AbsoluteUri);
					streamWriter.WriteLine("[InternetShortcut]");
					streamWriter.WriteLine("URL=" + effectiveDescription.SupportUri.AbsoluteUri);
					streamWriter.WriteLine();
					streamWriter.WriteLine("IconFile=" + PathHelper.ShortShimDllPath);
					streamWriter.WriteLine("IconIndex=" + 0.ToString(CultureInfo.InvariantCulture));
					streamWriter.WriteLine();
				}
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00019CC4 File Offset: 0x00018CC4
		private static void RemoveShortcuts(ShellExposure.ShellExposureInformation shellExposureInformation)
		{
			try
			{
				if (global::System.IO.File.Exists(shellExposureInformation.ApplicationShortcutPath))
				{
					global::System.IO.File.Delete(shellExposureInformation.ApplicationShortcutPath);
				}
				if (global::System.IO.File.Exists(shellExposureInformation.SupportShortcutPath))
				{
					global::System.IO.File.Delete(shellExposureInformation.SupportShortcutPath);
				}
				if (global::System.IO.File.Exists(shellExposureInformation.DesktopShortcutPath))
				{
					global::System.IO.File.Delete(shellExposureInformation.DesktopShortcutPath);
				}
				if (Directory.Exists(shellExposureInformation.ApplicationFolderPath))
				{
					string[] files = Directory.GetFiles(shellExposureInformation.ApplicationFolderPath);
					string[] directories = Directory.GetDirectories(shellExposureInformation.ApplicationFolderPath);
					if (files.Length == 0 && directories.Length == 0)
					{
						Directory.Delete(shellExposureInformation.ApplicationFolderPath);
					}
				}
				if (Directory.Exists(shellExposureInformation.ApplicationRootFolderPath))
				{
					string[] files2 = Directory.GetFiles(shellExposureInformation.ApplicationRootFolderPath);
					string[] directories2 = Directory.GetDirectories(shellExposureInformation.ApplicationRootFolderPath);
					if (files2.Length == 0 && directories2.Length == 0)
					{
						Directory.Delete(shellExposureInformation.ApplicationRootFolderPath);
					}
				}
			}
			catch (IOException ex)
			{
				throw new DeploymentException(ExceptionTypes.InvalidShortcut, Resources.GetString("Ex_ShortcutRemovalFailure"), ex);
			}
			catch (UnauthorizedAccessException ex2)
			{
				throw new DeploymentException(ExceptionTypes.InvalidShortcut, Resources.GetString("Ex_ShortcutRemovalFailure"), ex2);
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00019DD8 File Offset: 0x00018DD8
		internal static void RemovePins(SubscriptionState subState)
		{
			DefinitionIdentity subscriptionId = subState.SubscriptionId;
			ShellExposure.ShellExposureInformation shellExposureInformation = ShellExposure.ShellExposureInformation.CreateShellExposureInformation(subscriptionId);
			if (shellExposureInformation == null)
			{
				return;
			}
			if (global::System.IO.File.Exists(shellExposureInformation.ApplicationShortcutPath))
			{
				ShellExposure.UnpinShortcut(shellExposureInformation.ApplicationShortcutPath);
			}
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00019E10 File Offset: 0x00018E10
		public static void UpdateShellExtensions(SubscriptionState subState, ref ShellExposure.ShellExposureInformation shellExposureInformation)
		{
			string text = null;
			if (shellExposureInformation != null)
			{
				text = shellExposureInformation.AppProduct;
			}
			if (text == null)
			{
				text = subState.SubscriptionId.Name;
			}
			if (subState.PreviousBind != null)
			{
				ShellExposure.RemoveShellExtensions(subState.SubscriptionId, subState.PreviousApplicationManifest, text);
			}
			ShellExposure.AddShellExtensions(subState.SubscriptionId, subState.DeploymentProviderUri, subState.CurrentApplicationManifest);
			NativeMethods.SHChangeNotify(134217728, 0U, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00019E80 File Offset: 0x00018E80
		private static void UnpinShortcut(string shortcutPath)
		{
			NativeMethods.IShellItem shellItem = null;
			NativeMethods.IStartMenuPinnedList startMenuPinnedList = null;
			try
			{
				object obj = null;
				object obj2 = null;
				uint num = NativeMethods.SHCreateItemFromParsingName(shortcutPath, IntPtr.Zero, Constants.uuid, out obj);
				if (num == 0U)
				{
					shellItem = obj as NativeMethods.IShellItem;
					num = NativeMethods.CoCreateInstance(ref Constants.CLSID_StartMenuPin, null, 1, ref Constants.IID_IUnknown, out obj2);
					if (num == 0U)
					{
						startMenuPinnedList = obj2 as NativeMethods.IStartMenuPinnedList;
						startMenuPinnedList.RemoveFromList(shellItem);
					}
				}
			}
			catch (EntryPointNotFoundException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			finally
			{
				if (shellItem != null)
				{
					Marshal.ReleaseComObject(shellItem);
				}
				if (startMenuPinnedList != null)
				{
					Marshal.ReleaseComObject(startMenuPinnedList);
				}
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00019F28 File Offset: 0x00018F28
		private static void AddShellExtensions(DefinitionIdentity subId, Uri deploymentProviderUri, AssemblyManifest appManifest)
		{
			foreach (FileAssociation fileAssociation in appManifest.FileAssociations)
			{
				ShellExposure.AddFileAssociation(fileAssociation, subId, deploymentProviderUri);
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00019F58 File Offset: 0x00018F58
		private static void AddFileAssociation(FileAssociation fileAssociation, DefinitionIdentity subId, Uri deploymentProviderUri)
		{
			RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(fileAssociation.Extension);
			RegistryKey registryKey2 = Registry.ClassesRoot.OpenSubKey(fileAssociation.ProgID);
			if (registryKey != null || registryKey2 != null)
			{
				Logger.AddWarningInformation(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("SkippedFileAssoc"), new object[] { fileAssociation.Extension }));
				return;
			}
			string text = Guid.NewGuid().ToString("B");
			string text2 = subId.ToString();
			using (RegistryKey registryKey3 = Registry.CurrentUser.CreateSubKey("Software\\Classes"))
			{
				using (RegistryKey registryKey4 = registryKey3.CreateSubKey(fileAssociation.Extension))
				{
					registryKey4.SetValue(null, fileAssociation.ProgID);
					registryKey4.SetValue("AppId", text2);
					registryKey4.SetValue("Guid", text);
					registryKey4.SetValue("DeploymentProviderUrl", deploymentProviderUri.AbsoluteUri);
				}
				using (RegistryKey registryKey5 = registryKey3.CreateSubKey(fileAssociation.ProgID))
				{
					registryKey5.SetValue(null, fileAssociation.Description);
					registryKey5.SetValue("AppId", text2);
					registryKey5.SetValue("Guid", text);
					registryKey5.SetValue("DeploymentProviderUrl", deploymentProviderUri.AbsoluteUri);
					using (RegistryKey registryKey6 = registryKey5.CreateSubKey("shell"))
					{
						registryKey6.SetValue(null, "open");
						using (RegistryKey registryKey7 = registryKey6.CreateSubKey("open\\command"))
						{
							registryKey7.SetValue(null, "rundll32.exe dfshim.dll, ShOpenVerbExtension " + text + " %1");
						}
						using (RegistryKey registryKey8 = registryKey5.CreateSubKey("shellex\\IconHandler"))
						{
							registryKey8.SetValue(null, text);
						}
					}
				}
				using (RegistryKey registryKey9 = registryKey3.CreateSubKey("CLSID"))
				{
					using (RegistryKey registryKey10 = registryKey9.CreateSubKey(text))
					{
						registryKey10.SetValue(null, "Shell Icon Handler For " + fileAssociation.Description);
						registryKey10.SetValue("AppId", text2);
						registryKey10.SetValue("DeploymentProviderUrl", deploymentProviderUri.AbsoluteUri);
						registryKey10.SetValue("IconFile", fileAssociation.DefaultIcon);
						using (RegistryKey registryKey11 = registryKey10.CreateSubKey("InProcServer32"))
						{
							registryKey11.SetValue(null, "dfshim.dll");
							registryKey11.SetValue("ThreadingModel", "Apartment");
						}
					}
				}
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001A2BC File Offset: 0x000192BC
		private static void RemoveFileAssociation(FileAssociation fileAssociation, DefinitionIdentity subId, string productName)
		{
			using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Classes", true))
			{
				if (registryKey != null)
				{
					ShellExposure.RemoveFileAssociationExtentionInfo(fileAssociation, subId, registryKey, productName);
					string text = ShellExposure.RemoveFileAssociationProgIDInfo(fileAssociation, subId, registryKey, productName);
					if (text != null)
					{
						ShellExposure.RemoveFileAssociationCLSIDInfo(fileAssociation, subId, registryKey, text, productName);
					}
				}
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001A31C File Offset: 0x0001931C
		private static void RemoveFileAssociationExtentionInfo(FileAssociation fileAssociation, DefinitionIdentity subId, RegistryKey classesKey, string productName)
		{
			using (RegistryKey registryKey = classesKey.OpenSubKey(fileAssociation.Extension, true))
			{
				if (registryKey != null)
				{
					object value = registryKey.GetValue("AppId");
					if (value is string)
					{
						string text = (string)value;
						if (string.Equals(text, subId.ToString(), StringComparison.Ordinal))
						{
							try
							{
								classesKey.DeleteSubKeyTree(fileAssociation.Extension);
							}
							catch (ArgumentException ex)
							{
								throw new DeploymentException(ExceptionTypes.InvalidARPEntry, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileAssocExtDeleteFailed"), new object[] { fileAssociation.Extension, productName }), ex);
							}
						}
					}
				}
			}
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001A3D8 File Offset: 0x000193D8
		private static string RemoveFileAssociationProgIDInfo(FileAssociation fileAssociation, DefinitionIdentity subId, RegistryKey classesKey, string productName)
		{
			string text = null;
			using (RegistryKey registryKey = classesKey.OpenSubKey(fileAssociation.ProgID, true))
			{
				if (registryKey == null)
				{
					return null;
				}
				object value = registryKey.GetValue("AppId");
				if (!(value is string))
				{
					return null;
				}
				string text2 = (string)value;
				if (!string.Equals(text2, subId.ToString(), StringComparison.Ordinal))
				{
					return null;
				}
				text = (string)registryKey.GetValue("Guid");
				try
				{
					classesKey.DeleteSubKeyTree(fileAssociation.ProgID);
				}
				catch (ArgumentException ex)
				{
					throw new DeploymentException(ExceptionTypes.InvalidARPEntry, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileAssocProgIdDeleteFailed"), new object[] { fileAssociation.ProgID, productName }), ex);
				}
			}
			return text;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001A4BC File Offset: 0x000194BC
		private static void RemoveFileAssociationCLSIDInfo(FileAssociation fileAssociation, DefinitionIdentity subId, RegistryKey classesKey, string clsIdString, string productName)
		{
			using (RegistryKey registryKey = classesKey.OpenSubKey("CLSID", true))
			{
				if (registryKey != null)
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(clsIdString))
					{
						object value = registryKey2.GetValue("AppId");
						if (value is string)
						{
							string text = (string)value;
							if (string.Equals(text, subId.ToString(), StringComparison.Ordinal))
							{
								try
								{
									registryKey.DeleteSubKeyTree(clsIdString);
								}
								catch (ArgumentException ex)
								{
									throw new DeploymentException(ExceptionTypes.InvalidARPEntry, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileAssocCLSIDDeleteFailed"), new object[] { clsIdString, productName }), ex);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001A594 File Offset: 0x00019594
		private static void UpdateArpEntry(SubscriptionState subState, ShellExposure.ShellExposureInformation shellExposureInformation)
		{
			DefinitionIdentity subscriptionId = subState.SubscriptionId;
			string text = string.Format(CultureInfo.InvariantCulture, "rundll32.exe dfshim.dll,ShArpMaintain {0}", new object[] { subscriptionId.ToString() });
			string text2 = string.Format(CultureInfo.InvariantCulture, "dfshim.dll,2", new object[0]);
			AssemblyManifest currentDeploymentManifest = subState.CurrentDeploymentManifest;
			Description effectiveDescription = subState.EffectiveDescription;
			using (RegistryKey registryKey = ShellExposure.UninstallRoot.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall"))
			{
				using (RegistryKey registryKey2 = registryKey.CreateSubKey(ShellExposure.GenerateArpKeyName(subscriptionId)))
				{
					string[] array = new string[]
					{
						"DisplayName",
						shellExposureInformation.ARPDisplayName,
						"DisplayIcon",
						text2,
						"DisplayVersion",
						currentDeploymentManifest.Identity.Version.ToString(),
						"Publisher",
						effectiveDescription.FilteredPublisher,
						"UninstallString",
						text,
						"HelpLink",
						effectiveDescription.SupportUrl,
						"UrlUpdateInfo",
						subState.DeploymentProviderUri.AbsoluteUri,
						"ShortcutFolderName",
						shellExposureInformation.AppVendor,
						"ShortcutFileName",
						shellExposureInformation.AppProduct,
						"ShortcutSuiteName",
						shellExposureInformation.AppSuiteName,
						"SupportShortcutFileName",
						shellExposureInformation.AppSupportShortcut,
						"ShortcutAppId",
						shellExposureInformation.ShortcutAppId
					};
					for (int i = array.Length - 2; i >= 0; i -= 2)
					{
						string text3 = array[i];
						string text4 = array[i + 1];
						if (text4 != null)
						{
							registryKey2.SetValue(text3, text4);
						}
						else
						{
							registryKey2.DeleteValue(text3, false);
						}
					}
				}
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001A7A8 File Offset: 0x000197A8
		private static void RemoveArpEntry(DefinitionIdentity subId)
		{
			using (RegistryKey registryKey = ShellExposure.UninstallRoot.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall", true))
			{
				string text = null;
				try
				{
					if (registryKey != null)
					{
						text = ShellExposure.GenerateArpKeyName(subId);
						registryKey.DeleteSubKeyTree(text);
					}
				}
				catch (ArgumentException ex)
				{
					throw new DeploymentException(ExceptionTypes.InvalidARPEntry, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ArpEntryRemovalFailure"), new object[] { text }), ex);
				}
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001A82C File Offset: 0x0001982C
		private static string GenerateArpKeyName(DefinitionIdentity subId)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0:x16}", new object[] { subId.Hash });
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0001A85E File Offset: 0x0001985E
		private static RegistryKey UninstallRoot
		{
			get
			{
				if (!PlatformSpecific.OnWin9x)
				{
					return Registry.CurrentUser;
				}
				return Registry.LocalMachine;
			}
		}

		// Token: 0x020000C7 RID: 199
		public class ShellExposureInformation
		{
			// Token: 0x060004F3 RID: 1267 RVA: 0x0001A874 File Offset: 0x00019874
			public static ShellExposure.ShellExposureInformation CreateShellExposureInformation(DefinitionIdentity subscriptionIdentity)
			{
				ShellExposure.ShellExposureInformation shellExposureInformation = null;
				string text = null;
				string text2 = null;
				string text3 = null;
				string text4 = null;
				string text5 = "";
				using (RegistryKey registryKey = ShellExposure.UninstallRoot.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall"))
				{
					if (registryKey != null)
					{
						using (RegistryKey registryKey2 = registryKey.OpenSubKey(ShellExposure.GenerateArpKeyName(subscriptionIdentity)))
						{
							if (registryKey2 != null)
							{
								text = registryKey2.GetValue("ShortcutFolderName") as string;
								text2 = registryKey2.GetValue("ShortcutFileName") as string;
								if (registryKey2.GetValue("ShortcutSuiteName") != null)
								{
									text3 = registryKey2.GetValue("ShortcutSuiteName") as string;
								}
								else
								{
									text3 = "";
								}
								text4 = registryKey2.GetValue("SupportShortcutFileName") as string;
								if (registryKey2.GetValue("ShortcutAppId") != null)
								{
									text5 = registryKey2.GetValue("ShortcutAppId") as string;
								}
								else
								{
									text5 = "";
								}
							}
						}
					}
				}
				if (text != null && text2 != null && text4 != null)
				{
					shellExposureInformation = new ShellExposure.ShellExposureInformation();
					shellExposureInformation._applicationRootFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), text);
					if (string.IsNullOrEmpty(text3))
					{
						shellExposureInformation._applicationFolderPath = shellExposureInformation._applicationRootFolderPath;
					}
					else
					{
						shellExposureInformation._applicationFolderPath = Path.Combine(shellExposureInformation._applicationRootFolderPath, text3);
					}
					shellExposureInformation._applicationShortcutPath = Path.Combine(shellExposureInformation._applicationFolderPath, text2 + ".appref-ms");
					shellExposureInformation._supportShortcutPath = Path.Combine(shellExposureInformation._applicationFolderPath, text4 + ".url");
					shellExposureInformation._desktopShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), text2 + ".appref-ms");
					shellExposureInformation._appVendor = text;
					shellExposureInformation._appProduct = text2;
					shellExposureInformation._appSupportShortcut = text4;
					shellExposureInformation._shortcutAppId = text5;
					shellExposureInformation._appSuiteName = text3;
				}
				return shellExposureInformation;
			}

			// Token: 0x060004F4 RID: 1268 RVA: 0x0001AA50 File Offset: 0x00019A50
			public static ShellExposure.ShellExposureInformation CreateShellExposureInformation(string publisher, string suiteName, string product, string shortcutAppId)
			{
				ShellExposure.ShellExposureInformation shellExposureInformation = new ShellExposure.ShellExposureInformation();
				string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Programs), publisher);
				string text2 = text;
				if (!string.IsNullOrEmpty(suiteName))
				{
					text2 = Path.Combine(text, suiteName);
				}
				string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				for (int num = 0; num != 2147483647; num++)
				{
					string text3;
					if (num == 0)
					{
						text3 = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("ShellExposure_DisplayStringNoIndex"), new object[] { product });
					}
					else
					{
						text3 = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("ShellExposure_DisplayStringWithIndex"), new object[] { product, num });
					}
					string text4 = Path.Combine(text2, text3 + ".appref-ms");
					string text5 = Path.Combine(folderPath, text3 + ".appref-ms");
					if (!global::System.IO.File.Exists(text4) && !global::System.IO.File.Exists(text5))
					{
						shellExposureInformation._appVendor = publisher;
						shellExposureInformation._appProduct = text3;
						shellExposureInformation._appSuiteName = suiteName;
						shellExposureInformation._applicationFolderPath = text2;
						shellExposureInformation._applicationRootFolderPath = text;
						shellExposureInformation._applicationShortcutPath = text4;
						shellExposureInformation._desktopShortcutPath = text5;
						shellExposureInformation._appSupportShortcut = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("SupportUrlFormatter"), new object[] { text3 });
						shellExposureInformation._supportShortcutPath = Path.Combine(text2, shellExposureInformation._appSupportShortcut + ".url");
						shellExposureInformation._shortcutAppId = shortcutAppId;
						return shellExposureInformation;
					}
				}
				throw new OverflowException();
			}

			// Token: 0x1700011B RID: 283
			// (get) Token: 0x060004F5 RID: 1269 RVA: 0x0001ABC9 File Offset: 0x00019BC9
			public string ApplicationFolderPath
			{
				get
				{
					return this._applicationFolderPath;
				}
			}

			// Token: 0x1700011C RID: 284
			// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0001ABD1 File Offset: 0x00019BD1
			public string ApplicationRootFolderPath
			{
				get
				{
					return this._applicationRootFolderPath;
				}
			}

			// Token: 0x1700011D RID: 285
			// (get) Token: 0x060004F7 RID: 1271 RVA: 0x0001ABD9 File Offset: 0x00019BD9
			public string ApplicationShortcutPath
			{
				get
				{
					return this._applicationShortcutPath;
				}
			}

			// Token: 0x1700011E RID: 286
			// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0001ABE1 File Offset: 0x00019BE1
			public string SupportShortcutPath
			{
				get
				{
					return this._supportShortcutPath;
				}
			}

			// Token: 0x1700011F RID: 287
			// (get) Token: 0x060004F9 RID: 1273 RVA: 0x0001ABE9 File Offset: 0x00019BE9
			public string DesktopShortcutPath
			{
				get
				{
					return this._desktopShortcutPath;
				}
			}

			// Token: 0x17000120 RID: 288
			// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001ABF4 File Offset: 0x00019BF4
			public string ARPDisplayName
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(this._appProduct);
					if (PlatformSpecific.OnWin9x && stringBuilder.Length > 63)
					{
						stringBuilder.Length = 60;
						stringBuilder.Append("...");
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x17000121 RID: 289
			// (get) Token: 0x060004FB RID: 1275 RVA: 0x0001AC3F File Offset: 0x00019C3F
			public string AppVendor
			{
				get
				{
					return this._appVendor;
				}
			}

			// Token: 0x17000122 RID: 290
			// (get) Token: 0x060004FC RID: 1276 RVA: 0x0001AC47 File Offset: 0x00019C47
			public string AppProduct
			{
				get
				{
					return this._appProduct;
				}
			}

			// Token: 0x17000123 RID: 291
			// (get) Token: 0x060004FD RID: 1277 RVA: 0x0001AC4F File Offset: 0x00019C4F
			public string AppSuiteName
			{
				get
				{
					return this._appSuiteName;
				}
			}

			// Token: 0x17000124 RID: 292
			// (get) Token: 0x060004FE RID: 1278 RVA: 0x0001AC57 File Offset: 0x00019C57
			public string AppSupportShortcut
			{
				get
				{
					return this._appSupportShortcut;
				}
			}

			// Token: 0x17000125 RID: 293
			// (get) Token: 0x060004FF RID: 1279 RVA: 0x0001AC5F File Offset: 0x00019C5F
			// (set) Token: 0x06000500 RID: 1280 RVA: 0x0001AC67 File Offset: 0x00019C67
			public string ShortcutAppId
			{
				get
				{
					return this._shortcutAppId;
				}
				set
				{
					this._shortcutAppId = value;
				}
			}

			// Token: 0x06000501 RID: 1281 RVA: 0x0001AC70 File Offset: 0x00019C70
			protected ShellExposureInformation()
			{
			}

			// Token: 0x04000455 RID: 1109
			private string _applicationFolderPath;

			// Token: 0x04000456 RID: 1110
			private string _applicationRootFolderPath;

			// Token: 0x04000457 RID: 1111
			private string _applicationShortcutPath;

			// Token: 0x04000458 RID: 1112
			private string _desktopShortcutPath;

			// Token: 0x04000459 RID: 1113
			private string _supportShortcutPath;

			// Token: 0x0400045A RID: 1114
			private string _appVendor;

			// Token: 0x0400045B RID: 1115
			private string _appProduct;

			// Token: 0x0400045C RID: 1116
			private string _appSuiteName;

			// Token: 0x0400045D RID: 1117
			private string _appSupportShortcut;

			// Token: 0x0400045E RID: 1118
			private string _shortcutAppId;
		}
	}
}
