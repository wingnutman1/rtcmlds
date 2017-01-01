using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS_Web.Models
{
    public class SystemStatus
    {
        [Key]
        public int ID { get; set; }
        public bool incidentEngineEnabled { get; set; }
        public string incidentEngineStatus { get; set; }
        public bool todoEngineEnabled { get; set; }
        public string todoEngineStatus { get; set; }
        public bool exceptionEngineEnabled { get; set; }
        public string exceptionEngineStatus { get; set; }

    }
}