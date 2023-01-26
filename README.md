# Introduction 
JsonDataEngine is a small library that allows to transform json data by adding another json as a payload. Can be used for example with Microsoft Teams Adaptive cards.
The json data can be used as a "template" while the payload contains the variables.

Example of Json data "template" to modify:<br />
{  
&emsp"$schema": "http://adaptivecards.io/schemas/adaptive-card.json",  
    "version": "1.5",  
    "type": "AdaptiveCard",  
    "body": [  
        {  
            "type": "TextBlock",  
            "text": "$contact.greetings",  
            "wrap": true  
        },  
        {  
            "type": "FactSet",  
            "facts": "foreach $contact.friends use { \"title\": \"Friend Name\", \"value\": \"$fullname\" }"  
        }  
    ]  
}  
  
Example of Payload:  
{  
  "contact": {  
    "greetings": "Hello Mr Bob Dylan!",  
    "friends": [  
      {  
        "fullname": "Leonardo Galilee"  
      },  
      {  
        "fullname": "Rob Nalyd"  
      }  
    ]  
  }  
}  
  
# Getting Started  
Just add the package dll to your project.  
A few functions are supported:  
    - use the dollars sign in a string property followed by the json path based on payload structure.  
         - example "text": "$contact.greetings"  
    - use the following structure for arrays: foreach $payloadJsonPathPointingToAnArray use <whatever you want to be repeated, you can do recursive if needed, escape double quotes>  
         - example "facts": "foreach $contact.friends use { \"title\": \"Friend Name\", \"value\": \"$fullname\" }" will result in   
         "facts": [ { "title": "Friend Name", "value": "Leonardo Galilee" }, { "title": "Friend Name", "value": "Rob Nalyd" } ]   
           
Have fun!  
