using System;
using System.Reflection;

namespace JPush.TestServer.Extensions
{
	public static class TypeExtensions
	{
		/// <summary>
		/// Gets the custom attribute from the MemberInfo.
		/// </summary>
		/// <typeparam name="T">Type from the Attribute</typeparam>
		/// <param name="element">The element.</param>
		/// <returns>Attribute if found, else null</returns>
		public static T GetCustomAttribute<T>(this MemberInfo element)
			where T : Attribute
		{
			var attr = Attribute.GetCustomAttribute(element, typeof(T));

			return attr is T ? (T)attr : null;
		}
	}
}