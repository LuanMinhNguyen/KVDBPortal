// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentNew.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DocumentNew type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using EDMs.Data.DAO.Security;
using EDMs.Data.DAO.Document;

namespace EDMs.Data.Entities
{
    using System.Linq;

    using EDMs.Data.DAO.Document;

    /// <summary>
    /// The document new.
    /// </summary>
    public partial class DQREDocument
    {
        
        ////public bool HasAttachFile
        ////{
        ////    get
        ////    {
        ////        var temp = new DQREDocumentAttachFileDAO();
        ////        return temp.GetAll().Any(t => t.ProjectDocumentId == this.ID);
        ////    }
        ////}

        //////public string TransmittalNumber
        //////{
        //////    get
        //////    {
        //////        try
        //////        {
        //////            var attach = new AttachDocToTransmittalDAO();
        //////            var trans = new DQRETransmittalDAO();
        //////            var attachobj = attach.GetAll().FirstOrDefault(t => t.DocumentId == this.ID);
        //////            var transobj = trans.GetById(attachobj.TransmittalId.GetValueOrDefault());
        //////            return transobj != null ? transobj.TransmittalNo : "";
        //////        }catch
        //////        { return ""; }
        //////    }
        //////}
        ////public string CreatedByName
        ////{
        ////    get
        ////    {
        ////        var user = new UserDAO();
        ////        return this.CreatedBy != null? user.GetByID(this.CreatedBy.GetValueOrDefault()).FullName :"";
        ////    }
        ////}
        ////public string LastUpdateByName
        ////{
        ////    get
        ////    {
        ////        var user = new UserDAO();
        ////        return this.LastUpdatedBy != null ? user.GetByID(this.LastUpdatedBy.GetValueOrDefault()).FullName : "";
        ////    }
        ////}
    }
}
