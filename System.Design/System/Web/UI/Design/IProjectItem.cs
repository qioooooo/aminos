using System;

namespace System.Web.UI.Design
{
	public interface IProjectItem
	{
		string AppRelativeUrl { get; }

		string Name { get; }

		IProjectItem Parent { get; }

		string PhysicalPath { get; }
	}
}
