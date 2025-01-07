using System;
using System.IO;

namespace System.Web.UI.Design
{
	public interface IDocumentProjectItem
	{
		Stream GetContents();

		void Open();
	}
}
