using System;

namespace System.Web.UI.Design
{
	public sealed class ClientScriptItem
	{
		public ClientScriptItem(string text, string source, string language, string type, string id)
		{
			this._text = text;
			this._source = source;
			this._language = language;
			this._type = type;
			this._id = id;
		}

		public string Id
		{
			get
			{
				return this._id;
			}
		}

		public string Language
		{
			get
			{
				return this._language;
			}
		}

		public string Source
		{
			get
			{
				return this._source;
			}
		}

		public string Text
		{
			get
			{
				return this._text;
			}
		}

		public string Type
		{
			get
			{
				return this._type;
			}
		}

		private string _text;

		private string _source;

		private string _language;

		private string _type;

		private string _id;
	}
}
