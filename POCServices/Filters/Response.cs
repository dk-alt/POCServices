using POCEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POCServices.Filters
{
    public static class Response<R>
    {

       
            #region Success Response Post
            public static RestResponse<R> ReturnSuccessResponse()
            {
                RestResponse<R> response;

                response = new RestResponse<R>
                {
                    Header = new Header
                    {
                        StatusCode = "200",
                        Message = "Success"
                    }
                };
                return response;
            }
            #endregion

            #region InValid Data Response
            public static RestResponse<R> ReturnInvalidDataResponse()
            {
                RestResponse<R> response = null;

                response = new RestResponse<R>
                {
                    Header = new Header
                    {
                        StatusCode = "204",
                        Message = "InValid Data Response"
                    }
                };
                return response;
            }
            #endregion

            #region Fatal Error Response
            public static RestResponse<R> ReturnFataErrorResponse()
            {
                RestResponse<R> response;

                response = new RestResponse<R>
                {
                    Header = new Header
                    {
                        StatusCode = "500",
                        Message = "Fatal Error Response"
                    }
                };
                return response;
            }
            #endregion

            #region No Record
            public static RestResponse<R> ReturnNoRecordResponse()
            {
                RestResponse<R> response;

                response = new RestResponse<R>
                {
                    Header = new Header
                    {
                        StatusCode = "204",
                        Message = "No Record"
                    }
                };
                return response;
            }
            #endregion

            #region CustomResponse
            public static RestResponse<R> CustomResponse(string exString)
            {
                RestResponse<R> response;

                response = new RestResponse<R>
                {
                    Header = new Header
                    {
                        StatusCode = "502",
                        Message = exString
                    }
                };
                return response;
            }
            #endregion



            public static RestResponse<R> AuthenticationFailed(string exString)
            {
                RestResponse<R> response;

                response = new RestResponse<R>
                {
                    Header = new Header
                    {
                        StatusCode = "401",
                        Message = "Success"
                    }
                };
                return response;
            }

        }

    
}