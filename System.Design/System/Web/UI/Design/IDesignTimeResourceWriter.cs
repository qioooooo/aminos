using System;
using System.Resources;

namespace System.Web.UI.Design
{
	public interface IDesignTimeResourceWriter : IResourceWriter, IDisposable
	{
		string CreateResourceKey(string resourceName, object obj);
	}
}
