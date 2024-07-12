using System.Collections;
using System.Collections.Generic;
using Mizuvt.Common;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class OrderedSetTest {
    private OrderedSet<int> orderedSet = new OrderedSet<int>();

    [SetUp]
    public void Setup() {
        // orderedSet = new OrderedSet<int>();
    }

    [Test]
    public void TestAdd() {
        orderedSet.Add(1);
        orderedSet.Add(2);
        orderedSet.Add(3);
        Assert.AreEqual(3, orderedSet.Count);
        Assert.AreEqual(1, orderedSet[0]);
        Assert.AreEqual(2, orderedSet[1]);
        Assert.AreEqual(3, orderedSet[2]);
    }

    [Test]
    public void TestRemove() {
        orderedSet.Add(1);
        orderedSet.Add(2);
        orderedSet.Add(3);
        orderedSet.Remove(2);
        Assert.AreEqual(2, orderedSet.Count);
        Assert.IsFalse(orderedSet.Contains(2));
        Assert.AreEqual(1, orderedSet[0]);
        Assert.AreEqual(3, orderedSet[1]);
    }

    [Test]
    public void TestContains() {
        orderedSet.Add(1);
        orderedSet.Add(2);
        orderedSet.Add(3);
        Assert.IsTrue(orderedSet.Contains(2));
        Assert.IsFalse(orderedSet.Contains(4));
    }

    [Test]
    public void TestIndexer() {
        orderedSet.Add(1);
        orderedSet.Add(2);
        orderedSet.Add(3);
        Assert.AreEqual(2, orderedSet[1]);
    }

    [Test]
    public void TestClear() {
        orderedSet.Add(1);
        orderedSet.Add(2);
        orderedSet.Add(3);
        orderedSet.Clear();
        Assert.AreEqual(0, orderedSet.Count);
    }
}
