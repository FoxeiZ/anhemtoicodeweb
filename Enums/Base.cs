namespace anhemtoicodeweb.Enums
{
    public class CustomEnum
    {
        /// <summary>
        /// Gets the name of the enum.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the localized value of the enum.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomEnum"/> class with the same name and value.
        /// </summary>
        /// <param name="name">The name and value of the enum.</param>

        public CustomEnum(string name)
        {
            Name = Value = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomEnum"/> class with different name and value.
        /// </summary>
        /// <param name="name">The name of the enum.</param>
        /// <param name="value">The localized value of the enum.</param>
        public CustomEnum(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}