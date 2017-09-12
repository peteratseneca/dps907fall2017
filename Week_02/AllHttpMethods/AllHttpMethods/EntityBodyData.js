
// Entity body data

// Copy the JSON and paste it into Fiddler

var originalItem =
    {
        "EmployeeId": 8,
        "LastName": "Callahan",
        "FirstName": "Laura",
        "Title": "IT Staff",
        "ReportsTo": 6,
        "BirthDate": "1968-01-09T00:00:00",
        "HireDate": "2004-03-04T00:00:00",
        "Address": "923 7 ST NW",
        "City": "Lethbridge",
        "State": "AB",
        "Country": "Canada",
        "PostalCode": "T1H 1Y8",
        "Phone": "+1 (403) 467-3351",
        "Fax": "+1 (403) 467-8772",
        "Email": "laura@chinookcorp.com"
    }

var addedItem =
    {
        "LastName": "McIntyre",
        "FirstName": "Peter",
        "Title": "Professor",
        "BirthDate": "1986-05-09T00:00:00",
        "HireDate": "2014-03-04T00:00:00",
        "Address": "70 The Pond Road",
        "City": "Toronto",
        "State": "ON",
        "Country": "Canada",
        "PostalCode": "M3J 3M6",
        "Phone": "+1 (416) 491-5050",
        "Fax": "+1 (416) 491-5055",
        "Email": "peter@example.com"
    }

var editedItem = 
    {
        "EmployeeId": 0, // Change this value!!!
        "Phone": "+1 (416) 491-6060",
        "Fax": "+1 (416) 491-6066",
        "Email": "peter.mcintyre@example.com"
    }
