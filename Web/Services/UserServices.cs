using FastFill_API.Interfaces;

using FastFill_API.Web.Dto;
using FastFill_API.Web.Model;
using FastFill_API.Web.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PdfSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FastFill_API.Web.Services
{
    public class UserServices
    {

        private readonly IPublicRepository _repository;
        private readonly SecurityServices _securityServices;

        public UserServices(IPublicRepository repository, SecurityServices securityServices)
        {
            _repository = repository;
            _securityServices = securityServices;
        }

        //Add new user
        public async Task<bool> AddUser(User user, RoleType role)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(user.Password);
                user.Password = hashSalt.Hash;
                user.StoredSalt = hashSalt.Salt;
                user.RoleId = (int)role;
                return await _repository.GetUserRepository.Insert(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> AddCompanyUser(User user, RoleType role)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(user.Password);
                user.Password = hashSalt.Hash;
                user.StoredSalt = hashSalt.Salt;
                user.RoleId = (int)role;
                return await _repository.GetUserRepository.Insert(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool ExistsMobileNumber(string mobileNumber)
        {
            return _repository.GetUserRepository.ExistsMobileNumber(mobileNumber);
        }

        public async Task<User> GetUserById(int id, int? roleId)
        {
            return await _repository.GetUserRepository.GetById(id, roleId);

        }

        public async Task<IEnumerable<User>> GetUsers(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<User> users = await _repository.GetUserRepository.GetUsers();
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<User>> GetAdmins(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<User> users = await _repository.GetUserRepository.GetAdmins();
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<User>> GetSupports(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<User> users = await _repository.GetUserRepository.GetSupports();
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<bool> UpdateUserProfile(int userId, string mobileNumber, string name, string imageURL) {

            try
            {
                return await _repository.GetUserRepository.UpdateUserProfile(userId, mobileNumber, name, imageURL);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<bool> UpdateFirebaseToken(int userId, string firebaseToken)
        {

            try
            {
                return await _repository.GetUserRepository.UpdateFirebaseToken(userId, firebaseToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }
        public async Task<bool> Update(User user)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(user.Password);
                user.Password = hashSalt.Hash;
                user.StoredSalt = hashSalt.Salt;
                return await _repository.GetUserRepository.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
        public async Task<bool> UpdateUser(User newUser, string newPassword)
        {
            try
            {
                if (newPassword != "")
                {
                    var hashSalt = _securityServices.EncryptPassword(newPassword);
                    newUser.Password = hashSalt.Hash;
                    newUser.StoredSalt = hashSalt.Salt;
                }
                return await _repository.GetUserRepository.Update(newUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> UpdateWithoutPasswordEncryption(User user)
        {
            try
            {
                return await _repository.GetUserRepository.Update(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public bool Exists(int id)
        {
            return _repository.GetUserRepository.Exists(id);
        }

        public bool DeleteUser(User user)
        {
            try
            {
                _repository.GetUserRepository.Delete(user);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<User> GetByMobileNumber(string mobileNumber, int? roleId)
        {
            return await _repository.GetUserRepository.GetByMobileNumber(mobileNumber, roleId);
        }

        public bool ChangePassword(User oldUser, string newPassword)
        {
            try
            {
                var hashSalt = _securityServices.EncryptPassword(newPassword);
                oldUser.Password = hashSalt.Hash;
                oldUser.StoredSalt = hashSalt.Salt;
                _repository.GetUserRepository.Update(oldUser);
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> AddNotification(Notification notification) {
            try
            {
                return await _repository.GetUserRepository.AddNotification(notification);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> AddPaymentTransaction(PaymentTransaction paymentTransaction)
        {
            try
            {
                paymentTransaction.Cleared = false;
                return await _repository.GetUserRepository.AddPaymentTransaction(paymentTransaction);

            }
            catch (DbUpdateConcurrencyException x)
            {
                if (x.Message != null)
                    await LogError(0, "2", x.Message, "", "", "AddPaymentTransaction");
                if (x.InnerException != null)
                    if (x.InnerException.Message != null)
                        await LogError(0, "2", x.InnerException.Message, "inner message", "", "AddPaymentTransaction");
                return false;
            }
        }

        public async Task<IEnumerable<Notification>> GetNotifications(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<Notification> notifications = await _repository.GetUserRepository.GetNotifications(userId);
            paginationInfo.SetValues(pageSize, page, notifications.Count());
            return notifications.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<PaymentTransaction>> GetPaymentTransactions(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<PaymentTransaction> paymentTransactions = await _repository.GetUserRepository.GetPaymentTransactions(userId);
            paginationInfo.SetValues(pageSize, page, paymentTransactions.Count());
            return paymentTransactions.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<string> Upload(IFormFile file)
        {
            var ext = System.IO.Path.GetExtension(file.FileName);
            var fileName = "attachment_" + System.Guid.NewGuid().ToString() + ext;

            var path = Directory.GetCurrentDirectory() + @"\attachments\" + fileName;

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                await file.CopyToAsync(stream);
            }

            string fileURL = "https://fastfillpro.developitech.com/attachments/" + fileName;

            return fileURL;
        }

        public async Task<UserCredit> TopUpUserCredit(UserCredit userCredit)
        {
            try
            {
                return await _repository.GetUserRepository.TopUpUserCredit(userCredit);

            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
        }

        public async Task<bool> CheckPaymentForBushrapay(string transactionId)
        {
            try
            {
                return await _repository.GetUserRepository.CheckPaymentForBushrapay(transactionId);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> CheckPaymentForFaisal(string transactionId)
        {
            try
            {
                return await _repository.GetUserRepository.CheckPaymentForFaisal(transactionId);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }


        public async Task<bool> UpdateBankCard(BankCard bankCard)
        {
            try
            {
                return await _repository.GetUserRepository.UpdateBankCard(bankCard);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> AddBankCard(BankCard bankCard)
        {
            try
            {
                return await _repository.GetUserRepository.AddBankCard(bankCard);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<BankCard>> GetBankCards(int page, int pageSize, PaginationInfo paginationInfo, int userId)
        {
            IEnumerable<BankCard> bankCards = await _repository.GetUserRepository.GetBankCards(userId);
            paginationInfo.SetValues(pageSize, page, bankCards.Count());
            return bankCards.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<bool> DeleteBankCard(BankCard bankCard)
        {
            try
            {
                return await _repository.GetUserRepository.DeleteBankCard(bankCard);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<BankCard> GetBankCardById(int id)
        {
            return await _repository.GetUserRepository.GetBankCardById(id);
        }

        public async Task<bool> sendTransactionNotificationToCompany(int companyId, PaymentTransaction paymentTransaction)
        {
            try
            {
                string companyUsersTokens = "";

            List<User> companyUsers = await _repository.GetUserRepository.GetCompanyUsers(companyId);

            companyUsers.Where((d) => (d.RoleId == (int)RoleType.Company)).ToList().ForEach((cu) => {
                if (cu.FirebaseToken != null)
                {
                    if (cu.FirebaseToken.Trim() != "")
                    {
                        companyUsersTokens = companyUsersTokens + "\"" + cu.FirebaseToken + "\"" + ",";
                    }
                }
            });

            if (companyUsersTokens.EndsWith(","))
                companyUsersTokens = companyUsersTokens.Substring(0, companyUsersTokens.Length - 1);

            User u = await GetUserById(paymentTransaction.UserId, 0);

            string date = paymentTransaction.Date.ToString("yyyy-MM-dd");
            string time = paymentTransaction.Date.ToString("hh:mm:ss"); ;
            string typeId = "2";
            string userId = paymentTransaction.UserId.ToString();
            string imageURL = "";
            string notes = "";
            string liters = "";
            string material = paymentTransaction.FuelTypeId.ToString();
            string price = paymentTransaction.Amount.ToString();
            string address = "";
            string userName = u.FirstName;
            string userMobileNo = u.MobileNumber;
            string transactionId = paymentTransaction.Id.ToString();
            string fastFill = paymentTransaction.Fastfill.ToString();
            string notificationTitle = "Payment from " + userName + " - " + userMobileNo;
            string notificationContent = (paymentTransaction.Amount-paymentTransaction.Fastfill).ToString() + " SDG was successfully paid by Fast fuel fill    ";

            if (u.Language != 1)
            {
                notificationTitle = "دفع من قبل " + userName + " - " + userMobileNo;
                notificationContent = (paymentTransaction.Amount - paymentTransaction.Fastfill).ToString() + " ج.س تم دفعها من خلال تطبيق فاست فيل";
            }


            string FCMServerKey = Startup.Configuration.GetSection("Firebase")["ServerKey"].ToString(); ;
            string payload = "{ \"registration_ids\": [ " + companyUsersTokens + " ], " +
                              "\"priority\":\"" + "high" + "\"" +

                              "\"notification\": {\"sound\":\"" + "default" + "\"," + "\"click_action\":\"" + "FLUTTER_NOTIFICATION_CLICK" + "\"," +
                              "\"body\":\"" + notificationContent.Replace(Environment.NewLine, " ") + "\"," +
                              "\"title\":\"" + notificationTitle.Replace(Environment.NewLine, " ") + "\"," +
                              "\"image\":\"" + "" + "\"}" +
                              "\"content_available\":" + "true" + "," +    //with this ios won't receive notification (this is for data messages)
                                                                           //"\"notification\": {\"sound\":\"" + "default" + "\"," + "\"click_action\":\"" + "MainActivity" + "\"," +
                                                                           //"\"body\":\"" + NotificationContent + "\",\"title\":\"" + NotificationTitle + "\"}" +

                                "\"data\": {\"body\":\"" + notificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + notificationTitle.Replace(Environment.NewLine, " ") + "\"," +
                                "\"Type\":\"" + typeId + "\"," +
                                "\"date\":\"" + date + "\"," +
                                "\"time\":\"" + time + "\"," +
                                "\"userId\":\"" + userId + "\"," +
                                "\"imageUrl\":\"" + imageURL + "\"," +
                                "\"notes\":\"" + notes + "\"," +
                                "\"liters\":\"" + liters + "\"," +
                                "\"material\":\"" + material + "\"," +
                                "\"price\":\"" + price + "\"," +
                                "\"address\":\"" + address + "\"," +
                                "\"userName\":\"" + userName + "\"," +
                                "\"userMobileNo\":\"" + userMobileNo + "\"," +
                                "\"transactionId\":\"" + transactionId + "\"," +
                                "\"fastFill\":\"" + fastFill + "\"," +
                                "\"companyId\":\"" + companyId.ToString() + "\"," +
                                "\"notification\": {\"sound\":\"" + "default" + "\"," +
                                          "\"body\":\"" + notificationContent.Replace(Environment.NewLine, " ") + "\",\"title\":\"" + notificationTitle.Replace(Environment.NewLine, " ") + "\"," + "\"image\":\"" + "" + "\"}" +
                                "\"userTotalReceivedWaiting\":\"" + "0" + "\"}}";
            
                string postDataContentType = "application/json; charset=utf-8";
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FirebaseUtils.ValidateServerCertificate);
                HttpWebRequest Request;
                Stream dataStream;
                byte[] byteArray;

                byteArray = Encoding.UTF8.GetBytes(payload);
                string SendNotificationURL = Startup.Configuration.GetSection("Firebase")["SendNotificationURL"].ToString();
                Request = (HttpWebRequest)WebRequest.Create(SendNotificationURL);

                Request.Method = "POST";
                Request.KeepAlive = true;
                Request.ContentType = postDataContentType;
                Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                Request.ContentLength = byteArray.Length;

                dataStream = await Request.GetRequestStreamAsync();
                await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
                dataStream.Close();


                    WebResponse Response = await Request.GetResponseAsync();
                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                    StreamReader Reader = new StreamReader(Response.GetResponseStream());
                    string msg = Reader.ReadToEnd();
                    Console.WriteLine(msg);
                    Reader.Close();
                    return true;
                }
                catch (Exception x)
                {
                    if (x.Message != null) 
                        await LogError(0, "2", x.Message, "", "", "sendTransactionNotificationToCompany");
                    if (x.InnerException != null)
                        if (x.InnerException.Message != null)
                            await LogError(0, "2", x.InnerException.Message, "inner message", "", "sendTransactionNotificationToCompany");
                    return false;
                }
            
        }

        public void Log(string? text)
        {
            /*string filePath = Directory.GetCurrentDirectory() + @"\Log\log.txt";
            if (text != null)
            {
                File.AppendAllText(filePath, text);
            }
            else
                File.AppendAllText(filePath, "nothing");*/
        }

        public async Task<bool> NotifyUserPaymentStatus(string transactionId, bool fromCredit)
        {
            
            {
                if (fromCredit)
                    await LogError(0, "2", "FromCredit : true", "", "", "NotifyUserPaymentStatus");
                else
                    await LogError(0, "2", "FromCredit : false", "", "", "NotifyUserPaymentStatus");

                await LogError(0, "2", "TransactionId : " + transactionId, "", "", "NotifyUserPaymentStatus");

                string salt = Startup.Configuration.GetSection("SyberPay")["ApplicationSalt"].ToString();
                await LogError(0, "2", "Salt : " + salt, "", "", "NotifyUserPaymentStatus");

                string key = Startup.Configuration.GetSection("SyberPay")["ApplicationKey"].ToString();
                await LogError(0, "2", "Key : " + key, "", "", "NotifyUserPaymentStatus");

                string applicationId = Startup.Configuration.GetSection("SyberPay")["ApplicationId"].ToString();
                await LogError(0, "2", "ApplicationId : " + applicationId, "", "", "NotifyUserPaymentStatus");

                string checkPaymentStatusURL = Startup.Configuration.GetSection("SyberPay")["CheckPaymentStatusURL"].ToString();
                await LogError(0, "2", "CheckPaymentStatusURL : " + checkPaymentStatusURL, "", "", "NotifyUserPaymentStatus");

                string textToHash = key + "|" + applicationId + "|" + transactionId + "|" + salt;
                await LogError(0, "2", "Text To Hash : " + textToHash, "", "", "NotifyUserPaymentStatus");

                string hashValue = "";
                using (var sha256 = new SHA256Managed())
                {
                    hashValue = BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(textToHash))).Replace("-", "").ToLower();
                }

                await LogError(0, "2", "Hash Value: " + hashValue, "", "", "NotifyUserPaymentStatus");

                string postDataContentType = "application/json; charset=utf-8";
                string postDataAcceptType = "*/*";
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FirebaseUtils.ValidateServerCertificate);
                HttpWebRequest Request;
                Stream dataStream;
                byte[] byteArray;

                CheckPaymentStatusDto checkPaymentStatusDto = new CheckPaymentStatusDto();
                checkPaymentStatusDto.applicationId = applicationId;
                checkPaymentStatusDto.transactionId = transactionId;
                checkPaymentStatusDto.hash = hashValue;

                byteArray = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(checkPaymentStatusDto));

                Request = (HttpWebRequest)WebRequest.Create(checkPaymentStatusURL);

                Request.Method = "POST";
                Request.KeepAlive = true;
                Request.ContentType = postDataContentType;
                Request.ContentLength = byteArray.Length;
                Request.Accept = postDataAcceptType;

                dataStream = await Request.GetRequestStreamAsync();
                await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
                dataStream.Close();

                bool responseResult = false;

                if (fromCredit)
                {
                    responseResult = true;
                    await _repository.GetUserRepository.UpdateUserRefillStatus(transactionId, responseResult);

                    await sendPaymentStatusNotification(transactionId, responseResult);

                    return responseResult;
                }
                else
                {
                    try
                    {
                        WebResponse Response = await Request.GetResponseAsync();
                        HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                        StreamReader Reader = new StreamReader(Response.GetResponseStream());

                        string msg = Reader.ReadToEnd();
                        await LogError(0, "2", "Response Message: " + msg, "", "", "NotifyUserPaymentStatus");

                        SyberPayPaymentStatusResponse? res = JsonSerializer.Deserialize(msg, typeof(SyberPayPaymentStatusResponse)) as SyberPayPaymentStatusResponse;

                        responseResult = false;

                        if (res != null)
                        {
                            await LogError(0, "2", "Response Message is not NULL", "", "", "NotifyUserPaymentStatus");

                            if (res.payment != null)
                            {
                                await LogError(0, "2", "Payment in response is not NULL", "", "", "NotifyUserPaymentStatus");

                                if (res.payment.status != null)
                                {
                                    await LogError(0, "2", "Status in Payment in response is not null", "", "", "NotifyUserPaymentStatus");
                                    if (res.payment.status.ToLower().StartsWith("success"))
                                        responseResult = true;
                                    await LogError(0, "2", "Status in Payment in response : " + res.payment.status, "", "", "NotifyUserPaymentStatus");
                                }
                                else
                                    await LogError(0, "2", "Status in Payment in response is NULL", "", "", "NotifyUserPaymentStatus");
                            }
                            else
                                await LogError(0, "2", "Payment in response is NULL", "", "", "NotifyUserPaymentStatus");
                        }
                        else
                            await LogError(0, "2", "Response Message is null", "", "", "NotifyUserPaymentStatus");
                        Console.WriteLine(msg);
                        Reader.Close();

                        await _repository.GetUserRepository.UpdateUserRefillStatus(transactionId, responseResult);

                        await sendPaymentStatusNotification(transactionId, responseResult);

                        return responseResult;
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                        return false;
                    }
                }
            }
        }

        public async Task<bool> sendPaymentStatusNotification(string transactionId, bool status)
        {
            string userToken = "";

            UserRefillTransaction userRefillTransaction = await _repository.GetUserRepository.GetSyberpayTransactionById(transactionId);

            userToken = userRefillTransaction.User.FirebaseToken;
            int? language = userRefillTransaction.User.Language;

            string date = userRefillTransaction.Date.ToString("yyyy-MM-dd");
            string time = userRefillTransaction.Date.ToString("hh:mm:ss");
            string typeId = "1";//"3";
            //if (!status)
                //typeId = "4";
            string userId = userRefillTransaction.UserId.ToString();
            string imageURL = "";
            string notes = "";
            string liters = "";
            string material = "";
            string price = userRefillTransaction.Amount.ToString();
            string address = "";
            string userName = userRefillTransaction.User.FirstName;
            string userMobileNo = userRefillTransaction.User.MobileNumber;
            string fastFill = "";

            string paymentGateway = "";

            if ((userRefillTransaction.RefillSourceId == (int)RefillSource.Sybertech) || (userRefillTransaction.RefillSourceId == (int)RefillSource.SybertechApp))
            {
                paymentGateway = "Sybertech";
                if (language != 1)
                    paymentGateway = "سايبر تيك";

            }


            if ((userRefillTransaction.RefillSourceId == (int)RefillSource.Bushrapay))
            {
                paymentGateway = "Bushra Pay";
                if (language != 1)
                    paymentGateway = "بشرى باي";

            }


            if ((userRefillTransaction.RefillSourceId == (int)RefillSource.Faisal))
            {
                paymentGateway = "Fawori App - Faisal Bank";
                if (language != 1)
                    paymentGateway = "تطبيق فوري - بنك فيصل";

            }

            if ((userRefillTransaction.RefillSourceId == (int)RefillSource.BOK))
            {
                paymentGateway = "BOK";
                if (language != 1)
                    paymentGateway = "بنك الخرطوم";

            }

            if ((userRefillTransaction.RefillSourceId == (int)RefillSource.Manual))
            {
                paymentGateway = "Fast Fill";
                if (language != 1)
                    paymentGateway = "فاست فيل";

            }

            string notificationTitle = paymentGateway;
            if (language != 1)
                notificationTitle = paymentGateway;

            string notificationContent = "";

            if (status)
            {
                await LogError(0, "2", "Status is true", "", "", "sendPaymentStatusNotification");
                typeId = "3";
                if (language == 1)
                    notificationContent = userRefillTransaction.Amount.ToString() + " SDG succssfully topped up to your account"+" through " + paymentGateway;
                else
                    notificationContent = userRefillTransaction.Amount.ToString() + " ج.س تم ملئها في المحفظة بنجاح"+ " من خلال "+ paymentGateway;
            }
            else
            {
                await LogError(0, "2", "Status is false", "", "", "sendPaymentStatusNotification");
                typeId = "4";
                if (language == 1)
                    notificationContent = "Could not top up your account with " + userRefillTransaction.Amount.ToString() + " SDG" + " through "+paymentGateway ;
                else
                    notificationContent = "لم استطع ملئ المحفظة بقيمة " + userRefillTransaction.Amount.ToString() + " ج.س"+ " من خلال "+paymentGateway;
            }

            string SenderId = "893991898029";
            string FCMServerKey = Startup.Configuration.GetSection("Firebase")["ServerKey"].ToString(); ;

            string payload = "{" +
                "\"to\":\"" + userToken + "\"," +
                "\"notification\":{" +
                "\"body\":\"" + notificationContent + "\"," +
                "\"content_available\":true," +
                "\"apns-priority\":\"5\"," +
                "\"priority\":\"high\"," +
                "\"subtitle\":\"\"," +
                "\"Title\":\"" + notificationTitle + "\"," +
                "\"sound\":\"default\"," +
                "}," +
                "\"data\":{" +
                                "\"typeId\":\"" + typeId + "\"," +
                                "\"date\":\"" + date + "\"," +
                                "\"time\":\"" + time + "\"," +
                                "\"userId\":\"" + userId + "\"," +
                                "\"imageUrl\":\"" + imageURL + "\"," +
                                "\"notes\":\"" + notes + "\"," +
                                "\"liters\":\"" + liters + "\"," +
                                "\"material\":\"" + material + "\"," +
                                "\"price\":\"" + price + "\"," +
                                "\"address\":\"" + address + "\"," +
                                "\"userName\":\"" + userName + "\"," +
                                "\"userMobileNo\":\"" + userMobileNo + "\"," +
                                "\"transactionId\":\"" + transactionId + "\"," +
                                "\"fastFill\":\"" + fastFill + "\"," +
                                "\"companyId\":\"" + "" + "\"" +
                "}}";

            Notification notification = new Notification();
            notification.Address = address;
            notification.TypeId = typeId;
            notification.Date = date;
            notification.Time = time;
            notification.UserId = userRefillTransaction.UserId;
            notification.ImageURL = imageURL;
            notification.Notes = notes;
            notification.Liters = liters;
            notification.Material = material;
            notification.Price = price;
            notification.Title = notificationTitle;
            notification.Content = notificationContent;

            await AddNotification(notification);

            {
                string postDataContentType = "application/json; charset=utf-8";
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FirebaseUtils.ValidateServerCertificate);
                HttpWebRequest Request;
                Stream dataStream;
                byte[] byteArray;



                byteArray = Encoding.UTF8.GetBytes(payload);
                string SendNotificationURL = Startup.Configuration.GetSection("Firebase")["SendNotificationURL"].ToString();
                Request = (HttpWebRequest)WebRequest.Create(SendNotificationURL);

                Request.Method = "POST";
                Request.KeepAlive = true;
                Request.ContentType = postDataContentType;
                Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                Request.Headers.Add(string.Format("Sender: id={0}", SenderId));
                Request.ContentLength = byteArray.Length;

                dataStream = await Request.GetRequestStreamAsync();
                await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
                dataStream.Close();

                try
                {
                    WebResponse Response = await Request.GetResponseAsync();
                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                    StreamReader Reader = new StreamReader(Response.GetResponseStream());
                    string msg = Reader.ReadToEnd();
                    Console.WriteLine(msg);
                    Reader.Close();
                    return true;
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                    return false;
                }
            }
        }

        public async Task<bool> addAccountRefillSyberPayTransaction(UserRefillTransaction userRefillTransaction)
        {
            try
            {
                return await _repository.GetUserRepository.AddUserRefillTransaction(userRefillTransaction);

            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<double> getUserBalance(int userId)
        {
            try
            {
                return await _repository.GetUserRepository.GetUserBalance(userId);

            }
            catch (DbUpdateConcurrencyException)
            {
                return 0.0;
            }
        }

        public async Task<bool> UpdateUserLanguage(int userId, int language)
        {

            try
            {
                return await _repository.GetUserRepository.UpdateUserLanguage(userId, language);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<bool> LogError(int userId, string type, string message, string innerMessage, string code, string location)
        {

            try
            {
                return await _repository.GetUserRepository.LogError(userId, type, message, innerMessage, code, location);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }
        public async Task<bool> clearNotifications(int userId)
        {
            try
            {
                return await _repository.GetUserRepository.ClearNotifications(userId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> clearTransactions(int userId)
        {
            try
            {
                return await _repository.GetUserRepository.ClearTransactions(userId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> sendCustomNotification(int userId, string title, string content)
        {
            string userToken = "";

            User user = await _repository.GetUserRepository.GetById(userId, 0);

            userToken = user.FirebaseToken;
            string transactionId = "";
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string time = DateTime.Now.ToString("hh:mm:ss");
            string typeId = "1";
            string imageURL = "";
            string notes = "";
            string liters = "";
            string material = "";
            string price = "";
            string address = "";
            string userName = user.FirstName;
            string userMobileNo = user.MobileNumber;
            string fastFill = "";
            string notificationTitle = title;
            string notificationContent = content;


            string SenderId = "893991898029";
            string FCMServerKey = Startup.Configuration.GetSection("Firebase")["ServerKey"].ToString(); ;

            string payload = "{" +
                "\"to\":\"" + userToken + "\"," +
                "\"notification\":{" +
                "\"body\":\"" + notificationContent + "\"," +
                "\"content_available\":true," +
                "\"apns-priority\":\"5\"," +
                "\"priority\":\"high\"," +
                "\"subtitle\":\"\"," +
                "\"Title\":\"" + notificationTitle + "\"," +
                "\"sound\":\"default\"," +
                "}," +
                "\"data\":{" +
                                "\"typeId\":\"" + typeId + "\"," +
                                "\"date\":\"" + date + "\"," +
                                "\"time\":\"" + time + "\"," +
                                "\"userId\":\"" + userId + "\"," +
                                "\"imageUrl\":\"" + imageURL + "\"," +
                                "\"notes\":\"" + notes + "\"," +
                                "\"liters\":\"" + liters + "\"," +
                                "\"material\":\"" + material + "\"," +
                                "\"price\":\"" + price + "\"," +
                                "\"address\":\"" + address + "\"," +
                                "\"userName\":\"" + userName + "\"," +
                                "\"userMobileNo\":\"" + userMobileNo + "\"," +
                                "\"transactionId\":\"" + transactionId + "\"," +
                                "\"fastFill\":\"" + fastFill + "\"," +
                                "\"companyId\":\"" + "" + "\"" +
                "}}";

            Notification notification = new Notification();
            notification.Address = address;
            notification.TypeId = typeId;
            notification.Date = date;
            notification.Time = time;
            notification.UserId = user.Id;
            notification.ImageURL = imageURL;
            notification.Notes = notes;
            notification.Liters = liters;
            notification.Material = material;
            notification.Price = price;
            notification.Title = notificationTitle;
            notification.Content = notificationContent;

            await AddNotification(notification);

            {
                string postDataContentType = "application/json; charset=utf-8";
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FirebaseUtils.ValidateServerCertificate);
                HttpWebRequest Request;
                Stream dataStream;
                byte[] byteArray;



                byteArray = Encoding.UTF8.GetBytes(payload);
                string SendNotificationURL = Startup.Configuration.GetSection("Firebase")["SendNotificationURL"].ToString();
                Request = (HttpWebRequest)WebRequest.Create(SendNotificationURL);

                Request.Method = "POST";
                Request.KeepAlive = true;
                Request.ContentType = postDataContentType;
                Request.Headers.Add(string.Format("Authorization: key={0}", FCMServerKey));
                Request.Headers.Add(string.Format("Sender: id={0}", SenderId));
                Request.ContentLength = byteArray.Length;

                dataStream = await Request.GetRequestStreamAsync();
                await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
                dataStream.Close();

                try
                {
                    WebResponse Response = await Request.GetResponseAsync();
                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                    StreamReader Reader = new StreamReader(Response.GetResponseStream());
                    string msg = Reader.ReadToEnd();
                    Console.WriteLine(msg);
                    Reader.Close();
                    return true;
                }
                catch (Exception err)
                {
                    if (err.Message != null)
                        await LogError(0, "2", err.Message, "", "", "sendCustomNotification");

                    if (err.InnerException != null)
                        if (err.InnerException.Message != null)
                            await LogError(0, "2", err.InnerException.Message, "inner message", "", "sendCustomNotification");

                    Console.WriteLine(err);
                    return false;
                }
            }
        }

        public async Task<bool> sendCustomNotificationToAll(string title, string content)
        {

            List<User> tokens = await _repository.GetUserRepository.GetUsers();

            foreach (var item in tokens)
            {

                await sendCustomNotification(item.Id, title, content);
            }

            return true;

        }

        public async Task<bool> sendCustomNotificationToMultiple(string title, string content, List<string> mobileNumbers)
        {
           
            foreach (var item in mobileNumbers)
            {
                try
                {
                    User token = await _repository.GetUserRepository.GetByMobileNumber(item, 0);
                    if (token != null)
                        await sendCustomNotification(token.Id, title, content);
                }
                catch (Exception xx)
                { 
                    
                }
            }

            return true;
        }


        public async Task<IEnumerable<User>> GetCompaniesUsers(int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<User> users = await _repository.GetUserRepository.GetCompaniesUsers();
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }


        public async Task<TempSetting> ShowSignupInStationApp()
        {
            return await _repository.GetUserRepository.ShowSignupInStationApp();
        }

        public async Task<int> GetUsersCount()
        {
            return await _repository.GetUserRepository.GetUsersCount();
        }

        public async Task<bool> removeAccount(int userId)
        {
            try
            {
                return await _repository.GetUserRepository.RemoveAccount(userId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> logout(int userId)
        {
            try
            {
                return await _repository.GetUserRepository.Logout(userId);
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetCompanyAgents(int companyId, int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<User> users = await _repository.GetUserRepository.GetCompanyAgentUsers(companyId);
            paginationInfo.SetValues(pageSize, page, users.Count());
            return users.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<CompanyPump>> GetCompanyPumps(int companyId, int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<CompanyPump> pumps = await _repository.GetUserRepository.GetCompanyPumps(companyId);
            paginationInfo.SetValues(pageSize, page, pumps.Count());
            return pumps.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<IEnumerable<CompanyAgentPump>> GetPumpsAgents(int companyId, int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<CompanyAgentPump> pumpsAgents = await _repository.GetUserRepository.GetPumpsAgents(companyId);
            paginationInfo.SetValues(pageSize, page, pumpsAgents.Count());
            return pumpsAgents.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public async Task<bool> AssignPumpsAgents(int companyId, List<PumpAgentDto> pumpsAgents)
        {
            return await _repository.GetUserRepository.AssignPumpsAgents(companyId, pumpsAgents);
        }

        public async Task<IEnumerable<CompanyPumpState>> GetActivePumps(int companyId, int page, int pageSize, PaginationInfo paginationInfo)
        {
            IEnumerable<CompanyPumpState> pumps = await _repository.GetUserRepository.GetActivePumps(companyId);
            paginationInfo.SetValues(pageSize, page, pumps.Count());
            return pumps.Skip((page - 1) * pageSize).Take(pageSize);
        }


        public async Task<IEnumerable<UserRefill>> GetRefills(string? mobileNumber, DateTime? fromDate, DateTime? toDate, bool? status, string? transactionId, List<int>? refillSources, 
            int page, int pageSize, PaginationInfo paginationInfo, RefillsTotals refillsTotals)
        {
            IEnumerable<UserRefill> userRefills = await _repository.GetUserRepository.GetRefills(mobileNumber, fromDate, toDate, status, transactionId, refillSources);

            double? totalAmount = userRefills.Sum((x) => x.amount);
            double? totalSuccessAmount = userRefills.Where((x) => x.status == true).Sum((x) => x.amount);
            double? totalFailAmount = userRefills.Where((x) => x.status == false).Sum((x) => x.amount);

            int? totalCount = userRefills.Count();
            int? totalSuccessCount = userRefills.Where((x) => x.status == true).Count();
            int? totalFailCount = userRefills.Where((x) => x.status == false).Count();

            List<RefillSourceAmount> refillSourceAmounts = new List<RefillSourceAmount>();

            double? sourceSuccessAmount = 0;
            double? sourceFailAmount = 0;

            int? sourceSuccessCount = 0;
            int? sourceFailCount = 0;


            //SyberTech App
            sourceSuccessAmount = userRefills.Where((x) => x.status == true && x.sourceId == 0).Sum((x) => x.amount);
            sourceFailAmount = userRefills.Where((x) => x.status == false && x.sourceId == 0).Sum((x) => x.amount);
            sourceSuccessCount = userRefills.Where((x) => x.status == true && x.sourceId == 0).Count();
            sourceFailCount = userRefills.Where((x) => x.status == false && x.sourceId == 0).Count();
            RefillSourceAmount rsa = new RefillSourceAmount();
            rsa.failAmount = sourceFailAmount;
            rsa.failCount = sourceFailCount;
            rsa.successAmount = sourceSuccessAmount;
            rsa.successCount = sourceSuccessCount;
            RefillSourceBody rsb = new RefillSourceBody();
            rsb.Id = 0;
            rsb.ArabicName = "تطبيق سايبرتيك";
            rsb.EnglishName = "Sybertech App";
            rsa.refillSource = rsb;
            if ((sourceSuccessAmount > 0) && (sourceFailAmount > 0) && (sourceSuccessCount > 0) && (sourceFailCount > 0))
                refillSourceAmounts.Add(rsa);


            //SyberTech Gateway
            sourceSuccessAmount = userRefills.Where((x) => x.status == true && x.sourceId == 1).Sum((x) => x.amount);
            sourceFailAmount = userRefills.Where((x) => x.status == false && x.sourceId == 1).Sum((x) => x.amount);
            sourceSuccessCount = userRefills.Where((x) => x.status == true && x.sourceId == 1).Count();
            sourceFailCount = userRefills.Where((x) => x.status == false && x.sourceId == 1).Count();
            rsa = new RefillSourceAmount();
            rsa.failAmount = sourceFailAmount;
            rsa.failCount = sourceFailCount;
            rsa.successAmount = sourceSuccessAmount;
            rsa.successCount = sourceSuccessCount;
            rsb = new RefillSourceBody();
            rsb.Id = 1;
            rsb.ArabicName = "واجهة سايبرتيك";
            rsb.EnglishName = "Sybertech Gateway";
            rsa.refillSource = rsb;
            if ((sourceSuccessAmount > 0) && (sourceFailAmount > 0) && (sourceSuccessCount > 0) && (sourceFailCount > 0))
                refillSourceAmounts.Add(rsa);

            //Bushra
            sourceSuccessAmount = userRefills.Where((x) => x.status == true && x.sourceId == 2).Sum((x) => x.amount);
            sourceFailAmount = userRefills.Where((x) => x.status == false && x.sourceId == 2).Sum((x) => x.amount);
            sourceSuccessCount = userRefills.Where((x) => x.status == true && x.sourceId == 2).Count();
            sourceFailCount = userRefills.Where((x) => x.status == false && x.sourceId == 2).Count();
            rsa = new RefillSourceAmount();
            rsa.failAmount = sourceFailAmount;
            rsa.failCount = sourceFailCount;
            rsa.successAmount = sourceSuccessAmount;
            rsa.successCount = sourceSuccessCount;
            rsb = new RefillSourceBody();
            rsb.Id = 2;
            rsb.ArabicName = "بشرى باي";
            rsb.EnglishName = "Bushra Pay";
            rsa.refillSource = rsb;
            if ((sourceSuccessAmount > 0) && (sourceFailAmount > 0) && (sourceSuccessCount > 0) && (sourceFailCount > 0))
                refillSourceAmounts.Add(rsa);

            //BOK
            sourceSuccessAmount = userRefills.Where((x) => x.status == true && x.sourceId == 3).Sum((x) => x.amount);
            sourceFailAmount = userRefills.Where((x) => x.status == false && x.sourceId == 3).Sum((x) => x.amount);
            sourceSuccessCount = userRefills.Where((x) => x.status == true && x.sourceId == 3).Count();
            sourceFailCount = userRefills.Where((x) => x.status == false && x.sourceId == 3).Count();
            rsa = new RefillSourceAmount();
            rsa.failAmount = sourceFailAmount;
            rsa.failCount = sourceFailCount;
            rsa.successAmount = sourceSuccessAmount;
            rsa.successCount = sourceSuccessCount;
            rsb = new RefillSourceBody();
            rsb.Id = 3;
            rsb.ArabicName = "بنك الخرطوم";
            rsb.EnglishName = "BOK";
            rsa.refillSource = rsb;
            if ((sourceSuccessAmount > 0) && (sourceFailAmount > 0) && (sourceSuccessCount > 0) && (sourceFailCount > 0))
                refillSourceAmounts.Add(rsa);

            //SyberTech Gateway
            sourceSuccessAmount = userRefills.Where((x) => x.status == true && x.sourceId == 4).Sum((x) => x.amount);
            sourceFailAmount = userRefills.Where((x) => x.status == false && x.sourceId == 4).Sum((x) => x.amount);
            sourceSuccessCount = userRefills.Where((x) => x.status == true && x.sourceId == 4).Count();
            sourceFailCount = userRefills.Where((x) => x.status == false && x.sourceId == 4).Count();
            rsa = new RefillSourceAmount();
            rsa.failAmount = sourceFailAmount;
            rsa.failCount = sourceFailCount;
            rsa.successAmount = sourceSuccessAmount;
            rsa.successCount = sourceSuccessCount;
            rsb = new RefillSourceBody();
            rsb.Id = 4;
            rsb.ArabicName = "بنك فيصل";
            rsb.EnglishName = "Faisal Bank";
            rsa.refillSource = rsb;
            if ((sourceSuccessAmount > 0) && (sourceFailAmount > 0) && (sourceSuccessCount > 0) && (sourceFailCount > 0))
                refillSourceAmounts.Add(rsa);

            //Manual
            sourceSuccessAmount = userRefills.Where((x) => x.status == true && x.sourceId == 100).Sum((x) => x.amount);
            sourceFailAmount = userRefills.Where((x) => x.status == false && x.sourceId == 100).Sum((x) => x.amount);
            sourceSuccessCount = userRefills.Where((x) => x.status == true && x.sourceId == 100).Count();
            sourceFailCount = userRefills.Where((x) => x.status == false && x.sourceId == 100).Count();
            rsa = new RefillSourceAmount();
            rsa.failAmount = sourceFailAmount;
            rsa.failCount = sourceFailCount;
            rsa.successAmount = sourceSuccessAmount;
            rsa.successCount = sourceSuccessCount;
            rsb = new RefillSourceBody();
            rsb.Id = 100;
            rsb.ArabicName = "يدوي";
            rsb.EnglishName = "Manual";
            rsa.refillSource = rsb;
            if ((sourceSuccessAmount > 0) && (sourceFailAmount > 0) && (sourceSuccessCount > 0) && (sourceFailCount > 0))
                refillSourceAmounts.Add(rsa);


            refillsTotals.TotalAmount = totalAmount;
            refillsTotals.TotalCount = totalCount;
            refillsTotals.TotalSuccessAmount = totalSuccessAmount;
            refillsTotals.TotalSuccessCount = totalSuccessCount;
            refillsTotals.TotalFailAmount = totalFailAmount;
            refillsTotals.TotalFailCount = totalFailCount;
            refillsTotals.RefillSourceAmounts = refillSourceAmounts;

            paginationInfo.SetValues(10000, page, userRefills.Count());
            return userRefills.Skip((page - 1) * 10000).Take(10000);
        }


        public async Task<IEnumerable<PaymentTransaction>> GetPaymentTransactionResults(string? mobileNumber, DateTime? fromDate, DateTime? toDate, List<int>? companies,
            int page, int pageSize, PaginationInfo paginationInfo, PaymentTransactionResultTotals paymentTransactionResultTotals)
        {
            IEnumerable<PaymentTransaction> paymentTransactionResults = await _repository.GetUserRepository.GetPaymentTransactionResults(mobileNumber, fromDate, toDate, companies);

            double? totalAmount = paymentTransactionResults.Sum((x) => x.Amount-x.Fastfill);
            double? totalFastfill = paymentTransactionResults.Sum((x) => x.Fastfill);
            int? totalCount = paymentTransactionResults.Count();

            paymentTransactionResultTotals.TotalAmount = totalAmount;
            paymentTransactionResultTotals.TotalCount = totalCount;
            paymentTransactionResultTotals.TotalFastfill = totalFastfill;

            paginationInfo.SetValues(10000, page, paymentTransactionResults.Count());
            return paymentTransactionResults.Skip((page - 1) * 10000).Take(10000);
        }


        public async Task<bool> generateOtp(Otp otp)
        {
            string Sender = "FastFill";
            string apiKey = "ZmFzdGZpbGxzdWRhbkBnbWFpbC5jb206a0dVRWdKeiNOaQ==";

            {
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(FirebaseUtils.ValidateServerCertificate);
                HttpWebRequest Request;

                string url = "https://mazinhost.com/smsv1/sms/api?action=send-sms&api_key=ZmFzdGZpbGxzdWRhbkBnbWFpbC5jb206a0dVRWdKeiNOaQ==&to="+otp.mobileNumber+"&from=FastFill&sms=Your OTP Code is: "+otp.otpCode;
                Request = (HttpWebRequest)WebRequest.Create(url);
                try
                {
                    WebResponse Response = await Request.GetResponseAsync();
                    HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
                    StreamReader Reader = new StreamReader(Response.GetResponseStream());
                    string msg = Reader.ReadToEnd();
                    Console.WriteLine(msg);
                    Reader.Close();
                    return await _repository.GetUserRepository.GenerateOtp(otp);
                }
                catch (Exception err)
                {
                    if (err.Message != null)
                        await LogError(0, "2", err.Message, "", "", "generateOtp");

                    if (err.InnerException != null)
                        if (err.InnerException.Message != null)
                            await LogError(0, "2", err.InnerException.Message, "inner message", "", "generateOtp");

                    Console.WriteLine(err);
                    return false;
                }
            }          
        }

        public async Task<bool> verifyOtp(string registerId, string otpCode)
        {

            return await _repository.GetUserRepository.verifyOtp(registerId, otpCode);

        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            IEnumerable<Company> companies = await _repository.GetUserRepository.getAllCompanies();
            return companies;
        }
    }

}
