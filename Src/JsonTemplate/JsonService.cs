using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace JsonTemplate
{
	public static class JsonService
	{
		public static string ConvertJson(string jsonTemplate, string jsonData, string sourceName = "source")
		{
			var templateObject = JObject.Parse(jsonTemplate);
			var dataObject = JObject.Parse(jsonData);

			ConvertJToken(templateObject, dataObject);

			return "";
		}

		private static void ConvertJToken(JToken templateObject, JObject dataObject)
		{
			if (templateObject.Type == JTokenType.Array)
			{
				JArray jArray = (JArray)templateObject;
				foreach (var jItem in jArray)
					ConvertJToken(jItem, dataObject);
				return;
			}
			if (templateObject.Type != JTokenType.Object)
				return;

			JObject jObject = (JObject)templateObject;
			foreach (var property in jObject)
			{
				if (property.Value == null)
					continue;

				if (property.Value.Type == JTokenType.Object)
					ConvertJToken(property.Value, dataObject);

				if (property.Value.Type == JTokenType.Array)
					ConvertJToken(property.Value, dataObject);

				if (!property.Value.ToString().StartsWith("$"))
					continue;

				templateObject[property.Key] = GetPropertyValue((JToken)property.Value, dataObject);
			}
		}

		private static JToken GetPropertyValue(JToken propertyValue, JObject dataObject)
		{
			if (propertyValue.Type == JTokenType.Null)
				return null;

			var cleanValue = propertyValue.ToString().Substring(1);
			var sequence = ValidateSequence(cleanValue);

			if (!sequence.HasValue)
				return new JValue(propertyValue);

			if (sequence.Value.Key == "foreach")
			{
				var jsonPath = sequence.Value.Value[0];
				var jArray = dataObject.SelectToken(jsonPath);
				if (jArray.Type != JTokenType.Array)
					throw new Exception($"{jsonPath} must be array");

				var newJArray = new JArray();
				foreach (var jItem in (JArray)jArray)
				{
					var templateObject = JObject.Parse(sequence.Value.Value[1]);

					if (jItem.Type != JTokenType.Object)
						throw new Exception($"{jsonPath} must be array of objects");

					ConvertJToken(templateObject, (JObject)jItem);
					newJArray.Add(templateObject);
				}

				return newJArray;
			}

			if (sequence.Value.Key == "jsonPath")
			{
				var jsonPath = sequence.Value.Value[0];
				return dataObject.SelectToken(jsonPath);
			}

			return propertyValue;
		}

		private static KeyValuePair<string, List<string>>? ValidateSequence(string cleanValue)
		{
			var savedValue = cleanValue.ToString();
			foreach (var sequence in Sequences)
			{
				cleanValue = savedValue;
				var parameters = new List<string>();
				var valid = true;

				foreach (var word in sequence.Value)
				{
					if (word == "*")
					{
						parameters.Add(cleanValue);
						break;
					}

					var nextWord = GetFirstWordAndCleanValue(ref cleanValue);
					if (word == "$")
						parameters.Add(nextWord);
					else if (word != nextWord)
					{
						valid = false;
						break;
					}
				}

				if (valid)
					return new KeyValuePair<string, List<string>> (sequence.Key, parameters);
			}

			return null;
		}

		private static string GetFirstWordAndCleanValue(ref string cleanValue)
		{
			var firstWord = Regex.Match(cleanValue, @"(.*?) |.*").Value.Trim();
			cleanValue = cleanValue.Substring(firstWord.Length).Trim();
			return firstWord;
		}

		private static Dictionary<string, List<string>> Sequences = new Dictionary<string, List<string>>()
		{
			{
				"foreach", new List<string>()
				{
					"foreach", "$", "use", "*"
				}
			},
			{
				"jsonPath", new List<string>()
				{
					"$"
				}
			}
		};
	}
}
