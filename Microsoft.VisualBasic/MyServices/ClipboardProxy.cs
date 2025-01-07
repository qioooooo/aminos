using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Microsoft.VisualBasic.MyServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class ClipboardProxy
	{
		internal ClipboardProxy()
		{
		}

		public string GetText()
		{
			return Clipboard.GetText();
		}

		public string GetText(TextDataFormat format)
		{
			return Clipboard.GetText(format);
		}

		public bool ContainsText()
		{
			return Clipboard.ContainsText();
		}

		public bool ContainsText(TextDataFormat format)
		{
			return Clipboard.ContainsText(format);
		}

		public void SetText(string text)
		{
			Clipboard.SetText(text);
		}

		public void SetText(string text, TextDataFormat format)
		{
			Clipboard.SetText(text, format);
		}

		public Image GetImage()
		{
			return Clipboard.GetImage();
		}

		public bool ContainsImage()
		{
			return Clipboard.ContainsImage();
		}

		public void SetImage(Image image)
		{
			Clipboard.SetImage(image);
		}

		public Stream GetAudioStream()
		{
			return Clipboard.GetAudioStream();
		}

		public bool ContainsAudio()
		{
			return Clipboard.ContainsAudio();
		}

		public void SetAudio(byte[] audioBytes)
		{
			Clipboard.SetAudio(audioBytes);
		}

		public void SetAudio(Stream audioStream)
		{
			Clipboard.SetAudio(audioStream);
		}

		public StringCollection GetFileDropList()
		{
			return Clipboard.GetFileDropList();
		}

		public bool ContainsFileDropList()
		{
			return Clipboard.ContainsFileDropList();
		}

		public void SetFileDropList(StringCollection filePaths)
		{
			Clipboard.SetFileDropList(filePaths);
		}

		public object GetData(string format)
		{
			return Clipboard.GetData(format);
		}

		public bool ContainsData(string format)
		{
			return Clipboard.ContainsData(format);
		}

		public void SetData(string format, object data)
		{
			Clipboard.SetData(format, data);
		}

		public void Clear()
		{
			Clipboard.Clear();
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IDataObject GetDataObject()
		{
			return Clipboard.GetDataObject();
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void SetDataObject(DataObject data)
		{
			Clipboard.SetDataObject(data);
		}
	}
}
