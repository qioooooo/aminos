using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020002BC RID: 700
	[Editor("System.Drawing.Design.CursorEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[TypeConverter(typeof(CursorConverter))]
	[Serializable]
	public sealed class Cursor : IDisposable, ISerializable
	{
		// Token: 0x060026A6 RID: 9894 RVA: 0x0005EA7C File Offset: 0x0005DA7C
		internal Cursor(SerializationInfo info, StreamingContext context)
		{
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			if (enumerator == null)
			{
				return;
			}
			while (enumerator.MoveNext())
			{
				if (string.Equals(enumerator.Name, "CursorData", StringComparison.OrdinalIgnoreCase))
				{
					this.cursorData = (byte[])enumerator.Value;
					if (this.cursorData != null)
					{
						this.LoadPicture(new UnsafeNativeMethods.ComStreamFromDataStream(new MemoryStream(this.cursorData)));
					}
				}
				else if (string.Compare(enumerator.Name, "CursorResourceId", true, CultureInfo.InvariantCulture) == 0)
				{
					this.LoadFromResourceId((int)enumerator.Value);
				}
			}
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x0005EB21 File Offset: 0x0005DB21
		internal Cursor(int nResourceId, int dummy)
		{
			this.LoadFromResourceId(nResourceId);
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x0005EB44 File Offset: 0x0005DB44
		internal Cursor(string resource, int dummy)
		{
			Stream manifestResourceStream = typeof(Cursor).Module.Assembly.GetManifestResourceStream(typeof(Cursor), resource);
			this.cursorData = new byte[manifestResourceStream.Length];
			manifestResourceStream.Read(this.cursorData, 0, Convert.ToInt32(manifestResourceStream.Length));
			this.LoadPicture(new UnsafeNativeMethods.ComStreamFromDataStream(new MemoryStream(this.cursorData)));
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x0005EBD0 File Offset: 0x0005DBD0
		public Cursor(IntPtr handle)
		{
			IntSecurity.UnmanagedCode.Demand();
			if (handle == IntPtr.Zero)
			{
				throw new ArgumentException(SR.GetString("InvalidGDIHandle", new object[] { typeof(Cursor).Name }));
			}
			this.handle = handle;
			this.ownHandle = false;
		}

		// Token: 0x060026AA RID: 9898 RVA: 0x0005EC44 File Offset: 0x0005DC44
		public Cursor(string fileName)
		{
			FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			try
			{
				this.cursorData = new byte[fileStream.Length];
				fileStream.Read(this.cursorData, 0, Convert.ToInt32(fileStream.Length));
			}
			finally
			{
				fileStream.Close();
			}
			this.LoadPicture(new UnsafeNativeMethods.ComStreamFromDataStream(new MemoryStream(this.cursorData)));
		}

		// Token: 0x060026AB RID: 9899 RVA: 0x0005ECD0 File Offset: 0x0005DCD0
		public Cursor(Type type, string resource)
			: this(type.Module.Assembly.GetManifestResourceStream(type, resource))
		{
		}

		// Token: 0x060026AC RID: 9900 RVA: 0x0005ECEC File Offset: 0x0005DCEC
		public Cursor(Stream stream)
		{
			this.cursorData = new byte[stream.Length];
			stream.Read(this.cursorData, 0, Convert.ToInt32(stream.Length));
			this.LoadPicture(new UnsafeNativeMethods.ComStreamFromDataStream(new MemoryStream(this.cursorData)));
		}

		// Token: 0x17000618 RID: 1560
		// (get) Token: 0x060026AD RID: 9901 RVA: 0x0005ED52 File Offset: 0x0005DD52
		// (set) Token: 0x060026AE RID: 9902 RVA: 0x0005ED59 File Offset: 0x0005DD59
		public static Rectangle Clip
		{
			get
			{
				return Cursor.ClipInternal;
			}
			set
			{
				if (!value.IsEmpty)
				{
					IntSecurity.AdjustCursorClip.Demand();
				}
				Cursor.ClipInternal = value;
			}
		}

		// Token: 0x17000619 RID: 1561
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x0005ED74 File Offset: 0x0005DD74
		// (set) Token: 0x060026B0 RID: 9904 RVA: 0x0005EDB4 File Offset: 0x0005DDB4
		internal static Rectangle ClipInternal
		{
			get
			{
				NativeMethods.RECT rect = default(NativeMethods.RECT);
				SafeNativeMethods.GetClipCursor(ref rect);
				return Rectangle.FromLTRB(rect.left, rect.top, rect.right, rect.bottom);
			}
			set
			{
				if (value.IsEmpty)
				{
					UnsafeNativeMethods.ClipCursor(null);
					return;
				}
				NativeMethods.RECT rect = NativeMethods.RECT.FromXYWH(value.X, value.Y, value.Width, value.Height);
				UnsafeNativeMethods.ClipCursor(ref rect);
			}
		}

		// Token: 0x1700061A RID: 1562
		// (get) Token: 0x060026B1 RID: 9905 RVA: 0x0005EDFC File Offset: 0x0005DDFC
		// (set) Token: 0x060026B2 RID: 9906 RVA: 0x0005EE03 File Offset: 0x0005DE03
		public static Cursor Current
		{
			get
			{
				return Cursor.CurrentInternal;
			}
			set
			{
				IntSecurity.ModifyCursor.Demand();
				Cursor.CurrentInternal = value;
			}
		}

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x060026B3 RID: 9907 RVA: 0x0005EE18 File Offset: 0x0005DE18
		// (set) Token: 0x060026B4 RID: 9908 RVA: 0x0005EE3C File Offset: 0x0005DE3C
		internal static Cursor CurrentInternal
		{
			get
			{
				IntPtr cursor = SafeNativeMethods.GetCursor();
				IntSecurity.UnmanagedCode.Assert();
				return Cursors.KnownCursorFromHCursor(cursor);
			}
			set
			{
				IntPtr intPtr = ((value == null) ? IntPtr.Zero : value.handle);
				UnsafeNativeMethods.SetCursor(new HandleRef(value, intPtr));
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x060026B5 RID: 9909 RVA: 0x0005EE70 File Offset: 0x0005DE70
		public IntPtr Handle
		{
			get
			{
				if (this.handle == IntPtr.Zero)
				{
					throw new ObjectDisposedException(SR.GetString("ObjectDisposed", new object[] { base.GetType().Name }));
				}
				return this.handle;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x060026B6 RID: 9910 RVA: 0x0005EEBC File Offset: 0x0005DEBC
		public Point HotSpot
		{
			get
			{
				Point point = Point.Empty;
				NativeMethods.ICONINFO iconinfo = new NativeMethods.ICONINFO();
				Icon icon = null;
				IntSecurity.ObjectFromWin32Handle.Assert();
				try
				{
					icon = Icon.FromHandle(this.Handle);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				try
				{
					SafeNativeMethods.GetIconInfo(new HandleRef(this, icon.Handle), iconinfo);
					point = new Point(iconinfo.xHotspot, iconinfo.yHotspot);
				}
				finally
				{
					if (iconinfo.hbmMask != IntPtr.Zero)
					{
						SafeNativeMethods.ExternalDeleteObject(new HandleRef(null, iconinfo.hbmMask));
						iconinfo.hbmMask = IntPtr.Zero;
					}
					if (iconinfo.hbmColor != IntPtr.Zero)
					{
						SafeNativeMethods.ExternalDeleteObject(new HandleRef(null, iconinfo.hbmColor));
						iconinfo.hbmColor = IntPtr.Zero;
					}
					icon.Dispose();
				}
				return point;
			}
		}

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x060026B7 RID: 9911 RVA: 0x0005EFA0 File Offset: 0x0005DFA0
		// (set) Token: 0x060026B8 RID: 9912 RVA: 0x0005EFCB File Offset: 0x0005DFCB
		public static Point Position
		{
			get
			{
				NativeMethods.POINT point = new NativeMethods.POINT();
				UnsafeNativeMethods.GetCursorPos(point);
				return new Point(point.x, point.y);
			}
			set
			{
				IntSecurity.AdjustCursorPosition.Demand();
				UnsafeNativeMethods.SetCursorPos(value.X, value.Y);
			}
		}

		// Token: 0x1700061F RID: 1567
		// (get) Token: 0x060026B9 RID: 9913 RVA: 0x0005EFEB File Offset: 0x0005DFEB
		public Size Size
		{
			get
			{
				if (Cursor.cursorSize.IsEmpty)
				{
					Cursor.cursorSize = new Size(UnsafeNativeMethods.GetSystemMetrics(13), UnsafeNativeMethods.GetSystemMetrics(14));
				}
				return Cursor.cursorSize;
			}
		}

		// Token: 0x17000620 RID: 1568
		// (get) Token: 0x060026BA RID: 9914 RVA: 0x0005F016 File Offset: 0x0005E016
		// (set) Token: 0x060026BB RID: 9915 RVA: 0x0005F01E File Offset: 0x0005E01E
		[SRCategory("CatData")]
		[SRDescription("ControlTagDescr")]
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		[Bindable(true)]
		[Localizable(false)]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x0005F028 File Offset: 0x0005E028
		public IntPtr CopyHandle()
		{
			Size size = this.Size;
			return SafeNativeMethods.CopyImage(new HandleRef(this, this.Handle), 2, size.Width, size.Height, 0);
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x0005F05D File Offset: 0x0005E05D
		private void DestroyHandle()
		{
			if (this.ownHandle)
			{
				UnsafeNativeMethods.DestroyCursor(new HandleRef(this, this.handle));
			}
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x0005F079 File Offset: 0x0005E079
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x0005F088 File Offset: 0x0005E088
		private void Dispose(bool disposing)
		{
			if (this.handle != IntPtr.Zero)
			{
				this.DestroyHandle();
				this.handle = IntPtr.Zero;
			}
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x0005F0B0 File Offset: 0x0005E0B0
		private void DrawImageCore(Graphics graphics, Rectangle imageRect, Rectangle targetRect, bool stretch)
		{
			targetRect.X += (int)graphics.Transform.OffsetX;
			targetRect.Y += (int)graphics.Transform.OffsetY;
			int num = 13369376;
			IntPtr hdc = graphics.GetHdc();
			try
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				Size size = this.Size;
				int num6;
				int num7;
				if (!imageRect.IsEmpty)
				{
					num2 = imageRect.X;
					num3 = imageRect.Y;
					num6 = imageRect.Width;
					num7 = imageRect.Height;
				}
				else
				{
					num6 = size.Width;
					num7 = size.Height;
				}
				int num8;
				int num9;
				if (!targetRect.IsEmpty)
				{
					num4 = targetRect.X;
					num5 = targetRect.Y;
					num8 = targetRect.Width;
					num9 = targetRect.Height;
				}
				else
				{
					num8 = size.Width;
					num9 = size.Height;
				}
				int num10;
				int num11;
				int num12;
				int num13;
				if (stretch)
				{
					if (num8 == num6 && num9 == num7 && num2 == 0 && num3 == 0 && num == 13369376 && num6 == size.Width && num7 == size.Height)
					{
						SafeNativeMethods.DrawIcon(new HandleRef(graphics, hdc), num4, num5, new HandleRef(this, this.handle));
						return;
					}
					num10 = size.Width * num8 / num6;
					num11 = size.Height * num9 / num7;
					num12 = num8;
					num13 = num9;
				}
				else
				{
					if (num2 == 0 && num3 == 0 && num == 13369376 && size.Width <= num8 && size.Height <= num9 && size.Width == num6 && size.Height == num7)
					{
						SafeNativeMethods.DrawIcon(new HandleRef(graphics, hdc), num4, num5, new HandleRef(this, this.handle));
						return;
					}
					num10 = size.Width;
					num11 = size.Height;
					num12 = ((num8 < num6) ? num8 : num6);
					num13 = ((num9 < num7) ? num9 : num7);
				}
				if (num == 13369376)
				{
					SafeNativeMethods.IntersectClipRect(new HandleRef(this, this.Handle), num4, num5, num4 + num12, num5 + num13);
					SafeNativeMethods.DrawIconEx(new HandleRef(graphics, hdc), num4 - num2, num5 - num3, new HandleRef(this, this.handle), num10, num11, 0, NativeMethods.NullHandleRef, 3);
				}
			}
			finally
			{
				graphics.ReleaseHdcInternal(hdc);
			}
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x0005F320 File Offset: 0x0005E320
		public void Draw(Graphics g, Rectangle targetRect)
		{
			this.DrawImageCore(g, Rectangle.Empty, targetRect, false);
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x0005F330 File Offset: 0x0005E330
		public void DrawStretched(Graphics g, Rectangle targetRect)
		{
			this.DrawImageCore(g, Rectangle.Empty, targetRect, true);
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x0005F340 File Offset: 0x0005E340
		~Cursor()
		{
			this.Dispose(false);
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x0005F370 File Offset: 0x0005E370
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			if (this.cursorData != null)
			{
				si.AddValue("CursorData", this.cursorData, typeof(byte[]));
				return;
			}
			if (this.resourceId != 0)
			{
				si.AddValue("CursorResourceId", this.resourceId, typeof(int));
				return;
			}
			throw new SerializationException(SR.GetString("CursorNonSerializableHandle"));
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x0005F3D9 File Offset: 0x0005E3D9
		public static void Hide()
		{
			IntSecurity.AdjustCursorClip.Demand();
			UnsafeNativeMethods.ShowCursor(false);
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x0005F3EC File Offset: 0x0005E3EC
		private void LoadFromResourceId(int nResourceId)
		{
			this.ownHandle = false;
			try
			{
				this.resourceId = nResourceId;
				this.handle = SafeNativeMethods.LoadCursor(NativeMethods.NullHandleRef, nResourceId);
			}
			catch (Exception)
			{
				this.handle = IntPtr.Zero;
			}
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x0005F438 File Offset: 0x0005E438
		private Size GetIconSize(IntPtr iconHandle)
		{
			Size size = this.Size;
			NativeMethods.ICONINFO iconinfo = new NativeMethods.ICONINFO();
			SafeNativeMethods.GetIconInfo(new HandleRef(this, iconHandle), iconinfo);
			NativeMethods.BITMAP bitmap = new NativeMethods.BITMAP();
			if (iconinfo.hbmColor != IntPtr.Zero)
			{
				UnsafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmColor), Marshal.SizeOf(typeof(NativeMethods.BITMAP)), bitmap);
				SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmColor));
				size = new Size(bitmap.bmWidth, bitmap.bmHeight);
			}
			else if (iconinfo.hbmMask != IntPtr.Zero)
			{
				UnsafeNativeMethods.GetObject(new HandleRef(null, iconinfo.hbmMask), Marshal.SizeOf(typeof(NativeMethods.BITMAP)), bitmap);
				size = new Size(bitmap.bmWidth, bitmap.bmHeight / 2);
			}
			if (iconinfo.hbmMask != IntPtr.Zero)
			{
				SafeNativeMethods.IntDeleteObject(new HandleRef(null, iconinfo.hbmMask));
			}
			return size;
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x0005F530 File Offset: 0x0005E530
		private void LoadPicture(UnsafeNativeMethods.IStream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			try
			{
				Guid guid = typeof(UnsafeNativeMethods.IPicture).GUID;
				UnsafeNativeMethods.IPicture picture = null;
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
				try
				{
					picture = UnsafeNativeMethods.OleCreateIPictureIndirect(null, ref guid, true);
					UnsafeNativeMethods.IPersistStream persistStream = (UnsafeNativeMethods.IPersistStream)picture;
					persistStream.Load(stream);
					if (picture == null || picture.GetPictureType() != 3)
					{
						throw new ArgumentException(SR.GetString("InvalidPictureType", new object[] { "picture", "Cursor" }), "picture");
					}
					IntPtr intPtr = picture.GetHandle();
					Size iconSize = this.GetIconSize(intPtr);
					this.handle = SafeNativeMethods.CopyImageAsCursor(new HandleRef(this, intPtr), 2, iconSize.Width, iconSize.Height, 0);
					this.ownHandle = true;
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
					if (picture != null)
					{
						Marshal.ReleaseComObject(picture);
					}
				}
			}
			catch (COMException ex)
			{
				throw new ArgumentException(SR.GetString("InvalidPictureFormat"), "stream", ex);
			}
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x0005F644 File Offset: 0x0005E644
		internal void SavePicture(Stream stream)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (this.resourceId != 0)
			{
				throw new FormatException(SR.GetString("CursorCannotCovertToBytes"));
			}
			try
			{
				stream.Write(this.cursorData, 0, this.cursorData.Length);
			}
			catch (SecurityException)
			{
				throw;
			}
			catch (Exception)
			{
				throw new InvalidOperationException(SR.GetString("InvalidPictureFormat"));
			}
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x0005F6C0 File Offset: 0x0005E6C0
		public static void Show()
		{
			UnsafeNativeMethods.ShowCursor(true);
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x0005F6CC File Offset: 0x0005E6CC
		public override string ToString()
		{
			string text;
			if (!this.ownHandle)
			{
				text = TypeDescriptor.GetConverter(typeof(Cursor)).ConvertToString(this);
			}
			else
			{
				text = base.ToString();
			}
			return "[Cursor: " + text + "]";
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x0005F712 File Offset: 0x0005E712
		public static bool operator ==(Cursor left, Cursor right)
		{
			return object.ReferenceEquals(left, null) == object.ReferenceEquals(right, null) && (object.ReferenceEquals(left, null) || left.handle == right.handle);
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x0005F742 File Offset: 0x0005E742
		public static bool operator !=(Cursor left, Cursor right)
		{
			return !(left == right);
		}

		// Token: 0x060026CE RID: 9934 RVA: 0x0005F74E File Offset: 0x0005E74E
		public override int GetHashCode()
		{
			return (int)this.handle;
		}

		// Token: 0x060026CF RID: 9935 RVA: 0x0005F75B File Offset: 0x0005E75B
		public override bool Equals(object obj)
		{
			return obj is Cursor && this == (Cursor)obj;
		}

		// Token: 0x04001658 RID: 5720
		private static Size cursorSize = Size.Empty;

		// Token: 0x04001659 RID: 5721
		private byte[] cursorData;

		// Token: 0x0400165A RID: 5722
		private IntPtr handle = IntPtr.Zero;

		// Token: 0x0400165B RID: 5723
		private bool ownHandle = true;

		// Token: 0x0400165C RID: 5724
		private int resourceId;

		// Token: 0x0400165D RID: 5725
		private object userData;
	}
}
