## Week 7 code examples

Study the "ATTENTION" comment tokens in the code examples, as you also go through the class notes.  

<br>

### ProjectWithSecurity

Visual Studio web service project, with security.  

Open the EntityBodyData.txt source code file for examples of data package format that you can send to the web service. 

Please note that some user accounts in that file have already been created (so if you attempt to create them again, an error will be returned). These user account names are in the app:
* dev@example.com  
* student3@example.com  
* student5@example.com  
* student8@example.com  

The password for every account is: Password123!

Features:
* Simple non-enhanced web service project with security
* An "AuthInfo" controller that will return access token data (if you are authenticated)

<br>

### CustomAuthorizeAttributeApi.cs

This is a custom filter, to authorize a custom claim, for a Web API controller (a controller that inherits from ApiController).  

Add this to any project, in the Controllers folder, and edit its namespace. Then you can use it any controller.  
<br>

### SimpleClaims

Enhances the ProjectWithSecurity code example (above).  

Adds claims processing. When you register a new user account, you must provide a given name, a surname, and one or more role claims. See the EntityBodyData.txt source code file for examples.  

Features:
* Test controller that will enable you to test various user account scenarios
* AuthInfo controller that will decrypt/decode the access token, and display its contents

<br>
