using System;

namespace System.Web.UI.Design
{
	public interface IControlDesignerTag
	{
		bool IsDirty { get; }

		string GetAttribute(string name);

		string GetContent();

		void RemoveAttribute(string name);

		void SetAttribute(string name, string value);

		void SetContent(string content);

		void SetDirty(bool dirty);

		string GetOuterContent();
	}
}
