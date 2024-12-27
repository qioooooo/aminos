using System;
using System.Data.Common;
using System.Globalization;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Data.OracleClient
{
	// Token: 0x02000065 RID: 101
	[Serializable]
	public sealed class OracleException : DbException
	{
		// Token: 0x060004A1 RID: 1185 RVA: 0x00066CC4 File Offset: 0x000660C4
		private OracleException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
			this._code = (int)si.GetValue("code", typeof(int));
			base.HResult = -2146232008;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00066D04 File Offset: 0x00066104
		private OracleException(string message, int code)
			: base(message)
		{
			this._code = code;
			base.HResult = -2146232008;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x00066D2C File Offset: 0x0006612C
		public int Code
		{
			get
			{
				return this._code;
			}
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00066D40 File Offset: 0x00066140
		private static bool ConnectionIsBroken(int code)
		{
			bool flag;
			if (12500 <= code && code <= 12699)
			{
				flag = true;
			}
			else
			{
				if (code <= 1012)
				{
					if (code <= 24)
					{
						switch (code)
						{
						case 18:
						case 19:
							break;
						default:
							if (code != 24)
							{
								goto IL_00A5;
							}
							break;
						}
					}
					else if (code != 28 && code != 436 && code != 1012)
					{
						goto IL_00A5;
					}
				}
				else if (code <= 1075)
				{
					switch (code)
					{
					case 1033:
					case 1034:
						break;
					default:
						if (code != 1075)
						{
							goto IL_00A5;
						}
						break;
					}
				}
				else if (code != 2392 && code != 2399)
				{
					switch (code)
					{
					case 3113:
					case 3114:
						break;
					default:
						goto IL_00A5;
					}
				}
				return true;
				IL_00A5:
				flag = false;
			}
			return flag;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00066DF8 File Offset: 0x000661F8
		internal static OracleException CreateException(OciErrorHandle errorHandle, int rc)
		{
			OracleException ex;
			using (NativeBuffer nativeBuffer = new NativeBuffer_Exception(1000))
			{
				int num3;
				string text;
				if (errorHandle != null)
				{
					int num = 1;
					int num2 = TracedNativeMethods.OCIErrorGet(errorHandle, num, out num3, nativeBuffer);
					if (num2 == 0)
					{
						text = errorHandle.PtrToString(nativeBuffer);
						if (num3 != 0 && text.StartsWith("ORA-00000", StringComparison.Ordinal) && TracedNativeMethods.oermsg((short)num3, nativeBuffer) == 0)
						{
							text = errorHandle.PtrToString(nativeBuffer);
						}
					}
					else
					{
						text = Res.GetString("ADP_NoMessageAvailable", new object[] { rc, num2 });
						num3 = 0;
					}
					if (OracleException.ConnectionIsBroken(num3))
					{
						errorHandle.ConnectionIsBroken = true;
					}
				}
				else
				{
					text = Res.GetString("ADP_NoMessageAvailable", new object[] { rc, -1 });
					num3 = 0;
				}
				ex = new OracleException(text, num3);
			}
			return ex;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00066EF4 File Offset: 0x000662F4
		internal static OracleException CreateException(int rc, OracleInternalConnection internalConnection)
		{
			OracleException ex;
			using (NativeBuffer nativeBuffer = new NativeBuffer_Exception(1000))
			{
				int length = nativeBuffer.Length;
				int num = 0;
				int num2 = TracedNativeMethods.OraMTSOCIErrGet(ref num, nativeBuffer, ref length);
				string text;
				if (1 == num2)
				{
					text = nativeBuffer.PtrToStringAnsi(0, length);
				}
				else
				{
					text = Res.GetString("ADP_NoMessageAvailable", new object[] { rc, num2 });
					num = 0;
				}
				if (OracleException.ConnectionIsBroken(num))
				{
					internalConnection.DoomThisConnection();
				}
				ex = new OracleException(text, num);
			}
			return ex;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00066F9C File Offset: 0x0006639C
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		internal static void Check(OciErrorHandle errorHandle, int rc)
		{
			OCI.RETURNCODE returncode = (OCI.RETURNCODE)rc;
			switch (returncode)
			{
			case OCI.RETURNCODE.OCI_INVALID_HANDLE:
				throw ADP.InvalidOperation(Res.GetString("ADP_InternalError", new object[] { rc }));
			case OCI.RETURNCODE.OCI_ERROR:
				break;
			default:
				if (returncode != OCI.RETURNCODE.OCI_NO_DATA)
				{
					if (rc < 0 || rc == 99 || rc == 1)
					{
						throw ADP.Simple(Res.GetString("ADP_UnexpectedReturnCode", new object[] { rc.ToString(CultureInfo.CurrentCulture) }));
					}
					return;
				}
				break;
			}
			throw ADP.OracleError(errorHandle, rc);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00067020 File Offset: 0x00066420
		internal static void Check(int rc, OracleInternalConnection internalConnection)
		{
			if (rc != 0)
			{
				throw ADP.OracleError(rc, internalConnection);
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00067038 File Offset: 0x00066438
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (si == null)
			{
				throw new ArgumentNullException("si");
			}
			si.AddValue("code", this._code, typeof(int));
			base.GetObjectData(si, context);
		}

		// Token: 0x0400042B RID: 1067
		private int _code;
	}
}
