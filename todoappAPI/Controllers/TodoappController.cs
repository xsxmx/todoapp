using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace todoappAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoappController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TodoappController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetNotes")]
        public IActionResult GetNotes()
        {
            try
            {
                string query = "select * from dbo.Notes";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("todoappDBCon");
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        using (SqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            table.Load(myReader);
                        }
                    }
                    myCon.Close();
                }
                return Ok(table);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Błąd serwera: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("AddNotes")]
        public IActionResult AddNotes([FromBody] NoteModel note)
        {
            try
            {
                if (note == null || string.IsNullOrEmpty(note.Title) || string.IsNullOrEmpty(note.Description))
                {
                    return BadRequest("Nieprawidłowe dane wejściowe");
                }

                string query = "insert into dbo.Notes (title, description) values (@title, @description)";
                string sqlDataSource = _configuration.GetConnectionString("todoappDBCon");
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@title", note.Title);
                        myCommand.Parameters.AddWithValue("@description", note.Description);
                        myCommand.ExecuteNonQuery();
                    }
                    myCon.Close();
                }
                return Ok("Dodano pomyślnie");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Błąd serwera: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteNotes")]
        public IActionResult DeleteNotes(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Nieprawidłowe ID notatki");
                }

                string query = "delete from dbo.Notes where id=@id";
                string sqlDataSource = _configuration.GetConnectionString("todoappDBCon");
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@id", id);
                        myCommand.ExecuteNonQuery();
                    }
                    myCon.Close();
                }
                return Ok("Usunięto pomyślnie");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Błąd serwera: " + ex.Message);
            }
        }

        [HttpPut]
        [Route("EditNote/{id}")]
        public IActionResult EditNote(int id, [FromBody] NoteModel note)
        {
            try
            {
                if (id <= 0 || note == null || string.IsNullOrEmpty(note.Title) || string.IsNullOrEmpty(note.Description))
                {
                    return BadRequest("Nieprawidłowe dane wejściowe");
                }

                string query = "update dbo.Notes set title = @title, description = @description where id = @id";
                string sqlDataSource = _configuration.GetConnectionString("todoappDBCon");
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@id", id);
                        myCommand.Parameters.AddWithValue("@title", note.Title);
                        myCommand.Parameters.AddWithValue("@description", note.Description);
                        myCommand.ExecuteNonQuery();
                    }
                    myCon.Close();
                }
                return Ok("Zaktualizowano pomyślnie");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Błąd serwera: " + ex.Message);
            }
        }
    }

    public class NoteModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}