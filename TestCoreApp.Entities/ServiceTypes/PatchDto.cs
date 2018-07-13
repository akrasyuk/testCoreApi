namespace TestCoreApp.Entities.ServiceTypes
{
    /// <summary>
    /// Entity to aplly PATCH
    /// </summary>
    public class PatchDto
    {
        /// <summary>
        /// Name of property.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Value of property.
        /// </summary>
        public object PropertyValue { get; set; }
    }
}
