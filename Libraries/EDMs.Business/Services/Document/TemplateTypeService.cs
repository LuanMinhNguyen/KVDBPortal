using System.Collections.Generic;
using System.Linq;

namespace EDMs.Business.Services.Document
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class TemplateTypeService
    {

        private readonly List<TemplateType> templateType = new List<TemplateType>()
            {
                new TemplateType { ID = 1, Name = "EMDR Master report" },
                new TemplateType { ID = 2, Name = "EDMR report for workgroup" },
                new TemplateType { ID = 3, Name = "Process report" },
                new TemplateType { ID = 4, Name = "Transmittal template" },
            };

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<TemplateType> GetAll()
        {
            return this.templateType;
        }

        public TemplateType GetById(int id)
        {
            return this.templateType.FirstOrDefault(t => t.ID == id);
        }
    }

    /// <summary>
    /// The property.
    /// </summary>
    public class TemplateType
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
