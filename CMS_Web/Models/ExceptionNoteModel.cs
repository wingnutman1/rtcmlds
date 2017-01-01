using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS_Web.Models
{
    public class ExceptionNote
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int CreationStaffID { get; set; }
        public int LastEditStaffID { get; set; }
        public DateTime NoteDate { get; set; }
        public DateTime EditDate { get; set; }
        public bool Deleted { get; set; }
        public String NoteText { get; set; }

        public ExceptionNote()
        {
            Deleted = false;
        }
    }
}