using Aspose.Cells;
using EAM.LDAWebAPICore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EAM.LDAWebAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EAMUserController : ControllerBase
    {

        private IConfiguration _configuration;
        public EAMUserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET api/<EAMUserController>/5
        [HttpGet]
        public UserInforDto Get(string username)
        {
            var userInforFileName = _configuration.GetValue<string>("MySettings:UserInforFileName");
            //Pass the file path and file name to the StreamReader constructor
            var sr = new StreamReader(userInforFileName);
            //Read the first line of text
            var line = sr.ReadLine();
            //Continue to read until you reach end of file
            while (line != null)
            {
                var lineSplit = line.Split(';');
                if (!string.IsNullOrEmpty(lineSplit[0]) && lineSplit[0] == username)
                {
                    sr.Close();
                    return new UserInforDto()
                    {
                        UserName = lineSplit[0],
                        IsDelete = Convert.ToBoolean(lineSplit[1]),
                    };
                }
                //Read the next line
                line = sr.ReadLine();
            }
            //close the file
            sr.Close();

            return null;
        }

        // POST api/<EAMUserController>
        [HttpPost]
        public void Post([FromBody] UserInforDto userObj)
        {
            var userInforFileName = _configuration.GetValue<string>("MySettings:UserInforFileName");
            var text = string.Empty;


            using (StreamReader reader = new StreamReader(userInforFileName))
            {
                text = reader.ReadToEnd();
                text = text.Replace(userObj.UserName + ";" + (!userObj.IsDelete).ToString().ToLower(), userObj.UserName + ";" + userObj.IsDelete.ToString().ToLower());
                reader.Close();
            }

            using (StreamWriter writer = new StreamWriter(userInforFileName))
            {
                writer.WriteLine(text);
                writer.Close();
            }
        }

        ////// PUT api/<EAMUserController>/5
        ////[HttpPut("{id}")]
        ////public void Put(int id, [FromBody] string value)
        ////{
        ////}

        ////// DELETE api/<EAMUserController>/5
        ////[HttpDelete("{id}")]
        ////public void Delete(int id)
        ////{
        ////}
    }
}
