using System.Runtime.Serialization;

namespace JPush.Models
{
	/// <summary>
	/// Enum PushPlatform
	/// </summary>
	[DataContract]
	public enum PushPlatform
	{
		/// <summary>
		/// None
		/// </summary>
		[EnumMember]
		None = 0,

		/// <summary>
		/// Android
		/// </summary>
		[EnumMember]
		Android = 1,

		/// <summary>
		/// iOS
		/// </summary>
		[EnumMember]
		iOS = 2,

		/// <summary>
		/// Android and iOS
		/// </summary>
		[EnumMember]
		AndroidAndiOS = Android | iOS
	}
}