using Destek.BLL.Abstract;
using Destek.BLL.Results;
using Destek.Entities;
using Destek.Entities.ErrorMessages;
using Destek.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Destek.BLL
{
    public class DestekUserManager : ManagerBase<DestekUser>
    {
        public BusinessLayerResult<DestekUser> GetUserById(int id)
        {
            BusinessLayerResult<DestekUser> res = new BusinessLayerResult<DestekUser>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }

            return res;
        }
        public BusinessLayerResult<DestekUser> LoginUser(LoginViewModel data)
        {
            // Giriş kontrolü
            // Hesap aktive edilmiş mi?
            BusinessLayerResult<DestekUser> res = new BusinessLayerResult<DestekUser>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktif değil.");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı yada şifre uyuşmuyor.");
            }

            return res;
        }
        public BusinessLayerResult<DestekUser> UpdateProfile(DestekUser data)
        {
            DestekUser db_user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<DestekUser> res = new BusinessLayerResult<DestekUser>();

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.ModifiedName = data.ModifiedName.ToString();

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }

            return res;
        }
        public BusinessLayerResult<DestekUser> RemoveUserById(int id)
        {
            BusinessLayerResult<DestekUser> res = new BusinessLayerResult<DestekUser>();
            DestekUser destekUser = Find(x => x.Id == id);
            MessageManager messageManager = new MessageManager();
            if (destekUser != null)
            {
                if (Delete(destekUser) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }
        public new BusinessLayerResult<DestekUser> Insert(DestekUser data)
        {
            DestekUser user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<DestekUser> res = new BusinessLayerResult<DestekUser>();

            res.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
            }
            else
            {
                res.Result.ProfileImageFilename = "user_1.png";

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }

            return res;
        }
        public new BusinessLayerResult<DestekUser> Update(DestekUser data)
        {
            DestekUser db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<DestekUser> res = new BusinessLayerResult<DestekUser>();
            res.Result = data;

            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsAdmin = data.IsAdmin;
            res.Result.IsActive = data.IsActive;
            res.Result.BransId = data.BransId;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }
        public BusinessLayerResult<DestekUser> DeleteUser(DestekUser destekUser)
        {
            BusinessLayerResult<DestekUser> res = new BusinessLayerResult<DestekUser>();
            DestekUserManager destekUserManager = new DestekUserManager();

            if (destekUser != null)
            {
                if (Delete(destekUser) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi.");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }

            return res;
        }
    }
}
