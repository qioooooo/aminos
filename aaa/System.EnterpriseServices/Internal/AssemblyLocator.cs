using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000B6 RID: 182
	[Guid("458aa3b5-265a-4b75-bc05-9bea4630cf18")]
	public class AssemblyLocator : MarshalByRefObject, IAssemblyLocator
	{
		// Token: 0x06000456 RID: 1110 RVA: 0x0000D82C File Offset: 0x0000C82C
		string[] IAssemblyLocator.GetModules(string appdir, string appName, string name)
		{
			if (appdir != null && appdir.Length > 0)
			{
				AssemblyLocator assemblyLocator = null;
				try
				{
					AppDomain appDomain = AppDomain.CreateDomain(appName, null, new AppDomainSetup
					{
						ApplicationBase = appdir
					});
					if (appDomain != null)
					{
						ObjectHandle objectHandle = appDomain.CreateInstance(typeof(AssemblyLocator).Assembly.FullName, typeof(AssemblyLocator).FullName);
						if (objectHandle != null)
						{
							assemblyLocator = (AssemblyLocator)objectHandle.Unwrap();
						}
					}
				}
				catch (Exception)
				{
					return null;
				}
				catch
				{
					return null;
				}
				return ((IAssemblyLocator)assemblyLocator).GetModules(null, null, name);
			}
			string[] array2;
			try
			{
				Module[] modules = Assembly.Load(name).GetModules();
				string[] array = new string[modules.Length];
				for (int i = 0; i < modules.Length; i++)
				{
					array[i] = modules[i].FullyQualifiedName;
				}
				array2 = array;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			catch
			{
				throw;
			}
			return array2;
		}
	}
}
