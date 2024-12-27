using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Internal;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Windows.Forms
{
	// Token: 0x020003A3 RID: 931
	[ClassInterface(ClassInterfaceType.None)]
	public class DataObject : IDataObject, IDataObject
	{
		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x060038AA RID: 14506 RVA: 0x000CF465 File Offset: 0x000CE465
		// (set) Token: 0x060038AB RID: 14507 RVA: 0x000CF46D File Offset: 0x000CE46D
		internal bool RestrictedFormats
		{
			get
			{
				return this.restrictedFormats;
			}
			set
			{
				this.restrictedFormats = value;
			}
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x000CF476 File Offset: 0x000CE476
		internal DataObject(IDataObject data)
		{
			this.innerData = data;
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x000CF485 File Offset: 0x000CE485
		internal DataObject(IDataObject data)
		{
			if (data is DataObject)
			{
				this.innerData = data as IDataObject;
				return;
			}
			this.innerData = new DataObject.OleConverter(data);
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x000CF4AE File Offset: 0x000CE4AE
		public DataObject()
		{
			this.innerData = new DataObject.DataStore();
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x000CF4C4 File Offset: 0x000CE4C4
		public DataObject(object data)
		{
			if (data is IDataObject && !Marshal.IsComObject(data))
			{
				this.innerData = (IDataObject)data;
				return;
			}
			if (data is IDataObject)
			{
				this.innerData = new DataObject.OleConverter((IDataObject)data);
				return;
			}
			this.innerData = new DataObject.DataStore();
			this.SetData(data);
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x000CF520 File Offset: 0x000CE520
		public DataObject(string format, object data)
			: this()
		{
			this.SetData(format, data);
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x000CF530 File Offset: 0x000CE530
		private IntPtr GetCompatibleBitmap(Bitmap bm)
		{
			IntPtr hbitmap = bm.GetHbitmap();
			IntPtr dc = UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef);
			IntPtr intPtr = UnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, dc));
			IntPtr intPtr2 = SafeNativeMethods.SelectObject(new HandleRef(null, intPtr), new HandleRef(bm, hbitmap));
			IntPtr intPtr3 = UnsafeNativeMethods.CreateCompatibleDC(new HandleRef(null, dc));
			IntPtr intPtr4 = SafeNativeMethods.CreateCompatibleBitmap(new HandleRef(null, dc), bm.Size.Width, bm.Size.Height);
			IntPtr intPtr5 = SafeNativeMethods.SelectObject(new HandleRef(null, intPtr3), new HandleRef(null, intPtr4));
			SafeNativeMethods.BitBlt(new HandleRef(null, intPtr3), 0, 0, bm.Size.Width, bm.Size.Height, new HandleRef(null, intPtr), 0, 0, 13369376);
			SafeNativeMethods.SelectObject(new HandleRef(null, intPtr), new HandleRef(null, intPtr2));
			SafeNativeMethods.SelectObject(new HandleRef(null, intPtr3), new HandleRef(null, intPtr5));
			UnsafeNativeMethods.DeleteCompatibleDC(new HandleRef(null, intPtr));
			UnsafeNativeMethods.DeleteCompatibleDC(new HandleRef(null, intPtr3));
			UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, new HandleRef(null, dc));
			SafeNativeMethods.DeleteObject(new HandleRef(bm, hbitmap));
			return intPtr4;
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x000CF663 File Offset: 0x000CE663
		public virtual object GetData(string format, bool autoConvert)
		{
			return this.innerData.GetData(format, autoConvert);
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x000CF672 File Offset: 0x000CE672
		public virtual object GetData(string format)
		{
			return this.GetData(format, true);
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x000CF67C File Offset: 0x000CE67C
		public virtual object GetData(Type format)
		{
			if (format == null)
			{
				return null;
			}
			return this.GetData(format.FullName);
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x000CF690 File Offset: 0x000CE690
		public virtual bool GetDataPresent(Type format)
		{
			return format != null && this.GetDataPresent(format.FullName);
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x000CF6B0 File Offset: 0x000CE6B0
		public virtual bool GetDataPresent(string format, bool autoConvert)
		{
			return this.innerData.GetDataPresent(format, autoConvert);
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x000CF6CC File Offset: 0x000CE6CC
		public virtual bool GetDataPresent(string format)
		{
			return this.GetDataPresent(format, true);
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x000CF6E3 File Offset: 0x000CE6E3
		public virtual string[] GetFormats(bool autoConvert)
		{
			return this.innerData.GetFormats(autoConvert);
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x000CF6F1 File Offset: 0x000CE6F1
		public virtual string[] GetFormats()
		{
			return this.GetFormats(true);
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x000CF6FA File Offset: 0x000CE6FA
		public virtual bool ContainsAudio()
		{
			return this.GetDataPresent(DataFormats.WaveAudio, false);
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x000CF708 File Offset: 0x000CE708
		public virtual bool ContainsFileDropList()
		{
			return this.GetDataPresent(DataFormats.FileDrop, true);
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x000CF716 File Offset: 0x000CE716
		public virtual bool ContainsImage()
		{
			return this.GetDataPresent(DataFormats.Bitmap, true);
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x000CF724 File Offset: 0x000CE724
		public virtual bool ContainsText()
		{
			return this.ContainsText(TextDataFormat.UnicodeText);
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x000CF72D File Offset: 0x000CE72D
		public virtual bool ContainsText(TextDataFormat format)
		{
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			return this.GetDataPresent(DataObject.ConvertToDataFormats(format), false);
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x000CF762 File Offset: 0x000CE762
		public virtual Stream GetAudioStream()
		{
			return this.GetData(DataFormats.WaveAudio, false) as Stream;
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x000CF778 File Offset: 0x000CE778
		public virtual StringCollection GetFileDropList()
		{
			StringCollection stringCollection = new StringCollection();
			string[] array = this.GetData(DataFormats.FileDrop, true) as string[];
			if (array != null)
			{
				stringCollection.AddRange(array);
			}
			return stringCollection;
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x000CF7A8 File Offset: 0x000CE7A8
		public virtual Image GetImage()
		{
			return this.GetData(DataFormats.Bitmap, true) as Image;
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x000CF7BB File Offset: 0x000CE7BB
		public virtual string GetText()
		{
			return this.GetText(TextDataFormat.UnicodeText);
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x000CF7C4 File Offset: 0x000CE7C4
		public virtual string GetText(TextDataFormat format)
		{
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			string text = this.GetData(DataObject.ConvertToDataFormats(format), false) as string;
			if (text != null)
			{
				return text;
			}
			return string.Empty;
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x000CF814 File Offset: 0x000CE814
		public virtual void SetAudio(byte[] audioBytes)
		{
			if (audioBytes == null)
			{
				throw new ArgumentNullException("audioBytes");
			}
			this.SetAudio(new MemoryStream(audioBytes));
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x000CF830 File Offset: 0x000CE830
		public virtual void SetAudio(Stream audioStream)
		{
			if (audioStream == null)
			{
				throw new ArgumentNullException("audioStream");
			}
			this.SetData(DataFormats.WaveAudio, false, audioStream);
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x000CF850 File Offset: 0x000CE850
		public virtual void SetFileDropList(StringCollection filePaths)
		{
			if (filePaths == null)
			{
				throw new ArgumentNullException("filePaths");
			}
			string[] array = new string[filePaths.Count];
			filePaths.CopyTo(array, 0);
			this.SetData(DataFormats.FileDrop, true, array);
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x000CF88C File Offset: 0x000CE88C
		public virtual void SetImage(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			this.SetData(DataFormats.Bitmap, true, image);
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x000CF8A9 File Offset: 0x000CE8A9
		public virtual void SetText(string textData)
		{
			this.SetText(textData, TextDataFormat.UnicodeText);
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x000CF8B4 File Offset: 0x000CE8B4
		public virtual void SetText(string textData, TextDataFormat format)
		{
			if (string.IsNullOrEmpty(textData))
			{
				throw new ArgumentNullException("textData");
			}
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			this.SetData(DataObject.ConvertToDataFormats(format), false, textData);
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x000CF908 File Offset: 0x000CE908
		private static string ConvertToDataFormats(TextDataFormat format)
		{
			switch (format)
			{
			case TextDataFormat.UnicodeText:
				return DataFormats.UnicodeText;
			case TextDataFormat.Rtf:
				return DataFormats.Rtf;
			case TextDataFormat.Html:
				return DataFormats.Html;
			case TextDataFormat.CommaSeparatedValue:
				return DataFormats.CommaSeparatedValue;
			default:
				return DataFormats.UnicodeText;
			}
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x000CF950 File Offset: 0x000CE950
		private static string[] GetDistinctStrings(string[] formats)
		{
			ArrayList arrayList = new ArrayList();
			foreach (string text in formats)
			{
				if (!arrayList.Contains(text))
				{
					arrayList.Add(text);
				}
			}
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x000CF99C File Offset: 0x000CE99C
		private static string[] GetMappedFormats(string format)
		{
			if (format == null)
			{
				return null;
			}
			if (format.Equals(DataFormats.Text) || format.Equals(DataFormats.UnicodeText) || format.Equals(DataFormats.StringFormat))
			{
				return new string[]
				{
					DataFormats.StringFormat,
					DataFormats.UnicodeText,
					DataFormats.Text
				};
			}
			if (format.Equals(DataFormats.FileDrop) || format.Equals(DataObject.CF_DEPRECATED_FILENAME) || format.Equals(DataObject.CF_DEPRECATED_FILENAMEW))
			{
				return new string[]
				{
					DataFormats.FileDrop,
					DataObject.CF_DEPRECATED_FILENAMEW,
					DataObject.CF_DEPRECATED_FILENAME
				};
			}
			if (format.Equals(DataFormats.Bitmap) || format.Equals(typeof(Bitmap).FullName))
			{
				return new string[]
				{
					typeof(Bitmap).FullName,
					DataFormats.Bitmap
				};
			}
			return new string[] { format };
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x000CFA94 File Offset: 0x000CEA94
		private bool GetTymedUseable(TYMED tymed)
		{
			for (int i = 0; i < DataObject.ALLOWED_TYMEDS.Length; i++)
			{
				if ((tymed & DataObject.ALLOWED_TYMEDS[i]) != TYMED.TYMED_NULL)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x000CFAC4 File Offset: 0x000CEAC4
		private void GetDataIntoOleStructs(ref FORMATETC formatetc, ref STGMEDIUM medium)
		{
			if (this.GetTymedUseable(formatetc.tymed) && this.GetTymedUseable(medium.tymed))
			{
				string name = DataFormats.GetFormat((int)formatetc.cfFormat).Name;
				if (!this.GetDataPresent(name))
				{
					Marshal.ThrowExceptionForHR(-2147221404);
					return;
				}
				object data = this.GetData(name);
				if ((formatetc.tymed & TYMED.TYMED_HGLOBAL) != TYMED.TYMED_NULL)
				{
					int num = this.SaveDataToHandle(data, name, ref medium);
					if (NativeMethods.Failed(num))
					{
						Marshal.ThrowExceptionForHR(num);
						return;
					}
				}
				else
				{
					if ((formatetc.tymed & TYMED.TYMED_GDI) == TYMED.TYMED_NULL)
					{
						Marshal.ThrowExceptionForHR(-2147221399);
						return;
					}
					if (name.Equals(DataFormats.Bitmap) && data is Bitmap)
					{
						Bitmap bitmap = (Bitmap)data;
						if (bitmap != null)
						{
							medium.unionmember = this.GetCompatibleBitmap(bitmap);
							return;
						}
					}
				}
			}
			else
			{
				Marshal.ThrowExceptionForHR(-2147221399);
			}
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x000CFB90 File Offset: 0x000CEB90
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int IDataObject.DAdvise(ref FORMATETC pFormatetc, ADVF advf, IAdviseSink pAdvSink, out int pdwConnection)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.DAdvise(ref pFormatetc, advf, pAdvSink, out pdwConnection);
			}
			pdwConnection = 0;
			return -2147467263;
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x000CFBC3 File Offset: 0x000CEBC3
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void IDataObject.DUnadvise(int dwConnection)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				((DataObject.OleConverter)this.innerData).OleDataObject.DUnadvise(dwConnection);
				return;
			}
			Marshal.ThrowExceptionForHR(-2147467263);
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x000CFBF3 File Offset: 0x000CEBF3
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int IDataObject.EnumDAdvise(out IEnumSTATDATA enumAdvise)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.EnumDAdvise(out enumAdvise);
			}
			enumAdvise = null;
			return -2147221501;
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x000CFC24 File Offset: 0x000CEC24
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		IEnumFORMATETC IDataObject.EnumFormatEtc(DATADIR dwDirection)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.EnumFormatEtc(dwDirection);
			}
			if (dwDirection == DATADIR.DATADIR_GET)
			{
				return new DataObject.FormatEnumerator(this);
			}
			throw new ExternalException(SR.GetString("ExternalException"), -2147467263);
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x000CFC74 File Offset: 0x000CEC74
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int IDataObject.GetCanonicalFormatEtc(ref FORMATETC pformatetcIn, out FORMATETC pformatetcOut)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.GetCanonicalFormatEtc(ref pformatetcIn, out pformatetcOut);
			}
			pformatetcOut = default(FORMATETC);
			return 262448;
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x000CFCA8 File Offset: 0x000CECA8
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void IDataObject.GetData(ref FORMATETC formatetc, out STGMEDIUM medium)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				((DataObject.OleConverter)this.innerData).OleDataObject.GetData(ref formatetc, out medium);
				return;
			}
			medium = default(STGMEDIUM);
			if (this.GetTymedUseable(formatetc.tymed))
			{
				if ((formatetc.tymed & TYMED.TYMED_HGLOBAL) != TYMED.TYMED_NULL)
				{
					medium.tymed = TYMED.TYMED_HGLOBAL;
					medium.unionmember = UnsafeNativeMethods.GlobalAlloc(8258, 1);
					if (medium.unionmember == IntPtr.Zero)
					{
						throw new OutOfMemoryException();
					}
					try
					{
						((IDataObject)this).GetDataHere(ref formatetc, ref medium);
						return;
					}
					catch
					{
						UnsafeNativeMethods.GlobalFree(new HandleRef(medium, medium.unionmember));
						medium.unionmember = IntPtr.Zero;
						throw;
					}
				}
				medium.tymed = formatetc.tymed;
				((IDataObject)this).GetDataHere(ref formatetc, ref medium);
				return;
			}
			Marshal.ThrowExceptionForHR(-2147221399);
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x000CFD90 File Offset: 0x000CED90
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void IDataObject.GetDataHere(ref FORMATETC formatetc, ref STGMEDIUM medium)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				((DataObject.OleConverter)this.innerData).OleDataObject.GetDataHere(ref formatetc, ref medium);
				return;
			}
			this.GetDataIntoOleStructs(ref formatetc, ref medium);
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x000CFDC0 File Offset: 0x000CEDC0
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		int IDataObject.QueryGetData(ref FORMATETC formatetc)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				return ((DataObject.OleConverter)this.innerData).OleDataObject.QueryGetData(ref formatetc);
			}
			if (formatetc.dwAspect != DVASPECT.DVASPECT_CONTENT)
			{
				return -2147221397;
			}
			if (!this.GetTymedUseable(formatetc.tymed))
			{
				return -2147221399;
			}
			if (formatetc.cfFormat == 0)
			{
				return 1;
			}
			if (!this.GetDataPresent(DataFormats.GetFormat((int)formatetc.cfFormat).Name))
			{
				return -2147221404;
			}
			return 0;
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x000CFE3D File Offset: 0x000CEE3D
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		void IDataObject.SetData(ref FORMATETC pFormatetcIn, ref STGMEDIUM pmedium, bool fRelease)
		{
			if (this.innerData is DataObject.OleConverter)
			{
				((DataObject.OleConverter)this.innerData).OleDataObject.SetData(ref pFormatetcIn, ref pmedium, fRelease);
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x000CFE6C File Offset: 0x000CEE6C
		private int SaveDataToHandle(object data, string format, ref STGMEDIUM medium)
		{
			int num = -2147467259;
			if (data is Stream)
			{
				num = this.SaveStreamToHandle(ref medium.unionmember, (Stream)data);
			}
			else if (format.Equals(DataFormats.Text) || format.Equals(DataFormats.Rtf) || format.Equals(DataFormats.Html) || format.Equals(DataFormats.OemText))
			{
				num = this.SaveStringToHandle(medium.unionmember, data.ToString(), false);
			}
			else if (format.Equals(DataFormats.UnicodeText))
			{
				num = this.SaveStringToHandle(medium.unionmember, data.ToString(), true);
			}
			else if (format.Equals(DataFormats.FileDrop))
			{
				num = this.SaveFileListToHandle(medium.unionmember, (string[])data);
			}
			else if (format.Equals(DataObject.CF_DEPRECATED_FILENAME))
			{
				string[] array = (string[])data;
				num = this.SaveStringToHandle(medium.unionmember, array[0], false);
			}
			else if (format.Equals(DataObject.CF_DEPRECATED_FILENAMEW))
			{
				string[] array2 = (string[])data;
				num = this.SaveStringToHandle(medium.unionmember, array2[0], true);
			}
			else if (format.Equals(DataFormats.Dib) && data is Image)
			{
				num = -2147221399;
			}
			else if (format.Equals(DataFormats.Serializable) || data is ISerializable || (data != null && data.GetType().IsSerializable))
			{
				num = this.SaveObjectToHandle(ref medium.unionmember, data);
			}
			return num;
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x000CFFD8 File Offset: 0x000CEFD8
		private int SaveObjectToHandle(ref IntPtr handle, object data)
		{
			Stream stream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.Write(DataObject.serializedObjectID);
			DataObject.SaveObjectToHandleSerializer(stream, data);
			return this.SaveStreamToHandle(ref handle, stream);
		}

		// Token: 0x060038DA RID: 14554 RVA: 0x000D000C File Offset: 0x000CF00C
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private static void SaveObjectToHandleSerializer(Stream stream, object data)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(stream, data);
		}

		// Token: 0x060038DB RID: 14555 RVA: 0x000D0028 File Offset: 0x000CF028
		private int SaveStreamToHandle(ref IntPtr handle, Stream stream)
		{
			int num = (int)stream.Length;
			handle = UnsafeNativeMethods.GlobalAlloc(8194, num);
			if (handle == IntPtr.Zero)
			{
				return -2147024882;
			}
			IntPtr intPtr = UnsafeNativeMethods.GlobalLock(new HandleRef(null, handle));
			if (intPtr == IntPtr.Zero)
			{
				return -2147024882;
			}
			try
			{
				byte[] array = new byte[num];
				stream.Position = 0L;
				stream.Read(array, 0, num);
				Marshal.Copy(array, 0, intPtr, num);
			}
			finally
			{
				UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, handle));
			}
			return 0;
		}

		// Token: 0x060038DC RID: 14556 RVA: 0x000D00D4 File Offset: 0x000CF0D4
		private int SaveFileListToHandle(IntPtr handle, string[] files)
		{
			if (files == null)
			{
				return 0;
			}
			if (files.Length < 1)
			{
				return 0;
			}
			if (handle == IntPtr.Zero)
			{
				return -2147024809;
			}
			bool flag = Marshal.SystemDefaultCharSize != 1;
			IntPtr intPtr = IntPtr.Zero;
			int num = 20;
			int num2 = num;
			if (flag)
			{
				for (int i = 0; i < files.Length; i++)
				{
					num2 += (files[i].Length + 1) * 2;
				}
				num2 += 2;
			}
			else
			{
				for (int j = 0; j < files.Length; j++)
				{
					num2 += NativeMethods.Util.GetPInvokeStringLength(files[j]) + 1;
				}
				num2++;
			}
			IntPtr intPtr2 = UnsafeNativeMethods.GlobalReAlloc(new HandleRef(null, handle), num2, 8194);
			if (intPtr2 == IntPtr.Zero)
			{
				return -2147024882;
			}
			IntPtr intPtr3 = UnsafeNativeMethods.GlobalLock(new HandleRef(null, intPtr2));
			if (intPtr3 == IntPtr.Zero)
			{
				return -2147024882;
			}
			intPtr = intPtr3;
			int[] array = new int[5];
			array[0] = num;
			int[] array2 = array;
			if (flag)
			{
				array2[4] = -1;
			}
			Marshal.Copy(array2, 0, intPtr, array2.Length);
			intPtr = (IntPtr)((long)intPtr + (long)num);
			for (int k = 0; k < files.Length; k++)
			{
				if (flag)
				{
					UnsafeNativeMethods.CopyMemoryW(intPtr, files[k], files[k].Length * 2);
					intPtr = (IntPtr)((long)intPtr + (long)(files[k].Length * 2));
					byte[] array3 = new byte[2];
					Marshal.Copy(array3, 0, intPtr, 2);
					intPtr = (IntPtr)((long)intPtr + 2L);
				}
				else
				{
					int pinvokeStringLength = NativeMethods.Util.GetPInvokeStringLength(files[k]);
					UnsafeNativeMethods.CopyMemoryA(intPtr, files[k], pinvokeStringLength);
					intPtr = (IntPtr)((long)intPtr + (long)pinvokeStringLength);
					byte[] array4 = new byte[1];
					Marshal.Copy(array4, 0, intPtr, 1);
					intPtr = (IntPtr)((long)intPtr + 1L);
				}
			}
			if (flag)
			{
				char[] array5 = new char[1];
				Marshal.Copy(array5, 0, intPtr, 1);
				intPtr = (IntPtr)((long)intPtr + 2L);
			}
			else
			{
				byte[] array6 = new byte[1];
				Marshal.Copy(array6, 0, intPtr, 1);
				intPtr = (IntPtr)((long)intPtr + 1L);
			}
			UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr2));
			return 0;
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x000D02F4 File Offset: 0x000CF2F4
		private int SaveStringToHandle(IntPtr handle, string str, bool unicode)
		{
			if (handle == IntPtr.Zero)
			{
				return -2147024809;
			}
			IntPtr intPtr = IntPtr.Zero;
			if (unicode)
			{
				int num = str.Length * 2 + 2;
				intPtr = UnsafeNativeMethods.GlobalReAlloc(new HandleRef(null, handle), num, 8258);
				if (intPtr == IntPtr.Zero)
				{
					return -2147024882;
				}
				IntPtr intPtr2 = UnsafeNativeMethods.GlobalLock(new HandleRef(null, intPtr));
				if (intPtr2 == IntPtr.Zero)
				{
					return -2147024882;
				}
				char[] array = str.ToCharArray(0, str.Length);
				UnsafeNativeMethods.CopyMemoryW(intPtr2, array, array.Length * 2);
			}
			else
			{
				int num2 = UnsafeNativeMethods.WideCharToMultiByte(0, 0, str, str.Length, null, 0, IntPtr.Zero, IntPtr.Zero);
				byte[] array2 = new byte[num2];
				UnsafeNativeMethods.WideCharToMultiByte(0, 0, str, str.Length, array2, array2.Length, IntPtr.Zero, IntPtr.Zero);
				intPtr = UnsafeNativeMethods.GlobalReAlloc(new HandleRef(null, handle), num2 + 1, 8258);
				if (intPtr == IntPtr.Zero)
				{
					return -2147024882;
				}
				IntPtr intPtr3 = UnsafeNativeMethods.GlobalLock(new HandleRef(null, intPtr));
				if (intPtr3 == IntPtr.Zero)
				{
					return -2147024882;
				}
				UnsafeNativeMethods.CopyMemory(intPtr3, array2, num2);
				byte[] array3 = new byte[1];
				Marshal.Copy(array3, 0, (IntPtr)((long)intPtr3 + (long)num2), 1);
			}
			if (intPtr != IntPtr.Zero)
			{
				UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, intPtr));
			}
			return 0;
		}

		// Token: 0x060038DE RID: 14558 RVA: 0x000D0463 File Offset: 0x000CF463
		public virtual void SetData(string format, bool autoConvert, object data)
		{
			this.innerData.SetData(format, autoConvert, data);
		}

		// Token: 0x060038DF RID: 14559 RVA: 0x000D0473 File Offset: 0x000CF473
		public virtual void SetData(string format, object data)
		{
			this.innerData.SetData(format, data);
		}

		// Token: 0x060038E0 RID: 14560 RVA: 0x000D0482 File Offset: 0x000CF482
		public virtual void SetData(Type format, object data)
		{
			this.innerData.SetData(format, data);
		}

		// Token: 0x060038E1 RID: 14561 RVA: 0x000D0491 File Offset: 0x000CF491
		public virtual void SetData(object data)
		{
			this.innerData.SetData(data);
		}

		// Token: 0x04001C8C RID: 7308
		private const int DV_E_FORMATETC = -2147221404;

		// Token: 0x04001C8D RID: 7309
		private const int DV_E_LINDEX = -2147221400;

		// Token: 0x04001C8E RID: 7310
		private const int DV_E_TYMED = -2147221399;

		// Token: 0x04001C8F RID: 7311
		private const int DV_E_DVASPECT = -2147221397;

		// Token: 0x04001C90 RID: 7312
		private const int OLE_E_NOTRUNNING = -2147221499;

		// Token: 0x04001C91 RID: 7313
		private const int OLE_E_ADVISENOTSUPPORTED = -2147221501;

		// Token: 0x04001C92 RID: 7314
		private const int DATA_S_SAMEFORMATETC = 262448;

		// Token: 0x04001C93 RID: 7315
		private static readonly string CF_DEPRECATED_FILENAME = "FileName";

		// Token: 0x04001C94 RID: 7316
		private static readonly string CF_DEPRECATED_FILENAMEW = "FileNameW";

		// Token: 0x04001C95 RID: 7317
		private static readonly TYMED[] ALLOWED_TYMEDS = new TYMED[]
		{
			TYMED.TYMED_HGLOBAL,
			TYMED.TYMED_ISTREAM,
			TYMED.TYMED_ENHMF,
			TYMED.TYMED_MFPICT,
			TYMED.TYMED_GDI
		};

		// Token: 0x04001C96 RID: 7318
		private IDataObject innerData;

		// Token: 0x04001C97 RID: 7319
		private bool restrictedFormats;

		// Token: 0x04001C98 RID: 7320
		private static readonly byte[] serializedObjectID = new Guid("FD9EA796-3B13-4370-A679-56106BB288FB").ToByteArray();

		// Token: 0x020003A4 RID: 932
		private class FormatEnumerator : IEnumFORMATETC
		{
			// Token: 0x060038E3 RID: 14563 RVA: 0x000D04FC File Offset: 0x000CF4FC
			public FormatEnumerator(IDataObject parent)
				: this(parent, parent.GetFormats())
			{
			}

			// Token: 0x060038E4 RID: 14564 RVA: 0x000D050C File Offset: 0x000CF50C
			public FormatEnumerator(IDataObject parent, FORMATETC[] formats)
			{
				this.formats = new ArrayList();
				base..ctor();
				this.formats.Clear();
				this.parent = parent;
				this.current = 0;
				if (formats != null)
				{
					DataObject dataObject = parent as DataObject;
					if (dataObject != null && dataObject.RestrictedFormats && !Clipboard.IsFormatValid(formats))
					{
						throw new SecurityException(SR.GetString("ClipboardSecurityException"));
					}
					foreach (FORMATETC formatetc in formats)
					{
						FORMATETC formatetc2 = default(FORMATETC);
						formatetc2.cfFormat = formatetc.cfFormat;
						formatetc2.dwAspect = formatetc.dwAspect;
						formatetc2.ptd = formatetc.ptd;
						formatetc2.lindex = formatetc.lindex;
						formatetc2.tymed = formatetc.tymed;
						this.formats.Add(formatetc2);
					}
				}
			}

			// Token: 0x060038E5 RID: 14565 RVA: 0x000D05F0 File Offset: 0x000CF5F0
			public FormatEnumerator(IDataObject parent, string[] formats)
			{
				this.formats = new ArrayList();
				base..ctor();
				this.parent = parent;
				this.formats.Clear();
				string bitmap = DataFormats.Bitmap;
				string enhancedMetafile = DataFormats.EnhancedMetafile;
				string text = DataFormats.Text;
				string unicodeText = DataFormats.UnicodeText;
				string stringFormat = DataFormats.StringFormat;
				string stringFormat2 = DataFormats.StringFormat;
				if (formats != null)
				{
					DataObject dataObject = parent as DataObject;
					if (dataObject != null && dataObject.RestrictedFormats && !Clipboard.IsFormatValid(formats))
					{
						throw new SecurityException(SR.GetString("ClipboardSecurityException"));
					}
					foreach (string text2 in formats)
					{
						FORMATETC formatetc = default(FORMATETC);
						formatetc.cfFormat = (short)DataFormats.GetFormat(text2).Id;
						formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
						formatetc.ptd = IntPtr.Zero;
						formatetc.lindex = -1;
						if (text2.Equals(bitmap))
						{
							formatetc.tymed = TYMED.TYMED_GDI;
						}
						else if (text2.Equals(enhancedMetafile))
						{
							formatetc.tymed = TYMED.TYMED_ENHMF;
						}
						else if (text2.Equals(text) || text2.Equals(unicodeText) || text2.Equals(stringFormat) || text2.Equals(stringFormat2) || text2.Equals(DataFormats.Rtf) || text2.Equals(DataFormats.CommaSeparatedValue) || text2.Equals(DataFormats.FileDrop) || text2.Equals(DataObject.CF_DEPRECATED_FILENAME) || text2.Equals(DataObject.CF_DEPRECATED_FILENAMEW))
						{
							formatetc.tymed = TYMED.TYMED_HGLOBAL;
						}
						else
						{
							formatetc.tymed = TYMED.TYMED_HGLOBAL;
						}
						if (formatetc.tymed != TYMED.TYMED_NULL)
						{
							this.formats.Add(formatetc);
						}
					}
				}
			}

			// Token: 0x060038E6 RID: 14566 RVA: 0x000D07A0 File Offset: 0x000CF7A0
			public int Next(int celt, FORMATETC[] rgelt, int[] pceltFetched)
			{
				if (this.current < this.formats.Count && celt > 0)
				{
					FORMATETC formatetc = (FORMATETC)this.formats[this.current];
					rgelt[0].cfFormat = formatetc.cfFormat;
					rgelt[0].tymed = formatetc.tymed;
					rgelt[0].dwAspect = DVASPECT.DVASPECT_CONTENT;
					rgelt[0].ptd = IntPtr.Zero;
					rgelt[0].lindex = -1;
					if (pceltFetched != null)
					{
						pceltFetched[0] = 1;
					}
					this.current++;
					return 0;
				}
				if (pceltFetched != null)
				{
					pceltFetched[0] = 0;
				}
				return 1;
			}

			// Token: 0x060038E7 RID: 14567 RVA: 0x000D0850 File Offset: 0x000CF850
			public int Skip(int celt)
			{
				if (this.current + celt >= this.formats.Count)
				{
					return 1;
				}
				this.current += celt;
				return 0;
			}

			// Token: 0x060038E8 RID: 14568 RVA: 0x000D0878 File Offset: 0x000CF878
			public int Reset()
			{
				this.current = 0;
				return 0;
			}

			// Token: 0x060038E9 RID: 14569 RVA: 0x000D0884 File Offset: 0x000CF884
			public void Clone(out IEnumFORMATETC ppenum)
			{
				FORMATETC[] array = new FORMATETC[this.formats.Count];
				this.formats.CopyTo(array, 0);
				ppenum = new DataObject.FormatEnumerator(this.parent, array);
			}

			// Token: 0x04001C99 RID: 7321
			internal IDataObject parent;

			// Token: 0x04001C9A RID: 7322
			internal ArrayList formats;

			// Token: 0x04001C9B RID: 7323
			internal int current;
		}

		// Token: 0x020003A5 RID: 933
		private class OleConverter : IDataObject
		{
			// Token: 0x060038EA RID: 14570 RVA: 0x000D08BD File Offset: 0x000CF8BD
			public OleConverter(IDataObject data)
			{
				this.innerData = data;
			}

			// Token: 0x17000AA3 RID: 2723
			// (get) Token: 0x060038EB RID: 14571 RVA: 0x000D08CC File Offset: 0x000CF8CC
			public IDataObject OleDataObject
			{
				get
				{
					return this.innerData;
				}
			}

			// Token: 0x060038EC RID: 14572 RVA: 0x000D08D4 File Offset: 0x000CF8D4
			private object GetDataFromOleIStream(string format)
			{
				FORMATETC formatetc = default(FORMATETC);
				STGMEDIUM stgmedium = default(STGMEDIUM);
				formatetc.cfFormat = (short)DataFormats.GetFormat(format).Id;
				formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
				formatetc.lindex = -1;
				formatetc.tymed = TYMED.TYMED_ISTREAM;
				stgmedium.tymed = TYMED.TYMED_ISTREAM;
				if (this.QueryGetData(ref formatetc) != 0)
				{
					return null;
				}
				try
				{
					IntSecurity.UnmanagedCode.Assert();
					try
					{
						this.innerData.GetData(ref formatetc, out stgmedium);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				catch
				{
					return null;
				}
				if (stgmedium.unionmember != IntPtr.Zero)
				{
					UnsafeNativeMethods.IStream stream = (UnsafeNativeMethods.IStream)Marshal.GetObjectForIUnknown(stgmedium.unionmember);
					Marshal.Release(stgmedium.unionmember);
					NativeMethods.STATSTG statstg = new NativeMethods.STATSTG();
					stream.Stat(statstg, 0);
					int num = (int)statstg.cbSize;
					IntPtr intPtr = UnsafeNativeMethods.GlobalAlloc(8258, num);
					IntPtr intPtr2 = UnsafeNativeMethods.GlobalLock(new HandleRef(this.innerData, intPtr));
					stream.Read(intPtr2, num);
					UnsafeNativeMethods.GlobalUnlock(new HandleRef(this.innerData, intPtr));
					return this.GetDataFromHGLOBLAL(format, intPtr);
				}
				return null;
			}

			// Token: 0x060038ED RID: 14573 RVA: 0x000D0A14 File Offset: 0x000CFA14
			private object GetDataFromHGLOBLAL(string format, IntPtr hglobal)
			{
				object obj = null;
				if (hglobal != IntPtr.Zero)
				{
					if (format.Equals(DataFormats.Text) || format.Equals(DataFormats.Rtf) || format.Equals(DataFormats.Html) || format.Equals(DataFormats.OemText))
					{
						obj = this.ReadStringFromHandle(hglobal, false);
					}
					else if (format.Equals(DataFormats.UnicodeText))
					{
						obj = this.ReadStringFromHandle(hglobal, true);
					}
					else if (format.Equals(DataFormats.FileDrop))
					{
						obj = this.ReadFileListFromHandle(hglobal);
					}
					else if (format.Equals(DataObject.CF_DEPRECATED_FILENAME))
					{
						obj = new string[] { this.ReadStringFromHandle(hglobal, false) };
					}
					else if (format.Equals(DataObject.CF_DEPRECATED_FILENAMEW))
					{
						obj = new string[] { this.ReadStringFromHandle(hglobal, true) };
					}
					else
					{
						obj = this.ReadObjectFromHandle(hglobal);
					}
					UnsafeNativeMethods.GlobalFree(new HandleRef(null, hglobal));
				}
				return obj;
			}

			// Token: 0x060038EE RID: 14574 RVA: 0x000D0B00 File Offset: 0x000CFB00
			private object GetDataFromOleHGLOBAL(string format)
			{
				FORMATETC formatetc = default(FORMATETC);
				STGMEDIUM stgmedium = default(STGMEDIUM);
				formatetc.cfFormat = (short)DataFormats.GetFormat(format).Id;
				formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
				formatetc.lindex = -1;
				formatetc.tymed = TYMED.TYMED_HGLOBAL;
				stgmedium.tymed = TYMED.TYMED_HGLOBAL;
				object obj = null;
				if (this.QueryGetData(ref formatetc) == 0)
				{
					try
					{
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							this.innerData.GetData(ref formatetc, out stgmedium);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
						if (stgmedium.unionmember != IntPtr.Zero)
						{
							obj = this.GetDataFromHGLOBLAL(format, stgmedium.unionmember);
						}
					}
					catch
					{
					}
				}
				return obj;
			}

			// Token: 0x060038EF RID: 14575 RVA: 0x000D0BC4 File Offset: 0x000CFBC4
			private object GetDataFromOleOther(string format)
			{
				FORMATETC formatetc = default(FORMATETC);
				STGMEDIUM stgmedium = default(STGMEDIUM);
				TYMED tymed = TYMED.TYMED_NULL;
				if (format.Equals(DataFormats.Bitmap))
				{
					tymed = TYMED.TYMED_GDI;
				}
				else if (format.Equals(DataFormats.EnhancedMetafile))
				{
					tymed = TYMED.TYMED_ENHMF;
				}
				if (tymed == TYMED.TYMED_NULL)
				{
					return null;
				}
				formatetc.cfFormat = (short)DataFormats.GetFormat(format).Id;
				formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
				formatetc.lindex = -1;
				formatetc.tymed = tymed;
				stgmedium.tymed = tymed;
				object obj = null;
				if (this.QueryGetData(ref formatetc) == 0)
				{
					try
					{
						IntSecurity.UnmanagedCode.Assert();
						try
						{
							this.innerData.GetData(ref formatetc, out stgmedium);
						}
						finally
						{
							CodeAccessPermission.RevertAssert();
						}
					}
					catch
					{
					}
				}
				if (stgmedium.unionmember != IntPtr.Zero && format.Equals(DataFormats.Bitmap))
				{
					global::System.Internal.HandleCollector.Add(stgmedium.unionmember, NativeMethods.CommonHandles.GDI);
					Image image = null;
					IntSecurity.ObjectFromWin32Handle.Assert();
					try
					{
						image = Image.FromHbitmap(stgmedium.unionmember);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					if (image != null)
					{
						Image image2 = image;
						image = (Image)image.Clone();
						SafeNativeMethods.DeleteObject(new HandleRef(null, stgmedium.unionmember));
						image2.Dispose();
					}
					obj = image;
				}
				return obj;
			}

			// Token: 0x060038F0 RID: 14576 RVA: 0x000D0D20 File Offset: 0x000CFD20
			private object GetDataFromBoundOleDataObject(string format)
			{
				object obj = null;
				try
				{
					obj = this.GetDataFromOleOther(format);
					if (obj == null)
					{
						obj = this.GetDataFromOleHGLOBAL(format);
					}
					if (obj == null)
					{
						obj = this.GetDataFromOleIStream(format);
					}
				}
				catch (Exception)
				{
				}
				return obj;
			}

			// Token: 0x060038F1 RID: 14577 RVA: 0x000D0D64 File Offset: 0x000CFD64
			private Stream ReadByteStreamFromHandle(IntPtr handle, out bool isSerializedObject)
			{
				IntPtr intPtr = UnsafeNativeMethods.GlobalLock(new HandleRef(null, handle));
				if (intPtr == IntPtr.Zero)
				{
					throw new ExternalException(SR.GetString("ExternalException"), -2147024882);
				}
				Stream stream;
				try
				{
					int num = UnsafeNativeMethods.GlobalSize(new HandleRef(null, handle));
					byte[] array = new byte[num];
					Marshal.Copy(intPtr, array, 0, num);
					int num2 = 0;
					if (num > DataObject.serializedObjectID.Length)
					{
						isSerializedObject = true;
						for (int i = 0; i < DataObject.serializedObjectID.Length; i++)
						{
							if (DataObject.serializedObjectID[i] != array[i])
							{
								isSerializedObject = false;
								break;
							}
						}
						if (isSerializedObject)
						{
							num2 = DataObject.serializedObjectID.Length;
						}
					}
					else
					{
						isSerializedObject = false;
					}
					stream = new MemoryStream(array, num2, array.Length - num2);
				}
				finally
				{
					UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, handle));
				}
				return stream;
			}

			// Token: 0x060038F2 RID: 14578 RVA: 0x000D0E38 File Offset: 0x000CFE38
			private object ReadObjectFromHandle(IntPtr handle)
			{
				bool flag;
				Stream stream = this.ReadByteStreamFromHandle(handle, out flag);
				object obj;
				if (flag)
				{
					obj = DataObject.OleConverter.ReadObjectFromHandleDeserializer(stream);
				}
				else
				{
					obj = stream;
				}
				return obj;
			}

			// Token: 0x060038F3 RID: 14579 RVA: 0x000D0E60 File Offset: 0x000CFE60
			private static object ReadObjectFromHandleDeserializer(Stream stream)
			{
				return new BinaryFormatter
				{
					AssemblyFormat = FormatterAssemblyStyle.Simple
				}.Deserialize(stream);
			}

			// Token: 0x060038F4 RID: 14580 RVA: 0x000D0E84 File Offset: 0x000CFE84
			private string[] ReadFileListFromHandle(IntPtr hdrop)
			{
				string[] array = null;
				StringBuilder stringBuilder = new StringBuilder(260);
				int num = UnsafeNativeMethods.DragQueryFile(new HandleRef(null, hdrop), -1, null, 0);
				if (num > 0)
				{
					array = new string[num];
					for (int i = 0; i < num; i++)
					{
						int num2 = UnsafeNativeMethods.DragQueryFile(new HandleRef(null, hdrop), i, stringBuilder, stringBuilder.Capacity);
						string text = stringBuilder.ToString();
						if (text.Length > num2)
						{
							text = text.Substring(0, num2);
						}
						string fullPath = Path.GetFullPath(text);
						new FileIOPermission(FileIOPermissionAccess.PathDiscovery, fullPath).Demand();
						array[i] = text;
					}
				}
				return array;
			}

			// Token: 0x060038F5 RID: 14581 RVA: 0x000D0F18 File Offset: 0x000CFF18
			private unsafe string ReadStringFromHandle(IntPtr handle, bool unicode)
			{
				string text = null;
				IntPtr intPtr = UnsafeNativeMethods.GlobalLock(new HandleRef(null, handle));
				try
				{
					if (unicode)
					{
						text = new string((char*)(void*)intPtr);
					}
					else
					{
						text = new string((sbyte*)(void*)intPtr);
					}
				}
				finally
				{
					UnsafeNativeMethods.GlobalUnlock(new HandleRef(null, handle));
				}
				return text;
			}

			// Token: 0x060038F6 RID: 14582 RVA: 0x000D0F74 File Offset: 0x000CFF74
			public virtual object GetData(string format, bool autoConvert)
			{
				object obj = this.GetDataFromBoundOleDataObject(format);
				object obj2 = obj;
				if (autoConvert && (obj == null || obj is MemoryStream))
				{
					string[] mappedFormats = DataObject.GetMappedFormats(format);
					if (mappedFormats != null)
					{
						for (int i = 0; i < mappedFormats.Length; i++)
						{
							if (!format.Equals(mappedFormats[i]))
							{
								obj = this.GetDataFromBoundOleDataObject(mappedFormats[i]);
								if (obj != null && !(obj is MemoryStream))
								{
									obj2 = null;
									break;
								}
							}
						}
					}
				}
				if (obj2 != null)
				{
					return obj2;
				}
				return obj;
			}

			// Token: 0x060038F7 RID: 14583 RVA: 0x000D0FDB File Offset: 0x000CFFDB
			public virtual object GetData(string format)
			{
				return this.GetData(format, true);
			}

			// Token: 0x060038F8 RID: 14584 RVA: 0x000D0FE5 File Offset: 0x000CFFE5
			public virtual object GetData(Type format)
			{
				return this.GetData(format.FullName);
			}

			// Token: 0x060038F9 RID: 14585 RVA: 0x000D0FF3 File Offset: 0x000CFFF3
			public virtual void SetData(string format, bool autoConvert, object data)
			{
			}

			// Token: 0x060038FA RID: 14586 RVA: 0x000D0FF5 File Offset: 0x000CFFF5
			public virtual void SetData(string format, object data)
			{
				this.SetData(format, true, data);
			}

			// Token: 0x060038FB RID: 14587 RVA: 0x000D1000 File Offset: 0x000D0000
			public virtual void SetData(Type format, object data)
			{
				this.SetData(format.FullName, data);
			}

			// Token: 0x060038FC RID: 14588 RVA: 0x000D100F File Offset: 0x000D000F
			public virtual void SetData(object data)
			{
				if (data is ISerializable)
				{
					this.SetData(DataFormats.Serializable, data);
					return;
				}
				this.SetData(data.GetType(), data);
			}

			// Token: 0x060038FD RID: 14589 RVA: 0x000D1034 File Offset: 0x000D0034
			private int QueryGetData(ref FORMATETC formatetc)
			{
				IntSecurity.UnmanagedCode.Assert();
				int num;
				try
				{
					num = this.QueryGetDataInner(ref formatetc);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				return num;
			}

			// Token: 0x060038FE RID: 14590 RVA: 0x000D106C File Offset: 0x000D006C
			private int QueryGetDataInner(ref FORMATETC formatetc)
			{
				return this.innerData.QueryGetData(ref formatetc);
			}

			// Token: 0x060038FF RID: 14591 RVA: 0x000D107A File Offset: 0x000D007A
			public virtual bool GetDataPresent(Type format)
			{
				return this.GetDataPresent(format.FullName);
			}

			// Token: 0x06003900 RID: 14592 RVA: 0x000D1088 File Offset: 0x000D0088
			private bool GetDataPresentInner(string format)
			{
				FORMATETC formatetc = default(FORMATETC);
				formatetc.cfFormat = (short)DataFormats.GetFormat(format).Id;
				formatetc.dwAspect = DVASPECT.DVASPECT_CONTENT;
				formatetc.lindex = -1;
				for (int i = 0; i < DataObject.ALLOWED_TYMEDS.Length; i++)
				{
					formatetc.tymed |= DataObject.ALLOWED_TYMEDS[i];
				}
				int num = this.QueryGetData(ref formatetc);
				return num == 0;
			}

			// Token: 0x06003901 RID: 14593 RVA: 0x000D10F4 File Offset: 0x000D00F4
			public virtual bool GetDataPresent(string format, bool autoConvert)
			{
				IntSecurity.ClipboardRead.Demand();
				bool flag = false;
				IntSecurity.UnmanagedCode.Assert();
				try
				{
					flag = this.GetDataPresentInner(format);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
				if (!flag && autoConvert)
				{
					string[] mappedFormats = DataObject.GetMappedFormats(format);
					if (mappedFormats != null)
					{
						for (int i = 0; i < mappedFormats.Length; i++)
						{
							if (!format.Equals(mappedFormats[i]))
							{
								IntSecurity.UnmanagedCode.Assert();
								try
								{
									flag = this.GetDataPresentInner(mappedFormats[i]);
								}
								finally
								{
									CodeAccessPermission.RevertAssert();
								}
								if (flag)
								{
									break;
								}
							}
						}
					}
				}
				return flag;
			}

			// Token: 0x06003902 RID: 14594 RVA: 0x000D118C File Offset: 0x000D018C
			public virtual bool GetDataPresent(string format)
			{
				return this.GetDataPresent(format, true);
			}

			// Token: 0x06003903 RID: 14595 RVA: 0x000D1198 File Offset: 0x000D0198
			public virtual string[] GetFormats(bool autoConvert)
			{
				IEnumFORMATETC enumFORMATETC = null;
				ArrayList arrayList = new ArrayList();
				try
				{
					enumFORMATETC = this.innerData.EnumFormatEtc(DATADIR.DATADIR_GET);
				}
				catch
				{
				}
				if (enumFORMATETC != null)
				{
					enumFORMATETC.Reset();
					FORMATETC[] array = new FORMATETC[] { default(FORMATETC) };
					int[] array2 = new int[] { 1 };
					while (array2[0] > 0)
					{
						array2[0] = 0;
						try
						{
							enumFORMATETC.Next(1, array, array2);
						}
						catch
						{
						}
						if (array2[0] > 0)
						{
							string name = DataFormats.GetFormat((int)array[0].cfFormat).Name;
							if (autoConvert)
							{
								string[] mappedFormats = DataObject.GetMappedFormats(name);
								for (int i = 0; i < mappedFormats.Length; i++)
								{
									arrayList.Add(mappedFormats[i]);
								}
							}
							else
							{
								arrayList.Add(name);
							}
						}
					}
				}
				string[] array3 = new string[arrayList.Count];
				arrayList.CopyTo(array3, 0);
				return DataObject.GetDistinctStrings(array3);
			}

			// Token: 0x06003904 RID: 14596 RVA: 0x000D12A8 File Offset: 0x000D02A8
			public virtual string[] GetFormats()
			{
				return this.GetFormats(true);
			}

			// Token: 0x04001C9C RID: 7324
			internal IDataObject innerData;
		}

		// Token: 0x020003A6 RID: 934
		private class DataStore : IDataObject
		{
			// Token: 0x06003906 RID: 14598 RVA: 0x000D12CC File Offset: 0x000D02CC
			public virtual object GetData(string format, bool autoConvert)
			{
				DataObject.DataStore.DataStoreEntry dataStoreEntry = (DataObject.DataStore.DataStoreEntry)this.data[format];
				object obj = null;
				if (dataStoreEntry != null)
				{
					obj = dataStoreEntry.data;
				}
				object obj2 = obj;
				if (autoConvert && (dataStoreEntry == null || dataStoreEntry.autoConvert) && (obj == null || obj is MemoryStream))
				{
					string[] mappedFormats = DataObject.GetMappedFormats(format);
					if (mappedFormats != null)
					{
						for (int i = 0; i < mappedFormats.Length; i++)
						{
							if (!format.Equals(mappedFormats[i]))
							{
								DataObject.DataStore.DataStoreEntry dataStoreEntry2 = (DataObject.DataStore.DataStoreEntry)this.data[mappedFormats[i]];
								if (dataStoreEntry2 != null)
								{
									obj = dataStoreEntry2.data;
								}
								if (obj != null && !(obj is MemoryStream))
								{
									obj2 = null;
									break;
								}
							}
						}
					}
				}
				if (obj2 != null)
				{
					return obj2;
				}
				return obj;
			}

			// Token: 0x06003907 RID: 14599 RVA: 0x000D1371 File Offset: 0x000D0371
			public virtual object GetData(string format)
			{
				return this.GetData(format, true);
			}

			// Token: 0x06003908 RID: 14600 RVA: 0x000D137B File Offset: 0x000D037B
			public virtual object GetData(Type format)
			{
				return this.GetData(format.FullName);
			}

			// Token: 0x06003909 RID: 14601 RVA: 0x000D138C File Offset: 0x000D038C
			public virtual void SetData(string format, bool autoConvert, object data)
			{
				if (data is Bitmap && format.Equals(DataFormats.Dib))
				{
					if (!autoConvert)
					{
						throw new NotSupportedException(SR.GetString("DataObjectDibNotSupported"));
					}
					format = DataFormats.Bitmap;
				}
				this.data[format] = new DataObject.DataStore.DataStoreEntry(data, autoConvert);
			}

			// Token: 0x0600390A RID: 14602 RVA: 0x000D13DD File Offset: 0x000D03DD
			public virtual void SetData(string format, object data)
			{
				this.SetData(format, true, data);
			}

			// Token: 0x0600390B RID: 14603 RVA: 0x000D13E8 File Offset: 0x000D03E8
			public virtual void SetData(Type format, object data)
			{
				this.SetData(format.FullName, data);
			}

			// Token: 0x0600390C RID: 14604 RVA: 0x000D13F7 File Offset: 0x000D03F7
			public virtual void SetData(object data)
			{
				if (data is ISerializable && !this.data.ContainsKey(DataFormats.Serializable))
				{
					this.SetData(DataFormats.Serializable, data);
				}
				this.SetData(data.GetType(), data);
			}

			// Token: 0x0600390D RID: 14605 RVA: 0x000D142C File Offset: 0x000D042C
			public virtual bool GetDataPresent(Type format)
			{
				return this.GetDataPresent(format.FullName);
			}

			// Token: 0x0600390E RID: 14606 RVA: 0x000D143C File Offset: 0x000D043C
			public virtual bool GetDataPresent(string format, bool autoConvert)
			{
				if (!autoConvert)
				{
					return this.data.ContainsKey(format);
				}
				string[] formats = this.GetFormats(autoConvert);
				for (int i = 0; i < formats.Length; i++)
				{
					if (format.Equals(formats[i]))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x0600390F RID: 14607 RVA: 0x000D147D File Offset: 0x000D047D
			public virtual bool GetDataPresent(string format)
			{
				return this.GetDataPresent(format, true);
			}

			// Token: 0x06003910 RID: 14608 RVA: 0x000D1488 File Offset: 0x000D0488
			public virtual string[] GetFormats(bool autoConvert)
			{
				string[] array = new string[this.data.Keys.Count];
				this.data.Keys.CopyTo(array, 0);
				if (autoConvert)
				{
					ArrayList arrayList = new ArrayList();
					for (int i = 0; i < array.Length; i++)
					{
						if (((DataObject.DataStore.DataStoreEntry)this.data[array[i]]).autoConvert)
						{
							string[] mappedFormats = DataObject.GetMappedFormats(array[i]);
							for (int j = 0; j < mappedFormats.Length; j++)
							{
								arrayList.Add(mappedFormats[j]);
							}
						}
						else
						{
							arrayList.Add(array[i]);
						}
					}
					string[] array2 = new string[arrayList.Count];
					arrayList.CopyTo(array2, 0);
					array = DataObject.GetDistinctStrings(array2);
				}
				return array;
			}

			// Token: 0x06003911 RID: 14609 RVA: 0x000D153F File Offset: 0x000D053F
			public virtual string[] GetFormats()
			{
				return this.GetFormats(true);
			}

			// Token: 0x04001C9D RID: 7325
			private Hashtable data = new Hashtable(BackCompatibleStringComparer.Default);

			// Token: 0x020003A7 RID: 935
			private class DataStoreEntry
			{
				// Token: 0x06003912 RID: 14610 RVA: 0x000D1548 File Offset: 0x000D0548
				public DataStoreEntry(object data, bool autoConvert)
				{
					this.data = data;
					this.autoConvert = autoConvert;
				}

				// Token: 0x04001C9E RID: 7326
				public object data;

				// Token: 0x04001C9F RID: 7327
				public bool autoConvert;
			}
		}
	}
}
