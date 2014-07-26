angular.module('main')
    .controller('MainController', ['$scope', 'WebApiServce', function ($scope, WebApiServce) {
        $scope.$emit('SETTITLE', "Manage Bride and Groom");
        $scope.SaveChanges = function () {
            WebApiServce.SaveBrideGroom($scope.WeddingInfo).success(function () {
                $scope.$emit("SUCCESS", { Title: "Content Saved", Body: "Details Saved" });
            }).
                error(function () {
                    $scope.$emit("ERROR", { Title: "Content Not Saved", Body: "Oops details could not be saved!" });
                });
        }
        $scope.WeddingInfo = "";
        this.GetWeddingInfo = function () {
            $scope.$emit("LOAD");
            WebApiServce.GetWeddingInfo().success(function (data) {
                $scope.WeddingInfo = data;
                $scope.$emit("UNLOAD");
            }).
                error(function () {
                    $scope.$emit("UNLOAD");
                    $scope.$emit("ERROR", { Title: "Could Not Load", Body: "Oops details could not be loaded!" });
                });
        }
        this.GetWeddingInfo();
    }]);

angular.module('main')
    .controller('WeddingController', ['$scope', 'WebApiServce', function ($scope, WebApiServce) {
        $scope.$emit('SETTITLE', "Manage Wedding Information");

        $scope.SaveChanges = function () {
            var a = new Date($scope.time);
            a.setHours(a.getHours() + 2);
            $scope.WeddingInfo.Wedding.WeddingDate = new Date($scope.date.toLocaleDateString() + " " + a.toLocaleTimeString());
            WebApiServce.SaveWeddingInfo($scope.WeddingInfo).success(function () {
                $scope.$emit("SUCCESS", { Title: "Content Saved", Body: "Details Saved" });
            }).
                error(function () {
                    $scope.$emit("ERROR", { Title: "Content Not Saved", Body: "Oops details could not be saved!" });
                });
        }

        $scope.DeletePerson = function (person) {
            WebApiServce.DeletePerson(person).success(function () {
                $scope.$emit("SUCCESS", { Title: "Person Removed", Body: "Person removed from wedding" });
            }).error(function () {
                $scope.$emit("ERROR", { Title: "Content Not Saved", Body: "Oops details could not be saved!" });
            });
        }
        $scope.WeddingInfo = "";
        $scope.date = "";
        $scope.time = "";

        this.GetWeddingInfo = function () {
            $scope.$emit("LOAD");
            WebApiServce.GetWeddingInfo().success(function (data) {
                $scope.WeddingInfo = data;
                $scope.date = new Date($scope.WeddingInfo.Wedding.WeddingDate);
                $scope.time = new Date($scope.WeddingInfo.Wedding.WeddingDate);
                $scope.$emit("UNLOAD");
            }).
                error(function () {
                    $scope.$emit("UNLOAD");
                    $scope.$emit("ERROR", { Title: "Could Not Load", Body: "Oops details could not be loaded!" });
                });
        }

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        $scope.clear = function () {
            $scope.date = null;
        };

        this.GetWeddingInfo();

    }]);

angular.module('main')
    .controller('BridesmaideController', ['$scope', 'WebApiServce', function ($scope, WebApiServce) {
        $scope.$emit('SETTITLE', "Manage Guests");
        $scope.SaveChanges = function () {
            WebApiServce.SaveGuests($scope.Guests).success(function (data) {
                $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });
            }).
                error(function () {
                    $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
                });
        }

        $scope.RemoveItem = function (item) {
            if (!confirm("Delete?", "Are you sure you want to remove this item")) return;
            var index = $scope.Guests.indexOf(item);
            $scope.Guests.splice(index, 1);
            if (item.ID)
                WebApiServce.DeletePerson(item).success(function (data) {
                    $scope.$emit("SUCCESS", { Title: "Person Removed", Body: "Person removed from wedding" });
                }).
                 error(function () {
                     $scope.$emit("ERROR", { Title: "Failed to remove", Body: "Oops failed to delete current person!" });
                 });
        }
        $scope.UpdateItem = function (item) {
            console.log(item);
            $scope.GuestCount = item.GuestCount;
            $scope.CurrentGuest = item;
            $scope.updateIndex = $scope.Guests.indexOf(item);
            $scope.isUpdate = true;
        }
        $scope.Reset = function () {
            $scope.CurrentGuest = "";
            $scope.GuestCount = 1;
            $scope.isUpdate = false;
        }
        $scope.isUpdate = false;
        $scope.updateIndex = false;
        $scope.Guests = [];
        $scope.WeddingPersons = [];
        $scope.PersonTypes = [];
        $scope.CurrentGuest = "";
        $scope.GuestCount = 1;

        $scope.AddPerson = function () {
            if (IsValidPerson()) {
                $scope.CurrentGuest.GuestCount = $scope.GuestCount;
                if ($scope.isUpdate) {
                    WebApiServce.SavePerson($scope.CurrentGuest).success(function (data) {
                        $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });
                    }).
               error(function () {
                   $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
               });

                } else {
                    var obj = angular.copy($scope.CurrentGuest);
                    WebApiServce.AddPerson($scope.CurrentGuest).success(function (data) {
                        $scope.Guests.push(data);
                        $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });
                    }).
            error(function () {
                $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
            });
                }
                $scope.CurrentGuest = "";
                $scope.GuestCount = 1;
                $scope.isUpdate = false;
            }
            else {
                $scope.$emit("ERROR", { Title: "Incomplete", Body: "Please complete the person's details above" });
            }
        }
        function IsValidPerson() {
            if (!$scope.CurrentGuest)
                return
            if (!$scope.CurrentGuest.PersonType)
                return false;
            if ($scope.CurrentGuest.Name === "")
                return false;
            if ($scope.CurrentGuest.Surname === "")
                return false;
            return true;
        }
        this.GetBridesmaids = function () {
            $scope.$emit("LOAD");
            WebApiServce.GetBridesmaids().success(function (data) {
                $scope.Guests = data;
                WebApiServce.GetBridemaidTypes().success(function (data) {
                    $scope.PersonTypes = data;
                    $scope.$emit("UNLOAD");
                }).
            error(function () {
                $scope.$emit("UNLOAD");
                $scope.$emit("ERROR", { Title: "Could Not Load", Body: "Oops details could not be loaded!" });
            });
            }).
                error(function () {
                    $scope.$emit("UNLOAD");
                    $scope.$emit("ERROR", { Title: "Could Not Load", Body: "Oops details could not be loaded!" });
                });
        }
        this.GetBridesmaids();

    }]);


angular.module('main')
    .controller('GroomsmenController', ['$scope', 'WebApiServce', function ($scope, WebApiServce) {
        $scope.$emit('SETTITLE', "Manage Guests");
        $scope.SaveChanges = function () {
            WebApiServce.SaveGuests($scope.Guests).success(function (data) {
                $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });
            }).
                error(function () {
                    $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
                });
        }

        $scope.RemoveItem = function (item) {
            if (!confirm("Delete?", "Are you sure you want to remove this item")) return;
            var index = $scope.Guests.indexOf(item);
            $scope.Guests.splice(index, 1);
            if (item.ID)
                WebApiServce.DeletePerson(item).success(function (data) {
                    $scope.$emit("SUCCESS", { Title: "Person Removed", Body: "Person removed from wedding" });
                }).
                 error(function () {
                     $scope.$emit("ERROR", { Title: "Failed to remove", Body: "Oops failed to delete current person!" });
                 });
        }
        $scope.UpdateItem = function (item) {
            console.log(item);
            $scope.GuestCount = item.GuestCount;
            $scope.CurrentGuest = item;
            $scope.updateIndex = $scope.Guests.indexOf(item);
            $scope.isUpdate = true;
        }
        $scope.Reset = function () {
            $scope.CurrentGuest = "";
            $scope.GuestCount = 1;
            $scope.isUpdate = false;
        }
        $scope.isUpdate = false;
        $scope.updateIndex = false;
        $scope.Guests = [];
        $scope.WeddingPersons = [];
        $scope.PersonTypes = [];
        $scope.CurrentGuest = "";
        $scope.GuestCount = 1;

        $scope.AddPerson = function () {
            if (IsValidPerson()) {
                $scope.CurrentGuest.GuestCount = $scope.GuestCount;
                if ($scope.isUpdate) {
                    WebApiServce.SavePerson($scope.CurrentGuest).success(function (data) {
                        $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });
                    }).
               error(function () {
                   $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
               });

                } else {
                    var obj = angular.copy($scope.CurrentGuest);
                    WebApiServce.AddPerson($scope.CurrentGuest).success(function (data) {
                        $scope.Guests.push(data);
                        $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });
                    }).
            error(function () {
                $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
            });
                }
                $scope.CurrentGuest = "";
                $scope.GuestCount = 1;
                $scope.isUpdate = false;
            }
            else {
                $scope.$emit("ERROR", { Title: "Incomplete", Body: "Please complete the person's details above" });
            }
        }
        function IsValidPerson() {
            if (!$scope.CurrentGuest)
                return
            if (!$scope.CurrentGuest.PersonType)
                return false;
            if ($scope.CurrentGuest.Name === "")
                return false;
            if ($scope.CurrentGuest.Surname === "")
                return false;
            return true;
        }
        this.GetGroomsmen = function () {
            $scope.$emit("LOAD");
            WebApiServce.GetGroomsmen().success(function (data) {
                $scope.Guests = data;
                WebApiServce.GetGroomsmenTypes().success(function (data) {
                    $scope.PersonTypes = data;
                    $scope.$emit("UNLOAD");
                }).
            error(function () {
                $scope.$emit("UNLOAD");
                $scope.$emit("ERROR", { Title: "Could Not Load", Body: "Oops details could not be loaded!" });
            });
            }).
                error(function () {
                    $scope.$emit("UNLOAD");
                    $scope.$emit("ERROR", { Title: "Could Not Load", Body: "Oops details could not be loaded!" });
                });
        }
        this.GetGroomsmen();

    }]);

angular.module('main')
    .controller('GuestController', ['$scope', 'WebApiServce', function ($scope, WebApiServce) {
        $scope.$emit('SETTITLE', "Manage Guests");
        $scope.SaveChanges = function () {
            WebApiServce.SaveGuests($scope.Guests).success(function (data) {
                $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });
            }).
                error(function () {
                    $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
                });
        }

        $scope.RemoveItem = function (item) {
            if (!confirm("Delete?", "Are you sure you want to remove this item")) return;
            var index = $scope.Guests.indexOf(item);
            $scope.Guests.splice(index, 1);
            if (item.ID)
                WebApiServce.DeletePerson(item).success(function (data) {
                    $scope.$emit("SUCCESS", { Title: "Person Removed", Body: "Person removed from wedding" });
                }).
                 error(function () {
                     $scope.$emit("ERROR", { Title: "Failed to remove", Body: "Oops failed to delete current person!" });
                 });
        }
        $scope.UpdateItem = function (item) {
            console.log(item);
            $scope.GuestCount = item.GuestCount;
            $scope.CurrentGuest = item;
            $scope.updateIndex = $scope.Guests.indexOf(item);
            $scope.isUpdate = true;
        }
        $scope.Reset = function () {
            $scope.CurrentGuest = "";
            $scope.GuestCount = 1;
            $scope.isUpdate = false;
        }
        $scope.isUpdate = false;
        $scope.updateIndex = false;
        $scope.Guests = [];
        $scope.WeddingPersons = [];
        $scope.PersonTypes = [];
        $scope.CurrentGuest = "";
        $scope.GuestCount = 1;
     
        $scope.AddPerson = function () {
            if (IsValidPerson()) {
                $scope.CurrentGuest.GuestCount = $scope.GuestCount;
                if ($scope.isUpdate) {
                    WebApiServce.SavePerson($scope.CurrentGuest).success(function (data) {
                        $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });
                    }).
               error(function () {
                   $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
               });

                } else {
                    var obj = angular.copy($scope.CurrentGuest);
                    WebApiServce.AddPerson($scope.CurrentGuest).success(function (data) {
                        $scope.Guests.push(data);
                        $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });
                    }).
            error(function () {
                $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
            });
                }
                $scope.CurrentGuest = "";
                $scope.GuestCount = 1;
                $scope.isUpdate = false;
            }
            else {
                $scope.$emit("ERROR", { Title: "Incomplete", Body: "Please complete the person's details above" });
            }
        }
        function IsValidPerson() {
            if (!$scope.CurrentGuest)
                return
            if (!$scope.CurrentGuest.PersonType)
                return false;
            if ($scope.CurrentGuest.Name === "")
                return false;
            if ($scope.CurrentGuest.Surname === "")
                return false;
            return true;
        }
        this.GetGuests = function () {
            $scope.$emit("LOAD");
            WebApiServce.GetGuests().success(function (data) {
                $scope.Guests = data;
                WebApiServce.GetGuestTypes().success(function (data) {
                    $scope.PersonTypes = data;
                    $scope.$emit("UNLOAD");
                }).
            error(function () {
                $scope.$emit("UNLOAD");
                $scope.$emit("ERROR", { Title: "Could Not Load", Body: "Oops details could not be loaded!" });
            });
            }).
                error(function () {
                    $scope.$emit("UNLOAD");
                    $scope.$emit("ERROR", { Title: "Could Not Load", Body: "Oops details could not be loaded!" });
                });
        }
        this.GetGuests();

    }]);

angular.module('main')
    .controller('PhotoController', ['$scope', 'WebApiServce', function ($scope, WebApiServce) {
        $scope.$emit('SETTITLE', "Manage Photos");
        $scope.SaveChanges = function () {
            WebApiServce.SaveGuests($scope.Guests).success(function (data) {
                $scope.$emit("SUCCESS", { Title: "Details updated", Body: "Details has been updated" });

            }).
                error(function () {
                    $scope.$emit("ERROR", { Title: "Failed to save", Body: "Oops failed to update data!" });
                });
        }
        $scope.PhotoList = [];
        $scope.CurrentPhoto = "";
        this.GetPhotos = function () {
            $scope.$emit("LOAD");
            WebApiServce.GetPhotos().success(function (data) {
                $scope.PhotoList = data;
                $scope.$emit("UNLOAD");
            }).error(function () {
                $scope.$emit("UNLOAD");
                $scope.$emit("ERROR", { Title: "Could Not Load", Body: "Oops details could not be loaded!" });
            });
        }
        this.GetPhotos();

    }]);
