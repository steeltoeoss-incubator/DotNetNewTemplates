namespace Company.WebApplication1
#if (CircuitBreakerHystrixOption)

open System.Threading.Tasks
open Steeltoe.CircuitBreaker.Hystrix

type HelloHystrixCommand(name : string) =
    inherit HystrixCommand(HystrixCommandGroupKeyDefault.AsKey(name))
#endif
