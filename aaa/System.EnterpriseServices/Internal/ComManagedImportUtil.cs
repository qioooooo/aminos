using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000BB RID: 187
	[Guid("3b0398c9-7812-4007-85cb-18c771f2206f")]
	public class ComManagedImportUtil : IComManagedImportUtil
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x0000DAB8 File Offset: 0x0000CAB8
		public void GetComponentInfo(string assemblyPath, out string numComponents, out string componentInfo)
		{
			RegistrationServices registrationServices = new RegistrationServices();
			Assembly assembly = this.LoadAssembly(assemblyPath);
			Type[] registrableTypesInAssembly = registrationServices.GetRegistrableTypesInAssembly(assembly);
			int num = 0;
			string text = "";
			foreach (Type type in registrableTypesInAssembly)
			{
				if (type.IsClass && type.IsSubclassOf(typeof(ServicedComponent)))
				{
					num++;
					string text2 = Marshal.GenerateGuidForType(type).ToString();
					string text3 = Marshal.GenerateProgIdForType(type);
					if (text2.Length == 0 || text3.Length == 0)
					{
						throw new COMException();
					}
					string text4 = text;
					text = string.Concat(new string[] { text4, text3, ",{", text2, "}," });
				}
			}
			numComponents = num.ToString(CultureInfo.InvariantCulture);
			componentInfo = text;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000DBB0 File Offset: 0x0000CBB0
		private Assembly LoadAssembly(string assemblyFile)
		{
			string text = Path.GetFullPath(assemblyFile).ToLower(CultureInfo.InvariantCulture);
			bool flag = false;
			string directoryName = Path.GetDirectoryName(text);
			string currentDirectory = Environment.CurrentDirectory;
			if (currentDirectory != directoryName)
			{
				Environment.CurrentDirectory = directoryName;
				flag = true;
			}
			Assembly assembly = null;
			try
			{
				assembly = Assembly.LoadFrom(text);
			}
			catch
			{
			}
			if (flag)
			{
				Environment.CurrentDirectory = currentDirectory;
			}
			return assembly;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000DC1C File Offset: 0x0000CC1C
		public void InstallAssembly(string asmpath, string parname, string appname)
		{
			try
			{
				string text = null;
				InstallationFlags installationFlags = InstallationFlags.Default;
				RegistrationHelper registrationHelper = new RegistrationHelper();
				registrationHelper.InstallAssembly(asmpath, ref appname, parname, ref text, installationFlags);
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry(Resource.FormatString("Reg_InstallTitle"), Resource.FormatString("Reg_FailInstall", asmpath, appname) + "\n\n" + ex.ToString(), EventLogEntryType.Error);
				throw;
			}
			catch
			{
				EventLog.WriteEntry(Resource.FormatString("Reg_InstallTitle"), Resource.FormatString("Reg_FailInstall", asmpath, appname) + "\n\n" + Resource.FormatString("Err_NonClsException", "ComManagedImportUtil.InstallAssembly"), EventLogEntryType.Error);
				throw;
			}
		}
	}
}
