using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FailingHTTPApi
{
    public static class NotesService
    {
        private static int _LastId = 0;
        private static Dictionary<int,String> _Notes ;

        static NotesService()
        {
            _Notes = new Dictionary<int,string>();

            AddNote("Hello World"); 
            AddNote("Dear Mother, Dear Father"); 
            AddNote("Why so glum?");
            AddNote("*Javascript makes me sad?");
            
        }

        public static IEnumerable<string> Notes { get { return _Notes.Values; } }
        public static string GetNote(int id)
        {
            if (!_Notes.ContainsKey(id)) throw new ArgumentException();

            return _Notes[id];
        }

        public static IEnumerable<string> SearchNotes(string query)
        {
            return _Notes.Values.Where(n => n.Contains(query) && !n.StartsWith("*"));
        }
        public static IEnumerable<string> SecretNotes()
        {
            return _Notes.Values.Where(n => n.StartsWith("*"));
        }
        public static string GetLastNote()
        {
            return _Notes.Values.LastOrDefault();
        }
        public static void AddNote(string newNote)
        {
            _Notes[_LastId++] = newNote;
        }

        public static bool CheckNote(int id,string contains)
        {
            return _Notes[id].Contains(contains);
        }

        public static void LowerNote(int id)
        {
            _Notes[id] = _Notes[id].ToLower();
        }

        public static void DeleteNote(int id)
        {
            _Notes.Remove(id);
        }

        public static bool NoteExists(int id)
        {
            return _Notes.ContainsKey(id);
        }

        public static void SetNotes(int id, string note)
        {
            _Notes[id] = note;
        }
    }
}
