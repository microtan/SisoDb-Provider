﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using SisoDb.Structures;

namespace SisoDb.UnitTests.Structures.StructureBuilderTests
{
    [TestFixture]
    public abstract class StructureBuilderBaseTests : UnitTestBase
    {
        protected IStructureBuilder Builder;

        protected static GuidItem[] CreateGuidItems(int numOfItems, bool assignValues = false)
        {
            var items = new List<GuidItem>(numOfItems);

            if(!assignValues)
                for (var c = 0; c < numOfItems; c++)
                    items.Add(new GuidItem {Value = c + 1});
            else
                for (var c = 0; c < numOfItems; c++)
                    items.Add(new GuidItem { StructureId = Guid.NewGuid(), Value = c + 1 });

            return items.ToArray();
        }

        protected class GuidItem
        {
            public Guid StructureId { get; set; }

            public int Value { get; set; }
        }
    }
}