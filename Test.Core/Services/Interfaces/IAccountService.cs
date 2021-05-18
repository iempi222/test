using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Test.Core.ViewModels.Account;
using Test.Domain.Models.Account;
using Test.Domain.Models.ViewModels.Accounts;
using MainStaffViewModel = Test.Core.ViewModels.Account.MainStaffViewModel;
using StaffLoginViewModel = Test.Core.ViewModels.Account.StaffLoginViewModel;

namespace Test.Core.Services.Interfaces
{
    public interface IAccountService
    {

        #region Users

        Task<UserViewModels.UserPagingViewModel> GetAllUsers(UserViewModels.UserPagingViewModel usersPaging,
            int currentPage, int take = 10);
        Task AddUser(User user);
        Task<MainUserViewModel> GetUserByUserId(Guid userId);
        Task<bool> EditUser(MainUserViewModel user, string ipAddress, Guid whoEdited, IFormFile avatarUp, string oldPhoneNumber);
        IEnumerable<MainUserViewModel> GetAllUsers();
        Task DeleteUser(Guid userId, Guid whoDone, string ipAddress);
        Task UnDeleteUser(Guid userId, Guid whoDone, string ipAddress);
        Task ActiveUser(Guid userId, Guid whoDone, string ipAddress);
        Task DeActiveUser(Guid userId, Guid whoDone, string ipAddress);

        #endregion


        #region Staff

        Task<RegistrationResult> RegisterNewStaff(StaffRegistrationViewModel newStaffInformation,
            IFormFile nationalCardImage, IFormFile personnelImage,Guid registrantId, string registrantIpAddress);

        Task<LoginResult> StaffLogIn(StaffLoginViewModel staffLogin, string ipAddress);
        Task<StaffPagingViewModel> GetAllStaff(StaffPagingViewModel staffPaging, int currentPage, int take = 10);
        Task<MainStaffViewModel> GetStaffById(Guid staffId);
        Task LogOutEvent(Guid staffId);
        Task<PersonnelDetailViewModel> GetPersonnelDetail(Guid staffId);
        Task DeleteStaff(Guid staffId, Guid whoDone, string ipAddress);
        Task UnDeleteStaff(Guid staffId, Guid whoDone, string ipAddress);
        Task ActiveStaff(Guid staffId, Guid whoDone, string ipAddress);
        Task DeActiveStaff(Guid staffId, Guid whoDone, string ipAddress);

        #endregion


        #region User Registration and Login

        Task<bool> IsUserPhoneNumberExists(string phoneNumber);

        Task<bool> RegisterUser(RegisterViewModel register, string ipAddress);

        Task<UserLoginResultViewModel> UserLogIn(UserLoginViewModel userLogin, string ipAddress);

        #endregion
    }
}
