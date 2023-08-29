// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataPermission.cs" company="">
//   
// </copyright>
// <summary>
//   The data permissions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.Entities
{
    /// <summary>
    /// The data permissions.
    /// </summary>
    public class FolderPermissionItem
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is group.
        /// </summary>
        public bool IsGroup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is full permission.
        /// </summary>
        public bool IsFullPermission { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int ID { get; set; }
        public int roleId { get; set; }
    }
}
