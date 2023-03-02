﻿namespace Grean.AtomEventStore.UnitTests.FSharp

open System
open System.Reactive
open Grean.AtomEventStore
open Grean.AtomEventStore.UnitTests.FSharp.XmlRecords
open Grean.AtomEventStore.UnitTests.FSharp.TestDsl
open AutoFixture
open Xunit.Extensions
open Xunit

module XmlEventStreamFacadeTests =

    [<Theory; InMemoryXmlConventions>]
    let SutCorrectlyRoundTripsASingleElement
        (writer : AtomEventObserver<TestEventF>)
        (reader : FifoEvents<TestEventF>)
        (tef : TestEventF) =
        
        writer.AppendAsync(tef).Wait()
        let actual = reader |> Seq.toList

        Verify <@ actual.Length = 1 @>

    [<Theory; InMemoryXmlConventions>]
    let SutCorrectlyRoundTripsMultipleElements
        (writer : AtomEventObserver<TestEventF>)
        (reader : FifoEvents<TestEventF>)
        (g : Generator<TestEventF>) =

        let tefs = g |> Seq.take 3 |> Seq.toList

        tefs |> List.iter(fun tef -> writer.AppendAsync(tef).Wait())
        let actual = reader |> Seq.toList

        let expected = tefs
        Verify <@ expected = actual @>

    [<Theory; InMemoryXmlConventions>]
    let SutCorrectlyRoundTripsDiscriminatedUnions
        (writer : AtomEventObserver<obj>)
        (reader : FifoEvents<obj>)
        (tef : TestEventF)
        (teg : TestEventG) =

        let extract = function
            | F(x) -> x :> obj
            | G(x) -> x :> obj
        let duObs = Observer.Create(extract >> writer.OnNext)
        duObs.OnNext(tef |> F)
        duObs.OnNext(teg |> G)
        
        let infuse (x : obj) =
            match x with
            | :? TestEventF as f -> f |> F
            | :? TestEventG as g -> g |> G
            | _ -> raise(System.ArgumentException("Unknown event type."))
        let duSeq = reader |> Seq.map infuse
        let actual = duSeq |> Seq.toList

        let expected = [tef |> F; teg |> G]
        Verify <@ expected = actual @>

    [<Theory; InMemoryXmlConventions>]
    let SutCorrectlyRoundTripsChangesetOfDiscriminatedUnions
        (writer : AtomEventObserver<SerializableChangeset>)
        (reader : FifoEvents<SerializableChangeset>)
        (tef : TestEventF)
        (teg : TestEventG)
        (id : Guid) =

        let toObj changeset =
            let extract = function
                | F(x) -> x :> obj
                | G(x) -> x :> obj
            let events = changeset.Items |> Seq.map extract |> Seq.toArray
            { SerializableChangeset.Id = changeset.Id; Items = events }
        let duObs = Observer.Create(toObj >> writer.OnNext)
        duObs.OnNext({ Id = id; Items = [| tef |> F; teg |> G |]})

        let ofObj (changeset : SerializableChangeset) =
            let infuse (x : obj) =
                match x with
                | :? TestEventF as f -> f |> F
                | :? TestEventG as g -> g |> G
                | _ -> raise(System.ArgumentException("Unknown event type."))
            let events = changeset.Items |> Array.map infuse
            { Id = changeset.Id; Items = events }
        let duSeq = reader |> Seq.map ofObj
        let actual = duSeq |> Seq.toList

        let expected = [ { Id = id; Items = [| tef |> F; teg |> G |] } ]
        Verify <@ expected = actual @>