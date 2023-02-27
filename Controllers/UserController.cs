using AutoMapper;
using FastFill_API.Data;
using FastFill_API.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastFill_API.Web.Utils;
using FastFill_API.Web.Dto;
using FastFill_API.Web.Utils.Messages;
using System.Security.Claims;
using FastFill_API.Web;
using System.Text;
using System.IO;
using TelesignEnterprise;
using Telesign;
using System.Net;

namespace FastFill_API.Controllers
{
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserServices _userServices;
        private readonly SecurityServices _securityServices;
        private readonly CompanyServices _companyServices;

        private const long MaxFileSize = 30L * 1024L * 1024L; // 30MB, adjust to your need
        public UserController(IMapper mapper, UserServices userServices, SecurityServices securityServices, CompanyServices companyServices)
        {
            _mapper = mapper;
            _userServices = userServices;
            _securityServices = securityServices;
            _companyServices = companyServices;
        }


        // GET: api/user/users
        [HttpGet("users")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> users = await _userServices.GetUsers(page, pageSize, paginationInfo);
            IEnumerable<User> user = _mapper.Map<IEnumerable<User>>(users);

            var response = new
            {
                Users = user,
                PaginationInfo = paginationInfo
            };

            return Ok(response);
        }

        // GET: api/user/admins
        [HttpGet("admins")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdmins(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> users = await _userServices.GetAdmins(page, pageSize, paginationInfo);
            IEnumerable<User> user = _mapper.Map<IEnumerable<User>>(users);

            var response = new
            {
                Users = user,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }

        // GET: api/user/supports
        [HttpGet("supports")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSupports(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> users = await _userServices.GetSupports(page, pageSize, paginationInfo);
            IEnumerable<User> user = _mapper.Map<IEnumerable<User>>(users);

            var response = new
            {
                Users = user,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }

        // GET: api/user
        [HttpGet("{id}/{roleId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(int id, int? roleId)
        {
            if (id == 0)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }
            User user = await _userServices.GetUserById(id, roleId);

            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            return Ok(_mapper.Map<User>(user));
        }

        // GET: api/user/ByPhone
        [HttpGet("ByPhone/{mobileNumber}/{roleId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByMobileNumber (string mobileNumber, int? roleId)
        {
            if (mobileNumber == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (mobileNumber.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }
            User user = await _userServices.GetByMobileNumber(mobileNumber, roleId);

            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            return Ok(_mapper.Map<User>(user));
        }

        // POST: api/user
        [HttpPost("User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (_userServices.ExistsMobileNumber(user.MobileNumber))
            {
                ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User userEntity = _mapper.Map<User>(user);
            bool res = await _userServices.AddUser(userEntity, RoleType.User);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            //Generate JWT token
            IActionResult response = BadRequest(ResponseMessages.ERROR_UPDATE);

            if (userEntity != null)
            {
                PaginationInfo paginationInfo = new PaginationInfo();
                IEnumerable<Company> companies = await _userServices.GetAllCompanies();

                foreach (var c in companies)
                {
                    await _companyServices.AddCompanyToFavorite(userEntity.Id, c.Id);
                } 

                var tokenString = _securityServices.GenerateJWTToken(userEntity);
                userEntity.Token = tokenString;
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = _mapper.Map<User>(userEntity),
                });
            }

            return response;
        }

        // POST: api/user
        [HttpPost("CompanyUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostCompanyUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (_userServices.ExistsMobileNumber(user.MobileNumber))
            {
                ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User userEntity = _mapper.Map<User>(user);
            bool res = await _userServices.AddCompanyUser(userEntity, RoleType.Company);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            //Generate JWT token
            IActionResult response = BadRequest(ResponseMessages.ERROR_UPDATE);

            if (userEntity != null)
            {
                var tokenString = _securityServices.GenerateJWTToken(userEntity);
                userEntity.Token = tokenString;
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = _mapper.Map<User>(userEntity),
                });
            }

            return response;
        }

        // POST: api/user
        [HttpPost("Admin")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAdmin([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (_userServices.ExistsMobileNumber(user.MobileNumber))
            {
                ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User userEntity = _mapper.Map<User>(user);
            bool res = await _userServices.AddUser(userEntity, RoleType.Admin);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            //Generate JWT token
            IActionResult response = BadRequest(ResponseMessages.ERROR_UPDATE);

            if (userEntity != null)
            {
                var tokenString = _securityServices.GenerateJWTToken(userEntity);

                response = Ok(new
                {
                    token = tokenString,
                    userDetails = _mapper.Map<UserDto>(userEntity),
                });
            }

            return response;
        }

        // POST: api/user
        [HttpPost("Sybertech")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostSybertechUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (_userServices.ExistsMobileNumber(user.MobileNumber))
            {
                ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User userEntity = _mapper.Map<User>(user);
            bool res = await _userServices.AddUser(userEntity, RoleType.Sybertech);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            //Generate JWT token
            IActionResult response = BadRequest(ResponseMessages.ERROR_UPDATE);

            if (userEntity != null)
            {
                var tokenString = _securityServices.GenerateJWTToken(userEntity);

                response = Ok(new
                {
                    token = tokenString,
                    userDetails = _mapper.Map<UserDto>(userEntity),
                });
            }

            return response;
        }

        // POST: api/user
        [HttpPost("Support")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostSupport([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (_userServices.ExistsMobileNumber(user.MobileNumber))
            {
                ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User userEntity = _mapper.Map<User>(user);
            bool res = await _userServices.AddUser(userEntity, RoleType.Support);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            //Generate JWT token
            IActionResult response = BadRequest(ResponseMessages.ERROR_UPDATE);

            if (userEntity != null)
            {
                var tokenString = _securityServices.GenerateJWTToken(userEntity);

                response = Ok(new
                {
                    token = tokenString,
                    userDetails = _mapper.Map<UserDto>(userEntity),
                });
            }

            return response;
        }

        // PUT: api/user
        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            //get old user for check if user update whatsApp phone number and to check if user exist

            //get user id from token
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User oldUser = await _userServices.GetByMobileNumber(user.MobileNumber, 0);
            User currentUser = await _userServices.GetUserById(currentUserId, 0);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            if ((oldUser.Id != currentUserId) && (currentUser.RoleId != (int)RoleType.Admin))
            {
                ModelState.AddModelError("user", UserErrorMessages.NOT_OWNER);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (!oldUser.MobileNumber.Equals(user.MobileNumber))
            {
                if (_userServices.ExistsMobileNumber(user.MobileNumber))
                {
                    ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                oldUser.MobileNumber = user.MobileNumber;
            }

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;

            bool res = await _userServices.Update(oldUser);

            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        // PUT: api/user
        [HttpPut("UpdateUser")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User oldUser = await _userServices.GetUserById(user.Id??0, 0);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }


            if (!oldUser.MobileNumber.Equals(user.MobileNumber))
            {
                if (_userServices.ExistsMobileNumber(user.MobileNumber))
                {
                    ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                oldUser.MobileNumber = user.MobileNumber;
            }

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            oldUser.Disabled = user.Disabled;

            bool res = await _userServices.UpdateUser(oldUser, user.Password);

            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        [HttpPut("UpdateAdmin")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAdmin([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            //get old user for check if user update whatsApp phone number and to check if user exist

            //get user id from token
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User oldUser = await _userServices.GetByMobileNumber(user.MobileNumber, 0);
            User currentUser = await _userServices.GetUserById(currentUserId, 0);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            if (!oldUser.MobileNumber.Equals(user.MobileNumber))
            {
                if (_userServices.ExistsMobileNumber(user.MobileNumber))
                {
                    ModelState.AddModelError("user", UserErrorMessages.MOBILE_NUMBER_EXISTS);
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                oldUser.MobileNumber = user.MobileNumber;
            }

            oldUser.FirstName = user.FirstName;
            oldUser.LastName = user.LastName;
            bool res = await _userServices.Update(oldUser);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteUser(int id)
        {

            User user = await _userServices.GetUserById(id, 0);
          
            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);            
            User currentUser = await _userServices.GetUserById(currentUserId, 0);


            // Check if the person deleting the account is its owner
            if ((user.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)) && ((currentUser.RoleId != (int)RoleType.Admin)))
            {
                ModelState.AddModelError("user", UserErrorMessages.NOT_OWNER);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (!_userServices.DeleteUser(user))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.DELETED_SUCCESSFULLY);
        }

        // Put: api/user/ChangePassword
        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordRequest)
        {
            if (changePasswordRequest == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            User oldUser = await _userServices.GetByMobileNumber(changePasswordRequest.MobileNumber, 0);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User currentUser = await _userServices.GetUserById(currentUserId, 0);

            if ((oldUser.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)) && ((currentUser.RoleId != (int)RoleType.Admin)))
            {
                ModelState.AddModelError("user", UserErrorMessages.NOT_OWNER);
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            if (!_securityServices.VerifyPassword(changePasswordRequest.OldPassword, oldUser.StoredSalt, oldUser.Password))
            {
                ModelState.AddModelError("Password", LoginErrorMessages.INCORRECT_OLD_PASSWORD);
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            if (!_userServices.ChangePassword(oldUser, changePasswordRequest.NewPassword))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        // Put: api/user/ResetPassword
        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordBody resetPasswordBody)
        {
            if (resetPasswordBody == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            User oldUser = await _userServices.GetByMobileNumber(resetPasswordBody.MobileNumber, 0);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            //TODO
            //should check OTP VerificationId

            if (!_userServices.ChangePassword(oldUser, resetPasswordBody.NewPassword))
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }


        // Put: api/user/ResetPassword
        [HttpPut("UpdateUserProfile")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileDto updateUserProfileDto)
        {
            if (updateUserProfileDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            User oldUser = await _userServices.GetByMobileNumber(updateUserProfileDto.MobileNumber, 0);

            if (oldUser == null)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            //TODO
            //should check OTP VerificationId
            bool res = await _userServices.UpdateUserProfile(currentUserId, updateUserProfileDto.MobileNumber, updateUserProfileDto.Name, updateUserProfileDto.ImageURL);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        // Put: api/user/ResetPassword
        [HttpPut("UpdateFirebaseToken")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateFirebaseToken([FromBody] UpdateFirebaseTokenDto updateFirebaseTokenDto)
        {
            if (updateFirebaseTokenDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            bool res = await _userServices.UpdateFirebaseToken(currentUserId, updateFirebaseTokenDto.FirebaseToken);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }



        // POST: api/user
        [HttpPost("AddNotification")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddNotification([FromBody] NotificationDto notification)
        {
            if (notification == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (notification.UserId != currentUserId)
            {
                return BadRequest(ResponseMessages.NOT_LOGGEDIN);
            }

            Notification mappedNotification = _mapper.Map<Notification>(notification);
            mappedNotification.Cleared = false;

            bool res = await _userServices.AddNotification(mappedNotification);

            return Ok(res);
        }

        // POST: api/GetNotifications
        [HttpGet("GetNotifications")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetNotifications(int page = 1, int pageSize = 10)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<Notification> notifications = await _userServices.GetNotifications(page, pageSize, paginationInfo, currentUserId);
            IEnumerable<Notification> notification = _mapper.Map<IEnumerable<Notification>>(notifications);

            var response = new
            {
                Notifications = notification,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }


        // POST: api/user
        [HttpPost("AddPaymentTransaction")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPaymentTransaction([FromBody] PaymentTransactionDto paymentTransaction)
        {
            try
            {
                if (paymentTransaction == null)
                {
                    return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                double balance = await _userServices.getUserBalance(currentUserId);
                if (balance >= (paymentTransaction.Amount))
                {
                    if (paymentTransaction.UserId != currentUserId)
                    {
                        return BadRequest(ResponseMessages.NOT_LOGGEDIN);
                    }

                    PaymentTransaction mappedPaymentTransaction = new PaymentTransaction();//_mapper.Map<PaymentTransaction>(paymentTransaction);

                    mappedPaymentTransaction.Amount = paymentTransaction.Amount;
                    mappedPaymentTransaction.UserId = paymentTransaction.UserId;
                    mappedPaymentTransaction.CompanyId = paymentTransaction.CompanyId;
                    mappedPaymentTransaction.Fastfill = paymentTransaction.Fastfill;
                    mappedPaymentTransaction.FuelTypeId = paymentTransaction.FuelTypeId;
                    mappedPaymentTransaction.Status = paymentTransaction.Status;
                    mappedPaymentTransaction.Date = DateTime.Now;

                    bool res = await _userServices.AddPaymentTransaction(mappedPaymentTransaction);
                    if (res)
                    {
                        await _userServices.sendTransactionNotificationToCompany(mappedPaymentTransaction.CompanyId, mappedPaymentTransaction);
                    }

                    return Ok(res);
                }
                else
                    return Ok(false);
            }
            catch (Exception ex)
            {
                if (ex.Message != null)
                    await _userServices.LogError(0, "2", ex.Message, "", "", "AddPaymentTransaction");
                if (ex.InnerException != null)
                    if (ex.InnerException.Message != null)
                        await _userServices.LogError(0, "2", ex.InnerException.Message, "inner message", "", "AddPaymentTransaction");

                return Ok(false);                
            }            
        }


        // POST: api/GetNotifications
        [HttpGet("GetPaymentTransactions")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPaymentTransactions(int page = 1, int pageSize = 10)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<PaymentTransaction> paymentTransactions = await _userServices.GetPaymentTransactions(page, pageSize, paginationInfo, currentUserId);
            IEnumerable<PaymentTransaction> paymentTransaction = _mapper.Map<IEnumerable<PaymentTransaction>>(paymentTransactions);

            var response = new
            {
                PaymentTransactions = paymentTransaction,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }


        [RequestSizeLimit(MaxFileSize)]
        [HttpPost("UploadLogo")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAttachment(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "File not selected");
            }

            string url = await _userServices.Upload(file);

            var response = new
            {
                Url = url,
            };

            return Ok(response);
        }



        // Put: api/user/Update
        [HttpPut("Update")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            User mappedUser = await _userServices.GetUserById(updateUserDto.Id, 0);
            mappedUser.FirstName = updateUserDto.FirstName;
            mappedUser.LastName = updateUserDto.LastName;
            mappedUser.Username = updateUserDto.Username;
            mappedUser.FirstName = updateUserDto.MobileNumber;
            mappedUser.Disabled = updateUserDto.Disabled;
            mappedUser.RoleId = updateUserDto.RoleId;
            mappedUser.Password = updateUserDto.Password;

            bool res = await _userServices.Update(mappedUser);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.UPDATED_SUCCESSFULLY);
        }

        // Put: api/user/Update
        [HttpPost("Insert")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Insert([FromBody] UpdateUserDto updateUserDto)
        {
            if (updateUserDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(LoginErrorMessages.ModelStateParser(ModelState));
            }

            User mappedUser = new User();
            mappedUser.FirstName = updateUserDto.FirstName;
            mappedUser.LastName = updateUserDto.LastName;
            mappedUser.Username = updateUserDto.Username;
            mappedUser.MobileNumber = updateUserDto.MobileNumber;
            mappedUser.Disabled = updateUserDto.Disabled;
            mappedUser.RoleId = updateUserDto.RoleId;
            mappedUser.Password = updateUserDto.Password;

            bool res = await _userServices.AddUser(mappedUser, RoleType.User);
            if (!res)
            {
                return BadRequest(ResponseMessages.ERROR_UPDATE);
            }

            return Ok(ResponseMessages.ADDED_SUCCESSFULLY);
        }


        // POST: api/user
        [HttpGet("Inquiry")]
        [Authorize(Policy = Policies.Sybertech)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Inquiry(string mobileNumber)
        {
            _userServices.Log("Inquiry");
            _userServices.Log(mobileNumber);

            if ((mobileNumber == null) || (mobileNumber.Trim() == ""))
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User user = await _userServices.GetByMobileNumber(mobileNumber, 0);
            

            if (user == null)
            {
                var failedResponse = new
                {
                    Response = "",
                    ResponseCode = "1",
                    ResponseMessage = ResponseMessages.NOT_FOUND,
                };
                return Ok(failedResponse);
            }
            else
            {
                SybertechUserInfo sybertechUserInfo = new SybertechUserInfo();
                sybertechUserInfo.Fullname = user.FirstName;
                sybertechUserInfo.MobileNumber = user.MobileNumber;
                sybertechUserInfo.Id = user.Id;

                var successResponse = new
                {
                    Response = sybertechUserInfo,
                    ResponseCode = "0",
                    ResponseMessage = "",
                };
                return Ok(successResponse);
            }
        }


        // POST: api/user
        [HttpGet("InquiryForFaisal")]
        [Authorize(Policy = Policies.Faisal)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InquiryForFaisal(string mobileNumber)
        {
            _userServices.Log("Inquiry");
            _userServices.Log(mobileNumber);

            if ((mobileNumber == null) || (mobileNumber.Trim() == ""))
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User user = await _userServices.GetByMobileNumber(mobileNumber, 0);


            if (user == null)
            {
                var failedResponse = new
                {
                    Response = "",
                    ResponseCode = "1",
                    ResponseMessage = ResponseMessages.NOT_FOUND,
                };
                return Ok(failedResponse);
            }
            else
            {
                SybertechUserInfo sybertechUserInfo = new SybertechUserInfo();
                sybertechUserInfo.Fullname = user.FirstName;
                sybertechUserInfo.MobileNumber = user.MobileNumber;
                sybertechUserInfo.Id = user.Id;

                var successResponse = new
                {
                    Response = sybertechUserInfo,
                    ResponseCode = "0",
                    ResponseMessage = "",
                };
                return Ok(successResponse);
            }
        }



        [HttpGet("InquiryForBushrapay/{mobileNumber}/{lang}/{token}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> InquiryForBushrapay(string mobileNumber, string lang, string token)
        {

            try
            {
                if (token == "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4")
                {
                    _userServices.Log("Inquiry");
                    _userServices.Log(mobileNumber);

                    if ((mobileNumber == null) || (mobileNumber.Trim() == ""))
                    {
                        return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                    }

                    User user = await _userServices.GetByMobileNumber(mobileNumber, 0);


                    if (user == null)
                    {
                        var failedResponse = new
                        {
                            data = new { },
                            ResponseCode = "406",
                            ResponseStatus = "notfound",
                            ResponseMessage = ((lang == "ar") ? "لم استطع ايجاد مستخدم مرتبط برقم الموبايل:" : "Could not find user with mobile number: ") + mobileNumber,

                        };
                        return Ok(failedResponse);
                    }
                    else
                    {
                        SybertechUserInfo sybertechUserInfo = new SybertechUserInfo();
                        sybertechUserInfo.Fullname = user.FirstName;
                        sybertechUserInfo.MobileNumber = user.MobileNumber;
                        sybertechUserInfo.Id = user.Id;

                        var successResponse = new
                        {
                            data = new
                            {
                                customer_name = user.FirstName,
                                customer_id = user.Id,
                                customer_mobileNo = user.MobileNumber
                            },
                            ResponseCode = "1",
                            ResponseStatus = "success",
                            ResponseMessage = (lang == "ar") ? "تم عرض البيانات الخاصة بك بنجاح" : "Your information was successfully displayed",
                        };
                        return Ok(successResponse);
                    }
                }
                else
                {
                    throw new NullReferenceException("Invalid Token");
                }
            }
            catch (Exception ex)
            {
                var successResponse = new
                {
                    data = new { },
                    ResponseCode = "0",
                    ResponseStatus = "failure",
                    ResponseMessage = (lang == "ar") ? "خطأ غير معروف, يرجى إعادة المحاولة" : "Unknown error, Please try again",
                };
                return Ok(successResponse);
            }
        }


        // POST: api/user
        [HttpPost("TopUpCreditForBushrapay")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TopUpCreditForBushrapay([FromBody] TopUpCreditDto creditDto)
        {
            try
            {
                if (creditDto.token == "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4")
                {

                    _userServices.Log("CreditForBushrapay");
                    if (creditDto != null)
                    {
                        _userServices.Log(creditDto.mobileNumber);
                        _userServices.Log(creditDto.transactionId);
                        _userServices.Log(creditDto.amount.ToString());
                    }
                    else
                        _userServices.Log("Credit dto has nothing to do");
                    if (creditDto == null)
                    {
                        return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
                    }

                    if (!ModelState.IsValid)
                    {
                        return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                    }


                    bool status = await _userServices.CheckPaymentForBushrapay(creditDto.transactionId);

                    if (status)
                    {
                        var failedResponse = new
                        {
                            data = new { },
                            ResponseCode = "407",
                            ResponseStatus = "unsuccess",
                            ResponseMessage = (creditDto.lang == "ar") ? "رقم العملية مكرر, يرجى اعادة المحاولة" : "Duplicated transaction id, Please try again",
                        };
                        return Ok(failedResponse);
                    }
                    else
                    {

                        User user = await _userServices.GetByMobileNumber(creditDto.mobileNumber, 0);

                        if (user == null)
                        {
                            var failedResponse = new
                            {
                                data = new { },
                                ResponseCode = "406",
                                ResponseStatus = "unsuccess",
                                ResponseMessage = ((creditDto.lang == "ar") ? "لم استطع ايجاد مستخدم مرتبط برقم الموبايل:" : "Could not find user with mobile number: ") + creditDto.mobileNumber,
                            };
                            return Ok(failedResponse);
                        }
                        else
                        {
                            UserCredit f_us = _mapper.Map<UserCredit>(creditDto);
                            f_us.Date = DateTime.Now;
                            f_us.UserId = user.Id;
                            f_us.User = user;
                            f_us.RefillSourceId = (int)RefillSource.Bushrapay;

                            UserCredit uc = await _userServices.TopUpUserCredit(f_us);
                            if (uc == null)
                            {
                                var failedResponse = new
                                {
                                    data = new { },
                                    ResponseCode = "0",
                                    ResponseStatus = "failure",
                                    ResponseMessage = (creditDto.lang == "ar") ? "خطأ غير معروف, يرجى إعادة المحاولة" : "Unknown error, Please try again",
                                };
                                return Ok(failedResponse);
                            }
                            else
                            {
                                UserRefillTransactionDto userRefillTransactionDto = new UserRefillTransactionDto();
                                userRefillTransactionDto.Amount = creditDto.amount;
                                userRefillTransactionDto.transactionId = creditDto.transactionId;
                                userRefillTransactionDto.UserId = user.Id;
                                userRefillTransactionDto.status = true;
                                userRefillTransactionDto.Date = DateTime.Now;
                                userRefillTransactionDto.RefillSourceId = (int)RefillSource.Bushrapay;

                                await AddUserRefillTransaction(userRefillTransactionDto);
                                await _userServices.sendPaymentStatusNotification(creditDto.transactionId, true);
                                var successResponse = new
                                {
                                    data = uc,
                                    ResponseCode = "1",
                                    ResponseStatus = "success",
                                    ResponseMessage = (creditDto.lang == "ar") ? "تمت عملية الدفع بنجاح" : "Payment was successfully processed",
                                };
                                return Ok(successResponse);
                            }
                        }
                    }
                }
                else
                {
                    throw new NullReferenceException("Invalid Token");
                }
            }
            catch (Exception ex)
            {
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "TopUpCreditForBushrapay");
                var successResponse = new
                {
                    data = new { },
                    ResponseCode = "0",
                    ResponseStatus = "failure",
                    ResponseMessage = (creditDto.lang == "ar") ? "خطأ غير معروف, يرجى إعادة المحاولة" : "Unknown error, Please try again",
                };
                return Ok(successResponse);
            }
        }

        // POST: api/user
        [HttpGet("CheckPaymentForBushrapay/{transactionId}/{lang}/{token}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckPaymentForBushrapay(string transactionId, string lang, string token)
        {

            try
            {

                if (token == "eyJhbGciOiJIUzI1NiJ9.eyJSb2xlIjoiQWRtaW4")
                {
                    bool res = await _userServices.CheckPaymentForBushrapay(transactionId);

                    var response = new
                    {
                        data = new { },
                        ResponseCode = (res) ? "1" : "0",
                        ResponseStatus = (res) ? "success" : "unsuccess",
                        ResponseMessage = (lang == "ar") ? (res) ? "تمت عملية الدفع بنجاح" : "لم تتم عملية الدفع" : (res) ? "Payment was successfully processed" : "Payment unsuccessful"
                    };

                    return Ok(response);
                }
                else
                {
                    throw new NullReferenceException("Invalid Token");
                }
            }
            catch (Exception ex)
            {
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "TopUpCreditForBushrapay");
                var successResponse = new
                {
                    data = new { },
                    ResponseCode = "0",
                    ResponseStatus = "failure",
                    ResponseMessage = (lang == "ar") ? "خطأ غير معروف, يرجى إعادة المحاولة" : "Unknown error, Please try again",
                };
                return Ok(successResponse);
            }
        }

        // POST: api/user
        [HttpGet("CheckPaymentForFaisal")]
        //[Authorize(Policy = Policies.Bushrapay)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CheckPaymentForFaisal(string transactionId)
        {
            try
            {

                bool res = await _userServices.CheckPaymentForFaisal(transactionId);

                var response = new
                {
                    ResponseCode = (res) ? "0" : "1",
                    ResponseStatus = (res) ? "success" : "unsuccess",
                    ResponseMessage = (res) ? "Payment was successfully processed" : "Payment unsuccessful"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "TopUpCreditForFaisal");
                var successResponse = new
                {
                    data = new { },
                    ResponseCode = "0",
                    ResponseStatus = "unsuccess",
                    ResponseMessage = "",
                };
                return Ok(successResponse);
            }
        }

        // POST: api/user
        [HttpPost("Credit")]
        //[Authorize(Policy = Policies.Sybertech)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TopUpCredit([FromBody] TopUpCreditDto creditDto)
        {
            try
            {
                _userServices.Log("Credit");
                if (creditDto != null)
                {
                    _userServices.Log(creditDto.mobileNumber);
                    _userServices.Log(creditDto.transactionId);
                    _userServices.Log(creditDto.amount.ToString());
                }
                else
                    _userServices.Log("Credit dto has nothing to do");
                if (creditDto == null)
                {
                    return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                User user = await _userServices.GetByMobileNumber(creditDto.mobileNumber, 0);

                if (user == null)
                {
                    var failedResponse = new
                    {
                        Response = "",
                        ResponseCode = "1",
                        ResponseMessage = ResponseMessages.NOT_FOUND,
                    };
                    return Ok(failedResponse);
                }
                else
                {
                    UserCredit f_us = _mapper.Map<UserCredit>(creditDto);
                    f_us.Date = DateTime.Now;
                    f_us.UserId = user.Id;
                    f_us.User = user;
                    f_us.RefillSourceId = (int)RefillSource.Sybertech;

                    UserCredit uc = await _userServices.TopUpUserCredit(f_us);
                    if (uc == null)
                    {
                        var failedResponse = new
                        {
                            Response = "",
                            ResponseCode = "2",
                            ResponseMessage = ResponseMessages.ERROR_UPDATE,
                        };
                        return Ok(failedResponse);
                    }
                    else
                    {
                        UserRefillTransactionDto userRefillTransactionDto = new UserRefillTransactionDto();
                        userRefillTransactionDto.Amount = creditDto.amount;
                        userRefillTransactionDto.transactionId = creditDto.transactionId;
                        userRefillTransactionDto.UserId = user.Id;
                        userRefillTransactionDto.status = true;
                        userRefillTransactionDto.Date = DateTime.Now;
                        userRefillTransactionDto.RefillSourceId = (int)RefillSource.Sybertech;

                        await AddUserRefillTransaction(userRefillTransactionDto);
                        await _userServices.NotifyUserPaymentStatus(creditDto.transactionId, true);
                        var successResponse = new
                        {
                            Response = uc,
                            ResponseCode = "0",
                            ResponseMessage = "",
                        };
                        return Ok(successResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "TopUpCredit");
                var successResponse = new
                {
                    Response = "",
                    ResponseCode = "1",
                    ResponseMessage = "",
                };
                return Ok(successResponse);
            }
        }

        // POST: api/user
        [HttpPost("CreditForFaisal")]
        //[Authorize(Policy = Policies.Faisal)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TopUpCreditForFaisal([FromBody] TopUpCreditDto creditDto)
        {
            try
            {
                _userServices.Log("Credit");
                if (creditDto != null)
                {
                    _userServices.Log(creditDto.mobileNumber);
                    _userServices.Log(creditDto.transactionId);
                    _userServices.Log(creditDto.amount.ToString());
                }
                else
                    _userServices.Log("Credit dto has nothing to do");
                if (creditDto == null)
                {
                    return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                User user = await _userServices.GetByMobileNumber(creditDto.mobileNumber, 0);

                if (user == null)
                {
                    var failedResponse = new
                    {
                        Response = "",
                        ResponseCode = "1",
                        ResponseMessage = ResponseMessages.NOT_FOUND,
                    };
                    return Ok(failedResponse);
                }
                else
                {
                    UserCredit f_us = _mapper.Map<UserCredit>(creditDto);
                    f_us.Date = DateTime.Now;
                    f_us.UserId = user.Id;
                    f_us.User = user;
                    f_us.RefillSourceId = (int)RefillSource.Faisal;

                    UserCredit uc = await _userServices.TopUpUserCredit(f_us);
                    if (uc == null)
                    {
                        var failedResponse = new
                        {
                            Response = "",
                            ResponseCode = "2",
                            ResponseMessage = ResponseMessages.ERROR_UPDATE,
                        };
                        return Ok(failedResponse);
                    }
                    else
                    {
                        UserRefillTransactionDto userRefillTransactionDto = new UserRefillTransactionDto();
                        userRefillTransactionDto.Amount = creditDto.amount;
                        userRefillTransactionDto.transactionId = creditDto.transactionId;
                        userRefillTransactionDto.UserId = user.Id;
                        userRefillTransactionDto.status = true;
                        userRefillTransactionDto.Date = DateTime.Now;
                        userRefillTransactionDto.RefillSourceId = (int)RefillSource.Faisal;

                        await AddUserRefillTransaction(userRefillTransactionDto);
                        await _userServices.sendPaymentStatusNotification(creditDto.transactionId, true);
                        var successResponse = new
                        {
                            Response = uc,
                            ResponseCode = "0",
                            ResponseMessage = "",
                        };
                        return Ok(successResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "TopUpCreditForFaisal");
                var successResponse = new
                {
                    Response = "",
                    ResponseCode = "1",
                    ResponseMessage = "",
                };
                return Ok(successResponse);
            }
        }


        // POST: api/user
        [HttpPost("CreditFromAdmin")]
        //[Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TopUpCreditFromAdmin([FromBody] TopUpCreditDto creditDto)
        {
            try
            {
                _userServices.Log("Credit");
                if (creditDto != null)
                {
                    _userServices.Log(creditDto.mobileNumber);
                    _userServices.Log(creditDto.transactionId);
                    _userServices.Log(creditDto.amount.ToString());
                }
                else
                    _userServices.Log("Credit dto has nothing to do");
                if (creditDto == null)
                {
                    return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                User user = await _userServices.GetByMobileNumber(creditDto.mobileNumber, 0);

                if (user == null)
                {
                    var failedResponse = new
                    {
                        Response = "",
                        ResponseCode = "1",
                        ResponseMessage = ResponseMessages.NOT_FOUND,
                    };
                    return Ok(failedResponse);
                }
                else
                {
                    UserCredit f_us = _mapper.Map<UserCredit>(creditDto);
                    f_us.Date = DateTime.Now;
                    f_us.UserId = user.Id;
                    f_us.User = user;
                    f_us.RefillSourceId = (int)RefillSource.Manual;
                    UserCredit uc = await _userServices.TopUpUserCredit(f_us);
                    if (uc == null)
                    {
                        var failedResponse = new
                        {
                            Response = "",
                            ResponseCode = "2",
                            ResponseMessage = ResponseMessages.ERROR_UPDATE,
                        };
                        return Ok(failedResponse);
                    }
                    else
                    {
                        UserRefillTransactionDto userRefillTransactionDto = new UserRefillTransactionDto();
                        userRefillTransactionDto.Amount = creditDto.amount;
                        userRefillTransactionDto.transactionId = creditDto.transactionId;
                        userRefillTransactionDto.UserId = user.Id;
                        userRefillTransactionDto.status = true;
                        userRefillTransactionDto.Date = DateTime.Now;
                        userRefillTransactionDto.RefillSourceId = (int)RefillSource.Manual;

                        await AddUserRefillTransaction(userRefillTransactionDto);
                        await _userServices.sendPaymentStatusNotification(creditDto.transactionId, true);
                        var successResponse = new
                        {
                            Response = uc,
                            ResponseCode = "0",
                            ResponseMessage = "",
                        };
                        return Ok(successResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "TopUpCredit");
                var successResponse = new
                {
                    Response = "",
                    ResponseCode = "1",
                    ResponseMessage = "",
                };
                return Ok(successResponse);
            }
        }

        // POST: api/user
        [HttpPost("AddBankCard")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBankCard([FromBody] AddBankCardDto addBankCardDto)
        {
            if (addBankCardDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            BankCard mappedBankCard = _mapper.Map<BankCard>(addBankCardDto);

            mappedBankCard.UserId = currentUserId;

            bool res = await _userServices.AddBankCard(mappedBankCard);

            return Ok(res);
        }

        // POST: api/user
        [HttpPost("UpdateBankCard")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBankCard([FromBody] EditBankCardDto editBankCardDto)
        {
            if (editBankCardDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            BankCard mappedBankCard = _mapper.Map<BankCard>(editBankCardDto);

            mappedBankCard.UserId = currentUserId;

            bool res = await _userServices.UpdateBankCard(mappedBankCard);

            return Ok(res);
        }

        // POST: api/user
        [HttpDelete("DeleteBankCard/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBankCard(int id)
        {
            if (id <= 0)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            BankCard bankCard = await _userServices.GetBankCardById(id);

            if (bankCard.UserId != currentUserId)
                return BadRequest(UserErrorMessages.NOT_OWNER);
            
            bool res = await _userServices.DeleteBankCard(bankCard);

            return Ok(res);
        }


        // POST: api/GetNotifications
        [HttpGet("GetBankCards")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBankCards(int page = 1, int pageSize = 10)
        {
            try
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                PaginationInfo paginationInfo = new PaginationInfo();
                IEnumerable<BankCard> bankCards = await _userServices.GetBankCards(page, pageSize, paginationInfo, currentUserId);
                IEnumerable<BankCard> bankCard = _mapper.Map<IEnumerable<BankCard>>(bankCards);

                var response = new
                {
                    BankCards = bankCard,
                    PaginationInfo = paginationInfo
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userServices.LogError(currentUserId, "1", ex.Message, ex.InnerException?.Message ?? "", "", "GetBankCards");
                return Ok(null);
            }

        }


        // POST: api/user
        [HttpPost("PaymentNotify")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PaymentNotify([FromForm] PaymentNotifyDto paymentNotifyDto)
        {
            
            if (paymentNotifyDto != null)
                _userServices.Log(paymentNotifyDto.transactionId);
            else
                _userServices.Log("Payment Notify has nothing to print");

            await _userServices.LogError(0, "2", "TransactionId : " + paymentNotifyDto.transactionId, "", "", "PaymentNotify");

            if (paymentNotifyDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            CheckPaymentStatusDto checkPaymentStatusDto = new CheckPaymentStatusDto();

            try
            {
                bool res = await _userServices.NotifyUserPaymentStatus(paymentNotifyDto.transactionId, false);

                if (res)
                {
                    var successResponse = new
                    {
                        Response = "User Successfully notified ",
                        ResponseCode = "0",
                        ResponseMessage = "",
                    };
                    return Ok(successResponse);
                }
                else
                {
                    var invalidResponse = new
                    {
                        Response = "Could not notify user",
                        ResponseCode = "200",
                        ResponseMessage = "",
                    };
                    return Ok(invalidResponse);

                }
            }
            catch (Exception ex)
            {
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "PaymentNotify");

                var invalidResponse = new
                {
                    Response = "Could not notify user",
                    ResponseCode = "200",
                    ResponseMessage = "",
                };
                return Ok(invalidResponse);
            }
        }

        // POST: api/user
        [HttpPost("CancelPayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelPayment([FromBody] CancelPaymentDto cancelPaymentDto)
        {
            try
            {
                if (cancelPaymentDto != null)
                    _userServices.Log(cancelPaymentDto.responseStatus);
                else
                    _userServices.Log("Cancel Payment has nothing to print");

                if (cancelPaymentDto == null)
                {
                    return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                {
                    var successResponse = new
                    {
                        Response = "User Payment Successfully Cancelled",
                        ResponseCode = "0",
                        ResponseMessage = "",
                    };
                    return Ok(successResponse);
                }
            }
            catch (Exception ex)
            {
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "CancelPayment");
                var errorResponse = new
                {
                    Response = "Error while canceling user payment",
                    ResponseCode = "1",
                    ResponseMessage = "Error while canceling user payment",
                };
                return Ok(errorResponse);
            }
        }

        // POST: api/user
        [HttpPost("ReturnFromPayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReturnFromPayment([FromBody] ReturnFromPaymentDto returnFromPaymentDto)
        {
            try
            {
                if (returnFromPaymentDto != null)
                    _userServices.Log(returnFromPaymentDto.customerRef);
                else
                    _userServices.Log("Return from Payment has nothing to print");

                if (returnFromPaymentDto == null)
                {
                    return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                {
                    var successResponse = new
                    {
                        Response = "Successfully returned from user payment",
                        ResponseCode = "0",
                        ResponseMessage = "",
                    };
                    return Ok(successResponse);
                }
            }
            catch
            (Exception ex)
            {                
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "ReturnFromPayment");
                var errorResponse = new
                {
                    Response = "Error while returning from user payment",
                    ResponseCode = "1",
                    ResponseMessage = "Error while returning from user payment",
                };
                return Ok(errorResponse);

            }
        }

        // POST: api/user
        [HttpPost("AddUserRefillTransaction")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserRefillTransaction([FromBody] UserRefillTransactionDto userRefillTransactionDto)
        {
            try
            {
                if (userRefillTransactionDto == null)
                {
                    return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
                }

                //int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                UserRefillTransaction mappedUserRefillTransaction = _mapper.Map<UserRefillTransaction>(userRefillTransactionDto);

                //mappedUserRefillTransaction.UserId = currentUserId;

                bool res = await _userServices.addAccountRefillSyberPayTransaction(mappedUserRefillTransaction);

                return Ok(res);
            }
            catch (Exception ex)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userServices.LogError(currentUserId, "1", ex.Message, ex.InnerException?.Message ?? "", "", "AddUserRefillTransaction");
                return Ok(false);
            }
        }

        // POST: api/user
        [HttpPut("UpdateUserLanguage")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserLanguage([FromBody] UpdateUserLanguageDto updateUserLanguageDto)
        {
            try
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                User user = await _userServices.GetUserById(currentUserId, 0);
                user.Language = updateUserLanguageDto.LanguageId;
                await _userServices.UpdateWithoutPasswordEncryption(user);
                return Ok(true);
            }
            catch (Exception ex)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userServices.LogError(currentUserId, "1", ex.Message, ex.InnerException?.Message ?? "", "", "UpdateUserLanguage");
                return Ok(false);
            }
        }

        // POST: api/user
        [HttpGet("GetUserBalance")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserBalance()
        {
            try
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                double balance = await _userServices.getUserBalance(currentUserId);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userServices.LogError(currentUserId, "1", ex.Message, ex.InnerException?.Message??"", "", "GetUserBalance");
                return Ok(0.0);
            }
        }


        // POST: api/user
        [HttpGet("ClearNotifications")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClearNotifications()
        {
            try
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                bool res = await _userServices.clearNotifications(currentUserId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userServices.LogError(currentUserId, "1", ex.Message, ex.InnerException?.Message ?? "", "", "GetNotifications");
                return Ok(false);
            }
        }


        // POST: api/user
        [HttpGet("ClearTransactions")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ClearTransactions()
        {
            try
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                bool res = await _userServices.clearTransactions(currentUserId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userServices.LogError(currentUserId, "1", ex.Message, ex.InnerException?.Message ?? "", "", "ClearTransactions");
                return Ok(false);
            }
        }


        // POST: api/user
        [HttpPost("SendCustomNotification")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendCustomNotification([FromBody] CustomNotificationDto customNotificationDto)
        {
            try
            {
                User? user = await _userServices.GetByMobileNumber(customNotificationDto.mobileNumber, 0);
                if (user != null)
                {
                    bool res = await _userServices.sendCustomNotification(user.Id, customNotificationDto.title, customNotificationDto.content);
                    return Ok(res);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userServices.LogError(currentUserId, "1", ex.Message, ex.InnerException?.Message ?? "", "", "SendCustomNotification");
                return Ok(false);
            }
        }

        [HttpPost("SendCustomNotificationToMultiple")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendCustomNotificationToMultiple([FromBody] CustomNotificationToMultipleDto customNotificationToMultipleDto)
        {
            try
            {
                bool res = await _userServices.sendCustomNotificationToMultiple(customNotificationToMultipleDto.title, customNotificationToMultipleDto.content, customNotificationToMultipleDto.mobileNumbers);
                return Ok(res);
            }
            catch (Exception ex)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userServices.LogError(currentUserId, "1", ex.Message, ex.InnerException?.Message ?? "", "", "SendCustomNotification");
                return Ok(false);
            }
        }

        // GET: api/user/admins
        [HttpGet("CompaniesUsers")]
        [Authorize(Policy = Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompaniesUsers(int page = 1, int pageSize = 10)
        {
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> users = await _userServices.GetCompaniesUsers(page, pageSize, paginationInfo);
            IEnumerable<User> user = _mapper.Map<IEnumerable<User>>(users);

            var response = new
            {
                Users = user,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }

        // GET: api/user/ByPhone
        [HttpGet("CheckByPhone/{mobileNumber}")]
        public async Task<IActionResult> CheckUserByMobileNumber(string mobileNumber)
        {
            if (mobileNumber == null)
            {
                return Ok(false);
            }

            if (mobileNumber.Trim() == "")
            {
                return Ok(false);
            }
            User user = await _userServices.GetByMobileNumber(mobileNumber, 0);

            if (user == null)
            {
                return Ok(false);
            }

            return Ok(true);
        }

        // GET: api/user/admins
        [HttpGet("RemoveAccount")]
        [Authorize(Policy = Policies.User)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RemoveAccount()
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            bool res = await _userServices.removeAccount(currentUserId);
            return Ok(res);
        }


        // GET: api/user/admins
        [HttpGet("Logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            bool res = await _userServices.logout(currentUserId);
            return Ok(res);
        }


        // GET: api/user/admins
        [HttpGet("CompanyAgents")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyAgents(int page = 1, int pageSize = 10)
        {

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User companyUser = await _userServices.GetUserById(currentUserId, 0);
            int companyId = companyUser.CompanyId ?? 0;
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<User> users = await _userServices.GetCompanyAgents(companyId, page, pageSize, paginationInfo);
            IEnumerable<User> user = _mapper.Map<IEnumerable<User>>(users);

            var response = new
            {
                Users = user,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }

        [HttpGet("CompanyPumps")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCompanyPumps(int page = 1, int pageSize = 10)
        {

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User companyUser = await _userServices.GetUserById(currentUserId, 0);
            int companyId = companyUser.CompanyId ?? 0;
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyPump> pumps = await _userServices.GetCompanyPumps(companyId, page, pageSize, paginationInfo);
            IEnumerable<CompanyPump> pump = _mapper.Map<IEnumerable<CompanyPump>>(pumps);

            var response = new
            {
                Pumps = pump,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }

        // GET: api/user/admins
        [HttpPost("AssignPumpsAgents")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AssignPumpsAgents(List<PumpAgentDto> pumpsAgents)
        {

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User companyUser = await _userServices.GetUserById(currentUserId, 0);
            int companyId = companyUser.CompanyId ?? 0;
            bool res = false;
            if (companyId > 0)
                res = await _userServices.AssignPumpsAgents(companyId, pumpsAgents);
            return Ok(res);
        }

        // GET: api/user/admins
        [HttpGet("GetPumpsAgents")]
        [Authorize(Policy = Policies.Company)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPumpsAgents(int page = 1, int pageSize = 10)
        {

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            User companyUser = await _userServices.GetUserById(currentUserId, 0);
            int companyId = companyUser.CompanyId ?? 0;
            PaginationInfo paginationInfo = new PaginationInfo();
            IEnumerable<CompanyAgentPump> pumpsAgents = await _userServices.GetPumpsAgents(companyId, page, pageSize, paginationInfo);
            IEnumerable<CompanyAgentPump> pumpAgent = _mapper.Map<IEnumerable<CompanyAgentPump>>(pumpsAgents);

            var response = new
            {
                PumpsAgents = pumpAgent,
                PaginationInfo = paginationInfo
            };

            return Ok(response);

        }


        [HttpPost("SendCustomNotificationToAll")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendCustomNotificationToAll([FromBody] CustomNotificationDto customNotificationDto)
        {
            try
            {
                bool res = await _userServices.sendCustomNotificationToAll(customNotificationDto.title, customNotificationDto.content);
                return Ok(res);
            }
            catch (Exception ex)
            {
                int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _userServices.LogError(currentUserId, "1", ex.Message, ex.InnerException?.Message ?? "", "", "SendCustomNotificationToAll");
                return Ok(false);
            }
        }


        // GET: api/company/FrequentlyVisitedCompanies
        [HttpGet("GetFastFillFees")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFastFillFees()
        {
            return Ok(100.0);
        }

        // GET: api/company/FrequentlyVisitedCompanies
        [HttpGet("SendOTP/{mobileNumber}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SendOTP(string mobileNumber)
        {
            try
            {
                string registerId = Guid.NewGuid().ToString();
                Random generator = new Random();
                string otpCode = generator.Next(0, 1000000).ToString("D6");
                Otp otp = new Otp();
                otp.otpCode = otpCode;
                otp.registerId = registerId;
                otp.mobileNumber = mobileNumber;
                otp.Date = DateTime.Now;
                otp.status = false;

                bool res = await _userServices.generateOtp(otp);
                if (res)
                {
                    return Ok(registerId);
                }
                else
                    return Ok("Error");
            }
            catch (Exception ex)
            {
                await _userServices.LogError(0, "1", ex.Message, ex.InnerException?.Message ?? "", "", "SendOTP");
                return Ok("Error");
            }
        }


        [HttpGet("VerifyOTP/{registerId}/{otpCode}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> VerifyOTP(string registerId, string otpCode)
        {
            try
            {
                bool res = await _userServices.verifyOtp(registerId, otpCode);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }
    }
}
