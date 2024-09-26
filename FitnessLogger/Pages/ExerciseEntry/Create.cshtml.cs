using FitnessLogger.Pages.Exercises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FitnessLogger.Pages.ExerciseEntry
{
    public class CreateModel : PageModel
    {
		public EntryInfo entryInfo = new EntryInfo();
		public string errorMessage = "";
		public string successMessage = "";
		public void OnGet()
        {
        }
		public void OnPost()
		{
			entryInfo.entryID = Request.Form["entryID"];
			entryInfo.exerciseID = Request.Form["exerciseID"];
			entryInfo.weight = Request.Form["weight"];
			entryInfo.sets = Request.Form["sets"];
			entryInfo.repetitions = Request.Form["repetitions"];
			entryInfo.notes = Request.Form["notes"];

			if (entryInfo.entryID.Length == 0 || entryInfo.exerciseID.Length == 0 ||
				entryInfo.weight.Length == 0 || entryInfo.sets.Length == 0 ||
				entryInfo.repetitions.Length == 0 || entryInfo.notes.Length == 0)
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
					string sql = "INSERT INTO Entry_Exercise" + "(EntryID, ExerciseID, ExerciseWeight, ExerciseSets, ExerciseRepetitions, Notes) VALUES " + 
						"(@entryID, @exerciseID, @weight, @sets, @repetitions, @notes);";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@entryID", entryInfo.entryID);
						command.Parameters.AddWithValue("@exerciseID", entryInfo.exerciseID);
						command.Parameters.AddWithValue("@weight", entryInfo.weight);
						command.Parameters.AddWithValue("@sets", entryInfo.sets);
						command.Parameters.AddWithValue("@repetitions", entryInfo.repetitions);
						command.Parameters.AddWithValue("@notes", entryInfo.notes);

						command.ExecuteNonQuery();
					}
				}
			}

			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			entryInfo.entryID = "";
			entryInfo.exerciseID = "";
			entryInfo.weight = "";
			entryInfo.sets = "";
			entryInfo.repetitions = "";
			entryInfo.notes = "";
			successMessage = "New Exercise Entry Added Correctly";

			Response.Redirect("/ExerciseEntry/Index");
		}
    }
}
