using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAPi.Models;

namespace WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env; 
        public EmployeeController(IConfiguration configuration,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select * from dbo.Employee";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    myreader = cmd.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Employee emp)
        {
            string query = @"insert into dbo.Employee values ('" + emp.EmployeeName + @"','" + emp.Department + @"','" + emp.DateOfJoining + @"','" + emp.PhotoFileName + @"')";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    myreader = cmd.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    con.Close();
                }
            }
            return new JsonResult("Added Succesfully");

        }



        [HttpPut]
        public JsonResult Update(Employee emp)
        {
            string query = @"update dbo.Employee set EmployeeName= '" + emp.EmployeeName + @"',Department='" + emp.Department + @"',
             DateOfJoining='" + emp.DateOfJoining + @"'     where EmployeeId= " + emp.EmployeeId + @"";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    myreader = cmd.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    con.Close();
                }
            }
            return new JsonResult("Updated Succesfully");

        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete  from dbo.Employee 
                  where EmployeeId= " + id + @"";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    myreader = cmd.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    con.Close();
                }
            }
            return new JsonResult("Deleted Succesfully");

        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var pyhsicalPath = _env.ContentRootPath + "/Photos/" + fileName;

                using(var stream=new FileStream(pyhsicalPath,FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            string query = @"select DepartmentName from dbo.Department";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using (SqlConnection con = new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(query, con))
                {

                    myreader = cmd.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);

        }
    }
}
