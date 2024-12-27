using System;
using System.Deployment.Application.Manifest;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Policy;
using Microsoft.Win32;

namespace System.Deployment.Application
{
	// Token: 0x02000049 RID: 73
	internal static class DownloadManager
	{
		// Token: 0x0600024B RID: 587 RVA: 0x0000E5BB File Offset: 0x0000D5BB
		public static AssemblyManifest DownloadDeploymentManifest(SubscriptionStore subStore, ref Uri sourceUri, out TempFile tempFile)
		{
			return DownloadManager.DownloadDeploymentManifest(subStore, ref sourceUri, out tempFile, null, null);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000E5C8 File Offset: 0x0000D5C8
		public static AssemblyManifest DownloadDeploymentManifest(SubscriptionStore subStore, ref Uri sourceUri, out TempFile tempFile, IDownloadNotification notification, DownloadOptions options)
		{
			tempFile = null;
			TempFile tempFile2 = null;
			TempFile tempFile3 = null;
			AssemblyManifest assemblyManifest;
			try
			{
				ServerInformation serverInformation;
				assemblyManifest = DownloadManager.DownloadDeploymentManifestDirect(subStore, ref sourceUri, out tempFile2, notification, options, out serverInformation);
				Logger.SetSubscriptionServerInformation(serverInformation);
				tempFile = (DownloadManager.FollowDeploymentProviderUri(subStore, ref assemblyManifest, ref sourceUri, out tempFile3, notification, options) ? tempFile3 : tempFile2);
			}
			finally
			{
				if (tempFile2 != null && tempFile2 != tempFile)
				{
					tempFile2.Dispose();
					tempFile2 = null;
				}
				if (tempFile3 != null && tempFile3 != tempFile)
				{
					tempFile3.Dispose();
					tempFile3 = null;
				}
			}
			return assemblyManifest;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000E640 File Offset: 0x0000D640
		public static bool FollowDeploymentProviderUri(SubscriptionStore subStore, ref AssemblyManifest deployment, ref Uri sourceUri, out TempFile tempFile, IDownloadNotification notification, DownloadOptions options)
		{
			tempFile = null;
			bool flag = false;
			Zone zone = Zone.CreateFromUrl(sourceUri.AbsoluteUri);
			bool flag2 = false;
			if (zone.SecurityZone != SecurityZone.MyComputer)
			{
				flag2 = true;
			}
			else
			{
				DependentAssembly mainDependentAssembly = deployment.MainDependentAssembly;
				if (mainDependentAssembly == null || mainDependentAssembly.Codebase == null)
				{
					throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_NoAppInDeploymentManifest"));
				}
				Uri uri = new Uri(sourceUri, mainDependentAssembly.Codebase);
				Zone zone2 = Zone.CreateFromUrl(uri.AbsoluteUri);
				if (zone2.SecurityZone == SecurityZone.MyComputer && !global::System.IO.File.Exists(uri.LocalPath))
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				Uri providerCodebaseUri = deployment.Deployment.ProviderCodebaseUri;
				Logger.SetDeploymentProviderUrl(providerCodebaseUri);
				if (!PolicyKeys.SkipDeploymentProvider() && providerCodebaseUri != null && !providerCodebaseUri.Equals(sourceUri))
				{
					AssemblyManifest assemblyManifest = null;
					ServerInformation serverInformation;
					try
					{
						assemblyManifest = DownloadManager.DownloadDeploymentManifestDirect(subStore, ref providerCodebaseUri, out tempFile, notification, options, out serverInformation);
					}
					catch (InvalidDeploymentException ex)
					{
						if (ex.SubType == ExceptionTypes.Manifest || ex.SubType == ExceptionTypes.ManifestLoad || ex.SubType == ExceptionTypes.ManifestParse || ex.SubType == ExceptionTypes.ManifestSemanticValidation)
						{
							throw new InvalidDeploymentException(ExceptionTypes.Manifest, Resources.GetString("Ex_InvalidProviderManifest"), ex);
						}
						throw;
					}
					Logger.SetDeploymentProviderServerInformation(serverInformation);
					SubscriptionState subscriptionState = subStore.GetSubscriptionState(deployment);
					SubscriptionState subscriptionState2 = subStore.GetSubscriptionState(assemblyManifest);
					if (!subscriptionState2.SubscriptionId.Equals(subscriptionState.SubscriptionId))
					{
						throw new InvalidDeploymentException(ExceptionTypes.SubscriptionSemanticValidation, Resources.GetString("Ex_ProviderNotInSubscription"));
					}
					deployment = assemblyManifest;
					sourceUri = providerCodebaseUri;
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000E7C0 File Offset: 0x0000D7C0
		public static AssemblyManifest DownloadDeploymentManifestBypass(SubscriptionStore subStore, ref Uri sourceUri, out TempFile tempFile, out SubscriptionState subState, IDownloadNotification notification, DownloadOptions options)
		{
			tempFile = null;
			subState = null;
			TempFile tempFile2 = null;
			TempFile tempFile3 = null;
			AssemblyManifest assemblyManifest;
			try
			{
				ServerInformation serverInformation;
				assemblyManifest = DownloadManager.DownloadDeploymentManifestDirectBypass(subStore, ref sourceUri, out tempFile2, out subState, notification, options, out serverInformation);
				Logger.SetSubscriptionServerInformation(serverInformation);
				if (subState != null)
				{
					tempFile = tempFile2;
					return assemblyManifest;
				}
				tempFile = (DownloadManager.FollowDeploymentProviderUri(subStore, ref assemblyManifest, ref sourceUri, out tempFile3, notification, options) ? tempFile3 : tempFile2);
			}
			finally
			{
				if (tempFile2 != null && tempFile2 != tempFile)
				{
					tempFile2.Dispose();
				}
				if (tempFile3 != null && tempFile3 != tempFile)
				{
					tempFile3.Dispose();
				}
			}
			return assemblyManifest;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000E848 File Offset: 0x0000D848
		public static AssemblyManifest DownloadApplicationManifest(AssemblyManifest deploymentManifest, string targetDir, Uri deploymentUri, out Uri appSourceUri, out string appManifestPath)
		{
			return DownloadManager.DownloadApplicationManifest(deploymentManifest, targetDir, deploymentUri, null, null, out appSourceUri, out appManifestPath);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000E858 File Offset: 0x0000D858
		public static AssemblyManifest DownloadApplicationManifest(AssemblyManifest deploymentManifest, string targetDir, Uri deploymentUri, IDownloadNotification notification, DownloadOptions options, out Uri appSourceUri, out string appManifestPath)
		{
			DependentAssembly mainDependentAssembly = deploymentManifest.MainDependentAssembly;
			if (mainDependentAssembly == null || mainDependentAssembly.Codebase == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_NoAppInDeploymentManifest"));
			}
			appSourceUri = new Uri(deploymentUri, mainDependentAssembly.Codebase);
			Zone zone = Zone.CreateFromUrl(deploymentUri.AbsoluteUri);
			Zone zone2 = Zone.CreateFromUrl(appSourceUri.AbsoluteUri);
			if (!zone.Equals(zone2))
			{
				throw new InvalidDeploymentException(ExceptionTypes.Zone, Resources.GetString("Ex_DeployAppZoneMismatch"));
			}
			appManifestPath = Path.Combine(targetDir, mainDependentAssembly.Identity.Name + ".manifest");
			ServerInformation serverInformation;
			AssemblyManifest assemblyManifest = DownloadManager.DownloadManifest(ref appSourceUri, appManifestPath, notification, options, AssemblyManifest.ManifestType.Application, out serverInformation);
			Logger.SetApplicationUrl(appSourceUri);
			Logger.SetApplicationServerInformation(serverInformation);
			zone2 = Zone.CreateFromUrl(appSourceUri.AbsoluteUri);
			if (!zone.Equals(zone2))
			{
				throw new InvalidDeploymentException(ExceptionTypes.Zone, Resources.GetString("Ex_DeployAppZoneMismatch"));
			}
			if (assemblyManifest.Identity.Equals(deploymentManifest.Identity))
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DepSameDeploymentAndApplicationIdentity"), new object[] { assemblyManifest.Identity.ToString() }));
			}
			if (!assemblyManifest.Identity.Matches(mainDependentAssembly.Identity, assemblyManifest.Application))
			{
				throw new InvalidDeploymentException(ExceptionTypes.SubscriptionSemanticValidation, Resources.GetString("Ex_RefDefMismatch"));
			}
			if (!PolicyKeys.SkipApplicationDependencyHashCheck())
			{
				try
				{
					ComponentVerifier.VerifyFileHash(appManifestPath, mainDependentAssembly.HashCollection);
				}
				catch (InvalidDeploymentException ex)
				{
					if (ex.SubType == ExceptionTypes.HashValidation)
					{
						throw new InvalidDeploymentException(ExceptionTypes.HashValidation, Resources.GetString("Ex_AppManInvalidHash"), ex);
					}
					throw;
				}
			}
			if (assemblyManifest.RequestedExecutionLevel != null)
			{
				DownloadManager.VerifyRequestedPrivilegesSupport(assemblyManifest.RequestedExecutionLevel);
			}
			return assemblyManifest;
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000EA0C File Offset: 0x0000DA0C
		public static void DownloadDependencies(SubscriptionState subState, AssemblyManifest deployManifest, AssemblyManifest appManifest, Uri sourceUriBase, string targetDirectory, string group, IDownloadNotification notification, DownloadOptions options)
		{
			FileDownloader fileDownloader = FileDownloader.Create();
			fileDownloader.Options = options;
			if (group == null)
			{
				fileDownloader.CheckForSizeLimit(appManifest.CalculateDependenciesSize(), false);
			}
			DownloadManager.AddDependencies(fileDownloader, deployManifest, appManifest, sourceUriBase, targetDirectory, group);
			fileDownloader.DownloadModified += DownloadManager.ProcessDownloadedFile;
			if (notification != null)
			{
				fileDownloader.AddNotification(notification);
			}
			try
			{
				fileDownloader.Download(subState);
				fileDownloader.ComponentVerifier.VerifyComponents();
				DownloadManager.VerifyRequestedPrivilegesSupport(appManifest, targetDirectory);
			}
			finally
			{
				if (notification != null)
				{
					fileDownloader.RemoveNotification(notification);
				}
				fileDownloader.DownloadModified -= DownloadManager.ProcessDownloadedFile;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000EAB0 File Offset: 0x0000DAB0
		private static void VerifyRequestedPrivilegesSupport(AssemblyManifest appManifest, string targetDirectory)
		{
			if (appManifest.EntryPoints[0].CustomHostSpecified)
			{
				return;
			}
			string text = Path.Combine(targetDirectory, appManifest.EntryPoints[0].Assembly.Codebase);
			if (global::System.IO.File.Exists(text))
			{
				AssemblyManifest assemblyManifest = new AssemblyManifest(text);
				if (assemblyManifest.Id1ManifestPresent && assemblyManifest.Id1RequestedExecutionLevel != null)
				{
					DownloadManager.VerifyRequestedPrivilegesSupport(assemblyManifest.Id1RequestedExecutionLevel);
				}
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000EB10 File Offset: 0x0000DB10
		private static void VerifyRequestedPrivilegesSupport(string requestedExecutionLevel)
		{
			if (PlatformSpecific.OnVistaOrAbove)
			{
				bool flag = false;
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
				if (registryKey != null && registryKey.GetValue("EnableLUA") != null)
				{
					int num = (int)registryKey.GetValue("EnableLUA");
					if (num != 0)
					{
						flag = true;
					}
				}
				if (flag && (string.Compare(requestedExecutionLevel, "requireAdministrator", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(requestedExecutionLevel, "highestAvailable", StringComparison.OrdinalIgnoreCase) == 0))
				{
					throw new InvalidDeploymentException(ExceptionTypes.UnsupportedElevetaionRequest, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ManifestExecutionLevelNotSupported"), new object[0]));
				}
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000EBA0 File Offset: 0x0000DBA0
		private static AssemblyManifest DownloadDeploymentManifestDirect(SubscriptionStore subStore, ref Uri sourceUri, out TempFile tempFile, IDownloadNotification notification, DownloadOptions options, out ServerInformation serverInformation)
		{
			tempFile = subStore.AcquireTempFile(".application");
			AssemblyManifest assemblyManifest = DownloadManager.DownloadManifest(ref sourceUri, tempFile.Path, notification, options, AssemblyManifest.ManifestType.Deployment, out serverInformation);
			if (assemblyManifest.Identity.Version == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_DeploymentManifestNoVersion"));
			}
			if (assemblyManifest.Deployment == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_InvalidDeploymentManifest"));
			}
			return assemblyManifest;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000EC10 File Offset: 0x0000DC10
		private static AssemblyManifest DownloadDeploymentManifestDirectBypass(SubscriptionStore subStore, ref Uri sourceUri, out TempFile tempFile, out SubscriptionState subState, IDownloadNotification notification, DownloadOptions options, out ServerInformation serverInformation)
		{
			subState = null;
			tempFile = subStore.AcquireTempFile(".application");
			DownloadManager.DownloadManifestAsRawFile(ref sourceUri, tempFile.Path, notification, options, out serverInformation);
			bool flag = false;
			AssemblyManifest assemblyManifest = null;
			DefinitionAppId definitionAppId = null;
			try
			{
				assemblyManifest = ManifestReader.FromDocumentNoValidation(tempFile.Path);
				DefinitionIdentity identity = assemblyManifest.Identity;
				DefinitionIdentity definitionIdentity = new DefinitionIdentity(assemblyManifest.MainDependentAssembly.Identity);
				Uri uri = ((sourceUri.Query != null && sourceUri.Query.Length > 0) ? new Uri(sourceUri.GetLeftPart(UriPartial.Path)) : sourceUri);
				definitionAppId = new DefinitionAppId(uri.AbsoluteUri, new DefinitionIdentity[] { identity, definitionIdentity });
			}
			catch (InvalidDeploymentException)
			{
				flag = true;
			}
			catch (COMException)
			{
				flag = true;
			}
			catch (SEHException)
			{
				flag = true;
			}
			catch (IndexOutOfRangeException)
			{
				flag = true;
			}
			if (!flag)
			{
				SubscriptionState subscriptionState = subStore.GetSubscriptionState(assemblyManifest);
				bool flag2 = false;
				long num;
				using (subStore.AcquireReferenceTransaction(out num))
				{
					flag2 = subStore.CheckAndReferenceApplication(subscriptionState, definitionAppId, num);
				}
				if (flag2 && definitionAppId.Equals(subscriptionState.CurrentBind))
				{
					subState = subscriptionState;
					return subState.CurrentDeploymentManifest;
				}
				flag = true;
			}
			AssemblyManifest assemblyManifest2 = ManifestReader.FromDocument(tempFile.Path, AssemblyManifest.ManifestType.Deployment, sourceUri);
			if (assemblyManifest2.Identity.Version == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_DeploymentManifestNoVersion"));
			}
			if (assemblyManifest2.Deployment == null)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_InvalidDeploymentManifest"));
			}
			return assemblyManifest2;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000EDC0 File Offset: 0x0000DDC0
		private static AssemblyManifest DownloadManifest(ref Uri sourceUri, string targetPath, IDownloadNotification notification, DownloadOptions options, AssemblyManifest.ManifestType manifestType, out ServerInformation serverInformation)
		{
			DownloadManager.DownloadManifestAsRawFile(ref sourceUri, targetPath, notification, options, out serverInformation);
			return ManifestReader.FromDocument(targetPath, manifestType, sourceUri);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000EDD8 File Offset: 0x0000DDD8
		private static void DownloadManifestAsRawFile(ref Uri sourceUri, string targetPath, IDownloadNotification notification, DownloadOptions options, out ServerInformation serverInformation)
		{
			FileDownloader fileDownloader = FileDownloader.Create();
			fileDownloader.Options = options;
			if (notification != null)
			{
				fileDownloader.AddNotification(notification);
			}
			try
			{
				fileDownloader.AddFile(sourceUri, targetPath, 16777216);
				fileDownloader.Download(null);
				sourceUri = fileDownloader.DownloadResults[0].ResponseUri;
				serverInformation = fileDownloader.DownloadResults[0].ServerInformation;
			}
			finally
			{
				if (notification != null)
				{
					fileDownloader.RemoveNotification(notification);
				}
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000EE50 File Offset: 0x0000DE50
		private static void AddDependencies(FileDownloader downloader, AssemblyManifest deployManifest, AssemblyManifest appManifest, Uri sourceUriBase, string targetDirectory, string group)
		{
			long num = 0L;
			global::System.Deployment.Application.Manifest.File[] filesInGroup = appManifest.GetFilesInGroup(group, true);
			DownloadManager.ReorderFilesForIconFile(appManifest, filesInGroup);
			foreach (global::System.Deployment.Application.Manifest.File file in filesInGroup)
			{
				Uri uri = DownloadManager.MapFileSourceUri(deployManifest, sourceUriBase, file.Name);
				DownloadManager.AddFileToDownloader(downloader, deployManifest, appManifest, file, uri, targetDirectory, file.NameFS, file.HashCollection);
				num += (long)file.Size;
			}
			DependentAssembly[] privateAssembliesInGroup = appManifest.GetPrivateAssembliesInGroup(group, true);
			foreach (DependentAssembly dependentAssembly in privateAssembliesInGroup)
			{
				Uri uri = DownloadManager.MapFileSourceUri(deployManifest, sourceUriBase, dependentAssembly.Codebase);
				DownloadManager.AddFileToDownloader(downloader, deployManifest, appManifest, dependentAssembly, uri, targetDirectory, dependentAssembly.CodebaseFS, dependentAssembly.HashCollection);
				num += (long)dependentAssembly.Size;
			}
			downloader.SetExpectedBytesTotal(num);
			if (filesInGroup.Length == 0 && privateAssembliesInGroup.Length == 0)
			{
				throw new InvalidDeploymentException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_NoSuchDownloadGroup"), new object[] { group }));
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000EF55 File Offset: 0x0000DF55
		private static Uri MapFileSourceUri(AssemblyManifest deployManifest, Uri sourceUriBase, string fileName)
		{
			return UriHelper.UriFromRelativeFilePath(sourceUriBase, deployManifest.Deployment.MapFileExtensions ? (fileName + ".deploy") : fileName);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000EF78 File Offset: 0x0000DF78
		private static void AddFileToDownloader(FileDownloader downloader, AssemblyManifest deployManifest, AssemblyManifest appManifest, object manifestElement, Uri fileSourceUri, string targetDirectory, string targetFileName, HashCollection hashCollection)
		{
			string text = Path.Combine(targetDirectory, targetFileName);
			DownloadManager.DependencyDownloadCookie dependencyDownloadCookie = new DownloadManager.DependencyDownloadCookie(manifestElement, deployManifest, appManifest);
			downloader.AddFile(fileSourceUri, text, dependencyDownloadCookie, hashCollection);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000EFA4 File Offset: 0x0000DFA4
		private static void ProcessDownloadedFile(object sender, DownloadEventArgs e)
		{
			if (e.Cookie == null)
			{
				return;
			}
			string fileName = Path.GetFileName(e.FileLocalPath);
			FileDownloader fileDownloader = (FileDownloader)sender;
			if (e.FileResponseUri != null && !e.FileResponseUri.Equals(e.FileSourceUri))
			{
				throw new InvalidDeploymentException(ExceptionTypes.AppFileLocationValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DownloadAppFileAsmRedirected"), new object[] { fileName }));
			}
			DownloadManager.DependencyDownloadCookie dependencyDownloadCookie = (DownloadManager.DependencyDownloadCookie)e.Cookie;
			if (!(dependencyDownloadCookie.ManifestElement is DependentAssembly))
			{
				if (dependencyDownloadCookie.ManifestElement is global::System.Deployment.Application.Manifest.File)
				{
					global::System.Deployment.Application.Manifest.File file = (global::System.Deployment.Application.Manifest.File)dependencyDownloadCookie.ManifestElement;
					fileDownloader.ComponentVerifier.AddFileForVerification(e.FileLocalPath, file.HashCollection);
				}
				return;
			}
			DependentAssembly dependentAssembly = (DependentAssembly)dependencyDownloadCookie.ManifestElement;
			AssemblyManifest deployManifest = dependencyDownloadCookie.DeployManifest;
			AssemblyManifest appManifest = dependencyDownloadCookie.AppManifest;
			AssemblyManifest assemblyManifest = new AssemblyManifest(e.FileLocalPath);
			if (!assemblyManifest.Identity.Matches(dependentAssembly.Identity, true))
			{
				throw new InvalidDeploymentException(ExceptionTypes.RefDefValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DownloadRefDefMismatch"), new object[] { fileName }));
			}
			if (assemblyManifest.Identity.Equals(deployManifest.Identity) || assemblyManifest.Identity.Equals(appManifest.Identity))
			{
				throw new InvalidDeploymentException(ExceptionTypes.ManifestSemanticValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_AppPrivAsmIdSameAsDeployOrApp"), new object[] { assemblyManifest.Identity.ToString() }));
			}
			global::System.Deployment.Application.Manifest.File[] files = assemblyManifest.Files;
			for (int i = 0; i < files.Length; i++)
			{
				Uri uri = DownloadManager.MapFileSourceUri(deployManifest, e.FileSourceUri, files[i].Name);
				if (!uri.AbsoluteUri.Equals(e.FileSourceUri.AbsoluteUri, StringComparison.OrdinalIgnoreCase))
				{
					string directoryName = Path.GetDirectoryName(e.FileLocalPath);
					DownloadManager.AddFileToDownloader(fileDownloader, deployManifest, appManifest, files[i], uri, directoryName, files[i].NameFS, files[i].HashCollection);
				}
			}
			fileDownloader.ComponentVerifier.AddFileForVerification(e.FileLocalPath, dependentAssembly.HashCollection);
			if (assemblyManifest.Identity.PublicKeyToken == null)
			{
				fileDownloader.ComponentVerifier.AddSimplyNamedAssemblyForVerification(e.FileLocalPath, assemblyManifest);
				return;
			}
			fileDownloader.ComponentVerifier.AddStrongNameAssemblyForVerification(e.FileLocalPath, assemblyManifest);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000F200 File Offset: 0x0000E200
		private static void ReorderFilesForIconFile(AssemblyManifest manifest, global::System.Deployment.Application.Manifest.File[] files)
		{
			if (manifest.Description == null || manifest.Description.IconFile == null)
			{
				return;
			}
			for (int i = 0; i < files.Length; i++)
			{
				if (string.Compare(files[i].NameFS, manifest.Description.IconFileFS, StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (i != 0)
					{
						global::System.Deployment.Application.Manifest.File file = files[0];
						files[0] = files[i];
						files[i] = file;
					}
					return;
				}
			}
		}

		// Token: 0x0200004A RID: 74
		private class DependencyDownloadCookie
		{
			// Token: 0x0600025D RID: 605 RVA: 0x0000F25E File Offset: 0x0000E25E
			public DependencyDownloadCookie(object manifestElement, AssemblyManifest deployManifest, AssemblyManifest appManifest)
			{
				this.ManifestElement = manifestElement;
				this.DeployManifest = deployManifest;
				this.AppManifest = appManifest;
			}

			// Token: 0x040001D3 RID: 467
			public readonly object ManifestElement;

			// Token: 0x040001D4 RID: 468
			public readonly AssemblyManifest DeployManifest;

			// Token: 0x040001D5 RID: 469
			public readonly AssemblyManifest AppManifest;
		}
	}
}
