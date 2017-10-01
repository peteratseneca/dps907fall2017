
// Entity body data, which can be used in Fiddler

// Employee at the top
// Customer further down

var employee5 =
    {
        "EmployeeId": 5,
        "LastName": "Johnson",
        "FirstName": "Steve",
        "Title": "Sales Support Agent",
        "ReportsTo": 2,
        "BirthDate": "1965-03-03T00:00:00",
        "HireDate": "2003-10-17T00:00:00",
        "Address": "7727B 41 Ave",
        "City": "Calgary",
        "State": "AB",
        "Country": "Canada",
        "PostalCode": "T3B 1Y7",
        "Phone": "1 (780) 836-9987",
        "Fax": "1 (780) 836-9543",
        "Email": "steve@chinookcorp.com"
    };

var employee5original = 
    {
        "EmployeeId": 5,
        "Phone": "1 (780) 836-9987",
        "Fax": "1 (780) 836-9543",
        "Email": "steve@chinookcorp.com"
    };

var employee5edited =
    {
        "EmployeeId": 5,
        "Phone": "1 (416) 491-5050",
        "Fax": "1 (416) 491-5055",
        "Email": "steve.johnson@chinook.com"
    };

var employee5editedBadData =
    {
        "EmployeeId": 55,
        "Phone": "1 (416) 491-5050",
        "Fax": "Long string - Video provides a powerful way to help you prove your point. When you click Online Video, you can paste in the embed code for the video you want to add. You can also type a keyword to search online for the video that best fits your document. To make your document look professionally produced, Word provides header, footer, cover page, and text box designs that complement each other. For example, you can add a matching cover page, header, and sidebar. Click Insert and then choose the elements you want from the different galleries.",
        "Email": "steve.johnson -at- gmail.com"
    };

var employeeNew =
    {
        "LastName": "McIntyre",
        "FirstName": "Peter",
        "Title": "Professor",
        "ReportsTo": 2,
        "BirthDate": "1966-08-03T00:00:00",
        "HireDate": "2013-07-17T00:00:00",
        "Address": "70 The Pond Road",
        "City": "Toronto",
        "State": "ON",
        "Country": "Canada",
        "PostalCode": "M3J 3M6",
        "Phone": "1 (416) 967-1111",
        "Fax": "1 (416) 967-1122",
        "Email": "peter@example.com"
    };

var customer44 =
    {
        "CustomerId": 44,
        "FirstName": "Terhi",
        "LastName": "Hämäläinen",
        "Company": null,
        "Address": "Porthaninkatu 9",
        "City": "Helsinki",
        "State": null,
        "Country": "Finland",
        "PostalCode": "00530",
        "Phone": "+358 09 870 2000",
        "Fax": null,
        "Email": "terhi.hamalainen@apple.fi",
        "SupportRepId": 3
    };

var customerNew =
    {
        "FirstName": "Ian",
        "LastName": "Tipson",
        "Company": null,
        "Address": "12 Main Street",
        "City": "Toronto",
        "State": "ON",
        "Country": "Canada",
        "PostalCode": "M3J 3M6",
        "Phone": "1 (416) 491-5050",
        "Fax": "1 (416) 491-5055",
        "Email": "ian@example.com",
        "SupportRepId": 5
    };
