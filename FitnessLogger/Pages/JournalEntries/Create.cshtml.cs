using FitnessLogger.Pages.Exercises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FitnessLogger.Pages.JournalEntries
{
    public class CreateModel : PageModel
    {
        public JournalEntryInfo journalEntryInfo = new JournalEntryInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
		}
		public void OnPost() 
        {

			journalEntryInfo.date = Request.Form["date"];
			journalEntryInfo.title = Request.Form["title"];
			journalEntryInfo.exerciseName = Request.Form["exerciseName"];
			journalEntryInfo.weight = Request.Form["weight"];
			journalEntryInfo.sets = Request.Form["sets"];
			journalEntryInfo.repetitions = Request.Form["repetitions"];
			journalEntryInfo.entryNotes = Request.Form["entryNotes"];

			if (journalEntryInfo.date.Length == 0 || journalEntryInfo.title.Length == 0
				|| journalEntryInfo.exerciseName.Length == 0 || journalEntryInfo.weight.Length == 0
				|| journalEntryInfo.sets.Length == 0 || journalEntryInfo.repetitions.Length == 0
				|| journalEntryInfo.entryNotes.Length == 0)
			{
				errorMessage = "All the fields are required";
				return;
			}

			try
			{
				string connectionString = "Data Source=DESKTOP-BFJFBE3;Initial Catalog=FitnessLog;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();

					// Check if Exercise already exists
					string sql = "Select * from Exercise";
					bool eCheck = false;
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								if (reader.GetString(1) == journalEntryInfo.exerciseName)
								{
									eCheck = true;
									journalEntryInfo.exerciseID = reader.GetInt32(0).ToString();
									break;
								}
							}
						}
					}

					// Check if Journal Entry already exists
					sql = "Select * from Journal_Entries";
					bool jCheck = false;
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								if (FormatDate(reader.GetDateTime(1).ToString()) == journalEntryInfo.date)
								{
									jCheck = true;
									journalEntryInfo.entryID = reader.GetInt32(0).ToString();
									break;
								}
							}
						}
					}

					// adds exercise to database if it doesn't exist and gathers ID
					if (!eCheck)
					{
						sql = "INSERT INTO Exercise" + "(ExerciseName) VALUES " + "(@exerciseName) " + "SET @exercise_ID = SCOPE_IDENTITY()";
						using (SqlCommand command = new SqlCommand(sql, connection))
						{
							command.Parameters.AddWithValue("@exerciseName", journalEntryInfo.exerciseName);
							command.Parameters.Add("@exercise_ID", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
							command.ExecuteNonQuery();
							journalEntryInfo.exerciseID = (command.Parameters["@exercise_ID"].Value).ToString();
						}
					}

					// adds journal entry to database if it doesn't exist and gathers ID
					if (!jCheck)
					{
						sql = "INSERT INTO Journal_Entries" + "(EntryDate, JournalID, Title) VALUES " + "(@date, 1, @title) " + "SET @entry_ID = SCOPE_IDENTITY()";
						using (SqlCommand command = new SqlCommand(sql, connection))
						{
							command.Parameters.AddWithValue("@date", journalEntryInfo.date);
							command.Parameters.AddWithValue("@title", journalEntryInfo.title);
							command.Parameters.Add("@entry_ID", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
							command.ExecuteNonQuery();
							journalEntryInfo.entryID = (command.Parameters["@entry_ID"].Value).ToString();
						}
					}

					// insert into Entry_Exercise
					sql = "INSERT INTO Entry_Exercise" + "(EntryID, ExerciseID, ExerciseWeight, ExerciseSets, ExerciseRepetitions, Notes) VALUES " +
						"(@entryID, @exerciseID, @weight, @sets, @repetitions, @notes);";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@entryID", journalEntryInfo.entryID);
						command.Parameters.AddWithValue("@exerciseID", journalEntryInfo.exerciseID);
						command.Parameters.AddWithValue("@weight", journalEntryInfo.weight);
						command.Parameters.AddWithValue("@sets", journalEntryInfo.sets);
						command.Parameters.AddWithValue("@repetitions", journalEntryInfo.repetitions);
						command.Parameters.AddWithValue("@notes", journalEntryInfo.entryNotes);

						command.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				errorMessage = ex.Message;
				return;
			}

			journalEntryInfo.date = "";
			journalEntryInfo.title = "";
			journalEntryInfo.exerciseName = "";
			journalEntryInfo.weight = "";
			journalEntryInfo.sets = "";
			journalEntryInfo.repetitions = "";
			journalEntryInfo.entryNotes = "";
			journalEntryInfo.entryID = "";
			journalEntryInfo.exerciseID = "";
			successMessage = "New Journal Entry Added Correctly!";

			Response.Redirect("/JournalEntries/Index");
		}

		public string FormatDate(string dateTime)
		{
			return dateTime.Substring(0, dateTime.IndexOf(" "));
		}
	}
}
