using System;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design
{
	[Obsolete("Use of this type is not recommended because template editing is handled in ControlDesigner. To support template editing expose template data in the TemplateGroups property and call SetViewFlags(ViewFlags.TemplateEditing, true). http://go.microsoft.com/fwlink/?linkid=14202")]
	public interface ITemplateEditingFrame : IDisposable
	{
		Style ControlStyle { get; }

		string Name { get; }

		int InitialHeight { get; set; }

		int InitialWidth { get; set; }

		string[] TemplateNames { get; }

		Style[] TemplateStyles { get; }

		TemplateEditingVerb Verb { get; set; }

		void Close(bool saveChanges);

		void Open();

		void Resize(int width, int height);

		void Save();

		void UpdateControlName(string newName);
	}
}
