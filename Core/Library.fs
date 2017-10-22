namespace SomeBasicCassandraApp
open System

type Customer = {Id:int; FirstName:string ; LastName:string; Version:int}

type Product = {Id:int; Cost:decimal; Name: string; Version: int}

type Order = {Id:int; Customer:Customer; OrderDate:DateTime; Products: Product list; Version: int}

type Entity=
    | Customer of Customer
    | Product of Product
    | Order of Order

type IRepository=
    abstract member GetCustomer: int->Customer
    abstract member GetProduct: int->Product
    abstract member GetOrder: int->Order

    abstract member Save: Entity->unit

open System.Collections.Concurrent

type Repository()=
    let _customers = new ConcurrentDictionary<int, Customer>()
    let _products = new ConcurrentDictionary<int, Product>()
    let _orders = new ConcurrentDictionary<int, Order>()

    interface IRepository with
        member this.GetCustomer id=
            _customers.[id]
        member this.GetProduct id=
            _products.[id]
        member this.GetOrder id=
            _orders.[id]
        member this.Save entity=
            match entity with
                | Customer c -> _customers.[c.Id] <- c
                | Order o -> _orders.[o.Id] <- o
                | Product p -> _products.[p.Id] <- p


type Command = 
    | Empty
    | AddCustomerCommand of id:int * version:int * firstName:string * lastName:string
    | AddOrderCommand of id:int * version:int * customer:int * orderDate:DateTime
    | AddProductCommand of id:int * version:int * name:string * cost:decimal
    | AddProductToOrder of orderId:int * productId:int

module Commands=
    let handle (repository:IRepository) command=
        match command with
            | AddCustomerCommand(id=id ;version=version; firstName=firstName; lastName=lastName) -> 
                repository.Save(Entity.Customer({
                                                Id=id
                                                FirstName=firstName
                                                LastName=lastName
                                                Version=version
                                                }))
            | AddOrderCommand(id=id; version=version; customer=customer; orderDate=orderDate)-> 
                repository.Save(Entity.Order({
                                               Id=id
                                               OrderDate=orderDate
                                               Version=version
                                               Customer= repository.GetCustomer(customer)
                                               Products=List.empty
                                             }))
            | AddProductCommand(id=id; version=version; name=name; cost=cost)-> 
                repository.Save(Entity.Product({
                                                Id=id
                                                Version=version
                                                Cost=cost
                                                Name=name
                                               }))
            | AddProductToOrder(orderId=orderId; productId=productId)->
                let order = repository.GetOrder(orderId)
                let product = repository.GetProduct(productId)
                repository.Save(Entity.Order({order with Products= product :: order.Products}))
            | Empty -> ()
(*
open 
*)
open Cassandra
open Cassandra.Mapping
open Cassandra.Data.Linq

type CassandraMappings() as this=
  inherit Mappings ()
  do 
    this.For<Customer>().TableName("customers") |> ignore
    this.For<Order>().TableName("orders") |> ignore
    this.For<Product>().TableName("products") |> ignore

type CassandraRepository(session:ISession)=
    let config = MappingConfiguration().Define<CassandraMappings>()
    do
      Table<Customer>(session, config).CreateIfNotExists()
      Table<Order>(session, config).CreateIfNotExists()
      Table<Product>(session, config).CreateIfNotExists()
    let mapper = Mapper(session, config)
    interface IRepository with
        member this.GetCustomer id=
             mapper.Single<Customer>("SELECT * FROM customers WHERE id = ?", id)
        member this.GetProduct id=
             mapper.Single<Product>("SELECT * FROM products WHERE id = ?", id)
        member this.GetOrder id=
             mapper.Single<Order>("SELECT * FROM orders WHERE id = ?", id)
        member this.Save entity=
            match entity with
                | Customer c -> mapper.Insert c
                | Order o -> mapper.Insert o
                | Product p -> mapper.Insert p

    interface IDisposable with
        member this.Dispose()=session.Dispose()

