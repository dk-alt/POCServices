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
    [RoutePrefix("rest/api")]
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
                restResponse = Response<List<OfficerProfile>>.ReturnFataErrorResponse();
                restResponse.ResponseData = new List<OfficerProfile>();
                return restResponse;
            }
        }


        /// <summary>
        /// to convert lang and role
        /// </summary>
        /// <param name="officers"></param>
        /// <returns></returns>
        [NonAction]
        private List<OfficerProfile> GetOfficerProfiles(List<OfficerProfile> officers)
        {
            var lanInfo = repository.GetAll<LanguageInfo>(Constants.LanguageCollection).Result.ToList();
            var roleInfo = repository.GetAll<Role>(Constants.RoleandDutyCollection).Result.ToList();
            var result = new List<OfficerProfile>();
            foreach (var item in officers)
            {
                item.LanguagesKnown = GetLanguage(item.LanguagesKnown, lanInfo);
                item.RoleId = GetRole(item.RoleId, roleInfo);
                result.Add(item);
            }


            return result;
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
        [Route("Officer")]
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
                restResponse = Response<OfficerProfile>.ReturnFataErrorResponse();
                restResponse.ResponseData = new OfficerProfile();
                return restResponse;
            }
        }

    }
}
