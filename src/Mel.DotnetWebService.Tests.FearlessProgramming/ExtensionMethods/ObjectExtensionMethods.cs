namespace Mel.DotnetWebService.Tests.FearlessProgramming.ExtensionMethods;

 static class ObjectExtensionMethods
{
	public static T FromJArrayTo<T>(this object obj)
	=> ((Newtonsoft.Json.Linq.JArray) obj).ToObject<T>();
}
