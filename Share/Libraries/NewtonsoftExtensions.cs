﻿namespace Share
{
	
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;

	public static class NewtonsoftExtensions
	{
		public static T ToObject<T>(this string str)
		{
			return JsonConvert.DeserializeObject<T>(str);
		}

		public static string ToJsonString(this object obj){
			return JsonConvert.SerializeObject(obj);
		}
		public static string GetString(this JObject obj, string key, string defaultValue = null)
		{
			return obj.ContainsKey(key) ? obj.GetValue(key).ToString() : defaultValue;
		}

		public static T[] GetArray<T>(this JObject obj, string key, T[] defaultValue = null)
		{
			return obj.ContainsKey(key) ? obj.GetValue(key).ToObject<T[]>() : defaultValue;
		}

		public static T[] GetArrayFromChain<T>(this JObject jObject, string[] keys, T[] defaultValue = null)
		{
			var obj = jObject;
			for (int i = 0, j = keys.Length; i < j; i++) {
				if (i + 1 < j) {
					if (obj.ContainsKey(keys[i]))
						obj = obj.GetValue(keys[i]).ToObject<JObject>();
					else
						return defaultValue;
				} else {
					return GetArray<T>(obj, keys[i]);
				}
			}

			return defaultValue;
		}

		public static string GetStringFromChain(this JObject jObject, string[] keys, string defaultValue = null)
		{
			var obj = jObject;
			for (int i = 0, j = keys.Length; i < j; i++) {
				if (i + 1 < j) {
					if (obj.ContainsKey(keys[i]))
						obj = obj.GetValue(keys[i]).ToObject<JObject>();
					else
						return defaultValue;
				} else {
					return GetString(obj, keys[i]);
				}
			}

			return defaultValue;
		}
	}
}