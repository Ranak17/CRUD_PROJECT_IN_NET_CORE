using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }

        [StringLength(40)] //nvarchar(40)
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }
        [StringLength(10)]
        public string? Gender { get; set; }

        //uniqueidentifier
        public Guid? CountryID { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }
        //bit
        public bool? ReceiveNewsLetter { get; set; }
        [StringLength(40)]
        public string? TIN { get; set; }

        [ForeignKey(name:nameof(Country.CountryID))]
        public Country? Country { get; set; }
    }
}
