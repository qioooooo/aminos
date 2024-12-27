using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000156 RID: 342
	internal class StandardDiskBuildResultCache : DiskBuildResultCache
	{
		// Token: 0x06000FAF RID: 4015 RVA: 0x00045FD0 File Offset: 0x00044FD0
		internal StandardDiskBuildResultCache(string cacheDir)
			: base(cacheDir)
		{
			base.EnsureDiskCacheDirectoryCreated();
			this.FindSatelliteDirectories();
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x00045FE5 File Offset: 0x00044FE5
		private string GetSpecialFilesCombinedHashFileName()
		{
			return BuildManager.WebHashFilePath;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00045FEC File Offset: 0x00044FEC
		internal long GetPreservedSpecialFilesCombinedHash()
		{
			string specialFilesCombinedHashFileName = this.GetSpecialFilesCombinedHashFileName();
			if (!FileUtil.FileExists(specialFilesCombinedHashFileName))
			{
				return 0L;
			}
			long num;
			try
			{
				string text = Util.StringFromFile(specialFilesCombinedHashFileName);
				num = long.Parse(text, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
			}
			catch
			{
				num = 0L;
			}
			return num;
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0004603C File Offset: 0x0004503C
		internal void SavePreservedSpecialFilesCombinedHash(long hash)
		{
			StreamWriter streamWriter = null;
			try
			{
				string specialFilesCombinedHashFileName = this.GetSpecialFilesCombinedHashFileName();
				string directoryName = Path.GetDirectoryName(specialFilesCombinedHashFileName);
				if (!FileUtil.DirectoryExists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				streamWriter = new StreamWriter(specialFilesCombinedHashFileName, false, Encoding.UTF8);
				streamWriter.Write(hash.ToString("x", CultureInfo.InvariantCulture));
			}
			finally
			{
				if (streamWriter != null)
				{
					streamWriter.Close();
				}
			}
		}

		// Token: 0x06000FB3 RID: 4019 RVA: 0x000460A8 File Offset: 0x000450A8
		private void FindSatelliteDirectories()
		{
			string[] directories = Directory.GetDirectories(this._cacheDir);
			foreach (string text in directories)
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
				if (!(fileNameWithoutExtension == "assembly") && !(fileNameWithoutExtension == "hash") && Util.IsCultureName(fileNameWithoutExtension))
				{
					if (StandardDiskBuildResultCache._satelliteDirectories == null)
					{
						StandardDiskBuildResultCache._satelliteDirectories = new ArrayList();
					}
					StandardDiskBuildResultCache._satelliteDirectories.Add(Path.Combine(this._cacheDir, text));
				}
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x0004612C File Offset: 0x0004512C
		internal static void RemoveSatelliteAssemblies(string baseAssemblyName)
		{
			if (StandardDiskBuildResultCache._satelliteDirectories == null)
			{
				return;
			}
			string text = baseAssemblyName + ".resources";
			foreach (object obj in StandardDiskBuildResultCache._satelliteDirectories)
			{
				string text2 = (string)obj;
				string text3 = Path.Combine(text2, text);
				Util.DeleteFileIfExistsNoException(text3 + ".dll");
				Util.DeleteFileIfExistsNoException(text3 + ".pdb");
			}
		}

		// Token: 0x06000FB5 RID: 4021 RVA: 0x000461BC File Offset: 0x000451BC
		internal void RemoveOldTempFiles()
		{
			string text = this._cacheDir + "\\";
			foreach (object obj in ((IEnumerable)FileEnumerator.Create(text)))
			{
				FileData fileData = (FileData)obj;
				if (!fileData.IsDirectory)
				{
					string extension = Path.GetExtension(fileData.Name);
					if (!(extension == ".dll") && !(extension == ".pdb") && !(extension == ".web") && !(extension == ".ccu") && !(extension == ".compiled"))
					{
						if (extension != ".delete")
						{
							int num = fileData.Name.LastIndexOf('.');
							if (num > 0)
							{
								string text2 = fileData.Name.Substring(0, num);
								int num2 = text2.LastIndexOf('.');
								if (num2 > 0)
								{
									text2 = text2.Substring(0, num2);
								}
								if (FileUtil.FileExists(text + text2 + ".dll"))
								{
									continue;
								}
								if (FileUtil.FileExists(text + "App_Web_" + text2 + ".dll"))
								{
									continue;
								}
							}
							Util.DeleteFileNoException(fileData.FullName);
						}
						else
						{
							DiskBuildResultCache.CheckAndRemoveDotDeleteFile(new FileInfo(fileData.FullName));
						}
					}
				}
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00046338 File Offset: 0x00045338
		internal void RemoveAllCodegenFiles()
		{
			foreach (object obj in ((IEnumerable)FileEnumerator.Create(this._cacheDir)))
			{
				FileData fileData = (FileData)obj;
				if (fileData.IsDirectory)
				{
					if (fileData.Name == "assembly" || fileData.Name == "hash" || StringUtil.StringStartsWith(fileData.Name, "Sources_"))
					{
						continue;
					}
					try
					{
						this.DeleteFilesInDirectory(fileData.FullName);
						continue;
					}
					catch
					{
						continue;
					}
				}
				DiskBuildResultCache.TryDeleteFile(fileData.FullName);
			}
			AppDomainSetup setupInformation = Thread.GetDomain().SetupInformation;
			UnsafeNativeMethods.DeleteShadowCache(setupInformation.CachePath, setupInformation.ApplicationName);
		}

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00046414 File Offset: 0x00045414
		internal void DeleteFilesInDirectory(string path)
		{
			foreach (object obj in ((IEnumerable)FileEnumerator.Create(path)))
			{
				FileData fileData = (FileData)obj;
				if (fileData.IsDirectory)
				{
					Directory.Delete(fileData.FullName, true);
				}
				else
				{
					Util.RemoveOrRenameFile(fileData.FullName);
				}
			}
		}

		// Token: 0x04001604 RID: 5636
		private const string fusionCacheDirectoryName = "assembly";

		// Token: 0x04001605 RID: 5637
		private const string webHashDirectoryName = "hash";

		// Token: 0x04001606 RID: 5638
		private static ArrayList _satelliteDirectories;
	}
}
