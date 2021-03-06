﻿using log4net.logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using wedding.logic;
using wedding.logic.POCO;

namespace ginger.aalwyn.co.za.Controllers
{
    [Authorize]
    public class ManageApiController : ApiController
    {
        string _domain = "";
        IWeddingLogic _context;
        ILogger _logger;


        public ManageApiController(IWeddingLogic context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
     
        private void Init()
        {
            _domain = this.Request.RequestUri.DnsSafeHost;
            if (this.Request.RequestUri.Port != 80)
                _domain = string.Format("{0}:{1}", this.Request.RequestUri.DnsSafeHost, this.Request.RequestUri.Port.ToString());
            _domain = _domain.Replace("www.", "");
            _domain = _domain.Replace("www", "");
        }

        [HttpGet]
        public WeddingSummary GetWeddingInfo()
        {
            Init();
            return _context.GetWeddingByDomain(_domain);
        }

        [HttpGet]
        public List<WeddingPerson> GetBridesmaids()
        {
            Init();
            return _context.GetBridesmaids(_domain);
        }

        [HttpGet]
        public List<WeddingPerson> GetGroomsmen()
        {
            Init();
            return _context.GetGroomsmen(_domain);
        }

        [HttpGet]
        public List<WeddingPerson> GetGuests()
        {
            Init();
            return _context.GetGuests(_domain);
        }

        [HttpGet]
        public List<PersonType> GetBridemaidTypes()
        {
            Init();
            return _context.GetPersonType(WPersonType.Bridesmaids);
        }

        [HttpGet]
        public List<PersonType> GetGroomsmenTypes()
        {
            Init();
            return _context.GetPersonType(WPersonType.Groomsmen);
        }

        [HttpGet]
        public List<PersonType> GetGuestTypes()
        {
            Init();
            return _context.GetPersonType(WPersonType.Guest);
        }

        [HttpGet]
        public List<WeddingPhoto> GetPhotos()
        {
            Init();
            return _context.GetWeddingPhotos(_domain);
        }

        [HttpPost]
        public void SaveBrideGroom(WeddingSummary weddingSummary)
        {
            Init();
            _context.SaveBrideGroomInfo(_domain, weddingSummary);
        }

        [HttpPost]
        public void SaveWeddingInfo(WeddingSummary weddingSummary)
        {
            Init();
            _context.SaveWeddingInfo(_domain, weddingSummary);
        }

        [HttpPost]
        public void SaveBridesmaids(List<WeddingPerson> weddingPersons)
        {
            Init();
            _context.SaveWeddingPersons(weddingPersons, _domain, WPersonType.Bridesmaids);
        }

        [HttpPost]
        public void SaveGroomsmen(List<WeddingPerson> groomsmen)
        {
            Init();
            _context.SaveWeddingPersons(groomsmen, _domain, WPersonType.Groomsmen);
        }

        [HttpPost]
        public void SaveGuests(List<WeddingPerson> guests)
        {
            Init();
            _context.SaveWeddingPersons(guests, _domain, WPersonType.Guest);
        }

        [HttpPost]
        public void SavePerson(WeddingPerson weddingPerson)
        {
            Init();
            _context.SavePerson(weddingPerson, _domain);
        }

        [HttpPost]
        public WeddingPerson AddPerson(WeddingPerson weddingPerson)
        {
            Init();
            return  _context.AddPerson(weddingPerson, _domain);
        }

        [HttpPost]
        public void DeletePerson(WeddingPerson person)
        {
            Init();
            _context.DeleteWeddingPerson(person);
        }

        public WeddingSetting GetWeddingSetting()
        {
            Init();
            var setting = _context.GetWeddingSettings(_domain);
            return setting;
        }

        [HttpPost]
        public WeddingSetting SaveWeddingSetting(WeddingSetting setting)
        {
            Init();
            return _context.UpdateSetting(setting, _domain);
        }
    }
}
