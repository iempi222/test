using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Core.Extensions.Convertors;
using Test.Core.Security;
using Test.Core.Services.Interfaces;
using Test.Core.ViewModels.Account;
using Test.Domain.Interfaces;
using Test.Domain.Models.Account;
using ThemeShop.Core.Convertors;


namespace Test.Core.Services.Implementation
{
    public class AccountServices : IAccountServices
    {
        #region Dependancy Injection

        private readonly IAccountRepository _accountRepository;

        public AccountServices(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        #endregion


        #region Users

        public async Task ActiveUserById(Guid userId)
        {
            await _accountRepository.ActiveUserById(userId);
            await _accountRepository.SaveAsync();
        }

        public async Task AddUser(User user)
        {
            await _accountRepository.AddUser(user);
            await _accountRepository.SaveAsync();
        }

        public async Task DeActiveUserById(Guid userId)
        {
            await _accountRepository.DeActiveUserById(userId);
            await _accountRepository.SaveAsync();
        }

        public async Task DeleteUserById(Guid userId)
        {
            await _accountRepository.DeleteUserById(userId);
            await _accountRepository.SaveAsync();
        }

        public IEnumerable<MainUserViewModel> GetAllUsers()
        {
            var allUsers = _accountRepository.GetAllUsers();

            IEnumerable<MainUserViewModel> users = allUsers.Select(u => new MainUserViewModel()
            {
                IsAccountActive = u.IsAccountActive,
                IsDelete = u.IsDelete,
                Avatar = u.Avatar,
                IpAddress = u.IpAddress,
                IsPhoneNumberActive = u.IsPhoneNumberActive,
                LocationId = u.LocationId,
                Password = u.Password,
                PhoneNumber = u.PhoneNumber,
                RegistrationDate = u.RegistrationDate,
                UserId = u.UserId,
                UserName = u.UserName
            });

            return users;
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _accountRepository.GetUserById(userId);
        }

        public async Task UnDeleteUserById(Guid userId)
        {
            var user = await GetUserById(userId);
            user.IsDelete = false;
        }

        #endregion



        #region Staff

        public async Task<RegistrationResult> RegisterNewStaff(StaffRegistrationViewModel newStaffInformation, IFormFile nationalCardImage, IFormFile personnelImage, Guid registrantId, string registrantIpAddress)
        {

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(),
                $"wwwroot/admin/personnel_contents/{newStaffInformation.Name}_{newStaffInformation.Family}_{newStaffInformation.NationalCode}");


            //Check If BirthCertificate Exists
            if (await _accountRepository.IsBirthCertificateNumberExists(newStaffInformation.BirthCertificateNumber))
                return new RegistrationResult
                {
                    RegistrationResultEnum = RegistrationResultEnum.Error,
                    RegistrationResultDescriptionEnum = RegistrationResultDescriptionEnum.BirthCertificateNumberExists
                };

            //Check If NationalCode Exists
            if (await _accountRepository.IsNationalCodeExists(newStaffInformation.NationalCode))
                return new RegistrationResult
                {
                    RegistrationResultEnum = RegistrationResultEnum.Error,
                    RegistrationResultDescriptionEnum = RegistrationResultDescriptionEnum.NationalCodeExists
                };

            //Upload National Card Image
            #region Upload National Card Image

            if (nationalCardImage.IsImage())
            {
                newStaffInformation.NationalCardImageName = ImageConvertor.ImageNameGenerator("NationalCard") + Path.GetExtension(nationalCardImage.FileName);

                var saveImagePath = directoryPath + "/" + newStaffInformation.NationalCardImageName;

                Directory.CreateDirectory(directoryPath);

                await using (var stream = new FileStream(saveImagePath, FileMode.Create))
                {
                    await nationalCardImage.CopyToAsync(stream);
                }
            }
            else
            {
                return new RegistrationResult
                {
                    RegistrationResultEnum = RegistrationResultEnum.Error,
                    RegistrationResultDescriptionEnum = RegistrationResultDescriptionEnum.NationalCardImageInvalid
                };
            }

            #endregion


            //Upload Personnel Image
            #region Uoload Personnel Image

            if (nationalCardImage.IsImage())
            {
                newStaffInformation.ImageName = ImageConvertor.ImageNameGenerator("Personnel") + Path.GetExtension(nationalCardImage.FileName);

                var saveImagePath = directoryPath + "/" + newStaffInformation.ImageName;

                Directory.CreateDirectory(directoryPath);

                await using (var stream = new FileStream(saveImagePath, FileMode.Create))
                {
                    await nationalCardImage.CopyToAsync(stream);
                }
            }
            else
            {
                return new RegistrationResult
                {
                    RegistrationResultEnum = RegistrationResultEnum.Error,
                    RegistrationResultDescriptionEnum = RegistrationResultDescriptionEnum.PersonImageInvalid
                };
            }

            #endregion


            Staff newStaff = new Staff()
            {
                PhoneNumber = newStaffInformation.PhoneNumber,
                Password = PasswordHelper.EncodePasswordMd5(newStaffInformation.Password),
                Name = newStaffInformation.Name,
                Family = newStaffInformation.Family,
                NationalCardImageName = newStaffInformation.NationalCardImageName,
                ImageName = newStaffInformation.ImageName,
                RegistrationDate = DateTime.Now,
                Address = newStaffInformation.Address,
                BirthCertificateNumber = newStaffInformation.BirthCertificateNumber,
                MobileNumber = newStaffInformation.PhoneNumber,
                NationalCode = newStaffInformation.NationalCode,
                PersonnelCode = DateTime.Now.Ticks.ToString(),
                Registrant = registrantId,
                RegistrantIpAddress = registrantIpAddress,
                EmailAddress = newStaffInformation.EmailAddress.FixedEmail(),
                Gender = (GenderEnum)newStaffInformation.Gender
            };

            await _accountRepository.AddStaff(newStaff);
            await _accountRepository.SaveAsync();

            return new RegistrationResult()
            {
                RegistrationResultEnum = RegistrationResultEnum.Success,
                RegistrationResultDescriptionEnum = RegistrationResultDescriptionEnum.Registered
            };

        }

        #endregion



        #region User Register

        public async Task<bool> IsPhoneNumberExists(string phoneNumber)
        {
            return await _accountRepository.IsPhoneNumberExists(phoneNumber);
        }

        public async Task<bool> RegisterUser(RegisterViewModel register, string ipAddress)
        {
            if (await IsPhoneNumberExists(register.PhoneNumber))
                return false;

            User newUser = new User()
            {
                IsAccountActive = true,
                IsDelete = false,
                PhoneNumber = register.PhoneNumber,
                Avatar = "Default",
                IpAddress = ipAddress,
                IsPhoneNumberActive = false,
                LocationId = 0,
                //Password = PasswordHelper.EncodePasswordMd5(register.Password),
                RegistrationDate = DateTime.Now,
                UserName = register.UserName
            };

            await _accountRepository.AddUser(newUser);
            await _accountRepository.SaveAsync();

            return true;
        }

        #endregion
    }
}
