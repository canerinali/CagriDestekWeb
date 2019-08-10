using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destek.Entities
{
    [Table("Brans")]
    public class Brans : MyEntityBase
    {
        [DisplayName("Branş"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter içermeli.")]
        public string Title { get; set; }

        [DisplayName("Açıklama"),
            StringLength(150, ErrorMessage = "{0} alanı max. {1} karakter içermeli.")]
        public string Description { get; set; }

        public bool IsDraft { get; set; }

        public virtual List<Message> Messages { get; set; }
        public virtual List<DestekUser> DestekUser { get; set; }
        
        public Brans()
        {
            Messages = new List<Message>();
            DestekUser = new List<DestekUser>();
        }
    }
}
