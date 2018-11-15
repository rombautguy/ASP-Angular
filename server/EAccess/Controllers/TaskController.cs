using System;
using System.Web.Http;
using System.Configuration;
using System.Data.SqlClient;
using EAccess.Models;
using System.Data;
using System.Collections.Generic;

namespace EAccess.Controllers
{
    //[Authorize]
    [RoutePrefix("api/Task")]
    public class TaskController : ApiController
    {
        private string DBConnectionString = ConfigurationManager.ConnectionStrings["GraduwayConnectionString"].ConnectionString;

        public TaskController()
        {
        }

        // GET api/Task/GetTasks
        [Route("GetTasks")]
        public IHttpActionResult GetTasks(int userid)
        {
            List<UserTask> tasks = new List<UserTask>();

            try
            {
                SqlConnection myConn = new SqlConnection(DBConnectionString);
                myConn.Open();
                string cmdText = "SELECT * FROM Tasks WHERE userid = " + userid;
                SqlCommand myCmd = new SqlCommand(cmdText, myConn);
                SqlDataReader taskReader = myCmd.ExecuteReader();

                if (taskReader.HasRows)
                {
                    while (taskReader.Read())
                    {
                        UserTask task = new UserTask();
                        task.id = Convert.ToInt32(taskReader["id"]);
                        task.userid = Convert.ToInt32(taskReader["userid"]);
                        task.title = taskReader["title"].ToString().Trim();
                        task.description = taskReader["description"].ToString().Trim();
                        task.priority = Convert.ToInt32(taskReader["priority"]);
                        task.state = Convert.ToInt32(taskReader["state"]);
                        task.estimate = Convert.ToInt32(taskReader["estimate"]);

                        tasks.Add(task);
                    }
                }

                taskReader.Close();
                myConn.Close();

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST api/Task/CreateTask
        [Route("CreateTask")]
        public IHttpActionResult CreateTask(UserTask task)
        {
            SqlConnection myConnection = new SqlConnection(DBConnectionString);
            SqlCommand myCommand = new SqlCommand("INSERT INTO Tasks (title, description, priority, state, estimate, userid) SELECT @title, @description, @priority, @state, @estimate, @userid", myConnection);

            SqlParameter useridParam = myCommand.Parameters.Add("@userid", SqlDbType.Int);
            SqlParameter titleParam = myCommand.Parameters.Add("@title", SqlDbType.VarChar, 50);
            SqlParameter descriptionParam = myCommand.Parameters.Add("@description", SqlDbType.VarChar, 50);
            SqlParameter priorityParam = myCommand.Parameters.Add("@priority", SqlDbType.Int);
            SqlParameter stateParam = myCommand.Parameters.Add("@state", SqlDbType.Int);
            SqlParameter estimateParam = myCommand.Parameters.Add("@estimate", SqlDbType.Int);

            useridParam.Value = task.userid;
            titleParam.Value = task.title.Trim();
            descriptionParam.Value = task.description.Trim();
            priorityParam.Value = task.priority;
            stateParam.Value = task.state;
            estimateParam.Value = task.estimate;

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();
                int id = (int)myCommand.ExecuteScalar();

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

        // POST api/Task/UpdateTask
        [Route("UpdateTask")]
        public IHttpActionResult UpdateTask(UserTask task)
        {
            SqlConnection myConnection = new SqlConnection(DBConnectionString);
            string sql = "UPDATE Tasks SET title = @title, description = @description, priority = @priority, state = @state, estimate = @estimate WHERE id = @id AND userid = @userid";
            SqlCommand myCommand = new SqlCommand(sql, myConnection);

            SqlParameter idParam = myCommand.Parameters.Add("@id", SqlDbType.Int);
            SqlParameter useridParam = myCommand.Parameters.Add("@userid", SqlDbType.Int);
            SqlParameter titleParam = myCommand.Parameters.Add("@title", SqlDbType.VarChar, 50);
            SqlParameter descriptionParam = myCommand.Parameters.Add("@description", SqlDbType.VarChar, 50);
            SqlParameter priorityParam = myCommand.Parameters.Add("@priority", SqlDbType.Int);
            SqlParameter stateParam = myCommand.Parameters.Add("@state", SqlDbType.Int);
            SqlParameter estimateParam = myCommand.Parameters.Add("@estimate", SqlDbType.Int);

            idParam.Value = task.id;
            useridParam.Value = task.userid;
            titleParam.Value = task.title.Trim();
            descriptionParam.Value = task.description.Trim();
            priorityParam.Value = task.priority;
            stateParam.Value = task.state;
            estimateParam.Value = task.estimate;

            try
            {
                myConnection.Open();
                myCommand.ExecuteNonQuery();

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

        //// GET api/Task/DeleteTask
        //[Route("DeleteTask")]
        //[HttpGet]
        //public IHttpActionResult DeleteTask(string id)
        //{
        //    SqlConnection myConnection = new SqlConnection(DBConnectionString);
        //    string sql = "DELETE FROM Tasks WHERE id = @id ";
        //    SqlCommand myCmd = new SqlCommand(sql, myConnection);
        //    // Define Input Parameters
        //    SqlParameter TaskIdParam = myCmd.Parameters.Add("@id", SqlDbType.Int);
        //    TaskIdParam.Value = id;

        //    try
        //    {
        //        myConnection.Open();
        //        myCmd.ExecuteNonQuery();

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //    finally
        //    {
        //        myConnection.Close();
        //    }
        //}

    }
}
