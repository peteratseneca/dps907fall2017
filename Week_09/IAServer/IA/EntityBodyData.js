
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
Password=Password123!&ConfirmPassword=Password123!&Email=student3@example.com&GivenName=Student&Surname=Three&Roles=Student&Roles=Mentor&CustomClaims=OU%3dSICT&CustomClaims=OU%3dFASET&CustomClaims=Campus%3dYork&CustomClaims=Program%3dBSD&CustomClaims=Activity%3dVolunteer

Content-Type: application/json

{"Password":"Password123!","ConfirmPassword":"Password123!","Email":"student3@example.com"}


Request access token
====================

POST to the URI /token

Must send HTML Forms data...

Content-Type: application/x-www-form-urlencoded

grant_type=password&username=student8@example.com&password=Password123!

~~~~~ 

This is the kind of data that you will probably get back:

{
   "access_token":"Ll49GG3k1bKjl0R-K2NHOFh16s5LblQbCw2rhO0nogZw9Uw0yEzUAzhP-n3dND2Y2edFpi5vITFGvCqqxD4Ws3k4y4CW5Dy1WfDddPQe2h8DjZbR-2TViNF7WrbineugyYKn5OOTXF4Uip4jTFYs7R8SRwvuOhGeniOlz_4nLoWtGNZZ8U2b9f1p5NxlRkD8Sdmtz6a8t9ZCPgRvfbZ31K9Ql9PahVERenQfOUjCFqDQev1ydFALPgx2fKyGEHHK4ZE5xU5uQTRsGiPOJGx9sNjDKfiqeWVa86WDH9E-KbHwBLCuziAHKJ_dRgmyWUQMPJyaklG1YnDAiLVszlIXX1b48dOtFyxM0W8-gGNOhEIBoAMcMuGPUsN5LkGTS-aLd95NDdD6N9Sd658R-TrVknJmX6rcsAUrtBu-wfxdIJoMfoch1Loa6xDqqAQkRLdLUD-pbYT1OcoZMGYt8HKZkQHgtHj1YV33vm0lDjWhuPk",
   "token_type":"bearer",
   "expires_in":1209599,
   "userName":"student8@example.com",
   ".issued":"Fri, 14 Oct 2016 17:57:30 GMT",
   ".expires":"Fri, 28 Oct 2016 17:57:30 GMT"
}

Keep/save/use the value of the access token. The other data is visible to you, and it's also encoded/encrypted in the value of the access token.

The value of the access token also includes claims and other info useful for authorization. 

How do you use this value?

1. Create an "Authorization" request header
2. Its value will be the word "Bearer"...
3. ...a space, and...
4. ...the VALUE of the access token

For example:

Authorization: Bearer Ll49GG3k1bKjl0R-K2NHOFh16s5LblQbC...(etc., a long string)

*/
