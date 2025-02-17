﻿using System;

namespace Grean.AtomEventStore.UnitTests
{
    public class UuidIriTests
    {
        [Theory, AutoAtomData]
        public void ToStringReturnsCorrectResult([Frozen]Guid g, UuidIri sut)
        {
            var actual = sut.ToString();
            Assert.Equal("urn:uuid:" + g, actual);
        }

        [Theory, AutoAtomData]
        public void SutCorrectlyConvertsToGuid(
            [Frozen]Guid expected,
            UuidIri sut)
        {
            Guid actual = sut;
            Assert.Equal(expected, actual);
        }

        [Theory, AutoAtomData]
        public void GuidCorrectlyConvertsToSut(
            UuidIri sut)
        {
            Guid expected = sut;
            UuidIri actual = expected;
            Assert.Equal<Guid>(expected, actual);
        }

        [Theory, AutoAtomData]
        public void SutIsEquatable(UuidIri sut)
        {
            Assert.IsAssignableFrom<IEquatable<UuidIri>>(sut);
        }

        [Theory, AutoAtomData]
        public void TwoSutsWithIdenticalIdAreEqual(Guid guid)
        {
            UuidIri sut = guid;
            UuidIri other = guid;

            var actual1 = sut.Equals((object)other);
            var actual2 = sut.Equals(other);
            var actual3 = sut == other;
            var actual4 = sut != other;

            Assert.True(actual1, "Equals(object)");
            Assert.True(actual2, "Equals(UuidIri)");
            Assert.True(actual3, "==");
            Assert.False(actual4, "!=");
        }

        [Theory, AutoAtomData]
        public void TwoSutsWithDifferentIdsAreNotEquals(
            Guid x,
            Guid y)
        {
            Assert.NotEqual(x, y);

            UuidIri sut = x;
            UuidIri other = y;

            var actual1 = sut.Equals((object)other);
            var actual2 = sut.Equals(other);
            var actual3 = sut == other;
            var actual4 = sut != other;

            Assert.False(actual1, "Equals(object)");
            Assert.False(actual2, "Equals(UuidIri)");
            Assert.False(actual3, "==");
            Assert.True(actual4, "!=");
        }

        [Theory, AutoAtomData]
        public void GetHashCodeReturnsCorrectResult(
            UuidIri sut)
        {
            var actual = sut.GetHashCode();

            var expected = ((Guid)sut).GetHashCode();
            Assert.Equal(expected, actual);
        }

        [Theory, AutoAtomData]
        public void ParseCorrectlyFormattedStringReturnsCorrectResult(
            UuidIri expected)
        {
            var correctlyFormatted = expected.ToString();

            UuidIri actualFromTry;
            bool couldParse = UuidIri.TryParse(correctlyFormatted, out actualFromTry);
            UuidIri actual = UuidIri.Parse(correctlyFormatted);

            Assert.True(couldParse, "TryParse should succeed.");
            Assert.Equal(expected, actualFromTry);
            Assert.Equal(expected, actual);
        }

        [Theory, AutoAtomData]
        public void ParseIncorrectlyPrefixedStringReturnsCorrectResult(
            string incorrectPrefix,
            Guid guid)
        {
            Assert.NotEqual("urn:uuid:", incorrectPrefix);
            var inccorectlyPrefixed = incorrectPrefix + guid;

            UuidIri actualFromTry;
            bool couldParse = UuidIri.TryParse(inccorectlyPrefixed, out actualFromTry);
            Assert.Throws<ArgumentException>(() => UuidIri.Parse(inccorectlyPrefixed));

            Assert.False(couldParse, "TryParse should fail.");
            Assert.Equal(default(UuidIri), actualFromTry);
        }

        [Theory, AutoAtomData]
        public void ParseStringWithIncorrectIdReturnsCorrectResult(
            DateTimeOffset notAGuid)
        {
            var incorrectlyIdd = "urn:uuid:" + notAGuid.ToString();

            UuidIri actualFromTry;
            bool couldParse = UuidIri.TryParse(incorrectlyIdd, out actualFromTry);
            Assert.Throws<ArgumentException>(() => UuidIri.Parse(incorrectlyIdd));

            Assert.False(couldParse, "TryParse should fail.");
            Assert.Equal(default(UuidIri), actualFromTry);
        }

        [Theory, AutoAtomData]
        public void ParseNullReturnsCorrectResult()
        {
            UuidIri actualFromTry;
            bool couldParse = UuidIri.TryParse(null, out actualFromTry);
            Assert.Throws<ArgumentNullException>(() => UuidIri.Parse(null));

            Assert.False(couldParse, "TryParse should fail.");
            Assert.Equal(default(UuidIri), actualFromTry);
        }

        [Fact]
        public void NewIdReturnsUniqueId()
        {
            var x = UuidIri.NewId();
            var y = UuidIri.NewId();

            Assert.NotEqual(x, y);
        }
    }
}
