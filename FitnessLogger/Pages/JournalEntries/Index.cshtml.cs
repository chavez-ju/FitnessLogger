using FitnessLogger.Pages.Exercises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FitnessLogger.Pages.JournalEntries
{
    public class IndexModel : PageModel
    {
		public List<JournalEntryInfo> listEntries = new List<JournalEntryInfo>();
        public void OnGet()
        {
			try
			{
				String connectionString = "Data Source=DESKTOP-BFJFBE3;Initial Catalog=FitnessLog;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					string sql = "Select\r\nEntryDate,\r\nTitle,\r\nExerciseName,\r\nExerciseWeight,\r\nExerciseSets,\r\nExerciseRepetitions,\r\ndbo.Entry_Exercise.Notes\r\nFrom dbo.Journal_Entries\r\nJoin dbo.Entry_Exercise on dbo.Journal_Entries.EntryID = dbo.Entry_Exercise.EntryID\r\nJoin dbo.Exercise on dbo.Entry_Exercise.ExerciseID = dbo.Exercise.ExerciseID";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								JournalEntryInfo journalEntry = new JournalEntryInfo();
								journalEntry.date = FormatDate(reader.GetDateTime(0).ToString());
								journalEntry.title = reader.GetString(1);
								journalEntry.exerciseName = reader.GetString(2);
								journalEntry.weight = "" + reader.GetInt32(3);
								journalEntry.sets = "" + reader.GetInt32(4);
								journalEntry.repetitions = "" + reader.GetInt32(5);
								//doesnt change database null values, just shows n/a on web
								if (reader.IsDBNull(6))
								{
									journalEntry.entryNotes = "n/a";
								}
								else
								{
									journalEntry.entryNotes = reader.GetString(6);
								}

								listEntries.Add(journalEntry);
							}
						}
					}
				}
			}
			catch (Exception ex) 
			{
				Console.WriteLine("Exception: " + ex.ToString());
			}

        }

		public string FormatDate(string dateTime)
		{
			return dateTime.Substring(0, dateTime.IndexOf(" "));
		}
    }

	public class JournalEntryInfo
	{
		public string date;
		public string title;
		public string exerciseName;
		public string weight;
		public string sets;
		public string repetitions;
		public string entryNotes;
	}
}
