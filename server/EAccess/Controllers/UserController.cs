using System;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using EAccess.Models;
using System.Collections.Generic;
using System.Data;

namespace EAccess.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private string DBConnectionString = ConfigurationManager.ConnectionStrings["GraduwayConnectionString"].ConnectionString;
        public UserController()
        {
        }
        // GET api/User/GetUsers
        [Route("GetUsers")]
        public IHttpActionResult GetUsers()
        {
            List<User> users = new List<User>();

            try
            {
                SqlConnection myConn = new SqlConnection(DBConnectionString);
                myConn.Open();
                string cmdText = "SELECT * FROM Users";
                SqlCommand myCmd = new SqlCommand(cmdText, myConn);
                SqlDataReader userReader = myCmd.ExecuteReader();

                if (userReader.HasRows)
                {
                    while (userReader.Read())
                    {
                        User user = new User();
                        user.id = Convert.ToInt32(userReader["id"]);
                        user.name = userReader["name"].ToString().Trim();

                        users.Add(user);
                    }
                }

                userReader.Close();
                myConn.Close();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST api/User/CreateUser
        [Route("CreateUser")]
        public IHttpActionResult CreateUser(User user)
        {
            SqlConnection myConnection = new SqlConnection(DBConnectionString);
            SqlCommand myCommand = new SqlCommand("INSERT INTO Users (name) SELECT @Username", myConnection);
            SqlParameter UsernameParam = myCommand.Parameters.Add("@Username", SqlDbType.VarChar, 50);
            UsernameParam.Value = user.name.Trim();

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                myCommand.CommandText = "Select @@Identity";
                object obj = myCommand.ExecuteScalar();
                int id = Convert.ToInt32(myCommand.ExecuteScalar());

                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            finally
            {
                myConnection.Close();
            }
        }

        // POST api/User/UpdateUser
        [Route("UpdateUser")]
        public IHttpActionResult UpdateUser(User user)
        {
            SqlConnection myConnection = new SqlConnection(DBConnectionString);
            string sql = "UPDATE Users SET name = @name WHERE id = @id ";
            SqlCommand myCmd = new SqlCommand(sql, myConnection);
            // Define Input Parameters
            SqlParameter UsernameParam = myCmd.Parameters.Add("@name", SqlDbType.Char, 15);
            SqlParameter UserIdParam = myCmd.Parameters.Add("@id", SqlDbType.Int);
            UsernameParam.Value = user.name;
            UserIdParam.Value = user.id;

            try
            {
                myConnection.Open();
                myCmd.ExecuteNonQuery();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            finally
            {
                myConnection.Close();
            }
        }

        // GET api/User/DeleteUser
        [Route("DeleteUser")]
        [HttpGet]
        public IHttpActionResult DeleteUser(string id)
        {
            SqlConnection myConnection = new SqlConnection(DBConnectionString);
            string sql = "DELETE FROM Users WHERE id = @id ";
            SqlCommand myCmd = new SqlCommand(sql, myConnection);
            // Define Input Parameters
            SqlParameter UserIdParam = myCmd.Parameters.Add("@id", SqlDbType.Int);
            UserIdParam.Value = id;

            try
            {
                myConnection.Open();
                myCmd.ExecuteNonQuery();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            finally
            {
                myConnection.Close();
            }
        }

    }
}
