using FitnessLogger.Pages.Exercises;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FitnessLogger.Pages.ExerciseEntry
{
    public class IndexModel : PageModel
    {
        public List<EntryInfo> listEntries = new List<EntryInfo>();
        public void OnGet()
        {
            try
            {
				string connectionString = "Data Source=DESKTOP-BFJFBE3;Initial Catalog=FitnessLog;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
					string sql = "SELECT * FROM Entry_Exercise";
					using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EntryInfo info = new EntryInfo();
                                info.entryID = "" + reader.GetInt32(0);
                                info.exerciseID = "" + reader.GetInt32(1);
                                info.weight = "" + reader.GetInt32(2);
								info.sets = "" + reader.GetInt32(3);
                                info.repetitions = "" + reader.GetInt32(4);

								//doesnt change database null values, just shows n/a on web
								if (reader.IsDBNull(5))
								{
									info.notes = "n/a";
								}
								else
								{
									info.notes = reader.GetString(2);
								}

                                listEntries.Add(info);
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
    }

    public class EntryInfo()
    {
        public string entryID;
        public string exerciseID;
        public string weight;
        public string sets;
        public string repetitions;
        public string notes;
    }
}
