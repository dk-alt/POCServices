using POCDBAccess;
using POCEntities;
using POCLogger;
using POCServices.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace POCServices.Controllers
{
    [RoutePrefix("api")]
    public class UserController : ApiController
    {
        #region constructor
        private readonly IMongoConnect repository;

        public UserController(IMongoConnect repository)
        {
            this.repository = repository;

        }
        #endregion


        /// <summary>
        /// To get officers list
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [Route("officers/{skip}/{limit}")]
        [HttpGet]
        [LogActionFilter]
        public RestResponse<List<OfficerProfile>> GetOficers(int skip, int limit)
        {
            RestResponse<List<OfficerProfile>> restResponse = null;
            try
            {
                var result = repository.Get<OfficerProfile>(Constants.OfficersCollection, x => x.OfficerId != null, skip, limit, "FirstName").Result.ToList();

                restResponse = Response<List<OfficerProfile>>.ReturnSuccessResponse();
                restResponse.ResponseData = GetOfficerProfiles(result);
                return restResponse;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                restResponse = Response<List<OfficerProfile>>.ReturnFataErrorResponse();
                restResponse.ResponseData = new List<OfficerProfile>();
                return restResponse;
            }
        }


        /// <summary>
        /// get officer based on window id
        /// </summary>
        /// <returns></returns>
        [Route("officer")]
        [HttpGet]
        [LogActionFilter]
        public RestResponse<OfficerProfileEdit> GetOficer(string windowsId)
        {
            RestResponse<OfficerProfileEdit> restResponse = null;
            try
            {
                var result = repository.Find<OfficerProfile>(Constants.OfficersCollection, x => x.WindowsId.Equals(windowsId) ).Result.ToList();

                restResponse = Response<OfficerProfileEdit>.ReturnSuccessResponse();
                restResponse.ResponseData = GetOfficerProfileEdit(result.First());
                return restResponse;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                restResponse = Response<OfficerProfileEdit>.ReturnFataErrorResponse();
                restResponse.ResponseData = null;
                return restResponse;
            }
        }


        /// <summary>
        /// to get role and duty for drop down
        /// </summary>
        /// <returns></returns>
        [Route("roleandduty")]
        [HttpGet]
        [LogActionFilter]
        public RestResponse<List<Role>> Getroleandduty()
        {
            RestResponse<List<Role>> restResponse = null;
            try
            {
                var result = repository.GetAll<Role>(Constants.RoleandDutyCollection).Result.ToList();

                restResponse = Response<List<Role>>.ReturnSuccessResponse();
                restResponse.ResponseData = result;
                return restResponse;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                restResponse = Response<List<Role>>.ReturnFataErrorResponse();
                restResponse.ResponseData = new List<Role>();
                return restResponse;
            }
        }


        /// <summary>
        /// to get language for dropdown
        /// </summary>
        /// <returns></returns>
        [Route("language")]
        [HttpGet]
        [LogActionFilter]
        public RestResponse<List<LanguageInfo>> Getlanguage()
        {
            RestResponse<List<LanguageInfo>> restResponse = null;
            try
            {
                var result = repository.GetAll<LanguageInfo>(Constants.LanguageCollection).Result.ToList();

                restResponse = Response<List<LanguageInfo>>.ReturnSuccessResponse();
                restResponse.ResponseData = result;
                return restResponse;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                restResponse = Response<List<LanguageInfo>>.ReturnFataErrorResponse();
                restResponse.ResponseData = new List<LanguageInfo>();
                return restResponse;
            }
        }


        /// <summary>
        /// to get specialization for dropdown
        /// </summary>
        /// <returns></returns>
        [Route("specialization")]
        [HttpGet]
        [LogActionFilter]
        public RestResponse<List<Specialization>> GetSpecialization()
        {
            RestResponse<List<Specialization>> restResponse = null;
            try
            {
                var result = repository.GetAll<Specialization>(Constants.SpecializationCollection).Result.ToList();

                restResponse = Response<List<Specialization>>.ReturnSuccessResponse();
                restResponse.ResponseData = result;
                return restResponse;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                restResponse = Response<List<Specialization>>.ReturnFataErrorResponse();
                restResponse.ResponseData = new List<Specialization>();
                return restResponse;
            }
        }

        /// <summary>
        /// add officers to collections
        /// </summary>
        /// <param name="officerDetail"></param>
        /// <returns></returns>
        [Route("Officer/Add")]
        [HttpPost]
        [LogActionFilter]
        public RestResponse<OfficerProfile> AddOfficer(OfficerProfile officerDetail)
        {
            RestResponse<OfficerProfile> restResponse = null;
            try
            {
                var latestRecord = repository.CollectionCount<OfficerProfile>(Constants.OfficersCollection, x => x.OfficerId != null).Result;

                var officersID = Constants.OfficerIdPrefix + (latestRecord + 1).ToString("D" + 4);

                officerDetail.OfficerId = officersID;
                var result = repository.InsertOne(Constants.OfficersCollection, officerDetail);

                if (result.Exception == null)
                {
                    restResponse = Response<OfficerProfile>.ReturnSuccessResponse();
                    restResponse.ResponseData = officerDetail;
                }
                else
                {
                    restResponse = Response<OfficerProfile>.ReturnFataErrorResponse();
                    restResponse.ResponseData = new OfficerProfile();
                }
                return restResponse;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                restResponse = Response<OfficerProfile>.ReturnFataErrorResponse();
                restResponse.ResponseData = new OfficerProfile();
                return restResponse;
            }
        }


        [Route("Officer/Update")]
        [HttpPut]
        [LogActionFilter]
        public RestResponse<OfficerProfile> UpdateOfficer(OfficerProfile officerDetail)
        {
            RestResponse<OfficerProfile> restResponse = null;
            try
            {
                
                var result = repository.FindOne<OfficerProfile>(Constants.OfficersCollection, x=>x.OfficerId.Equals(officerDetail.OfficerId),"OfficerId").Result;
                officerDetail._id = result._id;

                var result1 = repository.FindOneAndReplace<OfficerProfile>(Constants.OfficersCollection, x => x.OfficerId.Equals(officerDetail.OfficerId), officerDetail);
                if (result1.Exception == null)
                {
                    restResponse = Response<OfficerProfile>.ReturnSuccessResponse();
                    restResponse.ResponseData = officerDetail;
                }
                else
                {
                    restResponse = Response<OfficerProfile>.ReturnFataErrorResponse();
                    restResponse.ResponseData = new OfficerProfile();
                }
                return restResponse;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                restResponse = Response<OfficerProfile>.ReturnFataErrorResponse();
                restResponse.ResponseData = new OfficerProfile();
                return restResponse;
            }
        }

        #region Non Action Methods

        /// <summary>
        /// Convert to officer profile for edit
        /// </summary>
        /// <param name="officers"></param>
        /// <returns></returns>
        [NonAction]
        private OfficerProfileEdit GetOfficerProfileEdit(OfficerProfile officers)
        {
            try
            {
                var lanInfo = repository.GetAll<LanguageInfo>(Constants.LanguageCollection).Result.ToList();
                var specializationInfo = repository.GetAll<Specialization>(Constants.SpecializationCollection).Result.ToList();

                OfficerProfileEdit result = new OfficerProfileEdit
                {
                    WindowsId = officers.WindowsId,
                    Specialization = GetSppecializationInfo(officers.Specialization, specializationInfo),
                    FirstName = officers.FirstName,
                    IsRosterAdministrator = officers.IsRosterAdministrator,
                    LanguagesKnown = GetLanguageInfo(officers.LanguagesKnown, lanInfo),
                    LastName = officers.LastName,
                    OfficerId = officers.OfficerId,
                    RoleId = officers.RoleId,
                    Sex = officers.Sex
                };

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return new OfficerProfileEdit();
            }
        }



        /// <summary>
        /// to convert lang and role
        /// </summary>
        /// <param name="officers"></param>
        /// <returns></returns>
        [NonAction]
        public List<OfficerProfile> GetOfficerProfiles(List<OfficerProfile> officers)
        {
            var result = new List<OfficerProfile>();
            try
            {
                var lanInfo = repository.GetAll<LanguageInfo>(Constants.LanguageCollection).Result.ToList();
                var roleInfo = repository.GetAll<Role>(Constants.RoleandDutyCollection).Result.ToList();

                foreach (var item in officers)
                {
                    item.LanguagesKnown = GetLanguage(item.LanguagesKnown, lanInfo);
                    item.RoleId = GetRole(item.RoleId, roleInfo);
                    result.Add(item);
                }


                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return result;
            }
        }

        /// <summary>
        /// to convert language
        /// </summary>
        /// <param name="lanCode"></param>
        /// <param name="lanInfo"></param>
        /// <returns></returns>
        [NonAction]
        private List<string> GetLanguage(List<string> lanCode, List<LanguageInfo> lanInfo)
        {
            List<string> result = new List<string>();

            foreach (var item in lanCode)
            {
                try

                {
                    var match = lanInfo.FirstOrDefault(x => x.LanguageId.Equals(item)).LanguageName;

                    result.Add(match);
                }
                catch (Exception ex)
                {
                    Logger.Error("while converting language", ex);
                    result.Add(item);
                }
            }


            return result;
        }


        /// <summary>
        /// get language whole structure
        /// </summary>
        /// <param name="lanCode"></param>
        /// <param name="lanInfo"></param>
        /// <returns></returns>
        [NonAction]
        private List<LanguageInfo> GetLanguageInfo(List<string> lanCode, List<LanguageInfo> lanInfo)
        {
            List<LanguageInfo> result = new List<LanguageInfo>();

            foreach (var item in lanCode)
            {
                try

                {
                    var match = lanInfo.FirstOrDefault(x => x.LanguageId.Equals(item));

                    result.Add(match);
                }
                catch (Exception ex)
                {
                    Logger.Error("while converting language", ex);
                    return new List<LanguageInfo>();
                }
            }


            return result;
        }


        /// <summary>
        /// get specializatino info
        /// </summary>
        /// <param name="specialCode"></param>
        /// <param name="lanInfo"></param>
        /// <returns></returns>
        [NonAction]
        private List<Specialization> GetSppecializationInfo(List<string> specialCode, List<Specialization> lanInfo)
        {
            List<Specialization> result = new List<Specialization>();

            foreach (var item in specialCode)
            {
                try

                {
                    var match = lanInfo.FirstOrDefault(x => x.SpecializationId.Equals(item));

                    result.Add(match);
                }
                catch (Exception ex)
                {
                    Logger.Error("while converting language", ex);
                    return new List<Specialization>();
                }
            }

            return result;
        }



        /// <summary>
        /// get role name
        /// </summary>
        /// <param name="role"></param>
        /// <param name="roleInfo"></param>
        /// <returns></returns>
        [NonAction]
        private string GetRole(string role, List<Role> roleInfo)
        {

            try
            {
                var match = roleInfo.FirstOrDefault(x => x.RoleId.Equals(role)).RoleName;
                return match;
            }
            catch (Exception ex)
            {
                Logger.Error("while converting language", ex);
                return role;
            }



        }


        #endregion
    }
}
