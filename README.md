# PerformanceLabsFramework - wrapper over mvc mini profiler enabling it's use offline in capturing time taken by methods with reporting capabilities

This is something I was asked to make at work but is no longer being used there and I got the permission to make it open source.

This is a wrapper over the popular MVC Mini Profiler project. Basically, it's a simple dll with mini profiler dll inside it. This dll gives your test project the capability to persist (for the first time) the time taken by your methods. Once the test suite has run, it gives a nice html file as report stating which methods took more time than usual and other alerts.

The basis of alerts is noting a spike in terms of multiplicativeness of the time taken or if time taken is a few standard deviations/z-scores away from the average. 

It has a nice demo project showcasing it's capabilities with a nice demo html report also checked in.

P.S. - I don't think this is very useful project.
