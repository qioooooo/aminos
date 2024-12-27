using System;
using System.Collections;
using System.EnterpriseServices.Thunk;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.MetadataServices;
using System.Threading;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000DE RID: 222
	internal class GenAssemblyFromWsdl
	{
		// Token: 0x0600051B RID: 1307 RVA: 0x00011B60 File Offset: 0x00010B60
		public GenAssemblyFromWsdl()
		{
			this.thisthread = new Thread(new ThreadStart(this.Generate));
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x00011BB8 File Offset: 0x00010BB8
		public void Run(string WsdlUrl, string FileName, string PathName)
		{
			try
			{
				if (WsdlUrl.Length > 0 && FileName.Length > 0)
				{
					this.wsdlurl = WsdlUrl;
					this.filename = PathName + FileName;
					this.pathname = PathName;
					if (!NativeMethods.OpenThreadToken(NativeMethods.GetCurrentThread(), 4U, true, ref this.threadtoken) && Marshal.GetLastWin32Error() != Util.ERROR_NO_TOKEN)
					{
						throw new COMException(Resource.FormatString("Err_OpenThreadToken"), Marshal.GetHRForLastWin32Error());
					}
					SafeUserTokenHandle safeUserTokenHandle = null;
					try
					{
						safeUserTokenHandle = new SafeUserTokenHandle(Security.SuspendImpersonation(), true);
						this.thisthread.Start();
					}
					finally
					{
						if (safeUserTokenHandle != null)
						{
							Security.ResumeImpersonation(safeUserTokenHandle.DangerousGetHandle());
							safeUserTokenHandle.Dispose();
						}
					}
					this.thisthread.Join();
					if (this.ExceptionThrown)
					{
						throw this.SavedException;
					}
				}
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "GenAssemblyFromWsdl.Run"));
				throw;
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x00011CC8 File Offset: 0x00010CC8
		public void Generate()
		{
			try
			{
				if (this.threadtoken != IntPtr.Zero && !NativeMethods.SetThreadToken(IntPtr.Zero, this.threadtoken))
				{
					throw new COMException(Resource.FormatString("Err_SetThreadToken"), Marshal.GetHRForLastWin32Error());
				}
				if (this.wsdlurl.Length > 0)
				{
					Stream stream = new MemoryStream();
					ArrayList arrayList = new ArrayList();
					MetaData.RetrieveSchemaFromUrlToStream(this.wsdlurl, stream);
					stream.Position = 0L;
					MetaData.ConvertSchemaStreamToCodeSourceStream(true, this.pathname, stream, arrayList);
					MetaData.ConvertCodeSourceStreamToAssemblyFile(arrayList, this.filename, null);
				}
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				this.SavedException = ex;
				this.ExceptionThrown = true;
			}
			catch
			{
				string text = Resource.FormatString("Err_NonClsException", "GenAssemblyFromWsdl.Generate");
				ComSoapPublishError.Report(text);
				this.SavedException = new RegistrationException(text);
				this.ExceptionThrown = true;
				throw;
			}
		}

		// Token: 0x04000202 RID: 514
		private const uint TOKEN_IMPERSONATE = 4U;

		// Token: 0x04000203 RID: 515
		private string wsdlurl = "";

		// Token: 0x04000204 RID: 516
		private string filename = "";

		// Token: 0x04000205 RID: 517
		private string pathname = "";

		// Token: 0x04000206 RID: 518
		private Thread thisthread;

		// Token: 0x04000207 RID: 519
		private IntPtr threadtoken = IntPtr.Zero;

		// Token: 0x04000208 RID: 520
		private Exception SavedException;

		// Token: 0x04000209 RID: 521
		private bool ExceptionThrown;
	}
}
