using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace FitnessLogger.Pages.Exercises
{
    public class IndexModel : PageModel
    {
        public List<ExerciseInfo> listExercises = new List<ExerciseInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=DESKTOP-BFJFBE3;Initial Catalog=FitnessLog;Integrated Security=True;Encrypt=True;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Exercise";
                    using (SqlCommand command = new SqlCommand(sql, connection)) 
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ExerciseInfo exerciseInfo = new ExerciseInfo();
                                exerciseInfo.id = "" + reader.GetInt32(0);
                                exerciseInfo.name = reader.GetString(1);

                                //doesnt change database null values, just shows n/a on web
                                if (reader.IsDBNull(2))
                                {
                                    exerciseInfo.notes = "n/a";
                                }
                                else
                                {
                                    exerciseInfo.notes = reader.GetString(2);
                                }

                                listExercises.Add(exerciseInfo);
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

    public class ExerciseInfo
    {
        public String id;
        public String name;
        public String notes;
    }
}
