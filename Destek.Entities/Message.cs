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
    [Table("Messages")]
    public class Message : MyEntityBase
    {
        [DisplayName("Talep Başlığı"), Required, StringLength(60)]
        public string Title { get; set; }
        [DisplayName("Talep Metni"), Required, StringLength(2000)]
        public string Text { get; set; }
        [StringLength(30), ScaffoldColumn(false)]
        public string PostImageFilename { get; set; }
        [DisplayName("Mesaj Durum")]
        public string MesajDurum { get; set; }

        public bool IsActive { get; set; }
        public int BransId { get; set; }

        public virtual DestekUser Owner { get; set; }
        public virtual Brans Brans { get; set; }
    }
}
