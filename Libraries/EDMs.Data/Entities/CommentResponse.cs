
using EDMs.Data.DAO.Library;

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Scope;

    /// <summary>
    /// The package.
    /// </summary>
    public partial class CommentResponse
    {
        private CodeDAO codeService = new CodeDAO();
        /// <summary>
        /// Gets the project obj.
        /// </summary>
        public bool IsFinal
        {
            get
            {
                return this.ReceiveCodeID.GetValueOrDefault() != 0 && codeService.GetById(this.ReceiveCodeID.GetValueOrDefault()).IsFinal.GetValueOrDefault();
            }
        }
    }
}
