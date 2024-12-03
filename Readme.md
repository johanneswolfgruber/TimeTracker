# TimeTracker

This was originally developed as a tool for tracking my time at work and automatically exporting it as an excel file in the format my company required. I also use it to experiment and try out new things. So, additionally to the WPF windows desktop application, I am also developing a Web API with ASP.Net Core and a React Web Client. The backend is currently configured to use a local SQLite database, which should be easily swappable to another provider through EF Core.

## Building/Running the Web API as a Docker Image

The Web API can be built as a Docker Image using the command `docker build -t timetrackerapi .` and then started using `docker run -p 5152:8080 timetrackerapi`.

## Disclaimer

As this is just a hobby project, this is by no means production ready and probably contains quite a few bugs, which I will try to fix over time. Feel free to use any whatever you want from this repository at your own risk.
