using DbLogic;
using DbLogic.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MVWedding.Controllers
{
    [Authorize]
    public class ManageApiController : ApiController
    {
        string _domain = "";
        DbLogic.WeddingLogic _context;
        public ManageApiController()
        {

        }
     
        private void Init()
        {
            _domain = this.Request.RequestUri.DnsSafeHost;
            if (this.Request.RequestUri.Port != 80)
                _domain = string.Format("{0}:{1}", this.Request.RequestUri.DnsSafeHost, this.Request.RequestUri.Port.ToString());
            _domain = _domain.Replace("www.", "");
            _domain = _domain.Replace("www", "");
            _context = new DbLogic.WeddingLogic();
        }
        [HttpGet]
        [Authorize]
        public MenuNodes GetMenuItems()
        {

            var list = new MenuNodes();
            list.Nodes = new List<MenuNode>();
            var sitemapsNodes = SiteMap.RootNode.ChildNodes;
            foreach (SiteMapNode item in sitemapsNodes)
            {
                list.Nodes.Add(new MenuNode()
                {
                    Title = item.Title,
                    Description = item.Description,
                    URL = item.Url
                });
            }
            return list;
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


      
    }
}
