using Destek.BLL.Abstract;
using Destek.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destek.BLL
{
    public class BransManager : ManagerBase<Brans>
    {
        public override int Delete(Brans brans)
        {
            DestekUserManager destekUserManager = new DestekUserManager();
            MessageManager messageManager = new MessageManager();
            // Kategori ile ilişkili notların silinmesi gerekiyor.
            foreach (Message message in brans.Messages.ToList())
            {

                messageManager.Delete(message);
            }
            foreach (DestekUser destekUser in brans.DestekUser.ToList())
            {

                destekUserManager.Delete(destekUser);
            }
           
            return base.Delete(brans);
        }
    }
}
