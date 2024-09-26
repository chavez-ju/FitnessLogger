using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessLogger.Pages.JournalEntries
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }

	public class JournalEntryInfo
	{
		public string entryID;
		public string date;
		public string journalID;
		public string title;
	}

	public class EntryExerciseInfo
	{
		public string entryID;
		public string exerciseID;
		public string weight;
		public string sets;
		public string repetitions;
		public string notes;
	}
	public class ExerciseInfo
	{
		public string exerciseID;
		public string name;
		public string notes;
	}
}
