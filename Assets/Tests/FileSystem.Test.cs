using Download.NodeSystem;
using NUnit.Framework;
using UnityEngine;
using Forest = Download.NodeSystem.Forest;

public class NodeSystemTests {
    [Test]
    public void TestFolderCreation() {
        // 루트 폴더 생성
        Folder root = Folder.CreateRoot();
        Assert.IsNull(root.Parent);
    }

    [Test]
    public void TestAddChildToFolder() {
        // 루트 폴더 및 서브 폴더 생성
        Folder root = Folder.CreateRoot();
        Folder subFolder = new Folder(root, "SubFolder");

        Assert.AreEqual(1, root.children.Count);
        Assert.AreSame(subFolder, root.children[0]);
        Assert.AreSame(root, subFolder.Parent);
    }

    [Test]
    public void TestTreeCreation() {
        // 루트 폴더 및 트리 생성
        Folder root = Folder.CreateRoot();
        Forest forest = new Forest(root, "forest");

        Assert.AreEqual(1, root.children.Count);
        Assert.AreSame(forest, root.children[0]);
        Assert.AreSame(root, forest.Parent);
    }

}
