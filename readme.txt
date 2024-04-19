# Project GRO-SD-TEST ScoreProcessor
***
This project implementation was oriented to produce a service-like application.

Next folders were included:
1. samples - Contains all txt files used for testing
2. src - Contains all sources files, main namespace was named Gro.SDTest.ScoreProcessor

Source files are all in the same namespace using a DDD orientation
1. Constants.cs --> Constants used in code
2. TeamEntity.cs --> Entity object which represents a Team
3. ScoreService.cs --> Service class which implements process control and logic 
4. RemoteApiService.cs --> Service class for obtaining information from a remote application

Samples included in txt files
- empty.txt --> Empty file with no records
- samples.txt --> A file with irregular records
- errors.txt --> A file with wrong records
These files may be used with added path, eg. "samples/empty.txt"