
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mizuvt.Common;
using UniRx;
using UnityEngine;

namespace Download.NodeSystem {
    public class ZipStart : Zip {
        public override float Volume => 4;

        public ZipStart(Folder parent, string name, NodeCreateOptions? nodeCreateOptions = null) : base(
            parent,
            name,
            new ZipOption(
                new RunOption(2500), new List<IStaticNode> { ForestStatic.Instance, PersonStatic.Instance }),
            nodeCreateOptions) {
        }


        public override string GetPrintString(string indent) {
            return $"{indent}ZipStart: {Name}\n";
        }

        public static IStaticNode StaticNode => ZipStartStatic.Instance;


        public override IStaticNode GetStaticNode() {
            return StaticNode;
        }
    }
}