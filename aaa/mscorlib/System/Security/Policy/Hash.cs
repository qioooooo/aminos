using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security.Util;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Policy
{
	// Token: 0x020004AA RID: 1194
	[ComVisible(true)]
	[Serializable]
	public sealed class Hash : ISerializable, IBuiltInEvidence
	{
		// Token: 0x06003049 RID: 12361 RVA: 0x000A650D File Offset: 0x000A550D
		internal Hash()
		{
		}

		// Token: 0x0600304A RID: 12362 RVA: 0x000A6520 File Offset: 0x000A5520
		internal Hash(SerializationInfo info, StreamingContext context)
		{
			this.m_md5 = (byte[])info.GetValueNoThrow("Md5", typeof(byte[]));
			this.m_sha1 = (byte[])info.GetValueNoThrow("Sha1", typeof(byte[]));
			this.m_peFile = SafePEFileHandle.InvalidHandle;
			this.m_rawData = (byte[])info.GetValue("RawData", typeof(byte[]));
			if (this.m_rawData == null)
			{
				IntPtr intPtr = (IntPtr)info.GetValue("PEFile", typeof(IntPtr));
				if (intPtr != IntPtr.Zero)
				{
					Hash._SetPEFileHandle(intPtr, ref this.m_peFile);
				}
			}
		}

		// Token: 0x0600304B RID: 12363 RVA: 0x000A65E5 File Offset: 0x000A55E5
		public Hash(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			Hash._GetPEFileFromAssembly(assembly.InternalAssembly, ref this.m_peFile);
		}

		// Token: 0x0600304C RID: 12364 RVA: 0x000A6618 File Offset: 0x000A5618
		public static Hash CreateSHA1(byte[] sha1)
		{
			if (sha1 == null)
			{
				throw new ArgumentNullException("sha1");
			}
			Hash hash = new Hash();
			hash.m_sha1 = new byte[sha1.Length];
			Array.Copy(sha1, hash.m_sha1, sha1.Length);
			return hash;
		}

		// Token: 0x0600304D RID: 12365 RVA: 0x000A6658 File Offset: 0x000A5658
		public static Hash CreateMD5(byte[] md5)
		{
			if (md5 == null)
			{
				throw new ArgumentNullException("md5");
			}
			Hash hash = new Hash();
			hash.m_md5 = new byte[md5.Length];
			Array.Copy(md5, hash.m_md5, md5.Length);
			return hash;
		}

		// Token: 0x0600304E RID: 12366 RVA: 0x000A6698 File Offset: 0x000A5698
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			info.AddValue("Md5", this.m_md5);
			info.AddValue("Sha1", this.m_sha1);
			if (context.State != StreamingContextStates.Clone && context.State != StreamingContextStates.CrossAppDomain)
			{
				if (!this.m_peFile.IsInvalid)
				{
					this.m_rawData = this.RawData;
				}
				info.AddValue("PEFile", IntPtr.Zero);
				info.AddValue("RawData", this.m_rawData);
				return;
			}
			info.AddValue("PEFile", this.m_peFile.DangerousGetHandle());
			if (this.m_peFile.IsInvalid)
			{
				info.AddValue("RawData", this.m_rawData);
				return;
			}
			info.AddValue("RawData", null);
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x0600304F RID: 12367 RVA: 0x000A6774 File Offset: 0x000A5774
		internal byte[] RawData
		{
			get
			{
				if (this.m_rawData == null)
				{
					if (this.m_peFile.IsInvalid)
					{
						throw new SecurityException(Environment.GetResourceString("Security_CannotGetRawData"));
					}
					byte[] array = Hash._GetRawData(this.m_peFile);
					if (array == null)
					{
						throw new SecurityException(Environment.GetResourceString("Security_CannotGenerateHash"));
					}
					this.m_rawData = array;
				}
				return this.m_rawData;
			}
		}

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06003050 RID: 12368 RVA: 0x000A67D4 File Offset: 0x000A57D4
		public byte[] SHA1
		{
			get
			{
				if (this.m_sha1 == null)
				{
					SHA1 sha = new SHA1Managed();
					this.m_sha1 = sha.ComputeHash(this.RawData);
				}
				byte[] array = new byte[this.m_sha1.Length];
				Array.Copy(this.m_sha1, array, this.m_sha1.Length);
				return array;
			}
		}

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06003051 RID: 12369 RVA: 0x000A6824 File Offset: 0x000A5824
		public byte[] MD5
		{
			get
			{
				if (this.m_md5 == null)
				{
					MD5 md = new MD5CryptoServiceProvider();
					this.m_md5 = md.ComputeHash(this.RawData);
				}
				byte[] array = new byte[this.m_md5.Length];
				Array.Copy(this.m_md5, array, this.m_md5.Length);
				return array;
			}
		}

		// Token: 0x06003052 RID: 12370 RVA: 0x000A6874 File Offset: 0x000A5874
		public byte[] GenerateHash(HashAlgorithm hashAlg)
		{
			if (hashAlg == null)
			{
				throw new ArgumentNullException("hashAlg");
			}
			if (hashAlg is SHA1)
			{
				return this.SHA1;
			}
			if (hashAlg is MD5)
			{
				return this.MD5;
			}
			return hashAlg.ComputeHash(this.RawData);
		}

		// Token: 0x06003053 RID: 12371 RVA: 0x000A68B0 File Offset: 0x000A58B0
		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			if (!verbose)
			{
				return position;
			}
			buffer[position++] = '\b';
			IntPtr intPtr = IntPtr.Zero;
			if (!this.m_peFile.IsInvalid)
			{
				intPtr = this.m_peFile.DangerousGetHandle();
			}
			BuiltInEvidenceHelper.CopyLongToCharArray((long)intPtr, buffer, position);
			return position + 4;
		}

		// Token: 0x06003054 RID: 12372 RVA: 0x000A68FA File Offset: 0x000A58FA
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			if (verbose)
			{
				return 5;
			}
			return 0;
		}

		// Token: 0x06003055 RID: 12373 RVA: 0x000A6904 File Offset: 0x000A5904
		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			this.m_peFile = SafePEFileHandle.InvalidHandle;
			IntPtr intPtr = (IntPtr)BuiltInEvidenceHelper.GetLongFromCharArray(buffer, position);
			Hash._SetPEFileHandle(intPtr, ref this.m_peFile);
			return position + 4;
		}

		// Token: 0x06003056 RID: 12374 RVA: 0x000A6938 File Offset: 0x000A5938
		private SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("System.Security.Policy.Hash");
			securityElement.AddAttribute("version", "1");
			securityElement.AddChild(new SecurityElement("RawData", Hex.EncodeHexString(this.RawData)));
			return securityElement;
		}

		// Token: 0x06003057 RID: 12375 RVA: 0x000A697C File Offset: 0x000A597C
		public override string ToString()
		{
			return this.ToXml().ToString();
		}

		// Token: 0x06003058 RID: 12376
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern byte[] _GetRawData(SafePEFileHandle handle);

		// Token: 0x06003059 RID: 12377
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetPEFileFromAssembly(Assembly assembly, ref SafePEFileHandle handle);

		// Token: 0x0600305A RID: 12378
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _ReleasePEFile(IntPtr handle);

		// Token: 0x0600305B RID: 12379
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void _SetPEFileHandle(IntPtr inHandle, ref SafePEFileHandle outHandle);

		// Token: 0x0400181F RID: 6175
		private SafePEFileHandle m_peFile = SafePEFileHandle.InvalidHandle;

		// Token: 0x04001820 RID: 6176
		private byte[] m_rawData;

		// Token: 0x04001821 RID: 6177
		private byte[] m_sha1;

		// Token: 0x04001822 RID: 6178
		private byte[] m_md5;
	}
}
