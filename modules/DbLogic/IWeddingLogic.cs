using DbLogic.POCO;
using System;
using System.Collections.Generic;
namespace DbLogic
{
    public interface IWeddingLogic
    {
        void DeleteWeddingPerson(WeddingPerson person);
        WeddingGuestList GetAllGuests(string domain);
        List<WeddingPerson> GetBridesmaids(string domain);
        List<WeddingPerson> GetGroomsmen(string domain);
        List<WeddingPerson> GetGuests(string domain);
        List<PersonType> GetPersonType(WPersonType wType);
        WeddingSummary GetWeddingByDomain(string domain);
        List<WeddingPhoto> GetWeddingPhotos(string domain);
        void SaveBrideGroomInfo(string domain, WeddingSummary brideGroomInfo);
        void SaveWeddingInfo(string _domain, WeddingSummary weddingSummary);
        void SaveWeddingPersons(List<WeddingPerson> weddingPersons, string domain, WPersonType personType);
    }
}
