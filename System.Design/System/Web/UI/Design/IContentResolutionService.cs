using System;
using System.Collections;

namespace System.Web.UI.Design
{
	public interface IContentResolutionService
	{
		ContentDesignerState GetContentDesignerState(string identifier);

		void SetContentDesignerState(string identifier, ContentDesignerState state);

		IDictionary ContentDefinitions { get; }
	}
}
