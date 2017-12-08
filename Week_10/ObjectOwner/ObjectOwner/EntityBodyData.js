
// Entity body data

/* 

Login as a user
===============

POST to the /token endpoint

Content-Type: application/x-www-form-urlencoded

grant_type=password&username=uam@example.com&password=Password123!
grant_type=password&username=dev@example.com&password=Password123!
grant_type=password&username=student@example.com&password=Password123!


Add a note
==========

Get an access token first, and include it in the request header collection

POST to the /api/notes endpoint

Content-Type: application/json

{"Title":"My first note","Content":"The quick brown fox..."}
{"Title":"Follow-up note","Content":"...jumped over the lazy dog."}

*/

