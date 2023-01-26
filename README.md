# Introduction 
JsonDataEngine is a small library that allows to transform json data by adding another json as a payload. Can be used for example with Microsoft Teams Adaptive cards.
The json data can be used as a "template" while the payload contains the variables.

Example of Json data "template" to modify:
{
    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
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
        "fullname": "Rob NalyD"
      }
    ]
  }
}

# Getting Started
Just add the package dll to your project.
