namespace EDMs.Business.Services.Library
{
    using System.Collections.Generic;

    /// <summary>
    /// The category service.
    /// </summary>
    public class OptionalTypePropertiesService
    {

        private List<OptionalTypeProperty> properties = new List<OptionalTypeProperty>()
            {
                new OptionalTypeProperty { ID = 1, Name = "Serial" },
                new OptionalTypeProperty { ID = 2, Name = "Model" },
                new OptionalTypeProperty { ID = 3, Name = "P&ID number" },
                new OptionalTypeProperty { ID = 4, Name = "Start date" },
                new OptionalTypeProperty { ID = 5, Name = "End date" },
                new OptionalTypeProperty { ID = 6, Name = "System" },
                new OptionalTypeProperty { ID = 7, Name = "Duty/Capacity" },
                new OptionalTypeProperty { ID = 8, Name = "Des. Temp." },
                new OptionalTypeProperty { ID = 9, Name = "Des. Press." },
                new OptionalTypeProperty { ID = 10, Name = "Diff. Pres." },
                new OptionalTypeProperty { ID = 11, Name = "Vendor" },
            };

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<OptionalTypeProperty> GetAll()
        {
            return this.properties;
        }
    }

    /// <summary>
    /// The property.
    /// </summary>
    public class OptionalTypeProperty
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
    }
}
