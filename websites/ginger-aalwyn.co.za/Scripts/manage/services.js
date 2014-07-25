// Define you service here. Services can be added to same module as 'main' or a seperate module can be created.

var angularStartServices = angular.module('angularStart.services', ['ngResource']);     //Define the services module

angularStartServices.factory('WebApiServce', ["$http", function ($http) {
    return {
        SaveBrideGroom: function (bridegroom) {
            return $http.post("/api/ManageApi/SaveBrideGroom", JSON.stringify(bridegroom));
        },
        SaveWeddingInfo: function (weddingInfo) {
            return $http.post("/api/ManageApi/SaveWeddingInfo", JSON.stringify(weddingInfo));
        },
        GetWeddingInfo: function () {
            return $http.get("/api/ManageApi/GetWeddingInfo");
        },
        GetBridesmaids: function () {
            return $http.get("/api/ManageApi/GetBridesmaids");
        },
        GetBridemaidTypes: function () {
            return $http.get("/api/ManageApi/GetBridemaidTypes");
        },
        SaveBridesmaids: function (bridesmaids) {
            return $http.post("/api/ManageApi/SaveBridesmaids", JSON.stringify(bridesmaids));
        },
        GetGroomsmen: function () {
            return $http.get("/api/ManageApi/GetGroomsmen");
        },
        GetGroomsmenTypes: function () {
            return $http.get("/api/ManageApi/GetGroomsmenTypes");
        },
        SaveGroomsmen: function (groomsmen) {
            return $http.post("/api/ManageApi/SaveGroomsmen", JSON.stringify(groomsmen));
        },
        GetGuests: function () {
            return $http.get("/api/ManageApi/GetGuests");
        },
        GetGuestTypes: function () {
            return $http.get("/api/ManageApi/GetGuestTypes");
        },
        SaveGuests: function (guests) {
            return $http.post("/api/ManageApi/SaveGuests", JSON.stringify(guests));
        },
        GetPhotos: function ()
        {
            return $http.get("/api/ManageApi/GetPhotos");
        },
        SavePhotos: function (photos)
        {
            return $http.post("/api/ManageApi/SaveGuests", JSON.stringify(photos));
        },
        DeletePerson: function (person) {
            return $http.post("/api/ManageApi/DeletePerson", JSON.stringify(person));
        },
        SavePerson: function (person) {
            return $http.post("/api/ManageApi/SavePerson", JSON.stringify(person));
        },
        AddPerson: function (person) {
            return $http.post("/api/ManageApi/AddPerson", JSON.stringify(person));
        }
    }
}]);
