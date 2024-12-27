using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using Microsoft.Win32;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000D6 RID: 214
	internal class AssemblyManager : MarshalByRefObject
	{
		// Token: 0x060004DA RID: 1242 RVA: 0x0000FC40 File Offset: 0x0000EC40
		internal string InternalGetGacName(string fName)
		{
			string text = "";
			try
			{
				AssemblyName assemblyName = AssemblyName.GetAssemblyName(fName);
				text = assemblyName.Name + ",Version=" + assemblyName.Version.ToString();
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "AssemblyManager.InternalGetGacName"));
			}
			return text;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0000FCBC File Offset: 0x0000ECBC
		public string GetGacName(string fName)
		{
			string text = "";
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			AppDomain appDomain = AppDomain.CreateDomain("SoapDomain", null, appDomainSetup);
			if (appDomain != null)
			{
				try
				{
					ObjectHandle objectHandle = appDomain.CreateInstance(typeof(AssemblyManager).Assembly.FullName, typeof(AssemblyManager).FullName);
					if (objectHandle != null)
					{
						AssemblyManager assemblyManager = (AssemblyManager)objectHandle.Unwrap();
						text = assemblyManager.InternalGetGacName(fName);
					}
				}
				finally
				{
					AppDomain.Unload(appDomain);
				}
			}
			return text;
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0000FD48 File Offset: 0x0000ED48
		internal string InternalGetFullName(string fName, string strAssemblyName)
		{
			string text = "";
			try
			{
				if (File.Exists(fName))
				{
					AssemblyName assemblyName = AssemblyName.GetAssemblyName(fName);
					text = assemblyName.FullName;
				}
				else
				{
					try
					{
						Assembly assembly = Assembly.LoadWithPartialName(strAssemblyName, null);
						text = assembly.FullName;
					}
					catch
					{
						throw new RegistrationException(Resource.FormatString("ServicedComponentException_AssemblyNotInGAC"));
					}
				}
			}
			catch (RegistrationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "AssemblyManager.InternalGetFullName"));
			}
			return text;
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0000FDF4 File Offset: 0x0000EDF4
		public string GetFullName(string fName, string strAssemblyName)
		{
			string text = "";
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			AppDomain appDomain = AppDomain.CreateDomain("SoapDomain", null, appDomainSetup);
			if (appDomain != null)
			{
				try
				{
					ObjectHandle objectHandle = appDomain.CreateInstance(typeof(AssemblyManager).Assembly.FullName, typeof(AssemblyManager).FullName);
					if (objectHandle != null)
					{
						AssemblyManager assemblyManager = (AssemblyManager)objectHandle.Unwrap();
						text = assemblyManager.InternalGetFullName(fName, strAssemblyName);
					}
				}
				finally
				{
					AppDomain.Unload(appDomain);
				}
			}
			return text;
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0000FE80 File Offset: 0x0000EE80
		internal string InternalGetTypeNameFromClassId(string assemblyPath, string classId)
		{
			string text = "";
			Assembly assembly = Assembly.LoadFrom(assemblyPath);
			Guid guid = new Guid(classId);
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (guid.Equals(type.GUID))
				{
					text = type.FullName;
					break;
				}
			}
			return text;
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0000FEE4 File Offset: 0x0000EEE4
		internal string InternalGetTypeNameFromProgId(string AssemblyPath, string ProgId)
		{
			string text = "";
			Assembly assembly = Assembly.LoadFrom(AssemblyPath);
			try
			{
				RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(ProgId + "\\CLSID");
				string text2 = (string)registryKey.GetValue("");
				Guid guid = new Guid(text2);
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (guid.Equals(type.GUID))
					{
						text = type.FullName;
						break;
					}
				}
			}
			catch
			{
				text = string.Empty;
				throw;
			}
			return text;
		}

		// Token: 0x060004E0 RID: 1248
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern bool CopyFile(string source, string dest, bool failifexists);

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000FF8C File Offset: 0x0000EF8C
		internal bool GetFromCache(string AssemblyPath, string srcTypeLib)
		{
			try
			{
				string cacheName = CacheInfo.GetCacheName(AssemblyPath, srcTypeLib);
				if (File.Exists(cacheName))
				{
					return AssemblyManager.CopyFile(cacheName, AssemblyPath, true);
				}
				return false;
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "AssemblyManager.GetFromCache"));
			}
			return false;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0000FFFC File Offset: 0x0000EFFC
		internal bool CopyToCache(string AssemblyPath, string srcTypeLib)
		{
			bool flag = false;
			try
			{
				string cacheName = CacheInfo.GetCacheName(AssemblyPath, srcTypeLib);
				if (File.Exists(cacheName))
				{
					return true;
				}
				return AssemblyManager.CopyFile(AssemblyPath, cacheName, false);
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "AssemblyManager.CopyToCache"));
			}
			return flag;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00010070 File Offset: 0x0000F070
		internal bool CompareToCache(string AssemblyPath, string srcTypeLib)
		{
			bool flag = true;
			try
			{
				string cacheName = CacheInfo.GetCacheName(AssemblyPath, srcTypeLib);
				if (!File.Exists(AssemblyPath))
				{
					return false;
				}
				if (!File.Exists(cacheName))
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				flag = false;
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				flag = false;
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "AssemblyManager.CompareToCache"));
			}
			return flag;
		}
	}
}
