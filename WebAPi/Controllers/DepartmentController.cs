using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using WebAPi.Models;

namespace WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select DepartmentId,DepartmentName from dbo.Department";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using(SqlConnection con=new SqlConnection(sqlDataSource))
            {
                con.Open();
                using (SqlCommand cmd=new SqlCommand(query,con)) {

                    myreader = cmd.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    con.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            string query = @"insert into dbo.Department values ('"+dep.DepartmentName+@"')";
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
        public JsonResult Update(Department dep)
        {
            string query = @"update dbo.Department set DepartmentName= '"+dep.DepartmentName+@"'
                  where DepartmentId= "+dep.DepartmentId + @"";
                  
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
            string query = @"delete  from dbo.Department 
                  where DepartmentId= " + id + @"";

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
    }
}
