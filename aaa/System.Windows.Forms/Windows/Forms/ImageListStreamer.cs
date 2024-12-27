using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000445 RID: 1093
	[Serializable]
	public sealed class ImageListStreamer : ISerializable
	{
		// Token: 0x0600416C RID: 16748 RVA: 0x000EA78E File Offset: 0x000E978E
		internal ImageListStreamer(ImageList il)
		{
			this.imageList = il;
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x000EA7A0 File Offset: 0x000E97A0
		private ImageListStreamer(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			if (enumerator == null)
			{
				return;
			}
			while (enumerator.MoveNext())
			{
				if (string.Equals(enumerator.Name, "Data", StringComparison.OrdinalIgnoreCase))
				{
					byte[] array = (byte[])enumerator.Value;
					if (array != null)
					{
						IntPtr intPtr = UnsafeNativeMethods.ThemingScope.Activate();
						try
						{
							MemoryStream memoryStream = new MemoryStream(this.Decompress(array));
							lock (ImageListStreamer.internalSyncObject)
							{
								SafeNativeMethods.InitCommonControls();
								this.nativeImageList = new ImageList.NativeImageList(SafeNativeMethods.ImageList_Read(new UnsafeNativeMethods.ComStreamFromDataStream(memoryStream)));
							}
						}
						finally
						{
							UnsafeNativeMethods.ThemingScope.Deactivate(intPtr);
						}
						if (this.nativeImageList.Handle == IntPtr.Zero)
						{
							throw new InvalidOperationException(SR.GetString("ImageListStreamerLoadFailed"));
						}
					}
				}
			}
		}

		// Token: 0x0600416E RID: 16750 RVA: 0x000EA884 File Offset: 0x000E9884
		private byte[] Compress(byte[] input)
		{
			int num = 0;
			int i = 0;
			int num2 = 0;
			while (i < input.Length)
			{
				byte b = input[i++];
				byte b2 = 1;
				while (i < input.Length && input[i] == b && b2 < 255)
				{
					b2 += 1;
					i++;
				}
				num += 2;
			}
			byte[] array = new byte[num + ImageListStreamer.HEADER_MAGIC.Length];
			Buffer.BlockCopy(ImageListStreamer.HEADER_MAGIC, 0, array, 0, ImageListStreamer.HEADER_MAGIC.Length);
			int num3 = ImageListStreamer.HEADER_MAGIC.Length;
			i = 0;
			while (i < input.Length)
			{
				byte b3 = input[i++];
				byte b4 = 1;
				while (i < input.Length && input[i] == b3 && b4 < 255)
				{
					b4 += 1;
					i++;
				}
				array[num3 + num2++] = b4;
				array[num3 + num2++] = b3;
			}
			return array;
		}

		// Token: 0x0600416F RID: 16751 RVA: 0x000EA954 File Offset: 0x000E9954
		private byte[] Decompress(byte[] input)
		{
			int num = 0;
			int num2 = 0;
			if (input.Length < ImageListStreamer.HEADER_MAGIC.Length)
			{
				return input;
			}
			int i;
			for (i = 0; i < ImageListStreamer.HEADER_MAGIC.Length; i++)
			{
				if (input[i] != ImageListStreamer.HEADER_MAGIC[i])
				{
					return input;
				}
			}
			for (i = ImageListStreamer.HEADER_MAGIC.Length; i < input.Length; i += 2)
			{
				num += (int)input[i];
			}
			byte[] array = new byte[num];
			i = ImageListStreamer.HEADER_MAGIC.Length;
			while (i < input.Length)
			{
				byte b = input[i++];
				byte b2 = input[i++];
				int j = num2;
				int num3 = num2 + (int)b;
				while (j < num3)
				{
					array[j++] = b2;
				}
				num2 += (int)b;
			}
			return array;
		}

		// Token: 0x06004170 RID: 16752 RVA: 0x000EA9FC File Offset: 0x000E99FC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo si, StreamingContext context)
		{
			MemoryStream memoryStream = new MemoryStream();
			IntPtr intPtr = IntPtr.Zero;
			if (this.imageList != null)
			{
				intPtr = this.imageList.Handle;
			}
			else if (this.nativeImageList != null)
			{
				intPtr = this.nativeImageList.Handle;
			}
			if (intPtr == IntPtr.Zero || !this.WriteImageList(intPtr, memoryStream))
			{
				throw new InvalidOperationException(SR.GetString("ImageListStreamerSaveFailed"));
			}
			si.AddValue("Data", this.Compress(memoryStream.ToArray()));
		}

		// Token: 0x06004171 RID: 16753 RVA: 0x000EAA7D File Offset: 0x000E9A7D
		internal ImageList.NativeImageList GetNativeImageList()
		{
			return this.nativeImageList;
		}

		// Token: 0x06004172 RID: 16754 RVA: 0x000EAA88 File Offset: 0x000E9A88
		private bool WriteImageList(IntPtr imagelistHandle, Stream stream)
		{
			try
			{
				int num = SafeNativeMethods.ImageList_WriteEx(new HandleRef(this, imagelistHandle), 1, new UnsafeNativeMethods.ComStreamFromDataStream(stream));
				return num == 0;
			}
			catch (EntryPointNotFoundException)
			{
			}
			return SafeNativeMethods.ImageList_Write(new HandleRef(this, imagelistHandle), new UnsafeNativeMethods.ComStreamFromDataStream(stream));
		}

		// Token: 0x04001F83 RID: 8067
		private static readonly byte[] HEADER_MAGIC = new byte[] { 77, 83, 70, 116 };

		// Token: 0x04001F84 RID: 8068
		private static object internalSyncObject = new object();

		// Token: 0x04001F85 RID: 8069
		private ImageList imageList;

		// Token: 0x04001F86 RID: 8070
		private ImageList.NativeImageList nativeImageList;
	}
}
