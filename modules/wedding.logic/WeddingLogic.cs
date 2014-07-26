using log4net.logging;
using wedding.logic.POCO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using obj.mapper;

namespace wedding.logic
{
    public enum WPersonType
    {
        Bridesmaids,
        Groomsmen,
        Guest
    }
    public class WeddingLogic : IWeddingLogic
    {
        private const string TAG = "WeddingLogic";
        WeddingContext _dbContext;
        ILogger _logger;
        ObjectMap _mapper = new ObjectMap();
        public WeddingLogic(ILogger logger)
        {
            _dbContext = new WeddingContext();
            _dbContext.Configuration.ProxyCreationEnabled = false;
            _dbContext.Configuration.LazyLoadingEnabled = false;
            _logger = logger;
            _logger.Log.Info("Spawning new db context for weddings");
        }

        #region Gets
        private Wedding GetWedding(string domain)
        {
            try
            {
                return _dbContext.Weddings.SingleOrDefault(c => c.Domain.Equals(domain));
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unexpected error has occurred while getting the wedding information", TAG);
            }
        }

        public WeddingSummary GetWeddingByDomain(string domain)
        {
            try
            {
                var summary = new WeddingSummary();
                var wedding = GetWedding(domain);
                if (wedding != null)
                {
                    summary.Wedding = wedding;
                    summary.Bride = _dbContext.WeddingPersons.SingleOrDefault(c => c.WeddingID == wedding.ID && c.PersonTypeId == 2);
                    summary.Groom = _dbContext.WeddingPersons.SingleOrDefault(c => c.WeddingID == wedding.ID && c.PersonTypeId == 1);
                    return summary;
                }
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unexpected error has occurred while getting the wedding information", TAG);
            }
            throw _logger.GetRaiseException("Unable to find wedding based on domain", TAG);
        }

        public WeddingGuestList GetAllGuests(string domain)
        {
            try
            {
                var glist = new WeddingGuestList();
                var query = from w in _dbContext.Weddings
                            join wp in _dbContext.WeddingPersons on w.ID equals wp.WeddingID
                            join pt in _dbContext.PersonTypes on wp.PersonTypeId equals pt.ID
                            where w.Domain == domain && pt.PersonTypeDesc != "Bride" && pt.PersonTypeDesc != "Groom"
                            select wp;
                var bm = query.Where(c => c.PersonType.PersonTypeDesc.Equals("Best man")).Include(c => c.PersonType);
                glist.BestMen = bm.ToList();

                var gm = query.Where(c => c.PersonType.PersonTypeDesc.Equals("Groomsman")).Include(c => c.PersonType);
                glist.Groomsmen = gm.ToList();

                var moh = query.Where(c => c.PersonType.PersonTypeDesc.Equals("Maid of Honour")).Include(c => c.PersonType);
                glist.MaidOfHonour = moh.ToList();

                var bms = query.Where(c => c.PersonType.PersonTypeDesc.Equals("Bridesmaid")).Include(c => c.PersonType);
                glist.Bridesmaids = bms.ToList();

                var gp = query.Where(c => c.PersonType.PersonTypeDesc.Equals("Parents of Groom")).Include(c => c.PersonType);
                glist.GroomsParents = gp.ToList();

                var gf = query.Where(c => c.PersonType.PersonTypeDesc.Equals("Family of Groom")).Include(c => c.PersonType);
                glist.GroomsFamily = gf.ToList();

                var bp = query.Where(c => c.PersonType.PersonTypeDesc.Equals("Parents of Bride")).Include(c => c.PersonType);
                glist.BridesParents = bp.ToList();

                var bf = query.Where(c => c.PersonType.PersonTypeDesc.Equals("Family of Bride")).Include(c => c.PersonType);
                glist.BridesFamily = bf.ToList();

                var gs = query.Where(c => c.PersonType.PersonTypeDesc.Equals("Friends")).Include(c => c.PersonType);
                glist.Friends = gs.ToList();
                return glist;
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unexpected error has occurred while getting the wedding information", TAG);
            }
            throw _logger.GetRaiseException("Unable to find wedding based on domain", TAG);
        }

        public List<WeddingPerson> GetBridesmaids(string domain)
        {
            try
            {
                var query = from w in _dbContext.Weddings
                            join p in _dbContext.WeddingPersons on w.ID equals p.WeddingID
                            join pt in _dbContext.PersonTypes on p.PersonTypeId equals pt.ID
                            where w.Domain == domain && pt.PersonTypeDesc == "Maid of Honour" || pt.PersonTypeDesc == "Bridesmaid"
                            select p;

                return query.Include(c => c.PersonType).ToList();
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unexpected error has occurred while getting the wedding bridesmaids", TAG);
            }
        }

        public List<WeddingPerson> GetGroomsmen(string domain)
        {
            try
            {
                var query = from w in _dbContext.Weddings
                            join p in _dbContext.WeddingPersons on w.ID equals p.WeddingID
                            join pt in _dbContext.PersonTypes on p.PersonTypeId equals pt.ID
                            where w.Domain == domain && (pt.PersonTypeDesc == "Groomsman" || pt.PersonTypeDesc == "Best man")
                            select p;

                return query.Include(c => c.PersonType).ToList();
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unexpected error has occurred while getting the wedding groomsmen", TAG);
            }
        }

        public List<WeddingPerson> GetGuests(string domain)
        {
            try
            {
                var query = from w in _dbContext.Weddings
                            join c in _dbContext.WeddingPersons on w.ID equals c.WeddingID
                            where w.Domain == domain && (c.PersonType.PersonTypeDesc != "Groomsman" && c.PersonType.PersonTypeDesc != "Best man" &&
                                    c.PersonType.PersonTypeDesc != "Maid of Honour" && c.PersonType.PersonTypeDesc != "Bridesmaid" &&
                                    c.PersonType.PersonTypeDesc != "Bride" && c.PersonType.PersonTypeDesc != "Groom")
                            select c;

                return query.Include(c => c.PersonType).OrderBy(c => c.Name).OrderBy(c => c.Surname).ToList();
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unexpected error has occurred while getting the wedding guest list", TAG);
            }
        }

        public List<PersonType> GetPersonType(WPersonType wType)
        {
            try
            {
                switch (wType)
                {
                    case WPersonType.Bridesmaids:
                        {
                            var query = from pt in _dbContext.PersonTypes
                                        where pt.PersonTypeDesc == "Maid of Honour" || pt.PersonTypeDesc == "Bridesmaid"
                                        select pt;
                            return query.ToList();
                        }
                    case WPersonType.Groomsmen:
                        {
                            var query = from pt in _dbContext.PersonTypes
                                        where pt.PersonTypeDesc == "Groomsman" || pt.PersonTypeDesc == "Best man"
                                        select pt;
                            return query.ToList();
                        }
                    case WPersonType.Guest:
                        {
                            var query = from pt in _dbContext.PersonTypes
                                        where pt.PersonTypeDesc != "Groomsman" && pt.PersonTypeDesc != "Best man"
                                        && pt.PersonTypeDesc != "Maid of Honour" && pt.PersonTypeDesc != "Bridesmaid"
                                        && pt.PersonTypeDesc != "Bride" && pt.PersonTypeDesc != "Groom"
                                        select pt;
                            return query.OrderBy(c => c.PersonTypeDesc).ToList();
                        }
                    default:
                        throw new Exception("No person type has been passed");
                }
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unexpected error has occurred while getting the wedding person types", TAG);
            }
        }

        public List<WeddingPhoto> GetWeddingPhotos(string domain)
        {
            try
            {
                return _dbContext.WeddingPhotos.Where(c => c.Wedding.Domain.Equals(domain)).ToList();
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unexpected error has occurred while getting the wedding photos", TAG);
            }
        }
        #endregion

        #region CustomSaves
        public void SaveBrideGroomInfo(string domain, WeddingSummary brideGroomInfo)
        {
            try
            {
                Trace.WriteLine("Running SaveBrideGroomInfo...", TAG);
                Trace.WriteLine("Getting bride info", TAG);
                var Bride = _dbContext.WeddingPersons.FirstOrDefault(c => c.ID.Equals(brideGroomInfo.Bride.ID));
                Bride.Name = brideGroomInfo.Bride.Name;
                Bride.Surname = brideGroomInfo.Bride.Surname;
                Bride.FacebookLink = brideGroomInfo.Bride.FacebookLink;
                Bride.TwiiterLink = brideGroomInfo.Bride.TwiiterLink;
                Bride.GooglePlus = brideGroomInfo.Bride.GooglePlus;
                Bride.Email = brideGroomInfo.Bride.Email;
                Bride.Cellphone = brideGroomInfo.Bride.Cellphone;
                Bride.Bio = brideGroomInfo.Bride.Bio;
                Bride.ShowFacebook = brideGroomInfo.Bride.ShowFacebook;
                Bride.ShowTwitter = brideGroomInfo.Bride.ShowTwitter;
                Bride.ShowGooglePlus = brideGroomInfo.Bride.ShowGooglePlus;
                Bride.DateModified = DateTime.Now;
                Trace.WriteLine("Getting groom info", TAG);
                var Groom = _dbContext.WeddingPersons.FirstOrDefault(c => c.ID.Equals(brideGroomInfo.Groom.ID));
                Groom.Name = brideGroomInfo.Groom.Name;
                Groom.Surname = brideGroomInfo.Groom.Surname;
                Groom.FacebookLink = brideGroomInfo.Groom.FacebookLink;
                Groom.TwiiterLink = brideGroomInfo.Groom.TwiiterLink;
                Groom.GooglePlus = brideGroomInfo.Groom.GooglePlus;
                Groom.Email = brideGroomInfo.Groom.Email;
                Groom.Cellphone = brideGroomInfo.Groom.Cellphone;
                Groom.Bio = brideGroomInfo.Groom.Bio;
                Groom.ShowFacebook = brideGroomInfo.Groom.ShowFacebook;
                Groom.ShowTwitter = brideGroomInfo.Groom.ShowTwitter;
                Groom.ShowGooglePlus = brideGroomInfo.Groom.ShowGooglePlus;
                Groom.DateModified = DateTime.Now;
                Trace.WriteLine("Trying to update info", TAG);
                _dbContext.SaveChanges();
                Trace.WriteLine("Change committed", TAG);
            }
            catch (Exception ex)
            {
                _logger.SafeException(ex, "Unexpected error occurred while saving the bride and groom information", TAG);
            }
        }

        public void SaveWeddingInfo(string _domain, WeddingSummary weddingSummary)
        {
            try
            {
                var Wedding = GetWedding(_domain);
                Wedding.DateModified = DateTime.Now;
                Wedding.GoogleMapLink = weddingSummary.Wedding.GoogleMapLink;
                Wedding.Directions = weddingSummary.Wedding.Directions;
                Wedding.EventDetails = weddingSummary.Wedding.EventDetails;
                Wedding.WeddingDate = weddingSummary.Wedding.WeddingDate;
                Wedding.OurStory = weddingSummary.Wedding.OurStory;
                Wedding.ShowEvent = weddingSummary.Wedding.ShowEvent;
                Wedding.ShowStory = weddingSummary.Wedding.ShowStory;
                Wedding.ShowDirections = weddingSummary.Wedding.ShowDirections;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.SafeException(ex, "Unexpected error occurred while saving the wedding information", TAG);
            }
        }

        public void SaveWeddingPersons(List<WeddingPerson> weddingPersons, string domain, WPersonType personType)
        {
            try
            {
                if (string.IsNullOrEmpty(domain))
                    _logger.RaiseException("No wedding has been passed for saving these people", TAG);
                var wedding = GetWedding(domain);
                var query = from w in _dbContext.Weddings
                            join p in _dbContext.WeddingPersons on w.ID equals p.WeddingID
                            join pt in _dbContext.PersonTypes on p.PersonTypeId equals pt.ID
                            select p;

                switch (personType)
                {
                    case WPersonType.Bridesmaids:
                        query = query.Where(c => c.PersonType.PersonTypeDesc == "Maid of Honour" || c.PersonType.PersonTypeDesc == "Bridesmaid"); break;
                    case WPersonType.Groomsmen:
                        query = query.Where(c => c.PersonType.PersonTypeDesc == "Groomsman" || c.PersonType.PersonTypeDesc == "Best man"); break;
                    case WPersonType.Guest:
                        query = query.Where(c => c.PersonType.PersonTypeDesc != "Groomsman" && c.PersonType.PersonTypeDesc != "Best man" &&
                                    c.PersonType.PersonTypeDesc != "Maid of Honour" && c.PersonType.PersonTypeDesc != "Bridesmaid" &&
                                    c.PersonType.PersonTypeDesc != "Bride" && c.PersonType.PersonTypeDesc != "Groom"); break;
                    default:
                        break;
                }

                var list = query.ToList();
                foreach (var item in list)
                {
                    // var foundExisting = false;
                    foreach (var updateItem in weddingPersons)
                    {
                        if (item.ID == updateItem.ID)
                        {
                            // foundExisting = true;
                            item.Name = updateItem.Name;
                            item.Surname = updateItem.Surname;
                            item.Email = updateItem.Email;
                            item.Cellphone = updateItem.Cellphone;
                            item.GuestCount = updateItem.GuestCount;
                            item.PersonTypeId = updateItem.PersonType != null ? updateItem.PersonType.ID : updateItem.PersonTypeId;
                            item.FirstGuestName = updateItem.FirstGuestName;
                            item.FirstGuestSurname = updateItem.FirstGuestSurname;
                            item.DateModified = DateTime.Now;
                        }
                    }

                    //if (!foundExisting) //delete existing item
                    //{
                    //    _dbContext.WeddingPersons.Remove(item);
                    //}
                }
                foreach (var updateItem in weddingPersons)
                {
                    var foundNew = true;
                    foreach (var item in list)
                    {
                        if (item.ID == updateItem.ID)
                            foundNew = false;
                    }
                    if (foundNew)
                    {
                        WeddingPerson p = new WeddingPerson()
                        {
                            Name = updateItem.Name,
                            Surname = updateItem.Surname,
                            Email = updateItem.Email,
                            Cellphone = updateItem.Cellphone,
                            PersonTypeId = updateItem.PersonType != null ? updateItem.PersonType.ID : updateItem.PersonTypeId,
                            WeddingID = wedding.ID,
                            GuestCount = updateItem.GuestCount,
                            FirstGuestName = updateItem.FirstGuestName,
                            FirstGuestSurname = updateItem.FirstGuestSurname,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now
                        };
                        _dbContext.WeddingPersons.Add(p);
                    }
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.SafeException(ex, "Unexpected error occurred while saving these people", TAG);
            }
        }
        #endregion

        #region CRUD
        public void DeleteWeddingPerson(WeddingPerson person)
        {
            try
            {
                var dbperson = _dbContext.WeddingPersons.FirstOrDefault(c => c.ID.Equals(person.ID));
                if (dbperson != null)
                {
                    _dbContext.WeddingPersons.Remove(dbperson);
                    _dbContext.SaveChanges();
                    return;
                }

                _logger.RaiseException("Person being deleted could not be found", TAG);
            }
            catch (Exception ex)
            {
                _logger.SafeException(ex, "Unexpected error occurred while deleting this person", TAG);
            }
        }

        public void SavePerson(WeddingPerson weddingPerson, string domain)
        {
            try
            {
                var updatePerson = _dbContext.WeddingPersons.FirstOrDefault(c => c.ID.Equals(weddingPerson.ID));
                if (updatePerson == null)
                {
                    _logger.RaiseException("Person being updated could not be found", TAG);
                }
                ObjectMap.BindObject(weddingPerson, updatePerson);
                weddingPerson.WeddingID = GetWedding(domain).ID;
                updatePerson.PersonTypeId = weddingPerson.PersonType != null ? weddingPerson.PersonType.ID : weddingPerson.PersonTypeId;
                updatePerson.DateModified = DateTime.Now;
                if (_dbContext.SaveChanges() != 1)
                {
                    _logger.RaiseException("More than one person has been affected by the update", TAG);
                }
                return;
            }
            catch (Exception ex)
            {
                _logger.SafeException(ex, "Unexpected error occurred while updating this person", TAG);
            }
        }

        public WeddingPerson AddPerson(WeddingPerson weddingPerson, string domain)
        {
            try
            {
                var insertPerson = new WeddingPerson();

                weddingPerson.WeddingID = GetWedding(domain).ID;
                if (weddingPerson.WeddingID == null)
                {
                    _logger.GetRaiseException("No wedding could be found to map this person to", TAG);
                }
                ObjectMap.BindObject(weddingPerson, insertPerson);
                insertPerson.PersonTypeId = weddingPerson.PersonType != null ? weddingPerson.PersonType.ID : weddingPerson.PersonTypeId;
                insertPerson.DateCreated = DateTime.Now;
                insertPerson.DateModified = DateTime.Now;
                _dbContext.WeddingPersons.Add(insertPerson);
                _dbContext.SaveChanges(); //check that only one item gets inserted into db base
                insertPerson.PersonType = weddingPerson.PersonType;
                return insertPerson;
            }
            catch (Exception ex)
            {
                throw _logger.GetSafeException(ex, "Unexpected error occurred while adding this person", TAG);
            }
        }
        #endregion
    }
}
