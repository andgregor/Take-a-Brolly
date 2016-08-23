# Take A Brolly #

## Description ##
This is a demo console application written in C# using Visual Studio 2015 Express. This application is designed to be run as a scheduled task and will check open weather for upcoming rain events and email you to tell you to *take a brolly* or not.

The application queries the openweather.org api for a five day forecast, parses the relevant parts of the XML response based on the settings and emails a response to your inbox.

## Installation ##

1. Register an API key with http://openweathermap.org/
2. Run the solution. The application will detect a missing ini file and create a new default one.
3. Configure using settings (below).
4. Run the built application via a scheduled task.

Upon first run a new default TakeABrolly.ini file will be created in the application folder. This ini file holds any available settings for the application.

## Settings ##
[SETTINGS]

service=false //Runs the console application with or without a visible console.

apikey= //Add your open weather API key here: [Sign Up Here](https://home.openweathermap.org/users/sign_up "Sign Up Here")

postcode=NN16 //Enter a postcode where you would like your weather search

country=GB //Enter the country code for your country. Country codes are [ISO 3166](http://www.iso.org/iso/country_codes "ISO 3166"). 

baseurl=http://api.openweathermap.org/data/2.5/ //This is the base url for openweather api.

rainmin=0.1 //A minimum threshold in millimetres per three hour period to consider it raining. 

timescale=10 //How many hours to look ahead for rain at runtime.

[DEBUG]

dodebug=true //Logs additional debugging to the log file and console if console is enabled.

doUpdate=true //Developer option to turn off updating at every runtime and use locally cached xml file for weather data. Primarily used to not abuse the API during testing / development.

[LOGGING]

writeErrLog=true //Write errors to local log file

emailErrLog=false //Emails any errors to the developer email. Currently this is set within the application.

[REPORTING]

emailReport=true //Sends the email containing the weather report.

recipient=andrew@andgregor.co.uk //Email address of the recipient of the weather report.

[SMTP]

ip= //IP or FQDN of mail server to send the report through.

sender= //Sender email address.

pass= //Password for email address (if required). 

## Support ##

Please contact [andrew@andgregor.co.uk](mailto:andrew@andgregor.co.uk "andrew@andgregor.co.uk") with any support issues or questions.
