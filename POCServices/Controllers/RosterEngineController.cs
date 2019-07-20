using POCDBAccess;
using POCEntities;
using POCServices.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace POCServices.Controllers
{
    [RoutePrefix("api/rosterengine")]
    public class RosterEngineController : ApiController
    {
        #region constructor
        private readonly IMongoConnect repository;

        public RosterEngineController(IMongoConnect repository)
        {
            this.repository = repository;

        }
        #endregion

        public void GenerateRoster()
        {
            var officersList = repository.GetAll<OfficerProfile>(Constants.OfficersCollection).Result.ToList();

            var top9SCA = officersList.Where(x => x.RoleId.Equals("SCA")).Take(9).ToList();

            var remOfficersList = officersList.Except(top9SCA).Take(12).ToList();

            remOfficersList.AddRange(top9SCA);

            
            var totalOfficer = officersList.Count;
            var dayNumber = 1;
            
            var chineseOfficers = officersList.Where(x => x.LanguagesKnown.Any(y => y.Equals("002"))).Take(totalOfficer / 2).ToList();
            var malayOfficers = officersList.Where(x => x.LanguagesKnown.Any(y => y.Equals("003"))).Take((int)Math.Round(totalOfficer * 0.3)).ToList();
            var tamilOfficers = officersList.Where(x => x.LanguagesKnown.Any(y => y.Equals("004"))).Take((int)Math.Round(totalOfficer * 0.2)).ToList();

            List<OfficerAllocation> officersAllocations = new List<OfficerAllocation>();

            officersAllocations.Add(AddingStandby(dayNumber, chineseOfficers, "1"));
            chineseOfficers.RemoveAt(0);
            officersAllocations.Add(AddingStandby(dayNumber, malayOfficers, "2"));
            malayOfficers.RemoveAt(0);
            officersAllocations.Add(AddingStandby(dayNumber, tamilOfficers, "3"));
            tamilOfficers.RemoveAt(0);

            var careerConnect1Chinese = chineseOfficers.Where(x => x.RoleId.Equals("SCA")).Take(3).ToList();


            
        }



        [NonAction]
        private OfficerAllocation AddingStandby(int dayNumber, List<OfficerProfile> officerList, string CCId)
        {
            OfficerAllocation officerAllocation = new OfficerAllocation();

            var chOfficer = officerList.Take(1).First();

            officerAllocation.WindowsId = chOfficer.WindowsId;
            officerAllocation.DayNumber = dayNumber;
            officerAllocation.CareersConnectId = CCId;
            officerAllocation.DutyName = "Stand-by";
            officerAllocation.RoleId = chOfficer.RoleId;
            officerAllocation.RosterType = "Draft";

            return officerAllocation;
        }
    }
}
