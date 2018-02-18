## Project Description
Provides remote access service (RAS) components for .NET languages like C#, VB.NET, and C++ CLR projects (2.0.50727 and later only).

Check out the video demo channel on [YouTube](http://www.youtube.com/user/DotRasProject)
You can also follow the project on [Twitter](http://twitter.com/DotRasProject). This will be used for notification of features as they are completed along with new releases.

## DotRas v1.3 has launched!

### Attention - Support for .NET 2.0 will be ending with the 2.0 release!
I wanted to make sure this was communicated well in advance. I am currently waiting for the 1.3 release to be finished, so this is probably quite a ways out (maybe a year or so). The intention is to make some modifications to adopt the new asynchronous programming model using the TPL used by .NET 4 for this version to make the product await compliant. Windows 8 will still be supported for the 1.3 release, which will also still support .NET 2.0. Please be aware of the methods marked obsolete methods in the 1.3 release because they will be removed in 2.0. As of right now these methods are simply marked as obsolete, they will not cause build errors, and are stilly fully functional, however as of 2.0 (the release after 1.3) they will be removed.

### Latest News
02/22/2014 - It's been quite the long while since I deployed the last release candidate for DotRas since Windows 8 launched. Thank you all for your patience while this release was vetted. I was a bit more cautious than usual this time due to the fact that a new operating system was being introduced into the mix, and that I have quite a few more developers using the product now. Thank you all for your wonderful support, and I hope you like the latest version! As usual, if you have any questions, please let me know.

01/03/2013 - Well it's official, DotRas v1.3 has finally moved into release candidate and all code is now frozen for the next few months while any bugs are identified and fixed before release. This release adds in much needed diagnostic capabilities for logging what is going on internally in the SDK. For more information, check the EnablingDiagnosticLogging example included with the download.

### Notes
I thought I would make a special note for those that remember the "RAS Library for .NET 1.1" project from its days back on the GotDotNet site before it was phased out for CodePlex. I didn't like how I wrote the assembly back then and it was my first attempt at p/invoke. This is a complete rewrite from the ground up of that assembly. If you're still using it in your project, I would suggest getting the latest copy of this project from the releases section.
