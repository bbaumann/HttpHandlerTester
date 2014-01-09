HttpHandlerTester
=================

This code provide the implementation of two Http Handlers and is meant to be use this way :
* http://DOMAIN/test?SleepTime=XX
* http://DOMAIN/testAsync?SleepTime=XX
* http://DOMAIN/testNotSoAsync?SleepTime=XX

SleepTime represent the time he request must take on the server.

* /test is a *synchronous* handler that calls a *synchronous* method
* /testAsync is an *asynchronous* handler (using TPL) that calls an *asynchronous* method
* /testNotSoAsync is an *asynchronous* handler (using TPL) that calls a *synchronous* method 

The load-tests may be run with apachebench for example :
ab -n1000 -c100 http://DOMAIN/testAsync?SleepTime=1000
perform 1000 requests, made 100 at a time, each request taking 1sec of computation time on the server. 