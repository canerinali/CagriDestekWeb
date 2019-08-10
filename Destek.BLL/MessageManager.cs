using Destek.BLL.Abstract;
using Destek.BLL.Results;
using Destek.Entities;
using Destek.Entities.ErrorMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destek.BLL
{
    public class MessageManager : ManagerBase<Message>
    {
        public BusinessLayerResult<Message> InsertPostFoto(Message message)
        {
            BusinessLayerResult<Message> res = new BusinessLayerResult<Message>();


            if (string.IsNullOrEmpty(message.PostImageFilename) == false)
            {
                res.Result.PostImageFilename = message.PostImageFilename;
            }

            if (base.Insert(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Post Eklenmedi.");
            }

            return res;
        }
        public BusinessLayerResult<Message> UpdatePostFoto(Message message)
        {
            BusinessLayerResult<Message> res = new BusinessLayerResult<Message>();
            res.Result = Find(x => x.Id == message.Id);
            res.Result.IsActive = message.IsActive;
            res.Result.BransId = message.BransId;
            res.Result.Text = message.Text;
            res.Result.Title = message.Title;
            res.Result.MesajDurum = message.MesajDurum;

            if (string.IsNullOrEmpty(message.PostImageFilename) == false)
            {
                res.Result.PostImageFilename = message.PostImageFilename;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Post Eklenmedi.");
            }

            return res;
        }
    }
}
