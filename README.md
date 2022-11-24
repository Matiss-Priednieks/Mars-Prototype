# Martian-Strategy-Sim
FYP for college


## NASA InSight API
Before delving into the planet generation, I first needed to figure out how to use the NASA Api for the weather on Mars.
There is great documentation on about the API, which is what I will be using to understand, and display the data in a nice and understandable way in Unity or Python.
Getting access to the API is as simple as signing up with an email and password and you're given access to an API key that you can make HTTPS requests to the service with.

The API returns back a JSON file with all of the data that it collects from the InSight Mars Lander, this data being labelled and explained using a demo key.

### What are all of these variables?
This section is mainly dedicated to going through what each of the variables in the JSON file refer to.
1. "Sol" refers to a "day" on mars, indicated by a number like "259" or "1240". Each previous day should be accessible.
2. "AT" This shows the Atmospheric temperature data on that day.
3. "HWS" refers to the Horizontal wind speed data on that day.
4. "PRE" refers to Pressure.
5. "WD" is a dictionary of arrays? of the Wind Direction. More details here https://api.nasa.gov/assets/insight/InSight%20Weather%20API%20Documentation.pdf
6. "First_UTC" shows season and UTC time range.

## Examples of what NASA has made

1. https://eyes.nasa.gov/apps/solar-system/#/home
