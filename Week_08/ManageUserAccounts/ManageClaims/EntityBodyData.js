
// Entity body data

/*

Create new user account
=======================

POST to the URI /api/account/register

Can send either JSON or HTML Forms data...

Content-Type: application/x-www-form-urlencoded

Email=admin@example.com&Password=Password123!&ConfirmPassword=Password123!&GivenName=Application&Surname=Administrator&Roles=Admin
Password=Password123!&ConfirmPassword=Password123!&Email=student3@example.com&GivenName=Student&Surname=Three&Roles=Student
ConfirmPassword=Password123!&Password=Password123!&Email=student9@example.com&GivenName=Student&Surname=Nine&Roles=Student&Roles=Mentor

With custom claims...
Custom claim format is Type=Value, but we cannot send extra equals signs...
So, replace the equal sign in between Type=Value with %3d
Password=Password123!&ConfirmPassword=Password123!&Email=student1@example.com&GivenName=Student&Surname=One&Roles=Student&CustomClaims=OU%3dSICT&CustomClaims=OU%3dFASET&CustomClaims=Campus%3dYork&CustomClaims=Task%3dSMILE%20Mentor

Content-Type: application/json

{"Password":"Password123!","ConfirmPassword":"Password123!","Email":"student3@example.com"}


Request access token
====================

POST to the URI /token

Must send HTML Forms data...

Content-Type: application/x-www-form-urlencoded

grant_type=password&username=student8@example.com&password=Password123!
grant_type=password&username=admin@example.com&password=Password123!

*/

var ac1 =
    {
        "ClaimType": "OU",
        "ClaimValue": "FASET",
        "Description": "Faculty of Applied Science and Engineering Technology"
    }

var ac2 = 
    {
        "ClaimType": "OU",
        "ClaimValue": "SICT",
        "Description": "School of Information and Communications Technology"
    }

var ac3 = 
    {
        "ClaimType": "OU",
        "ClaimValue": "CPA",
        "Description": "CPA academic program"
    }

var ac4 = 
    {
        "ClaimType": "Role",
        "ClaimValue": "Student",
        "Description": "Seneca student"
    }

var ac5 = 
    {
        "ClaimType": "Task",
        "ClaimValue": "SMILE Mentor",
        "Description": "SMILE program mentor"
    }
