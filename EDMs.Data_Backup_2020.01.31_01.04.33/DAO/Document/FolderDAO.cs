// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FolderDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class FolderDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderDAO"/> class.
        /// </summary>
        public FolderDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Folder> GetIQueryable()
        {
            return this.EDMsDataContext.Folders;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Folder> GetAll()
        {
            return this.EDMsDataContext.Folders.ToList();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public Folder GetById(int id)
        {
            return this.EDMsDataContext.Folders.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

        /// <summary>
        /// The get all by category.
        /// </summary>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Folder> GetAllByCategory(int categoryId)
        {
            return this.EDMsDataContext.Folders.Where(t => t.CategoryID == categoryId).ToList();
        }

        public Folder GetByDirName(string dirName)
        {
            return this.EDMsDataContext.Folders.FirstOrDefault(t => t.DirName == dirName);
        }
        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="ob">
        /// The ob.
        /// </param>
        /// <returns>
        /// The <see cref="int?"/>.
        /// </returns>
        public int? Insert(Folder ob)
        {
            try
            {
                this.EDMsDataContext.AddToFolders(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="src">
        /// Entity for update
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if update success, false if not
        /// </returns>
        public bool Update(Folder src)
        {
            try
            {
                Folder des = (from rs in this.EDMsDataContext.Folders
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.DirName = src.DirName;
                des.Description = src.Description;
                des.CategoryID = des.CategoryID;
                des.ParentID = src.ParentID;

                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="src">
        /// The src.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if delete success, false if not
        /// </returns>
        public bool Delete(Folder src)
        {
            try
            {
                var des = this.GetById(src.ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete By ID
        /// </summary>
        /// <param name="ID"></param>
        /// ID of entity
        /// <returns></returns>
        public bool Delete(int ID)
        {
            try
            {
                var des = this.GetById(ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Gets all parents menu items by children.
        /// </summary>
        /// <param name="children">The children.</param>
        /// <returns></returns>
        public IEnumerable<Folder> GetAllParentsFolderItemsByChildren(IEnumerable<Folder> children)
        {
            var parents = new List<Folder>();
            foreach (var child in children)
            {
                var temp = child;
                while (temp.ParentID != null)
                {
                    var parentMenu = this.GetById(temp.ParentID.Value);
                    temp = parentMenu;

                    if (parents.All(x => x.ID != parentMenu.ID))
                    {
                        parents.Add(parentMenu);
                    }
                }
            }
            return parents;
        }


        /// <summary>
        /// Gets all related permitted menu items.
        /// </summary>
        /// <param name="categoryId">
        /// The category Id.
        /// </param>
        /// <returns>
        /// </returns>
        public List<Folder> GetAllRelatedPermittedFolderItems(List<int> listFolId)
        {
            //Get all menu have permitted for current role.
            var categories = this.GetSpecificFolder(listFolId);

            if (categories != null)
            {

                //Gets and adds all parent menu items into the list.
                ////categories.AddRange(GetAllParentsFolderItemsByChildren(categories));
                return categories;
            }
            return new List<Folder>();
        }

        /// <summary>
        /// The get specific document.
        /// </summary>
        /// <param name="listDocId">
        /// The list doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Folder> GetSpecificFolder(List<int> listFolId)
        {
            return this.EDMsDataContext.Folders.ToArray().Where(t => t.ParentID == null && listFolId.Contains(t.ID)).ToList();
        }

        public List<Folder> GetSpecificFolderStatic(List<int> listFolId)
        {
            return this.EDMsDataContext.Folders.ToArray().Where(t => listFolId.Contains(t.ID)).ToList();
        }

        public List<Folder> GetAllSpecificFolder(List<int> listFolId)
        {
            return this.EDMsDataContext.Folders.ToArray().Where(t => listFolId.Contains(t.ID)).ToList();
        }

        public List<Folder> GetAllByParentId(int parentId, List<int> listFolId)
        {
            return this.EDMsDataContext.Folders.ToArray().Where(t => t.ParentID == parentId && listFolId.Contains(t.ID)).ToList();
        }
    }
}
