using System;
using System.Collections;

namespace System.Web.UI.Design
{
	public interface IFolderProjectItem
	{
		ICollection Children { get; }

		IDocumentProjectItem AddDocument(string name, byte[] content);

		IFolderProjectItem AddFolder(string name);
	}
}
