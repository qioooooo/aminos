using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	// Token: 0x02000277 RID: 631
	public sealed class Clipboard
	{
		// Token: 0x060021FC RID: 8700 RVA: 0x00049CE8 File Offset: 0x00048CE8
		private Clipboard()
		{
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x00049CF0 File Offset: 0x00048CF0
		private static bool IsFormatValid(DataObject data)
		{
			return Clipboard.IsFormatValid(data.GetFormats());
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x00049D00 File Offset: 0x00048D00
		internal static bool IsFormatValid(string[] formats)
		{
			if (formats != null && formats.Length <= 4)
			{
				for (int i = 0; i < formats.Length; i++)
				{
					string text;
					if ((text = formats[i]) == null || (!(text == "Text") && !(text == "UnicodeText") && !(text == "System.String") && !(text == "Csv")))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x00049D64 File Offset: 0x00048D64
		internal static bool IsFormatValid(FORMATETC[] formats)
		{
			if (formats != null && formats.Length <= 4)
			{
				for (int i = 0; i < formats.Length; i++)
				{
					short cfFormat = formats[i].cfFormat;
					if (cfFormat != 1 && cfFormat != 13 && (int)cfFormat != DataFormats.GetFormat("System.String").Id && (int)cfFormat != DataFormats.GetFormat("Csv").Id)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x00049DC7 File Offset: 0x00048DC7
		public static void SetDataObject(object data)
		{
			Clipboard.SetDataObject(data, false);
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x00049DD0 File Offset: 0x00048DD0
		public static void SetDataObject(object data, bool copy)
		{
			Clipboard.SetDataObject(data, copy, 10, 100);
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x00049DE0 File Offset: 0x00048DE0
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public static void SetDataObject(object data, bool copy, int retryTimes, int retryDelay)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (retryTimes < 0)
			{
				throw new ArgumentOutOfRangeException("retryTimes", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"retryTimes",
					retryTimes.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (retryDelay < 0)
			{
				throw new ArgumentOutOfRangeException("retryDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"retryDelay",
					retryDelay.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			DataObject dataObject = null;
			if (!(data is IDataObject))
			{
				dataObject = new DataObject(data);
			}
			bool flag = false;
			try
			{
				IntSecurity.ClipboardRead.Demand();
			}
			catch (SecurityException)
			{
				flag = true;
			}
			if (flag)
			{
				if (dataObject == null)
				{
					dataObject = data as DataObject;
				}
				if (!Clipboard.IsFormatValid(dataObject))
				{
					throw new SecurityException(SR.GetString("ClipboardSecurityException"));
				}
			}
			if (dataObject != null)
			{
				dataObject.RestrictedFormats = flag;
			}
			int num = retryTimes;
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				int num2;
				do
				{
					if (data is IDataObject)
					{
						num2 = UnsafeNativeMethods.OleSetClipboard((IDataObject)data);
					}
					else
					{
						num2 = UnsafeNativeMethods.OleSetClipboard(dataObject);
					}
					if (num2 != 0)
					{
						if (num == 0)
						{
							Clipboard.ThrowIfFailed(num2);
						}
						num--;
						Thread.Sleep(retryDelay);
					}
				}
				while (num2 != 0);
				if (copy)
				{
					num = retryTimes;
					do
					{
						num2 = UnsafeNativeMethods.OleFlushClipboard();
						if (num2 != 0)
						{
							if (num == 0)
							{
								Clipboard.ThrowIfFailed(num2);
							}
							num--;
							Thread.Sleep(retryDelay);
						}
					}
					while (num2 != 0);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x00049F90 File Offset: 0x00048F90
		public static IDataObject GetDataObject()
		{
			IntSecurity.ClipboardRead.Demand();
			if (Application.OleRequired() == ApartmentState.STA)
			{
				return Clipboard.GetDataObject(10, 100);
			}
			if (Application.MessageLoop)
			{
				throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
			}
			return null;
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x00049FC8 File Offset: 0x00048FC8
		private static IDataObject GetDataObject(int retryTimes, int retryDelay)
		{
			IDataObject dataObject = null;
			int num = retryTimes;
			int num2;
			do
			{
				num2 = UnsafeNativeMethods.OleGetClipboard(ref dataObject);
				if (num2 != 0)
				{
					if (num == 0)
					{
						Clipboard.ThrowIfFailed(num2);
					}
					num--;
					Thread.Sleep(retryDelay);
				}
			}
			while (num2 != 0);
			if (dataObject == null)
			{
				return null;
			}
			if (dataObject is IDataObject && !Marshal.IsComObject(dataObject))
			{
				return (IDataObject)dataObject;
			}
			return new DataObject(dataObject);
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x0004A01C File Offset: 0x0004901C
		public static void Clear()
		{
			Clipboard.SetDataObject(new DataObject());
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x0004A028 File Offset: 0x00049028
		public static bool ContainsAudio()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(DataFormats.WaveAudio, false);
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x0004A04C File Offset: 0x0004904C
		public static bool ContainsData(string format)
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(format, false);
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x0004A06C File Offset: 0x0004906C
		public static bool ContainsFileDropList()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop, true);
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x0004A090 File Offset: 0x00049090
		public static bool ContainsImage()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(DataFormats.Bitmap, true);
		}

		// Token: 0x0600220A RID: 8714 RVA: 0x0004A0B4 File Offset: 0x000490B4
		public static bool ContainsText()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				return Clipboard.ContainsText(TextDataFormat.Text);
			}
			return Clipboard.ContainsText(TextDataFormat.UnicodeText);
		}

		// Token: 0x0600220B RID: 8715 RVA: 0x0004A0E4 File Offset: 0x000490E4
		public static bool ContainsText(TextDataFormat format)
		{
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(Clipboard.ConvertToDataFormats(format), false);
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x0004A130 File Offset: 0x00049130
		public static Stream GetAudioStream()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null)
			{
				return dataObject.GetData(DataFormats.WaveAudio, false) as Stream;
			}
			return null;
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x0004A15C File Offset: 0x0004915C
		public static object GetData(string format)
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null)
			{
				return dataObject.GetData(format);
			}
			return null;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x0004A17C File Offset: 0x0004917C
		public static StringCollection GetFileDropList()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			StringCollection stringCollection = new StringCollection();
			if (dataObject != null)
			{
				string[] array = dataObject.GetData(DataFormats.FileDrop, true) as string[];
				if (array != null)
				{
					stringCollection.AddRange(array);
				}
			}
			return stringCollection;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x0004A1B8 File Offset: 0x000491B8
		public static Image GetImage()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null)
			{
				return dataObject.GetData(DataFormats.Bitmap, true) as Image;
			}
			return null;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0004A1E1 File Offset: 0x000491E1
		public static string GetText()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				return Clipboard.GetText(TextDataFormat.Text);
			}
			return Clipboard.GetText(TextDataFormat.UnicodeText);
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x0004A210 File Offset: 0x00049210
		public static string GetText(TextDataFormat format)
		{
			/*
An exception occurred when decompiling this method (06002211)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String System.Windows.Forms.Clipboard::GetText(System.Windows.Forms.TextDataFormat)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.BuildGraph(List`1 nodes, ILLabel entryLabel) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 92
   at ICSharpCode.Decompiler.ILAst.LoopsAndConditions.FindLoops(ILBlock block) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\LoopsAndConditions.cs:line 53
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 343
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x0004A269 File Offset: 0x00049269
		public static void SetAudio(byte[] audioBytes)
		{
			if (audioBytes == null)
			{
				throw new ArgumentNullException("audioBytes");
			}
			Clipboard.SetAudio(new MemoryStream(audioBytes));
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x0004A284 File Offset: 0x00049284
		public static void SetAudio(Stream audioStream)
		{
			if (audioStream == null)
			{
				throw new ArgumentNullException("audioStream");
			}
			IDataObject dataObject = new DataObject();
			dataObject.SetData(DataFormats.WaveAudio, false, audioStream);
			Clipboard.SetDataObject(dataObject, true);
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x0004A2BC File Offset: 0x000492BC
		public static void SetData(string format, object data)
		{
			IDataObject dataObject = new DataObject();
			dataObject.SetData(format, data);
			Clipboard.SetDataObject(dataObject, true);
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x0004A2E0 File Offset: 0x000492E0
		public static void SetFileDropList(StringCollection filePaths)
		{
			if (filePaths == null)
			{
				throw new ArgumentNullException("filePaths");
			}
			if (filePaths.Count == 0)
			{
				throw new ArgumentException(SR.GetString("CollectionEmptyException"));
			}
			foreach (string text in filePaths)
			{
				try
				{
					Path.GetFullPath(text);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					throw new ArgumentException(SR.GetString("Clipboard_InvalidPath", new object[] { text, "filePaths" }), ex);
				}
			}
			if (filePaths.Count > 0)
			{
				IDataObject dataObject = new DataObject();
				string[] array = new string[filePaths.Count];
				filePaths.CopyTo(array, 0);
				dataObject.SetData(DataFormats.FileDrop, true, array);
				Clipboard.SetDataObject(dataObject, true);
			}
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x0004A3D8 File Offset: 0x000493D8
		public static void SetImage(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			IDataObject dataObject = new DataObject();
			dataObject.SetData(DataFormats.Bitmap, true, image);
			Clipboard.SetDataObject(dataObject, true);
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x0004A40D File Offset: 0x0004940D
		public static void SetText(string text)
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				Clipboard.SetText(text, TextDataFormat.Text);
				return;
			}
			Clipboard.SetText(text, TextDataFormat.UnicodeText);
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x0004A440 File Offset: 0x00049440
		public static void SetText(string text, TextDataFormat format)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException("text");
			}
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			IDataObject dataObject = new DataObject();
			dataObject.SetData(Clipboard.ConvertToDataFormats(format), false, text);
			Clipboard.SetDataObject(dataObject, true);
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x0004A4A4 File Offset: 0x000494A4
		private static string ConvertToDataFormats(TextDataFormat format)
		{
			switch (format)
			{
			case TextDataFormat.Text:
				return DataFormats.Text;
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

		// Token: 0x0600221A RID: 8730 RVA: 0x0004A4F4 File Offset: 0x000494F4
		private static void ThrowIfFailed(int hr)
		{
			if (hr != 0)
			{
				ExternalException ex = new ExternalException(SR.GetString("ClipboardOperationFailed"), hr);
				throw ex;
			}
		}
	}
}
