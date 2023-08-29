namespace EDMs.Business.Services.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Library;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class DocPropertiesService
    {

        private List<Property> properties = new List<Property>()
            {
                new Property { ID = 1, Name = "Name" },
                new Property { ID = 2, Name = "Description" },
                new Property { ID = 3, Name = "Revision" },
                new Property { ID = 4, Name = "Vendor Name" },
                new Property { ID = 5, Name = "Drawing Number" },
                new Property { ID = 6, Name = "Year" },
                new Property { ID = 7, Name = "Plant" },
                new Property { ID = 8, Name = "System" },
                new Property { ID = 9, Name = "Discipline" },
                new Property { ID = 10, Name = "Document Type" },
                new Property { ID = 11, Name = "Tags" },
                new Property { ID = 12, Name = "Project" },
                new Property { ID = 13, Name = "Block" },
                new Property { ID = 14, Name = "Field" },
                new Property { ID = 15, Name = "Platform" },
                new Property { ID = 16, Name = "Well" },
                new Property { ID = 17, Name = "Start Date" },
                new Property { ID = 18, Name = "End Date" },
                new Property { ID = 19, Name = "Number Of Work" },
                new Property { ID = 20, Name = "Tag No" },
                new Property { ID = 21, Name = "Tag Des" },
                new Property { ID = 22, Name = "Manufacturers" },
                new Property { ID = 23, Name = "Serial No" },
                new Property { ID = 24, Name = "Model No" },
                new Property { ID = 25, Name = "Asset No" },
                new Property { ID = 26, Name = "Table Of Contents" },
                new Property { ID = 27, Name = "Publish Date" },
                new Property { ID = 28, Name = "From" },
                new Property { ID = 29, Name = "To" },
                new Property { ID = 30, Name = "Signer" },
                new Property { ID = 31, Name = "Other" },
                new Property { ID = 32, Name = "RIG" },
                new Property { ID = 33, Name = "Kind of repair" },
            };

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Property> GetAll()
        {
            return this.properties;
        }
    }

    /// <summary>
    /// The property.
    /// </summary>
    public class Property
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
