using System.Collections.Generic;
using System.Runtime.Serialization;

namespace JPush.V2.ExternalModels
{
    /// <summary>
    /// Class PushMessage.
    /// <see cref="PushMessageRequest"/> and <see cref="PushMessage"/> work together to send out push request.
    /// </summary>
    [DataContract]
    [KnownType(typeof(MessageType))]
    public class PushMessage
    {
        #region Constants

        protected const string PostKeyContent = "";

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        [DataMember]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the customized value.
        /// </summary>
        /// <value>The customized value.</value>
        [DataMember]
        public Dictionary<string, string> CustomizedValue { get; set; }

        #region iOS only

        /// <summary>
        /// Gets or sets the badge value.
        /// </summary>
        /// <value>The badge value.</value>
        [DataMember]
        public int BadgeValue { get; set; }

        /// <summary>
        /// Gets or sets the sound.
        /// For iOS only.
        /// </summary>
        /// <value>The sound.</value>
        [DataMember]
        public string Sound { get; set; }

        #endregion

        #region Android Only

        /// <summary>
        /// Gets or sets the push title.
        /// For Android Only.
        /// </summary>
        /// <value>The push title.</value>
        [DataMember]
        public string PushTitle { get; set; }

        /// <summary>
        /// Gets or sets the builder unique identifier.
        /// For Android Only.
        /// Default is 0. Valid value is 1-1000
        /// </summary>
        /// <value>The builder unique identifier.</value>
        [DataMember]
        public int BuilderId { get; set; }

        #endregion

        #endregion
    }
}