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

namespace FastFill_API.Controllers
{
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserServices _userServices;
        private readonly SecurityServices _securityServices;

        private const long MaxFileSize = 30L * 1024L * 1024L; // 30MB, adjust to your need
        public UserController(IMapper mapper, UserServices userServices, SecurityServices securityServices)
        {
            _mapper = mapper;
            _userServices = userServices;
            _securityServices = securityServices;
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
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(int id)
        {
            if (id == 0)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }
            User user = await _userServices.GetUserById(id);

            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            return Ok(_mapper.Map<User>(user));
        }

        // GET: api/user/ByPhone
        [HttpGet("ByPhone/{mobileNumber}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserByMobileNumber (string mobileNumber)
        {
            if (mobileNumber == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (mobileNumber.Trim() == "")
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }
            User user = await _userServices.GetByMobileNumber(mobileNumber);

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

            User oldUser = await _userServices.GetByMobileNumber(user.MobileNumber);
            User currentUser = await _userServices.GetUserById(currentUserId);

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

            User oldUser = await _userServices.GetByMobileNumber(user.MobileNumber);
            User currentUser = await _userServices.GetUserById(currentUserId);

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

            User user = await _userServices.GetUserById(id);
          
            if (user == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);            
            User currentUser = await _userServices.GetUserById(currentUserId);


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

            User oldUser = await _userServices.GetByMobileNumber(changePasswordRequest.MobileNumber);

            if (oldUser == null)
            {
                return NotFound(ResponseMessages.NOT_FOUND);
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            User currentUser = await _userServices.GetUserById(currentUserId);

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

            User oldUser = await _userServices.GetByMobileNumber(resetPasswordBody.MobileNumber);

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

            User oldUser = await _userServices.GetByMobileNumber(updateUserProfileDto.MobileNumber);

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

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (notification.UserId != currentUserId)
            {
                return BadRequest(ResponseMessages.NOT_LOGGEDIN);
            }

            Notification mappedNotification = _mapper.Map<Notification>(notification);

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
            if (paymentTransaction == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (paymentTransaction.UserId != currentUserId)
            {
                return BadRequest(ResponseMessages.NOT_LOGGEDIN);
            }

            PaymentTransaction mappedPaymentTransactio  = _mapper.Map<PaymentTransaction>(paymentTransaction);

            bool res = await _userServices.AddPaymentTransaction(mappedPaymentTransactio);

            return Ok(res);
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

            User mappedUser = await _userServices.GetUserById(updateUserDto.Id);
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
            if ((mobileNumber == null) || (mobileNumber.Trim() == ""))
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User user = await _userServices.GetByMobileNumber(mobileNumber);
            

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
        [HttpPost("Credit")]
        [Authorize(Policy = Policies.Sybertech)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> TopUpCredit([FromBody] TopUpCreditDto creditDto)
        {
            if (creditDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User user = await _userServices.GetByMobileNumber(creditDto.mobileNumber);

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
                f_us.UserId = user.Id;
                f_us.User = user;
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


        // POST: api/user
        [HttpPost("AddBankCard")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddBankCard([FromBody] AddEditBankCardDto addEditBankCardDto)
        {
            if (addEditBankCardDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            BankCard mappedBankCard = _mapper.Map<BankCard>(addEditBankCardDto);

            mappedBankCard.UserId = currentUserId;

            bool res = await _userServices.AddBankCard(mappedBankCard);

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


        // POST: api/user
        [HttpPost("PaymentNotify")]
        [Authorize(Policy = Policies.Sybertech)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PaymentNotify([FromBody] PaymentNotifyDto paymentNotifyDto)
        {
            if (paymentNotifyDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User user = await _userServices.GetByMobileNumber(paymentNotifyDto.mobileNumber);

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
                var successResponse = new
                {
                    Response = "Payment Successful",
                    ResponseCode = "0",
                    ResponseMessage = "",
                };
                return Ok(successResponse);
            }
        }

        // POST: api/user
        [HttpPost("PaymentCancel")]
        [Authorize(Policy = Policies.Sybertech)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PaymentCancel([FromBody] PaymentNotifyDto paymentNotifyDto)
        {
            if (paymentNotifyDto == null)
            {
                return BadRequest(ResponseMessages.PUSH_EMPTY_VALUE);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(UserErrorMessages.ModelStateParser(ModelState));
            }

            User user = await _userServices.GetByMobileNumber(paymentNotifyDto.mobileNumber);

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
                var successResponse = new
                {
                    Response = "Payment Cancelled",
                    ResponseCode = "0",
                    ResponseMessage = "",
                };
                return Ok(successResponse);
            }
        }

    }
}
