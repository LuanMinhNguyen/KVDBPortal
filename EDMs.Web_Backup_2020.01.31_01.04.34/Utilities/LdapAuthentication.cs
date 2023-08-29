using System;
using System.DirectoryServices;
using System.Text;
using System.Web;

namespace EDMs.Web.Utilities
{
    public class LdapAuthentication
    {
        public string Path
        {
            get { return _path; }
        }

        public string FilterAttribute
        {
            get { return _filterAttribute; }
        }

        private string _path;
        private string _filterAttribute;

        public LdapAuthentication(string path)
        {
            _path = path;
        }

        public DirectoryEntry GetUser(string domain, string UserName, string password)
        {

            string domainAndUsername = domain + @"\" + UserName;
            DirectoryEntry de = GetDirectoryObject(domainAndUsername, password);
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;
            deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + UserName + "))";
            deSearch.SearchScope = SearchScope.Subtree;
            try
            {
                object obj = de.NativeObject;
                SearchResult results = deSearch.FindOne();
                if (!(results == null))
                {
                    de = new DirectoryEntry(results.Path, domainAndUsername, password, AuthenticationTypes.Secure);
                    return de;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "The user name or password is incorrect." + Environment.NewLine)
                {
                    return null;
                }
            }
            return null;
        }

        private DirectoryEntry GetDirectoryObject( string UserName, string password)
        {
            DirectoryEntry oDE;
            oDE = new DirectoryEntry(_path, UserName, password, AuthenticationTypes.Secure);
            return oDE;
        }
        //public bool IsAuthenticated(string domain, string username, string pwd)
        //{
        //    string domainAndUsername = domain + @"\" + username;

        //     DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);
        //    try
        //    {
        //            entry.AuthenticationType = AuthenticationTypes.SecureSocketsLayer;

        //            //Bind to the native AdsObject to force authentication.
        //         //  object obj = entry.NativeObject;

        //            DirectorySearcher search = new DirectorySearcher(entry);

        //            search.Filter = "(SAMAccountName=" + username + ")";
        //            search.PropertiesToLoad.Add("cn");
        //            SearchResult result = search.FindOne();

        //            if (null == result)
        //            {
        //                return false;
        //            }

        //            //Update the new path to the user in the directory.
        //            _path = result.Path;
        //            _filterAttribute = (string)result.Properties["cn"][0];
        //        }
        //        catch (Exception ex)
        //        {
        //            //throw new Exception("Error authenticating user. " + ex.Message);

        //            return false;
        //        }

        //        return true;

        //}
   
        public string GetGroups()
        {
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();
                int propertyCount = result.Properties["memberOf"].Count;
                string dn;
                int equalsIndex, commaIndex;

                for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                {
                    dn = (string)result.Properties["memberOf"][propertyCounter];
                    equalsIndex = dn.IndexOf("=", 1);
                    commaIndex = dn.IndexOf(",", 1);
                    if (-1 == equalsIndex)
                    {
                        return null;
                    }
                    groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    groupNames.Append("|");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToString();
        }
    }
}