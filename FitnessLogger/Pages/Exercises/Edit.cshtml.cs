using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FitnessLogger.Pages.Exercises
{
    public class EditModel : PageModel
    {
        public ExerciseInfo exerciseInfo = new ExerciseInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
            string id = Request.Query["id"];

            try
            {
                string connectionString = "Data Source=DESKTOP-BFJFBE3;Initial Catalog=FitnessLog;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Exercise WHERE ExerciseID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                exerciseInfo.id = "" + reader.GetInt32(0);
                                exerciseInfo.name = reader.GetString(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        public void OnPost() 
        {
            exerciseInfo.id = Request.Form["id"];
			exerciseInfo.name = Request.Form["name"];

            if (exerciseInfo.id.Length == 0 || exerciseInfo.name.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                string connectionString = "Data Source=DESKTOP-BFJFBE3;Initial Catalog=FitnessLog;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Exercise " + "SET ExerciseName=@name " + "WHERE ExerciseID=@id";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", exerciseInfo.name);
                        command.Parameters.AddWithValue("@id", exerciseInfo.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("/Exercises/Index");
		}
    }
}
