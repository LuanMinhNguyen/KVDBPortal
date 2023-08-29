using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.CostContract;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.CostContract
{
   /// <summary>
      /// The category service.
      /// </summary>
        public class ShipmentDocumentFileService
        {
            /// <summary>
            /// The repo.
            /// </summary>
            private readonly ShipmentDocumentFileDAO repo;

            /// <summary>
            /// Initializes a new instance of the <see cref="ShipmentDocumentFileService"/> class.
            /// </summary>
            public ShipmentDocumentFileService()
            {
                this.repo = new ShipmentDocumentFileDAO();
            }

            #region Get (Advances)

            /// <summary>
            /// The get specific.
            /// </summary>
            /// <param name="tranId">
            /// The tran id.
            /// </param>
            /// <returns>
            /// The <see cref="List"/>.
            /// </returns>
            public List<ShipmentDocumentFile> GetOfShipment(Guid tranId)
            {
                return this.repo.GetAllShipmentId(tranId);
            }
            #endregion

            #region GET (Basic)
            /// <summary>
            /// Get All Categories
            /// </summary>
            /// <returns>
            /// The category
            /// </returns>
            public List<ShipmentDocumentFile> GetAll()
            {
                return this.repo.GetAll().ToList();
            }

         /// <summary>
         /// Get of type document
         /// </summary>
         /// <param name="prId"></param>
         /// <returns></returns>
            public List<ShipmentDocumentFile> GetAllType(string prId)
            {
                return this.repo.GetAll().Where(t => t.TypeId == prId).ToList();
            }

            /// <summary>
            /// Get Resource By ID
            /// </summary>
            /// <param name="id">
            /// ID of category
            /// </param>
            /// <returns>
            /// The category</returns>
            public ShipmentDocumentFile GetById(int id)
            {
                return this.repo.GetById(id);
            }

           /// <summary>
           ///  get name server
           /// </summary>
           /// <param name="nameServer"></param>
           /// <returns></returns>
            public ShipmentDocumentFile GetByNameServer(string nameServer)
            {
                return this.repo.GetAll().Find(t => t.FilePath.ToLower().Contains(nameServer.ToLower()));
            }
            #endregion

            #region Insert, Update, Delete
            /// <summary>
            /// Insert Resource
            /// </summary>
            /// <param name="bo"></param>
            /// <returns></returns>
            public int? Insert(ShipmentDocumentFile bo)
            {
                return this.repo.Insert(bo);
            }

            /// <summary>
            /// Update Resource
            /// </summary>
            /// <param name="bo"></param>
            /// <returns></returns>
            public bool Update(ShipmentDocumentFile bo)
            {
                try
                {
                    return this.repo.Update(bo);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            /// <summary>
            /// Delete Resource
            /// </summary>
            /// <param name="bo"></param>
            /// <returns></returns>
            public bool Delete(ShipmentDocumentFile bo)
            {
                try
                {
                    return this.repo.Delete(bo);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            /// <summary>
            /// Delete Resource By ID
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public bool Delete(int id)
            {
                try
                {
                    return this.repo.Delete(id);
                }
                catch (Exception)
                {
                    return false;
                }
            }
            #endregion
        }
    }
