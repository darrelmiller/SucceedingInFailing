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
        public static Collection<String> Notes { get; set; }

        static NotesService()
        {
            Notes = new Collection<string>
            {
                "Hello World", 
                "Dear Mother, Dear Father", 
                "Why so glum?",

            };
        }

        public static string GetNote(int id)
        {
            if (id < 0 || id > Notes.Count) throw new ArgumentException();

            return Notes[id];
        }

        public static string GetLastNote()
        {
            return Notes.LastOrDefault();
        }
        public static void AddNote(string newNote)
        {
            Notes.Add(newNote);
        }

        public static bool CheckNote(int id,string contains)
        {
            return Notes[id].Contains(contains);
        }

        public static void LowerNote(int id)
        {
            Notes[id] = Notes[id].ToLower();
        }

        public static void DeleteNote(int id)
        {
            Notes.Remove(Notes[id]);
        }
    }
}
