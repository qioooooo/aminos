using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Permissions;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000DB RID: 219
	[Guid("ecabafd1-7f19-11d2-978e-0000f8757e2a")]
	public class ClrObjectFactory : IClrObjectFactory
	{
		// Token: 0x0600050C RID: 1292 RVA: 0x00011784 File Offset: 0x00010784
		public object CreateFromAssembly(string AssemblyName, string TypeName, string Mode)
		{
			object obj2;
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				if (!AssemblyName.StartsWith("System.EnterpriseServices", StringComparison.Ordinal))
				{
					string clientPhysicalPath = Publish.GetClientPhysicalPath(false);
					string text = clientPhysicalPath + TypeName + ".config";
					if (File.Exists(text))
					{
						lock (ClrObjectFactory._htTypes)
						{
							if (!ClrObjectFactory._htTypes.ContainsKey(text))
							{
								RemotingConfiguration.Configure(text, false);
								ClrObjectFactory._htTypes.Add(text, text);
							}
							goto IL_008B;
						}
						goto IL_0076;
						IL_008B:
						Assembly assembly = Assembly.LoadWithPartialName(AssemblyName, null);
						if (assembly == null)
						{
							throw new COMException(Resource.FormatString("Err_ClassNotReg"), Util.REGDB_E_CLASSNOTREG);
						}
						object obj = assembly.CreateInstance(TypeName);
						if (obj == null)
						{
							throw new COMException(Resource.FormatString("Err_ClassNotReg"), Util.REGDB_E_CLASSNOTREG);
						}
						return obj;
					}
					IL_0076:
					throw new COMException(Resource.FormatString("Err_ClassNotReg"), Util.REGDB_E_CLASSNOTREG);
				}
				obj2 = null;
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "ClrObjectFactory.CreateFromAssembly"));
				throw;
			}
			return obj2;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000118B8 File Offset: 0x000108B8
		private string Url2File(string InUrl)
		{
			string text = InUrl.Replace("/", "0");
			text = text.Replace(":", "1");
			text = text.Replace("?", "2");
			text = text.Replace("\\", "3");
			text = text.Replace(".", "4");
			text = text.Replace("\"", "5");
			text = text.Replace("'", "6");
			text = text.Replace(" ", "7");
			text = text.Replace(";", "8");
			text = text.Replace("=", "9");
			text = text.Replace("|", "A");
			text = text.Replace("<", "[");
			return text.Replace(">", "]");
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000119A8 File Offset: 0x000109A8
		public object CreateFromVroot(string VrootUrl, string Mode)
		{
			string text = VrootUrl + "?wsdl";
			return this.CreateFromWsdl(text, Mode);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000119CC File Offset: 0x000109CC
		public object CreateFromWsdl(string WsdlUrl, string Mode)
		{
			object obj2;
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				string clientPhysicalPath = Publish.GetClientPhysicalPath(true);
				string text = "";
				string text2 = this.Url2File(WsdlUrl);
				if (text2.Length + clientPhysicalPath.Length > 250)
				{
					text2 = text2.Remove(0, text2.Length + clientPhysicalPath.Length - 250);
				}
				string text3 = text2 + ".dll";
				if (!File.Exists(clientPhysicalPath + text3))
				{
					GenAssemblyFromWsdl genAssemblyFromWsdl = new GenAssemblyFromWsdl();
					genAssemblyFromWsdl.Run(WsdlUrl, text3, clientPhysicalPath);
				}
				Assembly assembly = Assembly.LoadFrom(clientPhysicalPath + text3);
				Type[] types = assembly.GetTypes();
				for (long num = 0L; num < (long)types.GetLength(0); num += 1L)
				{
					checked
					{
						if (types[(int)((IntPtr)num)].IsClass)
						{
							text = types[(int)((IntPtr)num)].ToString();
						}
					}
				}
				object obj = assembly.CreateInstance(text);
				obj2 = obj;
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "ClrObjectFactory.CreateFromWsdl"));
				throw;
			}
			return obj2;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00011AF8 File Offset: 0x00010AF8
		public object CreateFromMailbox(string Mailbox, string Mode)
		{
			string text = Resource.FormatString("Soap_SmtpNotImplemented");
			ComSoapPublishError.Report(text);
			throw new COMException(text);
		}

		// Token: 0x04000201 RID: 513
		private static Hashtable _htTypes = new Hashtable();
	}
}
