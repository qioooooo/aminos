using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Reflection
{
	// Token: 0x02000329 RID: 809
	[ComVisible(true)]
	[Serializable]
	public class StrongNameKeyPair : IDeserializationCallback, ISerializable
	{
		// Token: 0x06001F70 RID: 8048 RVA: 0x0004F8CC File Offset: 0x0004E8CC
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public StrongNameKeyPair(FileStream keyPairFile)
		{
			if (keyPairFile == null)
			{
				throw new ArgumentNullException("keyPairFile");
			}
			int num = (int)keyPairFile.Length;
			this._keyPairArray = new byte[num];
			keyPairFile.Read(this._keyPairArray, 0, num);
			this._keyPairExported = true;
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x0004F917 File Offset: 0x0004E917
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public StrongNameKeyPair(byte[] keyPairArray)
		{
			if (keyPairArray == null)
			{
				throw new ArgumentNullException("keyPairArray");
			}
			this._keyPairArray = new byte[keyPairArray.Length];
			Array.Copy(keyPairArray, this._keyPairArray, keyPairArray.Length);
			this._keyPairExported = true;
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x0004F951 File Offset: 0x0004E951
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public StrongNameKeyPair(string keyPairContainer)
		{
			if (keyPairContainer == null)
			{
				throw new ArgumentNullException("keyPairContainer");
			}
			this._keyPairContainer = keyPairContainer;
			this._keyPairExported = false;
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x0004F978 File Offset: 0x0004E978
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected StrongNameKeyPair(SerializationInfo info, StreamingContext context)
		{
			this._keyPairExported = (bool)info.GetValue("_keyPairExported", typeof(bool));
			this._keyPairArray = (byte[])info.GetValue("_keyPairArray", typeof(byte[]));
			this._keyPairContainer = (string)info.GetValue("_keyPairContainer", typeof(string));
			this._publicKey = (byte[])info.GetValue("_publicKey", typeof(byte[]));
		}

		// Token: 0x17000544 RID: 1348
		// (get) Token: 0x06001F74 RID: 8052 RVA: 0x0004FA0C File Offset: 0x0004EA0C
		public byte[] PublicKey
		{
			get
			{
				if (this._publicKey == null)
				{
					this._publicKey = this.nGetPublicKey(this._keyPairExported, this._keyPairArray, this._keyPairContainer);
				}
				byte[] array = new byte[this._publicKey.Length];
				Array.Copy(this._publicKey, array, this._publicKey.Length);
				return array;
			}
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x0004FA64 File Offset: 0x0004EA64
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_keyPairExported", this._keyPairExported);
			info.AddValue("_keyPairArray", this._keyPairArray);
			info.AddValue("_keyPairContainer", this._keyPairContainer);
			info.AddValue("_publicKey", this._publicKey);
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x0004FAB5 File Offset: 0x0004EAB5
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x0004FAB7 File Offset: 0x0004EAB7
		private bool GetKeyPair(out object arrayOrContainer)
		{
			arrayOrContainer = (this._keyPairExported ? this._keyPairArray : this._keyPairContainer);
			return this._keyPairExported;
		}

		// Token: 0x06001F78 RID: 8056
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern byte[] nGetPublicKey(bool exported, byte[] array, string container);

		// Token: 0x04000D6D RID: 3437
		private bool _keyPairExported;

		// Token: 0x04000D6E RID: 3438
		private byte[] _keyPairArray;

		// Token: 0x04000D6F RID: 3439
		private string _keyPairContainer;

		// Token: 0x04000D70 RID: 3440
		private byte[] _publicKey;
	}
}
