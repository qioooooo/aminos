using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Drawing.Imaging
{
	// Token: 0x020000BD RID: 189
	public sealed class ImageCodecInfo
	{
		// Token: 0x06000B9E RID: 2974 RVA: 0x00022B98 File Offset: 0x00021B98
		internal ImageCodecInfo()
		{
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00022BA0 File Offset: 0x00021BA0
		// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x00022BA8 File Offset: 0x00021BA8
		public Guid Clsid
		{
			get
			{
				return this.clsid;
			}
			set
			{
				this.clsid = value;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00022BB1 File Offset: 0x00021BB1
		// (set) Token: 0x06000BA2 RID: 2978 RVA: 0x00022BB9 File Offset: 0x00021BB9
		public Guid FormatID
		{
			get
			{
				return this.formatID;
			}
			set
			{
				this.formatID = value;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000BA3 RID: 2979 RVA: 0x00022BC2 File Offset: 0x00021BC2
		// (set) Token: 0x06000BA4 RID: 2980 RVA: 0x00022BCA File Offset: 0x00021BCA
		public string CodecName
		{
			get
			{
				return this.codecName;
			}
			set
			{
				this.codecName = value;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000BA5 RID: 2981 RVA: 0x00022BD3 File Offset: 0x00021BD3
		// (set) Token: 0x06000BA6 RID: 2982 RVA: 0x00022BF4 File Offset: 0x00021BF4
		public string DllName
		{
			get
			{
				if (this.dllName != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this.dllName).Demand();
				}
				return this.dllName;
			}
			set
			{
				if (value != null)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, value).Demand();
				}
				this.dllName = value;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000BA7 RID: 2983 RVA: 0x00022C0C File Offset: 0x00021C0C
		// (set) Token: 0x06000BA8 RID: 2984 RVA: 0x00022C14 File Offset: 0x00021C14
		public string FormatDescription
		{
			get
			{
				return this.formatDescription;
			}
			set
			{
				this.formatDescription = value;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000BA9 RID: 2985 RVA: 0x00022C1D File Offset: 0x00021C1D
		// (set) Token: 0x06000BAA RID: 2986 RVA: 0x00022C25 File Offset: 0x00021C25
		public string FilenameExtension
		{
			get
			{
				return this.filenameExtension;
			}
			set
			{
				this.filenameExtension = value;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x00022C2E File Offset: 0x00021C2E
		// (set) Token: 0x06000BAC RID: 2988 RVA: 0x00022C36 File Offset: 0x00021C36
		public string MimeType
		{
			get
			{
				return this.mimeType;
			}
			set
			{
				this.mimeType = value;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x00022C3F File Offset: 0x00021C3F
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x00022C47 File Offset: 0x00021C47
		public ImageCodecFlags Flags
		{
			get
			{
				return this.flags;
			}
			set
			{
				this.flags = value;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00022C50 File Offset: 0x00021C50
		// (set) Token: 0x06000BB0 RID: 2992 RVA: 0x00022C58 File Offset: 0x00021C58
		public int Version
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00022C61 File Offset: 0x00021C61
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x00022C69 File Offset: 0x00021C69
		[CLSCompliant(false)]
		public byte[][] SignaturePatterns
		{
			get
			{
				return this.signaturePatterns;
			}
			set
			{
				this.signaturePatterns = value;
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x00022C72 File Offset: 0x00021C72
		// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x00022C7A File Offset: 0x00021C7A
		[CLSCompliant(false)]
		public byte[][] SignatureMasks
		{
			get
			{
				return this.signatureMasks;
			}
			set
			{
				this.signatureMasks = value;
			}
		}

		// Token: 0x06000BB5 RID: 2997 RVA: 0x00022C84 File Offset: 0x00021C84
		public static ImageCodecInfo[] GetImageDecoders()
		{
			int num2;
			int num3;
			int num = SafeNativeMethods.Gdip.GdipGetImageDecodersSize(out num2, out num3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num3);
			ImageCodecInfo[] array;
			try
			{
				num = SafeNativeMethods.Gdip.GdipGetImageDecoders(num2, num3, intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				array = ImageCodecInfo.ConvertFromMemory(intPtr, num2);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return array;
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x00022CE8 File Offset: 0x00021CE8
		public static ImageCodecInfo[] GetImageEncoders()
		{
			int num2;
			int num3;
			int num = SafeNativeMethods.Gdip.GdipGetImageEncodersSize(out num2, out num3);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			IntPtr intPtr = Marshal.AllocHGlobal(num3);
			ImageCodecInfo[] array;
			try
			{
				num = SafeNativeMethods.Gdip.GdipGetImageEncoders(num2, num3, intPtr);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				array = ImageCodecInfo.ConvertFromMemory(intPtr, num2);
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
			return array;
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x00022D4C File Offset: 0x00021D4C
		private static ImageCodecInfo[] ConvertFromMemory(IntPtr memoryStart, int numCodecs)
		{
			ImageCodecInfo[] array = new ImageCodecInfo[numCodecs];
			for (int i = 0; i < numCodecs; i++)
			{
				IntPtr intPtr = (IntPtr)((long)memoryStart + (long)(Marshal.SizeOf(typeof(ImageCodecInfoPrivate)) * i));
				ImageCodecInfoPrivate imageCodecInfoPrivate = new ImageCodecInfoPrivate();
				UnsafeNativeMethods.PtrToStructure(intPtr, imageCodecInfoPrivate);
				array[i] = new ImageCodecInfo();
				array[i].Clsid = imageCodecInfoPrivate.Clsid;
				array[i].FormatID = imageCodecInfoPrivate.FormatID;
				array[i].CodecName = Marshal.PtrToStringUni(imageCodecInfoPrivate.CodecName);
				array[i].DllName = Marshal.PtrToStringUni(imageCodecInfoPrivate.DllName);
				array[i].FormatDescription = Marshal.PtrToStringUni(imageCodecInfoPrivate.FormatDescription);
				array[i].FilenameExtension = Marshal.PtrToStringUni(imageCodecInfoPrivate.FilenameExtension);
				array[i].MimeType = Marshal.PtrToStringUni(imageCodecInfoPrivate.MimeType);
				array[i].Flags = (ImageCodecFlags)imageCodecInfoPrivate.Flags;
				array[i].Version = imageCodecInfoPrivate.Version;
				array[i].SignaturePatterns = new byte[imageCodecInfoPrivate.SigCount][];
				array[i].SignatureMasks = new byte[imageCodecInfoPrivate.SigCount][];
				for (int j = 0; j < imageCodecInfoPrivate.SigCount; j++)
				{
					array[i].SignaturePatterns[j] = new byte[imageCodecInfoPrivate.SigSize];
					array[i].SignatureMasks[j] = new byte[imageCodecInfoPrivate.SigSize];
					Marshal.Copy((IntPtr)((long)imageCodecInfoPrivate.SigMask + (long)(j * imageCodecInfoPrivate.SigSize)), array[i].SignatureMasks[j], 0, imageCodecInfoPrivate.SigSize);
					Marshal.Copy((IntPtr)((long)imageCodecInfoPrivate.SigPattern + (long)(j * imageCodecInfoPrivate.SigSize)), array[i].SignaturePatterns[j], 0, imageCodecInfoPrivate.SigSize);
				}
			}
			return array;
		}

		// Token: 0x04000A2C RID: 2604
		private Guid clsid;

		// Token: 0x04000A2D RID: 2605
		private Guid formatID;

		// Token: 0x04000A2E RID: 2606
		private string codecName;

		// Token: 0x04000A2F RID: 2607
		private string dllName;

		// Token: 0x04000A30 RID: 2608
		private string formatDescription;

		// Token: 0x04000A31 RID: 2609
		private string filenameExtension;

		// Token: 0x04000A32 RID: 2610
		private string mimeType;

		// Token: 0x04000A33 RID: 2611
		private ImageCodecFlags flags;

		// Token: 0x04000A34 RID: 2612
		private int version;

		// Token: 0x04000A35 RID: 2613
		private byte[][] signaturePatterns;

		// Token: 0x04000A36 RID: 2614
		private byte[][] signatureMasks;
	}
}
