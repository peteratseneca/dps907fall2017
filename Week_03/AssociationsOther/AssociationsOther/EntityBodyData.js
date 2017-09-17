
// Entity body data
// Employee, Address, JobDuty

// ########################################
// Employee

var ranjan =
    {
        "Id": 5,
        "LastName": "Bhattacharya",
        "FirstName": "Ranjan"
    };

var peter =
    {
        "Id": 1,
        "LastName": "McIntyre",
        "FirstName": "Peter"
    };

var newEmployee1 =
    {
        "LastName": "Kubba",
        "FirstName": "Nagham"
    };

var newEmployee2 =
    {
        "LastName": "Panesar",
        "FirstName": "Daman"
    };

var setPeterSupervisor =
    {
        "Employee": 1,
        "Supervisor": 5
    };

var setNaghamSupervisor = 
    {
        "Employee": 6,
        "Supervisor": 4
    };

// Set it to whatever the identifier is for the "clerk" job duty
var setDamanJobDuty = 
    {
        "Employee": 1,
        "JobDuty": 5
    };

// ########################################
// Address

var peterAddress =
    {
        "Id": 15,
        "AddressLine1": "1245v1 The Pond Road",
        "AddressLine2": "School of ICT",
        "CityAndProvince": "Toronto, ON",
        "PostalCode": "M3J3M6",
        "EmployeeId": 1
    };

var peterAddressEdited =
    {
        "Id": 15,
        "AddressLine1": "70 The Pond Road",
        "AddressLine2": "School of ICT",
        "CityAndProvince": "Toronto, ON",
        "PostalCode": "M3J3M6"
    };

var newAddress =
    {
        "AddressType": "Work",
        "AddressLine1": "1750 Finch Avenue East",
        "AddressLine2": "School of ICT",
        "CityAndProvince": "Toronto, ON",
        "PostalCode": "M2J2X5",
        "EmployeeId": 2
    };


// ########################################
// JobDuty

var jobDutyProfessor = 
    {
        "Id": 1,
        "Name": "Professor",
        "Description": "Teaches, researches, develops curriculum"
    };

var newJobDuty =
    {
        "Name": "Clerk",
        "Description": "Provides clerical and administrative support for the School's activities"
    };

