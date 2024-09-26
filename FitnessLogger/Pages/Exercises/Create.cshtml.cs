using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FitnessLogger.Pages.Exercises
{
    public class CreateModel : PageModel
    {
        public ExerciseInfo exerciseInfo = new ExerciseInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() 
        {
            exerciseInfo.name = Request.Form["name"];
			exerciseInfo.notes = Request.Form["notes"];

			if (exerciseInfo.name.Length == 0 || exerciseInfo.notes.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            //save the new client into the database
            try
            {
                string connectionString = "Data Source=DESKTOP-BFJFBE3;Initial Catalog=FitnessLog;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Exercise" + "(ExerciseName, Notes) VALUES " + "(@name, @notes);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", exerciseInfo.name);
						command.Parameters.AddWithValue("@notes", exerciseInfo.notes);

						command.ExecuteNonQuery();
                    }
                }
			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            exerciseInfo.name = "";
            exerciseInfo.notes = "";
            successMessage = "New Exercise Added Correctly";

            Response.Redirect("/Exercises/Index");
        }
    }
}
