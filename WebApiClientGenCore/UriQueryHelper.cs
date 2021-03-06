﻿using Fonlow.Web.Meta;
using System;
using System.Diagnostics;
using System.Linq;
using Tavis.UriTemplates;

namespace Fonlow.CodeDom.Web
{
	/// <summary>
	/// Generate a client function upon ApiDescription
	/// </summary>
	public static class UriQueryHelper
	{
		public static string CreateUriQuery(string uriText, ParameterDescription[] parameterDescriptions)
		{
			var template = new UriTemplate(uriText);
			var parameterNames = template.GetParameterNames().ToArray();
			if (parameterNames.Length == 0)
				return null;

			string newUriText = uriText;

			for (int i = 0; i < parameterNames.Length; i++)
			{
				var name = parameterNames[i];//PathSegmentVariableNames[i] always give uppercase
				var d = parameterDescriptions.FirstOrDefault(r => r.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
				Debug.Assert(d != null);

				newUriText = UriTemplateTransform.Transform(newUriText, d);
			}

			return newUriText;
		}

		public static string CreateUriQueryForTs(string uriText, ParameterDescription[] parameterDescriptions)
		{
			var template = new UriTemplate(uriText);
			var parameterNames = template.GetParameterNames().ToArray();
			if (parameterNames.Length == 0)
				return null;

			string newUriText = uriText;

			for (int i = 0; i < parameterNames.Length; i++)
			{
				var name = parameterNames[i];//PathSegmentVariableNames[i] always give uppercase
				var d = parameterDescriptions.FirstOrDefault(r => r.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
				Debug.Assert(d != null);
				newUriText = UriTemplateTransform.TransformForTs(newUriText, d);
			}

			return newUriText;
		}

	}

}
