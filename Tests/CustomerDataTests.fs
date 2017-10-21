module Tests.CustomerDataTests

open System
open Xunit


open System.IO
open System
open System.Collections.Generic
open SomeBasicCassandraApp
open GetCommands

let commands = getCommands()
let repo = CassandraRepository() :> IRepository
let commandHandler = Commands.handle repo
for command in commands |> Array.map WithSeqenceNumber.getCommand do
  commandHandler command
[<Fact>]
let CanGetCustomerById()=
  Assert.NotNull(repo.GetCustomer(1))

[<Fact>]
let CanGetProductById()=
  Assert.NotNull(repo.GetProduct(1))

[<Fact>]
let OrderContainsProduct()=
  let order = repo.GetOrder(1)
  Assert.True(order.Products |> List.tryFind( fun p -> p.Id = 1) |> Option.isSome)

//[<Fact>]
//member this.OrderHasACustomer()=
//    Assert.IsNotNullOrEmpty(_repository.GetTheCustomerOrder(1).Firstname)